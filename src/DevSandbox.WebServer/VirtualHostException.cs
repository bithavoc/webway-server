using System;

namespace DevSandbox.WebServer
{
	
	
	public class VirtualHostException : Exception
	{
		
		public VirtualHostException(string message,Exception innerException) : base(message,innerException)
		{
		}
	}//VirtualHostException
}
