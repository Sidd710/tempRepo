namespace Atum.Studio.Controls.Docking
{
    partial class DockPanelBase
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DockPanelBase));
            this.plTitle = new System.Windows.Forms.Panel();
            this.picAutoHide = new System.Windows.Forms.PictureBox();
            this.picClose = new System.Windows.Forms.PictureBox();
            this.lblTitle = new System.Windows.Forms.Label();
            this.plContent = new System.Windows.Forms.Panel();
            this.plGripSize = new System.Windows.Forms.Panel();
            this.plTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoHide)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).BeginInit();
            this.SuspendLayout();
            // 
            // plTitle
            // 
            this.plTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(132)))), ((int)(((byte)(194)))));
            this.plTitle.Controls.Add(this.picAutoHide);
            this.plTitle.Controls.Add(this.picClose);
            this.plTitle.Controls.Add(this.lblTitle);
            this.plTitle.Dock = System.Windows.Forms.DockStyle.Top;
            this.plTitle.Location = new System.Drawing.Point(0, 0);
            this.plTitle.Margin = new System.Windows.Forms.Padding(0);
            this.plTitle.Name = "plTitle";
            this.plTitle.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.plTitle.Size = new System.Drawing.Size(322, 19);
            this.plTitle.TabIndex = 1;
            this.plTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plTitle_MouseDown);
            this.plTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plTitle_MouseMove);
            this.plTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plTitle_MouseUp);
            // 
            // picAutoHide
            // 
            this.picAutoHide.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.picAutoHide.Image = global::Atum.Studio.Properties.Resources.DockPane_AutoHide;
            this.picAutoHide.Location = new System.Drawing.Point(287, 1);
            this.picAutoHide.Name = "picAutoHide";
            this.picAutoHide.Size = new System.Drawing.Size(16, 16);
            this.picAutoHide.TabIndex = 4;
            this.picAutoHide.TabStop = false;
            this.picAutoHide.Click += new System.EventHandler(this.picAutoHide_Click);
            // 
            // picClose
            // 
            this.picClose.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.picClose.Image = ((System.Drawing.Image)(resources.GetObject("picClose.Image")));
            this.picClose.Location = new System.Drawing.Point(303, 1);
            this.picClose.Name = "picClose";
            this.picClose.Size = new System.Drawing.Size(16, 16);
            this.picClose.TabIndex = 3;
            this.picClose.TabStop = false;
            this.picClose.Click += new System.EventHandler(this.picClose_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.ForeColor = System.Drawing.Color.White;
            this.lblTitle.Location = new System.Drawing.Point(5, 4);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(35, 13);
            this.lblTitle.TabIndex = 2;
            this.lblTitle.Text = "label1";
            this.lblTitle.MouseDown += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseDown);
            this.lblTitle.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseMove);
            this.lblTitle.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblTitle_MouseUp);
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plContent.Location = new System.Drawing.Point(0, 19);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(322, 294);
            this.plContent.TabIndex = 2;
            this.plContent.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plContent_MouseMove);
            // 
            // plGripSize
            // 
            this.plGripSize.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.plGripSize.Cursor = System.Windows.Forms.Cursors.VSplit;
            this.plGripSize.Dock = System.Windows.Forms.DockStyle.Left;
            this.plGripSize.Location = new System.Drawing.Point(0, 19);
            this.plGripSize.Name = "plGripSize";
            this.plGripSize.Size = new System.Drawing.Size(4, 294);
            this.plGripSize.TabIndex = 0;
            this.plGripSize.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plGripSize_MouseDown);
            this.plGripSize.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plGripSize_MouseMove);
            this.plGripSize.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plGripSize_MouseUp);
            // 
            // DockPanelBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(322, 313);
            this.ControlBox = false;
            this.Controls.Add(this.plGripSize);
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.plTitle);
            this.Name = "DockPanelBase";
            this.Opacity = 0.9D;
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Load += new System.EventHandler(this.DockPanelBase_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.DockPanelBase_KeyDown);
            this.plTitle.ResumeLayout(false);
            this.plTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picAutoHide)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picClose)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plTitle;
        private System.Windows.Forms.Label lblTitle;
        public System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.PictureBox picClose;
        private System.Windows.Forms.PictureBox picAutoHide;
        private System.Windows.Forms.Panel plGripSize;
    }
}
