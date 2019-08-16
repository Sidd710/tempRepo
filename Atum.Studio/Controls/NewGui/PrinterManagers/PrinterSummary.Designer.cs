namespace Atum.Studio.Controls.NewGUI.PrinterManagers
{
    partial class PrinterSummary
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
            this.txtprinterText = new Atum.Studio.Controls.NewGUI.NewGUIInputBoxReadonly();
            this.SuspendLayout();
            // 
            // txtprinterText
            // 
            this.txtprinterText.BackColor = System.Drawing.Color.White;
            this.txtprinterText.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.txtprinterText.Location = new System.Drawing.Point(0, 5);
            this.txtprinterText.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txtprinterText.Name = "txtprinterText";
            this.txtprinterText.Selected = false;
            this.txtprinterText.Size = new System.Drawing.Size(424, 61);
            this.txtprinterText.TabIndex = 1;
            this.txtprinterText.TabStop = false;
            this.txtprinterText.TextValue = "";
            // 
            // PrinterSummary
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.txtprinterText);
            this.Font = new System.Drawing.Font("Montserrat", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "PrinterSummary";
            this.Size = new System.Drawing.Size(428, 65);
            this.ResumeLayout(false);

        }

        #endregion

        private NewGUIInputBoxReadonly txtprinterText;
    }
}
