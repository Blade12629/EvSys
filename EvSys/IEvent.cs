using System;

namespace Server.Custom.Skyfly.EvSys
{
	public interface IEvent
	{
		/// <summary>
		/// Event id
		/// </summary>
		Guid Id { get; }
		/// <summary>
		/// Event Status, e.g.: Waiting
		/// </summary>
		EventStatus Status { get; }
		/// <summary>
		/// Should the event be requeued for activation once it executed?
		/// </summary>
		bool RequeueAfterExecution { get; set; }
		/// <summary>
		/// If true <see cref="OnEventFinish"/> will be called every 
		/// time <see cref="TryExecute"/> returns true even if <see cref="RequeueAfterExecution"/> is true
		/// <para>See: <see cref="OnEventFinish"/></para>
		/// </summary>
		bool FinishEvenWithRequeue { get; set; }

		/// <summary>
		/// Tries to execute the event
		/// </summary>
		/// <returns>Event was executed</returns>
		bool TryExecute();

		/// <summary>
		/// Called when <see cref="Execute"/> returns true and <see cref="RequeueAfterExecution"/> is false
		/// <para>To force this to be called everytime <see cref="Execute"/> returns true, set <see cref="FinishEvenWithRequeue"/> to true</para>
		/// <para>Will be called after the current EventController Tick (<see cref="EventConfig.EventCheckDelay"/>) but before the next events are executed</para>
		/// </summary>
		void OnEventFinish();

		/// <summary>
		/// Called when the event is requeued (happens at the same tick as the event finishes)
		/// </summary>
		void OnEventRequeue();
		/// <summary>
		/// Called when the event is queued
		/// <para>Does not get called should the event be requeued</para>
		/// </summary>
		void OnEventQueued();
	}
}
