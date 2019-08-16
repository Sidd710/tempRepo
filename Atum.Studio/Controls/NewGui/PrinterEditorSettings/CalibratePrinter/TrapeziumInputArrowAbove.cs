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

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter
{
    public partial class TrapeziumInputArrowAbove : UserControl
    {
        internal event EventHandler TextValueChanged;

        private float _textValue;
        public float TextValue
        {
            get
            {
                return this._textValue;
            }
            set
            {
                this._textValue = value;

                this.txtInputValue.TextChanged -= txtInputValue_TextChanged;
                this.txtInputValue.Text = this._textValue.ToString().Replace(",", ".");
                this.txtInputValue.TextChanged += txtInputValue_TextChanged;
            }
        }

        public TrapeziumInputArrowAbove()
        {
            InitializeComponent();

            if (FontManager.Loaded)
            {
                this.label1.Font = FontManager.Montserrat14Regular;
                this.label1.Width = TextRenderer.MeasureText(this.label1.Text, this.label1.Font).Width + this.label1.Padding.Left + this.label1.Padding.Right + 2;
                this.label1.Left = this.Width - this.label1.Width + 2;
                this.txtInputValue.Font = FontManager.Montserrat14Regular;
                this.txtInputValue.Left = this.label1.Left - this.txtInputValue.Width;
            }
        }

        public void SetValidationColor(Color color)
        {
            this.plValidationColor.BackColor = color;
        }

        public void ResetValidationColor()
        {
            this.plValidationColor.BackColor = Color.White;
        }

        private void txtInputValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.') && (e.KeyChar != ','))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
            else if ((e.KeyChar == ',') && ((sender as TextBox).Text.IndexOf(',') > -1))
            {
                e.Handled = true;
            }
        }

        private void txtInputValue_TextChanged(object sender, EventArgs e)
        {
            if (this.txtInputValue.Text != string.Empty)
            {
                var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
                var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
                numberFormat.NumberDecimalSeparator = ".";

                this._textValue = float.Parse(this.txtInputValue.Text.Replace(",", "."), numberFormat);
                this.TextValueChanged?.Invoke(null, null);
            }
        }

        private void txtInputValue_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtInputValue);
        }
    }
}
