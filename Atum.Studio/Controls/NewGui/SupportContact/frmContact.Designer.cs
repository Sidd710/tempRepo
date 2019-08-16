namespace Atum.Studio.Controls.NewGui.SupportContact
{
    partial class frmContact
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
            this.contactControl1 = new Atum.Studio.Controls.NewGui.SupportContact.ContactControl();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.contactControl1);
            this.plContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.plContent.Margin = new System.Windows.Forms.Padding(4);
            this.plContent.Size = new System.Drawing.Size(640, 304);
            // 
            // contactControl1
            // 
            this.contactControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.contactControl1.Location = new System.Drawing.Point(0, 0);
            this.contactControl1.Name = "contactControl1";
            this.contactControl1.Size = new System.Drawing.Size(640, 304);
            this.contactControl1.TabIndex = 0;
            // 
            // frmContact
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(640, 361);
            this.Name = "frmContact";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Operator Station";
            this.Load += new System.EventHandler(this.frmContact_Load);
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ContactControl contactControl1;
    }
}