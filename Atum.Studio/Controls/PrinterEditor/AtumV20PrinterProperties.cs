using Atum.DAL.ApplicationSettings;
using Atum.DAL.Compression.Zip;
using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Controls.DataGridView;
using Atum.Studio.Core.Drive;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelCorrections.LensWarpCorrection;
using Atum.Studio.Core.Models.Defaults;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV20PrinterProperties : UserControl
    {
        private AtumV20Printer _dataSource;
        private bool _inWizard;

        public bool inWizard
        {
            get
            {
                return _inWizard;
            }
            set
            {
                _inWizard = value;
                if (_inWizard)
                {
                    rbtn50Micron.Enabled = true;
                    rbtn75Micron.Enabled = true;
                    rbtn100Micron.Enabled = true;

                    this.Properties.Controls.Remove(this.tableLayoutPanel1);
                    this.tableLayoutPanel1.Dock = DockStyle.None;
                    this.tableLayoutPanel1.Top = this.lblHeader.Height + 10;
                    this.tableLayoutPanel1.Left += 10;
                    this.tbPrinterSettings.Visible = false;
                    this.Controls.Add(this.tableLayoutPanel1);
                    // ((Control)this.tabControl1.TabPages[1]).Enabled = false;
                }
                else
                {
                    rbtn50Micron.Enabled = false;
                    rbtn75Micron.Enabled = false;
                    rbtn100Micron.Enabled = false;
                    // ((Control)this.tabControl1.TabPages[1]).Enabled = true;
                }
            }
        }

        public AtumV20Printer DataSource
        {
            get
            {
                if (!this.DesignMode) this.UpdatePrinterSettings();
                return this._dataSource;
            }
            set
            {
                this._dataSource = value;

                if (this._dataSource != null)
                {
                    if (this._dataSource.Properties != null)
                    {
                        this.txtDisplayName.Text = this._dataSource.DisplayName;
                        this.txtDescription.Text = this._dataSource.Description;
                    }

                    this.atumPrinterCalibration1.AdvancedMode = false;
                    this.atumPrinterCalibration1.DataSource = this._dataSource;

                    switch (this._dataSource.PrinterXYResolution)
                    {
                        case AtumPrinter.PrinterXYResolutionType.Micron50:
                            this.rbtn50Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron75:
                            this.rbtn75Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron100:
                            this.rbtn100Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Unknown:
                            this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron50); // same as selected printer;
                            break;
                    }

                    if (this._dataSource.LensWarpingCorrection != null && this._dataSource.LensWarpingCorrection.Enabled)
                    {
                        this.ShowAdvancedCalibration();
                    }
                }
            }
        }


        public AtumV20PrinterProperties()
        {
            InitializeComponent();
            this.tbPrinterSettings.TabPages.Remove(this.AdvancedCalibration);

            //dpi correction
            this.txtDescription.Left = this.txtDisplayName.Left = this.label2.Left + this.label2.Width;

            for (var columnIndex = this.dgLensWarpCorrectionValues.ColumnCount - 1; columnIndex > 2; columnIndex--)
            {
                this.dgLensWarpCorrectionValues.Columns.RemoveAt(columnIndex);
            }

        }


        private void UpdatePrinterSettings()
        {
            this._dataSource.DisplayName = this.txtDisplayName.Text;
            this._dataSource.Description = this.txtDescription.Text;
            var selectedPrinter = this.atumPrinterCalibration1.DataSource;
            if (selectedPrinter != null)
            {
                if (selectedPrinter.IsDirty)
                {
                    this._dataSource.TrapeziumCorrectionSideA = selectedPrinter.TrapeziumCorrectionSideA;
                    this._dataSource.TrapeziumCorrectionSideB = selectedPrinter.TrapeziumCorrectionSideB;
                    this._dataSource.TrapeziumCorrectionSideC = selectedPrinter.TrapeziumCorrectionSideC;
                    this._dataSource.TrapeziumCorrectionSideD = selectedPrinter.TrapeziumCorrectionSideD;
                    this._dataSource.TrapeziumCorrectionSideE = selectedPrinter.TrapeziumCorrectionSideE;
                    this._dataSource.TrapeziumCorrectionSideF = selectedPrinter.TrapeziumCorrectionSideF;

                    this._dataSource.TrapeziumCorrectionInputA = selectedPrinter.TrapeziumCorrectionInputA;
                    this._dataSource.TrapeziumCorrectionInputB = selectedPrinter.TrapeziumCorrectionInputB;
                    this._dataSource.TrapeziumCorrectionInputC = selectedPrinter.TrapeziumCorrectionInputC;
                    this._dataSource.TrapeziumCorrectionInputD = selectedPrinter.TrapeziumCorrectionInputD;

                    LensWarpCorrectionItems.ToPrinterSettings((this.lensWarpCorrectionItemsBindingSource.DataSource as LensWarpCorrectionItems), this._dataSource);
                }
            }
        }
        private void rbtnMicron_CheckedChanged(object sender, System.EventArgs e)
        {
            var radioButtonSender = sender as RadioButton;
            if (radioButtonSender.Checked)
            {
                if (this.inWizard && this._dataSource != null)
                {
                    if (this.rbtn50Micron.Checked)
                    {
                        this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron50);
                    }
                    else if (this.rbtn75Micron.Checked)
                    {
                        this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron75);
                    }
                    else if (this.rbtn100Micron.Checked)
                    {
                        this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron100);
                    }
                    this.atumPrinterCalibration1.DataSource = this._dataSource;
                }
            }
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_inWizard && e.TabPage == Calibration)
            {
                e.Cancel = true;
            }
        }

        private void dgLensWarpCorrectionValues_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (this.dgLensWarpCorrectionValues.CurrentCell != null)
                {
                    if (this.dgLensWarpCorrectionValues.CurrentCell.ColumnIndex == 1)
                    {
                        //select next row
                        DataGridViewCell cell = this.dgLensWarpCorrectionValues.Rows[this.dgLensWarpCorrectionValues.CurrentCell.RowIndex].Cells[2];
                        this.dgLensWarpCorrectionValues.CurrentCell = cell;
                        this.dgLensWarpCorrectionValues.BeginEdit(true);
                    }
                    else if (this.dgLensWarpCorrectionValues.CurrentCell.ColumnIndex == 2 && this.dgLensWarpCorrectionValues.CurrentCell.RowIndex < this.dgLensWarpCorrectionValues.RowCount - 1)
                    {
                        //select next row
                        DataGridViewCell cell = this.dgLensWarpCorrectionValues.Rows[this.dgLensWarpCorrectionValues.CurrentCell.RowIndex + 1].Cells[1];
                        this.dgLensWarpCorrectionValues.CurrentCell = cell;
                        this.dgLensWarpCorrectionValues.BeginEdit(true);
                    }
                }
            }
        }

        private void dgLensWarpCorrectionValues_SelectionChanged(object sender, System.EventArgs e)
        {
            if (this.dgLensWarpCorrectionValues.SelectedCells.Count > 0)
            {
                DataGridViewCell cell = this.dgLensWarpCorrectionValues.SelectedCells[0];
                if (cell.EditType == typeof(DataGridViewNumericUpDownCell.NumericUpDownEditingControl))
                {
                    this.dgLensWarpCorrectionValues.CurrentCell = cell;
                    this.dgLensWarpCorrectionValues.BeginEdit(true);
                }
            }
        }

        #region Advanced Calibration
        private void cbMaterialManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                var selectedSupplier = this.cbAdvancedCalibrationMaterialSupplier.SelectedItem as MaterialsBySupplier;

                this.cbAdvancedCalibrationMaterialProduct.DataSource = selectedSupplier.Materials;

                var defaultMaterial = MaterialManager.DefaultMaterial;
                if (this.cbAdvancedCalibrationMaterialProduct != null)
                {
                    this.cbAdvancedCalibrationMaterialProduct.SelectedItem = defaultMaterial;

                    if (this.cbAdvancedCalibrationMaterialProduct.SelectedItem == null && this.cbAdvancedCalibrationMaterialProduct.Items.Count > 0)
                    {
                        this.cbAdvancedCalibrationMaterialProduct.SelectedIndex = 0;
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
            if (this.cbAdvancedCalibrationMaterialProduct != null)
            {
                this.plAdvancedCalibrationDriveLetters.Enabled = true;
                this.btnSaveAdvancedCalibration.Enabled = true;

                RefreshDriveLetters();
            }
            else
            {
                this.plAdvancedCalibrationDriveLetters.Enabled = true;
                this.btnSaveAdvancedCalibration.Enabled = true;
            }
        }

        private void RefreshDriveLetters()
        {
            var linuxSkipDriveLetters = new List<string>() { "/", "/usr" };
            this.cbAdvancedCalibrationDriveletters.Items.Clear();
            foreach (var drive in System.IO.DriveInfo.GetDrives())
            {

                try
                {
                    DAL.Managers.LoggingManager.WriteToLog("DriveType", drive.Name, drive.DriveType.ToString());
                    if (drive.DriveType == System.IO.DriveType.Removable
                        || ((DAL.OS.OSProvider.IsLinux && drive.Name.StartsWith("/media") && drive.DriveType == System.IO.DriveType.Fixed) && !linuxSkipDriveLetters.Contains(drive.Name))
                        || (DAL.OS.OSProvider.IsOSX && drive.Name.StartsWith("/Volumes") && drive.DriveType == System.IO.DriveType.Fixed))
                    {
                        this.cbAdvancedCalibrationDriveletters.Items.Add(new Core.Drive.USBDrive(drive));
                    }
                }
                catch
                {
                }
            }

            if (this.cbAdvancedCalibrationDriveletters.Items.Count > 0)
            {
                this.cbAdvancedCalibrationDriveletters.SelectedIndex = 0;
                this.btnSaveAdvancedCalibration.Enabled = true;
            }
            else
            {
                this.btnSaveAdvancedCalibration.Enabled = false;
            }

            //UpdateControls();
        }

        private void btnRefreshUSBDisks_Click(object sender, EventArgs e)
        {
            RefreshDriveLetters();
        }

        #endregion

        private void btnSaveAdvancedCalibration_Click(object sender, EventArgs e)
        {
            var printJob = new DAL.Print.PrintJob();
            printJob.Name = "AdvancedCalibration";
            printJob.SelectedPrinter = new AtumV15Printer()
            {
                CorrectionFactorX = this._dataSource.CorrectionFactorX,
                CorrectionFactorY = this._dataSource.CorrectionFactorY,
                Default = this._dataSource.Default,
                Description = this._dataSource.Description,
                DisplayName = this._dataSource.DisplayName,
                DisplayText = this._dataSource.DisplayText,
                PrinterXYResolution = this._dataSource.PrinterXYResolution,
                Projectors = this._dataSource.Projectors,
                Properties = this._dataSource.Properties,
                Selected = this._dataSource.Selected,
                TrapeziumCorrectionInputA = this._dataSource.TrapeziumCorrectionInputA,
                TrapeziumCorrectionInputB = this._dataSource.TrapeziumCorrectionInputB,
                TrapeziumCorrectionInputC = this._dataSource.TrapeziumCorrectionInputC,
                TrapeziumCorrectionInputD = this._dataSource.TrapeziumCorrectionInputD,
                TrapeziumCorrectionInputE = this._dataSource.TrapeziumCorrectionInputE,
                TrapeziumCorrectionInputF = this._dataSource.TrapeziumCorrectionInputF,
                TrapeziumCorrectionSideA = this._dataSource.TrapeziumCorrectionSideA,
                TrapeziumCorrectionSideB = this._dataSource.TrapeziumCorrectionSideB,
                TrapeziumCorrectionSideC = this._dataSource.TrapeziumCorrectionSideC,
                TrapeziumCorrectionSideD = this._dataSource.TrapeziumCorrectionSideD,
                TrapeziumCorrectionSideE = this._dataSource.TrapeziumCorrectionSideE,
                TrapeziumCorrectionSideF = this._dataSource.TrapeziumCorrectionSideF,
            };
            printJob.Material = this.cbAdvancedCalibrationMaterialProduct.SelectedItem as Material;
            printJob.SlicesPath = Path.Combine((this.cbAdvancedCalibrationDriveletters.SelectedItem as USBDrive).DriveLetter, "AdvancedCalibration");
            printJob.Option_TurnProjectorOn = true;
            printJob.Option_TurnProjectorOff = true;

            var pathPrinterJobXml = Path.Combine(printJob.SlicesPath, "printjob.apj");
            if (!Directory.Exists(printJob.SlicesPath)) Directory.CreateDirectory(printJob.SlicesPath);

            //serialize printjob.xml
            var serializer = new XmlSerializer(typeof(DAL.Print.PrintJob));
            using (var streamWriter = new StreamWriter(pathPrinterJobXml, false))
            {
                serializer.Serialize(streamWriter, printJob);
            }
            
            //checksum file
            var checksumFilePath = Path.Combine(printJob.SlicesPath, "checksum.crc");
            if (File.Exists(checksumFilePath)) { File.Delete(checksumFilePath); }
            using (FileStream checksumFile = new FileStream(checksumFilePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None))
            {

            }

            //slices
            FileStream tempFileStream = new FileStream(Path.Combine(printJob.SlicesPath, "slices.zip"), FileMode.Create, FileAccess.ReadWrite, FileShare.None);
            using (var zipStream = new ZipOutputStream(tempFileStream))
            {

                for (var sliceIndex = 0; sliceIndex < 99; sliceIndex++)
                {
                    using (var memStream = new MemoryStream())
                    {
                        var sliceBitmap = LensWarpCorrectionModel.CreateMeasurementGrid(this._dataSource);
                        if (sliceIndex < 4)
                        {
                            sliceBitmap = new Bitmap(this._dataSource.ProjectorResolutionX, this._dataSource.ProjectorResolutionY);
                            using (var g = Graphics.FromImage(sliceBitmap))
                            {
                                g.Clear(Color.White);
                            }
                        }

                        sliceBitmap.Save(memStream, ImageFormat.Png);
                        sliceBitmap.Dispose();

                        memStream.Position = 0;

                        lock (zipStream)
                        {
                            var entry = new ZipEntry(sliceIndex + ".png");
                            zipStream.PutNextEntry(entry);

                            try
                            {
                                byte[] transferBuffer = new byte[1024];
                                int bytesRead;
                                do
                                {
                                    bytesRead = memStream.Read(transferBuffer, 0, transferBuffer.Length);
                                    zipStream.Write(transferBuffer, 0, bytesRead);
                                }
                                while (bytesRead > 0);
                            }
                            finally
                            {
                                memStream.Close();
                            }

                            zipStream.CloseEntry();
                        }
                    }
                }

                zipStream.Close();
            }

            MessageBox.Show("Printjob saved on USB Flash device as 'AdvancedCalibration'." + Environment.NewLine + Environment.NewLine + "Please start the printjob on printer.", "Printjob saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void lblSwitchMode_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.ShowAdvancedCalibration();
        }

        private void ShowAdvancedCalibration()
        {
            this.tbPrinterSettings.TabPages.Remove(this.Calibration);

            if (!this.tbPrinterSettings.TabPages.Contains(this.AdvancedCalibration))
            {
                this.tbPrinterSettings.TabPages.Add(this.AdvancedCalibration);
            }

            this.tbPrinterSettings.SelectedTab = this.AdvancedCalibration;
            this.lensWarpCorrectionItemsBindingSource.DataSource = LensWarpCorrectionItems.FromPrinterSettings(this._dataSource);

            this._dataSource.LensWarpingCorrection.Enabled = true;

            var materialCatalog = new MaterialsCatalog();
            foreach (var supplier in MaterialManager.Catalog)
            {
                var materialSupplier = new MaterialsBySupplier();
                //filter on printer type

                //un filtered
                foreach (var material in supplier.Materials.Where(m => string.IsNullOrEmpty(m.PrinterHardwareType)))
                {
                    materialSupplier.Materials.Add(material);
                }

                if (materialSupplier.Materials.Count > 0)
                {
                    materialSupplier.Supplier = supplier.Supplier;
                    materialCatalog.Add(materialSupplier);
                }
            }

            this.cbAdvancedCalibrationMaterialSupplier.DataSource = materialCatalog;

            cbMaterialManufacturer_SelectedIndexChanged(null, null);
        }

        private void ShowBasicCalibration()
        {
            this.tbPrinterSettings.TabPages.Remove(this.AdvancedCalibration);

            if (!this.tbPrinterSettings.TabPages.Contains(this.Calibration))
            {
                this.tbPrinterSettings.TabPages.Add(this.Calibration);
            }

            this.tbPrinterSettings.SelectedTab = this.Calibration;
            this._dataSource.LensWarpingCorrection.Enabled = false;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            this.ShowBasicCalibration();
        }

        private void dgLensWarpCorrectionValues_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (this._dataSource != null)
            {
                this._dataSource.IsDirty = true;
            }
        }
    }
}