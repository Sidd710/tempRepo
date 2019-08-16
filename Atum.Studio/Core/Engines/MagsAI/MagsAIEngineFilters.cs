using Atum.DAL.Managers;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Atum.Studio.Core.Engines.MagsAI.MagsAIEngine;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    class MagsAIEngineFilters
    {
        internal static List<Vector3Class[]> DebugVectors = new List<Vector3Class[]>();

        internal static void FilterOnAngledPointAndWallThickness(MagsAISurfaceIntersectionData modelAngledSupportPoint, STLModel3D stlModel)
        {
            if (modelAngledSupportPoint.ModelIntersection != null && modelAngledSupportPoint.Filter == typeOfAutoSupportFilter.None)
            {
                if (!modelAngledSupportPoint.IsGroundSupport)
                {
                    if (modelAngledSupportPoint.LastSupportedCenterAngle < 135 && modelAngledSupportPoint.LastSupportedCenterAngle > 90)
                    {

                        //calc horizontal normal in inverted direction
                        var wallThicknessNormal = modelAngledSupportPoint.ModelIntersection.Normal;

                        //convert to horizontal normal
                        wallThicknessNormal.Z = 0;
                        wallThicknessNormal.Normalize();

                        TriangleIntersection[] triangleIntersections = null;
                        IntersectionProvider.IntersectTriangle(modelAngledSupportPoint.ModelIntersection.IntersectionPoint, wallThicknessNormal, stlModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out triangleIntersections);

                        if (triangleIntersections != null)
                        {
                            //find nearest intersection point
                            var distance = float.MaxValue;
                            foreach (var intersectionPoint in triangleIntersections)
                            {
                                if (intersectionPoint != null)
                                {
                                    if (VectorHelper.IsSameDirection((intersectionPoint.IntersectionPoint - modelAngledSupportPoint.ModelIntersection.IntersectionPoint), -wallThicknessNormal))
                                    {

                                        var intersectionDistance = (modelAngledSupportPoint.ModelIntersection.IntersectionPoint - intersectionPoint.IntersectionPoint).Length;

                                        if (intersectionDistance > 0f && intersectionDistance < distance)
                                        {
                                            distance = intersectionDistance;
                                        }
                                    }

                                }
                            }

                            if (distance != float.MaxValue && distance > 3f)
                            {
                                modelAngledSupportPoint.Filter = typeOfAutoSupportFilter.FilteredByDuplicatePoint;
                            }
                        }
                        else
                        {

                        }
                    }
                }

            }
        }

        internal static void DetermineIfFaceDownContoursHasEdgeDown(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContours, SortedDictionary<float, PolyTree> modelContours, float materialSupportConeOverhangDistance, STLModel3D stlModel, Dictionary<int, float> sliceIndexKeys)
        {
            var maxMaterialSupportConeOverhangDistance = 100 * materialSupportConeOverhangDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR;

            //foreach(var facingDownContourItemAsync in facingDownContours)
            Parallel.ForEach(facingDownContours, facingDownContourItemAsync =>
            {
                {
                    var facingDownContourItem = facingDownContourItemAsync;
                    var facingDownContour = facingDownContourItem.Key;
                    var facingDownSliceIndex = facingDownContour.SliceIndex;
                    var facingDownEndSliceIndex = facingDownSliceIndex + facingDownContourItem.Value.Count;

                    var contoursOnSameHeight = modelContours[facingDownContour.SliceHeight];
                    var totalCurrentPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();

                    var otherPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();
                    var totalOtherPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();

                    //find nextr triange indexes till end sliceheight

                    if (stlModel.SliceIndexes.Count > facingDownContour.SliceIndex + 40)
                    {
                        var facingDownEndSliceIndexHeight = sliceIndexKeys[facingDownContour.SliceIndex + 40];
                        totalCurrentPolygonTriangles = Helpers.TriangleHelper.GetConnectedTrianglesAbove(facingDownContour.Polygon.TriangleConnections, facingDownContour.SliceHeight, facingDownEndSliceIndexHeight, stlModel);


                        //other contours
                        foreach (var t in contoursOnSameHeight._allPolys)
                        {
                            if (!t.IsHole)
                            {
                                if (t != facingDownContour.Polygon)
                                {
                                    foreach (var triangleConnection in t.TriangleConnections.Keys)
                                    {
                                        if (!otherPolygonTriangles.ContainsKey(triangleConnection))
                                        {
                                            otherPolygonTriangles.Add(triangleConnection, false);
                                        }
                                    }
                                }
                            }
                        }

                        var t2 = Helpers.TriangleHelper.GetConnectedTrianglesEdgeDown(otherPolygonTriangles.Keys.ToArray(), totalCurrentPolygonTriangles, facingDownContour.SliceHeight, facingDownEndSliceIndexHeight, stlModel);

                        if (t2)
                        {
                            facingDownContour.HasEdgeDown = true;

                            foreach (var k in facingDownContour.Polygon.TriangleConnections)
                            {
                                stlModel.Triangles[k.Key.ArrayIndex][k.Key.TriangleIndex].UpdateColor(new Byte4Class(128, 0, 0, 255), true);
                            }
                        }
                    }
                }

            }
            );


            stlModel.UpdateBinding();

        }
        internal static void FilterOnFacingDownLowestPointWithOverhangPoints(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContours, ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> currentSupportPoints, float materialSupportConeTopRadius)
        {
            foreach (var supportPointContour in currentSupportPoints.Values)
            {
                foreach (var supportIntersectionPoint in supportPointContour.Keys)
                {
                    if (supportIntersectionPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        var supportIntersectionPointExists = false;
                        foreach (var lowestPointPolygon in facingDownContours.Keys)
                        {
                            foreach (var lowestPoint in lowestPointPolygon.LowestIntersectionPoints)
                            {
                                var xyDifference = (lowestPoint.TopPoint.Xy - supportIntersectionPoint.TopPoint.Xy).Length;
                                if (xyDifference < 0)
                                {
                                    xyDifference = -xyDifference;
                                }

                                if (xyDifference < materialSupportConeTopRadius * 2)
                                {
                                    var zDifference = lowestPoint.TopPoint.Z - supportIntersectionPoint.TopPoint.Z;
                                    if (zDifference < 0)
                                    {
                                        zDifference = -zDifference;
                                    }

                                    if (zDifference < 0.2)
                                    {
                                        supportIntersectionPoint.Filter = typeOfAutoSupportFilter.FilteredByLowestOffsetPointWithinLowestPointRangeOffset; ;
                                        break;
                                    }
                                }
                            }
                        }

                        if (supportIntersectionPointExists)
                        {
                            break;
                        }
                    }

                }
            }
        }

        internal static void FilteredByLowestPointOffsetLowerThenLowerPointTop(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, STLModel3D stlModel)
        {
            foreach (var lowestPointContourItem in lowestPointsContours)
            {
                foreach (var contourOffsetItem in lowestPointContourItem.Key.LowestPointsWithOffset)
                {
                    //add lowest point support cones
                    foreach (var supportPoint in contourOffsetItem.Value)
                    {
                        var triangleIntersectionBelow = new Vector3Class();
                        if (supportPoint.Filter == typeOfAutoSupportFilter.None)
                        {
                            var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, supportPoint.Point, lowestPointContourItem.Key.SliceHeight, triangleIntersectionBelow);
                            var intersectedData = new MagsAIIntersectionData() { BottomPoint = triangleIntersectionBelow, TopPoint = triangleIntersection.IntersectionPoint, SliceHeight = lowestPointContourItem.Key.SliceHeight };
                            if (!lowestPointContourItem.Key.LowestPointsWithOffsetIntersectionPoint.ContainsKey(contourOffsetItem.Key))
                            {
                                lowestPointContourItem.Key.LowestPointsWithOffsetIntersectionPoint.Add(contourOffsetItem.Key, new List<MagsAIIntersectionData>());
                            }
                            lowestPointContourItem.Key.LowestPointsWithOffsetIntersectionPoint[contourOffsetItem.Key].Add(intersectedData);
                            if (triangleIntersection.IntersectionPoint.Z < lowestPointContourItem.Key.LowestIntersectionPointsTop)
                            {
                                intersectedData.Filter = typeOfAutoSupportFilter.FilteredByLowestPointOffsetLowerThenLowerPointTop;
                            }
                        }
                    }
                }
            }
        }

        //internal static void FilterOnFacingDownLowestPointWithLayerBasedOverhangPoints(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContours, List<PolygonSupportPoint> supportPointContours, float materialSupportConeTopRadius)
        //{
        //    foreach (var supportPointContour in supportPointContours)
        //    {
        //        foreach (var supportIntersectionPoint in supportPointContour.LayerBasedOverhangPoints)
        //        {
        //            var supportIntersectionPointExists = false;
        //            foreach (var lowestPointPolygon in facingDownContours.Keys)
        //            {
        //                foreach (var lowestPoint in lowestPointPolygon.LowestIntersectionPoints)
        //                {
        //                    var xyDifference = (lowestPoint.TopPoint.Xy - supportIntersectionPoint.TopPoint.Xy).Length;
        //                    if (xyDifference < 0)
        //                    {
        //                        xyDifference = -xyDifference;
        //                    }

        //                    if (xyDifference > materialSupportConeTopRadius * 2)
        //                    {
        //                        var zDifference = lowestPoint.TopPoint.Z - supportIntersectionPoint.TopPoint.Z;
        //                        if (zDifference < 0)
        //                        {
        //                            zDifference = -zDifference;
        //                        }

        //                        if (zDifference < 0.2)
        //                        {
        //                            supportIntersectionPoint.Filter = typeOfAutoSupportFilter.FilteredBySupportPointWithinLowestPointRangeOffset; ;
        //                            break;
        //                        }
        //                    }
        //                }

        //                if (supportIntersectionPointExists)
        //                {
        //                    break;
        //                }
        //            }

        //        }
        //    }
        //}

        internal static void FilterOnFacingDownLowestPointWithEdgePoints(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContours, float materialSupportConeTopRadius)
        {
            foreach (var edgePointContour in facingDownContours.Keys)
            {
                foreach (var edgeIntersectionPoint in edgePointContour.EdgeIntersectionPoints)
                {
                    foreach (var lowestPointPolygon in facingDownContours.Keys)
                    {
                        foreach (var lowestPoint in lowestPointPolygon.LowestPoints)
                        {
                            if (lowestPoint.Filter == typeOfAutoSupportFilter.None)
                            {
                                var xyDifference = (lowestPoint.Point.AsVector3().Xy - edgeIntersectionPoint.TopPoint.Xy).Length;
                                if (xyDifference < 0)
                                {
                                    xyDifference = -xyDifference;
                                }

                                if (xyDifference < materialSupportConeTopRadius * 2)
                                {
                                    var zDifference = lowestPointPolygon.SliceHeight - edgePointContour.SliceHeight;
                                    if (zDifference < 0)
                                    {
                                        zDifference = -zDifference;
                                    }

                                    if (zDifference < 0.2)
                                    {
                                        lowestPoint.Filter = typeOfAutoSupportFilter.FilteredByLowestOffsetPointWithinLowestPointRangeOffset; ;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        internal static void FilterOnLowestPointsOffsetWithLowestPointsOffsets(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, float materialSupportConeTopRadius)
        {
            //check if center support cone intersects other offset support cone
            foreach (var lowestPointsContour in lowestPointsContours.Keys)
            {
                foreach (var lowestPointContourPoint in lowestPointsContour.LowestPoints)
                {
                    //check all higher offset point
                    foreach (var lowestPointContourOffsetCheckIndex in lowestPointsContour.LowestPointsWithOffset.Keys)
                    {
                        //check all higher offset point
                        foreach (var lowestPointContourOffsetCheckPoint in lowestPointsContour.LowestPointsWithOffset[lowestPointContourOffsetCheckIndex])
                        {
                            if (lowestPointContourOffsetCheckPoint.Filter == typeOfAutoSupportFilter.None && lowestPointContourPoint.Filter == typeOfAutoSupportFilter.None)
                            {
                                var xyDifference = (lowestPointContourPoint.Point.AsVector3().Xy - lowestPointContourOffsetCheckPoint.Point.AsVector3().Xy).Length;
                                if (xyDifference < 0)
                                {
                                    xyDifference = -xyDifference;
                                }

                                if (xyDifference < materialSupportConeTopRadius * 2.5)
                                {

                                    lowestPointContourOffsetCheckPoint.Filter = typeOfAutoSupportFilter.FilteredByLowestPointWithinLowestPointRangeOffset; ;

                                }
                            }

                        }
                    }
                }

            }


            //check if offset support cone intersects other offset support cone
            foreach (var lowestPointsContour in lowestPointsContours.Keys)
            {
                foreach (var lowestPointContourOffsetIndex in lowestPointsContour.LowestPointsWithOffset.Keys)
                {
                    foreach (var lowestPointContourOffsetPoint in lowestPointsContour.LowestPointsWithOffset[lowestPointContourOffsetIndex])
                    {
                        //check all higher offset point
                        foreach (var lowestPointContourOffsetCheckIndex in lowestPointsContour.LowestPointsWithOffset.Keys)
                        {
                            //check all higher offset point
                            if (lowestPointContourOffsetCheckIndex > lowestPointContourOffsetIndex)
                            {
                                foreach (var lowestPointContourOffsetCheckPoint in lowestPointsContour.LowestPointsWithOffset[lowestPointContourOffsetCheckIndex])
                                {
                                    if (lowestPointContourOffsetCheckPoint.Filter == typeOfAutoSupportFilter.None && lowestPointContourOffsetPoint.Filter == typeOfAutoSupportFilter.None)
                                    {
                                        var xyDifference = (lowestPointContourOffsetPoint.Point.AsVector3().Xy - lowestPointContourOffsetCheckPoint.Point.AsVector3().Xy).Length;
                                        if (xyDifference < 0)
                                        {
                                            xyDifference = -xyDifference;
                                        }

                                        if (xyDifference < materialSupportConeTopRadius * 2.5)
                                        {
                                            //var zDifference = lowestPointContourOffsetPoint.Point.AsVector3().Z - lowestPointsContour.SliceHeight;
                                            //if (zDifference < 0)
                                            //{
                                            //    zDifference = -zDifference;
                                            //}

                                            //if (zDifference < 1)
                                            //{
                                            lowestPointContourOffsetCheckPoint.Filter = typeOfAutoSupportFilter.FilteredByLowestOffsetPointWithinLowestPointRangeOffset; ;
                                            // break;
                                            //   }
                                        }
                                    }
                                }
                            }
                        }
                    }

                }

                //if (layerbasedOverhangPoint.Filter == typeOfAutoSupportFilter.None)
                //{
                //    var xyDifference = (layerbasedOverhangPoint.TopPoint.Xy - lowestPointOffsetIntersectionPoint.TopPoint.Xy).Length;
                //    if (xyDifference < 0)
                //    {
                //        xyDifference = -xyDifference;
                //    }

                //    if (xyDifference < materialSupportConeTopRadius * 2)
                //    {
                //        var zDifference = layerbasedOverhangPoint.TopPoint.Z - edgePointContour.SliceHeight;
                //        if (zDifference < 0)
                //        {
                //            zDifference = -zDifference;
                //        }

                //        if (zDifference < 1)
                //        {
                //            layerbasedOverhangPoint.Filter = typeOfAutoSupportFilter.FilteredBySupportPointWithinLowestPointRangeOffset; ;
                //            break;
                //        }
                //    }
                //}


            }

        }


        internal static void FilterOnOverhangPointsOffsetLayerBaseOverhangPoints(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContours, 
            ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> currentSupportPoints, float materialSupportConeTopRadius)
        {
            foreach (var edgePointContour in facingDownContours.Keys)
            {
                foreach (var lowestPointOffsetIntersectionPoints in edgePointContour.LowestPointsWithOffsetIntersectionPoint.Values)
                {
                    foreach (var lowestPointOffsetIntersectionPoint in lowestPointOffsetIntersectionPoints)
                    {
                        foreach (var supportPointContour in currentSupportPoints.Values)
                        {
                            foreach (var overhangPoint in supportPointContour.Keys)
                            {
                                if (overhangPoint.Filter == typeOfAutoSupportFilter.None)
                                {
                                    var xyDifference = (overhangPoint.TopPoint.Xy - lowestPointOffsetIntersectionPoint.TopPoint.Xy).Length;
                                    if (xyDifference < 0)
                                    {
                                        xyDifference = -xyDifference;
                                    }

                                    if (xyDifference < materialSupportConeTopRadius * 2)
                                    {
                                        var zDifference = overhangPoint.TopPoint.Z - edgePointContour.SliceHeight;
                                        if (zDifference < 0)
                                        {
                                            zDifference = -zDifference;
                                        }

                                        if (zDifference < 1)
                                        {
                                            overhangPoint.Filter = typeOfAutoSupportFilter.FilteredByLowestOffsetPointWithinLowestPointRangeOffset; ;
                                            break;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //internal static void FilterOnEdgePointsLayerBaseOverhangPoints(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContours, List<PolygonSupportPoint> supportPointContours, float materialSupportConeTopRadius)
        //{
        //    foreach (var edgePointContour in facingDownContours.Keys)
        //    {
        //        foreach (var edgeIntersectionPoint in edgePointContour.EdgeIntersectionPoints)
        //        {
        //            foreach (var supportPointContour in supportPointContours)
        //            {
        //                foreach (var layerbasedOverhangPoint in supportPointContour.LayerBasedOverhangPoints)
        //                {
        //                    if (layerbasedOverhangPoint.Filter == typeOfAutoSupportFilter.None)
        //                    {
        //                        var xyDifference = (layerbasedOverhangPoint.TopPoint.Xy - edgeIntersectionPoint.TopPoint.Xy).Length;
        //                        if (xyDifference < 0)
        //                        {
        //                            xyDifference = -xyDifference;
        //                        }

        //                        if (xyDifference < materialSupportConeTopRadius * 2)
        //                        {
        //                            var zDifference = layerbasedOverhangPoint.TopPoint.Z - edgePointContour.SliceHeight;
        //                            if (zDifference < 0)
        //                            {
        //                                zDifference = -zDifference;
        //                            }

        //                            if (zDifference < 0.2)
        //                            {
        //                                layerbasedOverhangPoint.Filter = typeOfAutoSupportFilter.FilteredBySupportPointWithinLowestPointRangeOffset; ;
        //                                break;
        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //        }
        //    }
        //}

        internal static void FilterOnEdgePointDistance(List<PolygonSupportPointCollection> supportPointContours, float materialSupportConeTopRadius)
        {
            //FILTER find edge points that are within there own boundry. If found remove all edge points and insert middle point
            var minSupportConeDistance = materialSupportConeTopRadius;
            foreach (var supportPointContour in supportPointContours)
            {
                var edgePointsWithinRange = new List<MagsAIIntersectionData>();

                for (var edgeIntersectionIndex = supportPointContour.EdgePoints.Count - 1; edgeIntersectionIndex >= 0; edgeIntersectionIndex--)
                {
                    if (supportPointContour.EdgePoints.Count > edgeIntersectionIndex)
                    {
                        var edgeIntersection = supportPointContour.EdgePoints[edgeIntersectionIndex];

                        edgePointsWithinRange.Clear();
                        edgePointsWithinRange.Add(edgeIntersection);

                        foreach (var checkSupportPointContour in supportPointContours)
                        {
                            for (var edgeIntersectionCheckIndex = checkSupportPointContour.EdgePoints.Count - 1; edgeIntersectionCheckIndex >= 0; edgeIntersectionCheckIndex--)
                            {
                                var checkEdgeIntersection = checkSupportPointContour.EdgePoints[edgeIntersectionCheckIndex];

                                if (checkEdgeIntersection.TopPoint != edgeIntersection.TopPoint && checkEdgeIntersection.TopPoint.Z == edgeIntersection.TopPoint.Z)
                                {
                                    var vectorDifference = (edgeIntersection.TopPoint.Xy - checkEdgeIntersection.TopPoint.Xy).Length;

                                    if (vectorDifference < minSupportConeDistance)
                                    {
                                        edgePointsWithinRange.Add(checkEdgeIntersection);

                                        LoggingManager.WriteToLog("Autosupport Engine", "Filter removed support points within edge distance", edgeIntersection.ToString());
                                        break;
                                    }
                                }
                            }
                        }


                        if (edgePointsWithinRange.Count > 1)
                        {
                            //find middle point
                            var middlePoint = new Vector3Class();
                            foreach (var edgePointWithinRange in edgePointsWithinRange)
                            {
                                middlePoint += edgePointWithinRange.TopPoint;

                                foreach (var checkSupportPointContour in supportPointContours)
                                {
                                    checkSupportPointContour.EdgePoints.RemoveAll(e => e.TopPoint == edgePointWithinRange.TopPoint);
                                }
                            }

                            middlePoint /= edgePointsWithinRange.Count;

                            supportPointContour.EdgePoints.Add(new MagsAIIntersectionData() { TopPoint = middlePoint, SliceHeight = edgePointsWithinRange[0].SliceHeight });
                        }
                    }
                }
            }
        }

        internal static void FilterOnFacingDownPointDistanceWithEdgePoint(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContourItems, List<PolygonSupportPointCollection> supportPointContours, float materialSupportConeTopRadius)
        {
            //FILTER facingDownContours on offset support points 0.5 * topradius and height += 10
            var minSupportConeDistance = 4 * materialSupportConeTopRadius;
            foreach (var supportPointContour in supportPointContours)
            {
                var currentSliceIndex = supportPointContour.SliceIndex - 5;
                var endSliceIndex = currentSliceIndex + 10;

                foreach (var edgePointItemPoint in supportPointContour.EdgePoints)
                {
                    var edgePointItemPointProcessed = false;
                    foreach (var checkFacingDownContour in facingDownContourItems)
                    {
                        if (supportPointContour.SliceIndex >= currentSliceIndex && supportPointContour.SliceIndex <= endSliceIndex && checkFacingDownContour.Key.SliceIndex >= currentSliceIndex && checkFacingDownContour.Key.SliceIndex <= endSliceIndex)
                        {
                            foreach (var checkOffsetItem in checkFacingDownContour.Key.LowestPointsWithOffsetIntersectionPoint)
                            {
                                for (var checkFacingDownContourIndex = checkOffsetItem.Value.Count - 1; checkFacingDownContourIndex >= 0; checkFacingDownContourIndex--)
                                {
                                    var checkFacingDownContourConnectedChildPoint = checkOffsetItem.Value.ElementAt(checkFacingDownContourIndex);
                                    {
                                        if (edgePointItemPoint.Filter == typeOfAutoSupportFilter.None && checkFacingDownContourConnectedChildPoint.Filter == typeOfAutoSupportFilter.None)
                                        {
                                            var facingDownVectorDifference = (edgePointItemPoint.TopPoint.Xy - checkFacingDownContourConnectedChildPoint.TopPoint.Xy).Length;

                                            if (facingDownVectorDifference < minSupportConeDistance)
                                            {
                                                edgePointItemPointProcessed = true;
                                                edgePointItemPoint.Filter |= typeOfAutoSupportFilter.FilterOnSupportPointDistanceWithEdgePoint;
                                                LoggingManager.WriteToLog("Autosupport Engine", "Filter removed support points within edge distance", edgePointItemPoint.ToString());
                                                //break;
                                            }
                                        }
                                    }
                                }

                                if (edgePointItemPointProcessed)
                                {
                                    //break;
                                }
                            }
                        }

                        if (edgePointItemPointProcessed)
                        {
                            //break;
                        }
                    }
                }
            }
        }

        internal static void FilterOnLowestOffsetPointsXYEdgePointDistance(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> facingDownContourItems, float materialSupportConeOverhang)
        {
            //FILTER facingDownContours on offset support points 0.5 * topradius and height += 10
            var minSupportConeDistance = materialSupportConeOverhang;
            foreach (var supportPointContour in facingDownContourItems.Keys)
            {
                foreach (var edgeSupportPointItemPoint in supportPointContour.EdgeIntersectionPoints)
                {

                    foreach (var checkOffsetItem in supportPointContour.LowestPointsWithOffsetIntersectionPoint)
                    {
                        for (var checkFacingDownContourIndex = checkOffsetItem.Value.Count - 1; checkFacingDownContourIndex >= 0; checkFacingDownContourIndex--)
                        {
                            var checkFacingDownContourConnectedChildPoint = checkOffsetItem.Value.ElementAt(checkFacingDownContourIndex);
                            {
                                if (edgeSupportPointItemPoint.Filter == typeOfAutoSupportFilter.None && checkFacingDownContourConnectedChildPoint.Filter == typeOfAutoSupportFilter.None)
                                {
                                    var facingDownVectorDifference = (edgeSupportPointItemPoint.TopPoint.Xy - checkFacingDownContourConnectedChildPoint.TopPoint.Xy).Length;

                                    if (facingDownVectorDifference < minSupportConeDistance)
                                    {
                                        checkFacingDownContourConnectedChildPoint.Filter |= typeOfAutoSupportFilter.FilteredByLowestPointOffsetLowerWithEdgeOverhang;
                                        LoggingManager.WriteToLog("Autosupport Engine", "Filter removed support points distance (edge overhang)", edgeSupportPointItemPoint.ToString());
                                        //break;
                                    }
                                }
                            }
                        }

                    }
                }
            }
        }
    }
}
