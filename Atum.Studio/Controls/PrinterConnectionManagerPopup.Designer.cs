namespace Atum.Studio.Controls
{
    partial class PrinterConnectionManagerPopup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PrinterConnectionManagerPopup));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAddPrinter = new BaseToolStripButton();
            this.btnRemoveSelectedPrinter = new BaseToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.btnProperties = new BaseToolStripButton();
            this.btnSetAsDefault = new BaseToolStripButton();
            this.dgAvailablePrinters = new System.Windows.Forms.DataGridView();
            this.selectedDataGridViewCheckBoxColumn = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.atumV2PrinterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAvailablePrinters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.atumV2PrinterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // plContent
            // 
            this.plContent.Size = new System.Drawing.Size(513, 405);
            // 
            // toolStrip1
            // 
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAddPrinter,
            this.btnRemoveSelectedPrinter,
            this.toolStripSeparator1,
            this.btnProperties,
            this.btnSetAsDefault});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(513, 27);
            this.toolStrip1.TabIndex = 7;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAddPrinter
            // 
            this.btnAddPrinter.Image = global::Atum.Studio.Properties.Resources.Add;
            this.btnAddPrinter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAddPrinter.Name = "btnAddPrinter";
            this.btnAddPrinter.Size = new System.Drawing.Size(57, 24);
            this.btnAddPrinter.Text = "Add";
            this.btnAddPrinter.Click += new System.EventHandler(this.btnAddPrinter_Click);
            // 
            // btnRemoveSelectedPrinter
            // 
            this.btnRemoveSelectedPrinter.Image = global::Atum.Studio.Properties.Resources.Remove;
            this.btnRemoveSelectedPrinter.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnRemoveSelectedPrinter.Name = "btnRemoveSelectedPrinter";
            this.btnRemoveSelectedPrinter.Size = new System.Drawing.Size(83, 24);
            this.btnRemoveSelectedPrinter.Text = "Remove";
            this.btnRemoveSelectedPrinter.Click += new System.EventHandler(this.btnRemoveSelectedPrinter_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 27);
            // 
            // btnProperties
            // 
            this.btnProperties.Image = global::Atum.Studio.Properties.Resources.Properties;
            this.btnProperties.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnProperties.Name = "btnProperties";
            this.btnProperties.Size = new System.Drawing.Size(96, 24);
            this.btnProperties.Text = "Properties";
            this.btnProperties.Click += new System.EventHandler(this.btnProperties_Click);
            // 
            // btnSetAsDefault
            // 
            this.btnSetAsDefault.Image = global::Atum.Studio.Properties.Resources.SetAsDefault;
            this.btnSetAsDefault.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSetAsDefault.Name = "btnSetAsDefault";
            this.btnSetAsDefault.Size = new System.Drawing.Size(119, 24);
            this.btnSetAsDefault.Text = "Set as default";
            this.btnSetAsDefault.Click += new System.EventHandler(this.btnSetAsDefault_Click);
            // 
            // dgAvailablePrinters
            // 
            this.dgAvailablePrinters.AllowUserToAddRows = false;
            this.dgAvailablePrinters.AllowUserToDeleteRows = false;
            this.dgAvailablePrinters.AllowUserToResizeColumns = false;
            this.dgAvailablePrinters.AllowUserToResizeRows = false;
            this.dgAvailablePrinters.AutoGenerateColumns = false;
            this.dgAvailablePrinters.BackgroundColor = System.Drawing.Color.White;
            this.dgAvailablePrinters.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgAvailablePrinters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgAvailablePrinters.ColumnHeadersVisible = false;
            this.dgAvailablePrinters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.selectedDataGridViewCheckBoxColumn,
            this.nameDataGridViewTextBoxColumn});
            this.dgAvailablePrinters.DataSource = this.atumV2PrinterBindingSource;
            this.dgAvailablePrinters.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgAvailablePrinters.Location = new System.Drawing.Point(0, 27);
            this.dgAvailablePrinters.MultiSelect = false;
            this.dgAvailablePrinters.Name = "dgAvailablePrinters";
            this.dgAvailablePrinters.ReadOnly = true;
            this.dgAvailablePrinters.RowHeadersVisible = false;
            this.dgAvailablePrinters.RowTemplate.Height = 24;
            this.dgAvailablePrinters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgAvailablePrinters.Size = new System.Drawing.Size(513, 378);
            this.dgAvailablePrinters.TabIndex = 6;
            this.dgAvailablePrinters.SelectionChanged += new System.EventHandler(this.dgAvailablePrinters_SelectionChanged);
            // 
            // selectedDataGridViewCheckBoxColumn
            // 
            this.selectedDataGridViewCheckBoxColumn.DataPropertyName = "Selected";
            this.selectedDataGridViewCheckBoxColumn.HeaderText = "Selected";
            this.selectedDataGridViewCheckBoxColumn.Name = "selectedDataGridViewCheckBoxColumn";
            this.selectedDataGridViewCheckBoxColumn.ReadOnly = true;
            this.selectedDataGridViewCheckBoxColumn.Visible = false;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "DisplayText";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // atumV2PrinterBindingSource
            // 
            this.atumV2PrinterBindingSource.DataSource = typeof(Atum.DAL.Hardware.AtumPrinter);
            // 
            // PrinterConnectionManagerPopup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(513, 450);
            this.Controls.Add(this.dgAvailablePrinters);
            this.Controls.Add(this.toolStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "PrinterConnectionManagerPopup";
            this.Text = "Printer Manager";
            this.Load += new System.EventHandler(this.PrinterConnectionManagerPopup_Load);
            this.Controls.SetChildIndex(this.plContent, 0);
            this.Controls.SetChildIndex(this.toolStrip1, 0);
            this.Controls.SetChildIndex(this.dgAvailablePrinters, 0);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgAvailablePrinters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.atumV2PrinterBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private BaseToolStripButton btnAddPrinter;
        private BaseToolStripButton btnRemoveSelectedPrinter;
        private System.Windows.Forms.DataGridView dgAvailablePrinters;
        private System.Windows.Forms.BindingSource atumV2PrinterBindingSource;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private BaseToolStripButton btnSetAsDefault;
        private BaseToolStripButton btnProperties;
        private System.Windows.Forms.DataGridViewCheckBoxColumn selectedDataGridViewCheckBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    }
}
