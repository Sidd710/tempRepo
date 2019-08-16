namespace Atum.Studio.Controls.NewGui
{
    partial class SceneControlProgressbar
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
            this.plMaxValue = new System.Windows.Forms.Panel();
            this.plProgressValue = new System.Windows.Forms.Panel();
            this.plMaxValue.SuspendLayout();
            this.SuspendLayout();
            // 
            // plMaxValue
            // 
            this.plMaxValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.plMaxValue.Controls.Add(this.plProgressValue);
            this.plMaxValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plMaxValue.Location = new System.Drawing.Point(0, 0);
            this.plMaxValue.Margin = new System.Windows.Forms.Padding(0);
            this.plMaxValue.Name = "plMaxValue";
            this.plMaxValue.Size = new System.Drawing.Size(600, 6);
            this.plMaxValue.TabIndex = 0;
            // 
            // plProgressValue
            // 
            this.plProgressValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(18)))), ((int)(((byte)(0)))));
            this.plProgressValue.Dock = System.Windows.Forms.DockStyle.Left;
            this.plProgressValue.Location = new System.Drawing.Point(0, 0);
            this.plProgressValue.Margin = new System.Windows.Forms.Padding(16);
            this.plProgressValue.Name = "plProgressValue";
            this.plProgressValue.Size = new System.Drawing.Size(1, 6);
            this.plProgressValue.TabIndex = 1;
            // 
            // SceneControlProgressbar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.plMaxValue);
            this.Name = "SceneControlProgressbar";
            this.Size = new System.Drawing.Size(600, 6);
            this.plMaxValue.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plMaxValue;
        private System.Windows.Forms.Panel plProgressValue;
    }
}