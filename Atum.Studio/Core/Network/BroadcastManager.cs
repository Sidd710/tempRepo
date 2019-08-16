using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Diagnostics;

namespace Atum.Studio.Core.Network
{
    public class BroadcastManager
    {
        public enum typeBroadcastAction
        {
            GetPrinter = 10
        }

        public static void SendAsBroadcast(typeBroadcastAction action, byte[] data)
        {
            var udp = new UdpClient();
            foreach (var nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {

                if ((nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Up || nic.OperationalStatus == System.Net.NetworkInformation.OperationalStatus.Unknown) && (nic.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Ethernet || nic.NetworkInterfaceType == System.Net.NetworkInformation.NetworkInterfaceType.Wireless80211))
                {
                    foreach (var ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        try
                        {
                            IPEndPoint groupEP = new IPEndPoint(IPAddress.Parse(GetBroadcastAddress(ip.Address.ToString(), ip.IPv4Mask.ToString())), 11000);
                            var actionAsBytes = Encoding.ASCII.GetBytes(((int)action).ToString());
                            var separatorAsBytes = Encoding.ASCII.GetBytes(";");
                            var sendBuffer = new Byte[actionAsBytes.Length + separatorAsBytes.Length + data.Length];

                            var sendBufferIndex = 0;

                            foreach (var actionByte in actionAsBytes)
                            {
                                sendBuffer[sendBufferIndex] = actionByte;
                                sendBufferIndex++;
                            }

                            foreach (var separatorByte in separatorAsBytes)
                            {
                                sendBuffer[sendBufferIndex] = separatorByte;
                                sendBufferIndex++;
                            }

                            foreach (var dataByte in data)
                            {
                                sendBuffer[sendBufferIndex] = dataByte;
                                sendBufferIndex++;
                            }

                            udp.Send(sendBuffer, sendBuffer.Length, groupEP);
                        }
                        catch (Exception exc)
                        {
                            Debug.WriteLine(exc.Message);
                        }
                    }
                }

            }

        }

        private static string GetBroadcastAddress(string ipAddress, string subnetMask)
        {
            //determines a broadcast address from an ip and subnet
            var ip = IPAddress.Parse(ipAddress);
            var mask = IPAddress.Parse(subnetMask);

            byte[] ipAdressBytes = ip.GetAddressBytes();
            byte[] subnetMaskBytes = mask.GetAddressBytes();

            if (ipAdressBytes.Length != subnetMaskBytes.Length)
                throw new ArgumentException("Lengths of IP address and subnet mask do not match.");

            byte[] broadcastAddress = new byte[ipAdressBytes.Length];
            for (int i = 0; i < broadcastAddress.Length; i++)
            {
                broadcastAddress[i] = (byte)(ipAdressBytes[i] | (subnetMaskBytes[i] ^ 255));
            }
            return new IPAddress(broadcastAddress).ToString();
        }
    }
}
