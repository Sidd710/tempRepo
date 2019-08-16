using Atum.Studio.Controls;

namespace Atum.Studio
{
    partial class frmStudioMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmStudioMain));
            this.selectObjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.printToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.undoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.userPreferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.modelsToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.systemToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calibrationModelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.managersToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debuggingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debuggingToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.monitorPrinterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.contactToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.licensesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.plTopMenu = new System.Windows.Forms.Panel();
            this.btnPrepare = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnInfo = new System.Windows.Forms.Panel();
            this.btnSettings = new System.Windows.Forms.Panel();
            this.btnAddObject = new System.Windows.Forms.Panel();
            this.btnOpenFileMenu = new System.Windows.Forms.Panel();
            this.plWorkSpace = new System.Windows.Forms.Panel();
            this.plContent = new System.Windows.Forms.Panel();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.plTopMenu.SuspendLayout();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // selectObjectToolStripMenuItem
            // 
            this.selectObjectToolStripMenuItem.Name = "selectObjectToolStripMenuItem";
            this.selectObjectToolStripMenuItem.Size = new System.Drawing.Size(32, 19);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Padding = new System.Windows.Forms.Padding(4, 2, 0, 2);
            this.menuStrip1.Size = new System.Drawing.Size(202, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToolStripMenuItem,
            this.toolStripSeparator1,
            this.importProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.clearProjectToolStripMenuItem,
            this.toolStripSeparator7,
            this.exportToolStripMenuItem,
            this.toolStripSeparator4,
            this.printToolStripMenuItem,
            this.toolStripSeparator2,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // addToolStripMenuItem
            // 
            this.addToolStripMenuItem.Name = "addToolStripMenuItem";
            this.addToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.I)));
            this.addToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.addToolStripMenuItem.Text = "Add Model";
            this.addToolStripMenuItem.Click += new System.EventHandler(this.addToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(205, 6);
            // 
            // importProjectToolStripMenuItem
            // 
            this.importProjectToolStripMenuItem.Name = "importProjectToolStripMenuItem";
            this.importProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.importProjectToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.importProjectToolStripMenuItem.Text = "Open Project";
            this.importProjectToolStripMenuItem.Click += new System.EventHandler(this.importProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveAsProjectToolStripMenuItem_Click);
            // 
            // clearProjectToolStripMenuItem
            // 
            this.clearProjectToolStripMenuItem.Name = "clearProjectToolStripMenuItem";
            this.clearProjectToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.E)));
            this.clearProjectToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.clearProjectToolStripMenuItem.Text = "Clear Project";
            this.clearProjectToolStripMenuItem.Click += new System.EventHandler(this.clearProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(205, 6);
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exportToolStripMenuItem.Text = "Export as STL";
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(205, 6);
            // 
            // printToolStripMenuItem
            // 
            this.printToolStripMenuItem.Name = "printToolStripMenuItem";
            this.printToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.P)));
            this.printToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.printToolStripMenuItem.Text = "Generate PrintJob";
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // editToolStripMenuItem
            // 
            this.editToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.undoToolStripMenuItem,
            this.userPreferencesToolStripMenuItem});
            this.editToolStripMenuItem.Name = "editToolStripMenuItem";
            this.editToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.editToolStripMenuItem.Text = "Edit";
            // 
            // undoToolStripMenuItem
            // 
            this.undoToolStripMenuItem.Name = "undoToolStripMenuItem";
            this.undoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.undoToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.undoToolStripMenuItem.Text = "Undo";
            // 
            // userPreferencesToolStripMenuItem
            // 
            this.userPreferencesToolStripMenuItem.Name = "userPreferencesToolStripMenuItem";
            this.userPreferencesToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.userPreferencesToolStripMenuItem.Text = "User Preferences";
            // 
            // modelsToolStripMenuItem1
            // 
            this.modelsToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.systemToolStripMenuItem});
            this.modelsToolStripMenuItem1.Name = "modelsToolStripMenuItem1";
            this.modelsToolStripMenuItem1.Size = new System.Drawing.Size(58, 20);
            this.modelsToolStripMenuItem1.Text = "Models";
            // 
            // systemToolStripMenuItem
            // 
            this.systemToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.calibrationModelToolStripMenuItem});
            this.systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            this.systemToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.systemToolStripMenuItem.Text = "System";
            // 
            // calibrationModelToolStripMenuItem
            // 
            this.calibrationModelToolStripMenuItem.Name = "calibrationModelToolStripMenuItem";
            this.calibrationModelToolStripMenuItem.Size = new System.Drawing.Size(169, 22);
            this.calibrationModelToolStripMenuItem.Text = "Calibration Model";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.managersToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(47, 20);
            this.toolsToolStripMenuItem.Text = "Tools";
            // 
            // managersToolStripMenuItem
            // 
            this.managersToolStripMenuItem.Name = "managersToolStripMenuItem";
            this.managersToolStripMenuItem.Size = new System.Drawing.Size(67, 22);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debuggingToolStripMenuItem,
            this.toolStripSeparator3,
            this.licensesToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "Help";
            // 
            // debuggingToolStripMenuItem
            // 
            this.debuggingToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debuggingToolStripMenuItem1,
            this.contactToolStripMenuItem});
            this.debuggingToolStripMenuItem.Name = "debuggingToolStripMenuItem";
            this.debuggingToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.debuggingToolStripMenuItem.Text = "Support";
            this.debuggingToolStripMenuItem.Visible = false;
            // 
            // debuggingToolStripMenuItem1
            // 
            this.debuggingToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitorPrinterToolStripMenuItem});
            this.debuggingToolStripMenuItem1.Name = "debuggingToolStripMenuItem1";
            this.debuggingToolStripMenuItem1.Size = new System.Drawing.Size(133, 22);
            this.debuggingToolStripMenuItem1.Text = "Debugging";
            this.debuggingToolStripMenuItem1.Visible = false;
            // 
            // monitorPrinterToolStripMenuItem
            // 
            this.monitorPrinterToolStripMenuItem.Name = "monitorPrinterToolStripMenuItem";
            this.monitorPrinterToolStripMenuItem.Size = new System.Drawing.Size(155, 22);
            this.monitorPrinterToolStripMenuItem.Text = "Monitor printer";
            // 
            // contactToolStripMenuItem
            // 
            this.contactToolStripMenuItem.Name = "contactToolStripMenuItem";
            this.contactToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.contactToolStripMenuItem.Text = "Contact";
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(115, 6);
            // 
            // licensesToolStripMenuItem
            // 
            this.licensesToolStripMenuItem.Name = "licensesToolStripMenuItem";
            this.licensesToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.licensesToolStripMenuItem.Text = "Licenses";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(118, 22);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // splitContainer1
            // 
            this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Margin = new System.Windows.Forms.Padding(2);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.plTopMenu);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.plWorkSpace);
            this.splitContainer1.Size = new System.Drawing.Size(763, 538);
            this.splitContainer1.SplitterDistance = 70;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 9;
            this.splitContainer1.TabStop = false;
            // 
            // plTopMenu
            // 
            this.plTopMenu.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.plTopMenu.Controls.Add(this.btnPrepare);
            this.plTopMenu.Controls.Add(this.btnExport);
            this.plTopMenu.Controls.Add(this.btnInfo);
            this.plTopMenu.Controls.Add(this.btnSettings);
            this.plTopMenu.Controls.Add(this.btnAddObject);
            this.plTopMenu.Controls.Add(this.btnOpenFileMenu);
            this.plTopMenu.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plTopMenu.Location = new System.Drawing.Point(0, 0);
            this.plTopMenu.Name = "plTopMenu";
            this.plTopMenu.Size = new System.Drawing.Size(763, 70);
            this.plTopMenu.TabIndex = 0;
            // 
            // btnPrepare
            // 
            this.btnPrepare.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnPrepare.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnPrepare.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnPrepare.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPrepare.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnPrepare.ForeColor = System.Drawing.Color.White;
            this.btnPrepare.Location = new System.Drawing.Point(311, 15);
            this.btnPrepare.Name = "btnPrepare";
            this.btnPrepare.Size = new System.Drawing.Size(120, 41);
            this.btnPrepare.TabIndex = 15;
            this.btnPrepare.Text = "Prepare";
            this.btnPrepare.UseVisualStyleBackColor = false;
            this.btnPrepare.Click += new System.EventHandler(this.btnPrepare_Click);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(433, 15);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 41);
            this.btnExport.TabIndex = 16;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnInfo
            // 
            this.btnInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnInfo.BackgroundImage = global::Atum.Studio.Properties.Resources.button_help_white1;
            this.btnInfo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnInfo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnInfo.Location = new System.Drawing.Point(653, 15);
            this.btnInfo.Name = "btnInfo";
            this.btnInfo.Size = new System.Drawing.Size(40, 40);
            this.btnInfo.TabIndex = 3;
            this.btnInfo.Click += new System.EventHandler(this.btnInfo_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSettings.BackgroundImage = global::Atum.Studio.Properties.Resources.button_settings_white;
            this.btnSettings.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSettings.Location = new System.Drawing.Point(708, 15);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(40, 40);
            this.btnSettings.TabIndex = 2;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // btnAddObject
            // 
            this.btnAddObject.BackgroundImage = global::Atum.Studio.Properties.Resources.button_add_white_large;
            this.btnAddObject.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnAddObject.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddObject.Location = new System.Drawing.Point(80, 15);
            this.btnAddObject.Name = "btnAddObject";
            this.btnAddObject.Size = new System.Drawing.Size(40, 40);
            this.btnAddObject.TabIndex = 1;
            this.btnAddObject.Click += new System.EventHandler(this.btnAddObject_Click);
            // 
            // btnOpenFileMenu
            // 
            this.btnOpenFileMenu.BackgroundImage = global::Atum.Studio.Properties.Resources.button_folder_white;
            this.btnOpenFileMenu.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnOpenFileMenu.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpenFileMenu.Location = new System.Drawing.Point(15, 15);
            this.btnOpenFileMenu.Name = "btnOpenFileMenu";
            this.btnOpenFileMenu.Size = new System.Drawing.Size(40, 40);
            this.btnOpenFileMenu.TabIndex = 0;
            this.btnOpenFileMenu.Click += new System.EventHandler(this.btnOpenFileMenu_Click);
            // 
            // plWorkSpace
            // 
            this.plWorkSpace.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plWorkSpace.Location = new System.Drawing.Point(0, 0);
            this.plWorkSpace.Margin = new System.Windows.Forms.Padding(2);
            this.plWorkSpace.Name = "plWorkSpace";
            this.plWorkSpace.Size = new System.Drawing.Size(763, 467);
            this.plWorkSpace.TabIndex = 19;
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.Transparent;
            this.plContent.Controls.Add(this.splitContainer1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Location = new System.Drawing.Point(0, 0);
            this.plContent.Margin = new System.Windows.Forms.Padding(2);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(763, 538);
            this.plContent.TabIndex = 3;
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(37, 24);
            this.toolStripButton1.Text = "toolStripButton1";
            // 
            // frmStudioMain
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(763, 538);
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.menuStrip1);
            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "frmStudioMain";
            this.ShowIcon = false;
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmStudioMain_FormClosing);
            this.Load += new System.EventHandler(this.frmStudioMain_Load);
            this.Shown += new System.EventHandler(this.frmStudioMain_Shown);
            this.SizeChanged += new System.EventHandler(this.frmStudioMain_SizeChanged);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.frmStudioMain_KeyDown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.plTopMenu.ResumeLayout(false);
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem printToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearProjectToolStripMenuItem;
        //  private Atum.Studio.Controls.Docking.DockPanel dockPanel;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debuggingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem debuggingToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem monitorPrinterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem selectObjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem contactToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem modelsToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem systemToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        internal System.Windows.Forms.Panel plWorkSpace;
        private System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripMenuItem calibrationModelToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem licensesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem userPreferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem managersToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem undoToolStripMenuItem;
        private System.Windows.Forms.Panel plTopMenu;
        private System.Windows.Forms.Panel btnOpenFileMenu;
        private System.Windows.Forms.Panel btnAddObject;
        private System.Windows.Forms.Panel btnInfo;
        private System.Windows.Forms.Panel btnSettings;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnPrepare;
    }
}