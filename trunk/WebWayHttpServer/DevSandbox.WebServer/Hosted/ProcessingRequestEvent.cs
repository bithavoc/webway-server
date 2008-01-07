// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/Hosted/ProcessingRequestEvent.cs created with MonoDevelop
// User: jhernandez at 2:31 PM 1/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DevSandbox.WebServer.Hosted
{
		public class ProcessingRequestEventArgs
		{
			private HttpContext context;
			public ProcessingRequestEventArgs(HttpContext context)
			{
				this.context = context;
			}
			public HttpContext Context
			{
				get
				{
					return this.context;
				}
			}
		}//ProcessingRequestEventArgs
		public delegate void ProcessingRequestEventHandler(HostedRequestLinker linker,ProcessingRequestEventArgs args);
}
