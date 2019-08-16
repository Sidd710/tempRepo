namespace Atum.Studio.Controls.MagsAI
{
    partial class MagsAISelect3DModelTabPanel
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
            this.label1 = new System.Windows.Forms.Label();
            this.txt3DModelFilePath = new System.Windows.Forms.TextBox();
            this.btnSelect3DModel = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lblDropLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(53, 65);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Pick a 3D Model:";
            // 
            // txt3DModelFilePath
            // 
            this.txt3DModelFilePath.Location = new System.Drawing.Point(75, 79);
            this.txt3DModelFilePath.Name = "txt3DModelFilePath";
            this.txt3DModelFilePath.ReadOnly = true;
            this.txt3DModelFilePath.Size = new System.Drawing.Size(229, 20);
            this.txt3DModelFilePath.TabIndex = 1;
            // 
            // btnSelect3DModel
            // 
            this.btnSelect3DModel.Location = new System.Drawing.Point(311, 79);
            this.btnSelect3DModel.Name = "btnSelect3DModel";
            this.btnSelect3DModel.Size = new System.Drawing.Size(75, 20);
            this.btnSelect3DModel.TabIndex = 2;
            this.btnSelect3DModel.Text = "Select";
            this.btnSelect3DModel.UseVisualStyleBackColor = true;
            this.btnSelect3DModel.Click += new System.EventHandler(this.btnSelect3DModel_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(53, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(18, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Or";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.lblDropLabel);
            this.panel1.Location = new System.Drawing.Point(75, 122);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(229, 164);
            this.panel1.TabIndex = 4;
            // 
            // lblDropLabel
            // 
            this.lblDropLabel.BackColor = System.Drawing.SystemColors.Control;
            this.lblDropLabel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblDropLabel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblDropLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblDropLabel.Location = new System.Drawing.Point(0, 0);
            this.lblDropLabel.Name = "lblDropLabel";
            this.lblDropLabel.Size = new System.Drawing.Size(229, 164);
            this.lblDropLabel.TabIndex = 0;
            this.lblDropLabel.Text = "Drop  3D Model file (STL)";
            this.lblDropLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(3, 444);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(593, 16);
            this.progressBar1.TabIndex = 5;
            // 
            // MagsAISelect3DModelTabPanel
            // 
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSelect3DModel);
            this.Controls.Add(this.txt3DModelFilePath);
            this.Controls.Add(this.label1);
            this.Name = "MagsAISelect3DModelTabPanel";
            this.Padding = new System.Windows.Forms.Padding(3, 3, 10, 6);
            this.Size = new System.Drawing.Size(609, 463);
            this.panel1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelect3DModel;
        private System.Windows.Forms.TextBox txt3DModelFilePath;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lblDropLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}
