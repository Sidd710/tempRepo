namespace Atum.Studio.Controls.NewGui
{
    partial class NewGUITextboxPanel
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
            this.plTextbox = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // plTextbox
            // 
            this.plTextbox.BackColor = System.Drawing.Color.White;
            this.plTextbox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plTextbox.ForeColor = System.Drawing.Color.Black;
            this.plTextbox.Location = new System.Drawing.Point(1, 1);
            this.plTextbox.Name = "plTextbox";
            this.plTextbox.Size = new System.Drawing.Size(430, 38);
            this.plTextbox.TabIndex = 0;
            this.plTextbox.Paint += new System.Windows.Forms.PaintEventHandler(this.plTextbox_Paint);
            // 
            // NewGUITextboxPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.Controls.Add(this.plTextbox);
            this.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Name = "NewGUITextboxPanel";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(432, 40);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plTextbox;
    }
}
