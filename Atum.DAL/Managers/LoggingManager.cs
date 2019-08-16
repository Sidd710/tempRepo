using System.Collections.Generic;
using System;
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;
using Atum.DAL.Logging;

namespace Atum.DAL.Managers
{
    public class LoggingManager
    {
        private static string _loggingPath;
        private static string _loggingFilePath;
        private static Queue<LoggingInfo> _loggingQueue;
        private static Timer _loggingWriterTimer;

        public static void Start()
        {
            _loggingPath = ApplicationSettings.Settings.BasePath;
            var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(_loggingPath);
            if (hasLocalWriteAccess)
            {
                _loggingPath = ApplicationSettings.Settings.LoggingPath;
            }
            else
            {
                _loggingPath = ApplicationSettings.Settings.RoamingLoggingPath;
            }

            if (!Directory.Exists(_loggingPath)) Directory.CreateDirectory(_loggingPath);

            _loggingFilePath = Path.Combine(_loggingPath, "verbose.log");

            try
            {
                if (File.Exists(_loggingFilePath)) {
                    File.Copy(_loggingFilePath, Path.Combine(_loggingPath, "verbose-old.log"), true);
                    File.Delete(_loggingFilePath);
                }
            }
            catch(Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            _loggingQueue = new Queue<LoggingInfo>();

            _loggingWriterTimer = new Timer();
            _loggingWriterTimer.Interval = 1000;
            _loggingWriterTimer.Tick += new EventHandler(_loggingWriterTimer_Tick);

            if (ApplicationSettings.Settings.VerboseMode )
            {
                _loggingWriterTimer.Start();
            }
        }

        static void _loggingWriterTimer_Tick(object sender, EventArgs e)
        {
            _loggingWriterTimer.Stop();
            if (_loggingQueue.Count > 0)
            {
                try
                {
                    if (File.Exists(_loggingPath) && (new FileInfo(_loggingFilePath)).Length > 10000000)
                    {
                        try
                        {
                            File.Copy(_loggingFilePath, _loggingPath + ".bak", true);
                            File.Delete(_loggingFilePath);
                        }
                        catch
                        {
                        }
                    }

                    using (var loggingWriter = new StreamWriter(_loggingFilePath, true))
                    {
                        while (_loggingQueue.Count > 0)
                        {
                            var logLine = _loggingQueue.Dequeue();
                            loggingWriter.WriteLine(String.Format("{0};{1};{2};{3}", logLine.LogTime.ToString(), logLine.Phase, logLine.Property, logLine.Value));
                        }
                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }
            }

            _loggingWriterTimer.Start();
        }

        public static void WriteToLog(string phase, string prop, string value)
        {
            try
            {
                if (phase != null && prop != null && value != null)
                {
                    if (_loggingQueue == null)
                    {
                        _loggingQueue = new Queue<LoggingInfo>();
                    }
                    lock (_loggingQueue)
                    {
                        _loggingQueue.Enqueue(new LoggingInfo(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), phase, prop, value));
                    }
                    Console.WriteLine(string.Format("{0};{1};{2};{3}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), phase, prop, value));
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        public static void WriteToLog(string phase, string prop, Exception exc)
        {
            if (phase != null && prop != null && exc != null)
            {
                _loggingQueue.Enqueue(new LoggingInfo(DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), phase, prop, exc.ToString()));
                Debug.WriteLine(string.Format("{0};{1};{2}", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), phase, prop, exc));
            }
        }


        public static void Stop()
        {

        }
    }
}