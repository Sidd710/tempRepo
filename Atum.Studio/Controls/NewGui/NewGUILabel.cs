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
    public partial class NewGUILabel : Label
    {
        public NewGUILabel()
        {
            InitializeComponent();

            this.AutoSize = false;
            this.Size = new Size(100,18);
            this.TextAlign = ContentAlignment.MiddleLeft;
        }

        private void NewGUILabel_TextChanged(object sender, System.EventArgs e)
        {
            if (!DesignMode)
            {
                this.Font = FontManager.Montserrat14Bold;
            }
        }


    }
}
