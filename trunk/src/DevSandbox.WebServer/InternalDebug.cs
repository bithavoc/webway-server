using System;

namespace DevSandbox.WebServer
{
	internal class InternalDebug
	{
		[System.Diagnostics.ConditionalAttribute("NINGUNO")]
		internal static void trace(string val,params object[] vals)
		{
			Console.WriteLine("Trace: " + val,vals);
			Console.Out.Flush();
		}
	}
}
