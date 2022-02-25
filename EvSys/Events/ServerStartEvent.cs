using System;
using Server.Custom.Skyfly.EvSys.Attributes;

namespace Server.Custom.Skyfly.EvSys.Events
{
	// Automatically queue up the event once the server has started
	[EventAutoQueue]
	public class ServerStartEvent : BaseEvent
	{
		int _executionCount;

		public ServerStartEvent() : base()
		{
			// The event should not be dequeued once it was executed
			RequeueAfterExecution = true;
		}

		protected override bool CanExecute()
		{
			return true; // we can execute the event
		}

		protected override bool Execute()
		{
			if (_executionCount >= 4)
			{
				// Do not execute the event again
				RequeueAfterExecution = false;
			}

			FinishEvenWithRequeue = !FinishEvenWithRequeue;

			Console.WriteLine($"The server has started ({_executionCount++})");
			return true; // event was executed
		}

		public override void OnEventFinish()
		{
			Console.WriteLine($"OnEventFinish ({_executionCount})");
		}

		public override void OnEventQueued()
		{
			Console.WriteLine($"OnEventQueued ({_executionCount})");
			base.OnEventQueued();
		}

		public override void OnEventRequeue()
		{
			Console.WriteLine($"OnEventRequeue ({_executionCount})");
			base.OnEventRequeue();
		}
	}
}
