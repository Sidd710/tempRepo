using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui.MaterialDisplay
{
    partial class displayMaterial
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
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblResinPropertyValue2 = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.lblResinPropertyName2 = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.lblResinPropertyName1 = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.lblResinPropertyValue1 = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.plmain = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.lblSupplierName = new System.Windows.Forms.Label();
            this.lblMaterialDisplayName = new System.Windows.Forms.Label();
            this.picColor = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.tableLayoutPanel1.SuspendLayout();
            this.plmain.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblMaterialName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMaterialName.Location = new System.Drawing.Point(16, 37);
            this.lblMaterialName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Size = new System.Drawing.Size(182, 44);
            this.lblMaterialName.TabIndex = 12;
            this.lblMaterialName.Text = "Rh High Rebound \r\nWhite High Wattage\r\n";
            this.lblMaterialName.Click += new System.EventHandler(this.plmain_Click);
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 55F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 45F));
            this.tableLayoutPanel1.Controls.Add(this.lblResinPropertyValue2, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblResinPropertyName2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblResinPropertyName1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.lblResinPropertyValue1, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(17, 84);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(3, 3, 16, 10);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(179, 44);
            this.tableLayoutPanel1.TabIndex = 13;
            this.tableLayoutPanel1.Click += new System.EventHandler(this.tableLayoutPanel1_Click);
            // 
            // lblResinPropertyValue2
            // 
            this.lblResinPropertyValue2.AutoSize = true;
            this.lblResinPropertyValue2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResinPropertyValue2.Location = new System.Drawing.Point(101, 22);
            this.lblResinPropertyValue2.Name = "lblResinPropertyValue2";
            this.lblResinPropertyValue2.Size = new System.Drawing.Size(72, 22);
            this.lblResinPropertyValue2.TabIndex = 3;
            this.lblResinPropertyValue2.Text = "lblResinPropertyValue2";
            this.lblResinPropertyValue2.Click += new System.EventHandler(this.lblResinPropertyName2_Click);
            // 
            // lblResinPropertyName2
            // 
            this.lblResinPropertyName2.AutoSize = true;
            this.lblResinPropertyName2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResinPropertyName2.Location = new System.Drawing.Point(0, 22);
            this.lblResinPropertyName2.Margin = new System.Windows.Forms.Padding(0);
            this.lblResinPropertyName2.Name = "lblResinPropertyName2";
            this.lblResinPropertyName2.Size = new System.Drawing.Size(96, 22);
            this.lblResinPropertyName2.TabIndex = 2;
            this.lblResinPropertyName2.Text = "lblResinPropertyName2";
            this.lblResinPropertyName2.Click += new System.EventHandler(this.lblResinPropertyName2_Click);
            // 
            // lblResinPropertyName1
            // 
            this.lblResinPropertyName1.AutoSize = true;
            this.lblResinPropertyName1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResinPropertyName1.Location = new System.Drawing.Point(0, 0);
            this.lblResinPropertyName1.Margin = new System.Windows.Forms.Padding(0);
            this.lblResinPropertyName1.Name = "lblResinPropertyName1";
            this.lblResinPropertyName1.Size = new System.Drawing.Size(96, 22);
            this.lblResinPropertyName1.TabIndex = 1;
            this.lblResinPropertyName1.Text = "lblResinPropertyName1";
            this.lblResinPropertyName1.Click += new System.EventHandler(this.lblResinPropertyName2_Click);
            // 
            // lblResinPropertyValue1
            // 
            this.lblResinPropertyValue1.AutoSize = true;
            this.lblResinPropertyValue1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblResinPropertyValue1.Location = new System.Drawing.Point(101, 0);
            this.lblResinPropertyValue1.Name = "lblResinPropertyValue1";
            this.lblResinPropertyValue1.Size = new System.Drawing.Size(72, 22);
            this.lblResinPropertyValue1.TabIndex = 0;
            this.lblResinPropertyValue1.Text = "lblResinPropertyValue1";
            this.lblResinPropertyValue1.Click += new System.EventHandler(this.lblResinPropertyName2_Click);
            // 
            // plmain
            // 
            this.plmain.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plmain.Controls.Add(this.lblSupplierName);
            this.plmain.Controls.Add(this.lblMaterialDisplayName);
            this.plmain.Controls.Add(this.picColor);
            this.plmain.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plmain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plmain.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plmain.Location = new System.Drawing.Point(2, 2);
            this.plmain.Margin = new System.Windows.Forms.Padding(4);
            this.plmain.Name = "plmain";
            this.plmain.Radius = 8;
            this.plmain.SingleBorder = false;
            this.plmain.Size = new System.Drawing.Size(204, 136);
            this.plmain.TabIndex = 0;
            this.plmain.UseVisualStyleBackColor = false;
            this.plmain.Click += new System.EventHandler(this.plmain_Click);
            // 
            // lblSupplierName
            // 
            this.lblSupplierName.AutoSize = true;
            this.lblSupplierName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSupplierName.Location = new System.Drawing.Point(14, 16);
            this.lblSupplierName.Margin = new System.Windows.Forms.Padding(3, 0, 4, 0);
            this.lblSupplierName.Name = "lblSupplierName";
            this.lblSupplierName.Size = new System.Drawing.Size(40, 17);
            this.lblSupplierName.TabIndex = 11;
            this.lblSupplierName.Text = "3DM";
            this.lblSupplierName.Click += new System.EventHandler(this.plmain_Click);
            // 
            // lblMaterialDisplayName
            // 
            this.lblMaterialDisplayName.AutoSize = true;
            this.lblMaterialDisplayName.Location = new System.Drawing.Point(18, 18);
            this.lblMaterialDisplayName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblMaterialDisplayName.Name = "lblMaterialDisplayName";
            this.lblMaterialDisplayName.Size = new System.Drawing.Size(0, 17);
            this.lblMaterialDisplayName.TabIndex = 0;
            // 
            // picColor
            // 
            this.picColor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(245)))), ((int)(((byte)(166)))), ((int)(((byte)(35)))));
            this.picColor.BorderThickness = 2;
            this.picColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.picColor.Location = new System.Drawing.Point(171, 14);
            this.picColor.Margin = new System.Windows.Forms.Padding(0, 16, 16, 0);
            this.picColor.Name = "picColor";
            this.picColor.Radius = 8;
            this.picColor.SingleBorder = true;
            this.picColor.Size = new System.Drawing.Size(19, 19);
            this.picColor.TabIndex = 4;
            this.picColor.TabStop = false;
            this.picColor.UseVisualStyleBackColor = false;
            this.picColor.Click += new System.EventHandler(this.plmain_Click);
            // 
            // displayMaterial
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.lblMaterialName);
            this.Controls.Add(this.plmain);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "displayMaterial";
            this.Padding = new System.Windows.Forms.Padding(2);
            this.Size = new System.Drawing.Size(208, 140);
            this.Click += new System.EventHandler(this.displayMaterial_Click);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.plmain.ResumeLayout(false);
            this.plmain.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private RoundedButton plmain;
        private System.Windows.Forms.Label lblMaterialDisplayName;
        private RoundedButton picColor;
        private System.Windows.Forms.Label lblSupplierName;
        private System.Windows.Forms.Label lblMaterialName;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private GenericLabelRegular14 lblResinPropertyValue2;
        private GenericLabelRegular14 lblResinPropertyName2;
        private GenericLabelRegular14 lblResinPropertyName1;
        private GenericLabelRegular14 lblResinPropertyValue1;
    }
}
