using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Atum.DAL.Network
{
    public class RemoteConnectionManager
    {
        private static List<string> _connectedEndPoints;

        public static Action<Remoting.RemoteControlAction> RemoteControlActionReceived;

        public static void Start()
        {
            _connectedEndPoints = new List<string>();
            ConnectionManager.AtumPrinterRemoteControlActionReceived += AtumPrinterRemoteControlActionReceived;
        }

        private static void AtumPrinterRemoteControlActionReceived(Remoting.RemoteControlAction remoteControlAction)
        {
            if (!_connectedEndPoints.Contains(remoteControlAction.RemoteHostIP)) _connectedEndPoints.Add(remoteControlAction.RemoteHostIP);
            if (RemoteControlActionReceived != null) { RemoteControlActionReceived(remoteControlAction); }
        }

        public static void UpdateRemoteControlDisplays(int lineIndex, string[] lines)
        {
            var remoteControlDisplay = new Remoting.RemoteControlDisplay(lineIndex, lines);
            if (_connectedEndPoints.Count > 0)
            {
                foreach (var endPoint in _connectedEndPoints)
                {
                    var endPointClone = endPoint;
                    remoteControlDisplay.Send(endPointClone);
                }
            }
        }
    }
}
