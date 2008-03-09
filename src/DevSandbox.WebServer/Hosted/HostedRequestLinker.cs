using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.WebServer.Hosted
{
   public class HostedRequestLinker : IRequestLinker
    {
       public ProcessingRequestEventHandler ProcessingRequest;
        #region IRequestLinker Members

        public void ProcessRequest(HttpContext context)
        {
            if (ProcessingRequest != null)
            {
                ProcessingRequest(this,new ProcessingRequestEventArgs(context));
            }
        }

        #endregion
    }
}
