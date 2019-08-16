using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;
using System.Drawing.Text;

namespace Atum.Studio.Controls
{
    internal partial class CircularProgressLabel : Label  
    {
        public CircularProgressLabel()
        {
            InitializeComponent();

            this.Text = "%";
        }

        private void PercLabel_Load(object sender, EventArgs e)
        {

        }

        // Direction
        private enum Direction
        {
            Horizontal,
            Vertical
        }

        // Distance from top-left corner
        private enum Distance
        {
            Near,
            Far
        }

        public override Font Font
        {
            get { return base.Font; }
            set
            {
                base.Font = value;
                GetSizeHorizontal(value);
                GetSizeVertical(value);
            }
        }

        // Vertical margin and height
        public int TextTop { get; set; }
        public int TextBottom { get; set; }

        public int GetTextHeight()
        {
            return TextBottom - TextTop;
        }

        private void GetSizeVertical(Font fnt)
        {
            Dictionary<Distance, int> result = GetFontString(fnt, "0123456789%", Direction.Vertical);

            TextTop = result[Distance.Near];
            TextBottom = result[Distance.Far];
        }

        // Horizontal margin and width            
        public int[] TextLeft
        {
            get { return _textLeft; }
        }
        private readonly int[] _textLeft = new int[102];

        public int[] TextRight
        {
            get { return _textRight; }
        }
        private readonly int[] _textRight = new int[102];

        public int GetTextWidth(int num)
        {
            return TextRight[num] - TextLeft[num];
        }

        private void GetSizeHorizontal(Font fnt)
        {
            Dictionary<Distance, int> digitSingle = GetFontString(fnt, "9", Direction.Horizontal);
            Dictionary<Distance, int> digitDouble = GetFontString(fnt, "99", Direction.Horizontal);
            Dictionary<Distance, int> digitTriple = GetFontString(fnt, "100", Direction.Horizontal);

            for (int i = 0; i <= 101; i++)
            {
                Dictionary<Distance, int> result;

                if (i <= 9) // 0 <= percentage number <= 9
                {
                    result = digitSingle;
                }
                else if (i <= 99) // 10 <= percentage number <= 99
                {
                    result = digitDouble;
                }
                else if (i == 100) // Percentage number = 100
                {
                    result = digitTriple;
                }
                else // Percentage unit
                {
                    result = GetFontString(fnt, this.Text, Direction.Horizontal);
                }

                TextLeft[i] = result[Distance.Near];
                TextRight[i] = result[Distance.Far];
            }
        }

        // Get margin and size of designated font and string.
        private Dictionary<Distance, int> GetFontString(Font fnt, string str, Direction order)
        {
            // Get rectangle size required to draw string.
            Size rectSize = TextRenderer.MeasureText(str, fnt);

            using (Bitmap bmp = new Bitmap(rectSize.Width, rectSize.Height))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                // Draw string (characters).
                g.TextRenderingHint = TextRenderingHint.ClearTypeGridFit;
                g.DrawString(str, fnt, Brushes.Black, Point.Empty); // Any brush color will do.

                // Convert bitmap data to byte array.
                BitmapData bmpData = bmp.LockBits(new Rectangle(Point.Empty, bmp.Size),
                                                  ImageLockMode.ReadOnly,
                                                  PixelFormat.Format32bppRgb);

                byte[] bytBuf = new byte[bmp.Width * bmp.Height * 4]; // 1 pixel corresponds to 4 bytes.
                Marshal.Copy(bmpData.Scan0, bytBuf, 0, bytBuf.Length);

                // Prepare dictionary to hold number of pixel(s) which has a part of character in each line.
                Dictionary<int, int> pxAlpha = new Dictionary<int, int>();

                switch (order)
                {
                    case Direction.Horizontal: // Horizontal margin and width
                        for (int x = 0; x <= bmp.Width - 1; x++)
                        {
                            int pxBuf = 0;

                            for (int y = 0; y <= bmp.Height - 1; y++)
                            {
                                // Check alpha component of pixel to determine if it has a part of character.
                                if (bytBuf[(y * bmp.Width + x) * 4 + 3] != 0) pxBuf++;
                            }

                            pxAlpha.Add(x, pxBuf);
                        }
                        break;

                    case Direction.Vertical: // Vertical margin and width
                        for (int y = 0; y <= bmp.Height - 1; y++)
                        {
                            int pxBuf = 0;

                            for (int x = 0; x <= bmp.Width - 1; x++)
                            {
                                // Check alpha component of pixel to determine if it has a part of character.
                                if (bytBuf[(y * bmp.Width + x) * 4 + 3] != 0) pxBuf++;
                            }

                            pxAlpha.Add(y, pxBuf);
                        }
                        break;
                }

                // Get lines that have parts of characters.
                var lstPxBlack = new List<int>();
                foreach (var pxAlphaDic in pxAlpha)
                {
                    if (0 < pxAlphaDic.Value) {
                        lstPxBlack.Add(pxAlphaDic.Key);
                    }
                }
                int[] pxBlack = lstPxBlack.ToArray(); ;

                // Find the nearest line (left or top).
                int lineNearest = pxBlack[0];

                // Find the farthest line (right or bottom).
                int lineFarthest = pxBlack[pxBlack.Length - 1];

                return new Dictionary<Distance, int>
                {
                    { Distance.Near, lineNearest },
                    { Distance.Far, lineFarthest }
                };
            }
        }
    }
}
