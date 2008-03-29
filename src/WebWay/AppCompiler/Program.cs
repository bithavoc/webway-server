using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Xml;
using System.CodeDom;
using System.CodeDom.Compiler;

namespace AppCompiler
{
    class Program
    {
        const int EXIT_ERR_NO_FILE = -23;
        const int EXIT_ERR_NO_ARGS = -24;

        private static void exitError(string error,int errorCode)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(error);
            Console.ResetColor();
            System.Environment.Exit(errorCode);
        }

        static void exitSuccess(int code)
        {
            System.Environment.Exit(code);
        }

        static void writeDescription()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("WebwayMVC 1.0 Application Initiator Compiler");
            Console.WriteLine("\twww.dev-sandbox.net/projects/webwaymvc");
            Console.WriteLine("\tby Johan Hernandez(thepumpkin)");
            Console.ResetColor();
            exitError("No arguments have been supplied", EXIT_ERR_NO_ARGS);
        }
        static void writeColored(ConsoleColor color, string text)
        {
            Console.ForegroundColor = color;
            Console.Write(text);
            Console.ResetColor();
        }
        static void writeColoredLine(ConsoleColor color, string text,params object[] prms)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text,prms);
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                writeDescription();
            }
            string appFile = args[0];
            if (!File.Exists(appFile)) exitError(string.Format("'{0}' file does not exists", appFile), EXIT_ERR_NO_FILE);
            string appDir = Path.GetDirectoryName(appFile);
            System.Environment.CurrentDirectory = appDir;

             Parser.ApplicationFileInfo appInfo = Parser.ApplicationFileParser.ParseAppFile(appFile);
             Compiler.ApplicationCompiler appCompiler = new AppCompiler.Compiler.ApplicationCompiler(appInfo);
             writeColoredLine(ConsoleColor.Gray, "Validating Application File... ");
             try
             {
                 appCompiler.Validate();
                 writeColoredLine(ConsoleColor.Green, "\tSUCCESS");
                 appCompiler.Run();
                 appCompiler.ExportAndCompileAll(appDir);
             }
             catch (Compiler.ActionTypeNotFoundException actionEx)
             {
                 writeColoredLine(ConsoleColor.Red, "\t\tWD-010: Action type could not be loaded. ActionName='{0}',TypeFullName='{1}'", actionEx.ActionName, actionEx.TypeFullname);
                 writeColoredLine(ConsoleColor.Red, "\tFAILED");
             }
             catch (Compiler.ApplicationCompilerException compilationEx)
             {
                 Console.ForegroundColor = ConsoleColor.Red;
                 foreach(CompilerError error in compilationEx.CompilerErrors)
                 {
                     
                     Console.WriteLine("Error: '{0}'", error.ToString());
                 }
                 Console.ResetColor();
             }
             finally
             {
                 
             }
             
        }
    }
}
