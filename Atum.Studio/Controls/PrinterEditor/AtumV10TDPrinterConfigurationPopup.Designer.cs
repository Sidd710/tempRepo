namespace Atum.Studio.Controls.PrinterEditor
{
    partial class AtumV10TDPrinterConfigurationPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AtumV10TDPrinterConfigurationPopup));
            this.Header = new System.Windows.Forms.Panel();
            this.atumV10TDPrinterConfigurationPanel1 = new Atum.Studio.Controls.PrinterEditor.AtumV10TDPrinterConfigurationPanel();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plContent.Controls.Add(this.atumV10TDPrinterConfigurationPanel1);
            this.plContent.Dock = System.Windows.Forms.DockStyle.None;
            this.plContent.Size = new System.Drawing.Size(516, 464);
            // 
            // Header
            // 
            this.Header.BackColor = System.Drawing.Color.White;
            this.Header.Dock = System.Windows.Forms.DockStyle.Top;
            this.Header.Location = new System.Drawing.Point(0, 0);
            this.Header.Name = "Header";
            this.Header.Size = new System.Drawing.Size(516, 50);
            this.Header.TabIndex = 1;
            // 
            // atumV10TDPrinterConfigurationPanel1
            // 
            this.atumV10TDPrinterConfigurationPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.atumV10TDPrinterConfigurationPanel1.ButtonBackEnabled = true;
            this.atumV10TDPrinterConfigurationPanel1.ButtonBackVisible = false;
            this.atumV10TDPrinterConfigurationPanel1.ButtonFinished = false;
            this.atumV10TDPrinterConfigurationPanel1.ButtonNextEnabled = true;
            this.atumV10TDPrinterConfigurationPanel1.DataSource = null;
            // 
            // atumV10TDPrinterConfigurationPanel1.Header
            // 
            this.atumV10TDPrinterConfigurationPanel1.Header.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.atumV10TDPrinterConfigurationPanel1.Header.BackColor = System.Drawing.Color.White;
            this.atumV10TDPrinterConfigurationPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.atumV10TDPrinterConfigurationPanel1.Header.Name = "Header";
            this.atumV10TDPrinterConfigurationPanel1.Header.Size = new System.Drawing.Size(516, 50);
            this.atumV10TDPrinterConfigurationPanel1.Header.TabIndex = 1;
            this.atumV10TDPrinterConfigurationPanel1.HideFooter = true;
            this.atumV10TDPrinterConfigurationPanel1.Location = new System.Drawing.Point(0, 0);
            this.atumV10TDPrinterConfigurationPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.atumV10TDPrinterConfigurationPanel1.Name = "atumV10TDPrinterConfigurationPanel1";
            this.atumV10TDPrinterConfigurationPanel1.Size = new System.Drawing.Size(516, 464);
            this.atumV10TDPrinterConfigurationPanel1.TabIndex = 0;
            // 
            // AtumV10TDPrinterConfigurationPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(516, 509);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AtumV10TDPrinterConfigurationPopup";
            this.Text = "Printer Properties";
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private AtumV10TDPrinterConfigurationPanel atumV10TDPrinterConfigurationPanel1;
        private System.Windows.Forms.Panel Header;
    }
}