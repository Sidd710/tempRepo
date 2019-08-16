using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.MaterialDisplay;
using Atum.Studio.Core.Managers;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    public partial class MaterialMenuStrip : UserControl
    {
        public event EventHandler onMaterialAdded;
        public MaterialMenuStrip()
        {
            InitializeComponent();

            this.plBackground.BackColor = BrandingManager.Menu_BackgroundColor;
            this.btnImport.Font = this.btnAddCatalog.Font = FontManager.Montserrat14Regular;
        }

        private void btnAddCatalog_Click(object sender, EventArgs e)
        {
            using (var frmMaterialDisplay = new frmMaterialOnlineCatalog())
            {
                frmMaterialDisplay.onMaterialAdded += FrmMaterialDisplay_onMaterialAdded;
                frmMaterialDisplay.ShowDialog();
            }
        }

        private void FrmMaterialDisplay_onMaterialAdded(object sender, EventArgs e)
        {
            var materialDisplay = sender as frmMaterialOnlineCatalog;
            this.onMaterialAdded?.Invoke(materialDisplay, null);
        }

        private void btnAddCatalog_MouseHover(object sender, EventArgs e)
        {
            btnAddCatalog.BackColor = BrandingManager.Menu_Item_HighlightColor;
        }

        private void btnAddCatalog_MouseLeave(object sender, EventArgs e)
        {
            btnAddCatalog.BackColor = BrandingManager.Menu_BackgroundColor;
        }

        private void btnImport_MouseHover(object sender, EventArgs e)
        {
            btnImport.BackColor = BrandingManager.Menu_Item_HighlightColor;
        }

        private void btnImport_MouseLeave(object sender, EventArgs e)
        {
            btnImport.BackColor = BrandingManager.Menu_BackgroundColor;
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
                                    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
                                    using (var materialReader = new StreamReader(fileStream))
                                    {
                                        var materialsBySupplier = (MaterialsBySupplier)serializer.Deserialize(materialReader);

                                        var catalog = MaterialManager.Catalog.Where(x => x.Supplier == materialsBySupplier.Supplier).FirstOrDefault();
                                        if (catalog != null)
                                        {
                                            foreach (var material in materialsBySupplier.Materials)
                                            {
                                                material.Id = Guid.NewGuid();
                                                catalog.Materials.Add(material);
                                                var materialWithSupplier = new MaterialWithSupplierModel()
                                                {
                                                    Supplier = materialsBySupplier.Supplier,
                                                    Material = material
                                                };
                                                this.onMaterialAdded?.Invoke(materialWithSupplier, null);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
        }
    }
}
