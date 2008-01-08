using System;

namespace DevSandbox.WebServer
{
	public class VirtualHostEndPoint
	{
		private string name;
		private int port;
		public VirtualHostEndPoint()
		{
			
		}
		public VirtualHostEndPoint(string name,int port)
		{
			this.name = name;
			this.port = port;
		}
		public string Name
		{
			get
			{
				return this.name;
			}
			set
			{
				this.name = value;
			}
		}
		public int Port
		{
			get
			{
				return this.port;
			}
			set
			{
				this.port = value;
			}
		}
	}
}
