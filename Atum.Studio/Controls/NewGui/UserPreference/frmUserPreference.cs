using Atum.Studio.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGUI.UserPreference
{
    public partial class frmUserPreference : NewGUIFormBase
    {
        internal Core.UserPreferences.DefaultPreferences DefaultPreferences { get; set; }
        internal Core.UserPreferences.AdvancedPreferences AdvancedPreferences { get; set; }

        private PropertyGridFiltered gpAdvancedSettings;

        public frmUserPreference()
        {
            InitializeComponent();
            //base.he = this.Text;
            AdvancedPreferencesSettingAdd();
        }

        public frmUserPreference(Core.Managers.UserProfileInfo userProfile, PerformanceSettings performanceSettings)
        {
            InitializeComponent();
            AdvancedPreferencesSettingAdd();

///            this.lblHeaderText = this.Text;

            //            this.Icon = Core.Managers.BrandingManager.MainForm_Icon;

            this.DefaultPreferences = new Core.UserPreferences.DefaultPreferences(userProfile);
            this.AdvancedPreferences = new Core.UserPreferences.AdvancedPreferences(userProfile, performanceSettings);

            #if LOCTITE
            //            this.gpAdvancedSettings.SelectedItemWithFocusBackColor = this.gpAdvancedSettings.CommandsBorderColor = this.gpAdvancedSettings.LineColor = Core.Managers.BrandingManager.DockPanelTitleBackground;
                        this.gpDisplaySoftware.Visible = false;
            #endif

                    this.gpAdvancedSettings.Visible = false;

            //default settings
            this.chkAnnotations.Checked = this.DefaultPreferences.SelectionOptions.UseAnnotations;
            this.chkAnnotations.CheckedChanged += chkDefaultPreferences_CheckedChanged;
            this.chkXYZAxis.Checked = this.DefaultPreferences.SelectionOptions.ShowXYZAxis;
            this.chkXYZAxis.CheckedChanged += chkDefaultPreferences_CheckedChanged;
            this.chkUserInterfaceUseLargeToolbarIcons.Checked = this.DefaultPreferences.UserInterface.UseLargeToolbarIcons;
            this.chkUserInterfaceUseLargeToolbarIcons.CheckedChanged += chkDefaultPreferences_CheckedChanged;
            this.chkSoftwareCheckForSoftwareUpdates.Checked = this.DefaultPreferences.SoftwareOptions.CheckForSoftwareUpdates;
            this.chkSoftwareCheckForSoftwareUpdates.CheckedChanged += chkDefaultPreferences_CheckedChanged;

        }
        private void chkDefaultPreferences_CheckedChanged(object sender, EventArgs e)
        {
            this.DefaultPreferences.UserInterface.UseLargeToolbarIcons = this.chkUserInterfaceUseLargeToolbarIcons.Checked;
            this.DefaultPreferences.SelectionOptions.ShowXYZAxis = this.chkXYZAxis.Checked;
            this.DefaultPreferences.SelectionOptions.UseAnnotations = this.chkAnnotations.Checked;
            this.DefaultPreferences.SoftwareOptions.CheckForSoftwareUpdates = this.chkSoftwareCheckForSoftwareUpdates.Checked;
        }

        void AdvancedPreferencesSettingAdd()
        {
            this.newGUIContentSplitContainerBase1.RightPanel.Controls.Clear();
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Clear();

            this.gpAdvancedSettings = new Atum.Studio.Controls.PropertyGridFiltered();
            this.newGUIContentSplitContainerBase1.RightPanel.Controls.Add(gpAdvancedSettings);


            var summaryHeight = 0;
            
            var AdvancedSettingsbase1 = new AdvancedSettingsbase();
            AdvancedSettingsbase1.onSelected += AdvancedSettingsbase_onSelected;
            AdvancedSettingsbase1.SettingText = "Model Properties";
            AdvancedSettingsbase1.Left = 0;
            AdvancedSettingsbase1.Top = summaryHeight;
            AdvancedSettingsbase1.Width = this.newGUIContentSplitContainerBase1.LeftPanel.Width;
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(AdvancedSettingsbase1);

            summaryHeight += 40;

            var AdvancedSettingsbase2 = new AdvancedSettingsbase();
            AdvancedSettingsbase2.onSelected += AdvancedSettingsbase_onSelected;
            AdvancedSettingsbase2.SettingText = "Support Engine";
            AdvancedSettingsbase2.Left = 0;
            AdvancedSettingsbase2.Top = summaryHeight;
            AdvancedSettingsbase2.Width = this.newGUIContentSplitContainerBase1.LeftPanel.Width;
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(AdvancedSettingsbase2);

            summaryHeight += 40;

            var AdvancedSettingsbase3 = new AdvancedSettingsbase();
            AdvancedSettingsbase3.onSelected += AdvancedSettingsbase_onSelected;
            AdvancedSettingsbase3.SettingText = "Performance";
            AdvancedSettingsbase3.Left = 0;
            AdvancedSettingsbase3.Top = summaryHeight;
            AdvancedSettingsbase3.Width = this.newGUIContentSplitContainerBase1.LeftPanel.Width;
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(AdvancedSettingsbase3);

            summaryHeight += 40;

            var AdvancedSettingsbase4 = new AdvancedSettingsbase();
            AdvancedSettingsbase4.onSelected += AdvancedSettingsbase_onSelected;
            AdvancedSettingsbase4.SettingText = "Licenses";
            AdvancedSettingsbase4.Left = 0;
            AdvancedSettingsbase4.Top = summaryHeight;
            AdvancedSettingsbase4.Width = this.newGUIContentSplitContainerBase1.LeftPanel.Width;
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(AdvancedSettingsbase4);

            summaryHeight += 40;
        }

        private void AdvancedSettingsbase_onSelected(object sender, EventArgs e)
        {

            foreach (var AdvancedSettingsbase in this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<AdvancedSettingsbase>())
            {
                if (AdvancedSettingsbase.Selected)
                {
                    AdvancedSettingsbase.Selected = false;
                }
            }

            var selectedAdvancedSettingsbase = sender as AdvancedSettingsbase;
            selectedAdvancedSettingsbase.Selected = true;
            RightpanelUpdate(selectedAdvancedSettingsbase);
        }

         void RightpanelUpdate(AdvancedSettingsbase advancedSettingsbase)
        {
            this.gpAdvancedSettings.Visible = false;

            var selectedNode = advancedSettingsbase;

            if (selectedNode != null)
            {
                switch (selectedNode.SettingText.ToLower().Replace(" ", String.Empty))
                {
                    case "modelproperties":
                        this.gpAdvancedSettings.SelectedObject = this.AdvancedPreferences.ModelPreferences;
                        this.gpAdvancedSettings.Visible = true;
                        this.gpAdvancedSettings.Focus();
                        break;
                    case "supportengine":
                        this.gpAdvancedSettings.SelectedObject = this.AdvancedPreferences.SupportEnginePreferences;
                        this.gpAdvancedSettings.Visible = true;
                        break;
                    case "performance":
                        this.gpAdvancedSettings.SelectedObject = this.AdvancedPreferences.PerformancePreferences;
                        this.gpAdvancedSettings.Visible = true;
                        break;

                    case "licenses":
                        this.gpAdvancedSettings.SelectedObject = this.AdvancedPreferences.LicensesPreferences;
                        this.gpAdvancedSettings.Visible = true;
                        break;
                    default:
                        break;

                }
            }
        }

    }
}
