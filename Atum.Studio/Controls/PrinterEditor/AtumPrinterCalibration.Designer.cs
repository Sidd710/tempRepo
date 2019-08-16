namespace Atum.Studio.Controls.PrinterEditor
{
    partial class AtumPrinterCalibration
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
            this.lblAdvancedMode = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.trapezoid = new Atum.Studio.Controls.Correction.Basic();
            this.SuspendLayout();
            // 
            // lblAdvancedMode
            // 
            this.lblAdvancedMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblAdvancedMode.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAdvancedMode.Location = new System.Drawing.Point(309, 5);
            this.lblAdvancedMode.Name = "lblAdvancedMode";
            this.lblAdvancedMode.Size = new System.Drawing.Size(112, 27);
            this.lblAdvancedMode.TabIndex = 18;
            this.lblAdvancedMode.Text = "Advanced";
            this.lblAdvancedMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblAdvancedMode.Click += new System.EventHandler(this.lblAdvancedMode_Click);
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(11, 287);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Unit: mm";
            // 
            // trapezoid
            // 
            this.trapezoid.AdvancedMode = false;
            this.trapezoid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trapezoid.BackColor = System.Drawing.Color.White;
            this.trapezoid.IsDirty = false;
            this.trapezoid.Location = new System.Drawing.Point(14, 34);
            this.trapezoid.Margin = new System.Windows.Forms.Padding(2);
            this.trapezoid.Name = "trapezoid";
            this.trapezoid.SelectedPrinter = null;
            this.trapezoid.Size = new System.Drawing.Size(402, 236);
            this.trapezoid.TabIndex = 17;
            // 
            // AtumPrinterCalibration
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblAdvancedMode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.trapezoid);
            this.Name = "AtumPrinterCalibration";
            this.Size = new System.Drawing.Size(434, 315);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblAdvancedMode;
        private Correction.Basic trapezoid;
        private System.Windows.Forms.Label label2;
    }
}
