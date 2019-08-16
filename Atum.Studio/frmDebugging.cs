using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;

namespace Atum.Studio
{
    public partial class frmDebugging : Form
    {
        public frmDebugging()
        {
            InitializeComponent();
        }

        private void frmDebugging_Load(object sender, EventArgs e)
        {
            RemoteLoggingManager.ReadRemoteLog(PrinterManager.DefaultPrinter.Connections[0].IPAddress);
            dataGridView1.DataSource = RemoteLoggingManager.Lines;
            RemoteLoggingManager.Lines.ListChanged += new ListChangedEventHandler(RemoteLoggingManager_ListChanged);
        }

        void RemoteLoggingManager_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (RemoteLoggingManager.LastStreamOffset > 0)
            {
                this.dataGridView1.FirstDisplayedScrollingRowIndex = this.dataGridView1.RowCount -1;
            }
        }
    }
}
