using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.WifiWizard
{
    public partial class WifiSaveConfigurationToUSBTabPanel : WizardTabPanel
    {
        private bool _configurationSaved;
        internal Atum.DAL.Hardware.PrinterWiFiConfiguration WiFiConfiguration { get; set; }

        public WifiSaveConfigurationToUSBTabPanel()
        {
            InitializeComponent();
        }

        private void WifiSaveConfigurationToUSBTabPanel_Load(object sender, EventArgs e)
        {
            RefreshDriveLetters();
        }

        private void RefreshDriveLetters()
        {
            this.btnSaveConfiguration.Enabled = false;

            this.cbUSBDriveLetters.Items.Clear();

            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {
                try
                {
                    DAL.Managers.LoggingManager.WriteToLog("DriveType", drive.Name, drive.DriveType.ToString() + " - " + drive.DriveFormat);
                    if (drive.DriveType == System.IO.DriveType.Removable || (DAL.OS.OSProvider.IsOSX && drive.Name.StartsWith("/Volumes") && drive.DriveType == System.IO.DriveType.Fixed))
                    {
                        this.cbUSBDriveLetters.Items.Add(new Core.Drive.USBDrive(drive));
                    }


                    if (this.cbUSBDriveLetters.Items.Count > 0)
                    {
                        this.cbUSBDriveLetters.SelectedIndex = 0;
                        this.btnSaveConfiguration.Enabled = true;

                    }
                }
                catch (Exception exc)
                {

                }


            }



            UpdateControls();
        }

        void UpdateControls()
        {

            if (this.cbUSBDriveLetters.Items.Count > 0)
            {
                var selectedDrive = (Core.Drive.USBDrive)this.cbUSBDriveLetters.SelectedItem;
                if (!selectedDrive.DriveFormat.StartsWith("FAT") && !selectedDrive.DriveFormat.ToLower().StartsWith("msdos"))
                {
                    System.Windows.Forms.MessageBox.Show("Selected drive format is not supported. Only FAT(32) is currently supported");
                    this.btnSaveConfiguration.Enabled = false;
                }
                else
                {
                    this.btnSaveConfiguration.Enabled = true;
                }
            }

            if (this._configurationSaved)
            {
                base.btnNext.Enabled = true;
            }
            else
            {
                base.btnNext.Enabled = false;
            }
        }

        private void btnRefreshUSBDisks_Click(object sender, EventArgs e)
        {
            RefreshDriveLetters();


        }

        private void btnSaveConfiguration_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedDrive = (Core.Drive.USBDrive)this.cbUSBDriveLetters.SelectedItem;
                var configurationFile = new Atum.DAL.Hardware.PrinterWiFiConfiguration();
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Atum.DAL.Hardware.PrinterWiFiConfiguration));
                var updatePath = System.IO.Path.Combine(selectedDrive.DriveLetter, "Atum.Update");
                if (!System.IO.Directory.Exists(updatePath)) System.IO.Directory.CreateDirectory(updatePath);
                using (var streamWriter = new System.IO.StreamWriter(System.IO.Path.Combine(updatePath,"Atum.WiFiConfiguration.xml"), false))
                {
                    serializer.Serialize(streamWriter, WiFiConfiguration);
                }

                this._configurationSaved = true;
            }
            catch (Exception exc)
            {
                System.Windows.Forms.MessageBox.Show(exc.Message);
            }

            UpdateControls();
        }

        private void cbUSBDriveLetters_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }

    }
}
