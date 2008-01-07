// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/HostedRequestLinker.cs created with MonoDevelop
// User: jhernandez at 7:38 AM 1/5/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DevSandbox.WebServer.Hosted
{
	public class HostedRequestLinker : IRequestLinker
	{		
		public HostedRequestLinker()
		{
			
		}
		
		public void ProcessRequest(HttpContext context)
		{
			if(this.ProcessingRequest != null)
			{
				this.ProcessingRequest(this,new ProcessingRequestEventArgs(context));
			}
		}

		public event ProcessingRequestEventHandler ProcessingRequest;
	}//HostedRequestLinker
}//ns
