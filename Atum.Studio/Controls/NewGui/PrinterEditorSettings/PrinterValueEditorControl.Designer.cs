using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    partial class PrinterValueEditorControl
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
            this.plPrinterValueEditor = new System.Windows.Forms.Panel();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.cplDisplayName = new Atum.Studio.Controls.NewGui.NewGUITextboxPanel();
            this.btnExportSettings = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.btnCalibrate = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plDescription = new System.Windows.Forms.Panel();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.newGUILabel4 = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblSerialNumberValue = new System.Windows.Forms.Label();
            this.lblSerialNumber = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.lblResolutionValue = new System.Windows.Forms.Label();
            this.lblResolution = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.newGUILabel1 = new Atum.Studio.Controls.NewGui.NewGUILabel();
            this.plPrinterValueEditor.SuspendLayout();
            this.plDescription.SuspendLayout();
            this.SuspendLayout();
            // 
            // plPrinterValueEditor
            // 
            this.plPrinterValueEditor.Controls.Add(this.txtDisplayName);
            this.plPrinterValueEditor.Controls.Add(this.cplDisplayName);
            this.plPrinterValueEditor.Controls.Add(this.btnExportSettings);
            this.plPrinterValueEditor.Controls.Add(this.btnCalibrate);
            this.plPrinterValueEditor.Controls.Add(this.plDescription);
            this.plPrinterValueEditor.Controls.Add(this.newGUILabel4);
            this.plPrinterValueEditor.Controls.Add(this.lblSerialNumberValue);
            this.plPrinterValueEditor.Controls.Add(this.lblSerialNumber);
            this.plPrinterValueEditor.Controls.Add(this.lblResolutionValue);
            this.plPrinterValueEditor.Controls.Add(this.lblResolution);
            this.plPrinterValueEditor.Controls.Add(this.newGUILabel1);
            this.plPrinterValueEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plPrinterValueEditor.Location = new System.Drawing.Point(0, 0);
            this.plPrinterValueEditor.Name = "plPrinterValueEditor";
            this.plPrinterValueEditor.Size = new System.Drawing.Size(480, 546);
            this.plPrinterValueEditor.TabIndex = 0;
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDisplayName.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.txtDisplayName.Location = new System.Drawing.Point(25, 55);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(424, 28);
            this.txtDisplayName.TabIndex = 0;
            this.txtDisplayName.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtDisplayName_MouseClick);
            this.txtDisplayName.TextChanged += new System.EventHandler(this.txtDisplayName_TextChanged);
            this.txtDisplayName.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDisplayName_KeyPress);
            this.txtDisplayName.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtDisplayName_KeyUp);
            // 
            // cplDisplayName
            // 
            this.cplDisplayName.BackColor = System.Drawing.Color.White;
            this.cplDisplayName.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.cplDisplayName.Location = new System.Drawing.Point(16, 45);
            this.cplDisplayName.Margin = new System.Windows.Forms.Padding(3, 3, 13, 3);
            this.cplDisplayName.Name = "cplDisplayName";
            this.cplDisplayName.Padding = new System.Windows.Forms.Padding(1);
            this.cplDisplayName.Size = new System.Drawing.Size(451, 40);
            this.cplDisplayName.TabIndex = 24;
            // 
            // btnExportSettings
            // 
            this.btnExportSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExportSettings.BackColor = System.Drawing.Color.Transparent;
            this.btnExportSettings.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnExportSettings.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExportSettings.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnExportSettings.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExportSettings.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExportSettings.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnExportSettings.Location = new System.Drawing.Point(307, 359);
            this.btnExportSettings.Margin = new System.Windows.Forms.Padding(0, 0, 13, 0);
            this.btnExportSettings.Name = "btnExportSettings";
            this.btnExportSettings.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnExportSettings.Radius = 20;
            this.btnExportSettings.SingleBorder = true;
            this.btnExportSettings.Size = new System.Drawing.Size(160, 40);
            this.btnExportSettings.TabIndex = 3;
            this.btnExportSettings.Text = "Export Settings";
            this.btnExportSettings.UseVisualStyleBackColor = false;
            this.btnExportSettings.Click += new System.EventHandler(this.btnExportSettings_Click);
            // 
            // btnCalibrate
            // 
            this.btnCalibrate.BackColor = System.Drawing.Color.Transparent;
            this.btnCalibrate.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCalibrate.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCalibrate.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnCalibrate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCalibrate.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCalibrate.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCalibrate.Location = new System.Drawing.Point(16, 359);
            this.btnCalibrate.Margin = new System.Windows.Forms.Padding(0);
            this.btnCalibrate.Name = "btnCalibrate";
            this.btnCalibrate.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnCalibrate.Radius = 20;
            this.btnCalibrate.SingleBorder = true;
            this.btnCalibrate.Size = new System.Drawing.Size(162, 42);
            this.btnCalibrate.TabIndex = 2;
            this.btnCalibrate.Text = "Calibrate";
            this.btnCalibrate.UseVisualStyleBackColor = false;
            this.btnCalibrate.Click += new System.EventHandler(this.btnCalibrate_Click);
            // 
            // plDescription
            // 
            this.plDescription.BackColor = System.Drawing.Color.White;
            this.plDescription.Controls.Add(this.txtDescription);
            this.plDescription.Location = new System.Drawing.Point(18, 127);
            this.plDescription.Name = "plDescription";
            this.plDescription.Size = new System.Drawing.Size(443, 80);
            this.plDescription.TabIndex = 1;
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
            this.txtDescription.Size = new System.Drawing.Size(439, 66);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtDescription_MouseClick);
            this.txtDescription.TextChanged += new System.EventHandler(this.txtDescription_TextChanged);
            this.txtDescription.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtDescription_KeyPress);
            // 
            // newGUILabel4
            // 
            this.newGUILabel4.AutoSize = true;
            this.newGUILabel4.BackColor = System.Drawing.Color.Transparent;
            this.newGUILabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.newGUILabel4.Location = new System.Drawing.Point(13, 101);
            this.newGUILabel4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.newGUILabel4.Name = "newGUILabel4";
            this.newGUILabel4.Size = new System.Drawing.Size(90, 17);
            this.newGUILabel4.TabIndex = 16;
            this.newGUILabel4.Text = "Description";
            this.newGUILabel4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSerialNumberValue
            // 
            this.lblSerialNumberValue.AutoSize = true;
            this.lblSerialNumberValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSerialNumberValue.ForeColor = System.Drawing.Color.Black;
            this.lblSerialNumberValue.Location = new System.Drawing.Point(13, 309);
            this.lblSerialNumberValue.Name = "lblSerialNumberValue";
            this.lblSerialNumberValue.Size = new System.Drawing.Size(67, 17);
            this.lblSerialNumberValue.TabIndex = 15;
            this.lblSerialNumberValue.Text = "DLP1234";
            this.lblSerialNumberValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblSerialNumber
            // 
            this.lblSerialNumber.AutoSize = true;
            this.lblSerialNumber.BackColor = System.Drawing.Color.Transparent;
            this.lblSerialNumber.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblSerialNumber.Location = new System.Drawing.Point(13, 283);
            this.lblSerialNumber.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSerialNumber.Name = "lblSerialNumber";
            this.lblSerialNumber.Size = new System.Drawing.Size(109, 17);
            this.lblSerialNumber.TabIndex = 14;
            this.lblSerialNumber.Text = "Serial number";
            this.lblSerialNumber.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblResolutionValue
            // 
            this.lblResolutionValue.AutoSize = true;
            this.lblResolutionValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblResolutionValue.ForeColor = System.Drawing.Color.Black;
            this.lblResolutionValue.Location = new System.Drawing.Point(13, 249);
            this.lblResolutionValue.Name = "lblResolutionValue";
            this.lblResolutionValue.Size = new System.Drawing.Size(32, 17);
            this.lblResolutionValue.TabIndex = 12;
            this.lblResolutionValue.Text = "100";
            this.lblResolutionValue.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // lblResolution
            // 
            this.lblResolution.AutoSize = true;
            this.lblResolution.BackColor = System.Drawing.Color.Transparent;
            this.lblResolution.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblResolution.Location = new System.Drawing.Point(13, 223);
            this.lblResolution.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblResolution.Name = "lblResolution";
            this.lblResolution.Size = new System.Drawing.Size(85, 17);
            this.lblResolution.TabIndex = 11;
            this.lblResolution.Text = "Resolution";
            this.lblResolution.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // newGUILabel1
            // 
            this.newGUILabel1.AutoSize = true;
            this.newGUILabel1.BackColor = System.Drawing.Color.Transparent;
            this.newGUILabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.newGUILabel1.Location = new System.Drawing.Point(13, 19);
            this.newGUILabel1.Margin = new System.Windows.Forms.Padding(0);
            this.newGUILabel1.Name = "newGUILabel1";
            this.newGUILabel1.Size = new System.Drawing.Size(105, 17);
            this.newGUILabel1.TabIndex = 10;
            this.newGUILabel1.Text = "Display name";
            this.newGUILabel1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // PrinterValueEditorControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.Controls.Add(this.plPrinterValueEditor);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PrinterValueEditorControl";
            this.Size = new System.Drawing.Size(480, 546);
            this.Load += new System.EventHandler(this.PrinterValueEditorControl_Load);
            this.plPrinterValueEditor.ResumeLayout(false);
            this.plPrinterValueEditor.PerformLayout();
            this.plDescription.ResumeLayout(false);
            this.plDescription.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plPrinterValueEditor;
        private System.Windows.Forms.Panel plDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private NewGUILabel newGUILabel4;
        private System.Windows.Forms.Label lblSerialNumberValue;
        private NewGUILabel lblSerialNumber;
        private System.Windows.Forms.Label lblResolutionValue;
        private NewGUILabel lblResolution;
        private NewGUILabel newGUILabel1;
        private RoundedButton btnExportSettings;
        private RoundedButton btnCalibrate;
        private NewGUITextboxPanel cplDisplayName;
        private System.Windows.Forms.TextBox txtDisplayName;
    }
}
