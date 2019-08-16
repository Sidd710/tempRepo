namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterPropertyControl
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
            this.gpHeader = new System.Windows.Forms.GroupBox();
            this.SuspendLayout();
            // 
            // gpHeader
            // 
            this.gpHeader.Location = new System.Drawing.Point(33, 23);
            this.gpHeader.Name = "gpHeader";
            this.gpHeader.Size = new System.Drawing.Size(265, 124);
            this.gpHeader.TabIndex = 0;
            this.gpHeader.TabStop = false;
            this.gpHeader.Text = "groupBox1";
            this.gpHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.gpHeader_Paint);
            // 
            // PrinterPropertyControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.gpHeader);
            this.Name = "PrinterPropertyControl";
            this.Size = new System.Drawing.Size(507, 150);
            this.Load += new System.EventHandler(this.PrinterPropertyControl_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gpHeader;
    }
}
