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
using Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl.RecentFiles;

namespace Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl
{
    public partial class RecentOpenFileControl : UserControl
    {
        public event EventHandler onClick;
        public RecentOpenedFile RecentOpenedFile { get; set; }
        public RecentOpenFileControl(RecentOpenedFile recentOpenedFile)
        {
            RecentOpenedFile = recentOpenedFile;
            InitializeComponent();
            InitControl();
        }

        private void InitControl()
        {
            if (FontManager.Loaded)
            {
                this.lblFileName.Font = FontManager.Montserrat14Regular;
            }
            this.lblFileName.Text = RecentOpenedFile.FileName;

            ttFileName.SetToolTip(lblFileName, RecentOpenedFile.FullPath);
        }

       
        private void RecentOpenFileControl_Load(object sender, EventArgs e)
        {

        }

        private void RecentOpenFileControl_MouseLeave(object sender, EventArgs e)
        {

        }

        private void RecentOpenFileControl_MouseEnter(object sender, EventArgs e)
        {

        }

        private void plRecentOpenFile_MouseEnter(object sender, EventArgs e)
        {
            OnEnterControl();
        }

        private void OnEnterControl()
        {
            this.plRecentOpenFile.BackColor =BrandingManager.Menu_Item_HighlightColor;
        }

        private void plRecentOpenFile_MouseLeave(object sender, EventArgs e)
        {
            OnLeaveControl();
        }

        private void OnLeaveControl()
        {
            this.plRecentOpenFile.BackColor = this.Parent.BackColor;
        }

        private void plRecentOpenFile_Click(object sender, EventArgs e)
        {
            OnClick();
        }
        private void OnClick()
        {
            this.onClick?.Invoke(this, null);
        }

        private void lblFileName_MouseEnter(object sender, EventArgs e)
        {
            OnEnterControl();
        }

        private void lblFileName_MouseLeave(object sender, EventArgs e)
        {
            OnLeaveControl();
        }

        private void lblFileName_Click_1(object sender, EventArgs e)
        {
            OnClick();
        }
    }
}
