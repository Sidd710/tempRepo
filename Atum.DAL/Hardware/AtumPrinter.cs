using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel;
using Atum.DAL.Managers;
using System.Xml.Serialization;
using System.Diagnostics;
using OpenTK;
using Atum.DAL.Hardware.Modules;

namespace Atum.DAL.Hardware
{
    [XmlInclude(typeof(AtumNULLPrinter))]
    [XmlInclude(typeof(AtumV10TDPrinter))]
    [XmlInclude(typeof(AtumV15Printer))]
    [XmlInclude(typeof(AtumV20Printer))]
    [XmlInclude(typeof(AtumDLPStation5))]
    [XmlInclude(typeof(LoctiteV10))]


    [Serializable]
    public abstract class AtumPrinter : INotifyPropertyChanged
    {
        public enum PrinterXYResolutionType
        {
            Unknown,
            Micron50,
            Micron40,
            Micron75,
            Micron100,
        }

        [XmlIgnore]
        public bool IsDirty { get; set; }

        public float MaxBuildSizeX
        {
            get
            {
                switch (PrinterXYResolution)
                {
                    case PrinterXYResolutionType.Micron100:
                        return 192;
                    case PrinterXYResolutionType.Micron75:
                        return 144f;
                    case PrinterXYResolutionType.Micron50:
                        return 96;
                    case PrinterXYResolutionType.Micron40:
                        return 76.8f;
                }

                return 0;
            }
        }

        public float MaxBuildSizeY
        {
            get
            {
                if (this is AtumDLPStation5 || this is LoctiteV10)
                {
                    switch (PrinterXYResolution)
                    {
                        case PrinterXYResolutionType.Micron100:
                            return 108;
                        case PrinterXYResolutionType.Micron75:
                            return 81;
                        case PrinterXYResolutionType.Micron50:
                            return 54;
                    }
                }
                else
                {
                    switch (PrinterXYResolution)
                    {
                        case PrinterXYResolutionType.Micron100:
                            return 120;
                        case PrinterXYResolutionType.Micron75:
                            return 90;
                        case PrinterXYResolutionType.Micron50:
                            return 60;
                        case PrinterXYResolutionType.Micron40:
                            return 28;
                    }
                }

                return 0;
            }
        }

        [XmlIgnore]
        private SortedDictionary<int, List<int>> _projectorLightFields;

        protected string _displayName { get; set; }
        protected string _serialNumber { get; set; }

        public string ID { get; set; }
        public string DisplayName { get { return this._displayName; } set { this._displayName = value; NotifyPropertyChanged("DisplayName"); } }
        public string SerialNumber { get { return this._serialNumber; } set { this._serialNumber = value; NotifyPropertyChanged("SerialNumber"); } }

        public string PrinterHardwareType { get; set; }
        public string Description { get; set; }
        public Projectors Projectors { get; set; }

        [field: NonSerialized()]
        public event PropertyChangedEventHandler PropertyChanged;
        public int ProjectorResolutionY { get; set; }
        public int ProjectorResolutionX { get; set; }
        public int Microsteps { get; set; }
        public int Steps { get; set; }
        public float SpindleRotation { get; set; }
        public float CorrectionFactorX { get; set; }
        public float CorrectionFactorY { get; set; }

        #region Trapezium Correction
        public float TrapeziumCorrectionSideA { get; set; }
        public float TrapeziumCorrectionSideB { get; set; }
        public float TrapeziumCorrectionSideC { get; set; }
        public float TrapeziumCorrectionSideD { get; set; }
        public float TrapeziumCorrectionSideE { get; set; }
        public float TrapeziumCorrectionSideF { get; set; }

        public float TrapeziumCorrectionInputA { get; set; }
        public float TrapeziumCorrectionInputB { get; set; }
        public float TrapeziumCorrectionInputC { get; set; }
        public float TrapeziumCorrectionInputD { get; set; }
        public float TrapeziumCorrectionInputE { get; set; }
        public float TrapeziumCorrectionInputF { get; set; }

        public float TrapeziumCorrectionRawInputA { get; set; }
        public float TrapeziumCorrectionRawInputB { get; set; }
        public float TrapeziumCorrectionRawInputC { get; set; }
        public float TrapeziumCorrectionRawInputD { get; set; }
        public float TrapeziumCorrectionRawInputE { get; set; }
        public float TrapeziumCorrectionRawInputF { get; set; }

        public DateTime TrapeziumCorrectionSetDateTime { get; set; }

        public List<TrapeziumCorrection> PreviousTrapeziumCorrections { get; set; }
        


        public float TrapeziumCorrectionFactorX
        {
            get
            {
                return (float)((Math.Min(this.TrapeziumCorrectionInputD, this.TrapeziumCorrectionInputB) / MaxBuildSizeX) * (MaxBuildSizeX / 192) * 1.2f);
            }
        }

