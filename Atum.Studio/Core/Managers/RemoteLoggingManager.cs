using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using DAL.Logging;
using System.IO;

namespace Atum.Studio.Core.Managers
{
    public class RemoteLoggingManager
    {
        private static string _remoteIP;
        private static Timer _tmrReadLogging;
        public static long LastStreamOffset;

        public static System.ComponentModel.BindingList<LoggingInfo> Lines;

        public static void ReadRemoteLog(string remoteIP)
        {
            _remoteIP = remoteIP;
            LastStreamOffset = 0;
            Lines = new System.ComponentModel.BindingList<LoggingInfo>();

            _tmrReadLogging = new Timer();
            _tmrReadLogging.Interval = 100;
            _tmrReadLogging.Tick += new EventHandler(_tmrReadLogging_Tick);
            _tmrReadLogging.Start();
        }

        static void _tmrReadLogging_Tick(object sender, EventArgs e)
        {
            try
            {

                using (var streamReader = new System.IO.StreamReader(string.Format(@"\\{0}\logging$\verbose.log", _remoteIP)))
                {
                    if (streamReader.BaseStream.Length < LastStreamOffset) { LastStreamOffset = 0; }
                    streamReader.BaseStream.Seek(LastStreamOffset, SeekOrigin.Begin);

                    //read out of the file until the EOF
                    string line = "";
                    while ((line = streamReader.ReadLine()) != null)
                    {
                        var logLine = line.Split(';');
                        var logging = new LoggingInfo(logLine[0], logLine[1], logLine[2], logLine[3]);

                        Lines.Add(logging);

                    }

                    //update the last max offset
                    LastStreamOffset = streamReader.BaseStream.Position;
                }
            }
            catch (Exception exc)
            {

            }

        }
    }
}
