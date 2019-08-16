using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace UpdateInstallerProject
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 2)
            {
                Console.Error.WriteLine("Usage: {0} [ProjectFileName] [Version]", Assembly.GetExecutingAssembly().GetName().Name);
                Environment.Exit(1);
            }
            else
            {
                string projectFilename = args[0];
                string version = args[1];
                InstallerProjectUpdater.UpdateInstallerProject(projectFilename, version);
                Environment.Exit(0);
            }
        }
    }
}
