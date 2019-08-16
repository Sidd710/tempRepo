using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public class RoundedButton : Button
    {
        Color border_color = Color.White;
        int border_thickness = 1;
        int radius = 0;

        int Round(float parameter)
        {
            return ((int)(parameter + 0.5F));
        }

        GraphicsPath RoundedRectanglePath(int x, int y, int width, int height)
        {
            int _radius = 0;
            GraphicsPath path = new GraphicsPath();
            x += 1;
            y += 1;

            if (this.BackColor == this.Parent.BackColor || this.BackColor == Color.White || this.SingleBorder)
            {
                width -= 3;
                height -= 3;
            }
            else
            {
                width -= 1;
                height -= 1;
            }

            if (Radius == 0)
            {
                _radius = Math.Min(width, height);
                _radius = Round((float)_radius / 2.0F);
            }
            else
            {
                _radius = Radius * 2;
            }

            path.StartFigure();
            path.AddArc((x + width - _radius), (y + height - _radius), _radius, _radius, 0.0F, 90.0F);
            path.AddArc(x, (y + height - _radius), _radius, _radius, 90.0F, 90.0F);
            path.AddArc(x, y, _radius, _radius, 180.0F, 90.0F);
            path.AddArc((x + width - _radius), y, _radius, _radius, 270.0F, 90.0F);
            path.CloseAllFigures();

            return (path);
        }

        public RoundedButton() : base()
        {
            this.SetStyle((ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint), true);
            this.UpdateStyles();

            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.MouseOverBackColor = this.BackColor;
            this.Cursor = Cursors.Hand;
            this.Margin = new Padding(0, 0, 0, 0);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
        }

        [Category("Appearance"), Description("Sets/Gets color of the button border"), DefaultValue(typeof(Color), "White"), Bindable(true)]
        public Color BorderColor
        {
            get
            {
                return (border_color);
            }
            set
            {
                if (border_color != value)
                {
                    border_color = value;
                    this.Refresh();
                }
            }
        }

        [Category("Appearance"), Description("Sets/Gets border style of button border"), DefaultValue(typeof(bool), "1") , Bindable(true)]
        private bool _singleBorder;
        public bool SingleBorder
        {
            get
            {
                return (this._singleBorder);
            }

            set
            {
                if (this._singleBorder != value)
                {
                    this._singleBorder = value;
                    this.Refresh();
                }
            }
        }

        [Category("Appearance"), Description("Sets/Gets thickness of button border"), DefaultValue(typeof(int), "1"), Bindable(true)]
        public int BorderThickness
        {
            get
            {
                return (border_thickness);
            }

            set
            {
                if (border_thickness != value)
                {
                    border_thickness = value;
                    this.Refresh();
                }
            }
        }
        [Category("Appearance"), Description("Sets/Gets radius for rounding corners"), DefaultValue(typeof(int), "0"), Bindable(true)]
        public int Radius
        {
            get
            {
                return (radius);
            }
            set
            {
                if (radius != value)
                {
                    radius = value;
                    this.Refresh();
                }
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (FontManager.Loaded && this.Font.FontFamily.Name != "Montserrat")
            {
                this.Font = FontManager.Montserrat14Regular;
            }

            base.OnPaint(e);
            e.Graphics.Clear(this.Parent.BackColor);
            GraphicsPath path = RoundedRectanglePath(0, 0, this.Width, this.Height);

            // this.Region = new Region(path);
            StringFormat stringFormat = new StringFormat();
            stringFormat.Alignment = StringAlignment.Center;
            stringFormat.LineAlignment = StringAlignment.Center;
            
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.FillPath(new SolidBrush(this.BackColor), path);
            e.Graphics.DrawPath(new Pen(BorderColor, BorderThickness), path);

            var width = this.Width -1;
            var x = 0;
            var y = 0;
            var height = this.Height - 1;
           
            if (!this.SingleBorder)
            {
                //width -= 2;
                height += 1;
                x = 2;
                if (this.Height <= 26)
                {
                    y = 1;
                }
            }

            e.Graphics.DrawString(this.Text, this.Font, new SolidBrush(this.ForeColor), new Rectangle(x,y, width, height), stringFormat);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
