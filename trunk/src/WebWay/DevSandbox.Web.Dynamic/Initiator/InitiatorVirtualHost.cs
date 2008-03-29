using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DevSandbox.Web.Dynamic.Initiator
{
    public class InitiatorVirtualHost
    {
        private string name;
        private int port;
        private string applicationAssembly;
        private string listenerIPAddress;

        public InitiatorVirtualHost()
        {

        }

        public InitiatorVirtualHost(
            string name, 
            int port, 
            string applicationAssembly, 
            string listenerIPAddress)
        {
            this.name = name;
            this.port = port;
            this.applicationAssembly = applicationAssembly;
            this.listenerIPAddress = listenerIPAddress;
        }

        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        [XmlAttribute("port")]
        public int Port
        {
            get { return port; }
            set { port = value; }
        }

        [XmlAttribute("applicationAssembly")]
        public string ApplicationAssembly
        {
            get { return applicationAssembly; }
            set { applicationAssembly = value; }
        }
               
        [XmlAttribute("listener-ip")]
        public string ListenerIPAddress
        {
            get { return listenerIPAddress; }
            set { listenerIPAddress = value; }
        }
	
    }
}
