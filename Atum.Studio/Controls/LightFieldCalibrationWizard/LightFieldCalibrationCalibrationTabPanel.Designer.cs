namespace Atum.Studio.Controls
{
    partial class LightFieldCalibrationCalibrationTabPanel
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
            this.plProjectCalibrationPanels = new System.Windows.Forms.Panel();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.plProjectCalibrationPanels);
            this.plContent.Size = new System.Drawing.Size(531, 464);
         
            // 
            // plProjectCalibrationPanels
            // 
            this.plProjectCalibrationPanels.Location = new System.Drawing.Point(24, 30);
            this.plProjectCalibrationPanels.Name = "plProjectCalibrationPanels";
            this.plProjectCalibrationPanels.Size = new System.Drawing.Size(490, 330);
            this.plProjectCalibrationPanels.TabIndex = 0;
            // 
            // LightFieldCalibrationCalibrationTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Name = "LightFieldCalibrationCalibrationTabPanel";
            this.Size = new System.Drawing.Size(531, 546);
            this.Load += new System.EventHandler(this.LightFieldCalibrationCalibrationTabPanel_Load);
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plProjectCalibrationPanels;

    }
}
