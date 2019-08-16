namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter
{
    partial class TrapeziumInputArrowBelow
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
            this.cplDisplayName = new Atum.Studio.Controls.NewGui.NewGUITextboxPanel();
            this.txtInputValue = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.plArrow = new System.Windows.Forms.Panel();
            this.plValidationColor = new System.Windows.Forms.Panel();
            this.SuspendLayout();
            // 
            // cplDisplayName
            // 
            this.cplDisplayName.BackColor = System.Drawing.Color.White;
            this.cplDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cplDisplayName.Location = new System.Drawing.Point(0, 0);
            this.cplDisplayName.Margin = new System.Windows.Forms.Padding(3, 3, 24, 0);
            this.cplDisplayName.Name = "cplDisplayName";
            this.cplDisplayName.Padding = new System.Windows.Forms.Padding(1);
            this.cplDisplayName.Size = new System.Drawing.Size(73, 32);
            this.cplDisplayName.TabIndex = 25;
            // 
            // txtInputValue
            // 
            this.txtInputValue.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtInputValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtInputValue.ForeColor = System.Drawing.Color.Black;
            this.txtInputValue.Location = new System.Drawing.Point(8, 8);
            this.txtInputValue.MaxLength = 7;
            this.txtInputValue.Name = "txtInputValue";
            this.txtInputValue.Size = new System.Drawing.Size(60, 16);
            this.txtInputValue.TabIndex = 26;
            this.txtInputValue.Text = "0";
            this.txtInputValue.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtInputValue.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtInputValue_MouseClick);
            this.txtInputValue.TextChanged += new System.EventHandler(this.txtInputValue_TextChanged);
            this.txtInputValue.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtInputValue_KeyPress);
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(158)))), ((int)(((byte)(159)))), ((int)(((byte)(158)))));
            this.label1.Location = new System.Drawing.Point(61, 0);
            this.label1.Name = "label1";
            this.label1.Padding = new System.Windows.Forms.Padding(0, 0, 6, 0);
            this.label1.Size = new System.Drawing.Size(43, 32);
            this.label1.TabIndex = 27;
            this.label1.Text = "mm";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // plArrow
            // 
            this.plArrow.BackgroundImage = global::Atum.Studio.Properties.Resources.printer_calibration_input_arrow;
            this.plArrow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plArrow.Location = new System.Drawing.Point(0, 32);
            this.plArrow.Margin = new System.Windows.Forms.Padding(0);
            this.plArrow.Name = "plArrow";
            this.plArrow.Size = new System.Drawing.Size(104, 12);
            this.plArrow.TabIndex = 28;
            // 
            // plValidationColor
            // 
            this.plValidationColor.BackColor = System.Drawing.Color.White;
            this.plValidationColor.Dock = System.Windows.Forms.DockStyle.Top;
            this.plValidationColor.Location = new System.Drawing.Point(0, 0);
            this.plValidationColor.Name = "plValidationColor";
            this.plValidationColor.Size = new System.Drawing.Size(104, 2);
            this.plValidationColor.TabIndex = 30;
            // 
            // TrapeziumInputArrowBelow
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Controls.Add(this.plValidationColor);
            this.Controls.Add(this.plArrow);
            this.Controls.Add(this.txtInputValue);
            this.Controls.Add(this.cplDisplayName);
            this.Controls.Add(this.label1);
            this.Name = "TrapeziumInputArrowBelow";
            this.Size = new System.Drawing.Size(104, 44);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private NewGui.NewGUITextboxPanel cplDisplayName;
        private System.Windows.Forms.TextBox txtInputValue;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel plArrow;
        private System.Windows.Forms.Panel plValidationColor;
    }
}
