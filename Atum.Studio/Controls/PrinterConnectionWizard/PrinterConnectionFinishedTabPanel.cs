using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionFinishedTabPanel : WizardTabPanel
    {
        public string printername
        {
            set
            {
                this.label1.Text = "Click Finished to create printer " + value;
            }
        }
        
        public PrinterConnectionFinishedTabPanel()
        {
            InitializeComponent();
           
        }
    }
}
