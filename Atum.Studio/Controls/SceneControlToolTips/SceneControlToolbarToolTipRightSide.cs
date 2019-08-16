using Atum.Studio.Core.Managers;
using Atum.Studio.Core.PInvoke;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SceneControlToolTips
{
    public class SceneControlToolbarToolTipRightSide : SceneControlToolbarToolTipBase
    {
        private Size _tooltipSize;
        private string _text;
        private int _tooltipOffsetY;

        private SceneControlToolbars.SceneControlToolbarBase ParentToolbar { get; set; }
        private PictureBox Button { get; set; }
        internal int Width {get; set; }

        internal string Text
        {
            get
            {
                return this._text;
            }
            set
            {
                this._text = value;
                SizeF s = TextRenderer.MeasureText(this._text, base._tooltipFont);
                _tooltipSize = new Size((int)s.Width + 30, 40);
                this.Width = this._tooltipSize.Width;
            }
        }

        internal SceneControlToolbarToolTipRightSide(SceneControlToolbars.SceneControlToolbarBase parentToolbar, PictureBox button, int tooltipOffsetY = 0)
        {
            this.ParentToolbar = parentToolbar;
            this.Button = button;
            this._tooltipOffsetY = tooltipOffsetY;

            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);
        }

        private void OnPopup(object sender, PopupEventArgs e) // use this event to set the size of the tool tip
        {
            if (this.Text != null)
            {
                e.ToolTipSize = this._tooltipSize;
            }
        }

        private void OnDraw(object sender, DrawToolTipEventArgs e) // use this event to customise the tool tip
        {
            var h = this.GetType().GetProperty("Handle", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            var tooltipHandle = (IntPtr)h.GetValue(this);
            DisableShadow(tooltipHandle);
            MoveToButtonLocation(tooltipHandle, e.Bounds);

            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 229, 229, 229)), e.Bounds);
            g.DrawString(Text, base._tooltipFont, Brushes.Black,
                new PointF(e.Bounds.X + 8, e.Bounds.Y + 11)); // top layer
        }

        private void MoveToButtonLocation(IntPtr handle, Rectangle bounds)
        {
            var location = new Point(this.ParentToolbar.ParentForm.Left + this.ParentToolbar.Location.X + this.ParentToolbar.Width + base.ToolTipOffsetX + 14, this.ParentToolbar.ParentForm.Top + this.ParentToolbar.Location.Y + 70 + this.Button.Location.Y + this.Button.Bounds.Height - 3 - 15);
            NATIVE.MoveWindow(handle, location.X, location.Y, bounds.Width, bounds.Height, false);
        }
    }
}
