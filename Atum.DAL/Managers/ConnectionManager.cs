using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.ComponentModel;
using System.Net;
using System.Threading;
using Atum.DAL.Managers;
using Atum.DAL.Hardware;
using Atum.DAL.Remoting;
using System.IO;
using System.Diagnostics;
using Atum.DAL.Licenses;

namespace Atum.DAL.Managers
{
    public class ConnectionManager
    {
        private static int _listenPort { get; set; }
        private static BinaryFormatter _binFormatter;
        private static byte[] _byteData;
        private static UdpClient _udpListener;
        private static Socket _tcpListener;
        private static IPEndPoint _remoteEndPoint;
        private static Socket _remoteSocket;

        private static FtpServer _ftpServer;
        private static BackgroundWorker _bwUDPListener;
        private static BackgroundWorker _bwFTPListener;
        private static BackgroundWorker _bwTCPListener;

        public static PrinterFirmware PrinterFirmware { get; set; }

        private static ManualResetEvent connectDone = new ManualResetEvent(false);

        public static Action<IPEndPoint> AtumFirmwareQuery;
        public static Action<PrinterFirmwareResult> AtumFirmwareResults;
        public static Action<AtumPrinter> AtumPrinterReceived;
        public static Action<AtumPrinter> AtumPrinterV15Received;
        public static Action<PrinterAction> AtumPrinterActionReceived;

        //licenses
        public static Action<PendingClientLicenseRequest> AtumClientLicenseRequest;
        public static Action<PendingClientLicenseResponse> AtumClientLicenseResponse;
        public static Action<RemoveActiveClientLicenseRequest> AtumClientRemoveLicenseRequest;

        public static event EventHandler PrintJobReceived;

        public static void Start(int listenPort, bool broadcastListener)
        {
            _listenPort = listenPort;

            _binFormatter = new BinaryFormatter();

            if (broadcastListener)
            {
                _bwUDPListener = new BackgroundWorker();
                _bwUDPListener.DoWork += new DoWorkEventHandler(_bwUDPListener_DoWork);
                _bwUDPListener.RunWorkerAsync();

                //ftp listener
                _bwFTPListener = new BackgroundWorker();
                _bwFTPListener.DoWork += new DoWorkEventHandler(_bwFTPListener_DoWork);
                _bwFTPListener.RunWorkerAsync();
            }

            _bwTCPListener = new BackgroundWorker();
            _bwTCPListener.DoWork += new DoWorkEventHandler(_bwTCPListener_DoWork);
            _bwTCPListener.RunWorkerAsync();

            LoggingManager.WriteToLog("Network", "TCP", "Listener started");

        }

        static void _bwFTPListener_DoWork(object sender, DoWorkEventArgs e)
        {
            _ftpServer = new FtpServer(IPAddress.Any, 11001);
            _ftpServer.Start();
            LoggingManager.WriteToLog("Network", "TCP", "File Listener started");
        }

        public static void Stop()
        {
        }

        static void _bwUDPListener_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                _udpListener = new UdpClient();
                var currentIPAddress = string.Empty;

                _udpListener.ExclusiveAddressUse = false;
                _remoteEndPoint = new IPEndPoint(IPAddress.Any, _listenPort);
                _udpListener.Client.Bind(_remoteEndPoint);

                UdpState state = new UdpState();
                state.RemoteEndPoint = _remoteEndPoint;
                state.UdpListener = _udpListener;

