namespace Atum.Studio.Controls.MaterialEditor
{
    partial class frmMaterialEditorPopup
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Catalog");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMaterialEditorPopup));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.trCatalog = new System.Windows.Forms.TreeView();
            this.contextMenuSupplier = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCTXSupplierAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCTXSupplierSave = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCTXSupplierRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCTXMaterialAdd = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCTXMaterialSave = new System.Windows.Forms.ToolStripMenuItem();
            this.btnCTXMaterialRemove = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnCTXMaterialDefault = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel3 = new System.Windows.Forms.ToolStripLabel();
            this.cbSelectedPrinter = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.btnManufacturerAdd = new System.Windows.Forms.ToolStripButton();
            this.btnManufacturerSave = new System.Windows.Forms.ToolStripButton();
            this.btnManufacturerRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.btnMaterialAdd = new System.Windows.Forms.ToolStripButton();
            this.btnMaterialSave = new System.Windows.Forms.ToolStripButton();
            this.btnMaterialRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMaterialAsDefault = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMaterialCatalogOnline = new System.Windows.Forms.ToolStripButton();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.contextMenuSupplier.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.splitContainer1);
            this.plContent.Controls.Add(this.toolStrip1);
            this.plContent.Size = new System.Drawing.Size(979, 500);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 27);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.trCatalog);
            this.splitContainer1.Size = new System.Drawing.Size(979, 473);
            this.splitContainer1.SplitterDistance = 324;
            this.splitContainer1.TabIndex = 0;
            // 
            // trCatalog
            // 
            this.trCatalog.ContextMenuStrip = this.contextMenuSupplier;
            this.trCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trCatalog.Location = new System.Drawing.Point(0, 0);
            this.trCatalog.Name = "trCatalog";
            treeNode1.Name = "Catalog";
            treeNode1.Text = "Catalog";
            this.trCatalog.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1});
            this.trCatalog.ShowLines = false;
            this.trCatalog.Size = new System.Drawing.Size(324, 473);
            this.trCatalog.TabIndex = 0;
            this.trCatalog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trCatalog_AfterSelect);
            // 
            // contextMenuSupplier
            // 
            this.contextMenuSupplier.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.contextMenuSupplier.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripMenuItem1,
            this.toolStripMenuItem2});
            this.contextMenuSupplier.Name = "contextMenuSupplier";
            this.contextMenuSupplier.Size = new System.Drawing.Size(147, 48);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCTXSupplierAdd,
            this.btnCTXSupplierSave,
            this.btnCTXSupplierRemove});
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem1.Text = "Manufacturer";
            // 
            // btnCTXSupplierAdd
            // 
            this.btnCTXSupplierAdd.Name = "btnCTXSupplierAdd";
            this.btnCTXSupplierAdd.Size = new System.Drawing.Size(117, 22);
            this.btnCTXSupplierAdd.Text = "Add";
            this.btnCTXSupplierAdd.Click += new System.EventHandler(this.btnCTXSupplierAdd_Click);
            // 
            // btnCTXSupplierSave
            // 
            this.btnCTXSupplierSave.Name = "btnCTXSupplierSave";
            this.btnCTXSupplierSave.Size = new System.Drawing.Size(117, 22);
            this.btnCTXSupplierSave.Text = "Save";
            this.btnCTXSupplierSave.Click += new System.EventHandler(this.btnCTXSupplierSave_Click);
            // 
            // btnCTXSupplierRemove
            // 
            this.btnCTXSupplierRemove.Name = "btnCTXSupplierRemove";
            this.btnCTXSupplierRemove.Size = new System.Drawing.Size(117, 22);
            this.btnCTXSupplierRemove.Text = "Remove";
            this.btnCTXSupplierRemove.Click += new System.EventHandler(this.btnCTXSupplierRemove_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnCTXMaterialAdd,
            this.btnCTXMaterialSave,
            this.btnCTXMaterialRemove,
            this.toolStripSeparator3,
            this.btnCTXMaterialDefault});
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(146, 22);
            this.toolStripMenuItem2.Text = "Material";
            // 
            // btnCTXMaterialAdd
            // 
            this.btnCTXMaterialAdd.Name = "btnCTXMaterialAdd";
            this.btnCTXMaterialAdd.Size = new System.Drawing.Size(117, 22);
            this.btnCTXMaterialAdd.Text = "Add";
            this.btnCTXMaterialAdd.Click += new System.EventHandler(this.btnCTXMaterialAdd_Click);
            // 
            // btnCTXMaterialSave
            // 
            this.btnCTXMaterialSave.Name = "btnCTXMaterialSave";
            this.btnCTXMaterialSave.Size = new System.Drawing.Size(117, 22);
            this.btnCTXMaterialSave.Text = "Save";
            this.btnCTXMaterialSave.Click += new System.EventHandler(this.btnCTXMaterialSave_Click);
            // 
            // btnCTXMaterialRemove
            // 
            this.btnCTXMaterialRemove.Name = "btnCTXMaterialRemove";
            this.btnCTXMaterialRemove.Size = new System.Drawing.Size(117, 22);
            this.btnCTXMaterialRemove.Text = "Remove";
            this.btnCTXMaterialRemove.Click += new System.EventHandler(this.btnCTXMaterialRemove_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(114, 6);
            // 
            // btnCTXMaterialDefault
            // 
            this.btnCTXMaterialDefault.Name = "btnCTXMaterialDefault";
            this.btnCTXMaterialDefault.Size = new System.Drawing.Size(117, 22);
            this.btnCTXMaterialDefault.Text = "Default";
            this.btnCTXMaterialDefault.Click += new System.EventHandler(this.btnCTXMaterialDefault_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel3,
            this.cbSelectedPrinter,
            this.toolStripLabel1,
            this.btnManufacturerAdd,
            this.btnManufacturerSave,
            this.btnManufacturerRemove,
            this.toolStripSeparator1,
            this.toolStripLabel2,
            this.btnMaterialAdd,
            this.btnMaterialSave,
            this.btnMaterialRemove,
            this.toolStripSeparator2,
            this.btnMaterialAsDefault,
            this.toolStripSeparator4,
            this.btnMaterialCatalogOnline});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(979, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripLabel3
            // 
            this.toolStripLabel3.Name = "toolStripLabel3";
            this.toolStripLabel3.Size = new System.Drawing.Size(92, 24);
            this.toolStripLabel3.Text = "Selected Printer:";
            // 
            // cbSelectedPrinter
            // 
            this.cbSelectedPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedPrinter.Name = "cbSelectedPrinter";
            this.cbSelectedPrinter.Size = new System.Drawing.Size(121, 27);
            this.cbSelectedPrinter.SelectedIndexChanged += new System.EventHandler(this.cbSelectedPrinter_SelectedIndexChanged);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(82, 24);
            this.toolStripLabel1.Text = "Manufacturer:";
            // 
            // btnManufacturerAdd
            // 
            this.btnManufacturerAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManufacturerAdd.Name = "btnManufacturerAdd";
            this.btnManufacturerAdd.Size = new System.Drawing.Size(53, 24);
            this.btnManufacturerAdd.Text = "Add";
            this.btnManufacturerAdd.Click += new System.EventHandler(this.btnManufacturerAdd_Click);
            // 
            // btnManufacturerSave
            // 
            this.btnManufacturerSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManufacturerSave.Name = "btnManufacturerSave";
            this.btnManufacturerSave.Size = new System.Drawing.Size(55, 24);
            this.btnManufacturerSave.Text = "Save";
            this.btnManufacturerSave.Click += new System.EventHandler(this.btnManufacturerSave_Click);
            // 
            // btnManufacturerRemove
            // 
            this.btnManufacturerRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManufacturerRemove.Name = "btnManufacturerRemove";
            this.btnManufacturerRemove.Size = new System.Drawing.Size(74, 24);
            this.btnManufacturerRemove.Text = "Remove";
            this.btnManufacturerRemove.Click += new System.EventHandler(this.btnManufacturerRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(53, 24);
            this.toolStripLabel2.Text = "Material:";
            // 
            // btnMaterialAdd
            // 
            this.btnMaterialAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialAdd.Name = "btnMaterialAdd";
            this.btnMaterialAdd.Size = new System.Drawing.Size(53, 24);
            this.btnMaterialAdd.Text = "Add";
            this.btnMaterialAdd.Click += new System.EventHandler(this.btnMaterialAdd_Click);
            // 
            // btnMaterialSave
            // 
            this.btnMaterialSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialSave.Name = "btnMaterialSave";
            this.btnMaterialSave.Size = new System.Drawing.Size(55, 24);
            this.btnMaterialSave.Text = "Save";
            this.btnMaterialSave.Click += new System.EventHandler(this.btnMaterialSave_Click);
            // 
            // btnMaterialRemove
            // 
            this.btnMaterialRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialRemove.Name = "btnMaterialRemove";
            this.btnMaterialRemove.Size = new System.Drawing.Size(74, 24);
            this.btnMaterialRemove.Text = "Remove";
            this.btnMaterialRemove.Click += new System.EventHandler(this.btnMaterialRemove_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            // 
            // btnMaterialAsDefault
            // 
            this.btnMaterialAsDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialAsDefault.Name = "btnMaterialAsDefault";
            this.btnMaterialAsDefault.Size = new System.Drawing.Size(101, 24);
            this.btnMaterialAsDefault.Text = "Set as default";
            this.btnMaterialAsDefault.Click += new System.EventHandler(this.btnMaterialAsDefault_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 27);
            // 
            // btnMaterialCatalogOnline
            // 
            this.btnMaterialCatalogOnline.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialCatalogOnline.Name = "btnMaterialCatalogOnline";
            this.btnMaterialCatalogOnline.Size = new System.Drawing.Size(110, 24);
            this.btnMaterialCatalogOnline.Text = "Online Catalog";
            this.btnMaterialCatalogOnline.Click += new System.EventHandler(this.btnMaterialCatalogOnline_Click);
            // 
            // frmMaterialEditorPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(979, 537);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3);
            this.Name = "frmMaterialEditorPopup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Material Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Controls.SetChildIndex(this.plContent, 0);
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.contextMenuSupplier.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TreeView trCatalog;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton btnManufacturerAdd;
        private System.Windows.Forms.ToolStripButton btnManufacturerSave;
        private System.Windows.Forms.ToolStripButton btnManufacturerRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        private System.Windows.Forms.ToolStripButton btnMaterialAdd;
        private System.Windows.Forms.ToolStripButton btnMaterialSave;
        private System.Windows.Forms.ToolStripButton btnMaterialRemove;
        private System.Windows.Forms.ToolStripButton btnMaterialAsDefault;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ContextMenuStrip contextMenuSupplier;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem btnCTXSupplierAdd;
        private System.Windows.Forms.ToolStripMenuItem btnCTXSupplierSave;
        private System.Windows.Forms.ToolStripMenuItem btnCTXSupplierRemove;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem btnCTXMaterialAdd;
        private System.Windows.Forms.ToolStripMenuItem btnCTXMaterialSave;
        private System.Windows.Forms.ToolStripMenuItem btnCTXMaterialRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem btnCTXMaterialDefault;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripButton btnMaterialCatalogOnline;
        private System.Windows.Forms.ToolStripLabel toolStripLabel3;
        private System.Windows.Forms.ToolStripComboBox cbSelectedPrinter;
    }
}

