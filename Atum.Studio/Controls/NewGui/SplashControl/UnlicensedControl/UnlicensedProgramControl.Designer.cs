namespace Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl
{
    partial class UnlicensedProgramControl
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
            this.pbWarning = new System.Windows.Forms.PictureBox();
            this.lblProgramUnlicensed = new System.Windows.Forms.Label();
            this.lblToAuthorise = new System.Windows.Forms.Label();
            this.lblSendTokenFile = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.lbl30DaysTrial = new System.Windows.Forms.LinkLabel();
            this.btnExport = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnImport = new Atum.Studio.Controls.NewGui.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            this.SuspendLayout();
            // 
            // pbWarning
            // 
            this.pbWarning.Location = new System.Drawing.Point(0, 61);
            this.pbWarning.Name = "pbWarning";
            this.pbWarning.Size = new System.Drawing.Size(288, 40);
            this.pbWarning.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbWarning.TabIndex = 0;
            this.pbWarning.TabStop = false;
            // 
            // lblProgramUnlicensed
            // 
            this.lblProgramUnlicensed.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblProgramUnlicensed.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblProgramUnlicensed.ForeColor = System.Drawing.Color.White;
            this.lblProgramUnlicensed.Location = new System.Drawing.Point(0, 133);
            this.lblProgramUnlicensed.Name = "lblProgramUnlicensed";
            this.lblProgramUnlicensed.Size = new System.Drawing.Size(288, 26);
            this.lblProgramUnlicensed.TabIndex = 1;
            this.lblProgramUnlicensed.Text = "No active license found";
            this.lblProgramUnlicensed.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblToAuthorise
            // 
            this.lblToAuthorise.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblToAuthorise.ForeColor = System.Drawing.Color.White;
            this.lblToAuthorise.Location = new System.Drawing.Point(0, 187);
            this.lblToAuthorise.Name = "lblToAuthorise";
            this.lblToAuthorise.Size = new System.Drawing.Size(288, 40);
            this.lblToAuthorise.TabIndex = 2;
            this.lblToAuthorise.Text = "To activate:\r\n1. Export the license token file";
            this.lblToAuthorise.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // lblSendTokenFile
            // 
            this.lblSendTokenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSendTokenFile.ForeColor = System.Drawing.Color.White;
            this.lblSendTokenFile.Location = new System.Drawing.Point(0, 309);
            this.lblSendTokenFile.Name = "lblSendTokenFile";
            this.lblSendTokenFile.Size = new System.Drawing.Size(288, 81);
            this.lblSendTokenFile.TabIndex = 3;
            this.lblSendTokenFile.Text = "2. E-mail the license token file to license@atum3d.com for activation; \r\n    in r" +
    "eply, you’ll receive the activated license token file.";
            this.lblSendTokenFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 407);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "3. Import the activated license token file";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbClose
            // 
            this.pbClose.Location = new System.Drawing.Point(248, 16);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbClose.TabIndex = 8;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // lbl30DaysTrial
            // 
            this.lbl30DaysTrial.ActiveLinkColor = System.Drawing.Color.White;
            this.lbl30DaysTrial.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lbl30DaysTrial.LinkColor = System.Drawing.Color.White;
            this.lbl30DaysTrial.Location = new System.Drawing.Point(0, 529);
            this.lbl30DaysTrial.Name = "lbl30DaysTrial";
            this.lbl30DaysTrial.Size = new System.Drawing.Size(288, 21);
            this.lbl30DaysTrial.TabIndex = 9;
            this.lbl30DaysTrial.TabStop = true;
            this.lbl30DaysTrial.Text = "Continue trial";
            this.lbl30DaysTrial.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lbl30DaysTrial.VisitedLinkColor = System.Drawing.Color.White;
            this.lbl30DaysTrial.Click += new System.EventHandler(this.lbl30DaysTrial_LinkClicked);
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.White;
            this.btnExport.Location = new System.Drawing.Point(86, 245);
            this.btnExport.Margin = new System.Windows.Forms.Padding(0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnExport.Radius = 20;
            this.btnExport.SingleBorder = false;
            this.btnExport.Size = new System.Drawing.Size(114, 42);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(86, 457);
            this.btnImport.Margin = new System.Windows.Forms.Padding(0);
            this.btnImport.Name = "btnImport";
            this.btnImport.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnImport.Radius = 20;
            this.btnImport.SingleBorder = false;
            this.btnImport.Size = new System.Drawing.Size(114, 42);
            this.btnImport.TabIndex = 19;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // UnlicensedProgramControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.lbl30DaysTrial);
            this.Controls.Add(this.pbClose);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblSendTokenFile);
            this.Controls.Add(this.lblToAuthorise);
            this.Controls.Add(this.lblProgramUnlicensed);
            this.Controls.Add(this.pbWarning);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "UnlicensedProgramControl";
            this.Size = new System.Drawing.Size(288, 576);
            ((System.ComponentModel.ISupportInitialize)(this.pbWarning)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbWarning;
        private System.Windows.Forms.Label lblProgramUnlicensed;
        private System.Windows.Forms.Label lblToAuthorise;
        private System.Windows.Forms.Label lblSendTokenFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.LinkLabel lbl30DaysTrial;
        private NewGui.RoundedButton btnExport;
        private NewGui.RoundedButton btnImport;
    }
}
