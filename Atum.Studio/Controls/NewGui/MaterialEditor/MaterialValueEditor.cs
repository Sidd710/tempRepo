using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.DAL.Materials;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    public partial class MaterialValueEditor : UserControl
    {
        public event EventHandler onTextChanged;
        private Material _material;
        private string _supplier;
        private bool isFromControlChange = false;
        public string Supplier
        {
            get
            {
                return this._supplier;
            }
            set
            {
                this._supplier = value;
            }
        }

        public Material Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
                this.UpdateControl();
            }
        }

        public MaterialValueEditor()
        {
            InitializeComponent();

            if (FontManager.Loaded)
            {
                this.lblMaterialName.Font = this.lblSupplierName.Font = this.lblDisplayName.Font = FontManager.Montserrat14Bold;
                this.lblMaterialValue.Font = this.lblSupplierValue.Font = FontManager.Montserrat14Regular;
                this.txtMaterialDisplayName1.Font = FontManager.Montserrat16Regular;
                this.lblSupplierValue.Text = this._supplier;
            }
        }

        private void UpdateControl()
        {
            this.isFromControlChange = true;
            if (this._supplier != null)
            {
                this.lblSupplierValue.Text = this._supplier;
            }

            if (this._material != null)
            {
                if (this.txtMaterialDisplayName1.Text.Length > 16)
                {
                    this.Material.DisplayName = this.txtMaterialDisplayName1.Text.Substring(0, 16);
                }

                this.lblMaterialValue.Text = this._material.Name.Replace(@"\n", " ");
                this.txtMaterialDisplayName1.Text = this._material.DisplayName;

                if (this._material.TechnicalSpecificationsAndResinVisibleProperties.Count > 0)
                {
                    var resinProperty1 = this._material.TechnicalSpecificationsAndResinProperties.ByType(this._material.TechnicalSpecificationsAndResinVisibleProperties[0].TechnicalSpecificationOrResinProperty);
                    this.lblTechnicalPropertyName1.Text = resinProperty1.DisplayName;
                    this.lblTechnicalPropertyValue1.Text = resinProperty1.DisplayValue;
                }
                else
                {
                    this.lblTechnicalPropertyName1.Text = this.lblTechnicalPropertyValue1.Text = string.Empty;
                }

                if (this._material.TechnicalSpecificationsAndResinVisibleProperties.Count > 1)
                {
                    var resinProperty2 = this._material.TechnicalSpecificationsAndResinProperties.ByType(this._material.TechnicalSpecificationsAndResinVisibleProperties[1].TechnicalSpecificationOrResinProperty);
                    this.lblTechnicalPropertyName2.Text = resinProperty2.DisplayName;
                    this.lblTechnicalPropertyValue2.Text = resinProperty2.DisplayValue;
                }
                else
                {
                    this.lblTechnicalPropertyName2.Text = this.lblTechnicalPropertyValue2.Text = string.Empty;
                }
            }
            this.isFromControlChange = false;
        }

        private void txtMaterialDisplayName1_TextChanged(object sender, EventArgs e)
        {
            if (!isFromControlChange)
            {
                this.Material.DisplayName = this.txtMaterialDisplayName1.Text;
                this.onTextChanged?.Invoke(this, null);

            }
            else
            {
                this.isFromControlChange = false;
            }
        }

        private void btnExportSettings_Click(object sender, EventArgs e)
        {
            if (Material != null && Supplier != null)
            {
                MaterialsBySupplier materialsBySupplier = new MaterialsBySupplier();
                materialsBySupplier.Supplier = Supplier;
                materialsBySupplier.Materials.Add(Material);

                using (var saveDialog = new SaveFileDialog())
                {
                    saveDialog.AddExtension = true;
                    saveDialog.FileName = Material.Name.Replace(" ", "") + ".xml";
                    saveDialog.DefaultExt = ".xml";
#if LOCTITE
                saveDialog.Filter = "Material Settings File(*.xml)|*.xml";
#else
                    saveDialog.Filter = "Material Settings File(*.xml)|*.xml";
#endif
                    if (saveDialog.ShowDialog() == DialogResult.OK && !string.IsNullOrEmpty(saveDialog.FileName))
                    {
                        var _serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
                        using (var streamWriter = new System.IO.StreamWriter(saveDialog.FileName, false))
                        {
                            _serializer.Serialize(streamWriter, materialsBySupplier);
                            streamWriter.Close();
                        }
                    }
                }
            }
        }

        private void txtMaterialDisplayName1_Enter(object sender, EventArgs e)
        {
            TouchScreenManager.ShowOSK(txtMaterialDisplayName1);
        }

        private void txtMaterialDisplayName1_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(txtMaterialDisplayName1);
        }
    }
}