        public float TrapeziumCorrectionFactorY
        {
            get
            {
                if (this is AtumDLPStation5 || this is LoctiteV10)
                {
                    return (float)(Math.Min(this.TrapeziumCorrectionInputA, this.TrapeziumCorrectionInputC) / MaxBuildSizeY) * (MaxBuildSizeY / 108) * 1.35f;
                }
                else
                {
                    return (float)(Math.Min(this.TrapeziumCorrectionInputA, this.TrapeziumCorrectionInputC) / MaxBuildSizeY) * (MaxBuildSizeY / 120) * 1.2f;
                }
            }
        }

        #endregion

        //#region Warp Correction


        //public LensWarpingCorrectionValues LensWarpingCorrection { get; set; }

        //#endregion

        private PrinterXYResolutionType _printerXYResolution;
        public PrinterXYResolutionType PrinterXYResolution
        {
            get
            {
                if (this._printerXYResolution == PrinterXYResolutionType.Unknown)
                {
                    if ((double)this.TrapeziumCorrectionInputD > 66.0 && (double)this.TrapeziumCorrectionInputD < 95.0)
                    {
                        this._printerXYResolution = PrinterXYResolutionType.Micron50;
                    }
                    else if ((double)this.TrapeziumCorrectionInputD >= 95.0 && (double)this.TrapeziumCorrectionInputD < 135.0)
                    {
                        this._printerXYResolution = PrinterXYResolutionType.Micron75;
                    }
                    else if ((double)this.TrapeziumCorrectionInputD >= 135.0 && (double)this.TrapeziumCorrectionInputD < 168.0)
                    {
                        this._printerXYResolution = PrinterXYResolutionType.Micron100;
                    }
                    else
                    {
                        this._printerXYResolution = PrinterXYResolutionType.Micron40;
                    }
                }

                return this._printerXYResolution;
            }
            set
            {
                this._printerXYResolution = value;
            }
        }

        public void SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType resolution)
        {
            this._printerXYResolution = resolution;

            if (this is AtumDLPStation5 || this is LoctiteV10)
            {
                switch (resolution)
                {
                    case PrinterXYResolutionType.Micron100:
                        this.TrapeziumCorrectionInputA = 80;
                        this.TrapeziumCorrectionInputB = 160;
                        this.TrapeziumCorrectionInputC = 80;
                        this.TrapeziumCorrectionInputD = 160;
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 178.8854f;
                        this.TrapeziumCorrectionSideA = 108;
                        this.TrapeziumCorrectionSideB = 192;
                        this.TrapeziumCorrectionSideC = 108;
                        this.TrapeziumCorrectionSideD = 192;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 220.2907f;
                        break;
                    case PrinterXYResolutionType.Micron75:

                        this.TrapeziumCorrectionInputA = 60f;
                        this.TrapeziumCorrectionInputB = 120;
                        this.TrapeziumCorrectionInputC = 60f;
                        this.TrapeziumCorrectionInputD = 120;
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 134.1640f;

                        this.TrapeziumCorrectionSideA = 81;
                        this.TrapeziumCorrectionSideB = 144f;
                        this.TrapeziumCorrectionSideC = 81;
                        this.TrapeziumCorrectionSideD = 144f;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 165.218f;
                        break;
                    case PrinterXYResolutionType.Micron50:
                        this.TrapeziumCorrectionInputA = 40;
                        this.TrapeziumCorrectionInputB = 80;
                        this.TrapeziumCorrectionInputC = 40;
                        this.TrapeziumCorrectionInputD = 80;
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 89.4427f;
                        this.TrapeziumCorrectionSideA = 54;
                        this.TrapeziumCorrectionSideB = 96;
                        this.TrapeziumCorrectionSideC = 54;
                        this.TrapeziumCorrectionSideD = 96;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 110.1453f;
                        break;
                }
            }
            else
            {
                switch (resolution)
                {
                    case PrinterXYResolutionType.Micron100:
                        this.TrapeziumCorrectionInputA = 100;
                        this.TrapeziumCorrectionInputB = 160;
                        this.TrapeziumCorrectionInputC = 100;
                        this.TrapeziumCorrectionInputD = 160;
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 188.6796f;
                        this.TrapeziumCorrectionSideA = 120;
                        this.TrapeziumCorrectionSideB = 192;
                        this.TrapeziumCorrectionSideC = 120;
                        this.TrapeziumCorrectionSideD = 192;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 226.4155f;
                        break;
                    case PrinterXYResolutionType.Micron75:

                        this.TrapeziumCorrectionInputA = 75; //60
                        this.TrapeziumCorrectionInputB = 120; //96
                        this.TrapeziumCorrectionInputC = 75; //60
                        this.TrapeziumCorrectionInputD = 120; //96
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 141.50971f; //113.2077f

                        this.TrapeziumCorrectionSideA = 90;
                        this.TrapeziumCorrectionSideB = 144f;
                        this.TrapeziumCorrectionSideC = 90;
                        this.TrapeziumCorrectionSideD = 144f;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 169.8116f;
                        break;
                    case PrinterXYResolutionType.Micron50:
                        this.TrapeziumCorrectionInputA = 50;
                        this.TrapeziumCorrectionInputB = 80;
                        this.TrapeziumCorrectionInputC = 50;
                        this.TrapeziumCorrectionInputD = 80;
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 94.3398f;
                        this.TrapeziumCorrectionSideA = 60;
                        this.TrapeziumCorrectionSideB = 96;
                        this.TrapeziumCorrectionSideC = 60;
                        this.TrapeziumCorrectionSideD = 96;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 113.2077f;
                        break;
                    case PrinterXYResolutionType.Micron40:
                        this.TrapeziumCorrectionInputA = 40;
                        this.TrapeziumCorrectionInputB = 64;
                        this.TrapeziumCorrectionInputC = 40;
                        this.TrapeziumCorrectionInputD = 64;
                        this.TrapeziumCorrectionInputE = this.TrapeziumCorrectionInputF = 75.4718f;
                        this.TrapeziumCorrectionSideA = 48;
                        this.TrapeziumCorrectionSideB = 76.8f;
                        this.TrapeziumCorrectionSideC = 48;
                        this.TrapeziumCorrectionSideD = 76.8f;
                        this.TrapeziumCorrectionSideE = this.TrapeziumCorrectionSideF = 90.5662f;
                        break;

                }
            }
        }

