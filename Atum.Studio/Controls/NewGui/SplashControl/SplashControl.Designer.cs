using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui.SplashControl
{
    partial class SplashControl
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
            this.lblOperatorStation = new System.Windows.Forms.Label();
            this.pbLogo = new System.Windows.Forms.PictureBox();
            this.plGradient = new System.Windows.Forms.Panel();
            this.pbContact = new System.Windows.Forms.PictureBox();
            this.lblContact = new System.Windows.Forms.Label();
            this.lblskipwelcome = new System.Windows.Forms.Label();
            this.plTrail = new System.Windows.Forms.Panel();
            this.lblDaysTrailRight = new System.Windows.Forms.Label();
            this.lblLinkAuthorize = new System.Windows.Forms.LinkLabel();
            this.lblDaysTrailLeft = new System.Windows.Forms.Label();
            this.plDaysLeft = new Atum.Studio.Controls.NewGui.RoundedButton();
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).BeginInit();
            this.plGradient.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbContact)).BeginInit();
            this.plTrail.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblOperatorStation
            // 
            this.lblOperatorStation.AutoSize = true;
            this.lblOperatorStation.BackColor = System.Drawing.Color.Transparent;
            this.lblOperatorStation.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblOperatorStation.ForeColor = System.Drawing.Color.Black;
            this.lblOperatorStation.Location = new System.Drawing.Point(22, 22);
            this.lblOperatorStation.Name = "lblOperatorStation";
            this.lblOperatorStation.Size = new System.Drawing.Size(242, 29);
            this.lblOperatorStation.TabIndex = 1;
            this.lblOperatorStation.Text = "Operator Station V1.2";
            // 
            // pbLogo
            // 
            this.pbLogo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.pbLogo.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pbLogo.Location = new System.Drawing.Point(530, 24);
            this.pbLogo.Margin = new System.Windows.Forms.Padding(4);
            this.pbLogo.Name = "pbLogo";
            this.pbLogo.Size = new System.Drawing.Size(222, 78);
            this.pbLogo.TabIndex = 2;
            this.pbLogo.TabStop = false;
            // 
            // plGradient
            // 
            this.plGradient.Controls.Add(this.pbContact);
            this.plGradient.Controls.Add(this.lblContact);
            this.plGradient.Controls.Add(this.lblskipwelcome);
            this.plGradient.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.plGradient.Location = new System.Drawing.Point(0, 496);
            this.plGradient.Name = "plGradient";
            this.plGradient.Size = new System.Drawing.Size(768, 80);
            this.plGradient.TabIndex = 6;
            this.plGradient.Paint += new System.Windows.Forms.PaintEventHandler(this.panel1_Paint);
            // 
            // pbContact
            // 
            this.pbContact.BackColor = System.Drawing.Color.Transparent;
            this.pbContact.Cursor = System.Windows.Forms.Cursors.Hand;
            this.pbContact.Location = new System.Drawing.Point(16, 28);
            this.pbContact.Margin = new System.Windows.Forms.Padding(4);
            this.pbContact.Name = "pbContact";
            this.pbContact.Size = new System.Drawing.Size(24, 24);
            this.pbContact.TabIndex = 8;
            this.pbContact.TabStop = false;
            this.pbContact.Click += new System.EventHandler(this.pbContact_Click);
            // 
            // lblContact
            // 
            this.lblContact.AutoSize = true;
            this.lblContact.BackColor = System.Drawing.Color.Transparent;
            this.lblContact.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblContact.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblContact.ForeColor = System.Drawing.Color.White;
            this.lblContact.Location = new System.Drawing.Point(44, 31);
            this.lblContact.Name = "lblContact";
            this.lblContact.Size = new System.Drawing.Size(56, 17);
            this.lblContact.TabIndex = 7;
            this.lblContact.Text = "Contact";
            this.lblContact.Click += new System.EventHandler(this.pbContact_Click);
            // 
            // lblskipwelcome
            // 
            this.lblskipwelcome.AutoSize = true;
            this.lblskipwelcome.BackColor = System.Drawing.Color.Transparent;
            this.lblskipwelcome.Cursor = System.Windows.Forms.Cursors.Hand;
            this.lblskipwelcome.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblskipwelcome.ForeColor = System.Drawing.Color.White;
            this.lblskipwelcome.Location = new System.Drawing.Point(140, 31);
            this.lblskipwelcome.Name = "lblskipwelcome";
            this.lblskipwelcome.Size = new System.Drawing.Size(140, 17);
            this.lblskipwelcome.TabIndex = 6;
            this.lblskipwelcome.Text = "Skip welcome screen";
            this.lblskipwelcome.Click += new System.EventHandler(this.lblskipwelcome_Click);
            // 
            // plTrail
            // 
            this.plTrail.BackColor = System.Drawing.Color.White;
            this.plTrail.Controls.Add(this.lblDaysTrailRight);
            this.plTrail.Controls.Add(this.lblLinkAuthorize);
            this.plTrail.Controls.Add(this.lblDaysTrailLeft);
            this.plTrail.Controls.Add(this.plDaysLeft);
            this.plTrail.Location = new System.Drawing.Point(24, 58);
            this.plTrail.Name = "plTrail";
            this.plTrail.Size = new System.Drawing.Size(337, 31);
            this.plTrail.TabIndex = 7;
            // 
            // lblDaysTrailRight
            // 
            this.lblDaysTrailRight.ForeColor = System.Drawing.Color.Black;
            this.lblDaysTrailRight.Location = new System.Drawing.Point(177, 0);
            this.lblDaysTrailRight.Name = "lblDaysTrailRight";
            this.lblDaysTrailRight.Size = new System.Drawing.Size(63, 31);
            this.lblDaysTrailRight.TabIndex = 3;
            this.lblDaysTrailRight.Text = " days.";
            this.lblDaysTrailRight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblLinkAuthorize
            // 
            this.lblLinkAuthorize.DisabledLinkColor = System.Drawing.Color.Black;
            this.lblLinkAuthorize.ForeColor = System.Drawing.Color.Black;
            this.lblLinkAuthorize.LinkColor = System.Drawing.Color.Black;
            this.lblLinkAuthorize.Location = new System.Drawing.Point(226, 5);
            this.lblLinkAuthorize.Name = "lblLinkAuthorize";
            this.lblLinkAuthorize.Size = new System.Drawing.Size(111, 17);
            this.lblLinkAuthorize.TabIndex = 2;
            this.lblLinkAuthorize.TabStop = true;
            this.lblLinkAuthorize.Text = "Activate now";
            this.lblLinkAuthorize.VisitedLinkColor = System.Drawing.Color.Black;
            // 
            // lblDaysTrailLeft
            // 
            this.lblDaysTrailLeft.ForeColor = System.Drawing.Color.Black;
            this.lblDaysTrailLeft.Location = new System.Drawing.Point(3, 0);
            this.lblDaysTrailLeft.Name = "lblDaysTrailLeft";
            this.lblDaysTrailLeft.Size = new System.Drawing.Size(129, 31);
            this.lblDaysTrailLeft.TabIndex = 1;
            this.lblDaysTrailLeft.Text = "Trial will expire in";
            this.lblDaysTrailLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // plDaysLeft
            // 
            this.plDaysLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.plDaysLeft.BorderColor = System.Drawing.SystemColors.Control;
            this.plDaysLeft.BorderThickness = 0;
            this.plDaysLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plDaysLeft.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plDaysLeft.ForeColor = System.Drawing.Color.White;
            this.plDaysLeft.Location = new System.Drawing.Point(145, 1);
            this.plDaysLeft.Margin = new System.Windows.Forms.Padding(0);
            this.plDaysLeft.Name = "plDaysLeft";
            this.plDaysLeft.SingleBorder = false;
            this.plDaysLeft.Size = new System.Drawing.Size(26, 26);
            this.plDaysLeft.TabIndex = 0;
            this.plDaysLeft.UseVisualStyleBackColor = false;
            // 
            // SplashControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.Transparent;
            this.BackgroundImage = global::Atum.Studio.Properties.Resources.splash_models;
            this.Controls.Add(this.plTrail);
            this.Controls.Add(this.pbLogo);
            this.Controls.Add(this.lblOperatorStation);
            this.Controls.Add(this.plGradient);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(0);
            this.Name = "SplashControl";
            this.Size = new System.Drawing.Size(768, 576);
            ((System.ComponentModel.ISupportInitialize)(this.pbLogo)).EndInit();
            this.plGradient.ResumeLayout(false);
            this.plGradient.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbContact)).EndInit();
            this.plTrail.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label lblOperatorStation;
        private System.Windows.Forms.PictureBox pbLogo;
        private System.Windows.Forms.Panel plGradient;
        private System.Windows.Forms.PictureBox pbContact;
        private System.Windows.Forms.Label lblContact;
        private System.Windows.Forms.Label lblskipwelcome;
        private System.Windows.Forms.Panel plTrail;
        private System.Windows.Forms.LinkLabel lblLinkAuthorize;
        private System.Windows.Forms.Label lblDaysTrailLeft;
        private RoundedButton plDaysLeft;
        private System.Windows.Forms.Label lblDaysTrailRight;
    }
}
