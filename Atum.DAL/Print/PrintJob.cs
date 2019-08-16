using System;
using System.Collections.Generic;
using System.Text;
using Atum.DAL.Materials;
using Atum.DAL.Hardware;
using System.Xml.Serialization;

namespace Atum.DAL.Print
{
    [Serializable]
    public class PrintJob
    {
        public string Name { get; set; }
        public float TotalPrintVolume { get; set; }
        public string ApplicationVersion { get; set; }
        public string SlicesPath { get; set; }
        public Material Material { get; set; }
        public AtumPrinter SelectedPrinter { get; set; }
        public double JobTimeInSeconds { get; set; }
        public byte[] Thumbnail { get; set; }
        public byte[] ThumbnailLarge { get; set;}

        [XmlIgnore]
        public bool UseProjectorOptions { get; set; }

        [XmlIgnore]
        public bool PostRenderCompleted { get; set; }

        [XmlIgnore]
        public int AntiAliasFactor { get; set; }

        [XmlIgnore]
        public AntiAliasType AntiAliasSide { get; set; }

        public byte AntiAliasColor { get
            {
                return (byte)Math.Floor((double)255 / 100 * AntiAliasFactor);
            }
        }

        public bool Option_TurnProjectorOn { get; set; }
        public bool Option_TurnProjectorOff { get; set; }

    public enum AntiAliasType
    {
        None = 0,
        Inside = 1,
        Outside = 2
    }

        public TimeSpan PrintingTimeLayer(int currentSliceIndex)
        {
            var result = new TimeSpan(0, 0, 0);
            if (this.Material.InitialLayers > 0 && currentSliceIndex < this.Material.InitialLayers)
            {
                double layerTime = 0;
                layerTime += this.Material.CT1;
                layerTime += this.Material.RH1 / (this.Material.RSD1 / 60); //mm per second
                layerTime += this.Material.RH1 / (this.Material.RSU1 / 60); //mm per second
                layerTime += this.Material.RT1;
                layerTime += this.Material.TAT1;

                result = result.Add(TimeSpan.FromSeconds(Convert.ToInt32(layerTime)));
            }
            else
            {
                double layerDefaultTime = 0;
                layerDefaultTime += this.Material.CT2;
                layerDefaultTime += this.Material.RH2 / (this.Material.RSD2 / 60); //mm per second
                layerDefaultTime += this.Material.RH2 / (this.Material.RSU2 / 60); //mm per second
                layerDefaultTime += this.Material.RT1;
                layerDefaultTime += this.Material.TAT2;

                result = result.Add(TimeSpan.FromSeconds(Convert.ToInt32(layerDefaultTime)));
            }

            return result;
        }

        public TimeSpan PrintingTimeRemaining(int currentSliceIndex, long totalSlices)
        {
            //initial layers time
            var result = new TimeSpan(0, 0, 0);
            var defaultSlices = totalSlices;
            if (this.Material.InitialLayers > 0 && currentSliceIndex < this.Material.InitialLayers)
            {
                defaultSlices = defaultSlices - this.Material.InitialLayers;
                //
                double layerTime = 0;
                layerTime += this.Material.CT1;
                layerTime += this.Material.RH1 / (this.Material.RSD1 / 60); //mm per second
                layerTime += this.Material.RH1 / (this.Material.RSU1 / 60); //mm per second
                layerTime += this.Material.RT1;
                layerTime += this.Material.TAT1;

                layerTime *= (this.Material.InitialLayers - currentSliceIndex);
                result = result.Add(TimeSpan.FromSeconds(Convert.ToInt32(layerTime)));
            }

            //default layers
            var remainingDefaultSlices = totalSlices - this.Material.InitialLayers;
            if (this.Material.InitialLayers > 0 && currentSliceIndex >= this.Material.InitialLayers)
            {
                remainingDefaultSlices -= (currentSliceIndex + 1 - this.Material.InitialLayers);
            }
            //
            double layerDefaultTime = 0;
            layerDefaultTime += this.Material.CT2;
            layerDefaultTime += this.Material.RH2 / (this.Material.RSD2 / 60); //mm per second
            layerDefaultTime += this.Material.RH2 / (this.Material.RSU2 / 60); //mm per second
            layerDefaultTime += this.Material.RT2;
            layerDefaultTime += this.Material.TAT2;

            layerDefaultTime *= (remainingDefaultSlices + 1);
            result = result.Add(TimeSpan.FromSeconds(Convert.ToInt32(layerDefaultTime)));


            return result;
        }


        public PrintJob()
        {
        }

        public string ToSerialString(int layerIndex)
        {
            var result = string.Empty;
            if (this.Material != null)
            {
                if (layerIndex == 1)
                {
                    result += ((int)(this.Material.CT1 * 1000)).ToString() + ";";
                    result += ((int)Math.Round((this.Material.RH1 * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation))).ToString() + ";";
                    result += ((int)Math.Round(((this.Material.RSU1 / 60) * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation))).ToString() + ";";
                    result += ((int)(this.Material.TAT1 * 1000)).ToString() + ";";
                    result += ((int)(this.Material.RT1 * 1000)).ToString() + ";";
                    result += ((int)Math.Round(((this.Material.RSD1 / 60) * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation))).ToString() + ";";

                    var ltSteps = ((int)(this.Material.LT1 * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation));
                    if (this.SelectedPrinter.SpindleRotation == 3f)
                    {
                        ltSteps++;
                        ltSteps++;
                    }
                    result += ltSteps.ToString();
                }
                else
                {
                    result += ((int)(this.Material.CT2 * 1000)).ToString() + ";";
                    result += ((int)Math.Round((this.Material.RH2 * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation))).ToString() + ";";
                    result += ((int)Math.Round(((this.Material.RSU2 / 60) * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation))).ToString() + ";";
                    result += ((int)(this.Material.TAT2 * 1000)).ToString() + ";";
                    result += ((int)(this.Material.RT2 * 1000)).ToString() + ";";
                    result += ((int)Math.Round(((this.Material.RSD2 / 60) * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation))).ToString() + ";";

                    var ltSteps = ((int)(this.Material.LT2 * (this.SelectedPrinter.Microsteps * this.SelectedPrinter.Steps) / this.SelectedPrinter.SpindleRotation));
                    if (this.SelectedPrinter.SpindleRotation == 3f)
                    {
                        ltSteps++;
                        ltSteps++;
                    }
                    result += ltSteps.ToString();
                }
            }

            return result + ";";
        }
    }
}
