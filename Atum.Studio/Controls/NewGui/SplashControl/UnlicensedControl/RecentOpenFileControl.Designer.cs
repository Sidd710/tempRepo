namespace Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl
{
    partial class RecentOpenFileControl
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
            this.components = new System.ComponentModel.Container();
            this.plRecentOpenFile = new System.Windows.Forms.Panel();
            this.lblFileName = new System.Windows.Forms.Label();
            this.ttFileName = new System.Windows.Forms.ToolTip(this.components);
            this.plRecentOpenFile.SuspendLayout();
            this.SuspendLayout();
            // 
            // plRecentOpenFile
            // 
            this.plRecentOpenFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.plRecentOpenFile.Controls.Add(this.lblFileName);
            this.plRecentOpenFile.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plRecentOpenFile.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plRecentOpenFile.Location = new System.Drawing.Point(0, 0);
            this.plRecentOpenFile.Name = "plRecentOpenFile";
            this.plRecentOpenFile.Size = new System.Drawing.Size(280, 40);
            this.plRecentOpenFile.TabIndex = 0;
            this.plRecentOpenFile.Click += new System.EventHandler(this.plRecentOpenFile_Click);
            this.plRecentOpenFile.MouseEnter += new System.EventHandler(this.plRecentOpenFile_MouseEnter);
            this.plRecentOpenFile.MouseLeave += new System.EventHandler(this.plRecentOpenFile_MouseLeave);
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblFileName.ForeColor = System.Drawing.Color.White;
            this.lblFileName.Location = new System.Drawing.Point(16, 10);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(72, 17);
            this.lblFileName.TabIndex = 1;
            this.lblFileName.Text = "untitled.stl";
            this.ttFileName.SetToolTip(this.lblFileName, "untitled.stl");
            this.lblFileName.Click += new System.EventHandler(this.lblFileName_Click_1);
            this.lblFileName.MouseEnter += new System.EventHandler(this.lblFileName_MouseEnter);
            this.lblFileName.MouseLeave += new System.EventHandler(this.lblFileName_MouseLeave);
            // 
            // ttFileName
            // 
            this.ttFileName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ttFileName.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            // 
            // RecentOpenFileControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.Controls.Add(this.plRecentOpenFile);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "RecentOpenFileControl";
            this.Size = new System.Drawing.Size(280, 40);
            this.Load += new System.EventHandler(this.RecentOpenFileControl_Load);
            this.MouseEnter += new System.EventHandler(this.RecentOpenFileControl_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.RecentOpenFileControl_MouseLeave);
            this.plRecentOpenFile.ResumeLayout(false);
            this.plRecentOpenFile.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plRecentOpenFile;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.ToolTip ttFileName;
    }
}
