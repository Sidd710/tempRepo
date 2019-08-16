using Atum.DAL.Hardware;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterConnectionPropertiesTabPanel : WizardTabPanel
    {
        private AtumPrinter _dataSource;

        public AtumPrinter DataSource
        {
            get
            {
                if (!DesignMode)
                {
                    if (_dataSource is AtumV15Printer)
                    {
                        return this.atumV15PrinterProperties1.DataSource;
                    }
                    else if (_dataSource is AtumV20Printer)
                    {
                        return this.atumV20PrinterProperties1.DataSource;
                    }
                    else if (_dataSource is AtumDLPStation5)
                    {
                        return this.atumV40PrinterProperties1.DataSource;
                    }
                    else if(_dataSource is LoctiteV10)
                    {
                        return this.loctiteV10PrinterProperties1.DataSource;
                    }
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
                        if (value is LoctiteV10)
                        {
                            this.atumV15PrinterProperties1.Visible = false;
                            this.atumV20PrinterProperties1.Visible = false;
                            this.atumV40PrinterProperties1.Visible = false;

                            this.loctiteV10PrinterProperties1.Visible = true;

                            this.loctiteV10PrinterProperties1.DataSource = this._dataSource as LoctiteV10;
                        }
                        else if (value is AtumV15Printer)
                        {
                            this.atumV15PrinterProperties1.Visible = true;
                            this.atumV20PrinterProperties1.Visible = false;
                            this.atumV40PrinterProperties1.Visible = false;

                            this.loctiteV10PrinterProperties1.Visible = false;

                            this.atumV15PrinterProperties1.DataSource = this._dataSource as AtumV15Printer;
                        }
                        else if (value is AtumV20Printer)
                        {
                            this.atumV15PrinterProperties1.Visible = false;
                            this.atumV20PrinterProperties1.Visible = true;
                            this.atumV40PrinterProperties1.Visible = false;
                            this.loctiteV10PrinterProperties1.Visible = false;

                            this.atumV20PrinterProperties1.DataSource = this._dataSource as AtumV20Printer;
                        }
                        else if (value is AtumDLPStation5)
                        {
                            this.atumV15PrinterProperties1.Visible = false;
                            this.atumV20PrinterProperties1.Visible = false;
                            this.atumV40PrinterProperties1.Visible = true;
                            this.loctiteV10PrinterProperties1.Visible = false;
                            
                            this.atumV40PrinterProperties1.DataSource = this._dataSource as AtumDLPStation5;
                        }

                    }
                }
            }
        }

        public PrinterConnectionPropertiesTabPanel()
        {
            InitializeComponent();
            this.atumV15PrinterProperties1.inWizard = true;
            this.atumV20PrinterProperties1.inWizard = true;
            this.atumV40PrinterProperties1.inWizard = true;
            this.loctiteV10PrinterProperties1.inWizard = true;
        }

    }
}
