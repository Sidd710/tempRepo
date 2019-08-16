namespace Atum.Studio.Controls.Correction
{
    partial class Trapezoid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Trapezoid));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.txtLower = new System.Windows.Forms.NumericUpDown();
            this.txtRight = new System.Windows.Forms.NumericUpDown();
            this.txtUpper = new System.Windows.Forms.NumericUpDown();
            this.txtLeft = new System.Windows.Forms.NumericUpDown();
            this.txtUpperLeftLowerRight = new System.Windows.Forms.NumericUpDown();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.txtLowerLeftUpperRight = new System.Windows.Forms.NumericUpDown();
            this.tableLayoutPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.txtLower)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpper)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeft)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperLeftLowerRight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowerLeftUpperRight)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 74F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 79F));
            this.tableLayoutPanel1.Controls.Add(this.txtLower, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.txtRight, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUpper, 1, 0);
            this.tableLayoutPanel1.Controls.Add(this.txtLeft, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtUpperLeftLowerRight, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.pictureBox1, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.txtLowerLeftUpperRight, 2, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 27F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 25F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(363, 214);
            this.tableLayoutPanel1.TabIndex = 3;
            // 
            // txtLower
            // 
            this.txtLower.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.txtLower.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLower.DecimalPlaces = 3;
            this.txtLower.Location = new System.Drawing.Point(142, 192);
            this.txtLower.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtLower.Name = "txtLower";
            this.txtLower.Size = new System.Drawing.Size(73, 22);
            this.txtLower.TabIndex = 3;
            this.txtLower.ValueChanged += new System.EventHandler(this.txtLower_ValueChanged);
            // 
            // txtRight
            // 
            this.txtRight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtRight.DecimalPlaces = 3;
            this.txtRight.Enabled = false;
            this.txtRight.Location = new System.Drawing.Point(287, 97);
            this.txtRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtRight.Name = "txtRight";
            this.txtRight.Size = new System.Drawing.Size(71, 22);
            this.txtRight.TabIndex = 0;
            // 
            // txtUpper
            // 
            this.txtUpper.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.txtUpper.DecimalPlaces = 3;
            this.txtUpper.Location = new System.Drawing.Point(142, 3);
            this.txtUpper.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtUpper.Name = "txtUpper";
            this.txtUpper.Size = new System.Drawing.Size(73, 22);
            this.txtUpper.TabIndex = 1;
            this.txtUpper.ValueChanged += new System.EventHandler(this.txtUpper_ValueChanged);
            // 
            // txtLeft
            // 
            this.txtLeft.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.txtLeft.Cursor = System.Windows.Forms.Cursors.Default;
            this.txtLeft.DecimalPlaces = 3;
            this.txtLeft.Location = new System.Drawing.Point(3, 97);
            this.txtLeft.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtLeft.Name = "txtLeft";
            this.txtLeft.Size = new System.Drawing.Size(68, 22);
            this.txtLeft.TabIndex = 2;
            this.txtLeft.ValueChanged += new System.EventHandler(this.txtLeft_ValueChanged);
            // 
            // txtUpperLeftLowerRight
            // 
            this.txtUpperLeftLowerRight.DecimalPlaces = 3;
            this.txtUpperLeftLowerRight.Location = new System.Drawing.Point(287, 192);
            this.txtUpperLeftLowerRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtUpperLeftLowerRight.Name = "txtUpperLeftLowerRight";
            this.txtUpperLeftLowerRight.Size = new System.Drawing.Size(71, 22);
            this.txtUpperLeftLowerRight.TabIndex = 4;
            this.txtUpperLeftLowerRight.ValueChanged += new System.EventHandler(this.txtUpperLeftLowerRight_ValueChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(77, 30);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(204, 156);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 5;
            this.pictureBox1.TabStop = false;
            // 
            // txtLowerLeftUpperRight
            // 
            this.txtLowerLeftUpperRight.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.txtLowerLeftUpperRight.DecimalPlaces = 3;
            this.txtLowerLeftUpperRight.Location = new System.Drawing.Point(287, 3);
            this.txtLowerLeftUpperRight.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.txtLowerLeftUpperRight.Name = "txtLowerLeftUpperRight";
            this.txtLowerLeftUpperRight.Size = new System.Drawing.Size(71, 22);
            this.txtLowerLeftUpperRight.TabIndex = 6;
            this.txtLowerLeftUpperRight.ValueChanged += new System.EventHandler(this.txtLowerLeftUpperRight_ValueChanged);
            // 
            // Trapezoid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "Trapezoid";
            this.Size = new System.Drawing.Size(363, 214);
            this.tableLayoutPanel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.txtLower)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpper)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLeft)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtUpperLeftLowerRight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txtLowerLeftUpperRight)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.NumericUpDown txtLower;
        private System.Windows.Forms.NumericUpDown txtRight;
        private System.Windows.Forms.NumericUpDown txtUpper;
        private System.Windows.Forms.NumericUpDown txtLeft;
        private System.Windows.Forms.NumericUpDown txtUpperLeftLowerRight;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.NumericUpDown txtLowerLeftUpperRight;

    }
}
