using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Licenses
{
    [Serializable]
    public class RemoveActiveClientLicenseRequestMessage
    {
        public string Message { get; set; }

        public RemoveActiveClientLicenseRequestMessage()
        {

        }

        internal RemoveActiveClientLicenseRequest DecodeToXml()
        {
            return RemoveActiveClientLicenseRequest.FromEncodedMessage(this.Message);
        }

        internal static RemoveActiveClientLicenseRequestMessage Deserialize(TextReader textReader)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(RemoveActiveClientLicenseRequestMessage));
            return (RemoveActiveClientLicenseRequestMessage)serializer.Deserialize(textReader);
        }
    }
}
