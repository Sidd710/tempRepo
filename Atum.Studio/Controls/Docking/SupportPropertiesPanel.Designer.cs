using System.Windows.Forms;

namespace Atum.Studio.Controls.Docking
{
    partial class SupportPropertiesPanel
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
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.pgSupport = new Atum.Studio.Controls.PropertyGridFiltered();
            this.btnSupportRemove = new BaseToolStripButton();
            this.btnSupportApplyToAll = new BaseToolStripButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSaveAsDefaults = new BaseToolStripButton();
            this.plContent.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.plContent.Controls.Add(this.pgSupport);
            this.plContent.Controls.Add(this.toolStrip1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Location = new System.Drawing.Point(0, 26);
            this.plContent.Margin = new System.Windows.Forms.Padding(0);
            this.plContent.Padding = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.plContent.Size = new System.Drawing.Size(257, 287);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // pgSupport
            // 
            this.pgSupport.BrowsableProperties = null;
            this.pgSupport.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.pgSupport.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pgSupport.HiddenAttributes = null;
            this.pgSupport.HiddenProperties = null;
            this.pgSupport.LineColor = System.Drawing.SystemColors.ScrollBar;
            this.pgSupport.Location = new System.Drawing.Point(3, 27);
            this.pgSupport.Name = "pgSupport";
            this.pgSupport.RenamedProperties = null;
            this.pgSupport.Size = new System.Drawing.Size(251, 257);
            this.pgSupport.TabIndex = 3;
            this.pgSupport.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.pgSupport_PropertyValueChanged);
            // 
            // btnSupportRemove
            // 
            this.btnSupportRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSupportRemove.Image = global::Atum.Studio.Properties.Resources.Remove;
            this.btnSupportRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSupportRemove.Name = "btnSupportRemove";
            this.btnSupportRemove.Size = new System.Drawing.Size(24, 24);
            this.btnSupportRemove.Text = "Remove";
            this.btnSupportRemove.ToolTipText = "Remove (Del)";
            this.btnSupportRemove.Click += new System.EventHandler(this.btnSupportRemove_Click);
            // 
            // btnSupportApplyToAll
            // 
            this.btnSupportApplyToAll.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSupportApplyToAll.Image = global::Atum.Studio.Properties.Resources.SetAsDefault;
            this.btnSupportApplyToAll.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSupportApplyToAll.Name = "btnSupportApplyToAll";
            this.btnSupportApplyToAll.Size = new System.Drawing.Size(24, 24);
            this.btnSupportApplyToAll.Text = "Apply settings to support cones";
            this.btnSupportApplyToAll.Click += new System.EventHandler(this.btnSupportApplyToAll_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSupportRemove,
            this.toolStripSeparator1,
            this.btnSaveAsDefaults,
            this.btnSupportApplyToAll});
            this.toolStrip1.Location = new System.Drawing.Point(3, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(251, 27);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSaveAsDefaults
            // 
            this.btnSaveAsDefaults.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSaveAsDefaults.Image = global::Atum.Studio.Properties.Resources.save_icon;
            this.btnSaveAsDefaults.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSaveAsDefaults.Name = "btnSaveAsDefaults";
            this.btnSaveAsDefaults.Size = new System.Drawing.Size(24, 24);
            this.btnSaveAsDefaults.Text = "Save as Defaults";
            this.btnSaveAsDefaults.Click += new System.EventHandler(this.btnSaveAsDefaults_Click);
            // 
            // SupportPropertiesPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 313);
            this.MinimumSize = new System.Drawing.Size(0, 300);
            this.Name = "SupportPropertiesPanel";
            this.Title = "Support Properties";
            this.ToolstripIconMouseOut = global::Atum.Studio.Properties.Resources.ToolStip_SupportProperties_MouseOut;
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PropertyGridFiltered pgSupport;
        private ToolStrip toolStrip1;
        private BaseToolStripButton btnSupportRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BaseToolStripButton btnSupportApplyToAll;
        private BaseToolStripButton btnSaveAsDefaults;
    }
}
