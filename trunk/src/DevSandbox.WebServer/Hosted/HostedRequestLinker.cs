using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.WebServer.Hosted
{
   public class HostedRequestLinker : IRequestLinker
    {
       public event ProcessingRequestEventHandler ProcessingRequest;
       bool initiated;
        #region IRequestLinker Members

        public void ProcessRequest(HttpContext context)
        {
            if (ProcessingRequest != null)
            {
                ProcessingRequest(this,new ProcessingRequestEventArgs(context));
            }
        }

        #endregion

        #region IRequestLinker Members


        public void Init()
        {
            this.initiated = true;
        }

        #endregion

        #region IRequestLinker Members


        public bool IsInitiated
        {
            get { return this.initiated; }
        }

        #endregion
    }
}
