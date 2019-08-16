using System;
using System.Collections.Generic;
using Atum.Studio.Core.Managers;
using Atum.DAL.Materials;
using System.Diagnostics;
using System.Windows.Forms;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionMaterialTabPanel : WizardTabPanel
    {
        public object SelectedDrive
        {
            get
            {
                return cbUSBDriveLetters.SelectedItem;
            }

        }

        private MaterialsCatalog _materialsCatalog;

        public Material SelectedMaterial
        {
            get
            {
                Material selectedMaterial = new Material();
                if (this.cbMaterialProduct.InvokeRequired)
                {
                    this.cbMaterialProduct.Invoke(new MethodInvoker(delegate
                    {
                        if (this.cbMaterialProduct.SelectedItem != null)
                        {
                            selectedMaterial = this.cbMaterialProduct.SelectedItem as Material;
                        }
                        else
                        {
                            selectedMaterial = null;
                        }
                    }));
                }
                else
                {
                    if (this.cbMaterialProduct.SelectedItem != null)
                    {
                        selectedMaterial =  this.cbMaterialProduct.SelectedItem as Material;
                    }
                    else
                    {
                        return null;
                    }
                }

                return selectedMaterial;
            }
        }

        public MaterialsCatalog MaterialCatalog { get
            {
                return this._materialsCatalog;
            }
            set
            {
                this._materialsCatalog = value;
                this.cbMaterialManufacturer.DataSource = this.MaterialCatalog;
            }
        }

        public PrinterConnectionMaterialTabPanel()
        {
            InitializeComponent();

                //dpi correction
                this.cbMaterialManufacturer.Left = this.cbMaterialProduct.Left = this.cbUSBDriveLetters.Left = this.lblManufacturer.Left + this.lblManufacturer.Width;
            this.btnRefreshUSBDisks.Left = this.cbUSBDriveLetters.Left + this.cbUSBDriveLetters.Width + 3;
            this.btnRefreshUSBDisks.Width = this.btnRefreshUSBDisks.Height = this.cbUSBDriveLetters.Height;

        }

        private void cbMaterialManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedSupplier = this.cbMaterialManufacturer.SelectedItem as MaterialsBySupplier;

                this.cbMaterialProduct.DataSource = selectedSupplier.Materials;

                var defaultMaterial = MaterialManager.DefaultMaterial;
                if (cbMaterialProduct != null)
                {
                    this.cbMaterialProduct.SelectedItem = defaultMaterial;

                    if (this.cbMaterialProduct.SelectedItem == null && this.cbMaterialProduct.Items.Count > 0)
                    {
                        this.cbMaterialProduct.SelectedIndex = 0;
                    }
                }
                else
                {
                    this.cbMaterialProduct_SelectedIndexChanged(null, null);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }
        }

        private void cbMaterialProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            //this.SelectedMaterial = (Material)cbMaterialProduct.SelectedItem
            //MaterialManager.CurrentMaterial = (Material)cbMaterialProduct.SelectedItem;
        }

        private void RefreshDriveLetters()
        {
            var linuxSkipDriveLetters = new List<string>() { "/", "/usr" };
            this.cbUSBDriveLetters.Items.Clear();
            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {

                try
                {
                    DAL.Managers.LoggingManager.WriteToLog("DriveType", drive.Name, drive.DriveType.ToString());
                    if (drive.DriveType == System.IO.DriveType.Removable
                        || ((DAL.OS.OSProvider.IsLinux && drive.Name.StartsWith("/media") && drive.DriveType == System.IO.DriveType.Fixed) && !linuxSkipDriveLetters.Contains(drive.Name))
                        || (DAL.OS.OSProvider.IsOSX && drive.Name.StartsWith("/Volumes") && drive.DriveType == System.IO.DriveType.Fixed))
                    {
                        this.cbUSBDriveLetters.Items.Add(new Core.Drive.USBDrive(drive));
                    }
                }
                catch
                {
                }
            }

            if (this.cbUSBDriveLetters.Items.Count > 0)
            {
                this.cbUSBDriveLetters.SelectedIndex = 0;
                this.ButtonNextEnabled = true;
            }
            else
            {
                this.ButtonNextEnabled = false;
            }

            //UpdateControls();
        }

        private void btnRefreshUSBDisks_Click(object sender, EventArgs e)
        {
            RefreshDriveLetters();
        }

        private void PrinterConnectionMaterialTabPanel_Load(object sender, EventArgs e)
        {
           // this.ButtonNextEnabled = false;
            RefreshDriveLetters();
        }
    }

}
