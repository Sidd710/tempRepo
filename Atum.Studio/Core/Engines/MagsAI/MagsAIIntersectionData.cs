﻿using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAIIntersectionData
    {
        internal Vector3Class TopPoint { get; set; }

        internal Vector3Class BottomPoint { get; set; }
        internal typeOfAutoSupportFilter Filter { get; set; }
        internal float SliceHeight { get; set; }
        internal Color RenderColor { get; set; }
        internal bool AddedToStructure { get; set; }
        internal TriangleIntersection ModelIntersection = new TriangleIntersection();
        internal STLModel3D _trianglesWithinXYRange;
        internal float LastSupportedSliceHeight = float.MinValue;
        //internal Dictionary<float, PolyTree> SupportedContours = new Dictionary<float, PolyTree>();

        internal MagsAISupportConeOutlinePoints CalcOutlinePoints(Material selectedMaterial)
        {
            return new MagsAISupportConeOutlinePoints(VectorHelper.GetCircleOutlinePoints(0, selectedMaterial.SupportProfiles.First().SupportInfillDistance, 26, new Vector3Class(this.TopPoint)));
        }


        internal void UpdateTriangleReference(STLModel3D stlModel, SupportProfile selectedMaterialProfile)
        {
            var supportPointPosition = this.TopPoint;
            supportPointPosition.Z = this.SliceHeight - 1f;

            if (this._trianglesWithinXYRange == null)
            {
                CalcTrianglesWithinRange(supportPointPosition, selectedMaterialProfile, stlModel);
            }
            //find latest supportcone supported height
            TriangleIntersection[] trianglesIntersected = null;

            var trianglesAboveSupportPoint = new STLModel3D() { Triangles = new TriangleInfoList() };
            trianglesAboveSupportPoint.Triangles[0] = this._trianglesWithinXYRange.Triangles[0].Where(s => s.Top > supportPointPosition.Z && s.Normal.Z < 0).ToList();

            IntersectionProvider.IntersectTriangle(supportPointPosition, new Vector3Class(0, 0, 1), trianglesAboveSupportPoint, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
            if (trianglesIntersected != null)
            {
                var nearestLastSupportedHeight = new Vector3Class();
                var nearestLastSupportedHeightDistance = float.MaxValue;
                foreach (var triangleIntersection in trianglesIntersected)
                {
                    if (triangleIntersection != null)
                    {
                        if (triangleIntersection.IntersectionPoint.Z > supportPointPosition.Z)
                        {
                            var distance = triangleIntersection.IntersectionPoint.Z - supportPointPosition.Z;
                            if (distance < nearestLastSupportedHeightDistance)
                            {
                                nearestLastSupportedHeightDistance = distance;
                                nearestLastSupportedHeight = triangleIntersection.IntersectionPoint;

                                this.ModelIntersection = triangleIntersection;
                            }
                        }
                    }
                }
            }
        }

        private void CalcTrianglesWithinRange(Vector3Class supportPointPosition, SupportProfile selectedMaterialProfile, STLModel3D stlModel)
        {

            var boundryOffset = selectedMaterialProfile.SupportOverhangDistance;

            var modelTrianglesLeftBoundry = supportPointPosition.X - boundryOffset;
            var modelTrianglesRightBoundry = supportPointPosition.X + boundryOffset;
            var modelTrianglesBackBoundry = supportPointPosition.Y - boundryOffset;
            var modelTrianglesFrontBoundry = supportPointPosition.Y + boundryOffset;
            var currentSupportConeSupportedLayer = MagsAIEngine.ConvertSupportPointsToCircles(new IntPoint(supportPointPosition), selectedMaterialProfile.SupportOverhangDistance);
            currentSupportConeSupportedLayer.Add(new IntPoint(supportPointPosition));

            this._trianglesWithinXYRange = new STLModel3D
            {
                Triangles = stlModel.Triangles.GetTrianglesWithinXYBoundry(modelTrianglesLeftBoundry, modelTrianglesBackBoundry, modelTrianglesRightBoundry, modelTrianglesFrontBoundry)
            };

        }

        internal void UpdateLastSupportedHeight(STLModel3D stlModel, SupportProfile selectedMaterialProfile, SortedDictionary<float, PolyTree> modelLayers)
        {

            //find latest supportcone supported height

            var supportPointPosition = this.TopPoint;
            supportPointPosition.Z = this.SliceHeight - 1;

            if (this._trianglesWithinXYRange == null)
            {
                CalcTrianglesWithinRange(supportPointPosition, selectedMaterialProfile, stlModel);
            }

            var trianglesAboveSupportPoint = new STLModel3D() { Triangles = new TriangleInfoList() };
            trianglesAboveSupportPoint.Triangles[0] = this._trianglesWithinXYRange.Triangles[0].Where(s => s.Bottom > supportPointPosition.Z && s.Normal.Z > 0).ToList();

            var maxLayerHeight = float.MinValue;
            var currentSupportConeSupportedLayer = MagsAIEngine.ConvertSupportPointsToCircles(new IntPoint(supportPointPosition), selectedMaterialProfile.SupportOverhangDistance);
            currentSupportConeSupportedLayer.Add(new IntPoint(supportPointPosition));
            foreach (var t in currentSupportConeSupportedLayer)
            {
                TriangleIntersection[] trianglesIntersected = null;
                IntersectionProvider.IntersectTriangle(t.AsVector3(), new Vector3Class(0, 0, -1), trianglesAboveSupportPoint, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);

                var nearestTriangleIntersection = float.MaxValue;
                TriangleIntersection nearestIntersectionPoint = null;
                if (trianglesIntersected != null)
                {
                    foreach (var triangleIntersected in trianglesIntersected)
                    {
                        if (triangleIntersected != null)
                        {
                            if (triangleIntersected.IntersectionPoint.Z >= this.SliceHeight)
                            {
                                var distance = (new  Vector3Class(supportPointPosition.X, supportPointPosition.Y , triangleIntersected.IntersectionPoint.Z) - supportPointPosition).Length;
                                if (distance < nearestTriangleIntersection)
                                {
                                    nearestTriangleIntersection = distance;
                                    nearestIntersectionPoint = triangleIntersected;
                                }
                            }
                        }
                    }
                }

                if (nearestIntersectionPoint != null && nearestIntersectionPoint.IntersectionPoint.Z > maxLayerHeight)
                {
                    maxLayerHeight = nearestIntersectionPoint.IntersectionPoint.Z;
                }
            }


            if (maxLayerHeight == float.MinValue)
            {
                maxLayerHeight = 10000;
            }

            this.LastSupportedSliceHeight = maxLayerHeight;

        }
    }
    
    internal enum typeOfAutoSupportFilter
    {
        None = 0,
        FilteredByLowestPointXYOffset = 1,
        FilteredByLowestPointXYRangeOffset = 2,
        FilteredByLowestPointXYWithinRangeOffset = 4,
        FilteredByLowestPointXYWithinZRangeOffset = 8,
        FilteredByDuplicatePoint = 16,
        FilteredByLowestOffsetPointWithinLowestPointRangeOffset = 32,
        FilteredByLowestPointOffsetLowerThenLowerPointTop = 64,
        FilteredByLowestPointOffsetXYWithinZRangeOffset = 128,
        FilterOnSupportPointDistanceWithinPolygon = 256,
        FilterOnSupportPointDistanceWithEdgePoint = 512,
        FilterOnFacingDownSupportPointDistanceWithEdgePoint = 1024,
        FilteredByLowestPointOffsetLowerWithEdgeOverhang = 2048,
        FilteredBySupportConeOverhangWithinRange = 4096,
        FilteredByLowestPointWithinLowestPointRangeOffset = 8192,

    }
}
