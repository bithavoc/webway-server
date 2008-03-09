using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.WebServer.Hosted
{
    public delegate void ProcessingRequestEventHandler(HostedRequestLinker linker,ProcessingRequestEventArgs args);
}
