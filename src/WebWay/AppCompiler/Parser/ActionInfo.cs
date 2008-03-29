using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AppCompiler.Parser
{
    [XmlType("Action")]
    public class ActionInfo
    {
        private string path;
        private string type;

        [XmlAttribute("path")]
        public string Path
        {
            get { return path; }
            set { path = value; }
        }
        [XmlAttribute("type")]
        public string Type
        {
            get { return type; }
            set { type = value; }
        }

    }
}
