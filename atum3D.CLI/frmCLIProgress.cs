using Atum.Studio.Controls.NewGui;
using Atum.Studio.Core.Managers;
using atum3D.Notification;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Atum3D.CLI.Extentions;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Atum3D.CLI.Native;

namespace Atum3D.CLI
{
    public partial class frmCLIProgress : Form
    {
        public frmCLIProgress()
        {
            InitializeComponent();
        }


        private static SceneControlProgressbar _progressbarMain = new SceneControlProgressbar();

        private void FrmCLI_Shown(object sender, EventArgs e)
        {
        
        }

        private void FrmCLI_Load(object sender, EventArgs e)
        {
            var notificationControl = new NotificationControl();
            this.Controls.Add(_progressbarMain);
            this.Controls.Add(notificationControl);

            this.Width = (this.Width - this.ClientSize.Width) + _progressbarMain.Width;
            this.Height = (this.Height - this.ClientSize.Height) + _progressbarMain.Height ;
            this.Text = "Exporting...";
            this.Icon = BrandingManager.MainForm_Icon;
           
            this.StartPosition = FormStartPosition.Manual;

            var firstNetfabbProcess = Process.GetProcessesByName("netfabb").FirstOrDefault();
            //develop

            if (firstNetfabbProcess == null)
            {
                firstNetfabbProcess = Process.GetProcessesByName("autodesknetfabb_debug").FirstOrDefault();
            }

            if (firstNetfabbProcess != null)
            {
                foreach (var handle in NATIVE.EnumerateProcessWindowHandles(firstNetfabbProcess.Id))
                {
                    var windowTextLength = NATIVE.GetWindowTextLength(handle);
                    var windowText = new StringBuilder(windowTextLength + 1);
                    NATIVE.GetWindowText(handle, windowText, windowTextLength);
                    if (windowText.ToString().ToLower().Contains("autodesk netfabb"))
                    {
                        var windowInfo = new NATIVE.WINDOWINFO();
                        var result = NATIVE.GetWindowInfo(handle, ref windowInfo);
                        this.Left = windowInfo.rcClient.Left + ((windowInfo.rcWindow.Right - windowInfo.rcWindow.Left) / 2) - (this.Width / 2);
                        this.Top = windowInfo.rcWindow.Top + ((windowInfo.rcWindow.Bottom - windowInfo.rcWindow.Top) / 2) + (this.Height / 2);
                        break;
                    }
                }
            }
        }

        internal void SetPercentage(float percentage)
        {
            _progressbarMain.ProgressValue = percentage;
        }
    }
}
