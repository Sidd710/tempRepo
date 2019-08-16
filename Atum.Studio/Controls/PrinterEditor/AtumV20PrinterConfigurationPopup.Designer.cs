namespace Atum.Studio.Controls.PrinterEditor
{
    partial class AtumV20PrinterConfigurationPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AtumV20PrinterConfigurationPopup));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.atumV20PrinterProperties1 = new Atum.Studio.Controls.PrinterEditor.AtumV20PrinterProperties();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plContent.Controls.Add(this.pictureBox1);
            this.plContent.Controls.Add(this.atumV20PrinterProperties1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.None;
            this.plContent.Size = new System.Drawing.Size(445, 419);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(389, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // atumV20PrinterProperties1
            // 
            this.atumV20PrinterProperties1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.atumV20PrinterProperties1.BackColor = System.Drawing.Color.White;
            this.atumV20PrinterProperties1.DataSource = null;
            this.atumV20PrinterProperties1.inWizard = false;
            this.atumV20PrinterProperties1.Location = new System.Drawing.Point(0, 0);
            this.atumV20PrinterProperties1.Name = "atumV20PrinterProperties1";
            this.atumV20PrinterProperties1.Size = new System.Drawing.Size(445, 419);
            this.atumV20PrinterProperties1.TabIndex = 3;
            // 
            // AtumV20PrinterConfigurationPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 455);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AtumV20PrinterConfigurationPopup";
            this.Text = "Printer Properties";
            this.plContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.PictureBox pictureBox1;
        private AtumV20PrinterProperties atumV20PrinterProperties1;
    }
}