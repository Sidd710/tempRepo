using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    public partial class PrinterSummaryControl : UserControl
    {
        public PrinterSummaryControl()
        {
            InitializeComponent();
        }

        public event EventHandler onSelected;
        private bool _selected;
        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                this.UpdateControl();
                if (this._selected)
                    this.OnEnterChange();
                this.OnLeaveChange();
            }
        }
        private AtumPrinter _selectedPrinter;
        public AtumPrinter SelectedPrinter
        {
            get
            {
                return this._selectedPrinter;
            }
            set
            {
                this._selectedPrinter = value;
                this.UpdateControl();
            }
        }
        public string ID
        {
            get; set;
        }


        public void UpdateControl()
        {
            this.ID = this._selectedPrinter.ID;
            this.lblPrinterName.Font = FontManager.Montserrat16Regular;
            this.lblPrinterName.Text = this._selectedPrinter.DisplayName;

            this.Select();

        }

        private void OnEnterChange()
        {
            plPrinterSummary.BackColor = Color.FromArgb(255, 24, 0);
            lblPrinterName.ForeColor = Color.White;
        }
        private void OnLeaveChange()
        {
            if (!this.Selected)
            {
                plPrinterSummary.BackColor = Color.White;
                lblPrinterName.ForeColor = Color.Black;
            }
        }

        private void OnControlClick()
        {
            this.onSelected?.Invoke(this, null);
            OnEnterChange();
        }

        private void PrinterSummaryControl_Click(object sender, EventArgs e)
        {
            OnControlClick();
        }

        private void lblPrinterName_Click(object sender, EventArgs e)
        {
            OnControlClick();
        }

        private void plPrinterSummary_Click(object sender, EventArgs e)
        {
            OnControlClick();
        }

        private void PrinterSummaryControl_MouseEnter(object sender, EventArgs e)
        {
            OnEnterChange();
        }

        private void PrinterSummaryControl_MouseLeave(object sender, EventArgs e)
        {
            OnLeaveChange();
        }

        private void lblPrinterName_MouseEnter_1(object sender, EventArgs e)
        {
            OnEnterChange();
        }

        private void lblPrinterName_MouseLeave_1(object sender, EventArgs e)
        {
            OnLeaveChange();
        }

        private void plPrinterSummary_MouseEnter(object sender, EventArgs e)
        {
            OnEnterChange();
        }

        private void plPrinterSummary_MouseLeave(object sender, EventArgs e)
        {
            OnLeaveChange();
        }
    }
}
