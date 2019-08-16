namespace Atum.Studio.Controls
{
    partial class BasePopup
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.plFooter = new System.Windows.Forms.Panel();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.plContent = new System.Windows.Forms.Panel();
            this.plFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // plFooter
            // 
            this.plFooter.Controls.Add(this.btnCancel);
            this.plFooter.Controls.Add(this.btnOK);
            this.plFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plFooter.Location = new System.Drawing.Point(0, 306);
            this.plFooter.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(296, 37);
            this.plFooter.TabIndex = 1;
            this.plFooter.Paint += new System.Windows.Forms.PaintEventHandler(this.plFooter_Paint);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(230, 8);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(56, 19);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.Location = new System.Drawing.Point(170, 8);
            this.btnOK.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(56, 19);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.White;
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Location = new System.Drawing.Point(0, 0);
            this.plContent.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(296, 306);
            this.plContent.TabIndex = 2;
            // 
            // BasePopup
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(296, 343);
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.plFooter);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "BasePopup";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.Text = "BasePopup";
            this.plFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        public System.Windows.Forms.Panel plContent;
        internal System.Windows.Forms.Panel plFooter;
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Button btnOK;
    }
}