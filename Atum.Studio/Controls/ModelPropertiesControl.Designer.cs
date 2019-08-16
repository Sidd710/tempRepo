namespace Atum.Studio.Controls
{
    partial class ModelPropertiesControl
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
            this.plHeader = new System.Windows.Forms.Panel();
            this.lblHeaderText = new System.Windows.Forms.Label();
            this.plContent = new System.Windows.Forms.Panel();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbModel = new System.Windows.Forms.TabPage();
            this.trModel = new System.Windows.Forms.TreeView();
            this.label12 = new System.Windows.Forms.Label();
            this.plHeader.SuspendLayout();
            this.plContent.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbModel.SuspendLayout();
            this.SuspendLayout();
            // 
            // plHeader
            // 
            this.plHeader.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plHeader.BackColor = System.Drawing.Color.White;
            this.plHeader.Controls.Add(this.lblHeaderText);
            this.plHeader.Location = new System.Drawing.Point(0, 0);
            this.plHeader.Name = "plHeader";
            this.plHeader.Size = new System.Drawing.Size(324, 29);
            this.plHeader.TabIndex = 0;
            this.plHeader.Paint += new System.Windows.Forms.PaintEventHandler(this.plHeader_Paint);
            // 
            // lblHeaderText
            // 
            this.lblHeaderText.AutoSize = true;
            this.lblHeaderText.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblHeaderText.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.lblHeaderText.Location = new System.Drawing.Point(4, 5);
            this.lblHeaderText.Name = "lblHeaderText";
            this.lblHeaderText.Size = new System.Drawing.Size(59, 20);
            this.lblHeaderText.TabIndex = 0;
            this.lblHeaderText.Text = "label1";
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plContent.Controls.Add(this.tabControl1);
            this.plContent.Location = new System.Drawing.Point(0, 35);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(324, 365);
            this.plContent.TabIndex = 1;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tbModel);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(324, 365);
            this.tabControl1.TabIndex = 0;
            // 
            // tbModel
            // 
            this.tbModel.Controls.Add(this.trModel);
            this.tbModel.Controls.Add(this.label12);
            this.tbModel.Location = new System.Drawing.Point(4, 25);
            this.tbModel.Name = "tbModel";
            this.tbModel.Padding = new System.Windows.Forms.Padding(3);
            this.tbModel.Size = new System.Drawing.Size(316, 336);
            this.tbModel.TabIndex = 4;
            this.tbModel.Text = "Model";
            this.tbModel.UseVisualStyleBackColor = true;
            // 
            // trModel
            // 
            this.trModel.Location = new System.Drawing.Point(3, 0);
            this.trModel.Name = "trModel";
            this.trModel.Size = new System.Drawing.Size(310, 298);
            this.trModel.TabIndex = 24;
            this.trModel.BeforeSelect += new System.Windows.Forms.TreeViewCancelEventHandler(this.trModel_BeforeSelect);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(206, 197);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(104, 17);
            this.label12.TabIndex = 22;
            this.label12.Text = "Bottom Radius:";
            this.label12.Visible = false;
            // 
            // ModelPropertiesControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.plHeader);
            this.Name = "ModelPropertiesControl";
            this.Size = new System.Drawing.Size(324, 400);
            this.Load += new System.EventHandler(this.ModelPropertiesControl_Load);
            this.plHeader.ResumeLayout(false);
            this.plHeader.PerformLayout();
            this.plContent.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbModel.ResumeLayout(false);
            this.tbModel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plHeader;
        private System.Windows.Forms.Label lblHeaderText;
        private System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbModel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TreeView trModel;
    }
}
