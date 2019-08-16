namespace Atum.Studio.Controls.Docking
{
    partial class ModelPropertiesPanel
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.pgModel = new Atum.Studio.Controls.PropertyGridFiltered();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnModelRemove = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnAddManualSupport = new System.Windows.Forms.ToolStripButton();
            this.btnAddManualSupport2Points = new System.Windows.Forms.ToolStripButton();
            this.btnAddGridSupport = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.btnMirrorModel = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRepairFaces = new System.Windows.Forms.ToolStripButton();
            this.plContent.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.plContent.Controls.Add(this.pgModel);
            this.plContent.Controls.Add(this.toolStrip1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Location = new System.Drawing.Point(0, 26);
            this.plContent.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.plContent.Size = new System.Drawing.Size(257, 474);
            // 
            // pgModel
            // 
            this.pgModel.BrowsableProperties = null;
            this.pgModel.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgModel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgModel.HiddenAttributes = null;
            this.pgModel.HiddenProperties = null;
            this.pgModel.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pgModel.Location = new System.Drawing.Point(3, 27);
            this.pgModel.Name = "pgModel";
            this.pgModel.RenamedProperties = null;
            this.pgModel.Size = new System.Drawing.Size(251, 444);
            this.pgModel.TabIndex = 3;
            this.pgModel.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgModel_PropertyValueChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnModelRemove,
            this.toolStripSeparator1,
            this.btnAddManualSupport,
            this.btnAddManualSupport2Points,
            this.btnAddGridSupport,
            this.toolStripSeparator3,
            this.btnMirrorModel,
            this.toolStripSeparator2,
            this.btnRepairFaces});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(251, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStrip1_MouseMove);
            // 
            // btnModelRemove
            // 
            this.btnModelRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnModelRemove.Image = global::Atum.Studio.Properties.Resources.Remove;
            this.btnModelRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnModelRemove.Name = "btnModelRemove";
            this.btnModelRemove.Size = new System.Drawing.Size(24, 24);
            this.btnModelRemove.Text = "Remove (Del)";
            this.btnModelRemove.Click += new System.EventHandler(this.btnSupportRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnAddManualSupport
            // 
            this.btnAddManualSupport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddManualSupport.Image = global::Atum.Studio.Properties.Resources.support_single_24x24;
            this.btnAddManualSupport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddManualSupport.Name = "btnAddManualSupport";
            this.btnAddManualSupport.Size = new System.Drawing.Size(24, 24);
            this.btnAddManualSupport.Text = "Add Manual Support";
            this.btnAddManualSupport.ToolTipText = "Add Manual Support (Q)";
            this.btnAddManualSupport.Click += new System.EventHandler(this.btnAddManualSupport_Click);
            // 
            // btnAddManualSupport2Points
            // 
            this.btnAddManualSupport2Points.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddManualSupport2Points.Image = global::Atum.Studio.Properties.Resources.support_2point;
            this.btnAddManualSupport2Points.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddManualSupport2Points.Name = "btnAddManualSupport2Points";
            this.btnAddManualSupport2Points.Size = new System.Drawing.Size(24, 24);
            this.btnAddManualSupport2Points.Text = "Add 2 Points Support";
            this.btnAddManualSupport2Points.Visible = false;
            this.btnAddManualSupport2Points.Click += new System.EventHandler(this.btnAddManualSupport2Points_Click);
            // 
            // btnAddGridSupport
            // 
            this.btnAddGridSupport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAddGridSupport.Image = global::Atum.Studio.Properties.Resources.support_grid_24x24;
            this.btnAddGridSupport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddGridSupport.Name = "btnAddGridSupport";
            this.btnAddGridSupport.Size = new System.Drawing.Size(24, 24);
            this.btnAddGridSupport.Text = "Add Grid Support";
            this.btnAddGridSupport.ToolTipText = "Add Grid Support (G)";
            this.btnAddGridSupport.Click += new System.EventHandler(this.btnAddGridSupport_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 27);
            // 
            // btnMirrorModel
            // 
            this.btnMirrorModel.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnMirrorModel.Image = global::Atum.Studio.Properties.Resources.Flip_horizontal;
            this.btnMirrorModel.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnMirrorModel.Name = "btnMirrorModel";
            this.btnMirrorModel.Size = new System.Drawing.Size(24, 24);
            this.btnMirrorModel.Text = "Mirror";
            this.btnMirrorModel.Click += new System.EventHandler(this.btnMirrorModel_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 27);
            this.toolStripSeparator2.Visible = false;
            // 
            // btnRepairFaces
            // 
            this.btnRepairFaces.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRepairFaces.Image = global::Atum.Studio.Properties.Resources.Repair_Faces;
            this.btnRepairFaces.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRepairFaces.Name = "btnRepairFaces";
            this.btnRepairFaces.Size = new System.Drawing.Size(24, 24);
            this.btnRepairFaces.Text = "Repair normals";
            this.btnRepairFaces.Visible = false;
            this.btnRepairFaces.Click += new System.EventHandler(this.btnRepairFaces_Click);
            // 
            // ModelPropertiesPanel
            // 
            this.ClientSize = new System.Drawing.Size(257, 500);
            this.MinimumSize = new System.Drawing.Size(0, 500);
            this.Name = "ModelPropertiesPanel";
            this.Title = "Model Properties";
            this.ToolstripIconMouseOut = global::Atum.Studio.Properties.Resources.ToolStip_ModelProperties_MouseOut;
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridFiltered pgModel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnModelRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton btnAddManualSupport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton btnRepairFaces;
        private System.Windows.Forms.ToolStripButton btnAddManualSupport2Points;
        private System.Windows.Forms.ToolStripButton btnAddGridSupport;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton btnMirrorModel;

    }
}
