using Atum.DAL.Hardware;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV10TDPrinterConfigurationPopup : BasePopup
    {
        public AtumV10TDPrinter DataSource
        {
            get
            {
                return this.atumV10TDPrinterConfigurationPanel1.DataSource;
            }
            set{
                this.atumV10TDPrinterConfigurationPanel1.DataSource = value;
            }
        }

        public AtumV10TDPrinterConfigurationPopup()
        {
            InitializeComponent();
        }
    }
}
