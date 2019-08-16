namespace Atum.Studio.Controls.PrinterEditor
{
    partial class AtumDLPStation5PrinterConfigurationPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AtumDLPStation5PrinterConfigurationPopup));
            this.atum40PrinterProperties1 = new Atum.Studio.Controls.PrinterEditor.AtumDLPStation5PrinterProperties();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
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
            this.plContent.Controls.Add(this.atum40PrinterProperties1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.None;
            this.plContent.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.plContent.Size = new System.Drawing.Size(446, 416);
            // 
            // atum40PrinterProperties1
            // 
            this.atum40PrinterProperties1.BackColor = System.Drawing.Color.White;
            this.atum40PrinterProperties1.DataSource = null;
            this.atum40PrinterProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atum40PrinterProperties1.inWizard = false;
            this.atum40PrinterProperties1.Location = new System.Drawing.Point(0, 0);
            this.atum40PrinterProperties1.Margin = new System.Windows.Forms.Padding(10, 4, 4, 4);
            this.atum40PrinterProperties1.Name = "atum40PrinterProperties1";
            this.atum40PrinterProperties1.Padding = new System.Windows.Forms.Padding(4, 4, 0, 6);
            this.atum40PrinterProperties1.Size = new System.Drawing.Size(446, 416);
            this.atum40PrinterProperties1.TabIndex = 1;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(390, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(48, 48);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // AtumDLPStation5PrinterConfigurationPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 455);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(3, 3, 3, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AtumDLPStation5PrinterConfigurationPopup";
            this.Text = "Printer Properties";
            this.plContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private AtumDLPStation5PrinterProperties atum40PrinterProperties1;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}