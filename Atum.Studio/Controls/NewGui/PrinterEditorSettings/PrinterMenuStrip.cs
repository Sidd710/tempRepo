using Atum.DAL.Hardware;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    public partial class PrinterMenuStrip : UserControl
    {
        public event EventHandler onPrinterAdded;
        public event EventHandler onAddNewDLPStation2Call;
        public event EventHandler onAddNewDLPStation5Call;
        public PrinterMenuStrip()
        {
            InitializeComponent();

            this.plPrinterFlyout.BackColor = BrandingManager.Menu_BackgroundColor;
            this.btnAddNewDLPStation2.Font = this.btnAddNewDLPStation5.Font = this.btnImport.Font = FontManager.Montserrat14Regular;
            this.btnAddNewDLPStation2.BackColor = this.btnAddNewDLPStation5.BackColor = BrandingManager.Menu_BackgroundColor;
            this.btnImport.BackColor = BrandingManager.Menu_BackgroundColor;
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            using (var popup = new OpenFileDialog())
            {
                popup.Filter = "(*.xml)|*.xml|(*.XML)|*.XML";
                popup.Multiselect = false;
                Stream fileStream = null;
                if (popup.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(popup.FileName))
                {
                    if (File.Exists(popup.FileName))
                    {
                        try
                        {
                            if ((fileStream = popup.OpenFile()) != null)
                            {
                                using (fileStream)
                                {
                                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(BindingList<AtumPrinter>));
                                    using (var streamReader = new StreamReader(fileStream, false))
                                    {

                                        var printersToImport = (BindingList<AtumPrinter>)serializer.Deserialize(streamReader);
                                        foreach (var printer in printersToImport)
                                        {
                                            PrinterManager.AvailablePrinters.Add(printer);
                                            //PrinterManager.Save();
                                            this.onPrinterAdded?.Invoke(printer, null);
                                        }  
                                    }
                                }
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                }
            }
            this.Hide();
        }

        private void btnAddNewDLPStation2_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.onAddNewDLPStation2Call?.Invoke(true, null);
        }

        private void btnAddNewDLPStation2_MouseEnter(object sender, EventArgs e)
        {
            btnAddNewDLPStation2.BackColor = BrandingManager.Menu_Item_HighlightColor;
        }

        private void btnAddNewDLPStation2_MouseLeave(object sender, EventArgs e)
        {
            btnAddNewDLPStation2.BackColor = BrandingManager.Menu_BackgroundColor;
        }


        private void btnAddNewDLPStation5_Click(object sender, EventArgs e)
        {            
            this.Hide();
            this.onAddNewDLPStation5Call?.Invoke(true, null);
        }

        private void btnAddNewDLPStation5_MouseEnter(object sender, EventArgs e)
        {
            btnAddNewDLPStation5.BackColor = BrandingManager.Menu_Item_HighlightColor;
        }

        private void btnAddNewDLPStation5_MouseLeave(object sender, EventArgs e)
        {
            btnAddNewDLPStation5.BackColor = BrandingManager.Menu_BackgroundColor;
        }

        private void btnImport_MouseEnter(object sender, EventArgs e)
        {
            btnImport.BackColor = BrandingManager.Menu_Item_HighlightColor;
        }

        private void btnImport_MouseLeave(object sender, EventArgs e)
        {
            btnImport.BackColor = BrandingManager.Menu_BackgroundColor;
        }
    }
}
