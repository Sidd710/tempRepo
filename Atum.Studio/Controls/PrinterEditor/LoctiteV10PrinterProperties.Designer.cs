namespace Atum.Studio.Controls.PrinterEditor
{
    partial class LoctiteV10PrinterProperties
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.Properties = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbtn100Micron = new System.Windows.Forms.RadioButton();
            this.rbtn50Micron = new System.Windows.Forms.RadioButton();
            this.rbtn75Micron = new System.Windows.Forms.RadioButton();
            this.Calibration = new System.Windows.Forms.TabPage();
            this.atumPrinterCalibration1 = new Atum.Studio.Controls.PrinterEditor.AtumPrinterCalibration();
            this.lblHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.Properties.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.Calibration.SuspendLayout();
            this.lblHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.Properties);
            this.tabControl1.Controls.Add(this.Calibration);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 46);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(441, 339);
            this.tabControl1.TabIndex = 0;
            this.tabControl1.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // Properties
            // 
            this.Properties.Controls.Add(this.tableLayoutPanel1);
            this.Properties.Location = new System.Drawing.Point(4, 22);
            this.Properties.Name = "Properties";
            this.Properties.Padding = new System.Windows.Forms.Padding(3);
            this.Properties.Size = new System.Drawing.Size(433, 313);
            this.Properties.TabIndex = 0;
            this.Properties.Text = "Printer properties";
            this.Properties.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.lblDisplayName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtDisplayName, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDescription, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(427, 307);
            this.tableLayoutPanel1.TabIndex = 20;
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(3, 0);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(73, 13);
            this.lblDisplayName.TabIndex = 14;
            this.lblDisplayName.Text = "Display name:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 15;
            this.label2.Text = "Printer Resolution:";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayName.Location = new System.Drawing.Point(102, 3);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(322, 20);
            this.txtDisplayName.TabIndex = 13;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.Location = new System.Drawing.Point(102, 29);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(322, 66);
            this.txtDescription.TabIndex = 11;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(3, 26);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 12;
            this.lblDescription.Text = "Description:";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Transparent;
            this.panel2.Controls.Add(this.rbtn100Micron);
            this.panel2.Controls.Add(this.rbtn50Micron);
            this.panel2.Controls.Add(this.rbtn75Micron);
            this.panel2.Location = new System.Drawing.Point(102, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(175, 100);
            this.panel2.TabIndex = 15;
            // 
            // rbtn100Micron
            // 
            this.rbtn100Micron.AutoSize = true;
            this.rbtn100Micron.Location = new System.Drawing.Point(2, 47);
            this.rbtn100Micron.Name = "rbtn100Micron";
            this.rbtn100Micron.Size = new System.Drawing.Size(77, 17);
            this.rbtn100Micron.TabIndex = 18;
            this.rbtn100Micron.Text = "100 micron";
            this.rbtn100Micron.UseVisualStyleBackColor = true;
            this.rbtn100Micron.CheckedChanged += new System.EventHandler(this.rbtnMicron_CheckedChanged);
            // 
            // rbtn50Micron
            // 
            this.rbtn50Micron.AutoSize = true;
            this.rbtn50Micron.Location = new System.Drawing.Point(2, 1);
            this.rbtn50Micron.Name = "rbtn50Micron";
            this.rbtn50Micron.Size = new System.Drawing.Size(71, 17);
            this.rbtn50Micron.TabIndex = 19;
            this.rbtn50Micron.Text = "50 micron";
            this.rbtn50Micron.UseVisualStyleBackColor = true;
            this.rbtn50Micron.CheckedChanged += new System.EventHandler(this.rbtnMicron_CheckedChanged);
            // 
            // rbtn75Micron
            // 
            this.rbtn75Micron.AutoSize = true;
            this.rbtn75Micron.Checked = true;
            this.rbtn75Micron.Location = new System.Drawing.Point(2, 24);
            this.rbtn75Micron.Name = "rbtn75Micron";
            this.rbtn75Micron.Size = new System.Drawing.Size(71, 17);
            this.rbtn75Micron.TabIndex = 17;
            this.rbtn75Micron.TabStop = true;
            this.rbtn75Micron.Text = "75 micron";
            this.rbtn75Micron.UseVisualStyleBackColor = true;
            this.rbtn75Micron.CheckedChanged += new System.EventHandler(this.rbtnMicron_CheckedChanged);
            // 
            // Calibration
            // 
            this.Calibration.Controls.Add(this.atumPrinterCalibration1);
            this.Calibration.Location = new System.Drawing.Point(4, 22);
            this.Calibration.Name = "Calibration";
            this.Calibration.Padding = new System.Windows.Forms.Padding(3);
            this.Calibration.Size = new System.Drawing.Size(433, 313);
            this.Calibration.TabIndex = 1;
            this.Calibration.Text = "Calibration";
            this.Calibration.UseVisualStyleBackColor = true;
            // 
            // atumPrinterCalibration1
            // 
            this.atumPrinterCalibration1.AdvancedMode = false;
            this.atumPrinterCalibration1.DataSource = null;
            this.atumPrinterCalibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atumPrinterCalibration1.Location = new System.Drawing.Point(3, 3);
            this.atumPrinterCalibration1.Margin = new System.Windows.Forms.Padding(4);
            this.atumPrinterCalibration1.Name = "atumPrinterCalibration1";
            this.atumPrinterCalibration1.Size = new System.Drawing.Size(427, 307);
            this.atumPrinterCalibration1.TabIndex = 0;
            // 
            // lblHeader
            // 
            this.lblHeader.Controls.Add(this.label1);
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Location = new System.Drawing.Point(3, 3);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(441, 43);
            this.lblHeader.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(101, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Loctite V10";
            // 
            // LoctiteV10PrinterProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.lblHeader);
            this.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.Name = "LoctiteV10PrinterProperties";
            this.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Size = new System.Drawing.Size(444, 388);
            this.tabControl1.ResumeLayout(false);
            this.Properties.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.Calibration.ResumeLayout(false);
            this.lblHeader.ResumeLayout(false);
            this.lblHeader.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage Properties;
        private System.Windows.Forms.TabPage Calibration;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private AtumPrinterCalibration atumPrinterCalibration1;
        private System.Windows.Forms.Panel lblHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtn100Micron;
        private System.Windows.Forms.RadioButton rbtn75Micron;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtn50Micron;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
    }
}
