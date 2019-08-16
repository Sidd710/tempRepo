namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionPropertiesTabPanel
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
            this.atumV15PrinterProperties1 = new Atum.Studio.Controls.PrinterEditor.AtumV15PrinterProperties();
            this.atumV20PrinterProperties1 = new Atum.Studio.Controls.PrinterEditor.AtumV20PrinterProperties();
            this.atumV40PrinterProperties1 = new Atum.Studio.Controls.PrinterEditor.AtumDLPStation5PrinterProperties();
            this.loctiteV10PrinterProperties1 = new Atum.Studio.Controls.PrinterEditor.LoctiteV10PrinterProperties();
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Location = new System.Drawing.Point(64, 4);
            this.txtHeader.Size = new System.Drawing.Size(376, 43);
            this.txtHeader.Text = "Properties";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.printer_blue_icon;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.loctiteV10PrinterProperties1);
            this.plContent.Controls.Add(this.atumV20PrinterProperties1);
            this.plContent.Controls.Add(this.atumV15PrinterProperties1);
            this.plContent.Controls.Add(this.atumV40PrinterProperties1);
            this.plContent.Size = new System.Drawing.Size(443, 321);
            this.plContent.TabIndex = 0;
            // 
            // atumV15PrinterProperties1
            // 
            this.atumV15PrinterProperties1.BackColor = System.Drawing.Color.White;
            this.atumV15PrinterProperties1.DataSource = null;
            this.atumV15PrinterProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atumV15PrinterProperties1.inWizard = false;
            this.atumV15PrinterProperties1.Location = new System.Drawing.Point(0, 0);
            this.atumV15PrinterProperties1.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.atumV15PrinterProperties1.Name = "atumV15PrinterProperties1";
            this.atumV15PrinterProperties1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.atumV15PrinterProperties1.Size = new System.Drawing.Size(443, 321);
            this.atumV15PrinterProperties1.TabIndex = 2;
            // 
            // atumV20PrinterProperties1
            // 
            this.atumV20PrinterProperties1.BackColor = System.Drawing.Color.White;
            this.atumV20PrinterProperties1.DataSource = null;
            this.atumV20PrinterProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atumV20PrinterProperties1.inWizard = false;
            this.atumV20PrinterProperties1.Location = new System.Drawing.Point(0, 0);
            this.atumV20PrinterProperties1.Name = "atumV20PrinterProperties1";
            this.atumV20PrinterProperties1.Size = new System.Drawing.Size(443, 321);
            this.atumV20PrinterProperties1.TabIndex = 3;
            // 
            // atumV40PrinterProperties1
            // 
            this.atumV40PrinterProperties1.BackColor = System.Drawing.Color.White;
            this.atumV40PrinterProperties1.DataSource = null;
            this.atumV40PrinterProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atumV40PrinterProperties1.inWizard = false;
            this.atumV40PrinterProperties1.Location = new System.Drawing.Point(0, 0);
            this.atumV40PrinterProperties1.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.atumV40PrinterProperties1.Name = "atumV40PrinterProperties1";
            this.atumV40PrinterProperties1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.atumV40PrinterProperties1.Size = new System.Drawing.Size(443, 321);
            this.atumV40PrinterProperties1.TabIndex = 4;
            // 
            // loctiteV10PrinterProperties1
            // 
            this.loctiteV10PrinterProperties1.BackColor = System.Drawing.Color.White;
            this.loctiteV10PrinterProperties1.DataSource = null;
            this.loctiteV10PrinterProperties1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.loctiteV10PrinterProperties1.inWizard = false;
            this.loctiteV10PrinterProperties1.Location = new System.Drawing.Point(0, 0);
            this.loctiteV10PrinterProperties1.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.loctiteV10PrinterProperties1.Name = "loctiteV10PrinterProperties1";
            this.loctiteV10PrinterProperties1.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.loctiteV10PrinterProperties1.Size = new System.Drawing.Size(443, 321);
            this.loctiteV10PrinterProperties1.TabIndex = 5;
            // 
            // PrinterConnectionPropertiesTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "PrinterConnectionPropertiesTabPanel";
            this.Size = new System.Drawing.Size(443, 418);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private PrinterEditor.AtumV15PrinterProperties atumV15PrinterProperties1;
        private PrinterEditor.AtumV20PrinterProperties atumV20PrinterProperties1;
        private PrinterEditor.AtumDLPStation5PrinterProperties atumV40PrinterProperties1;
        private PrinterEditor.LoctiteV10PrinterProperties loctiteV10PrinterProperties1;
    }
}
