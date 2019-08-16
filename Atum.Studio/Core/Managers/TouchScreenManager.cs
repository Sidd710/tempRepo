using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    class TouchScreenManager
    {
        private const int SWP_NOSIZE = 0x0001;
        private const int SWP_NOZORDER = 0x0004;
        private const int SWP_SHOWWINDOW = 0x0040;

        internal static void ShowOSK(Control selectedControl)
        {
            if (UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode)
            {
                Task.Run(() =>
                    {
                        var arrProcs = Process.GetProcessesByName("osk");

                        if (arrProcs.Length == 0)
                        {
                            Process.Start("osk");
                        }
                    });
            }
        }
        
    }
}
