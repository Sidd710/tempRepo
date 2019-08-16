using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atum3D.RemoteLogging
{
    public partial class Form1 : Form
    {
        private event EventHandler onPingSuccess;
        private event EventHandler<string> onPingFailed;

        private event EventHandler onConnectionSuccess;
        private event EventHandler<string> onConnectionFailed;

        private string _savePath;

        private SftpClient _client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.onPingSuccess += Form1_onPingSuccess;
            this.onConnectionSuccess += Form1_onConnectionSuccess;
        }

        private void Form1_onConnectionSuccess(object sender, EventArgs e)
        {
            using (var outputFile = new FileStream(Path.Combine(_savePath,"verbose.txt"), FileMode.Create))
            {
                _client.DownloadFile("/home/pi/Atum/Logging/verbose.log", outputFile);
            }

            using (var outputFile = new FileStream(Path.Combine(_savePath,"verbose-old.txt"), FileMode.Create))
            {
                _client.DownloadFile("/home/pi/Atum/Logging/verbose-old.log", outputFile);
            }
        }


        private void Form1_onPingSuccess(object sender, EventArgs e)
{
            var t = new NetworkCredential("", "Loctite2W0rk").SecurePassword;
            var connectionInfo = new ConnectionInfo("192.168.101.170", "pi");

#if LOCTITEEXPERT || LOCTITEDEFAULT
                                        new PasswordAuthenticationMethod("pi", Properties.Resources.),
#else
            //new PasswordAuthenticationMethod("pi", Properties.Resources.AtumSSHpwd.Substring(1, ),
#endif
            new PrivateKeyAuthenticationMethod("rsa.key");
            //    );

            _client = new SftpClient(connectionInfo);

            _client.Connect();

            if (_client.IsConnected)
            {
                onConnectionSuccess?.Invoke(null, null);
            }
            else
            {
                onConnectionFailed?.Invoke(null, null);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (var saveDialog = new FolderBrowserDialog())
            {
                var dialogResult = saveDialog.ShowDialog();
                if (dialogResult == DialogResult.OK)
                {
                    _savePath = saveDialog.SelectedPath;
                }
            }

            //check if printer is on same lan subnet
            var pingTest = new Ping();
            var pingTestReply = pingTest.Send("192.168.101.170");
            if (pingTestReply.Status == IPStatus.Success)
            {
                onPingSuccess?.Invoke(null, null);
            }
            else
            {
                onPingFailed?.Invoke(null, "ping failed");
            }
        }
    }
}
