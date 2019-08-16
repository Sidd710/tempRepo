using Atum.Studio.Core.Managers;
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



namespace Atum.Studio.Controls.NewGui.Preference
{
    public partial class frmUserPreferences : NewGUIFormBase
    {
        public frmUserPreferences()
        {
            InitializeComponent();

        }

        public frmUserPreferences(UserProfileInfo userProfile, PerformanceSettings performanceSettings)
        {
            InitializeComponent();

            //add check png's
            this.ilCheckbox.Images.Clear();
            this.ilCheckbox.Images.Add(Properties.Resources.checkbox_not_checked);
            this.ilCheckbox.Images.Add(BrandingManager.Checkbox_Checked);

            this.chkSkip.Checked = userProfile.Settings_Skip_Welcome_Screen_On_Next_Start;
            this.chkTouchModeEnabled.Checked = userProfile.Settings_Enable_Touch_Interface_Mode;
            this.chkBasement.Checked = userProfile.Settings_Use_Support_Basement;
            this.chkUseNumericInputForPositioning.Checked = userProfile.Settings_Use_Numeric_Input_For_Positioning;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            UserProfileManager.UserProfile.Settings_Skip_Welcome_Screen_On_Next_Start = this.chkSkip.Checked;
            UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode = this.chkTouchModeEnabled.Checked;
            UserProfileManager.UserProfile.Settings_Use_Support_Basement = this.chkBasement.Checked;
            UserProfileManager.UserProfile.Settings_Use_Numeric_Input_For_Positioning = this.chkUseNumericInputForPositioning.Checked;

            UserProfileManager.Save();
            this.Close();
        }

        private void chkSkip_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSkip.Checked)
            {
                chkSkip.ImageIndex = 1;
            }
            else
            {
                chkSkip.ImageIndex = 0;
            }
        }

        private void chkBasement_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBasement.Checked)
            {
                chkBasement.ImageIndex = 1;
            }
            else
            {
                chkBasement.ImageIndex = 0;
            }
        }

        private void chkTouchModeEnabled_CheckedChanged(object sender, EventArgs e)
        {
            if (chkTouchModeEnabled.Checked)
            {
                chkTouchModeEnabled.ImageIndex = 1;
            }
            else
            {
                chkTouchModeEnabled.ImageIndex = 0;
            }
        }

        private void chkUseNumericInputForPositioning_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseNumericInputForPositioning.Checked)
            {
                chkUseNumericInputForPositioning.ImageIndex = 1;
            }
            else
            {
                chkUseNumericInputForPositioning.ImageIndex = 0;
            }
        }
    }
}
