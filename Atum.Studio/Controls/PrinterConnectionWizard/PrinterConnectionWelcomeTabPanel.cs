using System;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionWelcomeTabPanel : WizardTabPanel
    {
        public bool ManualUSBPrinter
        {
            get
            {
                return this.rdAddUSBPrinter.Checked;
            }
        }

        public bool ManualNetworkEntryPrinter
        {
            get
            {
                return this.rdAddManualPrinter.Checked;
            }
        }

        public bool NetworkDiscoveryPrinter
        {
            get
            {
                return this.rdAddNetworkPrinterByDiscovery.Checked;
            }
        }

        public byte[] ManualNetworkIPAddress
        {
            get
            {
                return this.txtIPAddress.GetAddressBytes();
            }
        }

        public DAL.Hardware.AtumPrinter ManualUSBPrinterType
        {
            get
            {

#if LOCTITE
                return new DAL.Hardware.LoctiteV10();
#else
                if (this.cbPrinterType.SelectedIndex == 0)
                {
                    return new DAL.Hardware.AtumV15Printer();
                }
                else if (this.cbPrinterType.SelectedIndex == 1)
                {
                    return new DAL.Hardware.AtumV20Printer();
                }
                else
                {
                    return new DAL.Hardware.AtumDLPStation5();
                }
#endif
            }

        }

        public PrinterConnectionWelcomeTabPanel()
        {
            InitializeComponent();

            this.cbPrinterType.Left = this.label1.Width + this.label1.Left;
#if LOCTITE
            this.cbPrinterType.Items.Clear();
            this.cbPrinterType.Items.Add("Loctite V10");
            this.cbPrinterType.SelectedIndex = 0;
#else
            this.cbPrinterType.SelectedIndex = 2;
#endif
        }

        private void rdAddManualPrinter_CheckedChanged(object sender, EventArgs e)
        {
            this.txtIPAddress.Enabled = this.rdAddManualPrinter.Checked;
            this.cbPrinterType.Enabled = this.rdAddUSBPrinter.Checked;
        }
    }
}
