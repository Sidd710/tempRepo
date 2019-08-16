namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    partial class MaterialMenuStrip
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
            this.btnAddCatalog = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.plBackground = new System.Windows.Forms.Panel();
            this.plBackground.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnAddCatalog
            // 
            this.btnAddCatalog.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnAddCatalog.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAddCatalog.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnAddCatalog.FlatAppearance.BorderSize = 0;
            this.btnAddCatalog.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.btnAddCatalog.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.btnAddCatalog.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAddCatalog.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnAddCatalog.ForeColor = System.Drawing.Color.White;
            this.btnAddCatalog.Location = new System.Drawing.Point(0, 8);
            this.btnAddCatalog.Margin = new System.Windows.Forms.Padding(0);
            this.btnAddCatalog.Name = "btnAddCatalog";
            this.btnAddCatalog.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnAddCatalog.Size = new System.Drawing.Size(206, 40);
            this.btnAddCatalog.TabIndex = 13;
            this.btnAddCatalog.Text = "Add from online catalog";
            this.btnAddCatalog.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAddCatalog.UseVisualStyleBackColor = false;
            this.btnAddCatalog.Click += new System.EventHandler(this.btnAddCatalog_Click);
            this.btnAddCatalog.MouseLeave += new System.EventHandler(this.btnAddCatalog_MouseLeave);
            this.btnAddCatalog.MouseHover += new System.EventHandler(this.btnAddCatalog_MouseHover);
            // 
            // btnImport
            // 
            this.btnImport.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnImport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnImport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnImport.FlatAppearance.BorderSize = 0;
            this.btnImport.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.btnImport.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.btnImport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnImport.ForeColor = System.Drawing.Color.White;
            this.btnImport.Location = new System.Drawing.Point(0, 49);
            this.btnImport.Margin = new System.Windows.Forms.Padding(4);
            this.btnImport.Name = "btnImport";
            this.btnImport.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.btnImport.Size = new System.Drawing.Size(206, 40);
            this.btnImport.TabIndex = 14;
            this.btnImport.Text = "Import from file";
            this.btnImport.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.UseVisualStyleBackColor = false;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            this.btnImport.MouseLeave += new System.EventHandler(this.btnImport_MouseLeave);
            this.btnImport.MouseHover += new System.EventHandler(this.btnImport_MouseHover);
            // 
            // plBackground
            // 
            this.plBackground.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.plBackground.Controls.Add(this.btnImport);
            this.plBackground.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plBackground.Location = new System.Drawing.Point(0, 0);
            this.plBackground.Margin = new System.Windows.Forms.Padding(4, 8, 4, 8);
            this.plBackground.Name = "plBackground";
            this.plBackground.Size = new System.Drawing.Size(206, 96);
            this.plBackground.TabIndex = 15;
            // 
            // MaterialMenuStrip
            // 
            this.Controls.Add(this.btnAddCatalog);
            this.Controls.Add(this.plBackground);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Location = new System.Drawing.Point(325, 505);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "MaterialMenuStrip";
            this.Size = new System.Drawing.Size(206, 96);
            this.plBackground.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnAddCatalog;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Panel plBackground;
    }
}