                _udpListener.BeginReceive(new AsyncCallback(OnUDPReceive), state);
            }
            catch (Exception exc)
            {
                Debug.WriteLine("bwUDPListener_DoWork: " + exc.Message);
            }
            LoggingManager.WriteToLog("Network", "Broadcast", "Listener started");
            Debug.WriteLine(_remoteEndPoint.Address.ToString());

        }

        static void OnUDPReceive(IAsyncResult ar)
        {

            try
            {
                UdpClient u = (UdpClient)((UdpState)(ar.AsyncState)).UdpListener;
                IPEndPoint e = (IPEndPoint)((UdpState)(ar.AsyncState)).RemoteEndPoint;

                Byte[] receiveBytes = u.EndReceive(ar, ref e);
                string receiveString = Encoding.ASCII.GetString(receiveBytes);

                switch (receiveString)
                {
                    case "10;":
                        var ms = new System.IO.MemoryStream();
                        AtumPrinter printer = new AtumNULLPrinter();

                        switch (PrinterFirmware.HardwareType)
                        {
                            case "{289EF7C2-3342-4EA3-AF7D-883B26454318}":
                                printer = new Atum.DAL.Hardware.AtumV10TDPrinter();
                                printer.CreateProperties();
                                printer.CreateProjectors();
                                break;
                            case "{967BD0EC-35C1-436A-8D92-145823F17F6E}":
                            case "{57E748B0-EB49-4A59-AEB3-73B2A973D55F}": //AtumV2
                                printer = new Atum.DAL.Hardware.AtumV15Printer();
                                printer.CreateProperties();
                                printer.CreateProjectors();
                                break;
                        }

                        printer.DetectConnection();

                        _binFormatter.Serialize(ms, printer);

                        if (_remoteSocket == null)
                        {
                            _remoteEndPoint = new IPEndPoint(e.Address, _listenPort);
                        }

                        try
                        {
                            _remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                            connectDone = new ManualResetEvent(false);
                            Debug.WriteLine(string.Format("Connecting to: {0}:{1}", e.Address.ToString(), _listenPort));
                            _remoteSocket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _remoteSocket);
                            connectDone.WaitOne();
                            _remoteSocket.BeginSend(ms.ToArray(), 0, ms.ToArray().Length, SocketFlags.None, new AsyncCallback(OnTCPSend), _remoteSocket);
                        }
                        catch (Exception exc)
                        {
                            LoggingManager.WriteToLog("OnUDPReceive", "Exception2", exc.Message);
                        }
                        break;

                }

                UdpState s = new UdpState();
                s.RemoteEndPoint = _remoteEndPoint;
                s.UdpListener = _udpListener;
                u.BeginReceive(OnUDPReceive, s);

            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("OnUDPReceive", "Exception", exc.Message);
            }
        }

        private static void ConnectCallback(IAsyncResult ar)
        {
            // Retrieve the socket from the state Bobject.
            Socket client = (Socket)ar.AsyncState;

            try
            {
                
                // Complete the connection.
                client.EndConnect(ar);

                Debug.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());

                // Signal that the connection has been made.
                connectDone.Set();
            }
            catch(ObjectDisposedException ex)
            {
                Debug.WriteLine("Failed to connect license server: " + ex.Message);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                // Debug.WriteLine(e.ToString());
            }
        }


        public static void OnTCPSend(IAsyncResult ar)
        {
            try
            {
                Socket client;

                client = (Socket)ar.AsyncState;
                client.EndSend(ar);
                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        static void _bwTCPListener_DoWork(object sender, DoWorkEventArgs e)
        {
            try
            {
                var binFormatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                //We are using TCP sockets
                _tcpListener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                //Assign the any IP of the machine and listen on port number 1000
                IPEndPoint ipEndPoint = new IPEndPoint(IPAddress.Any, _listenPort);

                //Bind and listen on the given address  
                _tcpListener.Bind(ipEndPoint);
                _tcpListener.Listen(4);
                _byteData = new byte[_tcpListener.ReceiveBufferSize];

                //Accept the incoming clients
                _tcpListener.BeginAccept(new AsyncCallback(OnAccept), null);
            }
            catch
            {

            }

        }

        private static void OnAccept(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = _tcpListener.EndAccept(ar);

                //Start listening for more clients
                _tcpListener.BeginAccept(new AsyncCallback(OnAccept), null);

                //Once the client connects then start receiving the commands from her
                clientSocket.BeginReceive(_byteData, 0, _byteData.Length, SocketFlags.None,
                    new AsyncCallback(OnTCPReceive), clientSocket);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //   MessageBox.Show(ex.Message, "SGSserverTCP",
                //     MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static void OnTCPReceive(IAsyncResult ar)
        {
            try
            {
                Socket clientSocket = (Socket)ar.AsyncState;
                var remoteIP = new IPEndPoint(((IPEndPoint)clientSocket.RemoteEndPoint).Address, 11000);
                var bytesRead = clientSocket.EndReceive(ar);
                clientSocket.Close();

                var ms = new System.IO.MemoryStream(_byteData, 0, bytesRead);
                ms.Position = 0;
                var sr = new StreamReader(ms);
                var streamAsString = sr.ReadToEnd();
                // streamAsString = streamAsString.Replace("<?", "<").Replace("?>", ">");
                if (streamAsString.StartsWith("<"))
                {
                    var xmlReader = new System.IO.StringReader(streamAsString);

                    if (streamAsString.Contains("<PrinterRemoteControllerQuery"))
                    {

                    }
                    else if (streamAsString.Contains("<PendingClientLicenseRequestMessage"))
                    {
                        var clientLicenseRequestMessage = (PendingClientLicenseRequestMessage.Deserialize(xmlReader));
                        var clientLicenseRequest = clientLicenseRequestMessage.DecodeToXml();
                        clientLicenseRequest.IPAddress = remoteIP.ToString().Split(':')[0];
                        AtumClientLicenseRequest?.Invoke(clientLicenseRequest);
                    }
                    else if (streamAsString.Contains("<PendingClientLicenseResponseMessage"))
                    {
                        var clientLicenseResponseMessage = (PendingClientLicenseResponseMessage.Deserialize(xmlReader));
                        var clientLicenseResponse = clientLicenseResponseMessage.DecodeToXml();
                        AtumClientLicenseResponse?.Invoke(clientLicenseResponse);
                    }
                    else if (streamAsString.Contains("<RemoveActiveClientLicenseRequest"))
                    {
                        var clientRemoveLicenseRequestMessage = (RemoveActiveClientLicenseRequestMessage.Deserialize(xmlReader));
                        var clientRemoveLicenseRequest = clientRemoveLicenseRequestMessage.DecodeToXml();
                        AtumClientRemoveLicenseRequest?.Invoke(clientRemoveLicenseRequest);
                    }
                    //else if (streamAsString.Contains("<ArrayOfRemoteControlAction"))
                    //{
                    //    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(RemoteControlActions));
                    //    var remoteControlActions = (RemoteControlActions)serializer.Deserialize(xmlReader);
                    //    remoteControlActions[0].RemoteHostIP = remoteIP.Address.ToString();
                    //    if (AtumPrinterRemoteControlActionReceived != null) AtumPrinterRemoteControlActionReceived(remoteControlActions[0]);

                    //}

                }
                else
                {
                    ms.Position = 0;
                    var remoteObject = _binFormatter.Deserialize(ms);
                    var remoteObjectType = remoteObject.GetType().FullName;
                    LoggingManager.WriteToLog("TCP Listener", "Received", remoteObjectType);
                    switch (remoteObjectType)
                    {
                        case "Atum.DAL.Hardware.PrinterFirmwareResult":
                            AtumFirmwareResults?.Invoke((PrinterFirmwareResult)remoteObject);
                            break;
                        case "Atum.DAL.Hardware.PrinterFirmware":
                            AtumFirmwareQuery?.Invoke(remoteIP);
                            break;
                        case "Atum.DAL.Hardware.AtumV15Printer":
                            AtumPrinterReceived?.Invoke((AtumPrinter)remoteObject);
                            AtumPrinterV15Received?.Invoke((AtumPrinter)remoteObject);
                            break;
                        case "Atum.DAL.Print.PrintJob":
                            PrintJobReceived?.Invoke(remoteObject, null);
                            break;
                        case "Atum.DAL.Remoting.PrinterAction":
                            AtumPrinterActionReceived?.Invoke((PrinterAction)remoteObject);
                            break;
                        case "Atum.DAL.Hardware.PrinterControllerResult":
                            AtumFirmwareResults?.Invoke((PrinterFirmwareResult)remoteObject);
                            break;
                            //case "Atum.DAL.Remoting.RemoteControlActions":
                            //    var remoteAction = (RemoteControlActions)remoteObject;
                            //    if (remoteAction.Count > 0)
                            //    {
                            //        if (AtumPrinterRemoteControlActionReceived != null) AtumPrinterRemoteControlActionReceived(remoteAction[0]);
                            //    }
                            //    break;
                            //case "Atum.DAL.Remoting.RemoteControlDisplay":
                            //    var remoteDisplay = (RemoteControlDisplay)remoteObject;
                            //    if (AtumPrinterRemoteControlDisplayReceived != null) AtumPrinterRemoteControlDisplayReceived(remoteDisplay);
                            //    break;

                    }
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
        }

        public static void Send(object objectToSend, IPAddress remoteIPAddress, int remotePort)
        {
            var ms = new System.IO.MemoryStream();
            _remoteEndPoint = new IPEndPoint(remoteIPAddress, remotePort);
            if (_binFormatter == null) { _binFormatter = new BinaryFormatter(); }
            _binFormatter.Serialize(ms, objectToSend);
            _remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectDone = new ManualResetEvent(false);
            Debug.WriteLine(string.Format("Connecting to: {0}:{1}", remoteIPAddress.ToString(), remotePort.ToString()));
            _remoteSocket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _remoteSocket);
            connectDone.WaitOne();
            if (_remoteSocket.Connected)
            {
                _remoteSocket.BeginSend(ms.ToArray(), 0, ms.ToArray().Length, SocketFlags.None, new AsyncCallback(OnTCPSend), _remoteSocket);
            }
        }

        public static void SendAsXML(object objectToSend, IPAddress remoteIPAddress, int remotePort)
        {
            var ms = new System.IO.MemoryStream();
            _remoteEndPoint = new IPEndPoint(remoteIPAddress, remotePort);
            var serializer = new System.Xml.Serialization.XmlSerializer(objectToSend.GetType());
            serializer.Serialize(ms, objectToSend);
            _remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //connectDone = new ManualResetEvent(false);
            var result = _remoteSocket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _remoteSocket);

            bool success = result.AsyncWaitHandle.WaitOne(500, true);
            if (success)
            {
                if (_remoteSocket.Connected)
                {
                    _remoteSocket.BeginSend(ms.ToArray(), 0, ms.ToArray().Length, SocketFlags.None, new AsyncCallback(OnTCPSend), _remoteSocket);
                }
            }
            else
            {
                _remoteSocket.Close();
             //   throw new SocketException(10060); // Connection timed out.
            }

           // _remoteSocket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _remoteSocket);
           // connectDone.WaitOne();
           
        }

        public void SendFile(string sourceFilePath, string targetFilePath, int actionTypeId, IPAddress remoteIPAddress, int remotePort)
        {
            var actionTypeIdAsBytes = BitConverter.GetBytes(actionTypeId);
            var fileNameLengthAsBytes = BitConverter.GetBytes(targetFilePath.Length);
            var fileNameAsBytes = Encoding.UTF8.GetBytes(targetFilePath);
            var fileDataAsBytes = System.IO.File.ReadAllBytes(sourceFilePath);

            var bytesToSend = new byte[4 + 4 + 92 + fileDataAsBytes.Length];
            actionTypeIdAsBytes.CopyTo(bytesToSend, 0);
            fileNameLengthAsBytes.CopyTo(bytesToSend, 4);
            fileNameAsBytes.CopyTo(bytesToSend, 8);
            fileDataAsBytes.CopyTo(bytesToSend, 100);

            _remoteEndPoint = new IPEndPoint(remoteIPAddress, remotePort);
            _remoteSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            connectDone = new ManualResetEvent(false);
            Debug.WriteLine(string.Format("Connecting to: {0}:{1}", remoteIPAddress.Address.ToString(), remotePort.ToString()));
            _remoteSocket.BeginConnect(_remoteEndPoint, new AsyncCallback(ConnectCallback), _remoteSocket);
            connectDone.WaitOne();
            _remoteSocket.BeginSend(bytesToSend, 0, bytesToSend.Length, SocketFlags.None, new AsyncCallback(OnTCPSend), new List<object>() { _remoteSocket, actionTypeId });
        }

    }
}