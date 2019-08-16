using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumDLPStation5PrinterConfigurationPopup : BasePopup
    {
        public AtumDLPStation5 DataSource
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

        public AtumDLPStation5PrinterConfigurationPopup()
        {
            InitializeComponent();

            this.Height = this.plContent.Height + this.plFooter.Height;
            this.atum40PrinterProperties1.inWizard = false;

            var dpiScaleFactor = DisplayManager.GetResolutionScaleFactor();
            if (dpiScaleFactor > 1)
            {
                this.Width = (int)(this.Width  * dpiScaleFactor);
                this.Height = (int)(this.Height * dpiScaleFactor);
            }
           
        }

    }
}
