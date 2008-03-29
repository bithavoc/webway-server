using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.IO;

namespace DevSandbox.Web.Dynamic.Initiator
{
    [XmlRoot("Initiator")]
    public class InitiatorInfo
    {
        public InitiatorInfo()
        {
            this.virtualHosts = new List<InitiatorVirtualHost>();
        }
        public static InitiatorInfo ParseAppFile(string filePath)
        {

            XmlAttributeOverrides over = new XmlAttributeOverrides();

            XmlSerializer ser = new XmlSerializer(typeof(InitiatorInfo), over);
            using (StreamReader reader = new StreamReader(filePath))
            {
                InitiatorInfo appInfo = (InitiatorInfo)ser.Deserialize(reader);
                return appInfo;
            }
        }

        private List<InitiatorVirtualHost> virtualHosts;


        [XmlArray("Hosts")]
        [XmlArrayItem("VirtualHost",Form=XmlSchemaForm.Unqualified)]
        public List<InitiatorVirtualHost> VirtualHosts
        {
            get { return virtualHosts; }
            set { virtualHosts = value; }
        }

    }
}
