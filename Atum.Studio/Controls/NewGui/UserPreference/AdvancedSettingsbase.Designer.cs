namespace Atum.Studio.Controls.NewGUI.UserPreference
{
    partial class AdvancedSettingsbase
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
            this.txtSettingText = new Atum.Studio.Controls.NewGUI.NewGUIInputBoxReadonly();
            this.SuspendLayout();
            // 
            // txtSettingText
            // 
            this.txtSettingText.BackColor = System.Drawing.Color.White;
            this.txtSettingText.Location = new System.Drawing.Point(0, 4);
            this.txtSettingText.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtSettingText.Name = "txtSettingText";
            this.txtSettingText.Selected = false;
            this.txtSettingText.Size = new System.Drawing.Size(377, 49);
            this.txtSettingText.TabIndex = 1;
            this.txtSettingText.TabStop = false;
            this.txtSettingText.TextValue = "";
            // 
            // AdvancedSettingsbase
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.txtSettingText);
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "AdvancedSettingsbase";
            this.Size = new System.Drawing.Size(376, 55);
            this.ResumeLayout(false);

        }

        #endregion

        private NewGUIInputBoxReadonly txtSettingText;
    }
}
