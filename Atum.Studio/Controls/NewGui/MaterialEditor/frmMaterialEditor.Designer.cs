namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    partial class frmMaterialEditor
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
            this.newGUIContentSplitContainerBase1 = new Atum.Studio.Controls.NewGui.NewGUIContentSplitContainerBase();
            this.plFooter = new System.Windows.Forms.Panel();
            this.spcFooterContainer = new System.Windows.Forms.SplitContainer();
            this.pbMinusSign = new System.Windows.Forms.PictureBox();
            this.pbPlusSign = new System.Windows.Forms.PictureBox();
            this.btnApply = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plContent.SuspendLayout();
            this.plFooter.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcFooterContainer)).BeginInit();
            this.spcFooterContainer.Panel1.SuspendLayout();
            this.spcFooterContainer.Panel2.SuspendLayout();
            this.spcFooterContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbMinusSign)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlusSign)).BeginInit();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.plFooter);
            this.plContent.Controls.Add(this.newGUIContentSplitContainerBase1);
            this.plContent.Size = new System.Drawing.Size(720, 545);
            // 
            // newGUIContentSplitContainerBase1
            // 
            this.newGUIContentSplitContainerBase1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.newGUIContentSplitContainerBase1.Dock = System.Windows.Forms.DockStyle.Top;
            this.newGUIContentSplitContainerBase1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.newGUIContentSplitContainerBase1.Location = new System.Drawing.Point(0, 0);
            this.newGUIContentSplitContainerBase1.Margin = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.newGUIContentSplitContainerBase1.Name = "newGUIContentSplitContainerBase1";
            this.newGUIContentSplitContainerBase1.Size = new System.Drawing.Size(720, 489);
            this.newGUIContentSplitContainerBase1.TabIndex = 0;
            this.newGUIContentSplitContainerBase1.Load += new System.EventHandler(this.newGUIContentSplitContainerBase1_Load);
            // 
            // plFooter
            // 
            this.plFooter.Controls.Add(this.spcFooterContainer);
            this.plFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plFooter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plFooter.Location = new System.Drawing.Point(0, 489);
            this.plFooter.Margin = new System.Windows.Forms.Padding(5, 0, 5, 5);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(720, 56);
            this.plFooter.TabIndex = 1;
            // 
            // spcFooterContainer
            // 
            this.spcFooterContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcFooterContainer.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.spcFooterContainer.Location = new System.Drawing.Point(0, 0);
            this.spcFooterContainer.Margin = new System.Windows.Forms.Padding(0);
            this.spcFooterContainer.Name = "spcFooterContainer";
            // 
            // spcFooterContainer.Panel1
            // 
            this.spcFooterContainer.Panel1.BackColor = System.Drawing.Color.White;
            this.spcFooterContainer.Panel1.Controls.Add(this.pbMinusSign);
            this.spcFooterContainer.Panel1.Controls.Add(this.pbPlusSign);
            this.spcFooterContainer.Panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.spcFooterContainer.Panel1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.spcFooterContainer_Panel1_MouseMove);
            // 
            // spcFooterContainer.Panel2
            // 
            this.spcFooterContainer.Panel2.Controls.Add(this.btnApply);
            this.spcFooterContainer.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.spcFooterContainer.Size = new System.Drawing.Size(720, 56);
            this.spcFooterContainer.SplitterDistance = 240;
            this.spcFooterContainer.SplitterWidth = 1;
            this.spcFooterContainer.TabIndex = 0;
            // 
            // pbMinusSign
            // 
            this.pbMinusSign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pbMinusSign.BackColor = System.Drawing.Color.Transparent;
            this.pbMinusSign.Image = global::Atum.Studio.Properties.Resources.button_remove;
            this.pbMinusSign.Location = new System.Drawing.Point(184, 8);
            this.pbMinusSign.Name = "pbMinusSign";
            this.pbMinusSign.Size = new System.Drawing.Size(40, 40);
            this.pbMinusSign.TabIndex = 1;
            this.pbMinusSign.TabStop = false;
            this.pbMinusSign.Click += new System.EventHandler(this.pbMinusSign_Click);
            this.pbMinusSign.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pbMinusSign_MouseMove);
            // 
            // pbPlusSign
            // 
            this.pbPlusSign.BackColor = System.Drawing.Color.Transparent;
            this.pbPlusSign.Image = global::Atum.Studio.Properties.Resources.button_add;
            this.pbPlusSign.Location = new System.Drawing.Point(16, 8);
            this.pbPlusSign.Name = "pbPlusSign";
            this.pbPlusSign.Size = new System.Drawing.Size(40, 40);
            this.pbPlusSign.TabIndex = 0;
            this.pbPlusSign.TabStop = false;
            this.pbPlusSign.Click += new System.EventHandler(this.pbPlusSign_Click);
            // 
            // btnApply
            // 
            this.btnApply.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnApply.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnApply.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnApply.Location = new System.Drawing.Point(338, 6);
            this.btnApply.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.btnApply.Name = "btnApply";
            this.btnApply.Radius = 20;
            this.btnApply.SingleBorder = true;
            this.btnApply.Size = new System.Drawing.Size(125, 42);
            this.btnApply.TabIndex = 5;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // frmMaterialEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(720, 602);
            this.Margin = new System.Windows.Forms.Padding(5);
            this.Name = "frmMaterialEditor";
            this.Text = "Resin Settings";
            this.plContent.ResumeLayout(false);
            this.plFooter.ResumeLayout(false);
            this.spcFooterContainer.Panel1.ResumeLayout(false);
            this.spcFooterContainer.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcFooterContainer)).EndInit();
            this.spcFooterContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbMinusSign)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlusSign)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private NewGUIContentSplitContainerBase newGUIContentSplitContainerBase1;
        private System.Windows.Forms.Panel plFooter;
        private System.Windows.Forms.SplitContainer spcFooterContainer;
        private System.Windows.Forms.PictureBox pbMinusSign;
        private System.Windows.Forms.PictureBox pbPlusSign;
        private RoundedButton btnApply;
    }
}