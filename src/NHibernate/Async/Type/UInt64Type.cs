﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using NHibernate.Engine;
using NHibernate.SqlTypes;

namespace NHibernate.Type
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class UInt64Type : PrimitiveType, IDiscriminatorType, IVersionType
	{

		#region IVersionType Members

		public virtual Task<object> NextAsync(object current, ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			try
			{
				return Task.FromResult<object>(Next(current, session));
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		public virtual Task<object> SeedAsync(ISessionImplementor session, CancellationToken cancellationToken)
		{
			if (cancellationToken.IsCancellationRequested)
			{
				return Task.FromCanceled<object>(cancellationToken);
			}
			try
			{
				return Task.FromResult<object>(Seed(session));
			}
			catch (Exception ex)
			{
				return Task.FromException<object>(ex);
			}
		}

		#endregion
	}
}
