using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class PendingClientLicenseResponse
    {
        public string Id { get; set; }
        public string IPAddress { get; set; }
        public AvailableLicense.TypeOfLicense LicenseType { get; set; }
     
        public PendingClientLicenseResponse()
        {
        }

        public string ToEncodedXml()
        {
            var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(typeof(PendingClientLicenseResponse));
            var stringWriter = new StringWriter();
            xmlStreamWriter.Serialize(stringWriter, this);
            var encodedString = Helpers.CryptoHelper.Encrypt(stringWriter.ToString());
            return encodedString;
        }

        internal static PendingClientLicenseResponse FromEncodedMessage(string message)
        {
            var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(typeof(PendingClientLicenseResponse));
            var decodedMessage = Helpers.CryptoHelper.Decrypt(message);
            var stringReader = new StringReader(decodedMessage);
            return (PendingClientLicenseResponse)xmlStreamWriter.Deserialize(stringReader);
        }
    }
}
