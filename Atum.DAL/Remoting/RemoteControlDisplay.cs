using Atum.DAL.Network;
using System;

namespace Atum.DAL.Remoting
{
    [Serializable]
    public class RemoteControlDisplay
    {
        public int LineIndex { get; set; }
        public string[] Lines { get; set; }

        public RemoteControlDisplay()
        {
        }

        public RemoteControlDisplay(int lineIndex, string[] lines)
        {
            this.LineIndex = lineIndex;
            this.Lines = lines;
        }

        public void Send(object ipAddressAsString)
        {
            ConnectionManager.SendAsXML(this, System.Net.IPAddress.Parse((string)ipAddressAsString), 11000);
        }

    }
}
