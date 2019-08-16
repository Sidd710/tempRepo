using System;
using System.Collections.Generic;

namespace Atum.DAL.Licenses
{
    public class AvailableLicense
    {
        public enum TypeOfLicense
        {
            Unknown = 0,
            StudioStandard = 11,
            StudioLevel2 = 12,
            StudioLevel3 = 13,
            Dental = 20,
            Trial = 100,
        }

        public AvailableLicenseProperties Properties { get; set; }
        public TypeOfLicense LicenseType { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Activated { get; set; }
        public Guid LicenseGUID { get; set; }
        public int Amount { get; set; }

        public AvailableLicenseActivities Activities { get; set; }

        public AvailableLicense()
        {
            this.Properties = new AvailableLicenseProperties();
            this.Activities = new AvailableLicenseActivities();
        }
    }
}
