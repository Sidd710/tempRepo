namespace Atum.Studio.Core.UserPreferences
{
    internal class DefaultPreferences
    {
        internal UserInterfacePreferences UserInterface { get; set; }
        internal SoftwareOptionsPreferences SoftwareOptions { get; set; }
        internal SelectionOptionsPreferences SelectionOptions { get; set; }

        internal DefaultPreferences(Managers.UserProfileInfo userProfile)
        {
            this.UserInterface = new UserInterfacePreferences(userProfile.Settings_Studio_UseLargeToolbarIcons, userProfile.Settings_Skip_Welcome_Screen_On_Next_Start, userProfile.Settings_Use_Support_Basement, userProfile.Settings_Lift_Model_OnSupport);
            this.SoftwareOptions = new SoftwareOptionsPreferences(userProfile.Settings_Studio_AutoUpdateNotification);
            this.SelectionOptions = new SelectionOptionsPreferences(userProfile.SelectionOptions_Enable_Annotations, userProfile.SelectionOptions_Enable_XYZ_Axis);
        }
    }
}
