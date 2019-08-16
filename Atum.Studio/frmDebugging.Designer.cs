using DAL.Logging;
namespace Atum.Studio
{
    partial class frmDebugging
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.loggingInfoBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.logTimeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.phaseDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.propertyDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.valueDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.loggingInfoBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.AutoGenerateColumns = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.logTimeDataGridViewTextBoxColumn,
            this.phaseDataGridViewTextBoxColumn,
            this.propertyDataGridViewTextBoxColumn,
            this.valueDataGridViewTextBoxColumn});
            this.dataGridView1.DataSource = this.loggingInfoBindingSource;
            this.dataGridView1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView1.Location = new System.Drawing.Point(0, 0);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.RowHeadersVisible = false;
            this.dataGridView1.RowTemplate.Height = 24;
            this.dataGridView1.Size = new System.Drawing.Size(698, 551);
            this.dataGridView1.TabIndex = 0;
            // 
            // loggingInfoBindingSource
            // 
            this.loggingInfoBindingSource.DataSource = typeof(LoggingInfo);
            // 
            // logTimeDataGridViewTextBoxColumn
            // 
            this.logTimeDataGridViewTextBoxColumn.DataPropertyName = "LogTime";
            this.logTimeDataGridViewTextBoxColumn.HeaderText = "LogTime";
            this.logTimeDataGridViewTextBoxColumn.Name = "logTimeDataGridViewTextBoxColumn";
            this.logTimeDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // phaseDataGridViewTextBoxColumn
            // 
            this.phaseDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.phaseDataGridViewTextBoxColumn.DataPropertyName = "Phase";
            this.phaseDataGridViewTextBoxColumn.HeaderText = "Phase";
            this.phaseDataGridViewTextBoxColumn.Name = "phaseDataGridViewTextBoxColumn";
            this.phaseDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // propertyDataGridViewTextBoxColumn
            // 
            this.propertyDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.propertyDataGridViewTextBoxColumn.DataPropertyName = "Property";
            this.propertyDataGridViewTextBoxColumn.HeaderText = "Property";
            this.propertyDataGridViewTextBoxColumn.Name = "propertyDataGridViewTextBoxColumn";
            this.propertyDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // valueDataGridViewTextBoxColumn
            // 
            this.valueDataGridViewTextBoxColumn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.valueDataGridViewTextBoxColumn.DataPropertyName = "Value";
            this.valueDataGridViewTextBoxColumn.HeaderText = "Value";
            this.valueDataGridViewTextBoxColumn.Name = "valueDataGridViewTextBoxColumn";
            this.valueDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // frmDebugging
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 551);
            this.Controls.Add(this.dataGridView1);
            this.Name = "frmDebugging";
            this.Text = "Debugging";
            this.Load += new System.EventHandler(this.frmDebugging_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.loggingInfoBindingSource)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.DataGridViewTextBoxColumn logTimeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn phaseDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn propertyDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn valueDataGridViewTextBoxColumn;
        private System.Windows.Forms.BindingSource loggingInfoBindingSource;
    }
}