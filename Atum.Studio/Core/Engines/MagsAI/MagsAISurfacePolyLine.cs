using Atum.DAL.Materials;
using Atum.Studio.Core.Structs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    public class MagsAISurfacePolyLine
    {
        public Vector3Class Point { get; set; }
        public List<Vector3Class> Normals { get; set; }

        public MagsAISurfacePolyLine(Vector3Class point)
        {
            this.Point = point;
            this.Normals = new List<Vector3Class>();
        }
    }

    public class MagsAISurfacePolyLines: List<MagsAISurfacePolyLine>
    {
        public bool ClosedPath { get; set; }

        public MagsAISurfacePolyLines()
        {
            this.ClosedPath = true;
        }

        public List<List<IntPoint>> ToPolyPaths(SupportProfile selectedMaterialProfile)
        {
            var polyPaths = new List<List<IntPoint>>();
            var polygonPath = new List<IntPoint>();
            //first pass add normal points as intpoint
            foreach(var surfaceLine in this)
            {
                polygonPath.Add(new IntPoint(surfaceLine.Point));
            }

            polygonPath.Add(new IntPoint(this[0].Point));

            //if (this.ClosedPath)
            //{
            //    polygonPath.Add(new IntPoint(this[0].Point));
            //    polyNode.m_polygon = polygonPath;
            //    polyNodes.Add(polyNode);
            //    polyNode = new PolyNode();
            //    polygonPath = new List<IntPoint>();
            //}

            ////second pass reverse points and append avg normal
            //for(var surfaceLineIndex = this.Count - 1;surfaceLineIndex >= 0;surfaceLineIndex--)
            //{
            //    var avgLineNormal = new Vector3Class();
            //    foreach(var normal in this[surfaceLineIndex].Normals)
            //    {
            //        avgLineNormal += normal;
            //    }

            //    avgLineNormal.Z = 0;
            //    avgLineNormal = avgLineNormal.Normalized();

            //    polygonPath.Add(new IntPoint(this[surfaceLineIndex].Point + (avgLineNormal * selectedMaterialProfile.SupportOverhangDistance)));
            //}

            if (polygonPath.Count > 0)
            {
                polyPaths.Add(polygonPath);
            }
            return polyPaths;
        }
    }
}
