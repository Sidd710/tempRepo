using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.UserPreferences
{
    public class AdvancedLicensesPreferences
    {
        [Category("Licenses")]
        [DisplayName("License Server Name or IP")]
        [Description("License Server Name or IP")]
        public string LicenseServerNameOrIP { get; set; }

        public AdvancedLicensesPreferences(string licenseServerNameOrIP)
        {
            this.LicenseServerNameOrIP = licenseServerNameOrIP;
        }
    }
}
