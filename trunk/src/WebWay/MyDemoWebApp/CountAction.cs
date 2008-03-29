using System;
using System.Collections.Generic;
using System.Text;
using DevSandbox.Web.Dynamic;

namespace MyDemoWebApp
{
    public class CountAction : Action
    {
        protected override void Execute()
        {
            this.RenderView("Counter.CounterSuccess", new ViewParameter("Fecha",DateTime.Now.ToLongTimeString()));
        }
    }
}
