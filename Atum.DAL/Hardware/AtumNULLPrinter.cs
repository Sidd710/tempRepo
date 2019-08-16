using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class AtumNULLPrinter: AtumPrinter
    {
        public AtumNULLPrinter()
        {
            this.Connections = new Managers.NetworkConnections();

            //this.BuildPlatformSizeY = 100;
            //this.BuildPlatformSizeX = 200;
            this.DisplayName = "NULL Printer";
            this.PrinterHardwareType = "{D565EAF9-BE2D-404C-A2E5-151AC086B2BD}";

        }
    }
}
