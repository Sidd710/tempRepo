using Atum.Studio.Controls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.LicenseKeyGenerator
{
    public partial class Form1 : BasePopup
    {
        public Form1()
        {
            InitializeComponent();

            this.dateTimePicker1.Value = DateTime.Now.AddMonths(6);
        }

            private void txtRequestedCode_TextChanged(object sender, EventArgs e)
        {
            GenerateActivationKey();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GenerateActivationKey();
        }

        private void GenerateActivationKey()
        {
            if (!string.IsNullOrEmpty(this.txtRequestedCode.Text))
            {
                var licenseFile = DAL.Licenses.OnlineCatalogLicenses.FromLicenseStream(this.txtRequestedCode.Text);
                this.lblRequestedLicenseAmount.Text = licenseFile[0].LicenseType.ToString();

                if (licenseFile.Count == 1)
                {
                    licenseFile[0].Activated = true;
                    licenseFile[0].ExpirationDate = this.dateTimePicker1.Value;
                    licenseFile[0].ActivationDate = DateTime.Now;
                    licenseFile[0].LicenseType = DAL.Licenses.AvailableLicense.TypeOfLicense.StudioStandard;
                }

                this.txtActivationCode.Text = licenseFile.ToLicenseRequest();
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
