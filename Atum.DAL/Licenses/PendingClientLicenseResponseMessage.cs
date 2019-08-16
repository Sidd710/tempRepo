using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class PendingClientLicenseResponseMessage
    {
        public string Message { get; set; }

        public PendingClientLicenseResponseMessage()
        {

        }

        internal PendingClientLicenseResponse DecodeToXml()
        {
            return PendingClientLicenseResponse.FromEncodedMessage(this.Message);
        }

        internal static PendingClientLicenseResponseMessage Deserialize(TextReader textReader)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(PendingClientLicenseResponseMessage));
            return (PendingClientLicenseResponseMessage)serializer.Deserialize(textReader);
        }
    }
}
