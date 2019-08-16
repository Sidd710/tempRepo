namespace Atum.Studio.Controls.WifiWizard
{
    partial class WifiSaveConfigurationToUSBTabPanel
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.cbUSBDriveLetters = new System.Windows.Forms.ComboBox();
            this.btnRefreshUSBDisks = new System.Windows.Forms.Button();
            this.btnSaveConfiguration = new System.Windows.Forms.Button();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Text = "Save on Removable Medium";
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.just_wifi;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.btnSaveConfiguration);
            this.plContent.Controls.Add(this.btnRefreshUSBDisks);
            this.plContent.Controls.Add(this.cbUSBDriveLetters);
            this.plContent.Controls.Add(this.pictureBox1);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = global::Atum.Studio.Properties.Resources.USB_Stick;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(443, 449);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // cbUSBDriveLetters
            // 
            this.cbUSBDriveLetters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSBDriveLetters.FormattingEnabled = true;
            this.cbUSBDriveLetters.Location = new System.Drawing.Point(35, 323);
            this.cbUSBDriveLetters.Name = "cbUSBDriveLetters";
            this.cbUSBDriveLetters.Size = new System.Drawing.Size(155, 24);
            this.cbUSBDriveLetters.TabIndex = 1;
            this.cbUSBDriveLetters.SelectedIndexChanged += new System.EventHandler(this.cbUSBDriveLetters_SelectedIndexChanged);
            // 
            // btnRefreshUSBDisks
            // 
            this.btnRefreshUSBDisks.Location = new System.Drawing.Point(208, 323);
            this.btnRefreshUSBDisks.Name = "btnRefreshUSBDisks";
            this.btnRefreshUSBDisks.Size = new System.Drawing.Size(75, 23);
            this.btnRefreshUSBDisks.TabIndex = 2;
            this.btnRefreshUSBDisks.Text = "Refresh";
            this.btnRefreshUSBDisks.UseVisualStyleBackColor = true;
            this.btnRefreshUSBDisks.Click += new System.EventHandler(this.btnRefreshUSBDisks_Click);
            // 
            // btnSaveConfiguration
            // 
            this.btnSaveConfiguration.Location = new System.Drawing.Point(208, 353);
            this.btnSaveConfiguration.Name = "btnSaveConfiguration";
            this.btnSaveConfiguration.Size = new System.Drawing.Size(75, 23);
            this.btnSaveConfiguration.TabIndex = 3;
            this.btnSaveConfiguration.Text = "Save";
            this.btnSaveConfiguration.UseVisualStyleBackColor = true;
            this.btnSaveConfiguration.Click += new System.EventHandler(this.btnSaveConfiguration_Click);
            // 
            // WifiSaveConfigurationToUSBTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "WifiSaveConfigurationToUSBTabPanel";
            this.Load += new System.EventHandler(this.WifiSaveConfigurationToUSBTabPanel_Load);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnSaveConfiguration;
        private System.Windows.Forms.Button btnRefreshUSBDisks;
        private System.Windows.Forms.ComboBox cbUSBDriveLetters;
    }
}
