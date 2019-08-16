using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Controls.WifiWizard
{
    public partial class WifiSettingsTabPanel : WizardTabPanel
    {
        internal Atum.DAL.Hardware.PrinterWiFiConfiguration WifiConfiguration
        {
            get
            {
                var configuration = new Atum.DAL.Hardware.PrinterWiFiConfiguration()
                {
                    SSID = this.txtNetworkName.Text,
                    Passphrase = this.txtSecurityKey.Text
                };

                switch (this.cbSecurityType.SelectedIndex)
                {
                    case 0:
                        configuration.NetworkType = Atum.DAL.Hardware.PrinterWiFiConfiguration.typeNetwork.WPAPersonal;
                        break;
                    case 1:
                        configuration.NetworkType = Atum.DAL.Hardware.PrinterWiFiConfiguration.typeNetwork.WPA2Personal;
                        break;
                }

                switch (this.cbEncryptionType.SelectedIndex)
                {
                    case 0:
                        configuration.EncryptionType = Atum.DAL.Hardware.PrinterWiFiConfiguration.typeEncryption.TKIP;
                        break;
                    case 1:
                        configuration.EncryptionType = Atum.DAL.Hardware.PrinterWiFiConfiguration.typeEncryption.AES;
                        break;
                }

                return configuration;
            }
        }
        
        public WifiSettingsTabPanel()
        {
            InitializeComponent();
        }

        internal void FocusDefaultControl()
        {
            this.txtNetworkName.Focus();
        }

        private void chkHideCharacters_CheckedChanged(object sender, EventArgs e)
        {
            this.txtSecurityKey.PasswordChar = this.chkHideCharacters.Checked ? '*' : '\0';
        }

        private void WifiSettingsTabPanel_Load(object sender, EventArgs e)
        {
            this.cbSecurityType.SelectedIndex = 0;
            this.cbEncryptionType.SelectedIndex = 0;
        }
    }
}
