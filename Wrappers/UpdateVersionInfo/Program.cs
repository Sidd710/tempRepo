using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UpdateVersionInfo
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                ShowUsage();
                Environment.Exit(1);
            }

            string targetFilename = args[1];
            string executableWithVersionInfo = args[0];

            using (var logger = new StreamWriter(@"c:\temp\logging.txt"))
            {
                logger.WriteLine(executableWithVersionInfo);
            }
                if (!File.Exists(executableWithVersionInfo))
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The specified input file does not exist.");
                Console.ForegroundColor = defaultColor;
                Environment.Exit(1);
            }

            VersionUpdater.UpdateVersionInfo(executableWithVersionInfo, targetFilename);
        }

        private static void ShowUsage()
        {
            Console.WriteLine("Updates version information for C++ version resource files");
            Console.WriteLine("Usage: ");
            Console.WriteLine("{0} [targetfilename] [exe with version info]", Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
