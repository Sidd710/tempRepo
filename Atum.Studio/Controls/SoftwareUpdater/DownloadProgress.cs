
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Diagnostics;
using System.Xml;
using Atum.Studio.Core.SoftwareUpdater;
using Atum.DAL.Compression.Zip;

namespace Atum.Studio.Controls.SoftwareUpdater
{
    public partial class DownloadProgress : Form
    {
        #region The private fields
        private bool isFinished = false;
        private List<DownloadFileInfo> downloadFileList = null;
        private List<DownloadFileInfo> allFileList = null;
        private ManualResetEvent evtDownload = null;
        private WebClient clientDownload = null;
        #endregion

        #region The constructor of DownloadProgress
        public DownloadProgress(List<DownloadFileInfo> downloadFileListTemp)
        {
            InitializeComponent();

            this.downloadFileList = downloadFileListTemp;
            allFileList = new List<DownloadFileInfo>();
            foreach (DownloadFileInfo file in downloadFileListTemp)
            {
                allFileList.Add(file);
            }
        }
        #endregion

        #region The method and event
        private void OnFormClosing(object sender, FormClosingEventArgs e)
        {
            //if (!isFinished && DialogResult.No == MessageBox.Show(ConstFile.CANCELORNOT, ConstFile.MESSAGETITLE, MessageBoxButtons.YesNo, MessageBoxIcon.Question))
            //{
            //    e.Cancel = true;
            //    return;
            //}
            //else
            //{
            //    if (clientDownload != null)
            //        clientDownload.CancelAsync();

            //    evtDownload.Set();
            //    evtPerDonwload.Set();
            //}
        }

        private string _downloadUpdateFile;

        private void OnFormShown(object sender, EventArgs e)
        {

            evtDownload = new ManualResetEvent(true);
            evtDownload.Reset();
            ThreadPool.QueueUserWorkItem(new WaitCallback(this.ProcDownload));
        }

        private void ProcDownload(object o)
        {
            try
            {
                var updatePath = DAL.ApplicationSettings.Settings.UpdatePath;

                if (!DAL.Managers.FileSystemManager.HasWriteAccess(updatePath))
                {
                    updatePath = DAL.ApplicationSettings.Settings.RoamingUpdatePath;
                }

                this._downloadUpdateFile = Path.Combine(updatePath, "atum3D_OS_" + this.downloadFileList[0].Version + ".msi");
                using (clientDownload = new WebClient())
                {
                    clientDownload.Proxy = WebRequest.DefaultWebProxy;
                    clientDownload.DownloadFile(new Uri(this.downloadFileList[0].DownloadUrl), this._downloadUpdateFile);


                    try
                    {
                        this.SetProcessBar(1, downloadFileList.Count);
                    }
                    catch (Exception)
                    {
                        //log the error message,you can use the application's log code
                    }

                    this.downloadFileList.Clear();
                }
            }

            catch (Exception)
            {
                //ShowErrorAndRestartApplication();
                //throw;
            }

            Debug.WriteLine("All Downloaded");

            //After dealed with all files, clear the data
            this.allFileList.Clear();

            evtDownload.Set();
            Exit(true);
        }


        delegate void ShowCurrentDownloadFileNameCallBack(string name);
        private void ShowCurrentDownloadFileName(string name)
        {
            if (this.labelCurrentItem.InvokeRequired)
            {
                ShowCurrentDownloadFileNameCallBack cb = new ShowCurrentDownloadFileNameCallBack(ShowCurrentDownloadFileName);
                this.Invoke(cb, new object[] { name });
            }
            else
            {
                this.labelCurrentItem.Text = name;
            }
        }

        delegate void SetProcessBarCallBack(int current, int total);
        private void SetProcessBar(int current, int total)
        {
            if (this.progressBarCurrent.InvokeRequired)
            {
                SetProcessBarCallBack cb = new SetProcessBarCallBack(SetProcessBar);
                this.Invoke(cb, new object[] { current, total });
            }
            else
            {
                this.progressBarCurrent.Value = current;
            }
        }

        delegate void ExitCallBack(bool success);
        private void Exit(bool success)
        {
            if (this.InvokeRequired)
            {
                ExitCallBack cb = new ExitCallBack(Exit);
                this.Invoke(cb, new object[] { success });
            }
            else
            {
                this.isFinished = success;
                this.DialogResult = success ? DialogResult.OK : DialogResult.Cancel;
                this.Close();
            }
        }

        private void OnCancel(object sender, EventArgs e)
        {
         
        }
        
        #endregion
    }
}