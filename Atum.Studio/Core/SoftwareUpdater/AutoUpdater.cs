
using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using Atum.Studio.Controls.SoftwareUpdater;
using Atum.DAL.Compression.Zip;
using Atum.Studio.Core.Managers;
using Atum.Studio.Controls;

namespace Atum.Studio.Core.SoftwareUpdater
{
    #region The delegate
    public delegate void ShowHandler();
    #endregion

    public class AutoUpdater
    {

        private VersionUpdates _versionUpdates;

        #region The public event
        public event EventHandler NewUpdateAvailable;
        public event ShowHandler OnShow;
        #endregion

        #region The constructor of AutoUpdater
        public AutoUpdater()
        {
        }
        #endregion

        #region The public method
        public void Update()
        {
            try
            {
                this._versionUpdates = new VersionUpdates();
                var serializer = new XmlSerializer(typeof(VersionUpdates));
                var downloadString = DownloadManager.DownloadString(BrandingManager.SoftwareUpdate_Url);

                downloadString = downloadString.Substring(downloadString.IndexOf('<'));
                downloadString = downloadString.Replace("&", "&amp;");
                //Debug.WriteLine(downloadStream);

                if (downloadString != null)
                {
                    using (var xmlReader = new StringReader(downloadString))
                    {
                        this._versionUpdates = (VersionUpdates)serializer.Deserialize(xmlReader);
                    }
                }

                if (this._versionUpdates != null)
                {
                    if (this._versionUpdates[0].Version != Application.ProductVersion.ToString())
                    {
                        this.NewUpdateAvailable?.Invoke(null, null);
                    }
                }
            }
            catch (Exception exc)
            {
                DAL.Managers.LoggingManager.WriteToLog("Software Manager", "Exception", exc);
            }

        }

        internal void ShowUpdateDialog()
        {
            var downloadList = this._versionUpdates[0].FilesToDownload;
            downloadList[0].Version = this._versionUpdates[0].Version;


            DownloadConfirm dc = new DownloadConfirm(downloadList);

            this.OnShow?.Invoke();

            if (DialogResult.OK == dc.ShowDialog())
            {
                var updatePath = DAL.ApplicationSettings.Settings.UpdatePath;

                if (!DAL.Managers.FileSystemManager.HasWriteAccess(updatePath))
                {
                    updatePath = DAL.ApplicationSettings.Settings.RoamingUpdatePath;
                }

                if (!Directory.Exists(updatePath))
                {
                    Directory.CreateDirectory(updatePath);
                }

                StartDownload(downloadList);
            }
        }

        #region The private method

        private void StartDownload(List<DownloadFileInfo> downloadList)
        {
            DownloadProgress dp = new DownloadProgress(downloadList);
            if (dp.ShowDialog() == DialogResult.OK)
            {
                new frmMessageBox(Properties.Settings.Default.SoftwareUpdate_Restart_Text, "Close Operator Station to apply this update.", MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button2);
            }
        }


        #endregion

    }

}
#endregion