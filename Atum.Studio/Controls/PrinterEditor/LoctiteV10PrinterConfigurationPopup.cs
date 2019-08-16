using Atum.DAL.Hardware;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class LoctiteV10PrinterConfigurationPopup : BasePopup
    {
        public LoctiteV10 DataSource
        {
            get
            {
                return this.atum40PrinterProperties1.DataSource;
            }
            set{
                value.CreatePreserved();
                this.atum40PrinterProperties1.DataSource = value;

            }
        }

        public LoctiteV10PrinterConfigurationPopup()
        {
            InitializeComponent();

            this.Height = this.plContent.Height + this.plFooter.Height;
            this.atum40PrinterProperties1.inWizard = false;
           
        }

    }
}
