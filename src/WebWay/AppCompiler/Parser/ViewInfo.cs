using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AppCompiler.Parser
{
    [XmlType("View")]
    public class ViewInfo : ViewBaseInfo
    {
        private string source  ;
        private string typeName;

        [XmlAttribute("path")]
        public string Path
        {
            get { return source; }
            set { source = value; }
        }
        [XmlAttribute("typeName")]
        public string TypeName
        {
            get { return typeName; }
            set { typeName = value; }
        }
    }
}
