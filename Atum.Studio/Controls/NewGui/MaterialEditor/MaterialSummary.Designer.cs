using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    partial class MaterialSummary
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
            this.plMaterialSummary = new System.Windows.Forms.Panel();
            this.lblMaterialName = new System.Windows.Forms.Label();
            this.picColor = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plMaterialSummary.SuspendLayout();
            this.SuspendLayout();
            // 
            // plMaterialSummary
            // 
            this.plMaterialSummary.Controls.Add(this.lblMaterialName);
            this.plMaterialSummary.Controls.Add(this.picColor);
            this.plMaterialSummary.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plMaterialSummary.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.plMaterialSummary.Location = new System.Drawing.Point(0, 0);
            this.plMaterialSummary.Name = "plMaterialSummary";
            this.plMaterialSummary.Size = new System.Drawing.Size(240, 40);
            this.plMaterialSummary.TabIndex = 3;
            this.plMaterialSummary.Click += new System.EventHandler(this.txtMaterialText_Click);
            this.plMaterialSummary.Leave += new System.EventHandler(this.plMaterialSummary_Leave);
            // 
            // lblMaterialName
            // 
            this.lblMaterialName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblMaterialName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblMaterialName.ForeColor = System.Drawing.Color.Black;
            this.lblMaterialName.Location = new System.Drawing.Point(39, 1);
            this.lblMaterialName.Name = "lblMaterialName";
            this.lblMaterialName.Padding = new System.Windows.Forms.Padding(0, 0, 0, 5);
            this.lblMaterialName.Size = new System.Drawing.Size(203, 40);
            this.lblMaterialName.TabIndex = 4;
            this.lblMaterialName.Text = "3DM-FLEX";
            this.lblMaterialName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMaterialName.Click += new System.EventHandler(this.txtMaterialText_Click);
            // 
            // picColor
            // 
            this.picColor.BackColor = System.Drawing.Color.Orange;
            this.picColor.BorderThickness = 2;
            this.picColor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picColor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.picColor.Location = new System.Drawing.Point(16, 9);
            this.picColor.Margin = new System.Windows.Forms.Padding(0);
            this.picColor.Name = "picColor";
            this.picColor.Padding = new System.Windows.Forms.Padding(2);
            this.picColor.Radius = 7;
            this.picColor.SingleBorder = true;
            this.picColor.Size = new System.Drawing.Size(18, 18);
            this.picColor.TabIndex = 3;
            this.picColor.TabStop = false;
            this.picColor.UseVisualStyleBackColor = false;
            this.picColor.Click += new System.EventHandler(this.txtMaterialText_Click);
            // 
            // MaterialSummary
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.plMaterialSummary);
            this.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Name = "MaterialSummary";
            this.Size = new System.Drawing.Size(240, 40);
            this.Click += new System.EventHandler(this.txtMaterialText_Click);
            this.plMaterialSummary.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel plMaterialSummary;
        private System.Windows.Forms.Label lblMaterialName;
        private RoundedButton picColor;
    }
}
