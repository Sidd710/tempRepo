using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonRay2
    {
        public Vector2 P { get; set; }
        public Vector2 P1 { get; set; }
        public Vector2 P2 { get; set; }
        public Vector2 V { get; set; }

        public SkeletonRay2(Vector2 p, Vector2 v)
        {
            if (p.X == 208483 && p.Y == 164189)
            {

            }

            this.P = this.P1 = p;
            this.P2 = this.P + v;
            this.V = v;
        }

        public bool _u_in(float u)
        {
            return u >= 0.0;
        }

        public Vector2 Intersect(SkeletonRay2 other)
        {
            var d = Helpers.MathHelper.Cross(other.V, this.V);
            if (d == 0)
            {
                return Vector2.Zero;
            }

            var d2 = other.P - this.P;
            var ua = Helpers.MathHelper.Cross(this.V, d2) / d;

            if (!other._u_in(ua))
            {
                return Vector2.Zero;
            }

            var ub = Helpers.MathHelper.Cross(other.V, d2) / d;
            if (!this._u_in(ub))
            {
                return Vector2.Zero;
            }

            return other.P + ua * other.V;
        }
    }
}
