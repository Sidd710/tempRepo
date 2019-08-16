namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionAdjustTabPanel
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
            this.atumPrinterCalibration1 = new Atum.Studio.Controls.PrinterEditor.AtumPrinterCalibration();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(64, 4);
            this.txtHeader.Size = new System.Drawing.Size(376, 43);
            this.txtHeader.Text = "Calibration values";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.atumPrinterCalibration1);
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.label1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(288, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please print the calibration job and adjust calibration values:";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 420);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(335, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Click back to re-generate the calibration printjob using the new values";
            // 
            // atumPrinterCalibration1
            // 
            this.atumPrinterCalibration1.AdvancedMode = false;
            this.atumPrinterCalibration1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.atumPrinterCalibration1.DataSource = null;
            this.atumPrinterCalibration1.Location = new System.Drawing.Point(10, 59);
            this.atumPrinterCalibration1.Name = "atumPrinterCalibration1";
            this.atumPrinterCalibration1.Size = new System.Drawing.Size(421, 315);
            this.atumPrinterCalibration1.TabIndex = 2;
            // 
            // PrinterConnectionAdjustTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PrinterConnectionAdjustTabPanel";
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private PrinterEditor.AtumPrinterCalibration atumPrinterCalibration1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
    }
}
