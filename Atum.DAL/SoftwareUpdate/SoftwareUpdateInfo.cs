using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.SoftwareUpdate
{
    [Serializable]
    public class SoftwareUpdateInfo
    {
        public string FirmwareVersion { get; set; }
        public string ClientVersion { get; set; }
    }
}
