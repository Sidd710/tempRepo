using Atum.DAL.ApplicationSettings;
using Atum.DAL.Hardware;
using Atum.DAL.Managers;
using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.SplashControl.UnlicensedControl.RecentFiles;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace Atum.Studio.Core.Managers
{
    public static class UserProfileManager
    {

        internal static BindingList<UserProfileInfo> UserProfiles;
        internal static XmlSerializer _serializer;

        public static UserProfileInfo UserProfile
        {
            get
            {
                return UserProfiles[0];
            }
        }

        public static void Start()
        {

            try
            {
                _serializer = new XmlSerializer(typeof(BindingList<UserProfileInfo>));
            UserProfiles = new BindingList<UserProfileInfo>();
            UserProfiles.Add(new UserProfileInfo());

                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.SettingsPath);
                if (hasLocalWriteAccess && File.Exists(Settings.UserProfilesPath))
                {
                    LoggingManager.WriteToLog("User Profile Manager", "Loading profiles", Settings.UserProfilesPath);

                    if (File.Exists(Settings.UserProfilesPath))
                    {
                        using (var streamWriter = new StreamReader(Settings.UserProfilesPath, false))
                        {
                            UserProfiles = (BindingList<UserProfileInfo>)_serializer.Deserialize(streamWriter);
                            streamWriter.Close();
                        }

                    }
                    LoggingManager.WriteToLog("User Profile Manager", "Loaded profiles", Settings.UserProfilesPath);

                }

                //first check if local userprofiles.xml exists and not roaming userprofiles.xml
                else if (File.Exists(Settings.RoamingUserProfilesPath))
                {
                    LoggingManager.WriteToLog("User Profile Manager", "Loading profiles", Settings.RoamingUserProfilesPath);

                    if (File.Exists(Settings.RoamingUserProfilesPath))
                    {
                        using (var streamWriter = new StreamReader(Settings.RoamingUserProfilesPath, false))
                        {
                            UserProfiles = (BindingList<UserProfileInfo>)_serializer.Deserialize(streamWriter);
                            streamWriter.Close();
                        }
                    }

                    LoggingManager.WriteToLog("User Profile Manager", "Loaded profiles", Settings.RoamingUserProfilesPath);
                }

                if (UserProfiles != null)
                {

                    if (UserProfile.SelectedMaterial != null && !string.IsNullOrEmpty(UserProfile.SelectedMaterial.PrinterHardwareType) && MaterialManager.Catalog != null)
                    {
                        var materialFound = false;
                        foreach(var supplier in MaterialManager.Catalog)
                        {
                            foreach(var material in supplier.Materials)
                            {
                                if (material.Name == UserProfile.SelectedMaterial.Name && material.XYResolution == UserProfile.SelectedMaterial.XYResolution && material.PrinterHardwareType == UserProfile.SelectedMaterial.PrinterHardwareType)
                                {
                                    UserProfile.SelectedMaterial.Id = material.Id;
                                    materialFound = true;
                                    break;
                                }
                            }
                        }

                        if (!materialFound)
                        {
                            foreach (var supplier in MaterialManager.Catalog)
                            {
                                var firstAvailableMaterial = supplier.Materials.FirstOrDefault(m => m.XYResolution == UserProfile.SelectedMaterial.XYResolution && m.PrinterHardwareType == UserProfile.SelectedMaterial.PrinterHardwareType);
                                if (firstAvailableMaterial != null)
                                {
                                    UserProfile.SelectedMaterial = firstAvailableMaterial;
                                }
                            }
                        }

                        Save();
                    }
                    else
                    {
                        if (UserProfile.SelectedMaterial != null && UserProfile.SelectedMaterial.Id != Guid.Empty)
                        {
                            //check if material exists
                            var selectedMaterial = MaterialManager.Catalog.FindMaterialById(UserProfile.SelectedMaterial.Id);
                            var invalidMaterial = false;
                            if (selectedMaterial == null)
                            {
                                invalidMaterial = true;
                            }
                            else if (selectedMaterial.PrinterHardwareType != UserProfile.SelectedMaterial.PrinterHardwareType)
                            {
                                invalidMaterial = true;
                            }
                            else if (selectedMaterial.XYResolution != UserProfile.SelectedMaterial.XYResolution)
                            {
                                invalidMaterial = true;
                            }

                            if (invalidMaterial)
                            {
                                foreach (var supplier in MaterialManager.Catalog)
                                {
                                    var firstAvailableMaterial = supplier.Materials.FirstOrDefault(m => m.XYResolution == UserProfile.SelectedMaterial.XYResolution && m.PrinterHardwareType == UserProfile.SelectedMaterial.PrinterHardwareType);
                                    if (firstAvailableMaterial != null)
                                    {
                                        UserProfile.SelectedMaterial = firstAvailableMaterial;
                                    }
                                }
                            }
                            else
                            {
                                UserProfile.SelectedMaterial = selectedMaterial;
                            }
                        }
                    }
                }

            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            CleanupCachedFiles();
        }

        internal static void CleanupCachedFiles()
        {
            Task.Run(() =>
            {
                if (UserProfile.Settings_Studio_CleanOldPrintJobsIndays > 0)
                {
                    int timePeriod = UserProfile.Settings_Studio_CleanOldPrintJobsIndays;

                    //remove autosave path
                    var autosaveDir = Settings.RoamingAutoSavePath;

                    foreach (var file in Directory.EnumerateFiles(autosaveDir))
                    {
                        if ((new FileInfo(file)).LastWriteTime < DateTime.Now.AddDays(-timePeriod))
                        {
                            try
                            {
                                File.Delete(file);
                                LoggingManager.WriteToLog("Cleanup Manager", file, "Removed");
                            }
                            catch
                            {

                            }
                        }
                    }

                    var printjobCachePaths = new List<string> { Settings.RenderEnginePrintjobsPath, Settings.RenderEngineRoamingPrintjobsPath };
                    foreach (var printjobCachePath in printjobCachePaths)
                    {
                        if (Directory.Exists(printjobCachePath))
                        {
                            foreach (var printjobCacheDirectory in Directory.EnumerateDirectories(printjobCachePath))
                            {
                                try
                                {
                                    if ((new DirectoryInfo(printjobCacheDirectory).LastWriteTime < DateTime.Now.AddDays(-timePeriod)))
                                    {
                                        Directory.Delete(printjobCacheDirectory, true);

                                        LoggingManager.WriteToLog("Cleanup Manager", printjobCacheDirectory, "Removed");
                                    }
                                }
                                catch
                                {

                                }
                            }
                        }
                    }
                }
            });
        }
        
        public static void Save()
        {
            //check if write access
            try
            {
                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.SettingsPath);
                if (hasLocalWriteAccess)
                {
                    LoggingManager.WriteToLog("User Profile Manager", "Saving profiles", Settings.UserProfilesPath);

                    //save in base path
                    using (var streamWriter = new StreamWriter(Settings.UserProfilesPath, false))
                    {
                        _serializer.Serialize(streamWriter, UserProfiles);
                        streamWriter.Close();
                    }
                }
                else
                {
                    LoggingManager.WriteToLog("User Profile Manager", "Saving profiles", Settings.RoamingUserProfilesPath);

                    if (!Directory.Exists(Settings.RoamingSettingsPath)) Directory.CreateDirectory(Settings.RoamingSettingsPath);
                    using (var streamWriter = new StreamWriter(Settings.RoamingUserProfilesPath, false))
                    {
                        _serializer.Serialize(streamWriter, UserProfiles);
                        streamWriter.Close();
                    }

                    LoggingManager.WriteToLog("User Profile Manager", "Saved profiles", Settings.RoamingUserProfilesPath);
                }
            }
            catch(Exception exc)
            {
               // MessageBox.Show("UpdateProfile(): " + exc.Message);
            }
        }

    }
    public class UserProfileInfo
    {
        public string LicenseServer_ServerName = "localhost";

        public bool DockingPanel_PrintJobProperties_AutoHide = true;
        public bool DockingPanel_ModelProperties_AutoHide = true;
        public bool DockingPanel_SupportProperties_AutoHide = true;
        public bool DockingPanel_ExplorerProperties_AutoHide = true;
        public bool SelectionOptions_Enable_Annotations = true;
        public bool SelectionOptions_Enable_XYZ_Axis = true;
        public bool Settings_Studio_UseLargeToolbarIcons = false;

        public bool Settings_PrintJob_MirrorObjects = false;
        public bool Settings_Studio_AutoUpdateNotification = false;
        public bool Settings_Studio_OrbitToSelectedModel = true;

        public bool Settings_Skip_Welcome_Screen_On_Next_Start = false;
        public bool Settings_Enable_Touch_Interface_Mode = false;

        public bool Settings_Use_Support_Basement = true;
        public bool Settings_Lift_Model_OnSupport = true;
        public bool Settings_Use_Numeric_Input_For_Positioning;

        public AtumPrinter SelectedPrinter { get; set; }
        public Material SelectedMaterial { get; set; }
        public List<RecentOpenedFile> RecentOpenedFiles { get; set; }

        internal float Settings_Models_ClearanceBetweenClones = 0.5f;

        public int Settings_Studio_SelectionBox_Size = 5;
        public int Settings_Studio_CleanOldPrintJobsIndays = -1;

        public Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox Settings_Studio_SelectionBox_Type = Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox.Circle;

        internal Color Settings_Studio_SelectionBox_Color_AsColor = Color.Blue;

        [XmlElement("Settings_Studio_SelectionBox_Color")]
        public int Settings_Studio_SelectionBox_Color
        {
            get { return Settings_Studio_SelectionBox_Color_AsColor.ToArgb(); }
            set { this.Settings_Studio_SelectionBox_Color_AsColor = Color.FromArgb(value); }
        }

        public int Settings_PrintJob_AntiAlias_Factor = 50;
        public int Settings_PrintJob_AntiAlias_Side = 0;
        public int Settings_PrintJob_FirstSlice_MinAmountOfPixels = 10;

        public float Setting_MAGSAI_MinLowestPointArea_InPixels = 20f;
        public float Setting_MAGSAI_MinOverhangArea_InPixels = 20f;

        public float SupportEngine_Basement_Thickness = 0.2f;
        public float SupportEngine_Penetration_Depth = 0.1f;

        public UserProfileInfo()
        {
            try
            {
            }
            catch
            {

            }
        }

        public void AddRecentOpenedFile(RecentOpenedFile recentOpenedFile)
        {
            if (this.RecentOpenedFiles == null)
            {
                this.RecentOpenedFiles = new List<RecentOpenedFile>();
            }

            //remove duplicates
            var previousRecentFile = RecentOpenedFiles.FirstOrDefault(s => s.FullPath.ToLower() == recentOpenedFile.FullPath.ToLower());
            if (previousRecentFile != null)
            {
                RecentOpenedFiles.Remove(previousRecentFile);
            }

            if (this.RecentOpenedFiles.Count > 9)
            {
                this.RecentOpenedFiles.Remove(this.RecentOpenedFiles.OrderBy(x => x.AccessedDateTime).First());
            }

            this.RecentOpenedFiles.Add(recentOpenedFile);
        }
        public List<RecentOpenedFile> GetRecentOpenedFiles()
        {
            ////Added Dummy Records
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3d1.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3d1.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "NO_NAME.apf", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\NO_NAME.apf" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3DBenchy.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3DBenchy.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3d2.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3d2.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3DBenchy.com.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3DBenchy.com.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3DBenchyas.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3DBenchyas.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3DBenc.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3DBenc.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3DBe.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3DBe.stl" });
            //this.RecentOpenedFiles.Add(new RecentOpenedFile() { FileName = "3d1.stl", AccessedDateTime = DateTime.Now, FullPath = "D:\\dummy\\3D.stl" });

            if (RecentOpenedFiles != null)
            {
                return this.RecentOpenedFiles.OrderByDescending(x => x.AccessedDateTime).Take(10).ToList();
            }
            else
            {
                this.RecentOpenedFiles = new List<RecentOpenedFile>();
                return this.RecentOpenedFiles;
            }
        }

    }
}
