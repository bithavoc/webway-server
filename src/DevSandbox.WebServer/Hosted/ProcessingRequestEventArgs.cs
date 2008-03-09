using System;
using System.Collections.Generic;
using System.Text;

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
            get { return context; }
        }

    }
}
