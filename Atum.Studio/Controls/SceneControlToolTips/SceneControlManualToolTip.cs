using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SceneControlToolTips
{
    internal class SceneControlManualToolTip: UserControl
    {
        private string _text = string.Empty;
        private Label lblToolTipText;
        private Timer _tmrToolTipVisibility = new Timer();

        internal SceneControlManualToolTip()
        {
            InitializeComponent();

            _tmrToolTipVisibility.Tick += _tmrToolTipVisibility_Tick;
            _tmrToolTipVisibility.Interval = 2500;

            if (FontManager.Loaded)
            {
                this.lblToolTipText.Font = FontManager.Montserrat14Regular;
            }
        }

        internal void SetText(string text)
        {
            this.lblToolTipText.Text = text;
            this.Width = TextRenderer.MeasureText(text, this.lblToolTipText.Font).Width + 32;
        }

        internal void ShowToolTip(Point startPoint)
        {
            this.Location = startPoint;
            frmStudioMain.SceneControl.Controls.Add(this);
            _tmrToolTipVisibility.Start();
        }

        private void _tmrToolTipVisibility_Tick(object sender, EventArgs e)
        {
            _tmrToolTipVisibility.Stop();

            if (frmStudioMain.SceneControl.InvokeRequired)
            {
                frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate
                {
                    frmStudioMain.SceneControl.Controls.Remove(this);
                    frmStudioMain.SceneControl.Render();
                }));
            }
            else
            {
                frmStudioMain.SceneControl.Controls.Remove(this);
                frmStudioMain.SceneControl.Render();
            }
        }

        private void InitializeComponent()
        {
            this.lblToolTipText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblToolTipText
            // 
            this.lblToolTipText.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblToolTipText.BackColor = System.Drawing.Color.WhiteSmoke;
            this.lblToolTipText.Location = new System.Drawing.Point(2, 0);
            this.lblToolTipText.Name = "lblToolTipText";
            this.lblToolTipText.Padding = new System.Windows.Forms.Padding(16, 0, 0, 0);
            this.lblToolTipText.Size = new System.Drawing.Size(150, 40);
            this.lblToolTipText.TabIndex = 0;
            this.lblToolTipText.Text = "label1";
            this.lblToolTipText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SceneControlManualToolTip
            // 
            this.BackColor = System.Drawing.Color.Red;
            this.Controls.Add(this.lblToolTipText);
            this.DoubleBuffered = true;
            this.Name = "SceneControlManualToolTip";
            this.Size = new System.Drawing.Size(150, 40);
            this.ResumeLayout(false);

        }
    }
}
