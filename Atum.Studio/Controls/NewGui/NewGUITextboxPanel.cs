using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui
{
    public partial class NewGUITextboxPanel : UserControl
    {
        //private Color colorBack = Color.Transparent;
        //public Color BackColor
        //{
        //    get
        //    {
        //        return colorBack;
        //    }
        //    set
        //    {
        //        colorBack = value;
        //    }
        //}

        public NewGUITextboxPanel()
        {
            InitializeComponent();
            // this.BorderColor = Color.FromArgb(255, 24, 0);
            //this.BackColor = colorBack;
        }

        private void plTextbox_Paint(object sender, PaintEventArgs e)
        {
            //base.OnPaint(e);
            //e.Graphics.DrawRectangle(
            //    new Pen(
            //        new SolidBrush(colorBorder), 2),
            //        e.ClipRectangle);
        }
    }
}
