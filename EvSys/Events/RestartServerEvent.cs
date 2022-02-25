using System;
using Server.Custom.Skyfly.EvSys.EventTypes;

namespace Server.Custom.Skyfly.EvSys.Events
{
	public class RestartServerEvent : BaseDateEvent
	{
		public RestartServerEvent(DateTime restartTime) : base(restartTime)
		{

		}

		public RestartServerEvent() : this(DateTime.Now.AddDays(1))
		{

		}

		protected override bool Execute()
		{
			World.Save();
			Core.Kill(true);

			return true;
		}

		protected override DateTime GetNextDate()
		{
			return DateTime.MaxValue;
		}
	}
}
