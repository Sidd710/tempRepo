using System;
using System.Drawing;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SceneControlToolbars
{
    public partial class SceneControlToolbarBase : UserControl
    {
        public SceneControlToolbarBase()
        {
            InitializeComponent();

            this.Hide();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.BackColor = Color.Transparent;
        }

        private void Show(BaseToolStripButton buttonPressed)
        {
            this.Left = 3;
            this.Top = buttonPressed.Bounds.Top;
            this.Visible = true;
        }

        private void Show(Point mousePoint)
        {
            this.Left = mousePoint.X + 10 ;
            this.Top = mousePoint.Y;
            this.Visible = true;
        }

        private new void Hide()
        {
            this.Visible = false;
        }

        internal void UpdatePosition()
        {
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
        }

        internal void ProcessNumericValues(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1 ||
                e.KeyCode == Keys.D2 ||
                e.KeyCode == Keys.D3 ||
                e.KeyCode == Keys.D4 ||
                e.KeyCode == Keys.D5 ||
                e.KeyCode == Keys.D6 ||
                e.KeyCode == Keys.D7 ||
                e.KeyCode == Keys.D8 ||
                e.KeyCode == Keys.D9 ||
                e.KeyCode == Keys.D0 ||
                e.KeyCode == Keys.Enter ||
                e.KeyCode == Keys.Tab ||
                e.KeyCode == Keys.OemPeriod ||
                e.KeyCode == Keys.Oemcomma
                )
            {
                
            }
            else
            {
                frmStudioMain.SceneControl.ProcessKeyDown(this, e);
            }
        }

        private void SceneControlToolbarBase_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessNumericValues(e);
        }
    }
}
