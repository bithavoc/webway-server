using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.WebServer;

namespace DevSandbox.Web.Dynamic
{
    public abstract class View :IDisposable
    {
        private Dictionary<string, ViewParameter> parameters;

        public Dictionary<string, ViewParameter> Parameters
        {
            get { return parameters; }
        }

        private HttpContext context;
        public View()
        {
            
        }
        protected void Write(object content)
        {
            this.Write((string)content.ToString());
        }
        protected void Write(string content)
        {
            this.context.Response.Write(content);   
        }
        protected abstract void Render();

        internal void execute(HttpContext context, params ViewParameter[] parameters)
        {
            this.context = context;
            if (parameters != null && parameters.Length > 0)
            {
                this.parameters = new Dictionary<string, ViewParameter>();
                foreach (ViewParameter p in parameters)
                {
                    this.parameters.Add(p.Name, p);
                }
            }
            this.Render();
        }

        public virtual void Dispose()
        {
            
        }

        public object this[string parameterName]
        {
            get {
                if (this.parameters == null) return null;
                else
                    return this.parameters.ContainsKey(parameterName) ? this.parameters[parameterName].Value : null;
            }
        }
	
    }
}
