using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class TransparentPanel : Panel
    {
        public TransparentPanel()
        {
            InitializeComponent();

            SetStyle(ControlStyles.Opaque, true);

        }

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x00000020; // WS_EX_TRANSPARENT
                return cp;
            }
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            using (Graphics g = e.Graphics)
            {
                var p = new Pen(Color.Red, 3);
                var point1 = new Point(0, 0);
                var point2 = new Point(100, 100);
                g.DrawLine(p, point1, point2);
            }

        }
    }
}
