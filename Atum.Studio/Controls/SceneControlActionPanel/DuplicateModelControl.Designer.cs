namespace Atum.Studio.Controls.SceneControlActionPanel
{
    partial class DuplicateModelControl
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
            this.plDuplicateModelControl = new System.Windows.Forms.Panel();
            this.plDisplayName = new System.Windows.Forms.Panel();
            this.txtDuplicatesCount = new System.Windows.Forms.TextBox();
            this.btnDuplicatesPlus = new System.Windows.Forms.PictureBox();
            this.btnDuplicatesMinus = new System.Windows.Forms.PictureBox();
            this.lblModelName = new System.Windows.Forms.Label();
            this.plDuplicateModelControl.SuspendLayout();
            this.plDisplayName.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDuplicatesPlus)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDuplicatesMinus)).BeginInit();
            this.SuspendLayout();
            // 
            // plDuplicateModelControl
            // 
            this.plDuplicateModelControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.plDuplicateModelControl.Controls.Add(this.plDisplayName);
            this.plDuplicateModelControl.Controls.Add(this.btnDuplicatesPlus);
            this.plDuplicateModelControl.Controls.Add(this.btnDuplicatesMinus);
            this.plDuplicateModelControl.Controls.Add(this.lblModelName);
            this.plDuplicateModelControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plDuplicateModelControl.Location = new System.Drawing.Point(0, 0);
            this.plDuplicateModelControl.Name = "plDuplicateModelControl";
            this.plDuplicateModelControl.Size = new System.Drawing.Size(368, 60);
            this.plDuplicateModelControl.TabIndex = 0;
            // 
            // plDisplayName
            // 
            this.plDisplayName.BackColor = System.Drawing.Color.White;
            this.plDisplayName.Controls.Add(this.txtDuplicatesCount);
            this.plDisplayName.Location = new System.Drawing.Point(211, 10);
            this.plDisplayName.Name = "plDisplayName";
            this.plDisplayName.Size = new System.Drawing.Size(96, 40);
            this.plDisplayName.TabIndex = 16;
            // 
            // txtDuplicatesCount
            // 
            this.txtDuplicatesCount.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDuplicatesCount.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtDuplicatesCount.Location = new System.Drawing.Point(0, 0);
            this.txtDuplicatesCount.MinimumSize = new System.Drawing.Size(0, 40);
            this.txtDuplicatesCount.Multiline = true;
            this.txtDuplicatesCount.Name = "txtDuplicatesCount";
            this.txtDuplicatesCount.Size = new System.Drawing.Size(96, 40);
            this.txtDuplicatesCount.TabIndex = 13;
            this.txtDuplicatesCount.Text = "1";
            this.txtDuplicatesCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.txtDuplicatesCount.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtDuplicatesCount_MouseClick);
            this.txtDuplicatesCount.TextChanged += new System.EventHandler(this.txtDuplicatesCount_TextChanged);
            this.txtDuplicatesCount.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDuplicatesCount_KeyPress);
            // 
            // btnDuplicatesPlus
            // 
            this.btnDuplicatesPlus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDuplicatesPlus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDuplicatesPlus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDuplicatesPlus.Location = new System.Drawing.Point(307, 10);
            this.btnDuplicatesPlus.Name = "btnDuplicatesPlus";
            this.btnDuplicatesPlus.Size = new System.Drawing.Size(40, 40);
            this.btnDuplicatesPlus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnDuplicatesPlus.TabIndex = 15;
            this.btnDuplicatesPlus.TabStop = false;
            this.btnDuplicatesPlus.Click += new System.EventHandler(this.btnDuplicatesPlus_Click);
            // 
            // btnDuplicatesMinus
            // 
            this.btnDuplicatesMinus.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnDuplicatesMinus.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnDuplicatesMinus.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnDuplicatesMinus.Location = new System.Drawing.Point(171, 10);
            this.btnDuplicatesMinus.Name = "btnDuplicatesMinus";
            this.btnDuplicatesMinus.Size = new System.Drawing.Size(40, 40);
            this.btnDuplicatesMinus.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnDuplicatesMinus.TabIndex = 14;
            this.btnDuplicatesMinus.TabStop = false;
            this.btnDuplicatesMinus.Click += new System.EventHandler(this.btnDuplicatesMinus_Click);
            // 
            // lblModelName
            // 
            this.lblModelName.AutoSize = true;
            this.lblModelName.Location = new System.Drawing.Point(16, 20);
            this.lblModelName.Name = "lblModelName";
            this.lblModelName.Size = new System.Drawing.Size(74, 13);
            this.lblModelName.TabIndex = 0;
            this.lblModelName.Text = "lblModelName";
            // 
            // DuplicateModelControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(237)))), ((int)(((byte)(237)))), ((int)(((byte)(237)))));
            this.Controls.Add(this.plDuplicateModelControl);
            this.Name = "DuplicateModelControl";
            this.Size = new System.Drawing.Size(368, 60);
            this.plDuplicateModelControl.ResumeLayout(false);
            this.plDuplicateModelControl.PerformLayout();
            this.plDisplayName.ResumeLayout(false);
            this.plDisplayName.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.btnDuplicatesPlus)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnDuplicatesMinus)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plDuplicateModelControl;
        private System.Windows.Forms.Label lblModelName;
        private System.Windows.Forms.PictureBox btnDuplicatesPlus;
        private System.Windows.Forms.TextBox txtDuplicatesCount;
        private System.Windows.Forms.PictureBox btnDuplicatesMinus;
        private System.Windows.Forms.Panel plDisplayName;
    }
}
