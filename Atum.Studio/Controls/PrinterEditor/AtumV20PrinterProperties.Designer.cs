namespace Atum.Studio.Controls.PrinterEditor
{
    partial class AtumV20PrinterProperties
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
            this.components = new System.ComponentModel.Container();
            this.lblHeader = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.tbPrinterSettings = new System.Windows.Forms.TabControl();
            this.Properties = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rbtn50Micron = new System.Windows.Forms.RadioButton();
            this.rbtn75Micron = new System.Windows.Forms.RadioButton();
            this.rbtn100Micron = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.lblDescription = new System.Windows.Forms.Label();
            this.lblDisplayName = new System.Windows.Forms.Label();
            this.txtDisplayName = new System.Windows.Forms.TextBox();
            this.Calibration = new System.Windows.Forms.TabPage();
            this.lblSwitchMode = new System.Windows.Forms.LinkLabel();
            this.atumPrinterCalibration1 = new Atum.Studio.Controls.PrinterEditor.AtumPrinterCalibration();
            this.AdvancedCalibration = new System.Windows.Forms.TabPage();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tbValues = new System.Windows.Forms.TabPage();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.dgLensWarpCorrectionValues = new System.Windows.Forms.DataGridView();
            this.Number = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Horizontal = new Atum.Studio.Controls.DataGridView.DataGridViewNumericUpDownCell.NumericUpDownColumn();
            this.Vertical = new Atum.Studio.Controls.DataGridView.DataGridViewNumericUpDownCell.NumericUpDownColumn();
            this.numberDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.horizontalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.verticalDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.lensWarpCorrectionItemsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.tbGeneratePrintjob = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.lblManufacturer = new System.Windows.Forms.Label();
            this.cbAdvancedCalibrationMaterialProduct = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cbAdvancedCalibrationMaterialSupplier = new System.Windows.Forms.ComboBox();
            this.panel3 = new System.Windows.Forms.Panel();
            this.plAdvancedCalibrationDriveLetters = new System.Windows.Forms.Panel();
            this.btnAdvancedCalibrationRefreshDrives = new System.Windows.Forms.Button();
            this.cbAdvancedCalibrationDriveletters = new System.Windows.Forms.ComboBox();
            this.btnRefreshUSBDisks = new System.Windows.Forms.Button();
            this.cbUSBDriveLetters = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.btnSaveAdvancedCalibration = new System.Windows.Forms.Button();
            this.lblHeader.SuspendLayout();
            this.tbPrinterSettings.SuspendLayout();
            this.Properties.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.Calibration.SuspendLayout();
            this.AdvancedCalibration.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tbValues.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLensWarpCorrectionValues)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.lensWarpCorrectionItemsBindingSource)).BeginInit();
            this.tbGeneratePrintjob.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.plAdvancedCalibrationDriveLetters.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblHeader
            // 
            this.lblHeader.Controls.Add(this.label1);
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblHeader.Location = new System.Drawing.Point(3, 3);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(446, 39);
            this.lblHeader.TabIndex = 3;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(9, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(93, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Atum V2.0";
            // 
            // tbPrinterSettings
            // 
            this.tbPrinterSettings.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbPrinterSettings.Controls.Add(this.Properties);
            this.tbPrinterSettings.Controls.Add(this.Calibration);
            this.tbPrinterSettings.Controls.Add(this.AdvancedCalibration);
            this.tbPrinterSettings.Location = new System.Drawing.Point(3, 48);
            this.tbPrinterSettings.Name = "tbPrinterSettings";
            this.tbPrinterSettings.SelectedIndex = 0;
            this.tbPrinterSettings.Size = new System.Drawing.Size(446, 392);
            this.tbPrinterSettings.TabIndex = 2;
            this.tbPrinterSettings.Selecting += new System.Windows.Forms.TabControlCancelEventHandler(this.tabControl1_Selecting);
            // 
            // Properties
            // 
            this.Properties.Controls.Add(this.tableLayoutPanel1);
            this.Properties.Location = new System.Drawing.Point(4, 22);
            this.Properties.Name = "Properties";
            this.Properties.Padding = new System.Windows.Forms.Padding(3);
            this.Properties.Size = new System.Drawing.Size(438, 366);
            this.Properties.TabIndex = 0;
            this.Properties.Text = "Printer Properties";
            this.Properties.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.panel2, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.label2, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtDescription, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDescription, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblDisplayName, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtDisplayName, 1, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(3, 3);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel1.Size = new System.Drawing.Size(432, 360);
            this.tableLayoutPanel1.TabIndex = 24;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.White;
            this.panel2.Controls.Add(this.rbtn50Micron);
            this.panel2.Controls.Add(this.rbtn75Micron);
            this.panel2.Controls.Add(this.rbtn100Micron);
            this.panel2.Location = new System.Drawing.Point(102, 101);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(200, 100);
            this.panel2.TabIndex = 25;
            // 
            // rbtn50Micron
            // 
            this.rbtn50Micron.AutoSize = true;
            this.rbtn50Micron.Location = new System.Drawing.Point(3, 3);
            this.rbtn50Micron.Name = "rbtn50Micron";
            this.rbtn50Micron.Size = new System.Drawing.Size(71, 17);
            this.rbtn50Micron.TabIndex = 23;
            this.rbtn50Micron.TabStop = true;
            this.rbtn50Micron.Text = "50 micron";
            this.rbtn50Micron.UseVisualStyleBackColor = true;
            this.rbtn50Micron.CheckedChanged += new System.EventHandler(this.rbtnMicron_CheckedChanged);
            // 
            // rbtn75Micron
            // 
            this.rbtn75Micron.AutoSize = true;
            this.rbtn75Micron.Location = new System.Drawing.Point(3, 26);
            this.rbtn75Micron.Name = "rbtn75Micron";
            this.rbtn75Micron.Size = new System.Drawing.Size(71, 17);
            this.rbtn75Micron.TabIndex = 21;
            this.rbtn75Micron.TabStop = true;
            this.rbtn75Micron.Text = "75 micron";
            this.rbtn75Micron.UseVisualStyleBackColor = true;
            this.rbtn75Micron.CheckedChanged += new System.EventHandler(this.rbtnMicron_CheckedChanged);
            // 
            // rbtn100Micron
            // 
            this.rbtn100Micron.AutoSize = true;
            this.rbtn100Micron.Location = new System.Drawing.Point(3, 49);
            this.rbtn100Micron.Name = "rbtn100Micron";
            this.rbtn100Micron.Size = new System.Drawing.Size(77, 17);
            this.rbtn100Micron.TabIndex = 22;
            this.rbtn100Micron.TabStop = true;
            this.rbtn100Micron.Text = "100 micron";
            this.rbtn100Micron.UseVisualStyleBackColor = true;
            this.rbtn100Micron.CheckedChanged += new System.EventHandler(this.rbtnMicron_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(3, 98);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Printer Resolution:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(102, 29);
            this.txtDescription.Multiline = true;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(236, 66);
            this.txtDescription.TabIndex = 11;
            // 
            // lblDescription
            // 
            this.lblDescription.AutoSize = true;
            this.lblDescription.Location = new System.Drawing.Point(3, 26);
            this.lblDescription.Name = "lblDescription";
            this.lblDescription.Size = new System.Drawing.Size(63, 13);
            this.lblDescription.TabIndex = 12;
            this.lblDescription.Text = "Description:";
            // 
            // lblDisplayName
            // 
            this.lblDisplayName.AutoSize = true;
            this.lblDisplayName.Location = new System.Drawing.Point(3, 0);
            this.lblDisplayName.Name = "lblDisplayName";
            this.lblDisplayName.Size = new System.Drawing.Size(73, 13);
            this.lblDisplayName.TabIndex = 14;
            this.lblDisplayName.Text = "Display name:";
            // 
            // txtDisplayName
            // 
            this.txtDisplayName.Location = new System.Drawing.Point(102, 3);
            this.txtDisplayName.Name = "txtDisplayName";
            this.txtDisplayName.Size = new System.Drawing.Size(236, 20);
            this.txtDisplayName.TabIndex = 13;
            // 
            // Calibration
            // 
            this.Calibration.Controls.Add(this.lblSwitchMode);
            this.Calibration.Controls.Add(this.atumPrinterCalibration1);
            this.Calibration.Location = new System.Drawing.Point(4, 22);
            this.Calibration.Name = "Calibration";
            this.Calibration.Padding = new System.Windows.Forms.Padding(3);
            this.Calibration.Size = new System.Drawing.Size(438, 366);
            this.Calibration.TabIndex = 1;
            this.Calibration.Text = "Basic Calibration";
            this.Calibration.UseVisualStyleBackColor = true;
            // 
            // lblSwitchMode
            // 
            this.lblSwitchMode.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSwitchMode.AutoSize = true;
            this.lblSwitchMode.LinkColor = System.Drawing.Color.Black;
            this.lblSwitchMode.Location = new System.Drawing.Point(275, 347);
            this.lblSwitchMode.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.lblSwitchMode.Name = "lblSwitchMode";
            this.lblSwitchMode.Size = new System.Drawing.Size(157, 13);
            this.lblSwitchMode.TabIndex = 1;
            this.lblSwitchMode.TabStop = true;
            this.lblSwitchMode.Text = "Switch to: Advanced Correction";
            this.lblSwitchMode.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblSwitchMode.Visible = false;
            this.lblSwitchMode.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.lblSwitchMode_LinkClicked);
            // 
            // atumPrinterCalibration1
            // 
            this.atumPrinterCalibration1.AdvancedMode = false;
            this.atumPrinterCalibration1.DataSource = null;
            this.atumPrinterCalibration1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.atumPrinterCalibration1.Location = new System.Drawing.Point(3, 3);
            this.atumPrinterCalibration1.Name = "atumPrinterCalibration1";
            this.atumPrinterCalibration1.Size = new System.Drawing.Size(432, 360);
            this.atumPrinterCalibration1.TabIndex = 0;
            // 
            // AdvancedCalibration
            // 
            this.AdvancedCalibration.Controls.Add(this.tabControl1);
            this.AdvancedCalibration.Location = new System.Drawing.Point(4, 22);
            this.AdvancedCalibration.Margin = new System.Windows.Forms.Padding(0);
            this.AdvancedCalibration.Name = "AdvancedCalibration";
            this.AdvancedCalibration.Padding = new System.Windows.Forms.Padding(3);
            this.AdvancedCalibration.Size = new System.Drawing.Size(438, 366);
            this.AdvancedCalibration.TabIndex = 2;
            this.AdvancedCalibration.Text = "Advanced Calibration";
            this.AdvancedCalibration.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tbValues);
            this.tabControl1.Controls.Add(this.tbGeneratePrintjob);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(432, 360);
            this.tabControl1.TabIndex = 2;
            // 
            // tbValues
            // 
            this.tbValues.Controls.Add(this.linkLabel1);
            this.tbValues.Controls.Add(this.dgLensWarpCorrectionValues);
            this.tbValues.Location = new System.Drawing.Point(4, 22);
            this.tbValues.Name = "tbValues";
            this.tbValues.Padding = new System.Windows.Forms.Padding(3);
            this.tbValues.Size = new System.Drawing.Size(424, 334);
            this.tbValues.TabIndex = 0;
            this.tbValues.Text = "Measurements";
            this.tbValues.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            this.linkLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(284, 315);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(3, 0, 3, 3);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(134, 13);
            this.linkLabel1.TabIndex = 2;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "Switch to: Basic Correction";
            this.linkLabel1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // dgLensWarpCorrectionValues
            // 
            this.dgLensWarpCorrectionValues.AllowUserToAddRows = false;
            this.dgLensWarpCorrectionValues.AllowUserToDeleteRows = false;
            this.dgLensWarpCorrectionValues.AllowUserToResizeColumns = false;
            this.dgLensWarpCorrectionValues.AllowUserToResizeRows = false;
            this.dgLensWarpCorrectionValues.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgLensWarpCorrectionValues.AutoGenerateColumns = false;
            this.dgLensWarpCorrectionValues.BackgroundColor = System.Drawing.Color.White;
            this.dgLensWarpCorrectionValues.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgLensWarpCorrectionValues.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Number,
            this.Horizontal,
            this.Vertical,
            this.numberDataGridViewTextBoxColumn,
            this.horizontalDataGridViewTextBoxColumn,
            this.verticalDataGridViewTextBoxColumn});
            this.dgLensWarpCorrectionValues.DataSource = this.lensWarpCorrectionItemsBindingSource;
            this.dgLensWarpCorrectionValues.Location = new System.Drawing.Point(6, 6);
            this.dgLensWarpCorrectionValues.Name = "dgLensWarpCorrectionValues";
            this.dgLensWarpCorrectionValues.RowHeadersVisible = false;
            this.dgLensWarpCorrectionValues.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgLensWarpCorrectionValues.Size = new System.Drawing.Size(412, 303);
            this.dgLensWarpCorrectionValues.TabIndex = 0;
            this.dgLensWarpCorrectionValues.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgLensWarpCorrectionValues_CellValueChanged);
            this.dgLensWarpCorrectionValues.SelectionChanged += new System.EventHandler(this.dgLensWarpCorrectionValues_SelectionChanged);
            this.dgLensWarpCorrectionValues.KeyUp += new System.Windows.Forms.KeyEventHandler(this.dgLensWarpCorrectionValues_KeyUp);
            // 
            // Number
            // 
            this.Number.DataPropertyName = "Number";
            this.Number.HeaderText = "Number";
            this.Number.Name = "Number";
            this.Number.ReadOnly = true;
            // 
            // Horizontal
            // 
            this.Horizontal.DataPropertyName = "Horizontal";
            this.Horizontal.HeaderText = "Horizontal";
            this.Horizontal.Name = "Horizontal";
            // 
            // Vertical
            // 
            this.Vertical.DataPropertyName = "Vertical";
            this.Vertical.HeaderText = "Vertical";
            this.Vertical.Name = "Vertical";
            // 
            // numberDataGridViewTextBoxColumn
            // 
            this.numberDataGridViewTextBoxColumn.DataPropertyName = "Number";
            this.numberDataGridViewTextBoxColumn.HeaderText = "Number";
            this.numberDataGridViewTextBoxColumn.Name = "numberDataGridViewTextBoxColumn";
            // 
            // horizontalDataGridViewTextBoxColumn
            // 
            this.horizontalDataGridViewTextBoxColumn.DataPropertyName = "Horizontal";
            this.horizontalDataGridViewTextBoxColumn.HeaderText = "Horizontal";
            this.horizontalDataGridViewTextBoxColumn.Name = "horizontalDataGridViewTextBoxColumn";
            // 
            // verticalDataGridViewTextBoxColumn
            // 
            this.verticalDataGridViewTextBoxColumn.DataPropertyName = "Vertical";
            this.verticalDataGridViewTextBoxColumn.HeaderText = "Vertical";
            this.verticalDataGridViewTextBoxColumn.Name = "verticalDataGridViewTextBoxColumn";
            // 
            // lensWarpCorrectionItemsBindingSource
            // 
            this.lensWarpCorrectionItemsBindingSource.DataSource = typeof(Atum.Studio.Core.ModelCorrections.LensWarpCorrection.LensWarpCorrectionItems);
            // 
            // tbGeneratePrintjob
            // 
            this.tbGeneratePrintjob.Controls.Add(this.panel1);
            this.tbGeneratePrintjob.Location = new System.Drawing.Point(4, 22);
            this.tbGeneratePrintjob.Name = "tbGeneratePrintjob";
            this.tbGeneratePrintjob.Padding = new System.Windows.Forms.Padding(3);
            this.tbGeneratePrintjob.Size = new System.Drawing.Size(424, 334);
            this.tbGeneratePrintjob.TabIndex = 1;
            this.tbGeneratePrintjob.Text = "Job";
            this.tbGeneratePrintjob.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.tableLayoutPanel2);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(418, 328);
            this.panel1.TabIndex = 1;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 102F));
            this.tableLayoutPanel2.Controls.Add(this.label6, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.lblManufacturer, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.cbAdvancedCalibrationMaterialProduct, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.cbAdvancedCalibrationMaterialSupplier, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.panel3, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.btnSaveAdvancedCalibration, 1, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 7;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(418, 328);
            this.tableLayoutPanel2.TabIndex = 33;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label6, 3);
            this.label6.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(3, 144);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(412, 30);
            this.label6.TabIndex = 41;
            this.label6.Text = "Save job and print:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label4, 3);
            this.label4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(3, 84);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(412, 30);
            this.label4.TabIndex = 39;
            this.label4.Text = "Please insert a FAT32 formatted USB flashdrive and select the drive:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.tableLayoutPanel2.SetColumnSpan(this.label5, 3);
            this.label5.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Underline, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(3, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(412, 30);
            this.label5.TabIndex = 34;
            this.label5.Text = "Select material:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblManufacturer
            // 
            this.lblManufacturer.AutoSize = true;
            this.lblManufacturer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblManufacturer.Location = new System.Drawing.Point(3, 30);
            this.lblManufacturer.Name = "lblManufacturer";
            this.lblManufacturer.Size = new System.Drawing.Size(94, 27);
            this.lblManufacturer.TabIndex = 36;
            this.lblManufacturer.Text = "Manufacturer:";
            this.lblManufacturer.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAdvancedCalibrationMaterialProduct
            // 
            this.cbAdvancedCalibrationMaterialProduct.DisplayMember = "DisplayName";
            this.cbAdvancedCalibrationMaterialProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdvancedCalibrationMaterialProduct.FormattingEnabled = true;
            this.cbAdvancedCalibrationMaterialProduct.Location = new System.Drawing.Point(103, 60);
            this.cbAdvancedCalibrationMaterialProduct.Name = "cbAdvancedCalibrationMaterialProduct";
            this.cbAdvancedCalibrationMaterialProduct.Size = new System.Drawing.Size(206, 21);
            this.cbAdvancedCalibrationMaterialProduct.TabIndex = 38;
            this.cbAdvancedCalibrationMaterialProduct.SelectedIndexChanged += new System.EventHandler(this.cbMaterialProduct_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.label3.Location = new System.Drawing.Point(3, 57);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 27);
            this.label3.TabIndex = 37;
            this.label3.Text = "Product:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cbAdvancedCalibrationMaterialSupplier
            // 
            this.cbAdvancedCalibrationMaterialSupplier.DisplayMember = "supplier";
            this.cbAdvancedCalibrationMaterialSupplier.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdvancedCalibrationMaterialSupplier.FormattingEnabled = true;
            this.cbAdvancedCalibrationMaterialSupplier.Location = new System.Drawing.Point(103, 33);
            this.cbAdvancedCalibrationMaterialSupplier.Name = "cbAdvancedCalibrationMaterialSupplier";
            this.cbAdvancedCalibrationMaterialSupplier.Size = new System.Drawing.Size(206, 21);
            this.cbAdvancedCalibrationMaterialSupplier.TabIndex = 35;
            this.cbAdvancedCalibrationMaterialSupplier.SelectedIndexChanged += new System.EventHandler(this.cbMaterialManufacturer_SelectedIndexChanged);
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.plAdvancedCalibrationDriveLetters);
            this.panel3.Controls.Add(this.btnRefreshUSBDisks);
            this.panel3.Controls.Add(this.cbUSBDriveLetters);
            this.panel3.Controls.Add(this.button1);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(103, 117);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(210, 24);
            this.panel3.TabIndex = 39;
            // 
            // plAdvancedCalibrationDriveLetters
            // 
            this.plAdvancedCalibrationDriveLetters.Controls.Add(this.btnAdvancedCalibrationRefreshDrives);
            this.plAdvancedCalibrationDriveLetters.Controls.Add(this.cbAdvancedCalibrationDriveletters);
            this.plAdvancedCalibrationDriveLetters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plAdvancedCalibrationDriveLetters.Location = new System.Drawing.Point(0, 0);
            this.plAdvancedCalibrationDriveLetters.Name = "plAdvancedCalibrationDriveLetters";
            this.plAdvancedCalibrationDriveLetters.Size = new System.Drawing.Size(210, 24);
            this.plAdvancedCalibrationDriveLetters.TabIndex = 42;
            // 
            // btnAdvancedCalibrationRefreshDrives
            // 
            this.btnAdvancedCalibrationRefreshDrives.Image = global::Atum.Studio.Properties.Resources.Refresh16;
            this.btnAdvancedCalibrationRefreshDrives.Location = new System.Drawing.Point(183, 0);
            this.btnAdvancedCalibrationRefreshDrives.Margin = new System.Windows.Forms.Padding(0);
            this.btnAdvancedCalibrationRefreshDrives.Name = "btnAdvancedCalibrationRefreshDrives";
            this.btnAdvancedCalibrationRefreshDrives.Size = new System.Drawing.Size(23, 23);
            this.btnAdvancedCalibrationRefreshDrives.TabIndex = 41;
            this.btnAdvancedCalibrationRefreshDrives.UseVisualStyleBackColor = true;
            this.btnAdvancedCalibrationRefreshDrives.Click += new System.EventHandler(this.btnRefreshUSBDisks_Click);
            // 
            // cbAdvancedCalibrationDriveletters
            // 
            this.cbAdvancedCalibrationDriveletters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbAdvancedCalibrationDriveletters.FormattingEnabled = true;
            this.cbAdvancedCalibrationDriveletters.Location = new System.Drawing.Point(0, 0);
            this.cbAdvancedCalibrationDriveletters.Margin = new System.Windows.Forms.Padding(2);
            this.cbAdvancedCalibrationDriveletters.Name = "cbAdvancedCalibrationDriveletters";
            this.cbAdvancedCalibrationDriveletters.Size = new System.Drawing.Size(181, 21);
            this.cbAdvancedCalibrationDriveletters.TabIndex = 40;
            // 
            // btnRefreshUSBDisks
            // 
            this.btnRefreshUSBDisks.Image = global::Atum.Studio.Properties.Resources.Refresh16;
            this.btnRefreshUSBDisks.Location = new System.Drawing.Point(183, 0);
            this.btnRefreshUSBDisks.Margin = new System.Windows.Forms.Padding(0);
            this.btnRefreshUSBDisks.Name = "btnRefreshUSBDisks";
            this.btnRefreshUSBDisks.Size = new System.Drawing.Size(23, 23);
            this.btnRefreshUSBDisks.TabIndex = 41;
            this.btnRefreshUSBDisks.UseVisualStyleBackColor = true;
            // 
            // cbUSBDriveLetters
            // 
            this.cbUSBDriveLetters.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbUSBDriveLetters.FormattingEnabled = true;
            this.cbUSBDriveLetters.Location = new System.Drawing.Point(-20, 0);
            this.cbUSBDriveLetters.Margin = new System.Windows.Forms.Padding(2);
            this.cbUSBDriveLetters.Name = "cbUSBDriveLetters";
            this.cbUSBDriveLetters.Size = new System.Drawing.Size(201, 21);
            this.cbUSBDriveLetters.TabIndex = 40;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(131, 26);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 32;
            this.button1.Text = "Save";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnSaveAdvancedCalibration
            // 
            this.btnSaveAdvancedCalibration.Location = new System.Drawing.Point(103, 177);
            this.btnSaveAdvancedCalibration.Name = "btnSaveAdvancedCalibration";
            this.btnSaveAdvancedCalibration.Size = new System.Drawing.Size(75, 23);
            this.btnSaveAdvancedCalibration.TabIndex = 32;
            this.btnSaveAdvancedCalibration.Text = "Save";
            this.btnSaveAdvancedCalibration.UseVisualStyleBackColor = true;
            this.btnSaveAdvancedCalibration.Click += new System.EventHandler(this.btnSaveAdvancedCalibration_Click);
            // 
            // AtumV20PrinterProperties
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.lblHeader);
            this.Controls.Add(this.tbPrinterSettings);
            this.Margin = new System.Windows.Forms.Padding(8, 3, 3, 3);
            this.Name = "AtumV20PrinterProperties";
            this.Padding = new System.Windows.Forms.Padding(3, 3, 0, 3);
            this.Size = new System.Drawing.Size(449, 443);
            this.lblHeader.ResumeLayout(false);
            this.lblHeader.PerformLayout();
            this.tbPrinterSettings.ResumeLayout(false);
            this.Properties.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.Calibration.ResumeLayout(false);
            this.Calibration.PerformLayout();
            this.AdvancedCalibration.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tbValues.ResumeLayout(false);
            this.tbValues.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgLensWarpCorrectionValues)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.lensWarpCorrectionItemsBindingSource)).EndInit();
            this.tbGeneratePrintjob.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.plAdvancedCalibrationDriveLetters.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel lblHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabControl tbPrinterSettings;
        private System.Windows.Forms.TabPage Properties;
        private System.Windows.Forms.Label lblDisplayName;
        private System.Windows.Forms.TextBox txtDisplayName;
        private System.Windows.Forms.Label lblDescription;
        private System.Windows.Forms.TextBox txtDescription;
        private System.Windows.Forms.TabPage Calibration;
        private AtumPrinterCalibration atumPrinterCalibration1;
        private System.Windows.Forms.RadioButton rbtn100Micron;
        private System.Windows.Forms.RadioButton rbtn75Micron;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.RadioButton rbtn50Micron;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.LinkLabel lblSwitchMode;
        private System.Windows.Forms.TabPage AdvancedCalibration;
        private System.Windows.Forms.DataGridView dgLensWarpCorrectionValues;
        private System.Windows.Forms.BindingSource lensWarpCorrectionItemsBindingSource;
        private System.Windows.Forms.DataGridViewTextBoxColumn Number;
        private DataGridView.DataGridViewNumericUpDownCell.NumericUpDownColumn Horizontal;
        private DataGridView.DataGridViewNumericUpDownCell.NumericUpDownColumn Vertical;
        private System.Windows.Forms.DataGridViewTextBoxColumn numberDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn horizontalDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn verticalDataGridViewTextBoxColumn;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tbValues;
        private System.Windows.Forms.TabPage tbGeneratePrintjob;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnSaveAdvancedCalibration;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblManufacturer;
        private System.Windows.Forms.ComboBox cbAdvancedCalibrationMaterialProduct;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cbAdvancedCalibrationMaterialSupplier;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Panel plAdvancedCalibrationDriveLetters;
        private System.Windows.Forms.Button btnAdvancedCalibrationRefreshDrives;
        private System.Windows.Forms.ComboBox cbAdvancedCalibrationDriveletters;
        private System.Windows.Forms.Button btnRefreshUSBDisks;
        private System.Windows.Forms.ComboBox cbUSBDriveLetters;
        private System.Windows.Forms.LinkLabel linkLabel1;
    }
}
