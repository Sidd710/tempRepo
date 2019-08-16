using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;
using Atum.Studio.Core.Engines.MagsAI;
using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using System.Collections.Concurrent;
using Atum.DAL.Hardware;

namespace Atum.Studio.Core.Models
{
    internal class ExtrudedSurfaceModel : STLModel3D
    {
        internal Dictionary<IntPoint, MagsAISurfaceIntersectionData> AddedSupportPoints { get; set; }
        internal Dictionary<float, Dictionary<IntPoint, MagsAISurfaceIntersectionData>> AddedSupportPointsByLayer { get; set; }

        internal ConcurrentDictionary<float, PolyTree> ModelContours = new ConcurrentDictionary<float, PolyTree>();
        internal ConcurrentDictionary<float, PolyTree> ModelAngledContours = new ConcurrentDictionary<float, PolyTree>();

        internal ExtrudedSurfaceModel()
        {
        }

        internal Dictionary<IntPoint, MagsAISurfaceIntersectionData> CalcSupportPoints(SortedDictionary<float, PolyTree> modelLayers, ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> currentModelSupportPoints, float firstSurfaceAngleDistanceFactor, SupportProfile selectedMaterialProfile, float overhangFactor, ConcurrentDictionary<float, ConcurrentDictionary<float, ConcurrentDictionary<IntPoint, MagsAISurfaceIntersectionData>>> previousOverhangDistanceFactorBasedSupportPoints, STLModel3D stlModel, TriangleSurfaceInfo surface)
        {
            this.AddedSupportPoints = new Dictionary<IntPoint, MagsAISurfaceIntersectionData>();
            this.AddedSupportPointsByLayer = new Dictionary<float, Dictionary<IntPoint, MagsAISurfaceIntersectionData>>();

            //create support cones that exists in each layer
            var previousSliceHeight = float.MaxValue;

            foreach (var sliceHeight in this.ModelContours.Keys.OrderBy(s => s))
            {
                var sliceContours = this.ModelContours[sliceHeight];
                sliceContours = ContourHelper.IntersectModelSliceLayer(sliceContours, modelLayers[sliceHeight]); //create contour offset using max border as model contour instead of extrude contour
                
                if (!this.AddedSupportPointsByLayer.ContainsKey(sliceHeight))
                {
                    this.AddedSupportPointsByLayer.Add(sliceHeight, new Dictionary<IntPoint, MagsAISurfaceIntersectionData>());
                }

                if (previousSliceHeight != float.MaxValue)
                {
                    var supportedLayerAsPolyTree = new PolyTree();
                    if (this.AddedSupportPointsByLayer.ContainsKey(previousSliceHeight))
                    {
                        foreach (var supportIntersectionData in this.AddedSupportPointsByLayer[previousSliceHeight].Values)
                        {
                            if (supportIntersectionData.UnsupportedContour)
                            {
                                this.AddedSupportPointsByLayer[sliceHeight].Add(supportIntersectionData.TopPoint, supportIntersectionData);
                            }
                        }
                    }

                    //added support circles from current diff layer with overhang distance
                    foreach (var overhangDistanceFactor in previousOverhangDistanceFactorBasedSupportPoints)
                    {
                        var previousAngleBasedSupportPointsAsCircles = new List<List<IntPoint>>();
                        if (overhangDistanceFactor.Value.ContainsKey(sliceHeight))
                        {
                            foreach (var previousAngleBasedSupportPoint in overhangDistanceFactor.Value[sliceHeight].Values)
                            {
                                AddPointToCurrentLayerWhenExistsInContour(sliceContours, previousAngleBasedSupportPoint, sliceHeight);
                            }
                        }
                    }

                    //check if supportcones where allready added using other algorithms 
                    var supportSliceHeightsWithinRange = new List<float>() { (float)Math.Round(previousSliceHeight - (sliceHeight - previousSliceHeight), 2), sliceHeight };
                    var supportSliceHeightsWithinRangeMin = supportSliceHeightsWithinRange[0];
                    var supportSliceHeightsWithinRangeMax = supportSliceHeightsWithinRange[1];
                    foreach (var supportPointIndex in currentModelSupportPoints)
                    {
                        foreach (var supportPointValue in supportPointIndex.Value.Keys)
                        {
                            if (supportPointValue.SliceHeight <= sliceHeight && supportPointValue.LastSupportSliceHeight >= supportSliceHeightsWithinRangeMin)
                            {
                                var supportSlicePointAsIntPoint = new IntPoint(supportPointValue.TopPoint);
                                if (!this.AddedSupportPointsByLayer[sliceHeight].ContainsKey(supportSlicePointAsIntPoint))
                                {
                                    var supportConePoint2 = new MagsAISurfaceIntersectionData();
                                    supportConePoint2.TopPoint = supportSlicePointAsIntPoint;
                                    supportConePoint2.SliceHeight = sliceHeight;
                                    supportConePoint2.OverhangDistanceFactor = firstSurfaceAngleDistanceFactor;
                                    supportConePoint2.ModelIntersection = supportPointValue.ModelIntersection;
                                    supportConePoint2.LastSupportSliceHeight = supportPointValue.LastSupportSliceHeight;

                                    this.AddedSupportPointsByLayer[sliceHeight].Add(supportConePoint2.TopPoint, supportConePoint2);
                                }
                            }
                        }

                    }

                   
                    //combine all previous found points
                    supportedLayerAsPolyTree = new PolyTree();
                    if (AddedSupportPointsByLayer.ContainsKey(sliceHeight))
                    {
                        var supportCircles = new List<List<IntPoint>>();
                        foreach (var addedSupportPointByLayer in AddedSupportPointsByLayer[sliceHeight].Values)
                        {
                            var outlineCirclePoints = MagsAIEngine.ConvertSupportPointsToCircles(addedSupportPointByLayer.TopPoint, selectedMaterialProfile.SupportOverhangDistance);
                            supportCircles.Add(outlineCirclePoints);
                            var outlineCirclePolygon = MagsAIEngine.MergeSupportCircles(new List<List<IntPoint>>() { outlineCirclePoints });
                            var intersectedPolygons = ContourHelper.IntersectModelSliceLayer(ModelContours[sliceHeight], outlineCirclePolygon);

                            var intersectedPolygonFound = false;
                            foreach (var intersectedPolygon in intersectedPolygons._allPolys.Where(s => !s.IsHole))
                            {
                                if (ContourHelper.PointExistsInNotHolePolygon(intersectedPolygon, addedSupportPointByLayer.TopPoint))
                                {
                                    supportedLayerAsPolyTree._allPolys.Add(intersectedPolygon);
                                    break;
                                }
                            }

                            //check if intersected triangle is part of angle surface
                            if (!intersectedPolygonFound)
                            {
                                //check if triangle index is in surface
                                var pointFound = false;
                                foreach (var surfaceConnection in surface.Keys)
                                {

                                    if (addedSupportPointByLayer.ModelIntersection != null)
                                    {
                                        if (surfaceConnection.ArrayIndex == addedSupportPointByLayer.ModelIntersection.Index.ArrayIndex && surfaceConnection.TriangleIndex == addedSupportPointByLayer.ModelIntersection.Index.TriangleIndex)
                                        {
                                            foreach (var intersectedPolygon in intersectedPolygons._allPolys.Where(s => !s.IsHole))
                                            {
                                                supportedLayerAsPolyTree._allPolys.Add(intersectedPolygon);
                                                pointFound = true;
                                            }
                                            break;
                                        }
                                    }
                                }

                                if (!pointFound)
                                {

                                }
                            }
                        }

                       // TriangleHelper.SavePolyNodesContourToPng(ModelContours[sliceHeight]._allPolys, surface.Id.ToString("00") + "-" + sliceHeight.ToString("0.00") + "-" + overhangFactor.ToString(".00") + "-model");
//                        TriangleHelper.SavePolyNodesContourToPng(intersectedPolygons._allPolys, surface.Id.ToString("00") + "-" + sliceHeight.ToString("0.00") + "-" + overhangFactor.ToString(".00") + "-intersected");


                        //TriangleHelper.SavePolyNodesContourToPng(MagsAIEngine.MergeSupportCircles(supportCircles)._allPolys, surface.Id.ToString("00") + "-" + sliceHeight.ToString("0.00") + "-" + overhangFactor.ToString(".00") + "-supportcircles");

                        supportedLayerAsPolyTree = UnionModelSliceLayer(new PolyTree(), supportedLayerAsPolyTree);

                        //TriangleHelper.SavePolyNodesContourToPng(supportedLayerAsPolyTree._allPolys, surface.Id.ToString("00") + "-" + sliceHeight.ToString("0.00") + "-" + overhangFactor.ToString(".00") + "-supportedlayer");
                    }


                    //  this.SupportedLayers.Add(sliceHeight, supportedLayerAsPolyTree);
                    var unsupportedLayer = DifferenceModelSliceLayer(sliceContours, supportedLayerAsPolyTree);
                    ////if (sliceHeight >= 15f && sliceHeight <= 16f)
                    //    {
                    foreach (var verticalSurface in stlModel.Triangles.MagsAIVerticalSurfaces)
                    {
                        verticalSurface.UpdateBoundries(stlModel.Triangles);
                        if (verticalSurface.BottomPoint <= this.BottomPoint)
                        {
                            foreach (var workingSliceHeight in verticalSurface.VerticalSurfaceModelPolygons.Keys)
                            {
                                if (workingSliceHeight >= previousSliceHeight - 0.4f && workingSliceHeight <= sliceHeight)
                                {
                                    //TriangleHelper.SavePolyNodesContourToPng(verticalSurface.VerticalSurfaceModelPolygons[workingSliceHeight]._allPolys, sliceHeight.ToString("0.00") + "-" + workingSliceHeight + "-subtract-vertical-contour");

                                    unsupportedLayer = DifferenceModelSliceLayer(unsupportedLayer, verticalSurface.VerticalSurfaceModelPolygons[workingSliceHeight]);
                                    // TriangleHelper.SavePolyNodesContourToPng(unsupportedLayer._allPolys, sliceHeight.ToString("0.00") + "-unsupported-vertical-contour");
                                }
                            }
                        }
                    }
                    //}

                    //remove supportedLayer from current diff layer and previous found support points
                    //TriangleHelper.SavePolyNodesContourToPng(unsupportedLayer._allPolys, surface.Id.ToString("00") + "-" + sliceHeight.ToString(".00") + "-" + overhangFactor.ToString(".00") + "-surface-unsupported");
                    if (unsupportedLayer._allPolys.Count > 0)
                    {
                        while (unsupportedLayer._allPolys.Where(s => !s.IsHole).Count() > 0)
                        {
                            var skippedUnsupportedLayerPolyNodes = new List<PolyNode>();
                            foreach (var unsupportLayerHole in unsupportedLayer._allPolys.Where(s => s.IsHole))
                            {
                                skippedUnsupportedLayerPolyNodes.Add(unsupportLayerHole);
                            }

                            foreach (var areaToSmallPolygon in unsupportedLayer._allPolys.Where(s => !s.IsHole)){
                                if ((Clipper.Area(areaToSmallPolygon.Contour) / (CONTOURPOINT_TO_VECTORPOINT_FACTOR * CONTOURPOINT_TO_VECTORPOINT_FACTOR)) < 2)
                                {
                                    skippedUnsupportedLayerPolyNodes.Add(areaToSmallPolygon);
                                }
                            }

                            foreach(var removePolygon in skippedUnsupportedLayerPolyNodes)
                            {
                                unsupportedLayer._allPolys.Remove(removePolygon);
                            }

                            skippedUnsupportedLayerPolyNodes.Clear();

                            foreach (var unsupportedLayerPolyNode in unsupportedLayer._allPolys)
                            {
                                    var contourPolyTree = new PolyTree();
                                    contourPolyTree._allPolys.Add(unsupportedLayerPolyNode);
                                    //TriangleHelper.SavePolyNodesContourToPng(contourPolyTree._allPolys, "b");
                                    var supportConeIntersections = MagsAIEngine.CalcContourSupportStructure(contourPolyTree, null, null, selectedMaterialProfile, sliceHeight, this, true, false);
                                    if (supportConeIntersections.Count > 0)
                                    {
                                        var supportConeIntersectionPolyTree = new PolyTree();
                                        foreach (var supportConeIntersection in supportConeIntersections)
                                        {
                                            if (supportConeIntersection.LowestPoints.Count == 0)
                                            {
                                                skippedUnsupportedLayerPolyNodes.Add(unsupportedLayerPolyNode);
                                            }
                                            else
                                            {
                                                var k = 0;
                                                foreach (var supportConeIntersectionPoint in supportConeIntersection.LowestPoints)
                                                {
                                                    var supportConeIntersectionPointAsIntPoint = supportConeIntersectionPoint.Point;
                                                    supportConeIntersectionPointAsIntPoint.Z = (int)(sliceHeight * CONTOURPOINT_TO_VECTORPOINT_FACTOR);
                                                    var addedSupportPoint = new MagsAISurfaceIntersectionData() { TopPoint = supportConeIntersectionPointAsIntPoint, SliceHeight = sliceHeight };
                                                    addedSupportPoint.OverhangDistanceFactor = selectedMaterialProfile.SupportOverhangDistance * overhangFactor;
                                                    var outlineCirclePoints = MagsAIEngine.ConvertSupportPointsToCircles(supportConeIntersectionPointAsIntPoint, selectedMaterialProfile.SupportOverhangDistance * overhangFactor);
                                                    var outlineCirclePolygon = MagsAIEngine.MergeSupportCircles(new List<List<IntPoint>>() { outlineCirclePoints });


                                                    var intersectedPolygons = ContourHelper.IntersectModelSliceLayer(ModelContours[sliceHeight], outlineCirclePolygon);
                                                    var intersectionPointFound = false;
                                               
                                                    foreach (var intersectedPolygon in intersectedPolygons._allPolys.Where(s => !s.IsHole))
                                                    {
                                                        if (ContourHelper.PointExistsInNotHolePolygon(intersectedPolygon, addedSupportPoint.TopPoint))
                                                        {
                                                            supportConeIntersectionPolyTree._allPolys.Add(intersectedPolygon);
                                                            intersectionPointFound = true;
                                                        }
                                                    }

                                                    if (!intersectionPointFound)
                                                    {
                                                        foreach (var intersectedPolygon in intersectedPolygons._allPolys.Where(s => !s.IsHole))
                                                        {
                                                            supportConeIntersectionPolyTree._allPolys.Add(intersectedPolygon);
                                                        }
                                                    }

//                                                TriangleHelper.SavePolyNodesContourToPng(outlineCirclePolygon._allPolys, sliceHeight.ToString("0.00") + "-" + k.ToString());

                                                    addedSupportPoint.UpdateLastSupportedHeight(stlModel);
                                                    addedSupportPoint.UpdateTriangleReference(stlModel);
                                                    if (!this.AddedSupportPoints.ContainsKey(addedSupportPoint.TopPoint))
                                                    {
                                                        this.AddedSupportPoints.Add(addedSupportPoint.TopPoint, addedSupportPoint);
                                                    }

                                                    if (!this.AddedSupportPointsByLayer[sliceHeight].ContainsKey(addedSupportPoint.TopPoint))
                                                    {
                                                        addedSupportPoint.UnsupportedContour = true;
                                                        this.AddedSupportPointsByLayer[sliceHeight].Add(addedSupportPoint.TopPoint, addedSupportPoint);
                                                    }


                                                    k++;
                                                }
                                            }

                                            if (supportConeIntersectionPolyTree._allPolys.Count > 0)
                                            {
                                                unsupportedLayer = DifferenceModelSliceLayer(unsupportedLayer, supportConeIntersectionPolyTree);
                                               // TriangleHelper.SavePolyNodesContourToPng(unsupportedLayer._allPolys, "s");

                                                break;
                                            }
                                        }
                                    

                                }
                            }

                            if (skippedUnsupportedLayerPolyNodes.Count > 0)
                            {
                                foreach (var skippedUnsuppertedLayerPolyNode in skippedUnsupportedLayerPolyNodes)
                                {
                                    unsupportedLayer._allPolys.Remove(skippedUnsuppertedLayerPolyNode);
                                }
                            }
                        }
                    }

                    previousSliceHeight = sliceHeight;
                }


                if (previousSliceHeight == float.MaxValue)
                {
                    previousSliceHeight = sliceHeight;
                }
            }

            return this.AddedSupportPoints;
        }

