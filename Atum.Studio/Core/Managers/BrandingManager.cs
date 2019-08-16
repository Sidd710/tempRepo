using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Resources;
using System.Text;

namespace Atum.Studio.Core.Managers
{
    public class BrandingManager
    {
        internal static string ApplicationVersion
        {
            get
            {
                System.Version entryAssemblyVersion = Assembly.GetEntryAssembly().GetName().Version;
                return string.Format("{0}.{1}.{2}.{3}", entryAssemblyVersion.Major, entryAssemblyVersion.Minor, entryAssemblyVersion.Build, entryAssemblyVersion.Revision);
            }
        }

        internal static string MainForm_Title
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("MainForm_Title");
            }
        }

        internal static string LicenseManager_EmailAddress
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("LicenseManager_EmailAddress");
            }
        }

        internal static string MainForm_StudioName
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("MainForm_StudioName");
            }
        }

        public static Icon MainForm_Icon
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return (Icon)resourceManager.GetObject("MainForm_Icon");
            }
        }

        internal static Image Checkbox_Checked
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
                return (Image)resourceManager.GetObject("checkbox_checked_Henkel");
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
                return (Image)resourceManager.GetObject("checkbox_checked_atum3D");
#endif
            }
        }

        internal static Image About_Logo
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return (Image)resourceManager.GetObject("About_Logo");
            }
        }

        public static string CLI_Notification_Title
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("CLI_Notification_Title");
            }
        }
        internal static Color Button_HighlightColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_HighlightColor"));
            }
        }

        internal static Color Button_ForeColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_ForeColor"));
            }
        }

        internal static Color Button_BackgroundColor_Disabled
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_BackgroundColor_Disabled"));
            }
        }

        internal static Color Button_ForeColor_Dark
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_ForeColor_Dark"));
            }
        }
        internal static Color Button_BackColor_LightGray
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_BackColor_LightGray"));
            }
        }
        internal static Color Button_BackgroundColor_Dark
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_BackgroundColor_Dark"));
            }
        }

        internal static Color Button_BackgroundColor_LightDark
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Button_BackgroundColor_LightDark"));
            }
        }

        internal static Color Menu_BackgroundColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Menu_BackgroundColor"));
            }
        }

        internal static Color Menu_Item_HighlightColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Menu_Item_HighlightColor"));
            }
        }
        internal static Color Menu_Item_ForeColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Menu_Item_ForeColor"));
            }
        }
        internal static Color ContextMenu_Item_ForeColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("ContextMenu_Item_ForeColor"));
            }
        }
        internal static Color ContextMenu_BackgroundColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("ContextMenu_BackgroundColor"));
            }
        }

        internal static Color ContextMenu_Item_HighlightColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("ContextMenu_Item_HighlightColor"));
            }
        }
        internal static Color ContextMenu_Item_HighlightForeColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("ContextMenu_Item_HighlightForeColor"));
            }
        }


        internal static Color Textbox_HighlightColor_Error
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Textbox_HighlightColor_Error"));
            }
        }

        internal static Color Form_Header_BackgroundColor
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return System.Drawing.ColorTranslator.FromHtml(resourceManager.GetString("Form_Header_BackgroundColor"));
            }
        }

        internal static Image Printer_Calibration_Image
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
                return (Image)resourceManager.GetObject("printer_calibration_henkel");
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
                return (Image)resourceManager.GetObject("printer_calibration_atum3D");
#endif
            }
        }

        internal static Image Splash_Logo
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
                return (Image)resourceManager.GetObject("Loctite_SplashScreen");
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
                return (Image)resourceManager.GetObject("splash_logo");
#endif
            }
        }

        internal static string Contact_Content
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("Contact_Content");
            }
        }

        internal static string AdditiveManufacturingDeviceDLPStation5_XY50
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("AdditiveManufacturingDeviceDLPStation5_XY50");
            }
        }

        internal static string AdditiveManufacturingDeviceDLPStation5_XY75
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("AdditiveManufacturingDeviceDLPStation5_XY75");
            }
        }

        internal static string AdditiveManufacturingDeviceDLPStation5_XY100
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("AdditiveManufacturingDeviceDLPStation5_XY100");
            }
        }

        internal static string AdditiveManufacturingDeviceDLPStation5_XY50_OFFLINE
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("AdditiveManufacturingDeviceDLPStation5_XY50_OFFLINE");
            }
        }

        internal static string AdditiveManufacturingDeviceDLPStation5_XY75_OFFLINE
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("AdditiveManufacturingDeviceDLPStation5_XY75_OFFLINE");
            }
        }

        internal static string AdditiveManufacturingDeviceDLPStation5_XY100_OFFLINE
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("AdditiveManufacturingDeviceDLPStation5_XY100_OFFLINE");
            }
        }

        internal static string SoftwareUpdate_Url
        {
            get
            {
#if LOCTITE
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Loctite));
#else
                ResourceManager resourceManager = new ResourceManager(typeof(Atum.Studio.Branding_Atum3D));
#endif
                return resourceManager.GetString("SoftwareUpdate_Url");
            }
        }

        internal static RegistryKey LocalMachineSoftwareKey
        {
            get
            {
                try
                {
                    RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64);
                    RegistryKey localMachineRegistry32 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32);

#if LOCTITE
                    RegistryKey reg64 = localMachineRegistry64.OpenSubKey(@"Software\Loctite\Operator Station", false);
                    if (reg64 != null)
                    {
                        return reg64;
                    }

                    return localMachineRegistry32.OpenSubKey(@"Software\Loctite\Operator Station", false);
#else
                    RegistryKey reg64 = localMachineRegistry64.OpenSubKey(@"Software\atum3D\Operator Station", false);
                    if (reg64 != null)
                    {
                        return reg64;
                    }

                    return localMachineRegistry32.OpenSubKey(@"Software\atum3D\Operator Station", false);


#endif
                }
                catch
                {
                    return null;
                }
            }
        }

        internal static RegistryKey CurrentUserSoftwareKey
        {
            get
            {
                try
                {
                    //Check the 64-bit registry for "HKEY_LOCAL_MACHINE\SOFTWARE" 1st:
                    RegistryKey localMachineRegistry64 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry64);
                    RegistryKey localMachineRegistry32 = RegistryKey.OpenBaseKey(RegistryHive.CurrentUser, RegistryView.Registry32);

#if LOCTITE
                    RegistryKey reg64 = localMachineRegistry64.OpenSubKey(@"Software\Loctite\Operator Station", false);
                    if (reg64 != null)
                    {
                        return reg64;
                    }

                    return localMachineRegistry32.OpenSubKey(@"Software\Loctite\Operator Station", false);
#else
                    RegistryKey reg64 = localMachineRegistry64.OpenSubKey(@"Software\atum3D\Operator Station", false);
                    if (reg64 != null)
                    {
                        return reg64;
                    }

                    return localMachineRegistry32.OpenSubKey(@"Software\atum3D\Operator Station", false);


#endif
                }
                catch
                {
                    return null;
                }
            }
        }


    }
}
