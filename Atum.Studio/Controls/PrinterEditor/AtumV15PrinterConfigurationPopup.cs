using Atum.DAL.Hardware;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV15PrinterConfigurationPopup : BasePopup
    {
        public AtumV15Printer DataSource
        {
            get
            {
                return this.atumV15PrinterProperties1.DataSource;
            }
            set{
                value.CreatePreserved();
                this.atumV15PrinterProperties1.DataSource = value;

            }
        }

        public AtumV15PrinterConfigurationPopup()
        {
            InitializeComponent();

            this.Height = this.plContent.Height + this.plFooter.Height;
            this.atumV15PrinterProperties1.inWizard = false;
           
        }

    }
}
