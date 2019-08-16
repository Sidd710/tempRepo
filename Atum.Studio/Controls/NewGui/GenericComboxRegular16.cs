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
    public class GenericComboxRegular16 : Panel
    {

        internal event EventHandler DropdownButtonClicked;
        private Panel _openDropdownButton;

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
                    foreach (Control control in this.Controls)
                    {
                        if (!(control is Panel))
                        {
                            if (this.InvokeRequired)
                            {
                                this.Invoke(new MethodInvoker(delegate
                                {
                                    this.Controls.Remove(control);
                                }));
                            }
                            else
                            {
                                this.Controls.Remove(control);
                            }
                        }
                    }

                    var selectedPrinter = value as AtumPrinter;
                    var printerLabel = new GenericLabelRegular16();
                    printerLabel.AutoSize = false;
                    printerLabel.Left = 8;
                    printerLabel.Top = 10;
                    printerLabel.Width = 200;
                    printerLabel.Click += PrinterName_Selected;
                    printerLabel.Text = selectedPrinter.DisplayName.Replace(" (*)", string.Empty);

                    try
                    {
                        if (this.InvokeRequired)
                        {
                            this.Invoke(new MethodInvoker(delegate
                            {
                                this.Controls.Add(printerLabel);
                            }));
                        }
                        else
                        {
                            this.Controls.Add(printerLabel);
                        }
                    }
                    catch(Exception exc)
                    {

                    }
                    
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
                    materialSummary.Top += 2;
                    materialSummary.Left -= 4;
                    materialSummary.DisableHighlight = true;
                    materialSummary.onSelected += MaterialSummary_onSelected;
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

                    var materialSummary = new MaterialSummary() { Material = (Material)value };
                    materialSummary.BackColor = this.BackColor;
                    materialSummary.Top += 2;
                    materialSummary.Left -= 4;
                    materialSummary.DisableHighlight = true;
                    materialSummary.onSelected += MaterialSummary_onSelected;
                    this.Controls.Add(materialSummary);
                }

                this.UpdateControl();
            }
        }

        private void PrinterName_Selected(object sender, EventArgs e)
        {
            this.DropdownButtonClicked?.Invoke(null, null);
        }

        private void MaterialSummary_onSelected(object sender, EventArgs e)
        {
            this.DropdownButtonClicked?.Invoke(null, null);
        }

        public GenericComboxRegular16()
        {
            this.BackColor = Color.White;
            this.Height = 40;
            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat16Regular;
            }
            else
            {
                this.Font = new Font(FontFamily.GenericSerif, 16, FontStyle.Regular, GraphicsUnit.Pixel);
            }

            this._openDropdownButton = new Panel();
            this._openDropdownButton.Width = 18;
            this._openDropdownButton.Height = 18;

            this._openDropdownButton.BackgroundImage = Properties.Resources.toolbar_combobo_down;
            this._openDropdownButton.BackgroundImageLayout = ImageLayout.Center;
            this._openDropdownButton.Click += _openDropdownButton_Click;

            this.Controls.Add(this._openDropdownButton);

            this.UpdateControl();
        }

        private void UpdateControl()
        {
            this._openDropdownButton.Left = this.Width - this._openDropdownButton.Width - 8;
            this._openDropdownButton.Top = (this.Height / 2) - (_openDropdownButton.Height / 2);
        }

        private void _openDropdownButton_Click(object sender, EventArgs e)
        {
            var buttonRectangle = this.PointToScreen(this._openDropdownButton.Location);
            this.DropdownButtonClicked?.Invoke(buttonRectangle, null);
        }
    }
}
