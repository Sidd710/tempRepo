namespace Atum.Studio.Controls.NewGui.SplashControl
{
    partial class SplashFrm
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
            this.spcSplashContainer = new System.Windows.Forms.SplitContainer();
            ((System.ComponentModel.ISupportInitialize)(this.spcSplashContainer)).BeginInit();
            this.spcSplashContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // spcSplashContainer
            // 
            this.spcSplashContainer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.spcSplashContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcSplashContainer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.spcSplashContainer.ForeColor = System.Drawing.Color.White;
            this.spcSplashContainer.IsSplitterFixed = true;
            this.spcSplashContainer.Location = new System.Drawing.Point(0, 0);
            this.spcSplashContainer.Margin = new System.Windows.Forms.Padding(0);
            this.spcSplashContainer.Name = "spcSplashContainer";
            // 
            // spcSplashContainer.Panel1
            // 
            this.spcSplashContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.spcSplashContainer.Panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            // 
            // spcSplashContainer.Panel2
            // 
            this.spcSplashContainer.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.spcSplashContainer.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.spcSplashContainer.Size = new System.Drawing.Size(1056, 576);
            this.spcSplashContainer.SplitterDistance = 768;
            this.spcSplashContainer.SplitterWidth = 1;
            this.spcSplashContainer.TabIndex = 0;
            // 
            // SplashFrm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(1056, 576);
            this.ControlBox = false;
            this.Controls.Add(this.spcSplashContainer);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SplashFrm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "SplashFrm";
            ((System.ComponentModel.ISupportInitialize)(this.spcSplashContainer)).EndInit();
            this.spcSplashContainer.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer spcSplashContainer;
    }
}