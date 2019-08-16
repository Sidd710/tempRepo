using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public class GenericComboxRegular16Disabled: Panel
    {
        
        private object _selectedItem;

        internal Object SelectedItem
        {
            get
            {
                return this._selectedItem;
            }
            set
            {
                this._selectedItem = value;

                if (value is AtumPrinter)
                {
                    foreach(Control control in this.Controls)
                    {
                        if (!(control is Panel))
                        {
                            this.Controls.Remove(control);
                        }
                    }

                    var selectedPrinter = value as AtumPrinter;
                    var printerLabel = new GenericLabelRegular16();
                    printerLabel.Left = 8;
                    printerLabel.Top = 10;
                    printerLabel.Width = 200;
                    printerLabel.Text = selectedPrinter.DisplayName.Replace(" (*)", string.Empty);
                    this.Controls.Add(printerLabel);
                }
                else if (value is MaterialSummary)
                {
                    foreach (Control control in this.Controls)
                    {
                        if (!(control is Panel))
                        {
                            this.Controls.Remove(control);
                        }
                    }

                    var materialSummary = value as MaterialSummary;
                    materialSummary.BackColor = this.BackColor;
                    materialSummary.Top += 1;
                    materialSummary.Left -= 4;
                    materialSummary.DisableHighlight = true;
                    materialSummary.DisableMouseClick = true;
                    this.Controls.Add(materialSummary);
                }
                else if (value is Material)
                {
                    foreach (Control control in this.Controls)
                    {
                        if (!(control is Panel))
                        {
                            this.Controls.Remove(control);
                        }
                    }

                    var materialSummary = new MaterialSummary() { Material = (Material)value } ;
                    materialSummary.BackColor = this.BackColor;
                    materialSummary.Top += 2;
                    materialSummary.Left -= 4;
                    materialSummary.DisableHighlight = true;
                    this.Controls.Add(materialSummary);
                }
            }
        }

        public GenericComboxRegular16Disabled()
        {
            this.BackColor = Color.White;
            this.Width = 240;
            this.Height = 40;
            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat16Regular;
            }
            else
            {
                this.Font = new Font(FontFamily.GenericSerif, 16, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }
        
    }
}
