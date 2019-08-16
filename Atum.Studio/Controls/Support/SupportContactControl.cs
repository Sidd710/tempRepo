using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.Support
{
    public partial class SupportContactControl : UserControl
    {
        public SupportContactControl()
        {
            InitializeComponent();
        }

        private void plHeader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Gainsboro, 1), 0, 50, this.plHeader.Width, 50);
        }
    }
}
