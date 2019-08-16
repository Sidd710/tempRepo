using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls
{
    public partial class PrinterCorrectionWizard : Form
    {

        public PrinterCorrectionWizard()
        {
            InitializeComponent();
        }

        private void PrinterCorrectionWizard_Load(object sender, EventArgs e)
        {
            //default settings
            this.tbWizard.Top -= 23;
            this.tbWizard.Height += 25;
            this.tbWizard.Width += 5;

            this.printerCorrectionFactorTabPanel1.ButtonBackVisible = false;
            this.printerCorrectionFactorTabPanel1.txtCorrectionFactorX.Value = (decimal)PrinterManager.DefaultPrinter.CorrectionFactorX * 10;
            this.printerCorrectionFactorTabPanel1.txtCorrectionFactorY.Value = (decimal)PrinterManager.DefaultPrinter.CorrectionFactorY * 10;
        }

        private void printerCorrectionWelcomeTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
        }

        private void printerCorrectionFactorTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            PrinterManager.DefaultPrinter.CorrectionFactorX = (float)this.printerCorrectionFactorTabPanel1.txtCorrectionFactorX.Value / 10;
            PrinterManager.DefaultPrinter.CorrectionFactorY = (float)this.printerCorrectionFactorTabPanel1.txtCorrectionFactorY.Value /10;
            PrinterManager.Save();


            this.Close();
        }


    }
}