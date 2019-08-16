using Atum.Studio.Core.Objects;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Helpers
{
    class MathHelper
    {
        public static float Abs(PointF p)
        {
            return (float)Math.Sqrt(Math.Pow(p.X, 2) + Math.Pow(p.Y, 2));
        }

        public static float Abs(Vector2 v)
        {
            return (float)Math.Sqrt(Math.Pow(v.X, 2) + Math.Pow(v.Y, 2));
        }

        //public static float Cross(PointF p1, PointF p2)
        //{
        //    return p1.X * p2.Y - p2.X * p1.Y;
        //}

        //public static float Cross(Vector2 p1, Vector2 p2)
        //{
        //    return p1.X * p2.Y - p2.X * p1.Y;
        //}

        //public static SkeletonLineSegment ConnectLineToPoint(SkeletonLine2 l, Vector2 p)
        //{
        //    var d = l.V.LengthSquared;

        //    var u = ((p.X - l.P.X) * l.V.X + (p.Y - l.P.Y) * l.V.Y) / d;
        //    if (!l._u_in(u))
        //    {
        //        u = (float)Math.Max(Math.Min(u, 1.0), 0.0);
        //    }
        //    return new SkeletonLineSegment(p,
        //                        new Vector2(l.P.X + u * l.V.X,
        //                               l.P.Y + u * l.V.Y));
        //}

        public static float Distance(Vector2 a, Vector2 b)
        {
            return (a - b).Length;
        }

        internal static Vector2 GetIntersectionPoint(Vector2 A, Vector2 B, Vector2 C, Vector2 D)
        {
            var dy1 = B.Y - A.Y;
            var dx1 = B.X - A.X;
            var dy2 = D.Y - C.Y;
            var dx2 = D.X - C.X;
            var p = new Vector2();
            if (dy1 * dx2 == dy2 * dx1)
            {
                Console.WriteLine("Parallel");
                return p;
            }
            else
            {
                var x = ((C.Y - A.Y) * dx1 * dx2 + dy1 * dx2 * A.X - dy2 * dx1 * C.X) / (dy1 * dx2 - dy2 * dx1);

                var y = 0f;
                if (dx1 == 0)
                {
                    y = C.Y + ((x - C.X) / dx2) * dy2;
                }
                else
                {
                    y = A.Y + (dy1 / dx1) * (x - A.X);
                }
                p = new Vector2(x, y);
                return p;
            }
        }

        public static float Max(params float[] values)
        {
            return values.Max();
        }

        public static float Min(params float[] values)
        {
            return values.Min();
        }
    }
}
