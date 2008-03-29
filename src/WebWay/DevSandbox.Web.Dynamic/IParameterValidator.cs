using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.Web.Dynamic
{
    interface IParameterValidator : IValidator
    {
        string ParameterName { get;set; }
    }
}
