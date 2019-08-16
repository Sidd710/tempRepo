using Atum.Studio.Controls.NewGui;
using Atum.Studio.Controls.NewGui.SliderControl.SliderControlTracker;

namespace Atum.Studio.Controls.NewGui.ExportControl
{
    partial class ExportUserControl
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
            this.plExportControl = new System.Windows.Forms.Panel();
            this.plFooter = new Atum.Studio.Controls.OpenGL.SceneControlPrintPreviewPropertiesToolbar();
            this.plExportClick = new Atum.Studio.Controls.NewGui.RoundedButton();
            this.pbExport = new System.Windows.Forms.PictureBox();
            this.lblExportClick = new System.Windows.Forms.Label();
            this.plSlider = new System.Windows.Forms.Panel();
            this.plSliderWithIndexes = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.mTracker = new Atum.Studio.Controls.NewGui.SliderControl.SliderControlTracker.SliderControlTracker();
            this.lblSliceTotalCount = new System.Windows.Forms.Label();
            this.txtSlicerStartIndex = new System.Windows.Forms.RichTextBox();
            this.plDown = new System.Windows.Forms.Panel();
            this.plUp = new System.Windows.Forms.Panel();
            this.plExportControl.SuspendLayout();
            this.plExportClick.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExport)).BeginInit();
            this.plSlider.SuspendLayout();
            this.plSliderWithIndexes.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // plExportControl
            // 
            this.plExportControl.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.plExportControl.Controls.Add(this.plFooter);
            this.plExportControl.Controls.Add(this.plExportClick);
            this.plExportControl.Controls.Add(this.plSlider);
            this.plExportControl.Controls.Add(this.plDown);
            this.plExportControl.Controls.Add(this.plUp);
            this.plExportControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plExportControl.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.plExportControl.Location = new System.Drawing.Point(12, 12);
            this.plExportControl.Name = "plExportControl";
            this.plExportControl.Size = new System.Drawing.Size(1884, 928);
            this.plExportControl.TabIndex = 0;
            this.plExportControl.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plExportControl_MouseMove);
            this.plExportControl.Resize += new System.EventHandler(this.plExportControl_Resize);
            // 
            // plFooter
            // 
            this.plFooter.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(230)))), ((int)(((byte)(230)))), ((int)(((byte)(230)))));
            this.plFooter.ForeColor = System.Drawing.Color.Black;
            this.plFooter.Location = new System.Drawing.Point(54, 848);
            this.plFooter.Name = "plFooter";
            this.plFooter.Size = new System.Drawing.Size(1230, 64);
            this.plFooter.TabIndex = 9;
            this.plFooter.Visible = false;
            // 
            // plExportClick
            // 
            this.plExportClick.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.plExportClick.BackColor = System.Drawing.Color.White;
            this.plExportClick.Controls.Add(this.pbExport);
            this.plExportClick.Controls.Add(this.lblExportClick);
            this.plExportClick.Cursor = System.Windows.Forms.Cursors.Hand;
            this.plExportClick.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.plExportClick.Location = new System.Drawing.Point(1303, 848);
            this.plExportClick.Margin = new System.Windows.Forms.Padding(16, 3, 3, 3);
            this.plExportClick.Name = "plExportClick";
            this.plExportClick.Radius = 31;
            this.plExportClick.SingleBorder = false;
            this.plExportClick.Size = new System.Drawing.Size(160, 64);
            this.plExportClick.TabIndex = 8;
            this.plExportClick.UseVisualStyleBackColor = false;
            this.plExportClick.Visible = false;
            this.plExportClick.Click += new System.EventHandler(this.plExportClick_Click);
            // 
            // pbExport
            // 
            this.pbExport.BackColor = System.Drawing.Color.Transparent;
            this.pbExport.Location = new System.Drawing.Point(33, 15);
            this.pbExport.Margin = new System.Windows.Forms.Padding(0);
            this.pbExport.Name = "pbExport";
            this.pbExport.Size = new System.Drawing.Size(26, 34);
            this.pbExport.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pbExport.TabIndex = 21;
            this.pbExport.TabStop = false;
            this.pbExport.Click += new System.EventHandler(this.pbExport_Click);
            // 
            // lblExportClick
            // 
            this.lblExportClick.AutoSize = true;
            this.lblExportClick.BackColor = System.Drawing.Color.Transparent;
            this.lblExportClick.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel, ((byte)(0)));
            this.lblExportClick.ForeColor = System.Drawing.Color.Black;
            this.lblExportClick.Location = new System.Drawing.Point(66, 21);
            this.lblExportClick.Name = "lblExportClick";
            this.lblExportClick.Size = new System.Drawing.Size(62, 22);
            this.lblExportClick.TabIndex = 20;
            this.lblExportClick.Text = "Export";
            this.lblExportClick.Click += new System.EventHandler(this.lblExportClick_Click);
            // 
            // plSlider
            // 
            this.plSlider.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plSlider.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.plSlider.Controls.Add(this.plSliderWithIndexes);
            this.plSlider.Location = new System.Drawing.Point(1808, 84);
            this.plSlider.Margin = new System.Windows.Forms.Padding(0);
            this.plSlider.Name = "plSlider";
            this.plSlider.Size = new System.Drawing.Size(60, 664);
            this.plSlider.TabIndex = 2;
            this.plSlider.Visible = false;
            this.plSlider.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plSlider_MouseMove);
            // 
            // plSliderWithIndexes
            // 
            this.plSliderWithIndexes.Controls.Add(this.tableLayoutPanel1);
            this.plSliderWithIndexes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.plSliderWithIndexes.Location = new System.Drawing.Point(0, 0);
            this.plSliderWithIndexes.Margin = new System.Windows.Forms.Padding(0);
            this.plSliderWithIndexes.Name = "plSliderWithIndexes";
            this.plSliderWithIndexes.Size = new System.Drawing.Size(60, 664);
            this.plSliderWithIndexes.TabIndex = 10;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.mTracker, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.lblSliceTotalCount, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtSlicerStartIndex, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 38F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(60, 664);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // mTracker
            // 
            this.mTracker.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.mTracker.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.mTracker.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mTracker.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mTracker.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(123)))), ((int)(((byte)(125)))), ((int)(((byte)(123)))));
            this.mTracker.IndentHeight = 6;
            this.mTracker.IndentWidth = 4;
            this.mTracker.Location = new System.Drawing.Point(0, 41);
            this.mTracker.Margin = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.mTracker.Maximum = 100;
            this.mTracker.Minimum = 0;
            this.mTracker.Name = "mTracker";
            this.mTracker.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.mTracker.Size = new System.Drawing.Size(60, 582);
            this.mTracker.TabIndex = 9;
            this.mTracker.TickColor = System.Drawing.Color.Transparent;
            this.mTracker.TickHeight = 1;
            this.mTracker.TrackerColor = System.Drawing.Color.FromArgb(((int)(((byte)(24)))), ((int)(((byte)(130)))), ((int)(((byte)(198)))));
            this.mTracker.TrackerSize = new System.Drawing.Size(52, 52);
            this.mTracker.TrackLineColor = System.Drawing.Color.Black;
            this.mTracker.TrackLineHeight = 12;
            this.mTracker.Value = 10;
            this.mTracker.MouseMove += new System.Windows.Forms.MouseEventHandler(this.mTracker_MouseMove);
            // 
            // lblSliceTotalCount
            // 
            this.lblSliceTotalCount.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblSliceTotalCount.Location = new System.Drawing.Point(3, 0);
            this.lblSliceTotalCount.Name = "lblSliceTotalCount";
            this.lblSliceTotalCount.Padding = new System.Windows.Forms.Padding(0, 2, 0, 0);
            this.lblSliceTotalCount.Size = new System.Drawing.Size(54, 38);
            this.lblSliceTotalCount.TabIndex = 10;
            this.lblSliceTotalCount.Text = "99999";
            this.lblSliceTotalCount.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.lblSliceTotalCount.MouseMove += new System.Windows.Forms.MouseEventHandler(this.lblSliceTotalCount_MouseMove);
            // 
            // txtSlicerStartIndex
            // 
            this.txtSlicerStartIndex.BackColor = System.Drawing.Color.Black;
            this.txtSlicerStartIndex.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtSlicerStartIndex.DetectUrls = false;
            this.txtSlicerStartIndex.ForeColor = System.Drawing.Color.White;
            this.txtSlicerStartIndex.Location = new System.Drawing.Point(6, 636);
            this.txtSlicerStartIndex.Margin = new System.Windows.Forms.Padding(6, 10, 6, 6);
            this.txtSlicerStartIndex.Multiline = false;
            this.txtSlicerStartIndex.Name = "txtSlicerStartIndex";
            this.txtSlicerStartIndex.Size = new System.Drawing.Size(48, 22);
            this.txtSlicerStartIndex.TabIndex = 11;
            this.txtSlicerStartIndex.Tag = "";
            this.txtSlicerStartIndex.Text = "11111";
            this.txtSlicerStartIndex.MouseClick += new System.Windows.Forms.MouseEventHandler(this.txtSlicerStartIndex_MouseClick);
            this.txtSlicerStartIndex.TextChanged += new System.EventHandler(this.txtSlicerStartIndex_TextChanged);
            this.txtSlicerStartIndex.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSlicerStartIndex_KeyDown);
            this.txtSlicerStartIndex.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSlicerStartIndex_KeyPress);
            this.txtSlicerStartIndex.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSlicerStartIndex_KeyUp);
            this.txtSlicerStartIndex.MouseDown += new System.Windows.Forms.MouseEventHandler(this.txtSlicerStartIndex_MouseDown);
            this.txtSlicerStartIndex.MouseMove += new System.Windows.Forms.MouseEventHandler(this.txtSlicerStartIndex_MouseMove);
            // 
            // plDown
            // 
            this.plDown.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.plDown.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.plDown.BackgroundImage = global::Atum.Studio.Properties.Resources.slider_arrow_down;
            this.plDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plDown.Location = new System.Drawing.Point(1808, 756);
            this.plDown.Name = "plDown";
            this.plDown.Size = new System.Drawing.Size(60, 60);
            this.plDown.TabIndex = 1;
            this.plDown.Visible = false;
            this.plDown.Click += new System.EventHandler(this.plDown_Click);
            this.plDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plDown_MouseDown);
            this.plDown.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plDown_MouseMove);
            this.plDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plDown_MouseUp);
            // 
            // plUp
            // 
            this.plUp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.plUp.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(40)))), ((int)(((byte)(46)))), ((int)(((byte)(51)))));
            this.plUp.BackgroundImage = global::Atum.Studio.Properties.Resources.slider_arrow_up;
            this.plUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.plUp.Location = new System.Drawing.Point(1808, 16);
            this.plUp.Name = "plUp";
            this.plUp.Size = new System.Drawing.Size(60, 60);
            this.plUp.TabIndex = 0;
            this.plUp.Visible = false;
            this.plUp.Click += new System.EventHandler(this.plUp_Click);
            this.plUp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.plUp_MouseClick);
            this.plUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.plUp_MouseDown);
            this.plUp.MouseMove += new System.Windows.Forms.MouseEventHandler(this.plUp_MouseMove);
            this.plUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.plUp_MouseUp);
            // 
            // ExportUserControl
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(57)))), ((int)(((byte)(64)))), ((int)(((byte)(69)))));
            this.Controls.Add(this.plExportControl);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.Color.White;
            this.Name = "ExportUserControl";
            this.Padding = new System.Windows.Forms.Padding(12, 12, 24, 0);
            this.Size = new System.Drawing.Size(1920, 940);
            this.Load += new System.EventHandler(this.ExportUserControl_Load);
            this.Resize += new System.EventHandler(this.ExportUserControl_Resize);
            this.plExportControl.ResumeLayout(false);
            this.plExportClick.ResumeLayout(false);
            this.plExportClick.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbExport)).EndInit();
            this.plSlider.ResumeLayout(false);
            this.plSliderWithIndexes.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel plExportControl;
        private System.Windows.Forms.Panel plUp;
        private System.Windows.Forms.Panel plDown;
        private System.Windows.Forms.Panel plSlider;
        private RoundedButton plExportClick;
        private System.Windows.Forms.Label lblExportClick;
        private System.Windows.Forms.PictureBox pbExport;
        private SliderControlTracker mTracker;
        private OpenGL.SceneControlPrintPreviewPropertiesToolbar plFooter;
        private System.Windows.Forms.Panel plSliderWithIndexes;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Label lblSliceTotalCount;
        private System.Windows.Forms.RichTextBox txtSlicerStartIndex;
    }
}
