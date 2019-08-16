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
    public partial class GenericNumericUpDowRegular16 : NumericUpDown
    {
        public GenericNumericUpDowRegular16()
        {
            InitializeComponent();

            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat16Regular;
            }

            this.TextAlign = HorizontalAlignment.Right;

            this.BorderStyle = BorderStyle.None;
        }
    }
}
