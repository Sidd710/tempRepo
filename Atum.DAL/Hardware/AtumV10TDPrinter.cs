using System;
using System.Collections.Generic;
using System.Text;
using Atum.DAL.Managers;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Net;
using System.Diagnostics;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class AtumV10TDPrinter : AtumPrinter
    {
        public long StartPositionInMM { get; set; }

        public enum RemotePrinterV10TDAction
        {
            ProjectorOn = 100,
            ProjectorOff = 101
        }

        public AtumV10TDPrinter()
        {
            this.Connections = new Managers.NetworkConnections();
            this.Properties = new AtumPrinterPropertyList();

           // this.BuildPlatformSizeY = 150;
            //this.BuildPlatformSizeX = 240;
            this.ProjectorResolutionX = 1920;
            this.ProjectorResolutionY = 1200;
            this.Microsteps = 16;
            this.Steps = 200;
            this.DisplayName = "Atum V1.0 TopDown";
            this.ProjectorTurnOffDelay = 10;
            this.ProjectorTurnOnDelay = 100;
            this.PrinterHardwareType = "{289EF7C2-3342-4EA3-AF7D-883B26454318}";
            this.StartPositionInMM = 50;
        }

        public override void AtumPrinter_Loaded()
        {
             base.AtumPrinter_Loaded();

             if (this.TrapeziumCorrectionSideA == 0)
             {
                 this.CalcDefaultTrapezoidValues();
             }
        }

        public void CalcDefaultTrapezoidValues()
        {
            this.TrapeziumCorrectionSideA = this.TrapeziumCorrectionSideC = (float)this.ProjectorResolutionY / 20;
            this.TrapeziumCorrectionSideB = this.TrapeziumCorrectionSideD = (float)this.ProjectorResolutionX / 20;
            this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = (float)Math.Sqrt((this.TrapeziumCorrectionSideA * this.TrapeziumCorrectionSideA) + (this.TrapeziumCorrectionSideB * this.TrapeziumCorrectionSideB));

            if (this.CorrectionFactorX != 1 && this.CorrectionFactorX != 0)
            {
                this.TrapeziumCorrectionSideA = this.TrapeziumCorrectionSideC = this.TrapeziumCorrectionSideA * this.CorrectionFactorY;
                this.TrapeziumCorrectionSideB = this.TrapeziumCorrectionSideD = this.TrapeziumCorrectionSideD * this.CorrectionFactorX;
                this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = this.TrapeziumCorrectionSideF * this.CorrectionFactorY;
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

        public void SendRemoteAction(RemotePrinterV10TDAction printerAction, string remoteIP)
        {
            try{
                var remoteAction = new DAL.Remoting.PrinterAction();
                remoteAction.PrinterType = this.PrinterHardwareType;
                remoteAction.PrinterActionId = (int)printerAction;

                ConnectionManager.Send(remoteAction, IPAddress.Parse(remoteIP), 11000);
            }
            catch(Exception exc){
                Debug.WriteLine(exc.Message);
            }
        }
    }
}
