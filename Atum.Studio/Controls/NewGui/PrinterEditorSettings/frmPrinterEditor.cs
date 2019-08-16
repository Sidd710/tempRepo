using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    public partial class frmPrinterEditor : NewGUIFormBase
    {
        private readonly int defaultSummaryHeight = 40;

        private PrinterMenuStrip _printerMenuStrip;

        internal AtumPrinter SelectedPrinter
        {
            get
            {
                var firstAvailablePrinter = this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>().Where(o => o.Selected).FirstOrDefault();
                return firstAvailablePrinter != null? firstAvailablePrinter.SelectedPrinter: null;
            }
        }

        public frmPrinterEditor()
        {
            InitializeComponent();

            this._printerMenuStrip = new PrinterMenuStrip();
            this.newGUIContentSplitContainerBase1.LeftPanel.MouseMove += LeftPanel_MouseMove;

            //remove scrollbars
            this.newGUIContentSplitContainerBase1.LeftPanel.AutoScroll = false;
            this.newGUIContentSplitContainerBase1.LeftPanel.HorizontalScroll.Enabled = false;
            this.newGUIContentSplitContainerBase1.LeftPanel.HorizontalScroll.Visible = false;
            this.newGUIContentSplitContainerBase1.LeftPanel.HorizontalScroll.Maximum = 0;
            this.newGUIContentSplitContainerBase1.LeftPanel.AutoScroll = true;
            
            this.btnApply.ForeColor = BrandingManager.Button_ForeColor;
            this.btnApply.BackColor = this.btnApply.BorderColor = BrandingManager.Button_BackgroundColor_LightDark;
        }

        private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMenu();
        }

        public void ShowMenu()
        {
            if (!this.Controls.Contains(_printerMenuStrip))
            {
                _printerMenuStrip.Top = this.newGUIContentSplitContainerBase1.LeftPanel.Height - this._printerMenuStrip.Height;
                _printerMenuStrip.Left = 16;
                _printerMenuStrip.Visible = true;
                _printerMenuStrip.onPrinterAdded += FrmPrinterFlyout_onPrinterAdded;
                _printerMenuStrip.onAddNewDLPStation2Call += PrinterMenuStrip_onAddNewDLPStation2Call;
                _printerMenuStrip.onAddNewDLPStation5Call += PrinterMenuStrip_onAddNewDLPStation5Call;
                this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(_printerMenuStrip);
            }
            _printerMenuStrip.BringToFront();
        }

        public void CloseMenu()
        {
            if (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Contains(_printerMenuStrip))
            {
                this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Remove(_printerMenuStrip);
            }
        }

        private void PrinterMenuStrip_onAddNewDLPStation2Call(object sender, EventArgs e)
        {
            if ((bool)sender)
            {
                this.newGUIContentSplitContainerBase1.RightPanel.Controls.Clear();
                PrinterAddControl printerAddControl = new PrinterAddControl(new AtumV15Printer());
                printerAddControl.onSaved += PrinterAddControl_onSaved;
                printerAddControl.onCancel += PrinterAddControl_onCancel;
                this.newGUIContentSplitContainerBase1.RightPanel.Controls.Add(printerAddControl);
            }
        }

        private void PrinterMenuStrip_onAddNewDLPStation5Call(object sender, EventArgs e)
        {
            if ((bool)sender)
            {
                this.newGUIContentSplitContainerBase1.RightPanel.Controls.Clear();
                PrinterAddControl printerAddControl = new PrinterAddControl(new AtumDLPStation5());
                printerAddControl.onSaved += PrinterAddControl_onSaved;
                printerAddControl.onCancel += PrinterAddControl_onCancel;
                this.newGUIContentSplitContainerBase1.RightPanel.Controls.Add(printerAddControl);
            }
        }

        private void PrinterAddControl_onCancel(object sender, EventArgs e)
        {
            LoadPrinters();
        }

        private void PrinterAddControl_onSaved(object sender, EventArgs e)
        {
            if (sender is AtumPrinter)
            {
                var atumPrinter = sender as AtumPrinter;
                PrinterManager.AvailablePrinters.Add(atumPrinter);
                PrinterManager.Save();
                LoadPrinters(atumPrinter);
            }
        }

        private void pbMinusSign_Click(object sender, EventArgs e)
        {
            var selected = (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>()).Where(o => o.Selected).FirstOrDefault();
            if (new frmMessageBox("Remove selected DLP Station", string.Format("Are you sure you want to remove DLP Station {0} and all settings?", selected.SelectedPrinter.DisplayName), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button3).ShowDialog() == DialogResult.Yes)
            {
                PrinterManager.AvailablePrinters.Remove((Atum.DAL.Hardware.AtumPrinter)selected.SelectedPrinter);
                LoadPrinters();
            }
        }

        private void pbPlusSign_Click(object sender, EventArgs e)
        {
            ShowMenu();
        }
        private void FrmPrinterFlyout_onPrinterAdded(object sender, EventArgs e)
        {
            if (sender is AtumPrinter)
            {
                PrinterManager.Save();
                LoadPrinters();
            }
        }

        private void newGUIContentSplitContainerBase1_Load(object sender, EventArgs e)
        {
            if (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Count == 0)
            {
                LoadPrinters();
            }
        }
        public void LoadPrinters(AtumPrinter selectPrinter = null)
        {

            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Clear();

            AddValueEditorControl();

            var summaryHeight = 0;
            var firstPrinter = true;
            if (PrinterManager.AvailablePrinters != null) {
                if (PrinterManager.AvailablePrinters.Count > 0)
                {
                    foreach (var atumPrinter in PrinterManager.AvailablePrinters)
                    {
                        PrinterSummaryControl printerSummaryControl = PushPrinterInControl(summaryHeight, atumPrinter);
                        if (selectPrinter == null)
                        {
                            if (firstPrinter)
                            {
                                printerSummaryControl.Selected = true;
                                firstPrinter = false;
                                this.UpdateRightPanel(printerSummaryControl);
                            }
                        }
                        else
                        {
                            if (atumPrinter.ID == selectPrinter.ID)
                            {
                                printerSummaryControl.Selected = true;
                                this.UpdateRightPanel(printerSummaryControl);
                            }
                        }
                        summaryHeight += defaultSummaryHeight;
                    }
                }
                else
                {
                    PrinterMenuStrip_onAddNewDLPStation5Call(true, null);
                }
            }
            else
            {
                PrinterMenuStrip_onAddNewDLPStation5Call(true, null);
            }

            if (!this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>().Any(s => s.Selected)){
                if (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>().Count() > 0)
                {
                    var printerSummaryControl = this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>().First();
                    printerSummaryControl.Selected = true;
                    this.UpdateRightPanel(printerSummaryControl);
                }
            }
        }

        private PrinterValueEditorControl AddValueEditorControl()
        {
            this.newGUIContentSplitContainerBase1.RightPanel.Controls.Clear();
            var printerValueEditorControl = new PrinterValueEditorControl(this);
            printerValueEditorControl.onSaved += PrinterValueEditorControl_onSaved;
            printerValueEditorControl.onTextChanged += PrinterValueEditorControl_onTextChanged; ;
            this.newGUIContentSplitContainerBase1.RightPanel.Controls.Add(printerValueEditorControl);
            return printerValueEditorControl;
        }

        private void PrinterValueEditorControl_onTextChanged(object sender, EventArgs e)
        {
            var printerValueEditorControl = sender as PrinterValueEditorControl;
            foreach (PrinterSummaryControl printerSummary in this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>())
            {
                if (printerSummary.Selected)
                {
                    printerSummary.SelectedPrinter = printerValueEditorControl.SelectedPrinter;
                }
            }

            printerValueEditorControl.Focus();
            PrinterManager.Save();
        }

        private void PrinterValueEditorControl_onSaved(object sender, EventArgs e)
        {

        }

        private PrinterSummaryControl PushPrinterInControl(int summaryHeight, AtumPrinter atumPrinter)
        {
            var printerSummaryControl = new PrinterSummaryControl();
            printerSummaryControl.ID = atumPrinter.ID;
            printerSummaryControl.onSelected += PrinterSummaryControl_onSelected;
            printerSummaryControl.SelectedPrinter = atumPrinter;
            printerSummaryControl.Left = 0;
            printerSummaryControl.Top = summaryHeight;
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(printerSummaryControl);
            return printerSummaryControl;
        }

        private void PrinterSummaryControl_onSelected(object sender, EventArgs e)
        {
            foreach (PrinterSummaryControl printerSummaryControl in this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<PrinterSummaryControl>())
            {
                if (printerSummaryControl.Selected)
                {
                    printerSummaryControl.Selected = false;
                }
            }

            var selectedPrinterSummary = sender as PrinterSummaryControl;
            selectedPrinterSummary.Selected = true;
            this.UpdateRightPanel(selectedPrinterSummary);
        }
        private void UpdateRightPanel(PrinterSummaryControl selectedPrinter)
        {
            var printerValueEditorControl = this.newGUIContentSplitContainerBase1.RightPanel.Controls[0] as PrinterValueEditorControl;
            if (printerValueEditorControl == null)
            {
                printerValueEditorControl = AddValueEditorControl();
            }
            printerValueEditorControl.SelectedPrinter = selectedPrinter.SelectedPrinter;
        }

        private void spcFooterContainer_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.X > (pbMinusSign.Left / 2))
            {
                CloseMenu();
            }
        }

        private void pbMinusSign_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMenu();
        }

        private void frmPrinterEditor_FormClosing(object sender, FormClosingEventArgs e)
        {
            PrinterManager.Save();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
