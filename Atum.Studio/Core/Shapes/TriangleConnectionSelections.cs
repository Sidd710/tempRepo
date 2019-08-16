using Atum.Studio.Core.Structs;
using OpenTK;
using System.Collections.Generic;

namespace Atum.Studio.Core.Shapes
{
    class TriangleConnectionSelections: Dictionary<TriangleConnectionInfo, bool>
    {
        internal Vector3Class CalcAvgNormal(TriangleInfoList triangles)
        {
            var avgNormal = new Vector3Class();
            foreach (var triangleConnection in this.Keys)
            {
                avgNormal += triangles[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex].Normal;
            }

            avgNormal /= this.Count;

            return avgNormal;
        }
    }
}
