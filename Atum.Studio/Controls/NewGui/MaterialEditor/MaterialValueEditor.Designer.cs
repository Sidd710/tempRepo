namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    partial class MaterialValueEditor
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
            this.lblMaterialName = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblDisplayName = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblMaterialValue = new System.Windows.Forms.Label();
            this.plDisplayName = new System.Windows.Forms.Panel();
            this.txtMaterialDisplayName1 = new System.Windows.Forms.TextBox();
            this.btnExportSettings = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.lblSupplierValue = new System.Windows.Forms.Label();
            this.lblSupplierName = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblTechnicalPropertyValue1 = new System.Windows.Forms.Label();
            this.lblTechnicalPropertyName1 = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblTechnicalPropertyValue2 = new System.Windows.Forms.Label();
            this.lblTechnicalPropertyName2 = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.plDisplayName.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.AutoSize = true;
            this.lblMaterialName.BackColor = System.Drawing.Color.Transparent;
            this.lblMaterialName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMaterialName.Location = new System.Drawing.Point(17, 151);
            this.lblMaterialName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(49, 17);
            this.lblMaterialName.TabIndex = 5;
            this.lblMaterialName.Text = "Resin";
            this.lblMaterialName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblDisplayName.Location = new System.Drawing.Point(17, 19);
            this.lblDisplayName.Margin = new System.Windows.Forms.Padding(0);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(105, 17);
            this.lblDisplayName.TabIndex = 3;
            this.lblDisplayName.Text = "Display name";
            this.lblDisplayName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblMaterialValue
            // 
            this.lblMaterialValue.AutoSize = true;
            this.lblMaterialValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMaterialValue.ForeColor = System.Drawing.Color.Black;
            this.lblMaterialValue.Location = new System.Drawing.Point(17, 177);
            this.lblMaterialValue.Name = "lblMaterialValue";
            this.lblMaterialValue.Size = new System.Drawing.Size(94, 17);
            this.lblMaterialValue.TabIndex = 7;
            this.lblMaterialValue.Text = "MaterialValue";
            this.lblMaterialValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // plDisplayName
            // 
            this.plDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plDisplayName.BackColor = System.Drawing.Color.White;
            this.plDisplayName.Controls.Add(this.txtMaterialDisplayName1);
            this.plDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plDisplayName.Location = new System.Drawing.Point(22, 45);
            this.plDisplayName.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.plDisplayName.Name = "plDisplayName";
            this.plDisplayName.Size = new System.Drawing.Size(445, 40);
            this.plDisplayName.TabIndex = 9;
            // 
            // txtMaterialDisplayName1
            // 
            this.txtMaterialDisplayName1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtMaterialDisplayName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.txtMaterialDisplayName1.Location = new System.Drawing.Point(7, 10);
            this.txtMaterialDisplayName1.MaxLength = 16;
            this.txtMaterialDisplayName1.Name = "txtMaterialDisplayName1";
            this.txtMaterialDisplayName1.Size = new System.Drawing.Size(428, 37);
            this.txtMaterialDisplayName1.TabIndex = 9;
            this.txtMaterialDisplayName1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtMaterialDisplayName1_MouseClick);
            this.txtMaterialDisplayName1.TextChanged += new System.EventHandler(this.txtMaterialDisplayName1_TextChanged);
            this.txtMaterialDisplayName1.Enter += new System.EventHandler(this.txtMaterialDisplayName1_Enter);
            // 
            // btnExportSettings
            // 
            this.btnExportSettings.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.btnExportSettings.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExportSettings.BorderColor = System.Drawing.Color.Black;
            this.btnExportSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnExportSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExportSettings.ForeColor = System.Drawing.Color.Black;
            this.btnExportSettings.Location = new System.Drawing.Point(311, 328);
            this.btnExportSettings.Margin = new System.Windows.Forms.Padding(16, 16, 13, 16);
            this.btnExportSettings.Name = "btnExportSettings";
            this.btnExportSettings.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnExportSettings.Radius = 20;
            this.btnExportSettings.SingleBorder = false;
            this.btnExportSettings.Size = new System.Drawing.Size(156, 42);
            this.btnExportSettings.TabIndex = 18;
            this.btnExportSettings.Text = "Export Settings";
            this.btnExportSettings.UseVisualStyleBackColor = false;
            this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
            // 
            // lblSupplierValue
            // 
            this.lblSupplierValue.AutoSize = true;
            this.lblSupplierValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSupplierValue.ForeColor = System.Drawing.Color.Black;
            this.lblSupplierValue.Location = new System.Drawing.Point(17, 123);
            this.lblSupplierValue.Name = "lblSupplierValue";
            this.lblSupplierValue.Size = new System.Drawing.Size(96, 17);
            this.lblSupplierValue.TabIndex = 20;
            this.lblSupplierValue.Text = "SupplierValue";
            this.lblSupplierValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.BackColor = System.Drawing.Color.Transparent;
            this.lblSupplierName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSupplierName.Location = new System.Drawing.Point(17, 97);
            this.lblSupplierName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(68, 17);
            this.lblSupplierName.TabIndex = 19;
            this.lblSupplierName.Text = "Supplier";
            this.lblSupplierName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTechnicalPropertyValue1
            // 
            this.lblTechnicalPropertyValue1.AutoSize = true;
            this.lblTechnicalPropertyValue1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTechnicalPropertyValue1.ForeColor = System.Drawing.Color.Black;
            this.lblTechnicalPropertyValue1.Location = new System.Drawing.Point(17, 231);
            this.lblTechnicalPropertyValue1.Name = "lblTechnicalPropertyValue1";
            this.lblTechnicalPropertyValue1.Size = new System.Drawing.Size(167, 17);
            this.lblTechnicalPropertyValue1.TabIndex = 22;
            this.lblTechnicalPropertyValue1.Text = "TechnicalPropertyValue1";
            this.lblTechnicalPropertyValue1.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTechnicalPropertyName1
            // 
            this.lblTechnicalPropertyName1.AutoSize = true;
            this.lblTechnicalPropertyName1.BackColor = System.Drawing.Color.Transparent;
            this.lblTechnicalPropertyName1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTechnicalPropertyName1.Location = new System.Drawing.Point(17, 205);
            this.lblTechnicalPropertyName1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTechnicalPropertyName1.Name = "lblTechnicalPropertyName1";
            this.lblTechnicalPropertyName1.Size = new System.Drawing.Size(190, 17);
            this.lblTechnicalPropertyName1.TabIndex = 21;
            this.lblTechnicalPropertyName1.Text = "TechnicalPropertyName1";
            this.lblTechnicalPropertyName1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblTechnicalPropertyValue2
            // 
            this.lblTechnicalPropertyValue2.AutoSize = true;
            this.lblTechnicalPropertyValue2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTechnicalPropertyValue2.ForeColor = System.Drawing.Color.Black;
            this.lblTechnicalPropertyValue2.Location = new System.Drawing.Point(17, 285);
            this.lblTechnicalPropertyValue2.Name = "lblTechnicalPropertyValue2";
            this.lblTechnicalPropertyValue2.Size = new System.Drawing.Size(167, 17);
            this.lblTechnicalPropertyValue2.TabIndex = 24;
            this.lblTechnicalPropertyValue2.Text = "TechnicalPropertyValue2";
            this.lblTechnicalPropertyValue2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblTechnicalPropertyName2
            // 
            this.lblTechnicalPropertyName2.AutoSize = true;
            this.lblTechnicalPropertyName2.BackColor = System.Drawing.Color.Transparent;
            this.lblTechnicalPropertyName2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblTechnicalPropertyName2.Location = new System.Drawing.Point(17, 259);
            this.lblTechnicalPropertyName2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTechnicalPropertyName2.Name = "lblTechnicalPropertyName2";
            this.lblTechnicalPropertyName2.Size = new System.Drawing.Size(190, 17);
            this.lblTechnicalPropertyName2.TabIndex = 23;
            this.lblTechnicalPropertyName2.Text = "TechnicalPropertyName2";
            this.lblTechnicalPropertyName2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // MaterialValueEditor
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.lblTechnicalPropertyValue2);
            this.Controls.Add(this.lblTechnicalPropertyName2);
            this.Controls.Add(this.lblTechnicalPropertyValue1);
            this.Controls.Add(this.lblTechnicalPropertyName1);
            this.Controls.Add(this.lblSupplierValue);
            this.Controls.Add(this.lblSupplierName);
            this.Controls.Add(this.btnExportSettings);
            this.Controls.Add(this.plDisplayName);
            this.Controls.Add(this.lblMaterialValue);
            this.Controls.Add(this.lblMaterialName);
            this.Controls.Add(this.lblDisplayName);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MaterialValueEditor";
            this.Size = new System.Drawing.Size(480, 546);
            this.plDisplayName.ResumeLayout(false);
            this.plDisplayName.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private NewGUILabel lblDisplayName;
        private NewGUILabel lblMaterialName;
        private System.Windows.Forms.Label lblMaterialValue;
        private System.Windows.Forms.Panel plDisplayName;
        private System.Windows.Forms.TextBox txtMaterialDisplayName1;
        private NewGui.RoundedButton btnExportSettings;
        private System.Windows.Forms.Label lblSupplierValue;
        private NewGUILabel lblSupplierName;
        private System.Windows.Forms.Label lblTechnicalPropertyValue1;
        private NewGUILabel lblTechnicalPropertyName1;
        private System.Windows.Forms.Label lblTechnicalPropertyValue2;
        private NewGUILabel lblTechnicalPropertyName2;
    }
}
