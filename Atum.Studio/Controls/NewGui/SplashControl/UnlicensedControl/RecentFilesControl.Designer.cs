namespace Atum.Studio.Controls.NewGui.SplashControl
{
    partial class RecentFilesControl
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
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbPlusSign = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.plRecentFiles = new System.Windows.Forms.Panel();
            this.btnSave = new Atum.Studio.Controls.NewGui.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlusSign)).BeginInit();
            this.SuspendLayout();
            // 
            // pbClose
            // 
            this.pbClose.Location = new System.Drawing.Point(248, 17);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbClose.TabIndex = 8;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // pbPlusSign
            // 
            this.pbPlusSign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pbPlusSign.Image = global::Atum.Studio.Properties.Resources.btn_StartMainForm_24;
            this.pbPlusSign.Location = new System.Drawing.Point(232, 519);
            this.pbPlusSign.Name = "pbPlusSign";
            this.pbPlusSign.Size = new System.Drawing.Size(40, 40);
            this.pbPlusSign.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbPlusSign.TabIndex = 9;
            this.pbPlusSign.TabStop = false;
            this.pbPlusSign.Click += new System.EventHandler(this.pbPlusSign_Click);
            this.pbPlusSign.Resize += new System.EventHandler(this.pbPlusSign_Resize);
            // 
            // lblHeader
            // 
            this.lblHeader.AutoSize = true;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(16, 21);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(94, 17);
            this.lblHeader.TabIndex = 10;
            this.lblHeader.Text = "Recent files";
            // 
            // plRecentFiles
            // 
            this.plRecentFiles.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.plRecentFiles.Location = new System.Drawing.Point(2, 56);
            this.plRecentFiles.Margin = new System.Windows.Forms.Padding(0);
            this.plRecentFiles.Name = "plRecentFiles";
            this.plRecentFiles.Size = new System.Drawing.Size(284, 450);
            this.plRecentFiles.TabIndex = 11;
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(16, 518);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnSave.Radius = 20;
            this.btnSave.SingleBorder = false;
            this.btnSave.Size = new System.Drawing.Size(202, 42);
            this.btnSave.TabIndex = 18;
            this.btnSave.Text = "Open File...";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // RecentFilesControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.plRecentFiles);
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.pbPlusSign);
            this.Controls.Add(this.pbClose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "RecentFilesControl";
            this.Size = new System.Drawing.Size(288, 576);
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlusSign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbPlusSign;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.Panel plRecentFiles;
        private NewGui.RoundedButton btnSave;
    }
}
