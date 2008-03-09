using System;
using DevSandbox.WebServer;
using DevSandbox.WebServer.Hosted;

namespace webwayd
{
	class MainClass
	{
		public static void Main()
		{
			Console.WriteLine("webwayd 1.0");
			Console.WriteLine("Initializing");
			Server server = new Server();
			
			Console.WriteLine("\tInitializing Listeners");
			
			Console.WriteLine("\t\tListener at *:4427");
			RequestListener ls0 = new RequestListener(4427);
			server.Listeners.Add(ls0);
			
			Console.WriteLine("\tInitializing VirtualHosts");
			
			Console.WriteLine("\t\tVirtualhost at localhost:4427");
			VirtualHost vh0 = new VirtualHost();
			vh0.EndPoint = new VirtualHostEndPoint("localhost",4427);
			
			Console.WriteLine("\tUsing HostedRequestLinker");
			HostedRequestLinker hostedLinker = new HostedRequestLinker();
			vh0.RequestLinker = hostedLinker;
			hostedLinker.ProcessingRequest+=hostedLinkerRequest;
			server.VirtualHosts.Add(vh0);
			
			Console.WriteLine("\tStarting Server");
			server.Start();
			Console.WriteLine("\tServer Started and Listening... waiting for clients");
			Console.ReadLine();
		}
		
		private static void  hostedLinkerRequest(HostedRequestLinker linker,ProcessingRequestEventArgs args)
		{
			if(args.Context.Request.ResourcePath == "/")
			{
				Console.WriteLine("WebWayd found Resource path at Root");
				args.Context.Response.Write(string.Format("<h1>WebWay is alive at {0}</h1>",DateTime.Now.ToString()));
				for(int i = 0;i < 1500; i++)
				{
					args.Context.Response.Write(string.Format(@"<h6 style=""color:red"">{0}</h6>",i));
				}
				args.Context.Response.End();
			}
			else
			{
				args.Context.Response.StatusCode = 404;
				args.Context.Response.StatusReason = "NOT FOUND";
				args.Context.Response.Write("<h1>Not Found</h1>");
				args.Context.Response.End();
			}
			Console.WriteLine("WebWayd is about to create a response");
		}
		
	}//MainClass
}//webwayd