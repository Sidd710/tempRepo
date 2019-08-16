namespace Atum.Studio.Controls.OpenGL
{
    partial class SceneControlModelActionsToolbar
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
            this.btnModelActionRotate = new System.Windows.Forms.PictureBox();
            this.btnModelActionSupport = new System.Windows.Forms.PictureBox();
            this.btnModelActionMove = new System.Windows.Forms.PictureBox();
            this.btnModelActionDuplicate = new System.Windows.Forms.PictureBox();
            this.btnModelActionScale = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionRotate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionSupport)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionMove)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionDuplicate)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionScale)).BeginInit();
            this.SuspendLayout();
            // 
            // btnModelActionRotate
            // 
            this.btnModelActionRotate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelActionRotate.Location = new System.Drawing.Point(0, 60);
            this.btnModelActionRotate.Name = "btnModelActionRotate";
            this.btnModelActionRotate.Size = new System.Drawing.Size(60, 60);
            this.btnModelActionRotate.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnModelActionRotate.TabIndex = 5;
            this.btnModelActionRotate.TabStop = false;
            this.btnModelActionRotate.Click += new System.EventHandler(this.btnModelActionRotate_Click);
            // 
            // btnModelActionSupport
            // 
            this.btnModelActionSupport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelActionSupport.Location = new System.Drawing.Point(0, 180);
            this.btnModelActionSupport.Name = "btnModelActionSupport";
            this.btnModelActionSupport.Size = new System.Drawing.Size(60, 60);
            this.btnModelActionSupport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnModelActionSupport.TabIndex = 4;
            this.btnModelActionSupport.TabStop = false;
            this.btnModelActionSupport.Click += new System.EventHandler(this.btnModelActionSupport_Click);
            // 
            // btnModelActionMove
            // 
            this.btnModelActionMove.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelActionMove.Location = new System.Drawing.Point(0, 0);
            this.btnModelActionMove.Name = "btnModelActionMove";
            this.btnModelActionMove.Size = new System.Drawing.Size(60, 60);
            this.btnModelActionMove.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnModelActionMove.TabIndex = 3;
            this.btnModelActionMove.TabStop = false;
            this.btnModelActionMove.Click += new System.EventHandler(this.btnModelActionMove_Click);
            // 
            // btnModelActionDuplicate
            // 
            this.btnModelActionDuplicate.BackgroundImage = global::Atum.Studio.Properties.Resources.toolbar_actions_model_duplicate;
            this.btnModelActionDuplicate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.btnModelActionDuplicate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelActionDuplicate.Location = new System.Drawing.Point(0, 240);
            this.btnModelActionDuplicate.Name = "btnModelActionDuplicate";
            this.btnModelActionDuplicate.Size = new System.Drawing.Size(60, 60);
            this.btnModelActionDuplicate.TabIndex = 6;
            this.btnModelActionDuplicate.TabStop = false;
            this.btnModelActionDuplicate.Click += new System.EventHandler(this.btnModelActionDuplicate_Click);
            // 
            // btnModelActionScale
            // 
            this.btnModelActionScale.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModelActionScale.Location = new System.Drawing.Point(0, 120);
            this.btnModelActionScale.Name = "btnModelActionScale";
            this.btnModelActionScale.Size = new System.Drawing.Size(60, 60);
            this.btnModelActionScale.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.btnModelActionScale.TabIndex = 7;
            this.btnModelActionScale.TabStop = false;
            this.btnModelActionScale.Click += new System.EventHandler(this.btnModelActionScale_Click);
            // 
            // SceneControlModelActionsToolbar
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.btnModelActionScale);
            this.Controls.Add(this.btnModelActionDuplicate);
            this.Controls.Add(this.btnModelActionRotate);
            this.Controls.Add(this.btnModelActionSupport);
            this.Controls.Add(this.btnModelActionMove);
            this.Name = "SceneControlModelActionsToolbar";
            this.Size = new System.Drawing.Size(60, 300);
            this.Load += new System.EventHandler(this.SceneControlModelActionsToolbar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionRotate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionSupport)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionMove)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionDuplicate)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnModelActionScale)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnModelActionRotate;
        private System.Windows.Forms.PictureBox btnModelActionSupport;
        private System.Windows.Forms.PictureBox btnModelActionMove;
        private System.Windows.Forms.PictureBox btnModelActionDuplicate;
        private System.Windows.Forms.PictureBox btnModelActionScale;
    }
}
