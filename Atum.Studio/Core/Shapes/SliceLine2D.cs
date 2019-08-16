using Atum.Studio.Core.Structs;
using OpenTK;
using System;

namespace Atum.Studio.Core.Shapes
{
    internal class SliceLine2D
    {
        public SlicePoint2D p1, p2;
        public Vector3Class Normal { get; set; }

        public SliceLine2D() 
        {
            this.p1 = new SlicePoint2D();
            this.p2 = new SlicePoint2D();
        }

        public SlicePoint2D IntersectX(int xpos)
        {
            var pnt = new SlicePoint2D();
            double minx, maxx;
            double miny, maxy;
            double xp = xpos;
            minx = (double)Math.Min(p1.X, p2.X);
            maxx = (double)Math.Max(p1.X, p2.X);
            miny = (double)Math.Min(p1.Y, p2.Y);
            maxy = (double)Math.Max(p1.Y, p2.Y);
            double xrange = maxx - minx;// the range of the x coord
            double scale = (double)((xp - minx) / xrange);
            //pnt.x = (int)LERP(minx, maxx, scale);
            SlicePoint2D pmin, pmax;
            if (p1.X < p2.X) // find the point with the min x
            {
                pmin = p1;
                pmax = p2;
            }
            else
            {
                pmin = p2;
                pmax = p1;

            }
            pnt.X = xpos;
            pnt.Y = (float)LERP(pmin.Y, pmax.Y, scale);
            pnt.Normal = this.Normal;
            return pnt;

        }

        public SlicePoint2D IntersectY(int ypos) 
        {
            var pnt = new SlicePoint2D();
            double minx, maxx;
            double miny, maxy;
            double yp = ypos;
            minx = (double)Math.Min(p1.X, p2.X);
            maxx = (double)Math.Max(p1.X, p2.X);
            miny = (double)Math.Min(p1.Y, p2.Y);
            maxy = (double)Math.Max(p1.Y, p2.Y);
            double yrange = maxy - miny;// the range of the x coord
            double scale = (double)((yp - miny) / yrange);
            //pnt.x = (int)LERP(minx, maxx, scale);
            SlicePoint2D pmin, pmax;
            if (p1.Y < p2.Y) // find the point with the min y
            {
                pmin = p1;
                pmax = p2;
            }
            else
            {
                pmin = p2;
                pmax = p1;

            }
            pnt.X = (float)LERP(pmin.X, pmax.X, scale);
            pnt.Y = ypos;
            pnt.Normal = this.Normal;
            return pnt;
     
        }

        private static double LERP(double a, double b, double c) { return (double)(((b) - (a)) * (c) + (a)); }
    }
}
