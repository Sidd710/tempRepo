namespace Atum.Studio.Controls
{
    partial class frmLicenseInfo
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLicenseInfo));
            this.txtLicenseServerName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnSave = new System.Windows.Forms.Button();
            this.dgLicenses = new System.Windows.Forms.DataGridView();
            this.Type = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Status = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLicenses)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.dgLicenses);
            this.plContent.Controls.Add(this.btnSave);
            this.plContent.Controls.Add(this.label2);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Controls.Add(this.txtLicenseServerName);
            this.plContent.Size = new System.Drawing.Size(622, 377);
            // 
            // txtLicenseServerName
            // 
            this.txtLicenseServerName.Location = new System.Drawing.Point(112, 13);
            this.txtLicenseServerName.Name = "txtLicenseServerName";
            this.txtLicenseServerName.Size = new System.Drawing.Size(189, 22);
            this.txtLicenseServerName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Server name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(68, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Licenses:";
            // 
            // btnSave
            // 
            this.btnSave.Location = new System.Drawing.Point(335, 13);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(75, 23);
            this.btnSave.TabIndex = 5;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // dgLicenses
            // 
            this.dgLicenses.AllowUserToAddRows = false;
            this.dgLicenses.AllowUserToDeleteRows = false;
            this.dgLicenses.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgLicenses.BackgroundColor = System.Drawing.Color.White;
            this.dgLicenses.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLicenses.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Type,
            this.Status});
            this.dgLicenses.Location = new System.Drawing.Point(112, 42);
            this.dgLicenses.Name = "dgLicenses";
            this.dgLicenses.ReadOnly = true;
            this.dgLicenses.RowHeadersVisible = false;
            this.dgLicenses.RowTemplate.Height = 24;
            this.dgLicenses.ShowEditingIcon = false;
            this.dgLicenses.Size = new System.Drawing.Size(496, 329);
            this.dgLicenses.TabIndex = 6;
            // 
            // Type
            // 
            this.Type.HeaderText = "Type";
            this.Type.Name = "Type";
            this.Type.ReadOnly = true;
            this.Type.Width = 250;
            // 
            // Status
            // 
            this.Status.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.Status.HeaderText = "Status";
            this.Status.Name = "Status";
            this.Status.ReadOnly = true;
            // 
            // errorProvider1
            // 
            this.errorProvider.ContainerControl = this;
            // 
            // frmLicenseInfo
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(622, 422);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLicenseInfo";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Licenses";
            this.Load += new System.EventHandler(this.frmLicenseInfo_Load);
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLicenses)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLicenseServerName;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.DataGridView dgLicenses;
        private System.Windows.Forms.DataGridViewTextBoxColumn Type;
        private System.Windows.Forms.DataGridViewTextBoxColumn Status;
        private System.Windows.Forms.ErrorProvider errorProvider;
    }
}