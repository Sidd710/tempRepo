using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using OpenTK;
using System.IO;
using System.Drawing.Drawing2D;

namespace Atum.Studio.Controls.NewGui.SplashControl.LicensedControl
{
    public partial class LicensedProgramControl : UserControl
    {
        private static SplashFrm SplashForm = null;

        internal event EventHandler ControlClosed;

        internal event EventHandler OpenExistsProject;
        internal event EventHandler OpenNewProject;

        public LicensedProgramControl()
        {
            InitializeComponent();

            this.pbTick.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.tick, pbTick.Size);
            this.pbClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_white, pbClose.Size);

            if (FontManager.Loaded)
            {
                this.label1.Font = FontManager.Montserrat14Regular;
            }

            this.pbPlusSign.BackColor = BrandingManager.Button_HighlightColor;
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
