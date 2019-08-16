using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace Atum.Studio.Core.SoftwareUpdater
{
    public class DownloadFileInfo
    {
        #region The private fields
        string downloadUrl = string.Empty;
        public string Version = string.Empty;
        #endregion

        #region The public property
        public string DownloadUrl { get { return downloadUrl; } }
        #endregion

        #region The constructor of DownloadFileInfo
        public DownloadFileInfo() { }

        public DownloadFileInfo(string url)
        {
            this.downloadUrl = url;
        }
        #endregion
    }
}
