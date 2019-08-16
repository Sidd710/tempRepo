namespace Atum.Studio.Controls.NewGui.SplashControl.LicensedControl
{
    partial class LicensedProgramControl
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
            this.pbTick = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbPlusSign = new System.Windows.Forms.PictureBox();
            this.btnSave = new Atum.Studio.Controls.NewGui.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbTick)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlusSign)).BeginInit();
            this.SuspendLayout();
            // 
            // pbTick
            // 
            this.pbTick.Location = new System.Drawing.Point(0, 258);
            this.pbTick.Name = "pbTick";
            this.pbTick.Size = new System.Drawing.Size(288, 25);
            this.pbTick.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbTick.TabIndex = 0;
            this.pbTick.TabStop = false;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(0, 299);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 21);
            this.label1.TabIndex = 4;
            this.label1.Text = "Program authorised";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pbClose
            // 
            this.pbClose.Image = global::Atum.Studio.Properties.Resources.button_cross_white;
            this.pbClose.Location = new System.Drawing.Point(248, 16);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbClose.TabIndex = 8;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pbClose_Click);
            // 
            // pbPlusSign
            // 
            this.pbPlusSign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pbPlusSign.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pbPlusSign.BackgroundImage = global::Atum.Studio.Properties.Resources.btn_StartMainForm_24;
            this.pbPlusSign.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.pbPlusSign.Location = new System.Drawing.Point(232, 519);
            this.pbPlusSign.Name = "pbPlusSign";
            this.pbPlusSign.Padding = new System.Windows.Forms.Padding(8);
            this.pbPlusSign.Size = new System.Drawing.Size(41, 41);
            this.pbPlusSign.TabIndex = 9;
            this.pbPlusSign.TabStop = false;
            this.pbPlusSign.Click += new System.EventHandler(this.pbPlusSign_Click);
            this.pbPlusSign.Resize += new System.EventHandler(this.pbPlusSign_Resize);
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
            this.btnSave.TabIndex = 17;
            this.btnSave.Text = "Open File...";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // LicensedProgramControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.pbPlusSign);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pbTick);
            this.Controls.Add(this.pbClose);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "LicensedProgramControl";
            this.Size = new System.Drawing.Size(288, 576);
            ((System.ComponentModel.ISupportInitialize)(this.pbTick)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlusSign)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pbTick;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.PictureBox pbPlusSign;
        private NewGui.RoundedButton btnSave;
    }
}
