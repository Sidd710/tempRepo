using Atum.Studio.Controls.NewGui;

namespace Atum.Studio.Controls.NewGui
{
    partial class frmCalibratePrinter
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
            this.plContent = new System.Windows.Forms.Panel();
            this.spcContentsContainer = new System.Windows.Forms.SplitContainer();
            this.cbMaterialSelector = new Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter.MaterialSelector();
            this.lblOne = new System.Windows.Forms.Label();
            this.btnExport = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.lblOneText = new System.Windows.Forms.Label();
            this.plOne = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.lblTwo = new System.Windows.Forms.Label();
            this.plPrinterCalibrationOverview = new System.Windows.Forms.Panel();
            this.trapezoidSizeB = new Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowAbove();
            this.trapezoidSizeA = new Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowAbove();
            this.trapezoidSizeC = new Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowBelow();
            this.trapezoidSizeD = new Atum.Studio.Controls.NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowBelow();
            this.btnCheck = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblPreviousMeasurementsDate = new System.Windows.Forms.Label();
            this.lblTwoText = new System.Windows.Forms.Label();
            this.panel1 = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.plValidationMessage = new System.Windows.Forms.Panel();
            this.lblValidationMessage = new System.Windows.Forms.Label();
            this.plHeaderTitle = new System.Windows.Forms.Panel();
            this.pbHelp = new System.Windows.Forms.PictureBox();
            this.btnClose = new System.Windows.Forms.PictureBox();
            this.lblHeader = new System.Windows.Forms.Label();
            this.plContentSplitter = new System.Windows.Forms.SplitContainer();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcContentsContainer)).BeginInit();
            this.spcContentsContainer.Panel1.SuspendLayout();
            this.spcContentsContainer.Panel2.SuspendLayout();
            this.spcContentsContainer.SuspendLayout();
            this.plPrinterCalibrationOverview.SuspendLayout();
            this.plValidationMessage.SuspendLayout();
            this.plHeaderTitle.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plContentSplitter)).BeginInit();
            this.plContentSplitter.Panel1.SuspendLayout();
            this.plContentSplitter.Panel2.SuspendLayout();
            this.plContentSplitter.SuspendLayout();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.BackColor = System.Drawing.Color.WhiteSmoke;
            this.plContent.Controls.Add(this.spcContentsContainer);
            this.plContent.Controls.Add(this.plValidationMessage);
            this.plContent.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContent.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContent.Location = new System.Drawing.Point(0, 0);
            this.plContent.Margin = new System.Windows.Forms.Padding(0);
            this.plContent.Name = "plContent";
            this.plContent.Size = new System.Drawing.Size(800, 545);
            this.plContent.TabIndex = 1;
            // 
            // spcContentsContainer
            // 
            this.spcContentsContainer.Dock = System.Windows.Forms.DockStyle.Top;
            this.spcContentsContainer.IsSplitterFixed = true;
            this.spcContentsContainer.Location = new System.Drawing.Point(0, 50);
            this.spcContentsContainer.Name = "spcContentsContainer";
            // 
            // spcContentsContainer.Panel1
            // 
            this.spcContentsContainer.Panel1.Controls.Add(this.cbMaterialSelector);
            this.spcContentsContainer.Panel1.Controls.Add(this.lblOne);
            this.spcContentsContainer.Panel1.Controls.Add(this.btnExport);
            this.spcContentsContainer.Panel1.Controls.Add(this.lblOneText);
            this.spcContentsContainer.Panel1.Controls.Add(this.plOne);
            // 
            // spcContentsContainer.Panel2
            // 
            this.spcContentsContainer.Panel2.Controls.Add(this.lblTwo);
            this.spcContentsContainer.Panel2.Controls.Add(this.plPrinterCalibrationOverview);
            this.spcContentsContainer.Panel2.Controls.Add(this.btnCheck);
            this.spcContentsContainer.Panel2.Controls.Add(this.linkLabel1);
            this.spcContentsContainer.Panel2.Controls.Add(this.lblPreviousMeasurementsDate);
            this.spcContentsContainer.Panel2.Controls.Add(this.lblTwoText);
            this.spcContentsContainer.Panel2.Controls.Add(this.panel1);
            this.spcContentsContainer.Size = new System.Drawing.Size(800, 492);
            this.spcContentsContainer.SplitterDistance = 300;
            this.spcContentsContainer.SplitterWidth = 1;
            this.spcContentsContainer.TabIndex = 5;
            // 
            // cbMaterialSelector
            // 
            this.cbMaterialSelector.BackColor = System.Drawing.Color.White;
            this.cbMaterialSelector.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.cbMaterialSelector.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMaterialSelector.FormattingEnabled = true;
            this.cbMaterialSelector.IntegralHeight = false;
            this.cbMaterialSelector.ItemHeight = 30;
            this.cbMaterialSelector.Location = new System.Drawing.Point(80, 81);
            this.cbMaterialSelector.Margin = new System.Windows.Forms.Padding(3, 3, 16, 3);
            this.cbMaterialSelector.Name = "cbMaterialSelector";
            this.cbMaterialSelector.Size = new System.Drawing.Size(212, 36);
            this.cbMaterialSelector.TabIndex = 20;
            // 
            // lblOne
            // 
            this.lblOne.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblOne.Location = new System.Drawing.Point(36, 22);
            this.lblOne.Name = "lblOne";
            this.lblOne.Size = new System.Drawing.Size(20, 20);
            this.lblOne.TabIndex = 19;
            this.lblOne.Text = "1";
            this.lblOne.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExport.BackColor = System.Drawing.Color.WhiteSmoke;
            this.btnExport.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnExport.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnExport.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnExport.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnExport.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnExport.Location = new System.Drawing.Point(80, 144);
            this.btnExport.Margin = new System.Windows.Forms.Padding(0);
            this.btnExport.Name = "btnExport";
            this.btnExport.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnExport.Radius = 20;
            this.btnExport.SingleBorder = false;
            this.btnExport.Size = new System.Drawing.Size(116, 42);
            this.btnExport.TabIndex = 18;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = false;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // lblOneText
            // 
            this.lblOneText.Location = new System.Drawing.Point(80, 16);
            this.lblOneText.Name = "lblOneText";
            this.lblOneText.Size = new System.Drawing.Size(212, 43);
            this.lblOneText.TabIndex = 6;
            this.lblOneText.Text = "Select resin and export calibration model";
            // 
            // plOne
            // 
            this.plOne.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.plOne.BorderColor = System.Drawing.SystemColors.Control;
            this.plOne.BorderThickness = 0;
            this.plOne.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plOne.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plOne.Location = new System.Drawing.Point(24, 12);
            this.plOne.Margin = new System.Windows.Forms.Padding(0);
            this.plOne.Name = "plOne";
            this.plOne.Radius = 19;
            this.plOne.SingleBorder = true;
            this.plOne.Size = new System.Drawing.Size(42, 42);
            this.plOne.TabIndex = 5;
            this.plOne.UseVisualStyleBackColor = false;
            // 
            // lblTwo
            // 
            this.lblTwo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.lblTwo.Location = new System.Drawing.Point(49, 22);
            this.lblTwo.Name = "lblTwo";
            this.lblTwo.Size = new System.Drawing.Size(20, 20);
            this.lblTwo.TabIndex = 20;
            this.lblTwo.Text = "2";
            this.lblTwo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plPrinterCalibrationOverview
            // 
            this.plPrinterCalibrationOverview.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plPrinterCalibrationOverview.Controls.Add(this.trapezoidSizeB);
            this.plPrinterCalibrationOverview.Controls.Add(this.trapezoidSizeA);
            this.plPrinterCalibrationOverview.Controls.Add(this.trapezoidSizeC);
            this.plPrinterCalibrationOverview.Controls.Add(this.trapezoidSizeD);
            this.plPrinterCalibrationOverview.Location = new System.Drawing.Point(-10, 84);
            this.plPrinterCalibrationOverview.Name = "plPrinterCalibrationOverview";
            this.plPrinterCalibrationOverview.Size = new System.Drawing.Size(507, 241);
            this.plPrinterCalibrationOverview.TabIndex = 18;
            // 
            // trapezoidSizeB
            // 
            this.trapezoidSizeB.Location = new System.Drawing.Point(258, 174);
            this.trapezoidSizeB.Name = "trapezoidSizeB";
            this.trapezoidSizeB.Size = new System.Drawing.Size(104, 44);
            this.trapezoidSizeB.TabIndex = 23;
            this.trapezoidSizeB.TextValue = 0F;
            // 
            // trapezoidSizeA
            // 
            this.trapezoidSizeA.Location = new System.Drawing.Point(68, 190);
            this.trapezoidSizeA.Name = "trapezoidSizeA";
            this.trapezoidSizeA.Size = new System.Drawing.Size(104, 44);
            this.trapezoidSizeA.TabIndex = 22;
            this.trapezoidSizeA.TextValue = 0F;
            // 
            // trapezoidSizeC
            // 
            this.trapezoidSizeC.Location = new System.Drawing.Point(341, 4);
            this.trapezoidSizeC.Margin = new System.Windows.Forms.Padding(5);
            this.trapezoidSizeC.Name = "trapezoidSizeC";
            this.trapezoidSizeC.Size = new System.Drawing.Size(104, 44);
            this.trapezoidSizeC.TabIndex = 21;
            this.trapezoidSizeC.TextValue = 0F;
            // 
            // trapezoidSizeD
            // 
            this.trapezoidSizeD.Location = new System.Drawing.Point(159, 30);
            this.trapezoidSizeD.Margin = new System.Windows.Forms.Padding(5);
            this.trapezoidSizeD.Name = "trapezoidSizeD";
            this.trapezoidSizeD.Size = new System.Drawing.Size(104, 44);
            this.trapezoidSizeD.TabIndex = 20;
            this.trapezoidSizeD.TextValue = 0F;
            // 
            // btnCheck
            // 
            this.btnCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCheck.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.btnCheck.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCheck.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.btnCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCheck.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.btnCheck.ForeColor = System.Drawing.Color.White;
            this.btnCheck.Location = new System.Drawing.Point(167, 392);
            this.btnCheck.Margin = new System.Windows.Forms.Padding(0);
            this.btnCheck.Name = "btnCheck";
            this.btnCheck.Padding = new System.Windows.Forms.Padding(7, 3, 7, 3);
            this.btnCheck.Radius = 20;
            this.btnCheck.SingleBorder = false;
            this.btnCheck.Size = new System.Drawing.Size(126, 42);
            this.btnCheck.TabIndex = 17;
            this.btnCheck.Text = "Validate";
            this.btnCheck.UseVisualStyleBackColor = false;
            this.btnCheck.Click += new System.EventHandler(this.btnCheck_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(92, 335);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(216, 17);
            this.linkLabel1.TabIndex = 11;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "revert to previous measurements";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Click += new System.EventHandler(this.linkLabel1_Click);
            // 
            // lblPreviousMeasurementsDate
            // 
            this.lblPreviousMeasurementsDate.AutoSize = true;
            this.lblPreviousMeasurementsDate.Location = new System.Drawing.Point(92, 355);
            this.lblPreviousMeasurementsDate.Name = "lblPreviousMeasurementsDate";
            this.lblPreviousMeasurementsDate.Size = new System.Drawing.Size(104, 17);
            this.lblPreviousMeasurementsDate.TabIndex = 10;
            this.lblPreviousMeasurementsDate.Text = "26/02/18 21:54";
            // 
            // lblTwoText
            // 
            this.lblTwoText.Location = new System.Drawing.Point(92, 16);
            this.lblTwoText.Name = "lblTwoText";
            this.lblTwoText.Size = new System.Drawing.Size(314, 43);
            this.lblTwoText.TabIndex = 5;
            this.lblTwoText.Text = "Print the model and enter the \r\nmeasured outer dimensions below";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.panel1.BorderColor = System.Drawing.SystemColors.Control;
            this.panel1.BorderThickness = 0;
            this.panel1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.panel1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.panel1.Location = new System.Drawing.Point(36, 12);
            this.panel1.Margin = new System.Windows.Forms.Padding(0);
            this.panel1.Name = "panel1";
            this.panel1.Radius = 19;
            this.panel1.SingleBorder = true;
            this.panel1.Size = new System.Drawing.Size(42, 42);
            this.panel1.TabIndex = 4;
            this.panel1.UseVisualStyleBackColor = false;
            // 
            // plValidationMessage
            // 
            this.plValidationMessage.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(211)))), ((int)(((byte)(206)))));
            this.plValidationMessage.Controls.Add(this.lblValidationMessage);
            this.plValidationMessage.Dock = System.Windows.Forms.DockStyle.Top;
            this.plValidationMessage.Location = new System.Drawing.Point(0, 0);
            this.plValidationMessage.Name = "plValidationMessage";
            this.plValidationMessage.Size = new System.Drawing.Size(800, 50);
            this.plValidationMessage.TabIndex = 2;
            // 
            // lblValidationMessage
            // 
            this.lblValidationMessage.Location = new System.Drawing.Point(0, 15);
            this.lblValidationMessage.Name = "lblValidationMessage";
            this.lblValidationMessage.Size = new System.Drawing.Size(800, 17);
            this.lblValidationMessage.TabIndex = 0;
            this.lblValidationMessage.Text = "Further calibration required. Please repeat step 1 and step 2.";
            this.lblValidationMessage.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // plHeaderTitle
            // 
            this.plHeaderTitle.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.plHeaderTitle.Controls.Add(this.pbHelp);
            this.plHeaderTitle.Controls.Add(this.btnClose);
            this.plHeaderTitle.Controls.Add(this.lblHeader);
            this.plHeaderTitle.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plHeaderTitle.Location = new System.Drawing.Point(0, 0);
            this.plHeaderTitle.Margin = new System.Windows.Forms.Padding(0);
            this.plHeaderTitle.Name = "plHeaderTitle";
            this.plHeaderTitle.Size = new System.Drawing.Size(800, 56);
            this.plHeaderTitle.TabIndex = 0;
            // 
            // pbHelp
            // 
            this.pbHelp.Location = new System.Drawing.Point(720, 16);
            this.pbHelp.Margin = new System.Windows.Forms.Padding(4);
            this.pbHelp.Name = "pbHelp";
            this.pbHelp.Size = new System.Drawing.Size(24, 24);
            this.pbHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pbHelp.TabIndex = 2;
            this.pbHelp.TabStop = false;
            this.pbHelp.Visible = false;
            this.pbHelp.Click += new System.EventHandler(this.pbHelp_Click);
            // 
            // btnClose
            // 
            this.btnClose.Location = new System.Drawing.Point(760, 16);
            this.btnClose.Margin = new System.Windows.Forms.Padding(4);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(24, 24);
            this.btnClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.btnClose.TabIndex = 0;
            this.btnClose.TabStop = false;
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // lblHeader
            // 
            this.lblHeader.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblHeader.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.lblHeader.ForeColor = System.Drawing.Color.White;
            this.lblHeader.Location = new System.Drawing.Point(0, 0);
            this.lblHeader.Margin = new System.Windows.Forms.Padding(0);
            this.lblHeader.Name = "lblHeader";
            this.lblHeader.Size = new System.Drawing.Size(800, 56);
            this.lblHeader.TabIndex = 1;
            this.lblHeader.Text = "Dummy";
            this.lblHeader.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblHeader.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseMove);
            this.lblHeader.MouseUp += new System.Windows.Forms.MouseEventHandler(this.lblHeader_MouseUp);
            // 
            // plContentSplitter
            // 
            this.plContentSplitter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.plContentSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plContentSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.plContentSplitter.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContentSplitter.IsSplitterFixed = true;
            this.plContentSplitter.Location = new System.Drawing.Point(0, 0);
            this.plContentSplitter.Margin = new System.Windows.Forms.Padding(0);
            this.plContentSplitter.Name = "plContentSplitter";
            this.plContentSplitter.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // plContentSplitter.Panel1
            // 
            this.plContentSplitter.Panel1.Controls.Add(this.plHeaderTitle);
            this.plContentSplitter.Panel1MinSize = 56;
            // 
            // plContentSplitter.Panel2
            // 
            this.plContentSplitter.Panel2.Controls.Add(this.plContent);
            this.plContentSplitter.Panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.plContentSplitter.Size = new System.Drawing.Size(800, 602);
            this.plContentSplitter.SplitterDistance = 56;
            this.plContentSplitter.SplitterWidth = 1;
            this.plContentSplitter.TabIndex = 3;
            this.plContentSplitter.TabStop = false;
            // 
            // frmCalibratePrinter
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.WhiteSmoke;
            this.ClientSize = new System.Drawing.Size(800, 602);
            this.Controls.Add(this.plContentSplitter);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            this.Name = "frmCalibratePrinter";
            this.Text = "Calibrate {0}";
            this.Load += new System.EventHandler(this.frmCalibratePrinter_Load);
            this.plContent.ResumeLayout(false);
            this.spcContentsContainer.Panel1.ResumeLayout(false);
            this.spcContentsContainer.Panel2.ResumeLayout(false);
            this.spcContentsContainer.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.spcContentsContainer)).EndInit();
            this.spcContentsContainer.ResumeLayout(false);
            this.plPrinterCalibrationOverview.ResumeLayout(false);
            this.plValidationMessage.ResumeLayout(false);
            this.plHeaderTitle.ResumeLayout(false);
            this.plHeaderTitle.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbHelp)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.btnClose)).EndInit();
            this.plContentSplitter.Panel1.ResumeLayout(false);
            this.plContentSplitter.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.plContentSplitter)).EndInit();
            this.plContentSplitter.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        protected System.Windows.Forms.Panel plContent;
        private System.Windows.Forms.Panel plHeaderTitle;
        private System.Windows.Forms.PictureBox btnClose;
        private System.Windows.Forms.Label lblHeader;
        private System.Windows.Forms.SplitContainer plContentSplitter;
        private System.Windows.Forms.PictureBox pbHelp;
        private System.Windows.Forms.Panel plValidationMessage;
        private System.Windows.Forms.Label lblValidationMessage;
        private System.Windows.Forms.SplitContainer spcContentsContainer;
        private System.Windows.Forms.Label lblOneText;
        private RoundedButton plOne;
        private System.Windows.Forms.Label lblTwoText;
        private RoundedButton panel1;
        private System.Windows.Forms.Label lblPreviousMeasurementsDate;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private NewGui.RoundedButton btnExport;
        private NewGui.RoundedButton btnCheck;
        private System.Windows.Forms.Panel plPrinterCalibrationOverview;
        private NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowBelow trapezoidSizeD;
        private NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowBelow trapezoidSizeC;
        private NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowAbove trapezoidSizeB;
        private NewGui.PrinterEditorSettings.CalibratePrinter.TrapeziumInputArrowAbove trapezoidSizeA;
        private System.Windows.Forms.Label lblOne;
        private System.Windows.Forms.Label lblTwo;
        private NewGui.PrinterEditorSettings.CalibratePrinter.MaterialSelector cbMaterialSelector;
    }
}