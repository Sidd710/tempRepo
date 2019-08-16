using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models.Defaults;
using Atum.Studio.Core.ModelView;
using OpenTK;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public partial class frmCalibratePrinter : Form
    {
        private bool _inputValueChanged = false;
        private SortedList<DateTime, TrapeziumCorrection> _previousTrapezoidCorrections;

        private int _previousMeasurementIndex = 0;

        private float _calibrationModelSideX;
        private float _calibrationModelSideY;


        public AtumPrinter AtumPrinter { get; set; }

        public frmCalibratePrinter(AtumPrinter atumPrinter)
        {
            InitializeComponent();

            this.AtumPrinter = atumPrinter;

            this.btnClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_white, btnClose.Size);
            this.pbHelp.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.help_white, pbHelp.Size);

            this.btnExport.BorderColor = this.btnExport.ForeColor = this.btnCheck.BackColor = BrandingManager.Button_BackgroundColor_Dark;
            this.plPrinterCalibrationOverview.BackgroundImage = BrandingManager.Printer_Calibration_Image;

            CalcCalibrationDefaults();

            if (this.AtumPrinter.TrapeziumCorrectionRawInputA == 0) this.AtumPrinter.TrapeziumCorrectionRawInputA = _calibrationModelSideY;
            if (this.AtumPrinter.TrapeziumCorrectionRawInputB == 0) this.AtumPrinter.TrapeziumCorrectionRawInputB = _calibrationModelSideX;
            if (this.AtumPrinter.TrapeziumCorrectionRawInputC == 0) this.AtumPrinter.TrapeziumCorrectionRawInputC = _calibrationModelSideY;
            if (this.AtumPrinter.TrapeziumCorrectionRawInputD == 0) this.AtumPrinter.TrapeziumCorrectionRawInputD = _calibrationModelSideX;

            if (this.AtumPrinter.TrapeziumCorrectionRawInputE == 0)
            {
                this.AtumPrinter.TrapeziumCorrectionRawInputE = this.AtumPrinter.TrapeziumCorrectionRawInputF = CalcDiagonals(_calibrationModelSideY, _calibrationModelSideX, _calibrationModelSideY, _calibrationModelSideX);
            }

            BindMaterials();
            BindTrapeziumValues();
            ValidateCorrectionInput(false);

            this.trapezoidSizeA.TextValueChanged += TrapezoidSize_TextValueChanged;
            this.trapezoidSizeB.TextValueChanged += TrapezoidSize_TextValueChanged;
            this.trapezoidSizeC.TextValueChanged += TrapezoidSize_TextValueChanged;
            this.trapezoidSizeD.TextValueChanged += TrapezoidSize_TextValueChanged;
        }

        private void TrapezoidSize_TextValueChanged(object sender, EventArgs e)
        {
            this._inputValueChanged = true;
        }

        private void BindMaterials()
        {
            List<Material> materials = new List<Material>();
            foreach (var supplier in MaterialManager.Catalog)
            {
                foreach (var material in supplier.GetMaterialsByResolution(this.AtumPrinter))
                {
                    materials.Add(material.Materials[0]);
                }
            }

            var materialList = materials.OrderBy(x => x.DisplayName).ToList();
            if (materialList.Count > 0)
            {
                foreach (var material in materialList)
                {
                    this.cbMaterialSelector.Items.Add(material);
                }

                this.cbMaterialSelector.SelectedIndex = 0;
            }

            this.cbMaterialSelector.Font = FontManager.Montserrat14Regular;
            this.cbMaterialSelector.SelectedIndex = 0;
        }

        private void BindTrapeziumValues()
        {
            this.trapezoidSizeA.TextValue = this.AtumPrinter.TrapeziumCorrectionRawInputA;
            this.trapezoidSizeB.TextValue = this.AtumPrinter.TrapeziumCorrectionRawInputB;
            this.trapezoidSizeC.TextValue = this.AtumPrinter.TrapeziumCorrectionRawInputC;
            this.trapezoidSizeD.TextValue = this.AtumPrinter.TrapeziumCorrectionRawInputD;

            if (this.AtumPrinter.PreviousTrapeziumCorrections != null && this.AtumPrinter.PreviousTrapeziumCorrections.Count > 0)
            {
                this.linkLabel1.Visible = this.lblPreviousMeasurementsDate.Visible = true;
                _previousMeasurementIndex = this.AtumPrinter.PreviousTrapeziumCorrections.Count - 1;
                this.lblPreviousMeasurementsDate.Text = this.AtumPrinter.PreviousTrapeziumCorrections[_previousMeasurementIndex].TrapeziumCorrectionSetDateTime.ToShortDateString() + " " + this.AtumPrinter.PreviousTrapeziumCorrections[_previousMeasurementIndex].TrapeziumCorrectionSetDateTime.ToShortTimeString();
                this._previousTrapezoidCorrections = new SortedList<DateTime, TrapeziumCorrection>();
                foreach (var previousCorrection in this.AtumPrinter.PreviousTrapeziumCorrections)
                {
                    this._previousTrapezoidCorrections.Add(previousCorrection.TrapeziumCorrectionSetDateTime, previousCorrection);
                }
            }
            else
            {
                this.linkLabel1.Visible = this.lblPreviousMeasurementsDate.Visible = false;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void pbHelp_Click(object sender, EventArgs e)
        {

        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            //check if value has been changed
            if (this._inputValueChanged)
            {

                this.AtumPrinter.PreviousTrapeziumCorrections.Clear();
                this.AtumPrinter.PreviousTrapeziumCorrections.Add(new TrapeziumCorrection()
                {
                    TrapeziumCorrectionInputA = this.AtumPrinter.TrapeziumCorrectionInputA,
                    TrapeziumCorrectionInputB = this.AtumPrinter.TrapeziumCorrectionInputB,
                    TrapeziumCorrectionInputC = this.AtumPrinter.TrapeziumCorrectionInputC,
                    TrapeziumCorrectionInputD = this.AtumPrinter.TrapeziumCorrectionInputD,
                    TrapeziumCorrectionInputE = this.AtumPrinter.TrapeziumCorrectionInputE,
                    TrapeziumCorrectionInputF = this.AtumPrinter.TrapeziumCorrectionInputF,

                    TrapeziumCorrectionRawInputA = this.AtumPrinter.TrapeziumCorrectionRawInputA,
                    TrapeziumCorrectionRawInputB = this.AtumPrinter.TrapeziumCorrectionRawInputB,
                    TrapeziumCorrectionRawInputC = this.AtumPrinter.TrapeziumCorrectionRawInputC,
                    TrapeziumCorrectionRawInputD = this.AtumPrinter.TrapeziumCorrectionRawInputD,
                    TrapeziumCorrectionRawInputE = this.AtumPrinter.TrapeziumCorrectionRawInputE,
                    TrapeziumCorrectionRawInputF = this.AtumPrinter.TrapeziumCorrectionRawInputF,
                    TrapeziumCorrectionSetDateTime = DateTime.Now
                });

                //calc new values
                CalcTrapezoidValues();

                ValidateCorrectionInput(true);

                BindTrapeziumValues();
            }

            //this.DialogResult = DialogResult.OK;
            //this.Close();
        }

        private void CalcCalibrationDefaults()
        {
            if (this.AtumPrinter is AtumDLPStation5)
            {
                switch (this.AtumPrinter.PrinterXYResolution)
                {
                    case AtumPrinter.PrinterXYResolutionType.Micron50:
                        this._calibrationModelSideX = 80;
                        this._calibrationModelSideY = 40;

                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron75:
                        this._calibrationModelSideX = 120; //120
                        this._calibrationModelSideY = 60; //75

                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron100:
                        this._calibrationModelSideX = 160;
                        this._calibrationModelSideY = 80;

                        break;
                }
            }
            else
            {
                switch (this.AtumPrinter.PrinterXYResolution)
                {
                    case AtumPrinter.PrinterXYResolutionType.Micron40:
                        this._calibrationModelSideX = 50;
                        this._calibrationModelSideY = 30;
                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron50:
                        this._calibrationModelSideX = 80;
                        this._calibrationModelSideY = 50;

                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron75:
                        this._calibrationModelSideX = 120; //120
                        this._calibrationModelSideY = 75; //75

                        break;
                    case AtumPrinter.PrinterXYResolutionType.Micron100:
                        this._calibrationModelSideX = 160;
                        this._calibrationModelSideY = 100;

                        break;
                }
            }
        }


        private void CalcTrapezoidValues()
        {


            if ((float)this.trapezoidSizeA.TextValue != this.AtumPrinter.TrapeziumCorrectionRawInputA)
            {
                this.AtumPrinter.TrapeziumCorrectionInputA = ((float)this.trapezoidSizeA.TextValue / this._calibrationModelSideY) * this.AtumPrinter.TrapeziumCorrectionInputA;
                this.AtumPrinter.TrapeziumCorrectionRawInputA = (float)this.trapezoidSizeA.TextValue;
            }

            if ((float)this.trapezoidSizeB.TextValue != this.AtumPrinter.TrapeziumCorrectionRawInputB)
            {
                this.AtumPrinter.TrapeziumCorrectionInputB = ((float)this.trapezoidSizeB.TextValue / this._calibrationModelSideX) * this.AtumPrinter.TrapeziumCorrectionInputB;
                this.AtumPrinter.TrapeziumCorrectionRawInputB = (float)this.trapezoidSizeB.TextValue;
            }

            if ((float)this.trapezoidSizeC.TextValue != this.AtumPrinter.TrapeziumCorrectionRawInputC)
            {
                this.AtumPrinter.TrapeziumCorrectionInputC = ((float)this.trapezoidSizeC.TextValue / this._calibrationModelSideY) * this.AtumPrinter.TrapeziumCorrectionInputC;
                this.AtumPrinter.TrapeziumCorrectionRawInputC = (float)this.trapezoidSizeC.TextValue;
            }

            if ((float)this.trapezoidSizeD.TextValue != this.AtumPrinter.TrapeziumCorrectionRawInputD)
            {
                this.AtumPrinter.TrapeziumCorrectionInputD = ((float)this.trapezoidSizeD.TextValue / this._calibrationModelSideX) * this.AtumPrinter.TrapeziumCorrectionInputD;
                this.AtumPrinter.TrapeziumCorrectionRawInputD = (float)this.trapezoidSizeD.TextValue;
            }


            this.AtumPrinter.TrapeziumCorrectionRawInputE = this.AtumPrinter.TrapeziumCorrectionRawInputF = CalcDiagonals(this.AtumPrinter.TrapeziumCorrectionRawInputA, this.AtumPrinter.TrapeziumCorrectionRawInputB, this.AtumPrinter.TrapeziumCorrectionRawInputC, this.AtumPrinter.TrapeziumCorrectionRawInputD); ;

            //calc new diagonals
            this.AtumPrinter.TrapeziumCorrectionInputE = this.AtumPrinter.TrapeziumCorrectionInputF = CalcDiagonals(this.AtumPrinter.TrapeziumCorrectionInputA, this.AtumPrinter.TrapeziumCorrectionInputB, this.AtumPrinter.TrapeziumCorrectionInputC, this.AtumPrinter.TrapeziumCorrectionInputD);
        }

        private void ValidateCorrectionInput(bool closeWindowWhenCompleted)
        {
            this.trapezoidSizeA.SetValidationColor(Color.LimeGreen);
            this.trapezoidSizeB.SetValidationColor(Color.LimeGreen);
            this.trapezoidSizeC.SetValidationColor(Color.LimeGreen);
            this.trapezoidSizeD.SetValidationColor(Color.LimeGreen);

            var offsetASide = _calibrationModelSideY / this.AtumPrinter.TrapeziumCorrectionRawInputA;
            var offsetBSide = _calibrationModelSideX / this.AtumPrinter.TrapeziumCorrectionRawInputB;
            var offsetCSide = _calibrationModelSideY / this.AtumPrinter.TrapeziumCorrectionRawInputC;
            var offsetDSide = _calibrationModelSideX / this.AtumPrinter.TrapeziumCorrectionRawInputD;


            var offsetASideValid = true;
            if (offsetASide < 0.99 || offsetASide > 1.01)
            {
                offsetASideValid = false;
                this.trapezoidSizeA.SetValidationColor(BrandingManager.Textbox_HighlightColor_Error);
            }
            var offsetBSideValid = true;
            if (offsetBSide < 0.99 || offsetBSide > 1.01)
            {
                offsetBSideValid = false;
                this.trapezoidSizeB.SetValidationColor(BrandingManager.Textbox_HighlightColor_Error);
            }

            var offsetCSideValid = true;
            if (offsetCSide < 0.99 || offsetCSide > 1.01)
            {
                offsetCSideValid = false;
                this.trapezoidSizeC.SetValidationColor(BrandingManager.Textbox_HighlightColor_Error);
            }
            var offsetDSideValid = true;
            if (offsetDSide < 0.99 || offsetDSide > 1.01)
            {
                offsetDSideValid = false;
                this.trapezoidSizeD.SetValidationColor(BrandingManager.Textbox_HighlightColor_Error);
            }

            if (!offsetASideValid || !offsetBSideValid || !offsetCSideValid || !offsetDSideValid)
            {
                SetValidationMessage("Further calibration required. Please repeat step 1 and step 2.");
            }
            else
            {
                ResetValidationControl();

                if (closeWindowWhenCompleted)
                {
                    this.DialogResult = DialogResult.OK;
                    this.Close();
                }
            }

        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            var selectedMaterial = ((Material)this.cbMaterialSelector.SelectedItem);
            using (var usbSelector = new frmUSBDriveSelector())
            {
                if (usbSelector.ShowDialog() == DialogResult.Yes && usbSelector.SelectedDrive != null && Directory.Exists(usbSelector.SelectedDrive.Name))
                {
                    if (PrintJobManager.SelectedPrinter == null)
                    {
                        PrintJobManager.SelectedPrinter = this.AtumPrinter;
                    }

                    var selectedDrive = usbSelector.SelectedDrive;
                    Task.Run(() => RenderJob(selectedMaterial, this.AtumPrinter, selectedDrive)).Wait();

                    ObjectView.Objects3D.Remove(ObjectView.Objects3D.Last());
                    new frmMessageBox("Export completed", string.Format("Calibration Job is saved on: {0} ({1})", selectedDrive.VolumeLabel, selectedDrive.Name), MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
                }
            }

            

        }

        private void linkLabel1_Click(object sender, EventArgs e)
        {
            if (this._previousMeasurementIndex > -1)
            {
                var previousValues = this.AtumPrinter.PreviousTrapeziumCorrections[this._previousMeasurementIndex];
                this.trapezoidSizeA.TextValue = previousValues.TrapeziumCorrectionRawInputA;
                this.trapezoidSizeB.TextValue = previousValues.TrapeziumCorrectionRawInputB;
                this.trapezoidSizeC.TextValue = previousValues.TrapeziumCorrectionRawInputC;
                this.trapezoidSizeD.TextValue = previousValues.TrapeziumCorrectionRawInputD;


                this.AtumPrinter.TrapeziumCorrectionInputA = previousValues.TrapeziumCorrectionInputA;
                this.AtumPrinter.TrapeziumCorrectionInputB = previousValues.TrapeziumCorrectionInputB;
                this.AtumPrinter.TrapeziumCorrectionInputC = previousValues.TrapeziumCorrectionInputC;
                this.AtumPrinter.TrapeziumCorrectionInputD = previousValues.TrapeziumCorrectionInputD;
                this.AtumPrinter.TrapeziumCorrectionInputE = previousValues.TrapeziumCorrectionInputE;
                this.AtumPrinter.TrapeziumCorrectionInputF = previousValues.TrapeziumCorrectionInputF;

                this.AtumPrinter.TrapeziumCorrectionRawInputA = previousValues.TrapeziumCorrectionRawInputA;
                this.AtumPrinter.TrapeziumCorrectionRawInputB = previousValues.TrapeziumCorrectionRawInputB;
                this.AtumPrinter.TrapeziumCorrectionRawInputC = previousValues.TrapeziumCorrectionRawInputC;
                this.AtumPrinter.TrapeziumCorrectionRawInputD = previousValues.TrapeziumCorrectionRawInputD;
                this.AtumPrinter.TrapeziumCorrectionRawInputE = previousValues.TrapeziumCorrectionRawInputE;
                this.AtumPrinter.TrapeziumCorrectionRawInputF = previousValues.TrapeziumCorrectionRawInputF;

                ValidateCorrectionInput(false);

                this.linkLabel1.Visible = this.lblPreviousMeasurementsDate.Visible = false;

                this.AtumPrinter.PreviousTrapeziumCorrections.Clear();
                this._previousMeasurementIndex = -1;
            }

            //Just for checking control
            //SetValidationMessage("Deviation too big. Please re-measure the dimension.");
        }

        private void SetValidationMessage(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                this.plValidationMessage.BackColor = Color.FromArgb(255, 211, 206);
                this.lblValidationMessage.Text = message;
            }
            else
            {
                ResetValidationControl();
            }
        }

        private void ResetValidationControl()
        {
            this.plValidationMessage.BackColor = Color.WhiteSmoke;
            this.lblValidationMessage.Text = string.Empty;
        }

        float CalcY(float sideAC, float sideAB, float sideBC)
        {
            float x = ((sideAC * sideAC + sideAB * sideAB - sideBC * sideBC) / (2 * sideAB));
            float h = (float)Math.Sqrt((sideAC * sideAC) - (x * x));
            float angleA = (float)(Math.Atan(h / x));
            if (angleA < 0) { angleA = (float)(angleA + Math.PI); }
            if (angleA > Math.PI) { angleA = (float)(angleA - Math.PI); }
            angleA = (float)Math.Round(MathHelper.RadiansToDegrees(angleA), 3);
            float pointB_Y = (float)((float)sideAB * Math.Sin(MathHelper.DegreesToRadians(angleA - 90)));
            return (float)Math.Round(pointB_Y, 3);

        }

        static float CalcX(float sideAC, float sideAB, float sideBC)
        {
            var x = ((sideAC * sideAC + sideAB * sideAB - sideBC * sideBC) / (2 * sideAB));
            var h = Math.Sqrt((sideAC * sideAC) - (x * x));
            var angleA = (Math.Atan(h / x));
            if (angleA < 0) { angleA = angleA + Math.PI; }
            if (angleA > Math.PI) { angleA = angleA - Math.PI; }
            angleA = MathHelper.RadiansToDegrees(angleA);
            var pointB_X = sideAB * Math.Cos(MathHelper.DegreesToRadians(angleA - 90));

            return (float)Math.Round(pointB_X, 4);

        }

        private float CalcDiagonals(float sideA, float sideB, float sideC, float sideD)
        {

            //first calc side upper left -> right down
            var diagonal = (float)Math.Round(Math.Sqrt((sideA * sideA) +
                (float)(sideD * sideD)), 4);


            var pointB = new Vector2(CalcX(sideA, sideB, diagonal),
                                CalcY(sideA, sideB, diagonal));
            var pointC = new Vector2(CalcX(sideA, sideD, diagonal),
                CalcY(sideA, sideD, diagonal));

            pointB.Y = -pointB.Y;

            if (pointC.X != Single.NaN)
            {
                pointC.Y += sideA;

                if (!Single.IsNaN(pointB.X))
                {
                    var approxSideC = (pointC - pointB).Length;
                    if (approxSideC < sideC)
                    {
                        while (approxSideC < sideC)
                        {
                            diagonal += 0.0001f;
                            pointB = new Vector2(CalcX(sideA, sideB, diagonal),
                                       CalcY(sideA, sideB, diagonal));
                            pointC = new Vector2(CalcX(sideA, sideD, diagonal),
                                CalcY(sideA, sideD, diagonal));

                            pointB.Y = -pointB.Y;

                            if (pointC.X != Single.NaN)
                            {
                                pointC.Y += sideA;

                                if (!Single.IsNaN(pointB.X))
                                {
                                    approxSideC = (pointC - pointB).Length;
                                    // Debug.WriteLine(sideC);
                                }
                            }
                        }
                    }
                    else if (approxSideC > sideC)
                    {
                        while (approxSideC > sideC)
                        {
                            diagonal -= 0.0001f;
                            pointB = new Vector2(CalcX(sideA, sideB, diagonal),
                                       CalcY(sideA, sideB, diagonal));
                            pointC = new Vector2(CalcX(sideA, sideD, diagonal),
                                CalcY(sideA, sideD, diagonal));

                            pointB.Y = -pointB.Y;

                            if (pointC.X != Single.NaN)
                            {
                                pointC.Y += sideA;

                                if (!Single.IsNaN(pointB.X))
                                {
                                    approxSideC = (pointC - pointB).Length;
                                    //Debug.WriteLine(sideC);
                                }
                            }
                        }
                    }

                    return diagonal;
                }
            }

            return 0f;

        }


        private void cbMaterials_DrawItem(object sender, DrawItemEventArgs e)
        {
            //try
            //{
            //    e.DrawBackground();
            //    Brush myBrush = Brushes.Black;
            //    e.Graphics.DrawString(cbMaterials.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);                
            //    e.DrawFocusRectangle();
            //}
            //catch
            //{

            //}
        }

        private void frmCalibratePrinter_Load(object sender, EventArgs e)
        {

            if (FontManager.Loaded)
            {
                this.lblHeader.Font = FontManager.Montserrat18Regular;
                this.Text = this.lblHeader.Text = string.Format(this.Text, this.AtumPrinter.DisplayName);
                this.Icon = BrandingManager.MainForm_Icon;

                this.lblOne.Font = this.lblTwo.Font = FontManager.Montserrat16Regular;

                this.lblValidationMessage.Font = this.lblPreviousMeasurementsDate.Font = this.linkLabel1.Font = this.lblTwoText.Font = this.lblOneText.Font = FontManager.Montserrat14Regular;
            }

        }


        private void RenderJob(Material selectedMaterial, AtumPrinter selectedPrinter, DriveInfo selectedDrive)
        {
            //load calibration model
            for (var objectViewModelIndex = ObjectView.Objects3D.Count - 1; objectViewModelIndex > 1; objectViewModelIndex--)
            {
                ObjectView.Objects3D.RemoveAt(objectViewModelIndex);
            }

            RenderEngine.PrintJob = new DAL.Print.PrintJob();
            RenderEngine.PrintJob.Name = "Calibration";
            RenderEngine.PrintJob.Material = selectedMaterial;
            RenderEngine.PrintJob.SelectedPrinter = this.AtumPrinter;

           
            var basicCorrection = new BasicCorrectionModel();
            basicCorrection.Open((string)null, false, selectedMaterial.ModelColor, ObjectView.NextObjectIndex, basicCorrection.Triangles, enableProgressStatus:false);
            basicCorrection.UpdateBoundries();
            for (var modelIndex = ObjectView.Objects3D.Count - 1; modelIndex > 0; modelIndex--)
            {
                ObjectView.Objects3D.RemoveAt(modelIndex);
            }
            ObjectView.AddModel(basicCorrection, false, false, false);

            RenderEngine.RenderAsync();

            try
            {

                while (RenderEngine.TotalAmountSlices != RenderEngine.TotalProcessedSlices && !RenderEngine.PrintJob.PostRenderCompleted)
                {
                    var progress = (((decimal)RenderEngine.TotalProcessedSlices / (decimal)RenderEngine.TotalAmountSlices) * 100);
                    Thread.Sleep(250);
                }
            }
            catch (Exception exc)
            {
                Atum.DAL.Managers.LoggingManager.WriteToLog("GeneratePrintJobAsync", "Exc", exc);
                new frmMessageBox("GeneratePrintJobAsync", exc.Message, MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
            }

            try
            {

                var usbPrintjobPath = string.Format(@"{0}{1}", selectedDrive.Name, RenderEngine.PrintJob.Name);

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
                if (!Directory.Exists(usbPrintjobPath))
                    Directory.CreateDirectory(usbPrintjobPath);

                var zipCopied = false;
                while (!zipCopied)
                {
                    try
                    {
                        File.Copy(RenderEngine.PrintJob.SlicesPath, usbPrintjobPath + osPathSeparator + "slices.zip", true);
                        File.Copy(pathPrinterJobXml, usbPrintjobPath + osPathSeparator + "printjob.apj", true);
                        File.Copy(pathPrinterJobChecksumXml, usbPrintjobPath + osPathSeparator + "checksum.crc", true);

                        zipCopied = true;
                    }
                    catch
                    {
                        Thread.Sleep(250);
                    }
                }
            }
            catch (Exception exc)
            {
                new frmMessageBox("Failed to save printjob", "Failed to save printjob", MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
            }

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
    }
}
