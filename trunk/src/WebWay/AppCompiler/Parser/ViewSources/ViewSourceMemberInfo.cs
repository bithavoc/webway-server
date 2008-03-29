using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace AppCompiler.Parser.ViewSources
{
    public class ViewSourceMemberInfo
    {
        private string content;

        [XmlText()]
        public string Content
        {
            get { return content; }
            set { content = value; }
        }
    }
}
