using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui
{

    public partial class NewGUIInputBox : UserControl
    {
        public string TextValue
        {
            get
            {
                return this.txtValue.Text;
            }
            set
            {
                this.txtValue.Text = value;

                if (!DesignMode)
                {
                    this.Font = FontManager.Montserrat14Regular;
                }
            }
        }

        public NewGUIInputBox()
        {
            InitializeComponent();
        }

        private void NewGUIInputBox_KeyDown(object sender, KeyEventArgs e)
        {

        }

        private void NewGUIInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void NewGUIInputBox_KeyUp(object sender, KeyEventArgs e)
        {

        }
    }
}
