// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/RequestListener.cs created with MonoDevelop
// User: jhernandez at 8:11 AM 1/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;


namespace DevSandbox.WebServer
{
	public class RequestListener
	{
		public const int DefaultMaxPendingConnections = 600;
		private IPEndPoint networkEndPoint;
		private Server server;
		private Socket listenerSocket; //Socket that receives the connections.
		private RequestListenerProtocolType protocolType;
		private int maxPendingConnections = DefaultMaxPendingConnections;
		private Thread listenerThread;
		
		public RequestListener(IPEndPoint networkEndPoint,RequestListenerProtocolType protocolType)
		{
			this.networkEndPoint = networkEndPoint;
			this.listenerThread = new Thread(new ThreadStart(threadedListenerLoop));
			//Initialize Socket.
			this.listenerSocket = new Socket(networkEndPoint.AddressFamily,SocketType.Stream,parseProtocolType(protocolType));
		}
		
		private static ProtocolType parseProtocolType(RequestListenerProtocolType pt)
		{
			if(pt == RequestListenerProtocolType.IPv4)
			{
				return System.Net.Sockets.ProtocolType.IP;
			}
			else if(pt == RequestListenerProtocolType.IPv6)
			{
				return System.Net.Sockets.ProtocolType.IPv6;
			}
			else
			{
				throw new Exception(string.Format("Can not parse protocol type {0}",pt));
			}
		}
		
		public RequestListener(int port,RequestListenerProtocolType protocolType) : this(new IPEndPoint(IPAddress.Any,port),protocolType)
		{
			
		}
		public RequestListener(int port) : this(port,RequestListenerProtocolType.IPv4)
		{
			
		}
		
		public int MaxPendingConnections
		{
			get
			{
				return this.maxPendingConnections;
			}
			set
			{
				//Changes on this property will have effect only after restart the Listener.
				this.maxPendingConnections = value;
			}
		}
		
		internal void __setServer(Server server)
		{
			this.server = server;
		}
		
		public IPEndPoint NetworkEndPoint
		{
			get
			{
				return this.networkEndPoint;
			}
		}
		public RequestListenerProtocolType ProtocolType
		{
				get
				{
					return this.protocolType;
				}
		}
		
		internal void __start()
		{
		/*	if(this.server == null)
			{
				throw new Exception(string.Format("RequestListener on '{0}' is not working on any server",this.networkEndPoint));
			}
			if(this.isListening)
			{
				throw new Exception(string.Format("RequestListener on '{0}' is already started",this.networkEndPoint));
			}*/
			try
			{
				this.listenerSocket.Bind(this.networkEndPoint);
			}
			catch(SocketException sex)
			{
				throw new RequestListenerBindException(string.Format("Unable to bind listener to address '{0}'",this.NetworkEndPoint),sex);
			}
			try
			{
				this.listenerSocket.Listen(this.maxPendingConnections);
			}
			catch(SocketException sex)
			{
				throw new RequestListenerBindException(string.Format("Unable to listen on binded address '{0}'",this.NetworkEndPoint),sex);
			}
			this.listenerThread.Start();
			InternalDebug.trace("Listener Thread at {0} started",this.networkEndPoint);
		}
		
		internal void __stop()
		{
			/*if(!this.isListening)
			{
				throw new Exception(string.Format("RequestListener on '{0}' is already stopped",this.networkEndPoint));
			}*/
			InternalDebug.trace("Listener at {0} is stopping",this.networkEndPoint);
			this.listenerThread.Abort();
			this.listenerSocket.Shutdown(SocketShutdown.Both);
		}
		
		private void threadedListenerLoop()
		{
			
			while(true)
			{
				InternalDebug.trace("Listener Thread at {0} is waiting for clients",this.networkEndPoint);
				Socket clientSocket = this.listenerSocket.Accept();
				InternalDebug.trace("Listener at {0} just accepts a client",this.networkEndPoint);
				Connection conn = new Connection(clientSocket,this.server,this);
				conn.Listen();
				clientSocket = null;
			}
		}
	}
}
