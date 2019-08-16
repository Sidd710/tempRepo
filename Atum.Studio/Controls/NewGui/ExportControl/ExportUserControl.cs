using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using Atum.DAL.Compression.Zip;
using System.IO;
using System.Diagnostics;
using Atum.DAL.Hardware;
using System.Net.NetworkInformation;
using Atum.DAL.Managers;
using System.Net;
using System.Xml.Serialization;
using Atum.Studio.Core.Engines;
using Atum.Studio.Controls.NewGui.SliderControl.SliderControlTracker;
using Atum.Studio.Controls.SceneControlToolTips;
using Atum.Studio.Controls.NewGui;
using Atum.Studio.Properties;

namespace Atum.Studio.Controls.NewGui.ExportControl
{
    public partial class ExportUserControl : UserControl
    {
        internal event EventHandler ExportCompleted;

        public CirclularProgressBar CirclularProgressBar { get; set; }

        public PictureboxWithMagnifier picPreview { get; set; }

        internal float ModelVolume { get; set; }

        long _sliceIndex = 1;
        long _sliceCount = 0;
        private ZipFile _printJobZip;

        internal float _value;
        internal float Value
        {
            get
            {
                return _value;
            }
            set
            {
                this._value = value;
                this.CirclularProgressBar.Value = Convert.ToInt32(value);
            }
        }

        public ExportUserControl()
        {
            //this.picPreview.Visible = false;

            InitializeComponent();

            pbExport.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.combined_shape, pbExport.Size);
            if (frmStudioMain.SceneControl != null)
                this.Size = frmStudioMain.SceneControl.Size;

