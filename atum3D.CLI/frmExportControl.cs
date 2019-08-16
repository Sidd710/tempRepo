using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum3D.CLI
{
    public partial class frmExportControl : Form
    {
        public frmExportControl()
        {
            InitializeComponent();
        }

        private void FrmExportControl_Load(object sender, EventArgs e)
        {
            this.Text = "Export";
            this.Icon = BrandingManager.MainForm_Icon;

            this.StartPosition = FormStartPosition.CenterParent;
        }
    }
}
