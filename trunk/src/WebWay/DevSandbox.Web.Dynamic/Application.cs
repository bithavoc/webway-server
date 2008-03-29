using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.WebServer;

namespace DevSandbox.Web.Dynamic
{
    public abstract class Application
    {
        private Dictionary<string, Type> actions;
        private Dictionary<string, Type> views;

        public Application()
        {
            this.actions = new Dictionary<string, Type>();
            this.views = new Dictionary<string, Type>();
        }

        public bool ContainsAction(string path)
        {
            return this.actions.ContainsKey(path);
        }

        public void RegisterAction(string path, Type actionClassType)
        {
            this.actions.Add(path, actionClassType);
        }

        internal Action executeAction(HttpContext context, string path)
        {
            Type tp = this.actions[path];
            Action actionObj = (Action)Activator.CreateInstance(tp, null);
            actionObj.run(this,context);
            return actionObj;
        }

        internal View renderView(HttpContext context, string path,ViewParameter[] viewParameters)
        {
            Type tp = this.views[path];
            View viewObj = (View)Activator.CreateInstance(tp, null);
            viewObj.execute(context,viewParameters);
            return viewObj;
        }

        public void RegisterView(string path, Type actionClassType)
        {
            this.views.Add(path, actionClassType);
        }

        protected internal abstract void Initialize();
    }
}
