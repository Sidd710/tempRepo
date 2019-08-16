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
    public class RoundedLeftButton : Button
    {
        internal bool IsSelected { get; set; }

        GraphicsPath _graphicsPath;
        GraphicsPath _graphicsIconPath;
        int radius = 0;
        
        int Round(float parameter)
        {
            return ((int)(parameter + 0.5F));
        }

        GraphicsPath RoundedLeftRectanglePath(int x, int y, int width, int height, bool close)
        {
            int _radius = 0;
            GraphicsPath path = new GraphicsPath();
            x += 1;
            y += 1;
            width -= 1;
            height -= 1;

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
            path.AddLine(x + width, y + height, (x + _radius), (y + height));
            path.AddArc(x, (y + height - _radius), _radius, _radius, 90.0F, 90.0F);
            path.AddArc(x, y, _radius, _radius, 180.0F, 90.0F);
            path.AddLine(x, y, (x + width), y);
            if (close)path.CloseAllFigures();

            return (path);
        }
        
        public RoundedLeftButton() : base()
        {
            this.SetStyle((ControlStyles.DoubleBuffer | ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint), true);
            this.UpdateStyles();

            FlatAppearance.BorderSize = 0;
            FlatStyle = FlatStyle.Flat;
            this.Cursor = Cursors.Hand;
            this.Margin = new Padding(0, 0, 0, 0);

            base.Layout += RoundedLeftButton_Layout;
        }

        private void RoundedLeftButton_Layout(object sender, LayoutEventArgs e)
        {
            this._graphicsPath = RoundedLeftRectanglePath(0, 0, this.Width, this.Height, true);
        }

        internal GraphicsPath IconGraphicsPath
        {
            set
            {
                this._graphicsIconPath = value;
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


        [Category("Appearance"), Description("Sets/Gets background color when selected"), DefaultValue(typeof(Color), "White"), Bindable(true)]
        private Color _selectedBackgroundColor;
        public Color SelectedBackgroundColor
        {
            get
            {
                return (this._selectedBackgroundColor);
            }
            set
            {
                if (this._selectedBackgroundColor != value)
                {
                    this._selectedBackgroundColor = value;
                    this.Refresh();
                }
            }
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.Clear(this.Parent.BackColor);
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            if (this.IsSelected)
            {
                e.Graphics.FillPath(new SolidBrush(this._selectedBackgroundColor), _graphicsPath);
            }
            else
            {
                e.Graphics.FillPath(new SolidBrush(this.BackColor), _graphicsPath);
            }

            if (this._graphicsIconPath != null)
            {
                if (this.IsSelected)
                {
                    e.Graphics.DrawPath(new Pen(Color.White, 2), _graphicsIconPath);
                }
                else
                {
                    e.Graphics.DrawPath(new Pen(Color.FromArgb(255, 30,33,37), 2), _graphicsIconPath);
                }
            }

            var path = RoundedLeftRectanglePath(0, 0, this.Width, this.Height, false);
            e.Graphics.DrawPath(new Pen(this.Parent.BackColor, 1), path);
            this.Region = new Region(path);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }
    }
}
