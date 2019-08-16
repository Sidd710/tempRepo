namespace Atum.Studio.Controls.SceneControlActionPanel
{
    partial class SceneControlModelDuplicate
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
            this.spcModelControls = new System.Windows.Forms.SplitContainer();
            this.plFooter = new System.Windows.Forms.Panel();
            this.btnDuplicatesFillPlate = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.lblErrorMessage = new System.Windows.Forms.Label();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcModelControls)).BeginInit();
            this.spcModelControls.Panel2.SuspendLayout();
            this.spcModelControls.SuspendLayout();
            this.plFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.plContent.Controls.Add(this.spcModelControls);
            this.plContent.Size = new System.Drawing.Size(360, 158);
            // 
            // spcModelControls
            // 
            this.spcModelControls.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.spcModelControls.Dock = System.Windows.Forms.DockStyle.Fill;
            this.spcModelControls.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.spcModelControls.Location = new System.Drawing.Point(0, 0);
            this.spcModelControls.Name = "spcModelControls";
            this.spcModelControls.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // spcModelControls.Panel1
            // 
            this.spcModelControls.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.spcModelControls.Panel1MinSize = 60;
            // 
            // spcModelControls.Panel2
            // 
            this.spcModelControls.Panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.spcModelControls.Panel2.Controls.Add(this.plFooter);
            this.spcModelControls.Size = new System.Drawing.Size(360, 158);
            this.spcModelControls.SplitterDistance = 66;
            this.spcModelControls.SplitterWidth = 1;
            this.spcModelControls.TabIndex = 4;
            // 
            // plFooter
            // 
            this.plFooter.Controls.Add(this.btnDuplicatesFillPlate);
            this.plFooter.Controls.Add(this.lblErrorMessage);
            this.plFooter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plFooter.Location = new System.Drawing.Point(0, 0);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(360, 91);
            this.plFooter.TabIndex = 0;
            // 
            // btnDuplicatesFillPlate
            // 
            this.btnDuplicatesFillPlate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right)));
            this.btnDuplicatesFillPlate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnDuplicatesFillPlate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnDuplicatesFillPlate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDuplicatesFillPlate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDuplicatesFillPlate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnDuplicatesFillPlate.Location = new System.Drawing.Point(171, 41);
            this.btnDuplicatesFillPlate.Margin = new System.Windows.Forms.Padding(0);
            this.btnDuplicatesFillPlate.Name = "btnDuplicatesFillPlate";
            this.btnDuplicatesFillPlate.Radius = 20;
            this.btnDuplicatesFillPlate.SingleBorder = false;
            this.btnDuplicatesFillPlate.Size = new System.Drawing.Size(176, 40);
            this.btnDuplicatesFillPlate.TabIndex = 4;
            this.btnDuplicatesFillPlate.Text = "Fill Platform";
            this.btnDuplicatesFillPlate.UseVisualStyleBackColor = false;
            this.btnDuplicatesFillPlate.Click += new System.EventHandler(this.btnDuplicatesFillPlate_Click);
            // 
            // lblErrorMessage
            // 
            this.lblErrorMessage.AutoSize = true;
            this.lblErrorMessage.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.lblErrorMessage.Location = new System.Drawing.Point(16, 9);
            this.lblErrorMessage.Name = "lblErrorMessage";
            this.lblErrorMessage.Size = new System.Drawing.Size(120, 13);
            this.lblErrorMessage.TabIndex = 6;
            this.lblErrorMessage.Text = "The build platform is full.";
            this.lblErrorMessage.Visible = false;
            // 
            // SceneControlModelDuplicate
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SceneControlModelDuplicate";
            this.Size = new System.Drawing.Size(360, 215);
            this.Load += new System.EventHandler(this.SceneControlModelDuplicate_Load);
            this.plContent.ResumeLayout(false);
            this.spcModelControls.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.spcModelControls)).EndInit();
            this.spcModelControls.ResumeLayout(false);
            this.plFooter.ResumeLayout(false);
            this.plFooter.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.SplitContainer spcModelControls;
        private NewGui.RoundedButton btnDuplicatesFillPlate;
        private System.Windows.Forms.Panel plFooter;
        private System.Windows.Forms.Label lblErrorMessage;
    }
}
