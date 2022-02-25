using System;
using System.Collections.Generic;

namespace Server.Custom.Skyfly.EvSys
{
	/// <summary>
	/// Barebones event class
	/// </summary>
	public abstract class BaseEvent : IEvent, IEquatable<BaseEvent>
	{
		/// <summary>
		/// <inheritdoc cref="IEvent.Id"/>
		/// </summary>
		public Guid Id { get; }
		/// <summary>
		/// <inheritdoc cref="IEvent.Status"/>
		/// </summary>
		public virtual EventStatus Status { get; protected set; }
		/// <summary>
		/// <inheritdoc cref="IEvent.RequeueAfterExecution"/>
		/// </summary>
		public virtual bool RequeueAfterExecution { get; set; }
		/// <summary>
		/// <inheritdoc cref="IEvent.FinishEvenWithRequeue"/>
		/// </summary>
		public virtual bool FinishEvenWithRequeue { get; set; }

		public BaseEvent()
		{
			Id = Guid.NewGuid();
		}

		/// <summary>
		/// Equivalent to calling <see cref="EventController.RegisterEvent(IEvent)"/>
		/// </summary>
		public void EnqueueEvent()
		{
			EventController.Controller.RegisterEvent(this);
		}

		/// <summary>
		/// Equivalent to calling <see cref="EventController.RemoveEvent(IEvent)"/>
		/// </summary>
		public bool DequeueEvent()
		{
			return EventController.Controller.RemoveEvent(this);
		}

		/// <summary>
		/// <inheritdoc cref="IEvent.TryExecute"/>
		/// </summary>
		/// <returns></returns>
		public bool TryExecute()
		{
			if (Status == EventStatus.Finished || !CanExecute() || !Execute())
				return false;

			Status = EventStatus.Finished;
			return true;
		}

		/// <summary>
		/// <inheritdoc cref="OnEventFinish"/>
		/// </summary>
		public virtual void OnEventFinish()
		{

		}

		/// <summary>
		/// <inheritdoc cref="IEvent.OnEventRequeue"/>
		/// </summary>
		public virtual void OnEventRequeue()
		{
			Status = EventStatus.Waiting;
		}

		/// <summary>
		/// <inheritdoc cref="IEvent.OnEventQueued"/>
		/// </summary>
		public virtual void OnEventQueued()
		{
			Status = EventStatus.Waiting;
		}

		/// <summary>
		/// Can the event be executed?
		/// </summary>
		/// <returns>Event can be executed</returns>
		protected abstract bool CanExecute();

		/// <summary>
		/// Executes the event
		/// </summary>
		/// <returns>Event was executed</returns>
		protected abstract bool Execute();

		#region vs-generated
		public override bool Equals(object obj)
		{
			return Equals(obj as BaseEvent);
		}

		public bool Equals(BaseEvent other)
		{
			return other != null &&
				   Id.Equals(other.Id);
		}

		public override int GetHashCode()
		{
			return 2108858624 + Id.GetHashCode();
		}

		public static bool operator ==(BaseEvent left, BaseEvent right)
		{
			return EqualityComparer<BaseEvent>.Default.Equals(left, right);
		}

		public static bool operator !=(BaseEvent left, BaseEvent right)
		{
			return !(left == right);
		}
		#endregion
	}
}
