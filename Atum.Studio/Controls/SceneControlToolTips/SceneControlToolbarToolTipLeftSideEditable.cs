using Atum.Studio.Controls.NewGui.SliderControl.SliderControlTracker;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.PInvoke;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SceneControlToolTips
{
    public class SceneControlToolbarToolTipLeftSideEditable : SceneControlToolbarToolTipBase
    {
        private string _text;
        private Size _tooltipSize;
        internal int Width { get; set; }
        private SliderControlTracker Tracker { get; set; }
        private SliderControlTracker ParentControl { get; set; }

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


        internal SceneControlToolbarToolTipLeftSideEditable()
        {
            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);
        }

        internal SceneControlToolbarToolTipLeftSideEditable(SliderControlTracker parentControl, SliderControlTracker tracker, string text)
        {
            this.ParentControl = parentControl;
            this.Tracker = tracker;

            this.Popup += new PopupEventHandler(this.OnPopup);
            this.Draw += new DrawToolTipEventHandler(this.OnDraw);
            this.BackColor = Color.FromArgb(255, 240, 240, 240);
            this.ShowAlways = true;

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
            base.DisableShadow(tooltipHandle);

            MoveToButtonLocation(tooltipHandle, e.Bounds);

            Graphics g = e.Graphics;
            g.FillRectangle(new SolidBrush(Color.FromArgb(255, 229, 229, 229)), e.Bounds);
            g.DrawString(Text, base._tooltipFont, Brushes.Black,
                new PointF(e.Bounds.X + 8, e.Bounds.Y + 11)); // top layer
        }

        private void MoveToButtonLocation(IntPtr handle, Rectangle bounds)
        {
            var location = new Point(frmStudioMain.SceneControl.ParentForm.Left + this.ParentControl.Location.X - this.Width - this.ToolTipOffsetX + 4, frmStudioMain.SceneControl.ParentForm.Top + this.ParentControl.Top +  (int)this.Tracker._trackerRect.Y +((int)this.Tracker._trackerRect.Height / 2) + 70 + 23);
            NATIVE.MoveWindow(handle, location.X, location.Y, bounds.Width, bounds.Height, false);
        }

    }
}
