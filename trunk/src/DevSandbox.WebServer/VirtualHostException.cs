// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/VirtualHostException.cs created with MonoDevelop
// User: jhernandez at 7:50 AM 1/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DevSandbox.WebServer
{
	
	
	public class VirtualHostException : Exception
	{
		
		public VirtualHostException(string message,Exception innerException) : base(message,innerException)
		{
		}
	}
}
