using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.Web.Dynamic
{
    public struct ViewParameter
    {
        private string name;
        private object value;

        public ViewParameter(string name, object value)
        {
            this.name = name;
            this.value = value;
        }
        public string Name
        {
            get { return name; }
            set { name = value; }
        }

        public object Value
        {
            get { return value; }
            set { this.value = value; }
        }
    }
}
