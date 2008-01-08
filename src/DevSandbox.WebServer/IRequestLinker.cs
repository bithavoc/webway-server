using System;

namespace DevSandbox.WebServer
{
	public interface IRequestLinker
	{
		void ProcessRequest(HttpContext context);
	}
}
