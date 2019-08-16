
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public class GenericRichTextBoxRegular16: RichTextBox
    {
        public GenericRichTextBoxRegular16()
        {
            this.Font = FontManager.Montserrat16Regular;
        }
    }
}
