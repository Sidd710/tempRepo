namespace Atum.Studio.Controls.NewGui
{
    partial class frmUSBDriveSelector
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
            this.btnSelect = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnRefresh = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plContentItems = new System.Windows.Forms.Panel();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plContent.Controls.Add(this.plContentItems);
            this.plContent.Controls.Add(this.btnRefresh);
            this.plContent.Controls.Add(this.btnSelect);
            this.plContent.Size = new System.Drawing.Size(500, 343);
            // 
            // btnSelect
            // 
            this.btnSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnSelect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelect.FlatAppearance.BorderSize = 0;
            this.btnSelect.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnSelect.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelect.ForeColor = System.Drawing.Color.White;
            this.btnSelect.Location = new System.Drawing.Point(371, 285);
            this.btnSelect.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.btnSelect.Name = "btnSelect";
            this.btnSelect.Radius = 20;
            this.btnSelect.SingleBorder = false;
            this.btnSelect.Size = new System.Drawing.Size(112, 40);
            this.btnSelect.TabIndex = 0;
            this.btnSelect.Text = "Export";
            this.btnSelect.UseVisualStyleBackColor = false;
            this.btnSelect.Click += new System.EventHandler(this.btnSelect_Click);
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnRefresh.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnRefresh.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRefresh.FlatAppearance.BorderSize = 0;
            this.btnRefresh.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRefresh.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnRefresh.Location = new System.Drawing.Point(15, 285);
            this.btnRefresh.Margin = new System.Windows.Forms.Padding(16);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Radius = 18;
            this.btnRefresh.SingleBorder = false;
            this.btnRefresh.Size = new System.Drawing.Size(112, 40);
            this.btnRefresh.TabIndex = 1;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.btnRefresh_Click);
            // 
            // plContentItems
            // 
            this.plContentItems.Location = new System.Drawing.Point(0, 0);
            this.plContentItems.Name = "plContentItems";
            this.plContentItems.Size = new System.Drawing.Size(499, 267);
            this.plContentItems.TabIndex = 2;
            // 
            // frmUSBDriveSelector
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(500, 400);
            this.Name = "frmUSBDriveSelector";
            this.Text = "USB Drives";
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private RoundedButton btnSelect;
        private RoundedButton btnRefresh;
        private System.Windows.Forms.Panel plContentItems;
    }
}
