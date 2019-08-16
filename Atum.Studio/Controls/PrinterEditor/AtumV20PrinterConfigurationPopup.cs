using Atum.DAL.Hardware;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV20PrinterConfigurationPopup : BasePopup
    {
        public AtumV20Printer DataSource
        {
            get
            {
                return this.atumV20PrinterProperties1.DataSource;
            }

            set
            {
                value.CreatePreserved();
                this.atumV20PrinterProperties1.DataSource = value;
            }
        }

        public AtumV20PrinterConfigurationPopup()
        {
            InitializeComponent();

            this.Height = this.plContent.Height + this.plFooter.Height;
            this.atumV20PrinterProperties1.inWizard = false;
        }

    }
}
