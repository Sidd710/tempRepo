using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGUI.PrinterManagers
{
    public partial class PrinterManagerPopup : BasePopupNoTitle
    {
        public PrinterManagerPopup()
        {
            InitializeComponent();
        }

        private void PrinterManagerPopup_Load(object sender, EventArgs e)
        {
            this.lblHeader.Text = this.Text;

            this.plList.Controls.Clear();

            //if (!DesignMode)
           // {
                var summaryHeight = 0;
                var firstMaterial = true;

                foreach (var PrintersSupplier in PrinterManager.SelectedPrinters)
                {
                    var printerSummary = new PrinterSummary();
                    printerSummary.onSelected += printerSummary_onSelected;
                    printerSummary.AtumPrinter = PrintersSupplier;
                    printerSummary.Left = 0;
                    printerSummary.Top = summaryHeight;
                    printerSummary.Width = plList.Width;
                    plList.Controls.Add(printerSummary);

                    if (firstMaterial)
                    {
                        printerSummary.Selected = true;
                        firstMaterial = false;
                    }
                    summaryHeight += 40;

                }
        //    }

            UpdateControls();
        }

        private void printerSummary_onSelected(object sender, EventArgs e)
        {

            foreach (PrinterSummary printerSummary in this.plList.Controls.OfType<PrinterSummary>())
            {
                if (printerSummary.Selected)
                {
                    printerSummary.Selected = false;
                }
            }

            var selectedMaterialSummary = sender as PrinterSummary;
            selectedMaterialSummary.Selected = true;

            UpdateControls();
            //this.UpdateRightPanel(selectedMaterialSummary);
        }

        void UpdateControls()
        {
            try
            {
                this.btnRemoveSelectedPrinter.InvokeEnabled = false;
                this.btnProperties.InvokeEnabled = false;
                this.btnSetAsDefault.InvokeEnabled = false;
                var selected = (this.plList.Controls.OfType<PrinterSummary>()).Where(o => o.Selected).FirstOrDefault();
                if (selected != null)
                {
                    if (this.plList.Controls.OfType<PrinterSummary>().Count() > 1)
                    {
                        this.btnRemoveSelectedPrinter.InvokeEnabled = true;
                    }
                    this.btnProperties.InvokeEnabled = true;

                    if (!(selected.AtumPrinter as AtumPrinter).Default)
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

            //            PrinterManagerPopup_Load(this, null);
            //        }
            //    }
            //}

            //UpdateControls();
        }

        private void btnRemoveSelectedPrinter_Click(object sender, EventArgs e)
        {
            var selected = (this.plList.Controls.OfType<PrinterSummary>()).Where(o => o.Selected).FirstOrDefault();

            PrinterManager.AvailablePrinters.Remove((Atum.DAL.Hardware.AtumPrinter)selected.AtumPrinter);

            PrinterManagerPopup_Load(this, null);
        }

        private void btnProperties_Click(object sender, EventArgs e)
        {
            var selected = (this.plList.Controls.OfType<PrinterSummary>()).Where(o => o.Selected).FirstOrDefault();
            if (selected != null)
            {

                if (selected.AtumPrinter is AtumV15Printer)
                {
                    var selectedPrinter = selected.AtumPrinter as AtumV15Printer;
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
                else if (selected.AtumPrinter is AtumV20Printer)
                {
                    var selectedPrinter = selected.AtumPrinter as AtumV20Printer;
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
                else if (selected.AtumPrinter is AtumDLPStation5)
                {
                    var selectedPrinter = selected.AtumPrinter as AtumDLPStation5;
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

                else if (selected.AtumPrinter is LoctiteV10)
                {
                    var selectedPrinter = selected.AtumPrinter as LoctiteV10;
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
            var selected = (this.plList.Controls.OfType<PrinterSummary>()).Where(o => o.Selected).FirstOrDefault();

            if (selected != null)
            {
                var selectedPrinter = selected.AtumPrinter as AtumPrinter;
                foreach (var atumPrinter in PrinterManager.SelectedPrinters)
                {
                    atumPrinter.Default = false;
                }

                selectedPrinter.Default = true;
                PrinterManager.Save();

                //update ground pane
                ObjectView.GroundPane.UpdateVertexArray((PrinterManager.DefaultPrinter.ProjectorResolutionX / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorX, (PrinterManager.DefaultPrinter.ProjectorResolutionY / 10) * PrinterManager.DefaultPrinter.TrapeziumCorrectionFactorY, 0.5f);
                ObjectView.GroundPane.UpdateBinding();

                UpdateControls();
            }
        }
    }
}
