using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonEdge
    {
        public SkeletonLineSegment Edge { get; set; }
        public SkeletonRay2 BiSector_Left { get; set; }
        public SkeletonRay2 BiSector_Right { get; set; }

        public SkeletonEdge(SkeletonLineSegment edge, SkeletonRay2 biSector_Left, SkeletonRay2 biSector_Right)
        {
            this.Edge = edge;
            this.BiSector_Left = biSector_Left;
            this.BiSector_Right = biSector_Right;
        }
    }
}
