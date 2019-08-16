namespace Atum.Studio.Controls
{
    partial class frmUserPreferences
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
            System.Windows.Forms.TreeNode treeNode1 = new System.Windows.Forms.TreeNode("Model Properties");
            System.Windows.Forms.TreeNode treeNode2 = new System.Windows.Forms.TreeNode("Support Engine");
            System.Windows.Forms.TreeNode treeNode3 = new System.Windows.Forms.TreeNode("Performance");
            System.Windows.Forms.TreeNode treeNode4 = new System.Windows.Forms.TreeNode("Licenses");
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserPreferences));
            this.panel1 = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbDisplay = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.gpDisplaySoftware = new System.Windows.Forms.GroupBox();
            this.chkSoftwareCheckForSoftwareUpdates = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkXYZAxis = new System.Windows.Forms.CheckBox();
            this.chkAnnotations = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkUserInterfaceUseLargeToolbarIcons = new System.Windows.Forms.CheckBox();
            this.tbAdvanced = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.trAdvancedSettings = new System.Windows.Forms.TreeView();
            this.gpAdvancedSettings = new Atum.Studio.Controls.PropertyGridFiltered();
            this.plContent.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbDisplay.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.gpDisplaySoftware.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tbAdvanced.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.panel1);
            this.plContent.Size = new System.Drawing.Size(503, 386);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tabControl1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(6);
            this.panel1.Size = new System.Drawing.Size(503, 386);
            this.panel1.TabIndex = 0;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbDisplay);
            this.tabControl1.Controls.Add(this.tbAdvanced);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(6, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(491, 374);
            this.tabControl1.TabIndex = 1;
            // 
            // tbDisplay
            // 
            this.tbDisplay.Controls.Add(this.tableLayoutPanel1);
            this.tbDisplay.Location = new System.Drawing.Point(4, 22);
            this.tbDisplay.Name = "tbDisplay";
            this.tbDisplay.Padding = new System.Windows.Forms.Padding(3);
            this.tbDisplay.Size = new System.Drawing.Size(483, 348);
            this.tbDisplay.TabIndex = 0;
            this.tbDisplay.Text = "Display";
            this.tbDisplay.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.gpDisplaySoftware, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 5;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(477, 342);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // gpDisplaySoftware
            // 
            this.gpDisplaySoftware.Controls.Add(this.chkSoftwareCheckForSoftwareUpdates);
            this.gpDisplaySoftware.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpDisplaySoftware.Location = new System.Drawing.Point(3, 103);
            this.gpDisplaySoftware.Name = "gpDisplaySoftware";
            this.gpDisplaySoftware.Size = new System.Drawing.Size(232, 94);
            this.gpDisplaySoftware.TabIndex = 2;
            this.gpDisplaySoftware.TabStop = false;
            this.gpDisplaySoftware.Text = "Software";
            // 
            // chkSoftwareCheckForSoftwareUpdates
            // 
            this.chkSoftwareCheckForSoftwareUpdates.AutoSize = true;
            this.chkSoftwareCheckForSoftwareUpdates.Location = new System.Drawing.Point(7, 19);
            this.chkSoftwareCheckForSoftwareUpdates.Name = "chkSoftwareCheckForSoftwareUpdates";
            this.chkSoftwareCheckForSoftwareUpdates.Size = new System.Drawing.Size(156, 17);
            this.chkSoftwareCheckForSoftwareUpdates.TabIndex = 0;
            this.chkSoftwareCheckForSoftwareUpdates.Text = "Check for software updates";
            this.chkSoftwareCheckForSoftwareUpdates.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkXYZAxis);
            this.groupBox1.Controls.Add(this.chkAnnotations);
            this.groupBox1.Location = new System.Drawing.Point(241, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(232, 94);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Selection Options";
            // 
            // chkXYZAxis
            // 
            this.chkXYZAxis.AutoSize = true;
            this.chkXYZAxis.Location = new System.Drawing.Point(6, 42);
            this.chkXYZAxis.Name = "chkXYZAxis";
            this.chkXYZAxis.Size = new System.Drawing.Size(69, 17);
            this.chkXYZAxis.TabIndex = 1;
            this.chkXYZAxis.Text = "XYZ Axis";
            this.chkXYZAxis.UseVisualStyleBackColor = true;
            // 
            // chkAnnotations
            // 
            this.chkAnnotations.AutoSize = true;
            this.chkAnnotations.Location = new System.Drawing.Point(6, 19);
            this.chkAnnotations.Name = "chkAnnotations";
            this.chkAnnotations.Size = new System.Drawing.Size(82, 17);
            this.chkAnnotations.TabIndex = 0;
            this.chkAnnotations.Text = "Annotations";
            this.chkAnnotations.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkUserInterfaceUseLargeToolbarIcons);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 3);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(232, 94);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "User Interface";
            // 
            // chkUserInterfaceUseLargeToolbarIcons
            // 
            this.chkUserInterfaceUseLargeToolbarIcons.AutoSize = true;
            this.chkUserInterfaceUseLargeToolbarIcons.Location = new System.Drawing.Point(7, 19);
            this.chkUserInterfaceUseLargeToolbarIcons.Name = "chkUserInterfaceUseLargeToolbarIcons";
            this.chkUserInterfaceUseLargeToolbarIcons.Size = new System.Drawing.Size(134, 17);
            this.chkUserInterfaceUseLargeToolbarIcons.TabIndex = 0;
            this.chkUserInterfaceUseLargeToolbarIcons.Text = "Use large toolbar icons";
            this.chkUserInterfaceUseLargeToolbarIcons.UseVisualStyleBackColor = true;
            // 
            // tbAdvanced
            // 
            this.tbAdvanced.Controls.Add(this.tableLayoutPanel2);
            this.tbAdvanced.Location = new System.Drawing.Point(4, 22);
            this.tbAdvanced.Name = "tbAdvanced";
            this.tbAdvanced.Padding = new System.Windows.Forms.Padding(6);
            this.tbAdvanced.Size = new System.Drawing.Size(276, 268);
            this.tbAdvanced.TabIndex = 1;
            this.tbAdvanced.Text = "Advanced";
            this.tbAdvanced.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 30F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 70F));
            this.tableLayoutPanel2.Controls.Add(this.trAdvancedSettings, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.gpAdvancedSettings, 1, 0);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(6, 6);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 2;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(264, 256);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // trAdvancedSettings
            // 
            this.trAdvancedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.trAdvancedSettings.Location = new System.Drawing.Point(3, 3);
            this.trAdvancedSettings.Name = "trAdvancedSettings";
            treeNode1.Name = "ModelProperties";
            treeNode1.Text = "Model Properties";
            treeNode2.Name = "SupportEngine";
            treeNode2.Text = "Support Engine";
            treeNode3.Name = "Performance";
            treeNode3.Text = "Performance";
            treeNode4.Name = "Licenses";
            treeNode4.Text = "Licenses";
            this.trAdvancedSettings.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode1,
            treeNode2,
            treeNode3,
            treeNode4});
            this.tableLayoutPanel2.SetRowSpan(this.trAdvancedSettings, 2);
            this.trAdvancedSettings.ShowRootLines = false;
            this.trAdvancedSettings.Size = new System.Drawing.Size(73, 250);
            this.trAdvancedSettings.TabIndex = 0;
            this.trAdvancedSettings.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.trAdvancedSettings_AfterSelect);
            // 
            // pgAdvancedSettings
            // 
            this.gpAdvancedSettings.BrowsableProperties = null;
            this.gpAdvancedSettings.CanShowVisualStyleGlyphs = false;
            this.gpAdvancedSettings.CommandsBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(194)))));
            this.gpAdvancedSettings.CommandsDisabledLinkColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.gpAdvancedSettings.CommandsForeColor = System.Drawing.Color.Black;
            this.gpAdvancedSettings.CommandsVisibleIfAvailable = false;
            this.gpAdvancedSettings.Dock = System.Windows.Forms.DockStyle.Fill;
            this.gpAdvancedSettings.HelpVisible = false;
            this.gpAdvancedSettings.HiddenAttributes = null;
            this.gpAdvancedSettings.HiddenProperties = null;
            this.gpAdvancedSettings.LineColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(194)))));
            this.gpAdvancedSettings.Location = new System.Drawing.Point(82, 3);
            this.gpAdvancedSettings.Name = "pgAdvancedSettings";
            this.gpAdvancedSettings.PropertySort = System.Windows.Forms.PropertySort.Alphabetical;
            this.gpAdvancedSettings.RenamedProperties = null;
            this.tableLayoutPanel2.SetRowSpan(this.gpAdvancedSettings, 2);
            this.gpAdvancedSettings.SelectedItemWithFocusBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(194)))));
            this.gpAdvancedSettings.Size = new System.Drawing.Size(179, 250);
            this.gpAdvancedSettings.TabIndex = 1;
            this.gpAdvancedSettings.ToolbarVisible = false;
            this.gpAdvancedSettings.ViewForeColor = System.Drawing.Color.Black;
            // 
            // frmUserPreferences
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(503, 423);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmUserPreferences";
            this.Text = "User Preferences";
            this.plContent.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbDisplay.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.gpDisplaySoftware.ResumeLayout(false);
            this.gpDisplaySoftware.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tbAdvanced.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbDisplay;
        private System.Windows.Forms.TabPage tbAdvanced;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkXYZAxis;
        private System.Windows.Forms.CheckBox chkAnnotations;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.TreeView trAdvancedSettings;
        private PropertyGridFiltered gpAdvancedSettings;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkUserInterfaceUseLargeToolbarIcons;
        private System.Windows.Forms.GroupBox gpDisplaySoftware;
        private System.Windows.Forms.CheckBox chkSoftwareCheckForSoftwareUpdates;
    }
}