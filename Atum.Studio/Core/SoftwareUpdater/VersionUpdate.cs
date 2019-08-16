using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.SoftwareUpdater
{
    public class VersionUpdate
    {
        public enum typeOfVersionUpdate{
            Stable = 0,
            Beta = 1
        }

        public typeOfVersionUpdate VersionType { get; set; }
        public string Files { get; set; }
        public string Version { get; set; }
        public string ContentText { get; set; }
        public string ContentURL { get; set; }

        internal List<DownloadFileInfo> FilesToDownload
        {
            get
            {
                var files = this.Files.Split(';');
                var downloadFiles = new List<DownloadFileInfo>();
                foreach (var file in files)
                {
                    if (!string.IsNullOrEmpty(file))
                    {
                        downloadFiles.Add(new DownloadFileInfo(file));
                    }
                }
                return downloadFiles;
            }
        }

    }

    public class VersionUpdates: List<VersionUpdate>
    {

    }
}
