namespace Atum.Studio.Controls.Support
{
    partial class SupportContactControl
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
            this.txtHeader = new System.Windows.Forms.Label();
            this.picHeader = new System.Windows.Forms.PictureBox();
            this.plHeader = new System.Windows.Forms.Panel();
            this.plContent = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plHeader.SuspendLayout();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHeader.Location = new System.Drawing.Point(3, 4);
            this.txtHeader.Name = "txtHeader";
            this.txtHeader.Size = new System.Drawing.Size(411, 43);
            this.txtHeader.TabIndex = 2;
            this.txtHeader.Text = "Contact";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // picHeader
            // 
            this.picHeader.Location = new System.Drawing.Point(10, 1);
            this.picHeader.Name = "picHeader";
            this.picHeader.Size = new System.Drawing.Size(48, 48);
            this.picHeader.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHeader.TabIndex = 0;
            this.picHeader.TabStop = false;
            // 
            // plHeader
            // 
            this.plHeader.BackColor = System.Drawing.Color.White;
            this.plHeader.Controls.Add(this.picHeader);
            this.plHeader.Controls.Add(this.txtHeader);
            this.plHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.plHeader.Location = new System.Drawing.Point(0, 0);
            this.plHeader.Name = "plHeader";
            this.plHeader.Size = new System.Drawing.Size(417, 51);
            this.plHeader.TabIndex = 4;
            this.plHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.plHeader_Paint);
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.White;
            this.plContent.Controls.Add(this.label1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Location = new System.Drawing.Point(0, 51);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(417, 404);
            this.plContent.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(21, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(360, 81);
            this.label1.TabIndex = 0;
            this.label1.Text = "Please contact your reseller with any questions about the software or the printer" +
    "";
            // 
            // SupportContactControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.plHeader);
            this.Name = "SupportContactControl";
            this.Size = new System.Drawing.Size(417, 455);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plHeader.ResumeLayout(false);
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label txtHeader;
        protected System.Windows.Forms.PictureBox picHeader;
        public System.Windows.Forms.Panel plHeader;
        public System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.Label label1;

    }
}
