namespace Atum.Studio.Controls.MagsAI
{
    partial class MagsAIMaterialTabPanel
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
            this.cbDefaultPrinter = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbMaterialProduct = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblManufacturer = new System.Windows.Forms.Label();
            this.cbMaterialManufacturer = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkSupportBasement = new System.Windows.Forms.CheckBox();
            this.chkLiftModelOnSupport = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // cbDefaultPrinter
            // 
            this.cbDefaultPrinter.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cbDefaultPrinter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbDefaultPrinter.FormattingEnabled = true;
            this.cbDefaultPrinter.Location = new System.Drawing.Point(259, 81);
            this.cbDefaultPrinter.Name = "cbDefaultPrinter";
            this.cbDefaultPrinter.Size = new System.Drawing.Size(225, 21);
            this.cbDefaultPrinter.TabIndex = 39;
            this.cbDefaultPrinter.SelectedIndexChanged += new System.EventHandler(this.cbDefaultPrinter_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(138, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 13);
            this.label4.TabIndex = 38;
            this.label4.Text = "Printer:";
            // 
            // cbMaterialProduct
            // 
            this.cbMaterialProduct.DisplayMember = "DisplayName";
            this.cbMaterialProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterialProduct.FormattingEnabled = true;
            this.cbMaterialProduct.Location = new System.Drawing.Point(259, 187);
            this.cbMaterialProduct.Name = "cbMaterialProduct";
            this.cbMaterialProduct.Size = new System.Drawing.Size(206, 21);
            this.cbMaterialProduct.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(180, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Product:";
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.AutoSize = true;
            this.lblManufacturer.Location = new System.Drawing.Point(180, 151);
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(73, 13);
            this.lblManufacturer.TabIndex = 12;
            this.lblManufacturer.Text = "Manufacturer:";
            // 
            // cbMaterialManufacturer
            // 
            this.cbMaterialManufacturer.DisplayMember = "supplier";
            this.cbMaterialManufacturer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterialManufacturer.FormattingEnabled = true;
            this.cbMaterialManufacturer.Location = new System.Drawing.Point(259, 148);
            this.cbMaterialManufacturer.Name = "cbMaterialManufacturer";
            this.cbMaterialManufacturer.Size = new System.Drawing.Size(206, 21);
            this.cbMaterialManufacturer.TabIndex = 11;
            //this.cbMaterialManufacturer.SelectedIndexChanged += new System.EventHandler(this.cbMaterialManufacturer_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(138, 106);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "Select material:";
            // 
            // chkSupportBasement
            // 
            this.chkSupportBasement.AutoSize = true;
            this.chkSupportBasement.Checked = true;
            this.chkSupportBasement.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSupportBasement.Location = new System.Drawing.Point(259, 247);
            this.chkSupportBasement.Name = "chkSupportBasement";
            this.chkSupportBasement.Size = new System.Drawing.Size(135, 17);
            this.chkSupportBasement.TabIndex = 40;
            this.chkSupportBasement.Text = "Use Support Basement";
            this.chkSupportBasement.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.chkLiftModelOnSupport.AutoSize = true;
            this.chkLiftModelOnSupport.Checked = true;
            this.chkLiftModelOnSupport.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLiftModelOnSupport.Location = new System.Drawing.Point(259, 224);
            this.chkLiftModelOnSupport.Name = "checkBox1";
            this.chkLiftModelOnSupport.Size = new System.Drawing.Size(127, 17);
            this.chkLiftModelOnSupport.TabIndex = 41;
            this.chkLiftModelOnSupport.Text = "Lift Model on Support";
            this.chkLiftModelOnSupport.UseVisualStyleBackColor = true;
            // 
            // MagsAIMaterialTabPanel
            // 
            this.Controls.Add(this.chkLiftModelOnSupport);
            this.Controls.Add(this.chkSupportBasement);
            this.Controls.Add(this.cbDefaultPrinter);
            this.Controls.Add(this.cbMaterialProduct);
            this.Controls.Add(this.lblManufacturer);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cbMaterialManufacturer);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label1);
            this.Name = "MagsAIMaterialTabPanel";
            this.Size = new System.Drawing.Size(668, 546);
            this.Load += new System.EventHandler(this.MagsAIMaterialTabPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ComboBox cbMaterialProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblManufacturer;
        private System.Windows.Forms.ComboBox cbMaterialManufacturer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbDefaultPrinter;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.CheckBox chkSupportBasement;
        private System.Windows.Forms.CheckBox chkLiftModelOnSupport;
    }
}
