using System;
using System.Collections.Generic;
using System.Text;

using DevSandbox.WebServer;
using DevSandbox.Web.Dynamic;
using DevSandbox.Web.Dynamic.Initiator;

namespace initiatord
{
    public class InitiatorServer : MarshalByRefObject
    {
        Server server;
        Initiator initor;
        public InitiatorServer()
        {
            server = new Server();
            initor = new Initiator(server);
        }
        public void RunFromFile(string file)
        {
            Console.WriteLine("Loading Initiator Configuration");
            initor.LoadFromFile(file);
            Console.WriteLine("Starting Server");
            server.Start();
            Console.WriteLine("Server Started and Listening");
        }
    }
}
