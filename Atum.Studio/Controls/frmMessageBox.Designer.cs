namespace Atum.Studio.Controls
{
    partial class frmMessageBox
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
            this.btnLeft = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnMiddle = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnRight = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.label1 = new System.Windows.Forms.Label();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.label1);
            this.plContent.Controls.Add(this.btnRight);
            this.plContent.Controls.Add(this.btnMiddle);
            this.plContent.Controls.Add(this.btnLeft);
            this.plContent.Padding = new System.Windows.Forms.Padding(1, 0, 2, 2);
            this.plContent.Size = new System.Drawing.Size(800, 393);
            this.plContent.Paint += new System.Windows.Forms.PaintEventHandler(this.plContent_Paint);
            // 
            // btnLeft
            // 
            this.btnLeft.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLeft.FlatAppearance.BorderSize = 0;
            this.btnLeft.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLeft.Location = new System.Drawing.Point(26, 311);
            this.btnLeft.Margin = new System.Windows.Forms.Padding(16);
            this.btnLeft.Name = "btnLeft";
            this.btnLeft.Radius = 20;
            this.btnLeft.SingleBorder = false;
            this.btnLeft.Size = new System.Drawing.Size(100, 40);
            this.btnLeft.TabIndex = 0;
            this.btnLeft.Text = "Left";
            this.btnLeft.UseVisualStyleBackColor = true;
            this.btnLeft.Click += new System.EventHandler(this.btnLeft_Click);
            // 
            // btnMiddle
            // 
            this.btnMiddle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnMiddle.FlatAppearance.BorderSize = 0;
            this.btnMiddle.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnMiddle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMiddle.Location = new System.Drawing.Point(329, 328);
            this.btnMiddle.Margin = new System.Windows.Forms.Padding(16);
            this.btnMiddle.Name = "btnMiddle";
            this.btnMiddle.Radius = 20;
            this.btnMiddle.SingleBorder = false;
            this.btnMiddle.Size = new System.Drawing.Size(100, 40);
            this.btnMiddle.TabIndex = 1;
            this.btnMiddle.Text = "OK";
            this.btnMiddle.UseVisualStyleBackColor = true;
            this.btnMiddle.Click += new System.EventHandler(this.btnMiddle_Click);
            // 
            // btnRight
            // 
            this.btnRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRight.FlatAppearance.BorderSize = 0;
            this.btnRight.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnRight.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnRight.Location = new System.Drawing.Point(675, 328);
            this.btnRight.Margin = new System.Windows.Forms.Padding(16);
            this.btnRight.Name = "btnRight";
            this.btnRight.Radius = 20;
            this.btnRight.SingleBorder = false;
            this.btnRight.Size = new System.Drawing.Size(100, 40);
            this.btnRight.TabIndex = 2;
            this.btnRight.Text = "Right";
            this.btnRight.UseVisualStyleBackColor = true;
            this.btnRight.Click += new System.EventHandler(this.btnRight_Click);
            // 
            // label1
            // 
            this.label1.Dock = System.Windows.Forms.DockStyle.Top;
            this.label1.Location = new System.Drawing.Point(1, 0);
            this.label1.Margin = new System.Windows.Forms.Padding(1);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(16);
            this.label1.Size = new System.Drawing.Size(797, 81);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // frmMessageBox
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Name = "frmMessageBox";
            this.Text = "frmMessageBox";
            this.Resize += new System.EventHandler(this.frmMessageBox_Resize);
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private NewGui.RoundedButton btnLeft;
        private NewGui.RoundedButton btnRight;
        private NewGui.RoundedButton btnMiddle;
        private System.Windows.Forms.Label label1;
    }
}