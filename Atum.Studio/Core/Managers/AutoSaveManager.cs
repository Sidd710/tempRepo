using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Core.ModelView;
using System;
using System.Timers;

namespace Atum.Studio.Core.Managers
{
    public class AutoSaveManager
    {
        private static Timer _autosaveTimer;
        public static void Start()
        {
            _autosaveTimer = new Timer();
            _autosaveTimer.Interval = (new TimeSpan(0, 5, 0)).TotalMilliseconds; //5 minutes
            _autosaveTimer.Start();
            _autosaveTimer.Elapsed += _autosaveTimer_Elapsed;
        }

        public static void Stop()
        {
            if (_autosaveTimer != null)
            {
                _autosaveTimer.Stop();
            }
        }

        private static void _autosaveTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _autosaveTimer.Stop();

            if (ObjectView.Objects3D.Count > 1)
            {
                var autosaveDir = DAL.ApplicationSettings.Settings.RoamingAutoSavePath;

                try
                {
                    if (!System.IO.Directory.Exists(autosaveDir))
                    {
                        System.IO.Directory.CreateDirectory(autosaveDir);
                    }

                    var autosaveFilePath = System.IO.Path.Combine(autosaveDir, SceneControlToolbarManager.PrintjobName + ".apf");
                    Engines.ExportEngine.ExportCurrentWorkspace(true, autosaveFilePath, magsAIComments: null);

                    //remove all files that are older then 1 day
                    foreach(var file in System.IO.Directory.EnumerateFiles(autosaveDir))
                    {
                        if ((new System.IO.FileInfo(file)).LastWriteTime < DateTime.Now.AddDays(-1))
                        {
                            System.IO.File.Delete(file);
                        }
                    }
                }
                catch(Exception exc)
                {

                }
            }

            _autosaveTimer.Start();
        }
    }
}
