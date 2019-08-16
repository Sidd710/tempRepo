using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Managers
{
    [Serializable]
    public class NetworkConnection
    {
        public enum typeConnection{
            Unknown =0,
            USB = 1,
            LAN = 2,
        }

        public string IPAddress { get; set; }
        public string SubnetMask { get; set; }
        public string Gateway { get; set; }
        public string MacAddress { get; set; }
        public typeConnection ConnectionType { get; set; }

        public NetworkConnection()
        {
            this.IPAddress = string.Empty;
            this.MacAddress = string.Empty;
            this.SubnetMask = string.Empty;
            this.Gateway = string.Empty;
        }
    }

    [Serializable]
    public class NetworkConnections : List<NetworkConnection> { }
}
