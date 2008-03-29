using System;
using System.Collections.Generic;
using System.Text;
using AppCompiler.Parser;
using System.Reflection;
using System.CodeDom;
using Microsoft.CSharp;
using System.CodeDom.Compiler;
using System.IO;

namespace AppCompiler.Compiler
{
    public class ApplicationCompiler
    {
        private ApplicationFileInfo appInfo;
        private CodeCompileUnit appCompileUnit;
        private CodeMemberMethod initializeMethod  = null;
        public ApplicationCompiler(ApplicationFileInfo appInfo)
        {
            this.appInfo = appInfo;
        }

        public void Validate()
        {
            /*Assembly asm = null;
            foreach (ReferenceInfo refInfo in this.appInfo.References)
            {
                asm = Assembly.LoadFile(Path.GetFullPath(refInfo.Name));
               
            }
            foreach (Parser.ActionInfo action in this.appInfo.Actions)
            {
                Type t = asm.GetType(action.Type,true,true);
                if (t == null)
                {
                    throw new ActionTypeNotFoundException(action.Path, action.Type, string.Format("Action '{0}' has an undeclared or not references type '{1}'", action.Path, action.Type), null);
                }
            }
             * */
        }
        
        public void Run()
        {
            this.Validate();
            CodeCompileUnit unit = this.appCompileUnit = new CodeCompileUnit();
            unit.ReferencedAssemblies.Add("DevSandbox.Web.Dynamic.dll");
            unit.ReferencedAssemblies.Add("DevSandbox.WebServer.dll");
            
            //Referencias para Compilacion
            foreach (ReferenceInfo refInfo in this.appInfo.References)
            {
                unit.ReferencedAssemblies.Add(refInfo.Name);   
            }
            CodeNamespace appNamespace = new CodeNamespace();
            appNamespace.Name = this.appInfo.Name;
            appNamespace.Imports.Add(new CodeNamespaceImport("DevSandbox.Web.Dynamic"));
            appNamespace.Imports.Add(new CodeNamespaceImport("DevSandbox.Web.Dynamic.Initiator"));
            
            
            CodeTypeDeclaration appTypeDec = new CodeTypeDeclaration(this.appInfo.Name + "Application");
            string appClassFullQNs = appNamespace.Name + "." + appTypeDec.Name;
            //Is a class
            appTypeDec.IsClass = true;

            //AppClass inherits from DevSandbox.Web.Dynamic.Application
            CodeTypeReference dynappRef = new CodeTypeReference(typeof(DevSandbox.Web.Dynamic.Application), CodeTypeReferenceOptions.GenericTypeParameter);
            appTypeDec.BaseTypes.Add(dynappRef);

            this.initializeMethod = new CodeMemberMethod();
            initializeMethod.Name = "Initialize";
            initializeMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;

            foreach (Parser.ActionInfo action in this.appInfo.Actions)
            {
                CodeMethodInvokeExpression registerActionCall = 
                    new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
                        new CodeThisReferenceExpression(),"RegisterAction"), 
                        new CodePrimitiveExpression(action.Path), 
                        new CodeTypeOfExpression(action.Type));
                initializeMethod.Statements.Add(registerActionCall);
            }
            
            foreach (Parser.ViewBaseInfo bview in this.appInfo.Views)
            {
                if (bview is ViewInfo)
                {
                    ViewInfo view = (ViewInfo)bview;
                    addRegisterViewCall(view);
                }
                if (bview is ViewSourceInfo)
                {
                    ViewSourceInfo sview = (ViewSourceInfo)bview;
                    CompileView(sview.FileName);
                }
            }

            appTypeDec.Members.Add(initializeMethod);
            appNamespace.Types.Add(appTypeDec);
            unit.Namespaces.Add(appNamespace);

