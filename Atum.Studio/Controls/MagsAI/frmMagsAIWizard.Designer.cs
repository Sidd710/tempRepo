namespace Atum.Studio.Controls.MagsAI
{
    partial class frmMagsAIWizard
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
            this.tbWizard = new System.Windows.Forms.TabControl();
            this.tbWelcome = new System.Windows.Forms.TabPage();
            this.tbMagsAIPrinterMaterial = new System.Windows.Forms.TabPage();
            this.tbMagsAIOrientation = new System.Windows.Forms.TabPage();
            this.tbPreview = new System.Windows.Forms.TabPage();
            this.plFooter = new System.Windows.Forms.Panel();
            this.btnDebug = new System.Windows.Forms.Button();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtSavePath = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.magsAIWelcomeTab1 = new Atum.Studio.Controls.MagsAI.MagsAISelect3DModelTabPanel();
            this.magsAIMaterialTabPanel1 = new Atum.Studio.Controls.MagsAI.MagsAIMaterialTabPanel();
            this.magsAIOrientationTabPanel1 = new Atum.Studio.Controls.MagsAI.MagsAIOrientationTabPanel();
            this.magsAIPreview1 = new Atum.Studio.Controls.MagsAI.MagsAIPreview();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnCreateScreenshot = new System.Windows.Forms.Button();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.btnSaveMagsAIDebugInformation = new System.Windows.Forms.Button();
            this.tbWizard.SuspendLayout();
            this.tbWelcome.SuspendLayout();
            this.tbMagsAIPrinterMaterial.SuspendLayout();
            this.tbMagsAIOrientation.SuspendLayout();
            this.tbPreview.SuspendLayout();
            this.plFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWizard
            // 
            this.tbWizard.Controls.Add(this.tbWelcome);
            this.tbWizard.Controls.Add(this.tbMagsAIPrinterMaterial);
            this.tbWizard.Controls.Add(this.tbMagsAIOrientation);
            this.tbWizard.Controls.Add(this.tbPreview);
            this.tbWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWizard.Location = new System.Drawing.Point(0, 0);
            this.tbWizard.Margin = new System.Windows.Forms.Padding(0);
            this.tbWizard.Name = "tbWizard";
            this.tbWizard.SelectedIndex = 0;
            this.tbWizard.Size = new System.Drawing.Size(600, 606);
            this.tbWizard.TabIndex = 2;
            this.tbWizard.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbWizard_Selecting);
            // 
            // tbWelcome
            // 
            this.tbWelcome.Controls.Add(this.magsAIWelcomeTab1);
            this.tbWelcome.Location = new System.Drawing.Point(4, 22);
            this.tbWelcome.Name = "tbWelcome";
            this.tbWelcome.Size = new System.Drawing.Size(592, 580);
            this.tbWelcome.TabIndex = 0;
            this.tbWelcome.Text = "Welcome";
            this.tbWelcome.UseVisualStyleBackColor = true;
            // 
            // tbMagsAIPrinterMaterial
            // 
            this.tbMagsAIPrinterMaterial.Controls.Add(this.magsAIMaterialTabPanel1);
            this.tbMagsAIPrinterMaterial.Location = new System.Drawing.Point(4, 22);
            this.tbMagsAIPrinterMaterial.Name = "tbMagsAIPrinterMaterial";
            this.tbMagsAIPrinterMaterial.Size = new System.Drawing.Size(592, 580);
            this.tbMagsAIPrinterMaterial.TabIndex = 6;
            this.tbMagsAIPrinterMaterial.Text = "Printer/Material";
            this.tbMagsAIPrinterMaterial.UseVisualStyleBackColor = true;
            // 
            // tbMagsAIOrientation
            // 
            this.tbMagsAIOrientation.Controls.Add(this.magsAIOrientationTabPanel1);
            this.tbMagsAIOrientation.Location = new System.Drawing.Point(4, 22);
            this.tbMagsAIOrientation.Name = "tbMagsAIOrientation";
            this.tbMagsAIOrientation.Size = new System.Drawing.Size(592, 580);
            this.tbMagsAIOrientation.TabIndex = 3;
            this.tbMagsAIOrientation.Text = "Orientation";
            this.tbMagsAIOrientation.UseVisualStyleBackColor = true;
            // 
            // tbPreview
            // 
            this.tbPreview.Controls.Add(this.magsAIPreview1);
            this.tbPreview.Location = new System.Drawing.Point(4, 22);
            this.tbPreview.Name = "tbPreview";
            this.tbPreview.Size = new System.Drawing.Size(592, 580);
            this.tbPreview.TabIndex = 4;
            this.tbPreview.Text = "Preview";
            this.tbPreview.UseVisualStyleBackColor = true;
            // 
            // plFooter
            // 
            this.plFooter.Controls.Add(this.btnDebug);
            this.plFooter.Controls.Add(this.btnBack);
            this.plFooter.Controls.Add(this.btnNext);
            this.plFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plFooter.Location = new System.Drawing.Point(0, 606);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(948, 41);
            this.plFooter.TabIndex = 3;
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(12, 9);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(75, 23);
            this.btnDebug.TabIndex = 2;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.AutoSize = true;
            this.btnBack.Location = new System.Drawing.Point(781, 7);
            this.btnBack.Name = "btnBack";
            this.btnBack.Size = new System.Drawing.Size(75, 27);
            this.btnBack.TabIndex = 1;
            this.btnBack.Text = "Back";
            this.btnBack.UseVisualStyleBackColor = true;
            this.btnBack.Click += new System.EventHandler(this.btnBack_Click);
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.AutoSize = true;
            this.btnNext.Location = new System.Drawing.Point(862, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 27);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tbWizard);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tableLayoutPanel1);
            this.splitContainer1.Panel2MinSize = 0;
            this.splitContainer1.Size = new System.Drawing.Size(948, 606);
            this.splitContainer1.SplitterDistance = 600;
            this.splitContainer1.SplitterWidth = 1;
            this.splitContainer1.TabIndex = 1;
            // 
            // txtSavePath
            // 
            this.txtSavePath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtSavePath.Location = new System.Drawing.Point(82, 549);
            this.txtSavePath.Name = "txtSavePath";
            this.txtSavePath.Size = new System.Drawing.Size(262, 27);
            this.txtSavePath.TabIndex = 5;
            this.txtSavePath.TabStop = true;
            this.txtSavePath.Text = "<RoamingLoggingPath>";
            this.txtSavePath.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.txtSavePath_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label1.Location = new System.Drawing.Point(3, 549);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 27);
            this.label1.TabIndex = 4;
            this.label1.Text = "Save path:";
            // 
            // magsAIWelcomeTab1
            // 
            this.magsAIWelcomeTab1.AllowDrop = true;
            this.magsAIWelcomeTab1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magsAIWelcomeTab1.Location = new System.Drawing.Point(0, 0);
            this.magsAIWelcomeTab1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIWelcomeTab1.Name = "magsAIWelcomeTab1";
            this.magsAIWelcomeTab1.Padding = new System.Windows.Forms.Padding(3, 3, 10, 6);
            this.magsAIWelcomeTab1.Size = new System.Drawing.Size(592, 580);
            this.magsAIWelcomeTab1.TabIndex = 0;
            // 
            // magsAIMaterialTabPanel1
            // 
            this.magsAIMaterialTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.magsAIMaterialTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIMaterialTabPanel1.Name = "magsAIMaterialTabPanel1";
            this.magsAIMaterialTabPanel1.Size = new System.Drawing.Size(940, 621);
            this.magsAIMaterialTabPanel1.TabIndex = 0;
            // 
            // magsAIOrientationTabPanel1
            // 
            this.magsAIOrientationTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magsAIOrientationTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.magsAIOrientationTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIOrientationTabPanel1.Name = "magsAIOrientationTabPanel1";
            this.magsAIOrientationTabPanel1.Size = new System.Drawing.Size(592, 580);
            this.magsAIOrientationTabPanel1.TabIndex = 0;
            // 
            // magsAIPreview1
            // 
            this.magsAIPreview1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magsAIPreview1.Location = new System.Drawing.Point(0, 0);
            this.magsAIPreview1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIPreview1.Name = "magsAIPreview1";
            this.magsAIPreview1.Size = new System.Drawing.Size(592, 580);
            this.magsAIPreview1.TabIndex = 0;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 23.05475F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 76.94524F));
            this.tableLayoutPanel1.Controls.Add(this.btnCreateScreenshot, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.flowLayoutPanel1, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.label1, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.btnSaveMagsAIDebugInformation, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.txtSavePath, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 4;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 5.309734F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 94.69027F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(347, 606);
            this.tableLayoutPanel1.TabIndex = 6;
            // 
            // btnCreateScreenshot
            // 
            this.btnCreateScreenshot.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCreateScreenshot.Location = new System.Drawing.Point(223, 3);
            this.btnCreateScreenshot.Name = "btnCreateScreenshot";
            this.btnCreateScreenshot.Size = new System.Drawing.Size(121, 23);
            this.btnCreateScreenshot.TabIndex = 3;
            this.btnCreateScreenshot.Text = "Add comment";
            this.btnCreateScreenshot.UseVisualStyleBackColor = true;
            this.btnCreateScreenshot.Click += new System.EventHandler(this.btnCreateScreenshot_Click);
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.AutoScroll = true;
            this.tableLayoutPanel1.SetColumnSpan(this.flowLayoutPanel1, 2);
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(3, 32);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(341, 514);
            this.flowLayoutPanel1.TabIndex = 4;
            // 
            // btnSaveMagsAIDebugInformation
            // 
            this.btnSaveMagsAIDebugInformation.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveMagsAIDebugInformation.Location = new System.Drawing.Point(269, 580);
            this.btnSaveMagsAIDebugInformation.Name = "btnSaveMagsAIDebugInformation";
            this.btnSaveMagsAIDebugInformation.Size = new System.Drawing.Size(75, 23);
            this.btnSaveMagsAIDebugInformation.TabIndex = 5;
            this.btnSaveMagsAIDebugInformation.Text = "Save";
            this.btnSaveMagsAIDebugInformation.UseVisualStyleBackColor = true;
            // 
            // frmMagsAIWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(948, 647);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.plFooter);
            this.Name = "frmMagsAIWizard";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MAGS AI Wizard";
            this.Shown += new System.EventHandler(this.frmMagsAIWizard_Shown);
            this.tbWizard.ResumeLayout(false);
            this.tbWelcome.ResumeLayout(false);
            this.tbMagsAIPrinterMaterial.ResumeLayout(false);
            this.tbMagsAIOrientation.ResumeLayout(false);
            this.tbPreview.ResumeLayout(false);
            this.plFooter.ResumeLayout(false);
            this.plFooter.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbWizard;
        private System.Windows.Forms.TabPage tbWelcome;
        private System.Windows.Forms.TabPage tbMagsAIOrientation;
        private System.Windows.Forms.TabPage tbPreview;
        private MagsAISelect3DModelTabPanel magsAIWelcomeTab1;
        private MagsAIOrientationTabPanel magsAIOrientationTabPanel1;
        private System.Windows.Forms.TabPage tbMagsAIPrinterMaterial;
        private MagsAIMaterialTabPanel magsAIMaterialTabPanel1;
        private MagsAIPreview magsAIPreview1;
        private System.Windows.Forms.Panel plFooter;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.Button btnDebug;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.LinkLabel txtSavePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnCreateScreenshot;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.Button btnSaveMagsAIDebugInformation;
    }
}