using System;

namespace DevSandbox.WebServer
{
	public class HttpContext
	{
		private Request request;
		private Response response;
		private Server server;
		
		internal HttpContext(Request request,Response response,Server server)
		{
			this.request = request;
			this.response = response;
			this.response.Context = this;
			this.server = server;
		}
		public Request Request
		{
			get
			{
				return this.request;
			}
		}
		public Response Response
		{
			get
			{
				return this.response;
			}
		}
		public Server Server
		{
			get
			{
				return this.server;
			}
		}
	}
}
