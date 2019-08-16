using Atum.DAL.Compression.Zip;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ComparePrintJobs
{
    public partial class frmComparePrintJobs : Form
    {
        private string _originPath;
        private string _comparePath;

        private ZipFile _originZip;
        private ZipFile _compareZip;

        public frmComparePrintJobs()
        {
            InitializeComponent();
        }

        private void btnComparePrintJobHeader_Click(object sender, EventArgs e)
        {
            GetFolderPath(this.lblComparePrintJobPath);
        }


        private void btnOriginalProjobPath_Click(object sender, EventArgs e)
        {
            GetFolderPath(this.lblOriginalProjobPath);
        }

        private void GetFolderPath(Label lblControl)
        {
            this.btnCompare.Enabled = true;

            var openFolderDialog = new FolderBrowserDialog();
            if (openFolderDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(openFolderDialog.SelectedPath))
            {
                if (Directory.EnumerateFiles(openFolderDialog.SelectedPath, "*.apj").Count() == 1)
                {
                    if (Directory.EnumerateFiles(openFolderDialog.SelectedPath, "*.zip").Count() == 1)
                    {
                        if (lblControl == this.lblComparePrintJobPath)
                        {
                            _comparePath = openFolderDialog.SelectedPath;
                            lblControl.Text = _comparePath;
                        }
                        else
                        {
                            _originPath = openFolderDialog.SelectedPath;
                            lblControl.Text = _originPath;
                        }
                    }
                    else
                    {
                        this.btnCompare.Enabled = false;
                        MessageBox.Show("Selected path does not contain slices.zip");
                    }
                }
                else
                {
                    this.btnCompare.Enabled = false;
                    MessageBox.Show("Selected path does not contain printjob.apj");
                }
            }
        }

        private void btnCompare_Click(object sender, EventArgs e)
        {
            var stringBuilder = new StringBuilder();

            this.plProgressbarValue.BackColor = Color.LimeGreen;
            this.plProgressbarValue.Width = 0;

            if (this._originZip != null)
            {
                this._originZip.Close();
            }

            if (this._compareZip != null)
            {
                this._compareZip.Close();
            }

            //mount zip streams
            this._originZip = new ZipFile(new StreamReader(Path.Combine(this._originPath, "slices.zip")).BaseStream);
            this._compareZip = new ZipFile(new StreamReader(Path.Combine(this._comparePath, "slices.zip")).BaseStream);

            if (this._originZip.Count != this._compareZip.Count)
            {
                this.plProgressbarValue.BackColor = Color.Red;
                this.plProgressbarValue.Width = this.plProgressbar.Width;

                stringBuilder.AppendLine("Different size of slices (" + this._originZip.Count + "/" + this._compareZip.Count + ")");
                this.txtOutput.Text = stringBuilder.ToString();
            }
            else
            {
                //check each slice for bit difference
                var progressbarValueStepSize = (float)this.plProgressbar.Width / this._compareZip.Count;
                for (var sliceIndex = 0; sliceIndex < this._compareZip.Count; sliceIndex++)
                {
                    var originZipEntry = this._originZip.GetEntry(string.Format("{0}.png", sliceIndex));
                    var originBitmapAsArray = new byte[originZipEntry.Size];

                    this._originZip.GetInputStream(originZipEntry).Read(originBitmapAsArray, 0, (int)originZipEntry.Size);

                    var compareZipEntry = this._compareZip.GetEntry(string.Format("{0}.png", sliceIndex));
                    var compareBitmapAsArray = new byte[compareZipEntry.Size];
                    this._compareZip.GetInputStream(compareZipEntry).Read(compareBitmapAsArray, 0, (int)compareZipEntry.Size);

                    if (originBitmapAsArray.Length != compareBitmapAsArray.Length)
                    {
                        this.plProgressbarValue.BackColor = Color.Red;
                        stringBuilder.AppendLine("Different slice size. Sliceindex: " + sliceIndex + " (" + originBitmapAsArray.Length + "/" + compareBitmapAsArray.Length + ")");
                    }
                    else
                    {
                        for(var byteArrayIndex = 0;byteArrayIndex < originBitmapAsArray.Length; byteArrayIndex++)
                        {
                            if (originBitmapAsArray[byteArrayIndex] != compareBitmapAsArray[byteArrayIndex])
                            {
                                this.plProgressbarValue.BackColor = Color.Red;
                                stringBuilder.AppendLine("Different slice picture. Sliceindex: " + sliceIndex + " (" + byteArrayIndex + "/" + byteArrayIndex + ")");
                                break;
                            }
                        }
                    }

                    this.plProgressbarValue.Width = (int)((sliceIndex + 1) * progressbarValueStepSize);
                }

                var xmlManifestError = false;
                var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Atum.DAL.Print.PrintJob));
                var originPrintjobManifest = (Atum.DAL.Print.PrintJob)xmlSerializer.Deserialize(new StreamReader(string.Format("{0}", Path.Combine(this._originPath, "printjob.apj"))));
                var comparePrintjobManifest = (Atum.DAL.Print.PrintJob)xmlSerializer.Deserialize(new StreamReader(string.Format("{0}", Path.Combine(this._comparePath, "printjob.apj"))));

                if (originPrintjobManifest.AntiAliasColor != comparePrintjobManifest.AntiAliasColor)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: AnitiAliasColor");
                }

                if (originPrintjobManifest.AntiAliasFactor != comparePrintjobManifest.AntiAliasFactor)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: AntiAliasFactor");
                }

                if (originPrintjobManifest.AntiAliasSide != comparePrintjobManifest.AntiAliasSide)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: AntiAliasSide");
                }

                if (originPrintjobManifest.Material != comparePrintjobManifest.Material)
                {
                    if (originPrintjobManifest.Material.BleedingOffset != comparePrintjobManifest.Material.BleedingOffset)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material BleedingOffset");
                    }

                    if (originPrintjobManifest.Material.BleedingXYOffset_Inside != comparePrintjobManifest.Material.BleedingXYOffset_Inside)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material BleedingXYOffset_Inside");
                    }

                    if (originPrintjobManifest.Material.BleedingXYOffset_Outside != comparePrintjobManifest.Material.BleedingXYOffset_Outside)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material BleedingXYOffset_Outside");
                    }

                    if (originPrintjobManifest.Material.Color != comparePrintjobManifest.Material.Color)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material Color");
                    }

                    if (originPrintjobManifest.Material.CT1 != comparePrintjobManifest.Material.CT1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material CT1");
                    }

                    if (originPrintjobManifest.Material.CT2 != comparePrintjobManifest.Material.CT2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material CT2");
                    }

                    if (originPrintjobManifest.Material.InitialLayers != comparePrintjobManifest.Material.InitialLayers)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material InitialLayers");
                    }

                    if (originPrintjobManifest.Material.LightIntensityPercentage1 != comparePrintjobManifest.Material.LightIntensityPercentage1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material LightIntensityPercentage1");
                    }

                    if (originPrintjobManifest.Material.LightIntensityPercentage2 != comparePrintjobManifest.Material.LightIntensityPercentage2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material LightIntensityPercentage2");
                    }

                    if (originPrintjobManifest.Material.LT1 != comparePrintjobManifest.Material.LT1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material LT1");
                    }

                    if (originPrintjobManifest.Material.LT2 != comparePrintjobManifest.Material.LT2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material LT2");
                    }

                    if (originPrintjobManifest.Material.PreparationLayersCount != comparePrintjobManifest.Material.PreparationLayersCount)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material PreparationLayersCount");
                    }

                    if (originPrintjobManifest.Material.PrinterHardwareType != comparePrintjobManifest.Material.PrinterHardwareType)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material PrinterHardwareType");
                    }

                    if (originPrintjobManifest.Material.RH1 != comparePrintjobManifest.Material.RH1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RH1");
                    }

                    if (originPrintjobManifest.Material.RH2 != comparePrintjobManifest.Material.RH2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RH2");
                    }

                    if (originPrintjobManifest.Material.RSD1 != comparePrintjobManifest.Material.RSD1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RSD1");
                    }

                    if (originPrintjobManifest.Material.RSD2 != comparePrintjobManifest.Material.RSD2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RSD2");
                    }

                    if (originPrintjobManifest.Material.RSU1 != comparePrintjobManifest.Material.RSU1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RSU1");
                    }

                    if (originPrintjobManifest.Material.RSU2 != comparePrintjobManifest.Material.RSU2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RSU2");
                    }

                    if (originPrintjobManifest.Material.RT1 != comparePrintjobManifest.Material.RT1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RT1");
                    }

                    if (originPrintjobManifest.Material.RT2 != comparePrintjobManifest.Material.RT2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material RT2");
                    }

                    if (originPrintjobManifest.Material.ShrinkFactor != comparePrintjobManifest.Material.ShrinkFactor)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material ShrinkFactor");
                    }

                    if (originPrintjobManifest.Material.SmoothingOffset != comparePrintjobManifest.Material.SmoothingOffset)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material SmoothingOffset");
                    }

                    if (originPrintjobManifest.Material.TAT1 != comparePrintjobManifest.Material.TAT1)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material TAT1");
                    }

                    if (originPrintjobManifest.Material.TAT2 != comparePrintjobManifest.Material.TAT2)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material TAT2");
                    }

                    if (originPrintjobManifest.Material.TransitionLayers.Count != comparePrintjobManifest.Material.TransitionLayers.Count)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material TransitionLayers count");
                    }
                    else
                    {
                        for (var transitionLayerIndex = 0; transitionLayerIndex < comparePrintjobManifest.Material.TransitionLayers.Count; transitionLayerIndex++)
                        {
                            if (originPrintjobManifest.Material.TransitionLayers[transitionLayerIndex].CT != comparePrintjobManifest.Material.TransitionLayers[transitionLayerIndex].CT)
                            {
                                xmlManifestError = true;
                                stringBuilder.AppendLine("Difference in apj: Material TransitionLayer " + transitionLayerIndex + " CT");
                            }

                            if (originPrintjobManifest.Material.TransitionLayers[transitionLayerIndex].LI != comparePrintjobManifest.Material.TransitionLayers[transitionLayerIndex].LI)
                            {
                                xmlManifestError = true;
                                stringBuilder.AppendLine("Difference in apj: Material TransitionLayer " + transitionLayerIndex + " LI");
                            }
                        }
                    }
                   
                    if (originPrintjobManifest.Material.XYResolution != comparePrintjobManifest.Material.XYResolution)
                    {
                        xmlManifestError = true;
                        stringBuilder.AppendLine("Difference in apj: Material XYResolution");
                    }
                }
                    

                if (originPrintjobManifest.Option_TurnProjectorOff != comparePrintjobManifest.Option_TurnProjectorOff)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: Option_TurnProjectOff");
                }

                if (originPrintjobManifest.Option_TurnProjectorOn != comparePrintjobManifest.Option_TurnProjectorOn)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: Option_TurnProjectOn");
                }

                if (originPrintjobManifest.TotalPrintVolume != comparePrintjobManifest.TotalPrintVolume)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: TotalPrintVolume");
                }

                if (originPrintjobManifest.UseProjectorOptions != comparePrintjobManifest.UseProjectorOptions)
                {
                    xmlManifestError = true;
                    stringBuilder.AppendLine("Difference in apj: UseProjectorOptions");
                }

                if (xmlManifestError)
                {
                    this.plProgressbarValue.BackColor = Color.Red;
                }
            }

            if (stringBuilder.ToString() == string.Empty)
            {
                stringBuilder.AppendLine("No differences found");
            }


            this.txtOutput.Text = stringBuilder.ToString();
        }

        private void txtOutput_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
