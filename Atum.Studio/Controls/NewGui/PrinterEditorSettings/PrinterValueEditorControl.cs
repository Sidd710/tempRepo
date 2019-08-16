using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.DAL.Hardware;
using Atum.DAL.ApplicationSettings;
using Atum.Studio.Core.Managers;
using System.IO;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    public partial class PrinterValueEditorControl : UserControl
    {
        public event EventHandler onTextChanged;
        public event EventHandler onSaved;
        private bool isFromControlChange = false;
        private frmPrinterEditor _frmPrinterEditor { get; set; }
        public PrinterValueEditorControl(frmPrinterEditor frmPrinterEditor)
        {
            _frmPrinterEditor = frmPrinterEditor;
            InitializeComponent();

            this.btnCalibrate.BorderColor = this.btnCalibrate.ForeColor = this.btnExportSettings.BorderColor = this.btnExportSettings.ForeColor = BrandingManager.Button_BackgroundColor_Dark; ;
        }

        private AtumPrinter _atumPrinter;
        public AtumPrinter SelectedPrinter
        {
            get
            {
                return this._atumPrinter;
            }
            set
            {
                this._atumPrinter = value;
                this.UpdateControl();
            }
        }
        private void UpdateControl()
        {
            this.isFromControlChange = true;

            if (this._atumPrinter != null)
            {
                this.txtDisplayName.Font = this.txtDescription.Font = FontManager.Montserrat16Regular;
                this.lblResolutionValue.Font = this.lblSerialNumberValue.Font = FontManager.Montserrat14Regular;
                this.txtDisplayName.Text = this._atumPrinter.DisplayName;
                this.txtDescription.Text = this._atumPrinter.Description;
                this.lblResolutionValue.Text = this._atumPrinter.PrinterXYResolutionAsInt.ToString();
                this.lblSerialNumberValue.Text = this._atumPrinter.SerialNumber;
            }
            ChangeDisplayNamePanelBorder();
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            BindingList<AtumPrinter> bindingPrinters = new BindingList<AtumPrinter>();
            //foreach (var SelectedPrinter in PrinterManager.SelectedPrinters)
            //{
            bindingPrinters.Add(SelectedPrinter);
            //}

            using (var saveDialog = new SaveFileDialog())
            {
                saveDialog.AddExtension = true;
                saveDialog.FileName = SelectedPrinter.DisplayName.Replace(" ","") + ".xml" ;
                saveDialog.DefaultExt = ".xml";
                #if LOCTITE
                saveDialog.Filter = "Printer Settings File(*.xml)|*.xml";
                #else
                saveDialog.Filter = "Printer Settings File(*.xml)|*.xml";
                #endif
                if (saveDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
                {
                    var _serializer = new System.Xml.Serialization.XmlSerializer(typeof(BindingList<AtumPrinter>));
                    using (var streamWriter = new System.IO.StreamWriter(saveDialog.FileName, false))
                    {
                        _serializer.Serialize(streamWriter, bindingPrinters);
                        streamWriter.Close();
                    }
                }
            }           
            //PrinterManager.Save();
        }

        private void btnCalibrate_Click(object sender, EventArgs e)
        {
            SceneControlToolbarManager.SelectedPrinter = SelectedPrinter;
            using (var frmCalibratePrinter = new frmCalibratePrinter(SelectedPrinter))
            {
                frmCalibratePrinter.StartPosition = FormStartPosition.CenterParent;
                var dialogResult = frmCalibratePrinter.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    var previousLocation = this._frmPrinterEditor.Location;
                    var centerPointYOfParent = (this._frmPrinterEditor.Height / 2) + this._frmPrinterEditor.Top;
                    var centerPointXOfParent = this._frmPrinterEditor.Left;
                    this._frmPrinterEditor.Location = new Point(-100000, 100000);
                    var frmCalibrationComplete = new frmCalibrationComplete(frmCalibratePrinter);
                    frmCalibrationComplete.StartPosition = FormStartPosition.Manual;
                    frmCalibrationComplete.Top = centerPointYOfParent - (frmCalibrationComplete.Height / 2);
                    frmCalibrationComplete.Left = centerPointXOfParent;
                    frmCalibrationComplete.ShowDialog();
                    this._frmPrinterEditor.Location = previousLocation;
                }
            }
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (!isFromControlChange)
            {
                if (this.SelectedPrinter != null)
                {
                    this.SelectedPrinter.DisplayName = this.txtDisplayName.Text;
                    this.onTextChanged?.Invoke(this, null);
                }
            }
            else
            {
                this.isFromControlChange = false;
            }            
        }
        private void txtDescription_TextChanged(object sender, EventArgs e)
        {
            if (!isFromControlChange)
            {
                this.SelectedPrinter.Description = this.txtDescription.Text;
            }
            else
            {
                this.isFromControlChange = false;
            }
        }


        private void txtDisplayName_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void ChangeDisplayNamePanelBorder()
        {
            if (string.IsNullOrEmpty(txtDisplayName.Text))
            {
                cplDisplayName.BackColor = Color.FromArgb(255, 24, 0);
            }
            else
            {
                cplDisplayName.BackColor = Color.White;
            }
        }

       
        private void txtDescription_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void txtDisplayName_KeyUp(object sender, KeyEventArgs e)
        {
            ChangeDisplayNamePanelBorder();
        }

        private void txtDescription_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtDescription);
        }

        private void txtDisplayName_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtDisplayName);
        }

        private void PrinterValueEditorControl_Load(object sender, EventArgs e)
        {
            this.txtDisplayName.Focus();
        }
    }
}
