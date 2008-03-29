using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.WebServer;

namespace DevSandbox.Web.Dynamic
{
    public abstract class Action : IDisposable
    {
        private List<IValidator> validators;
        private Application app;
        private HttpContext context;

        public HttpContext Context
        {
            get { return context; }
        }

        public Action()
        {
            this.validators = new List<IValidator>();
        }

        public Application Application
        {
            get
            {
                return this.app;
            }
        }

        protected virtual void Initialize()
        {

        }

        protected void RegisterValidator(IValidator validator)
        {

        }

        protected abstract void Execute();

        protected virtual void HandleError()
        {

        }

        protected virtual bool Validate()
        {
            
            return true;
        }

        private bool validate()
        {
            if (this.validators.Count > 0)
            {
                foreach (IValidator validator in this.validators)
                {
                    if (!validator.Validate()) return false;
                }
            }
            return this.Validate();
        }

        internal void run(Application app,HttpContext context)
        {
            this.app = app;
            this.context = context;

            this.Initialize();
            if (!validate())
            {
                this.HandleError();
            }
            this.Execute();
        }

        public virtual void Dispose()
        {

        }

        protected void RenderView(string path, params ViewParameter[] parameters)
        {
            this.app.renderView(this.context, path, parameters);
        }

    }
}