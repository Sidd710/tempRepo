﻿using Svg;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Controls
{
    public static class SVGParser
    {
        /// <summary>
        /// The maximum image size supported.
        /// </summary>
        public static Size MaximumSize { get; set; }
        /// <summary>
        /// Gets a SvgDocument for manipulation using the path provided.
        /// </summary>
        /// <param name="filePath">The path of the Bitmap image.</param>
        /// <returns>Returns the SVG Document.</returns>
        public static Bitmap GetBitmapFromSVG(byte[] resourceFile, Size imageBoxSize)
        {
            SvgDocument document = SvgDocument.Open<SvgDocument>(new MemoryStream(resourceFile));
            document = OptimizeSize(document, imageBoxSize);

            return document.Draw();
        }

        private static SvgDocument OptimizeSize(SvgDocument document, Size imageBoxSize)
        {
            if (document.Height > imageBoxSize.Height)
            {
                document.Width = (int)((document.Width / (double)document.Height) * imageBoxSize.Height);
                document.Height = imageBoxSize.Height;
            }
            else if (document.Height < imageBoxSize.Height)
            {
                //maintain aspect ratio
                var aspectRatio = (float)imageBoxSize.Height / (float)document.Height;
                document.Height = imageBoxSize.Height;
                document.Width = imageBoxSize.Width * aspectRatio;

            }
            return document;
        }


        /// <summary>
        /// Converts an SVG file to a Bitmap image.
        /// </summary>
        /// <param name="filePath">The full path of the SVG image.</param>
        /// <returns>Returns the converted Bitmap image.</returns>
        //public static Bitmap GetBitmapFromSVG(string filePath)
        //{
        //    SvgDocument document = GetSvgDocument(filePath);

        //    Bitmap bmp = document.Draw();
        //    return bmp;
        //}

        /// <summary>
        /// Gets a SvgDocument for manipulation using the path provided.
        /// </summary>
        /// <param name="filePath">The path of the Bitmap image.</param>
        /// <returns>Returns the SVG Document.</returns>
        //public static SvgDocument GetSvgDocument(string filePath)
        //{
        //    SvgDocument document = SvgDocument.Open(filePath);
        //    return AdjustSize(document);
        //}

        /// <summary>
        /// Makes sure that the image does not exceed the maximum size, while preserving aspect ratio.
        /// </summary>
        /// <param name="document">The SVG document to resize.</param>
        /// <returns>Returns a resized or the original document depending on the document.</returns>
        //private static SvgDocument AdjustSize(SvgDocument document)
        //{
        //    if (document.Height > MaximumSize.Height)
        //    {
        //        document.Width = (int)((document.Width / (double)document.Height) * MaximumSize.Height);
        //        document.Height = MaximumSize.Height;
        //    }
        //    return document;
        //}

    }
}
