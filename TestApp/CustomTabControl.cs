using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms;

namespace TestApp
{
    class CustomTabControl: System.Windows.Forms.TabControl
    {

        public CustomTabControl()
        {
            this.DrawMode = TabDrawMode.OwnerDrawFixed;
this.SetStyle(ControlStyles.UserPaint, true);
this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
this.SetStyle(ControlStyles.DoubleBuffer, true);
this.SetStyle(ControlStyles.ResizeRedraw, true);
this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //	We must always paint the entire area of the tab control
            if (e.ClipRectangle.Equals(this.ClientRectangle))
            {
                //this.CustomPaint(e.Graphics);
                PaintTransparentBackground(e.Graphics, e.ClipRectangle);
            }
            else
            {
                //	it is less intensive to just reinvoke the paint with the whole surface available to draw on.
                this.Invalidate();
            }
        }

        protected void PaintTransparentBackground(Graphics graphics, Rectangle clipRect)
        {

            if ((this.Parent != null))
            {

                //	Set the cliprect to be relative to the parent
                clipRect.Offset(this.Location);

                //	Save the current state before we do anything.
                GraphicsState state = graphics.Save();

                //	Set the graphicsobject to be relative to the parent
                graphics.TranslateTransform((float)-this.Location.X, (float)-this.Location.Y);
                graphics.SmoothingMode = SmoothingMode.HighSpeed;

                //	Paint the parent
                PaintEventArgs e = new PaintEventArgs(graphics, clipRect);
                try
                {
                    this.InvokePaintBackground(this.Parent, e);
                    this.InvokePaint(this.Parent, e);
                }
                finally
                {
                    //	Restore the graphics state and the clipRect to their original locations
                    graphics.Restore(state);
                    clipRect.Offset(-this.Location.X, -this.Location.Y);
                }
            }
        }


        private void DrawTabPage(int index, Graphics graphics)
        {
            graphics.SmoothingMode = SmoothingMode.HighSpeed;

            //	Get TabPageBorder
            using (GraphicsPath tabPageBorderPath = this.GetTabPageBorder(index))
            {

                //	Paint the background
                //using (Brush fillBrush = this._StyleProvider.GetPageBackgroundBrush(index))
               /// {
               //     graphics.FillPath(fillBrush, tabPageBorderPath);
               // }

                //if (this._Style != TabStyle.None)
                //{

                    //	Paint the tab
                 //   this._StyleProvider.PaintTab(index, graphics);

                    //	Draw any image
                 //   this.DrawTabImage(index, graphics);

                    //	Draw the text
                   // this.DrawTabText(index, graphics);

                //}

                //	Paint the border
                //this.DrawTabBorder(tabPageBorderPath, index, graphics);

            }
        }

        private GraphicsPath GetTabPageBorder(int index)
        {

            GraphicsPath path = new GraphicsPath();
            Rectangle pageBounds = this.GetPageBounds(index);
            //Rectangle tabBounds = this.TabPages[0].index);
            //this._StyleProvider.AddTabBorder(path, tabBounds);
            //this.AddPageBorder(path, pageBounds, tabBounds);

            path.CloseFigure();
            return path;
        }

        public Rectangle GetPageBounds(int index)
        {
            Rectangle pageBounds = this.TabPages[index].Bounds;
            pageBounds.Width += 1;
            pageBounds.Height += 1;
            pageBounds.X -= 1;
            pageBounds.Y -= 1;

            if (pageBounds.Bottom > this.Height - 4)
            {
                pageBounds.Height -= (pageBounds.Bottom - this.Height + 4);
            }
            return pageBounds;
        }

    }
}
