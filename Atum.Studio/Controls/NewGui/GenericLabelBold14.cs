using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public class GenericLabelBold14: Label
    {
        public GenericLabelBold14()
        {
            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat14Bold;
            }
        }
    }
}
