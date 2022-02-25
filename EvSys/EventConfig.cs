using System;

namespace Server.Custom.Skyfly.EvSys
{
	public class EventConfig
	{
		public static bool IsEnabled
		{
			get => _isEnabled;
			set
			{
				if (value == _isEnabled)
					return;

				_isEnabled = value;
				Config.Set("Events.IsEnabled", value);
			}
		}

		public static TimeSpan EventCheckDelay
		{
			get => _eventCheckDelay;
			set
			{
				if (value == _eventCheckDelay)
					return;

				_eventCheckDelay = value;
				Config.Set("Events.EventCheckDelay", value);
			}
		}

		static bool _isEnabled;
		static TimeSpan _eventCheckDelay;

		static EventConfig()
		{
			_isEnabled = Config.Get("Events.IsEnabled", true);
			_eventCheckDelay = Config.Get("Events.EventCheckDelay", TimeSpan.FromSeconds(1));
		}
	}
}
