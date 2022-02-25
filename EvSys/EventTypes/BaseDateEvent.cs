using System;

namespace Server.Custom.Skyfly.EvSys.EventTypes
{
	/// <summary>
	/// Create events that are executed at a specific date
	/// </summary>
	public abstract class BaseDateEvent : BaseEvent
	{
		/// <summary>
		/// Date when the event should be executed
		/// </summary>
		public virtual DateTime ExecutionTime { get; set; }

		public BaseDateEvent(DateTime executionTime) : base()
		{
			ExecutionTime = executionTime;
		}

		protected override bool CanExecute()
		{
			return ExecutionTime <= DateTime.Now;
		}

		public override void OnEventRequeue()
		{
			ExecutionTime = GetNextDate();
		}

		/// <summary>
		/// Get the next date when we should execute the event again
		/// <para>Note: This is only called when <see cref="IEvent.RequeueAfterExecution"/> is true</para>
		/// </summary>
		/// <returns>The next execution date</returns>
		protected abstract DateTime GetNextDate();
	}
}
