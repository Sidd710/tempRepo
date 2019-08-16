namespace Atum.Studio.Controls.SceneControlActionPanel
{
    partial class SceneControlModelMagsAI
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
            this.btnGenerateSupport = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plSlider = new System.Windows.Forms.Panel();
            this.magsAISelectionSize = new Atum.Studio.Controls.NewGui.MAGSAITracker();
            this.btnSelectionSquare = new Atum.Studio.Controls.NewGui.RoundedRightButton();
            this.btnSelectionCircle = new Atum.Studio.Controls.NewGui.RoundedLeftButton();
            this.lblInformation = new System.Windows.Forms.Label();
            this.btnModelMarkings = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.lblMoreOptions = new System.Windows.Forms.Label();
            this.plMoreOptions = new System.Windows.Forms.Panel();
            this.plModelAddGridSupportRound = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plAddGridSupport = new System.Windows.Forms.PictureBox();
            this.btnModelAddGridSupport = new System.Windows.Forms.Panel();
            this.btnModelAddSingleSupport = new System.Windows.Forms.Panel();
            this.plModelAddManualSupport = new System.Windows.Forms.PictureBox();
            this.plModelAddManualSupportRound = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plSupportClearAll = new System.Windows.Forms.Panel();
            this.picSupportClearAll = new System.Windows.Forms.PictureBox();
            this.btnSupportClearAll = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plContent.SuspendLayout();
            this.plSlider.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plAddGridSupport)).BeginInit();
            this.btnModelAddGridSupport.SuspendLayout();
            this.btnModelAddSingleSupport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.plModelAddManualSupport)).BeginInit();
            this.plSupportClearAll.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picSupportClearAll)).BeginInit();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.plContent.Controls.Add(this.plSupportClearAll);
            this.plContent.Controls.Add(this.btnModelAddSingleSupport);
            this.plContent.Controls.Add(this.btnModelAddGridSupport);
            this.plContent.Controls.Add(this.plMoreOptions);
            this.plContent.Controls.Add(this.lblMoreOptions);
            this.plContent.Controls.Add(this.btnModelMarkings);
            this.plContent.Controls.Add(this.lblInformation);
            this.plContent.Controls.Add(this.btnGenerateSupport);
            this.plContent.Controls.Add(this.plSlider);
            this.plContent.Size = new System.Drawing.Size(356, 325);
            // 
            // btnGenerateSupport
            // 
            this.btnGenerateSupport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnGenerateSupport.BorderThickness = 0;
            this.btnGenerateSupport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnGenerateSupport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerateSupport.ForeColor = System.Drawing.Color.White;
            this.btnGenerateSupport.Location = new System.Drawing.Point(184, 124);
            this.btnGenerateSupport.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.btnGenerateSupport.Name = "btnGenerateSupport";
            this.btnGenerateSupport.Radius = 20;
            this.btnGenerateSupport.SingleBorder = false;
            this.btnGenerateSupport.Size = new System.Drawing.Size(156, 42);
            this.btnGenerateSupport.TabIndex = 3;
            this.btnGenerateSupport.Text = "Generate";
            this.btnGenerateSupport.UseVisualStyleBackColor = false;
            this.btnGenerateSupport.Click += new System.EventHandler(this.btnGenerateSupport_Click);
            // 
            // plSlider
            // 
            this.plSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.plSlider.Controls.Add(this.magsAISelectionSize);
            this.plSlider.Controls.Add(this.btnSelectionSquare);
            this.plSlider.Controls.Add(this.btnSelectionCircle);
            this.plSlider.Location = new System.Drawing.Point(0, 40);
            this.plSlider.Name = "plSlider";
            this.plSlider.Size = new System.Drawing.Size(356, 60);
            this.plSlider.TabIndex = 0;
            // 
            // magsAISelectionSize
            // 
            this.magsAISelectionSize.BackColor = System.Drawing.Color.Transparent;
            this.magsAISelectionSize.BorderColor = System.Drawing.SystemColors.Control;
            this.magsAISelectionSize.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.magsAISelectionSize.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.magsAISelectionSize.IndentHeight = 18;
            this.magsAISelectionSize.IndentWidth = 30;
            this.magsAISelectionSize.Location = new System.Drawing.Point(117, 0);
            this.magsAISelectionSize.Maximum = 10;
            this.magsAISelectionSize.Minimum = 0;
            this.magsAISelectionSize.Name = "magsAISelectionSize";
            this.magsAISelectionSize.Size = new System.Drawing.Size(239, 60);
            this.magsAISelectionSize.TabIndex = 2;
            this.magsAISelectionSize.TickColor = System.Drawing.Color.FromArgb(((int)(((byte)(148)))), ((int)(((byte)(146)))), ((int)(((byte)(148)))));
            this.magsAISelectionSize.TickHeight = 4;
            this.magsAISelectionSize.TrackerColor = System.Drawing.Color.Teal;
            this.magsAISelectionSize.TrackerSize = new System.Drawing.Size(24, 24);
            this.magsAISelectionSize.TrackLineColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(93)))), ((int)(((byte)(90)))));
            this.magsAISelectionSize.TrackLineHeight = 3;
            this.magsAISelectionSize.Value = 0;
            this.magsAISelectionSize.ValueChanged += new Atum.Studio.Controls.NewGui.ValueChangedHandler(this.magsAISelectionSize_ValueChanged);
            // 
            // btnSelectionSquare
            // 
            this.btnSelectionSquare.BackColor = System.Drawing.Color.White;
            this.btnSelectionSquare.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectionSquare.Location = new System.Drawing.Point(66, 13);
            this.btnSelectionSquare.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectionSquare.Name = "btnSelectionSquare";
            this.btnSelectionSquare.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.btnSelectionSquare.Size = new System.Drawing.Size(48, 32);
            this.btnSelectionSquare.TabIndex = 1;
            this.btnSelectionSquare.Click += new System.EventHandler(this.btnSelectionSquare_Click);
            // 
            // btnSelectionCircle
            // 
            this.btnSelectionCircle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnSelectionCircle.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectionCircle.FlatAppearance.BorderSize = 0;
            this.btnSelectionCircle.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.btnSelectionCircle.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSelectionCircle.Location = new System.Drawing.Point(19, 13);
            this.btnSelectionCircle.Margin = new System.Windows.Forms.Padding(0);
            this.btnSelectionCircle.Name = "btnSelectionCircle";
            this.btnSelectionCircle.Radius = 3;
            this.btnSelectionCircle.SelectedBackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnSelectionCircle.Size = new System.Drawing.Size(48, 32);
            this.btnSelectionCircle.TabIndex = 0;
            this.btnSelectionCircle.UseVisualStyleBackColor = false;
            this.btnSelectionCircle.Click += new System.EventHandler(this.btnSelectionCircle_Click);
            // 
            // lblInformation
            // 
            this.lblInformation.Location = new System.Drawing.Point(16, 0);
            this.lblInformation.Name = "lblInformation";
            this.lblInformation.Size = new System.Drawing.Size(324, 40);
            this.lblInformation.TabIndex = 0;
            this.lblInformation.Text = "Mark the surface where you don’t want supports.";
            // 
            // btnModelMarkings
            // 
            this.btnModelMarkings.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.btnModelMarkings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnModelMarkings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelMarkings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnModelMarkings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnModelMarkings.Location = new System.Drawing.Point(16, 124);
            this.btnModelMarkings.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            this.btnModelMarkings.Name = "btnModelMarkings";
            this.btnModelMarkings.Radius = 18;
            this.btnModelMarkings.SingleBorder = false;
            this.btnModelMarkings.Size = new System.Drawing.Size(156, 42);
            this.btnModelMarkings.TabIndex = 4;
            this.btnModelMarkings.Text = "Clear";
            this.btnModelMarkings.UseVisualStyleBackColor = false;
            this.btnModelMarkings.Click += new System.EventHandler(this.btnModelClearSupport_Click);
            // 
            // lblMoreOptions
            // 
            this.lblMoreOptions.AutoSize = true;
            this.lblMoreOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblMoreOptions.Location = new System.Drawing.Point(120, 192);
            this.lblMoreOptions.Name = "lblMoreOptions";
            this.lblMoreOptions.Size = new System.Drawing.Size(68, 13);
            this.lblMoreOptions.TabIndex = 5;
            this.lblMoreOptions.Text = "More options";
            this.lblMoreOptions.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.lblMoreOptions.Click += new System.EventHandler(this.lblMoreOptions_Click);
            // 
            // plMoreOptions
            // 
            this.plMoreOptions.BackgroundImage = global::Atum.Studio.Properties.Resources.toolbar_model_actions_arrowdown;
            this.plMoreOptions.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plMoreOptions.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plMoreOptions.Location = new System.Drawing.Point(198, 189);
            this.plMoreOptions.Name = "plMoreOptions";
            this.plMoreOptions.Size = new System.Drawing.Size(22, 22);
            this.plMoreOptions.TabIndex = 6;
            this.plMoreOptions.Click += new System.EventHandler(this.plMoreOptions_Click);
            // 
            // plModelAddGridSupportRound
            // 
            this.plModelAddGridSupportRound.BackColor = System.Drawing.Color.White;
            this.plModelAddGridSupportRound.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plModelAddGridSupportRound.FlatAppearance.BorderSize = 0;
            this.plModelAddGridSupportRound.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.plModelAddGridSupportRound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plModelAddGridSupportRound.Location = new System.Drawing.Point(0, 0);
            this.plModelAddGridSupportRound.Margin = new System.Windows.Forms.Padding(0);
            this.plModelAddGridSupportRound.Name = "plModelAddGridSupportRound";
            this.plModelAddGridSupportRound.Radius = 27;
            this.plModelAddGridSupportRound.SingleBorder = false;
            this.plModelAddGridSupportRound.Size = new System.Drawing.Size(60, 60);
            this.plModelAddGridSupportRound.TabIndex = 7;
            this.plModelAddGridSupportRound.UseVisualStyleBackColor = false;
            this.plModelAddGridSupportRound.Click += new System.EventHandler(this.plModelAddGridSupportRound_Click);
            // 
            // plAddGridSupport
            // 
            this.plAddGridSupport.BackColor = System.Drawing.Color.White;
            this.plAddGridSupport.BackgroundImage = global::Atum.Studio.Properties.Resources.toolbar_actions_model_gridsupport;
            this.plAddGridSupport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plAddGridSupport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plAddGridSupport.Location = new System.Drawing.Point(10, 10);
            this.plAddGridSupport.Name = "plAddGridSupport";
            this.plAddGridSupport.Size = new System.Drawing.Size(40, 40);
            this.plAddGridSupport.TabIndex = 8;
            this.plAddGridSupport.TabStop = false;
            this.plAddGridSupport.Click += new System.EventHandler(this.plAddGridSupport_Click);
            // 
            // btnModelAddGridSupport
            // 
            this.btnModelAddGridSupport.Controls.Add(this.plAddGridSupport);
            this.btnModelAddGridSupport.Controls.Add(this.plModelAddGridSupportRound);
            this.btnModelAddGridSupport.Location = new System.Drawing.Point(148, 240);
            this.btnModelAddGridSupport.Name = "btnModelAddGridSupport";
            this.btnModelAddGridSupport.Size = new System.Drawing.Size(60, 60);
            this.btnModelAddGridSupport.TabIndex = 9;
            this.btnModelAddGridSupport.Click += new System.EventHandler(this.btnModelAddGridSupport_Click);
            // 
            // btnModelAddSingleSupport
            // 
            this.btnModelAddSingleSupport.Controls.Add(this.plModelAddManualSupport);
            this.btnModelAddSingleSupport.Controls.Add(this.plModelAddManualSupportRound);
            this.btnModelAddSingleSupport.Location = new System.Drawing.Point(72, 240);
            this.btnModelAddSingleSupport.Name = "btnModelAddSingleSupport";
            this.btnModelAddSingleSupport.Size = new System.Drawing.Size(60, 60);
            this.btnModelAddSingleSupport.TabIndex = 10;
            this.btnModelAddSingleSupport.Click += new System.EventHandler(this.btnModelAddSingleSupport_Click);
            // 
            // plModelAddManualSupport
            // 
            this.plModelAddManualSupport.BackColor = System.Drawing.Color.White;
            this.plModelAddManualSupport.BackgroundImage = global::Atum.Studio.Properties.Resources.toolbar_actions_model_singlesupport;
            this.plModelAddManualSupport.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plModelAddManualSupport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plModelAddManualSupport.Location = new System.Drawing.Point(10, 10);
            this.plModelAddManualSupport.Name = "plModelAddManualSupport";
            this.plModelAddManualSupport.Size = new System.Drawing.Size(40, 40);
            this.plModelAddManualSupport.TabIndex = 8;
            this.plModelAddManualSupport.TabStop = false;
            this.plModelAddManualSupport.Click += new System.EventHandler(this.plModelAddManualSupport_Click);
            // 
            // plModelAddManualSupportRound
            // 
            this.plModelAddManualSupportRound.BackColor = System.Drawing.Color.White;
            this.plModelAddManualSupportRound.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plModelAddManualSupportRound.FlatAppearance.BorderSize = 0;
            this.plModelAddManualSupportRound.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.plModelAddManualSupportRound.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plModelAddManualSupportRound.Image = global::Atum.Studio.Properties.Resources.toolbar_model_action_gridsupport;
            this.plModelAddManualSupportRound.Location = new System.Drawing.Point(0, 0);
            this.plModelAddManualSupportRound.Margin = new System.Windows.Forms.Padding(0);
            this.plModelAddManualSupportRound.Name = "plModelAddManualSupportRound";
            this.plModelAddManualSupportRound.Radius = 27;
            this.plModelAddManualSupportRound.SingleBorder = false;
            this.plModelAddManualSupportRound.Size = new System.Drawing.Size(60, 60);
            this.plModelAddManualSupportRound.TabIndex = 7;
            this.plModelAddManualSupportRound.UseVisualStyleBackColor = false;
            this.plModelAddManualSupportRound.Click += new System.EventHandler(this.roundedButton2_Click);
            // 
            // plSupportClearAll
            // 
            this.plSupportClearAll.Controls.Add(this.picSupportClearAll);
            this.plSupportClearAll.Controls.Add(this.btnSupportClearAll);
            this.plSupportClearAll.Location = new System.Drawing.Point(224, 240);
            this.plSupportClearAll.Name = "plSupportClearAll";
            this.plSupportClearAll.Size = new System.Drawing.Size(60, 60);
            this.plSupportClearAll.TabIndex = 10;
            this.plSupportClearAll.Click += new System.EventHandler(this.plSupportClearAll_Click);
            // 
            // picSupportClearAll
            // 
            this.picSupportClearAll.BackColor = System.Drawing.Color.White;
            this.picSupportClearAll.BackgroundImage = global::Atum.Studio.Properties.Resources.btnSupport_ClearAll;
            this.picSupportClearAll.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.picSupportClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.picSupportClearAll.Location = new System.Drawing.Point(10, 10);
            this.picSupportClearAll.Name = "picSupportClearAll";
            this.picSupportClearAll.Size = new System.Drawing.Size(40, 40);
            this.picSupportClearAll.TabIndex = 8;
            this.picSupportClearAll.TabStop = false;
            this.picSupportClearAll.Click += new System.EventHandler(this.picSupportClearAll_Click);
            // 
            // btnSupportClearAll
            // 
            this.btnSupportClearAll.BackColor = System.Drawing.Color.White;
            this.btnSupportClearAll.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSupportClearAll.FlatAppearance.BorderSize = 0;
            this.btnSupportClearAll.FlatAppearance.MouseOverBackColor = System.Drawing.SystemColors.Control;
            this.btnSupportClearAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSupportClearAll.Location = new System.Drawing.Point(0, 0);
            this.btnSupportClearAll.Margin = new System.Windows.Forms.Padding(0);
            this.btnSupportClearAll.Name = "btnSupportClearAll";
            this.btnSupportClearAll.Radius = 27;
            this.btnSupportClearAll.SingleBorder = false;
            this.btnSupportClearAll.Size = new System.Drawing.Size(60, 60);
            this.btnSupportClearAll.TabIndex = 7;
            this.btnSupportClearAll.UseVisualStyleBackColor = false;
            this.btnSupportClearAll.Click += new System.EventHandler(this.btnSupportClearAll_Click);
            // 
            // SceneControlModelMagsAI
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SceneControlModelMagsAI";
            this.Size = new System.Drawing.Size(356, 382);
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            this.plSlider.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plAddGridSupport)).EndInit();
            this.btnModelAddGridSupport.ResumeLayout(false);
            this.btnModelAddSingleSupport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plModelAddManualSupport)).EndInit();
            this.plSupportClearAll.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picSupportClearAll)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plSlider;
        private NewGui.RoundedButton btnGenerateSupport;
        private System.Windows.Forms.Label lblInformation;
        private NewGui.RoundedLeftButton btnSelectionCircle;
        private NewGui.RoundedRightButton btnSelectionSquare;
        private NewGui.MAGSAITracker magsAISelectionSize;
        private NewGui.RoundedButton btnModelMarkings;
        private System.Windows.Forms.Label lblMoreOptions;
        private System.Windows.Forms.Panel plMoreOptions;
        private NewGui.RoundedButton plModelAddGridSupportRound;
        private System.Windows.Forms.Panel btnModelAddGridSupport;
        private System.Windows.Forms.PictureBox plAddGridSupport;
        private System.Windows.Forms.Panel btnModelAddSingleSupport;
        private System.Windows.Forms.PictureBox plModelAddManualSupport;
        private NewGui.RoundedButton plModelAddManualSupportRound;
        private System.Windows.Forms.Panel plSupportClearAll;
        private System.Windows.Forms.PictureBox picSupportClearAll;
        private NewGui.RoundedButton btnSupportClearAll;
    }
}
