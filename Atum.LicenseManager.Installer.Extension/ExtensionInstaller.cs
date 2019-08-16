using System.ComponentModel;
using System.Configuration.Install;
using System.IO;

namespace Atum.LicenseManager.Installer.Extension
{
    [RunInstaller(true)]
    public partial class ExtensionInstaller : System.Configuration.Install.Installer
    {
        public ExtensionInstaller()
        {
            InitializeComponent();

           this.AfterInstall += ExtensionInstaller_AfterInstall;
           this.BeforeUninstall += ExtensionInstaller_BeforeUninstall;
        }

        private void ExtensionInstaller_BeforeUninstall(object sender, InstallEventArgs e)
        {
            var path = Path.GetDirectoryName(Context.Parameters["assemblypath"]);
            ServiceManager.RemoveService(Path.Combine(path, "Atum.LicenseManager.exe"));
        }

       
        private  void ExtensionInstaller_AfterInstall(object sender, InstallEventArgs e)
        {
            var path = Path.GetDirectoryName(Context.Parameters["assemblypath"]);
            FirewallManager.OpenFirewallForProgram(Path.Combine(path, "Atum.LicenseManager.exe"), "atum3D License Manager");
            ServiceManager.InstallService(Path.Combine(path, "Atum.LicenseManager.exe"), Context.Parameters["CustomerID"], path);
            ServiceManager.StartService("atum3D License Manager");

        }
        
    }
}
