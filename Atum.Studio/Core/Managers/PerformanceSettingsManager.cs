using Atum.Studio.Core.Platform;
using System.IO;
using Atum.DAL.Managers;

namespace Atum.Studio.Core.Managers
{
    public class PerformanceSettingsManager
    {
        internal static System.Xml.Serialization.XmlSerializer _serializer;
        internal static PerformanceSettings Settings;

        public static void Start()
        {
            _serializer = new System.Xml.Serialization.XmlSerializer(typeof(PerformanceSettings));
            if (File.Exists(DAL.ApplicationSettings.Settings.RoamingPerformanceProfilesPath))
            {
                using (var streamReader = new StreamReader(DAL.ApplicationSettings.Settings.RoamingPerformanceProfilesPath))
                {
                    Settings = (PerformanceSettings)_serializer.Deserialize(streamReader);
                }
            }
            else if (File.Exists(DAL.ApplicationSettings.Settings.PerformanceProfilesPath))
            {
                using (var streamReader = new StreamReader(DAL.ApplicationSettings.Settings.PerformanceProfilesPath))
                {
                    Settings = (PerformanceSettings)_serializer.Deserialize(streamReader);
                }
            }
            else
            {
                Settings = new PerformanceSettings();
                Settings.PrintJobGenerationMultiThreading = true;
                Settings.PrintJobGenerationMaxMemory = 2048;

                //save default settings to XML
                Save();
            }
        }

        internal static void Save()
        {
            var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(DAL.ApplicationSettings.Settings.BasePath);
            if (hasLocalWriteAccess)
            {
                if (!Directory.Exists(DAL.ApplicationSettings.Settings.SettingsPath)) Directory.CreateDirectory(DAL.ApplicationSettings.Settings.SettingsPath);
                using (var streamWriter = new StreamWriter(DAL.ApplicationSettings.Settings.PerformanceProfilesPath))
                {
                    _serializer.Serialize(streamWriter, Settings);
                }
            }
            else
            {
                if (!Directory.Exists(DAL.ApplicationSettings.Settings.RoamingSettingsPath)) Directory.CreateDirectory(DAL.ApplicationSettings.Settings.RoamingSettingsPath);
                using (var streamWriter = new StreamWriter(DAL.ApplicationSettings.Settings.RoamingPerformanceProfilesPath))
                {
                    _serializer.Serialize(streamWriter, Settings);
                }
            }
            
        }
    }
}
