// /home/jhernandez/Projects/MsTCP/DevSandbox.WebServer/IRequestLinker.cs created with MonoDevelop
// User: jhernandez at 1:38 AM 1/4/2008
//
// To change standard headers go to Edit->Preferences->Coding->Standard Headers
//

using System;

namespace DevSandbox.WebServer
{
	public interface IRequestLinker
	{
		void ProcessRequest(HttpContext context);
	}
}
