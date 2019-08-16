using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    [Designer(typeof(WizardTabPanelDesigner))]
    public partial class WizardTabPanel : UserControl
    {
        public event EventHandler ButtonBack_Click;
        public event EventHandler ButtonNext_Click;

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public Panel Header
        {
            get { return this.plHeader; }
        }

        public bool ButtonBackEnabled
        {
            get
            {
                return this.btnBack.Enabled;
            }
            set
            {
                this.btnBack.Enabled = value;
            }
        }

        public bool ButtonBackVisible
        {
            get
            {
                return this.btnBack.Visible;
            }
            set
            {
                this.btnBack.Visible = value;
            }
        }

        public bool ButtonNextVisible
        {
            get
            {
                return this.btnNext.Visible;
            }
            set
            {
                this.btnNext.Visible = value;
            }
        }

        public string ButtonNextText
        {
            get
            {
                return this.btnNext.Text;
            }
            set
            {
                this.btnNext.Text = value;
            }
        }

        public bool ButtonNextEnabled
        {
            get
            {
                return this.btnNext.Enabled;
            }
            set
            {
                this.btnNext.Enabled = value;
            }
        }

        public bool ButtonFinished
        {
            get
            {
               return this.btnNext.Text == "Finished";
            }
            set
            {
                this.btnNext.Text = value ? "Finished" : "Next";
            }
        }

        public bool HideFooter
        {
            get
            {
                return !this.plFooter.Visible;
            }
            set
            {
                this.plFooter.Visible = !value;
            }
        }

        public WizardTabPanel()
        {
            InitializeComponent();

            //dpi correction
            this.btnBack.Top = this.btnNext.Top = (this.plFooter.Height / 2) - (this.btnNext.Height / 2) + 3 ;
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            if (this.ButtonBack_Click != null) { ButtonBack_Click(null, null); }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if (this.ButtonNext_Click != null) { ButtonNext_Click(null, null); }
        }

        private void plFooter_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.DarkGray), new Point(0, 0), new Point(this.plFooter.Width, 0));
        }

        private void plHeader_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.DrawLine(new Pen(Color.Gainsboro, 1), 0, 50, 10000, 50);
            e.Graphics.DrawLine(new Pen(Color.Gainsboro, 1), 0, 0, 1000, 0);
        }

    }

    public class WizardTabPanelDesigner : System.Windows.Forms.Design.ParentControlDesigner
    {
        public override void Initialize(IComponent component)
        {
            base.Initialize(component);

            if (this.Control is WizardTabPanel) {
                this.EnableDesignMode(((WizardTabPanel)this.Control).plHeader, "Header");
            }

        }
    }
}
