using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAIEdgePoints :List<MagsAIEdgePoint>
    {
        internal bool AreaToSmallToOffset { get; set; }
    }
}
