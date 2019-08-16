using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public partial class NewGUIFormBase : Form
    {
        public NewGUIFormBase()
        {
            InitializeComponent();

            this.plContentSplitter.BackColor = this.lblHeader.BackColor = this.plHeaderTitle.BackColor = BrandingManager.Form_Header_BackgroundColor;
            this.btnClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_white, this.btnClose.Size);
            this.Icon = BrandingManager.MainForm_Icon;
        }

        private void NewGUIFormBase_Load(object sender, EventArgs e)
        {
            if (FontManager.Loaded)
            {
                this.lblHeader.Font = FontManager.Montserrat18Regular;

                this.lblHeader.Text = this.Text;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool _mouseIsDown;
        private Point _mouseIsDownLoc = new Point();

        private void lblHeader_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                if (this._mouseIsDown == false)
                {
                    this._mouseIsDown = true;
                    this._mouseIsDownLoc = new Point(e.X, e.Y);
                }

                this.Location = new Point(this.Location.X + e.X - this._mouseIsDownLoc.X, this.Location.Y + e.Y - this._mouseIsDownLoc.Y);
            }
        }

        private void lblHeader_MouseUp(object sender, MouseEventArgs e)
        {
            this._mouseIsDown = false;
        }

        private void NewGUIFormBase_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
        }

     

    }
}
