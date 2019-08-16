using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAISurfaceIntersectionData
    {
        internal Guid Id { get; set; }
        internal IntPoint TopPoint { get; set; }
        internal Vector3Class BottomPoint { get; set; }
        internal typeOfAutoSupportFilter Filter { get; set; }
        internal float SliceHeight { get; set; }
        internal float OverhangDistance { get; set; }
        internal Color RenderColor { get; set; }
        internal bool UnsupportedContour { get; set; }
        internal float LastSupportedCenterAngle { get; set; }
        internal float LastSupportedCenterSliceHeight { get; set; }
        internal float LastSupportedSliceHeight { get; set; }
        internal TriangleIntersection ModelIntersection { get; set; }

        internal STLModel3D _trianglesWithinXYRange;


        internal bool IsGroundSupport
        {
            get
            {
                return this.BottomPoint.Z <= 0.1f;
            }
        }

        public float SupportOverhangDistanceFactor
        {
            get
            {
                if (this.LastSupportedCenterAngle > 90 && LastSupportedCenterAngle < 180)
                {
                    return  (1 + ((1f / 90) * (180 - LastSupportedCenterAngle))) ;
                }
                else
                {

                }

                return 1f;

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

        internal void UpdateTriangleReference(STLModel3D stlModel, SupportProfile selectedMaterialProfile)
        {
            //find latest supportcone supported height
            TriangleIntersection[] trianglesIntersected = null;
            var supportPointPosition = this.TopPoint.AsVector3();
            supportPointPosition.Z = this.SliceHeight - 1f;

            if (this._trianglesWithinXYRange == null)
            {
                CalcTrianglesWithinRange(supportPointPosition, selectedMaterialProfile, stlModel);
            }

            var trianglesAboveSupportPoint = new STLModel3D() { Triangles = new TriangleInfoList() };
            trianglesAboveSupportPoint.Triangles[0] = this._trianglesWithinXYRange.Triangles[0].Where(s => s.Top > supportPointPosition.Z && s.Normal.Z < 0).ToList();

            IntersectionProvider.IntersectTriangle(new Vector3Class(supportPointPosition.Xy), new Vector3Class(0, 0, 1), trianglesAboveSupportPoint, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
            if (trianglesIntersected != null)
            {
                var nearestLastSupportedHeight = new Vector3Class();
                var nearestLastSupportedHeightDistance = float.MaxValue;
                foreach (var triangleIntersection in trianglesIntersected)
                {
                    if (triangleIntersection != null)
                    {
                        var distance = (triangleIntersection.IntersectionPoint - supportPointPosition).Length;
                        if (distance < nearestLastSupportedHeightDistance)
                        {
                            nearestLastSupportedHeightDistance = distance;
                            nearestLastSupportedHeight = triangleIntersection.IntersectionPoint;

                            this.ModelIntersection = triangleIntersection;
                            this.LastSupportedCenterAngle = OpenTK.MathHelper.RadiansToDegrees(Vector3Class.CalculateAngle(this.ModelIntersection.Normal, Vector3Class.UnitZ));
                        }
                    }
                }
            }
        }

        internal void UpdateLastSupportedHeight(STLModel3D stlModel, SortedDictionary<float, PolyTree> modelAngleLayers, SupportProfile selectedMaterialProfile)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            if (this.Id == null || this.Id == Guid.Empty)
            {
                this.Id = Guid.NewGuid();
            }
            //find latest supportcone supported height
            TriangleIntersection[] trianglesIntersected = null;

            var supportPointPosition = this.TopPoint.AsVector3();
            supportPointPosition.Z = this.SliceHeight;
            var currentSupportConeSupportedLayer = MagsAIEngine.ConvertSupportPointsToCircles(this.TopPoint, this.OverhangDistance * this.SupportOverhangDistanceFactor);
            currentSupportConeSupportedLayer.Add(this.TopPoint);

            if (this._trianglesWithinXYRange == null)
            {
                CalcTrianglesWithinRange(supportPointPosition, selectedMaterialProfile, stlModel);
            }

            var trianglesAboveSupportPoint = new STLModel3D() { Triangles = new TriangleInfoList() };
            trianglesAboveSupportPoint.Triangles[0] = this._trianglesWithinXYRange.Triangles[0].Where(s => s.Bottom > supportPointPosition.Z && s.Normal.Z > 0).ToList();

            //IntersectionProvider.IntersectTriangle(supportPointPosition, new Vector3Class(0, 0, -1), trianglesAboveSupportPoint, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
            //if (trianglesIntersected != null)
            //{
            //    var nearestLastSupportedHeight = new Vector3Class();
            //    var nearestLastSupportedHeightDistance = float.MaxValue;
            //    foreach (var triangleIntersection in trianglesIntersected)
            //    {
            //        if (triangleIntersection != null)
            //        {

            //            var distance = (triangleIntersection.IntersectionPoint - supportPointPosition).Length;
            //            if (distance < nearestLastSupportedHeightDistance)
            //            {
            //                nearestLastSupportedHeightDistance = distance;
            //                nearestLastSupportedHeight = triangleIntersection.IntersectionPoint;
            //            }
            //        }
            //    }

            //    if (nearestLastSupportedHeight != new Vector3Class())
            //    {
            //        this.LastSupportedCenterSliceHeight = nearestLastSupportedHeight.Z;
            //    }
            //}

            ////determine the last supported slice height using the half of material overhangdistance and removing the layers above
            //if (this.LastSupportedCenterAngle == 0f)
            //{
            //    this.UpdateTriangleReference(stlModel, selectedMaterialProfile);
            //}

            var maxLayerHeight = float.MinValue;

            foreach (var t in currentSupportConeSupportedLayer)
            {
                var tAsVector = t.AsVector3();
                tAsVector.Z = 1000;
                TriangleIntersection[] trianglesIntersected2 = null;
                    IntersectionProvider.IntersectTriangle(tAsVector, new Vector3Class(0, 0, -1), trianglesAboveSupportPoint, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected2);

                var nearestTriangleIntersection = float.MaxValue;
                TriangleIntersection nearestIntersectionPoint = null;
                if (trianglesIntersected2 != null) {
                    foreach (var triangleIntersected in trianglesIntersected2)
                    {
                        if (triangleIntersected != null)
                        {
                            if (triangleIntersected.IntersectionPoint.Z >= this.SliceHeight){
                                var distance = (triangleIntersected.IntersectionPoint - supportPointPosition).Length;
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

           // Console.WriteLine("SurfaceIntersectionData: Measurement 2: " + maxLayerHeight);
           // Console.WriteLine("SurfaceIntersectionData: Measurement 2: " + stopwatch.ElapsedMilliseconds + "ms");
        }


        internal void UpdateBottomSupportedHeight(STLModel3D stlModel)
        {
            //find latest supportcone supported height
            TriangleIntersection[] trianglesIntersected = null;
            var supportPointPosition = this.TopPoint.AsVector3();
            supportPointPosition.Z = this.SliceHeight;
            IntersectionProvider.IntersectTriangle(supportPointPosition, new Vector3Class(0, 0, -1), stlModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
            this.BottomPoint = new Vector3Class(supportPointPosition.Xy);

            if (trianglesIntersected != null)
            {
                var nearestLastSupportedHeight = new Vector3Class();
                var nearestLastSupportedHeightDistance = float.MaxValue;
                foreach (var triangleIntersection in trianglesIntersected)
                {
                    if (triangleIntersection != null)
                    {
                        if (triangleIntersection.IntersectionPoint.Z < supportPointPosition.Z)
                        {
                            var distance = (triangleIntersection.IntersectionPoint - supportPointPosition).Length;
                            if (distance < nearestLastSupportedHeightDistance)
                            {
                                nearestLastSupportedHeightDistance = distance;
                                nearestLastSupportedHeight = triangleIntersection.IntersectionPoint;
                            }
                        }
                    }
                }

                if (nearestLastSupportedHeight != new Vector3Class())
                {
                    this.BottomPoint = nearestLastSupportedHeight;
                }
            }
            else
            {

            }
        }

        internal float CalcWallThickness()
        {
            var wallThickness = 0f;

            return wallThickness;
        }

        //   internal PolyTree OutlineCirclePolygon { get; set; }
    }
}
