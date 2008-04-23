using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.WebServer
{
    public class ConnectionClosedException : Exception
    {
        public ConnectionClosedException(string message) : base(message)
        {

        }
	
    }
}
