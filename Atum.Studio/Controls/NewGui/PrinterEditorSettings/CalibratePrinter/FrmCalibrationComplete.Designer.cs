using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui
{
    partial class frmCalibrationComplete
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
            this.plContentSplitter = new System.Windows.Forms.SplitContainer();
            this.plHeaderTitle = new System.Windows.Forms.Panel();
            this.pbClose = new System.Windows.Forms.PictureBox();
            this.pbHelp = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.plContent = new System.Windows.Forms.Panel();
            this.btnCheck = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.label1 = new System.Windows.Forms.Label();
            this.lblTwoText = new System.Windows.Forms.Label();
            this.plCorrectMark = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.pbTickMark = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.plContentSplitter)).BeginInit();
            this.plContentSplitter.Panel1.SuspendLayout();
            this.plContentSplitter.Panel2.SuspendLayout();
            this.plContentSplitter.SuspendLayout();
            this.plHeaderTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.plContent.SuspendLayout();
            this.plCorrectMark.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbTickMark)).BeginInit();
            this.SuspendLayout();
            // 
            // plContentSplitter
            // 
            this.plContentSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
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
            this.plContentSplitter.Size = new System.Drawing.Size(720, 332);
            this.plContentSplitter.SplitterDistance = 56;
            this.plContentSplitter.SplitterWidth = 1;
            this.plContentSplitter.TabIndex = 4;
            this.plContentSplitter.TabStop = false;
            // 
            // plHeaderTitle
            // 
            this.plHeaderTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.plHeaderTitle.Controls.Add(this.pbClose);
            this.plHeaderTitle.Controls.Add(this.pbHelp);
            this.plHeaderTitle.Controls.Add(this.btnClose);
            this.plHeaderTitle.Controls.Add(this.lblHeader);
            this.plHeaderTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.plHeaderTitle.Margin = new System.Windows.Forms.Padding(0);
            this.plHeaderTitle.Name = "plHeaderTitle";
            this.plHeaderTitle.Size = new System.Drawing.Size(720, 56);
            this.plHeaderTitle.TabIndex = 0;
            // 
            // pbClose
            // 
            this.pbClose.Location = new System.Drawing.Point(680, 16);
            this.pbClose.Margin = new System.Windows.Forms.Padding(4);
            this.pbClose.Name = "pbClose";
            this.pbClose.Size = new System.Drawing.Size(24, 24);
            this.pbClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbClose.TabIndex = 3;
            this.pbClose.TabStop = false;
            this.pbClose.Click += new System.EventHandler(this.pictureBox1_Click);
            // 
            // pbHelp
            // 
            this.pbHelp.Image = global::Atum.Studio.Properties.Resources.button_help_white;
            this.pbHelp.Location = new System.Drawing.Point(720, 16);
            this.pbHelp.Margin = new System.Windows.Forms.Padding(4);
            this.pbHelp.Name = "pbHelp";
            this.pbHelp.Size = new System.Drawing.Size(24, 24);
            this.pbHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHelp.TabIndex = 2;
            this.pbHelp.TabStop = false;
            // 
            // btnClose
            // 
            this.btnClose.Image = global::Atum.Studio.Properties.Resources.button_cross_white;
            this.btnClose.Location = new System.Drawing.Point(760, 16);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            // 
            // lblHeader
            // 
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
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plContent.Controls.Add(this.btnCheck);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Controls.Add(this.lblTwoText);
            this.plContent.Controls.Add(this.plCorrectMark);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContent.Location = new System.Drawing.Point(0, 0);
            this.plContent.Margin = new System.Windows.Forms.Padding(0);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(720, 275);
            this.plContent.TabIndex = 1;
            // 
            // btnCheck
            // 
            this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheck.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCheck.ForeColor = System.Drawing.Color.White;
            this.btnCheck.Location = new System.Drawing.Point(293, 212);
            this.btnCheck.Margin = new System.Windows.Forms.Padding(0);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnCheck.Radius = 20;
            this.btnCheck.SingleBorder = false;
            this.btnCheck.Size = new System.Drawing.Size(123, 42);
            this.btnCheck.TabIndex = 18;
            this.btnCheck.Text = "Close";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCloseCalibration_Click);
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.label1.Location = new System.Drawing.Point(0, 124);
            this.label1.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(720, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Calibration completed";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTwoText
            // 
            this.lblTwoText.Location = new System.Drawing.Point(0, 144);
            this.lblTwoText.Margin = new System.Windows.Forms.Padding(5, 0, 5, 0);
            this.lblTwoText.Name = "lblTwoText";
            this.lblTwoText.Size = new System.Drawing.Size(720, 20);
            this.lblTwoText.TabIndex = 7;
            this.lblTwoText.Text = "Congratulations, your printer is calibrated now.";
            this.lblTwoText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plCorrectMark
            // 
            this.plCorrectMark.BackColor = System.Drawing.Color.White;
            this.plCorrectMark.BorderThickness = 0;
            this.plCorrectMark.Controls.Add(this.pbTickMark);
            this.plCorrectMark.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plCorrectMark.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plCorrectMark.Location = new System.Drawing.Point(330, 48);
            this.plCorrectMark.Margin = new System.Windows.Forms.Padding(0);
            this.plCorrectMark.Name = "plCorrectMark";
            this.plCorrectMark.Radius = 28;
            this.plCorrectMark.SingleBorder = false;
            this.plCorrectMark.Size = new System.Drawing.Size(60, 60);
            this.plCorrectMark.TabIndex = 6;
            this.plCorrectMark.UseVisualStyleBackColor = false;
            // 
            // pbTickMark
            // 
            this.pbTickMark.BackColor = System.Drawing.Color.Transparent;
            this.pbTickMark.Location = new System.Drawing.Point(0, 0);
            this.pbTickMark.Margin = new System.Windows.Forms.Padding(0);
            this.pbTickMark.Name = "pbTickMark";
            this.pbTickMark.Size = new System.Drawing.Size(60, 60);
            this.pbTickMark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pbTickMark.TabIndex = 4;
            this.pbTickMark.TabStop = false;
            // 
            // frmCalibrationComplete
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(720, 332);
            this.Controls.Add(this.plContentSplitter);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmCalibrationComplete";
            this.Text = "Calibrate for {{PrinterName}}";
            this.plContentSplitter.Panel1.ResumeLayout(false);
            this.plContentSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plContentSplitter)).EndInit();
            this.plContentSplitter.ResumeLayout(false);
            this.plHeaderTitle.ResumeLayout(false);
            this.plHeaderTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbClose)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plCorrectMark.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pbTickMark)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer plContentSplitter;
        private System.Windows.Forms.Panel plHeaderTitle;
        private System.Windows.Forms.PictureBox pbHelp;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.Label lblHeader;
        protected System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.Label lblTwoText;
        private RoundedButton plCorrectMark;
        private System.Windows.Forms.PictureBox pbClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pbTickMark;
        private NewGui.RoundedButton btnCheck;
    }
}