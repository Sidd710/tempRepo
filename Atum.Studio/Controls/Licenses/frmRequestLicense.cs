using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.Licenses
{
    public partial class frmRequestLicense : BasePopup
    {
        private DAL.Licenses.OnlineCatalogLicenses _license;

        public DAL.Licenses.OnlineCatalogLicenses DataSource
        {
            get
            {
                return this._license;
            }
            set
            {
                this._license = value;

                this.UpdateControl();
            }
        }

        public frmRequestLicense()
        {
            InitializeComponent();

            this.Icon = Core.Managers.BrandingManager.MainForm_Icon;

        }

        void UpdateControl()
        {
            if (this._license[0].LicenseType == DAL.Licenses.AvailableLicense.TypeOfLicense.Trial && (this._license[0].ExpirationDate >= DateTime.Now))
            {
                this.lblCurrentStatus.Text = "Trial";
            }
            else if (this._license[0].ExpirationDate < DateTime.Now)
            {
                this.lblCurrentStatus.Text = "Expired";
            }
            else
            {
                this.lblCurrentStatus.Text = "Activated";
            }

            this.lblExpiresAfter.Text = this._license[0].ExpirationDate.ToShortDateString();

            this.txtLicenseInfo.Text = this._license.ToLicenseRequest();
        }

        private void frmRequestLicense_Load(object sender, EventArgs e)
        {
            this.lblSendLicenseTo.Text = string.Format(this.lblSendLicenseTo.Text, Core.Managers.BrandingManager.LicenseManager_EmailAddress);
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(txtLicenseInfo.Text);
        }

        private void btnFromClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                txtActivationCode.Text = Clipboard.GetText();
            }
            catch
            {

            }
        }

        private void txtActivationCode_TextChanged(object sender, EventArgs e)
        {
            //parse activation code
            ParseActivationCode();

        }

        private void ParseActivationCode()
        {
            if (!string.IsNullOrEmpty(this.txtActivationCode.Text))
            {
                try
                {
                    this.DataSource = DAL.Licenses.OnlineCatalogLicenses.FromLicenseStream(this.txtActivationCode.Text);
                    this.DataSource.ToLicenseFile();
                    MessageBox.Show(this.DataSource[0].Activated ? "License activated" : "Activation failed", "License activation", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    this.lblCurrentStatus.Text = "Activated";
                    this.lblExpiresAfter.Text = this.DataSource[0].ExpirationDate.ToShortDateString();
                }
                catch
                {

                }
            }
        }
    }
}
