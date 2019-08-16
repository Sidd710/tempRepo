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
    public partial class GenericTextboxRegular16 : TextBox
    {
        public GenericTextboxRegular16()
        {
            InitializeComponent();

            this.BackColor = Color.White;

            this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            this.SetStyle(ControlStyles.DoubleBuffer, true);


            if (FontManager.Loaded)
            {
                this.Font = FontManager.Montserrat16Regular;
            }

            this.BorderStyle = BorderStyle.None;
        }
    }
}
