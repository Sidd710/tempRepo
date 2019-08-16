using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.LicenseManager
{
    public partial class frmLicenseManager : Form
    {
        public frmLicenseManager()
        {
            InitializeComponent();
        }

        private void frmLicenseManager_Load(object sender, EventArgs e)
        {
            DAL.Managers.LoggingManager.Start();
            Managers.LicenseServerManager.Start();

            Managers.LicenseServerOnlineCatalogManager.Start();
        }
    }
}
