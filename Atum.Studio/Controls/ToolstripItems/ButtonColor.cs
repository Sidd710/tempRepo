using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.Design;

namespace Atum.Studio.Controls.ToolstripItems
{
    [ToolStripItemDesignerAvailability(ToolStripItemDesignerAvailability.ToolStrip | ToolStripItemDesignerAvailability.StatusStrip)]
    public class ToolStripButtonColor : ToolStripControlHost
    {
        public ToolStripButtonColor()
        : base(CreateControlInstance())
    {
        }

        public ToolStripButton ToolStripButton2
        {
            get
            {
                return Control as ToolStripButton;
            }
        }
        
        private static Control CreateControlInstance()
        {
            ToolStripButton t = new ToolStripButton();
            t.AutoSize = false;
            return t;
        }

        /// <param name="control"></param>
        protected override void OnSubscribeControlEvents(Control control)
        {
            base.OnSubscribeControlEvents(control);
            Button trackBar = control as Button;
            trackBar.Click += new EventHandler(button_Clicked);
        }

        protected override void OnUnsubscribeControlEvents(Control control)
        {
            base.OnUnsubscribeControlEvents(control);
            Button trackBar = control as Button;
            trackBar.Click -= new EventHandler(button_Clicked);
        }

        void button_Clicked(object sender, EventArgs e)
        {
            this.Clicked?.Invoke(sender, e);
        }
        public event EventHandler Clicked;
       
        protected override Size DefaultSize
        {
            get
            {
                return new Size(20, 20);
            }
        }
    }
}
