using System;
using System.Collections.Generic;
using System.Text;

namespace AppCompiler.Compiler
{
    public class ApplicationCompilerException : Exception
    {
        private System.CodeDom.Compiler.CompilerErrorCollection compilerErrors;
        public ApplicationCompilerException(string message,Exception inner,System.CodeDom.Compiler.CompilerErrorCollection compilerErrors) : base(message,inner)
        {
            this.compilerErrors = compilerErrors;
        }

        public System.CodeDom.Compiler.CompilerErrorCollection CompilerErrors
        {
            get { return compilerErrors; }
        }

    }
}
