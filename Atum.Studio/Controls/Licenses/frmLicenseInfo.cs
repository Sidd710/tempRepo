using Atum.Studio.Core.Managers;
using System;
using System.Diagnostics;
using System.Net;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class frmLicenseInfo : BasePopup
    {
        public frmLicenseInfo()
        {
            InitializeComponent();

            LicenseClientManager.AvailableLicensesChanged += LicenseClientManager_AvailableLicensesChanged;
            LicenseClientManager.PendingLicensesChanged += LicenseClientManager_PendingLicensesChanged;
        }

        
        private void frmLicenseInfo_Load(object sender, EventArgs e)
        {
            this.txtLicenseServerName.Text = UserProfileManager.UserProfile.LicenseServer_ServerName;

            UpdateControl();
        }

        private void LicenseClientManager_PendingLicensesChanged(object sender, EventArgs e)
        {
            UpdateControl();
        }

        private void LicenseClientManager_AvailableLicensesChanged(object sender, EventArgs e)
        {
            UpdateControl();
        }

        private void UpdateControl()
        {
            if (this.dgLicenses.InvokeRequired)
            {
                this.dgLicenses.Invoke(new MethodInvoker(delegate { UpdateControl(); }));
                return;
            }

            this.dgLicenses.Rows.Clear();

            foreach (var pendingLicense in LicenseClientManager.PendingClientLicenseRequests)
            {
                this.dgLicenses.Rows.Add(new[] { pendingLicense.LicenseType.ToString(), "Pending" });
            }

            foreach (var activeLicense in LicenseClientManager.AvailableLicenses)
            {
                this.dgLicenses.Rows.Add(new[] { activeLicense.LicenseType.ToString(), "Activated" });
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                LicenseClientManager.Stop();

                Dns.GetHostAddresses(this.txtLicenseServerName.Text);
                UserProfileManager.UserProfile.LicenseServer_ServerName = this.txtLicenseServerName.Text;

                errorProvider.SetError(this.txtLicenseServerName, "");

                LicenseClientManager.Start();

                var licenseRequest = new DAL.Licenses.PendingClientLicenseRequest()
                {
                    LicenseType = DAL.Licenses.PendingClientLicenseRequest.TypeOfClientLicenseRequest.Studio
                };
                LicenseClientManager.AddPendingLicenseRequest(licenseRequest);
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
                errorProvider.SetError(this.txtLicenseServerName, "Invalid hostname");
                
            }
            
        }
    }
}
