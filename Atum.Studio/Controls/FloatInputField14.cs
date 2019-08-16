using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public class FloatInputField14: RichTextBox
    {

        private bool _allowNegativeValue = true;
        private float _defaultValue = 0f;
        private HorizontalAlignment _selectionAlignment = HorizontalAlignment.Right;

        private const int _delayTextChanged = 500;
        private bool _delayTextChangeEnabled;
        private Timer _tmrlastInputChanged = new Timer();

        internal event EventHandler<float> ValueChanged;


        internal bool AllowNegativeValue
        {
            set
            {
                _allowNegativeValue = value;
            }
        }

        internal float DefaultValue
        {
            set
            {
                _defaultValue = value;
            }
        }

        internal HorizontalAlignment HorizontalAlignment
        {
            set
            {
                _selectionAlignment = value;
            }
        }

        public float Value
        {
            get
            {
                var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
                var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
                numberFormat.NumberDecimalSeparator = ".";

                if (this.Text.Length > 0)
                {
                    var result = float.Parse(this.Text.Replace(",", "."), numberFormat);
                    if (result == 0)
                    {
                        return _defaultValue;
                    }
                    else{
                        return result;
                    }
                }
                else
                {
                    return _defaultValue;
                }
            }
            set
            {
                this.Text = Math.Round(value, 2).ToString();

                UpdateControl();
            }
        }

        public FloatInputField14()
        {
            this.SelectAll();
            this.SelectionAlignment = this._selectionAlignment;
            this.DeselectAll();

            this.Multiline = false;
            this.Height = 24;
            
            this.BorderStyle = BorderStyle.None;

            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat14Regular;
            }
        }

       
        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            if (Char.IsDigit(e.KeyChar))
            {

            }
            else if ((e.KeyChar == '-' && _allowNegativeValue) && !this.Text.Contains("-") && this.SelectionStart == 0)
            {

            }
            else if (e.KeyChar == ',' || e.KeyChar == '.')
            {
                if ((!this.Text.Contains(".") && !this.Text.Contains(".")))
                {
                    e.KeyChar = '.';
                }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                e.Handled = true;
            }

            if (!e.Handled)
            {
                float parseResult = 0f;
                if (this.SelectedText.Length > 0) { 
                    if (float.TryParse(this.SelectedText, out parseResult))
                    {
                        var currentSelectionIndex2 = this.SelectionStart;
                        var currentSelectionLength = this.SelectionLength;

                        //replace current selected text to changed text
                        var textEndTextPlaceHolder = string.Empty;
                        if (currentSelectionIndex2 + currentSelectionLength < this.Text.Length)
                        {
                            textEndTextPlaceHolder = this.Text.Substring(currentSelectionIndex2 + currentSelectionLength);
                        }
                        
                        var textBeginTextPlaceHolder = this.Text.Substring(0, currentSelectionIndex2);
                        this.Text = textBeginTextPlaceHolder + e.KeyChar + textEndTextPlaceHolder;
                        this.SelectionStart = currentSelectionIndex2 + 1;

                        e.Handled = true;

                    }
                    else
                    {
                        this.Text = string.Empty;
                    }
                }
                var currentSelectionIndex = this.SelectionStart;
                UpdateControl();
                this.SelectionStart = currentSelectionIndex;
            }

            //base.OnKeyPress(e);
        }

        internal void UpdateControl()
        {
            var currentSelectionIndex = this.SelectionStart;
            this.SelectAll();
            this.SelectionAlignment = this._selectionAlignment;
            this.DeselectAll();
            this.SelectionStart = currentSelectionIndex;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // FloatInputField14
            // 
            this.ResumeLayout(false);

            _tmrlastInputChanged.Interval = _delayTextChanged;

        }

        internal void EnableDelayedValueChanged()
        {
            _delayTextChangeEnabled = true;
            _tmrlastInputChanged.Tick += _tmrlastInputChanged_Tick;
        }

        private void _tmrlastInputChanged_Tick(object sender, EventArgs e)
        {
            _tmrlastInputChanged.Stop();

            ValueChanged?.Invoke(null, this.Value);
        }

        internal void DisableTextChangeTrigger()
        {
            this.TextChanged -= InputField_TextChanged;
        }

        internal void EnableTextChangeTrigger()
        {
            this.TextChanged += InputField_TextChanged;
        }

        private void InputField_TextChanged(object sender, EventArgs e)
        {
            if (_delayTextChangeEnabled)
            {
                _tmrlastInputChanged.Stop();
                _tmrlastInputChanged.Start();
            }
            else
            {
                ValueChanged?.Invoke(null, this.Value);
            }
        }
    }
}
