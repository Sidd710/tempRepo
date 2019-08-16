namespace Atum.Studio.Controls.MagsAI
{
    partial class MagsAIOrientationTabPanel
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
            this.rdExcludeModelParts = new System.Windows.Forms.RadioButton();
            this.rdLayFlatByTriangle = new System.Windows.Forms.RadioButton();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSelectionBoxRound = new System.Windows.Forms.ToolStripButton();
            this.btnSelectionBoxSquare = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonColor1 = new System.Windows.Forms.ToolStripButton();
            this.trSelectionBoxSize = new Atum.Studio.Controls.ToolstripItems.ToolStripTrackBarItem();
            this.plOpenGL = new System.Windows.Forms.Panel();
            this.rdOrbit = new System.Windows.Forms.RadioButton();
            this.rdPan = new System.Windows.Forms.RadioButton();
            this.rdZoom = new System.Windows.Forms.RadioButton();
            this.toolStrip1.SuspendLayout();
            this.plOpenGL.SuspendLayout();
            this.SuspendLayout();
            // 
            // rdExcludeModelParts
            // 
            this.rdExcludeModelParts.AutoSize = true;
            this.rdExcludeModelParts.Location = new System.Drawing.Point(175, 3);
            this.rdExcludeModelParts.Name = "rdExcludeModelParts";
            this.rdExcludeModelParts.Size = new System.Drawing.Size(128, 17);
            this.rdExcludeModelParts.TabIndex = 1;
            this.rdExcludeModelParts.Text = "Excluded Model Parts";
            this.rdExcludeModelParts.UseVisualStyleBackColor = true;
            this.rdExcludeModelParts.CheckedChanged += new System.EventHandler(this.rdExcludeModelParts_CheckedChanged);
            // 
            // rdLayFlatByTriangle
            // 
            this.rdLayFlatByTriangle.AutoSize = true;
            this.rdLayFlatByTriangle.Checked = true;
            this.rdLayFlatByTriangle.Location = new System.Drawing.Point(3, 3);
            this.rdLayFlatByTriangle.Name = "rdLayFlatByTriangle";
            this.rdLayFlatByTriangle.Size = new System.Drawing.Size(166, 17);
            this.rdLayFlatByTriangle.TabIndex = 0;
            this.rdLayFlatByTriangle.TabStop = true;
            this.rdLayFlatByTriangle.Text = "Lay Flat by Model Intersection";
            this.rdLayFlatByTriangle.UseVisualStyleBackColor = true;
            this.rdLayFlatByTriangle.CheckedChanged += new System.EventHandler(this.rdLayFlatByTriangle_CheckedChanged);
            // 
            // toolStrip1
            // 
            this.toolStrip1.BackColor = System.Drawing.Color.White;
            this.toolStrip1.CanOverflow = false;
            this.toolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.toolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSelectionBoxRound,
            this.btnSelectionBoxSquare,
            this.toolStripButtonColor1,
            this.trSelectionBoxSize});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Flow;
            this.toolStrip1.Location = new System.Drawing.Point(126, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System;
            this.toolStrip1.Size = new System.Drawing.Size(167, 23);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSelectionBoxRound
            // 
            this.btnSelectionBoxRound.CheckOnClick = true;
            this.btnSelectionBoxRound.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectionBoxRound.Image = global::Atum.Studio.Properties.Resources.btnMAGSAI_Selection_Round;
            this.btnSelectionBoxRound.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectionBoxRound.Name = "btnSelectionBoxRound";
            this.btnSelectionBoxRound.Size = new System.Drawing.Size(23, 20);
            this.btnSelectionBoxRound.Text = "Round";
            this.btnSelectionBoxRound.Click += new System.EventHandler(this.btnSelectionBoxRound_Click);
            // 
            // btnSelectionBoxSquare
            // 
            this.btnSelectionBoxSquare.CheckOnClick = true;
            this.btnSelectionBoxSquare.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSelectionBoxSquare.Image = global::Atum.Studio.Properties.Resources.btnMAGSAI_Selection_Sqaure;
            this.btnSelectionBoxSquare.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSelectionBoxSquare.Name = "btnSelectionBoxSquare";
            this.btnSelectionBoxSquare.Size = new System.Drawing.Size(23, 20);
            this.btnSelectionBoxSquare.Text = "Square";
            this.btnSelectionBoxSquare.Click += new System.EventHandler(this.btnSelectionBoxSquare_Click);
            // 
            // toolStripButtonColor1
            // 
            this.toolStripButtonColor1.AutoSize = false;
            this.toolStripButtonColor1.Margin = new System.Windows.Forms.Padding(0);
            this.toolStripButtonColor1.Name = "toolStripButtonColor1";
            this.toolStripButtonColor1.Size = new System.Drawing.Size(20, 20);
            this.toolStripButtonColor1.ToolTipText = "Selectionbox Color";
            this.toolStripButtonColor1.Click += new System.EventHandler(this.toolStripButtonColor1_Click);
            this.toolStripButtonColor1.Paint += new System.Windows.Forms.PaintEventHandler(this.toolStripButtonColor1_Paint);
            // 
            // trSelectionBoxSize
            // 
            this.trSelectionBoxSize.Name = "trSelectionBoxSize";
            this.trSelectionBoxSize.Size = new System.Drawing.Size(100, 20);
            this.trSelectionBoxSize.Text = "Selection Size";
            this.trSelectionBoxSize.ToolTipText = "Selection Size";
            this.trSelectionBoxSize.Value = 1;
            this.trSelectionBoxSize.ValueChanged += new System.EventHandler(this.trSelectionBoxSize_ValueChanged);
            // 
            // plOpenGL
            // 
            this.plOpenGL.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.plOpenGL.Controls.Add(this.toolStrip1);
            this.plOpenGL.Location = new System.Drawing.Point(3, 26);
            this.plOpenGL.Name = "plOpenGL";
            this.plOpenGL.Size = new System.Drawing.Size(662, 517);
            this.plOpenGL.TabIndex = 3;
            // 
            // rdOrbit
            // 
            this.rdOrbit.AutoSize = true;
            this.rdOrbit.Location = new System.Drawing.Point(369, 3);
            this.rdOrbit.Name = "rdOrbit";
            this.rdOrbit.Size = new System.Drawing.Size(47, 17);
            this.rdOrbit.TabIndex = 4;
            this.rdOrbit.Text = "Orbit";
            this.rdOrbit.UseVisualStyleBackColor = true;
            this.rdOrbit.CheckedChanged += new System.EventHandler(this.rdOrbit_CheckedChanged);
            // 
            // rdPan
            // 
            this.rdPan.AutoSize = true;
            this.rdPan.Location = new System.Drawing.Point(422, 3);
            this.rdPan.Name = "rdPan";
            this.rdPan.Size = new System.Drawing.Size(44, 17);
            this.rdPan.TabIndex = 5;
            this.rdPan.Text = "Pan";
            this.rdPan.UseVisualStyleBackColor = true;
            this.rdPan.CheckedChanged += new System.EventHandler(this.rdPan_CheckedChanged);
            // 
            // rdZoom
            // 
            this.rdZoom.AutoSize = true;
            this.rdZoom.Location = new System.Drawing.Point(472, 3);
            this.rdZoom.Name = "rdZoom";
            this.rdZoom.Size = new System.Drawing.Size(52, 17);
            this.rdZoom.TabIndex = 6;
            this.rdZoom.Text = "Zoom";
            this.rdZoom.UseVisualStyleBackColor = true;
            this.rdZoom.CheckedChanged += new System.EventHandler(this.rdZoom_CheckedChanged);
            // 
            // MagsAIOrientationTabPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.rdZoom);
            this.Controls.Add(this.rdPan);
            this.Controls.Add(this.rdOrbit);
            this.Controls.Add(this.plOpenGL);
            this.Controls.Add(this.rdExcludeModelParts);
            this.Controls.Add(this.rdLayFlatByTriangle);
            this.Name = "MagsAIOrientationTabPanel";
            this.Size = new System.Drawing.Size(668, 546);
            this.Load += new System.EventHandler(this.MagsAIOrientationTabPanel_Load);
            this.Resize += new System.EventHandler(this.MagsAIOrientationTabPanel_Resize);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.plOpenGL.ResumeLayout(false);
            this.plOpenGL.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RadioButton rdExcludeModelParts;
        private System.Windows.Forms.RadioButton rdLayFlatByTriangle;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnSelectionBoxRound;
        private System.Windows.Forms.ToolStripButton btnSelectionBoxSquare;
        private ToolstripItems.ToolStripTrackBarItem trSelectionBoxSize;
        private System.Windows.Forms.ToolStripButton toolStripButtonColor1;
        private System.Windows.Forms.Panel plOpenGL;
        private System.Windows.Forms.RadioButton rdOrbit;
        private System.Windows.Forms.RadioButton rdPan;
        private System.Windows.Forms.RadioButton rdZoom;
    }
}
