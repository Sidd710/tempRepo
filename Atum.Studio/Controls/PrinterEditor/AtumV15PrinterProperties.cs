using System.Windows.Forms;
using Atum.DAL.Hardware;
using System;
using System.Runtime.InteropServices;

namespace Atum.Studio.Controls.PrinterEditor
{
    public partial class AtumV15PrinterProperties : UserControl
    {
        private AtumV15Printer _dataSource;
        private bool _inWizard;

        public bool inWizard
        {
            get
            {
                return _inWizard;
            }
            set
            {
                _inWizard = value;
                if (_inWizard)
                {
                    rbtn50Micron.Enabled = true;
                    rbtn75Micron.Enabled = true;
                    rbtn100Micron.Enabled = true;
                    this.Properties.Controls.Remove(this.tableLayoutPanel1);
                    this.tableLayoutPanel1.Dock = DockStyle.None;
                    this.tableLayoutPanel1.Top = this.lblHeader.Height + 10;
                    this.tableLayoutPanel1.Left += 10;
                    this.tabControl1.Visible = false;
                    this.Controls.Add(this.tableLayoutPanel1);
                    
                    //  ((Control)this.tabControl1.TabPages[1]).Enabled = false;

                }
                else
                {
                    rbtn50Micron.Enabled = false;
                    rbtn75Micron.Enabled = false;
                    rbtn100Micron.Enabled = false;
                    //   ((Control)this.tabControl1.TabPages[1]).Enabled = true;
                }
            }
        }

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
                    }


                    this.atumPrinterCalibration1.DataSource = this._dataSource;
                    this.atumPrinterCalibration1.AdvancedMode = false;

                    switch (this._dataSource.PrinterXYResolution)
                    {
                        case AtumPrinter.PrinterXYResolutionType.Micron40:
                            this.rbtn50Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron50:
                            this.rbtn50Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron75:
                            this.rbtn75Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Micron100:
                            this.rbtn100Micron.Checked = true;
                            break;
                        case AtumPrinter.PrinterXYResolutionType.Unknown:
                            this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron50); // same as selected printer;
                            break;
                    }

                }
            }
        }

        public AtumV15PrinterProperties()
        {
            InitializeComponent();

            //dpi correction
            this.txtDescription.Left = this.txtDisplayName.Left = this.label2.Left + this.label2.Width;
        }

        private void UpdatePrinterSettings()
        {
            this._dataSource.DisplayName = this.txtDisplayName.Text;
            this._dataSource.Description = this.txtDescription.Text;

            var selectedPrinter = this.atumPrinterCalibration1.DataSource;

            if (selectedPrinter != null)
            {
                if (selectedPrinter.IsDirty)
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
        }

        private void rbtnMicron_CheckedChanged(object sender, System.EventArgs e)
        {
            var radioButtonSender = sender as RadioButton;
            if (radioButtonSender.Checked)
            {
                if (this.inWizard && this._dataSource != null)
                {
                    if (this.rbtn50Micron.Checked)
                    {
                        this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron50);
                    }
                    else if (this.rbtn75Micron.Checked)
                    {
                        this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron75);
                    }
                    else if (this.rbtn100Micron.Checked)
                    {
                        this._dataSource.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron100);
                    }
                    this.atumPrinterCalibration1.DataSource = this._dataSource;
                }
            }

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (_inWizard && e.TabPage == tbCalibration)
            {
                e.Cancel = true;
            }
        }
    }
}
