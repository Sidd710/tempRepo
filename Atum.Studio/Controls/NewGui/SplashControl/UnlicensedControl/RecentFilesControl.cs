using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using OpenTK;
using Atum.Studio.Core.Engines;
using System.Drawing.Drawing2D;
using Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl;

namespace Atum.Studio.Controls.NewGui.SplashControl
{
    public partial class RecentFilesControl : UserControl
    {

        internal event EventHandler SelectedRecentFileChanged;
        internal event EventHandler OpenExistsProject;
        internal event EventHandler OpenNewProject;
        internal event EventHandler ControlClosed;

        public RecentFilesControl()
        {
            InitializeComponent();

            this.pbClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_white, this.pbClose.Size);
            this.pbPlusSign.BackColor = BrandingManager.Button_HighlightColor;
            
            InitRecentFilesControl();
        }

        private void InitRecentFilesControl()
        {
            if (FontManager.Loaded)
            {
                this.lblHeader.Font = FontManager.Montserrat16Bold;
            }

            plRecentFiles.Controls.Clear();
            if (Core.Managers.UserProfileManager.UserProfile != null)
            {
                var recentOpenFilesHeight = 0;
                var recentfiles = Core.Managers.UserProfileManager.UserProfile.GetRecentOpenedFiles();
                
                foreach (var recentfile in recentfiles)
                {
                    var recentOpenFileControl = new RecentOpenFileControl(recentfile);
                    recentOpenFileControl.onClick += RecentOpenFileControl_onClick;
                    recentOpenFileControl.Left = 0;
                    recentOpenFileControl.Top = recentOpenFilesHeight;
                    plRecentFiles.Controls.Add(recentOpenFileControl);
                    recentOpenFilesHeight += recentOpenFileControl.Height;
                }
                plRecentFiles.AutoScroll = true;
            }
        }

        private void RecentOpenFileControl_onClick(object sender, EventArgs e)
        {
            var recentOpenFileControl = sender as RecentOpenFileControl;
            var recentOpenedFile = recentOpenFileControl.RecentOpenedFile;
            if (!string.IsNullOrEmpty(recentOpenedFile.FullPath) && File.Exists(recentOpenedFile.FullPath))
            {
                List<string> arguments = new List<string>() { recentOpenedFile.FullPath };
                SelectedRecentFileChanged?.Invoke(arguments, null);
            }
            else if (!File.Exists(recentOpenedFile.FullPath))
            {
                new frmMessageBox("File not found", "Can’t open the selected item because the file could not be found", MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
            }
        }

        private void pbClose_Click(object sender, EventArgs e)
        {
            ControlClosed?.Invoke(null, null);
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            using (var popup = new OpenFileDialog())
            {
                popup.Filter = "Supported Types | *.stl; *.apf; *.3mf";
                popup.Multiselect = false;
                if (popup.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(popup.FileName))
                {
                    if (File.Exists(popup.FileName))
                    {
                        var fileName = popup.FileName;
                        List<string> arguments = new List<string>() { fileName };
                        OpenExistsProject?.Invoke(arguments, null);
                    }
                }
            }
        }

        private void pbPlusSign_Click(object sender, EventArgs e)
        {
            OpenNewProject?.Invoke(null, null);
        }
              
        private void pbPlusSign_Resize(object sender, EventArgs e)
        {
            base.OnResize(e);
            using (var gp = new GraphicsPath())
            {
                gp.AddEllipse(new Rectangle(0, 0, this.Width - 2, this.Height - 2));
                this.Region = new Region(gp);
            }
        }

    }
}
