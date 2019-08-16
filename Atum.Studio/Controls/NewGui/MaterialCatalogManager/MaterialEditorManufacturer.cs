using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.MaterialCatalogManager;

namespace Atum.Studio.Controls.MaterialEditor
{
    public partial class MaterialEditorManufacturer : UserControl
    {
        private MaterialsBySupplier _supplier;
        public event EventHandler<MaterialSupplierDisplayNameArgs> DisplayNameChanged;

        public MaterialsBySupplier CurrentSupplier
        {
            get
            {
                return this._supplier;
            }
            set
            {
                this._supplier = value;
                if (value != null)
                {
                    this.materialsBySupplierBindingSource.DataSource = this._supplier;
                }
            }
        }

        public MaterialEditorManufacturer()
        {
            InitializeComponent();
        }

        private void txtSupplier_TextChanged(object sender, EventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(this.txtSupplier.Text, @"^[\w\-. ]+$"))
            {
                this.errorProvider1.SetError(this.txtSupplier, "The manufacturer name can only contain the following characters:\r\n - '-' (minus)\r\n - '.' (dot)\r\n - '' (space)");
            }
            else
            {
                this.errorProvider1.SetError(this.txtSupplier, string.Empty);
                this.DisplayNameChanged?.Invoke(null, new MaterialSupplierDisplayNameArgs() { DisplayName = this.txtSupplier.Text });
            }
        }
    }
}
