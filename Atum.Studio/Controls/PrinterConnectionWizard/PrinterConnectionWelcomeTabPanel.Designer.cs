namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionWelcomeTabPanel
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtIPAddress = new Atum.Studio.Controls.IPAddressControl();
            this.rdAddManualPrinter = new System.Windows.Forms.RadioButton();
            this.rdAddNetworkPrinterByDiscovery = new System.Windows.Forms.RadioButton();
            this.rdAddUSBPrinter = new System.Windows.Forms.RadioButton();
            this.label3 = new System.Windows.Forms.Label();
            this.cbPrinterType = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(64, 4);
            this.txtHeader.Size = new System.Drawing.Size(376, 43);
            this.txtHeader.Text = "Add Printer Wizard";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.printer_blue_icon;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.label4);
            this.plContent.Controls.Add(this.cbPrinterType);
            this.plContent.Controls.Add(this.label3);
            this.plContent.Controls.Add(this.rdAddUSBPrinter);
            this.plContent.Controls.Add(this.rdAddManualPrinter);
            this.plContent.Controls.Add(this.rdAddNetworkPrinterByDiscovery);
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.txtIPAddress);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Size = new System.Drawing.Size(443, 300);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(25, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(113, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select the printer type:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(111, 253);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "IP Address:";
            this.label2.Visible = false;
            // 
            // txtIPAddress
            // 
            this.txtIPAddress.AllowInternalTab = false;
            this.txtIPAddress.AutoHeight = true;
            this.txtIPAddress.BackColor = System.Drawing.SystemColors.Window;
            this.txtIPAddress.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.txtIPAddress.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtIPAddress.Enabled = false;
            this.txtIPAddress.Location = new System.Drawing.Point(218, 251);
            this.txtIPAddress.MinimumSize = new System.Drawing.Size(87, 20);
            this.txtIPAddress.Name = "txtIPAddress";
            this.txtIPAddress.ReadOnly = false;
            this.txtIPAddress.Size = new System.Drawing.Size(114, 20);
            this.txtIPAddress.TabIndex = 6;
            this.txtIPAddress.Text = "...";
            this.txtIPAddress.Visible = false;
            // 
            // rdAddManualPrinter
            // 
            this.rdAddManualPrinter.AutoSize = true;
            this.rdAddManualPrinter.Location = new System.Drawing.Point(91, 224);
            this.rdAddManualPrinter.Name = "rdAddManualPrinter";
            this.rdAddManualPrinter.Size = new System.Drawing.Size(161, 17);
            this.rdAddManualPrinter.TabIndex = 9;
            this.rdAddManualPrinter.Text = "Add network printer manually";
            this.rdAddManualPrinter.UseVisualStyleBackColor = true;
            this.rdAddManualPrinter.Visible = false;
            this.rdAddManualPrinter.CheckedChanged += new System.EventHandler(this.rdAddManualPrinter_CheckedChanged);
            // 
            // rdAddNetworkPrinterByDiscovery
            // 
            this.rdAddNetworkPrinterByDiscovery.AutoSize = true;
            this.rdAddNetworkPrinterByDiscovery.Location = new System.Drawing.Point(91, 197);
            this.rdAddNetworkPrinterByDiscovery.Name = "rdAddNetworkPrinterByDiscovery";
            this.rdAddNetworkPrinterByDiscovery.Size = new System.Drawing.Size(179, 17);
            this.rdAddNetworkPrinterByDiscovery.TabIndex = 8;
            this.rdAddNetworkPrinterByDiscovery.Text = "Add network printer by discovery";
            this.rdAddNetworkPrinterByDiscovery.UseVisualStyleBackColor = true;
            this.rdAddNetworkPrinterByDiscovery.Visible = false;
            this.rdAddNetworkPrinterByDiscovery.CheckedChanged += new System.EventHandler(this.rdAddManualPrinter_CheckedChanged);
            // 
            // rdAddUSBPrinter
            // 
            this.rdAddUSBPrinter.AutoSize = true;
            this.rdAddUSBPrinter.Checked = true;
            this.rdAddUSBPrinter.Location = new System.Drawing.Point(91, 140);
            this.rdAddUSBPrinter.Name = "rdAddUSBPrinter";
            this.rdAddUSBPrinter.Size = new System.Drawing.Size(193, 17);
            this.rdAddUSBPrinter.TabIndex = 10;
            this.rdAddUSBPrinter.TabStop = true;
            this.rdAddUSBPrinter.Text = "Add standalone printer (no network)";
            this.rdAddUSBPrinter.UseVisualStyleBackColor = true;
            this.rdAddUSBPrinter.Visible = false;
            this.rdAddUSBPrinter.CheckedChanged += new System.EventHandler(this.rdAddManualPrinter_CheckedChanged);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(25, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(385, 21);
            this.label3.TabIndex = 11;
            this.label3.Text = "Select one of the actions:";
            this.label3.Visible = false;
            // 
            // cbPrinterType
            // 
            this.cbPrinterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbPrinterType.FormattingEnabled = true;
            this.cbPrinterType.Items.AddRange(new object[] {
            "Atum V1.5",
            "Atum V2.0",
            "DLP Station 5"});
            this.cbPrinterType.Location = new System.Drawing.Point(146, 47);
            this.cbPrinterType.Name = "cbPrinterType";
            this.cbPrinterType.Size = new System.Drawing.Size(169, 21);
            this.cbPrinterType.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(111, 170);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "Printer type:";
            this.label4.Visible = false;
            // 
            // PrinterConnectionWelcomeTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonBackVisible = false;
            this.Name = "PrinterConnectionWelcomeTabPanel";
            this.Size = new System.Drawing.Size(443, 397);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private IPAddressControl txtIPAddress;
        private System.Windows.Forms.RadioButton rdAddManualPrinter;
        private System.Windows.Forms.RadioButton rdAddNetworkPrinterByDiscovery;
        private System.Windows.Forms.RadioButton rdAddUSBPrinter;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cbPrinterType;
    }
}
