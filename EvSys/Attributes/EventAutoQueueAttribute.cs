using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server.Custom.Skyfly.EvSys.Attributes
{
	/// <summary>
	/// Automatically instantiates and queues the event which implements this attribute upon server start
	/// <para>This requires a constructor with no parameters</para>
	/// </summary>
	[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
	public sealed class EventAutoQueueAttribute : Attribute
	{
	}
}
