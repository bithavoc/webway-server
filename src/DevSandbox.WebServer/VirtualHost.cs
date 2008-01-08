using System;

namespace DevSandbox.WebServer
{
	public class VirtualHost
	{
		private  VirtualHostState state = VirtualHostState.Online;//Online byDefault
		private IRequestLinker requestLinker;
		private VirtualHostEndPoint endPoint;
		private Server server;
		
		public VirtualHost()
		{
			
		}
		
		internal void __setServer(Server server)
		{
			this.server = server;
		}
		
		public bool IsServed
		{
			get
			{
				//Indicate if this virtualhost is on a server.
				return this.server != null;
			}
		}
		public VirtualHostState State
		{
			get
			{
				return this.state;
			}
			set
			{
				this.state = value;
			}
		}
		
		//This method is called on a private thread for the request.
		internal void ProcessRequest(HttpContext context)
		{
			this.requestLinker.ProcessRequest(context);
		}
		
		public IRequestLinker RequestLinker
		{
			get
			{
				return this.requestLinker;
			}
			set
			{
				this.requestLinker = value;
			}
		}
		
		public VirtualHostEndPoint EndPoint
		{
			get
			{
				return this.endPoint;
			}
			set
			{
				this.endPoint = value;
			}
		}
	}
}
