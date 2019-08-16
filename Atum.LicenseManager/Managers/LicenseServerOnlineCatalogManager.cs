using Atum.DAL.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Xml;
using System.Xml.Linq;

namespace Atum.LicenseManager.Managers
{
    class LicenseServerOnlineCatalogManager
    {
        private static Timer _tmrOnlineCatalogManager;

        public static Action<DAL.Licenses.OnlineCatalogLicenses> OnCatalogLicensesFetched;
        public static string CustomerID { get; set; }

        public static void Start()
        {
            _tmrOnlineCatalogManager = new Timer();
            _tmrOnlineCatalogManager.Interval = 300000;
            _tmrOnlineCatalogManager.Elapsed += _tmrOnlineCatalogManager_Tick;


            var customerLicPath = Path.Combine(DAL.ApplicationSettings.Settings.BasePath, "Customer.dat");
            if (File.Exists(customerLicPath))
            {
                using (var streamReader = new StreamReader(customerLicPath))
                {
                    CustomerID = streamReader.ReadLine();

                    EventLogManager.WriteToEventLog("License", "Customer ID " + CustomerID);
                }

                _tmrOnlineCatalogManager.Start();

                //start once on load
                _tmrOnlineCatalogManager_Tick(null, null);
            }
            else
            {
                EventLogManager.WriteToEventLog("License", "Customer ID not found");
            }
            
        }

        private static void _tmrOnlineCatalogManager_Tick(object sender, EventArgs e)
        {
            _tmrOnlineCatalogManager.Stop();

            var deserialisedLicenses = new DAL.Licenses.OnlineCatalogLicenses();
            var licenseServerURL = string.Format(Properties.Resources.LICENSE_CATALOG_URL, CustomerID);
            using (var webclient = new WebClient())
            {
                try
                {
                    var jsonData = webclient.DownloadString(licenseServerURL);

                    if (!string.IsNullOrEmpty(jsonData))
                    {
                        //convert to xml (change the dictionary entries 1-2-3 to serializerdata
                        var jsonAsXml = XDocument.Load(JsonReaderWriterFactory.CreateJsonReader(Encoding.ASCII.GetBytes(jsonData), new XmlDictionaryReaderQuotas()));
                        if (jsonAsXml != null)
                        {
                            XmlDocument xmlDoc = new XmlDocument();
                            xmlDoc.LoadXml(jsonAsXml.ToString());
                            foreach (XmlNode rootNode in xmlDoc.SelectNodes("/root"))
                            {
                                //skip numeric indexes
                                foreach (XmlNode numericNode in rootNode.ChildNodes)
                                {
                                    //convert sibbling nodes to 
                                    foreach (XmlNode licenseNode in numericNode.ChildNodes)
                                    {
                                        var deserializedLicense = new DAL.Licenses.OnlineCatalogLicense();
                                        foreach (XmlNode licenseDataNode in licenseNode.ChildNodes)
                                        {
                                            switch (licenseDataNode.Name)
                                            {
                                                case "code":
                                                    deserializedLicense.Description = licenseDataNode.InnerText;
                                                    break;
                                                case "activation_date":
                                                    foreach (XmlNode activationDateNode in licenseDataNode)
                                                    {
                                                        if (activationDateNode.Name == "date")
                                                        {
                                                            deserializedLicense.ActivationDate = DateTime.ParseExact(activationDateNode.InnerText, "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
                                                        }
                                                    }
                                                    break;
                                                case "expiration_date":
                                                    foreach (XmlNode activationDateNode in licenseDataNode)
                                                    {
                                                        if (activationDateNode.Name == "date")
                                                        {
                                                            deserializedLicense.ExpirationDate = DateTime.ParseExact(activationDateNode.InnerText, "yyyy-MM-dd HH:mm:ss.ffffff", CultureInfo.InvariantCulture);
                                                        }
                                                    }
                                                    break;
                                                case "active":
                                                    if (licenseDataNode.InnerText == "true")
                                                    {
                                                        deserializedLicense.Activated = true;
                                                    }
                                                    break;
                                                case "amount":
                                                    deserializedLicense.Amount = int.Parse(licenseDataNode.InnerText);
                                                    break;
                                                case "type":
                                                    deserializedLicense.LicenseTypeAsGuid = licenseDataNode.InnerText;
                                                    break;
                                            }
                                        }

                                        if (deserializedLicense.Activated && deserializedLicense.Amount > 0 && deserializedLicense.ExpirationDate >= DateTime.Now)
                                        {
                                            deserialisedLicenses.Add(deserializedLicense);
                                        }
                                    }
                                }
                            }
                        }

                        OnCatalogLicensesFetched?.Invoke(deserialisedLicenses);

                    }
                    else
                    {
                        EventLogManager.WriteToEventLog("Failed to fetch current license(s)", "Invalid license format");
                    }
                }
                catch (Exception exc)
                {
                    EventLogManager.WriteToEventLog("Failed to fetch current license(s)", exc.Message);
                }
            }

            _tmrOnlineCatalogManager.Start();
        }
    }
}
