using System;
using System.Drawing;
using System.Windows.Forms;
using Atum.DAL.Materials;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.MaterialDisplay
{
    public partial class displayMaterial : UserControl
    {

        public MaterialsBySupplier Material;
        public bool IsSelected { get; set; }
        public event EventHandler SelectionChanged;
        public displayMaterial(MaterialsBySupplier material)
        {
            InitializeComponent();

            this.lblSupplierName.Font = FontManager.Montserrat14Bold;
            this.lblMaterialDisplayName.Font = FontManager.Montserrat16Regular;
            this.plmain.BorderThickness = 2;
            this.plmain.SingleBorder = true;


            Material = material;
            UpdateControl();
        }
        public void UpdateControl()
        {
            this.lblSupplierName.Text = this.Material.Supplier;
            this.lblMaterialName.Text = this.Material.Materials[0].Name.Replace(@"\n", Environment.NewLine);

            if (this.Material.Materials[0].TechnicalSpecificationsAndResinVisibleProperties.Count > 0)
            {
                var resinProperty1 = this.Material.Materials[0].TechnicalSpecificationsAndResinProperties.ByType(this.Material.Materials[0].TechnicalSpecificationsAndResinVisibleProperties[0].TechnicalSpecificationOrResinProperty);
                this.lblResinPropertyName1.Text = resinProperty1.DisplayNameShort;
                this.lblResinPropertyValue1.Text = resinProperty1.DisplayValueShort;
            }
            else
            {
                this.lblResinPropertyName1.Text = this.lblResinPropertyValue1.Text = string.Empty;
            }

            if (this.Material.Materials[0].TechnicalSpecificationsAndResinVisibleProperties.Count > 1)
            {
                var resinProperty2 = this.Material.Materials[0].TechnicalSpecificationsAndResinProperties.ByType(this.Material.Materials[0].TechnicalSpecificationsAndResinVisibleProperties[1].TechnicalSpecificationOrResinProperty);
                this.lblResinPropertyName2.Text = resinProperty2.DisplayNameShort;
                this.lblResinPropertyValue2.Text = resinProperty2.DisplayValueShort;
            }
            else
            {
                this.lblResinPropertyName2.Text = this.lblResinPropertyValue2.Text = string.Empty;
            }

            this.picColor.BackColor = this.Material.Materials[0].ModelColor == null ? Color.Gray : this.Material.Materials[0].ModelColor;
            this.picColor.BorderColor = BrandingManager.Button_ForeColor_Dark;
            this.picColor.BorderThickness = 1;
            this.IsSelected = false;
        }
        
        private void ChangeSelection()
        {
            if (this.IsSelected)
            {
                this.plmain.BorderColor = Color.WhiteSmoke;
            }
            else
            {
                this.plmain.BorderColor = BrandingManager.Menu_Item_HighlightColor;
            }
            this.IsSelected = !this.IsSelected;
            SelectionChanged?.Invoke(this, null);
        }

        private void displayMaterial_Click(object sender, EventArgs e)
        {
            ChangeSelection();
        }
        private void plmain_Click(object sender, EventArgs e)
        {
            ChangeSelection();
        }

        private void tableLayoutPanel1_Click(object sender, EventArgs e)
        {
            ChangeSelection();
        }

        private void lblResinPropertyName2_Click(object sender, EventArgs e)
        {
            ChangeSelection();
        }
    }
}
