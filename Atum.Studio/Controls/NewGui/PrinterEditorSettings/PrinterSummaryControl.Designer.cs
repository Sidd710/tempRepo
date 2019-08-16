namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    partial class PrinterSummaryControl
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
            this.plPrinterSummary = new System.Windows.Forms.Panel();
            this.lblPrinterName = new System.Windows.Forms.Label();
            this.plPrinterSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // plPrinterSummary
            // 
            this.plPrinterSummary.Controls.Add(this.lblPrinterName);
            this.plPrinterSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plPrinterSummary.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.plPrinterSummary.Location = new System.Drawing.Point(0, 0);
            this.plPrinterSummary.Name = "plPrinterSummary";
            this.plPrinterSummary.Size = new System.Drawing.Size(240, 40);
            this.plPrinterSummary.TabIndex = 6;
            this.plPrinterSummary.Click += new System.EventHandler(this.plPrinterSummary_Click);
            this.plPrinterSummary.MouseEnter += new System.EventHandler(this.plPrinterSummary_MouseEnter);
            this.plPrinterSummary.MouseLeave += new System.EventHandler(this.plPrinterSummary_MouseLeave);
            // 
            // lblPrinterName
            // 
            this.lblPrinterName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblPrinterName.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblPrinterName.ForeColor = System.Drawing.Color.Black;
            this.lblPrinterName.Location = new System.Drawing.Point(7, 10);
            this.lblPrinterName.Name = "lblPrinterName";
            this.lblPrinterName.Size = new System.Drawing.Size(226, 21);
            this.lblPrinterName.TabIndex = 6;
            this.lblPrinterName.Text = "DPL Station 4";
            this.lblPrinterName.Click += new System.EventHandler(this.lblPrinterName_Click);
            this.lblPrinterName.MouseEnter += new System.EventHandler(this.lblPrinterName_MouseEnter_1);
            this.lblPrinterName.MouseLeave += new System.EventHandler(this.lblPrinterName_MouseLeave_1);
            // 
            // PrinterSummaryControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.plPrinterSummary);
            this.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Name = "PrinterSummaryControl";
            this.Size = new System.Drawing.Size(240, 40);
            this.Click += new System.EventHandler(this.PrinterSummaryControl_Click);
            this.MouseEnter += new System.EventHandler(this.PrinterSummaryControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.PrinterSummaryControl_MouseLeave);
            this.plPrinterSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plPrinterSummary;
        private System.Windows.Forms.Label lblPrinterName;
    }
}
