using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.Network;
using Atum.Studio.Core.Managers;
using Atum.DAL.Managers;
using Atum.DAL.Hardware;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionDiscoveryTabPanel : WizardTabPanel
    {
        private int _currentTick = 0;
        private System.Windows.Forms.Timer _timerProgressBar = new Timer();

        public byte[] ManualIPAddress { get; set; }

        internal Atum.DAL.Hardware.AtumPrinter SelectedPrinter
        {
            get
            {
                return this.dgDiscoveredPrinters.SelectedRows.Count > 0 ? (Atum.DAL.Hardware.AtumPrinter)this.dgDiscoveredPrinters.SelectedRows[0].DataBoundItem : null;
            }
        }

        public PrinterConnectionDiscoveryTabPanel()
        {
            InitializeComponent();
        }

        delegate void SetAvailablePrintersDataSource();
        private void PrinterConnectionDiscoveryTabPanel_Load(object sender, EventArgs e)
        {
            try
            {
                if (this.ManualIPAddress != null)
                {
                    ConnectionManager.AtumFirmwareResults += PrinterFirmwareResults;
                    var bwSendManualIP = new BackgroundWorker();
                    bwSendManualIP.DoWork += bwSendManualIP_DoWork;
                    bwSendManualIP.RunWorkerAsync();
                }
                else
                {
                    BroadcastManager.SendAsBroadcast(BroadcastManager.typeBroadcastAction.GetPrinter, (new byte[0]));
                }
            }
            catch
            {
            }

            this._currentTick = 0;
            this._timerProgressBar.Interval = 1000;
            this._timerProgressBar.Tick += new EventHandler(_timerProgressBar_Tick);
            this._timerProgressBar.Start();


            if (PrinterManager.AvailablePrinters == null) { PrinterManager.AvailablePrinters = new BindingList<DAL.Hardware.AtumPrinter>(); }
            PrinterManager.AvailablePrinters.ListChanged += new ListChangedEventHandler(AvailablePrinters_ListChanged);

            this.UpdateAvailablePrinters();
        }

        void bwSendManualIP_DoWork(object sender, DoWorkEventArgs e)
        {
            ConnectionManager.Send(new Atum.DAL.Hardware.PrinterFirmware(), new System.Net.IPAddress(this.ManualIPAddress), 11000);
        }

        private void PrinterFirmwareResults(PrinterFirmwareResult printerFirmware)
        {
            if (printerFirmware != null)
            {
                PrinterManager.Append(printerFirmware.Printer);
            }
        }

        void AvailablePrinters_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.dgDiscoveredPrinters.InvokeRequired)
            {
                var d = new SetAvailablePrintersDataSource(UpdateAvailablePrinters);
                this.Invoke(d, new object[] { });
            }
            else
            {
                UpdateAvailablePrinters();
            }
        }

        void UpdateAvailablePrinters()
        {
            this.atumV2PrinterBindingSource.DataSource = null;
            this.atumV2PrinterBindingSource.DataSource = PrinterManager.AvailablePrinters;

        }

        void _timerProgressBar_Tick(object sender, EventArgs e)
        {
            this.pgSearchResults.Maximum = 5;
            this.pgSearchResults.Value = this._currentTick++;

            if (this.pgSearchResults.Maximum == this.pgSearchResults.Value)
            {
                this._timerProgressBar.Stop();
            }
        }

      
    }
}
