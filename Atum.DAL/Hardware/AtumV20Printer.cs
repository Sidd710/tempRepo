using System;
using Atum.DAL.Managers;
using System.Net;
using System.IO;
using Atum.DAL.Compression.Zip;
using System.Diagnostics;
using System.Collections.Generic;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class AtumV20Printer : AtumPrinter
    {
        public enum RemotePrinterV20Action
        {
            ProjectorOn = 100,
            ProjectorOff = 101
        }

        public AtumV20Printer()
        {
            this.Connections = new Managers.NetworkConnections();
            this.Properties = new AtumPrinterPropertyList();

           // this.BuildPlatformSizeY = 150;
           // this.BuildPlatformSizeX = 240;
            this.ProjectorResolutionX = 1920;
            this.ProjectorResolutionY = 1200;
            this.Microsteps = 16;
            this.Steps = 200;
            this.SpindleRotation = 2.5f;
            this.DisplayName = "Atum V2.0";
            this.ProjectorTurnOffDelay = 100;
            this.ProjectorTurnOnDelay = 100;
            this.PrinterHardwareType = "{E1C7D9F9-F88E-4FB3-A431-D4B4EF5E8DA2}";

            this.SetDefaultPrinterResolution(PrinterXYResolutionType.Micron75);
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


            //var rowMeasurePoints = new List<float> { 0, 319, 575, 831, 1087, 1343, 1599, 1919 };
            //var columnMeasurePoints = new List<float> { 0, 200, 360, 520, 680, 840, 1000, 1199 };

            ////13.23f,
            //var rowMeasuredSizes1 = new List<float> { 13.03f, 12.94f, 12.86f, 12.87f, 12.84f, 12.83f, 12.85f, 12.85f };
            //var rowMeasuredSizes2 = new List<float> { 13.03f, 12.93f, 12.88f, 12.87f, 12.92f, 12.89f, 12.97f, 12.99f };
            //var rowMeasuredSizes3 = new List<float> { 13.03f, 12.96f, 12.92f, 12.88f, 12.89f, 12.90f, 13.04f, 13.03f };
            //var rowMeasuredSizes4 = new List<float> { 13.14f, 13.01f, 12.91f, 12.91f, 12.94f, 12.96f, 13.06f, 13.06f };
            //var rowMeasuredSizes5 = new List<float> { 13.15f, 13.06f, 12.95f, 12.94f, 12.97f, 13.01f, 13.10f, 13.09f };
            //var rowMeasuredSizes6 = new List<float> { 13.15f, 13.11f, 12.98f, 12.93f, 12.94f, 12.97f, 13.01f, 13.01f };
            //var rowMeasuredSizes7 = new List<float> { 13.15f, 13.15f, 12.89f, 12.84f, 12.82f, 12.89f, 12.95f, 12.93f };
            //var rowMeasuredSizes8 = new List<float> { 13.15f, 13.14f, 12.99f, 12.93f, 12.99f, 13.02f, 12.95f, 12.90f };

            //var rowMeasuredSizes = new List<List<float>>
            //{
            //    rowMeasuredSizes1, rowMeasuredSizes2, rowMeasuredSizes3, rowMeasuredSizes4, rowMeasuredSizes5, rowMeasuredSizes6, rowMeasuredSizes7, rowMeasuredSizes8
            //};


            //var columnMeasuredSizes1 = new List<float> { 7.22f, 7.28f, 7.29f, 7.25f, 7.24f, 7.16f, 7.12f, 7.08f };
            //var columnMeasuredSizes2 = new List<float> { 7.27f, 7.31f, 7.35f, 7.32f, 7.27f, 7.25f, 7.24f, 7.18f };
            //var columnMeasuredSizes3 = new List<float> { 7.31f, 7.36f, 7.42f, 7.34f, 7.36f, 7.27f, 7.33f, 7.24f };
            //var columnMeasuredSizes4 = new List<float> { 7.39f, 7.43f, 7.40f, 7.34f, 7.36f, 7.35f, 7.39f, 7.27f };
            //var columnMeasuredSizes5 = new List<float> { 7.46f, 7.54f, 7.55f, 7.43f, 7.37f, 7.31f, 7.44f, 7.29f };
            //var columnMeasuredSizes6 = new List<float> { 7.47f, 7.60f, 7.57f, 7.48f, 7.51f, 7.43f, 7.41f, 7.30f };
            //var columnMeasuredSizes7 = new List<float> { 7.60f, 7.66f, 7.67f, 7.63f, 7.53f, 7.56f, 7.41f, 7.31f }; //7.25f
            //var columnMeasuredSizes8 = new List<float> { 7.62f, 7.70f, 7.72f, 7.67f, 7.62f, 7.55f, 7.48f, 7.27f }; // 7.18f 

            //var columnMeasuredSizes = new List<List<float>>
            //{
            //    columnMeasuredSizes1, columnMeasuredSizes2, columnMeasuredSizes3, columnMeasuredSizes4, columnMeasuredSizes5, columnMeasuredSizes6, columnMeasuredSizes7, columnMeasuredSizes8
            //};

           // this.LensWarpingCorrection = new LensWarpingCorrectionValues(rowMeasuredSizes, columnMeasuredSizes);
           // this.LensWarpingCorrection.Enabled = true;
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

        public void SendRemoteAction(RemotePrinterV20Action printerAction, string remoteIP)
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
