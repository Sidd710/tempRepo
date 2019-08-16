namespace Atum.Studio.Controls.OpenGL
{
    partial class SceneGLControl
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
            this.SuspendLayout();
            // 
            // SceneGLControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "SceneGLControl";
            this.Scroll += new System.Windows.Forms.ScrollEventHandler(this.SceneGLControl_Scroll);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.SceneGLControl_KeyDown);
            this.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.SceneGLControl_MouseDoubleClick);
            this.ResumeLayout(false);

        }

        #endregion
    }
}
