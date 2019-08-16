using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Models;
using OpenTK;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Utils
{
    internal class ISectData : IComparable
    {
        internal ISectData(object o, Triangle p, Vector3Class isect, Vector3Class orgin, Vector3Class dir) 
        {
            intersect = new Vector3Class(isect);
            origin = new Vector3Class(orgin);
            direction = new Vector3Class(dir);
            poly = p;
        }

        public int CompareTo(object c)
        {
            ISectData c1 = this;
            ISectData c2 = (ISectData)c;
            double d1 = (c1.origin.X - c1.intersect.X) * (c1.origin.X - c1.intersect.X) +
                        (c1.origin.Y - c1.intersect.Y) * (c1.origin.Y - c1.intersect.Y) +
                        (c1.origin.Z - c1.intersect.Z) * (c1.origin.Z - c1.intersect.Z);

            double d2 = (c2.origin.X - c2.intersect.X) * (c2.origin.X - c2.intersect.X) +
                        (c2.origin.Y - c2.intersect.Y) * (c2.origin.Y - c2.intersect.Y) +
                        (c2.origin.Z - c2.intersect.Z) * (c2.origin.Z - c2.intersect.Z);

            if (d1 > d2)
                return 1;
            if (d1 < d2)
                return -1;
            else
                return 0;
        }

        public Vector3Class origin;
        public Vector3Class direction;

        public Triangle poly; // the polygon that was intersected - the surface normal can be obtained here
        public Vector3Class intersect; // the intersection point
    }
}
