using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class LowestPointPolygon
    {
        internal PolyNode Polygon = new PolyNode();
        internal List<MagsAIIntersectionPointWithFilter> LowestPoints = new List<MagsAIIntersectionPointWithFilter>();
        internal List<MagsAIIntersectionData> LowestIntersectionPoints = new List<MagsAIIntersectionData>();
        internal Dictionary<float, List<MagsAIIntersectionPointWithFilter>> LowestPointsWithOffset = new Dictionary<float, List<MagsAIIntersectionPointWithFilter>>();
        internal Dictionary<float, List<MagsAIIntersectionData>> LowestPointsWithOffsetIntersectionPoint = new Dictionary<float, List<MagsAIIntersectionData>>();
        internal List<MagsAIEdgePoint> EdgePoints = new List<MagsAIEdgePoint>();
        internal List<MagsAIIntersectionPointWithFilter> LayerBasedOverhangPoints = new List<MagsAIIntersectionPointWithFilter>();
        internal List<MagsAIIntersectionData> EdgeIntersectionPoints = new List<MagsAIIntersectionData>();
        internal PolyTree SupportedContours = new PolyTree();
        internal int SliceIndex;
        internal bool HasEdgeDown;
        internal bool HasComplexConnectedUpperChildContour;
        internal float SliceHeight;

        internal float LowestIntersectionPointsTop
        {
            get
            {
                var result = -1f;
                foreach (var lowestPoint in this.LowestIntersectionPoints)
                {
                    if (lowestPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        if (lowestPoint.TopPoint.Z > result)
                        {
                            result = lowestPoint.TopPoint.Z;
                        }
                    }
                }

                return result;
            }
        }

        internal IntPoint FindNearestLowestPoint(IntPoint modelIntersectionPoint)
        {
            var lowestPointDistance = float.MaxValue;
            var lowestPointNearest = new IntPoint();

            foreach (var lowestPoint in this.LowestPoints)
            {
                var distance = (lowestPoint.Point.XY - modelIntersectionPoint.XY).Length;
                if (distance < lowestPointDistance)
                {
                    lowestPointDistance = distance;
                    lowestPointNearest = lowestPoint.Point;
                }
            }

            return lowestPointNearest;
        }

        internal void UpdateSupportedContour(Material selectedMaterial, int index)
        {
            var polyTreeSupportedContours = new PolyTree();

            foreach (var intersectionSupportConeOutlinePoint in this.LowestPoints)
            {
                var lowestPointPolyNode = new PolyNode();
                var intersectionPointsSupportConeOutline = intersectionSupportConeOutlinePoint.CalcOutlinePoints(selectedMaterial);
                
                foreach (var intersectionPointSupportConeOutline in intersectionPointsSupportConeOutline)
                {
                    lowestPointPolyNode.m_polygon.Add(new IntPoint(intersectionPointSupportConeOutline));
                }
                polyTreeSupportedContours._allPolys.Add(lowestPointPolyNode);
            }

            foreach (var edgeSupportConeOutlinePoint in this.EdgePoints)
            {
                var lowestPointPolyNode = new PolyNode();
                var edgePointsSupportConeOutline = edgeSupportConeOutlinePoint.CalcOutlinePointsAsVector3(selectedMaterial);

                foreach (var edgePointSupportConeOutline in edgePointsSupportConeOutline)
                {
                    lowestPointPolyNode.m_polygon.Add(new IntPoint(edgePointSupportConeOutline));
                }
                polyTreeSupportedContours._allPolys.Add(lowestPointPolyNode);
            }

            polyTreeSupportedContours = UnionModelSliceLayer(new PolyTree(), polyTreeSupportedContours);
            //TriangleHelper.SavePolyNodesContourToPng(polyTreeSupportedContours._allPolys, "t" + index);
            var p = new PolyTree();
            p._allPolys.Add(this.Polygon);
            this.SupportedContours = IntersectModelSliceLayer(p, polyTreeSupportedContours);

            //TriangleHelper.SavePolyNodesContourToPng(this.SupportedContours._allPolys, "S" + index);
        }
    }

    internal class LowestPointsPolygons: List<LowestPointPolygon>
    {
        internal void UpdateSupportedContours(Material selectedMaterial)
        {
            var index = 0;
            foreach(var lowestPolygon in this)
            {
                lowestPolygon.UpdateSupportedContour(selectedMaterial, index);
                index++;
            }
        }
    }
}
