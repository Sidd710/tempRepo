namespace Atum.Studio.Controls.WifiWizard
{
    partial class WifiSettingsTabPanel
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
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.chkHideCharacters = new System.Windows.Forms.CheckBox();
            this.cbSecurityType = new System.Windows.Forms.ComboBox();
            this.txtSecurityKey = new System.Windows.Forms.TextBox();
            this.txtNetworkName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cbEncryptionType = new System.Windows.Forms.ComboBox();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Text = "Settings";
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.just_wifi;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.label4);
            this.plContent.Controls.Add(this.cbEncryptionType);
            this.plContent.Controls.Add(this.label3);
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.chkHideCharacters);
            this.plContent.Controls.Add(this.cbSecurityType);
            this.plContent.Controls.Add(this.txtSecurityKey);
            this.plContent.Controls.Add(this.txtNetworkName);
            this.plContent.Controls.Add(this.label1);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 189);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 17);
            this.label3.TabIndex = 19;
            this.label3.Text = "Security Key:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 97);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(94, 17);
            this.label2.TabIndex = 18;
            this.label2.Text = "Security type:";
            // 
            // chkHideCharacters
            // 
            this.chkHideCharacters.AutoSize = true;
            this.chkHideCharacters.Location = new System.Drawing.Point(411, 189);
            this.chkHideCharacters.Name = "chkHideCharacters";
            this.chkHideCharacters.Size = new System.Drawing.Size(130, 21);
            this.chkHideCharacters.TabIndex = 17;
            this.chkHideCharacters.Text = "Hide characters";
            this.chkHideCharacters.UseVisualStyleBackColor = true;
            this.chkHideCharacters.CheckedChanged += new System.EventHandler(this.chkHideCharacters_CheckedChanged);
            // 
            // cbSecurityType
            // 
            this.cbSecurityType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbSecurityType.FormattingEnabled = true;
            this.cbSecurityType.Items.AddRange(new object[] {
            "WPA-Personal",
            "WPA2-Personal"});
            this.cbSecurityType.Location = new System.Drawing.Point(193, 92);
            this.cbSecurityType.Name = "cbSecurityType";
            this.cbSecurityType.Size = new System.Drawing.Size(201, 24);
            this.cbSecurityType.TabIndex = 1;
            // 
            // txtSecurityKey
            // 
            this.txtSecurityKey.Location = new System.Drawing.Point(193, 186);
            this.txtSecurityKey.Name = "txtSecurityKey";
            this.txtSecurityKey.Size = new System.Drawing.Size(201, 22);
            this.txtSecurityKey.TabIndex = 3;
            // 
            // txtNetworkName
            // 
            this.txtNetworkName.Location = new System.Drawing.Point(193, 47);
            this.txtNetworkName.Name = "txtNetworkName";
            this.txtNetworkName.Size = new System.Drawing.Size(201, 22);
            this.txtNetworkName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "Network name:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 144);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(110, 17);
            this.label4.TabIndex = 21;
            this.label4.Text = "Encryption type:";
            // 
            // cbEncryptionType
            // 
            this.cbEncryptionType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEncryptionType.FormattingEnabled = true;
            this.cbEncryptionType.Items.AddRange(new object[] {
            "TKIP",
            "EAS"});
            this.cbEncryptionType.Location = new System.Drawing.Point(193, 139);
            this.cbEncryptionType.Name = "cbEncryptionType";
            this.cbEncryptionType.Size = new System.Drawing.Size(201, 24);
            this.cbEncryptionType.TabIndex = 2;
            // 
            // WifiSettingsTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "WifiSettingsTabPanel";
            this.Load += new System.EventHandler(this.WifiSettingsTabPanel_Load);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox chkHideCharacters;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.ComboBox cbSecurityType;
        public System.Windows.Forms.TextBox txtSecurityKey;
        public System.Windows.Forms.TextBox txtNetworkName;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.ComboBox cbEncryptionType;
    }
}
