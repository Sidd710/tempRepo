using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class BasePopupNoTitle : Form
    {
        public BasePopupNoTitle()
        {
            InitializeComponent();

        }

        internal virtual void btnOK_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }

        private void plFooter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(0, 0), new Point(this.plFooter.Width, 0));
        }

    }
}
