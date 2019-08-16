using System;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class LoctiteV10 : AtumPrinter
    {
        public LoctiteV10()
        {
            this.Connections = new Managers.NetworkConnections();
            this.Properties = new AtumPrinterPropertyList();

            //this.BuildPlatformSizeX = 240; //factor 8
            //this.BuildPlatformSizeY = 135; //factor 8
            this.ProjectorResolutionX = 1920;
            this.ProjectorResolutionY = 1080;
            this.Microsteps = 16;
            this.Steps = 200;
            this.SpindleRotation = 2.5f;
            this.DisplayName = "Loctite V1.0";
            this.ProjectorTurnOffDelay = 0;
            this.ProjectorTurnOnDelay = 0;
            this.PrinterHardwareType = "{D2C7D9F9-F88E-4FB3-A431-D4B4EF5E8DA2}";

            this.SetDefaultPrinterResolution(PrinterXYResolutionType.Micron100);

            //this.LensWarpingCorrection = new LensWarpingCorrectionValues();
        }

        public override void AtumPrinter_Loaded()
        {
            base.AtumPrinter_Loaded();

            if (this.TrapeziumCorrectionSideA == 0)
            {
                this.CalcDefaultTrapezoidValues();
            }

            if (string.IsNullOrEmpty(this.ID))
            {
                this.ID = Guid.NewGuid().ToString().ToUpper();
            }
        }
          
        public void CalcDefaultTrapezoidValues()
        {
            this.TrapeziumCorrectionSideA = this.TrapeziumCorrectionSideC = (float)this.ProjectorResolutionY / 10;
            this.TrapeziumCorrectionSideB = this.TrapeziumCorrectionSideD = (float)this.ProjectorResolutionX / 10;
            this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = (float)Math.Sqrt((this.TrapeziumCorrectionSideA * this.TrapeziumCorrectionSideA) + (this.TrapeziumCorrectionSideB * this.TrapeziumCorrectionSideB));

            if (this.CorrectionFactorX != 1 && this.CorrectionFactorX != 0)
            {
                this.TrapeziumCorrectionSideA = this.TrapeziumCorrectionSideC = this.TrapeziumCorrectionSideA * this.CorrectionFactorY;
                this.TrapeziumCorrectionSideB = this.TrapeziumCorrectionSideD = this.TrapeziumCorrectionSideD * this.CorrectionFactorX;
                this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = (float)Math.Sqrt((this.TrapeziumCorrectionSideA * this.TrapeziumCorrectionSideA) + (this.TrapeziumCorrectionSideB * this.TrapeziumCorrectionSideB));
            }
        }

        public override void CreateProperties()
        {
            var spindleProperty = new AtumPrinterProperty(AtumPrinterProperty.typePrinterProperty.TypeOfSpindle);
            spindleProperty.Name = "Type of Spindle";
            spindleProperty.Values.Add(new AtumPrinterPropertyValue() { Text = "Trapezium spindle", Value = 0 });
            spindleProperty.Values.Add(new AtumPrinterPropertyValue() { Text = "Ball spindle", Value = 1, Selected = true });

            this.Properties.Add(spindleProperty);
        }

        public override void CreateProjectors()
        {
            this.Projectors.Add(new Projector(0));
        }


        public override string ToString()
        {
            return this.DisplayName;
        }
    }
}
