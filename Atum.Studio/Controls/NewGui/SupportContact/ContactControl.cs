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
using System.Reflection;

namespace Atum.Studio.Controls.NewGui.SupportContact
{
    public partial class ContactControl : UserControl
    {

        public string AssemblyVersion
        {
            get
            {
                Assembly entryAssembly = Assembly.GetEntryAssembly();

                return string.Format("{0}.{1}.{2}.{3}", Assembly.GetEntryAssembly().GetName().Version.Major, Assembly.GetEntryAssembly().GetName().Version.Minor, Assembly.GetEntryAssembly().GetName().Version.Build, Assembly.GetEntryAssembly().GetName().Version.Revision);
            }
        }

        public ContactControl()
        {
            InitializeComponent();

        }

        private void ContactControl_Load(object sender, EventArgs e)
        {

            if (FontManager.Loaded)
            {
                this.label1.Font = this.label2.Font = this.label3.Font = FontManager.Montserrat14Regular;
                this.lblVersion.Font = FontManager.Montserrat18Regular;
            }

            if (!ConnectivityManager.InternetAvailable)
            {
                this.operatorStationOnlineManual1.Enabled = false;
                this.submitBugFeatureRequest1.Enabled = false;
            }

            this.lblVersion.Text = String.Format("{0}", AssemblyVersion);

            this.picLogo.BackColor = this.BackColor;
            this.picLogo.BackgroundImage = BrandingManager.Splash_Logo;


        }
    }
}
