using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Shapes
{
    [Serializable]
    public class TriangleWithIndex : Triangle
    {

        public TriangleConnectionInfo TriangleIndex { get; set; }

        public Triangle BaseTriangle
        {
            get
            {
                return (Triangle)this.Clone();
            }
        }

        public TriangleWithIndex()
        {

        }

        public TriangleWithIndex(Triangle triangle)
        {
            this.Vectors = triangle.Vectors;
            this.TriangleIndex = triangle.Index;
            this.AngleZ = triangle.AngleZ;
            this.Normal = triangle.Normal;
        }
    }
}
