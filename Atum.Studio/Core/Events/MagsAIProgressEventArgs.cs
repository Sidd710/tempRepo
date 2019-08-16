using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Events
{
    internal class MagsAIProgressEventArgs: EventArgs
    {
        internal int Percentage { get; set; }
    }
}