        public int PrinterXYResolutionAsInt
        {
            get
            {
                var selectedPrinterResolution = 50;
                switch (this.PrinterXYResolution)
                {
                    case PrinterXYResolutionType.Micron50:
                        break;
                    case PrinterXYResolutionType.Micron75:
                        selectedPrinterResolution = 75;
                        break;
                    case PrinterXYResolutionType.Micron100:
                        selectedPrinterResolution = 100;
                        break;
                }

                return selectedPrinterResolution;

            }
        }

        public int ProjectorTurnOffDelay { get; set; }
        public int ProjectorTurnOnDelay { get; set; }

        public AtumPrinterPropertyList Properties { get; set; }


        private void NotifyPropertyChanged(String info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }



        public NetworkConnections Connections { get; set; }
        public AtumPrinter()
        {
            this.Connections = new Managers.NetworkConnections();
            this.Description = string.Empty;
            //this.CorrectionFactorX = 1f;
            //this.CorrectionFactorY = 1f;
            this.Projectors = new Projectors();
            //this.LensWarpingCorrection = new LensWarpingCorrectionValues();
            this.PreviousTrapeziumCorrections = new List<TrapeziumCorrection>();

        }

        public virtual void AtumPrinter_Loaded()
        {
        }



        public void GenerateProjectorLightFields()
        {
            this._projectorLightFields = new SortedDictionary<int, List<int>>();

            foreach (var projector in this.Projectors)
            {
                for (int projectorLightFieldIndex = 0; projectorLightFieldIndex < projector.LightFieldCalibration.Count; projectorLightFieldIndex++)
                {
                    if (!this._projectorLightFields.ContainsKey(projector.LightFieldCalibration[projectorLightFieldIndex]))
                    {
                        this._projectorLightFields.Add(projector.LightFieldCalibration[projectorLightFieldIndex], new List<int>());
                    }

                    this._projectorLightFields[projector.LightFieldCalibration[projectorLightFieldIndex]].Add(projectorLightFieldIndex);
                }
            }
        }

        public virtual void CreateProperties() { }

        public virtual void CreateProjectors() { }

        [XmlIgnore]
        public SortedDictionary<int, List<int>> ProjectorLightFieldCalibration
        {
            get
            {
                return this._projectorLightFields;
            }
        }

        public string FirstIPAddress
        {
            get
            {
                if (this.Connections.Count > 0)
                {
                    return this.Connections[0].IPAddress.Split(';')[0];
                }
                else
                {
                    return null;
                }
            }
        }

        public string FirstMACAddress
        {
            get
            {
                if (this.Connections.Count > 0)
                {
                    return this.Connections[0].MacAddress.Split(';')[0];
                }
                else
                {
                    return null;
                }
            }
        }


