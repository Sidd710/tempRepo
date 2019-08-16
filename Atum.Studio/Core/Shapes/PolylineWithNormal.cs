using Atum.Studio.Core.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Shapes
{
    public class PolylineWithNormal
    {
        public IntPoint Point { get; set; }
        public Vector3Class Normal { get; set; }
        public float AngleZ { get; set; }

        public Vector3Class PointAsVector
        {
            get
            {
                return this.Point.AsVector3();
            }
        }
    }
}
