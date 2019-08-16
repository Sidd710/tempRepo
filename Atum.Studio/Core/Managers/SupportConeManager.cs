using Atum.DAL.ApplicationSettings;
using Atum.DAL.Catalogs;
using Atum.DAL.Managers;
using Atum.Studio.Core.Models;
using OpenTK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    public class SupportManager
    {
        private static SupportCatalog _supportCatalog;

        public static void Start()
        {
            //bind manufacturer comboxbox
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SupportCatalog));
            _supportCatalog = new SupportCatalog();

            //if no material catalog is found then save default items
            try
            {
                if (File.Exists(Settings.RoamingSupportConeProfilesPath))
                {
                    using (var streamReader = new StreamReader(Settings.RoamingSupportConeProfilesPath))
                    {
                        try
                        {
                            _supportCatalog = (SupportCatalog)serializer.Deserialize(streamReader);
                        }
                        catch
                        {
                        }
                    }
                }
                else if (File.Exists(Settings.SupportConeProfilesPath))
                {
                    using (var streamReader = new StreamReader(Settings.SupportConeProfilesPath))
                    {
                        try
                        {
                            _supportCatalog = (SupportCatalog)serializer.Deserialize(streamReader);
                        }
                        catch
                        {
                        }
                    }
                }
                else
                {
                    _supportCatalog.Add(new SupportCatalogItem()
                    {
                        TopHeight = 1,
                        TopRadius = 0.35f,
                        MiddleRadius = 1f,
                        BottomRadius = 6f,
                        BottomHeight = 1f,
                        IsDefault = true
                    });
                    Save();
                }
                
            }
            catch(Exception exc) {
                MessageBox.Show(exc.StackTrace);
            }

        }

        public static void Save()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(SupportCatalog));
            //if no material catalog is found then save default items
            try
            {
                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.BasePath);

                if (hasLocalWriteAccess)
                {
                    if (!Directory.Exists(Settings.SettingsPath)) Directory.CreateDirectory(Settings.SettingsPath);

                    using (var streamWriter = new StreamWriter(Settings.SupportConeProfilesPath))
                    {
                        try
                        {
                            serializer.Serialize(streamWriter, _supportCatalog);
                        }
                        catch
                        {
                        }
                    }

                    if (!File.Exists(Settings.RoamingSupportConeProfilesPath))
                    {
                        using (var streamWriter = new StreamWriter(Settings.RoamingSupportConeProfilesPath))
                        {
                            try
                            {
                                serializer.Serialize(streamWriter, _supportCatalog);
                            }
                            catch
                            {
                            }
                        }
                    }

                }
                else
                {

                    if (!Directory.Exists(Settings.RoamingSettingsPath)) Directory.CreateDirectory(Settings.RoamingSettingsPath);

                    using (var streamWriter = new StreamWriter(Settings.RoamingSupportConeProfilesPath))
                    {
                        try
                        {
                            serializer.Serialize(streamWriter, _supportCatalog);
                        }
                        catch
                        {
                        }
                    }
                }

            }
            catch { }
        }

        public static SupportCatalogItem DefaultSupportSettings
        {
            get
            {
                foreach (var supportItem in _supportCatalog)
                {
                    if (supportItem.IsDefault)
                    {
                        return supportItem;
                    }
                }

                return null;
            }
            set
            {
                if (_supportCatalog.Count == 0)
                {
                    _supportCatalog.Add(value);
                }
                else
                {
                    _supportCatalog[0] = value;
                }
            }
        }
    }
}
