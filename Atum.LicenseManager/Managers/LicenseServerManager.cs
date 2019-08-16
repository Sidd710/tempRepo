using Atum.DAL.Licenses;
using Atum.DAL.Managers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Timers;

namespace Atum.LicenseManager.Managers
{
    class LicenseServerManager
    {

        public static Timer tmrObsoleteLicenses = new Timer();
        public static AvailableLicenses AvailableLicenses { get; set; }

        public static void Start()
        {
            //obsolete timer start
            tmrObsoleteLicenses.Interval = 300000;
            tmrObsoleteLicenses.Elapsed += TmrObsoleteLicenses_Elapsed;
            tmrObsoleteLicenses.Start();


            //online licenses
            LicenseServerOnlineCatalogManager.OnCatalogLicensesFetched += OnCatalogLicensesFetched;

            ConnectionManager.Start(11006, false);
            ConnectionManager.AtumClientLicenseRequest += ConnectionManager_AtumClientLicenseRequest;
            ConnectionManager.AtumClientRemoveLicenseRequest += ConnectionManager_AtumClientRemoveLicenseRequest;


            AvailableLicenses = new AvailableLicenses();

            //check if previous license file exists then preload them
            if (File.Exists(DAL.ApplicationSettings.Settings.LocalLicenseFilePath))
            {
                var offlineLicenses = OnlineCatalogLicenses.FromLicenseFile();

                if (offlineLicenses != null)
                {
                    foreach (var offlineLicense in offlineLicenses)
                    {
                        if (offlineLicense.Activated && offlineLicense.Amount > 0 && offlineLicense.ExpirationDate >= DateTime.Now)
                        {
                            AvailableLicenses.Add(new AvailableLicense()
                            {
                                Activated = offlineLicense.Activated,
                                LicenseGUID = offlineLicense.LicenseGUID,
                                ExpirationDate = offlineLicense.ExpirationDate,
                                LicenseType = offlineLicense.LicenseType,
                                Amount = offlineLicense.Amount
                            });
                        }
                    }
                }

                EventLogManager.WriteToEventLog("Offline license(s) found", "Amount " + AvailableLicenses.Count);
            }

        }

        private static void TmrObsoleteLicenses_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (AvailableLicenses != null)
            {
                lock (AvailableLicenses)
                {
                    foreach(var availableLicense in AvailableLicenses)
                    {
                        for (var activityIndex = availableLicense.Activities.Count - 1; activityIndex >= 0; activityIndex--)
                        {
                            if (availableLicense.Activities[activityIndex].EndsOn < DateTime.Now)
                            {
                                EventLogManager.WriteToEventLog("Removed obsolete license activity", availableLicense.LicenseType.ToString() + ":" + availableLicense.Activities[activityIndex].Computername);
                                availableLicense.Activities.RemoveAt(activityIndex);
                            }
                        }
                    }
                }
            }
        }

        public static void Stop()
        {

        }

        public static void OnCatalogLicensesFetched(OnlineCatalogLicenses catalogLicenses)
        {
            if (catalogLicenses.Count > 0)
            {
                EventLogManager.WriteToEventLog("Amount of fetched license(s)", catalogLicenses.Count.ToString());

                //append additional licenses
                foreach (var catalogLicense in catalogLicenses)
                {
                    var availableLicense = AvailableLicenses.FirstOrDefault(s => s.LicenseType == catalogLicense.LicenseType);
                    if (availableLicense == null)
                    {
                        AvailableLicenses.Add(new AvailableLicense()
                        {
                            Activated = catalogLicense.Activated,
                            LicenseGUID = catalogLicense.LicenseGUID,
                            ExpirationDate = catalogLicense.ExpirationDate,
                            LicenseType = catalogLicense.LicenseType,
                            Amount = catalogLicense.Amount
                        });

                        EventLogManager.WriteToEventLog("Added available license", catalogLicense.LicenseType.ToString());
                    }
                    else
                    {
                        if (availableLicense.Amount != catalogLicense.Amount)
                        {
                            EventLogManager.WriteToEventLog("Updated available license amount", string.Format("{0}->{1}", availableLicense.Amount, catalogLicense.Amount));
                            availableLicense.Amount = catalogLicense.Amount;

                        }

                        if (availableLicense.ExpirationDate != catalogLicense.ExpirationDate)
                        {
                            EventLogManager.WriteToEventLog("Updated available license expiration date", string.Format("{0}->{1}", availableLicense.ExpirationDate.ToShortDateString(), catalogLicense.ExpirationDate.ToShortDateString()));
                            availableLicense.ExpirationDate = catalogLicense.ExpirationDate;
                        }
                    }
                }

                //remove obsolete licenses
                var obsoleteLicenses = new List<AvailableLicense>();
                foreach (var availableLicense in AvailableLicenses)
                {
                    if (!catalogLicenses.Exists(s => s.LicenseType == availableLicense.LicenseType))
                    {
                        obsoleteLicenses.Add(availableLicense);
                    }
                }

                foreach (var obsoleteLicense in obsoleteLicenses)
                {
                    AvailableLicenses.Remove(obsoleteLicense);
                }


                catalogLicenses.ToLicenseFile();
            }
            else
            {
                EventLogManager.WriteToEventLog("No available license found", string.Empty);
                AvailableLicenses.Clear();
            }



        }

