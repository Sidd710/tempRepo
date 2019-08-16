using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration.Install;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.LicenseManager.Installer.Extension
{
    class ServiceManager
    {
        public static void StartService(string serviceName)
        {
            ServiceController service = new ServiceController(serviceName);
            service.Start();
        }

        public static void InstallService(string exeFilename, string customerID, string assemblyPath)
        {
            string[] commandLineOptions = new string[] {};

            var customerLicPath = System.IO.Path.Combine(assemblyPath, "Customer.dat");
            using (var streamWriter = new System.IO.StreamWriter(customerLicPath, false))
            {
                streamWriter.WriteLine(customerID);
            }
            var installer = new AssemblyInstaller(exeFilename, commandLineOptions);

            installer.UseNewContext = true;
            installer.Install(null);
            installer.Commit(null);

        }

        public static void RemoveService(string exeFilename)
        {
            var serviceInstaller = new ServiceInstaller();
            InstallContext Context = new InstallContext("uninstall.log", null);
            serviceInstaller.Context = Context;
            serviceInstaller.ServiceName = "atum3D License Manager";
            serviceInstaller.Uninstall(null);
        }
    }
}
