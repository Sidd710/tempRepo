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

    public partial class NewGUIInputBoxReadonly : UserControl
    {
        public event EventHandler onSelected;

        public bool Selected { get; set; }

        public string TextValue
        {
            get
            {
                return this.txtValue.Text;
            }
            set
            {
                this.txtValue.Text = value;

                if (!DesignMode)
                {
                    this.Font = FontManager.Montserrat14Regular;
                }
            }
        }

        public NewGUIInputBoxReadonly()
        {
            InitializeComponent();

            this.txtValue.Click += NewGUIInputBoxReadonly_Click;
            this.Click += NewGUIInputBoxReadonly_Click;
        }

        private void NewGUIInputBoxReadonly_Click(object sender, EventArgs e)
        {
            this.onSelected?.Invoke(this, null);

            this.Selected = true;
        }
    }
}
