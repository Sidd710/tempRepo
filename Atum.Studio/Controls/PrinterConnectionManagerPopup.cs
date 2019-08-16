using System;
using System.ComponentModel;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using System.Net;
using Atum.DAL.Hardware;
using Atum.Studio.Core.ModelView;
using System.Diagnostics;

namespace Atum.Studio.Controls
{
    public partial class PrinterConnectionManagerPopup : BasePopupNoTitle
    {
        public PrinterConnectionManagerPopup()
        {
            InitializeComponent();
        }

        private void PrinterConnectionManagerPopup_Load(object sender, EventArgs e)
        {

            PrinterManager.AvailablePrinters.ListChanged += new ListChangedEventHandler(AvailablePrinters_ListChanged);

            AvailablePrinters_ListChanged(null, null);

            UpdateControls();
        }


        delegate void SetAvailablePrintersDataSource();
        void AvailablePrinters_ListChanged(object sender, ListChangedEventArgs e)
        {

            if (this.dgAvailablePrinters.InvokeRequired)
            {
                var d = new SetAvailablePrintersDataSource(UpdateAvailablePrinters);
                this.Invoke(d, new object[] { });
            }
            else
            {
                UpdateAvailablePrinters();
            }

            UpdateControls();
        }

        void UpdateAvailablePrinters()
        {
            this.dgAvailablePrinters.DataSource = null;
            this.dgAvailablePrinters.DataSource = PrinterManager.SelectedPrinters;
        }

       
        private void btnAddPrinter_Click(object sender, EventArgs e)
        {
           
            //using (var printConnectionWizard = new PrinterConnectionWizard.PrinterConnectionWizard())
            //{
            //    if (printConnectionWizard.ShowDialog() == DialogResult.OK)
            //    {
            //        if (printConnectionWizard.SelectedPrinter != null)
            //        {
            //            foreach (var availablePrinter in PrinterManager.AvailablePrinters)
            //            {
            //                availablePrinter.Default = false;
            //            }

            //            printConnectionWizard.SelectedPrinter.Selected = true;
            //            printConnectionWizard.SelectedPrinter.Default = true;

            //            if (printConnectionWizard.SelectedPrinter.FirstIPAddress != null)
            //            {
            //                DAL.Managers.ConnectionManager.Send(PrinterManager.DefaultPrinter, IPAddress.Parse(printConnectionWizard.SelectedPrinter.FirstIPAddress), 11000);
            //            };
            //            PrinterManager.AvailablePrinters.Add(printConnectionWizard.SelectedPrinter);
            //            PrinterManager.Save();

            //            UpdateAvailablePrinters();
            //        }
            //    }
            //}

            UpdateControls();
        }

        private void btnRemoveSelectedPrinter_Click(object sender, EventArgs e)
        {
            PrinterManager.AvailablePrinters.Remove((Atum.DAL.Hardware.AtumPrinter)this.dgAvailablePrinters.SelectedRows[0].DataBoundItem);

            UpdateControls();
        }

