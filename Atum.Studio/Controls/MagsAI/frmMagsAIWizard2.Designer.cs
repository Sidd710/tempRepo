namespace Atum.Studio.Controls.MagsAI
{
    partial class frmMagsAIWizard2
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
            System.Windows.Forms.TreeNode treeNode6 = new System.Windows.Forms.TreeNode("Select 3D Model");
            System.Windows.Forms.TreeNode treeNode7 = new System.Windows.Forms.TreeNode("Select Properties");
            System.Windows.Forms.TreeNode treeNode8 = new System.Windows.Forms.TreeNode("Select Orientation");
            System.Windows.Forms.TreeNode treeNode9 = new System.Windows.Forms.TreeNode("Processing");
            System.Windows.Forms.TreeNode treeNode10 = new System.Windows.Forms.TreeNode("Preview");
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.panel1 = new System.Windows.Forms.Panel();
            this.plFooter = new System.Windows.Forms.Panel();
            this.btnBack = new System.Windows.Forms.Button();
            this.btnNext = new System.Windows.Forms.Button();
            this.plWizardSteps = new System.Windows.Forms.Panel();
            this.trWizardSteps = new System.Windows.Forms.TreeView();
            this.tbWizard = new System.Windows.Forms.TabControl();
            this.tbSelect3DModel = new System.Windows.Forms.TabPage();
            this.tbSelectProperties = new System.Windows.Forms.TabPage();
            this.tbSelectOrientation = new System.Windows.Forms.TabPage();
            this.tbProcessing = new System.Windows.Forms.TabPage();
            this.tbPreview = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.plHeader = new System.Windows.Forms.Panel();
            this.magsAIWelcomeTabPanel1 = new Atum.Studio.Controls.MagsAI.MagsAISelect3DModelTabPanel();
            this.magsAIMaterialTabPanel1 = new Atum.Studio.Controls.MagsAI.MagsAIMaterialTabPanel();
            this.magsAIOrientationTabPanel1 = new Atum.Studio.Controls.MagsAI.MagsAIOrientationTabPanel();
            this.magsAIPreview1 = new Atum.Studio.Controls.MagsAI.MagsAIPreview();
            this.panel1.SuspendLayout();
            this.plFooter.SuspendLayout();
            this.plWizardSteps.SuspendLayout();
            this.tbWizard.SuspendLayout();
            this.tbSelect3DModel.SuspendLayout();
            this.tbSelectProperties.SuspendLayout();
            this.tbSelectOrientation.SuspendLayout();
            this.tbPreview.SuspendLayout();
            this.plHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackColor = System.Drawing.Color.White;
            this.panel1.Controls.Add(this.plFooter);
            this.panel1.Controls.Add(this.plWizardSteps);
            this.panel1.Controls.Add(this.tbWizard);
            this.panel1.Location = new System.Drawing.Point(0, 104);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(1294, 570);
            this.panel1.TabIndex = 0;
            // 
            // plFooter
            // 
            this.plFooter.Controls.Add(this.btnBack);
            this.plFooter.Controls.Add(this.btnNext);
            this.plFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plFooter.Location = new System.Drawing.Point(0, 529);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(1294, 41);
            this.plFooter.TabIndex = 2;
            this.plFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.plFooter_Paint);
            // 
            // btnBack
            // 
            this.btnBack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnBack.AutoSize = true;
            this.btnBack.Location = new System.Drawing.Point(1127, 7);
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
            this.btnNext.Location = new System.Drawing.Point(1208, 7);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 27);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // plWizardSteps
            // 
            this.plWizardSteps.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.plWizardSteps.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plWizardSteps.Controls.Add(this.trWizardSteps);
            this.plWizardSteps.Location = new System.Drawing.Point(0, -5);
            this.plWizardSteps.Name = "plWizardSteps";
            this.plWizardSteps.Size = new System.Drawing.Size(272, 546);
            this.plWizardSteps.TabIndex = 1;
            // 
            // trWizardSteps
            // 
            this.trWizardSteps.BackColor = System.Drawing.Color.WhiteSmoke;
            this.trWizardSteps.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.trWizardSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.trWizardSteps.Location = new System.Drawing.Point(19, 18);
            this.trWizardSteps.Name = "trWizardSteps";
            treeNode6.Name = "select3DModelNode";
            treeNode6.Text = "Select 3D Model";
            treeNode7.Name = "Node0";
            treeNode7.Text = "Select Properties";
            treeNode8.Name = "selectOrientationNode";
            treeNode8.Text = "Select Orientation";
            treeNode9.Name = "processingNode";
            treeNode9.Text = "Processing";
            treeNode10.Name = "previewNode";
            treeNode10.Text = "Preview";
            this.trWizardSteps.Nodes.AddRange(new System.Windows.Forms.TreeNode[] {
            treeNode6,
            treeNode7,
            treeNode8,
            treeNode9,
            treeNode10});
            this.trWizardSteps.ShowLines = false;
            this.trWizardSteps.ShowPlusMinus = false;
            this.trWizardSteps.ShowRootLines = false;
            this.trWizardSteps.Size = new System.Drawing.Size(250, 293);
            this.trWizardSteps.TabIndex = 0;
            this.trWizardSteps.TabStop = false;
            this.trWizardSteps.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trWizardSteps_BeforeSelect);
            // 
            // tbWizard
            // 
            this.tbWizard.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.tbWizard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWizard.Controls.Add(this.tbSelect3DModel);
            this.tbWizard.Controls.Add(this.tbSelectProperties);
            this.tbWizard.Controls.Add(this.tbSelectOrientation);
            this.tbWizard.Controls.Add(this.tbProcessing);
            this.tbWizard.Controls.Add(this.tbPreview);
            this.tbWizard.Location = new System.Drawing.Point(265, -2);
            this.tbWizard.Name = "tbWizard";
            this.tbWizard.SelectedIndex = 0;
            this.tbWizard.Size = new System.Drawing.Size(1038, 555);
            this.tbWizard.TabIndex = 3;
            this.tbWizard.SelectedIndexChanged += new System.EventHandler(this.tbWizard_SelectedIndexChanged);
            this.tbWizard.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbWizard_Selecting);
            // 
            // tbSelect3DModel
            // 
            this.tbSelect3DModel.Controls.Add(this.magsAIWelcomeTabPanel1);
            this.tbSelect3DModel.Location = new System.Drawing.Point(4, 4);
            this.tbSelect3DModel.Name = "tbSelect3DModel";
            this.tbSelect3DModel.Padding = new System.Windows.Forms.Padding(3);
            this.tbSelect3DModel.Size = new System.Drawing.Size(1030, 529);
            this.tbSelect3DModel.TabIndex = 1;
            this.tbSelect3DModel.Text = "Select 3D Model";
            this.tbSelect3DModel.UseVisualStyleBackColor = true;
            // 
            // tbSelectProperties
            // 
            this.tbSelectProperties.Controls.Add(this.magsAIMaterialTabPanel1);
            this.tbSelectProperties.Location = new System.Drawing.Point(4, 4);
            this.tbSelectProperties.Name = "tbSelectProperties";
            this.tbSelectProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tbSelectProperties.Size = new System.Drawing.Size(1030, 529);
            this.tbSelectProperties.TabIndex = 0;
            this.tbSelectProperties.Text = "Select Properties";
            this.tbSelectProperties.UseVisualStyleBackColor = true;
            // 
            // tbSelectOrientation
            // 
            this.tbSelectOrientation.Controls.Add(this.magsAIOrientationTabPanel1);
            this.tbSelectOrientation.Location = new System.Drawing.Point(4, 4);
            this.tbSelectOrientation.Name = "tbSelectOrientation";
            this.tbSelectOrientation.Size = new System.Drawing.Size(1030, 529);
            this.tbSelectOrientation.TabIndex = 2;
            this.tbSelectOrientation.Text = "Select Orientation";
            this.tbSelectOrientation.UseVisualStyleBackColor = true;
            // 
            // tbProcessing
            // 
            this.tbProcessing.Location = new System.Drawing.Point(4, 4);
            this.tbProcessing.Name = "tbProcessing";
            this.tbProcessing.Size = new System.Drawing.Size(1030, 529);
            this.tbProcessing.TabIndex = 3;
            this.tbProcessing.Text = "Processing";
            this.tbProcessing.UseVisualStyleBackColor = true;
            // 
            // tbPreview
            // 
            this.tbPreview.Controls.Add(this.magsAIPreview1);
            this.tbPreview.Location = new System.Drawing.Point(4, 4);
            this.tbPreview.Name = "tbPreview";
            this.tbPreview.Size = new System.Drawing.Size(1030, 529);
            this.tbPreview.TabIndex = 4;
            this.tbPreview.Text = "Preview";
            this.tbPreview.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(12, 37);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(260, 37);
            this.label1.TabIndex = 1;
            this.label1.Text = "MAGS AI Wizard";
            // 
            // plHeader
            // 
            this.plHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plHeader.Controls.Add(this.label1);
            this.plHeader.Location = new System.Drawing.Point(-5, -7);
            this.plHeader.Margin = new System.Windows.Forms.Padding(0);
            this.plHeader.Name = "plHeader";
            this.plHeader.Size = new System.Drawing.Size(1327, 110);
            this.plHeader.TabIndex = 2;
            // 
            // magsAIWelcomeTabPanel1
            // 
            this.magsAIWelcomeTabPanel1.AllowDrop = true;
            this.magsAIWelcomeTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magsAIWelcomeTabPanel1.Location = new System.Drawing.Point(3, 3);
            this.magsAIWelcomeTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIWelcomeTabPanel1.Name = "magsAIWelcomeTabPanel1";
            this.magsAIWelcomeTabPanel1.Padding = new System.Windows.Forms.Padding(3, 3, 3, 6);
            this.magsAIWelcomeTabPanel1.Size = new System.Drawing.Size(1024, 523);
            this.magsAIWelcomeTabPanel1.TabIndex = 0;
            // 
            // magsAIMaterialTabPanel1
            // 
            this.magsAIMaterialTabPanel1.Location = new System.Drawing.Point(3, 3);
            this.magsAIMaterialTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIMaterialTabPanel1.Name = "magsAIMaterialTabPanel1";
            this.magsAIMaterialTabPanel1.Size = new System.Drawing.Size(1025, 518);
            this.magsAIMaterialTabPanel1.TabIndex = 0;
            // 
            // magsAIOrientationTabPanel1
            // 
            this.magsAIOrientationTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magsAIOrientationTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.magsAIOrientationTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIOrientationTabPanel1.Name = "magsAIOrientationTabPanel1";
            this.magsAIOrientationTabPanel1.Size = new System.Drawing.Size(1030, 529);
            this.magsAIOrientationTabPanel1.TabIndex = 0;
            // 
            // magsAIPreview1
            // 
            this.magsAIPreview1.ButtonBackEnabled = true;
            this.magsAIPreview1.ButtonBackVisible = false;
            this.magsAIPreview1.ButtonFinished = false;
            this.magsAIPreview1.ButtonNextEnabled = true;
            this.magsAIPreview1.ButtonNextText = "Next";
            this.magsAIPreview1.ButtonNextVisible = false;
            this.magsAIPreview1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // magsAIPreview1.Header
            // 
            this.magsAIPreview1.Header.BackColor = System.Drawing.Color.White;
            this.magsAIPreview1.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.magsAIPreview1.Header.Location = new System.Drawing.Point(0, 0);
            this.magsAIPreview1.Header.Name = "Header";
            this.magsAIPreview1.Header.Size = new System.Drawing.Size(1030, 55);
            this.magsAIPreview1.Header.TabIndex = 1;
            this.magsAIPreview1.HideFooter = true;
            this.magsAIPreview1.Location = new System.Drawing.Point(0, 0);
            this.magsAIPreview1.Margin = new System.Windows.Forms.Padding(0);
            this.magsAIPreview1.Name = "magsAIPreview1";
            this.magsAIPreview1.Size = new System.Drawing.Size(1030, 529);
            this.magsAIPreview1.TabIndex = 0;
            // 
            // frmMagsAIWizard2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1294, 674);
            this.Controls.Add(this.plHeader);
            this.Controls.Add(this.panel1);
            this.Name = "frmMagsAIWizard2";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "MAGS AI Wizard";
            this.Load += new System.EventHandler(this.frmMagsAIWizard2_Load);
            this.panel1.ResumeLayout(false);
            this.plFooter.ResumeLayout(false);
            this.plFooter.PerformLayout();
            this.plWizardSteps.ResumeLayout(false);
            this.tbWizard.ResumeLayout(false);
            this.tbSelect3DModel.ResumeLayout(false);
            this.tbSelectProperties.ResumeLayout(false);
            this.tbSelectOrientation.ResumeLayout(false);
            this.tbPreview.ResumeLayout(false);
            this.plHeader.ResumeLayout(false);
            this.plHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel plWizardSteps;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TreeView trWizardSteps;
        private System.Windows.Forms.Panel plFooter;
        private System.Windows.Forms.Button btnBack;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.TabControl tbWizard;
        private System.Windows.Forms.TabPage tbSelect3DModel;
        private System.Windows.Forms.TabPage tbSelectProperties;
        private System.Windows.Forms.TabPage tbSelectOrientation;
        private System.Windows.Forms.TabPage tbProcessing;
        private System.Windows.Forms.TabPage tbPreview;
        private MagsAISelect3DModelTabPanel magsAIWelcomeTabPanel1;
        private MagsAIOrientationTabPanel magsAIOrientationTabPanel1;
        private MagsAIMaterialTabPanel magsAIMaterialTabPanel1;
        private MagsAIPreview magsAIPreview1;
        private System.Windows.Forms.Panel plHeader;
    }
}