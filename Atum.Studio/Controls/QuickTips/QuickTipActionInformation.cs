
using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Atum.Studio.Controls.QuickTips
{
    public partial class QuickTipActionInformation : UserControl
    {
        internal event EventHandler OnShown;

        public string QuickTipCaption { get
            {
                return this.lblCaption.Text;
            }
            set
            {
                this.lblCaption.Text = value;
            }
        }

        public string QuickTipText
        {
            get
            {
                return this.lblText.Text;
            }
            set
            {
                this.lblText.Text = value;
            }
        }

        public QuickTipActionInformation()
        {
            this.Visible = false;
            InitializeComponent();
        }

        internal new void Hide()
        {
            this.Visible = false;
        }

        internal new void Show()
        {
            this.Width = 500;
            this.Height = 600;
            this.Visible = true;
        }

        internal void Show(string caption, string text)
        {
            this.lblCaption.Text = caption;
            this.lblText.Text = text;
        }
    
        private void lblText_Resize(object sender, System.EventArgs e)
        {
            this.Width = this.lblText.Width + (2 * this.lblText.Left);
            this.Height = this.lblText.Height + (2 * this.lblText.Top);

            this.Visible = true;
            this.OnShown?.Invoke(null, null);
        }
    }
}
