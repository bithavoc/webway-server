using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;

namespace initiatord
{
    class Program
    {
        static void writeDescription()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Webway-Server Initiator 1.0");
            Console.WriteLine("\twww.dev-sandbox.net/projects/webwayinit");
            Console.WriteLine("\tby Johan Hernandez(thepumpkin)");
            Console.ResetColor();
        }
        static void Main(string[] args)
        {
            writeDescription();
            if (args.Length == 0)
            {
                Console.WriteLine("You need to specify the Initiator Xml File");
                return;
            }
            string filename = Path.GetFullPath(args[0]);
            /*AppDomainSetup setup = new AppDomainSetup();
            
            string dir = Path.GetDirectoryName(filename);
            setup.ApplicationBase = dir;
            setup.PrivateBinPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);

            AppDomain serverDomain = AppDomain.CreateDomain("Initiator Domain", null, setup);
            
            InitiatorServer server = (InitiatorServer)serverDomain.CreateInstanceAndUnwrap(System.Reflection.Assembly.GetExecutingAssembly().FullName, "initiatord.InitiatorServer");*/
            InitiatorServer server = new InitiatorServer();
            server.RunFromFile(filename);
            Console.ReadKey();
        }
    }
}
