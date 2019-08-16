namespace Atum.Studio.Controls.Licenses
{
    partial class frmRequestLicense
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
            this.TabControl2 = new System.Windows.Forms.TabControl();
            this.Requestedcode = new System.Windows.Forms.TabPage();
            this.lblSendLicenseTo = new System.Windows.Forms.Label();
            this.txtLicenseInfo = new System.Windows.Forms.TextBox();
            this.btnCopyToClipboard = new System.Windows.Forms.Button();
            this.lbltextBelow = new System.Windows.Forms.Label();
            this.Activationcode = new System.Windows.Forms.TabPage();
            this.Label6 = new System.Windows.Forms.Label();
            this.btnFromClipboard = new System.Windows.Forms.Button();
            this.txtActivationCode = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lblCurrentStatus = new System.Windows.Forms.Label();
            this.lblExpiresAfter = new System.Windows.Forms.Label();
            this.plContent.SuspendLayout();
            this.TabControl2.SuspendLayout();
            this.Requestedcode.SuspendLayout();
            this.Activationcode.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.lblCurrentStatus);
            this.plContent.Controls.Add(this.lblExpiresAfter);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Controls.Add(this.TabControl2);
            this.plContent.Size = new System.Drawing.Size(498, 375);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(432, 8);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(372, 8);
            // 
            // TabControl2
            // 
            this.TabControl2.Alignment = System.Windows.Forms.TabAlignment.Bottom;
            this.TabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.TabControl2.Controls.Add(this.Requestedcode);
            this.TabControl2.Controls.Add(this.Activationcode);
            this.TabControl2.Location = new System.Drawing.Point(12, 57);
            this.TabControl2.Multiline = true;
            this.TabControl2.Name = "TabControl2";
            this.TabControl2.SelectedIndex = 0;
            this.TabControl2.Size = new System.Drawing.Size(472, 313);
            this.TabControl2.TabIndex = 22;
            // 
            // Requestedcode
            // 
            this.Requestedcode.Controls.Add(this.lblSendLicenseTo);
            this.Requestedcode.Controls.Add(this.txtLicenseInfo);
            this.Requestedcode.Controls.Add(this.btnCopyToClipboard);
            this.Requestedcode.Controls.Add(this.lbltextBelow);
            this.Requestedcode.Location = new System.Drawing.Point(4, 4);
            this.Requestedcode.Name = "Requestedcode";
            this.Requestedcode.Padding = new System.Windows.Forms.Padding(3);
            this.Requestedcode.Size = new System.Drawing.Size(464, 287);
            this.Requestedcode.TabIndex = 0;
            this.Requestedcode.Text = "Requested code";
            this.Requestedcode.UseVisualStyleBackColor = true;
            // 
            // lblSendLicenseTo
            // 
            this.lblSendLicenseTo.AutoSize = true;
            this.lblSendLicenseTo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSendLicenseTo.Location = new System.Drawing.Point(-3, 13);
            this.lblSendLicenseTo.Name = "lblSendLicenseTo";
            this.lblSendLicenseTo.Size = new System.Drawing.Size(139, 13);
            this.lblSendLicenseTo.TabIndex = 22;
            this.lblSendLicenseTo.Text = "customer information to: {0} ";
            // 
            // txtLicenseInfo
            // 
            this.txtLicenseInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLicenseInfo.Location = new System.Drawing.Point(0, 33);
            this.txtLicenseInfo.Multiline = true;
            this.txtLicenseInfo.Name = "txtLicenseInfo";
            this.txtLicenseInfo.ReadOnly = true;
            this.txtLicenseInfo.Size = new System.Drawing.Size(418, 249);
            this.txtLicenseInfo.TabIndex = 19;
            // 
            // btnCopyToClipboard
            // 
            this.btnCopyToClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCopyToClipboard.Image = global::Atum.Studio.Properties.Resources.btn_copy_24_2;
            this.btnCopyToClipboard.Location = new System.Drawing.Point(421, 257);
            this.btnCopyToClipboard.Name = "btnCopyToClipboard";
            this.btnCopyToClipboard.Size = new System.Drawing.Size(25, 25);
            this.btnCopyToClipboard.TabIndex = 21;
            this.btnCopyToClipboard.UseVisualStyleBackColor = false;
            this.btnCopyToClipboard.Click += new System.EventHandler(this.btnCopyToClipboard_Click);
            // 
            // lbltextBelow
            // 
            this.lbltextBelow.AutoSize = true;
            this.lbltextBelow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbltextBelow.Location = new System.Drawing.Point(-3, 0);
            this.lbltextBelow.Name = "lbltextBelow";
            this.lbltextBelow.Size = new System.Drawing.Size(411, 13);
            this.lbltextBelow.TabIndex = 19;
            this.lbltextBelow.Text = "Please copy the text below into a new mail message and send this message with you" +
    "r ";
            // 
            // Activationcode
            // 
            this.Activationcode.Controls.Add(this.Label6);
            this.Activationcode.Controls.Add(this.btnFromClipboard);
            this.Activationcode.Controls.Add(this.txtActivationCode);
            this.Activationcode.Location = new System.Drawing.Point(4, 4);
            this.Activationcode.Name = "Activationcode";
            this.Activationcode.Padding = new System.Windows.Forms.Padding(3);
            this.Activationcode.Size = new System.Drawing.Size(464, 287);
            this.Activationcode.TabIndex = 1;
            this.Activationcode.Text = "Activation code";
            this.Activationcode.UseVisualStyleBackColor = true;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(-3, 0);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(369, 13);
            this.Label6.TabIndex = 23;
            this.Label6.Text = "Please copy the activation code, from the mail message, in the textbox below";
            // 
            // btnFromClipboard
            // 
            this.btnFromClipboard.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnFromClipboard.Image = global::Atum.Studio.Properties.Resources.btn_paste_24;
            this.btnFromClipboard.Location = new System.Drawing.Point(1027, 464);
            this.btnFromClipboard.Name = "btnFromClipboard";
            this.btnFromClipboard.Size = new System.Drawing.Size(25, 25);
            this.btnFromClipboard.TabIndex = 22;
            this.btnFromClipboard.UseVisualStyleBackColor = false;
            this.btnFromClipboard.Click += new System.EventHandler(this.btnFromClipboard_Click);
            // 
            // txtActivationCode
            // 
            this.txtActivationCode.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtActivationCode.Location = new System.Drawing.Point(0, 33);
            this.txtActivationCode.Multiline = true;
            this.txtActivationCode.Name = "txtActivationCode";
            this.txtActivationCode.Size = new System.Drawing.Size(1024, 456);
            this.txtActivationCode.TabIndex = 20;
            this.txtActivationCode.TextChanged += new System.EventHandler(this.txtActivationCode_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 23;
            this.label1.Text = "Current Status:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 31);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 13);
            this.label2.TabIndex = 24;
            this.label2.Text = "Expires after:";
            // 
            // lblCurrentStatus
            // 
            this.lblCurrentStatus.AutoSize = true;
            this.lblCurrentStatus.Location = new System.Drawing.Point(95, 9);
            this.lblCurrentStatus.Name = "lblCurrentStatus";
            this.lblCurrentStatus.Size = new System.Drawing.Size(13, 13);
            this.lblCurrentStatus.TabIndex = 25;
            this.lblCurrentStatus.Text = "0";
            // 
            // lblExpiresAfter
            // 
            this.lblExpiresAfter.AutoSize = true;
            this.lblExpiresAfter.Location = new System.Drawing.Point(95, 31);
            this.lblExpiresAfter.Name = "lblExpiresAfter";
            this.lblExpiresAfter.Size = new System.Drawing.Size(13, 13);
            this.lblExpiresAfter.TabIndex = 26;
            this.lblExpiresAfter.Text = "0";
            // 
            // frmRequestLicense
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 412);
            this.Name = "frmRequestLicense";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "License Activation";
            this.Load += new System.EventHandler(this.frmRequestLicense_Load);
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.TabControl2.ResumeLayout(false);
            this.Requestedcode.ResumeLayout(false);
            this.Requestedcode.PerformLayout();
            this.Activationcode.ResumeLayout(false);
            this.Activationcode.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        internal System.Windows.Forms.TabControl TabControl2;
        internal System.Windows.Forms.TabPage Requestedcode;
        internal System.Windows.Forms.Label lblSendLicenseTo;
        internal System.Windows.Forms.TextBox txtLicenseInfo;
        internal System.Windows.Forms.Button btnCopyToClipboard;
        internal System.Windows.Forms.Label lbltextBelow;
        internal System.Windows.Forms.TabPage Activationcode;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.Button btnFromClipboard;
        internal System.Windows.Forms.TextBox txtActivationCode;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblCurrentStatus;
        private System.Windows.Forms.Label lblExpiresAfter;
    }
}