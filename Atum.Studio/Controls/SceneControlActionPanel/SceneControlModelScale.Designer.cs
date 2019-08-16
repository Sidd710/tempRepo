namespace Atum.Studio.Controls.SceneControlActionPanel
{
    partial class SceneControlModelScale
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
            this.plMoreOptions = new System.Windows.Forms.Panel();
            this.plSlider = new System.Windows.Forms.Panel();
            this.plUniformScaleFactor = new System.Windows.Forms.Panel();
            this.txtUniformScaleFactor = new Atum.Studio.Controls.FloatInputField14();
            this.genericLabelRegular141 = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtXValue = new Atum.Studio.Controls.FloatInputField14();
            this.panel3 = new System.Windows.Forms.Panel();
            this.txtYValue = new Atum.Studio.Controls.FloatInputField14();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtZValue = new Atum.Studio.Controls.FloatInputField14();
            this.lblX = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.lblY = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.lblZ = new Atum.Studio.Controls.NewGui.GenericLabelBold14();
            this.btnUniformScaleLink = new System.Windows.Forms.Panel();
            this.plLinkedLines = new System.Windows.Forms.Panel();
            this.lblMoreOptions = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.lblXValue = new System.Windows.Forms.Panel();
            this.btnXTypeDropdown = new System.Windows.Forms.Panel();
            this.lblXValueType = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.lblYValue = new System.Windows.Forms.Panel();
            this.btnYTypeDropdown = new System.Windows.Forms.Panel();
            this.lblYValueType = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.lblZValue = new System.Windows.Forms.Panel();
            this.btnZTypeDropdown = new System.Windows.Forms.Panel();
            this.lblZValueType = new Atum.Studio.Controls.NewGui.GenericLabelRegular14();
            this.btnApply = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plFooter = new System.Windows.Forms.Panel();
            this.plContent.SuspendLayout();
            this.plSlider.SuspendLayout();
            this.plUniformScaleFactor.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.plLinkedLines.SuspendLayout();
            this.lblXValue.SuspendLayout();
            this.lblYValue.SuspendLayout();
            this.lblZValue.SuspendLayout();
            this.plFooter.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.plContent.Controls.Add(this.plFooter);
            this.plContent.Controls.Add(this.lblZValue);
            this.plContent.Controls.Add(this.lblYValue);
            this.plContent.Controls.Add(this.lblXValue);
            this.plContent.Controls.Add(this.lblMoreOptions);
            this.plContent.Controls.Add(this.plLinkedLines);
            this.plContent.Controls.Add(this.lblZ);
            this.plContent.Controls.Add(this.lblY);
            this.plContent.Controls.Add(this.lblX);
            this.plContent.Controls.Add(this.panel3);
            this.plContent.Controls.Add(this.panel4);
            this.plContent.Controls.Add(this.panel2);
            this.plContent.Controls.Add(this.plMoreOptions);
            this.plContent.Controls.Add(this.plSlider);
            this.plContent.Size = new System.Drawing.Size(335, 357);
            // 
            // plMoreOptions
            // 
            this.plMoreOptions.BackgroundImage = global::Atum.Studio.Properties.Resources.toolbar_model_actions_arrowdown;
            this.plMoreOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plMoreOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plMoreOptions.Location = new System.Drawing.Point(198, 83);
            this.plMoreOptions.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.plMoreOptions.Name = "plMoreOptions";
            this.plMoreOptions.Size = new System.Drawing.Size(22, 22);
            this.plMoreOptions.TabIndex = 11;
            this.plMoreOptions.Click += new System.EventHandler(this.plMoreOptions_Click);
            // 
            // plSlider
            // 
            this.plSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.plSlider.Controls.Add(this.plUniformScaleFactor);
            this.plSlider.Controls.Add(this.genericLabelRegular141);
            this.plSlider.Location = new System.Drawing.Point(0, 0);
            this.plSlider.Name = "plSlider";
            this.plSlider.Size = new System.Drawing.Size(387, 60);
            this.plSlider.TabIndex = 7;
            // 
            // plUniformScaleFactor
            // 
            this.plUniformScaleFactor.BackColor = System.Drawing.Color.White;
            this.plUniformScaleFactor.Controls.Add(this.txtUniformScaleFactor);
            this.plUniformScaleFactor.Location = new System.Drawing.Point(220, 10);
            this.plUniformScaleFactor.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.plUniformScaleFactor.Name = "plUniformScaleFactor";
            this.plUniformScaleFactor.Size = new System.Drawing.Size(100, 40);
            this.plUniformScaleFactor.TabIndex = 2;
            // 
            // txtUniformScaleFactor
            // 
            this.txtUniformScaleFactor.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtUniformScaleFactor.ForeColor = System.Drawing.Color.Black;
            this.txtUniformScaleFactor.Location = new System.Drawing.Point(0, 12);
            this.txtUniformScaleFactor.Multiline = false;
            this.txtUniformScaleFactor.Name = "txtUniformScaleFactor";
            this.txtUniformScaleFactor.Size = new System.Drawing.Size(100, 24);
            this.txtUniformScaleFactor.TabIndex = 1;
            this.txtUniformScaleFactor.Text = "0";
            this.txtUniformScaleFactor.Value = 0F;
            this.txtUniformScaleFactor.TextChanged += new System.EventHandler(this.txtUniformScaleFactor_TextChanged);
            // 
            // genericLabelRegular141
            // 
            this.genericLabelRegular141.Dock = System.Windows.Forms.DockStyle.Left;
            this.genericLabelRegular141.Location = new System.Drawing.Point(0, 0);
            this.genericLabelRegular141.Name = "genericLabelRegular141";
            this.genericLabelRegular141.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.genericLabelRegular141.Size = new System.Drawing.Size(140, 60);
            this.genericLabelRegular141.TabIndex = 0;
            this.genericLabelRegular141.Text = "Scale factor";
            this.genericLabelRegular141.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.txtXValue);
            this.panel2.Location = new System.Drawing.Point(85, 123);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(100, 40);
            this.panel2.TabIndex = 12;
            // 
            // txtXValue
            // 
            this.txtXValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtXValue.Location = new System.Drawing.Point(0, 12);
            this.txtXValue.Multiline = false;
            this.txtXValue.Name = "txtXValue";
            this.txtXValue.Size = new System.Drawing.Size(100, 24);
            this.txtXValue.TabIndex = 1;
            this.txtXValue.Text = "0";
            this.txtXValue.Value = 0F;
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.Color.White;
            this.panel3.Controls.Add(this.txtYValue);
            this.panel3.Location = new System.Drawing.Point(85, 182);
            this.panel3.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(100, 40);
            this.panel3.TabIndex = 3;
            // 
            // txtYValue
            // 
            this.txtYValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtYValue.Location = new System.Drawing.Point(0, 12);
            this.txtYValue.Multiline = false;
            this.txtYValue.Name = "txtYValue";
            this.txtYValue.Size = new System.Drawing.Size(100, 24);
            this.txtYValue.TabIndex = 1;
            this.txtYValue.Text = "0";
            this.txtYValue.Value = 0F;
            // 
            // panel4
            // 
            this.panel4.BackColor = System.Drawing.Color.White;
            this.panel4.Controls.Add(this.txtZValue);
            this.panel4.Location = new System.Drawing.Point(85, 241);
            this.panel4.Margin = new System.Windows.Forms.Padding(3, 20, 3, 3);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(100, 40);
            this.panel4.TabIndex = 3;
            // 
            // txtZValue
            // 
            this.txtZValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtZValue.Location = new System.Drawing.Point(0, 12);
            this.txtZValue.Multiline = false;
            this.txtZValue.Name = "txtZValue";
            this.txtZValue.Size = new System.Drawing.Size(100, 24);
            this.txtZValue.TabIndex = 1;
            this.txtZValue.Text = "0";
            this.txtZValue.Value = 0F;
            // 
            // lblX
            // 
            this.lblX.Location = new System.Drawing.Point(60, 123);
            this.lblX.Name = "lblX";
            this.lblX.Size = new System.Drawing.Size(20, 40);
            this.lblX.TabIndex = 13;
            this.lblX.Text = "L";
            this.lblX.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblY
            // 
            this.lblY.Location = new System.Drawing.Point(60, 182);
            this.lblY.Name = "lblY";
            this.lblY.Size = new System.Drawing.Size(20, 40);
            this.lblY.TabIndex = 16;
            this.lblY.Text = "W";
            this.lblY.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblZ
            // 
            this.lblZ.Location = new System.Drawing.Point(60, 243);
            this.lblZ.Name = "lblZ";
            this.lblZ.Size = new System.Drawing.Size(20, 40);
            this.lblZ.TabIndex = 17;
            this.lblZ.Text = "H";
            this.lblZ.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnUniformScaleLink
            // 
            this.btnUniformScaleLink.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUniformScaleLink.Location = new System.Drawing.Point(3, 72);
            this.btnUniformScaleLink.Name = "btnUniformScaleLink";
            this.btnUniformScaleLink.Size = new System.Drawing.Size(18, 18);
            this.btnUniformScaleLink.TabIndex = 18;
            this.btnUniformScaleLink.Click += new System.EventHandler(this.btnUniformScaleLink_Click);
            // 
            // plLinkedLines
            // 
            this.plLinkedLines.Controls.Add(this.btnUniformScaleLink);
            this.plLinkedLines.Location = new System.Drawing.Point(16, 122);
            this.plLinkedLines.Name = "plLinkedLines";
            this.plLinkedLines.Size = new System.Drawing.Size(38, 160);
            this.plLinkedLines.TabIndex = 19;
            // 
            // lblMoreOptions
            // 
            this.lblMoreOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMoreOptions.Location = new System.Drawing.Point(100, 83);
            this.lblMoreOptions.Name = "lblMoreOptions";
            this.lblMoreOptions.Size = new System.Drawing.Size(95, 22);
            this.lblMoreOptions.TabIndex = 20;
            this.lblMoreOptions.Text = "More options";
            this.lblMoreOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMoreOptions.Click += new System.EventHandler(this.lblMoreOptions_Click);
            // 
            // lblXValue
            // 
            this.lblXValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblXValue.Controls.Add(this.btnXTypeDropdown);
            this.lblXValue.Controls.Add(this.lblXValueType);
            this.lblXValue.Location = new System.Drawing.Point(200, 123);
            this.lblXValue.Name = "lblXValue";
            this.lblXValue.Size = new System.Drawing.Size(120, 40);
            this.lblXValue.TabIndex = 3;
            this.lblXValue.Click += new System.EventHandler(this.lblXValue_Click);
            // 
            // btnXTypeDropdown
            // 
            this.btnXTypeDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnXTypeDropdown.Location = new System.Drawing.Point(99, 12);
            this.btnXTypeDropdown.Name = "btnXTypeDropdown";
            this.btnXTypeDropdown.Size = new System.Drawing.Size(18, 18);
            this.btnXTypeDropdown.TabIndex = 1;
            this.btnXTypeDropdown.Visible = false;
            this.btnXTypeDropdown.Click += new System.EventHandler(this.btnXTypeDropdown_Click);
            // 
            // lblXValueType
            // 
            this.lblXValueType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblXValueType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblXValueType.Location = new System.Drawing.Point(0, 0);
            this.lblXValueType.Name = "lblXValueType";
            this.lblXValueType.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblXValueType.Size = new System.Drawing.Size(93, 40);
            this.lblXValueType.TabIndex = 0;
            this.lblXValueType.Text = "Percentage";
            this.lblXValueType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblXValueType.Click += new System.EventHandler(this.lblXValueType_Click);
            // 
            // lblYValue
            // 
            this.lblYValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblYValue.Controls.Add(this.btnYTypeDropdown);
            this.lblYValue.Controls.Add(this.lblYValueType);
            this.lblYValue.Location = new System.Drawing.Point(200, 182);
            this.lblYValue.Name = "lblYValue";
            this.lblYValue.Size = new System.Drawing.Size(120, 40);
            this.lblYValue.TabIndex = 4;
            this.lblYValue.Click += new System.EventHandler(this.lblYValue_Click);
            // 
            // btnYTypeDropdown
            // 
            this.btnYTypeDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnYTypeDropdown.Location = new System.Drawing.Point(99, 12);
            this.btnYTypeDropdown.Name = "btnYTypeDropdown";
            this.btnYTypeDropdown.Size = new System.Drawing.Size(18, 18);
            this.btnYTypeDropdown.TabIndex = 1;
            this.btnYTypeDropdown.Visible = false;
            this.btnYTypeDropdown.Click += new System.EventHandler(this.btnYTypeDropdown_Click);
            // 
            // lblYValueType
            // 
            this.lblYValueType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblYValueType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblYValueType.Location = new System.Drawing.Point(0, 0);
            this.lblYValueType.Name = "lblYValueType";
            this.lblYValueType.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblYValueType.Size = new System.Drawing.Size(93, 40);
            this.lblYValueType.TabIndex = 0;
            this.lblYValueType.Text = "Percentage";
            this.lblYValueType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblYValueType.Click += new System.EventHandler(this.lblYValueType_Click);
            // 
            // lblZValue
            // 
            this.lblZValue.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblZValue.Controls.Add(this.btnZTypeDropdown);
            this.lblZValue.Controls.Add(this.lblZValueType);
            this.lblZValue.Location = new System.Drawing.Point(200, 241);
            this.lblZValue.Name = "lblZValue";
            this.lblZValue.Size = new System.Drawing.Size(120, 40);
            this.lblZValue.TabIndex = 5;
            this.lblZValue.Click += new System.EventHandler(this.lblZValue_Click);
            // 
            // btnZTypeDropdown
            // 
            this.btnZTypeDropdown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnZTypeDropdown.Location = new System.Drawing.Point(99, 12);
            this.btnZTypeDropdown.Name = "btnZTypeDropdown";
            this.btnZTypeDropdown.Size = new System.Drawing.Size(18, 18);
            this.btnZTypeDropdown.TabIndex = 1;
            this.btnZTypeDropdown.Visible = false;
            this.btnZTypeDropdown.Click += new System.EventHandler(this.btnZTypeDropdown_Click);
            // 
            // lblZValueType
            // 
            this.lblZValueType.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblZValueType.Dock = System.Windows.Forms.DockStyle.Left;
            this.lblZValueType.Location = new System.Drawing.Point(0, 0);
            this.lblZValueType.Name = "lblZValueType";
            this.lblZValueType.Padding = new System.Windows.Forms.Padding(6, 0, 0, 0);
            this.lblZValueType.Size = new System.Drawing.Size(93, 40);
            this.lblZValueType.TabIndex = 0;
            this.lblZValueType.Text = "Percentage";
            this.lblZValueType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblZValueType.Click += new System.EventHandler(this.lblZValueType_Click);
            // 
            // btnApply
            // 
            this.btnApply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnApply.BorderThickness = 0;
            this.btnApply.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnApply.ForeColor = System.Drawing.Color.White;
            this.btnApply.Location = new System.Drawing.Point(140, 11);
            this.btnApply.Margin = new System.Windows.Forms.Padding(0, 0, 16, 16);
            this.btnApply.Name = "btnApply";
            this.btnApply.Radius = 20;
            this.btnApply.SingleBorder = false;
            this.btnApply.Size = new System.Drawing.Size(176, 42);
            this.btnApply.TabIndex = 21;
            this.btnApply.Text = "Apply";
            this.btnApply.UseVisualStyleBackColor = false;
            this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
            // 
            // plFooter
            // 
            this.plFooter.Controls.Add(this.btnApply);
            this.plFooter.Location = new System.Drawing.Point(3, 288);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(340, 68);
            this.plFooter.TabIndex = 22;
            // 
            // SceneControlModelScale
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SceneControlModelScale";
            this.Size = new System.Drawing.Size(335, 414);
            this.plContent.ResumeLayout(false);
            this.plSlider.ResumeLayout(false);
            this.plUniformScaleFactor.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel4.ResumeLayout(false);
            this.plLinkedLines.ResumeLayout(false);
            this.lblXValue.ResumeLayout(false);
            this.lblYValue.ResumeLayout(false);
            this.lblZValue.ResumeLayout(false);
            this.plFooter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plMoreOptions;
        private System.Windows.Forms.Panel plSlider;
        private FloatInputField14 txtUniformScaleFactor;
        private NewGui.GenericLabelRegular14 genericLabelRegular141;
        private System.Windows.Forms.Panel plUniformScaleFactor;
        private NewGui.GenericLabelBold14 lblZ;
        private NewGui.GenericLabelBold14 lblY;
        private NewGui.GenericLabelBold14 lblX;
        private System.Windows.Forms.Panel panel3;
        private FloatInputField14 txtYValue;
        private System.Windows.Forms.Panel panel4;
        private FloatInputField14 txtZValue;
        private System.Windows.Forms.Panel panel2;
        private FloatInputField14 txtXValue;
        private System.Windows.Forms.Panel btnUniformScaleLink;
        private System.Windows.Forms.Panel plLinkedLines;
        private NewGui.GenericLabelRegular14 lblMoreOptions;
        private System.Windows.Forms.Panel lblXValue;
        private NewGui.GenericLabelRegular14 lblXValueType;
        private System.Windows.Forms.Panel lblYValue;
        private NewGui.GenericLabelRegular14 lblYValueType;
        private System.Windows.Forms.Panel lblZValue;
        private NewGui.GenericLabelRegular14 lblZValueType;
        private System.Windows.Forms.Panel btnZTypeDropdown;
        private System.Windows.Forms.Panel btnYTypeDropdown;
        private System.Windows.Forms.Panel btnXTypeDropdown;
        private NewGui.RoundedButton btnApply;
        private System.Windows.Forms.Panel plFooter;
    }
}
