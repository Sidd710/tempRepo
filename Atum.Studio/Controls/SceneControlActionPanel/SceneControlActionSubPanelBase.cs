using Atum.Studio.Core.Managers;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlActionSubPanelBase : UserControl
    {
        internal event EventHandler onClosed;

        protected bool _updateModel = true;

        private string _headerText = string.Empty;

        public string HeaderText {
            get { 
                return this._headerText;
            }
            set
            {
                this._headerText = value;
                this.lblHeader.Text = value;
            }
        }

        public SceneControlActionSubPanelBase()
        {
            InitializeComponent();

            if (FontManager.Loaded)
            {
                this.lblHeader.Font = FontManager.Montserrat14Bold;
            }
            this.btnClose.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.cross_black, btnClose.Size);

            this.Hide();
        }

        public void Show(PictureBox buttonPressed)
        {
            if (buttonPressed != null)
            {
                this.lblHeader.Font = FontManager.Montserrat16Bold;
                this.Left = buttonPressed.Parent.Location.X + buttonPressed.Parent.Width + 16;
                this.Top = buttonPressed.Parent.Top + buttonPressed.Location.Y;
                this.Visible = true;
            }
        }

        public void Show(Point mousePoint)
        {
            this.lblHeader.Font = FontManager.Montserrat16Bold;
            this.Left = mousePoint.X + 10 ;
            this.Top = mousePoint.Y;
            this.Visible = true;
        }

        public new void Hide()
        {
            this.Visible = false;
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

        private void SceneControlActionPanelBase_KeyDown(object sender, KeyEventArgs e)
        {
            this.ProcessNumericValues(e);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
            onClosed?.Invoke(null, null);
        }
    }
}