        internal void CalcSlicesIndexes(Material selectedMaterial, AtumPrinter selectedPrinter, int sliceIndexModulus = 1)
        {
            this.CalcSliceIndexes(selectedMaterial, true);

            var firstSliceIndex = 0;
            var firstSliceHeight = 0f;
            foreach (var sliceHeight in this.SliceIndexes.Keys)
            {
                if (sliceHeight > this.BottomPoint)
                {
                    firstSliceHeight = sliceHeight;
                    break;
                }

                firstSliceIndex++;
            }

            var lastSliceHeight = this.SliceIndexes.Last().Key;

            Parallel.ForEach(MagsAIEngine.SliceHeightsWithModulus.Keys, sliceHeightAsync =>
            {
                var sliceHeight = sliceHeightAsync;
                var sliceIndex = firstSliceIndex;
                if (sliceHeight >= firstSliceHeight && sliceHeight <= lastSliceHeight)
                {
                    var sliceAngledContours = new PolyTree(); ;
                var sliceContours = this.GetSliceContours(sliceIndex, sliceHeight, selectedPrinter, selectedMaterial, out sliceAngledContours);
                    ModelContours.TryAdd(sliceHeight, sliceContours);
                    ModelAngledContours.TryAdd(sliceHeight, sliceAngledContours);
                    sliceIndex++;
                }
                
            });

        }

        private void AddPointToCurrentLayerWhenExistsInContour(PolyTree sliceContours, MagsAISurfaceIntersectionData intersectionData, float sliceHeight)
        {
            foreach (var sliceContour in sliceContours._allPolys.Where(s => !s.IsHole))
            {
                //if (ContourHelper.PointExistsInNotHolePolygon(sliceContour, intersectionData.TopPoint))
                // {
                if (!this.AddedSupportPointsByLayer[sliceHeight].ContainsKey(intersectionData.TopPoint))
                {
                    this.AddedSupportPointsByLayer[sliceHeight].Add(intersectionData.TopPoint, intersectionData);
                }
                break;
                // }
            }

        }
    }
}
