using System;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{ 

    public partial class TriangleSelectionBox : TransparentPanel
    {
        public event EventHandler<MouseEventArgs> OnChildMouseMove;

        public TriangleSelectionBox()
        {
            InitializeComponent();

            this.Controls.Add(new ComboBox());
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            this.OnChildMouseMove?.Invoke(null, e);
        }
    }
}
