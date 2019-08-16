namespace Atum.Studio.Controls.NewGui.ContextMenu
{
    partial class ContextMenuItem
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
            this.lblMenuName = new System.Windows.Forms.Label();
            this.plMenuItem = new System.Windows.Forms.Panel();
            this.plMenuItem.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMenuName
            // 
            this.lblMenuName.BackColor = System.Drawing.Color.Transparent;
            this.lblMenuName.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblMenuName.ForeColor = System.Drawing.Color.White;
            this.lblMenuName.Location = new System.Drawing.Point(0, 0);
            this.lblMenuName.Name = "lblMenuName";
            this.lblMenuName.Size = new System.Drawing.Size(178, 48);
            this.lblMenuName.TabIndex = 1;
            this.lblMenuName.Text = "New Project";
            this.lblMenuName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblMenuName.Click += new System.EventHandler(this.lblMenuName_Click);
            this.lblMenuName.MouseEnter += new System.EventHandler(this.plMenuItem_MouseEnter);
            this.lblMenuName.MouseLeave += new System.EventHandler(this.plMenuItem_MouseLeave);
            // 
            // plMenuItem
            // 
            this.plMenuItem.BackColor = System.Drawing.Color.Transparent;
            this.plMenuItem.Controls.Add(this.lblMenuName);
            this.plMenuItem.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plMenuItem.ForeColor = System.Drawing.Color.White;
            this.plMenuItem.Location = new System.Drawing.Point(0, 0);
            this.plMenuItem.Name = "plMenuItem";
            this.plMenuItem.Size = new System.Drawing.Size(178, 48);
            this.plMenuItem.TabIndex = 2;
            this.plMenuItem.Click += new System.EventHandler(this.lblMenuName_Click);
            this.plMenuItem.MouseEnter += new System.EventHandler(this.plMenuItem_MouseEnter);
            this.plMenuItem.MouseLeave += new System.EventHandler(this.plMenuItem_MouseLeave);
            // 
            // ContextMenuItem
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.plMenuItem);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "ContextMenuItem";
            this.Size = new System.Drawing.Size(178, 48);
            this.plMenuItem.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblMenuName;
        private System.Windows.Forms.Panel plMenuItem;
    }
}
