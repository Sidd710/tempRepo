namespace Atum.Studio.Controls.Docking
{
    partial class PrintJobNavigationPanel
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnRemove = new BaseToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnRefresh = new BaseToolStripButton();
            this.trItems = new System.Windows.Forms.TreeView();
            this.plContent.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.plContent.Controls.Add(this.trItems);
            this.plContent.Controls.Add(this.toolStrip1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Top;
            this.plContent.Location = new System.Drawing.Point(0, 26);
            this.plContent.Size = new System.Drawing.Size(257, 429);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnRemove,
            this.toolStripSeparator1,
            this.btnRefresh});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(257, 27);
            this.toolStrip1.TabIndex = 19;
            this.toolStrip1.Text = "toolStrip1";
            this.toolStrip1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.toolStrip1_MouseMove);
            // 
            // btnRemove
            // 
            this.btnRemove.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRemove.Image = global::Atum.Studio.Properties.Resources.Remove;
            this.btnRemove.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemove.Name = "btnRemove";
            this.btnRemove.Size = new System.Drawing.Size(24, 24);
            this.btnRemove.Text = "Remove";
            this.btnRemove.Click += new System.EventHandler(this.btnRemove_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnRefresh
            // 
            this.btnRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnRefresh.Image = global::Atum.Studio.Properties.Resources.refresh;
            this.btnRefresh.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(24, 24);
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // trItems
            // 
            this.trItems.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trItems.Location = new System.Drawing.Point(0, 27);
            this.trItems.Name = "trItems";
            this.trItems.ShowRootLines = false;
            this.trItems.Size = new System.Drawing.Size(257, 402);
            this.trItems.TabIndex = 20;
            this.trItems.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trItems_BeforeSelect);
            // 
            // PrintJobNavigationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(257, 450);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(0, 450);
            this.Name = "PrintJobNavigationPanel";
            this.Text = "Print Job Navigation";
            this.Title = "Print Job Navigation";
            this.ToolstripIconMouseOut = global::Atum.Studio.Properties.Resources.ToolStip_Explorer_MouseOut;
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.PrintJobNavigationPanel_KeyUp);
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TreeView trItems;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private BaseToolStripButton btnRemove;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BaseToolStripButton btnRefresh;
    }
}
