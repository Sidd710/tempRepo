namespace Atum.Studio.Controls.NewGui.MaterialCatalogEditor
{
    partial class frmMaterialCatalogManager
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.plLine = new System.Windows.Forms.Panel();
            this.rbMaterialResolution100 = new System.Windows.Forms.RadioButton();
            this.rbMaterialResolution75 = new System.Windows.Forms.RadioButton();
            this.rbMaterialResolution50 = new System.Windows.Forms.RadioButton();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.trCatalog = new System.Windows.Forms.TreeView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.lblManufacturer = new System.Windows.Forms.ToolStripLabel();
            this.btnManufacturerAdd = new System.Windows.Forms.ToolStripButton();
            this.btnManufacturerSave = new System.Windows.Forms.ToolStripButton();
            this.btnManufacturerRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.lblMaterial = new System.Windows.Forms.ToolStripLabel();
            this.btnMaterialAdd = new System.Windows.Forms.ToolStripButton();
            this.btnMaterialSave = new System.Windows.Forms.ToolStripButton();
            this.btnMaterialRemove = new System.Windows.Forms.ToolStripButton();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plContent.Controls.Add(this.splitContainer1);
            this.plContent.Size = new System.Drawing.Size(1495, 651);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.plLine);
            this.splitContainer1.Panel1.Controls.Add(this.rbMaterialResolution100);
            this.splitContainer1.Panel1.Controls.Add(this.rbMaterialResolution75);
            this.splitContainer1.Panel1.Controls.Add(this.rbMaterialResolution50);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(12, 12, 12, 16);
            this.splitContainer1.Size = new System.Drawing.Size(1493, 649);
            this.splitContainer1.SplitterDistance = 93;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 0;
            this.splitContainer1.TabStop = false;
            // 
            // plLine
            // 
            this.plLine.BackColor = System.Drawing.Color.Black;
            this.plLine.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plLine.Location = new System.Drawing.Point(0, 92);
            this.plLine.Name = "plLine";
            this.plLine.Size = new System.Drawing.Size(1493, 1);
            this.plLine.TabIndex = 5;
            // 
            // rbMaterialResolution100
            // 
            this.rbMaterialResolution100.AutoSize = true;
            this.rbMaterialResolution100.Checked = true;
            this.rbMaterialResolution100.Location = new System.Drawing.Point(226, 15);
            this.rbMaterialResolution100.Margin = new System.Windows.Forms.Padding(16, 8, 16, 8);
            this.rbMaterialResolution100.Name = "rbMaterialResolution100";
            this.rbMaterialResolution100.Size = new System.Drawing.Size(73, 21);
            this.rbMaterialResolution100.TabIndex = 2;
            this.rbMaterialResolution100.TabStop = true;
            this.rbMaterialResolution100.Text = "100 µm";
            this.rbMaterialResolution100.UseVisualStyleBackColor = true;
            this.rbMaterialResolution100.CheckedChanged += new System.EventHandler(this.rbMaterialResolution100_CheckedChanged);
            // 
            // rbMaterialResolution75
            // 
            this.rbMaterialResolution75.AutoSize = true;
            this.rbMaterialResolution75.Location = new System.Drawing.Point(110, 15);
            this.rbMaterialResolution75.Margin = new System.Windows.Forms.Padding(16, 8, 16, 16);
            this.rbMaterialResolution75.Name = "rbMaterialResolution75";
            this.rbMaterialResolution75.Size = new System.Drawing.Size(65, 21);
            this.rbMaterialResolution75.TabIndex = 1;
            this.rbMaterialResolution75.Text = "75 µm";
            this.rbMaterialResolution75.UseVisualStyleBackColor = true;
            this.rbMaterialResolution75.CheckedChanged += new System.EventHandler(this.rbMaterialResolution75_CheckedChanged);
            // 
            // rbMaterialResolution50
            // 
            this.rbMaterialResolution50.AutoSize = true;
            this.rbMaterialResolution50.Location = new System.Drawing.Point(13, 15);
            this.rbMaterialResolution50.Margin = new System.Windows.Forms.Padding(16, 16, 16, 8);
            this.rbMaterialResolution50.Name = "rbMaterialResolution50";
            this.rbMaterialResolution50.Size = new System.Drawing.Size(65, 21);
            this.rbMaterialResolution50.TabIndex = 0;
            this.rbMaterialResolution50.Text = "50 µm";
            this.rbMaterialResolution50.UseVisualStyleBackColor = true;
            this.rbMaterialResolution50.CheckedChanged += new System.EventHandler(this.rbMaterialResolution50_CheckedChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(12, 12);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.trCatalog);
            this.splitContainer2.Panel1.Controls.Add(this.toolStrip1);
            this.splitContainer2.Size = new System.Drawing.Size(1469, 527);
            this.splitContainer2.SplitterDistance = 349;
            this.splitContainer2.TabIndex = 4;
            // 
            // trCatalog
            // 
            this.trCatalog.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trCatalog.Location = new System.Drawing.Point(0, 26);
            this.trCatalog.Name = "trCatalog";
            this.trCatalog.ShowLines = false;
            this.trCatalog.Size = new System.Drawing.Size(349, 501);
            this.trCatalog.TabIndex = 1;
            this.trCatalog.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trCatalog_AfterSelect);
            // 
            // toolStrip1
            // 
            this.toolStrip1.AutoSize = false;
            this.toolStrip1.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblManufacturer,
            this.btnManufacturerAdd,
            this.btnManufacturerSave,
            this.btnManufacturerRemove,
            this.toolStripSeparator4,
            this.lblMaterial,
            this.btnMaterialAdd,
            this.btnMaterialSave,
            this.btnMaterialRemove});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(349, 26);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(95, 23);
            this.lblManufacturer.Text = "Manufacturer:";
            // 
            // btnManufacturerAdd
            // 
            this.btnManufacturerAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManufacturerAdd.Image = global::Atum.Studio.Properties.Resources.button_plus;
            this.btnManufacturerAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManufacturerAdd.Name = "btnManufacturerAdd";
            this.btnManufacturerAdd.Size = new System.Drawing.Size(24, 23);
            this.btnManufacturerAdd.Text = "Add";
            this.btnManufacturerAdd.Click += new System.EventHandler(this.btnManufacturerAdd_Click);
            // 
            // btnManufacturerSave
            // 
            this.btnManufacturerSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManufacturerSave.Image = global::Atum.Studio.Properties.Resources.save_icon;
            this.btnManufacturerSave.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnManufacturerSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManufacturerSave.Name = "btnManufacturerSave";
            this.btnManufacturerSave.Size = new System.Drawing.Size(24, 23);
            this.btnManufacturerSave.Text = "Save";
            this.btnManufacturerSave.Click += new System.EventHandler(this.btnManufacturerSave_Click);
            // 
            // btnManufacturerRemove
            // 
            this.btnManufacturerRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnManufacturerRemove.Image = global::Atum.Studio.Properties.Resources.button_minus;
            this.btnManufacturerRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnManufacturerRemove.Name = "btnManufacturerRemove";
            this.btnManufacturerRemove.Size = new System.Drawing.Size(24, 23);
            this.btnManufacturerRemove.Text = "Remove";
            this.btnManufacturerRemove.Click += new System.EventHandler(this.btnManufacturerRemove_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(6, 26);
            // 
            // lblMaterial
            // 
            this.lblMaterial.Name = "lblMaterial";
            this.lblMaterial.Size = new System.Drawing.Size(62, 23);
            this.lblMaterial.Text = "Material:";
            // 
            // btnMaterialAdd
            // 
            this.btnMaterialAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaterialAdd.Image = global::Atum.Studio.Properties.Resources.button_plus;
            this.btnMaterialAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialAdd.Name = "btnMaterialAdd";
            this.btnMaterialAdd.Size = new System.Drawing.Size(24, 23);
            this.btnMaterialAdd.Text = "Add";
            this.btnMaterialAdd.Click += new System.EventHandler(this.btnMaterialAdd_Click);
            // 
            // btnMaterialSave
            // 
            this.btnMaterialSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaterialSave.Image = global::Atum.Studio.Properties.Resources.save_icon;
            this.btnMaterialSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialSave.Name = "btnMaterialSave";
            this.btnMaterialSave.Size = new System.Drawing.Size(24, 23);
            this.btnMaterialSave.Text = "Save";
            this.btnMaterialSave.Click += new System.EventHandler(this.btnMaterialSave_Click);
            // 
            // btnMaterialRemove
            // 
            this.btnMaterialRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMaterialRemove.Image = global::Atum.Studio.Properties.Resources.button_minus;
            this.btnMaterialRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMaterialRemove.Name = "btnMaterialRemove";
            this.btnMaterialRemove.Size = new System.Drawing.Size(24, 23);
            this.btnMaterialRemove.Text = "Remove";
            this.btnMaterialRemove.Click += new System.EventHandler(this.btnMaterialRemove_Click);
            // 
            // frmMaterialCatalogManager
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1495, 708);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMaterialCatalogManager";
            this.Text = "Material Catalog Manager v1.01";
            this.Load += new System.EventHandler(this.frmMaterialCatalogManager_Load);
            this.plContent.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.RadioButton rbMaterialResolution100;
        private System.Windows.Forms.RadioButton rbMaterialResolution75;
        private System.Windows.Forms.RadioButton rbMaterialResolution50;
        private System.Windows.Forms.Panel plLine;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TreeView trCatalog;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel lblManufacturer;
        private System.Windows.Forms.ToolStripButton btnManufacturerAdd;
        private System.Windows.Forms.ToolStripButton btnManufacturerSave;
        private System.Windows.Forms.ToolStripButton btnManufacturerRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripLabel lblMaterial;
        private System.Windows.Forms.ToolStripButton btnMaterialAdd;
        private System.Windows.Forms.ToolStripButton btnMaterialSave;
        private System.Windows.Forms.ToolStripButton btnMaterialRemove;
    }
}