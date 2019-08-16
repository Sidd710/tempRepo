namespace Atum.Studio.Controls
{
    partial class PrinterCorrectionWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterCorrectionWizard));
            this.tbWizard = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.printerCorrectionWelcomeTabPanel1 = new Atum.Studio.Controls.PrinterCorrectionWelcomeTabPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.printerCorrectionFactorTabPanel1 = new Atum.Studio.Controls.PrinterCorrectionFactorTabPanel();
            this.tbWizard.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.printerCorrectionWelcomeTabPanel1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.printerCorrectionFactorTabPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWizard
            // 
            this.tbWizard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWizard.Controls.Add(this.tabPage1);
            this.tbWizard.Controls.Add(this.tabPage2);
            this.tbWizard.Location = new System.Drawing.Point(-4, -2);
            this.tbWizard.Margin = new System.Windows.Forms.Padding(0);
            this.tbWizard.Name = "tbWizard";
            this.tbWizard.SelectedIndex = 0;
            this.tbWizard.Size = new System.Drawing.Size(389, 397);
            this.tbWizard.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.printerCorrectionWelcomeTabPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(381, 368);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // printerCorrectionWelcomeTabPanel1
            // 
            this.printerCorrectionWelcomeTabPanel1.ButtonBackEnabled = true;
            this.printerCorrectionWelcomeTabPanel1.ButtonBackVisible = false;
            this.printerCorrectionWelcomeTabPanel1.ButtonFinished = false;
            this.printerCorrectionWelcomeTabPanel1.ButtonNextEnabled = true;
            this.printerCorrectionWelcomeTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerCorrectionWelcomeTabPanel1.Header
            // 
            this.printerCorrectionWelcomeTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerCorrectionWelcomeTabPanel1.Header.Name = "Header";
            this.printerCorrectionWelcomeTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.printerCorrectionWelcomeTabPanel1.Header.TabIndex = 1;
            this.printerCorrectionWelcomeTabPanel1.Location = new System.Drawing.Point(3, 3);
            this.printerCorrectionWelcomeTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerCorrectionWelcomeTabPanel1.Name = "printerCorrectionWelcomeTabPanel1";
            this.printerCorrectionWelcomeTabPanel1.Size = new System.Drawing.Size(375, 362);
            this.printerCorrectionWelcomeTabPanel1.TabIndex = 0;
            this.printerCorrectionWelcomeTabPanel1.ButtonNext_Click += new System.EventHandler(this.printerCorrectionWelcomeTabPanel1_ButtonNext_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.printerCorrectionFactorTabPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(381, 368);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // printerCorrectionFactorTabPanel1
            // 
            this.printerCorrectionFactorTabPanel1.ButtonBackEnabled = true;
            this.printerCorrectionFactorTabPanel1.ButtonBackVisible = false;
            this.printerCorrectionFactorTabPanel1.ButtonFinished = true;
            this.printerCorrectionFactorTabPanel1.ButtonNextEnabled = true;
            this.printerCorrectionFactorTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerCorrectionFactorTabPanel1.Header
            // 
            this.printerCorrectionFactorTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerCorrectionFactorTabPanel1.Header.Name = "Header";
            this.printerCorrectionFactorTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.printerCorrectionFactorTabPanel1.Header.TabIndex = 1;
            this.printerCorrectionFactorTabPanel1.Location = new System.Drawing.Point(3, 3);
            this.printerCorrectionFactorTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerCorrectionFactorTabPanel1.Name = "printerCorrectionFactorTabPanel1";
            this.printerCorrectionFactorTabPanel1.Size = new System.Drawing.Size(375, 362);
            this.printerCorrectionFactorTabPanel1.TabIndex = 0;
            this.printerCorrectionFactorTabPanel1.ButtonNext_Click += new System.EventHandler(this.printerCorrectionFactorTabPanel1_ButtonNext_Click);
            // 
            // PrinterCorrectionWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 393);
            this.ControlBox = false;
            this.Controls.Add(this.tbWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrinterCorrectionWizard";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.PrinterCorrectionWizard_Load);
            this.tbWizard.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.printerCorrectionWelcomeTabPanel1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.printerCorrectionFactorTabPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbWizard;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private PrinterCorrectionWelcomeTabPanel printerCorrectionWelcomeTabPanel1;
        private PrinterCorrectionFactorTabPanel printerCorrectionFactorTabPanel1;
     
    }
}
