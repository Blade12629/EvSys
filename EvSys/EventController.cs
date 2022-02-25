using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Server.Custom.Skyfly.EvSys.Attributes;

namespace Server.Custom.Skyfly.EvSys
{
	/// <summary>
	/// Handling all events
	/// </summary>
	public sealed class EventController
	{
		public static readonly EventController Controller = new EventController();

		Dictionary<Guid, IEvent> _events;
		Queue<IEvent> _finishedEvents;

		private EventController()
		{
			_events = new Dictionary<Guid, IEvent>();
			_finishedEvents = new Queue<IEvent>();
		}

		/// <summary>
		/// Gets a queued event
		/// </summary>
		/// <param name="id">Event id</param>
		/// <returns>Event or null if not queued</returns>
		public IEvent this[Guid id]
		{
			get
			{
				if (_events.TryGetValue(id, out IEvent ev))
					return ev;

				return null;
			}
		}

		/// <summary>
		/// Enqueues an event
		/// </summary>
		public IEvent this[IEvent ev]
		{
			set
			{
				RegisterEvent(ev);
			}
		}

		public static void Configure()
		{
			if (!EventConfig.IsEnabled)
				return;

			Console.WriteLine("Configuring EvSystem");

			// Get all event types which implement the EventAutoQueueAttribute
			Type[] types = Assembly.GetAssembly(typeof(EventController))
								   .GetTypes()
								   .Where(t => t.GetCustomAttributes()
												.Any(a => a is EventAutoQueueAttribute))
								   .ToArray();

			Console.WriteLine($"Queueing up {types.Length} events");

			int actualLoadedTypes = 0;
			for (int i = 0; i < types.Length; i++)
			{
				Type t = types[i];

				// If we don't have any empty constructor we will just skip the event
				if (!t.GetConstructors().Any(c => c.GetParameters().Length == 0))
				{
					Console.WriteLine($"Unable to auto create event {t.Name}, no constructor without parameters found");
					continue;
				}

				IEvent ev = (IEvent)Activator.CreateInstance(t);
				Controller.RegisterEvent(ev);
				actualLoadedTypes++;
			}

			Console.WriteLine($"Queued {actualLoadedTypes} events");
		}

		public static void Initialize()
		{
			if (!EventConfig.IsEnabled)
				return;

			Console.WriteLine("Initializing EvSystem");

			Timer.DelayCall(TimeSpan.FromSeconds(1), EventConfig.EventCheckDelay, new TimerCallback(Controller.TryExecuteEvents));
		}

		/// <summary>
		/// Enqueues an event
		/// </summary>
		public void RegisterEvent(IEvent ev)
		{
			if (ev == null || _events.ContainsKey(ev.Id))
				return;

			_events[ev.Id] = ev;
			ev.OnEventQueued();
		}

		/// <summary>
		/// Removes a queued event
		/// </summary>
		/// <returns>Event was found and removed</returns>
		public bool RemoveEvent(IEvent ev)
		{
			if (ev == null)
				return false;

			return RemoveEvent(ev.Id);
		}

		/// <summary>
		/// Removes a queued event
		/// </summary>
		/// <param name="id"><see cref="IEvent.Id"/></param>
		/// <returns>Event was found and removed</returns>
		public bool RemoveEvent(Guid id)
		{
			return _events.Remove(id);
		}

		void TryExecuteEvents()
		{
			while (_finishedEvents.Count > 0)
			{
				IEvent ev = _finishedEvents.Dequeue();
				ev.OnEventFinish();
			}

			for (int i = 0; i < _events.Count; i++)
			{
				IEvent ev = _events.ElementAt(i).Value;

				if (ev.TryExecute())
				{
					if (!ev.RequeueAfterExecution)
					{
						_finishedEvents.Enqueue(ev);
						RemoveEvent(ev);
						i--;
					}
					else
					{
						if (ev.FinishEvenWithRequeue)
						{
							_finishedEvents.Enqueue(ev);
						}

						ev.OnEventRequeue();
					}
				}
			}
		}
	}
}
