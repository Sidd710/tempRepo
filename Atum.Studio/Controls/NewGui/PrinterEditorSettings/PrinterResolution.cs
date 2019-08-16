using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.DAL.Hardware.AtumPrinter;

namespace Atum.Studio.Controls.NewGui.PrinterEditorSettings
{
    public class PrinterResolution
    {
        public string Text { get; set; }
        public PrinterXYResolutionType Value { get; set; }
    }
}
