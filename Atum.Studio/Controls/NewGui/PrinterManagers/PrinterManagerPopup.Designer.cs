using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGUI.PrinterManagers
{
    partial class PrinterManagerPopup
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
            this.plHeader = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddPrinter = new Atum.Studio.Controls.BaseToolStripButton();
            this.btnRemoveSelectedPrinter = new Atum.Studio.Controls.BaseToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnProperties = new Atum.Studio.Controls.BaseToolStripButton();
            this.btnSetAsDefault = new Atum.Studio.Controls.BaseToolStripButton();
            this.lblHeader = new System.Windows.Forms.Label();
            this.plList = new System.Windows.Forms.Panel();
            this.plContent.SuspendLayout();
            this.plHeader.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.plList);
            this.plContent.Controls.Add(this.plHeader);
            this.plContent.Size = new System.Drawing.Size(296, 345);
            // 
            // plHeader
            // 
            this.plHeader.Controls.Add(this.toolStrip1);
            this.plHeader.Controls.Add(this.lblHeader);
            this.plHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.plHeader.Location = new System.Drawing.Point(0, 0);
            this.plHeader.Name = "plHeader";
            this.plHeader.Size = new System.Drawing.Size(296, 77);
            this.plHeader.TabIndex = 10;
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddPrinter,
            this.btnRemoveSelectedPrinter,
            this.toolStripSeparator1,
            this.btnProperties,
            this.btnSetAsDefault});
            this.toolStrip1.Location = new System.Drawing.Point(0, 50);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(296, 27);
            this.toolStrip1.TabIndex = 8;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddPrinter
            // 
            this.btnAddPrinter.ForeColor = System.Drawing.Color.White;
            this.btnAddPrinter.Image = global::Atum.Studio.Properties.Resources.button_add_white;
            this.btnAddPrinter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPrinter.MinVisibleLevel = Atum.Studio.Core.Enums.SoftwareLevelType.Bronze;
            this.btnAddPrinter.Name = "btnAddPrinter";
            this.btnAddPrinter.Size = new System.Drawing.Size(24, 24);
            this.btnAddPrinter.Click += new System.EventHandler(this.btnAddPrinter_Click);
            // 
            // btnRemoveSelectedPrinter
            // 
            this.btnRemoveSelectedPrinter.ForeColor = System.Drawing.Color.White;
            this.btnRemoveSelectedPrinter.Image = global::Atum.Studio.Properties.Resources.Remove;
            this.btnRemoveSelectedPrinter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveSelectedPrinter.MinVisibleLevel = Atum.Studio.Core.Enums.SoftwareLevelType.Bronze;
            this.btnRemoveSelectedPrinter.Name = "btnRemoveSelectedPrinter";
            this.btnRemoveSelectedPrinter.Size = new System.Drawing.Size(24, 24);
            this.btnRemoveSelectedPrinter.Click += new System.EventHandler(this.btnRemoveSelectedPrinter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.WhiteSmoke;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnProperties
            // 
            this.btnProperties.ForeColor = System.Drawing.Color.White;
            this.btnProperties.Image = global::Atum.Studio.Properties.Resources.Properties;
            this.btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProperties.MinVisibleLevel = Atum.Studio.Core.Enums.SoftwareLevelType.Bronze;
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(24, 24);
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.ForeColor = System.Drawing.Color.White;
            this.btnSetAsDefault.Image = global::Atum.Studio.Properties.Resources.SetAsDefault;
            this.btnSetAsDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetAsDefault.MinVisibleLevel = Atum.Studio.Core.Enums.SoftwareLevelType.Bronze;
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Size = new System.Drawing.Size(101, 24);
            this.btnSetAsDefault.Text = "Set as default";
            this.btnSetAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(296, 50);
            this.lblHeader.TabIndex = 0;
            this.lblHeader.Text = "label1";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plList
            // 
            this.plList.AutoScroll = true;
            this.plList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plList.Location = new System.Drawing.Point(0, 77);
            this.plList.Name = "plList";
            this.plList.Size = new System.Drawing.Size(296, 268);
            this.plList.TabIndex = 11;
            // 
            // PrinterManagerPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(296, 391);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "PrinterManagerPopup";
            this.Text = "PrinterManagerPopup";
            this.Load += new System.EventHandler(this.PrinterManagerPopup_Load);
            this.plContent.ResumeLayout(false);
            this.plHeader.ResumeLayout(false);
            this.plHeader.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel plHeader;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private BaseToolStripButton btnAddPrinter;
        private BaseToolStripButton btnRemoveSelectedPrinter;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BaseToolStripButton btnProperties;
        private BaseToolStripButton btnSetAsDefault;
        private BindingSource atumV2PrinterBindingSource;
        private Panel plList;
    }
}