using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class frmMessageBox : NewGui.NewGUIFormBase
    {
        private MessageBoxButtons _buttonStyle;
        private MessageBoxDefaultButton _buttonSelected;

        public frmMessageBox()
        {
            InitializeComponent();
        }
        public frmMessageBox(string title, string content, MessageBoxButtons buttonStyle, MessageBoxDefaultButton defaultButton)
        {
            InitializeComponent();

            this._buttonStyle = buttonStyle;
            this.KeyPreview = true;

            if (buttonStyle == MessageBoxButtons.OK)
            {
                this.btnLeft.Visible = this.btnRight.Visible = false;
                this.btnMiddle.ForeColor = Color.White;
                this.btnMiddle.BackColor = Color.FromArgb(255, 30, 33, 37);
                this.btnMiddle.Select();
                _buttonSelected = MessageBoxDefaultButton.Button2;
            }
            else if (buttonStyle == MessageBoxButtons.YesNo)
            {
                this.btnMiddle.Visible = false;
                this.btnLeft.Text = "Yes";
                this.btnRight.Text = "No";

                if (defaultButton == MessageBoxDefaultButton.Button1)
                {
                    this.btnLeft.ForeColor = Color.White;
                    this.btnLeft.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnRight.BorderColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnLeft.Select();
                    
                    _buttonSelected = MessageBoxDefaultButton.Button1;
                }
                else if (defaultButton == MessageBoxDefaultButton.Button3)
                {
                    this.btnRight.ForeColor = Color.White;
                    this.btnRight.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnLeft.BorderColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnRight.Select();

                    _buttonSelected = MessageBoxDefaultButton.Button3;
                }
            }
            else if (buttonStyle == MessageBoxButtons.YesNoCancel)
            {
                this.btnLeft.Text = "Yes";
                this.btnRight.Text = "No";
                this.btnMiddle.Text = "Cancel";

                if (defaultButton == MessageBoxDefaultButton.Button1)
                {
                    this.btnLeft.ForeColor = Color.White;
                    this.btnLeft.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnMiddle.BorderColor = this.btnRight.BorderColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnLeft.Select();
                    this.btnLeft.Focus();

                    _buttonSelected = MessageBoxDefaultButton.Button1;
                }
                else if (defaultButton == MessageBoxDefaultButton.Button2)
                {
                    this.btnMiddle.Width = 120;
                    this.btnMiddle.ForeColor = Color.White;
                    this.btnMiddle.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnLeft.BorderColor = this.btnRight.BorderColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnMiddle.Select();
                    this.btnMiddle.Focus();

                    _buttonSelected = MessageBoxDefaultButton.Button2;
                }
                else if (defaultButton == MessageBoxDefaultButton.Button3)
                {
                    this.btnRight.ForeColor = Color.White;
                    this.btnRight.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnLeft.BorderColor = this.btnMiddle.BorderColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnRight.Select();
                    this.btnRight.Focus();

                    _buttonSelected = MessageBoxDefaultButton.Button3;
                }

            }
            
            this.Text = title;
            this.label1.Text = content;
            if (FontManager.Loaded)
            {
                this.label1.Font = FontManager.Montserrat14Regular;
                var measureText = TextRenderer.MeasureText(this.label1.Text, this.label1.Font);
                this.Height = measureText.Height + 32 + 32 + this.btnLeft.Height + 32 + 40;
                this.Width = measureText.Width < 300 ? 300 : measureText.Width + 32 + 32;

                if (buttonStyle == MessageBoxButtons.YesNoCancel)
                {
                    var buttonsWidth = 16 + this.btnLeft.Width + 16 + this.btnMiddle.Width + 16 + this.btnRight.Width + 16;
                    if (buttonsWidth > this.Width)
                    {
                        this.Width = buttonsWidth;
                    }
                }

                this.label1.AutoSize = false;
                this.label1.Dock = DockStyle.Top;
                this.label1.Height = this.btnLeft.Top - this.label1.Top;

                UpdateControl();
            }
        }

        private void frmMessageBox_Resize(object sender, EventArgs e)
        {
            UpdateControl();
        }

        private void UpdateControl()
        {
            this.btnLeft.Top = this.btnMiddle.Top = this.btnRight.Top = this.plContent.Height - 16 - this.btnLeft.Height;

            if (this._buttonStyle != MessageBoxButtons.YesNoCancel)
            {
                this.btnMiddle.Left = (this.plContent.Width / 2) - (this.btnMiddle.Width / 2) + 11;
                this.btnRight.Left = (this.plContent.Width - this.btnRight.Width - 16);
                this.btnLeft.Left = this.btnRight.Left - 16 - this.btnLeft.Width;
            }
            else
            {
                this.btnLeft.Left = 16;
                this.btnMiddle.Left = (this.plContent.Width / 2) - (this.btnMiddle.Width / 2);
                this.btnRight.Left = (this.plContent.Width - this.btnRight.Width - 16);
            }

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Yes;
            this.Close();
        }

        private void btnMiddle_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.No;
            this.Close();
        }

        private void plContent_Paint(object sender, PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.DrawRectangle(new Pen(Color.FromArgb(255, 57, 64, 69), 1), 0, -1, this.plContent.Width - 1, this.plContent.Height);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {

            if (e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
            {
                UpdateButtonColors(e);
            }
            else
            {
                base.OnKeyUp(e);
            }
        }

        private void UpdateButtonColors(KeyEventArgs e)
        {
            this.btnLeft.ForeColor = this.btnMiddle.ForeColor = this.btnRight.ForeColor = Color.Black;
            this.btnLeft.BorderColor = this.btnMiddle.BorderColor = this.btnRight.BorderColor = Color.FromArgb(255, 30, 33, 37);
            this.btnLeft.BackColor = this.btnMiddle.BackColor = this.btnRight.BackColor = Color.WhiteSmoke;

            if (this._buttonStyle == MessageBoxButtons.YesNoCancel)
            {
                if (e.KeyCode == Keys.Left)
                {
                    if (this.btnMiddle.Visible && _buttonSelected == MessageBoxDefaultButton.Button2)
                    {
                        this.btnLeft.BorderColor = Color.WhiteSmoke;
                        this.btnLeft.BackColor = Color.FromArgb(255, 30, 33, 37);
                        this.btnLeft.ForeColor = Color.White;

                        _buttonSelected = MessageBoxDefaultButton.Button1;

                        e.Handled = true;

                    }
                    else if (_buttonSelected == MessageBoxDefaultButton.Button1)
                    {
                        this.btnRight.BorderColor = Color.WhiteSmoke;
                        this.btnRight.BackColor = Color.FromArgb(255, 30, 33, 37);
                        this.btnRight.ForeColor = Color.White;

                        _buttonSelected = MessageBoxDefaultButton.Button3;

                        e.Handled = true;
                    }
                    else if (_buttonSelected == MessageBoxDefaultButton.Button3)
                    {
                        this.btnMiddle.BorderColor = Color.WhiteSmoke;
                        this.btnMiddle.BackColor = Color.FromArgb(255, 30, 33, 37);
                        this.btnMiddle.ForeColor = Color.White;

                        _buttonSelected = MessageBoxDefaultButton.Button2;

                        e.Handled = true;
                    }
                }
                else
                {
                        if (this.btnMiddle.Visible && _buttonSelected == MessageBoxDefaultButton.Button2)
                        {
                            this.btnRight.BorderColor = Color.WhiteSmoke;
                            this.btnRight.BackColor = Color.FromArgb(255, 30, 33, 37);
                            this.btnRight.ForeColor = Color.White;

                            _buttonSelected = MessageBoxDefaultButton.Button3;

                            e.Handled = true;

                        }
                        else if (_buttonSelected == MessageBoxDefaultButton.Button1)
                        {
                            this.btnMiddle.BorderColor = Color.WhiteSmoke;
                            this.btnMiddle.BackColor = Color.FromArgb(255, 30, 33, 37);
                            this.btnMiddle.ForeColor = Color.White;

                            _buttonSelected = MessageBoxDefaultButton.Button2;

                            e.Handled = true;
                        }
                        else if (_buttonSelected == MessageBoxDefaultButton.Button3)
                        {
                            this.btnLeft.BorderColor = Color.WhiteSmoke;
                            this.btnLeft.BackColor = Color.FromArgb(255, 30, 33, 37);
                            this.btnLeft.ForeColor = Color.White;

                            _buttonSelected = MessageBoxDefaultButton.Button1;

                            e.Handled = true;
                        }
                }
            }
            else
            {
                if (_buttonSelected == MessageBoxDefaultButton.Button3)
                {
                    this.btnLeft.BorderColor = Color.WhiteSmoke;
                    this.btnLeft.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnLeft.ForeColor = Color.White;

                    _buttonSelected = MessageBoxDefaultButton.Button1;

                    e.Handled = true;
                }
                else
                {
                    this.btnRight.BorderColor = Color.WhiteSmoke;
                    this.btnRight.BackColor = Color.FromArgb(255, 30, 33, 37);
                    this.btnRight.ForeColor = Color.White;

                    _buttonSelected = MessageBoxDefaultButton.Button3;

                    e.Handled = true;
                }
            }
        }
    }
}
