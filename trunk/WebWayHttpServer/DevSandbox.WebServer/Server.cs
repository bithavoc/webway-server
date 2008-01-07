// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/Server.cs created with MonoDevelop
// User: jhernandez at 1:32 AM 1/4/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DevSandbox.WebServer
{
	public class Server
	{
		private VirtualHostCollection virtualHosts;
		private RequestListenerCollection listeners;
		
		public Server()
		{
			this.virtualHosts = new VirtualHostCollection(this);
			this.listeners = new RequestListenerCollection(this);
		}
		
		private void changeListenerStatus(bool toOnline)
		{
			
			foreach(RequestListener listener in this.listeners)
			{
				if(toOnline) listener.__start();
				else listener.__stop();
			}
		}
		
		public void Start()
		{
			InternalDebug.trace("Starting Server");
			InternalDebug.trace("Bringing all Listener to online");
			changeListenerStatus(true);//Bring all Listener to online
		}//Start
		
		public void Stop()
		{
			changeListenerStatus(true);//Bring all Listener to offline
		}//Stop
		
		public VirtualHostCollection VirtualHosts
		{
			get
			{
				return this.virtualHosts;
			}
		}//VirtualHosts
		
		public RequestListenerCollection Listeners
		{
			get
			{
				return this.listeners;
			}
		}//Listeners
		
		internal void __processRequestFromListener(RequestListener listener,Request request,Response response)
		{
			
			//RequestListener invokes this method after Parsing the request.
			/*InternalDebug.trace("Host {0}",request.Hostname);
			InternalDebug.trace("HostPort {0}",request.Port);
			InternalDebug.trace("UserAgent {0}",request.UserAgent);
			InternalDebug.trace("Request Received from Listener");
			*/
			//Find the virtualhost and delegate the request.
			InternalDebug.trace("Finding Virtualhost");
			foreach(VirtualHost vh in this.virtualHosts)
			{
				if(vh.EndPoint.Name == request.Hostname && vh.EndPoint.Port == request.Port && vh.State == VirtualHostState.Online)
				{
					InternalDebug.trace("Virtualhost found");
					HttpContext context = new HttpContext(request,response,this);
					//Spare a Thread for the request.
					System.Threading.ThreadPool.QueueUserWorkItem(delegate
					                                              {
						vh.ProcessRequest(context);
					});
					break;
				}//if
			}//foreach
			
		}//processRequestFromListener
		
		public class VirtualHostCollection : System.Collections.CollectionBase
		{
			private Server server;
			internal VirtualHostCollection(Server server)
			{
				this.server = server;
			}
			
			public void Add(VirtualHost virtualHost)
			{
				virtualHost.__setServer(this.server);
				this.List.Add(virtualHost);
			}
			public void Remove(VirtualHost virtualHost)
			{
				this.List.Remove(virtualHost);
			}
		}//VirtualHostCollection

		public class RequestListenerCollection : System.Collections.CollectionBase
		{
			private Server server;
			internal RequestListenerCollection(Server server)
			{
				this.server = server;
			}
			
			public void Add(RequestListener listener)
			{
				this.List.Add(listener);
				listener.__setServer(this.server);
			}
			public void Remove(RequestListener listener)
			{
				this.List.Remove(listener);
			}
		}//VirtualHostCollection
	}//Server
}//ns
