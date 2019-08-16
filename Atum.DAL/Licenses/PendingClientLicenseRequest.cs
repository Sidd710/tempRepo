using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class PendingClientLicenseRequest
    {
        public enum TypeOfClientLicenseRequest
        {
            Unknown = 0,
            Studio = 10,
            Dental = 20,
        }

        public string Id { get; set; }
        public TypeOfClientLicenseRequest LicenseType { get; set; }
        public string Username { get; set; }
        public string IPAddress { get; set; }
        public string Computername { get; set; }


        public PendingClientLicenseRequest()
        {
            this.Id = Guid.NewGuid().ToString();
            if (!String.IsNullOrEmpty(Environment.UserDomainName))
            {
                this.Username = Environment.UserDomainName + "\\";
            }
            else
            {
                this.Username = "";
            }
            this.Username += Environment.UserName;
            this.Computername = Environment.MachineName;
        }

        public string ToEncodedXml()
        {
            var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(typeof(PendingClientLicenseRequest));
            var stringWriter = new StringWriter();
            xmlStreamWriter.Serialize(stringWriter, this);
            var encodedString = Helpers.CryptoHelper.Encrypt(stringWriter.ToString());
            return encodedString;
        }

        internal static PendingClientLicenseRequest FromEncodedMessage(string message)
        {
            var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(typeof(PendingClientLicenseRequest));
            var decodedMessage = Helpers.CryptoHelper.Decrypt(message);
            var stringReader = new StringReader(decodedMessage);
            return (PendingClientLicenseRequest)xmlStreamWriter.Deserialize(stringReader);
        }
    }
}
