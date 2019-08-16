using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Managers
{
    public class FontManager
    {
        [DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont, IntPtr pdv, [In] ref uint pcFonts);

        internal static bool Loaded { get; set; }

        internal static PrivateFontCollection CustomBoldFonts = new PrivateFontCollection();
        internal static PrivateFontCollection CustomRegularFonts = new PrivateFontCollection();

        internal static FontFamily MontserratBold { get; set; }
        internal static FontFamily MontserratRegular { get; set; }

        internal static Font Montserrat12Bold
        {
            get
            {
                return new Font(MontserratBold, 12, FontStyle.Bold, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat14Bold
        {
            get
            {
                return new Font(MontserratBold, 14, FontStyle.Bold, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat16Bold
        {
            get
            {
                return new Font(MontserratBold, 16, FontStyle.Bold, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat18Bold
        {
            get
            {
                return new Font(MontserratBold, 18, FontStyle.Bold, GraphicsUnit.Pixel);
            }
        }


        internal static Font Montserrat12Regular
        {
            get
            {
                return new Font(MontserratRegular, 12, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat14Regular
        {
            get
            {
                return new Font(MontserratRegular, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat16Regular
        {
            get
            {
                return new Font(MontserratRegular, 16, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat18Regular
        {
            get
            {
                return new Font(MontserratRegular, 18, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }

        internal static Font Montserrat48Regular
        {
            get
            {
                return new Font(MontserratRegular, 48, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }



        public static void LoadDefaultFonts()
        {
            //first add bold values
            foreach (var fontData in (new List<byte[]>(){
                Properties.Resources.Montserrat_Bold}))
            {

                IntPtr data = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, data, (int)fontData.Length);

                CustomBoldFonts.AddMemoryFont(data, (int)fontData.Length);
                // free up the unsafe memory
                Marshal.FreeCoTaskMem(data);
            }

            //second add regular values
            foreach (var fontData in (new List<byte[]>(){
                Properties.Resources.Montserrat_Regular}))
            {

                IntPtr data = Marshal.AllocCoTaskMem(fontData.Length);
                Marshal.Copy(fontData, 0, data, (int)fontData.Length);

                CustomRegularFonts.AddMemoryFont(data, (int)fontData.Length);
                // free up the unsafe memory
                Marshal.FreeCoTaskMem(data);
            }

            MontserratBold = CustomBoldFonts.Families[0];
            MontserratRegular = CustomRegularFonts.Families[0];

            Loaded = true;
        }

    }
}
