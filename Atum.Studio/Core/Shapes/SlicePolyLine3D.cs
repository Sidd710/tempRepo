using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Core.Structs;
using OpenTK;

namespace Atum.Studio.Core.Shapes
{
    public class SlicePolyLine3D
    {
        internal Vector3Class Normal { get; set; }
        internal List<Vector3Class> Points { get; set; }
        internal TriangleConnectionInfo TriangleConnection { get; set; }

        internal SlicePolyLine3D()
        {
            this.Points = new List<Vector3Class>();
        }
    }
}
