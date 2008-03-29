using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AppCompiler.Parser
{
    public static class ApplicationFileParser
    {
        public static ApplicationFileInfo ParseAppFile(string filePath)
        {
            /*using (FileStream fs = new FileStream(filePath+".xml", FileMode.Create, FileAccess.Write))
            {
                WriteAppFile(fs);
            }*/
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return ParseAppFile(fs);
            }
        }

        public static ApplicationFileInfo ParseAppFile(Stream stream)
        {
            XmlAttributeOverrides over = new XmlAttributeOverrides();
            
            XmlSerializer ser = new XmlSerializer(typeof(ApplicationFileInfo), over);
            ApplicationFileInfo appInfo = (ApplicationFileInfo)ser.Deserialize(stream);
            return appInfo;
        }

        /*public static void WriteAppFile(Stream stream)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("dynamic", "http://dev-sandbox.com/DynamicView.xsd");
            XmlSerializer ser = new XmlSerializer(typeof(ApplicationFileInfo));
            
            ApplicationFileInfo appInfo = new ApplicationFileInfo();
            appInfo.Name = "App1";
            appInfo.Version = "2.1";
            appInfo.References = new List<ReferenceInfo>();
            ReferenceInfo rf = new ReferenceInfo();
            rf.Name = "System.data.dll";
            appInfo.References.Add(rf);
            rf = new ReferenceInfo();
            rf.Name = "System.web.dll";
            appInfo.References.Add(rf);
            ser.Serialize(stream, appInfo, ns);
        }*/
    }
}
