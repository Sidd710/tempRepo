using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls
{
    public partial class LightFieldCalibrationWizard : Form
    {

        public LightFieldCalibrationWizard()
        {
            InitializeComponent();
        }

        private void LightFieldCalibrationWizard_Load(object sender, EventArgs e)
        {
            //default settings
            this.tbWizard.Top -= 23;
            this.tbWizard.Height += 25;
            this.tbWizard.Width += 5;

            this.lightFieldCalibrationCalibrationTabPanel1.ButtonBackVisible = true;
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonFinished = true;
            this.lightFieldCalibrationCalibrationTabPanel1.ButtonNextVisible = true;

        }

        private void lightFieldCalibrationWelcomeTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex++;
        }

        private void lightFieldCalibrationCalibrationTabPanel1_ButtonBack_Click(object sender, EventArgs e)
        {
            this.tbWizard.SelectedIndex--;
        }

        private void lightFieldCalibrationCalibrationTabPanel1_ButtonNext_Click(object sender, EventArgs e)
        {
            PrinterManager.Save();
            this.Close();
        }

    }
}