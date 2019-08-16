using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Managers
{
    public class RegistryProfile
    {
        public bool DebugMode { get; set; }
        public bool VerboseMode { get; set; }
    }

    public class RegistryManager
    {
        public static RegistryProfile RegistryProfile { get; set; }

        public enum registrySettingType
        {
            DebugMode = 1,
            VerboseMode = 2
        }

        private static object GetRegistrySetting(registrySettingType settingType)
        {
            var currentUserKey = BrandingManager.CurrentUserSoftwareKey;
            var localMachineKey = BrandingManager.LocalMachineSoftwareKey;

            switch (settingType)
            {
                case registrySettingType.DebugMode:
                    object registryDebugModeValue = null;
                    if (currentUserKey != null){ registryDebugModeValue = (int)currentUserKey.GetValue("DebugMode");}
                    if (registryDebugModeValue == null && localMachineKey != null) { registryDebugModeValue = localMachineKey.GetValue("DebugMode"); }

#if DEBUG
                    return true;
#else
                    if (registryDebugModeValue != null) { return (int)registryDebugModeValue == 1? true:false; }
                    return false;
#endif


                case registrySettingType.VerboseMode:
                    object registryVerboseModeValue = null;
                    if (currentUserKey != null) { registryVerboseModeValue = currentUserKey.GetValue("VerboseMode"); }
                    if (registryVerboseModeValue == null && localMachineKey != null) { registryVerboseModeValue = localMachineKey.GetValue("VerboseMode"); }

                    if (registryVerboseModeValue != null) { return (int)registryVerboseModeValue == 1 ? true : false; }
                    return false;
            }

            return null;
        }

        public static void GetRegistryProfileSettings()
        {
            if (RegistryProfile == null) { RegistryProfile = new RegistryProfile(); }
            RegistryProfile.DebugMode = bool.Parse(GetRegistrySetting(registrySettingType.DebugMode).ToString());
            RegistryProfile.VerboseMode = bool.Parse(GetRegistrySetting(registrySettingType.VerboseMode).ToString());
        }
    }
}
