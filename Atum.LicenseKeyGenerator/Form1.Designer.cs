namespace Atum.LicenseKeyGenerator
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtRequestedCode = new System.Windows.Forms.TextBox();
            this.dateTimePicker1 = new System.Windows.Forms.DateTimePicker();
            this.label3 = new System.Windows.Forms.Label();
            this.txtActivationCode = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.lblRequestedLicenseAmount = new System.Windows.Forms.Label();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.lblRequestedLicenseAmount);
            this.plContent.Controls.Add(this.label4);
            this.plContent.Controls.Add(this.txtActivationCode);
            this.plContent.Controls.Add(this.dateTimePicker1);
            this.plContent.Controls.Add(this.txtRequestedCode);
            this.plContent.Controls.Add(this.label3);
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Size = new System.Drawing.Size(512, 581);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCancel.Location = new System.Drawing.Point(445, 7);
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOK.Location = new System.Drawing.Point(385, 7);
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(36, 320);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Activation Code:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(36, 36);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Requested Code:";
            // 
            // txtRequestedCode
            // 
            this.txtRequestedCode.Location = new System.Drawing.Point(133, 36);
            this.txtRequestedCode.Multiline = true;
            this.txtRequestedCode.Name = "txtRequestedCode";
            this.txtRequestedCode.Size = new System.Drawing.Size(320, 225);
            this.txtRequestedCode.TabIndex = 2;
            this.txtRequestedCode.TextChanged += new System.EventHandler(this.txtRequestedCode_TextChanged);
            // 
            // dateTimePicker1
            // 
            this.dateTimePicker1.Location = new System.Drawing.Point(133, 267);
            this.dateTimePicker1.Name = "dateTimePicker1";
            this.dateTimePicker1.Size = new System.Drawing.Size(200, 20);
            this.dateTimePicker1.TabIndex = 3;
            this.dateTimePicker1.ValueChanged += new System.EventHandler(this.dateTimePicker1_ValueChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 271);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Activated Until:";
            // 
            // txtActivationCode
            // 
            this.txtActivationCode.Location = new System.Drawing.Point(133, 317);
            this.txtActivationCode.Multiline = true;
            this.txtActivationCode.Name = "txtActivationCode";
            this.txtActivationCode.Size = new System.Drawing.Size(320, 225);
            this.txtActivationCode.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 295);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(74, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "License Type:";
            // 
            // lblRequestedLicenseAmount
            // 
            this.lblRequestedLicenseAmount.AutoSize = true;
            this.lblRequestedLicenseAmount.Location = new System.Drawing.Point(130, 295);
            this.lblRequestedLicenseAmount.Name = "lblRequestedLicenseAmount";
            this.lblRequestedLicenseAmount.Size = new System.Drawing.Size(46, 13);
            this.lblRequestedLicenseAmount.TabIndex = 7;
            this.lblRequestedLicenseAmount.Text = "Amount:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(512, 618);
            this.Name = "Form1";
            this.Text = "Form1";
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtRequestedCode;
        private System.Windows.Forms.DateTimePicker dateTimePicker1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtActivationCode;
        private System.Windows.Forms.Label lblRequestedLicenseAmount;
        private System.Windows.Forms.Label label4;
    }
}

