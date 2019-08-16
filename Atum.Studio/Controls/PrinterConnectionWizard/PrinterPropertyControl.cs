using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.DAL.Hardware;

namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    public partial class PrinterPropertyControl : UserControl
    {
        private AtumPrinterProperty _printerProperty;

        public bool HideBorder { get; set; }
        public PrinterPropertyControl()
        {
            InitializeComponent();
        }

        public AtumPrinterProperty PrinterProperty
        {
            get
            {
                foreach (var valueItem in this._printerProperty.Values)
                {
                    valueItem.Selected = false;
                }
                foreach (var control in this.gpHeader.Controls)
                {
                    if (control is RadioButton)
                    {
                        var radiobutton = (RadioButton)control;
                        if (radiobutton.Checked)
                        {
                            foreach (var valueItem in this._printerProperty.Values)
                            {
                                if (valueItem.Value == radiobutton.Tag)
                                {
                                    valueItem.Selected = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                return this._printerProperty;
            }
            set
            {
                this._printerProperty = value;
                this.gpHeader.Text = value.Name;

                if (value.Values != null)
                {
                    var top = 20;
                    foreach (var valueItem in value.Values)
                    {
                        var rdValue = new RadioButton();
                        rdValue.Left = 10;
                        rdValue.Text = valueItem.Text;
                        rdValue.Tag = valueItem.Value;
                        rdValue.Checked = valueItem.Selected;
                        rdValue.Top = top;
                        this.gpHeader.Controls.Add(rdValue);

                        top += rdValue.Height;

                        this.gpHeader.Height = top + 5;
                    }

                    //check if one is checked
                    var oneCheckedItem = false;
                    foreach (var control in this.gpHeader.Controls)
                    {
                        if (control is RadioButton)
                        {
                            if (((RadioButton)control).Checked)
                            {
                                oneCheckedItem = true;
                                break;
                            }
                        }
                    }

                    if (!oneCheckedItem)
                    {
                        foreach (var control in this.gpHeader.Controls)
                        {
                            if (control is RadioButton)
                            {
                                ((RadioButton)control).Checked = true;
                                break;
                            }
                        }
                    }
                }
                
            }
        }

        private void PrinterPropertyControl_Load(object sender, EventArgs e)
        {

        }

        private void gpHeader_Paint(object sender, PaintEventArgs e)
        {
            if (!this.HideBorder)
            {
                base.OnPaint(e);
            }
            else
            {
                e.Graphics.Clear(Color.White);
            }
        }
    }
}
