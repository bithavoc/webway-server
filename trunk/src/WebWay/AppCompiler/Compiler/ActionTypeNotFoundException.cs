using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompiler.Compiler
{
    public class ActionTypeNotFoundException : ApplicationCompilerException
    {
        private string actionName;
        private string typeFullname;
        public ActionTypeNotFoundException(string actionName,string typefullname,string msg,Exception inner) : base(msg,inner,null)
        {
            this.actionName = actionName;
            this.typeFullname = typefullname;
        }

        public string ActionName
        {
            get { return actionName; }
        }
        public string TypeFullname
        {
            get { return typeFullname; }
        }
    }
}
