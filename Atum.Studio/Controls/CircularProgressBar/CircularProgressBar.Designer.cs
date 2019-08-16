namespace Atum.Studio.Controls
{
    partial class CirclularProgressBar
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
            this.lblNum = new Atum.Studio.Controls.CircularProgressLabel();
            this.lblTagLine = new Atum.Studio.Controls.CircularProgressLabel();
            this.SuspendLayout();
            // 
            // lblNum
            // 
            this.lblNum.AutoSize = true;
            this.lblNum.Location = new System.Drawing.Point(30, 80);
            this.lblNum.Name = "lblNum";
            this.lblNum.Size = new System.Drawing.Size(19, 13);
            this.lblNum.TabIndex = 0;
            this.lblNum.Text = "10";
            this.lblNum.TextBottom = 0;
            this.lblNum.TextTop = 0;
            this.lblNum.Visible = false;
            // 
            // lblTagLine
            // 
            this.lblTagLine.AutoSize = true;
            this.lblTagLine.Location = new System.Drawing.Point(30, 80);
            this.lblTagLine.Name = "lblTagLine";
            this.lblTagLine.Size = new System.Drawing.Size(19, 13);
            this.lblTagLine.TabIndex = 0;
            this.lblTagLine.Text = "10";
            this.lblTagLine.TextBottom = 0;
            this.lblTagLine.TextTop = 0;
            this.lblTagLine.Visible = false;
            // 
            // CirclularProgressBar
            // 
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.Controls.Add(this.lblNum);
            this.Size = new System.Drawing.Size(200, 200);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private CircularProgressLabel lblNum;
        private CircularProgressLabel lblTagLine;
        //private CircularProgressLabel lblTagLine;
    }
}
