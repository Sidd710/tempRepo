using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.IO;
using System.Diagnostics;

namespace Atum.Studio.Controls
{
    public partial class PictureboxWithMagnifier : PictureBox
    {
        private frmMagnifier _frmMagnifier;
        private Size _magnifierSize = new Size(192, 120);
        private Point _magnifierPoint = new Point();

        internal event EventHandler ImageChanged;

        public PictureboxWithMagnifier()
        {
            InitializeComponent();

            this.BackgroundImageLayout = ImageLayout.Zoom;
           
			if (Atum.DAL.OS.OSProvider.IsWindows) {
				this.MouseMove += PictureboxWithMagnifier_MouseMove;
				this.MouseLeave += PictureboxWithMagnifier_MouseLeave;
				this.ImageChanged += PictureboxWithMagnifier_ImageChanged;
			}
        }

        internal void HideMagnifier()
        {
            if (this._frmMagnifier != null)
            {
                this._frmMagnifier.Hide();
            }
        }

        void PictureboxWithMagnifier_MouseLeave(object sender, EventArgs e)
        {
            HideMagnifier();
        }

        void PictureboxWithMagnifier_ImageChanged(object sender, EventArgs e)
        {
            if (this._frmMagnifier == null)
            {
                this._frmMagnifier = new frmMagnifier(this.BackgroundImage);
            }
            else
            {
                lock (this._frmMagnifier.Image)
                {
                    this._frmMagnifier.Image = this.BackgroundImage;
                }
            }
        }

        internal void ImageHasChanged()
        {
            this.ImageChanged?.Invoke(null, null);
        }

        void PictureboxWithMagnifier_MouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                this._magnifierPoint = this.PointToClient(Cursor.Position);

                MagnifierRefreshFromBitmap();
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

        }

        internal void MagnifierRefreshFromBitmap()
        {
            Point unscaled_p = new Point();
            if (this.BackgroundImage != null)
            {
                int pictureboxImageWidth = this.BackgroundImage.Width;
                int pictureboxImageHeight = this.BackgroundImage.Height;
                int pictureboxWidth = this.Width;
                int pictureboxHeight = this.Height;

                float imageRatio = pictureboxImageWidth / (float)pictureboxImageHeight; // image W:H ratio
                float containerRatio = pictureboxWidth / (float)pictureboxHeight; // container W:H ratio
                float filler;

                if (imageRatio >= containerRatio)
                {
                    // horizontal image
                    float scaleFactor = pictureboxWidth / (float)pictureboxImageWidth;
                    float scaledHeight = pictureboxImageHeight * scaleFactor;
                    // calculate gap between top of container and top of image
                    filler = Math.Abs(pictureboxHeight - scaledHeight) / 2;
                    unscaled_p.X = (int)(this._magnifierPoint.X / scaleFactor);
                    unscaled_p.Y = (int)((this._magnifierPoint.Y - filler) / scaleFactor);
                }
                else
                {
                    // vertical image
                    float scaleFactor = pictureboxHeight / (float)pictureboxImageHeight;
                    float scaledWidth = pictureboxImageWidth * scaleFactor;
                    filler = Math.Abs(pictureboxWidth - scaledWidth) / 2;
                    unscaled_p.X = (int)((this._magnifierPoint.X - filler) / scaleFactor);
                    unscaled_p.Y = (int)(this._magnifierPoint.Y / scaleFactor);
                }

                var disableMagnifier = false;
                var magnifierSizeWidthScaled = this._magnifierSize.Width / 4;
                var magnifierSizeHeightScaled = this._magnifierSize.Height / 4;

                if ((this._magnifierPoint.X < filler || this._magnifierPoint.X > this.Width - filler) && imageRatio < containerRatio)
                {
                    disableMagnifier = true;
                }
                else if (unscaled_p.X >= this.BackgroundImage.Width - magnifierSizeWidthScaled)
                {
                    unscaled_p.X = this.BackgroundImage.Width - magnifierSizeWidthScaled;
                }
                else if (unscaled_p.X <= (magnifierSizeWidthScaled / 2))
                {
                    unscaled_p.X = (magnifierSizeWidthScaled / 2);
                }

                if ((this._magnifierPoint.Y < filler || this._magnifierPoint.Y > this.Height - filler) && imageRatio >= containerRatio)
                {
                    disableMagnifier = true;
                }
                else if (unscaled_p.Y >= pictureboxImageHeight - magnifierSizeHeightScaled)
                {
                    unscaled_p.Y = pictureboxImageHeight - magnifierSizeHeightScaled;
                }
                else if (unscaled_p.Y < magnifierSizeHeightScaled)
                {
                    unscaled_p.Y = magnifierSizeHeightScaled;
                }


                if (!disableMagnifier)
                {
                    if (this._frmMagnifier.Image != null)
                    {
                        Bitmap magnifierImage = (((Bitmap)this._frmMagnifier.Image).Clone(new Rectangle(unscaled_p.X - (magnifierSizeWidthScaled / 2), unscaled_p.Y - (magnifierSizeHeightScaled / 2), magnifierSizeWidthScaled, magnifierSizeHeightScaled), System.Drawing.Imaging.PixelFormat.Format24bppRgb));

                        if (!this._frmMagnifier.Visible)
                        {
                            this._frmMagnifier.Size = _magnifierSize;
                            this._frmMagnifier.StartPosition = FormStartPosition.Manual;
                            this._frmMagnifier.Show(this);
                            this._frmMagnifier.Location = new Point(0, -3200);
                        }

                        if (Core.Managers.UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode)
                        {
                            this._frmMagnifier.Location = new Point(Cursor.Position.X, Cursor.Position.Y + 10);
                        }
                        else
                        {
                            this._frmMagnifier.Location = new Point(Cursor.Position.X - (this._frmMagnifier.Width / 2), Cursor.Position.Y + 10);
                        }

                        this._frmMagnifier.BackgroundImage = magnifierImage;
                        this._frmMagnifier.Visible = true;
                    }
                }
                else
                {
                    if (this._frmMagnifier.Visible)
                    {
                        this._frmMagnifier.Hide();
                    }
                }
            }
        }

    }

    public partial class frmMagnifier : Form
    {
        private Image _image;

        public Image Image
        {
            get
            {
                return _image;
            }
            set
            {
                this._image = value;
            }
        }

        public frmMagnifier(Image image)
        {
            this.ShowInTaskbar = false;
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.ControlBox = false;
            this.BackgroundImageLayout = ImageLayout.Zoom;
            this.FormBorderStyle = FormBorderStyle.None;
            this.Image = image;
            this.Paint += frmMagnifier_Paint;
        }

        void frmMagnifier_Paint(object sender, PaintEventArgs e)
        {
            var borderColor = Color.FromArgb(255, 255, 24, 0);
            if (Core.Managers.UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode)
            {
                var arrowHeader = new List<PointF>() { new PointF(0, 0), new PointF(10, 0), new PointF(0, 10) };
                e.Graphics.FillPolygon(new SolidBrush(borderColor), arrowHeader.ToArray());

                e.Graphics.DrawRectangle(new Pen(borderColor, 1), 16, 0, 180, 1);
                e.Graphics.DrawRectangle(new Pen(borderColor, 1), 0, 14, 1, 110);
                e.Graphics.DrawRectangle(new Pen(borderColor, 1), 190, 0, 1, 118);
                e.Graphics.DrawRectangle(new Pen(borderColor, 1), 0, 118, 190, 1);
            }
            else
            {
                e.Graphics.DrawRectangle(new Pen(borderColor, 2), 1, 1, 190, 118);
            }
        }
    }
}
