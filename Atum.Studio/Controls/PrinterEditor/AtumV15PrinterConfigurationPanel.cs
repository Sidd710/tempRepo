using System;
using System.Windows.Forms;
using Atum.DAL.Hardware;
using Atum.Studio.Controls.PrinterConnectionWizard;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV15PrinterConfigurationPanel : WizardTabPanel
    {
        private AtumV15Printer _dataSource;

        public AtumV15Printer DataSource
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
                        foreach (var property in this._dataSource.Properties)
                        {
                            printerPropertyControl1.PrinterProperty = property;
                        }
                    }

                    this.label3.Text = "Advanced";
                    this.trapezoid.AdvancedMode = false;

                    this.trapezoid.SelectedPrinter = this._dataSource;
                }
            }
        }

        public AtumV15PrinterConfigurationPanel()
        {
            InitializeComponent();

            this.btnBack.Visible = false;
            this.txtHeader.Location = new System.Drawing.Point(this.txtHeader.Location.X - 15, this.txtHeader.Location.Y);

            this.txtHeader.Text = " Atum V1.5";
        }

        private void UpdatePrinterSettings()
        {
            this._dataSource.DisplayName = this.txtDisplayName.Text;
            this._dataSource.Description = this.txtDescription.Text;
            if (this.trapezoid != null)
            {
                var selectedPrinter = this.trapezoid.SelectedPrinter;
                if (selectedPrinter != null)
                {
                    this._dataSource.TrapeziumCorrectionSideA = selectedPrinter.TrapeziumCorrectionSideA;
                    this._dataSource.TrapeziumCorrectionSideB = selectedPrinter.TrapeziumCorrectionSideB;
                    this._dataSource.TrapeziumCorrectionSideC = selectedPrinter.TrapeziumCorrectionSideC;
                    this._dataSource.TrapeziumCorrectionSideD = selectedPrinter.TrapeziumCorrectionSideD;
                    this._dataSource.TrapeziumCorrectionSideE = selectedPrinter.TrapeziumCorrectionSideE;
                    this._dataSource.TrapeziumCorrectionSideF = selectedPrinter.TrapeziumCorrectionSideF;

                    this._dataSource.TrapeziumCorrectionInputA = selectedPrinter.TrapeziumCorrectionInputA;
                    this._dataSource.TrapeziumCorrectionInputB = selectedPrinter.TrapeziumCorrectionInputB;
                    this._dataSource.TrapeziumCorrectionInputC = selectedPrinter.TrapeziumCorrectionInputC;
                    this._dataSource.TrapeziumCorrectionInputD = selectedPrinter.TrapeziumCorrectionInputD;

                }
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

        private void label3_Click(object sender, EventArgs e)
        {
            trapezoid.AdvancedMode = !trapezoid.AdvancedMode;
            label3.Text = trapezoid.AdvancedMode ? "Normal" : "Advanced";
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            this._dataSource.DisplayName = this.txtDisplayName.Text;
        }
    }
}
