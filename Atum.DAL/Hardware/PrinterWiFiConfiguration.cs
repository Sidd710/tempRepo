using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class PrinterWiFiConfiguration
    {
        public enum typeNetwork
        {
            NoAuthentication= 1,
            WEP = 2,
            WPAPersonal = 7,
            WPA2Personal = 8
        }

        public enum typeEncryption
        {
            WEB = 0,
            AES = 4,
            TKIP = 5,
        }
        
        public string SSID { get; set; }
        public typeNetwork NetworkType { get; set; }
        public typeEncryption EncryptionType { get; set; }
        public string Passphrase { get; set; }
        public int PassphraseIndex { get; set; }
    }
}
