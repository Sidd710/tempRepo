using System;
using System.ComponentModel;
using System.Windows.Forms;
using Atum.DAL.Hardware;

using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models.Defaults;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Engines;
using System.IO;
using System.Linq;
using System.Diagnostics;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionWizard : Form
    {
        private WaitWindowManager _waitWindowManager;
        private AtumPrinter _selectedPrinter;
        //private AtumPrinter SavePrinter;
        private int currenttabindex;
        private Core.Drive.USBDrive selectedDrive;

        public PrinterConnectionWizard()
        {
            InitializeComponent();
        }

        internal AtumPrinter SelectedPrinter
        {
            get
            {
                return this._selectedPrinter;
            }
        }

        private void PrinterConnectionWizard_Load(object sender, EventArgs e)
        {
            //default settings
            this.tbWizard.Top -= 24;
            this.tbWizard.Height += 25;
            this.tbWizard.Width += 5;
            //   this.printerConnectionDiscoveryTabPanel1.ButtonBackVisible = true;
            this.printerConnectionFinishedTabPanel1.ButtonBackVisible = true;
            this.printerConnectionPropertiesTabPanel1.ButtonBackVisible = true;
            this.printerConnectionWelcomeTabPanel1.HideFooter = false;
            this.printerConnectionMaterialTabPanel1.HideFooter = false;
            this.printerConnectionMaterialTabPanel1.ButtonBackVisible = true;
            this.printerConnectionAdjustTabPanel1.HideFooter = false;
            this.printerConnectionAdjustTabPanel1.ButtonBackVisible = true;
            // this.printerConnectionDiscoveryTabPanel1.HideFooter = false;
            this.printerConnectionPropertiesTabPanel1.HideFooter = false;
            this.printerConnectionFinishedTabPanel1.HideFooter = false;

            RenderEngine.RenderToCalibrationjobCompleted += GenerateCalibrationPrintJobAsync_Completed;
        }

        private void DefaultNextClick(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
        }

        private void DefaultBackClick(object sender, EventArgs e)
        {
            if (this.tbWizard.SelectedTab == this.tbAdjust)
            {
                this._selectedPrinter = this.printerConnectionAdjustTabPanel1.DataSource;
                //this.printerConnectionPropertiesTabPanel1.DataSource = this.printerConnectionAdjustTabPanel1.DataSource;
            }

            this.tbWizard.SelectedIndex--;
        }

        private void printerConnectionFinishedTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }


        private void tbWizard_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (e.TabPage == tbPrinterProperties)
            {
                if (this._selectedPrinter == null)
                {
                    this._selectedPrinter = this.printerConnectionWelcomeTabPanel1.ManualUSBPrinterType;

                    this._selectedPrinter.CreateProperties();
                    this._selectedPrinter.CreateProjectors();
                }

                this.printerConnectionPropertiesTabPanel1.Dock = DockStyle.Fill;
                this.printerConnectionPropertiesTabPanel1.DataSource = this._selectedPrinter;
                
            }
            else if (e.TabPage == tbCalibration)
            {
                this.printerConnectionAdjustTabPanel1.DataSource = this._selectedPrinter;
                this._selectedPrinter.DisplayName = this.printerConnectionPropertiesTabPanel1.DataSource.DisplayName;
                this._selectedPrinter.Description = this.printerConnectionPropertiesTabPanel1.DataSource.Description;

                if (MaterialManager.Catalog != null)
                {
                    var materialCatalog = new DAL.Materials.MaterialsCatalog();
                    foreach (var supplier in MaterialManager.Catalog)
                    {
                        var materialSupplier = new DAL.Materials.MaterialsBySupplier();
                        var selectedPrinterResolution = this._selectedPrinter.PrinterXYResolutionAsInt;
                        //filter on printer type
                        if (!(this._selectedPrinter is DAL.Hardware.AtumDLPStation5) && !(this._selectedPrinter is DAL.Hardware.LoctiteV10))
                        {
                            //un filtered
                            foreach (var material in supplier.Materials.Where(m => string.IsNullOrEmpty(m.PrinterHardwareType)))
                            {
                                materialSupplier.Materials.Add(material);
                            }
                        }
                        else
                        {
                            //filter on hardwaretype and resolution
                            var amountOfMaterialBySelectedPrinterTypeAndResolution = supplier.Materials.Count(m => m.XYResolution == selectedPrinterResolution && m.PrinterHardwareType == this._selectedPrinter.PrinterHardwareType);

                            if (amountOfMaterialBySelectedPrinterTypeAndResolution > 0)
                            {
                                foreach (var material in supplier.Materials.Where(m => m.XYResolution == selectedPrinterResolution && m.PrinterHardwareType == this._selectedPrinter.PrinterHardwareType))
                                {
                                    materialSupplier.Materials.Add(material);
                                }
                            }
                        }

                        if (materialSupplier.Materials.Count > 0)
                        {
                            materialSupplier.Supplier = supplier.Supplier;
                            materialCatalog.Add(materialSupplier);
                        }
                    }

                    this.printerConnectionMaterialTabPanel1.MaterialCatalog = materialCatalog;

                }
                //this.printerConnectionAdjustTabPanel1.DataSource = this._selectedPrinter;
                this.printerConnectionFinishedTabPanel1.printername = this._selectedPrinter.DisplayName;
            }
            else if (e.TabPage == tbAdjust)
            {
                // From calibration to adjust
                if (printerConnectionMaterialTabPanel1.SelectedDrive == null)
                {
                    e.Cancel = true;
                    return;
                }
                else
                {
                    selectedDrive = (Core.Drive.USBDrive)printerConnectionMaterialTabPanel1.SelectedDrive;
                    if (!selectedDrive.DriveFormat.StartsWith("FAT") && !selectedDrive.DriveFormat.ToLower().StartsWith("msdos"))
                    {
                        MessageBox.Show("Selected drive format is not supported. Only FAT(32) is currently supported", "Disk format type error");
                        e.Cancel = true;
                        return;
                    }
                }

                //MaterialManager.CurrentMaterial = printerConnectionMaterialTabPanel1.SelectedMaterial;
                //MaterialManager.CurrentMaterial.IsDefault = true;
                RenderJob();

            }



            if ((e.TabPageIndex - currenttabindex) > 0 && (e.TabPageIndex - currenttabindex > 1))
            {
                e.Cancel = true;
            }
            else
            { currenttabindex = e.TabPageIndex; }
        }

        private void RenderJob()
        {
            this.Enabled = false;

            var t = new BackgroundWorker();

            _waitWindowManager = new WaitWindowManager();
            _waitWindowManager.Start(this, t);
            if (t != null) t.ReportProgress(0, new WaitWindowUserState(0.01d, "Loading calibration model"));
            Application.DoEvents();

            t.DoWork += new DoWorkEventHandler(RenderJobAsync);

            //load calibration model
            ObjectView.Objects3D.Clear();
            SceneView.UpdateGroundPane(); //ZIE ONDER

            //SavePrinter = PrinterManager.DefaultPrinter;
            this._selectedPrinter.Selected = true;
            PrinterManager.Append((AtumPrinter)(this._selectedPrinter));
            PrinterManager.SetDefaultPrinter((AtumPrinter)this._selectedPrinter);


            BasicCorrectionModel basicCorrection = new BasicCorrectionModel();
            basicCorrection.Open((string)null, false, printerConnectionMaterialTabPanel1.SelectedMaterial.ModelColor, ObjectView.NextObjectIndex, basicCorrection.Triangles);
            basicCorrection.UpdateBoundries();
            basicCorrection.UpdateSelectionboxText();
            if (ObjectView.BindingSupported)
                basicCorrection.BindModel();
            ObjectView.AddModel((object)basicCorrection);
            Application.DoEvents();
            // System.Threading.Thread.Sleep(250);

            //let op hier nog op null printer (initieel) checken

            t.RunWorkerAsync();


        }

        private void RenderJobAsync(object sender, DoWorkEventArgs e)
        {
            var t = sender as BackgroundWorker;

            if (t != null) t.ReportProgress(0, new WaitWindowUserState(0.01d, "Printjob initializing"));

            RenderEngine renderEngine = new RenderEngine();
            //var renderSliceInfo = new Core.Slices.RenderSliceInfo();

            try
            {

                //  renderSliceInfo.Material = MaterialManager.CurrentMaterial;
                RenderEngine.PrintJob = new DAL.Print.PrintJob();
                RenderEngine.PrintJob.Name = "Calibration";
                RenderEngine.PrintJob.Option_TurnProjectorOff = true;
                RenderEngine.PrintJob.Option_TurnProjectorOn = true;
                if (this._selectedPrinter is AtumV20Printer)
                {
                    RenderEngine.PrintJob.SelectedPrinter = new AtumV15Printer()
                    {

                        CorrectionFactorX = this._selectedPrinter.CorrectionFactorX,
                        CorrectionFactorY = this._selectedPrinter.CorrectionFactorY,
                        Default = this._selectedPrinter.Default,
                        Description = this._selectedPrinter.Description,
                        DisplayName = this._selectedPrinter.DisplayName,
                        DisplayText = this._selectedPrinter.DisplayText,
                        PrinterXYResolution = this._selectedPrinter.PrinterXYResolution,
                        Projectors = this._selectedPrinter.Projectors,
                        Properties = this._selectedPrinter.Properties,
                        Selected = this._selectedPrinter.Selected,
                        TrapeziumCorrectionInputA = this._selectedPrinter.TrapeziumCorrectionInputA,
                        TrapeziumCorrectionInputB = this._selectedPrinter.TrapeziumCorrectionInputB,
                        TrapeziumCorrectionInputC = this._selectedPrinter.TrapeziumCorrectionInputC,
                        TrapeziumCorrectionInputD = this._selectedPrinter.TrapeziumCorrectionInputD,
                        TrapeziumCorrectionInputE = this._selectedPrinter.TrapeziumCorrectionInputE,
                        TrapeziumCorrectionInputF = this._selectedPrinter.TrapeziumCorrectionInputF,
                        TrapeziumCorrectionSideA = this._selectedPrinter.TrapeziumCorrectionSideA,
                        TrapeziumCorrectionSideB = this._selectedPrinter.TrapeziumCorrectionSideB,
                        TrapeziumCorrectionSideC = this._selectedPrinter.TrapeziumCorrectionSideC,
                        TrapeziumCorrectionSideD = this._selectedPrinter.TrapeziumCorrectionSideD,
                        TrapeziumCorrectionSideE = this._selectedPrinter.TrapeziumCorrectionSideE,
                        TrapeziumCorrectionSideF = this._selectedPrinter.TrapeziumCorrectionSideF
                    };
                }
                else if (this._selectedPrinter is AtumDLPStation5 || this._selectedPrinter is LoctiteV10)
                {
                    RenderEngine.PrintJob.SelectedPrinter = new AtumV15Printer()
                    {

                        CorrectionFactorX = this._selectedPrinter.CorrectionFactorX,
                        CorrectionFactorY = this._selectedPrinter.CorrectionFactorY,
                        Default = this._selectedPrinter.Default,
                        Description = this._selectedPrinter.Description,
                        DisplayName = this._selectedPrinter.DisplayName,
                        DisplayText = this._selectedPrinter.DisplayText,
                        PrinterXYResolution = this._selectedPrinter.PrinterXYResolution,
                        Projectors = this._selectedPrinter.Projectors,
                        Properties = this._selectedPrinter.Properties,
                        Selected = this._selectedPrinter.Selected,
                        ProjectorResolutionX = 1920,
                        ProjectorResolutionY = 1080,
                        TrapeziumCorrectionInputA = this._selectedPrinter.TrapeziumCorrectionInputA,
                        TrapeziumCorrectionInputB = this._selectedPrinter.TrapeziumCorrectionInputB,
                        TrapeziumCorrectionInputC = this._selectedPrinter.TrapeziumCorrectionInputC,
                        TrapeziumCorrectionInputD = this._selectedPrinter.TrapeziumCorrectionInputD,
                        TrapeziumCorrectionInputE = this._selectedPrinter.TrapeziumCorrectionInputE,
                        TrapeziumCorrectionInputF = this._selectedPrinter.TrapeziumCorrectionInputF,
                        TrapeziumCorrectionSideA = this._selectedPrinter.TrapeziumCorrectionSideA,
                        TrapeziumCorrectionSideB = this._selectedPrinter.TrapeziumCorrectionSideB,
                        TrapeziumCorrectionSideC = this._selectedPrinter.TrapeziumCorrectionSideC,
                        TrapeziumCorrectionSideD = this._selectedPrinter.TrapeziumCorrectionSideD,
                        TrapeziumCorrectionSideE = this._selectedPrinter.TrapeziumCorrectionSideE,
                        TrapeziumCorrectionSideF = this._selectedPrinter.TrapeziumCorrectionSideF
                    };
                }
                else
                {
                    RenderEngine.PrintJob.SelectedPrinter = this._selectedPrinter;
                }
                RenderEngine.PrintJob.Option_TurnProjectorOn = RenderEngine.PrintJob.Option_TurnProjectorOff = true; //turn projectors on
                RenderEngine.PrintJob.Material = printerConnectionMaterialTabPanel1.SelectedMaterial;
                RenderEngine.PrintJob.CalibrationJob = true;

                RenderEngine.RenderAsync();

                t.ReportProgress(0, new WaitWindowUserState(0.01d, "Printjob initializing"));

                while (RenderEngine.TotalAmountSlices != RenderEngine.TotalProcessedSlices || !RenderEngine.PrintJob.PostRenderCompleted)
                {
                    var progress = (((decimal)RenderEngine.TotalProcessedSlices / (decimal)RenderEngine.TotalAmountSlices) * 100);
                    t.ReportProgress((int)progress, new WaitWindowUserState((int)progress, string.Format("Generating calibration printjob: {0:N2}%", progress)));
                    System.Threading.Thread.Sleep(250);
                }
            }
            catch (Exception exc)
            {
                Atum.DAL.Managers.LoggingManager.WriteToLog("GeneratePrintJobAsync", "Exc", exc);
                MessageBox.Show(exc.Message);
            }

            try
            {
                t.ReportProgress(0, new WaitWindowUserState(0.01d, "Saving printjob"));
                Application.DoEvents();

                var selectedDriveLetter = (this.selectedDrive).DriveLetter;
                var usbPrintjobPath = string.Format(@"{0}{1}", selectedDriveLetter, RenderEngine.PrintJob.Name);

                var printingTime = RenderEngine.PrintJob.PrintingTimeRemaining(0, RenderEngine.TotalAmountSlices);
                if (RenderEngine.PrintJob.UseProjectorOptions)
                {
                    printingTime = printingTime.Add(new TimeSpan(0, 0, RenderEngine.PrintJob.Option_TurnProjectorOn ? RenderEngine.PrintJob.SelectedPrinter.ProjectorTurnOnDelay : 0));
                }
                else
                {
                    RenderEngine.PrintJob.Option_TurnProjectorOff = RenderEngine.PrintJob.Option_TurnProjectorOff = false;
                }

                RenderEngine.PrintJob.JobTimeInSeconds = printingTime.TotalSeconds;

                var pathPrinterJobXml = Path.Combine(Path.GetTempPath(), "printjob.xml"); //serialize first to temp
                var serializer = new System.Xml.Serialization.XmlSerializer(typeof(DAL.Print.PrintJob));
                using (var streamWriter = new StreamWriter(pathPrinterJobXml, false))
                {
                    serializer.Serialize(streamWriter, RenderEngine.PrintJob);
                }

                var pathPrinterJobChecksumXml = Path.Combine(System.IO.Path.GetTempPath(), "checksum.crc");
                using (var checksumWriter = new StreamWriter(pathPrinterJobChecksumXml, false))
                {
                    checksumWriter.WriteLine(DateTime.Now.Ticks.ToString());
                }
                var osPathSeparator = "\\";
                if (!DAL.OS.OSProvider.IsWindows) { osPathSeparator = "/"; }
                if (!Directory.Exists(usbPrintjobPath))
                    Directory.CreateDirectory(usbPrintjobPath);
                File.Copy(RenderEngine.PrintJob.SlicesPath, usbPrintjobPath + osPathSeparator + "slices.zip", true);
                File.Copy(pathPrinterJobXml, usbPrintjobPath + osPathSeparator + "printjob.apj", true);
                File.Copy(pathPrinterJobChecksumXml, usbPrintjobPath + osPathSeparator + "checksum.crc", true);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                MessageBox.Show("Failed to save printjob", "Failed to save printjob", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }



            //close wait dialog
            t.ReportProgress(100, new WaitWindowUserState(100, string.Empty));
        }

        private void GenerateCalibrationPrintJobAsync_Completed(object sender, EventArgs e)
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke((MethodInvoker)delegate { GenerateCalibrationPrintJobAsync_Completed(sender, e); });
                    return;
                }

                PrinterManager.SelectedPrinters.Remove(this._selectedPrinter);
                PrinterManager.AvailablePrinters.Remove(this._selectedPrinter);
                //SavePrinter.Selected = true;
                this._selectedPrinter.Selected = true;
                PrinterManager.SetDefaultPrinter(this._selectedPrinter);
                this.Enabled = true;
                this.Focus();

            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            //ProcessActionEnd();
        }


        private void PrinterConnectionWizard_FormClosing(object sender, FormClosingEventArgs e)
        {
            ObjectView.Objects3D.Clear();
            SceneView.UpdateGroundPane();

        }
    }
}
