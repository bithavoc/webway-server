using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.WebServer;

namespace DevSandbox.Web.Dynamic
{
    public sealed class ApplicationRequestLinker : IRequestLinker
    {
        
        bool isInitiated;
        Application app;
        public ApplicationRequestLinker(Application app)
        {
            this.app = app;
        }

        public void ProcessRequest(HttpContext context)
        {
            bool exists = this.app.ContainsAction(context.Request.ResourcePath);
            if (exists)
            {
                this.app.executeAction(context,context.Request.ResourcePath);
                context.Response.Flush();
                context.Response.End();
            }
            //Console.WriteLine(string.Format("Error, no hay action para '{0}'", context.Request.ResourcePath));
            context.Response.StatusCode = 404;
            context.Response.StatusReason = "NOT FOUND";
            context.Response.Write("<h1>Not Found</h1>");
            context.Response.Flush();
            context.Response.End();
        }

        public void Init()
        {
            this.app.Initialize();
            this.isInitiated = true;
        }

        #region IRequestLinker Members


        public bool IsInitiated
        {
            get { return isInitiated; }
        }

        #endregion
    }
}
