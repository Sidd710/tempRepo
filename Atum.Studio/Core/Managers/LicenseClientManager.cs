using Atum.DAL.Licenses;
using System;
using System.IO;
using System.Net;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    class LicenseClientManager
    {
        private static Timer _licenseRequestPoller;

        public static OnlineCatalogLicenses CurrentLicenses { get; set; }

        public static event EventHandler AvailableLicensesChanged;
        public static event EventHandler PendingLicensesChanged;

        public static AvailableLicenses AvailableLicenses { get; set; }
        public static PendingClientLicenseRequests ActivedLicenses { get; set; }
        public static PendingClientLicenseRequests PendingClientLicenseRequests { get; set; }

        public static void Start()
        {
            CurrentLicenses = OnlineCatalogLicenses.FromLicenseFile() ;

            PendingClientLicenseRequests = new PendingClientLicenseRequests();
            ActivedLicenses = new PendingClientLicenseRequests();
            AvailableLicenses = new AvailableLicenses();
            DAL.Managers.ConnectionManager.AtumClientLicenseResponse += ConnectionManager_AtumClientLicenseResponse;

            _licenseRequestPoller = new Timer();
            _licenseRequestPoller.Interval = 2000;
            _licenseRequestPoller.Tick += _licenseRequestPoller_Tick;
          //  _licenseRequestPoller.Start();

        }

        public static void SaveToFile()
        {
            if (CurrentLicenses != null)
            {
                CurrentLicenses.ToLicenseFile(DAL.ApplicationSettings.Settings.LocalLicenseFilePath);
            }
        }

        private static void _licenseRequestPoller_Tick(object sender, System.EventArgs e)
        {
            _licenseRequestPoller.Stop();

            if (!string.IsNullOrEmpty(UserProfileManager.UserProfile.LicenseServer_ServerName))
            {
                if (PendingClientLicenseRequests.Count > 0)
                {
                    _licenseRequestPoller.Interval = 2000;
                    foreach (var pendingRequest in PendingClientLicenseRequests)
                    {
                        SendRequest(pendingRequest);
                    }
                }
                else
                {
                    _licenseRequestPoller.Interval = 90000; //change timer when licenses are processed

                    //update activities in license manager
                    foreach(var activatedLicense in ActivedLicenses)
                    {
                        SendRequest(activatedLicense);
                    }
                }
            }

            _licenseRequestPoller.Start();
        }

        internal static void AddPendingLicenseRequest(PendingClientLicenseRequest clientLicenseRequest)
        {
            PendingClientLicenseRequests.Add(clientLicenseRequest);
            PendingLicensesChanged?.Invoke(null, null);
        }

        private static void SendRequest(PendingClientLicenseRequest clientLicenseRequest)
        {
            if (clientLicenseRequest != null)
            {
                var clientLicenseRequestAsEncodedXml = clientLicenseRequest.ToEncodedXml();
                var clientEncodedLicenseRequestMessage = new PendingClientLicenseRequestMessage();
                clientEncodedLicenseRequestMessage.Message = clientLicenseRequestAsEncodedXml;

                try
                {
                    var ipAddresses = Dns.GetHostAddresses(UserProfileManager.UserProfile.LicenseServer_ServerName);
                    if (ipAddresses != null && ipAddresses.Length > 0)
                    {
                        foreach (var ipAddress in ipAddresses)
                        {
                            if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                            {
                                DAL.Managers.ConnectionManager.SendAsXML(clientEncodedLicenseRequestMessage, ipAddress, 11006);
                                DAL.Managers.LoggingManager.WriteToLog("License Manager", "Pending License: " + clientLicenseRequest.LicenseType.ToString(), "Send to server: " + ipAddress.ToString());
                            }
                        }
                    }
                }
                catch (Exception exc)
                {
                    DAL.Managers.LoggingManager.WriteToLog("License Manager Error", "Pending License", "Send to server (" + exc.Message + ")");
                }
            }
        }

        public static void Stop()
        {
            if (_licenseRequestPoller != null)
            {
                _licenseRequestPoller.Stop();

                PendingClientLicenseRequests.Clear();
                ActivedLicenses.Clear();
                AvailableLicenses.Clear();

                PendingLicensesChanged?.Invoke(null, null);
                AvailableLicensesChanged?.Invoke(null, null);

                if (!string.IsNullOrEmpty(UserProfileManager.UserProfile.LicenseServer_ServerName))
                {
                    var clientRemoveLicenseRequestAsEncodedXml = new RemoveActiveClientLicenseRequest().ToEncodedXml();
                    var clientEncodedRemoveLicenseRequestMessage = new RemoveActiveClientLicenseRequestMessage();
                    clientEncodedRemoveLicenseRequestMessage.Message = clientRemoveLicenseRequestAsEncodedXml;
                    try
                    {
                        var ipAddresses = Dns.GetHostAddresses(UserProfileManager.UserProfile.LicenseServer_ServerName);
                        if (ipAddresses != null && ipAddresses.Length > 0)
                        {
                            foreach (var ipAddress in ipAddresses)
                            {
                                if (ipAddress.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                                {
                                    DAL.Managers.ConnectionManager.SendAsXML(clientEncodedRemoveLicenseRequestMessage, ipAddress, 11006);
                                    DAL.Managers.LoggingManager.WriteToLog("License Manager", "Remove active licenses from server", "Send to server: " + ipAddress.ToString());
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        DAL.Managers.LoggingManager.WriteToLog("License Manager Error", "Remove active licenses from server", "Send to server (" + exc.Message + ")");
                    }
                }
            }
        }

        public static void ConnectionManager_AtumClientLicenseResponse(PendingClientLicenseResponse clientLicenseResponse)
        {
            lock (PendingClientLicenseRequests)
            {
                foreach (var pendingLicense in PendingClientLicenseRequests)
                {
                    if (pendingLicense.Id == clientLicenseResponse.Id)
                    {
                        if (!ActivedLicenses.Exists(s => s.Id == pendingLicense.Id))
                        {
                            ActivedLicenses.Add(pendingLicense);
                        }
                        
                        PendingClientLicenseRequests.Remove(pendingLicense);
                        DAL.Managers.LoggingManager.WriteToLog("License Manager", "Pending License: " + clientLicenseResponse.LicenseType.ToString(), "Removed");
                        break;
                    }
                }
            }

            lock (AvailableLicenses)
            {
                var currentLicenseFound = false;
                foreach (var availableLicense in AvailableLicenses)
                {
                    if (availableLicense.LicenseType == clientLicenseResponse.LicenseType)
                    {
                        currentLicenseFound = true;
                        availableLicense.ExpirationDate = availableLicense.ExpirationDate.AddHours(4);
                        DAL.Managers.LoggingManager.WriteToLog("License Manager", "License: " + clientLicenseResponse.LicenseType.ToString(), "EndsOn Timestamp updated");

                        PendingLicensesChanged?.Invoke(null, null);
                        break;
                    }
                }

                if (!currentLicenseFound)
                {
                    AvailableLicenses.Add(new AvailableLicense() { Activated = true, ExpirationDate = DateTime.Now.AddDays(1), LicenseType = clientLicenseResponse.LicenseType });
                    DAL.Managers.LoggingManager.WriteToLog("License Manager", "License: " + clientLicenseResponse.LicenseType.ToString(), "Added");

                    AvailableLicensesChanged?.Invoke(null, null);
                }
            }
        }
    }
}
