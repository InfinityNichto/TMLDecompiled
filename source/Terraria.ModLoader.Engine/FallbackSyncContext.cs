using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Terraria.ModLoader.Engine;

/// <summary>
/// Provides a SynchronizationContext for running continuations on the Main thread in the Update loop, for platforms which don't initialized with one
/// </summary>
internal static class FallbackSyncContext
{
	private class SyncContext : SynchronizationContext
	{
		private static ConcurrentQueue<Action> actions = new ConcurrentQueue<Action>();

		public override void Send(SendOrPostCallback d, object state)
		{
			ManualResetEvent handle = new ManualResetEvent(initialState: false);
			Exception e = null;
			actions.Enqueue(delegate
			{
				try
				{
					d(state);
				}
				catch (Exception ex)
				{
					e = ex;
				}
				finally
				{
					handle.Set();
				}
			});
			handle.WaitOne();
			if (e != null)
			{
				throw e;
			}
		}

		public override void Post(SendOrPostCallback d, object state)
		{
			actions.Enqueue(delegate
			{
				try
				{
					d(state);
				}
				catch (Exception ex)
				{
					Logging.tML.Error((object)"Posted event", ex);
				}
			});
		}

		public override SynchronizationContext CreateCopy()
		{
			return this;
		}

		internal void Update()
		{
			Action action;
			while (actions.TryDequeue(out action))
			{
				action();
			}
		}
	}

	private static SyncContext ctx;

	public static void Update()
	{
		if (SynchronizationContext.Current == null)
		{
			SynchronizationContext.SetSynchronizationContext(ctx = new SyncContext());
			Logging.tML.Debug((object)"Fallback synchronization context assigned");
		}
		ctx?.Update();
	}
}
