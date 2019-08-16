using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.DAL.Licenses
{
    public class OnlineCatalogLicense
    {
        public Guid LicenseGUID { get; set; }
        public Guid SystemID { get; set; }
        public string Description { get; set; }
        public DateTime ActivationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Activated { get; set; }
        public int Amount { get; set; }
        public AvailableLicense.TypeOfLicense LicenseType { get; set; }

        public string _licentTypeAsGuid;
        public string LicenseTypeAsGuid
        {
            get
            {
                return this._licentTypeAsGuid;
            }
            set
            {
                this._licentTypeAsGuid = value;

                switch (value)
                {
                    case "5B54DCA1-D137-412F-A282-11F57E1D4964":
                        this.LicenseType = AvailableLicense.TypeOfLicense.StudioStandard;
                        break;

                    case "7C28B717-8561-4B87-B015-3388B49401C7":
                        this.LicenseType = AvailableLicense.TypeOfLicense.Dental;
                        break;
                }
            }
        }

        internal string ToEncodedString()
        {
            var xmlStreamWriter = new System.Xml.Serialization.XmlSerializer(this.GetType());
            var stringWriter = new StringWriter();
            xmlStreamWriter.Serialize(stringWriter, this);
            return Helpers.CryptoHelper.Encrypt(stringWriter.ToString());
        }

        internal static OnlineCatalogLicense FromEncodedString(string encodedString)
        {
            var decodedString = Helpers.CryptoHelper.Decrypt(encodedString);
            var xmlStreamReader = new System.Xml.Serialization.XmlSerializer(typeof(OnlineCatalogLicense));
            using (TextReader reader = new StringReader(decodedString))
            {
                return (OnlineCatalogLicense)xmlStreamReader.Deserialize(reader);
            }
        }
    }

    public class OnlineCatalogLicenses : List<OnlineCatalogLicense>
    {
        public void ToLicenseFile(string path)
        {
            using (var streamWriter = new StreamWriter(path, false))
            {
                foreach (var license in this)
                {
                    streamWriter.WriteLine(license.ToEncodedString());
                }
            }
        }

        public string ToLicenseRequest()
        {
            var result = new StringBuilder();
            result.AppendLine("---StartOfLicenseCode---");
            foreach (var license in this)
            {
                result.AppendLine(license.ToEncodedString());
            }
            result.AppendLine("---EndofLicenseCode---");

            return result.ToString();
        }

        public static OnlineCatalogLicenses FromLicenseFile()
        {
            var offlineLicenses = new OnlineCatalogLicenses();

            if (!Directory.Exists(ApplicationSettings.Settings.SettingsPath))
            {
                Directory.CreateDirectory(ApplicationSettings.Settings.SettingsPath);
            }

            if (File.Exists(ApplicationSettings.Settings.LocalLicenseFilePath))
            {
                using (var streamReader = new StreamReader(ApplicationSettings.Settings.LocalLicenseFilePath))
                {
                    while (!streamReader.EndOfStream)
                    {
                        offlineLicenses.Add(OnlineCatalogLicense.FromEncodedString(streamReader.ReadLine()));
                    }
                }
            }
            else
            {
                offlineLicenses = CreateTrialLicense(DAL.ApplicationSettings.Settings.LocalLicenseFilePath);
            }

            return offlineLicenses;

        }

        public static OnlineCatalogLicenses CreateTrialLicense(string path)
        {
            var offlineLicenses = new OnlineCatalogLicenses();
            offlineLicenses.Add(new OnlineCatalogLicense() { ExpirationDate = DateTime.Now.AddMonths(2), LicenseType = AvailableLicense.TypeOfLicense.Trial, SystemID = Managers.SystemManager.GetUUID() });
            offlineLicenses.ToLicenseFile(path);

            return offlineLicenses;
        }

        public static OnlineCatalogLicenses FromLicenseStream(string requestedString)
        {
            if (!string.IsNullOrEmpty(requestedString))
            {
                var offlineLicenses = new OnlineCatalogLicenses();
                requestedString = requestedString.Replace("---StartOfLicenseCode---", "");
                requestedString = requestedString.Replace("---EndofLicenseCode---", "");
                requestedString = requestedString.Replace(Environment.NewLine, "");
                requestedString = requestedString.Replace("\\r\\n", "");
                offlineLicenses.Add(OnlineCatalogLicense.FromEncodedString(requestedString));

                return offlineLicenses;
            }
            else
            {
                return new OnlineCatalogLicenses();
            }
        }

        public static OnlineCatalogLicenses FromLicenseStreamUnencoded(string requestedString)
        {
            if (!string.IsNullOrEmpty(requestedString))
            {
                var offlineLicenses = new OnlineCatalogLicenses();
                requestedString = requestedString.Replace("---StartOfLicenseCode---", "");
                requestedString = requestedString.Replace("---EndofLicenseCode---", "");
                requestedString = requestedString.Replace(Environment.NewLine, "");
                offlineLicenses.Add(OnlineCatalogLicense.FromEncodedString(requestedString));

                return offlineLicenses;
            }
            else
            {
                return new OnlineCatalogLicenses();
            }
        }

        public static OnlineCatalogLicenses ValidateActivationLicenseStream(string activatedString)
        {
            var offlineLicenses = new OnlineCatalogLicenses();
            try
            {
                activatedString = activatedString.Replace("---StartOfLicenseCode---", "");
                activatedString = activatedString.Replace("---EndofLicenseCode---", "");
                activatedString = activatedString.Replace(Environment.NewLine, "");
                offlineLicenses.Add(OnlineCatalogLicense.FromEncodedString(activatedString));
                offlineLicenses.ToLicenseFile(DAL.ApplicationSettings.Settings.LocalLicenseFilePath);
            }
            catch
            {

            }

            return offlineLicenses;
        }
    }
}
