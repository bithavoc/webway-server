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
