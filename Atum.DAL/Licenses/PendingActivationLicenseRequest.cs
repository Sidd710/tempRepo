using System;
using System.Collections.Generic;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class PendingActivationLicenseRequest
    {
        public enum TypeOfLicenseRequest
        {
            Unknown = 0,
            New = 10,
            ChangeAmount = 20,
            Renewal = 50,
        }
        
        public int LicenseId { get; set; }
        public PendingLicenseProperties Properties { get; set; }
        public TypeOfLicenseRequest Type { get; set; }
        public DateTime RequestedOn { get; set; }

        public PendingActivationLicenseRequest()
        {
            this.Properties = new PendingLicenseProperties();
        }
    }
}
