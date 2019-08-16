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
    public partial class LightFieldCalibrationCalibrationTabPanel : WizardTabPanel
    {
        public LightFieldCalibrationCalibrationTabPanel()
        {
            InitializeComponent();
        }

        private void LightFieldCalibrationCalibrationTabPanel_Load(object sender, EventArgs e)
        {
            var currentPrinter = PrinterManager.DefaultPrinter;
            if (currentPrinter != null)
            {
                if (currentPrinter.Projectors != null && currentPrinter.Projectors.Count > 0)
                {
                    foreach (var projector in currentPrinter.Projectors)
                    {
                        var plLightCalibrationTop = -1;
                        for (var projectorLightIndex = 0; projectorLightIndex < projector.LightFieldCalibration.Count; projectorLightIndex++)
                        {
                            var plLightCalibration = new Panel();
                            plLightCalibration.Size = new Size(60, 80);
                            plLightCalibration.BackColor = Color.Orange;
                            if (projectorLightIndex % 8 == 0) { plLightCalibrationTop++; }

                            var txtNumericUpDown = new NumericUpDown();
                            txtNumericUpDown.Tag = projectorLightIndex;
                            txtNumericUpDown.DecimalPlaces = 0;
                            txtNumericUpDown.Minimum = 0;
                            txtNumericUpDown.Maximum = 1000;
                            txtNumericUpDown.Increment = 1;
                            txtNumericUpDown.Width = 50;
                            txtNumericUpDown.Value = projector.LightFieldCalibration[projectorLightIndex];
                            txtNumericUpDown.ValueChanged += new EventHandler(txtNumericUpDown_ValueChanged);

                            plLightCalibration.Controls.Add(txtNumericUpDown);
                            txtNumericUpDown.Left = (plLightCalibration.Width - txtNumericUpDown.Width) / 2;
                            txtNumericUpDown.Top = (plLightCalibration.Height - txtNumericUpDown.Height) / 2;
                            plLightCalibration.Location = new Point(((projectorLightIndex % 8) * plLightCalibration.Width + (projectorLightIndex % 8)), (plLightCalibrationTop * plLightCalibration.Height) + (plLightCalibrationTop));
                            this.plProjectCalibrationPanels.Controls.Add(plLightCalibration);
                        }
                    }
                }

                this.plProjectCalibrationPanels.Left = (this.Width - this.plProjectCalibrationPanels.Width) / 2;
            }
            else
            {
                this.plProjectCalibrationPanels.Controls.Add(new Label() { Text = "Projector calibration not available" });
            }
        }

        void txtNumericUpDown_ValueChanged(object sender, EventArgs e)
        {
            var txtNumeric = sender as NumericUpDown;
            PrinterManager.DefaultPrinter.Projectors[0].LightFieldCalibration[(int)txtNumeric.Tag] = Convert.ToInt32(txtNumeric.Value);
        }
    }
}
