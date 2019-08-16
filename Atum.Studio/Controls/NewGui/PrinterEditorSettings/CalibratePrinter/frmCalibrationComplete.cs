using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public partial class frmCalibrationComplete : Form
    {
        public frmCalibratePrinter FrmCalibratePrinter { get; set; }
        public frmCalibrationComplete(frmCalibratePrinter frmCalibratePrinter)
        {
            FrmCalibratePrinter = frmCalibratePrinter;
            InitializeComponent();

            this.pbTickMark.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.tick, pbTickMark.Size);
            this.pbClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_white, pbClose.Size);

            this.lblHeader.Font = FontManager.Montserrat18Regular;
            this.Text = this.lblHeader.Text = this.Text.Replace("{{PrinterName}}", FrmCalibratePrinter.AtumPrinter.DisplayName);
            this.Icon = BrandingManager.MainForm_Icon;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnCloseCalibration_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
