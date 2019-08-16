using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Licenses.Activator.Objects
{
    [Serializable]
    public class AtumEmailLicenseRequest
    {
        public string Sender { get; set; }
        public DateTime ReceivedDateTime { get; set; }
        public Atum.DAL.Licenses.OnlineCatalogLicense LicenseRequest { get; set; }

        public AtumEmailLicenseRequest()
        {

        }

        public AtumEmailLicenseRequest(string sender, DateTime receivedDateTime, Atum.DAL.Licenses.OnlineCatalogLicense licenseRequest)
        {
            this.Sender = sender;
            this.ReceivedDateTime = receivedDateTime;
            this.LicenseRequest = licenseRequest;
        }

        public void Save()
        {
            var saveFilePath = "atum3D";
            if (!Directory.Exists(saveFilePath)) Directory.CreateDirectory(saveFilePath);
            saveFilePath = Path.Combine(saveFilePath, ReceivedDateTime.Year.ToString("D2"));
            if (!Directory.Exists(saveFilePath)) Directory.CreateDirectory(saveFilePath);
            saveFilePath = Path.Combine(saveFilePath, ReceivedDateTime.Month.ToString("D2"));
            if (!Directory.Exists(saveFilePath)) Directory.CreateDirectory(saveFilePath);
            saveFilePath = Path.Combine(saveFilePath, ReceivedDateTime.Day.ToString("D2"));
            if (!Directory.Exists(saveFilePath)) Directory.CreateDirectory(saveFilePath);
            saveFilePath = Path.Combine(saveFilePath, DateTime.Now.Ticks.ToString() + ".xml");

            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AtumEmailLicenseRequest));
            using (var fileWriter = new FileStream(saveFilePath, FileMode.Create))
            {
                xmlSerializer.Serialize(fileWriter, this);
            }
                
        }

        public string SaveAsString()
        {
            var xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(AtumEmailLicenseRequest));
            using (var fileWriter = new StringWriter())
            {
                xmlSerializer.Serialize(fileWriter, this);

                return fileWriter.ToString();
            }
        }
    }
}
