using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Core.Structs;
using OpenTK;

namespace Atum.Studio.Core.Shapes
{
    internal class SliceLine3D
    {
        private Vector3Class _point1;
        private Vector3Class _point2;

        internal SliceLine3D(Vector3Class point1, Vector3Class point2)
        {
            this._point1 = point1;
            this._point2 = point2;
        }

        internal Vector3Class IntersectZ(float zcur)
        {
            try
            {

                //if both points are above or below it, return nothing
                if ((this._point1.Z > zcur && this._point2.Z > zcur) || (this._point1.Z < zcur && this._point2.Z < zcur))
                {
                    return null;// no intersection
                }

                // if both points are on the line
                if (this._point2.Z == zcur && this._point1.Z == zcur)
                    return null;

                // if one points z cordinate equals the z level:
                if (this._point1.Z == zcur || this._point2.Z == zcur)
                {
                    // if the other point is below, return nothing
                    if ((this._point1.Z == zcur && this._point2.Z < zcur) || (this._point2.Z == zcur && this._point1.Z < zcur))
                    {
                        return null;
                    }
                    // if the other point is above, return the first point
                    if (this._point1.Z == zcur && this._point2.Z > zcur)
                        return this._point1;
                    if (this._point2.Z == zcur && this._point1.Z > zcur)
                        return this._point2;
                }


                var p3d = new Vector3Class();
                var minz = Math.Min(this._point1.Z, this._point2.Z);
                var maxz = Math.Max(this._point1.Z, this._point2.Z);

                Vector3Class pmin;
                Vector3Class pmax;
                if (this._point1.Z < this._point2.Z) // find the point with the min z
                {
                    pmin = this._point1;
                    pmax = this._point2;
                }
                else
                {
                    pmin = this._point2;
                    pmax = this._point1;
                }

                float zrange = maxz - minz;// the range of the z coord
                float scale = ((zcur - minz) / zrange);
                p3d.Z = zcur; // set to the current z
                //p3d.x = LERP(pmin.x, pmax.x, scale); // do the intersection
                //p3d.y = LERP(pmin.y, pmax.y, scale);
                p3d.X = (pmax.X - pmin.X) * scale + pmin.X;
                p3d.Y = (pmax.Y - pmin.Y) * scale + pmin.Y;

                return p3d;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
