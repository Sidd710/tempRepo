using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.SupportContact
{
    public partial class frmContact : NewGUIFormBase
    {
        public frmContact()
        {
            InitializeComponent();

//            this.lblHeader.Text = this.Text;
//#if LOCTITE
//#else
//            this.lblVersion.Padding += new Padding(0, 0, 0, 4);
//#endif
        }


        public string AssemblyVersion
        {
            get
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                return string.Format("{0}.{1}.{2}.{3}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor, Assembly.GetEntryAssembly().GetName().Version.Build, Assembly.GetEntryAssembly().GetName().Version.Revision);
            }
        }

        private void frmContact_Load(object sender, EventArgs e)
        {
            if (FontManager.Loaded)
            {
                //this.lblHeader.Font = new Font(FontManager.MontserratRegular, 14, FontStyle.Regular, GraphicsUnit.Pixel);
                //this.lblVersion.Font = new Font(FontManager.MontserratRegular, 10, FontStyle.Regular, GraphicsUnit.Pixel);
            }
        }
    }
}
