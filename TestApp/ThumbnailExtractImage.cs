using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Drawing;

namespace ThumbnailProvider
{
    [ComVisible(true), ClassInterface(ClassInterfaceType.None)]
    [ProgId("IExtractImage_Example.ExtractImage"), Guid("7CA3151C-2F5C-11E0-B2B8-B039E0D72085")]
    public class ExtractImage : IExtractImage, IPersistFile
    {
        #region ExtractImage Private Fields

        private Size m_size = Size.Empty;
        private string m_filename = String.Empty;

        #endregion

        private const long S_OK = 0x00000000L;
        private const long E_PENDING = 0x8000000AL;

        #region IExtractImage Members
        public long GetLocation(out StringBuilder pszPathBuffer, int cch, ref int pdwPriority, ref SIZE prgSize, int dwRecClrDepth, ref int pdwFlags)
        {
            pszPathBuffer = new StringBuilder();
            pszPathBuffer.Append(m_filename);
            m_size = new Size(prgSize.cx, prgSize.cy);

            if (((IEIFLAG)pdwFlags & IEIFLAG.ASYNC) != 0)
                return E_PENDING;

            return S_OK;
        }

        public long Extract(out IntPtr phBmpThumbnail)
        {
            Bitmap bmp = new Bitmap(m_size.Width, m_size.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (Pen p = new Pen(Color.Black))
                {
                    g.Clear(Color.White);
                    g.DrawLine(p, 0, 0, m_size.Width, m_size.Height);
                }
            }
            phBmpThumbnail = bmp.GetHbitmap();

            return S_OK;
        }
        #endregion

        #region IPersistFile Members
        public void GetClassID(out Guid pClassID)
        {
            throw new NotImplementedException();
        }

        public void GetCurFile(out string ppszFileName)
        {
            throw new NotImplementedException();
        }

        public int IsDirty()
        {
            throw new NotImplementedException();
        }

        public void Load(string pszFileName, int dwMode)
        {
            m_filename = pszFileName;
        }

        public void Save(string pszFileName, bool fRemember)
        {
            throw new NotImplementedException();
        }

        public void SaveCompleted(string pszFileName)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}