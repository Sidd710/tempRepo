namespace atum3D.RemoteLogging
{
    partial class Form1
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
            this.btnSaveRemoteLogging = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnSaveRemoteLogging
            // 
            this.btnSaveRemoteLogging.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveRemoteLogging.Location = new System.Drawing.Point(199, 17);
            this.btnSaveRemoteLogging.Margin = new System.Windows.Forms.Padding(3, 3, 6, 6);
            this.btnSaveRemoteLogging.Name = "btnSaveRemoteLogging";
            this.btnSaveRemoteLogging.Size = new System.Drawing.Size(75, 23);
            this.btnSaveRemoteLogging.TabIndex = 0;
            this.btnSaveRemoteLogging.Text = "Save logs";
            this.btnSaveRemoteLogging.UseVisualStyleBackColor = true;
            this.btnSaveRemoteLogging.Click += new System.EventHandler(this.button1_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(289, 55);
            this.Controls.Add(this.btnSaveRemoteLogging);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Form1";
            this.Text = "Remote logging";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnSaveRemoteLogging;
    }
}

