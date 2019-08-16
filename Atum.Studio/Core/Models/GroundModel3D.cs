using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Core.Shapes;

namespace Atum.Studio.Core.Models
{
    internal class GroundModel3D : DrawableShapeInfo
    {
        internal GroundModel3D()
            : base(false)
        {
        }

        //internal Point3dInfoList Points
        //{
        //    get
        //    {
        //        var points = new Point3dInfoList();
        //        points.Add(new Point3dInfo(-500, -500, 0, 0));
        //        points.Add(new Point3dInfo(500, -500, 0, 0));
        //        points.Add(new Point3dInfo(500, 500, 0, 0));
        //        points.Add(new Point3dInfo(-500, 500, 0, 0));

        //        return points;
        //    }
        //}

        //internal PolygonInfoList Polygons
        //{
        //    get
        //    {
        //        var polygons = new PolygonInfoList();

        //        var points = this.Points;
        //        var polygon1 = new Polygon();
        //        polygon1.m_points = new Point3dInfo[3];
        //        polygon1.m_points[0] = points[0];
        //        polygon1.m_points[1] = points[1];
        //        polygon1.m_points[2] = points[2];

        //        var polygon2 = new Polygon();
        //        polygon2.m_points = new Point3dInfo[3];
        //        polygon2.m_points[0] = points[0];
        //        polygon2.m_points[1] = points[1];
        //        polygon2.m_points[2] = points[2];
        //        polygons.Add(polygon1);
        //        polygons.Add(polygon2);

        //        return polygons;
        //    }
        //}

        //internal Point3dInfo Center
        //{
        //    get
        //    {
        //        return new Point3dInfo();
        //    }
        //}

        //internal double Radius
        //{
        //    get
        //    {
        //        double maxdist = 0.0;
        //        double td = 0.0;
        //        foreach (Point3dInfo p in Points)
        //        {
        //            td = (p.x - 0) * (p.x - 0);
        //            td += (p.y - 0) * (p.y - 0);
        //            td += (p.z - 0) * (p.z - 0);
        //            td = Math.Sqrt(td);
        //            if (td >= maxdist)
        //                maxdist = td;
        //        }
        //        return maxdist;

        //    }
        //}
    }
}
