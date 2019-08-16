using Atum.DAL.Hardware;
using Atum.DAL.Managers;
using Atum.DAL.Materials;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAIEngine
    {
        internal static List<MagsAIEdgePoint> DebugPoints = new List<MagsAIEdgePoint>();
        internal static float PixelOffsetOptimisation = 0.4f;

        internal static ConcurrentDictionary<float, PolyTree> ModelLayersFacingDown = new ConcurrentDictionary<float, PolyTree>();
        internal static SortedList<float, int> SliceHeightsWithModulus = new SortedList<float, int>();

        internal static event EventHandler<MagsAIProgressEventArgs> ModelProgressChanged;
        internal static event EventHandler<MagsAIProgressEventArgs> CalcModelAngledSurfaceSupportSliceProcessed;

        private static Dictionary<int, float> sliceIndexKeys = new Dictionary<int, float>();
        internal static List<Vector3> Calculate(STLModel3D stlModel, Material selectedMaterial, AtumPrinter selectedPrinter)
        {
            frmStudioMain.SceneControl.DisableRendering();

            SliceHeightsWithModulus.Clear();
            ModelLayersFacingDown.Clear();
            lock (DebugPoints)
            {
                DebugPoints.Clear();
            }

            //update markeded triangles
            stlModel.MAGSAISelectionOverlay.Triangles = stlModel.GetMAGSAIMarkedTriangles();
            stlModel.MAGSAISelectionOverlay.UpdateBinding();

            ModelLayersFacingDown = new ConcurrentDictionary<float, PolyTree>();
            stlModel.SupportStructure.Clear();

            if (stlModel.Triangles.HorizontalSurfaces != null)
            {
                foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                {
                    if (horizontalSurface != null && horizontalSurface.SupportStructure != null)
                    {
                        horizontalSurface.SupportStructure.Clear();
                    }
                }
            }

            if (stlModel.Triangles.FlatSurfaces != null)
            {
                foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                {
                    if (flatSurface != null && flatSurface.SupportStructure != null)
                    {
                        flatSurface.SupportStructure.Clear();
                    }
                }
            }

            if (selectedMaterial.SupportProfiles == null || selectedMaterial.SupportProfiles.Count == 0)
            {
                selectedMaterial.SupportProfiles.Add(SupportProfile.CreateDefault());
            }
            var selectedMaterialSupportProfile = selectedMaterial.SupportProfiles[0];

            TriangleHelper.CleanPolyNodesContourToPng();

            MemoryHelpers.ForceGCCleanup();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var results = new List<Vector3>();

            stlModel.UpdateBoundries();

            var sliceIndexModulus = 2;
            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 5 });

            stlModel.CalcSliceIndexes(selectedMaterial, true);

            sliceIndexKeys.Clear();
            var sliceKeyIndex = 0;
            foreach (var sliceIndexKey in stlModel.SliceIndexes.Keys)
            {
                sliceIndexKeys.Add(sliceKeyIndex, sliceIndexKey);
                sliceKeyIndex++;
            }
            CalcSliceHeightsWithModulus(stlModel, sliceIndexModulus, sliceIndexKeys);

            //intersections
            LoggingManager.WriteToLog("AutoSupportEngine", "Slice indexes completed in ", stopwatch.ElapsedMilliseconds + "ms");

            var sliceIndex = 0;
            var stopSlice = 100000;
            var amountOfSibblingLayers = 40;
            var debugMode = true;

            var supportLayerPolyTree = new PolyTree();

            var modelLayers = new SortedDictionary<float, PolyTree>();
            var modelAngledLayers = new SortedDictionary<float, PolyTree>();
            var modelWallLayers = new SortedDictionary<float, PolyTree>();
            foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
            {
                modelLayers.Add(sliceHeight, null);
            }

            foreach (var sliceHeight in SliceHeightsWithModulus.Keys)
            {
                //always check if sliceheight contains triangle with angle range
                modelAngledLayers.Add(sliceHeight, null);
                modelWallLayers.Add(sliceHeight, null);
            }

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 10 });

            #region ZSupport
            PreCacheModelLayers(stlModel, modelLayers, modelAngledLayers, modelWallLayers, sliceIndex, debugMode, selectedPrinter, selectedMaterial, stopSlice + 40);

            LoggingManager.WriteToLog("AutoSupportEngine", "Facing down contours in", stopwatch.ElapsedMilliseconds + "ms");

            MemoryHelpers.ForceGCCleanup();

            var totalSupportedLayer = new PolyTree();
            LoggingManager.WriteToLog("AutoSupportEngine", "Pre-caching in", stopwatch.ElapsedMilliseconds + "ms");
            var lowestPointsContours = CalcLowestPointsSupport(stlModel, selectedMaterial, selectedMaterialSupportProfile);

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 20 });
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter ModelContoursFacingDown", stopwatch.ElapsedMilliseconds + "ms");

            MagsAIEngineFilters.DetermineIfFaceDownContoursHasEdgeDown(lowestPointsContours, modelLayers, selectedMaterialSupportProfile.SupportOverhangDistance, stlModel, sliceIndexKeys);
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter DetermineIsFaceDownContoursHasEdgeDown", stopwatch.ElapsedMilliseconds + "ms");

            DetermineIfLowestContoursHasSibblingEdgeDown(lowestPointsContours, stlModel, modelLayers, amountOfSibblingLayers);
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Sibbling layers", stopwatch.ElapsedMilliseconds + "ms");

            MemoryHelpers.ForceGCCleanup();
            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 30 });

            MagsAIEngineFilters.FilterOnFacingDownLowestPointWithEdgePoints(lowestPointsContours, selectedMaterialSupportProfile.SupportTopRadius);
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter FilterOnLowestPointsZIntersections", stopwatch.ElapsedMilliseconds + "ms");

            //get lowest point offset points
            ProcessLowestPointsOffsets(lowestPointsContours, stlModel, modelLayers, selectedMaterial, selectedMaterialSupportProfile);
            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 40 });

            LoggingManager.WriteToLog("AutoSupportEngine Manager", "ModelIntersections", stopwatch.ElapsedMilliseconds + "ms");

            //first contour facing down (z-support)
            DetermineLowestPointModelIntersectionPoints(lowestPointsContours, stlModel);
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Added LowestIntersectionPoints", stopwatch.ElapsedMilliseconds + "ms");
            //stopwatch.Restart();

            MagsAIEngineFilters.FilterOnLowestPointsOffsetWithLowestPointsOffsets(lowestPointsContours, selectedMaterialSupportProfile.SupportTopRadius);
            MagsAIEngineFilters.FilteredByLowestPointOffsetLowerThenLowerPointTop(lowestPointsContours, stlModel);
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter FilterOnLowestOffsetPointsXYDistance", stopwatch.ElapsedMilliseconds + "ms");
            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 45 });

            var supportColors = new System.Drawing.Color[7];
            supportColors[0] = System.Drawing.Color.Aqua;
            supportColors[1] = System.Drawing.Color.Blue;
            supportColors[2] = System.Drawing.Color.Green;
            supportColors[3] = System.Drawing.Color.GreenYellow;
            supportColors[4] = System.Drawing.Color.Yellow;
            supportColors[5] = System.Drawing.Color.Orange;
            supportColors[6] = System.Drawing.Color.Red;

            var currentModelLowestSupportPoints = CalcLowestPointIntersections(lowestPointsContours, modelLayers, stlModel, selectedMaterial, supportColors, selectedMaterialSupportProfile);
            var currentModelLowestSupportPointsToStructure = new ConcurrentDictionary<MagsAIIntersectionData, bool>();
            foreach (var currentModelLowestSupportPoint in currentModelLowestSupportPoints)
            {
                var currentModelLowestSupportPointsValue = currentModelLowestSupportPoint.Value;
                foreach (var a in currentModelLowestSupportPointsValue.Keys)
                {
                    currentModelLowestSupportPointsToStructure.TryAdd(a, false);
                }
            }

            var zsupportTask = AddUnfilterSupportPointsToSupportStructure(currentModelLowestSupportPointsToStructure, stlModel, selectedMaterial, "Z-support");

            #endregion

            #region VerticalSurfaces

            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Process vertical support2", stopwatch.ElapsedMilliseconds + "ms");
            ProcessVerticalWallContours(modelLayers, modelWallLayers);
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Process vertical support", stopwatch.ElapsedMilliseconds + "ms");
            #endregion

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 45 });

            //overhang support
            var currentModelOtherPoints = new SortedDictionary<float, PolygonSupportPointCollection>();
            var overhangSupportPoints = ProcessModelOverhangSupport(stlModel, modelLayers, lowestPointsContours, currentModelOtherPoints, selectedMaterialSupportProfile);
            var overhangSupportTask = AddUnfilterSupportPointsToSupportStructure(overhangSupportPoints, stlModel, selectedMaterial, "Overhang support");

            foreach (var overhangSupportPoint in overhangSupportPoints)
            {
                if (!currentModelLowestSupportPoints.ContainsKey(overhangSupportPoint.Key.SliceHeight))
                {
                    currentModelLowestSupportPoints.TryAdd(overhangSupportPoint.Key.SliceHeight, new ConcurrentDictionary<MagsAIIntersectionData, bool>());
                }

                currentModelLowestSupportPoints[overhangSupportPoint.Key.SliceHeight].TryAdd(overhangSupportPoint.Key, false);
            }

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 50 });

            //other points
            var otherSupportPoints = ProcessOtherPointsToModelSupportStructure(currentModelLowestSupportPoints, currentModelOtherPoints, modelLayers, stlModel, selectedMaterial, selectedMaterialSupportProfile);
            var otherSupportTask = AddUnfilterSupportPointsToSupportStructure(otherSupportPoints, stlModel, selectedMaterial, "Other support points");

            //angled support
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Creating2 angled support", stopwatch.ElapsedMilliseconds + "ms");
            var modelAngledSupportPoints = ProcessAngledContours(modelLayers, modelAngledLayers, modelWallLayers, currentModelLowestSupportPoints, stlModel, selectedMaterialSupportProfile, selectedMaterial);

            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Creating angled support", stopwatch.ElapsedMilliseconds + "ms");
            var angledSupportTask = Task.Run(() =>
            {
                Parallel.ForEach(modelAngledSupportPoints.Values, supportIntersectionPointAsync =>
                {
                    var supportIntersectionPoint = supportIntersectionPointAsync;
                    supportIntersectionPoint.UpdateTriangleReference(stlModel, selectedMaterialSupportProfile);
                    supportIntersectionPoint.UpdateLastSupportedHeight(stlModel, modelAngledLayers, selectedMaterialSupportProfile);
                    var supportCone = SupportEngine.AddMagsAISupportCone(stlModel, supportIntersectionPoint.ModelIntersection, supportIntersectionPoint.RenderColor, selectedMaterial, supportIntersectionPoint._trianglesWithinXYRange, supportIntersectionPoint.LastSupportedSliceHeight);
                    if (supportCone != null)
                    {
                        //var angledSurfaceSupport = new MagsAISurfaceIntersectionData();
                        //angledSurfaceSupport.TopPoint = new IntPoint(new Vector3Class(supportCone.ModelIntersectionPoint));
                        //angledSurfaceSupport.ModelIntersection = supportIntersectionPoint.ModelIntersection;
                        //angledSurfaceSupport.ModelIntersection.IntersectionPoint = new Vector3Class(supportCone.ModelIntersectionPoint);
                        //angledSurfaceSupport.UpdateLastSupportedHeight(stlModel, modelAngledLayers, selectedMaterialSupportProfile);
                        //angledSurfaceSupport.UpdateTriangleReference(stlModel, selectedMaterialSupportProfile);
                        //angledSurfaceSupport.CalcWallThickness();
                        //angledSurfaceSupport.BottomPoint = new Vector3Class(supportCone.Position.X, supportCone.Position.Y, supportCone.BottomPoint);

                        //  MagsAIEngineFilters.FilterOnAngledPointAndWallThickness(angledSurfaceSupport, stlModel);

                        //  if (angledSurfaceSupport.Filter != typeOfAutoSupportFilter.None)
                        //  {
                        //      stlModel.SupportStructure.Remove(supportCone);
                        //  }

                    }
                });

                LoggingManager.WriteToLog("AutoSupportEngine Manager", "Created angled support", stopwatch.ElapsedMilliseconds + "ms");
            });


            zsupportTask.Wait();
            otherSupportTask.Wait();
            angledSupportTask.Wait();

            stlModel.UpdateSupportBasement();

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 100 });


            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Total Support cones", stlModel.SupportStructure.Count.ToString());
            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Total MAGS AI time", stopwatch.ElapsedMilliseconds + "ms");

            MessageBox.Show(stopwatch.ElapsedMilliseconds + "ms");

            if (selectedMaterialSupportProfile.SupportIntermittedConnectionHeight == 0)
            {
                selectedMaterialSupportProfile.SupportIntermittedConnectionHeight = 0.5f;
            }
            foreach (var supportCone in stlModel.SupportStructure.OfType<SupportConeV2>())
            {
                supportCone.UpdateInterlinkConnections(selectedMaterialSupportProfile);
                //  break;
            }

            SliceHeightsWithModulus.Clear();
            ModelLayersFacingDown.Clear();

            Task.Run(() => { 
            MemoryHelpers.ForceGCCleanup();
            MemoryHelpers.ForceGCCleanup();
        });

            stlModel.UpdateBinding();

            return results;
        }


        private static ConcurrentDictionary<IntPoint, MagsAISurfaceIntersectionData> CalcModelAngledSurfaceSupportPoints(SortedDictionary<float, PolyTree> modelContours, SortedDictionary<float, PolyTree> modelAngledContours,
              SortedDictionary<float, PolyTree> modelWallContours,
              ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> currentModelSupportPoints,
              SupportProfile selectedMaterialProfile, STLModel3D stlModel)
        {
            var addedSupportPoints = new ConcurrentDictionary<IntPoint, MagsAISurfaceIntersectionData>();
            var addedSupportPointsByLayer = new Dictionary<float, Dictionary<IntPoint, MagsAISurfaceIntersectionData>>();
            var previousSupportedLayerAsPolyTree = new PolyTree();

            //create support cones that exists in each layer
            var previousSliceHeight = float.MaxValue;
            var modelAngledSliceIndexes = modelAngledContours.Keys;
            var processCachedLayers = 0;

            foreach (var sliceHeight in modelAngledSliceIndexes)
            {
                //loop through all layers
                var supportedLayerAsPolyTree = new PolyTree();
                var updateCachedSupportedLayerContour = false;

                if (!addedSupportPointsByLayer.ContainsKey(sliceHeight))
                {
                    addedSupportPointsByLayer.Add(sliceHeight, new Dictionary<IntPoint, MagsAISurfaceIntersectionData>());
                }

                if (modelAngledContours[sliceHeight] != null && modelAngledContours[sliceHeight]._allPolys.Count > 0)
                {
                    var modelAngledContoursBySliceHeight = modelAngledContours[sliceHeight];

                    //check if supportcones where allready added using other algorithms 
                    var supportSliceHeightsWithinRange = new List<float>() { (float)Math.Round(previousSliceHeight - (sliceHeight - previousSliceHeight), 2), sliceHeight + (sliceHeight - previousSliceHeight) };
                    var supportSliceHeightsWithinRangeMin = supportSliceHeightsWithinRange[0];
                    var supportSliceHeightsWithinRangeMax = supportSliceHeightsWithinRange[1];
                    var supportSliceHeightInclude = sliceHeight + (sliceHeight - previousSliceHeight);
                    foreach (var supportPointIndex in currentModelSupportPoints.Where(s => s.Key <= supportSliceHeightInclude))
                    {
                        foreach (var supportPointValue in supportPointIndex.Value.Keys)
                        {
                            if (supportPointValue.Filter == typeOfAutoSupportFilter.None && supportPointValue.SliceHeight <= sliceHeight + (sliceHeight - previousSliceHeight) && supportPointValue.LastSupportedSliceHeight >= supportSliceHeightsWithinRangeMin)
                            {
                                var supportSlicePointAsIntPoint = new IntPoint(supportPointValue.TopPoint);
                                if (!addedSupportPointsByLayer.ContainsKey(previousSliceHeight))
                                {
                                    addedSupportPointsByLayer.Add(previousSliceHeight, new Dictionary<IntPoint, MagsAISurfaceIntersectionData>());
                                }
                                if (!addedSupportPointsByLayer[previousSliceHeight].ContainsKey(supportSlicePointAsIntPoint))
                                {
                                    var supportConePoint2 = new MagsAISurfaceIntersectionData();
                                    supportConePoint2.TopPoint = supportSlicePointAsIntPoint;
                                    supportConePoint2.SliceHeight = sliceHeight;
                                    supportConePoint2.ModelIntersection = supportPointValue.ModelIntersection;
                                    supportConePoint2.OverhangDistance = selectedMaterialProfile.SupportOverhangDistance;
                                    supportConePoint2.LastSupportedSliceHeight = supportPointValue.LastSupportedSliceHeight;
                                    supportConePoint2.LastSupportedCenterSliceHeight = supportPointValue.LastSupportedSliceHeight;
                                    supportConePoint2.LastSupportedCenterAngle = OpenTK.MathHelper.RadiansToDegrees(Vector3Class.CalculateAngle(supportPointValue.ModelIntersection.Normal, Vector3Class.UnitZ));

                                    addedSupportPointsByLayer[previousSliceHeight].Add(supportSlicePointAsIntPoint, supportConePoint2);
                                }
                            }
                        }

                    }


                    //combine all previous found points
                    supportedLayerAsPolyTree = new PolyTree();
                    if (addedSupportPointsByLayer.ContainsKey(previousSliceHeight))
                    {
                        var previousSupportedPoints = addedSupportPointsByLayer[previousSliceHeight].Values.Where(s => s != null && s.LastSupportedSliceHeight >= sliceHeight && s.SliceHeight <= sliceHeight).ToArray();

                        foreach (var previousSupportPoint in previousSupportedPoints)
                        {
                            if (!addedSupportPointsByLayer[sliceHeight].ContainsKey(previousSupportPoint.TopPoint))
                            {
                                addedSupportPointsByLayer[sliceHeight].Add(previousSupportPoint.TopPoint, previousSupportPoint);
                            }
                        }

                        if (previousSupportedLayerAsPolyTree._allPolys.Count > 0 && addedSupportPointsByLayer[previousSliceHeight].Keys.SequenceEqual(addedSupportPointsByLayer[sliceHeight].Keys))
                        {
                            supportedLayerAsPolyTree = previousSupportedLayerAsPolyTree;
                            processCachedLayers++;
                        }
                        else
                        {
                            var supportCircles = new List<List<IntPoint>>();
                            foreach (var previousSupportPoint in previousSupportedPoints)
                            {
                                supportCircles.Add(ConvertSupportPointsToCircles(previousSupportPoint.TopPoint, previousSupportPoint.OverhangDistance * previousSupportPoint.SupportOverhangDistanceFactor));
                            }
                            var outlineCirclePolygon = MagsAIEngine.MergeSupportCircles(supportCircles);
                            supportedLayerAsPolyTree._allPolys.AddRange(outlineCirclePolygon._allPolys);
                        }

                    }

                    var unsupportedLayer = DifferenceModelSliceLayer(modelAngledContoursBySliceHeight, supportedLayerAsPolyTree);

                    //    //      TriangleHelper.SavePolyNodesContourToPng(unsupportedLayer._allPolys, sliceHeight.ToString("00.00") + "-unsupportedLayer");
                    // if (unsupportedLayer._allPolys.Count > 0)
                    // {
                    while (unsupportedLayer._allPolys.Where(s => !s.IsHole).Count() > 0)
                    {
                        var skippedUnsupportedLayerPolyNodes = new List<PolyNode>();
                        foreach (var unsupportLayerHole in unsupportedLayer._allPolys.Where(s => s.IsHole))
                        {
                            skippedUnsupportedLayerPolyNodes.Add(unsupportLayerHole);
                        }

                        foreach (var areaToSmallPolygon in unsupportedLayer._allPolys.Where(s => !s.IsHole))
                        {
                            if ((Clipper.Area(areaToSmallPolygon.Contour) / (CONTOURPOINT_TO_VECTORPOINT_FACTOR * CONTOURPOINT_TO_VECTORPOINT_FACTOR)) < 2)
                            {
                                skippedUnsupportedLayerPolyNodes.Add(areaToSmallPolygon);
                            }
                        }

                        foreach (var removePolygon in skippedUnsupportedLayerPolyNodes)
                        {
                            unsupportedLayer._allPolys.Remove(removePolygon);
                        }

                        skippedUnsupportedLayerPolyNodes.Clear();

                        foreach (var unsupportedLayerPolyNode in unsupportedLayer._allPolys)
                        {
                            var contourPolyTree = new PolyTree();
                            contourPolyTree._allPolys.Add(unsupportedLayerPolyNode);
                            var supportConeIntersections = MagsAIEngine.CalcContourSupportStructure(contourPolyTree, null, null, selectedMaterialProfile, sliceHeight, stlModel, true, false);
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
                                        Parallel.ForEach(supportConeIntersection.LowestPoints, supportConeIntersectionPointAsync =>
                                        {
                                            // foreach (var supportConeIntersectionPoint in supportConeIntersection.LowestPoints)
                                            // {
                                            try
                                            {

                                                var supportConeIntersectionPoint = supportConeIntersectionPointAsync;
                                                var supportConeIntersectionPointAsIntPoint = supportConeIntersectionPoint.Point;
                                                supportConeIntersectionPointAsIntPoint.Z = (int)(sliceHeight * CONTOURPOINT_TO_VECTORPOINT_FACTOR);
                                                var addedSupportPoint = new MagsAISurfaceIntersectionData() { TopPoint = supportConeIntersectionPointAsIntPoint, SliceHeight = sliceHeight };
                                                addedSupportPoint.OverhangDistance = selectedMaterialProfile.SupportOverhangDistance;

                                                //get intersected angles
                                                addedSupportPoint.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                                                addedSupportPoint.UpdateLastSupportedHeight(stlModel, modelAngledContours, selectedMaterialProfile);


                                                var outlineCirclePoints = MagsAIEngine.ConvertSupportPointsToCircles(supportConeIntersectionPointAsIntPoint, selectedMaterialProfile.SupportOverhangDistance * addedSupportPoint.SupportOverhangDistanceFactor);
                                                var outlineCirclePolygon = MagsAIEngine.MergeSupportCircles(new List<List<IntPoint>>() { outlineCirclePoints });


                                                //check if point is within polygon
                                                var intersectionPointFound = false;
                                                var intersectedPolygons = ContourHelper.IntersectModelSliceLayer(modelAngledContoursBySliceHeight, outlineCirclePolygon);

                                                var intersectedPolygonsNotHole = intersectedPolygons._allPolys.Where(s => !s.IsHole);
                                                foreach (var intersectedPolygon in intersectedPolygonsNotHole)
                                                {
                                                    if (ContourHelper.PointExistsInNotHolePolygon(intersectedPolygon, addedSupportPoint.TopPoint))
                                                    {
                                                        supportConeIntersectionPolyTree._allPolys.Add(intersectedPolygon);
                                                        intersectionPointFound = true;
                                                        break;
                                                    }
                                                }

                                                if (!intersectionPointFound)
                                                {
                                                    foreach (var intersectedPolygon in intersectedPolygonsNotHole)
                                                    {
                                                        supportConeIntersectionPolyTree._allPolys.Add(intersectedPolygon);
                                                    }
                                                }


                                                if (!addedSupportPoints.ContainsKey(addedSupportPoint.TopPoint))
                                                {
                                                    addedSupportPoints.TryAdd(addedSupportPoint.TopPoint, addedSupportPoint);
                                                }

                                                if (!addedSupportPointsByLayer[sliceHeight].ContainsKey(addedSupportPoint.TopPoint))
                                                {
                                                    addedSupportPoint.UnsupportedContour = true;
                                                    addedSupportPointsByLayer[sliceHeight].Add(addedSupportPoint.TopPoint, addedSupportPoint);
                                                }
                                            }
                                            catch (Exception exc)
                                            {

                                            }

                                            k++;
                                        }
                                   );
                                    }

                                    if (supportConeIntersectionPolyTree._allPolys.Count > 0)
                                    {
                                        unsupportedLayer = DifferenceModelSliceLayer(unsupportedLayer, supportConeIntersectionPolyTree);
                                        updateCachedSupportedLayerContour = true;

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
                    //    }
                }

                CalcModelAngledSurfaceSupportSliceProcessed?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = (int)(50f + (40f / modelAngledSliceIndexes.Last()) * sliceHeight) });

                if (updateCachedSupportedLayerContour)
                {
                    previousSupportedLayerAsPolyTree = new PolyTree();
                }
                else
                {
                    previousSupportedLayerAsPolyTree = supportedLayerAsPolyTree;
                }
                previousSliceHeight = sliceHeight;

            }


            LoggingManager.WriteToLog("t", "t", processCachedLayers.ToString());
            return addedSupportPoints;
        }

        private static void ProcessVerticalWallContours(SortedDictionary<float, PolyTree> modelContours, SortedDictionary<float, PolyTree> modelWallContours)
        {
            SortedDictionary<float, PolyTree> modelWallCombinedContours = new SortedDictionary<float, PolyTree>();
            //Parallel.ForEach(modelContours.Keys, sliceHeightAsync =>
            foreach (var sliceHeightAsync in modelContours.Keys)
            {
                var sliceHeight = sliceHeightAsync;


                //calc the wall thickness polygon
                var strengthWallSliceHeights = new List<float>() { sliceHeight - 1f, sliceHeight - 2f };
                var validWallSlicesFound = true;
                foreach (var strengthWallSliceHeight in strengthWallSliceHeights)
                {
                    if (!modelWallContours.ContainsKey(strengthWallSliceHeight) || modelWallContours[strengthWallSliceHeight] == null || modelWallContours[strengthWallSliceHeight]._allPolys.Count == 0)
                    {
                        validWallSlicesFound = false;
                        break;
                    }
                }

                if (validWallSlicesFound)
                {
                    if (modelWallContours.ContainsKey(sliceHeight) && modelWallContours[sliceHeight] != null && modelWallContours[sliceHeight]._allPolys.Count > 0)
                    {
                        //combine previous wall layers to determine the strength of the wall
                        var strengthWallPolyTree = modelWallContours[sliceHeight];
                        var wallStrengthSkippedPolyNodes = new List<PolyNode>();
                        foreach (var strengthWallSliceHeight in strengthWallSliceHeights)
                        {
                            foreach (var polyNode in strengthWallPolyTree._allPolys)
                            {
                                if (Clipper.Area(polyNode.Contour) < 20 * CONTOURPOINT_TO_VECTORPOINT_FACTOR) //when less than a pixels skip wall polynode
                                {
                                    wallStrengthSkippedPolyNodes.Add(polyNode);
                                }
                            }

                            foreach (var t in wallStrengthSkippedPolyNodes)
                            {
                                strengthWallPolyTree._allPolys.Remove(t);
                            }

                            strengthWallPolyTree = IntersectModelSliceLayer(strengthWallPolyTree, modelWallContours[strengthWallSliceHeight]);
                        }

                        lock (modelWallCombinedContours)
                        {
                            modelWallCombinedContours.Add(sliceHeight, strengthWallPolyTree);
                        }
                    }

                }
            }//);

        }

        private static ConcurrentDictionary<IntPoint, MagsAISurfaceIntersectionData> ProcessAngledContours(SortedDictionary<float, PolyTree> modelContours,
            SortedDictionary<float, PolyTree> modelAngledContours, SortedDictionary<float, PolyTree> modelWallContours,
            ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> currentModelSupportPoints, STLModel3D stlModel, SupportProfile selectedMaterialSupportProfile, Material selectedMaterial)
        {
            //first combine all contours

            var previousAngledContourSliceHeight = 0f;
            var angledContourSliceHeights = modelAngledContours.Keys.ToArray(); //prevent enumator updates exception on keys

            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Creating layers", "");

            foreach (var angledContourIndex in angledContourSliceHeights)
            {
                if (previousAngledContourSliceHeight > 0f && modelAngledContours[previousAngledContourSliceHeight] != null && modelAngledContours[angledContourIndex] != null && modelAngledContours[angledContourIndex]._allPolys.Count > 0)
                {
                    modelAngledContours[angledContourIndex] = UnionModelSliceLayer(modelAngledContours[previousAngledContourSliceHeight], modelAngledContours[angledContourIndex]);
                    modelAngledContours[angledContourIndex] = IntersectModelSliceLayer(modelAngledContours[angledContourIndex], modelContours[angledContourIndex]);
                    ReducePointsInPolyTree(modelAngledContours[angledContourIndex], PixelOffsetOptimisation);
                }

                previousAngledContourSliceHeight = angledContourIndex;
            }

            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Created layers", "");

            //process support cones
            var modelAngledSupportPoints = CalcModelAngledSurfaceSupportPoints(modelContours, modelAngledContours, modelWallContours, currentModelSupportPoints, selectedMaterialSupportProfile, stlModel);

            LoggingManager.WriteToLog("AutoSupportEngine Manager", "Created layers2", "");

            //Filter found points on wall thickness
            Parallel.ForEach(modelAngledSupportPoints.Values, supportIntersectionPointAsync =>
            {
                var supportIntersectionPoint = supportIntersectionPointAsync;
                if (supportIntersectionPoint != null)
                {
                    if (supportIntersectionPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        supportIntersectionPoint.UpdateTriangleReference(stlModel, selectedMaterialSupportProfile);

                        if (RegistryManager.RegistryProfile.DebugMode)
                        {
                            supportIntersectionPoint.RenderColor = System.Drawing.Color.Pink;
                        }
                        else
                        {
                            supportIntersectionPoint.RenderColor = Properties.Settings.Default.DefaultSupportColor;
                        }
                    }
                }
            });

            return modelAngledSupportPoints;
        }

        private static Task AddUnfilterSupportPointsToSupportStructure(ConcurrentDictionary<MagsAIIntersectionData, bool> supportPoints, STLModel3D stlModel, Material selectedMaterial, string taskName)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            return Task.Run(() =>
            {
                LoggingManager.WriteToLog("AutoSupportEngine Manager", "Creating " + taskName, "");

                Parallel.ForEach(supportPoints.Keys, a =>
                {
                    SupportEngine.AddMagsAISupportCone(stlModel, a.ModelIntersection, a.RenderColor, selectedMaterial, a._trianglesWithinXYRange, a.LastSupportedSliceHeight);
                });

                LoggingManager.WriteToLog("AutoSupportEngine Manager", "Created " + taskName, stopwatch.ElapsedMilliseconds + "ms");
            }
            );
        }

        private static void CalcSliceHeightsWithModulus(STLModel3D stlModel, int sliceIndexModulus, Dictionary<int, float> sliceIndexKeys)
        {
            for (var sliceIndex = 0; sliceIndex < stlModel.SliceIndexes.Count; sliceIndex++)
            {
                if (sliceIndex % sliceIndexModulus == 0)
                {
                    var sliceHeight = sliceIndexKeys[sliceIndex];
                    SliceHeightsWithModulus.Add(sliceHeight, sliceIndex);
                }
            }

        }

        internal static float FindSliceHeightBelowModelIntersectionPointUsingModulus(STLModel3D stlModel, Vector3Class intersectionPoint, float lastSupportSliceHeight)
        {
            TriangleIntersection[] modelIntersectionPoints = null;
            IntersectionProvider.IntersectTriangle(intersectionPoint, new Vector3Class(0, 0, 1), (object)stlModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out modelIntersectionPoints);
            var currentDistance = float.MaxValue;
            var currentIntersectionPoint = new Vector3Class();
            if (modelIntersectionPoints != null)
            {
                foreach (var modelIntersectionPoint in modelIntersectionPoints)
                {
                    if (modelIntersectionPoint != null)
                    {
                        if (modelIntersectionPoint.IntersectionPoint.Z < lastSupportSliceHeight)
                        {
                            var newDistance = lastSupportSliceHeight - modelIntersectionPoint.IntersectionPoint.Z;
                            if (newDistance < currentDistance)
                            {
                                currentDistance = newDistance;
                                currentIntersectionPoint = modelIntersectionPoint.IntersectionPoint;
                            }
                        }
                    }
                }
            }

            if (currentDistance != float.MaxValue)
            {
                var previousSliceHeight = float.MaxValue;
                foreach (var sliceHeightWithModulus in SliceHeightsWithModulus.Keys)
                {
                    if (sliceHeightWithModulus > currentIntersectionPoint.Z)
                    {
                        return previousSliceHeight;
                    }

                    previousSliceHeight = sliceHeightWithModulus;
                }
            }
            else
            {

            }

            return float.MaxValue;
        }

        private static ConcurrentDictionary<MagsAIIntersectionData, bool> ProcessOtherPointsToModelSupportStructure(ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> currentModelSupportPoints,
            SortedDictionary<float, PolygonSupportPointCollection> currentModelOtherSupportPoints, SortedDictionary<float, PolyTree> modelLayers,
            STLModel3D stlModel, Material selectedMaterial, SupportProfile selectedMaterialProfile)
        {
            var supportPoints = new ConcurrentDictionary<MagsAIIntersectionData, bool>();

            //build current supportPoints list that is filtered using 
            foreach (var supportPointContour in currentModelOtherSupportPoints.Values)
            {
                Parallel.ForEach(supportPointContour.OverhangPoints, supportIntersectionPointAsync =>
                //foreach (var supportIntersectionPoint in supportPointContour.OverhangPoints)
                {
                    var supportIntersectionPoint = supportIntersectionPointAsync;
                    if (supportIntersectionPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        supportIntersectionPoint.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                        supportIntersectionPoint.UpdateLastSupportedHeight(stlModel, selectedMaterialProfile, modelLayers);
                        if (RegistryManager.RegistryProfile.DebugMode)
                        {
                            supportIntersectionPoint.RenderColor = System.Drawing.Color.Orange;
                        }
                        else
                        {
                            supportIntersectionPoint.RenderColor = Properties.Settings.Default.DefaultSupportColor;
                        }

                        if (!currentModelSupportPoints.ContainsKey(supportIntersectionPoint.SliceHeight))
                        {
                            currentModelSupportPoints.TryAdd(supportIntersectionPoint.SliceHeight, new ConcurrentDictionary<MagsAIIntersectionData, bool>());
                        }

                        currentModelSupportPoints[supportIntersectionPoint.SliceHeight].TryAdd(supportIntersectionPoint, false);
                        supportPoints.TryAdd(supportIntersectionPoint, false);

                    }
                }
                );

                //edge points
                Parallel.ForEach(supportPointContour.EdgePoints, supportIntersectionPointAsync =>
                //foreach (var edgeIntersectionPoint in supportPointContour.EdgePoints)
                {
                    var edgeIntersectionPoint = supportIntersectionPointAsync;
                    if (edgeIntersectionPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        edgeIntersectionPoint.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                        edgeIntersectionPoint.UpdateLastSupportedHeight(stlModel, selectedMaterialProfile, modelLayers);

                        if (RegistryManager.RegistryProfile.DebugMode)
                        {
                            edgeIntersectionPoint.RenderColor = System.Drawing.Color.Teal;
                        }
                        else
                        {
                            edgeIntersectionPoint.RenderColor = Properties.Settings.Default.DefaultSupportColor;
                        }

                        if (!currentModelSupportPoints.ContainsKey(edgeIntersectionPoint.SliceHeight))
                        {
                            currentModelSupportPoints.TryAdd(edgeIntersectionPoint.SliceHeight, new ConcurrentDictionary<MagsAIIntersectionData, bool>());
                        }

                        currentModelSupportPoints[edgeIntersectionPoint.SliceHeight].TryAdd(edgeIntersectionPoint, false);
                    }
                }
                );
            }

            return supportPoints;

        }

        private static ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>> CalcLowestPointIntersections(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, SortedDictionary<float, PolyTree> modelLayers, STLModel3D stlModel, Material selectedMaterial, System.Drawing.Color[] supportColors, SupportProfile selectedMaterialProfile)
        {
            var currentModelSupportPoints = new ConcurrentDictionary<float, ConcurrentDictionary<MagsAIIntersectionData, bool>>();
            foreach (var lowestPointContourItem in lowestPointsContours)
            {
                Parallel.ForEach(lowestPointContourItem.Key.LowestIntersectionPoints, lowestPointSupportPointAsync =>
                //  foreach (var lowestPointSupportPointAsync in lowestPointContourItem.Key.LowestIntersectionPoints)
                {
                    var lowestPointSupportPoint = lowestPointSupportPointAsync;
                    lowestPointSupportPoint.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                    lowestPointSupportPoint.UpdateLastSupportedHeight(stlModel, selectedMaterialProfile, modelLayers);

                    if (!currentModelSupportPoints.ContainsKey(lowestPointSupportPoint.SliceHeight))
                    {
                        currentModelSupportPoints.TryAdd(lowestPointSupportPoint.SliceHeight, new ConcurrentDictionary<MagsAIIntersectionData, bool>());
                    }

                    lowestPointSupportPoint.RenderColor = Properties.Settings.Default.DefaultSupportColor;
                    currentModelSupportPoints[lowestPointSupportPoint.SliceHeight].TryAdd(lowestPointSupportPoint, false);
                    //break;
                });

                Parallel.ForEach(lowestPointContourItem.Key.EdgeIntersectionPoints, edgeIntersectionPointAsync =>
                {
                    var edgeSupportPoint = edgeIntersectionPointAsync;
                    edgeSupportPoint.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                    edgeSupportPoint.UpdateLastSupportedHeight(stlModel, selectedMaterialProfile, modelLayers);

                    if (!currentModelSupportPoints.ContainsKey(edgeSupportPoint.SliceHeight))
                    {
                        currentModelSupportPoints.TryAdd(edgeSupportPoint.SliceHeight, new ConcurrentDictionary<MagsAIIntersectionData, bool>());
                    }

                    edgeSupportPoint.RenderColor = Properties.Settings.Default.DefaultSupportColor;

                    if (RegistryManager.RegistryProfile.DebugMode)
                    {
                        edgeSupportPoint.RenderColor = Color.Teal;
                    }

                    currentModelSupportPoints[edgeSupportPoint.SliceHeight].TryAdd(edgeSupportPoint, false);
                });

                foreach (var contourOffsetItem in lowestPointContourItem.Key.LowestPointsWithOffsetIntersectionPoint)
                {
                    //add lowest point support cones
                    Parallel.ForEach(contourOffsetItem.Value, supportPointAsync =>
                    {
                        var supportPoint = supportPointAsync;
                        if (supportPoint.Filter == typeOfAutoSupportFilter.None)
                        {
                            if (!currentModelSupportPoints.ContainsKey(supportPoint.SliceHeight))
                            {
                                currentModelSupportPoints.TryAdd(supportPoint.SliceHeight, new ConcurrentDictionary<MagsAIIntersectionData, bool>());
                            }

                            supportPoint.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                            supportPoint.UpdateLastSupportedHeight(stlModel, selectedMaterialProfile, modelLayers);
                            currentModelSupportPoints[supportPoint.SliceHeight].TryAdd(supportPoint, false);

                            var supportConeColor = supportColors[0];
                            if (selectedMaterialProfile.SupportLowestPointsOffset.Count >= 2 && contourOffsetItem.Key == selectedMaterialProfile.SupportLowestPointsOffset[1])
                            {
                                supportConeColor = supportColors[1];
                            }
                            else if (selectedMaterialProfile.SupportLowestPointsOffset.Count >= 3 && contourOffsetItem.Key == selectedMaterialProfile.SupportLowestPointsOffset[2])
                            {
                                supportConeColor = supportColors[2];
                            }
                            else if (selectedMaterialProfile.SupportLowestPointsOffset.Count >= 4 && contourOffsetItem.Key == selectedMaterialProfile.SupportLowestPointsOffset[3])
                            {
                                supportConeColor = supportColors[3];
                            }

                            if (lowestPointContourItem.Key.HasComplexConnectedUpperChildContour)
                            {
                                supportConeColor = supportColors[5];
                            }

                            if (supportPoint.TopPoint.Z - lowestPointContourItem.Key.SliceHeight > 40 * 0.05)
                            {
                                supportConeColor = supportColors[6];
                            }
                            else
                            {
                                if (!lowestPointContourItem.Key.HasComplexConnectedUpperChildContour)
                                {
                                    //supportPoint.UpdateTriangleReference(stlModel);
                                    //supportPoint.UpdateLastSupportedHeight(stlModel, selectedMaterial, modelLayers);

                                    //async
                                    var topHeight = supportPoint.ModelIntersection.IntersectionPoint.Z - selectedMaterialProfile.SupportBottomHeight;
                                    supportPoint.RenderColor = Properties.Settings.Default.DefaultSupportColor;
                                    return;
                                }
                            }
                        }
                    });

                }
            }

            return currentModelSupportPoints;
        }

        private static void DetermineLowestPointModelIntersectionPoints(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, STLModel3D stlModel)
        {
            foreach (var facingDownContourItem in lowestPointsContours)
            {
                //add support
                foreach (var lowestPointSupportPoint in facingDownContourItem.Key.LowestPoints)
                {
                    if (lowestPointSupportPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        //add lowest point support cones
                        var triangleIntersectionBelow = new Vector3Class();
                        var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, lowestPointSupportPoint.Point, facingDownContourItem.Key.SliceHeight - 0.05f, triangleIntersectionBelow);
                        if (triangleIntersection.IntersectionPoint.Z > facingDownContourItem.Key.SliceHeight + 1f)
                        {
                            triangleIntersection.IntersectionPoint.Z = facingDownContourItem.Key.SliceHeight;
                        }
                        facingDownContourItem.Key.LowestIntersectionPoints.Add(new MagsAIIntersectionData() { BottomPoint = triangleIntersectionBelow, TopPoint = triangleIntersection.IntersectionPoint, SliceHeight = facingDownContourItem.Key.SliceHeight });
                    }
                }
            }
        }

        //private static void CalcVerticalSurfaces(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, STLModel3D stlModel, SortedDictionary<float, PolyTree> modelLayers, Material selectedMaterial, AtumPrinter selectedPrinter)
        //{
        //    var stopWatch = new Stopwatch();
        //    stopWatch.Start();
        //    foreach (var surface in stlModel.Triangles.GetSurfaceBetweenAngles(85, 95))
        //    {
        //        surface.CalcVerticalModel(stlModel, selectedMaterial, selectedPrinter, SliceHeightsWithModulus, true);
        //        stlModel.Triangles.MagsAIVerticalSurfaces.Add(surface);
        //    }

        //    LoggingManager.WriteToLog("MAGS AI", "ProcessVerticalSurfaces", stopWatch.ElapsedMilliseconds + " ms");
        //}

        private static void ProcessLowestPointsOffsets(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, STLModel3D stlModel, SortedDictionary<float, PolyTree> modelLayers, Material selectedMaterial, SupportProfile selectedMaterialProfile)
        {
            var supportConeOverhangDistance = selectedMaterialProfile.SupportTopRadius * 3f;

            Parallel.ForEach(lowestPointsContours, facingDownContourItem =>
            //foreach (var lowestPointContourItem in lowestPointsContours)
            {
                var lowestPointContour = facingDownContourItem;
                var lowestPointContourParent = lowestPointContour.Key;
                var lowestPointContourParentPolygon = lowestPointContourParent.Polygon;
                var lowestPointContourParentSupportPoints = lowestPointContourParent.LowestPoints;
                var lowestPointContourParentSibblingPolyNodes = lowestPointContour.Value;
                var lowestPointContourParentSliceIndex = lowestPointContourParent.SliceIndex;
                //   var lowestPointContourFirstOverhangContour = ClipperOffset(lowestPointContourParentPolygon.Contour, supportConeOverhangDistance);
                //   var lowestPointContourFirstOverhangContourHasHoleChild = !lowestPointContourParentPolygon.IsHole && lowestPointContourParentPolygon.ChildCount > 0;

                //TriangleHelper.SavePolyNodesContourToPng(new List<PolyNode>() { lowestPointContour.Key.Polygon }, lowestPointContour.Key.SliceHeight + "-facingdown");

                if (lowestPointContour.Value.Count > 0 && lowestPointContourParent.LowestPoints.Count <= 5)
                {
                    //  if (lowestPointContour.Key.SliceHeight > 21) { 
                    //determine the intersection next layer intersection point by using the listOfLowestPointsOffset values 
                    if (!lowestPointContourParent.HasEdgeDown)
                    {
                        var listOfLowestPointsDistanceOffsetIndex = 0;
                        var previousListOfLowestPointOffset = -1f;

                        foreach (var listOfLowestPointOffset in selectedMaterialProfile.SupportLowestPointsOffset)
                        {
                            var offsetSupportDistance = supportConeOverhangDistance + (listOfLowestPointsDistanceOffsetIndex * supportConeOverhangDistance) + selectedMaterialProfile.SupportTopRadius;
                            if (previousListOfLowestPointOffset == -1 || lowestPointContourParent.LowestPointsWithOffset.ContainsKey(previousListOfLowestPointOffset))
                            {
                                var lowestPointContourParentSliceHeight = lowestPointContourParent.SliceHeight;
                                var previousLayerSliceHeight = lowestPointContourParentSliceHeight;

                                var lowestPointsCircles = ConvertSupportPointsToCircles(lowestPointContourParent.LowestPoints, offsetSupportDistance);
                                var lowestPointsCirclesPolyTree = MergeSupportCircles(lowestPointsCircles);

                                //combine next amount of sibbling layers
                                var unionLowestPointCirclesPolyTree = new PolyTree();
                                for (var nextLayerIndex = lowestPointContourParentSliceIndex; nextLayerIndex < lowestPointContourParentSliceIndex + 100; nextLayerIndex++)
                                {
                                    if (nextLayerIndex < stlModel.SliceIndexes.Count)
                                    {
                                        var nextLayerHeight = sliceIndexKeys[nextLayerIndex];
                                        var modelContoursWithinSupportContourFiltered = new PolyTree();
                                        if (modelLayers[nextLayerHeight] != null)
                                        {
                                            var modelContoursWithinSupportContour = IntersectModelSliceLayer(modelLayers[nextLayerHeight], lowestPointsCirclesPolyTree);

                                            foreach (var modelContourWithinSupportContour in modelContoursWithinSupportContour._allPolys.Where(s => !s.IsHole))
                                            {
                                                foreach (var lowestPoint in lowestPointContourParent.LowestPoints)
                                                {
                                                    if (ContourHelper.PointExistsInNotHolePolygon(modelContourWithinSupportContour, lowestPoint.Point))
                                                    {
                                                        modelContoursWithinSupportContourFiltered._allPolys.Add(modelContourWithinSupportContour);
                                                        break;
                                                    }
                                                }
                                            }
                                        }
                                        unionLowestPointCirclesPolyTree = UnionModelSliceLayer(unionLowestPointCirclesPolyTree, modelContoursWithinSupportContourFiltered);
                                    }
                                    // TriangleHelper.SavePolyNodesContourToPng(unionLowestPointCirclesPolyTree._allPolys, nextLayerHeight + "-lowestoffset");
                                }

                                //find outerpath on union 
                                var outerPaths = new List<List<IntPoint>>();
                                var outPath = new List<IntPoint>();
                                if (unionLowestPointCirclesPolyTree._allPolys.Count > 0)
                                {
                                    foreach (var unionLowestPointCirclesPolyTreeContour in unionLowestPointCirclesPolyTree._allPolys)
                                    {
                                        foreach (var outerPathPoint in unionLowestPointCirclesPolyTreeContour.Contour)
                                        {
                                            var outerPathDistance = float.MinValue;
                                            foreach (var lowestPoint in lowestPointContour.Key.LowestPoints)
                                            {
                                                outerPathDistance = Math.Max(outerPathDistance, (outerPathPoint.AsVector3() - lowestPoint.Point.AsVector3()).Length);
                                                if (outerPathDistance > selectedMaterialProfile.SupportLowestPointsDistance * listOfLowestPointOffset)
                                                {
                                                    outPath.Add(outerPathPoint);
                                                }
                                                else
                                                {
                                                    if (outPath.Count > 0)
                                                    {
                                                        outerPaths.Add(outPath);
                                                        outPath = new List<IntPoint>();
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                if (outPath.Count > 0)
                                {
                                    outerPaths.Add(outPath);
                                    outPath = new List<IntPoint>();
                                }

                                if (outerPaths.Count > 0)
                                {
                                    //shrink outerpath to make sure that all support cone exist in the path
                                    var shrinkOuterPathsPolyTree = MergeSupportCircles(outerPaths);
                                    shrinkOuterPathsPolyTree = ClipperOffset(shrinkOuterPathsPolyTree, -selectedMaterialProfile.SupportTopRadius);

                                    foreach (var outerPath in shrinkOuterPathsPolyTree._allPolys.Where(s => !s.IsHole))
                                    {
                                        var pointsOnPath = VectorHelper.GetPointsOnPath(outerPath.Contour, 4 * selectedMaterialProfile.SupportTopRadius, false, true, 0f);
                                        foreach (var pointOnPath in pointsOnPath[0])
                                        {
                                            if (!lowestPointContourParent.LowestPointsWithOffset.ContainsKey(listOfLowestPointOffset))
                                            {
                                                lowestPointContourParent.LowestPointsWithOffset.Add(listOfLowestPointOffset, new List<MagsAIIntersectionPointWithFilter>());
                                            }
                                            lowestPointContourParent.LowestPointsWithOffset[listOfLowestPointOffset].Add(new MagsAIIntersectionPointWithFilter() { Point = pointOnPath });
                                        }
                                    }
                                }
                            }
                            listOfLowestPointsDistanceOffsetIndex++;

                            previousListOfLowestPointOffset = listOfLowestPointOffset;
                        }
                    }
                }
                //   }
            });

        }

        private static void DetermineIfLowestContoursHasSibblingEdgeDown(ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours, STLModel3D stlModel, SortedDictionary<float, PolyTree> modelLayers, int amountOfSibblingLayers)
        {
            //determine layerdown contour sibbling childs
            Parallel.ForEach(lowestPointsContours, lowestPointsContourAsync =>
            //foreach(var facingDownContourAsync in facingDownContours)
            {
                var lowestPointContour = lowestPointsContourAsync;
                if (!lowestPointContour.Key.HasEdgeDown)
                {
                    var connectedLayerPolyNodes = new Dictionary<float, List<PolyNode>>();

                    //find upper layer polynode by using triangle connections
                    var currentSibblingLayerIndex = 0;
                    //foreach (var upperSliceHeight in stlModel.SliceIndexes.Keys)
                    //{
                    //  if (upperSliceHeight == lowestPointContour.Key.SliceHeight)
                    //{
                    connectedLayerPolyNodes.Add(lowestPointContour.Key.SliceHeight, new List<PolyNode>() { lowestPointContour.Key.Polygon });

                    for (var a = currentSibblingLayerIndex + 1; a < amountOfSibblingLayers; a++)
                    {
                        if (stlModel.SliceIndexes.Count - 1 > (lowestPointContour.Key.SliceIndex + a))
                        {
                            var currentLayerHeight = sliceIndexKeys[lowestPointContour.Key.SliceIndex + a - 1];
                            var nextLayerHeight = sliceIndexKeys[lowestPointContour.Key.SliceIndex + a];

                            if (modelLayers.ContainsKey(nextLayerHeight) && modelLayers[nextLayerHeight] != null)
                            {
                                //find next layer polygon by using triangle connection
                                var connectedNextContourFound = false;
                                var toMuchConnectedNodesFound = false;
                                foreach (var nextModelContour in modelLayers[nextLayerHeight]._allPolys)
                                {
                                    if (!nextModelContour.IsHole)
                                    {
                                        if (connectedLayerPolyNodes.ContainsKey(currentLayerHeight))
                                        {
                                            foreach (var currentPolyNode in connectedLayerPolyNodes[currentLayerHeight])
                                            {
                                                var nextModelContourTriangleConnectionFound = false;
                                                foreach (var currentPolyNodeTriangleConnection in currentPolyNode.TriangleConnections)
                                                {
                                                    if (nextModelContour.TriangleConnections.ContainsKey(currentPolyNodeTriangleConnection.Key))
                                                    {
                                                        //found contour
                                                        nextModelContourTriangleConnectionFound = true;
                                                        break;
                                                    }
                                                }

                                                if (nextModelContourTriangleConnectionFound)
                                                {
                                                    if (!connectedLayerPolyNodes.ContainsKey(nextLayerHeight))
                                                    {
                                                        connectedLayerPolyNodes.Add(nextLayerHeight, new List<PolyNode>());
                                                    }

                                                    connectedLayerPolyNodes[nextLayerHeight].Add(nextModelContour);

                                                    if (connectedLayerPolyNodes[nextLayerHeight].Count > 4)
                                                    {
                                                        a = stlModel.SliceIndexes.Count;
                                                        toMuchConnectedNodesFound = true;
                                                        lowestPointContour.Key.HasComplexConnectedUpperChildContour = true;
                                                        break;
                                                    }

                                                    connectedNextContourFound = true;
                                                }
                                            }
                                        }

                                        if (toMuchConnectedNodesFound)
                                        {
                                            break;
                                        }
                                    }
                                }

                                if (!connectedNextContourFound)
                                {
                                    AutoSupportHelpers.FindNextChildConnection(stlModel, currentLayerHeight, nextLayerHeight, connectedLayerPolyNodes, modelLayers);
                                }
                            }
                        }
                    }

                    lock (lowestPointsContours)
                    {
                        foreach (var connectedLayerPolyNode in connectedLayerPolyNodes)
                        {
                            lowestPointContour.Value.Add(connectedLayerPolyNode.Key, connectedLayerPolyNode.Value);
                        }

                    }

                    //           break;
                }
                //     }
                //   }
            }
    );
        }


        private static ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> CalcLowestPointsSupport(STLModel3D stlModel, Material selectedMaterial, SupportProfile selectedSupportProfile)
        {
            var lowestPointsContours = new ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>>();

            //calc supprt of facing down conturs
            //Parallel.For(0, stlModel.SliceIndexes.Count - 1, sliceIndex2 =>
            for (var sliceIndex2 = 0; sliceIndex2 < stlModel.SliceIndexes.Count - 1; sliceIndex2++)
            {
                var sliceIndexAsync = (int)sliceIndex2;
                var sliceHeightAsync = sliceIndexKeys[sliceIndexAsync];
                if (ModelLayersFacingDown.ContainsKey(sliceHeightAsync))
                {
                    //TriangleHelper.SavePolyNodesContourToPng(ModelLayersFacingDown[sliceHeightAsync]._allPolys, sliceHeightAsync.ToString("00") + "-lowestpointcontour");
                    var facingDownPolygons = CalcContourSupportStructure(ModelLayersFacingDown[sliceHeightAsync], new PolyTree(), new PolyTree(), selectedSupportProfile, sliceHeightAsync, stlModel, false, true);
                    facingDownPolygons.UpdateSupportedContours(selectedMaterial);
                    // TriangleHelper.SaveLowestPointsContourToPng(facingDownPolygons, ModelLayersFacingDown[sliceHeightAsync], selectedMaterial, sliceIndexAsync);

                    foreach (var facingDownPolygon in facingDownPolygons)
                    {
                        foreach (var upperSliceHeight in stlModel.SliceIndexes.Keys)
                        {
                            if (upperSliceHeight == sliceHeightAsync)
                            {
                                facingDownPolygon.SliceHeight = sliceHeightAsync;
                                facingDownPolygon.SliceIndex = sliceIndexAsync;
                                lowestPointsContours.TryAdd(facingDownPolygon, new Dictionary<float, List<PolyNode>>());

                                break;
                            }
                        }
                    }
                }
            }
            //  );

            return lowestPointsContours;
        }


        private static ConcurrentDictionary<MagsAIIntersectionData, bool> ProcessModelOverhangSupport(STLModel3D stlModel, SortedDictionary<float, PolyTree> modelLayers,
            ConcurrentDictionary<LowestPointPolygon, Dictionary<float, List<PolyNode>>> lowestPointsContours,
            SortedDictionary<float, PolygonSupportPointCollection> currentModelOtherSupportPoints, SupportProfile selectedMaterialProfile)
        {
            var supportPoints = new ConcurrentDictionary<MagsAIIntersectionData, bool>();

            var overhangSliceIndexes = stlModel.Triangles.CalcOverhangSliceIndex(stlModel);

            Parallel.ForEach(overhangSliceIndexes, sliceHeightAsync =>
            //foreach(var overhangSliceIndex in overhangSliceIndexes.Keys)
            {
                var sliceIndex = sliceHeightAsync.Key;
                var sliceHeight = sliceHeightAsync.Value;

                //TriangleHelper.SavePolyNodesContourToPng(modelLayers[sliceHeight]._allPolys, sliceIndex + "d");

                if (sliceIndex > 0)
                {
                    var referenceLayerSliceHeight = sliceIndexKeys[sliceIndex - 1];
                    if (modelLayers[referenceLayerSliceHeight] != null && modelLayers[referenceLayerSliceHeight]._allPolys.Count > 0)
                    {
                        //TriangleHelper.SavePolyNodesContourToPng(modelLayers[referenceLayerSliceHeight]._allPolys, sliceIndex + "d");

                        var currentLayer = modelLayers[sliceHeight];

                        if (currentLayer != null && currentLayer._allPolys.Count > 0)
                        {

                            //get overhang triangles
                            var triangleList = new List<Triangle>();
                            foreach(var t in stlModel.SliceIndexes[sliceHeight])
                            {
                                var angleZ = stlModel.Triangles[t.ArrayIndex][t.TriangleIndex].AngleZ;
                                if ((angleZ > 89 && angleZ < 91) || (angleZ > 269 && angleZ < 271))
                                {
                                    triangleList.Add(stlModel.Triangles[t.ArrayIndex][t.TriangleIndex]);
                                }

                                //convert triangles to polygons
                            }

                            var a = new List<List<PolylineWithNormal>>();
                            var overhangContoursPolyTree = new List<PolyTree>();

                            if (triangleList.Count > 0)
                            {
                                var triangleZPolys = RenderEngine.GetZIntersections(triangleList, sliceHeight); ;
                                overhangContoursPolyTree = PolyTree.FromListOfModelIntersectionsUsingPolygons(sliceHeight, triangleZPolys, PrintJobManager.SelectedPrinter, new List<float[]>(), new List<float[]>(), out a, out a, false, true, false, false);

                                //TriangleHelper.SavePolyNodesContourToPng(overhangContoursPolyTree[0]._allPolys, sliceIndex + "overhang");

                                var referenceLayerWithOverhangOffset = ClipperOffset(modelLayers[referenceLayerSliceHeight], selectedMaterialProfile.SupportOverhangDistance);

                                var diffSupportLayer = DifferenceModelSliceLayer(overhangContoursPolyTree[0], referenceLayerWithOverhangOffset);

                                var support = CalcContourSupportStructure(diffSupportLayer, null, null, selectedMaterialProfile, sliceHeight, stlModel, true, false);

                                if (support != null)
                                {
                                    //TriangleHelper.SavePolyNodesContourToPng(referenceLayerWithOverhangOffset._allPolys, sliceIndex + "reference");
                                    foreach (var s in support)
                                    {
                                        Parallel.ForEach(s.LowestPoints, sPointAsync =>
                                       {
                                           var sPoint = sPointAsync;
                                           var supportIntersection = new MagsAIIntersectionData
                                           {
                                               SliceHeight = sliceHeight,
                                               TopPoint = sPoint.Point.AsVector3(),
                                               RenderColor = Color.Orange
                                           };

                                           supportIntersection.UpdateTriangleReference(stlModel, selectedMaterialProfile);
                                           supportIntersection.UpdateLastSupportedHeight(stlModel, selectedMaterialProfile, modelLayers);

                                           if (!currentModelOtherSupportPoints.ContainsKey(sliceHeight))
                                           {
                                               currentModelOtherSupportPoints.Add(sliceHeight, new PolygonSupportPointCollection());
                                           }

                                           currentModelOtherSupportPoints[sliceHeight].OverhangPoints.Add(supportIntersection);
                                           supportPoints.TryAdd(supportIntersection, false);
                                       });
                                    }
                                }
                            }
                        }
                    }
                }
            }
            );

            return supportPoints;
        }

        private static void AddModelContourByLayer(int sliceIndex, float sliceHeight, STLModel3D model, SortedDictionary<float, PolyTree> modelContours,
            SortedDictionary<float, PolyTree> modelAngledContours, SortedDictionary<float, PolyTree> modelWallContours,
            AtumPrinter selectedPrinter, Material selectedMaterial, List<float[]> angleSideAngles, List<float[]> wallSideAngles)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var modelAngledContoursResult = new PolyTree();
            if (!modelAngledContours.ContainsKey(sliceHeight))
            {
                modelAngledContoursResult = null;
            }

            var modelWallContoursResult = new PolyTree();
            if (!modelWallContours.ContainsKey(sliceHeight))
            {
                modelWallContoursResult = null;
            }

            var contours = model.GetSliceContourStructure(sliceIndex, sliceHeight, selectedPrinter, selectedMaterial,
                out modelAngledContoursResult, out modelWallContoursResult, angleSideAngles, wallSideAngles,
                modelAngledContoursResult != null, modelWallContoursResult != null);

            if (sliceHeight > 5)
            {
                //TriangleHelper.SavePolyNodesContourToPng(contours._allPolys, "render_" + sliceHeight.ToString("00"));
                ContourHelper.ReducePointsInPolyTree(contours, 0.3f);
                //TriangleHelper.SavePolyNodesContourToPng(contours._allPolys, "reduced_" + sliceHeight.ToString("00"));

                foreach (var polyNode in contours._allPolys)
                {
                    polyNode.Area = Clipper.Area(polyNode.Contour) / CONTOURPOINT_TO_VECTORPOINT_FACTOR / CONTOURPOINT_TO_VECTORPOINT_FACTOR * 10 * 10;
                }

                //TriangleHelper.SavePolyNodesContourToPng(contours._allPolys, sliceHeight.ToString("00") + "-small-area");
            }

            lock (ModelLayersFacingDown)
            {
                modelContours[sliceHeight] = contours;

                if (modelAngledContours.ContainsKey(sliceHeight))
                {
                    modelAngledContours[sliceHeight] = modelAngledContoursResult;
                }

                if (modelWallContours.ContainsKey(sliceHeight))
                {
                    modelWallContours[sliceHeight] = modelWallContoursResult;
                }
            }

            //Console.WriteLine("AddModelContourByLayer: " + stopwatch.ElapsedMilliseconds + "ms");
            //Console.WriteLine(sliceHeight);
        }


        private static bool AddContourFacingDown(int sliceIndex, float sliceHeight, PolyTree currentLayer, PolyTree previousLayer, ConcurrentDictionary<float, PolyTree> contoursFacingDown)
        {
            var currentPolyNodesWithoutPreviousLayerPoint = new List<PolyNode>();

            if (currentLayer != null)
            {
                //check if there are polynodes below currentlayer that is a point inside this polynode
                foreach (var currentLayerPolyNode in currentLayer._allPolys)
                {
                    var previousLayerPointWithinCurrentPolyNode = false;
                    if (!currentLayerPolyNode.IsHole && (currentLayerPolyNode.Area < -(UserProfileManager.UserProfile.Setting_MAGSAI_MinLowestPointArea_InPixels) || currentLayerPolyNode.Area > (UserProfileManager.UserProfile.Setting_MAGSAI_MinLowestPointArea_InPixels)))
                    {
                        foreach (var previousPolyNode in previousLayer._allPolys)
                        {
                            if (!previousPolyNode.IsHole)
                            {
                                if ((previousPolyNode.Area < -(UserProfileManager.UserProfile.Setting_MAGSAI_MinLowestPointArea_InPixels) || previousPolyNode.Area > (UserProfileManager.UserProfile.Setting_MAGSAI_MinLowestPointArea_InPixels)))
                                {
                                    foreach (var previousLayerPoint in previousPolyNode.Contour)
                                    {
                                        if (PointExistsInNotHolePolygon(currentLayerPolyNode, previousLayerPoint))
                                        {
                                            previousLayerPointWithinCurrentPolyNode = true;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (previousLayerPointWithinCurrentPolyNode)
                            {
                                break;
                            }
                        }

                        if (!previousLayerPointWithinCurrentPolyNode)
                        {
                            var currentLayerPointWithinPreviousLayer = false;
                            //check current layer points within previouslayer
                            foreach (var previousPolyNode in previousLayer._allPolys)
                            {
                                if (!previousPolyNode.IsHole)
                                {
                                    if ((previousPolyNode.Area < -(UserProfileManager.UserProfile.Setting_MAGSAI_MinLowestPointArea_InPixels) || previousPolyNode.Area > (UserProfileManager.UserProfile.Setting_MAGSAI_MinLowestPointArea_InPixels)))
                                    {
                                        foreach (var currentLayerPoint in currentLayerPolyNode.Contour)
                                        {
                                            //point inside currentPolygon
                                            if (PointExistsInNotHolePolygon(previousPolyNode, currentLayerPoint))
                                            {
                                                currentLayerPointWithinPreviousLayer = true;
                                                break;
                                            }
                                        }
                                    }
                                }

                                if (currentLayerPointWithinPreviousLayer)
                                {
                                    break;
                                }
                            }

                            if (!currentLayerPointWithinPreviousLayer)
                            {
                                currentPolyNodesWithoutPreviousLayerPoint.Add(currentLayerPolyNode);
                            }
                        }
                    }
                }
            }


            if (currentPolyNodesWithoutPreviousLayerPoint.Count > 0)
            {
                if (!contoursFacingDown.ContainsKey(sliceHeight))
                {
                    contoursFacingDown.TryAdd(sliceHeight, new PolyTree());
                }
                if (contoursFacingDown[sliceHeight] == null)
                {
                    contoursFacingDown[sliceHeight] = new PolyTree();
                }

                foreach (var currentPolyNodeWithoutPreviousLayerPoint in currentPolyNodesWithoutPreviousLayerPoint)
                {
                    currentPolyNodeWithoutPreviousLayerPoint.FacingDown = true;
                    contoursFacingDown[sliceHeight]._allPolys.Add(currentPolyNodeWithoutPreviousLayerPoint);
                }


                return true;
            }

            return false;
        }


        private static void PreCacheModelLayers(STLModel3D stlModel, SortedDictionary<float, PolyTree> modelLayers, SortedDictionary<float, PolyTree> modelAngledLayers, SortedDictionary<float, PolyTree> modelWallLayers, int startingSliceIndex, bool debugMode, DAL.Hardware.AtumPrinter selectedPrinter, Material selectedMaterial, int stopIndex = -1)
        {
            LoggingManager.WriteToLog("AutoSupportEngine", "Pre-caching", "Started");

            try
            {
                var precacheAmount = stopIndex == -1 ? stlModel.SliceIndexes.Keys.Count : stopIndex;
                var endSliceIndex = startingSliceIndex + precacheAmount + 1;
                if (endSliceIndex >= modelLayers.Keys.Count)
                {
                    endSliceIndex = modelLayers.Keys.Count - 1;
                }

                //clean previous cache items
                var stopwatch = new Stopwatch();
                stopwatch.Start();
                if (startingSliceIndex > 0)
                {
                    var maxContourHeight = sliceIndexKeys[startingSliceIndex - 1];
                    foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
                    {
                        if (sliceHeight < maxContourHeight && modelLayers[sliceHeight] != null)
                        {
                            modelLayers[sliceHeight] = null;
                            ModelLayersFacingDown[sliceHeight] = null;
                        }
                    }
                }

                LoggingManager.WriteToLog("AutoSupportEngine", "Cleaning in", stopwatch.ElapsedMilliseconds + "ms");

                //get all model slice contours
                var sliceIndexesCount = stlModel.SliceIndexes.Count;
                var parallelStepItems = Enumerable.Range(0, sliceIndexesCount); //.Where(i => (i - bottomSliceIndex) % amountOfParallelItems == 0 || i == bottomSliceIndex);

                var angleSideAngles = new List<float[]>()
            {
                new float[]{120, 240},
            };

                var wallSideAngles = new List<float[]>()
            {
                new float[]{80, 90},
                new float[]{260, 270},
            };
                Parallel.ForEach(parallelStepItems, parallelStepIndex =>
                //foreach(var parallelStepIndex in parallelStepItems)
                {
                    var sliceIndex = parallelStepIndex;
                    var sliceHeightAsync = sliceIndexKeys[sliceIndex];

                    AddModelContourByLayer(sliceIndex, sliceHeightAsync, stlModel, modelLayers, modelAngledLayers, modelWallLayers, selectedPrinter, selectedMaterial, angleSideAngles, wallSideAngles);
                });

                LoggingManager.WriteToLog("AutoSupportEngine", "Model contours in", stopwatch.ElapsedMilliseconds + "ms");


                //  stopwatch.Reset();
                stopwatch.Start();

                //determine facing down contours
                Parallel.ForEach(parallelStepItems, parallelStepIndex =>
                //{
                //for (var i = 0; i < sliceIndexesCount; i++)
                {
                    var sliceIndex = parallelStepIndex;
                    if (sliceIndex > 0)
                    {
                        var previousSliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndex - 1);
                        var sliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndex);
                        if (modelLayers[previousSliceHeightAsync] != null)
                        {
                            AddContourFacingDown(sliceIndex, sliceHeightAsync, modelLayers[sliceHeightAsync], modelLayers[previousSliceHeightAsync], ModelLayersFacingDown);
                        }
                    }
                    //});
                });

                LoggingManager.WriteToLog("AutoSupportEngine", "Facing down contours in", stopwatch.ElapsedMilliseconds + "ms");
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("AutoSupportEngine", "Exception", exc.StackTrace);
            }
        }


        internal static LowestPointsPolygons CalcContourSupportStructure(PolyTree modelContoursFacingDownOnHeight, PolyTree modelContour, PolyTree modelContoursOverhangWithOffsetOnHeight, SupportProfile supportProfile, float sliceHeight, STLModel3D stlModel, bool skipEdgeDetection, bool allowCalculationOnSmallContour)
        {
            var contourSupportConeOffset = 0.1f;

            var result = new LowestPointsPolygons();

            if (modelContoursFacingDownOnHeight != null)
            {
                var maxSupportConeDistance = supportProfile.SupportTopRadius * 5 * CONTOURPOINT_TO_VECTORPOINT_FACTOR;
                //check if there are poly nodes that are facing down
                if (modelContoursFacingDownOnHeight._allPolys.Count > 0)
                {
                    //when exists create support structure based on clipper offset to center and stop when no contour exists
                    var contourFacingDownOffset = new PolyTree();
                    foreach (var modelFacingDownPolyNode in modelContoursFacingDownOnHeight._allPolys)
                    {
                        var modelContourProcessed = false;
                        var polygonEdgePoints = new MagsAIEdgePoints();
                        var edgeModelIntersectionPoints = new List<MagsAIIntersectionData>();

                        if (!skipEdgeDetection)
                        {
                            //  var polygonArea = Clipper.Area(modelFacingDownPolyNode.Contour);
                            //  polygonArea /= ((CONTOURPOINT_TO_VECTORPOINT_FACTOR * CONTOURPOINT_TO_VECTORPOINT_FACTOR));
                            polygonEdgePoints = CalcPolygonEdgePoints(modelFacingDownPolyNode, sliceHeight, supportProfile.SupportTopRadius);
                            foreach (var edgePoint in polygonEdgePoints)
                            {
                                if (!polygonEdgePoints.AreaToSmallToOffset) //only add endpoints when area is large enough to to offsetting
                                {
                                    edgePoint.ContourIntersectionPoint = edgePoint.EdgePoint;
                                }
                            }
                        }

                        var contourSupportPoints = new List<MagsAIIntersectionPointWithFilter>();
                        //find inner contour based on support top with small offset
                        contourFacingDownOffset = ClipperOffset(modelFacingDownPolyNode.Contour, -(supportProfile.SupportTopRadius + contourSupportConeOffset));
                        //     TriangleHelper.SavePolyNodesContourToPng(contourFacingDownOffset._allPolys, "o");
                        //   TriangleHelper.SavePolyNodesContourToPng(new List<PolyNode>() { modelFacingDownPolyNode }, "v");
                        if (contourFacingDownOffset._allPolys.Count > 0 && ContourHelper.PointExistsInNotHolePolygon(modelFacingDownPolyNode, contourFacingDownOffset._allPolys[0].Contour[0]))
                        {
                            //process single innerpath
                            var lowestPolyNodeSupportPoints = new List<IntPoint>();
                            foreach (var singlePath in contourFacingDownOffset._allPolys.Where(s => !s.IsHole))
                            {
                                var pointsOnSinglePath = VectorHelper.GetPointsOnPath(singlePath.Contour, supportProfile.SupportInfillDistance, true, false, 0);

                                if (pointsOnSinglePath.Count > 0)
                                {
                                    if (pointsOnSinglePath[0].Count > 2)
                                    {
                                        //determine distance between last 2 points
                                        var firstPoint = pointsOnSinglePath[0][pointsOnSinglePath[0].Count - 2];
                                        var lastPoint = pointsOnSinglePath[0][pointsOnSinglePath[0].Count - 1];

                                        var distance = (firstPoint.AsVector3() - lastPoint.AsVector3()).Length;
                                        if (distance < 0.05f)
                                        {
                                            pointsOnSinglePath[0].RemoveAt(pointsOnSinglePath[0].Count - 1);
                                        }
                                    }
                                }

                                //check if points are within overhang distance if so remove them
                                var minSupportConeDistance = supportProfile.SupportOverhangDistance / 2;
                                for (var currentPointIndex = 0; currentPointIndex < pointsOnSinglePath[0].Count; currentPointIndex++)
                                {
                                    var indexesToRemove = new List<int>();

                                    for (var nextPointIndex = 1; nextPointIndex < pointsOnSinglePath[0].Count; nextPointIndex++)
                                    {
                                        if (currentPointIndex != nextPointIndex)
                                        {
                                            var distance = (pointsOnSinglePath[0][currentPointIndex].AsVector3() - pointsOnSinglePath[0][nextPointIndex].AsVector3()).Length;
                                            if (distance < minSupportConeDistance)
                                            {
                                                indexesToRemove.Add(nextPointIndex);
                                            }
                                        }
                                    }
                                    indexesToRemove.Reverse();
                                    foreach (var indexToRemove in indexesToRemove)
                                    {
                                        pointsOnSinglePath[0].RemoveAt(indexToRemove);
                                    }
                                }

                                //convert support points on path to model intersection points
                                foreach (var pointOnSinglePath in pointsOnSinglePath[0])
                                {
                                    var triangleIntersectionBelow = new Vector3Class();
                                    var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, pointOnSinglePath, sliceHeight, triangleIntersectionBelow);
                                    if (triangleIntersection.IntersectionPoint.Z > sliceHeight + 1f)
                                    {
                                        triangleIntersection.IntersectionPoint.Z = sliceHeight;
                                    }

                                    var intersectionPointAsIntPoints = new IntPoint(triangleIntersection.IntersectionPoint);
                                    if (!lowestPolyNodeSupportPoints.Contains(intersectionPointAsIntPoints))
                                    {
                                        lowestPolyNodeSupportPoints.Add(intersectionPointAsIntPoints);
                                    }
                                }
                            }

                            foreach (var lowestPolyNodeSupportPoint in lowestPolyNodeSupportPoints)
                            {
                                contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = lowestPolyNodeSupportPoint });
                            }

                            //FILTER POINTS: when 2 points are within same contour with distance < 3 * topradius merge them
                            if (contourSupportPoints.Count == 2)
                            {
                                var distanceBetweenPoints = (contourSupportPoints[0].Point.XY - contourSupportPoints[1].Point.XY).Length;
                                if (distanceBetweenPoints < maxSupportConeDistance) //merge the points
                                {
                                    var mergedContourSupportPoint = new IntPoint((contourSupportPoints[0].Point.X + contourSupportPoints[1].Point.X) / 2, (contourSupportPoints[1].Point.Y + contourSupportPoints[1].Point.Y) / 2);
                                    contourSupportPoints.Clear();
                                    contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = mergedContourSupportPoint });
                                }
                            }
                            else
                            {
                                var materialSupportConeOverhangDistanceOffset = supportProfile.SupportTopRadius + contourSupportConeOffset;
                                materialSupportConeOverhangDistanceOffset += supportProfile.SupportInfillDistance;
                                var previousArea = Clipper.Area(modelFacingDownPolyNode.Contour);

                                while (contourFacingDownOffset._allPolys.Where(s => !s.IsHole).Count() > 0)
                                {
                                    contourFacingDownOffset = ClipperOffset(modelFacingDownPolyNode.Contour, -materialSupportConeOverhangDistanceOffset);

                                    //check if offset is within contour by check first point
                                    var offsetContourIsValid = false;
                                    if (contourFacingDownOffset._allPolys.Count > 0)
                                    {
                                        if (contourFacingDownOffset._allPolys[0].Contour.Count > 0 && PointExistsInNotHolePolygon(modelFacingDownPolyNode, contourFacingDownOffset._allPolys[0].Contour[0]))
                                        {
                                            offsetContourIsValid = true;
                                        }
                                    }

                                    if (!offsetContourIsValid)
                                    {
                                        contourFacingDownOffset._allPolys.Clear();
                                    }


                                    //        TriangleHelper.SavePolyNodesContourToPng(new List<PolyNode>() { modelFacingDownPolyNode }, "t");
                                    //    TriangleHelper.SavePolyNodesContourToPng(contourFacingDownOffset._allPolys, "o");

                                    var childAreas = 0d;
                                    foreach (var contourFacingDownChildOffset in contourFacingDownOffset._allPolys)
                                    {
                                        childAreas += Clipper.Area(contourFacingDownChildOffset.Contour);
                                    }

                                    if (childAreas >= previousArea)
                                    {
                                        contourFacingDownOffset._allPolys.Clear();
                                    }
                                    else
                                    {
                                        previousArea = childAreas;
                                        if (contourFacingDownOffset._allPolys.Count > 0)
                                        {
                                            lowestPolyNodeSupportPoints.Clear();

                                            //process multiple innerpath
                                            foreach (var contourFacingDownChild in contourFacingDownOffset._allPolys)
                                            {
                                                var pointsOnContour = VectorHelper.GetPointsOnPath(contourFacingDownChild.Contour, supportProfile.SupportInfillDistance, true, false, 0f);

                                                if (pointsOnContour[0].Count > 2)
                                                {
                                                    //determine distance between last 2 points
                                                    var firstPoint = pointsOnContour[0][pointsOnContour[0].Count - 2];
                                                    var lastPoint = pointsOnContour[0][pointsOnContour[0].Count - 1];

                                                    var distance = (firstPoint.AsVector3() - lastPoint.AsVector3()).Length;
                                                    if (distance < 0.05f)
                                                    {
                                                        pointsOnContour[0].RemoveAt(pointsOnContour[0].Count - 1);
                                                    }
                                                }


                                                foreach (var pointOnContour in pointsOnContour[0])
                                                {
                                                    //check if hole contour is child. If so check each point do determine the i
                                                    if (modelFacingDownPolyNode.Childs.Count > 0)
                                                    {
                                                        var pointExistsInHolePolygon = false;
                                                        foreach (var modelFacingDownPolyNodeChild in modelFacingDownPolyNode.Childs)
                                                        {
                                                            if (modelFacingDownPolyNodeChild.IsHole)
                                                            {
                                                                if (Clipper.PointInPolygon(pointOnContour, modelFacingDownPolyNodeChild.Contour) != 0)
                                                                {
                                                                    pointExistsInHolePolygon = true;
                                                                }
                                                            }
                                                        }

                                                        if (!pointExistsInHolePolygon)
                                                        {
                                                            lowestPolyNodeSupportPoints.Add(pointOnContour);
                                                        }
                                                    }
                                                    else
                                                    {
                                                        lowestPolyNodeSupportPoints.Add(pointOnContour);
                                                    }
                                                }
                                            }

                                            foreach (var lowestPolyNodeSupportPoint in lowestPolyNodeSupportPoints)
                                            {
                                                contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = lowestPolyNodeSupportPoint });
                                            }

                                            materialSupportConeOverhangDistanceOffset += supportProfile.SupportInfillDistance;
                                        }
                                    }

                                }
                            }

                            var edgeModelIntersectionPoints2 = new List<MagsAIIntersectionData>();
                            foreach (var edgePoint in polygonEdgePoints)
                            {
                                if (edgePoint.ContourIntersectionPoint != new Vector2())
                                {
                                    edgeModelIntersectionPoints2.Add(new MagsAIIntersectionData() { TopPoint = new Vector3Class(edgePoint.ContourIntersectionPoint.X, edgePoint.ContourIntersectionPoint.Y, sliceHeight), SliceHeight = sliceHeight });
                                }
                            }
                            result.Add(new LowestPointPolygon() { Polygon = modelFacingDownPolyNode, LowestPoints = contourSupportPoints, EdgeIntersectionPoints = edgeModelIntersectionPoints2 });
                        }
                        else
                        {
                            //when no support point exists in surface use contour as path
                            if (contourSupportPoints.Count == 0 && !modelContourProcessed)
                            {
                                modelContourProcessed = true;

                                var modelFacingDownContourArea = Clipper.Area(modelFacingDownPolyNode.Contour);
                                modelFacingDownContourArea /= (CONTOURPOINT_TO_VECTORPOINT_FACTOR * CONTOURPOINT_TO_VECTORPOINT_FACTOR);
                                if (modelFacingDownContourArea < supportProfile.SupportOverhangDistance)
                                {
                                    var modelFacingDownPolyNodeBoundries = Clipper.GetBounds(new List<List<IntPoint>> { modelFacingDownPolyNode.Contour });
                                    var modelFacingDownPolyNodeCenterX = modelFacingDownPolyNodeBoundries.left + ((modelFacingDownPolyNodeBoundries.right - modelFacingDownPolyNodeBoundries.left) / 2f);
                                    var modelFacingDownPolyNodeCenterY = modelFacingDownPolyNodeBoundries.top + ((modelFacingDownPolyNodeBoundries.bottom - modelFacingDownPolyNodeBoundries.top) / 2f);
                                    var centerPoint = new IntPoint(modelFacingDownPolyNodeCenterX, modelFacingDownPolyNodeCenterY);
                                    var t = centerPoint.AsVector3();
                                    var centerpointCircle = MagsAIEngine.ConvertSupportPointsToCircles(centerPoint, 2.5f);
                                    var centerPointCircleAsPolyTree = MergeSupportCircles(new List<List<IntPoint>>() { centerpointCircle });


                                    var modelFacingDownPolyTree = new PolyTree();
                                    modelFacingDownPolyTree._allPolys.Add(modelFacingDownPolyNode);
                                    var modelContourIntersectionLines = CalcModelIntersectionLines(centerPointCircleAsPolyTree, modelFacingDownPolyTree);

                                    if (modelContourIntersectionLines.Count > 0)
                                    {
                                        var supportPointsAsCircles = new List<List<IntPoint>>();
                                        foreach (var modelContourIntersectionLine in modelContourIntersectionLines)
                                        {
                                            var supportPointsOnIntersectionLine = VectorHelper.GetPointsOnPath(modelContourIntersectionLine, supportProfile.SupportInfillDistance, false, true, 0);
                                            foreach (var supportPointOnIntersectionLine in supportPointsOnIntersectionLine[0])
                                            {
                                                var supportPointCircle = MagsAIEngine.ConvertSupportPointsToCircles(supportPointOnIntersectionLine, 5f);
                                                supportPointsAsCircles.Add(supportPointCircle);

                                                contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = supportPointOnIntersectionLine });
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (allowCalculationOnSmallContour)
                                        {
                                            var pointsOnPath = VectorHelper.GetPointsOnPath(modelFacingDownPolyNode.Contour, supportProfile.SupportInfillDistance, false, true, 0);
                                            if (pointsOnPath.Count == 1)
                                            {
                                                contourSupportPoints = new List<MagsAIIntersectionPointWithFilter>();
                                                foreach (var point in pointsOnPath[0])
                                                {
                                                    contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = point });
                                                    break;
                                                }

                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    // TriangleHelper.SavePolyNodesContourToPng(new List<PolyNode>() { modelFacingDownPolyNode }, sliceHeight.ToString("00") + "-supported");

                                    var pointsOnPath = VectorHelper.GetPointsOnPath(modelFacingDownPolyNode.Contour, supportProfile.SupportInfillDistance, true, false, 0);
                                    if (pointsOnPath.Count == 1)
                                    {
                                        //check if points are within overhang distance if so remove them
                                        var minSupportConeDistance = supportProfile.SupportOverhangDistance / 2;
                                        for (var currentPointIndex = 0; currentPointIndex < pointsOnPath[0].Count; currentPointIndex++)
                                        {
                                            var indexesToRemove = new List<int>();

                                            for (var nextPointIndex = 1; nextPointIndex < pointsOnPath[0].Count; nextPointIndex++)
                                            {
                                                if (currentPointIndex != nextPointIndex)
                                                {
                                                    var distance = (pointsOnPath[0][currentPointIndex].AsVector3() - pointsOnPath[0][nextPointIndex].AsVector3()).Length;
                                                    if (distance < minSupportConeDistance)
                                                    {
                                                        indexesToRemove.Add(nextPointIndex);
                                                    }
                                                }
                                            }
                                            indexesToRemove.Reverse();
                                            foreach (var indexToRemove in indexesToRemove)
                                            {
                                                pointsOnPath[0].RemoveAt(indexToRemove);
                                            }
                                        }

                                        contourSupportPoints = new List<MagsAIIntersectionPointWithFilter>();
                                        foreach (var point in pointsOnPath[0])
                                        {
                                            contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = point });
                                        }
                                    }
                                }
                            }

                            if (contourSupportPoints.Count == 2)
                            {
                                var distanceBetweenPoints = (contourSupportPoints[0].Point.XY - contourSupportPoints[1].Point.XY).Length;
                                if (distanceBetweenPoints < maxSupportConeDistance) //merge the points
                                {
                                    var mergedContourSupportPoint = new IntPoint((contourSupportPoints[0].Point.X + contourSupportPoints[1].Point.X) / 2, (contourSupportPoints[1].Point.Y + contourSupportPoints[1].Point.Y) / 2);
                                    contourSupportPoints.Clear();
                                    contourSupportPoints.Add(new MagsAIIntersectionPointWithFilter() { Point = mergedContourSupportPoint });
                                }
                            }
                        }


                        //calc edges on poly contour
                        foreach (var edgePoint in polygonEdgePoints)
                        {
                            if (edgePoint.ContourIntersectionPoint != new Vector2()) //area large enough for edge support
                            {
                                edgeModelIntersectionPoints.Add(new MagsAIIntersectionData() { TopPoint = new Vector3Class(edgePoint.ContourIntersectionPoint.X, edgePoint.ContourIntersectionPoint.Y, sliceHeight), SliceHeight = sliceHeight });
                            }
                        }

                        //check if polygon allready exists
                        var polygonFound = result.Any(s => s.Polygon == modelFacingDownPolyNode);
                        if (!polygonFound)
                        {
                            result.Add(new LowestPointPolygon()
                            {
                                Polygon = modelFacingDownPolyNode,
                                LowestPoints = contourSupportPoints,
                                EdgePoints = polygonEdgePoints,
                                EdgeIntersectionPoints = edgeModelIntersectionPoints,
                            });
                        }
                    }

                }
            }

            return result;
        }

        private static MagsAIEdgePoints CalcPolygonEdgePoints(PolyNode polyNode, float sliceHeight, float materialSupportConeTopRadius)
        {
            var result = new MagsAIEdgePoints();
            var edgeOffsetContourPolyTree = ClipperOffset(polyNode.Contour, -(materialSupportConeTopRadius * 1.1f));
            var edgeOffsetContourPolyNode = polyNode;
            if (edgeOffsetContourPolyTree._allPolys.Where(s => !s.IsHole).Count() == 1)
            {
                edgeOffsetContourPolyNode = edgeOffsetContourPolyTree._allPolys.First(s => !s.IsHole);
            }
            else
            {
                result.AreaToSmallToOffset = true;
            }

            for (var contourPointIndex = 0; contourPointIndex < edgeOffsetContourPolyNode.Contour.Count; contourPointIndex++)
            {
                var currentPoint = edgeOffsetContourPolyNode.Contour[contourPointIndex].AsVector3();

                var previousPoint = new Vector3Class();
                if (contourPointIndex == 0)
                {
                    previousPoint = edgeOffsetContourPolyNode.Contour[edgeOffsetContourPolyNode.Contour.Count - 1].AsVector3();
                }
                else
                {
                    previousPoint = edgeOffsetContourPolyNode.Contour[contourPointIndex - 1].AsVector3();
                }

                var nextPoint = new Vector3Class();
                if (contourPointIndex == edgeOffsetContourPolyNode.Contour.Count - 1)
                {
                    nextPoint = edgeOffsetContourPolyNode.Contour[0].AsVector3();
                }
                else
                {
                    nextPoint = edgeOffsetContourPolyNode.Contour[contourPointIndex + 1].AsVector3();
                }

                var edgeAngle = VectorHelper.CalcAngleBetweenVectorPoints(previousPoint.Xy, currentPoint.Xy, nextPoint.Xy);

                if (edgeAngle <= 125 && !polyNode.IsHole)// || edgeAngle >= 360 - 95)
                {
                    //check if new point exists in polygon
                    var edgePoint = new MagsAIEdgePoint(previousPoint.Xy, currentPoint.Xy, nextPoint.Xy, edgeAngle, sliceHeight);
                    result.Add(edgePoint);
                }
            }


            //DebugPoints.AddRange(result);

            return result;
        }

        internal static TriangleIntersection Convert2DSupportPointToIntersectionTriangle(STLModel3D stlModel, IntPoint sliceIntersectionPoint, float previousSliceHeight, Vector3Class pointBelowIntersection)
        {
            var result = new TriangleIntersection();

            var sliceIntersectionPointAsVector = sliceIntersectionPoint.AsVector3();
            sliceIntersectionPointAsVector.Z = previousSliceHeight;
            TriangleIntersection[] intersectedTriangles = null;
            IntersectionProvider.IntersectTriangle(new Vector3Class(sliceIntersectionPointAsVector.X, sliceIntersectionPointAsVector.Y, 0), new Vector3Class(0, 0, 1), stlModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out intersectedTriangles);

            if (intersectedTriangles != null)
            {
                var nearestDistance = float.MaxValue;
                var nearestIntersection = new TriangleIntersection();
                foreach (var intersectedTriangle in intersectedTriangles)
                {
                    if (intersectedTriangle != null)
                    {
                        if (intersectedTriangle.Normal.Z < 0 && intersectedTriangle.IntersectionPoint.Z >= previousSliceHeight)
                        {
                            var distance = (sliceIntersectionPointAsVector - intersectedTriangle.IntersectionPoint).Length;

                            if (distance < nearestDistance)
                            {
                                nearestDistance = distance;
                                nearestIntersection = intersectedTriangle;
                            }
                        }
                    }
                }

                result = nearestIntersection;
            }

            if (result.IntersectionPoint != new Vector3Class())
            {
                //find intersectionpoint below
                pointBelowIntersection = new Vector3Class();
                for (var intersectedTriangleIndex = 0; intersectedTriangleIndex < intersectedTriangles.Length; intersectedTriangleIndex++)
                {
                    if (intersectedTriangles[intersectedTriangleIndex] != null)
                    {
                        if (intersectedTriangles[intersectedTriangleIndex].IntersectionPoint.Z == result.IntersectionPoint.Z)
                        {
                            if (intersectedTriangleIndex > 0 && intersectedTriangles[intersectedTriangleIndex - 1] != null)
                            {
                                pointBelowIntersection = intersectedTriangles[intersectedTriangleIndex - 1].IntersectionPoint;
                            }
                        }
                    }
                }
            }
            else
            {
                result.IntersectionPoint = sliceIntersectionPoint.AsVector3() + new Vector3Class(0, 0, previousSliceHeight + 0.05f);
            }

            return result;
        }


        internal static PolyTree ClipperOffset(PolyTree polyNode, float overhangSupportConeDistance)
        {
            var offsetPolyTree = new PolyTree();
            if (polyNode != null)
            {
                var clipperOffset = new ClipperOffset();
                foreach (var child in polyNode._allPolys)
                {
                    //if (!child.IsHole)
                    //{
                    clipperOffset.AddPath(child.Contour, JoinType.jtMiter, EndType.etClosedPolygon);
                    // }
                }

                clipperOffset.Execute(ref offsetPolyTree, overhangSupportConeDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR);
            }

            //offsetPolyTree = UnionModelSliceLayer(offsetPolyTree, new PolyTree());

            if (offsetPolyTree._allPolys.Count > 0 && offsetPolyTree._allPolys[0].Contour.Count == 4 && !offsetPolyTree._allPolys[0].IsHole)
            {
                if (offsetPolyTree._allPolys[0].Childs.Count > 0 && !offsetPolyTree._allPolys[0].Childs[0].IsHole)
                {
                    offsetPolyTree._allPolys.RemoveAt(0);
                }

            }

            return offsetPolyTree;
        }

        private static PolyTree ClipperOffset(List<IntPoint> supportLayerContour, float overhangSupportConeDistance)
        {
            var clipperOffset = new ClipperOffset();
            var offsetPolyTree = new PolyTree();
            clipperOffset.AddPath(supportLayerContour, JoinType.jtMiter, EndType.etClosedPolygon);
            clipperOffset.Execute(ref offsetPolyTree, overhangSupportConeDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR);

            var validOffsetPolyTree = new PolyTree();
            foreach (var child in offsetPolyTree.Childs)
            {
                validOffsetPolyTree._allPolys.Add(child);
            }


            return validOffsetPolyTree;
        }

        internal static PolyTree MergeSupportCircles(List<List<IntPoint>> supportPointAsCirclesIntPoint)
        {
            var supportLayerWithSupportCircles = new PolyTree();
            var clipper = new Clipper();
            clipper.AddPath(new List<IntPoint>(), PolyType.ptSubject, true);
            clipper.AddPaths(supportPointAsCirclesIntPoint, PolyType.ptClip, true);

            clipper.Execute(ClipType.ctUnion, supportLayerWithSupportCircles, PolyFillType.pftNonZero);

            return supportLayerWithSupportCircles;
        }

        internal static PolyTree MergeSupportCircles(List<List<IntPoint>> supportPointAsCirclesIntPoint, PolyTree parentPolyTree)
        {
            var supportLayerWithSupportCircles = new PolyTree();
            var clipper = new Clipper();
            clipper.AddPath(new List<IntPoint>(), PolyType.ptSubject, true);
            clipper.AddPaths(supportPointAsCirclesIntPoint, PolyType.ptClip, true);
            foreach (var polyNode in parentPolyTree._allPolys)
            {
                clipper.AddPath(polyNode.Contour, PolyType.ptClip, true);
            }

            clipper.Execute(ClipType.ctUnion, supportLayerWithSupportCircles, PolyFillType.pftNonZero);

            return supportLayerWithSupportCircles;
        }

        internal static PolyTree MergeSupportCircles(ConcurrentBag<List<IntPoint>> supportPointAsCirclesIntPoint)
        {
            var supportLayerWithSupportCircles = new PolyTree();
            var clipper = new Clipper();
            clipper.AddPath(new List<IntPoint>(), PolyType.ptSubject, true);
            clipper.AddPaths(supportPointAsCirclesIntPoint.ToList(), PolyType.ptClip, true);

            clipper.Execute(ClipType.ctUnion, supportLayerWithSupportCircles, PolyFillType.pftNonZero);

            return supportLayerWithSupportCircles;
        }


        private static List<List<IntPoint>> CalcModelIntersectionLines(PolyTree currentLayer, PolyTree supportLayer)
        {
            var totalSupportContourSublines = new List<List<IntPoint>>();

            var intersectPoly1 = IntersectModelSliceLayer(currentLayer, supportLayer);
            var intersectPoly2 = DifferenceModelSliceLayer(supportLayer, currentLayer);
            //TriangleHelper.SavePolyNodesContourToPng(intersectPoly1._allPolys, "int1");
            //TriangleHelper.SavePolyNodesContourToPng(intersectPoly2._allPolys, "int2");

            foreach (var intersectPolyNode2 in intersectPoly2._allPolys.Where(s => !s.IsHole))
            {
                for (var intersectPolyNode2PointIndex = 0; intersectPolyNode2PointIndex < intersectPolyNode2.Contour.Count; intersectPolyNode2PointIndex++)
                {
                    var intersectPolyNode2Point = intersectPolyNode2.Contour[intersectPolyNode2PointIndex];

                    //check if point exists in  polynode 2
                    var pointFound = false;
                    foreach (var intersectPolyNode1 in intersectPoly1._allPolys.Where(s => !s.IsHole))
                    {
                        foreach (var point in intersectPolyNode1.Contour)
                        {
                            if (point == intersectPolyNode2Point)
                            {
                                pointFound = true;
                                break;
                            }
                        }
                    }

                    if (!pointFound)
                    {
                        //reset point to far off
                        intersectPolyNode2.Contour[intersectPolyNode2PointIndex] = new IntPoint(int.MaxValue, int.MaxValue);
                    }

                }
            }

            //get sublines from polynode2
            foreach (var polynode2 in intersectPoly2._allPolys.Where(s => !s.IsHole))
            {
                var polyNodeSubLines = new List<List<IntPoint>>();
                var subline = new List<IntPoint>();
                for (var subLinePointIndex = 0; subLinePointIndex < polynode2.Contour.Count; subLinePointIndex++)
                {
                    var sublinePoint = polynode2.Contour[subLinePointIndex];

                    if (sublinePoint.X != int.MaxValue)
                    {
                        subline.Add(sublinePoint);
                    }
                    else
                    {
                        if (subline.Count > 0)
                        {
                            polyNodeSubLines.Add(subline);
                            subline = new List<IntPoint>();
                        }
                    }

                    //when last index check if first point in contour is sublinepoint
                    if (subLinePointIndex == polynode2.Contour.Count - 1)
                    {
                        if (polynode2.Contour[0].X != int.MaxValue)
                        {
                            //merge current subline list with first array list
                            subline.AddRange(polyNodeSubLines[0]);
                            polyNodeSubLines.RemoveAt(0);
                            polyNodeSubLines.Add(subline);
                        }
                        else
                        {
                            if (subline.Count > 0)
                            {
                                polyNodeSubLines.Add(subline);
                                subline = new List<IntPoint>();
                            }
                        }
                    }

                }

                totalSupportContourSublines.AddRange(polyNodeSubLines);
            }


            return totalSupportContourSublines;
        }

        internal static List<IntPoint> ConvertSupportPointsToCircles(IntPoint supportLayerSupportPoint, float materialSupportInfillDistance)
        {
            var circleDiameter = materialSupportInfillDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR;

            var supportPointAsCircleIntPoint = new List<IntPoint>();
            var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, circleDiameter, 16, new Vector3Class(supportLayerSupportPoint.X, supportLayerSupportPoint.Y, 0));
            foreach (var supportPointOnCircle in supportPointAsCircle)
            {
                supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle.X, supportPointOnCircle.Y, supportLayerSupportPoint.Z));
            }

            return supportPointAsCircleIntPoint;
        }

        internal static List<List<IntPoint>> ConvertSupportPointsToCircles(List<Vector3> supportLayerSupportPoints, float materialSupportInfillDistance)
        {
            var circleDiameter = materialSupportInfillDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR;

            var supportPointCircles = new List<List<IntPoint>>();
            foreach (var supportPoint in supportLayerSupportPoints)
            {

                var supportPointAsCircleIntPoint = new List<IntPoint>();
                var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, circleDiameter, 16, new Vector3Class(supportPoint.X * CONTOURPOINT_TO_VECTORPOINT_FACTOR, supportPoint.Y * CONTOURPOINT_TO_VECTORPOINT_FACTOR, 0));
                foreach (var supportPointOnCircle in supportPointAsCircle)
                {
                    supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle.X, supportPointOnCircle.Y, supportPoint.Z));
                }
                supportPointCircles.Add(supportPointAsCircleIntPoint);
            }

            return supportPointCircles;
        }

        internal static List<List<IntPoint>> ConvertSupportPointsToCircles(List<MagsAISurfaceIntersectionData> supportLayerSupportPoints, float materialSupportInfillDistance)
        {
            var supportPointCircles = new List<List<IntPoint>>();
            foreach (var supportPoint in supportLayerSupportPoints)
            {
                var circleDiameter = materialSupportInfillDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR * supportPoint.OverhangDistance;
                var supportPointAsCircleIntPoint = new List<IntPoint>();
                var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, circleDiameter, 16, new Vector3Class(supportPoint.TopPoint.X, supportPoint.TopPoint.Y, 0));
                foreach (var supportPointOnCircle in supportPointAsCircle)
                {
                    supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle.X, supportPointOnCircle.Y, supportPoint.TopPoint.Z));
                }
                supportPointCircles.Add(supportPointAsCircleIntPoint);
            }

            return supportPointCircles;
        }

        private static List<List<IntPoint>> ConvertSupportPointsToCircles(List<MagsAIIntersectionPointWithFilter> supportLayerSupportPoints, float materialSupportInfillDistance)
        {
            var supportPointCircles = new List<List<IntPoint>>();
            var circleDiameter = materialSupportInfillDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR;
            foreach (var supportPoint in supportLayerSupportPoints)
            {
                var supportPointAsCircleIntPoint = new List<IntPoint>();
                var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, circleDiameter, 16, new Vector3Class(supportPoint.Point.X, supportPoint.Point.Y, 0));
                foreach (var supportPointOnCircle in supportPointAsCircle)
                {
                    supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle.X, supportPointOnCircle.Y, supportPoint.Point.Z));
                }
                supportPointCircles.Add(supportPointAsCircleIntPoint);
            }

            return supportPointCircles;
        }

        private static PolyTree CalcOffsetIntersectionPoints(PolyTree currentContourSubtractSupportLayer, PolyTree offsetPolyTree)
        {
            var clipper = new Clipper();
            foreach (var child in currentContourSubtractSupportLayer._allPolys)
            {
                // if (!child.IsHole)
                // {
                clipper.AddPath(child.Contour, PolyType.ptSubject, true);
                // }
            }

            foreach (var child in offsetPolyTree.Childs)
            {
                //if (!child.IsHole)
                // {
                clipper.AddPath(child.Contour, PolyType.ptClip, true);
                // }
            }

            var supportLayerWithOffsetContour = new PolyTree();
            clipper.Execute(ClipType.ctDifference, supportLayerWithOffsetContour, PolyFillType.pftNonZero);

            return supportLayerWithOffsetContour;
        }

        private static List<List<IntPoint>> ConvertSupportContourLinesToSupportCircles(List<List<IntPoint>> totalSupportContourLines, STLModel3D overhangModel, float materialSupportConeInfillDistance, Dictionary<float, List<PolyNode>> facingDownSibblingNodes, List<IntPoint> currentLayerSupportPoints, Dictionary<IntPoint, Vector3Class> checkedLayerSupportPoints, float sliceHeight, int sliceIndex, float previousSliceHeight, float materialSupportInfillDistance, float materialSupportConeOverhangDistance)
        {
            var supportPointAsCirclesIntPoint = new List<List<IntPoint>>();
            foreach (var totalSupportContourLine in totalSupportContourLines)
            {
                if (totalSupportContourLine.Count == 1)
                {
                    checkedLayerSupportPoints.Add(totalSupportContourLine[0], new Vector3Class());

                    var supportPointAsCircleIntPoint = new List<IntPoint>();
                    var supportPoint = totalSupportContourLine[0].AsVector3().Xy;
                    var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, materialSupportInfillDistance, 16, new Vector3Class(supportPoint.X, supportPoint.Y, 0));

                    foreach (var supportPointOnCircle in supportPointAsCircle)
                    {
                        supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle));
                    }

                    supportPointAsCirclesIntPoint.Add(supportPointAsCircleIntPoint);
                }
                else
                {
                    var uncheckSupportPoints = VectorHelper.GetPointsOnPath(totalSupportContourLine, materialSupportInfillDistance, false, true, 0f);

                    //FILTER on NEXT MODEL CONTOUR INTERSECTION
                    var nextSliceHeight = sliceIndexKeys[sliceIndex + 1];
                    var modelIntersectedSupportPoints = new List<List<IntPoint>>();
                    modelIntersectedSupportPoints.Add(new List<IntPoint>());
                    foreach (var supportContourPoint in uncheckSupportPoints[0])
                    {
                        if (facingDownSibblingNodes.ContainsKey(nextSliceHeight))
                        {
                            if (facingDownSibblingNodes[nextSliceHeight] != null)
                            {
                                if (PointInModelContour(supportContourPoint, facingDownSibblingNodes[nextSliceHeight]))
                                {
                                    modelIntersectedSupportPoints[0].Add(supportContourPoint);
                                }
                            }
                        }
                    }

                    //FILTER on supportconedistance
                    var supportConeDistanceCheckedSupportPoints = new List<IntPoint>();

                    foreach (var supportPoint in modelIntersectedSupportPoints[0])
                    {
                        var distanceToSmall = false;
                        //check all support cones for distance
                        foreach (var currentSupportPoint in currentLayerSupportPoints)
                        {
                            var supportPointDistance = (currentSupportPoint - supportPoint).Length;
                            if (supportPointDistance > -(materialSupportConeOverhangDistance / 2) && supportPointDistance < (materialSupportConeOverhangDistance / 2))
                            {
                                //distanceToSmall = true;
                                break;
                            }
                        }

                        if (!distanceToSmall)
                        {
                            supportConeDistanceCheckedSupportPoints.Add(supportPoint);
                        }
                    }

                    //FILTER on 3d model intersectionpoint
                    var modelIntersectionCheckedSupportPoints = new Dictionary<Vector3Class, Vector3Class>();

                    foreach (var supportContourPoint in supportConeDistanceCheckedSupportPoints)
                    {
                        var modelIntersection = new TriangleIntersection();
                        modelIntersection.IntersectionPoint = supportContourPoint.AsVector3();
                        modelIntersection.IntersectionPoint.Z = sliceHeight;
                        var modelIntersectionAsIntPoint = new IntPoint(modelIntersection.IntersectionPoint);
                        if (modelIntersection != new TriangleIntersection() && !modelIntersectionCheckedSupportPoints.ContainsKey(modelIntersection.IntersectionPoint) && !checkedLayerSupportPoints.ContainsKey(modelIntersectionAsIntPoint))
                        {
                            modelIntersectionCheckedSupportPoints.Add(modelIntersection.IntersectionPoint, modelIntersection.Normal);
                            checkedLayerSupportPoints.Add(modelIntersectionAsIntPoint, modelIntersection.Normal);
                        }
                    }

                    //convert support points to circle
                    foreach (var checkedSupportPoint in supportConeDistanceCheckedSupportPoints)
                    {
                        var supportPointAsCircleIntPoint = new List<IntPoint>();
                        //* (checkedSupportPoint.Value.Z + 2)
                        var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, materialSupportInfillDistance, 16, new Vector3Class(checkedSupportPoint.X, checkedSupportPoint.Y, 0));

                        foreach (var supportPointOnCircle in supportPointAsCircle)
                        {
                            supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle));
                        }

                        supportPointAsCirclesIntPoint.Add(supportPointAsCircleIntPoint);
                    }
                }
            }
            return supportPointAsCirclesIntPoint;
        }

        private static bool PointInModelContour(IntPoint point, List<PolyNode> listOfPolyNodes, List<PolyNode> polyNodes = null)
        {
            var result = false;
            foreach (var polyNode in listOfPolyNodes)
            {
                if (Clipper.PointInPolygon(point, polyNode.Contour) != 0)
                {
                    if (polyNode.IsHole && result)
                    {
                        //point
                        result = false;
                    }
                    else if (!polyNode.IsHole)
                    {
                        result = true;

                        if (polyNodes != null)
                        {
                            polyNodes.Add(polyNode);
                        }
                    }
                }
            }

            return result;
        }
    }
}
