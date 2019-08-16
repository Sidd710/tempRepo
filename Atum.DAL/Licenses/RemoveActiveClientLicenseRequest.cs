using System;
using System.IO;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class RemoveActiveClientLicenseRequest { 
            public string Computername { get; set; }


            public RemoveActiveClientLicenseRequest()
            {
                this.Computername = Environment.MachineName;
            }

            public string ToEncodedXml()
            {
                var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(typeof(RemoveActiveClientLicenseRequest));
                var stringWriter = new StringWriter();
                xmlStreamWriter.Serialize(stringWriter, this);
                var encodedString = Helpers.CryptoHelper.Encrypt(stringWriter.ToString());
                return encodedString;
            }

            internal static RemoveActiveClientLicenseRequest FromEncodedMessage(string message)
            {
                var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(typeof(RemoveActiveClientLicenseRequest));
                var decodedMessage = Helpers.CryptoHelper.Decrypt(message);
                var stringReader = new StringReader(decodedMessage);
                return (RemoveActiveClientLicenseRequest)xmlStreamWriter.Deserialize(stringReader);
            }
        }

}
