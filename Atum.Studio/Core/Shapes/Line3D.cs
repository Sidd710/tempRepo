using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Shapes
{
    public class Line3D
    {
        public Vector3Class StartPoint { get; set; }
        public Vector3Class EndPoint { get; set; }
        public Vector3Class Normal { get; set; }
        internal TriangleConnectionInfo TriangleConnection { get; set; }

        public Line3D(Vector3Class startPoint, Vector3Class endPoint, Vector3Class normal)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;
            this.Normal = normal;
        }

        public override string ToString()
        {
            return string.Format("{0} -> {1}", this.StartPoint, this.EndPoint);
        }
    }
}
