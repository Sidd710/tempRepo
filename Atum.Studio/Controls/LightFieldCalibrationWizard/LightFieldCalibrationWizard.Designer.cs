namespace Atum.Studio.Controls
{
    partial class LightFieldCalibrationWizard
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
            this.tbWizard = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lightFieldCalibrationWelcomeTabPanel1 = new Atum.Studio.Controls.LightFieldCalibrationWelcomeTabPanel();
            this.lightFieldCalibrationCalibrationTabPanel1 = new Atum.Studio.Controls.LightFieldCalibrationCalibrationTabPanel();
            this.tbWizard.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tbWizard
            // 
            this.tbWizard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbWizard.Controls.Add(this.tabPage1);
            this.tbWizard.Controls.Add(this.tabPage2);
            this.tbWizard.Location = new System.Drawing.Point(0, 0);
            this.tbWizard.Margin = new System.Windows.Forms.Padding(0);
            this.tbWizard.Name = "tbWizard";
            this.tbWizard.SelectedIndex = 0;
            this.tbWizard.Size = new System.Drawing.Size(584, 485);
            this.tbWizard.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lightFieldCalibrationWelcomeTabPanel1);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(576, 456);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "tabPage1";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lightFieldCalibrationCalibrationTabPanel1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(576, 456);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lightFieldCalibrationWelcomeTabPanel1
            // 
            this.lightFieldCalibrationWelcomeTabPanel1.ButtonBackEnabled = true;
            this.lightFieldCalibrationWelcomeTabPanel1.ButtonBackVisible = false;
            this.lightFieldCalibrationWelcomeTabPanel1.ButtonFinished = false;
            this.lightFieldCalibrationWelcomeTabPanel1.ButtonNextEnabled = true;
            this.lightFieldCalibrationWelcomeTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightFieldCalibrationWelcomeTabPanel1.Location = new System.Drawing.Point(3, 3);
            this.lightFieldCalibrationWelcomeTabPanel1.Name = "lightFieldCalibrationWelcomeTabPanel1";
            this.lightFieldCalibrationWelcomeTabPanel1.Size = new System.Drawing.Size(570, 450);
            this.lightFieldCalibrationWelcomeTabPanel1.TabIndex = 0;
            this.lightFieldCalibrationWelcomeTabPanel1.ButtonNext_Click += new System.EventHandler(this.lightFieldCalibrationWelcomeTabPanel1_ButtonNext_Click);
            // 
            // lightFieldCalibrationCalibrationTabPanel1
            // 
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonBackEnabled = true;
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonBackVisible = true;
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonFinished = true;
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonNextEnabled = true;
            this.lightFieldCalibrationCalibrationTabPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lightFieldCalibrationCalibrationTabPanel1.Location = new System.Drawing.Point(3, 3);
            this.lightFieldCalibrationCalibrationTabPanel1.Name = "lightFieldCalibrationCalibrationTabPanel1";
            this.lightFieldCalibrationCalibrationTabPanel1.Size = new System.Drawing.Size(570, 450);
            this.lightFieldCalibrationCalibrationTabPanel1.TabIndex = 0;
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonBack_Click += new System.EventHandler(this.lightFieldCalibrationCalibrationTabPanel1_ButtonBack_Click);
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonNext_Click += new System.EventHandler(this.lightFieldCalibrationCalibrationTabPanel1_ButtonNext_Click);
            // 
            // LightFieldCalibrationWizard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(586, 485);
            this.Controls.Add(this.tbWizard);
            this.Name = "LightFieldCalibrationWizard";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Light Field Calibration Wizard";
            this.Load += new System.EventHandler(this.LightFieldCalibrationWizard_Load);
            this.tbWizard.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tbWizard;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private LightFieldCalibrationWelcomeTabPanel lightFieldCalibrationWelcomeTabPanel1;
        private LightFieldCalibrationCalibrationTabPanel lightFieldCalibrationCalibrationTabPanel1;
     
    }
}
