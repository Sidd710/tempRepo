﻿using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public class FloatInputField14MM: UserControl
    {
        private Label labelSuffix;
        private FloatInputField14 inputField;

        private const int _delayTextChanged = 400;
        private bool _delayTextChangeEnabled;
        private Timer _tmrlastInputChanged = new Timer();

        internal event EventHandler<float> ValueChanged;

        internal float Value
        {
            get
            {
                var currentCulture = System.Globalization.CultureInfo.InstalledUICulture;
                var numberFormat = (System.Globalization.NumberFormatInfo)currentCulture.NumberFormat.Clone();
                numberFormat.NumberDecimalSeparator = ".";

                if (this.inputField.Text.Length > 0 && this.inputField.Text != "-")
                {
                    return float.Parse(this.inputField.Text.Replace(",", "."), numberFormat);
                }
                else
                {
                    return 0;
                }
            }
            set
            {
                this.inputField.Text = value.ToString();

                this.inputField.SelectAll();
                this.inputField.SelectionAlignment = HorizontalAlignment.Right;
            }

        }

        public FloatInputField14MM()
        {
            InitializeComponent();

            this.Height = 25;
            this.Width = 80;

            this.BackColor = Color.White;
            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);

            if (FontManager.Loaded)
            {
                labelSuffix.Font = FontManager.Montserrat14Regular;
                labelSuffix.ForeColor = System.Drawing.Color.Gray;
            }

            labelSuffix.Width = 12 + TextRenderer.MeasureText(labelSuffix.Text, labelSuffix.Font).Width;

            inputField.Width = this.Width - labelSuffix.Width;
            inputField.TextChanged += InputField_TextChanged;
            inputField.KeyDown += InputField_KeyDown;

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
            inputField.TextChanged -= InputField_TextChanged;
        }

        internal void EnableTextChangeTrigger()
        {
            inputField.TextChanged += InputField_TextChanged;
        }

        private void InputField_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.inputField.SelectAll();
                this.inputField.SelectionAlignment = HorizontalAlignment.Right;
            }
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

        private void LabelSuffix_Click(object sender, EventArgs e)
        {
            this.inputField.Focus();

            this.inputField.SelectAll();
            this.inputField.SelectionAlignment = HorizontalAlignment.Right;
        }

        protected override void OnClick(EventArgs e)
        {
            this.inputField.Focus();

            this.inputField.SelectAll();
            this.inputField.SelectionAlignment = HorizontalAlignment.Right;
        }

        private void InitializeComponent()
        {
            this.labelSuffix = new System.Windows.Forms.Label();
            this.inputField = new Atum.Studio.Controls.FloatInputField14();
            this.SuspendLayout();
            // 
            // labelSuffix
            // 
            this.labelSuffix.BackColor = System.Drawing.Color.White;
            this.labelSuffix.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelSuffix.Location = new System.Drawing.Point(85, 4);
            this.labelSuffix.Name = "labelSuffix";
            this.labelSuffix.Padding = new System.Windows.Forms.Padding(0, 0, 4, 4);
            this.labelSuffix.Size = new System.Drawing.Size(40, 28);
            this.labelSuffix.TabIndex = 0;
            this.labelSuffix.Text = "mm";
            this.labelSuffix.TextAlign = System.Drawing.ContentAlignment.BottomRight;
            this.labelSuffix.Click += new System.EventHandler(this.LabelSuffix_Click);
            // 
            // inputField
            // 
            this.inputField.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.inputField.Dock = System.Windows.Forms.DockStyle.Left;
            this.inputField.Location = new System.Drawing.Point(0, 4);
            this.inputField.Multiline = false;
            this.inputField.Name = "inputField";
            this.inputField.Size = new System.Drawing.Size(100, 28);
            this.inputField.TabIndex = 1;
            this.inputField.Text = "0";
            this.inputField.Leave += new System.EventHandler(this.inputField_Leave);
            // 
            // FloatInputField14MM
            // 
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.inputField);
            this.Controls.Add(this.labelSuffix);
            this.DoubleBuffered = true;
            this.Name = "FloatInputField14MM";
            this.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            this.Size = new System.Drawing.Size(125, 32);
            this.Enter += new System.EventHandler(this.FloatInputFieldMM_Enter);
            this.ResumeLayout(false);

        }

        private void inputField_Leave(object sender, EventArgs e)
        {
            if (this.inputField.Text == string.Empty)
            {
                this.inputField.Text = "0";
            }
        }

        private void FloatInputFieldMM_Enter(object sender, EventArgs e)
        {
            this.inputField.Focus();

            this.inputField.SelectAll();
            this.inputField.SelectionAlignment = HorizontalAlignment.Right;
        }
    }
}