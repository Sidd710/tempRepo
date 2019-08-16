using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.Network;
using Atum.Studio.Core.Managers;
using Atum.DAL.Hardware;

namespace Atum.Studio.Controls
{
    public partial class PrintPropertiesControl : UserControl
    {
        public event EventHandler txtSliceThicknessChanged;

        public TypePrintProperty PrintPropertyType { get; set; }

        public enum TypePrintProperty
        {
            Printer = 10,
            Other = 99
        }

        private delegate AtumPrinter GetComboboxItem_Delegate();
        public AtumPrinter GetSelectedPrinter()
        {
            if (this.cbSelectedPrinters.InvokeRequired)
            {
                GetComboboxItem_Delegate gci = new GetComboboxItem_Delegate(GetSelectedPrinter);
                return (AtumPrinter)this.cbSelectedPrinters.Invoke(gci);
            }
            else
            {
                return (AtumPrinter)this.cbSelectedPrinters.SelectedItem;
            }
        }

        public PrintPropertiesControl()
        {
            InitializeComponent();

            this.tabControl1.Height += 30;
            this.tabControl1.Top -= 25;
            this.tabControl1.Left -= 5;
            this.tabControl1.Width += 10;
        }

        private void PrintSettingControl_Load(object sender, EventArgs e)
        {
         
            this.Width = this.Parent.Width;
            switch (this.PrintPropertyType)
            {
                case TypePrintProperty.Other:
                    {
                        this.lblHeaderText.Text = "Other";
                        this.tabControl1.TabPages.Remove(this.tbPrinter);
                        break;
                    }
                case TypePrintProperty.Printer:
                    {
                        this.lblHeaderText.Text = "Printer";
                        this.tabControl1.TabPages.Remove(this.tbOther);
               //         PrinterManager.AvailablePrinters.ListChanged += new ListChangedEventHandler(AvailablePrinters_ListChanged);

              //          this.UpdateAvailablePrinters();

                        break;
                    }
            }

            var currentTop = 0;
            foreach (Control control in this.tabControl1.SelectedTab.Controls)
            {
                if (control.Top > currentTop)
                {
                    currentTop = control.Top;
                }
            }

            this.Height = this.plHeader.Height + currentTop + 30;
        }

        private void plHeader_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;

            ////dashed line
            //float[] dashValues = { 2, 2 };
            //Pen dashedPen = new Pen(Color.Gainsboro, 1);
            //dashedPen.DashPattern = dashValues;
            //e.Graphics.DrawLine(dashedPen, new Point(1, this.plHeader.Height - 2), new Point(this.Width, this.plHeader.Height - 2));
        }

        private void txtSliceThickness_ValueChanged(object sender, EventArgs e)
        {
            txtSliceThicknessChanged(sender, e);
        }


        //private void RefreshMonitors()
        //{
        //    try
        //    {
        //        this.lstMonitors.Items.Clear();
        //        foreach (Screen screen in Screen.AllScreens)
        //        {
        //            this.lstMonitors.Items.Add(new ScreenListItem(screen));
        //        }
        //        if (this.lstMonitors.Items.Count > 0)
        //            this.lstMonitors.SelectedIndex = 0;
        //    }
        //    catch (Exception)
        //    {

        //    }

        //}

        private void btnWizard_Click(object sender, EventArgs e)
        {

            using (var printerManager = new PrinterConnectionManagerPopup())
            {
                var result = printerManager.ShowDialog();
                if (result == DialogResult.OK)
                {
                    PrinterManager.Save();
                }
            }
        }

        delegate void SetAvailablePrintersDataSource();
        void AvailablePrinters_ListChanged(object sender, ListChangedEventArgs e)
        {
            if (this.cbSelectedPrinters.InvokeRequired)
            {
                var d = new SetAvailablePrintersDataSource(UpdateAvailablePrinters);
                this.Invoke(d, new object[] { });
            }
            else
            {
                UpdateAvailablePrinters();

             
            }
        }

        void UpdateAvailablePrinters()
        {
            this.atumV2PrinterBindingSource.DataSource = null;
            this.atumV2PrinterBindingSource.DataSource = PrinterManager.SelectedPrinters;

            if (this.cbSelectedPrinters.Items.Count > 0)
            {
                foreach (var defaultPrinter in PrinterManager.SelectedPrinters)
                {
                    if (defaultPrinter.Default)
                    {
                        this.cbSelectedPrinters.SelectedItem = defaultPrinter;
                        break;
                    }
                }
            }
        }
    }
}
