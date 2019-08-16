using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Controls;

namespace Atum.Studio.Core.Events
{
    public class RenderSliceCompletedEventArgs: EventArgs
    {
        internal long TotalProcessedSlices { get; set; }
        internal long TotalAmountSlices { get; set; }
    }
}
