using Atum.Studio.Controls.WifiWizard;
namespace Atum.Studio.Controls.WifiWizard
{
    partial class WifiWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WifiWizard));
            this.tbWizard = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.wifiWelcomeTabPanel1 = new Atum.Studio.Controls.WifiWizard.WifiWelcomeTabPanel();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.wifiSettingsTabPanel1 = new Atum.Studio.Controls.WifiWizard.WifiSettingsTabPanel();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.wifiSaveConfigurationToUSB1 = new Atum.Studio.Controls.WifiWizard.WifiSaveConfigurationToUSBTabPanel();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.wifiUpdatePrinterTabPanel1 = new Atum.Studio.Controls.WifiWizard.WifiUpdatePrinterTabPanel();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.wifiFinishedTabPanel1 = new Atum.Studio.Controls.WifiWizard.WifiFinishedTabPanel();
            this.tbWizard.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWizard
            // 
            this.tbWizard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWizard.Controls.Add(this.tabPage1);
            this.tbWizard.Controls.Add(this.tabPage2);
            this.tbWizard.Controls.Add(this.tabPage4);
            this.tbWizard.Controls.Add(this.tabPage5);
            this.tbWizard.Controls.Add(this.tabPage3);
            this.tbWizard.Location = new System.Drawing.Point(-4, -2);
            this.tbWizard.Margin = new System.Windows.Forms.Padding(0);
            this.tbWizard.Name = "tbWizard";
            this.tbWizard.SelectedIndex = 0;
            this.tbWizard.Size = new System.Drawing.Size(558, 489);
            this.tbWizard.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.wifiWelcomeTabPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(550, 460);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Welcome";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // wifiWelcomeTabPanel1
            // 
            this.wifiWelcomeTabPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wifiWelcomeTabPanel1.ButtonBackEnabled = true;
            this.wifiWelcomeTabPanel1.ButtonBackVisible = false;
            this.wifiWelcomeTabPanel1.ButtonFinished = false;
            this.wifiWelcomeTabPanel1.ButtonNextEnabled = true;
            this.wifiWelcomeTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // wifiWelcomeTabPanel1.Header
            // 
            this.wifiWelcomeTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.wifiWelcomeTabPanel1.Header.Name = "Header";
            this.wifiWelcomeTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.wifiWelcomeTabPanel1.Header.TabIndex = 1;
            this.wifiWelcomeTabPanel1.HideFooter = false;
            this.wifiWelcomeTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.wifiWelcomeTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.wifiWelcomeTabPanel1.Name = "wifiWelcomeTabPanel1";
            this.wifiWelcomeTabPanel1.Size = new System.Drawing.Size(550, 460);
            this.wifiWelcomeTabPanel1.TabIndex = 0;
            this.wifiWelcomeTabPanel1.ButtonBack_Click += new System.EventHandler(this.wifiWelcomeTabPanel1_ButtonBack_Click);
            this.wifiWelcomeTabPanel1.ButtonNext_Click += new System.EventHandler(this.wifiWelcomeTabPanel1_ButtonNext_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.wifiSettingsTabPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Size = new System.Drawing.Size(550, 460);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Settings";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // wifiSettingsTabPanel1
            // 
            this.wifiSettingsTabPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wifiSettingsTabPanel1.ButtonBackEnabled = true;
            this.wifiSettingsTabPanel1.ButtonBackVisible = false;
            this.wifiSettingsTabPanel1.ButtonFinished = false;
            this.wifiSettingsTabPanel1.ButtonNextEnabled = true;
            this.wifiSettingsTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // wifiSettingsTabPanel1.Header
            // 
            this.wifiSettingsTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.wifiSettingsTabPanel1.Header.Name = "Header";
            this.wifiSettingsTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.wifiSettingsTabPanel1.Header.TabIndex = 1;
            this.wifiSettingsTabPanel1.HideFooter = true;
            this.wifiSettingsTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.wifiSettingsTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.wifiSettingsTabPanel1.Name = "wifiSettingsTabPanel1";
            this.wifiSettingsTabPanel1.Size = new System.Drawing.Size(550, 460);
            this.wifiSettingsTabPanel1.TabIndex = 0;
            this.wifiSettingsTabPanel1.ButtonBack_Click += new System.EventHandler(this.wifiSettingsTabPanel1_ButtonBack_Click);
            this.wifiSettingsTabPanel1.ButtonNext_Click += new System.EventHandler(this.wifiSettingsTabPanel1_ButtonNext_Click);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.wifiSaveConfigurationToUSB1);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(550, 460);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "SaveConfiguration";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // wifiSaveConfigurationToUSB1
            // 
            this.wifiSaveConfigurationToUSB1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wifiSaveConfigurationToUSB1.ButtonBackEnabled = true;
            this.wifiSaveConfigurationToUSB1.ButtonBackVisible = false;
            this.wifiSaveConfigurationToUSB1.ButtonFinished = false;
            this.wifiSaveConfigurationToUSB1.ButtonNextEnabled = false;
            this.wifiSaveConfigurationToUSB1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // wifiSaveConfigurationToUSB1.Header
            // 
            this.wifiSaveConfigurationToUSB1.Header.Location = new System.Drawing.Point(0, 0);
            this.wifiSaveConfigurationToUSB1.Header.Name = "Header";
            this.wifiSaveConfigurationToUSB1.Header.Size = new System.Drawing.Size(200, 100);
            this.wifiSaveConfigurationToUSB1.Header.TabIndex = 1;
            this.wifiSaveConfigurationToUSB1.HideFooter = true;
            this.wifiSaveConfigurationToUSB1.Location = new System.Drawing.Point(0, 0);
            this.wifiSaveConfigurationToUSB1.Margin = new System.Windows.Forms.Padding(0);
            this.wifiSaveConfigurationToUSB1.Name = "wifiSaveConfigurationToUSB1";
            this.wifiSaveConfigurationToUSB1.Size = new System.Drawing.Size(550, 460);
            this.wifiSaveConfigurationToUSB1.TabIndex = 0;
            this.wifiSaveConfigurationToUSB1.ButtonBack_Click += new System.EventHandler(this.wifiSaveConfigurationToUSB1_ButtonBack_Click);
            this.wifiSaveConfigurationToUSB1.ButtonNext_Click += new System.EventHandler(this.wifiSaveConfigurationToUSB1_ButtonNext_Click);
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.wifiUpdatePrinterTabPanel1);
            this.tabPage5.Location = new System.Drawing.Point(4, 25);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Size = new System.Drawing.Size(550, 460);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "InsertIntoPrinter";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // wifiUpdatePrinterTabPanel1
            // 
            this.wifiUpdatePrinterTabPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wifiUpdatePrinterTabPanel1.ButtonBackEnabled = true;
            this.wifiUpdatePrinterTabPanel1.ButtonBackVisible = false;
            this.wifiUpdatePrinterTabPanel1.ButtonFinished = false;
            this.wifiUpdatePrinterTabPanel1.ButtonNextEnabled = true;
            this.wifiUpdatePrinterTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // wifiUpdatePrinterTabPanel1.Header
            // 
            this.wifiUpdatePrinterTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.wifiUpdatePrinterTabPanel1.Header.Name = "Header";
            this.wifiUpdatePrinterTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.wifiUpdatePrinterTabPanel1.Header.TabIndex = 1;
            this.wifiUpdatePrinterTabPanel1.HideFooter = true;
            this.wifiUpdatePrinterTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.wifiUpdatePrinterTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.wifiUpdatePrinterTabPanel1.Name = "wifiUpdatePrinterTabPanel1";
            this.wifiUpdatePrinterTabPanel1.Size = new System.Drawing.Size(550, 460);
            this.wifiUpdatePrinterTabPanel1.TabIndex = 0;
            this.wifiUpdatePrinterTabPanel1.ButtonBack_Click += new System.EventHandler(this.wifiUpdatePrinterTabPanel1_ButtonBack_Click);
            this.wifiUpdatePrinterTabPanel1.ButtonNext_Click += new System.EventHandler(this.wifiUpdatePrinterTabPanel1_ButtonNext_Click);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.wifiFinishedTabPanel1);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(0);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(550, 460);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Finish";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // wifiFinishedTabPanel1
            // 
            this.wifiFinishedTabPanel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wifiFinishedTabPanel1.ButtonBackEnabled = true;
            this.wifiFinishedTabPanel1.ButtonBackVisible = false;
            this.wifiFinishedTabPanel1.ButtonFinished = true;
            this.wifiFinishedTabPanel1.ButtonNextEnabled = true;
            this.wifiFinishedTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // wifiFinishedTabPanel1.Header
            // 
            this.wifiFinishedTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.wifiFinishedTabPanel1.Header.Name = "Header";
            this.wifiFinishedTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.wifiFinishedTabPanel1.Header.TabIndex = 1;
            this.wifiFinishedTabPanel1.HideFooter = true;
            this.wifiFinishedTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.wifiFinishedTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.wifiFinishedTabPanel1.Name = "wifiFinishedTabPanel1";
            this.wifiFinishedTabPanel1.Size = new System.Drawing.Size(550, 460);
            this.wifiFinishedTabPanel1.TabIndex = 0;
            this.wifiFinishedTabPanel1.ButtonBack_Click += new System.EventHandler(this.wifiFinishedTabPanel1_ButtonBack_Click);
            this.wifiFinishedTabPanel1.ButtonNext_Click += new System.EventHandler(this.wifiFinishedTabPanel1_ButtonNext_Click);
            // 
            // WifiWizard
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(555, 485);
            this.Controls.Add(this.tbWizard);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "WifiWizard";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Load += new System.EventHandler(this.WifiWizard_Load);
            this.tbWizard.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbWizard;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private WifiSettingsTabPanel wifiSettingsTabPanel1;
        private WifiWelcomeTabPanel wifiWelcomeTabPanel1;
        private System.Windows.Forms.TabPage tabPage3;
        private WifiFinishedTabPanel wifiFinishedTabPanel1;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TabPage tabPage5;
        private WifiSaveConfigurationToUSBTabPanel wifiSaveConfigurationToUSB1;
        private WifiUpdatePrinterTabPanel wifiUpdatePrinterTabPanel1;

    }
}