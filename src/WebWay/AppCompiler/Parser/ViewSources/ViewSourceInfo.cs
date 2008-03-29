using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace AppCompiler.Parser.ViewSources
{
    [XmlRoot("View",Namespace="http://dev-sandbox.com/DynamicView.xsd")]
    public class ViewSourceInfo
    {
        private string name;
        private List<ViewSourceMemberInfo> body;
        private string ns;
        private string path;

        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [XmlAttribute("namespace")]
        public string Namespace
        {
            get { return ns; }
            set { ns = value; }
        }
        [XmlAttribute("path")]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        [XmlArray("Body", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Html",Type=typeof(HtmlMemberInfo),Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArrayItem("Code", Type = typeof(CodeMemberInfo), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<ViewSourceMemberInfo> Body
        {
            get { return body; }
            set { body = value; }
        }

        public static ViewSourceInfo ParseFile(string filePath)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read))
            {
                return ParseFile(fs);
            }
        }

        public static ViewSourceInfo ParseFile(Stream stream)
        {
            XmlAttributeOverrides over = new XmlAttributeOverrides();

            XmlSerializer ser = new XmlSerializer(typeof(ViewSourceInfo), over);
            ViewSourceInfo sourceInfo = (ViewSourceInfo)ser.Deserialize(stream);
            return sourceInfo;
        }
    }
}
