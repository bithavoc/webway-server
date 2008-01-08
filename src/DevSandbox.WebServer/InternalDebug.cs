// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/InternalDebug.cs created with MonoDevelop
// User: jhernandez at 10:12 AM 1/6/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

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
