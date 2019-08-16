namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionWizard
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterConnectionWizard));
            this.tbWizard = new System.Windows.Forms.TabControl();
            this.tbWelcome = new System.Windows.Forms.TabPage();
            this.printerConnectionWelcomeTabPanel1 = new Atum.Studio.Controls.PrinterConnectionWizard.PrinterConnectionWelcomeTabPanel();
            this.tbPrinterProperties = new System.Windows.Forms.TabPage();
            this.printerConnectionPropertiesTabPanel1 = new Atum.Studio.Controls.PrinterConnectionWizard.PrinterConnectionPropertiesTabPanel();
            this.tbCalibration = new System.Windows.Forms.TabPage();
            this.printerConnectionMaterialTabPanel1 = new Atum.Studio.Controls.PrinterConnectionWizard.PrinterConnectionMaterialTabPanel();
            this.tbAdjust = new System.Windows.Forms.TabPage();
            this.printerConnectionAdjustTabPanel1 = new Atum.Studio.Controls.PrinterConnectionWizard.PrinterConnectionAdjustTabPanel();
            this.tbFinish = new System.Windows.Forms.TabPage();
            this.printerConnectionFinishedTabPanel1 = new Atum.Studio.Controls.PrinterConnectionWizard.PrinterConnectionFinishedTabPanel();
            this.tbWizard.SuspendLayout();
            this.tbWelcome.SuspendLayout();
            this.tbPrinterProperties.SuspendLayout();
            this.tbCalibration.SuspendLayout();
            this.tbAdjust.SuspendLayout();
            this.tbFinish.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWizard
            // 
            this.tbWizard.Controls.Add(this.tbWelcome);
            this.tbWizard.Controls.Add(this.tbPrinterProperties);
            this.tbWizard.Controls.Add(this.tbCalibration);
            this.tbWizard.Controls.Add(this.tbAdjust);
            this.tbWizard.Controls.Add(this.tbFinish);
            this.tbWizard.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tbWizard.Location = new System.Drawing.Point(0, 0);
            this.tbWizard.Margin = new System.Windows.Forms.Padding(0);
            this.tbWizard.Name = "tbWizard";
            this.tbWizard.SelectedIndex = 0;
            this.tbWizard.Size = new System.Drawing.Size(447, 534);
            this.tbWizard.TabIndex = 1;
            this.tbWizard.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tbWizard_Selecting);
            // 
            // tbWelcome
            // 
            this.tbWelcome.Controls.Add(this.printerConnectionWelcomeTabPanel1);
            this.tbWelcome.Location = new System.Drawing.Point(4, 22);
            this.tbWelcome.Margin = new System.Windows.Forms.Padding(0);
            this.tbWelcome.Name = "tbWelcome";
            this.tbWelcome.Size = new System.Drawing.Size(439, 508);
            this.tbWelcome.TabIndex = 0;
            this.tbWelcome.Text = "Welcome";
            this.tbWelcome.UseVisualStyleBackColor = true;
            // 
            // printerConnectionWelcomeTabPanel1
            // 
            this.printerConnectionWelcomeTabPanel1.ButtonBackEnabled = true;
            this.printerConnectionWelcomeTabPanel1.ButtonBackVisible = false;
            this.printerConnectionWelcomeTabPanel1.ButtonFinished = false;
            this.printerConnectionWelcomeTabPanel1.ButtonNextEnabled = true;
            this.printerConnectionWelcomeTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerConnectionWelcomeTabPanel1.Header
            // 
            this.printerConnectionWelcomeTabPanel1.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printerConnectionWelcomeTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionWelcomeTabPanel1.Header.Name = "Header";
            this.printerConnectionWelcomeTabPanel1.Header.Size = new System.Drawing.Size(439, 55);
            this.printerConnectionWelcomeTabPanel1.Header.TabIndex = 1;
            this.printerConnectionWelcomeTabPanel1.HideFooter = true;
            this.printerConnectionWelcomeTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionWelcomeTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerConnectionWelcomeTabPanel1.Name = "printerConnectionWelcomeTabPanel1";
            this.printerConnectionWelcomeTabPanel1.Size = new System.Drawing.Size(439, 508);
            this.printerConnectionWelcomeTabPanel1.TabIndex = 0;
            this.printerConnectionWelcomeTabPanel1.ButtonNext_Click += new System.EventHandler(this.DefaultNextClick);
            // 
            // tbPrinterProperties
            // 
            this.tbPrinterProperties.Controls.Add(this.printerConnectionPropertiesTabPanel1);
            this.tbPrinterProperties.Location = new System.Drawing.Point(4, 22);
            this.tbPrinterProperties.Margin = new System.Windows.Forms.Padding(0);
            this.tbPrinterProperties.Name = "tbPrinterProperties";
            this.tbPrinterProperties.Size = new System.Drawing.Size(439, 508);
            this.tbPrinterProperties.TabIndex = 3;
            this.tbPrinterProperties.Text = "Properties";
            this.tbPrinterProperties.UseVisualStyleBackColor = true;
            // 
            // printerConnectionPropertiesTabPanel1
            // 
            this.printerConnectionPropertiesTabPanel1.ButtonBackEnabled = true;
            this.printerConnectionPropertiesTabPanel1.ButtonBackVisible = false;
            this.printerConnectionPropertiesTabPanel1.ButtonFinished = false;
            this.printerConnectionPropertiesTabPanel1.ButtonNextEnabled = true;
            this.printerConnectionPropertiesTabPanel1.DataSource = null;
            this.printerConnectionPropertiesTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerConnectionPropertiesTabPanel1.Header
            // 
            this.printerConnectionPropertiesTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionPropertiesTabPanel1.Header.Name = "Header";
            this.printerConnectionPropertiesTabPanel1.Header.Size = new System.Drawing.Size(443, 53);
            this.printerConnectionPropertiesTabPanel1.Header.TabIndex = 1;
            this.printerConnectionPropertiesTabPanel1.HideFooter = true;
            this.printerConnectionPropertiesTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionPropertiesTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerConnectionPropertiesTabPanel1.Name = "printerConnectionPropertiesTabPanel1";
            this.printerConnectionPropertiesTabPanel1.Size = new System.Drawing.Size(439, 508);
            this.printerConnectionPropertiesTabPanel1.TabIndex = 0;
            this.printerConnectionPropertiesTabPanel1.ButtonBack_Click += new System.EventHandler(this.DefaultBackClick);
            this.printerConnectionPropertiesTabPanel1.ButtonNext_Click += new System.EventHandler(this.DefaultNextClick);
            // 
            // tbCalibration
            // 
            this.tbCalibration.Controls.Add(this.printerConnectionMaterialTabPanel1);
            this.tbCalibration.Location = new System.Drawing.Point(4, 22);
            this.tbCalibration.Name = "tbCalibration";
            this.tbCalibration.Size = new System.Drawing.Size(439, 508);
            this.tbCalibration.TabIndex = 4;
            this.tbCalibration.Text = "Calibration";
            this.tbCalibration.UseVisualStyleBackColor = true;
            // 
            // printerConnectionMaterialTabPanel1
            // 
            this.printerConnectionMaterialTabPanel1.ButtonBackEnabled = true;
            this.printerConnectionMaterialTabPanel1.ButtonBackVisible = false;
            this.printerConnectionMaterialTabPanel1.ButtonFinished = false;
            this.printerConnectionMaterialTabPanel1.ButtonNextEnabled = false;
            this.printerConnectionMaterialTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerConnectionMaterialTabPanel1.Header
            // 
            this.printerConnectionMaterialTabPanel1.Header.BackColor = System.Drawing.Color.White;
            this.printerConnectionMaterialTabPanel1.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printerConnectionMaterialTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionMaterialTabPanel1.Header.Name = "Header";
            this.printerConnectionMaterialTabPanel1.Header.Size = new System.Drawing.Size(439, 55);
            this.printerConnectionMaterialTabPanel1.Header.TabIndex = 1;
            this.printerConnectionMaterialTabPanel1.HideFooter = true;
            this.printerConnectionMaterialTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionMaterialTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerConnectionMaterialTabPanel1.Name = "printerConnectionMaterialTabPanel1";
            this.printerConnectionMaterialTabPanel1.Size = new System.Drawing.Size(439, 508);
            this.printerConnectionMaterialTabPanel1.TabIndex = 0;
            this.printerConnectionMaterialTabPanel1.ButtonBack_Click += new System.EventHandler(this.DefaultBackClick);
            this.printerConnectionMaterialTabPanel1.ButtonNext_Click += new System.EventHandler(this.DefaultNextClick);
            // 
            // tbAdjust
            // 
            this.tbAdjust.Controls.Add(this.printerConnectionAdjustTabPanel1);
            this.tbAdjust.Location = new System.Drawing.Point(4, 22);
            this.tbAdjust.Name = "tbAdjust";
            this.tbAdjust.Size = new System.Drawing.Size(439, 508);
            this.tbAdjust.TabIndex = 5;
            this.tbAdjust.Text = "Adjust";
            this.tbAdjust.UseVisualStyleBackColor = true;
            // 
            // printerConnectionAdjustTabPanel1
            // 
            this.printerConnectionAdjustTabPanel1.ButtonBackEnabled = true;
            this.printerConnectionAdjustTabPanel1.ButtonBackVisible = false;
            this.printerConnectionAdjustTabPanel1.ButtonFinished = false;
            this.printerConnectionAdjustTabPanel1.ButtonNextEnabled = true;
            this.printerConnectionAdjustTabPanel1.DataSource = null;
            this.printerConnectionAdjustTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerConnectionAdjustTabPanel1.Header
            // 
            this.printerConnectionAdjustTabPanel1.Header.BackColor = System.Drawing.Color.White;
            this.printerConnectionAdjustTabPanel1.Header.Dock = System.Windows.Forms.DockStyle.Fill;
            this.printerConnectionAdjustTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionAdjustTabPanel1.Header.Name = "Header";
            this.printerConnectionAdjustTabPanel1.Header.Size = new System.Drawing.Size(439, 55);
            this.printerConnectionAdjustTabPanel1.Header.TabIndex = 1;
            this.printerConnectionAdjustTabPanel1.HideFooter = true;
            this.printerConnectionAdjustTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionAdjustTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerConnectionAdjustTabPanel1.Name = "printerConnectionAdjustTabPanel1";
            this.printerConnectionAdjustTabPanel1.Size = new System.Drawing.Size(439, 508);
            this.printerConnectionAdjustTabPanel1.TabIndex = 0;
            this.printerConnectionAdjustTabPanel1.ButtonBack_Click += new System.EventHandler(this.DefaultBackClick);
            this.printerConnectionAdjustTabPanel1.ButtonNext_Click += new System.EventHandler(this.DefaultNextClick);
            // 
            // tbFinish
            // 
            this.tbFinish.Controls.Add(this.printerConnectionFinishedTabPanel1);
            this.tbFinish.Location = new System.Drawing.Point(4, 22);
            this.tbFinish.Margin = new System.Windows.Forms.Padding(0);
            this.tbFinish.Name = "tbFinish";
            this.tbFinish.Size = new System.Drawing.Size(439, 508);
            this.tbFinish.TabIndex = 2;
            this.tbFinish.Text = "Finish";
            this.tbFinish.UseVisualStyleBackColor = true;
            // 
            // printerConnectionFinishedTabPanel1
            // 
            this.printerConnectionFinishedTabPanel1.ButtonBackEnabled = true;
            this.printerConnectionFinishedTabPanel1.ButtonBackVisible = false;
            this.printerConnectionFinishedTabPanel1.ButtonFinished = true;
            this.printerConnectionFinishedTabPanel1.ButtonNextEnabled = true;
            this.printerConnectionFinishedTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            // 
            // printerConnectionFinishedTabPanel1.Header
            // 
            this.printerConnectionFinishedTabPanel1.Header.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionFinishedTabPanel1.Header.Name = "Header";
            this.printerConnectionFinishedTabPanel1.Header.Size = new System.Drawing.Size(200, 100);
            this.printerConnectionFinishedTabPanel1.Header.TabIndex = 1;
            this.printerConnectionFinishedTabPanel1.HideFooter = true;
            this.printerConnectionFinishedTabPanel1.Location = new System.Drawing.Point(0, 0);
            this.printerConnectionFinishedTabPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.printerConnectionFinishedTabPanel1.Name = "printerConnectionFinishedTabPanel1";
            this.printerConnectionFinishedTabPanel1.Size = new System.Drawing.Size(439, 508);
            this.printerConnectionFinishedTabPanel1.TabIndex = 0;
            this.printerConnectionFinishedTabPanel1.ButtonBack_Click += new System.EventHandler(this.DefaultBackClick);
            this.printerConnectionFinishedTabPanel1.ButtonNext_Click += new System.EventHandler(this.printerConnectionFinishedTabPanel1_ButtonNext_Click);
            // 
            // PrinterConnectionWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(447, 534);
            this.Controls.Add(this.tbWizard);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "PrinterConnectionWizard";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Add Printer Wizard";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.PrinterConnectionWizard_FormClosing);
            this.Load += new System.EventHandler(this.PrinterConnectionWizard_Load);
            this.tbWizard.ResumeLayout(false);
            this.tbWelcome.ResumeLayout(false);
            this.tbPrinterProperties.ResumeLayout(false);
            this.tbCalibration.ResumeLayout(false);
            this.tbAdjust.ResumeLayout(false);
            this.tbFinish.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbWizard;
        private System.Windows.Forms.TabPage tbWelcome;
        private System.Windows.Forms.TabPage tbFinish;
        private PrinterConnectionWelcomeTabPanel printerConnectionWelcomeTabPanel1;
        private PrinterConnectionFinishedTabPanel printerConnectionFinishedTabPanel1;
        private System.Windows.Forms.TabPage tbPrinterProperties;
        private PrinterConnectionPropertiesTabPanel printerConnectionPropertiesTabPanel1;
        private System.Windows.Forms.TabPage tbCalibration;
        private PrinterConnectionMaterialTabPanel printerConnectionMaterialTabPanel1;
        private System.Windows.Forms.TabPage tbAdjust;
        private PrinterConnectionAdjustTabPanel printerConnectionAdjustTabPanel1;
    }
}
