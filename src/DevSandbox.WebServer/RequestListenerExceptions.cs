// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/RequestListenerExceptions.cs created with MonoDevelop
// User: jhernandez at 4:57 PM 1/6/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DevSandbox.WebServer
{
	public class RequestListenerBindException : Exception
	{		
		public RequestListenerBindException(string message,Exception innerException) : base(message,innerException)
		{
			
		}
	}
}
