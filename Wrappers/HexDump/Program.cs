using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace HexDump
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

            string inputFilename = args[0];
            string outputFilename = args[1];

            if (!File.Exists(inputFilename))
            {
                var defaultColor = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The specified input file does not exist.");
                Console.ForegroundColor = defaultColor;
                Environment.Exit(1);
            }

            HexTools.DumpFile(inputFilename, outputFilename);
        }

        private static void ShowUsage()
        {
            Console.WriteLine("HexDump - dumps a (exe) file to C-style array");
            Console.WriteLine("Usage: ");
            Console.WriteLine("{0} [input-filename] [target file to generate array]", Assembly.GetExecutingAssembly().GetName().Name);
        }
    }
}
