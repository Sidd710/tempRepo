using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonLineSegment
    {
        public Vector2 P { get; set; }
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }
        public Vector2 V { get; set; }
        public float Length
        {
            get
            {
                return this.V.Length;
            }
        }

        public SkeletonLineSegment(Vector2 p1, Vector2 p2)
        {
            this.P = this.P1 = p1;
            this.P2 = p2;
            this.V = p2 - p1;
        }

        public bool _u_in(float u)
        {
            return u >= 0.0 && u <= 1.0;
        }
    }
}
