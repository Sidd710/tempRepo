namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionMaterialTabPanel
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
            this.cbMaterialProduct = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblManufacturer = new System.Windows.Forms.Label();
            this.cbMaterialManufacturer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnRefreshUSBDisks = new System.Windows.Forms.Button();
            this.cbUSBDriveLetters = new System.Windows.Forms.ComboBox();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(64, 7);
            this.txtHeader.Size = new System.Drawing.Size(268, 43);
            this.txtHeader.Text = "Calibration";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.printer_blue_icon;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.btnRefreshUSBDisks);
            this.plContent.Controls.Add(this.cbUSBDriveLetters);
            this.plContent.Controls.Add(this.label4);
            this.plContent.Controls.Add(this.cbMaterialProduct);
            this.plContent.Controls.Add(this.label3);
            this.plContent.Controls.Add(this.lblManufacturer);
            this.plContent.Controls.Add(this.cbMaterialManufacturer);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Size = new System.Drawing.Size(443, 325);
            // 
            // cbMaterialProduct
            // 
            this.cbMaterialProduct.DisplayMember = "DisplayName";
            this.cbMaterialProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterialProduct.FormattingEnabled = true;
            this.cbMaterialProduct.Location = new System.Drawing.Point(151, 128);
            this.cbMaterialProduct.Name = "cbMaterialProduct";
            this.cbMaterialProduct.Size = new System.Drawing.Size(206, 24);
            this.cbMaterialProduct.TabIndex = 9;
            this.cbMaterialProduct.SelectedIndexChanged += new System.EventHandler(this.cbMaterialProduct_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(72, 131);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(61, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "Product:";
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.AutoSize = true;
            this.lblManufacturer.Location = new System.Drawing.Point(72, 92);
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(96, 17);
            this.lblManufacturer.TabIndex = 7;
            this.lblManufacturer.Text = "Manufacturer:";
            // 
            // cbMaterialManufacturer
            // 
            this.cbMaterialManufacturer.DisplayMember = "supplier";
            this.cbMaterialManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterialManufacturer.FormattingEnabled = true;
            this.cbMaterialManufacturer.Location = new System.Drawing.Point(151, 89);
            this.cbMaterialManufacturer.Name = "cbMaterialManufacturer";
            this.cbMaterialManufacturer.Size = new System.Drawing.Size(206, 24);
            this.cbMaterialManufacturer.TabIndex = 6;
            this.cbMaterialManufacturer.SelectedIndexChanged += new System.EventHandler(this.cbMaterialManufacturer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(30, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 17);
            this.label1.TabIndex = 5;
            this.label1.Text = "Select material for calibration job:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 179);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(441, 17);
            this.label4.TabIndex = 10;
            this.label4.Text = "Please insert a FAT32 formatted USB flashdrive and select the drive:";
            // 
            // btnRefreshUSBDisks
            // 
            this.btnRefreshUSBDisks.Image = global::Atum.Studio.Properties.Resources.Refresh16;
            this.btnRefreshUSBDisks.Location = new System.Drawing.Point(334, 206);
            this.btnRefreshUSBDisks.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefreshUSBDisks.Name = "btnRefreshUSBDisks";
            this.btnRefreshUSBDisks.Size = new System.Drawing.Size(23, 23);
            this.btnRefreshUSBDisks.TabIndex = 12;
            this.btnRefreshUSBDisks.UseVisualStyleBackColor = true;
            this.btnRefreshUSBDisks.Click += new System.EventHandler(this.btnRefreshUSBDisks_Click);
            // 
            // cbUSBDriveLetters
            // 
            this.cbUSBDriveLetters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSBDriveLetters.FormattingEnabled = true;
            this.cbUSBDriveLetters.Location = new System.Drawing.Point(151, 207);
            this.cbUSBDriveLetters.Margin = new System.Windows.Forms.Padding(2);
            this.cbUSBDriveLetters.Name = "cbUSBDriveLetters";
            this.cbUSBDriveLetters.Size = new System.Drawing.Size(181, 24);
            this.cbUSBDriveLetters.TabIndex = 11;
            // 
            // PrinterConnectionMaterialTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PrinterConnectionMaterialTabPanel";
            this.Size = new System.Drawing.Size(443, 422);
            this.Load += new System.EventHandler(this.PrinterConnectionMaterialTabPanel_Load);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ComboBox cbMaterialProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblManufacturer;
        private System.Windows.Forms.ComboBox cbMaterialManufacturer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button btnRefreshUSBDisks;
        private System.Windows.Forms.ComboBox cbUSBDriveLetters;
    }
}