        public bool DetectConnection()
        {
            foreach (var nic in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
            {
                try
                {
                    var connection = new Managers.NetworkConnection();
                    foreach (var ip in nic.GetIPProperties().UnicastAddresses)
                    {
                        Debug.WriteLine(nic.Description);
                        Debug.WriteLine(ip.Address.ToString());
                        if (ip.Address.ToString() != "::1" && ip.Address.ToString() != "127.0.0.1")
                        {
                            connection.IPAddress += ip.Address + ";";
                            //connection.SubnetMask += ip.IPv4Mask + ";";
                            Debug.WriteLine(nic.Description);
                        }
                    }

                    foreach (var gateway in nic.GetIPProperties().GatewayAddresses)
                    {
                        if (gateway.Address.ToString() != "::1" && gateway.Address.ToString() != "127.0.0.1")
                        {
                            connection.Gateway += gateway.Address + ";";
                        }
                    }

                    if (connection.IPAddress.EndsWith(";")) connection.IPAddress = connection.IPAddress.Substring(0, connection.IPAddress.Length - 1);
                    if (connection.SubnetMask.EndsWith(";")) connection.SubnetMask = connection.SubnetMask.Substring(0, connection.SubnetMask.Length - 1);
                    if (connection.Gateway.EndsWith(";")) connection.Gateway = connection.Gateway.Substring(0, connection.Gateway.Length - 1);
                    connection.MacAddress = nic.GetPhysicalAddress().ToString();
                    connection.ConnectionType = Managers.NetworkConnection.typeConnection.LAN;

                    if (!String.IsNullOrEmpty(connection.IPAddress))
                    {
                        this.Connections.Add(connection);
                    }
                }
                catch (Exception exc)
                {
                    LoggingManager.WriteToLog("DetectConnection", "Exception", exc.Message);
                }
            }
            Debug.WriteLine("test5");
            return true;
        }

        public string DisplayText
        {
            get
            {
                var displayText = ToString().Replace(" (*)", string.Empty);
                return displayText;
            }
        }

        public override string ToString()
        {
            return this.DisplayName.Trim();
        }

        public string ToSerialString()
        {
            var result = string.Empty;
            result += this.Microsteps.ToString() + ";";
            result += this.Steps.ToString() + ";";
            result += this.SpindleRotation.ToString() + ";";
            return result;
        }

        private AtumPrinter _preserved;

        public void CreatePreserved()
        {
            this._preserved = Utils.Toolbox.DeepClone(this);
        }

        public void RevertChanges()
        {
            this.TrapeziumCorrectionSideA = this._preserved.TrapeziumCorrectionSideA;
            this.TrapeziumCorrectionSideB = this._preserved.TrapeziumCorrectionSideB;
            this.TrapeziumCorrectionSideC = this._preserved.TrapeziumCorrectionSideC;
            this.TrapeziumCorrectionSideD = this._preserved.TrapeziumCorrectionSideD;
            this.TrapeziumCorrectionSideE = this._preserved.TrapeziumCorrectionSideE;
            this.TrapeziumCorrectionSideF = this._preserved.TrapeziumCorrectionSideF;
        }

    }
    [Serializable]
    public class AtumPrinterProperty
    {
        public enum typePrinterProperty
        {
            Unknown = 0,
            TypeOfSpindle = 1
        }

        public string Name { get; set; }
        public typePrinterProperty Type { get; set; }
        public AtumPrinterPropertyValueList Values { get; set; }

        public AtumPrinterProperty()
        {
        }

        public AtumPrinterProperty(typePrinterProperty type)
        {
            this.Values = new AtumPrinterPropertyValueList();
            this.Type = type;
        }

    }

    [Serializable]
    public class AtumPrinterPropertyList : List<AtumPrinterProperty>
    {
    }

    [Serializable]
    public class AtumPrinterPropertyValue
    {
        public string Text { get; set; }
        public object Value { get; set; }
        public bool Selected { get; set; }
    }

    [Serializable]
    public class AtumPrinterPropertyValueList : List<AtumPrinterPropertyValue>
    {
    }

    [Serializable]
    public class PrinterFirmware
    {
        public enum PrinterHardwareType
        {
            AtumV15 = 0,
            AtumV10TD = 1,
        }

        public string FirmwareVersion { get; set; }
        public string HardwareType { get; set; }
        public string DALVersion { get; set; }

        public PrinterHardwareType PrinterHardware
        {
            get
            {
                switch (this.HardwareType)
                {
                    case "{289EF7C2-3342-4EA3-AF7D-883B26454318}":
                        return PrinterHardwareType.AtumV10TD;
                    case "{967BD0EC-35C1-436A-8D92-145823F17F6E}":
                        return PrinterHardwareType.AtumV15;
                    case "{57E748B0-EB49-4A59-AEB3-73B2A973D55F}":
                        return PrinterHardwareType.AtumV15;
                }
                return PrinterHardwareType.AtumV15; ;
            }
        }

        public AtumPrinter Printer { get; set; }

    }

    [Serializable]
    public class PrinterFirmwareResult
    {
        public string FirmwareVersion { get; set; }
        public string HardwareType { get; set; }
        public string DALVersion { get; set; }
        public AtumPrinter Printer { get; set; }
    }
}