            unit.AssemblyCustomAttributes.Add(
                new CodeAttributeDeclaration(
                "DevSandbox.Web.Dynamic.Initiator.ApplicationInitiatorAttribute",
                new CodeAttributeArgument("ApplicationType",new CodeTypeOfExpression(appClassFullQNs))));

            
        }
        void addRegisterViewCall(ViewInfo view)
        {
            CodeMethodInvokeExpression registerViewCall =
                        new CodeMethodInvokeExpression(new CodeMethodReferenceExpression(
                            new CodeThisReferenceExpression(), "RegisterView"),
                            new CodePrimitiveExpression(view.Path),
                            new CodeTypeOfExpression(view.TypeName));
            initializeMethod.Statements.Add(registerViewCall);
        }
        public void ExportSourceCode(string outputSourceCodeFile)
        {
            CodeGeneratorOptions codeGenOpt = new CodeGeneratorOptions();
            codeGenOpt.VerbatimOrder = true;
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            using (StreamWriter writer = new StreamWriter(outputSourceCodeFile, false))
            {
                codeProvider.GenerateCodeFromCompileUnit(this.appCompileUnit, writer, codeGenOpt);
            }
        }
        public void Compile(string outputDirectory)
        {
            CSharpCodeProvider codeProvider = new CSharpCodeProvider();
            ICodeCompiler codeCompiler = codeProvider.CreateCompiler();
            CompilerParameters parameters = new CompilerParameters();
            parameters.GenerateExecutable = false;
            parameters.GenerateInMemory = false;
            parameters.OutputAssembly =Path.Combine(outputDirectory,this.appInfo.Name + "-Initiator.dll");

            CompilerResults results = codeCompiler.CompileAssemblyFromDom(parameters, this.appCompileUnit);
            if (results.Errors.Count > 0)
            {
                throw new ApplicationCompilerException("Unexpected errors during Initiator assembly compilation", null, results.Errors);
            }
        }
        public void ExportAndCompileAll(string outputDirectory)
        {
            this.ExportSourceCode(Path.Combine(outputDirectory, this.appInfo.Name + "-Initiator.cs"));
            this.Compile(outputDirectory);
        }

         public void CompileView(string filePath)
        {
            Parser.ViewSources.ViewSourceInfo sourceInfo = Parser.ViewSources.ViewSourceInfo.ParseFile(filePath);
             
            CodeNamespace viewNs = new CodeNamespace();
            viewNs.Name = sourceInfo.Namespace;
            this.appCompileUnit.Namespaces.Add(viewNs);

            CodeTypeDeclaration viewDec = new CodeTypeDeclaration(sourceInfo.Name);
            //Is a class
            viewDec.IsClass = true;

            //Inhierits from View
            viewDec.BaseTypes.Add(new CodeTypeReference(typeof(DevSandbox.Web.Dynamic.View)));
            CodeMemberMethod renderMethod = new CodeMemberMethod();
            renderMethod.Name = "Render";
            renderMethod.Attributes = MemberAttributes.Family | MemberAttributes.Override;

            foreach (Parser.ViewSources.ViewSourceMemberInfo member in sourceInfo.Body)
            {
                if (member is Parser.ViewSources.HtmlMemberInfo)
                {
                    //Call the View.Write method with the Literal for the HTML Code.
                    Parser.ViewSources.HtmlMemberInfo htmlCode = (Parser.ViewSources.HtmlMemberInfo)member;
                    CodeMethodInvokeExpression methodInvoke = new CodeMethodInvokeExpression(new CodeThisReferenceExpression(),"Write", new CodePrimitiveExpression(htmlCode.Content));
                    renderMethod.Statements.Add(methodInvoke);
                }
                if (member is Parser.ViewSources.CodeMemberInfo)
                {
                    //Paste the code snippet.
                    Parser.ViewSources.CodeMemberInfo code = (Parser.ViewSources.CodeMemberInfo)member;
                    CodeSnippetStatement statement = new CodeSnippetStatement(code.Content);
                    renderMethod.Statements.Add(statement);
                }
            }

            viewDec.Members.Add(renderMethod);
            viewNs.Types.Add(viewDec);

            ViewInfo viewInfo4Register = new ViewInfo();
            viewInfo4Register.Path = sourceInfo.Path;
            viewInfo4Register.TypeName = string.Format("{0}.{1}", viewNs.Name, viewDec.Name);
            addRegisterViewCall(viewInfo4Register);
        }
    }
}
