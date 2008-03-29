using System;
using System.Collections.Generic;
using System.Text;

namespace DevSandbox.Web.Dynamic
{
    public interface IValidator
    {
        bool IsValid { get;set; }
        bool Validate();
    }
}