        public static void ConnectionManager_AtumClientLicenseRequest(PendingClientLicenseRequest clientLicenseRequest)
        {
            EventLogManager.WriteToEventLog("Received client license request", clientLicenseRequest.LicenseType.ToString() + ":" + clientLicenseRequest.IPAddress);

            //license available?
            if (AvailableLicenses != null)
            {
                foreach (var availableLicense in AvailableLicenses)
                {
                    if (availableLicense != null)
                    {
                        if (clientLicenseRequest.LicenseType == PendingClientLicenseRequest.TypeOfClientLicenseRequest.Studio)
                        {
                            if (availableLicense.LicenseType == AvailableLicense.TypeOfLicense.StudioStandard)
                            {
                                //check if client computer has activity
                                if (availableLicense.Activities.Count(s => s.Computername.ToLower() == clientLicenseRequest.Computername.ToLower()) == 0)
                                {
                                    if (availableLicense.Amount - 1 >= availableLicense.Activities.Count)
                                    {
                                        //add activity
                                        availableLicense.Activities.Add(new AvailableLicenseActivity()
                                        {
                                            Computername = clientLicenseRequest.Computername.ToLower(),
                                            Username = clientLicenseRequest.Username.ToLower(),
                                            EndsOn = DateTime.Now.AddHours(2)
                                        });

                                        EventLogManager.WriteToEventLog("Added license activity", availableLicense.LicenseType.ToString() + ":" + clientLicenseRequest.Computername);
                                        EventLogManager.WriteToEventLog("Amount of licenses", availableLicense.LicenseType.ToString() + ":" + availableLicense.Activities.Count + "/" + availableLicense.Amount);
                                    }
                                    else
                                    {
                                        EventLogManager.WriteToEventLog("Maximum amount of available licenses reached", availableLicense.LicenseType.ToString());
                                    }

                                }
                                else
                                {
                                    //update endson
                                    foreach (var activity in availableLicense.Activities)
                                    {
                                        if (activity.Computername == clientLicenseRequest.Computername.ToLower())
                                        {
                                            activity.EndsOn = DateTime.Now.AddHours(2);
                                            EventLogManager.WriteToEventLog("Update license activity expiretime", availableLicense.LicenseType.ToString() + ":" + clientLicenseRequest.Computername);
                                        }
                                    }
                                }



                                var pendingClientLicenseResponse = new PendingClientLicenseResponse();
                                pendingClientLicenseResponse.LicenseType = AvailableLicense.TypeOfLicense.StudioStandard;
                                pendingClientLicenseResponse.IPAddress = clientLicenseRequest.IPAddress;
                                pendingClientLicenseResponse.Id = clientLicenseRequest.Id;

                                SendClientResponse(pendingClientLicenseResponse);

                                break;
                            }
                        }
                    }
                }
            }
        }

        public static void ConnectionManager_AtumClientRemoveLicenseRequest(RemoveActiveClientLicenseRequest removeLicenseRequest)
        {
            foreach (var activeLicense in AvailableLicenses)
            {
                for (var activityIndex = activeLicense.Activities.Count - 1; activityIndex >= 0; activityIndex--)
                {
                    if (activeLicense.Activities[activityIndex].Computername.ToLower() == removeLicenseRequest.Computername.ToLower())
                    {
                        activeLicense.Activities.RemoveAt(activityIndex);
                        EventLogManager.WriteToEventLog("Removed license activity", activeLicense.LicenseType.ToString() + ":" + removeLicenseRequest.Computername);
                    }
                }
            }
        }

        private static void SendClientResponse(PendingClientLicenseResponse clientLicenseResponse)
        {
            if (clientLicenseResponse != null)
            {
                EventLogManager.WriteToEventLog("Sending client license response", clientLicenseResponse.LicenseType.ToString() + ":" + clientLicenseResponse.IPAddress);
                var clientLicenseResponseAsEncodedXml = clientLicenseResponse.ToEncodedXml();
                var clientEncodedLicenseResponseMessage = new PendingClientLicenseResponseMessage();
                clientEncodedLicenseResponseMessage.Message = clientLicenseResponseAsEncodedXml;
                ConnectionManager.SendAsXML(clientEncodedLicenseResponseMessage, System.Net.IPAddress.Parse(clientLicenseResponse.IPAddress), 11000);
                EventLogManager.WriteToEventLog("Send client license response", clientLicenseResponse.LicenseType.ToString() + ":" + clientLicenseResponse.IPAddress);
            }
        }
    }
}
