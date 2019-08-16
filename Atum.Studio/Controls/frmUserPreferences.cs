using Atum.Studio.Core.Platform;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace Atum.Studio.Controls
{
    public partial class frmUserPreferences : BasePopup
    {
        internal Core.UserPreferences.DefaultPreferences DefaultPreferences { get; set; }
        internal Core.UserPreferences.AdvancedPreferences AdvancedPreferences { get; set; }

        public frmUserPreferences()
        {
            InitializeComponent();
        }

        public frmUserPreferences(Core.Managers.UserProfileInfo userProfile, PerformanceSettings performanceSettings)
        {
            InitializeComponent();

            this.Icon = Core.Managers.BrandingManager.MainForm_Icon;

            this.DefaultPreferences = new Core.UserPreferences.DefaultPreferences(userProfile);
            this.AdvancedPreferences = new Core.UserPreferences.AdvancedPreferences(userProfile, performanceSettings);

#if LOCTITE
            this.gpAdvancedSettings.SelectedItemWithFocusBackColor = this.gpAdvancedSettings.CommandsBorderColor = this.gpAdvancedSettings.LineColor = Core.Managers.BrandingManager.DockPanelTitleBackground;
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

        private void trAdvancedSettings_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.gpAdvancedSettings.Visible = false;

            var selectedNode = e.Node;

            if (selectedNode != null)
            {
                switch (selectedNode.Name.ToLower())
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
