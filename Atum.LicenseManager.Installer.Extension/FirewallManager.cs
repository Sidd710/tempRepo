using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.LicenseManager.Installer.Extension
{
    class FirewallManager
    {
        internal static void OpenFirewallForProgram(string exeFileName, string displayName)
        {
            var proc = Process.Start(
                new ProcessStartInfo
                {
                    FileName = "netsh",
                    Arguments =
                            string.Format(
                                "advfirewall firewall add rule program=\"{0}\" name=\"{1}\" profile=any dir=in action=allow",
                                exeFileName, displayName),
                    WindowStyle = ProcessWindowStyle.Hidden
                });
            proc.WaitForExit();
        }
    }
}
