using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using Atum.DAL.Hardware;


namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionAdjustTabPanel : WizardTabPanel
    {
        private AtumPrinter _dataSource;

        public AtumPrinter DataSource
        {
            get
            {
                if (!DesignMode)
                {
                    return this.atumPrinterCalibration1.DataSource;
                }

                return null;
            }
            set
            {
                this._dataSource = value;

                if (this._dataSource != null)
                {
                    if (this._dataSource.Properties != null)
                    {

                        this.atumPrinterCalibration1.DataSource = this._dataSource;

                    }
                }
            }
        }
        public PrinterConnectionAdjustTabPanel()
        {
            InitializeComponent();
        }

       
    }
}
