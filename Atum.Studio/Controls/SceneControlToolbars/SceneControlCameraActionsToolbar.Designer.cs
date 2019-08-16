namespace Atum.Studio.Controls.OpenGL
{
    partial class SceneControlCameraActionsToolbar
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
            this.btnCameraActionPan = new System.Windows.Forms.PictureBox();
            this.btnCameraActionZoom = new System.Windows.Forms.PictureBox();
            this.btnCameraActionOrbit = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.btnCameraActionPan)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCameraActionZoom)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCameraActionOrbit)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCameraActionPan
            // 
            this.btnCameraActionPan.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCameraActionPan.Location = new System.Drawing.Point(10, 10);
            this.btnCameraActionPan.Name = "btnCameraActionPan";
            this.btnCameraActionPan.Size = new System.Drawing.Size(40, 40);
            this.btnCameraActionPan.TabIndex = 0;
            this.btnCameraActionPan.TabStop = false;
            this.btnCameraActionPan.Click += new System.EventHandler(this.btnCameraActionPan_Click);
            // 
            // btnCameraActionZoom
            // 
            this.btnCameraActionZoom.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCameraActionZoom.Location = new System.Drawing.Point(10, 130);
            this.btnCameraActionZoom.Name = "btnCameraActionZoom";
            this.btnCameraActionZoom.Size = new System.Drawing.Size(40, 40);
            this.btnCameraActionZoom.TabIndex = 1;
            this.btnCameraActionZoom.TabStop = false;
            this.btnCameraActionZoom.Click += new System.EventHandler(this.btnCameraActionZoom_Click);
            // 
            // btnCameraActionOrbit
            // 
            this.btnCameraActionOrbit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCameraActionOrbit.Location = new System.Drawing.Point(10, 70);
            this.btnCameraActionOrbit.Name = "btnCameraActionOrbit";
            this.btnCameraActionOrbit.Size = new System.Drawing.Size(40, 40);
            this.btnCameraActionOrbit.TabIndex = 2;
            this.btnCameraActionOrbit.TabStop = false;
            this.btnCameraActionOrbit.Click += new System.EventHandler(this.btnCameraActionOrbit_Click);
            // 
            // SceneControlCameraActionsToolbar
            // 
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.Controls.Add(this.btnCameraActionOrbit);
            this.Controls.Add(this.btnCameraActionZoom);
            this.Controls.Add(this.btnCameraActionPan);
            this.Name = "SceneControlCameraActionsToolbar";
            this.Size = new System.Drawing.Size(60, 180);
            this.Load += new System.EventHandler(this.SceneControlRenderModeToolbar_Load);
            ((System.ComponentModel.ISupportInitialize)(this.btnCameraActionPan)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCameraActionZoom)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnCameraActionOrbit)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox btnCameraActionPan;
        private System.Windows.Forms.PictureBox btnCameraActionZoom;
        private System.Windows.Forms.PictureBox btnCameraActionOrbit;
    }
}
