namespace Atum.Studio.Controls.NewGui
{
    partial class NewGUIPanel
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
            this.plnHeder = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // plnHeder
            // 
            this.plnHeder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.plnHeder.Dock = System.Windows.Forms.DockStyle.Top;
            this.plnHeder.Location = new System.Drawing.Point(0, 0);
            this.plnHeder.Margin = new System.Windows.Forms.Padding(4);
            this.plnHeder.Name = "plnHeder";
            this.plnHeder.Size = new System.Drawing.Size(568, 58);
            this.plnHeder.TabIndex = 0;
            // 
            // NewGUIPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.plnHeder);
            this.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NewGUIPanel";
            this.Size = new System.Drawing.Size(568, 185);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plnHeder;
    }
}
