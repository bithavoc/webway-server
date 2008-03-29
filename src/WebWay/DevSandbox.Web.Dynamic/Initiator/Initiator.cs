using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Reflection;
using System.IO;
using DevSandbox.WebServer;

namespace DevSandbox.Web.Dynamic.Initiator
{
    public class Initiator
    {
        private Server server;

        public Initiator(Server server)
        {
            this.server = server;
        }

        public Server Server
        {
            get 
            {
                return server; 
            }
        }

        public void LoadFromFile(string filePath)
        {
            InitiatorInfo info = InitiatorInfo.ParseAppFile(filePath);
            //string appDir = Path.GetDirectoryName(filePath);
            //Environment.CurrentDirectory = appDir;
            LoadFromInfo(info);
        }
        public void LoadFromInfo(InitiatorInfo info)
        {
            foreach (InitiatorVirtualHost ivh in info.VirtualHosts)
            {
                Assembly asm = Assembly.LoadFile(Path.GetFullPath(ivh.ApplicationAssembly));
                ApplicationInitiatorAttribute[] appInitAtts = (ApplicationInitiatorAttribute[])asm.GetCustomAttributes(typeof(ApplicationInitiatorAttribute), true);
                if (appInitAtts.Length == 0) continue;
                ApplicationInitiatorAttribute appInitAtt = appInitAtts[0];
                Application app = (Application)Activator.CreateInstance(appInitAtt.ApplicationType, null);

                VirtualHost vh = new VirtualHost();
                vh.RequestLinker = new ApplicationRequestLinker(app);
                vh.EndPoint = new VirtualHostEndPoint(ivh.Name, ivh.Port);
                this.server.Listeners.Add(new RequestListener(new System.Net.IPEndPoint(System.Net.IPAddress.Parse(ivh.ListenerIPAddress), ivh.Port), RequestListenerProtocolType.IPv4));
                server.VirtualHosts.Add(vh);
            }
        }
    }
}
