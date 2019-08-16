namespace Atum.Studio.Controls.NewGui.ContextMenu
{
    partial class SupportConeContextMenuStripControl
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
            this.plMenuList = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // plMenuList
            // 
            this.plMenuList.BackColor = System.Drawing.Color.Transparent;
            this.plMenuList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plMenuList.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plMenuList.Location = new System.Drawing.Point(0, 0);
            this.plMenuList.Margin = new System.Windows.Forms.Padding(5);
            this.plMenuList.Name = "plMenuList";
            this.plMenuList.Size = new System.Drawing.Size(178, 96);
            this.plMenuList.TabIndex = 1;
            // 
            // ContextMenuStripControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.Controls.Add(this.plMenuList);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Location = new System.Drawing.Point(16, 78);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "ContextMenuStripControl";
            this.Size = new System.Drawing.Size(178, 96);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plMenuList;
    }
}
