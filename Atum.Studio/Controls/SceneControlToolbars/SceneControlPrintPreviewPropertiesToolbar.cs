using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.OpenGL
{
    internal partial class SceneControlPrintPreviewPropertiesToolbar : SceneControlToolbars.SceneControlToolbarBase
    {
        
        public SceneControlPrintPreviewPropertiesToolbar()
        {
            InitializeComponent();
        }

        private void SceneControlPrintJobPropertiesToolbar_Load(object sender, EventArgs e)
        {

        }

        internal void Init()
        {
            this.txtPrintjobName.Text = RenderEngine.PrintJob.Name;
            this.cbPrinters.SelectedItem = RenderEngine.PrintJob.SelectedPrinter;
            this.cbMaterials.SelectedItem = PrintJobManager.SelectedMaterialSummary;

            if (RenderEngine.PrintJob.TotalPrintVolume < 0)
            {
                RenderEngine.PrintJob.TotalPrintVolume = -RenderEngine.PrintJob.TotalPrintVolume;
            }
            this.lblVolume.Text = Math.Ceiling(RenderEngine.PrintJob.TotalPrintVolume).ToString() + " ml";

            var printingTime = RenderEngine.PrintJob.PrintingTimeRemaining(0, RenderEngine.TotalAmountSlices);
            RenderEngine.PrintJob.JobTimeInSeconds = RenderEngine.PrintJob.JobTimeInSeconds;
            this.lblManufacteringTime.Text = "" + (int)printingTime.Hours + "h" + printingTime.Minutes + "m" + printingTime.Seconds + "s";
        }
    }
}
