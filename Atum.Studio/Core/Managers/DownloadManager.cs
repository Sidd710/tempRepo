using System.IO;
using System.Net;
using System.Text;

namespace Atum.Studio.Core.Managers
{
    internal class DownloadManager
    {
        internal static string DownloadString(string url)
        {
            var result = string.Empty;

            var cookieContainer = new CookieContainer();
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.CookieContainer = cookieContainer;

            using (var responseStream = webRequest.GetResponse().GetResponseStream())
            using (var sr = new StreamReader(responseStream, Encoding.UTF8))
            {
                result = sr.ReadToEnd();
            }

            return result;
        }
    }
}
