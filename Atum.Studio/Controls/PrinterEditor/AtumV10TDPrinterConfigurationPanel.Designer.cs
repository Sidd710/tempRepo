namespace Atum.Studio.Controls.PrinterEditor
{
    partial class AtumV10TDPrinterConfigurationPanel
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
            this.customTabControl1 = new System.Windows.Forms.CustomTabControl();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtStartPrintOffset = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.printerPropertyControl1 = new Atum.Studio.Controls.PrinterConnectionWizard.PrinterPropertyControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnLowerRightCalc = new System.Windows.Forms.Button();
            this.btnUpperRigthCalc = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.trapezoid1 = new Atum.Studio.Controls.Correction.Trapezoid();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.customTabControl1.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartPrintOffset)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnBack
            // 
            this.btnBack.Location = new System.Drawing.Point(1063, 9);
            // 
            // btnNext
            // 
            this.btnNext.Location = new System.Drawing.Point(1144, 9);
            // 
            // plHeader
            // 
            this.plHeader.Size = new System.Drawing.Size(534, 55);
            // 
            // txtHeader
            // 
            this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.txtHeader.Size = new System.Drawing.Size(167, 43);
            this.txtHeader.Text = "Atum V1.0TD";
            // 
            // picHeader
            // 
            this.picHeader.Visible = false;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.customTabControl1);
            this.plContent.Size = new System.Drawing.Size(534, 459);
            // 
            // customTabControl1
            // 
            this.customTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.customTabControl1.Controls.Add(this.tabPage3);
            this.customTabControl1.Controls.Add(this.tabPage2);
            // 
            // 
            // 
            this.customTabControl1.DisplayStyleProvider.BorderColor = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorHot = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.BorderColorSelected = System.Drawing.Color.FromArgb(((int)(((byte)(127)))), ((int)(((byte)(157)))), ((int)(((byte)(185)))));
            this.customTabControl1.DisplayStyleProvider.CloserColor = System.Drawing.Color.DarkGray;
            this.customTabControl1.DisplayStyleProvider.FocusTrack = true;
            this.customTabControl1.DisplayStyleProvider.HotTrack = false;
            this.customTabControl1.DisplayStyleProvider.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.customTabControl1.DisplayStyleProvider.Opacity = 1F;
            this.customTabControl1.DisplayStyleProvider.Overlap = 0;
            this.customTabControl1.DisplayStyleProvider.Padding = new System.Drawing.Point(6, 3);
            this.customTabControl1.DisplayStyleProvider.Radius = 2;
            this.customTabControl1.DisplayStyleProvider.ShowTabCloser = false;
            this.customTabControl1.DisplayStyleProvider.TextColor = System.Drawing.SystemColors.ControlText;
            this.customTabControl1.DisplayStyleProvider.TextColorDisabled = System.Drawing.SystemColors.ControlDark;
            this.customTabControl1.DisplayStyleProvider.TextColorSelected = System.Drawing.SystemColors.ControlText;
            this.customTabControl1.Location = new System.Drawing.Point(4, -1);
            this.customTabControl1.Name = "customTabControl1";
            this.customTabControl1.SelectedIndex = 0;
            this.customTabControl1.Size = new System.Drawing.Size(420, 400);
            this.customTabControl1.TabIndex = 30;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.panel2);
            this.tabPage3.Location = new System.Drawing.Point(4, 26);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(412, 370);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Printer Properties";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.txtStartPrintOffset);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.lblDisplayName);
            this.panel2.Controls.Add(this.txtDisplayName);
            this.panel2.Controls.Add(this.lblDescription);
            this.panel2.Controls.Add(this.txtDescription);
            this.panel2.Controls.Add(this.printerPropertyControl1);
            this.panel2.Location = new System.Drawing.Point(3, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(406, 364);
            this.panel2.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(27, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 17);
            this.label3.TabIndex = 21;
            this.label3.Text = "Start position:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(198, 107);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 17);
            this.label4.TabIndex = 20;
            this.label4.Text = "mm";
            // 
            // txtStartPrintOffset
            // 
            this.txtStartPrintOffset.Location = new System.Drawing.Point(131, 105);
            this.txtStartPrintOffset.Name = "txtStartPrintOffset";
            this.txtStartPrintOffset.Size = new System.Drawing.Size(49, 22);
            this.txtStartPrintOffset.TabIndex = 19;
            this.txtStartPrintOffset.Value = new decimal(new int[] {
            5,
            0,
            0,
            0});
            this.txtStartPrintOffset.ValueChanged += new System.EventHandler(this.txtStartPrintOffset_ValueChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "Spindle type:";
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(27, 17);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(97, 17);
            this.lblDisplayName.TabIndex = 10;
            this.lblDisplayName.Text = "Display name:";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(131, 14);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(236, 22);
            this.txtDisplayName.TabIndex = 9;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(27, 140);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(83, 17);
            this.lblDescription.TabIndex = 8;
            this.lblDescription.Text = "Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(131, 137);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(236, 101);
            this.txtDescription.TabIndex = 7;
            // 
            // printerPropertyControl1
            // 
            this.printerPropertyControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.printerPropertyControl1.HideBorder = true;
            this.printerPropertyControl1.Location = new System.Drawing.Point(93, 1);
            this.printerPropertyControl1.Name = "printerPropertyControl1";
            this.printerPropertyControl1.Size = new System.Drawing.Size(274, 93);
            this.printerPropertyControl1.TabIndex = 12;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.btnLowerRightCalc);
            this.tabPage2.Controls.Add(this.btnUpperRigthCalc);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.trapezoid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 26);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(402, 360);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Calibration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnLowerRightCalc
            // 
            this.btnLowerRightCalc.Location = new System.Drawing.Point(306, 244);
            this.btnLowerRightCalc.Name = "btnLowerRightCalc";
            this.btnLowerRightCalc.Size = new System.Drawing.Size(60, 23);
            this.btnLowerRightCalc.TabIndex = 3;
            this.btnLowerRightCalc.Text = "Calc";
            this.btnLowerRightCalc.UseVisualStyleBackColor = true;
            this.btnLowerRightCalc.Click += new System.EventHandler(this.btnLowerRightCalc_Click);
            // 
            // btnUpperRigthCalc
            // 
            this.btnUpperRigthCalc.Location = new System.Drawing.Point(306, 11);
            this.btnUpperRigthCalc.Name = "btnUpperRigthCalc";
            this.btnUpperRigthCalc.Size = new System.Drawing.Size(60, 23);
            this.btnUpperRigthCalc.TabIndex = 2;
            this.btnUpperRigthCalc.Text = "Calc";
            this.btnUpperRigthCalc.UseVisualStyleBackColor = true;
            this.btnUpperRigthCalc.Click += new System.EventHandler(this.btnUpperRigthCalc_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(6, 293);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Unit: mm";
            // 
            // trapezoid1
            // 
            this.trapezoid1.BackColor = System.Drawing.Color.White;
            this.trapezoid1.Location = new System.Drawing.Point(3, 36);
            this.trapezoid1.Name = "trapezoid1";
            this.trapezoid1.Size = new System.Drawing.Size(366, 202);
            this.trapezoid1.TabIndex = 0;
            // 
            // AtumV10TDPrinterConfigurationPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "AtumV10TDPrinterConfigurationPanel";
            this.Size = new System.Drawing.Size(534, 556);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.customTabControl1.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartPrintOffset)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CustomTabControl customTabControl1;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private PrinterConnectionWizard.PrinterPropertyControl printerPropertyControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Button btnLowerRightCalc;
        private System.Windows.Forms.Button btnUpperRigthCalc;
        private System.Windows.Forms.Label label2;
        private Correction.Trapezoid trapezoid1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown txtStartPrintOffset;
    }
}
