using Atum.Studio.Core.Managers;
using Atum.Studio.Core.PInvoke;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public class GenericEditableLabel16 : Panel
    {
        private bool _isSelected = false;
        private TextBox _valueBox;
        private TextBox _dummyLoseFocus;

        internal bool IsSelected
        {
            get
            {
                return this._isSelected;
            }
        }

        internal new string Text
        {
            get
            {
                return this._valueBox.Text;
            }
            set
            {
                if (this._valueBox.InvokeRequired)
                {
                    this._valueBox.Invoke(new MethodInvoker(delegate
                    {
                        this._valueBox.Text = value;
                    }));
                }
                else
                {
                    this._valueBox.Text = value;
                }
                
            }
        }

        internal int MaxLength
        {
            get
            {
                return this._valueBox.MaxLength;
            }
            set
            {
                this._valueBox.MaxLength = value;
            }
        }

        public GenericEditableLabel16()
        {
            SetStyle(ControlStyles.UserPaint, true);

            this._valueBox = new TextBox();
            if (FontManager.Loaded)
            {
                this._valueBox.Font = FontManager.Montserrat16Regular;
            }
            else
            {
                this._valueBox.Font = new Font(FontFamily.GenericSerif, 16, FontStyle.Regular, GraphicsUnit.Pixel);
            }

            this._valueBox.Top = 10;
            this._valueBox.Left = 8;
            this._valueBox.MouseHover += _valueBox_MouseHover;
            this._valueBox.MouseEnter += _valueBox_MouseEnter;
            this._valueBox.MouseLeave += _valueBox_MouseLeave;
            this._valueBox.LostFocus += _valueBox_LostFocus;
            this._valueBox.BorderStyle = BorderStyle.None;
            this._valueBox.Width = this.Width;

            this._dummyLoseFocus = new TextBox();
            this._dummyLoseFocus.Top = this.Height * 2;

           // _dummyLoseFocus.Visible = false;
            this.Controls.Add(this._valueBox);
            this.Controls.Add(this._dummyLoseFocus);

            this.Paint += new PaintEventHandler(this.GenericEditableLabel18_Paint);
        }

        private void _valueBox_LostFocus(object sender, EventArgs e)
        {
            this._isSelected = false;
            this.UpdateControl();
        }

        private void _valueBox_MouseLeave(object sender, EventArgs e)
        {
            this._isSelected = false;
            this.UpdateControl();
        }

        private void _valueBox_MouseEnter(object sender, EventArgs e)
        {
            this._isSelected = true;
            this.UpdateControl();
        }

        private void _valueBox_MouseHover(object sender, EventArgs e)
        {
            this._isSelected = true;
            this.UpdateControl();
        }

        protected override void OnMouseHover(EventArgs e)
        {
            base.OnMouseHover(e);
            this._isSelected = true;

            this.UpdateControl();
        }
        
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            this._isSelected = true;

            this.UpdateControl();
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            this._isSelected = true;

            this.UpdateControl();
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            this._isSelected = false;

            this.UpdateControl();
        }

        private void UpdateControl()
        {
            if (this._isSelected)
            {
                this._valueBox.Focus();
                this._valueBox.ReadOnly = false;
            }
            else
            {
                this._valueBox.ReadOnly = true;
                this._dummyLoseFocus.Focus();
            }

            this.Invalidate();
        }

        private void GenericEditableLabel18_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;

            if (this._isSelected)
            {
                //draw edit text box
                this._valueBox.BackColor = Color.White;
                graphics.Clear(Color.White);

            }
            else
            {
                this._valueBox.BackColor = Parent.BackColor;
                graphics.Clear(Parent.BackColor);
            }

            //graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.Black), 12, 12);
        }
    }
}
