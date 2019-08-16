namespace Atum.Studio.Controls.NewGUI.UserPreference
{
    partial class frmUserPreference
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmPreference));
            this.lblHeader = new System.Windows.Forms.Label();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.plContent = new System.Windows.Forms.Panel();
            this.btnApply = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.pbPlatform = new System.Windows.Forms.PictureBox();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.chkLiftModel = new System.Windows.Forms.CheckBox();
            this.ilCheckbox = new System.Windows.Forms.ImageList(this.components);
            this.chkBasement = new System.Windows.Forms.CheckBox();
            this.lblPrint = new System.Windows.Forms.Label();
            this.chkXyz = new System.Windows.Forms.CheckBox();
            this.chkAnnotation = new System.Windows.Forms.CheckBox();
            this.lblSelection = new System.Windows.Forms.Label();
            this.chkSkip = new System.Windows.Forms.CheckBox();
            this.lblGeneral = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlatform)).BeginInit();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Font = new System.Drawing.Font("Montserrat", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Padding = new System.Windows.Forms.Padding(40, 0, 0, 0);
            this.lblHeader.Size = new System.Drawing.Size(720, 70);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Preferences";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnClose
            // 
            this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnClose.Image = global::Atum.Studio.Properties.Resources.cross_white;
            this.btnClose.Location = new System.Drawing.Point(675, 20);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.TabIndex = 2;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plContent.Controls.Add(this.btnApply);
            this.plContent.Controls.Add(this.btnCancel);
            this.plContent.Controls.Add(this.pbPlatform);
            this.plContent.Controls.Add(this.lblPlatform);
            this.plContent.Controls.Add(this.chkLiftModel);
            this.plContent.Controls.Add(this.chkBasement);
            this.plContent.Controls.Add(this.lblPrint);
            this.plContent.Controls.Add(this.chkXyz);
            this.plContent.Controls.Add(this.chkAnnotation);
            this.plContent.Controls.Add(this.lblSelection);
            this.plContent.Controls.Add(this.chkSkip);
            this.plContent.Controls.Add(this.lblGeneral);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.plContent.Location = new System.Drawing.Point(0, 70);
            this.plContent.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(720, 650);
            this.plContent.TabIndex = 3;
            // 
            // btnApply
            // 
            this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnApply.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(378, 570);
            this.btnApply.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnApply.Name = "btnApply";
            this.btnApply.Size = new System.Drawing.Size(180, 50);
            this.btnApply.TabIndex = 12;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.BackColor = System.Drawing.Color.White;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnCancel.Location = new System.Drawing.Point(162, 570);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(8, 8, 8, 8);
            this.btnCancel.Size = new System.Drawing.Size(180, 50);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // pbPlatform
            // 
            this.pbPlatform.Location = new System.Drawing.Point(309, 352);
            this.pbPlatform.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pbPlatform.Name = "pbPlatform";
            this.pbPlatform.Size = new System.Drawing.Size(306, 160);
            this.pbPlatform.TabIndex = 9;
            this.pbPlatform.TabStop = false;
            // 
            // lblPlatform
            // 
            this.lblPlatform.AutoSize = true;
            this.lblPlatform.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblPlatform.Location = new System.Drawing.Point(144, 352);
            this.lblPlatform.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(152, 21);
            this.lblPlatform.TabIndex = 8;
            this.lblPlatform.Text = "Origin of platform";
            // 
            // chkLiftModel
            // 
            this.chkLiftModel.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkLiftModel.AutoSize = true;
            this.chkLiftModel.BackColor = System.Drawing.Color.Transparent;
            this.chkLiftModel.FlatAppearance.BorderSize = 0;
            this.chkLiftModel.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkLiftModel.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkLiftModel.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkLiftModel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkLiftModel.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkLiftModel.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkLiftModel.ImageIndex = 0;
            this.chkLiftModel.ImageList = this.ilCheckbox;
            this.chkLiftModel.Location = new System.Drawing.Point(309, 278);
            this.chkLiftModel.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkLiftModel.Name = "chkLiftModel";
            this.chkLiftModel.Size = new System.Drawing.Size(205, 31);
            this.chkLiftModel.TabIndex = 7;
            this.chkLiftModel.Text = "Lift model on support";
            this.chkLiftModel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkLiftModel.UseVisualStyleBackColor = false;
            this.chkLiftModel.CheckedChanged += new System.EventHandler(this.chkLiftModel_CheckedChanged);
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
            this.chkBasement.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkBasement.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkBasement.ImageIndex = 0;
            this.chkBasement.ImageList = this.ilCheckbox;
            this.chkBasement.Location = new System.Drawing.Point(309, 238);
            this.chkBasement.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkBasement.Name = "chkBasement";
            this.chkBasement.Size = new System.Drawing.Size(214, 31);
            this.chkBasement.TabIndex = 6;
            this.chkBasement.Text = "Use support basement";
            this.chkBasement.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkBasement.UseVisualStyleBackColor = false;
            this.chkBasement.CheckedChanged += new System.EventHandler(this.chkBasement_CheckedChanged);
            // 
            // lblPrint
            // 
            this.lblPrint.AutoSize = true;
            this.lblPrint.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblPrint.Location = new System.Drawing.Point(250, 241);
            this.lblPrint.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblPrint.Name = "lblPrint";
            this.lblPrint.Size = new System.Drawing.Size(49, 21);
            this.lblPrint.TabIndex = 5;
            this.lblPrint.Text = "Print";
            // 
            // chkXyz
            // 
            this.chkXyz.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkXyz.AutoSize = true;
            this.chkXyz.BackColor = System.Drawing.Color.Transparent;
            this.chkXyz.FlatAppearance.BorderSize = 0;
            this.chkXyz.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkXyz.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkXyz.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkXyz.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkXyz.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkXyz.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkXyz.ImageIndex = 0;
            this.chkXyz.ImageList = this.ilCheckbox;
            this.chkXyz.Location = new System.Drawing.Point(309, 168);
            this.chkXyz.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkXyz.Name = "chkXyz";
            this.chkXyz.Size = new System.Drawing.Size(108, 31);
            this.chkXyz.TabIndex = 4;
            this.chkXyz.Text = "XYZ Axis";
            this.chkXyz.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkXyz.UseVisualStyleBackColor = false;
            this.chkXyz.CheckedChanged += new System.EventHandler(this.chkXyz_CheckedChanged);
            // 
            // chkAnnotation
            // 
            this.chkAnnotation.Appearance = System.Windows.Forms.Appearance.Button;
            this.chkAnnotation.AutoSize = true;
            this.chkAnnotation.BackColor = System.Drawing.Color.Transparent;
            this.chkAnnotation.FlatAppearance.BorderSize = 0;
            this.chkAnnotation.FlatAppearance.CheckedBackColor = System.Drawing.Color.Transparent;
            this.chkAnnotation.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.chkAnnotation.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.chkAnnotation.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.chkAnnotation.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkAnnotation.ImageAlign = System.Drawing.ContentAlignment.TopCenter;
            this.chkAnnotation.ImageIndex = 0;
            this.chkAnnotation.ImageList = this.ilCheckbox;
            this.chkAnnotation.Location = new System.Drawing.Point(309, 128);
            this.chkAnnotation.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkAnnotation.Name = "chkAnnotation";
            this.chkAnnotation.Size = new System.Drawing.Size(135, 31);
            this.chkAnnotation.TabIndex = 3;
            this.chkAnnotation.Text = "Annotations";
            this.chkAnnotation.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkAnnotation.UseVisualStyleBackColor = false;
            this.chkAnnotation.CheckedChanged += new System.EventHandler(this.chkAnnotation_CheckedChanged);
            // 
            // lblSelection
            // 
            this.lblSelection.AutoSize = true;
            this.lblSelection.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSelection.Location = new System.Drawing.Point(169, 133);
            this.lblSelection.Margin = new System.Windows.Forms.Padding(0);
            this.lblSelection.Name = "lblSelection";
            this.lblSelection.Size = new System.Drawing.Size(127, 21);
            this.lblSelection.TabIndex = 2;
            this.lblSelection.Text = "View Selection";
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
            this.chkSkip.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.chkSkip.ImageIndex = 0;
            this.chkSkip.ImageList = this.ilCheckbox;
            this.chkSkip.Location = new System.Drawing.Point(309, 58);
            this.chkSkip.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.chkSkip.Name = "chkSkip";
            this.chkSkip.Size = new System.Drawing.Size(262, 31);
            this.chkSkip.TabIndex = 1;
            this.chkSkip.Text = "Skip welcome screen on start";
            this.chkSkip.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.chkSkip.UseVisualStyleBackColor = false;
            this.chkSkip.CheckedChanged += new System.EventHandler(this.chkSkip_CheckedChanged);
            // 
            // lblGeneral
            // 
            this.lblGeneral.AutoSize = true;
            this.lblGeneral.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblGeneral.Location = new System.Drawing.Point(226, 62);
            this.lblGeneral.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblGeneral.Name = "lblGeneral";
            this.lblGeneral.Size = new System.Drawing.Size(70, 21);
            this.lblGeneral.TabIndex = 0;
            this.lblGeneral.Text = "General";
            // 
            // frmPreference
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.ClientSize = new System.Drawing.Size(720, 720);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.plContent);
            this.Controls.Add(this.lblHeader);
            this.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "frmPreference";
            this.Text = "Preferences";
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbPlatform)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.CheckBox chkSkip;
        private System.Windows.Forms.Label lblGeneral;
        private System.Windows.Forms.PictureBox pbPlatform;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.CheckBox chkLiftModel;
        private System.Windows.Forms.CheckBox chkBasement;
        private System.Windows.Forms.Label lblPrint;
        private System.Windows.Forms.CheckBox chkXyz;
        private System.Windows.Forms.CheckBox chkAnnotation;
        private System.Windows.Forms.Label lblSelection;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnApply;
        private System.Windows.Forms.ImageList ilCheckbox;
    }
}