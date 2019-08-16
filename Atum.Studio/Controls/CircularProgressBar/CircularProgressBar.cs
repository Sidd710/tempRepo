using System;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls
{
    public partial class CirclularProgressBar : Control
    {
        public int OuterRingWidth { get; set; }
        public Color OuterRingBaseColor { get; set; }
        public string TagLine { get; set; }

        internal float _value;
        public float Value
        {
            get
            {
                return this._value;
            }
            set
            {
                this._value = value;
                this.Invalidate();
            }
        }

        public CirclularProgressBar()
        {
            InitializeComponent();

            if (FontManager.Loaded)
            {
                this.lblNum.Font = FontManager.Montserrat48Regular;
                this.lblNum.ForeColor = Color.FromArgb(255, 40, 0);
                this.lblTagLine.Font = FontManager.Montserrat18Regular;
            }
        }

        private Color BaseColor; // BackColor of parent Control or Form

        private bool oncePainted = false; // Whether this Control is painted once

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            // None (To avoid background being painted so as to prevent flickering).
        }

        bool _paintingInProgress = false;
        Bitmap _progressBitmap = null;
        Graphics _graphics = null;

        protected override void OnPaint(PaintEventArgs e)
        {
            if (!_paintingInProgress)
            {
                _paintingInProgress = true;

                //======================================
                // Prepare for initial draw (only once)
                //======================================
                if (!oncePainted)
                {
                    oncePainted = true;

                    BaseColor = (this.Parent ?? this.FindForm()).BackColor;

                    this.lblNum.Location = new Point(this.Width / 2, (this.Height / 2) - this.lblNum.Height + 20);
                    this.lblTagLine.Text = !string.IsNullOrEmpty(this.TagLine) ? this.TagLine : "";
                    this.lblTagLine.Location = new Point(this.Width / 2, (this.Height / 2) + this.lblTagLine.Height);
                }

                //=======================
                // Set percentage number
                //=======================
                this.lblNum.Text = Convert.ToString(this.Value) + "%...";

                //=======================
                // Draw circles and text
                //=======================
                if (this._progressBitmap == null)
                {
                    this._progressBitmap = new Bitmap(this.Width, this.Height);
                    this._graphics = Graphics.FromImage(this._progressBitmap);
                }

                this._graphics.Clear(this.BackColor);
                this._graphics.SmoothingMode = SmoothingMode.AntiAlias;
                this._graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                this._graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                this._graphics.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;

                //---------------------------------------------
                // Base circle (for antialiasing outer circle)
                //---------------------------------------------
                using (SolidBrush brushBase = new SolidBrush(this.BaseColor))
                using (SolidBrush brushOuterBack = new SolidBrush(OuterRingBaseColor))
                {
                    this._graphics.FillEllipse(brushBase, GetSquare(0));
                }

                using (SolidBrush brushBack = new SolidBrush(this.BackColor))
                using (SolidBrush brushOuterBack = new SolidBrush(OuterRingBaseColor))
                using (SolidBrush brushTagLine = new SolidBrush(Color.White))
                using (SolidBrush brushNumFor = new SolidBrush(this.lblNum.ForeColor))
                using (SolidBrush brushFore = new SolidBrush(this.ForeColor))
                {

                    //--------------
                    // Outer circle
                    //--------------
                    this._graphics.FillEllipse(brushOuterBack, Rectangle.Inflate(GetSquare(0), -4, -4));

                    var startPointPosition = new Rectangle();
                    startPointPosition.X = (this.Width / 2) - 3;
                    startPointPosition.Y = 4;
                    startPointPosition.Width = 4;
                    startPointPosition.Height = 4;
                    this._graphics.FillEllipse(brushFore, startPointPosition);
                    //-----
                    // Pie
                    //-----
                    float startAngle = -90.0F; // Starting angle
                    float sweepAngle = Convert.ToSingle(3.6 * this.Value); // Sweeping angle

                    this._graphics.FillPie(brushFore, Rectangle.Inflate(GetSquare(0), -4, -4), startAngle, sweepAngle);

                    //---------------
                    // Center circle
                    //---------------
                    this._graphics.FillEllipse(brushBack, GetSquare(Convert.ToInt32(this.Width - (this.OuterRingWidth))));


                    StringFormat stringFormat = new StringFormat();
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;

                    this._graphics.DrawString(this.lblNum.Text, this.lblNum.Font, brushNumFor, this.lblNum.Location, stringFormat);
                    this._graphics.DrawString(this.lblTagLine.Text, this.lblTagLine.Font, brushTagLine, this.lblTagLine.Location, stringFormat);

                }

                e.Graphics.DrawImage(this._progressBitmap, new PointF());
            }

            _paintingInProgress = false;
        }

        // Get rectangle with designated offset.
        private Rectangle GetSquare(int offset)
        {
            var rectangleDimension = this.Height - offset * 2;
            rectangleDimension = Math.Min(rectangleDimension, this.Width - offset * 2);
            return new Rectangle(offset, offset, rectangleDimension, rectangleDimension);
        }

        private float NormalizeAngle(float angle)
        {
            while (angle >= 360) angle -= 360;
            while (angle < 0) angle += 360;
            return angle;
        }

    }
}
