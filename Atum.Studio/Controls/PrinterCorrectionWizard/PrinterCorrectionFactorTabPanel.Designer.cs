namespace Atum.Studio.Controls
{
    partial class PrinterCorrectionFactorTabPanel
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
            this.label3 = new System.Windows.Forms.Label();
            this.txtCorrectionFactorX = new System.Windows.Forms.NumericUpDown();
            this.txtCorrectionFactorY = new System.Windows.Forms.NumericUpDown();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorrectionFactorX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorrectionFactorY)).BeginInit();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.btnNext.Text = "Finished";
            // 
            // txtHeader
            // 
            this.txtHeader.Text = "Correction";
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.resize;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.txtCorrectionFactorY);
            this.plContent.Controls.Add(this.txtCorrectionFactorX);
            this.plContent.Controls.Add(this.label3);
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.label1);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(139, 17);
            this.label1.TabIndex = 0;
            this.label1.Text = "Projecting: 10x10 cm";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(21, 90);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(54, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Real X:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(21, 117);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 2;
            this.label3.Text = "Real Y:";
            // 
            // txtCorrectionFactorX
            // 
            this.txtCorrectionFactorX.DecimalPlaces = 4;
            this.txtCorrectionFactorX.Location = new System.Drawing.Point(120, 88);
            this.txtCorrectionFactorX.Name = "txtCorrectionFactorX";
            this.txtCorrectionFactorX.Size = new System.Drawing.Size(67, 22);
            this.txtCorrectionFactorX.TabIndex = 3;
            // 
            // txtCorrectionFactorY
            // 
            this.txtCorrectionFactorY.DecimalPlaces = 4;
            this.txtCorrectionFactorY.Location = new System.Drawing.Point(120, 115);
            this.txtCorrectionFactorY.Name = "txtCorrectionFactorY";
            this.txtCorrectionFactorY.Size = new System.Drawing.Size(67, 22);
            this.txtCorrectionFactorY.TabIndex = 4;
            // 
            // PrinterCorrectionFactorTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonFinished = true;
            this.Name = "PrinterCorrectionFactorTabPanel";
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorrectionFactorX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtCorrectionFactorY)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        internal System.Windows.Forms.NumericUpDown txtCorrectionFactorY;
        internal System.Windows.Forms.NumericUpDown txtCorrectionFactorX;
    }
}
