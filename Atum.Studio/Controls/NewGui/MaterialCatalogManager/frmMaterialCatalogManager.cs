using Atum.DAL.ApplicationSettings;
using Atum.DAL.Materials;
using Atum.Studio.Controls.MaterialEditor;
using Atum.Studio.Controls.NewGui.MaterialCatalogManager;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.MaterialCatalogEditor
{
    public partial class frmMaterialCatalogManager : NewGUIFormBase
    {
        private bool _magsAITabHidden = false;

        public bool ToolbarXYResolutionVisible
        {
            get
            {
                return !this.splitContainer1.Panel1Collapsed;
            }
            set
            {
                this.splitContainer1.Panel1Collapsed = !value;
            }
        }

        public bool TabMagsAIVisible
        {
            get
            {
                return !_magsAITabHidden;
            }
            set
            {
                _magsAITabHidden = !value;
            }
        }

        public frmMaterialCatalogManager()
        {
            InitializeComponent();
            this.plLine.BackColor = BrandingManager.Button_BackgroundColor_Dark;

            this.FormBorderStyle = FormBorderStyle.None;
            this.Text = "Material Catalog Manager";
            FilterMaterialsFromCatalog();

            this.splitContainer2.Panel2.Controls.Clear();
        }

        private void rbMaterialResolution100_CheckedChanged(object sender, EventArgs e)
        {
            FilterMaterialsFromCatalog();

            this.splitContainer2.Panel2.Controls.Clear();
        }

        private void rbMaterialResolution75_CheckedChanged(object sender, EventArgs e)
        {
            FilterMaterialsFromCatalog();

            this.splitContainer2.Panel2.Controls.Clear();
        }

        private void rbMaterialResolution50_CheckedChanged(object sender, EventArgs e)
        {
            FilterMaterialsFromCatalog();

            this.splitContainer2.Panel2.Controls.Clear();
        }

        private int GetSelectedResolutionAsInt()
        {
            var xyResolutionAsInt = 100;

            if (this.rbMaterialResolution50.Checked)
            {
                xyResolutionAsInt = 50;
            }
            else if (this.rbMaterialResolution75.Checked)
            {
                xyResolutionAsInt = 75;
            }

            return xyResolutionAsInt;
        }

        private void FilterMaterialsFromCatalog()
        {
            this.trCatalog.Nodes.Clear();

            try
            {
                //disble events
                var xyResolutionAsInt = GetSelectedResolutionAsInt();

                foreach (var supplier in MaterialManager.Catalog)
                {
                    var listItems = new List<Material>();
                        var supplierTreeNode = new TreeNode(supplier.Supplier) { Tag = supplier };
                        foreach (var material in supplier.Materials)
                        {
                            if (material.XYResolution == xyResolutionAsInt)
                            {
                                listItems.Add(material);
                            }
                        }

                        //order by name
                        var materialOrdered = (listItems.OrderBy(s => s.Name)).ToArray();
                        foreach (var material in materialOrdered)
                        {
                            var materialTreeNode = new TreeNode(material.Name) { Tag = material };
                            supplierTreeNode.Nodes.Add(materialTreeNode);
                        }

                        this.trCatalog.Nodes.Add(supplierTreeNode);
                }

                this.trCatalog.ExpandAll();
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.StackTrace);
            }

            UpdateControls();
        }

        private void frmMaterialCatalogManager_Load(object sender, EventArgs e)
        {
        }

        private void txtResinPrice_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // only allow one decimal point
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void trCatalog_AfterSelect(object sender, TreeViewEventArgs e)
        {
            foreach (ToolStripItem button in this.toolStrip1.Items)
            {
                button.Visible = false;
            }


            foreach (TreeNode rootNode in this.trCatalog.Nodes)
            {
                rootNode.BackColor = this.trCatalog.BackColor;
                rootNode.ForeColor = this.trCatalog.ForeColor;
                foreach (TreeNode supplierNode in rootNode.Nodes)
                {
                    supplierNode.BackColor = this.trCatalog.BackColor;
                    supplierNode.ForeColor = this.trCatalog.ForeColor;
                    foreach (TreeNode materialNode in supplierNode.Nodes)
                    {
                        materialNode.BackColor = this.trCatalog.BackColor;
                        materialNode.ForeColor = this.trCatalog.ForeColor;
                    }
                }
            }

            this.trCatalog.SelectedNode.BackColor = SystemColors.Highlight;
            this.trCatalog.SelectedNode.ForeColor = SystemColors.HighlightText;
            switch (this.trCatalog.SelectedNode.Level)
            {
                case 0:
                    var materialEditorManufacturer = new MaterialEditorManufacturer();
                    if (!(this.splitContainer2.Panel2.Controls.Count > 0 && this.splitContainer2.Panel2.Controls[0] is MaterialEditorManufacturer))
                    {
                        this.splitContainer2.Panel2.Controls.Clear();
                        materialEditorManufacturer.Dock = DockStyle.Fill;
                        this.splitContainer2.Panel2.Controls.Add(materialEditorManufacturer);
                        materialEditorManufacturer.DisplayNameChanged += MaterialEditorManufacturer_DisplayNameChanged;
                    }
                    else
                    {
                        materialEditorManufacturer = this.splitContainer2.Panel2.Controls[0] as MaterialEditorManufacturer;
                    }

                    materialEditorManufacturer.CurrentSupplier = e.Node.Tag as MaterialsBySupplier;


                    break;
                case 1:

                    var materialEditorMaterial = new MaterialEditorMaterial();
                    materialEditorMaterial.TabMagsAIVisible = !this._magsAITabHidden;
                    if (!(this.splitContainer2.Panel2.Controls.Count > 0 && this.splitContainer2.Panel2.Controls[0] is MaterialEditorMaterial))
                    {
                        this.splitContainer2.Panel2.Controls.Clear();
                        materialEditorMaterial.Dock = DockStyle.Fill;
                        this.splitContainer2.Panel2.Controls.Add(materialEditorMaterial);
                        materialEditorMaterial.NameChanged += materialEditorMaterial_NameChanged; ;
                    }
                    else
                    {
                        materialEditorMaterial = this.splitContainer2.Panel2.Controls[0] as MaterialEditorMaterial;
                    }

                    var materialsBySupplier = e.Node.Tag as Material;
                    materialEditorMaterial.LoadSelectedMaterial(e.Node.Tag as Material, e.Node.Parent.Tag as MaterialsBySupplier);

                    break;
            }

            this.UpdateControls();
        }

        private void MaterialEditorManufacturer_DisplayNameChanged(object sender, MaterialCatalogManager.MaterialSupplierDisplayNameArgs e)
        {
            if (e.DisplayName.Length > 0 && System.Text.RegularExpressions.Regex.IsMatch(e.DisplayName, @"^[\w\-. ]+$"))
            {
                this.btnManufacturerSave.Enabled = true;
                this.trCatalog.SelectedNode.Text = e.DisplayName;
            }
            else
            {
                this.btnManufacturerSave.Enabled = false;
            }
        }

        private void materialEditorMaterial_NameChanged(object sender, MaterialCatalogManager.MaterialDisplayNameArgs e)
        {
            if (this.trCatalog.SelectedNode != null)
            {
                this.trCatalog.SelectedNode.Text = e.DisplayName;
            }
        }

        private void btnManufacturerAdd_Click(object sender, EventArgs e)
        {
            var newSupplier = new MaterialsBySupplier() { Supplier = "New manufacturer" };
            var catalogNode = this.trCatalog.Nodes;
            var newSupplierNode = new TreeNode() { Text = newSupplier.Supplier, Tag = newSupplier };
            MaterialManager.Catalog.Add(newSupplier);
            catalogNode.Add(newSupplierNode);

            //add new node to treeview
            this.trCatalog.HideSelection = false;
            this.trCatalog.SelectedNode = newSupplierNode;

            this.btnManufacturerSave.Enabled = true;
        }

        private Material GenerateNewMaterial()
        {
            var newMaterial = new Material()
            {
                Name = "New material name",
                DisplayName = "New displayname",
                LT1 = 0.05f,
                LT2 = 0.05f,
                RH1 = 8,
                RH2 = 8,
                RSU1 = 150,
                RSU2 = 150,
                RSD1 = 150,
                RSD2 = 150,
                CT1 = 8,
                CT2 = 1,
                Id = Guid.NewGuid(),
                InitialLayers = 2,
                LightIntensityPercentage1 = 100,
                LightIntensityPercentage2 = 100,
                XYResolution = GetSelectedResolutionAsInt()
            };


#if LOCTITE
            newMaterial.PrinterHardwareType = new DAL.Hardware.LoctiteV10().PrinterHardwareType;
#else
            newMaterial.PrinterHardwareType = new DAL.Hardware.AtumDLPStation5().PrinterHardwareType;
#endif

            return newMaterial;
        }

        private void btnMaterialAdd_Click(object sender, EventArgs e)
        {
            var currentSupplier = this.trCatalog.SelectedNode.Tag as MaterialsBySupplier;
            var newMaterialNode = new TreeNode("<EMPTY>");
            newMaterialNode.Tag = GenerateNewMaterial();

            //add new node to treeview
            this.trCatalog.HideSelection = false;
            this.trCatalog.SelectedNode.Nodes.Add(newMaterialNode);
            this.trCatalog.SelectedNode = newMaterialNode;
        }

        private void btnManufacturerSave_Click(object sender, EventArgs e)
        {
            var supplier = this.trCatalog.SelectedNode.Tag as MaterialsBySupplier;

            if (!string.IsNullOrEmpty(supplier.FilePath))
            {
                try
                {
                    File.Delete(supplier.FilePath);
                }
                catch
                {

                }
            }

            MaterialManager.SaveMaterial(supplier);
        }

        private void btnManufacturerRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to remove: {0}?", this.trCatalog.SelectedNode.Text), "Remove selected manufacturer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                var supplierNode = this.trCatalog.SelectedNode;
                var supplier = supplierNode.Tag as MaterialsBySupplier;

                this.trCatalog.SelectedNode.Remove();

                MaterialManager.RemoveSupplier(supplier);
            }
        }

        private void btnMaterialSave_Click(object sender, EventArgs e)
        {
            SaveSelectedMaterial();
        }

        private void btnMaterialRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to remove: {0}?", this.trCatalog.SelectedNode.Text), "Remove selected material", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                var supplierNode = this.trCatalog.SelectedNode.Parent;
                var supplier = supplierNode.Tag as MaterialsBySupplier;
                supplier.Materials.Remove(this.trCatalog.SelectedNode.Tag as Material);
                this.trCatalog.SelectedNode.Remove();
                this.trCatalog.SelectedNode = supplierNode;

                supplier.SaveToFile();
            }
        }

        void CurrentMaterial_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.btnMaterialSave.Enabled = true;
        }

        private void UpdateControls()
        {
            foreach (ToolStripItem item in this.toolStrip1.Items)
            {
                if (!(item is ToolStripLabel))
                {
                    item.Enabled = false;
                }

                item.Visible = false;
            }

            if (this.trCatalog.SelectedNode != null)
            {
                switch (this.trCatalog.SelectedNode.Level)
                {
                    case 0:
                        this.btnMaterialAdd.Enabled = true;
                        this.btnManufacturerAdd.Enabled = true;
                        this.btnManufacturerRemove.Enabled = true;

                        this.lblManufacturer.Visible = true;
                        this.btnManufacturerAdd.Visible = true;
                        this.btnManufacturerSave.Visible = true;
                        this.btnManufacturerRemove.Visible = true;

                        this.lblMaterial.Visible = true;
                        this.btnMaterialAdd.Visible = true;

                        break;
                    case 1:
                        this.btnMaterialRemove.Enabled = true;

                        this.btnMaterialSave.Enabled = true;

                        this.lblMaterial.Visible = true;
                        this.btnMaterialAdd.Visible = true;
                        this.btnMaterialSave.Visible = true;
                        this.btnMaterialRemove.Visible = true;

                        break;
                }
            }
            else
            {
                this.btnMaterialAdd.Enabled = true;
                this.btnManufacturerAdd.Enabled = true;
                this.btnManufacturerRemove.Enabled = true;

                this.lblManufacturer.Visible = true;
                this.btnManufacturerAdd.Visible = true;
                this.btnManufacturerSave.Visible = true;
                this.btnManufacturerRemove.Visible = true;

                this.lblMaterial.Visible = true;
                this.btnMaterialAdd.Visible = true;
            }
        }

        private void SaveSelectedMaterial()
        {
            var selectedMaterialControl = splitContainer2.Panel2.Controls[0] as MaterialEditorMaterial;
            selectedMaterialControl.SaveSelectedMaterial();
            
        }
    }
}
