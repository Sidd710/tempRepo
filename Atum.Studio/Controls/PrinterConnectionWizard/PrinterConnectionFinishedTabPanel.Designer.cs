namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionFinishedTabPanel
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
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnNext
            // 
            this.ButtonNextText = "Finished";
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(64, 4);
            this.txtHeader.Size = new System.Drawing.Size(376, 43);
            this.txtHeader.Text = "Finished";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.printer_blue_icon;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.label1);
            this.plContent.Size = new System.Drawing.Size(443, 302);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(137, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Click finish to create printer ";
            // 
            // PrinterConnectionFinishedTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ButtonFinished = true;
            this.Name = "PrinterConnectionFinishedTabPanel";
            this.Size = new System.Drawing.Size(443, 399);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
    }
}
