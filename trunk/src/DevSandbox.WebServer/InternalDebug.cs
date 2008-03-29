using System;

namespace DevSandbox.WebServer
{
	internal class InternalDebug
	{
        public const string DEBUG_SYMBOL = "NINGUNO";
        [System.Diagnostics.ConditionalAttribute(DEBUG_SYMBOL)]
		internal static void trace(string val,params object[] vals)
		{
			Console.WriteLine("Trace: " + val,vals);
			Console.Out.Flush();
		}
	}
}
