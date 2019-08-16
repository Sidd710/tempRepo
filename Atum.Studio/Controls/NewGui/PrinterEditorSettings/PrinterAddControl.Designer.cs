using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    partial class PrinterAddControl
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
            this.plPrinterAdd = new System.Windows.Forms.Panel();
            this.txtSerialNumber = new System.Windows.Forms.TextBox();
            this.cplSerialNumber = new Atum.Studio.Controls.NewGui.NewGUITextboxPanel();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.cplDisplayName = new Atum.Studio.Controls.NewGui.NewGUITextboxPanel();
            this.lblResolution = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblResolutionAsterisk = new System.Windows.Forms.Label();
            this.lblDisplayNameAsterisk = new System.Windows.Forms.Label();
            this.lblSerialNumber = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblSerialAsterisk = new System.Windows.Forms.Label();
            this.cbResolution = new System.Windows.Forms.ComboBox();
            this.btnSave = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnCancel = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plDescription = new System.Windows.Forms.Panel();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.newGUILabel4 = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.newGUILabel1 = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.plPrinterAdd.SuspendLayout();
            this.plDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // plPrinterAdd
            // 
            this.plPrinterAdd.Controls.Add(this.txtSerialNumber);
            this.plPrinterAdd.Controls.Add(this.cplSerialNumber);
            this.plPrinterAdd.Controls.Add(this.txtDisplayName);
            this.plPrinterAdd.Controls.Add(this.cplDisplayName);
            this.plPrinterAdd.Controls.Add(this.lblResolution);
            this.plPrinterAdd.Controls.Add(this.lblResolutionAsterisk);
            this.plPrinterAdd.Controls.Add(this.lblDisplayNameAsterisk);
            this.plPrinterAdd.Controls.Add(this.lblSerialNumber);
            this.plPrinterAdd.Controls.Add(this.lblSerialAsterisk);
            this.plPrinterAdd.Controls.Add(this.cbResolution);
            this.plPrinterAdd.Controls.Add(this.btnSave);
            this.plPrinterAdd.Controls.Add(this.btnCancel);
            this.plPrinterAdd.Controls.Add(this.plDescription);
            this.plPrinterAdd.Controls.Add(this.newGUILabel4);
            this.plPrinterAdd.Controls.Add(this.newGUILabel1);
            this.plPrinterAdd.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plPrinterAdd.Location = new System.Drawing.Point(0, 0);
            this.plPrinterAdd.Name = "plPrinterAdd";
            this.plPrinterAdd.Size = new System.Drawing.Size(480, 546);
            this.plPrinterAdd.TabIndex = 11;
            // 
            // txtSerialNumber
            // 
            this.txtSerialNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSerialNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtSerialNumber.Location = new System.Drawing.Point(29, 341);
            this.txtSerialNumber.Name = "txtSerialNumber";
            this.txtSerialNumber.Size = new System.Drawing.Size(420, 16);
            this.txtSerialNumber.TabIndex = 2;
            this.txtSerialNumber.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSerialNumber_MouseClick);
            this.txtSerialNumber.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSerialNumber_KeyUp);
            // 
            // cplSerialNumber
            // 
            this.cplSerialNumber.BackColor = System.Drawing.Color.White;
            this.cplSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cplSerialNumber.Location = new System.Drawing.Point(21, 330);
            this.cplSerialNumber.Name = "cplSerialNumber";
            this.cplSerialNumber.Padding = new System.Windows.Forms.Padding(1);
            this.cplSerialNumber.Size = new System.Drawing.Size(434, 40);
            this.cplSerialNumber.TabIndex = 12;
            this.cplSerialNumber.TabStop = false;
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtDisplayName.Location = new System.Drawing.Point(29, 56);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(420, 16);
            this.txtDisplayName.TabIndex = 0;
            this.txtDisplayName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDisplayName_KeyUp);
            // 
            // cplDisplayName
            // 
            this.cplDisplayName.BackColor = System.Drawing.Color.White;
            this.cplDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cplDisplayName.Location = new System.Drawing.Point(24, 45);
            this.cplDisplayName.Margin = new System.Windows.Forms.Padding(3, 3, 25, 3);
            this.cplDisplayName.Name = "cplDisplayName";
            this.cplDisplayName.Padding = new System.Windows.Forms.Padding(1);
            this.cplDisplayName.Size = new System.Drawing.Size(431, 40);
            this.cplDisplayName.TabIndex = 10;
            this.cplDisplayName.TabStop = false;
            this.cplDisplayName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.cplDisplayName_MouseClick);
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.BackColor = System.Drawing.Color.Transparent;
            this.lblResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblResolution.Location = new System.Drawing.Point(21, 223);
            this.lblResolution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(85, 17);
            this.lblResolution.TabIndex = 11;
            this.lblResolution.Text = "Resolution";
            this.lblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResolutionAsterisk
            // 
            this.lblResolutionAsterisk.BackColor = System.Drawing.Color.Transparent;
            this.lblResolutionAsterisk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.lblResolutionAsterisk.Location = new System.Drawing.Point(112, 223);
            this.lblResolutionAsterisk.Margin = new System.Windows.Forms.Padding(0);
            this.lblResolutionAsterisk.Name = "lblResolutionAsterisk";
            this.lblResolutionAsterisk.Size = new System.Drawing.Size(15, 18);
            this.lblResolutionAsterisk.TabIndex = 24;
            this.lblResolutionAsterisk.Text = "*";
            // 
            // lblDisplayNameAsterisk
            // 
            this.lblDisplayNameAsterisk.BackColor = System.Drawing.Color.Transparent;
            this.lblDisplayNameAsterisk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.lblDisplayNameAsterisk.Location = new System.Drawing.Point(133, 19);
            this.lblDisplayNameAsterisk.Margin = new System.Windows.Forms.Padding(0);
            this.lblDisplayNameAsterisk.Name = "lblDisplayNameAsterisk";
            this.lblDisplayNameAsterisk.Size = new System.Drawing.Size(15, 18);
            this.lblDisplayNameAsterisk.TabIndex = 23;
            this.lblDisplayNameAsterisk.Text = "*";
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSerialNumber.Location = new System.Drawing.Point(21, 304);
            this.lblSerialNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(109, 17);
            this.lblSerialNumber.TabIndex = 14;
            this.lblSerialNumber.Text = "Serial number";
            this.lblSerialNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSerialAsterisk
            // 
            this.lblSerialAsterisk.BackColor = System.Drawing.Color.Transparent;
            this.lblSerialAsterisk.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(24)))), ((int)(((byte)(0)))));
            this.lblSerialAsterisk.Location = new System.Drawing.Point(138, 304);
            this.lblSerialAsterisk.Margin = new System.Windows.Forms.Padding(0);
            this.lblSerialAsterisk.Name = "lblSerialAsterisk";
            this.lblSerialAsterisk.Size = new System.Drawing.Size(15, 18);
            this.lblSerialAsterisk.TabIndex = 22;
            this.lblSerialAsterisk.Text = "*";
            // 
            // cbResolution
            // 
            this.cbResolution.BackColor = System.Drawing.Color.White;
            this.cbResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbResolution.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cbResolution.ForeColor = System.Drawing.Color.Black;
            this.cbResolution.FormattingEnabled = true;
            this.cbResolution.IntegralHeight = false;
            this.cbResolution.ItemHeight = 17;
            this.cbResolution.Location = new System.Drawing.Point(27, 248);
            this.cbResolution.Name = "cbResolution";
            this.cbResolution.Size = new System.Drawing.Size(428, 25);
            this.cbResolution.TabIndex = 1;
            this.cbResolution.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.cbResolution_DrawItem);
            this.cbResolution.SelectedIndexChanged += new System.EventHandler(this.cbResolution_SelectedIndexChanged);
            // 
            // btnSave
            // 
            this.btnSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnSave.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSave.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnSave.ForeColor = System.Drawing.Color.White;
            this.btnSave.Location = new System.Drawing.Point(344, 402);
            this.btnSave.Margin = new System.Windows.Forms.Padding(0);
            this.btnSave.Name = "btnSave";
            this.btnSave.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnSave.Radius = 20;
            this.btnSave.SingleBorder = false;
            this.btnSave.Size = new System.Drawing.Size(112, 40);
            this.btnSave.TabIndex = 4;
            this.btnSave.Text = "Save";
            this.btnSave.UseVisualStyleBackColor = false;
            this.btnSave.EnabledChanged += new System.EventHandler(this.btnSave_EnabledChanged);
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCancel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCancel.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCancel.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCancel.Location = new System.Drawing.Point(24, 402);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnCancel.Radius = 20;
            this.btnCancel.SingleBorder = true;
            this.btnCancel.Size = new System.Drawing.Size(114, 42);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = false;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // plDescription
            // 
            this.plDescription.BackColor = System.Drawing.Color.White;
            this.plDescription.Controls.Add(this.txtDescription);
            this.plDescription.Location = new System.Drawing.Point(24, 127);
            this.plDescription.Name = "plDescription";
            this.plDescription.Size = new System.Drawing.Size(431, 80);
            this.plDescription.TabIndex = 0;
            // 
            // txtDescription
            // 
            this.txtDescription.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDescription.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(3, 7);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(427, 66);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtDescription_MouseClick);
            // 
            // newGUILabel4
            // 
            this.newGUILabel4.AutoSize = true;
            this.newGUILabel4.BackColor = System.Drawing.Color.Transparent;
            this.newGUILabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.newGUILabel4.Location = new System.Drawing.Point(21, 101);
            this.newGUILabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.newGUILabel4.Name = "newGUILabel4";
            this.newGUILabel4.Size = new System.Drawing.Size(90, 17);
            this.newGUILabel4.TabIndex = 16;
            this.newGUILabel4.Text = "Description";
            this.newGUILabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newGUILabel1
            // 
            this.newGUILabel1.AutoSize = true;
            this.newGUILabel1.BackColor = System.Drawing.Color.Transparent;
            this.newGUILabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.newGUILabel1.Location = new System.Drawing.Point(21, 19);
            this.newGUILabel1.Margin = new System.Windows.Forms.Padding(0);
            this.newGUILabel1.Name = "newGUILabel1";
            this.newGUILabel1.Size = new System.Drawing.Size(105, 17);
            this.newGUILabel1.TabIndex = 10;
            this.newGUILabel1.Text = "Display name";
            this.newGUILabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PrinterAddControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.plPrinterAdd);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrinterAddControl";
            this.Size = new System.Drawing.Size(480, 546);
            this.Load += new System.EventHandler(this.PrinterAddControl_Load);
            this.plPrinterAdd.ResumeLayout(false);
            this.plPrinterAdd.PerformLayout();
            this.plDescription.ResumeLayout(false);
            this.plDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plPrinterAdd;
        private System.Windows.Forms.TextBox txtSerialNumber;
        private RoundedButton btnSave;
        private RoundedButton btnCancel;
        private System.Windows.Forms.Panel plDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private NewGUILabel newGUILabel4;
        private NewGUILabel lblSerialNumber;
        private NewGUILabel lblResolution;
        private NewGUILabel newGUILabel1;
        private System.Windows.Forms.ComboBox cbResolution;
        private System.Windows.Forms.Label lblSerialAsterisk;
        private System.Windows.Forms.Label lblDisplayNameAsterisk;
        private System.Windows.Forms.Label lblResolutionAsterisk;
        private System.Windows.Forms.TextBox txtDisplayName;
        private NewGUITextboxPanel cplDisplayName;
        private NewGUITextboxPanel cplSerialNumber;
    }
}
