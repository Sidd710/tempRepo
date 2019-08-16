using Atum.DAL.Materials;
using Atum.Studio.Controls;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Linq;
using System.Windows.Forms;
using Atum.Studio.Controls.NewGui.MaterialCatalogManager;

namespace Atum.Studio.Controls.MaterialEditor
{
    public partial class frmMaterialEditorPopup : BasePopup
    {
        private DAL.Hardware.AtumPrinter _selectedPrinter;

        public DAL.Hardware.AtumPrinter SelectedPrinter
        {
            get
            {
                return this._selectedPrinter;
            }
            set
            {
                this._selectedPrinter = value;

                this.RefreshCatalog();
            }

        }

        public Material SelectedMaterial
        {
            set
            {
                if (value != null)
                {
                    foreach (TreeNode rootNode in this.trCatalog.Nodes)
                    {
                        foreach (TreeNode supplierNode in rootNode.Nodes)
                        {
                            foreach (TreeNode materialNode in supplierNode.Nodes)
                            {
                                if (materialNode.Tag == value)
                                {
                                    this.trCatalog.SelectedNode = materialNode;
                                    var treeviewEvent = new TreeViewEventArgs(materialNode);
                                    this.trCatalog_AfterSelect(materialNode, treeviewEvent);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        public frmMaterialEditorPopup()
        {
            InitializeComponent();

            _materialCatalogOnline.DownloadMaterialsCompleted += materialCatalogOnline_DownloadMaterialsCompleted;


            this.cbSelectedPrinter.Items.Clear();

            var selectedIndex = 0;
            for (var printerIndex = 0; printerIndex < PrinterManager.AvailablePrinters.Count; printerIndex++)
            {
                if (PrinterManager.SelectedPrinters[printerIndex].Default)
                {
                    selectedIndex = printerIndex;
                }

                this.cbSelectedPrinter.Items.Add(PrinterManager.SelectedPrinters[printerIndex]);
            }

            if (this.cbSelectedPrinter.Items.Count > 0) this.cbSelectedPrinter.SelectedIndex = selectedIndex;

        }

        private void RefreshCatalog()
        {
            this.trCatalog.Nodes.Clear();
            this.trCatalog.Nodes.Add("Catalog");
            if (MaterialManager.Catalog != null)
            {
                foreach (var supplier in MaterialManager.Catalog)
                {
                    var selectedPrinterResolution = this.SelectedPrinter.PrinterXYResolutionAsInt;
                    //filter on printer type
                    if (!(this.SelectedPrinter is DAL.Hardware.AtumDLPStation5) && !(this.SelectedPrinter is DAL.Hardware.LoctiteV10))
                    {
                        //un filtered
                        var supplierNode = new TreeNode(supplier.Supplier);
                        supplierNode.Tag = supplier;
                        this.trCatalog.Nodes[0].Nodes.Add(supplierNode);

                        foreach (var material in supplier.Materials.Where(m => string.IsNullOrEmpty(m.PrinterHardwareType)))
                        {
                            var materialDisplayName = material.DisplayName.Replace(" (*)", string.Empty);
                            var materialNode = new TreeNode(materialDisplayName + (material.IsDefault ? " (*)" : ""));
                            materialNode.Tag = material;
                            supplierNode.Nodes.Add(materialNode);
                        }
                    }
                    else
                    {
                        //filter on hardwaretype and resolution
                        var amountOfMaterialBySelectedPrinterTypeAndResolution = supplier.Materials.Count(m => m.XYResolution == selectedPrinterResolution && m.PrinterHardwareType == this.SelectedPrinter.PrinterHardwareType);

                        if (amountOfMaterialBySelectedPrinterTypeAndResolution > 0)
                        {
                            var supplierNode = new TreeNode(supplier.Supplier);
                            supplierNode.Tag = supplier;
                            this.trCatalog.Nodes[0].Nodes.Add(supplierNode);

                            foreach (var material in supplier.Materials.Where(m => m.XYResolution == selectedPrinterResolution && m.PrinterHardwareType == this.SelectedPrinter.PrinterHardwareType))
                            {
                                var materialDisplayName = material.DisplayName.Replace(" (*)", string.Empty);
                                var materialNode = new TreeNode(materialDisplayName + (material.IsDefault ? " (*)" : ""));
                                materialNode.Tag = material;
                                supplierNode.Nodes.Add(materialNode);
                            }
                        }
                    }
                }
            }

            this.trCatalog.ExpandAll();
        }

        private void trCatalog_AfterSelect(object sender, TreeViewEventArgs e)
        {
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
                    if (!(this.splitContainer1.Panel2.Controls.Count > 0 && this.splitContainer1.Panel2.Controls[0] is MaterialEditorCatalog)) { this.splitContainer1.Panel2.Controls.Clear(); };
                    var materialEditorCatalog = new MaterialEditorCatalog();
                    materialEditorCatalog.Dock = DockStyle.Fill;
                    this.splitContainer1.Panel2.Controls.Add(materialEditorCatalog);
                    break;
                case 1:
                    var materialEditorManufacturer = new MaterialEditorManufacturer();
                    if (!(this.splitContainer1.Panel2.Controls.Count > 0 && this.splitContainer1.Panel2.Controls[0] is MaterialEditorManufacturer))
                    {
                        this.splitContainer1.Panel2.Controls.Clear();
                        materialEditorManufacturer.Dock = DockStyle.Fill;
                        this.splitContainer1.Panel2.Controls.Add(materialEditorManufacturer);
                        materialEditorManufacturer.DisplayNameChanged += materialEditorManufacturer_DisplayNameChanged;
                    }
                    else
                    {
                        materialEditorManufacturer = this.splitContainer1.Panel2.Controls[0] as MaterialEditorManufacturer;
                    }

                    materialEditorManufacturer.CurrentSupplier = e.Node.Tag as MaterialsBySupplier;

                    break;
                case 2:

                    var materialEditorMaterial = new MaterialEditorMaterial();
                    if (!(this.splitContainer1.Panel2.Controls.Count > 0 && this.splitContainer1.Panel2.Controls[0] is MaterialEditorMaterial))
                    {
                        this.splitContainer1.Panel2.Controls.Clear();
                        materialEditorMaterial.Dock = DockStyle.Fill;
                        this.splitContainer1.Panel2.Controls.Add(materialEditorMaterial);
                        materialEditorMaterial.DisplayNameChanged += materialEditorMaterial_DisplayNameChanged;
                    }
                    else
                    {
                        materialEditorMaterial = this.splitContainer1.Panel2.Controls[0] as MaterialEditorMaterial;
                    }
                    materialEditorMaterial.CurrentMaterial = e.Node.Tag as Material;
                    materialEditorMaterial.CurrentMaterial.PropertyChanged += CurrentMaterial_PropertyChanged;

                    break;
            }


            this.UpdateControls();
        }

        void materialEditorManufacturer_DisplayNameChanged(object sender, MaterialSupplierDisplayNameArgs e)
        {
            if (e.DisplayName.Length > 0 && System.Text.RegularExpressions.Regex.IsMatch(e.DisplayName, @"^[\w\-. ]+$"))
            {
                this.btnCTXSupplierSave.Enabled = true;
                this.btnManufacturerSave.Enabled = true;
                this.trCatalog.SelectedNode.Text = e.DisplayName;
            }
            else
            {
                this.btnManufacturerSave.Enabled = false;
                this.btnCTXSupplierSave.Enabled = false;
            }
        }

        void CurrentMaterial_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            this.btnMaterialSave.Enabled = true;
            this.btnCTXMaterialSave.Enabled = true;
        }

        void materialEditorMaterial_DisplayNameChanged(object sender, MaterialDisplayNameArgs e)
        {
            if (this.trCatalog.SelectedNode != null)
            {
                this.trCatalog.SelectedNode.Text = e.DisplayName.Replace(" (*)", string.Empty);
                if (e.Material.IsDefault) this.trCatalog.SelectedNode.Text += " (*)";
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (DAL.OS.OSProvider.IsOSX)
            {
                this.Size = new Size(this.Width, this.Height + 100);
            }

            this.trCatalog.ExpandAll();

            if (this.trCatalog.SelectedNode == null)
            {
                this.trCatalog.SelectedNode = this.trCatalog.Nodes[0];
            }

        }

        private void UpdateControls()
        {
            foreach (ToolStripItem item in this.toolStrip1.Items)
            {
                if (!(item is ToolStripLabel))
                {
                    item.Enabled = false;
                }
            }

            this.btnMaterialCatalogOnline.Enabled = true;

            //contextmenu
            foreach (ToolStripMenuItem item in this.contextMenuSupplier.Items)
            {
                foreach (ToolStripItem childitem in item.DropDownItems)
                {
                    if (!(childitem is ToolStripLabel))
                    {
                        childitem.Enabled = false;
                    }
                }
            }

            this.cbSelectedPrinter.Enabled = true;
            switch (this.trCatalog.SelectedNode.Level)
            {
                case 0:
                    this.btnManufacturerAdd.Enabled = true;
                    this.btnCTXSupplierAdd.Enabled = true;
                    break;
                case 1:
                    this.btnMaterialAdd.Enabled = true;
                    this.btnCTXMaterialAdd.Enabled = true;
                    this.btnManufacturerRemove.Enabled = true;
                    this.btnCTXSupplierRemove.Enabled = true;
                    //this.btnManufacturerSave.Enabled = true;
                    break;
                case 2:
                    this.btnMaterialRemove.Enabled = true;
                    this.btnCTXMaterialRemove.Enabled = true;
                    this.btnMaterialAsDefault.Enabled = true;
                    this.btnCTXMaterialDefault.Enabled = true;

                    this.btnMaterialSave.Enabled = true;
                    this.btnCTXMaterialSave.Enabled = true;

                    break;
            }

            if (!ConnectivityManager.InternetAvailable)
            {
                this.btnMaterialCatalogOnline.Enabled = false;
            }
        }

        private void btnMaterialSave_Click(object sender, EventArgs e)
        {
            MaterialManager.Save();
            this.btnMaterialSave.Enabled = false;
        }

        private void btnManufacturerSave_Click(object sender, EventArgs e)
        {
            MaterialManager.Save();
            this.btnManufacturerSave.Enabled = false;
        }

        private void btnMaterialAdd_Click(object sender, EventArgs e)
        {
            var selectedPrinter = ((this.cbSelectedPrinter.SelectedItem) as Atum.DAL.Hardware.AtumPrinter);
            var currentSupplier = this.trCatalog.SelectedNode.Tag as MaterialsBySupplier;
            var newMaterialNode = new TreeNode("<EMPTY>");
            newMaterialNode.Tag = new Material();
            var newMaterial = newMaterialNode.Tag as Material;
            newMaterial.XYResolution = selectedPrinter.PrinterXYResolutionAsInt;
            newMaterial.PrinterHardwareType = selectedPrinter.PrinterHardwareType;
            currentSupplier.Materials.Add(newMaterial);

            //add new node to treeview
            this.trCatalog.HideSelection = false;
            this.trCatalog.SelectedNode.Nodes.Add(newMaterialNode);
            this.trCatalog.SelectedNode = newMaterialNode;
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

                Atum.Studio.Core.Managers.MaterialManager.Save();
            }
        }

        private void btnManufacturerRemove_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show(string.Format("Are you sure you want to remove: {0}?", this.trCatalog.SelectedNode.Text), "Remove selected manufacturer", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
            {
                var catalogNode = this.trCatalog.Nodes[0];
                var supplierNode = this.trCatalog.SelectedNode;
                var supplier = supplierNode.Tag as MaterialsBySupplier;

                this.trCatalog.SelectedNode.Remove();
                this.trCatalog.SelectedNode = catalogNode;

                MaterialManager.Catalog.Remove(supplier);
                MaterialManager.Save();
            }
        }

        private void btnManufacturerAdd_Click(object sender, EventArgs e)
        {
            var catalogNode = this.trCatalog.SelectedNode;
            var newSupplierNode = new TreeNode("<EMPTY>");
            newSupplierNode.Tag = new MaterialsBySupplier();
            MaterialManager.Catalog.Add(newSupplierNode.Tag as MaterialsBySupplier);
            catalogNode.Nodes.Add(newSupplierNode);

            //add new node to treeview
            this.trCatalog.HideSelection = false;
            this.trCatalog.SelectedNode = newSupplierNode;
        }

        internal override void btnOK_Click(object sender, EventArgs e)
        {
            if (this.btnManufacturerSave.Enabled || this.btnMaterialSave.Enabled)
            {
                if (MessageBox.Show("Save uncommitted changes?", "Save uncommitted changes?", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                {
                    MaterialManager.Save();
                }
            }
            base.btnOK_Click(sender, e);
        }

        private void btnMaterialAsDefault_Click(object sender, EventArgs e)
        {
            MaterialManager.Save();

            if (this.trCatalog.SelectedNode.Tag is Material)
            {
                MaterialManager.SetAsDefault(this.trCatalog.SelectedNode.Tag as Material);

                foreach (TreeNode materialSupplierNode in this.trCatalog.Nodes[0].Nodes)
                {
                    foreach (TreeNode materialNode in materialSupplierNode.Nodes)
                    {
                        var material = materialNode.Tag as Material;
                        materialNode.Text = material.DisplayName + (material.IsDefault ? " (*)" : "");
                    }
                }
            }

            MaterialManager.Save();
        }

        private void btnCTXSupplierAdd_Click(object sender, EventArgs e)
        {
            this.btnManufacturerAdd.PerformClick();
        }

        private void btnCTXSupplierSave_Click(object sender, EventArgs e)
        {
            this.btnManufacturerSave.PerformClick();
        }

        private void btnCTXSupplierRemove_Click(object sender, EventArgs e)
        {
            this.btnManufacturerRemove.PerformClick();
        }

        private void btnCTXMaterialAdd_Click(object sender, EventArgs e)
        {
            this.btnMaterialAdd.PerformClick();
        }

        private void btnCTXMaterialSave_Click(object sender, EventArgs e)
        {
            this.btnMaterialSave.PerformClick();
        }

        private void btnCTXMaterialRemove_Click(object sender, EventArgs e)
        {
            this.btnMaterialRemove.PerformClick();
        }

        private void btnCTXMaterialDefault_Click(object sender, EventArgs e)
        {
            this.btnMaterialAsDefault.PerformClick();
        }

        private frmMaterialCatalogOnline _materialCatalogOnline = new frmMaterialCatalogOnline();
        private void btnMaterialCatalogOnline_Click(object sender, EventArgs e)
        {
            _materialCatalogOnline.DownloadMaterialsAsycn(this, this.SelectedPrinter);
        }

        void materialCatalogOnline_DownloadMaterialsCompleted(bool value)
        {
            if (_materialCatalogOnline.ShowDialog(this) == DialogResult.OK)
            {
                var selectedMaterials = _materialCatalogOnline.SelectedMaterials;
                if (selectedMaterials.Count > 0)
                {
                    foreach (var supplier in selectedMaterials)
                    {
                        var supplierFound = false;
                        foreach (var currentSupplier in MaterialManager.Catalog)
                        {
                            if (currentSupplier.Supplier.ToLower() == supplier.Supplier.ToLower())
                            {
                                supplierFound = true;

                                //check if material exists
                                foreach (var selectedMaterial in supplier.Materials)
                                {
                                    var materialFound = false;

                                    foreach (var currentMaterial in currentSupplier.Materials)
                                    {
                                        if (currentMaterial.DisplayName != null && currentMaterial.DisplayName.ToLower() == selectedMaterial.DisplayName.ToLower() && currentMaterial.XYResolution == selectedMaterial.XYResolution && currentMaterial.PrinterHardwareType != null && currentMaterial.PrinterHardwareType == selectedMaterial.PrinterHardwareType)
                                        {
                                            materialFound = true;
                                            break;
                                        }
                                    }

                                    if (!materialFound)
                                    {
                                        currentSupplier.Materials.Add(selectedMaterial);
                                    }
                                }
                                break;
                            }
                        }

                        if (!supplierFound)
                        {
                            MaterialManager.Catalog.Add(supplier);
                        }
                    }
                }


                MaterialManager.Save();
                this.RefreshCatalog();
            }
        }

        private void cbSelectedPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.SelectedPrinter = this.cbSelectedPrinter.SelectedItem as DAL.Hardware.AtumPrinter;
            RefreshCatalog();
        }
    }
}
