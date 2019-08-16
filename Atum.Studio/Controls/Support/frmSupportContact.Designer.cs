namespace Atum.Studio
{
    partial class frmSupportContact
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSupportContact));
            this.supportContactControl1 = new Atum.Studio.Controls.Support.SupportContactControl();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.supportContactControl1);
            // 
            // supportContactControl1
            // 
            this.supportContactControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.supportContactControl1.Location = new System.Drawing.Point(0, 0);
            this.supportContactControl1.Margin = new System.Windows.Forms.Padding(0);
            this.supportContactControl1.Name = "supportContactControl1";
            this.supportContactControl1.Size = new System.Drawing.Size(394, 377);
            this.supportContactControl1.TabIndex = 0;
            // 
            // frmSupportContact
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.btnCancelHidden = true;
            this.ClientSize = new System.Drawing.Size(394, 422);
            this.ControlBox = false;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSupportContact";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "";
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Controls.Support.SupportContactControl supportContactControl1;
    }
}