namespace Atum.Studio.Controls
{
    partial class PrintPropertiesControl
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
            this.components = new System.ComponentModel.Container();
            this.plHeader = new System.Windows.Forms.Panel();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.plContent = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbPrinter = new System.Windows.Forms.TabPage();
            this.cbSelectedPrinters = new System.Windows.Forms.ComboBox();
            this.atumV2PrinterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.btnPrinterManager = new System.Windows.Forms.Button();
            this.tbOther = new System.Windows.Forms.TabPage();
            this.txtVolume = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.plHeader.SuspendLayout();
            this.plContent.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbPrinter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.atumV2PrinterBindingSource)).BeginInit();
            this.tbOther.SuspendLayout();
            this.SuspendLayout();
            // 
            // plHeader
            // 
            this.plHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plHeader.BackColor = System.Drawing.Color.White;
            this.plHeader.Controls.Add(this.lblHeaderText);
            this.plHeader.Location = new System.Drawing.Point(0, 0);
            this.plHeader.Name = "plHeader";
            this.plHeader.Size = new System.Drawing.Size(324, 29);
            this.plHeader.TabIndex = 0;
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderText.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblHeaderText.Location = new System.Drawing.Point(4, 5);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(59, 20);
            this.lblHeaderText.TabIndex = 0;
            this.lblHeaderText.Text = "label1";
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plContent.Controls.Add(this.tabControl1);
            this.plContent.Location = new System.Drawing.Point(0, 35);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(324, 365);
            this.plContent.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbPrinter);
            this.tabControl1.Controls.Add(this.tbOther);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(324, 365);
            this.tabControl1.TabIndex = 0;
            // 
            // tbPrinter
            // 
            this.tbPrinter.Controls.Add(this.cbSelectedPrinters);
            this.tbPrinter.Controls.Add(this.btnPrinterManager);
            this.tbPrinter.Location = new System.Drawing.Point(4, 25);
            this.tbPrinter.Name = "tbPrinter";
            this.tbPrinter.Padding = new System.Windows.Forms.Padding(3);
            this.tbPrinter.Size = new System.Drawing.Size(316, 336);
            this.tbPrinter.TabIndex = 3;
            this.tbPrinter.Text = "Printer";
            this.tbPrinter.UseVisualStyleBackColor = true;
            // 
            // cbSelectedPrinters
            // 
            this.cbSelectedPrinters.DataSource = this.atumV2PrinterBindingSource;
            this.cbSelectedPrinters.DisplayMember = "Name";
            this.cbSelectedPrinters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSelectedPrinters.FormattingEnabled = true;
            this.cbSelectedPrinters.Location = new System.Drawing.Point(17, 14);
            this.cbSelectedPrinters.Name = "cbSelectedPrinters";
            this.cbSelectedPrinters.Size = new System.Drawing.Size(121, 24);
            this.cbSelectedPrinters.TabIndex = 6;
            // 
            // atumV2PrinterBindingSource
            // 
            this.atumV2PrinterBindingSource.DataSource = typeof(Atum.DAL.Hardware.AtumV15Printer);
            // 
            // btnPrinterManager
            // 
            this.btnPrinterManager.Location = new System.Drawing.Point(153, 10);
            this.btnPrinterManager.Name = "btnPrinterManager";
            this.btnPrinterManager.Size = new System.Drawing.Size(120, 28);
            this.btnPrinterManager.TabIndex = 5;
            this.btnPrinterManager.Text = "Printer Manager";
            this.btnPrinterManager.UseVisualStyleBackColor = true;
            this.btnPrinterManager.Click += new System.EventHandler(this.btnWizard_Click);
            // 
            // tbOther
            // 
            this.tbOther.Controls.Add(this.txtVolume);
            this.tbOther.Controls.Add(this.label4);
            this.tbOther.Location = new System.Drawing.Point(4, 25);
            this.tbOther.Name = "tbOther";
            this.tbOther.Padding = new System.Windows.Forms.Padding(3);
            this.tbOther.Size = new System.Drawing.Size(316, 336);
            this.tbOther.TabIndex = 1;
            this.tbOther.Text = "Other";
            this.tbOther.UseVisualStyleBackColor = true;
            // 
            // txtVolume
            // 
            this.txtVolume.Enabled = false;
            this.txtVolume.Location = new System.Drawing.Point(73, 7);
            this.txtVolume.Name = "txtVolume";
            this.txtVolume.Size = new System.Drawing.Size(100, 22);
            this.txtVolume.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 7);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 17);
            this.label4.TabIndex = 0;
            this.label4.Text = "Volume:";
            // 
            // PrintPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.plHeader);
            this.Name = "PrintPropertiesControl";
            this.Size = new System.Drawing.Size(324, 400);
            this.Load += new System.EventHandler(this.PrintSettingControl_Load);
            this.plHeader.ResumeLayout(false);
            this.plHeader.PerformLayout();
            this.plContent.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbPrinter.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.atumV2PrinterBindingSource)).EndInit();
            this.tbOther.ResumeLayout(false);
            this.tbOther.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plHeader;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbOther;
        private System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtVolume;
        private System.Windows.Forms.TabPage tbPrinter;
        private System.Windows.Forms.Button btnPrinterManager;
        private System.Windows.Forms.ComboBox cbSelectedPrinters;
        private System.Windows.Forms.BindingSource atumV2PrinterBindingSource;
    }
}
