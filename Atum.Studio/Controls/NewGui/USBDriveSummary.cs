using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui
{
    public partial class USBDriveSummary : UserControl
    {
        internal event EventHandler USBDriveSelected;

        private bool _selected;

        internal DriveInfo Drive { get; set; }
        internal bool Selected { get {
                return this._selected;
            }
            set
            {
                if (value != this._selected)
                {
                    this._selected = value;

                    if (this._selected)
                    {
                        USBDriveSummary_Click(null, null);
                    }
                }
            }

        }

        public USBDriveSummary()
        {
            InitializeComponent();
        }

        public USBDriveSummary(DriveInfo usbDrive)
        {
            InitializeComponent();

            Drive = usbDrive;

            if (FontManager.Loaded)
            {
                this.lblName.Font = FontManager.Montserrat14Regular;
            }
            this.lblName.Text = string.Format("{0} ({1})", Drive.VolumeLabel, Drive.Name);
        }

        internal void USBDriveSummary_Click(object sender, EventArgs e)
        {
            USBDriveSelected?.Invoke(this, null);
            this.Selected = true;

            this.BackColor = Color.Red;
            this.ForeColor = Color.White;
        }

        internal void ResetSelectedColor()
        {
            this.BackColor = Color.White;
            this.ForeColor = Color.Black;

            this.Selected = false;
        }


        private void lblName_Click(object sender, EventArgs e)
        {
            this.USBDriveSummary_Click(null, null);
        }
    }
}
