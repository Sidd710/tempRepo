using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Controls.NewGui;
using Atum.Studio.Core.Managers;
using System.IO;

namespace Atum.Studio.Controls.NewGui
{
    public partial class frmUSBDriveSelector : NewGUIFormBase
    {
        internal DriveInfo SelectedDrive { get; set; }

        public frmUSBDriveSelector()
        {
            InitializeComponent();

            if (FontManager.Loaded)
            {
                this.btnSelect.Font = this.btnSelect.Font = FontManager.Montserrat14Regular;
            }

            RefreshDrives();
        }

        private void RefreshDrives()
        {
            this.plContentItems.Controls.Clear();

            var top = 0;
            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                try
                {
                    if (drive.DriveType == System.IO.DriveType.Removable)
                    {
                        var usbDriveSummary = new USBDriveSummary(drive);
                        usbDriveSummary.USBDriveSelected += UsbDriveSummary_USBDriveSelected;
                        usbDriveSummary.Top = top;
                        usbDriveSummary.Width = this.plContent.Width;

                        if (top == 0)
                        {
                            usbDriveSummary.Selected= true;
                        }

                        this.plContentItems.Controls.Add(usbDriveSummary);
                        top += usbDriveSummary.Height;
                    }
                }
                catch
                {
                }
            }

        }



        private void UsbDriveSummary_USBDriveSelected(object sender, EventArgs e)
        {
            var selectedSummary = sender as USBDriveSummary;
            SelectedDrive = selectedSummary.Drive;
            foreach (USBDriveSummary usbDrive in this.plContentItems.Controls)
            {
                if (usbDrive != selectedSummary)
                {
                    usbDrive.ResetSelectedColor();
                }
            }
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDrives();
        }
    }
}
