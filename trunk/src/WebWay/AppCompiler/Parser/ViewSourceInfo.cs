using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace AppCompiler.Parser
{
    [XmlType("ViewSource")]
    public class ViewSourceInfo : ViewBaseInfo
    {
        private string source  ;

        [XmlAttribute("fileName")]
        public string FileName
        {
            get { return source; }
            set { source = value; }
        }
    }
}
