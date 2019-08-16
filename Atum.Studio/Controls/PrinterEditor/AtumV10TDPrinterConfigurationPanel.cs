using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.DAL.Hardware;
using Atum.Studio.Controls.PrinterConnectionWizard;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV10TDPrinterConfigurationPanel : WizardTabPanel
    {
        private AtumV10TDPrinter _dataSource;

        public AtumV10TDPrinter DataSource
        {
            get
            {
                if (!this.DesignMode) this.UpdatePrinterSettings();
                return this._dataSource;
            }
            set
            {
                this._dataSource = value;

                if (this._dataSource != null)
                {
                    if (this._dataSource.Properties != null)
                    {
                        this.txtDisplayName.Text = this._dataSource.DisplayName;
                        this.txtDescription.Text = this._dataSource.Description;
                        this.txtStartPrintOffset.Value = this._dataSource.StartPositionInMM;
                        foreach (var property in this._dataSource.Properties)
                        {
                            printerPropertyControl1.PrinterProperty = property;
                        }
                    }

                    this.trapezoid1.SelectedPrinter = this._dataSource;
                }
            }
        }

        public AtumV10TDPrinterConfigurationPanel()
        {
            InitializeComponent();

            this.btnBack.Visible = false;
            this.txtHeader.Location = new System.Drawing.Point(this.txtHeader.Location.X - 15, this.txtHeader.Location.Y);
        }

        private void UpdatePrinterSettings()
        {
            this._dataSource.DisplayName = this.txtDisplayName.Text;
            this._dataSource.Description = this.txtDescription.Text;

            if (this.trapezoid1.SelectedPrinter != null)
            {
                this._dataSource.TrapeziumCorrectionSideA = (float)this.trapezoid1.SelectedPrinter.TrapeziumCorrectionSideA;
                this._dataSource.TrapeziumCorrectionSideB = (float)this.trapezoid1.SelectedPrinter.TrapeziumCorrectionSideB;
                this._dataSource.TrapeziumCorrectionSideC = (float)this.trapezoid1.SelectedPrinter.TrapeziumCorrectionSideC;
                this._dataSource.TrapeziumCorrectionSideD = (float)this.trapezoid1.SelectedPrinter.TrapeziumCorrectionSideD;
                this._dataSource.TrapeziumCorrectionSideE = (float)this.trapezoid1.SelectedPrinter.TrapeziumCorrectionSideE;
                this._dataSource.TrapeziumCorrectionSideF = (float)this.trapezoid1.SelectedPrinter.TrapeziumCorrectionSideF;
            }

            foreach (Control rootControl in this.plContent.Controls)
            {

                if (rootControl is CustomTabControl)
                {
                    foreach (var childControl in ((CustomTabControl)rootControl).TabPages[0].Controls)
                    {
                        if (childControl is Panel)
                        {
                            foreach (var control in ((Panel)childControl).Controls)
                            {
                                if (control is PrinterPropertyControl)
                                {
                                    var printerPropertyControl = (PrinterPropertyControl)control;
                                    switch (printerPropertyControl.PrinterProperty.Type)
                                    {
                                        case AtumPrinterProperty.typePrinterProperty.TypeOfSpindle:
                                            foreach (var valueitem in printerPropertyControl.PrinterProperty.Values)
                                            {
                                                if (valueitem.Selected)
                                                {

                                                    if ((int)valueitem.Value == 0)
                                                    {
                                                        //trapezium
                                                        this._dataSource.SpindleRotation = 3f;
                                                    }
                                                    else if ((int)valueitem.Value == 1)
                                                    {
                                                        //ball
                                                        this._dataSource.SpindleRotation = 2.5f;
                                                    }
                                                    break;
                                                }
                                            }
                                            break;
                                    }
                                }
                            }

                        }
                    }
                }
            }
        }

        private void btnUpperRigthCalc_Click(object sender, EventArgs e)
        {
            this.trapezoid1.CalcUpperRight();
        }

        private void btnLowerRightCalc_Click(object sender, EventArgs e)
        {
            this.trapezoid1.CalcLowerRight();
        }

        private void txtStartPrintOffset_ValueChanged(object sender, EventArgs e)
        {
            this._dataSource.StartPositionInMM = Convert.ToInt32(this.txtStartPrintOffset.Value);
        }
    }
}
