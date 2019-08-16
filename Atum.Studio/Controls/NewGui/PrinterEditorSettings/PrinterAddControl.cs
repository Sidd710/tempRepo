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
using static Atum.DAL.Hardware.AtumPrinter;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    public partial class PrinterAddControl : UserControl
    {
        public event EventHandler onSaved;
        public event EventHandler onCancel;
        public List<PrinterResolution> PrinterResolutions { get; set; }

        private AtumPrinter _newPrinter;
        public PrinterAddControl()
        {
            InitializeComponent();

            BindData();
            IsValidState();
            ChangeDisplayNamePanelBorder();
            ChangeSerialNumberPanelBorder();

            this.btnCancel.BorderColor = this.btnCancel.ForeColor = BrandingManager.Button_BackgroundColor_Dark;
        }

        public PrinterAddControl(AtumPrinter atumPrinter)
        {
            InitializeComponent();

            BindData();
            IsValidState();
            ChangeDisplayNamePanelBorder();
            ChangeSerialNumberPanelBorder();

            this.btnCancel.BorderColor = this.btnCancel.ForeColor = BrandingManager.Button_BackgroundColor_Dark;

            this._newPrinter = atumPrinter;
        }


        private void BindData()
        {
            PrinterResolutions = new List<PrinterResolution>();
            PrinterResolutions.Add(new PrinterResolution() { Text = "50", Value = PrinterXYResolutionType.Micron50 });
            PrinterResolutions.Add(new PrinterResolution() { Text = "100", Value = PrinterXYResolutionType.Micron100 });

            try
            {


                cbResolution.Items.Clear();
                //foreach (var printerResolution in PrinterResolutions)
                //{
                //    cbResolution.Items.Add(printerResolution);
                //}
                this.cbResolution.DataSource = PrinterResolutions;
                cbResolution.DisplayMember = "Text";
                cbResolution.ValueMember = "Value";
            }
            catch (Exception ex)
            {

                //  throw;
            }
            cbResolution.SelectedIndex = 1;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.onCancel?.Invoke(this, null);
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (this._newPrinter == null)
            {
                this._newPrinter = new AtumDLPStation5();
            }

            this._newPrinter.SerialNumber = this.txtSerialNumber.Text;
            this._newPrinter.DisplayName = this.txtDisplayName.Text;
            this._newPrinter.Description = this.txtDescription.Text;
            this._newPrinter.SetDefaultPrinterResolution((PrinterXYResolutionType)cbResolution.SelectedValue);

            if (this._newPrinter is AtumDLPStation5)
            {
                (this._newPrinter as AtumDLPStation5).CalcDefaultTrapezoidValues();
            }
            else
            {
                (this._newPrinter as AtumV15Printer).CalcDefaultTrapezoidValues();
            }
            
            if (string.IsNullOrEmpty(this._newPrinter.ID))
            {
                this._newPrinter.ID = Guid.NewGuid().ToString().ToUpper();
            }

            this.onSaved?.Invoke(this._newPrinter, null);
        }
        private bool IsValidState()
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(txtDisplayName.Text))
            {
                isValid = false;
                lblDisplayNameAsterisk.Visible = true;
            }
            else
            {
                lblDisplayNameAsterisk.Visible = false;
            }

            if (cbResolution.SelectedIndex < 0)
            {
                isValid = false;
                lblResolutionAsterisk.Visible = true;
            }
            else
            {
                lblResolutionAsterisk.Visible = false;
            }

            if (string.IsNullOrEmpty(txtSerialNumber.Text))
            {
                isValid = false;
                lblSerialAsterisk.Visible = true;
            }
            else
            {
                lblSerialAsterisk.Visible = false;
            }

            if (isValid)
            {
                btnSave.Enabled = true;
            }
            else
            {
                btnSave.Enabled = false;
                btnSave.BackColor = BrandingManager.Button_BackgroundColor_Disabled;
            }
            return isValid;
        }

        private void txtSerialNumber_KeyUp(object sender, KeyEventArgs e)
        {
            ChangeSerialNumberPanelBorder();
            IsValidState();
        }

        private void txtDisplayName_KeyUp(object sender, KeyEventArgs e)
        {
            ChangeDisplayNamePanelBorder();
            IsValidState();
        }
        private void ChangeDisplayNamePanelBorder()
        {
            if (string.IsNullOrEmpty(txtDisplayName.Text))
            {
                cplDisplayName.BackColor = BrandingManager.Textbox_HighlightColor_Error;
            }
            else
            {
                cplDisplayName.BackColor = Color.White;
            }
        }
        private void ChangeSerialNumberPanelBorder()
        {
            if (string.IsNullOrEmpty(txtSerialNumber.Text))
            {
                cplSerialNumber.BackColor = BrandingManager.Textbox_HighlightColor_Error;
            }
            else
            {
                cplSerialNumber.BackColor = Color.White;
            }
        }

        private void cbResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            IsValidState();
        }

        private void btnSave_EnabledChanged(object sender, EventArgs e)
        {
            btnSave.ForeColor = BrandingManager.Button_ForeColor;
            btnSave.BackColor = btnSave.Enabled == false ? BrandingManager.Button_BackgroundColor_Disabled: BrandingManager.Button_BackgroundColor_Dark;
        }

        private void cbResolution_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                // Draw the background of the ListBox control for each item.
                e.DrawBackground();
                // Define the default color of the brush as black.
                Brush myBrush = Brushes.Black;

                // Draw the current item text based on the current Font 
                // and the custom brush settings.
                e.Graphics.DrawString(cbResolution.Items[e.Index].ToString(), e.Font, myBrush, e.Bounds, StringFormat.GenericDefault);
                // If the ListBox has focus, draw a focus rectangle around the selected item.
                e.DrawFocusRectangle();
            }
            catch
            {

            }
        }

        private void cplDisplayName_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.cplDisplayName);
        }

        private void txtDescription_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtDescription);
        }

        private void txtSerialNumber_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtSerialNumber);
        }

        private void PrinterAddControl_Load(object sender, EventArgs e)
        {
            this.txtDisplayName.Select();
            this.txtDisplayName.Focus();
        }
    }
}
