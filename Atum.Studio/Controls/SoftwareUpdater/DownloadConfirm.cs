
using Atum.Studio.Core.SoftwareUpdater;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SoftwareUpdater
{
    public partial class DownloadConfirm : Form
    {
        #region The private fields
        List<DownloadFileInfo> downloadFileList = null;
        #endregion

        #region The constructor of DownloadConfirm
        public DownloadConfirm(List<DownloadFileInfo> downloadfileList)
        {
            InitializeComponent();

            downloadFileList = downloadfileList;
        }
        #endregion

        #region The private method
        private void OnLoad(object sender, EventArgs e)
        {
           
            this.Activate();
            this.Focus();
        }
        #endregion
    }
}