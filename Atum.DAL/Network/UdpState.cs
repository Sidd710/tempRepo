using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Atum.DAL.Managers
{
    class UdpState
    {
        public IPEndPoint RemoteEndPoint { get; set; }
        public UdpClient UdpListener { get; set; }
    }
}
