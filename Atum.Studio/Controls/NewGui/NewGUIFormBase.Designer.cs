namespace Atum.Studio.Controls.NewGui
{
    partial class NewGUIFormBase
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
            this.plHeaderTitle = new System.Windows.Forms.Panel();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.plContent = new System.Windows.Forms.Panel();
            this.plContentSplitter = new System.Windows.Forms.SplitContainer();
            this.plHeaderTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plContentSplitter)).BeginInit();
            this.plContentSplitter.Panel1.SuspendLayout();
            this.plContentSplitter.Panel2.SuspendLayout();
            this.plContentSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // plHeaderTitle
            // 
            this.plHeaderTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.plHeaderTitle.Controls.Add(this.btnClose);
            this.plHeaderTitle.Controls.Add(this.lblHeader);
            this.plHeaderTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.plHeaderTitle.Margin = new System.Windows.Forms.Padding(0);
            this.plHeaderTitle.Name = "plHeaderTitle";
            this.plHeaderTitle.Size = new System.Drawing.Size(720, 56);
            this.plHeaderTitle.TabIndex = 0;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.Location = new System.Drawing.Point(680, 16);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(720, 56);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Dummy";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContent.Location = new System.Drawing.Point(0, 0);
            this.plContent.Margin = new System.Windows.Forms.Padding(0);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(720, 395);
            this.plContent.TabIndex = 1;
            // 
            // plContentSplitter
            // 
            this.plContentSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.plContentSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContentSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.plContentSplitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContentSplitter.IsSplitterFixed = true;
            this.plContentSplitter.Location = new System.Drawing.Point(0, 0);
            this.plContentSplitter.Margin = new System.Windows.Forms.Padding(0);
            this.plContentSplitter.Name = "plContentSplitter";
            this.plContentSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // plContentSplitter.Panel1
            // 
            this.plContentSplitter.Panel1.Controls.Add(this.plHeaderTitle);
            this.plContentSplitter.Panel1MinSize = 56;
            // 
            // plContentSplitter.Panel2
            // 
            this.plContentSplitter.Panel2.Controls.Add(this.plContent);
            this.plContentSplitter.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContentSplitter.Size = new System.Drawing.Size(720, 452);
            this.plContentSplitter.SplitterDistance = 56;
            this.plContentSplitter.SplitterWidth = 1;
            this.plContentSplitter.TabIndex = 2;
            this.plContentSplitter.TabStop = false;
            // 
            // NewGUIFormBase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(720, 452);
            this.ControlBox = false;
            this.Controls.Add(this.plContentSplitter);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.KeyPreview = true;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "NewGUIFormBase";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "NewGUIFormBase";
            this.Load += new System.EventHandler(this.NewGUIFormBase_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.NewGUIFormBase_KeyDown);
            this.plHeaderTitle.ResumeLayout(false);
            this.plHeaderTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.plContentSplitter.Panel1.ResumeLayout(false);
            this.plContentSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plContentSplitter)).EndInit();
            this.plContentSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plHeaderTitle;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.Label lblHeader;
        protected System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.SplitContainer plContentSplitter;
    }
}