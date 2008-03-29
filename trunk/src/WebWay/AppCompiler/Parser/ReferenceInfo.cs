using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AppCompiler.Parser
{
    [XmlType("Reference",Namespace="http://dev-sandbox.com/DynamicView.xsd")]
    public class ReferenceInfo
    {
        private string name;

        [XmlAttribute("name")]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

    }
}
