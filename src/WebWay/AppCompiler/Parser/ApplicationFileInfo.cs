using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AppCompiler.Parser
{
    [XmlRoot("Application", Namespace = "http://dev-sandbox.com/DynamicView.xsd")]
    public class ApplicationFileInfo
    {
        private List<ReferenceInfo> references;
        private string name;
        private string version;

        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        [XmlAttribute("version")]
        public string Version
        {
            get { return version; }
            set { version = value; }
        }
        [XmlArrayItem("Reference",Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArray("References",Form=System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<ReferenceInfo> References
        {
            get { return references; }
            set { references = value; }
        }

        private List<ActionInfo> actions;

        [XmlArrayItem("Action", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArray("Actions", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<ActionInfo> Actions
        {
            get { return actions; }
            set { actions = value; }
        }

        private List<ViewBaseInfo> views;

        [XmlArrayItem("ViewSource", Type=typeof(ViewSourceInfo), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArrayItem("View", Type=typeof(ViewInfo), Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        [XmlArray("Views", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public List<ViewBaseInfo> Views
        {
            get { return views; }
            set { views = value; }
        }
    }
}
