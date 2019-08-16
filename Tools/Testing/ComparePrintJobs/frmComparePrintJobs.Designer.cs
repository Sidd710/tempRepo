namespace ComparePrintJobs
{
    partial class frmComparePrintJobs
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmComparePrintJobs));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.btnOriginalProjobPath = new System.Windows.Forms.Button();
            this.lblOriginalProjobPath = new System.Windows.Forms.Label();
            this.lblOriginalPrintJobHeader = new System.Windows.Forms.Label();
            this.btnCompare = new System.Windows.Forms.Button();
            this.btnComparePrintJobHeader = new System.Windows.Forms.Button();
            this.lblComparePrintJobPath = new System.Windows.Forms.Label();
            this.lblComparePrintJobHeader = new System.Windows.Forms.Label();
            this.plProgressbar = new System.Windows.Forms.Panel();
            this.plProgressbarValue = new System.Windows.Forms.Panel();
            this.txtOutput = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.plProgressbar.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.plProgressbar);
            this.splitContainer1.Panel2.Controls.Add(this.txtOutput);
            this.splitContainer1.Size = new System.Drawing.Size(800, 450);
            this.splitContainer1.SplitterDistance = 266;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.btnOriginalProjobPath);
            this.splitContainer2.Panel1.Controls.Add(this.lblOriginalProjobPath);
            this.splitContainer2.Panel1.Controls.Add(this.lblOriginalPrintJobHeader);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.btnCompare);
            this.splitContainer2.Panel2.Controls.Add(this.btnComparePrintJobHeader);
            this.splitContainer2.Panel2.Controls.Add(this.lblComparePrintJobPath);
            this.splitContainer2.Panel2.Controls.Add(this.lblComparePrintJobHeader);
            this.splitContainer2.Size = new System.Drawing.Size(800, 266);
            this.splitContainer2.SplitterDistance = 400;
            this.splitContainer2.TabIndex = 0;
            // 
            // btnOriginalProjobPath
            // 
            this.btnOriginalProjobPath.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOriginalProjobPath.Location = new System.Drawing.Point(313, 44);
            this.btnOriginalProjobPath.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.btnOriginalProjobPath.Name = "btnOriginalProjobPath";
            this.btnOriginalProjobPath.Size = new System.Drawing.Size(75, 23);
            this.btnOriginalProjobPath.TabIndex = 2;
            this.btnOriginalProjobPath.Text = "Select";
            this.btnOriginalProjobPath.UseVisualStyleBackColor = true;
            this.btnOriginalProjobPath.Click += new System.EventHandler(this.btnOriginalProjobPath_Click);
            // 
            // lblOriginalProjobPath
            // 
            this.lblOriginalProjobPath.Location = new System.Drawing.Point(33, 49);
            this.lblOriginalProjobPath.Name = "lblOriginalProjobPath";
            this.lblOriginalProjobPath.Size = new System.Drawing.Size(274, 18);
            this.lblOriginalProjobPath.TabIndex = 1;
            this.lblOriginalProjobPath.Text = "Select  path -->";
            // 
            // lblOriginalPrintJobHeader
            // 
            this.lblOriginalPrintJobHeader.AutoSize = true;
            this.lblOriginalPrintJobHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblOriginalPrintJobHeader.Location = new System.Drawing.Point(12, 9);
            this.lblOriginalPrintJobHeader.Name = "lblOriginalPrintJobHeader";
            this.lblOriginalPrintJobHeader.Size = new System.Drawing.Size(183, 24);
            this.lblOriginalPrintJobHeader.TabIndex = 0;
            this.lblOriginalPrintJobHeader.Text = "Original printjob path";
            // 
            // btnCompare
            // 
            this.btnCompare.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCompare.Location = new System.Drawing.Point(309, 231);
            this.btnCompare.Margin = new System.Windows.Forms.Padding(3, 3, 12, 12);
            this.btnCompare.Name = "btnCompare";
            this.btnCompare.Size = new System.Drawing.Size(75, 23);
            this.btnCompare.TabIndex = 5;
            this.btnCompare.Text = "Compare";
            this.btnCompare.UseVisualStyleBackColor = true;
            this.btnCompare.Click += new System.EventHandler(this.btnCompare_Click);
            // 
            // btnComparePrintJobHeader
            // 
            this.btnComparePrintJobHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnComparePrintJobHeader.Location = new System.Drawing.Point(309, 44);
            this.btnComparePrintJobHeader.Margin = new System.Windows.Forms.Padding(3, 3, 12, 3);
            this.btnComparePrintJobHeader.Name = "btnComparePrintJobHeader";
            this.btnComparePrintJobHeader.Size = new System.Drawing.Size(75, 23);
            this.btnComparePrintJobHeader.TabIndex = 4;
            this.btnComparePrintJobHeader.Text = "Select";
            this.btnComparePrintJobHeader.UseVisualStyleBackColor = true;
            this.btnComparePrintJobHeader.Click += new System.EventHandler(this.btnComparePrintJobHeader_Click);
            // 
            // lblComparePrintJobPath
            // 
            this.lblComparePrintJobPath.Location = new System.Drawing.Point(29, 49);
            this.lblComparePrintJobPath.Name = "lblComparePrintJobPath";
            this.lblComparePrintJobPath.Size = new System.Drawing.Size(274, 18);
            this.lblComparePrintJobPath.TabIndex = 3;
            this.lblComparePrintJobPath.Text = "Select  path -->";
            // 
            // lblComparePrintJobHeader
            // 
            this.lblComparePrintJobHeader.AutoSize = true;
            this.lblComparePrintJobHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblComparePrintJobHeader.Location = new System.Drawing.Point(12, 9);
            this.lblComparePrintJobHeader.Name = "lblComparePrintJobHeader";
            this.lblComparePrintJobHeader.Size = new System.Drawing.Size(196, 24);
            this.lblComparePrintJobHeader.TabIndex = 1;
            this.lblComparePrintJobHeader.Text = "Compare printjob path";
            // 
            // plProgressbar
            // 
            this.plProgressbar.Controls.Add(this.plProgressbarValue);
            this.plProgressbar.Dock = System.Windows.Forms.DockStyle.Top;
            this.plProgressbar.Location = new System.Drawing.Point(0, 0);
            this.plProgressbar.Name = "plProgressbar";
            this.plProgressbar.Size = new System.Drawing.Size(800, 2);
            this.plProgressbar.TabIndex = 1;
            // 
            // plProgressbarValue
            // 
            this.plProgressbarValue.BackColor = System.Drawing.Color.LimeGreen;
            this.plProgressbarValue.Location = new System.Drawing.Point(0, 0);
            this.plProgressbarValue.Name = "plProgressbarValue";
            this.plProgressbarValue.Size = new System.Drawing.Size(800, 2);
            this.plProgressbarValue.TabIndex = 2;
            // 
            // txtOutput
            // 
            this.txtOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtOutput.Location = new System.Drawing.Point(0, 0);
            this.txtOutput.Multiline = true;
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(800, 180);
            this.txtOutput.TabIndex = 0;
            this.txtOutput.TextChanged += new System.EventHandler(this.txtOutput_TextChanged);
            // 
            // frmComparePrintJobs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmComparePrintJobs";
            this.Text = "atum3D Compare PrintJobs";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            this.splitContainer2.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.plProgressbar.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Button btnOriginalProjobPath;
        private System.Windows.Forms.Label lblOriginalProjobPath;
        private System.Windows.Forms.Label lblOriginalPrintJobHeader;
        private System.Windows.Forms.Button btnComparePrintJobHeader;
        private System.Windows.Forms.Label lblComparePrintJobPath;
        private System.Windows.Forms.Label lblComparePrintJobHeader;
        private System.Windows.Forms.Button btnCompare;
        private System.Windows.Forms.Panel plProgressbar;
        private System.Windows.Forms.Panel plProgressbarValue;
    }
}

