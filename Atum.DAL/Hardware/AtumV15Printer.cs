using System;
using Atum.DAL.Managers;
using System.Net;
using Atum.DAL.Compression.Zip;
using System.IO;
using System.Diagnostics;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class AtumV15Printer : AtumPrinter
    {
        public enum RemotePrinterV15Action
        {
            ProjectorOn = 100,
            ProjectorOff = 101
        }

        public AtumV15Printer()
        {
            this.Connections = new Managers.NetworkConnections();
            this.Properties = new AtumPrinterPropertyList();

            //this.BuildPlatformSizeY = 150;
            //this.BuildPlatformSizeX = 240;
            this.ProjectorResolutionX = 1920;
            this.ProjectorResolutionY = 1200;
            this.SpindleRotation = 2.5f;
            this.Microsteps = 16;
            this.Steps = 200;
            this.DisplayName = "Atum V1.5";
            this.ProjectorTurnOffDelay = 100;
            this.ProjectorTurnOnDelay = 100;
            this.PrinterHardwareType = "{967BD0EC-35C1-436A-8D92-145823F17F6E}";

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

        public void SendRemoteAction(RemotePrinterV15Action printerAction, string remoteIP)
        {
            try
            {
                var remoteAction = new DAL.Remoting.PrinterAction();
                remoteAction.PrinterType = this.PrinterHardwareType;
                remoteAction.PrinterActionId = (int)printerAction;

                ConnectionManager.Send(remoteAction, IPAddress.Parse(remoteIP), 11000);
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }
    }
}
