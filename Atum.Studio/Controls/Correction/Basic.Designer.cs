namespace Atum.Studio.Controls.Correction
{
    partial class Basic
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
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLower = new System.Windows.Forms.NumericUpDown();
            this.txtRight = new System.Windows.Forms.NumericUpDown();
            this.txtUpper = new System.Windows.Forms.NumericUpDown();
            this.txtLeft = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtUpperLeftLowerRight = new System.Windows.Forms.NumericUpDown();
            this.txtLowerLeftUpperRight = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperLeftLowerRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowerLeftUpperRight)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 100F));
            this.tableLayoutPanel1.Controls.Add(this.txtLower, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtRight, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUpper, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLeft, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUpperLeftLowerRight, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtLowerLeftUpperRight, 2, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(2);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(333, 246);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // txtLower
            // 
            this.txtLower.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtLower.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLower.DecimalPlaces = 3;
            this.txtLower.Location = new System.Drawing.Point(136, 224);
            this.txtLower.Margin = new System.Windows.Forms.Padding(2);
            this.txtLower.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtLower.Name = "txtLower";
            this.txtLower.Size = new System.Drawing.Size(60, 20);
            this.txtLower.TabIndex = 3;
            this.txtLower.ValueChanged += new System.EventHandler(this.txtLower_ValueChanged);
            // 
            // txtRight
            // 
            this.txtRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtRight.DecimalPlaces = 3;
            this.txtRight.Location = new System.Drawing.Point(271, 113);
            this.txtRight.Margin = new System.Windows.Forms.Padding(2);
            this.txtRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtRight.Name = "txtRight";
            this.txtRight.Size = new System.Drawing.Size(60, 20);
            this.txtRight.TabIndex = 0;
            this.txtRight.ValueChanged += new System.EventHandler(this.txtRight_ValueChanged);
            // 
            // txtUpper
            // 
            this.txtUpper.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtUpper.DecimalPlaces = 3;
            this.txtUpper.Location = new System.Drawing.Point(136, 2);
            this.txtUpper.Margin = new System.Windows.Forms.Padding(2);
            this.txtUpper.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtUpper.Name = "txtUpper";
            this.txtUpper.Size = new System.Drawing.Size(60, 20);
            this.txtUpper.TabIndex = 1;
            this.txtUpper.ValueChanged += new System.EventHandler(this.txtUpper_ValueChanged);
            // 
            // txtLeft
            // 
            this.txtLeft.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLeft.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLeft.DecimalPlaces = 3;
            this.txtLeft.Location = new System.Drawing.Point(2, 113);
            this.txtLeft.Margin = new System.Windows.Forms.Padding(2);
            this.txtLeft.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(60, 20);
            this.txtLeft.TabIndex = 2;
            this.txtLeft.ValueChanged += new System.EventHandler(this.txtLeft_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::Atum.Studio.Properties.Resources.Controls_BasicCorrection_Preview;
            this.pictureBox1.Location = new System.Drawing.Point(102, 29);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(129, 188);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // txtUpperLeftLowerRight
            // 
            this.txtUpperLeftLowerRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtUpperLeftLowerRight.DecimalPlaces = 3;
            this.txtUpperLeftLowerRight.Location = new System.Drawing.Point(271, 222);
            this.txtUpperLeftLowerRight.Margin = new System.Windows.Forms.Padding(2);
            this.txtUpperLeftLowerRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtUpperLeftLowerRight.Name = "txtUpperLeftLowerRight";
            this.txtUpperLeftLowerRight.Size = new System.Drawing.Size(60, 20);
            this.txtUpperLeftLowerRight.TabIndex = 7;
            this.txtUpperLeftLowerRight.ValueChanged += new System.EventHandler(this.txtUpperLeftLowerRight_ValueChanged);
            // 
            // txtLowerLeftUpperRight
            // 
            this.txtLowerLeftUpperRight.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtLowerLeftUpperRight.DecimalPlaces = 3;
            this.txtLowerLeftUpperRight.Enabled = false;
            this.txtLowerLeftUpperRight.Location = new System.Drawing.Point(271, 3);
            this.txtLowerLeftUpperRight.Margin = new System.Windows.Forms.Padding(2);
            this.txtLowerLeftUpperRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtLowerLeftUpperRight.Name = "txtLowerLeftUpperRight";
            this.txtLowerLeftUpperRight.Size = new System.Drawing.Size(60, 20);
            this.txtLowerLeftUpperRight.TabIndex = 6;
            // 
            // Basic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Basic";
            this.Size = new System.Drawing.Size(335, 246);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperLeftLowerRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowerLeftUpperRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown txtLower;
        private System.Windows.Forms.NumericUpDown txtRight;
        private System.Windows.Forms.NumericUpDown txtUpper;
        private System.Windows.Forms.NumericUpDown txtLeft;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown txtLowerLeftUpperRight;
        private System.Windows.Forms.NumericUpDown txtUpperLeftLowerRight;
    }
}
