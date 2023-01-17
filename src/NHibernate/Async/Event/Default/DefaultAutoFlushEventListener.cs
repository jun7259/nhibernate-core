﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by AsyncGenerator.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------


using System;

using NHibernate.Engine;

namespace NHibernate.Event.Default
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class DefaultAutoFlushEventListener : AbstractFlushingEventListener, IAutoFlushEventListener
	{

		#region IAutoFlushEventListener Members

		/// <summary>
		/// Handle the given auto-flush event.
		/// </summary>
		/// <param name="event">The auto-flush event to be handled.</param>
		/// <param name="cancellationToken">A cancellation token that can be used to cancel the work</param>
		public virtual async Task OnAutoFlushAsync(AutoFlushEvent @event, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			IEventSource source = @event.Session;

			if (FlushMightBeNeeded(source))
			{
				using (source.SuspendAutoFlush())
				{
					int oldSize = source.ActionQueue.CollectionRemovalsCount;

					await (FlushEverythingToExecutionsAsync(@event, cancellationToken)).ConfigureAwait(false);

					var flushIsReallyNeeded = FlushIsReallyNeeded(@event, source);
					if (flushIsReallyNeeded)
					{
						if (log.IsDebugEnabled())
							log.Debug("Need to execute flush");

						await (PerformExecutionsAsync(source, cancellationToken)).ConfigureAwait(false);
						PostFlush(source);
						// note: performExecutions() clears all collectionXxxxtion
						// collections (the collection actions) in the session

						if (source.Factory.Statistics.IsStatisticsEnabled)
						{
							source.Factory.StatisticsImplementor.Flush();
						}
					}
					else
					{
						if (log.IsDebugEnabled())
							log.Debug("Dont need to execute flush");
						source.ActionQueue.ClearFromFlushNeededCheck(oldSize);
					}

					@event.FlushRequired = flushIsReallyNeeded;
				}
			}
		}

		#endregion
	}
}