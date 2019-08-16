using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.WifiWizard
{
    public partial class WifiWizard : Form
    {
        public WifiWizard()
        {
            InitializeComponent();
        }

        private void WifiWizard_Load(object sender, EventArgs e)
        {
            //default settings
            this.tbWizard.Top -= 23;
            this.tbWizard.Height += 25;
            this.tbWizard.Width += 5;
            this.wifiWelcomeTabPanel1.HideFooter = false;
            this.wifiWelcomeTabPanel1.btnBack.Enabled = true;
            this.wifiWelcomeTabPanel1.btnBack.Visible = true;
            this.wifiWelcomeTabPanel1.btnBack.Text = "Cancel";
            this.wifiSettingsTabPanel1.HideFooter = false;
            this.wifiSettingsTabPanel1.ButtonBackVisible = true;
            this.wifiFinishedTabPanel1.ButtonBackVisible = true;
            this.wifiFinishedTabPanel1.HideFooter = false;
            this.wifiSaveConfigurationToUSB1.ButtonBackVisible = true;
            this.wifiSaveConfigurationToUSB1.HideFooter = false;
            this.wifiUpdatePrinterTabPanel1.ButtonBackVisible = true;
            this.wifiUpdatePrinterTabPanel1.HideFooter = false;
        }

        private void wifiWelcomeTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
            this.wifiSettingsTabPanel1.FocusDefaultControl();
        }

        private void wifiSettingsTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
            this.wifiSaveConfigurationToUSB1.WiFiConfiguration = this.wifiSettingsTabPanel1.WifiConfiguration;
        }

        private void wifiSettingsTabPanel1_ButtonBack_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex--;
        }

        private void wifiFinishedTabPanel1_ButtonBack_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex--;
        }

        private void wifiFinishedTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void wifiSaveConfigurationToUSB1_ButtonBack_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex--;
        }

        private void wifiSaveConfigurationToUSB1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
        }

        private void wifiUpdatePrinterTabPanel1_ButtonBack_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex--;
        }

        private void wifiUpdatePrinterTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
        }

        private void wifiWelcomeTabPanel1_ButtonBack_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }


    }
}