            CirclularProgressBar = new CirclularProgressBar();
            CirclularProgressBar.TagLine = "Generating print job…";
            CirclularProgressBar.Value = 0;
            CirclularProgressBar.OuterRingWidth = 8;
            CirclularProgressBar.BackColor = this.BackColor;
            CirclularProgressBar.ForeColor = Color.FromArgb(255, 24, 0);
            CirclularProgressBar.OuterRingBaseColor = Color.FromArgb(40, 46, 51);
            CirclularProgressBar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Bottom)));
            CirclularProgressBar.MinimumSize = new Size(200, 200);

            CirclularProgressBar.MaximumSize = new Size(480, 480);

            CalculateCircularProgressBarSize();
            CirclularProgressBar.BringToFront();
            plExportControl.Controls.Add(CirclularProgressBar);
            plExportControl.Dock = DockStyle.Fill;

            lblExportClick.Font = FontManager.Montserrat18Bold;

            this.mTracker.ValueChanged += MTracker_ValueChanged;


        }

        private void MTracker_ValueChanged(object sender, decimal value)
        {
            this.mTracker.Value = (int)value;
            this._sliceIndex = this.mTracker.Value; //zerobased tracker index
            this.ShowImage();
            this.txtSlicerStartIndex.Text = (this._sliceIndex + 1).ToString();
            this.txtSlicerStartIndex.Select(0, this.txtSlicerStartIndex.Text.Length);                 // Select from there to the end
            this.txtSlicerStartIndex.SelectionAlignment = HorizontalAlignment.Center;                // Set the alignment of the selection.
            this.txtSlicerStartIndex.DeselectAll();

            this.plDown.Enabled = this._sliceIndex > 1;
            this.plUp.Enabled = this._sliceIndex < this._sliceCount;
        }

        private void CalculateCircularProgressBarSize()
        {
            int value = this.Width * 480 / 1920;
            CirclularProgressBar.Size = new Size(value, value);
            CirclularProgressBar.Left = (this.Width - this.Padding.Left - CirclularProgressBar.Width) / 2;
            CirclularProgressBar.Top = (this.Height - this.Padding.Top - CirclularProgressBar.Height) / 2;
        }

        public void ResizeControl()
        {
            this.Size = frmStudioMain.SceneControl.Size;
            CalculateCircularProgressBarSize();
        }


        public void ExportEvent()
        {
            //unload pictures
            if (this._printJobZip != null)
            {
                this.picPreview.Image = null;
                this._printJobZip.Close();
                this._printJobZip = null;

                GC.Collect();
                GC.Collect();
            }

            using (var usbSelector = new frmUSBDriveSelector())
            {
                if (usbSelector.ShowDialog() == DialogResult.Yes && usbSelector.SelectedDrive != null)
                {
                    SaveToDrive(usbSelector.SelectedDrive);
                }
            }

        }

        public void SaveToDrive(DriveInfo selectedDrive)
        {

            if (!selectedDrive.DriveFormat.StartsWith("FAT") && !selectedDrive.DriveFormat.ToLower().StartsWith("msdos"))
            {
                new frmMessageBox("FAT32 not supported", "Selected drive format is not supported. Only FAT(32) is currently supported", MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
                return;
            }

            //close zip file
            if (this._printJobZip != null)
            {
                this.picPreview.Image = null;
                this._printJobZip.Close();
                this._printJobZip = null;
            }

            GC.Collect();
            GC.Collect();

            
            try
            {
                var usbPrintjobPath = string.Format(@"{0}{1}", selectedDrive.Name, Core.Helpers.StringHelper.RemoveDiacritics(RenderEngine.PrintJob.Name));
                var saveToUSB = true;
                while (Directory.Exists(usbPrintjobPath) && Directory.GetFiles(usbPrintjobPath).Length > 0)
                {
                    if (new frmMessageBox("Replace existing printjob?", "Replace existing printjob?", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button1).ShowDialog() == DialogResult.No)
                    {
                        var printJobName = "New printjob name";
                        if (InputDialog.Show(ref printJobName) == DialogResult.OK)
                        {
                            RenderEngine.PrintJob.Name = printJobName;
                            usbPrintjobPath = string.Format(@"{0}{1}", selectedDrive.Name, RenderEngine.PrintJob.Name);
                        }
                        else
                        {
                            //
                            saveToUSB = false;
                            break;
                        }
                    }
                    else
                    {
                        foreach (var file in Directory.GetFiles(usbPrintjobPath)) {
                            File.Delete(file);
                        }
                    }
                }

                if (saveToUSB)
                {
                    var pathPrinterJobXml = Path.Combine(Path.GetTempPath(), "printjob.xml");
                    var pathPrinterJobChecksumXml = Path.Combine(Path.GetTempPath(), "checksum.crc");

                    var serializer = new XmlSerializer(typeof(DAL.Print.PrintJob));
                    using (var streamWriter = new StreamWriter(pathPrinterJobXml, false))
                    {
                        serializer.Serialize(streamWriter, RenderEngine.PrintJob);
                    }

                    //checksumfile
                    using (var checksumWriter = new StreamWriter(pathPrinterJobChecksumXml, false))
                    {
                        checksumWriter.WriteLine(DateTime.Now.Ticks.ToString());
                    }

                    if (!Directory.Exists(usbPrintjobPath)) Directory.CreateDirectory(usbPrintjobPath);

                    File.Copy(RenderEngine.PrintJob.SlicesPath, Path.Combine(usbPrintjobPath, "slices.zip"), true);
                    File.Copy(pathPrinterJobXml, Path.Combine(usbPrintjobPath, "printjob.apj"), true);
                    File.Copy(pathPrinterJobChecksumXml, Path.Combine(usbPrintjobPath, "checksum.crc"), true);

                    this.ExportCompleted?.Invoke(null, null);
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                new frmMessageBox("Failed to save printjob", "Failed to save printjob to selected drive.\r\n\r\nUsually the directory exists and is locked by a different application.", MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
            }

        }


        private void plExportClick_Click(object sender, EventArgs e)
        {
            ExportEvent();
        }

        private void pbExport_Click(object sender, EventArgs e)
        {
            ExportEvent();
        }

        private void lblExportClick_Click(object sender, EventArgs e)
        {
            ExportEvent();
        }
        public void InitPrintJob()
        {
            ExchangeControls();

            this.MouseWheel += new MouseEventHandler(frmPrintPreview_MouseWheel);
            if (RenderEngine.PrintJob == null || RenderEngine.PrintJob.SlicesPath == null)
            {
                while (RenderEngine.PrintJob.SlicesPath == null)
                {
                    System.Threading.Thread.Sleep(1000);
                }
            }

            if (!File.Exists(RenderEngine.PrintJob.SlicesPath))
            {
                System.Threading.Thread.Sleep(1000);
            }


            while (true)
            {
                try
                {

                    this._printJobZip = new ZipFile(new StreamReader(RenderEngine.PrintJob.SlicesPath).BaseStream);
                    this._sliceCount = this._printJobZip.Count;
                    this.mTracker.Maximum = (int)this._sliceCount - 1;
                    this.mTracker.Value = this.mTracker.Minimum;

                    this.lblSliceTotalCount.Text = (this._sliceCount).ToString();

                    this.plFooter.Init();
                    this.ShowImage();

                    this.UpdateControls();

                    break;
                }
                catch (IOException exc)
                {
                    Debug.WriteLine(exc);
                    System.Threading.Thread.Sleep(1000);
                }
            }
        }

        internal void UnloadControl()
        {
            if (_printJobZip != null)
            {
                this._printJobZip.Close();
            }
        }

        private void ExchangeControls()
        {
            this.picPreview = new PictureboxWithMagnifier();
            picPreview.BackColor = Color.Black;
            picPreview.SendToBack();

            this.UpdatePositions();

            var circularProgressBarControl = this.plExportControl.Controls.OfType<CirclularProgressBar>().FirstOrDefault();
            if (circularProgressBarControl != null)
                this.plExportControl.Controls.Remove(circularProgressBarControl);

            this.BackColor = this.plExportControl.BackColor = Color.Black;
            this.plDown.Visible = true;
            this.plUp.Visible = true;
            this.plSlider.Visible = true;
            plExportClick.Visible = true;

            if (FontManager.Loaded)
            {
                this.lblSliceTotalCount.Font = this.txtSlicerStartIndex.Font = FontManager.Montserrat16Regular;
            }

            this.plExportControl.Controls.Add(picPreview);
        }

        private void ShowImage()
        {
            if (this._printJobZip == null)
            {
                this._printJobZip = new ZipFile(new StreamReader(RenderEngine.PrintJob.SlicesPath).BaseStream);
            }
            var zipEntry = this._printJobZip.GetEntry(string.Format("{0}.png", this._sliceIndex));
            if (zipEntry != null)
            {
                using (var imageZipStream = this._printJobZip.GetInputStream(zipEntry))
                {
                    this.picPreview.BackgroundImage = Image.FromStream(imageZipStream);
                    this.picPreview.ImageHasChanged();
                    this.picPreview.MagnifierRefreshFromBitmap();
                }
            }
        }
        void UpdateControls()
        {
            this.mTracker.Value = (int)this._sliceIndex;
            this.txtSlicerStartIndex.Text = (this._sliceIndex + 1).ToString();
            this.txtSlicerStartIndex.Select(0, this.txtSlicerStartIndex.Text.Length);                 // Select from there to the end
            this.txtSlicerStartIndex.SelectionAlignment = HorizontalAlignment.Center;                // Set the alignment of the selection.
            this.txtSlicerStartIndex.DeselectAll();


        }

        void UpdatePositions()
        {
            if (this.plSlider != null && this.plFooter != null && picPreview != null)
            {
                var xyOffset = 16;

                this.plFooter.Left = (this.Width / 2) - (this.plFooter.Width / 2) - (this.plExportClick.Width / 2) - (this.plExportClick.Margin.Left);
                this.plFooter.Top = this.plExportClick.Top = this.Height - 26 - this.plFooter.Height;
                this.plExportClick.Left = this.plFooter.Left + this.plFooter.Width + this.plExportClick.Margin.Left;

                //update pixturebox
                var xMinPosition = 0;
                var xMaxPosition = this.plSlider.Left - xyOffset;
                var yMinPosition = 0;
                var yMaxPosition = this.plFooter.Top - xyOffset;
                picPreview.Left = xMinPosition;
                picPreview.Top = yMinPosition;
                picPreview.Width = xMaxPosition - xMinPosition;
                picPreview.Height = yMaxPosition - yMinPosition;



                this.plFooter.Visible = true;
            }
        }

        void frmPrintPreview_MouseWheel(object sender, MouseEventArgs e)
        {
            //uncomment 

            if (e.Delta > 0)
            {
                NextSlice();
            }
            else
            {
                PreviousSlice();
            }
        }

        private void PreviousSlice()
        {
            this.mTracker.Value--;
        }

        private void NextSlice()
        {
            this.mTracker.Value++;
        }

        private void plUp_Click(object sender, EventArgs e)
        {
            NextSlice();
        }

        private void plDown_Click(object sender, EventArgs e)
        {
            PreviousSlice();
        }

        private void plExportControl_Resize(object sender, EventArgs e)
        {
            this.plExportControl.Width = this.Width;
            UpdatePositions();
        }

        private void ExportUserControl_Resize(object sender, EventArgs e)
        {
            UpdatePositions();
        }

        private void txtSlicerStartIndex_TextChanged(object sender, EventArgs e)
        {
        }

        private void txtSlicerStartIndex_KeyPress(object sender, KeyPressEventArgs e)
        {
            var currentIndex = this.txtSlicerStartIndex.SelectionStart;
            if (e.KeyChar == (char)Keys.D0 || e.KeyChar == (char)Keys.D1 || e.KeyChar == (char)Keys.D2 || e.KeyChar == (char)Keys.D3 || e.KeyChar == (char)Keys.D4 || e.KeyChar == (char)Keys.D5
                || e.KeyChar == (char)Keys.D6 || e.KeyChar == (char)Keys.D7 || e.KeyChar == (char)Keys.D8 || e.KeyChar == (char)Keys.D9)
            {
                if (this.txtSlicerStartIndex.SelectedText == this.txtSlicerStartIndex.Text)
                {
                    this.txtSlicerStartIndex.Text = e.KeyChar.ToString();
                }
                else
                {
                    this.txtSlicerStartIndex.Text += e.KeyChar.ToString();
                }

                ProcessJumpToText();
            }

            e.Handled = true;

        }

        private void ProcessJumpToText()
        {
            //fast jump to
            if (this.txtSlicerStartIndex.Text != string.Empty)
            {
                var index = int.Parse(this.txtSlicerStartIndex.Text);
                if (index - 1 <= this.mTracker.Maximum && index >= 1)
                {
                    this.mTracker.Value = index - 1;
                }
                else if (index > this.mTracker.Maximum)
                {
                    this.mTracker.Value = this.mTracker.Maximum;
                    this.txtSlicerStartIndex.Text = (this.mTracker.Value + 1).ToString();
                }
            }

            //center input
            this.txtSlicerStartIndex.Select(0, this.txtSlicerStartIndex.Text.Length);                 // Select from there to the end
            this.txtSlicerStartIndex.SelectionAlignment = HorizontalAlignment.Center;                // Set the alignment of the selection.
            this.txtSlicerStartIndex.DeselectAll();
            this.txtSlicerStartIndex.SelectionStart = this.txtSlicerStartIndex.Text.Length;

            this.txtSlicerStartIndex.Select();
            this.txtSlicerStartIndex.Focus();

        }

        private void txtSlicerStartIndex_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void plSlider_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMagnifier();
        }

        private void plDown_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMagnifier();
        }

        private void plUp_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMagnifier();
        }

        private void CloseMagnifier()
        {
            if (picPreview != null)
            {
                picPreview.HideMagnifier();
            }
        }

        private void mTracker_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMagnifier();
        }

        private void lblSliceTotalCount_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMagnifier();
        }

        private void txtSlicerStartIndex_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMagnifier();
        }

        private void plExportControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (this.plDown.Visible)
            {
                CloseMagnifier();
            }
        }

        private void txtSlicerStartIndex_MouseDown(object sender, MouseEventArgs e)
        {
            this.txtSlicerStartIndex.Select(0, this.txtSlicerStartIndex.TextLength);
        }

        private void txtSlicerStartIndex_KeyUp(object sender, KeyEventArgs e)
        {
            if ((Keys)e.KeyData == Keys.Delete)
            {
                ProcessJumpToText();
                e.Handled = true;
            }
            else if ((Keys)e.KeyData == Keys.Back)
            {
                ProcessJumpToText();
                e.Handled = true;
            }
        }

        private void txtSlicerStartIndex_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtSlicerStartIndex);
        }

        private void ExportUserControl_Load(object sender, EventArgs e)
        {

        }

        private Timer _fastMove = new Timer();
        private void plUp_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_fastMove.Enabled)
            {
                _fastMove = new Timer();
                _fastMove.Tick += _fastMoveUp_Tick;
                _fastMove.Interval = 100;
                _fastMove.Start();
            }
        }

        private void _fastMoveUp_Tick(object sender, EventArgs e)
        {
            if (this.mTracker.Value < this.mTracker.Maximum)
            {
                this.mTracker.Value += 1;
            }
        }

        private void plUp_MouseUp(object sender, MouseEventArgs e)
        {
            _fastMove.Stop();
        }

        private void plDown_MouseDown(object sender, MouseEventArgs e)
        {
            if (!_fastMove.Enabled)
            {
                _fastMove = new Timer();
                _fastMove.Tick += _fastMoveDown_Tick;
                _fastMove.Interval = 100;
                _fastMove.Start();

            }
        }

        private void _fastMoveDown_Tick(object sender, EventArgs e)
        {
            if (this.mTracker.Value > 0)
            {
                this.mTracker.Value -= 1;
            }
        }

        private void plDown_MouseUp(object sender, MouseEventArgs e)
        {
            _fastMove.Stop();
        }

        private void plUp_MouseClick(object sender, MouseEventArgs e)
        {
            
        }
    }
}
