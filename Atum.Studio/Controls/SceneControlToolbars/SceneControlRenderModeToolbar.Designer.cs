namespace Atum.Studio.Controls.OpenGL
{
    partial class SceneControlRenderModeToolbar
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
            this.btnModelRenderMode = new System.Windows.Forms.PictureBox();
            this.btnGroundPaneRenderMode = new System.Windows.Forms.PictureBox();
            this.btnSupportRenderMode = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelRenderMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGroundPaneRenderMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSupportRenderMode)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModelRenderMode
            // 
            this.btnModelRenderMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelRenderMode.Location = new System.Drawing.Point(15, 15);
            this.btnModelRenderMode.Name = "btnModelRenderMode";
            this.btnModelRenderMode.Size = new System.Drawing.Size(30, 30);
            this.btnModelRenderMode.TabIndex = 0;
            this.btnModelRenderMode.TabStop = false;
            this.btnModelRenderMode.Click += new System.EventHandler(this.btnModelRenderMode_Click);
            // 
            // btnGroundPaneRenderMode
            // 
            this.btnGroundPaneRenderMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGroundPaneRenderMode.Location = new System.Drawing.Point(15, 135);
            this.btnGroundPaneRenderMode.Name = "btnGroundPaneRenderMode";
            this.btnGroundPaneRenderMode.Size = new System.Drawing.Size(30, 30);
            this.btnGroundPaneRenderMode.TabIndex = 1;
            this.btnGroundPaneRenderMode.TabStop = false;
            this.btnGroundPaneRenderMode.Click += new System.EventHandler(this.btnGroundPaneRenderMode_Click);
            // 
            // btnSupportRenderMode
            // 
            this.btnSupportRenderMode.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSupportRenderMode.Location = new System.Drawing.Point(15, 75);
            this.btnSupportRenderMode.Name = "btnSupportRenderMode";
            this.btnSupportRenderMode.Size = new System.Drawing.Size(30, 30);
            this.btnSupportRenderMode.TabIndex = 2;
            this.btnSupportRenderMode.TabStop = false;
            this.btnSupportRenderMode.Click += new System.EventHandler(this.btnSupportRenderMode_Click);
            // 
            // SceneControlRenderModeToolbar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.btnSupportRenderMode);
            this.Controls.Add(this.btnGroundPaneRenderMode);
            this.Controls.Add(this.btnModelRenderMode);
            this.Name = "SceneControlRenderModeToolbar";
            this.Size = new System.Drawing.Size(60, 180);
            this.Load += new System.EventHandler(this.SceneControlRenderModeToolbar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnModelRenderMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnGroundPaneRenderMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnSupportRenderMode)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnModelRenderMode;
        private System.Windows.Forms.PictureBox btnGroundPaneRenderMode;
        private System.Windows.Forms.PictureBox btnSupportRenderMode;
    }
}
