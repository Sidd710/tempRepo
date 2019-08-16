namespace Atum.Studio.Controls.SceneControlActionPanel
{
    partial class SceneControlModelSupportProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SceneControlModelSupportProperties));
            this.lblInformation = new System.Windows.Forms.Label();
            this.plSupportConeOverview = new System.Windows.Forms.Panel();
            this.txtRadiusInput = new Atum.Studio.Controls.FloatInputField14MM();
            this.txtHeightInput = new Atum.Studio.Controls.FloatInputField14MM();
            this.btnRadius = new System.Windows.Forms.Panel();
            this.btnHeight = new System.Windows.Forms.Panel();
            this.btnTop = new Atum.Studio.Controls.NewGui.GenericLabelBold12();
            this.btnMiddle = new Atum.Studio.Controls.NewGui.GenericLabelBold12();
            this.btnBottom = new Atum.Studio.Controls.NewGui.GenericLabelBold12();
            this.btnSetAsDefault = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnApplyToAll = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plContent.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.plContent.Controls.Add(this.btnApplyToAll);
            this.plContent.Controls.Add(this.btnSetAsDefault);
            this.plContent.Controls.Add(this.lblInformation);
            this.plContent.Controls.Add(this.btnTop);
            this.plContent.Controls.Add(this.btnBottom);
            this.plContent.Controls.Add(this.btnMiddle);
            this.plContent.Controls.Add(this.btnHeight);
            this.plContent.Controls.Add(this.btnRadius);
            this.plContent.Controls.Add(this.txtHeightInput);
            this.plContent.Controls.Add(this.txtRadiusInput);
            this.plContent.Controls.Add(this.plSupportConeOverview);
            this.plContent.Size = new System.Drawing.Size(287, 229);
            // 
            // lblInformation
            // 
            this.lblInformation.Location = new System.Drawing.Point(17, 2);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(324, 28);
            this.lblInformation.TabIndex = 14;
            this.lblInformation.Text = "Select the section of the support to edit";
            // 
            // plSupportConeOverview
            // 
            this.plSupportConeOverview.Location = new System.Drawing.Point(18, 24);
            this.plSupportConeOverview.Name = "plSupportConeOverview";
            this.plSupportConeOverview.Size = new System.Drawing.Size(93, 160);
            this.plSupportConeOverview.TabIndex = 15;
            this.plSupportConeOverview.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plSupportConeOverview_MouseDown);
            // 
            // txtRadiusInput
            // 
            this.txtRadiusInput.BackColor = System.Drawing.Color.White;
            this.txtRadiusInput.Location = new System.Drawing.Point(168, 125);
            this.txtRadiusInput.Name = "txtRadiusInput";
            this.txtRadiusInput.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.txtRadiusInput.Size = new System.Drawing.Size(80, 24);
            this.txtRadiusInput.TabIndex = 1;
            // 
            // txtHeightInput
            // 
            this.txtHeightInput.BackColor = System.Drawing.Color.White;
            this.txtHeightInput.Location = new System.Drawing.Point(168, 91);
            this.txtHeightInput.Name = "txtHeightInput";
            this.txtHeightInput.Padding = new System.Windows.Forms.Padding(0, 6, 0, 0);
            this.txtHeightInput.Size = new System.Drawing.Size(80, 24);
            this.txtHeightInput.TabIndex = 0;
            // 
            // btnRadius
            // 
            this.btnRadius.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnRadius.BackgroundImage")));
            this.btnRadius.Location = new System.Drawing.Point(138, 125);
            this.btnRadius.Name = "btnRadius";
            this.btnRadius.Size = new System.Drawing.Size(24, 24);
            this.btnRadius.TabIndex = 18;
            // 
            // btnHeight
            // 
            this.btnHeight.BackgroundImage = global::Atum.Studio.Properties.Resources.btnHeight_24x24;
            this.btnHeight.Location = new System.Drawing.Point(138, 91);
            this.btnHeight.Name = "btnHeight";
            this.btnHeight.Size = new System.Drawing.Size(24, 24);
            this.btnHeight.TabIndex = 19;
            // 
            // btnTop
            // 
            this.btnTop.Location = new System.Drawing.Point(92, 42);
            this.btnTop.Margin = new System.Windows.Forms.Padding(0);
            this.btnTop.Name = "btnTop";
            this.btnTop.Size = new System.Drawing.Size(60, 23);
            this.btnTop.TabIndex = 20;
            this.btnTop.Text = "Top";
            this.btnTop.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnTop.Click += new System.EventHandler(this.btnTop_Click);
            // 
            // btnMiddle
            // 
            this.btnMiddle.Location = new System.Drawing.Point(152, 42);
            this.btnMiddle.Margin = new System.Windows.Forms.Padding(0);
            this.btnMiddle.Name = "btnMiddle";
            this.btnMiddle.Size = new System.Drawing.Size(60, 23);
            this.btnMiddle.TabIndex = 21;
            this.btnMiddle.Text = "Middle";
            this.btnMiddle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnMiddle.Click += new System.EventHandler(this.btnMiddle_Click);
            // 
            // btnBottom
            // 
            this.btnBottom.Location = new System.Drawing.Point(212, 42);
            this.btnBottom.Margin = new System.Windows.Forms.Padding(0);
            this.btnBottom.Name = "btnBottom";
            this.btnBottom.Size = new System.Drawing.Size(60, 23);
            this.btnBottom.TabIndex = 22;
            this.btnBottom.Text = "Bottom";
            this.btnBottom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.btnBottom.Click += new System.EventHandler(this.btnBottom_Click);
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnSetAsDefault.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetAsDefault.FlatAppearance.BorderSize = 0;
            this.btnSetAsDefault.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnSetAsDefault.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSetAsDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.btnSetAsDefault.ForeColor = System.Drawing.Color.White;
            this.btnSetAsDefault.Location = new System.Drawing.Point(153, 181);
            this.btnSetAsDefault.Margin = new System.Windows.Forms.Padding(0, 0, 9, 16);
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Radius = 18;
            this.btnSetAsDefault.SingleBorder = false;
            this.btnSetAsDefault.Size = new System.Drawing.Size(124, 36);
            this.btnSetAsDefault.TabIndex = 25;
            this.btnSetAsDefault.Text = "Set as default";
            this.btnSetAsDefault.UseVisualStyleBackColor = false;
            this.btnSetAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // btnApplyToAll
            // 
            this.btnApplyToAll.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(30)))), ((int)(((byte)(33)))), ((int)(((byte)(37)))));
            this.btnApplyToAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApplyToAll.FlatAppearance.BorderSize = 0;
            this.btnApplyToAll.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnApplyToAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApplyToAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.25F);
            this.btnApplyToAll.ForeColor = System.Drawing.Color.White;
            this.btnApplyToAll.Location = new System.Drawing.Point(9, 181);
            this.btnApplyToAll.Margin = new System.Windows.Forms.Padding(9, 0, 9, 16);
            this.btnApplyToAll.Name = "btnApplyToAll";
            this.btnApplyToAll.Radius = 18;
            this.btnApplyToAll.SingleBorder = false;
            this.btnApplyToAll.Size = new System.Drawing.Size(124, 36);
            this.btnApplyToAll.TabIndex = 26;
            this.btnApplyToAll.Text = "Apply to all";
            this.btnApplyToAll.UseVisualStyleBackColor = false;
            this.btnApplyToAll.Click += new System.EventHandler(this.btnApplyToAll_Click);
            // 
            // SceneControlModelSupportProperties
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SceneControlModelSupportProperties";
            this.Size = new System.Drawing.Size(287, 270);
            this.VisibleChanged += new System.EventHandler(this.SceneControlModelSupportProperties_VisibleChanged);
            this.plContent.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label lblInformation;
        private System.Windows.Forms.Panel plSupportConeOverview;
        private FloatInputField14MM txtHeightInput;
        private FloatInputField14MM txtRadiusInput;
        private System.Windows.Forms.Panel btnRadius;
        private System.Windows.Forms.Panel btnHeight;
        private NewGui.GenericLabelBold12 btnBottom;
        private NewGui.GenericLabelBold12 btnMiddle;
        private NewGui.GenericLabelBold12 btnTop;
        private NewGui.RoundedButton btnSetAsDefault;
        private NewGui.RoundedButton btnApplyToAll;
    }
}
