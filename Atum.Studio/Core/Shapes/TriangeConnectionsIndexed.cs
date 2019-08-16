using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Shapes
{
    [Serializable]
    internal class TriangeConnectionsIndexed : Dictionary<TriangleConnectionInfo, bool>
    {
        internal void Append(TriangleConnectionInfo triangleConnection)
        {
            if (!this.ContainsKey(triangleConnection))
            {
                this.Add(triangleConnection, false);
            }
        }

    }
}
