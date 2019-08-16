using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Shapes
{
    internal class SlicePoint2D : IComparable
    {
        internal float X, Y;
        internal Vector3Class Normal { get; set; }
        internal SliceLine2D ParentLine { get; set; }
        internal TriangleConnectionInfo TriangleConnection { get; set; }

        internal SlicePoint2D()
        {

        }

        internal SlicePoint2D(IntPoint intPoint)
        {
            this.X = intPoint.AsVector3().X;
            this.Y = intPoint.AsVector3().Y;
        }

        internal int XasPx
        {
            get
            {
                return (int)Math.Round(this.X, 0);
            }
        }

        internal int YasPx
        {
            get
            {
                return (int)Math.Round(this.Y, 0);
            }
        }

        int IComparable.CompareTo(object obj)
        {
            var p = (SlicePoint2D)obj;
            if (p.X > X)
            {
                return -1;
            }
            else if (p.X < X)
            {
                return 1;
            }
            else
            {
                    return 0;
            }
        }

        public override bool Equals(object obj)
        {
            var p = (SlicePoint2D)obj;
            return (p.X == this.X && p.Y == this.Y);

        }

    }
}
