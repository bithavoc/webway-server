using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.Web.Dynamic;

namespace MyDemoWebApp
{
    public class LoginAction : Action
    {
        protected override void Initialize()
        {
            base.Initialize();
        }
        protected override void Execute()
        {
            this.RenderView("DemoApp.Login");
        }
        protected override bool Validate()
        {
            return base.Validate();
        }
    }
}
