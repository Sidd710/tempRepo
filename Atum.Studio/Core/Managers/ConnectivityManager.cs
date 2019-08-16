using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Configuration;
using System.Reflection;
using System.Text;
using System.Threading;

namespace Atum.Studio.Core.Managers
{

    public static class ConnectivityManager
    {
        public static event Action<bool> InternetConnected;

        public static bool InternetAvailable { get; set; }

        public static void Start()
        {
            ThreadPool.QueueUserWorkItem(CheckInternetConnectivity);
        }

        static void CheckInternetConnectivity(object state)
        {
            if (System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable())
            {
                ToggleAllowUnsafeHeaderParsing(true);
                using (WebClient webClient = new WebClient())
                {
                    webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.BypassCache);
                    webClient.Proxy = null;
                    
                    webClient.OpenReadCompleted += webClient_OpenReadCompleted;
                    webClient.OpenReadAsync(new Uri("http://www.google.com"));
                }
            }
        }

        public static bool ToggleAllowUnsafeHeaderParsing(bool enable)
        {
            //Get the assembly that contains the internal class
            Assembly assembly = Assembly.GetAssembly(typeof(SettingsSection));
            if (assembly != null)
            {
                //Use the assembly in order to get the internal type for the internal class
                Type settingsSectionType = assembly.GetType("System.Net.Configuration.SettingsSectionInternal");
                if (settingsSectionType != null)
                {
                    //Use the internal static property to get an instance of the internal settings class.
                    //If the static instance isn't created already invoking the property will create it for us.
                    object anInstance = settingsSectionType.InvokeMember("Section",
                    BindingFlags.Static | BindingFlags.GetProperty | BindingFlags.NonPublic, null, null, new object[] { });
                    if (anInstance != null)
                    {
                        //Locate the private bool field that tells the framework if unsafe header parsing is allowed
                        FieldInfo aUseUnsafeHeaderParsing = settingsSectionType.GetField("useUnsafeHeaderParsing", BindingFlags.NonPublic | BindingFlags.Instance);
                        if (aUseUnsafeHeaderParsing != null)
                        {
                            aUseUnsafeHeaderParsing.SetValue(anInstance, enable);
                            return true;
                        }

                    }
                }
            }
            return false;
        }


        static void webClient_OpenReadCompleted(object sender, OpenReadCompletedEventArgs e)
        {
            if (e.Error == null)
            {
                InternetAvailable = true;

                InternetConnected?.Invoke(true);
            }
            else
            {
               // System.Windows.Forms.MessageBox.Show(e.Error.Message);
            }
                

        }
    }
}