        void UpdateControls()
        {
            try
            {
                this.btnRemoveSelectedPrinter.InvokeEnabled = false;
                this.btnProperties.InvokeEnabled = false;
                this.btnSetAsDefault.InvokeEnabled = false;

                if (this.dgAvailablePrinters.SelectedRows.Count > 0)
                {
                    if (this.dgAvailablePrinters.RowCount > 1)
                    {
                        this.btnRemoveSelectedPrinter.InvokeEnabled = true;
                    }
                    this.btnProperties.InvokeEnabled = true;

                    if (!(this.dgAvailablePrinters.SelectedRows[0].DataBoundItem as AtumPrinter).Default)
                    {
                        this.btnSetAsDefault.InvokeEnabled = true;
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            if (this.dgAvailablePrinters.SelectedRows.Count == 1){
                if (this.dgAvailablePrinters.SelectedRows[0].DataBoundItem is AtumV15Printer)
                {
                    var selectedPrinter = this.dgAvailablePrinters.SelectedRows[0].DataBoundItem as AtumV15Printer;
                    using (var printerPropertiesPopup = new PrinterEditor.AtumV15PrinterConfigurationPopup())
                    {
                        printerPropertiesPopup.DataSource = selectedPrinter;
                        printerPropertiesPopup.StartPosition = FormStartPosition.CenterParent;
                        if (printerPropertiesPopup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            selectedPrinter.DisplayName = printerPropertiesPopup.DataSource.DisplayName;
                            PrinterManager.Save();

                            //update ground pane
                            if (selectedPrinter.Default)
                            {
                                ObjectView.GroundPane.UpdateVertexArray((PrinterManager.DefaultPrinter.ProjectorResolutionX / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX, (PrinterManager.DefaultPrinter.ProjectorResolutionY / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY, 0.5f);
                                ObjectView.GroundPane.UpdateBinding();
                            }
                        }
                        else
                        {
                            selectedPrinter.RevertChanges();
                        }
                    }
                }
                else if (this.dgAvailablePrinters.SelectedRows[0].DataBoundItem is AtumV20Printer)
                {
                    var selectedPrinter = this.dgAvailablePrinters.SelectedRows[0].DataBoundItem as AtumV20Printer;
                    using (var printerPropertiesPopup = new PrinterEditor.AtumV20PrinterConfigurationPopup())
                    {
                        printerPropertiesPopup.DataSource = selectedPrinter;
                        printerPropertiesPopup.StartPosition = FormStartPosition.CenterParent;
                        if (printerPropertiesPopup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            selectedPrinter.DisplayName = printerPropertiesPopup.DataSource.DisplayName;
                            PrinterManager.Save();

                            //update ground pane
                            if (selectedPrinter.Default)
                            {
                                ObjectView.GroundPane.UpdateVertexArray((PrinterManager.DefaultPrinter.ProjectorResolutionX / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX, (PrinterManager.DefaultPrinter.ProjectorResolutionY / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY, 0.5f);
                                ObjectView.GroundPane.UpdateBinding();
                            }
                        }
                        else
                        {
                            selectedPrinter.RevertChanges();
                        }
                    }

                }
                else if (this.dgAvailablePrinters.SelectedRows[0].DataBoundItem is AtumDLPStation5)
                {
                    var selectedPrinter = this.dgAvailablePrinters.SelectedRows[0].DataBoundItem as AtumDLPStation5;
                    using (var printerPropertiesPopup = new PrinterEditor.AtumDLPStation5PrinterConfigurationPopup())
                    {
                        printerPropertiesPopup.DataSource = selectedPrinter;
                        printerPropertiesPopup.StartPosition = FormStartPosition.CenterParent;
                        if (printerPropertiesPopup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            selectedPrinter.DisplayName = printerPropertiesPopup.DataSource.DisplayName;
                            PrinterManager.Save();

                            //update ground pane
                            if (selectedPrinter.Default)
                            {
                                ObjectView.GroundPane.UpdateVertexArray((PrinterManager.DefaultPrinter.ProjectorResolutionX / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX, (PrinterManager.DefaultPrinter.ProjectorResolutionY / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY, 0.5f);
                                ObjectView.GroundPane.UpdateBinding();
                            }
                        }
                        else
                        {
                            selectedPrinter.RevertChanges();
                        }
                    }
                }

                else if (this.dgAvailablePrinters.SelectedRows[0].DataBoundItem is LoctiteV10)
                {
                    var selectedPrinter = this.dgAvailablePrinters.SelectedRows[0].DataBoundItem as LoctiteV10;
                    using (var printerPropertiesPopup = new PrinterEditor.LoctiteV10PrinterConfigurationPopup())
                    {
                        printerPropertiesPopup.DataSource = selectedPrinter;
                        printerPropertiesPopup.StartPosition = FormStartPosition.CenterParent;
                        if (printerPropertiesPopup.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
                        {
                            selectedPrinter.DisplayName = printerPropertiesPopup.DataSource.DisplayName;
                            PrinterManager.Save();

                            //update ground pane
                            if (selectedPrinter.Default)
                            {
                                ObjectView.GroundPane.UpdateVertexArray((PrinterManager.DefaultPrinter.ProjectorResolutionX / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX, (PrinterManager.DefaultPrinter.ProjectorResolutionY / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY, 0.5f);
                                ObjectView.GroundPane.UpdateBinding();
                            }
                        }
                        else
                        {
                            selectedPrinter.RevertChanges();
                        }
                    }
                }
            } 
        }

        private void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            if (this.dgAvailablePrinters.SelectedRows.Count == 1)
            {
                var selectedPrinter = this.dgAvailablePrinters.SelectedRows[0].DataBoundItem as AtumPrinter;
                foreach (var atumPrinter in PrinterManager.SelectedPrinters)
                {
                    atumPrinter.Default = false;
                }

                selectedPrinter.Default = true;
                PrinterManager.Save();

                //update ground pane
                ObjectView.GroundPane.UpdateVertexArray((PrinterManager.DefaultPrinter.ProjectorResolutionX / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX, (PrinterManager.DefaultPrinter.ProjectorResolutionY / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY, 0.5f);
                ObjectView.GroundPane.UpdateBinding();
            }
        }

        private void dgAvailablePrinters_SelectionChanged(object sender, EventArgs e)
        {
            UpdateControls();
        }
    }
}
