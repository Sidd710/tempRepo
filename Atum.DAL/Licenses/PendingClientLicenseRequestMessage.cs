using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class PendingClientLicenseRequestMessage
    {
        public string Message { get; set; }

        public PendingClientLicenseRequestMessage()
        {

        }

        internal PendingClientLicenseRequest DecodeToXml()
        {
            return PendingClientLicenseRequest.FromEncodedMessage(this.Message);
        }

        internal static PendingClientLicenseRequestMessage Deserialize(TextReader textReader)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(PendingClientLicenseRequestMessage));

            return (PendingClientLicenseRequestMessage)serializer.Deserialize(textReader);
        }
    }
}
