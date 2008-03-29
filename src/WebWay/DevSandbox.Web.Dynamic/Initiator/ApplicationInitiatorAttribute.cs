using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.Web.Dynamic.Initiator
{
    [AttributeUsage(AttributeTargets.Assembly)]
    public class ApplicationInitiatorAttribute : Attribute
    {
        public ApplicationInitiatorAttribute()
        {

        }
        private Type appClassPath;

        public Type ApplicationType
        {
            get { return appClassPath; }
            set { appClassPath = value; }
        }

    }
}
