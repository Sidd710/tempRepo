using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Shapes
{
    [Serializable]
    public class TriangleIntersection: Triangle
    {
        public Vector3Class IntersectionPoint;

        public TriangleIntersection()
        {

        }

        public TriangleIntersection(Triangle triangle, Vector3Class intersectionPoint)
        {
            this.Normal = triangle.Normal;
            this.Vectors = triangle.Vectors;
            this.Index = triangle.Index;
            this.IntersectionPoint = intersectionPoint;
        }

    }
}
