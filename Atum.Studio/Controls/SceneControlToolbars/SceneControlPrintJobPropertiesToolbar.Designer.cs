using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.OpenGL
{
    partial class SceneControlPrintJobPropertiesToolbar
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
            this.lblWidth = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.lblWidthValue = new Atum.Studio.Controls.NewGui.GenericLabelRegular16();
            this.lblDepth = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.lblDepthValue = new Atum.Studio.Controls.NewGui.GenericLabelRegular16();
            this.lblHeight = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.lblHeightValue = new Atum.Studio.Controls.NewGui.GenericLabelRegular16();
            this.genericLabelBold141 = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.cbPrinters = new Atum.Studio.Controls.NewGui.GenericComboxRegular16();
            this.genericLabelBold142 = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.cbMaterials = new Atum.Studio.Controls.NewGui.GenericComboxRegular16();
            this.genericLabelBold143 = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.plPrintJobName = new System.Windows.Forms.Panel();
            this.txtPrintJobName = new GenericTextboxRegular16();
            this.plPrintJobName.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblWidth
            // 
            this.lblWidth.Location = new System.Drawing.Point(24, 12);
            this.lblWidth.Name = "lblWidth";
            this.lblWidth.Size = new System.Drawing.Size(18, 40);
            this.lblWidth.TabIndex = 3;
            this.lblWidth.Text = "L";
            this.lblWidth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblWidthValue
            // 
            this.lblWidthValue.Location = new System.Drawing.Point(48, 12);
            this.lblWidthValue.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.lblWidthValue.Name = "lblWidthValue";
            this.lblWidthValue.Size = new System.Drawing.Size(110, 40);
            this.lblWidthValue.TabIndex = 4;
            this.lblWidthValue.Text = "-";
            this.lblWidthValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepth
            // 
            this.lblDepth.Location = new System.Drawing.Point(169, 12);
            this.lblDepth.Name = "lblDepth";
            this.lblDepth.Size = new System.Drawing.Size(24, 40);
            this.lblDepth.TabIndex = 5;
            this.lblDepth.Text = "W";
            this.lblDepth.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDepthValue
            // 
            this.lblDepthValue.Location = new System.Drawing.Point(193, 12);
            this.lblDepthValue.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.lblDepthValue.Name = "lblDepthValue";
            this.lblDepthValue.Size = new System.Drawing.Size(110, 40);
            this.lblDepthValue.TabIndex = 6;
            this.lblDepthValue.Text = "-";
            this.lblDepthValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeight
            // 
            this.lblHeight.Location = new System.Drawing.Point(314, 12);
            this.lblHeight.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(15, 40);
            this.lblHeight.TabIndex = 7;
            this.lblHeight.Text = "H";
            this.lblHeight.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblHeightValue
            // 
            this.lblHeightValue.Location = new System.Drawing.Point(344, 12);
            this.lblHeightValue.Margin = new System.Windows.Forms.Padding(3, 0, 24, 0);
            this.lblHeightValue.Name = "lblHeightValue";
            this.lblHeightValue.Size = new System.Drawing.Size(110, 40);
            this.lblHeightValue.TabIndex = 8;
            this.lblHeightValue.Text = "-";
            this.lblHeightValue.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // genericLabelBold141
            // 
            this.genericLabelBold141.Location = new System.Drawing.Point(467, 12);
            this.genericLabelBold141.Margin = new System.Windows.Forms.Padding(3, 0, 6, 0);
            this.genericLabelBold141.Name = "genericLabelBold141";
            this.genericLabelBold141.Size = new System.Drawing.Size(66, 40);
            this.genericLabelBold141.TabIndex = 9;
            this.genericLabelBold141.Text = "Project";
            this.genericLabelBold141.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbPrinters
            // 
            this.cbPrinters.BackColor = System.Drawing.Color.White;
            this.cbPrinters.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbPrinters.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cbPrinters.Location = new System.Drawing.Point(832, 12);
            this.cbPrinters.Name = "cbPrinters";
            this.cbPrinters.Size = new System.Drawing.Size(240, 40);
            this.cbPrinters.TabIndex = 12;
            this.cbPrinters.Click += new System.EventHandler(this.cbPrinters_Click);
            // 
            // genericLabelBold142
            // 
            this.genericLabelBold142.Location = new System.Drawing.Point(723, 12);
            this.genericLabelBold142.Margin = new System.Windows.Forms.Padding(24, 0, 6, 0);
            this.genericLabelBold142.Name = "genericLabelBold142";
            this.genericLabelBold142.Size = new System.Drawing.Size(100, 40);
            this.genericLabelBold142.TabIndex = 11;
            this.genericLabelBold142.Text = "DLP Station";
            this.genericLabelBold142.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // cbMaterials
            // 
            this.cbMaterials.BackColor = System.Drawing.Color.White;
            this.cbMaterials.Cursor = System.Windows.Forms.Cursors.Hand;
            this.cbMaterials.Font = new System.Drawing.Font("Times New Roman", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.cbMaterials.Location = new System.Drawing.Point(1146, 12);
            this.cbMaterials.Margin = new System.Windows.Forms.Padding(3, 3, 24, 3);
            this.cbMaterials.Name = "cbMaterials";
            this.cbMaterials.Size = new System.Drawing.Size(300, 40);
            this.cbMaterials.TabIndex = 14;
            this.cbMaterials.Click += new System.EventHandler(this.cbMaterials_Click);
            // 
            // genericLabelBold143
            // 
            this.genericLabelBold143.Location = new System.Drawing.Point(1086, 12);
            this.genericLabelBold143.Margin = new System.Windows.Forms.Padding(24, 0, 6, 0);
            this.genericLabelBold143.Name = "genericLabelBold143";
            this.genericLabelBold143.Size = new System.Drawing.Size(51, 40);
            this.genericLabelBold143.TabIndex = 13;
            this.genericLabelBold143.Text = "Resin";
            this.genericLabelBold143.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // plPrintJobName
            // 
            this.plPrintJobName.BackColor = System.Drawing.Color.White;
            this.plPrintJobName.Controls.Add(this.txtPrintJobName);
            this.plPrintJobName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plPrintJobName.Location = new System.Drawing.Point(542, 12);
            this.plPrintJobName.Margin = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.plPrintJobName.Name = "plPrintJobName";
            this.plPrintJobName.Size = new System.Drawing.Size(181, 40);
            this.plPrintJobName.TabIndex = 15;
            // 
            // txtPrintJobName
            // 
            this.txtPrintJobName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtPrintJobName.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F);
            this.txtPrintJobName.Location = new System.Drawing.Point(7, 10);
            this.txtPrintJobName.MaxLength = 16;
            this.txtPrintJobName.Name = "txtPrintJobName";
            this.txtPrintJobName.Size = new System.Drawing.Size(171, 37);
            this.txtPrintJobName.TabIndex = 9;
            this.txtPrintJobName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtPrintJobName_MouseClick);
            this.txtPrintJobName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtPrintJobName_KeyUp);
            // 
            // SceneControlPrintJobPropertiesToolbar
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.Controls.Add(this.plPrintJobName);
            this.Controls.Add(this.cbMaterials);
            this.Controls.Add(this.genericLabelBold143);
            this.Controls.Add(this.cbPrinters);
            this.Controls.Add(this.genericLabelBold142);
            this.Controls.Add(this.genericLabelBold141);
            this.Controls.Add(this.lblHeightValue);
            this.Controls.Add(this.lblHeight);
            this.Controls.Add(this.lblDepthValue);
            this.Controls.Add(this.lblDepth);
            this.Controls.Add(this.lblWidthValue);
            this.Controls.Add(this.lblWidth);
            this.Name = "SceneControlPrintJobPropertiesToolbar";
            this.Size = new System.Drawing.Size(1470, 64);
            this.Load += new System.EventHandler(this.SceneControlPrintJobPropertiesToolbar_Load);
            this.plPrintJobName.ResumeLayout(false);
            this.plPrintJobName.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private NewGui.GenericLabelBold14 lblWidth;
        private NewGui.GenericLabelRegular16 lblWidthValue;
        private NewGui.GenericLabelBold14 lblDepth;
        private NewGui.GenericLabelRegular16 lblDepthValue;
        private NewGui.GenericLabelBold14 lblHeight;
        private NewGui.GenericLabelRegular16 lblHeightValue;
        private NewGui.GenericLabelBold14 genericLabelBold141;
        private NewGui.GenericComboxRegular16 cbPrinters;
        private NewGui.GenericLabelBold14 genericLabelBold142;
        private NewGui.GenericComboxRegular16 cbMaterials;
        private NewGui.GenericLabelBold14 genericLabelBold143;
        private System.Windows.Forms.Panel plPrintJobName;
        private GenericTextboxRegular16 txtPrintJobName;
    }
}
