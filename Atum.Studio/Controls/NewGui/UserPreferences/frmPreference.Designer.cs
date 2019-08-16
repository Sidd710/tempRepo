using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui.Preference
{
    partial class frmUserPreferences
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserPreferences));
            this.btnApply = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnCancel = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.chkTouchModeEnabled = new System.Windows.Forms.CheckBox();
            this.ilCheckbox = new System.Windows.Forms.ImageList(this.components);
            this.chkBasement = new System.Windows.Forms.CheckBox();
            this.lblPrint = new System.Windows.Forms.Label();
            this.chkSkip = new System.Windows.Forms.CheckBox();
            this.lblGeneral = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.chkUseNumericInputForPositioning = new System.Windows.Forms.CheckBox();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.chkUseNumericInputForPositioning);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Controls.Add(this.btnApply);
            this.plContent.Controls.Add(this.btnCancel);
            this.plContent.Controls.Add(this.chkTouchModeEnabled);
            this.plContent.Controls.Add(this.chkBasement);
            this.plContent.Controls.Add(this.lblPrint);
            this.plContent.Controls.Add(this.chkSkip);
            this.plContent.Controls.Add(this.lblGeneral);
            this.plContent.Size = new System.Drawing.Size(640, 519);
            this.plContent.TabIndex = 0;
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(342, 456);
            this.btnApply.Margin = new System.Windows.Forms.Padding(32, 5, 4, 5);
            this.btnApply.Name = "btnApply";
            this.btnApply.Radius = 20;
            this.btnApply.SingleBorder = false;
            this.btnApply.Size = new System.Drawing.Size(160, 40);
            this.btnApply.TabIndex = 3;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnCancel.Location = new System.Drawing.Point(144, 456);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(8);
            this.btnCancel.Radius = 20;
            this.btnCancel.SingleBorder = false;
            this.btnCancel.Size = new System.Drawing.Size(162, 42);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkTouchModeEnabled
            // 
            this.chkTouchModeEnabled.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkTouchModeEnabled.AutoSize = true;
            this.chkTouchModeEnabled.BackColor = System.Drawing.Color.Transparent;
            this.chkTouchModeEnabled.FlatAppearance.BorderSize = 0;
            this.chkTouchModeEnabled.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkTouchModeEnabled.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkTouchModeEnabled.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkTouchModeEnabled.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkTouchModeEnabled.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkTouchModeEnabled.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkTouchModeEnabled.ImageIndex = 0;
            this.chkTouchModeEnabled.ImageList = this.ilCheckbox;
            this.chkTouchModeEnabled.Location = new System.Drawing.Point(264, 160);
            this.chkTouchModeEnabled.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkTouchModeEnabled.Name = "chkTouchModeEnabled";
            this.chkTouchModeEnabled.Size = new System.Drawing.Size(227, 30);
            this.chkTouchModeEnabled.TabIndex = 2;
            this.chkTouchModeEnabled.Text = " Enable touch interface mode";
            this.chkTouchModeEnabled.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkTouchModeEnabled.UseVisualStyleBackColor = false;
            this.chkTouchModeEnabled.CheckedChanged += new System.EventHandler(this.chkTouchModeEnabled_CheckedChanged);
            // 
            // ilCheckbox
            // 
            this.ilCheckbox.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("ilCheckbox.ImageStream")));
            this.ilCheckbox.TransparentColor = System.Drawing.Color.Transparent;
            this.ilCheckbox.Images.SetKeyName(0, "checkbox-unselected.png");
            this.ilCheckbox.Images.SetKeyName(1, "checkbox-selected.png");
            // 
            // chkBasement
            // 
            this.chkBasement.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkBasement.AutoSize = true;
            this.chkBasement.BackColor = System.Drawing.Color.Transparent;
            this.chkBasement.FlatAppearance.BorderSize = 0;
            this.chkBasement.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkBasement.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkBasement.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkBasement.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkBasement.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkBasement.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBasement.ImageIndex = 0;
            this.chkBasement.ImageList = this.ilCheckbox;
            this.chkBasement.Location = new System.Drawing.Point(264, 105);
            this.chkBasement.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkBasement.Name = "chkBasement";
            this.chkBasement.Size = new System.Drawing.Size(189, 30);
            this.chkBasement.TabIndex = 1;
            this.chkBasement.Text = " Use support basement";
            this.chkBasement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkBasement.UseVisualStyleBackColor = false;
            this.chkBasement.CheckedChanged += new System.EventHandler(this.chkBasement_CheckedChanged);
            // 
            // lblPrint
            // 
            this.lblPrint.AutoSize = true;
            this.lblPrint.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblPrint.Location = new System.Drawing.Point(214, 112);
            this.lblPrint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrint.Name = "lblPrint";
            this.lblPrint.Size = new System.Drawing.Size(42, 17);
            this.lblPrint.TabIndex = 5;
            this.lblPrint.Text = "Print";
            this.lblPrint.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkSkip
            // 
            this.chkSkip.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkSkip.AutoSize = true;
            this.chkSkip.BackColor = System.Drawing.Color.Transparent;
            this.chkSkip.FlatAppearance.BorderSize = 0;
            this.chkSkip.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkSkip.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkSkip.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkSkip.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkSkip.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkSkip.ImageIndex = 0;
            this.chkSkip.ImageList = this.ilCheckbox;
            this.chkSkip.Location = new System.Drawing.Point(264, 50);
            this.chkSkip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSkip.Name = "chkSkip";
            this.chkSkip.Size = new System.Drawing.Size(178, 30);
            this.chkSkip.TabIndex = 0;
            this.chkSkip.Text = " Skip welcome screen";
            this.chkSkip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkSkip.UseVisualStyleBackColor = false;
            this.chkSkip.CheckedChanged += new System.EventHandler(this.chkSkip_CheckedChanged);
            // 
            // lblGeneral
            // 
            this.lblGeneral.AutoSize = true;
            this.lblGeneral.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblGeneral.Location = new System.Drawing.Point(190, 57);
            this.lblGeneral.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeneral.Name = "lblGeneral";
            this.lblGeneral.Size = new System.Drawing.Size(66, 17);
            this.lblGeneral.TabIndex = 0;
            this.lblGeneral.Text = "General";
            this.lblGeneral.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(145, 167);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(111, 17);
            this.label1.TabIndex = 13;
            this.label1.Text = "User Interface";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // chkUseNumericInputForPositioning
            // 
            this.chkUseNumericInputForPositioning.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkUseNumericInputForPositioning.AutoSize = true;
            this.chkUseNumericInputForPositioning.BackColor = System.Drawing.Color.Transparent;
            this.chkUseNumericInputForPositioning.FlatAppearance.BorderSize = 0;
            this.chkUseNumericInputForPositioning.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkUseNumericInputForPositioning.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkUseNumericInputForPositioning.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkUseNumericInputForPositioning.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkUseNumericInputForPositioning.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkUseNumericInputForPositioning.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkUseNumericInputForPositioning.ImageIndex = 0;
            this.chkUseNumericInputForPositioning.ImageList = this.ilCheckbox;
            this.chkUseNumericInputForPositioning.Location = new System.Drawing.Point(264, 200);
            this.chkUseNumericInputForPositioning.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkUseNumericInputForPositioning.Name = "chkUseNumericInputForPositioning";
            this.chkUseNumericInputForPositioning.Size = new System.Drawing.Size(253, 30);
            this.chkUseNumericInputForPositioning.TabIndex = 14;
            this.chkUseNumericInputForPositioning.Text = " Use numeric input for positioning";
            this.chkUseNumericInputForPositioning.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkUseNumericInputForPositioning.UseVisualStyleBackColor = false;
            this.chkUseNumericInputForPositioning.CheckedChanged += new System.EventHandler(this.chkUseNumericInputForPositioning_CheckedChanged);
            // 
            // frmUserPreferences
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(640, 576);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmUserPreferences";
            this.Text = "Preferences   ";
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.CheckBox chkSkip;
        private System.Windows.Forms.Label lblGeneral;
        private System.Windows.Forms.CheckBox chkTouchModeEnabled;
        private System.Windows.Forms.CheckBox chkBasement;
        private System.Windows.Forms.Label lblPrint;
        private RoundedButton btnCancel;
        private RoundedButton btnApply;
        private System.Windows.Forms.ImageList ilCheckbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUseNumericInputForPositioning;
    }
}