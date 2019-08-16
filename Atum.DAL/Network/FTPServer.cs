using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;

namespace Atum.DAL.Managers
{
        public class FtpServer : IDisposable
        {

            private bool _disposed = false;
            private bool _listening = false;

            private TcpListener _listener;
            private List<FTPClientConnection> _activeConnections;

            private IPEndPoint _localEndPoint;

            public FtpServer()
                : this(IPAddress.Any, 11001)
            {
            }

            public FtpServer(IPAddress ipAddress, int port)
            {
                _localEndPoint = new IPEndPoint(ipAddress, port);
            }

            public void Start()
            {
                _listener = new TcpListener(_localEndPoint);

                _listening = true;
                _listener.Start();

                _activeConnections = new List<FTPClientConnection>();

                _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);
            }

            public void Stop()
            {

                _listening = false;
                _listener.Stop();

                _listener = null;
            }

            private void HandleAcceptTcpClient(IAsyncResult result)
            {
                if (_listening)
                {
                    _listener.BeginAcceptTcpClient(HandleAcceptTcpClient, _listener);

                    TcpClient client = _listener.EndAcceptTcpClient(result);

                    FTPClientConnection connection = new FTPClientConnection(client);

                    _activeConnections.Add(connection);

                    ThreadPool.QueueUserWorkItem(connection.HandleClient, client);
                }
            }

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
                        Stop();

                        foreach (FTPClientConnection conn in _activeConnections)
                        {
                            conn.Dispose();
                        }
                    }
                }

                _disposed = true;
            }
        }
    }

