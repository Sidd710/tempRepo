using Atum.Studio.Core.Platform;

namespace Atum.Studio.Core.UserPreferences
{
    internal class AdvancedPreferences
    {
        internal AdvancedModelPreferences ModelPreferences { get; set; }
        internal AdvancedSupportEnginePreferences SupportEnginePreferences { get; set; }
        internal AdvancedPerformancePreferences PerformancePreferences { get; set; }
        internal AdvancedLicensesPreferences LicensesPreferences { get; set; }

        internal AdvancedPreferences(Managers.UserProfileInfo userProfile, PerformanceSettings performanceSettings)
        {
            this.ModelPreferences = new AdvancedModelPreferences(userProfile.Settings_PrintJob_MirrorObjects, userProfile.Settings_PrintJob_FirstSlice_MinAmountOfPixels);
            this.SupportEnginePreferences = new AdvancedSupportEnginePreferences(userProfile.SupportEngine_Basement_Thickness, userProfile.SupportEngine_Penetration_Depth);
            this.PerformancePreferences = new AdvancedPerformancePreferences(performanceSettings.PrintJobGenerationMultiThreading);
            this.LicensesPreferences = new AdvancedLicensesPreferences(userProfile.LicenseServer_ServerName);
        }
    }
}
