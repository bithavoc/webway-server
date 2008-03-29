using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.Web.Dynamic;

namespace MyDemoWebApp
{
    public class AuthenticateAction : Action
    {
        protected override void Execute()
        {
            this.RenderView("DemoApp.AuthenticateSuccess",new ViewParameter("username",Context.Request.PostParameters["name"]));
        }
    }
}
