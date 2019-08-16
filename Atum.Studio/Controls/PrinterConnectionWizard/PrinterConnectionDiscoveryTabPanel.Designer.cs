namespace Atum.Studio.Controls.PrinterConnectionWizard
{
    partial class PrinterConnectionDiscoveryTabPanel
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
            this.pgSearchResults = new System.Windows.Forms.ProgressBar();
            this.label1 = new System.Windows.Forms.Label();
            this.dgDiscoveredPrinters = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.atumV2PrinterBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.plHeader.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).BeginInit();
            this.plContent.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDiscoveredPrinters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.atumV2PrinterBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // txtHeader
            // 
            this.txtHeader.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)));
            this.txtHeader.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.txtHeader.Location = new System.Drawing.Point(64, 0);
            this.txtHeader.Size = new System.Drawing.Size(379, 43);
            this.txtHeader.Text = "Network discovery";
            this.txtHeader.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // picHeader
            // 
            this.picHeader.Image = global::Atum.Studio.Properties.Resources.printer_blue_icon;
            // 
            // plContent
            // 
            this.plContent.Controls.Add(this.dgDiscoveredPrinters);
            this.plContent.Controls.Add(this.label1);
            this.plContent.Controls.Add(this.pgSearchResults);
            this.plContent.Size = new System.Drawing.Size(443, 388);
            // 
            // pgSearchResults
            // 
            this.pgSearchResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgSearchResults.Location = new System.Drawing.Point(15, 29);
            this.pgSearchResults.Name = "pgSearchResults";
            this.pgSearchResults.Size = new System.Drawing.Size(416, 23);
            this.pgSearchResults.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Discovery";
            // 
            // dgDiscoveredPrinters
            // 
            this.dgDiscoveredPrinters.AllowUserToAddRows = false;
            this.dgDiscoveredPrinters.AllowUserToDeleteRows = false;
            this.dgDiscoveredPrinters.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgDiscoveredPrinters.AutoGenerateColumns = false;
            this.dgDiscoveredPrinters.BackgroundColor = System.Drawing.Color.White;
            this.dgDiscoveredPrinters.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgDiscoveredPrinters.ColumnHeadersVisible = false;
            this.dgDiscoveredPrinters.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn});
            this.dgDiscoveredPrinters.DataSource = this.atumV2PrinterBindingSource;
            this.dgDiscoveredPrinters.Location = new System.Drawing.Point(15, 59);
            this.dgDiscoveredPrinters.MultiSelect = false;
            this.dgDiscoveredPrinters.Name = "dgDiscoveredPrinters";
            this.dgDiscoveredPrinters.ReadOnly = true;
            this.dgDiscoveredPrinters.RowHeadersVisible = false;
            this.dgDiscoveredPrinters.RowTemplate.Height = 24;
            this.dgDiscoveredPrinters.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgDiscoveredPrinters.Size = new System.Drawing.Size(416, 289);
            this.dgDiscoveredPrinters.TabIndex = 3;
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "DisplayName";
            this.nameDataGridViewTextBoxColumn.HeaderText = "DisplayName";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // atumV2PrinterBindingSource
            // 
            this.atumV2PrinterBindingSource.DataSource = typeof(Atum.DAL.Hardware.AtumPrinter);
            // 
            // PrinterConnectionDiscoveryTabPanel
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.Name = "PrinterConnectionDiscoveryTabPanel";
            this.Size = new System.Drawing.Size(443, 485);
            this.Load += new System.EventHandler(this.PrinterConnectionDiscoveryTabPanel_Load);
            this.plHeader.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picHeader)).EndInit();
            this.plContent.ResumeLayout(false);
            this.plContent.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgDiscoveredPrinters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.atumV2PrinterBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar pgSearchResults;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.BindingSource atumV2PrinterBindingSource;
        private System.Windows.Forms.DataGridView dgDiscoveredPrinters;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
    }
}
