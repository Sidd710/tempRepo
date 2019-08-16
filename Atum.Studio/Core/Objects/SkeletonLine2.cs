using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonLine2
    {
            public Vector2 P { get; set; }
            public Vector2 V { get; set; }

            public SkeletonLine2(SkeletonLineSegment lineSegment)
            {
                this.P = lineSegment.P;
                this.V = lineSegment.V;
            }

            public SkeletonLine2(Vector2 p, Vector2 v)
            {
                this.P = p;
                this.V = v;
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

                return other.P + ua * other.V;
            }

            public Vector2 Intersect(SkeletonLineSegment other, bool rayline = false)
            {
                var d = Helpers.MathHelper.Cross(other.V, this.V);
                if (d == 0)
                {
                    return Vector2.Zero;
                }

                var d2 = other.P - this.P;
                var ua = Helpers.MathHelper.Cross(this.V, d2) / d;
                if (rayline)
                {
                    if (!other._u_in(ua))
                    {
                        return Vector2.Zero;
                    }
                }
                var ub = Helpers.MathHelper.Cross(other.V, d2) / d;

                return other.P + ua * other.V;

            }

            public bool _u_in(float u)
            {
                return true;
            }

            public float SqrMagnitude(Vector2 v)
            {
                return v.X * v.X + v.Y * v.Y;
            }

            public SkeletonLineSegment ConnectPoint2Line2(Vector2 p, SkeletonLineSegment l)
            {
                var d = SqrMagnitude(l.V);
                //assert d != 0;
                var u = ((p.X - l.P.X) * l.V.X + (p.Y - l.P.Y) * l.V.Y) / d;
                if (l._u_in(u))
                {
                    u = (float)Math.Max(Math.Min(u, 1.0), 0.0);
                }
                return new SkeletonLineSegment(P, l.P + u * l.V);
            }

            public float Distance(Vector2 a)
            {
                var d = SqrMagnitude(this.V); //magnitudesqrt
                if (d != 0)
                {
                    var u = ((a.X - this.P.X) * this.V.X + (a.Y - this.P.Y) * this.V.Y) / d;
                    //# Parallel, connect an endpoint with a line
                    var t = Helpers.MathHelper.ConnectLineToPoint(this, a);

                    if (!this._u_in(u))
                    {
                        u = (float)Math.Max(Math.Min(u, 1.0), 0.0);
                        t = new SkeletonLineSegment(a, this.P + u * this.V);
                    }

                    //swap
                    var oldP = t.P;
                    t.P = t.P1 = t.P2;
                    t.P2 = oldP;
                    t.V *= -1;

                    return t.Length;
                }

                return 0f;
        }
    }
}
