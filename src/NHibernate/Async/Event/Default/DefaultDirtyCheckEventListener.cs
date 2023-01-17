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
using NHibernate.Util;

namespace NHibernate.Event.Default
{
	using System.Threading.Tasks;
	using System.Threading;
	public partial class DefaultDirtyCheckEventListener : AbstractFlushingEventListener, IDirtyCheckEventListener
	{

		public virtual async Task OnDirtyCheckAsync(DirtyCheckEvent @event, CancellationToken cancellationToken)
		{
			cancellationToken.ThrowIfCancellationRequested();
			int oldSize = @event.Session.ActionQueue.CollectionRemovalsCount;

			try
			{
				await (FlushEverythingToExecutionsAsync(@event, cancellationToken)).ConfigureAwait(false);
				bool wasNeeded = @event.Session.ActionQueue.HasAnyQueuedActions;
				log.Debug(wasNeeded ? "session dirty" : "session not dirty");
				@event.Dirty = wasNeeded;
			}
			finally
			{
				@event.Session.ActionQueue.ClearFromFlushNeededCheck(oldSize);
			}
		}
	}
}