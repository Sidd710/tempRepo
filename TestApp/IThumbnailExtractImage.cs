using System;
using System.Text;
using System.Runtime.InteropServices;

namespace ThumbnailProvider
{
    public enum IEIFLAG
    {
        ASYNC = 0x0001, // ask the extractor if it supports ASYNC extract (free threaded)
        CACHE = 0x0002, // returned from the extractor if it does NOT cache the thumbnail
        ASPECT = 0x0004, // passed to the extractor to beg it to render to the aspect ratio of the supplied rect
        OFFLINE = 0x0008, // if the extractor shouldn't hit the net to get any content neede for the rendering
        GLEAM = 0x0010, // does the image have a gleam ? this will be returned if it does
        SCREEN = 0x0020, // render as if for the screen (this is exlusive with IEIFLAG_ASPECT )
        ORIGSIZE = 0x0040, // render to the approx size passed, but crop if neccessary
        NOSTAMP = 0x0080, // returned from the extractor if it does NOT want an icon stamp on the thumbnail
        NOBORDER = 0x0100, // returned from the extractor if it does NOT want an a border around the thumbnail
        QUALITY = 0x0200 // passed to the Extract method to indicate that a slower, higher quality image is desired, re-compute the thumbnail
    }

    /// <summary>
    /// The SIZE structure specifies the width and height of a rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    public struct SIZE
    {
        /// <summary>
        /// Specifies the rectangle's width. The units depend on which function uses this.
        /// </summary>
        public int cx;

        /// <summary>
        /// Specifies the rectangle's height. The units depend on which function uses this.
        /// </summary>
        public int cy;

        /// <summary>
        /// Simple constructor for SIZE structs.
        /// </summary>
        /// <param name="cx">The initial width of the SIZE structure.</param>
        /// <param name="cy">The initial height of the SIZE structure.</param>
        public SIZE(int cx, int cy)
        {
            this.cx = cx;
            this.cy = cy;
        }
    }

    /// <summary>
    /// Exposes methods that request a thumbnail image from a Shell folder.
    /// </summary>
    [ComImportAttribute()]
    [GuidAttribute("BB2E617C-0920-11d1-9A0B-00C04FC2D6C1")]
    [InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)]
    interface IExtractImage
    {
        /// <summary>
        /// Gets a path to the image that is to be extracted.
        /// </summary>
        /// <param name="pszPathBuffer">The buffer used to return the path description. This value identifies the image so you can avoid loading the same one more than once.</param>
        /// <param name="cch">The size of pszPathBuffer in characters.</param>
        /// <param name="pdwPriority">Not used.</param>
        /// <param name="prgSize">A pointer to a SIZE structure with the desired width and height of the image. Must not be NULL.</param>
        /// <param name="dwRecClrDepth">The recommended color depth in units of bits per pixel. Must not be NULL.</param>
        /// <param name="pdwFlags">Flags that specify how the image is to be handled.</param>
        [PreserveSig]
        long GetLocation(out StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags);

        /// <summary>
        /// Requests an image from an object, such as an item in a Shell folder.
        /// </summary>
        /// <param name="phBmpThumbnail">The buffer to hold the bitmapped image.</param>
        [PreserveSig]
        long Extract(out IntPtr phBmpThumbnail);
    }
}