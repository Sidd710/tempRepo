using System.Windows.Forms;
using Atum.DAL.Hardware;
using System;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumPrinterCalibration : UserControl
    {
     //   internal bool IsDirty { get; set; }

        public bool AdvancedMode
        {
            get
            {
                return this.trapezoid.AdvancedMode;
            }
            set
            {
                this.lblAdvancedMode.Text = value ? "Normal" : "Advanced";
                this.trapezoid.AdvancedMode = value;
            }
        }

        public AtumPrinter DataSource
        {
            get
            {
                return this.trapezoid.SelectedPrinter;
            }
            set
            {
                this.trapezoid.SelectedPrinter = value;
            }
        }
        public AtumPrinterCalibration()
        {
            InitializeComponent();

            //this.trapezoid.DirtyStateChanged += Trapezoid_DirtyStateChanged;
        }

        //private void Trapezoid_DirtyStateChanged(bool obj)
        //{
        //    this.IsDirty = obj;
        //}

        private void lblAdvancedMode_Click(object sender, System.EventArgs e)
        {
            this.AdvancedMode = !this.AdvancedMode;
        }
    }
}
