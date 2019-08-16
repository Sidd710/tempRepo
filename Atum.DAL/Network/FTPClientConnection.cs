using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using Atum.DAL.Managers;
using System.Diagnostics;

namespace Atum.DAL.Managers
{
    public class FTPClientConnection : IDisposable
    {
        private class DataConnectionOperation
        {
            public string Arguments { get; set; }
        }

        private class DataConnectionStoreOperation : DataConnectionOperation
        {

        }

        private class DataConnectionListOperation : DataConnectionOperation
        {

        }


        #region Copy Stream Implementations

        private static long CopyStream(Stream input, Stream output, int bufferSize)
        {
            byte[] buffer = new byte[bufferSize];
            int count = 0;
            long total = 0;

            while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, count);
                total += count;
            }

            return total;
        }

        private static long CopyStreamAscii(Stream input, Stream output, int bufferSize)
        {
            char[] buffer = new char[bufferSize];
            int count = 0;
            long total = 0;

            using (StreamReader rdr = new StreamReader(input, Encoding.ASCII))
            {
                using (StreamWriter wtr = new StreamWriter(output, Encoding.ASCII))
                {
                    while ((count = rdr.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        wtr.Write(buffer, 0, count);
                        total += count;
                    }
                }
            }

            return total;
        }

        private long CopyStream(Stream input, Stream output)
        {
            Stream limitedStream = output; // new RateLimitingStream(output, 131072, 0.5);

            if (_connectionType == TransferType.Image)
            {
                return CopyStream(input, limitedStream, 4096);
            }
            else
            {
                return CopyStreamAscii(input, limitedStream, 4096);
            }
        }

        #endregion

        #region Enums

        private enum TransferType
        {
            Ascii,
            Ebcdic,
            Image,
            Local,
        }

        private enum DataConnectionType
        {
            Passive,
            Active,
        }

        #endregion

        private bool _disposed = false;

        private TcpListener _passiveListener;

        private TcpClient _controlClient;
        private TcpClient _dataClient;

        private NetworkStream _controlStream;
        private StreamReader _controlReader;
        private StreamWriter _controlWriter;

        private TransferType _connectionType = TransferType.Ascii;
        private DataConnectionType _dataConnectionType = DataConnectionType.Active;

        private string _username;
        private string _currentDirectory;
        private IPEndPoint _dataEndpoint;
        private IPEndPoint _remoteEndPoint;

        private string _clientIP;

        private FTPUser _currentUser;

        private List<string> _validCommands;

        public FTPClientConnection(TcpClient client)
        {
            _controlClient = client;

            _validCommands = new List<string>();
        }

        private string CheckUser()
        {
            if (_currentUser == null)
            {
                return "530 Not logged in";
            }

            return null;
        }

        public void HandleClient(object obj)
        {
            _remoteEndPoint = (IPEndPoint)_controlClient.Client.RemoteEndPoint;

            _clientIP = _remoteEndPoint.Address.ToString();

            _controlStream = _controlClient.GetStream();

            _controlReader = new StreamReader(_controlStream);
            _controlWriter = new StreamWriter(_controlStream);

            _controlWriter.WriteLine("220 Service Ready.");
            _controlWriter.Flush();

            _validCommands.AddRange(new string[] { "AUTH", "USER", "PASS", "QUIT", "HELP", "NOOP" });

            string line;

            _dataClient = new TcpClient();

            try
            {
                while ((line = _controlReader.ReadLine()) != null)
                {
                    string response = null;

                    string[] command = line.Split(' ');

                    string cmd = command[0].ToUpperInvariant();
                    string arguments = command.Length > 1 ? line.Substring(command[0].Length + 1) : null;

                    if (arguments != null && arguments.Trim().Length == 0)
                    {
                        arguments = null;
                    }

                    LoggingManager.WriteToLog("File connection", _clientIP, arguments);

                    if (!_validCommands.Contains(cmd))
                    {
                        response = CheckUser();
                    }

                    if (response == null)
                    {
                        switch (cmd)
                        {
                            case "USER":
                                response = User(arguments);
                                break;
                            case "PASS":
                                response = Password(arguments);
                                break;
                            case "CWD":
                                response = ChangeWorkingDirectory(arguments);
                                break;
                            case "CDUP":
                                response = ChangeWorkingDirectory("..");
                                break;
                            case "QUIT":
                                response = "221 Service closing control connection";
                                break;
                            case "REIN":
                                _currentUser = null;
                                _username = null;
                                _passiveListener = null;
                                _dataClient = null;

                                response = "220 Service ready for new user";
                                break;
                            case "PASV":
                                response = Passive();
                                break;
                            case "PORT":
                                response = Port(arguments);
                                // logEntry.CPort = _dataEndpoint.Port.ToString();
                                break;
                            case "TYPE":
                                response = Type(command[1], command.Length == 3 ? command[2] : null);
                                break;
                            case "MKD":
                                response = CreateDir(arguments);
                                break;
                            case "PWD":
                                response = PrintWorkingDirectory();
                                break;
                            case "STOR":
                                response = Store(arguments);
                                break;
                            case "LIST":
                                response = List(arguments ?? _currentDirectory);
                                //    logEntry.Date = DateTime.Now;
                                break;
                            // Extensions defined by rfc 3659
                            case "EPSV":
                                response = EPassive();
                                break;

                            default:
                                response = "502 Command not implemented";
                                break;
                        }
                    }

                    LoggingManager.WriteToLog("File connection", _clientIP + "(" + this._username + ")", cmd);

                    if (_controlClient == null || !_controlClient.Connected)
                    {
                        break;
                    }
                    else
                    {
                        _controlWriter.WriteLine(response);
                        _controlWriter.Flush();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }

            Dispose();
        }

        private string Port(string hostPort)
        {
            _dataConnectionType = DataConnectionType.Active;

            string[] ipAndPort = hostPort.Split(',');

            byte[] ipAddress = new byte[4];
            byte[] port = new byte[2];

            for (int i = 0; i < 4; i++)
            {
                ipAddress[i] = Convert.ToByte(ipAndPort[i]);
            }

            for (int i = 4; i < 6; i++)
            {
                port[i - 4] = Convert.ToByte(ipAndPort[i]);
            }

            if (BitConverter.IsLittleEndian)
                Array.Reverse(port);

            short portNumber = BitConverter.ToInt16(port, 0);
            portNumber = (short)(portNumber < 0 ? -portNumber : portNumber);
            _dataEndpoint = new IPEndPoint(new IPAddress(ipAddress), portNumber);

            return "200 Data Connection Established";
        }



        private string NormalizeFilename(string path)
        {
            if (path == null)
            {
                path = string.Empty;
            }


            path = new FileInfo(Path.Combine(_currentDirectory, path)).FullName;

            return path;
        }

        #region FTP Commands


        private string User(string username)
        {
            _username = username;

            return "331 Username ok, need password";
        }

        private string Password(string password)
        {
            //if (this._username == "printqueue" && password == "printqueue")
            //{
            //    this._currentUser = new FTPUser();
            //    this._currentUser.Username = this._username;
            //    this._currentUser.Password = password;
            //    this._currentUser.HomeDir = ApplicationSettings.Settings.RPIPrintQueuePath;
            //    this._currentDirectory = this._currentUser.HomeDir;
            //}

            //else if (this._username == "atumupdate" && password == "@tum.Update")
            //{
            //    this._currentUser = new FTPUser();
            //    this._currentUser.Username = this._username;
            //    this._currentUser.Password = password;
            //    this._currentUser.HomeDir = ApplicationSettings.Settings.RPIUpdatePath;
            //    this._currentDirectory = this._currentUser.HomeDir;
            //}

            if (_currentUser != null)
            {
                return "230 User logged in";
            }
            else
            {
                return "530 Not logged in";
            }
        }

        private string ChangeWorkingDirectory(string pathname)
        {

            string newDir;

            pathname = pathname.Replace('/', '\\');
            newDir = Path.Combine(_currentDirectory, pathname);

            _currentDirectory = new DirectoryInfo(newDir).FullName;

            return "250 Changed to new directory";
        }

        private string Passive()
        {
            _dataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)_controlClient.Client.LocalEndPoint).Address;

            _passiveListener = new TcpListener(localIp, 0);
            _passiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)_passiveListener.LocalEndpoint;

            byte[] address = passiveListenerEndpoint.Address.GetAddressBytes();
            short port = (short)passiveListenerEndpoint.Port;

            byte[] portArray = BitConverter.GetBytes(port);

            if (BitConverter.IsLittleEndian)
                Array.Reverse(portArray);

            return string.Format("227 Entering Passive Mode ({0},{1},{2},{3},{4},{5})", address[0], address[1], address[2], address[3], portArray[0], portArray[1]);
        }

        private string EPassive()
        {
            _dataConnectionType = DataConnectionType.Passive;

            IPAddress localIp = ((IPEndPoint)_controlClient.Client.LocalEndPoint).Address;

            _passiveListener = new TcpListener(localIp, 0);
            _passiveListener.Start();

            IPEndPoint passiveListenerEndpoint = (IPEndPoint)_passiveListener.LocalEndpoint;

            return string.Format("229 Entering Extended Passive Mode (|||{0}|)", passiveListenerEndpoint.Port);
        }

        private string Type(string typeCode, string formatControl)
        {
            switch (typeCode.ToUpperInvariant())
            {
                case "A":
                    _connectionType = TransferType.Ascii;
                    break;
                case "I":
                    _connectionType = TransferType.Image;
                    break;
                default:
                    return "504 Command not implemented for that parameter";
            }

            return string.Format("200 Type set to N");
        }

        private string CreateDir(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                if (!Directory.Exists(pathname))
                {
                    Directory.CreateDirectory(pathname);
                }
                else
                {
                    return "550 Directory already exists";
                }

                return "250 Requested file action okay, completed";
            }

            return "550 Directory Not Found";
        }

        private string Store(string pathname)
        {
            var filePath = System.IO.Path.Combine(this._currentUser.HomeDir, pathname.Replace("~", "/"));
            var directoryPath = (new FileInfo(filePath)).Directory.FullName;
            if (!System.IO.Directory.Exists(directoryPath)) System.IO.Directory.CreateDirectory(directoryPath);

            if (pathname != null)
            {
                var state = new DataConnectionStoreOperation { Arguments = filePath };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for STOR", _dataConnectionType);
            }

            return "450 Requested file action not taken";
        }


        private string PrintWorkingDirectory()
        {
            string current = _currentDirectory.Replace('\\', '/');

            if (current.Length == 0)
            {
                current = "/";
            }

            return string.Format("257 \"{0}\" is current directory.", current); ;
        }

        private string List(string pathname)
        {
            pathname = NormalizeFilename(pathname);

            if (pathname != null)
            {
                var state = new DataConnectionListOperation { Arguments = pathname };

                SetupDataConnectionOperation(state);

                return string.Format("150 Opening {0} mode data transfer for LIST", _dataConnectionType);
            }

            return "450 Requested file action not taken";
        }


        private string Mode(string mode)
        {
            if (mode.ToUpperInvariant() == "S")
            {
                return "200 OK";
            }
            else
            {
                return "504 Command not implemented for that parameter";
            }
        }

        #endregion

        #region DataConnection Operations

        private void HandleAsyncResult(IAsyncResult result)
        {
            if (this._dataConnectionType == DataConnectionType.Active)
            {
                this._dataClient.EndConnect(result);
            }
            else
            {
                this._dataClient = _passiveListener.EndAcceptTcpClient(result);
            }
        }

        private void SetupDataConnectionOperation(DataConnectionOperation state)
        {
            if (_dataConnectionType == DataConnectionType.Active)
            {
                this._dataClient = new TcpClient(_dataEndpoint.AddressFamily);
                this._dataClient.BeginConnect(_dataEndpoint.Address, _dataEndpoint.Port, DoDataConnectionOperation, state);
            }
            else
            {
                this._passiveListener.BeginAcceptTcpClient(DoDataConnectionOperation, state);
            }
        }

        private void DoDataConnectionOperation(IAsyncResult result)
        {
            HandleAsyncResult(result);

            if (result.AsyncState is DataConnectionStoreOperation)
            {
                DataConnectionStoreOperation op = result.AsyncState as DataConnectionStoreOperation;

                try
                {
                    if (File.Exists(op.Arguments))
                    {
                        File.Delete(op.Arguments);
                    }

                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }

                string response = string.Empty;

                using (NetworkStream dataStream = _dataClient.GetStream())
                {
                    long bytes = 0;


                    using (FileStream fs = new FileStream(op.Arguments, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
                    {
                        bytes = CopyStream(dataStream, fs);
                    }

                    LoggingManager.WriteToLog("File connection", _clientIP + "(" + this._currentUser.Username + ")", "STOR");

                    response = "226 Closing data connection, file transfer successful";
                }

                this._dataClient.Close();
                this._dataClient = null;

                this._controlWriter.WriteLine(response);
                this._controlWriter.Flush();
            }
            else if (result.AsyncState is DataConnectionListOperation)
            {
                DataConnectionListOperation op = result.AsyncState as DataConnectionListOperation;

                string response;

                using (NetworkStream dataStream = _dataClient.GetStream())
                {
                    response = ListOperation(dataStream, op.Arguments);
                    //response = op.Operation(dataStream, op.Arguments);
                }

                _dataClient.Close();
                _dataClient = null;

                _controlWriter.WriteLine(response);
                _controlWriter.Flush();
            }


        }

        private string AppendOperation(NetworkStream dataStream, string pathname)
        {
            long bytes = 0;

            using (FileStream fs = new FileStream(pathname, FileMode.Append, FileAccess.Write, FileShare.None, 4096, FileOptions.SequentialScan))
            {
                bytes = CopyStream(dataStream, fs);
            }

            LoggingManager.WriteToLog("File connection", _clientIP + "(" + this._currentUser.Username + ")", "APPE");

            return "226 Closing data connection, file transfer successful";
        }

        private string ListOperation(NetworkStream dataStream, string pathname)
        {
            StreamWriter dataWriter = new StreamWriter(dataStream, Encoding.ASCII);

            foreach (string dirPath in Directory.GetDirectories(pathname))
            {
                DirectoryInfo d = new DirectoryInfo(dirPath);

                string date = d.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    d.LastWriteTime.ToString("MMM dd  yyyy") :
                    d.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("drwxr-xr-x    2 2003     2003     {0,8} {1} {2}", "4096", date, d.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            foreach (string filePath in Directory.GetFiles(pathname))
            {
                FileInfo f = new FileInfo(filePath);

                string date = f.LastWriteTime < DateTime.Now - TimeSpan.FromDays(180) ?
                    f.LastWriteTime.ToString("MMM dd  yyyy") :
                    f.LastWriteTime.ToString("MMM dd HH:mm");

                string line = string.Format("-rw-r--r--    2 2003     2003     {0,8} {1} {2}", f.Length, date, f.Name);

                dataWriter.WriteLine(line);
                dataWriter.Flush();
            }

            LoggingManager.WriteToLog("File connection", _clientIP + "(" + this._currentUser.Username + ")", "LIST");

            return "226 Transfer complete";
        }

        #endregion

        #region IDisposable

        public void Dispose()
        {
            Dispose(true);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    if (_controlClient != null)
                    {
                        _controlClient.Close();
                    }

                    if (_dataClient != null)
                    {
                        _dataClient.Close();
                    }

                    if (_controlStream != null)
                    {
                        _controlStream.Close();
                    }

                    if (_controlReader != null)
                    {
                        _controlReader.Close();
                    }

                    if (_controlWriter != null)
                    {
                        _controlWriter.Close();
                    }
                }
            }

            _disposed = true;
        }

        #endregion
    }
}
