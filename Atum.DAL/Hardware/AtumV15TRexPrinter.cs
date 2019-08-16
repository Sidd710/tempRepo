using System;
using Atum.DAL.Managers;
using System.Net;
using System.Diagnostics;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class AtumV15TRexPrinter : AtumPrinter
    {
        public enum RemotePrinterV15Action
        {
            ProjectorOn = 100,
            ProjectorOff = 101
        }

        public AtumV15TRexPrinter()
        {
            this.Connections = new Managers.NetworkConnections();
            this.Connections.Add(new NetworkConnection() { IPAddress = "127.0.0.1", ConnectionType = NetworkConnection.typeConnection.LAN });
            this.Properties = new AtumPrinterPropertyList();

            //this.BuildPlatformSizeY = 150;
            //this.BuildPlatformSizeX = 240;
            this.ProjectorResolutionX = 1920;
            this.ProjectorResolutionY = 1200;
            this.Microsteps = 16;
            this.Steps = 200;
            this.DisplayName = "Atum V1.5 T-Rex";
            this.ProjectorTurnOffDelay = 100;
            this.ProjectorTurnOnDelay = 100;
            this.PrinterHardwareType = "{7182A4D3-687E-4EB9-BA04-F626A291A24D}";
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

        public override void CreateProjectors()
        {
            this.Projectors.Add(new Projector(0));
            this.Projectors.Add(new Projector(1));
            this.Projectors.Add(new Projector(2));
            this.Projectors.Add(new Projector(3));
            this.Projectors.Add(new Projector(4));
            this.Projectors.Add(new Projector(5));
        }


        public override string ToString()
        {
            return this.DisplayName;
        }

        public void SendRemoteAction(RemotePrinterV15Action printerAction, string remoteIP)
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
