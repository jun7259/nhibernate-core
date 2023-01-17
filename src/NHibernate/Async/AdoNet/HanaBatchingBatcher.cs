﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;
using NHibernate.AdoNet.Util;
using NHibernate.Driver;
using NHibernate.Exceptions;

namespace NHibernate.AdoNet
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class HanaBatchingBatcher : AbstractBatcher
	{

		public override Task AddToBatchAsync(IExpectation expectation, CancellationToken cancellationToken)
		{
			// HanaCommands are cloneable
			if (!(Driver.UnwrapDbCommand(CurrentCommand) is ICloneable cloneableCurrentCommand))
				throw new InvalidOperationException("Current command is not an ICloneable");
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			return InternalAddToBatchAsync();
			async Task InternalAddToBatchAsync()
			{

				var batchUpdate = CurrentCommand;
				await (PrepareAsync(batchUpdate, cancellationToken)).ConfigureAwait(false);
				Driver.AdjustCommand(batchUpdate);

				_totalExpectedRowsAffected += expectation.ExpectedRowCount;
				string lineWithParameters = null;
				var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
				if (sqlStatementLogger.IsDebugEnabled || Log.IsDebugEnabled())
				{
					lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
					var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
					lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
					_currentBatchCommandsLog.Append("command ")
						.Append(_countOfCommands)
						.Append(":")
						.AppendLine(lineWithParameters);
				}
				if (Log.IsDebugEnabled())
				{
					Log.Debug("Adding to batch:{0}", lineWithParameters);
				}

				if (_currentBatch == null)
				{
					// use first command as the batching command
					_currentBatch = cloneableCurrentCommand.Clone() as DbCommand;
				}

				_currentBatchCommands.Add(cloneableCurrentCommand.Clone() as DbCommand);

				_countOfCommands++;

				if (_countOfCommands >= _batchSize)
				{
					await (DoExecuteBatchAsync(batchUpdate, cancellationToken)).ConfigureAwait(false);
				}
			}
		}

		protected override async Task DoExecuteBatchAsync(DbCommand ps, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			Log.Info("Executing batch");
			await (CheckReadersAsync(cancellationToken)).ConfigureAwait(false);

			if (Factory.Settings.SqlStatementLogger.IsDebugEnabled)
			{
				Factory.Settings.SqlStatementLogger.LogBatchCommand(_currentBatchCommandsLog.ToString());
				_currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
			}

			try
			{
				int rowCount = 0;

				if (_countOfCommands > 0)
				{
					_currentBatch.Parameters.Clear();

					foreach (var command in _currentBatchCommands)
					{
						// Batching with HANA works by simply defining multiple times each command parameter.
						// (Undocumented feature explained by a developer of the provider.)
						foreach (DbParameter parameter in command.Parameters)
						{
							_currentBatch.Parameters.Add(parameter);
						}
					}

					_currentBatch.Prepare();

					try
					{
						rowCount = await (_currentBatch.ExecuteNonQueryAsync(cancellationToken)).ConfigureAwait(false);
					}
					catch (DbException e)
					{
						throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not execute batch command.");
					}
				}

				Expectations.VerifyOutcomeBatched(_totalExpectedRowsAffected, rowCount, ps);
			}
			finally
			{
				// Cleaning up even if batched outcome is invalid
				_totalExpectedRowsAffected = 0;
				_countOfCommands = 0;
				CloseBatchCommands();
			}
		}
	}
}