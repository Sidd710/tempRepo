using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Reflection;
using System.Drawing;
using Atum.DAL.DisplaySettings;

namespace Atum.DAL.ApplicationSettings
{
    public class Settings
    {
        public static bool VerboseMode = false;
        public static bool UseOfflineMaterialCatalog = false;
        public static bool UseSupportConeModelIntersectionColor = true;

        public static string BasePath = (new FileInfo(Assembly.GetExecutingAssembly().Location)).DirectoryName; //fody fix

        public static string MaterialsPath = BasePath + "/Materials/";
        public static string ModelsPath = BasePath + "/Models/";
        public static string ModulesPath = BasePath + "/Modules/";
        public static string UpdatePath = BasePath + "/Update/";
        public static string SettingsPath = BasePath + "/Settings/";
        public static string LoggingPath = BasePath + "/Logging/";
        public static string RenderEnginePrintjobsPath = BasePath + "/PrintJobs/";

        public static string DisplayProfilesPath = Path.Combine(SettingsPath, "DisplaySettings.xml");
        public static string PerformanceProfilesPath = Path.Combine(SettingsPath, "PerformanceSettings.xml");
        public static string PrinterProfilesPath = Path.Combine(SettingsPath, "Printers.xml");
        public static string UserProfilesPath = Path.Combine(SettingsPath, "UserProfiles.xml");
        public static string SupportConeProfilesPath = Path.Combine(SettingsPath, "SupportCatalog.xml");

#if LOCTITE
        public static string RoamingBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Loctite", "Operator Station");
#else
        public static string RoamingBasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "atum3D", "Operator Station 3");
#endif

        public static string RoamingSettingsPath = RoamingBasePath + "/Settings/";
        public static string RenderEngineRoamingPrintjobsPath = RoamingBasePath + "/PrintJobs/";
        public static string RoamingMaterialsPath = RoamingBasePath + "/Materials/";
        public static string RoamingModelsPath = RoamingBasePath + "/Models/";
        public static string RoamingModulesPath = RoamingBasePath + "/Modules/";
        public static string RoamingLoggingPath = RoamingBasePath + "/Logging/";
        public static string RoamingUpdatePath = RoamingBasePath + "/Update/";
        public static string RoamingAutoSavePath = RoamingBasePath + "/AutoSave/";
        public static string RoamingDebugPath = RoamingBasePath + "/Debug/";

        public static string RoamingDisplayProfilesPath = Path.Combine(RoamingSettingsPath, "DisplaySettings.xml");
        public static string RoamingPrinterProfilesPath = Path.Combine(RoamingSettingsPath, "Printers.xml");
        public static string RoamingUserProfilesPath = Path.Combine(RoamingSettingsPath, "UserProfiles.xml");
        public static string RoamingPerformanceProfilesPath = Path.Combine(RoamingSettingsPath, "PerformanceSettings.xml");
        public static string RoamingSupportConeProfilesPath = Path.Combine(RoamingSettingsPath, "SupportCatalog.xml");

        public static string LocalLicenseFilePath = Path.Combine(SettingsPath, "Licenses.dat");

        public static Rectangle OpenGLOrientationGizmoDimensions = new Rectangle(15, 50, 150, 150);

        public static List<DpiScalingFactor> DpiScalingFactors = new List<DpiScalingFactor>();


        public static string DALVersion
        {
            get
            {
                return Assembly.GetExecutingAssembly().GetName().Version.ToString();
            }
        }
    }
}
