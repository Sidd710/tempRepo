namespace Atum.Studio.Controls.QuickTips
{
    partial class QuickTipActionInformation
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
            this.lblCaption = new System.Windows.Forms.Label();
            this.lblText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblCaption
            // 
            this.lblCaption.AutoSize = true;
            this.lblCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCaption.Location = new System.Drawing.Point(28, 8);
            this.lblCaption.Name = "lblCaption";
            this.lblCaption.Size = new System.Drawing.Size(66, 13);
            this.lblCaption.TabIndex = 1;
            this.lblCaption.Text = "Quick Tip:";
            // 
            // lblText
            // 
            this.lblText.AutoSize = true;
            this.lblText.Location = new System.Drawing.Point(28, 28);
            this.lblText.Name = "lblText";
            this.lblText.Size = new System.Drawing.Size(35, 39);
            this.lblText.TabIndex = 2;
            this.lblText.Text = "label2\r\ntest\r\ntest\r\n";
            this.lblText.Resize += new System.EventHandler(this.lblText_Resize);
            // 
            // QuickTipActionInformation
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.lblText);
            this.Controls.Add(this.lblCaption);
            this.DoubleBuffered = true;
            this.Name = "QuickTipActionInformation";
            this.Size = new System.Drawing.Size(248, 125);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblText;
        internal System.Windows.Forms.Label lblCaption;
    }
}
