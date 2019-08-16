using Atum.DAL.Materials;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines
{
    internal class MagsAIEngine
    {
        internal static Dictionary<float, PolyTree> ModelContoursFacingDown { get; set; }
        internal static Dictionary<float, PolyTree> ModelContoursDifference { get; set; }

        internal static List<Vector3> Calculate(STLModel3D stlModel, Material selectedMaterial)
        {
            var selectedMaterialSupportProfile = selectedMaterial.SupportProfiles[0];
            selectedMaterialSupportProfile.SupportLowestPointsDistance = 5; //mm
            selectedMaterialSupportProfile.SupportOverhangDistance = 0.5f;
            selectedMaterialSupportProfile.SupportTopHeight = 2f;
            selectedMaterialSupportProfile.SupportBottomWidthCorrection = 0.005f;

            GC.Collect();
            GC.Collect();

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var results = new List<Vector3>();

            stlModel.UpdateBoundries();
            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 5 });

            stlModel.CalcSliceIndexes(MaterialManager.DefaultMaterial, true);

            //intersections
            Debug.WriteLine("Slice Indexes: " + stopwatch.ElapsedMilliseconds + "ms");

            TriangleHelper.CleanPolyNodesContourToPng();

            stopwatch.Reset();
            stopwatch.Start();

            var sliceIndex = 0;
            var stopSlice = 800;
            var amountOfSibblingLayers = 40;
            var debugMode = true;

            var supportLayerPolyTree = new PolyTree();

            ModelContoursDifference = new Dictionary<float, PolyTree>();
            ModelContoursFacingDown = new Dictionary<float, PolyTree>();

            var modelContours = new SortedDictionary<float, PolyTree>();
            foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
            {
                modelContours.Add(sliceHeight, null);
                ModelContoursDifference.Add(sliceHeight, null);
                //ModelContoursFacingDown.Add(sliceHeight, new PolyTree());
            }

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 30 });

            #region ZSupport

            PreCacheModelContours(stlModel, modelContours, sliceIndex, debugMode, stopSlice + 40);

            GC.Collect();
            GC.Collect();

            var totalSupportedLayer = new PolyTree();
            Debug.WriteLine("Precache: " + stopwatch.ElapsedMilliseconds + "ms");

            var supportLayerSupportPoints = new Dictionary<IntPoint, Vector3>();
            var facingDownContours = new Dictionary<PolygonLowestPoint, Dictionary<float, List<PolyNode>>>();

            sliceIndex = 0;

            stopwatch.Reset();
            stopwatch.Start();

            //do not multithread. This will lock on CalcContourSupportPoints
            for (var sliceIndex2 = 0; sliceIndex2 < stlModel.SliceIndexes.Count - 1; sliceIndex2++)
            {
                //foreach(var sliceHeight in ModelContoursFacingDown.Keys){
                var sliceIndexAsync = (int)sliceIndex2;
                var sliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndexAsync);
                if (ModelContoursFacingDown.ContainsKey(sliceHeightAsync))
                {
                    var facingDownPolygons = CalcContourSupportPoints(ModelContoursFacingDown[sliceHeightAsync], selectedMaterialSupportProfile, sliceIndexAsync, sliceHeightAsync, stlModel);

                    foreach (var facingDownPolygon in facingDownPolygons)
                    {
                        var processingStopWatch = new Stopwatch();
                        processingStopWatch.Start();
                        var connectedLayerPolyNodes = new Dictionary<float, List<PolyNode>>();

                        foreach (var upperSliceHeight in stlModel.SliceIndexes.Keys)
                        {
                            if (upperSliceHeight == sliceHeightAsync)
                            {
                                facingDownPolygon.SliceHeight = sliceHeightAsync;
                                facingDownPolygon.SliceIndex = sliceIndexAsync;
                                lock (facingDownContours)
                                {
                                    facingDownContours.Add(facingDownPolygon, new Dictionary<float, List<PolyNode>>());
                                }

                                break;
                            }
                        }
                    }
                }
            }//);

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 40 });

            stopwatch.Reset();
            stopwatch.Start();
            MagsAIEngineFilters.DetermineIsFaceDownContoursHasEdgeDown(facingDownContours, modelContours, selectedMaterialSupportProfile.SupportLowestPointsOffset[0] * selectedMaterialSupportProfile.SupportOverhangDistance, stlModel);
            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter DetermineIsFaceDownContoursHasEdgeDown", stopwatch.ElapsedMilliseconds + "ms");

            //determine layerdown contour sibbling chils
            stopwatch.Reset();
            stopwatch.Start();
            Parallel.ForEach(facingDownContours, facingDownContourAsync =>
            //foreach(var facingDownContourAsync in facingDownContours)
            {
                var facingDownContour = facingDownContourAsync;
                if (!facingDownContour.Key.HasEdgeDown)
                {
                    var connectedLayerPolyNodes = new Dictionary<float, List<PolyNode>>();

                    //find upper layer polynode by using triangle connections
                    var currentSibblingLayerIndex = 0;
                    foreach (var upperSliceHeight in stlModel.SliceIndexes.Keys)
                    {
                        if (upperSliceHeight == facingDownContour.Key.SliceHeight)
                        {

                            connectedLayerPolyNodes.Add(facingDownContour.Key.SliceHeight, new List<PolyNode>() { facingDownContour.Key.Polygon });

                            for (var a = currentSibblingLayerIndex + 1; a < amountOfSibblingLayers; a++)
                            {
                                if (stlModel.SliceIndexes.Count - 1 > (facingDownContour.Key.SliceIndex + a))
                                {
                                    var currentLayerHeight = stlModel.SliceIndexes.Keys.ElementAt(facingDownContour.Key.SliceIndex + a - 1);
                                    var nextLayerHeight = stlModel.SliceIndexes.Keys.ElementAt(facingDownContour.Key.SliceIndex + a);

                                    if (modelContours.ContainsKey(nextLayerHeight) && modelContours[nextLayerHeight] != null)
                                    {
                                        //find next layer polygon by using triangle connection
                                        var connectedNextContourFound = false;
                                        var toMuchConnectedNodesFound = false;
                                        foreach (var nextModelContour in modelContours[nextLayerHeight]._allPolys)
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
                                                                facingDownContour.Key.HasComplexConnectedUpperChildContour = true;
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
                                            AutoSupportHelpers.FindNextChildConnection(stlModel, currentLayerHeight, nextLayerHeight, connectedLayerPolyNodes, modelContours);
                                        }
                                    }
                                }
                            }

                            lock (facingDownContours)
                            {
                                foreach (var connectedLayerPolyNode in connectedLayerPolyNodes)
                                {
                                    facingDownContour.Value.Add(connectedLayerPolyNode.Key, connectedLayerPolyNode.Value);
                                }

                            }

                            break;
                        }
                    }
                }
            });

            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Sibbling layers", stopwatch.ElapsedMilliseconds + "ms");


            GC.Collect();
            GC.Collect();

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 50 });

            stopwatch.Reset();
            stopwatch.Start();

            MagsAIEngineFilters.FilterOnLowestPointsZIntersections(facingDownContours, selectedMaterialSupportProfile.SupportTopRadius);

            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter FilterOnLowestPointsZIntersections", stopwatch.ElapsedMilliseconds + "ms");

            //get model intersection points
            stopwatch.Reset();
            stopwatch.Start();

            var facingDownIndex = 0;
            Parallel.ForEach(facingDownContours, facingDownContourItem =>
            //foreach(var facingDownContourItem in facingDownContours)
            {
                var facingDownContour = facingDownContourItem;
                var facingDownParent = facingDownContour.Key;
                var facingDownParentPolygon = facingDownParent.Polygon;
                var facingDownParentSupportPoints = facingDownParent.LowestPoints;
                var facingDownParentSibblingPolyNodes = facingDownContour.Value;
                var facingDownParentSliceIndex = facingDownParent.SliceIndex;
                var facingDownFirstOverhangContour = ClipperOffset(facingDownParentPolygon.Contour, -selectedMaterialSupportProfile.SupportLowestPointsDistance / 4);
                var facingDownFirstOverhangContourHasHoleChild = !facingDownParentPolygon.IsHole && facingDownParentPolygon.ChildCount > 0;

                if (facingDownContour.Value.Count > 0)
                {

                    //determine the intersection next layer intersection point by using the listOfLowestPointsOffset values 
                    if (!facingDownParent.HasEdgeDown)
                    {
                        var listOfLowestPointsDistanceOffsetIndex = 0;
                        var previousListOfLowestPointOffset = -1f;
                        foreach (var listOfLowestPointOffset in selectedMaterialSupportProfile.SupportLowestPointsOffset)
                        {
                            if (previousListOfLowestPointOffset == -1 || facingDownParent.LowestPointsWithOffset.ContainsKey(previousListOfLowestPointOffset))
                            {
                                var facingDownParentSliceHeight = facingDownParent.SliceHeight;
                                var previousLayerSliceHeight = facingDownParentSliceHeight;

                                var lowestPointsCircles = ConvertSupportPointsToCircles(facingDownParent.LowestPoints, (selectedMaterialSupportProfile.SupportLowestPointsDistance * listOfLowestPointOffset));
                                var lowestPointsCirclesPolyTree = MergeSupportCircles(lowestPointsCircles);
                                var maxDistance = (listOfLowestPointOffset * selectedMaterialSupportProfile.SupportLowestPointsDistance) / 2 * CONTOURPOINT_TO_VECTORPOINT_FACTOR;
                                maxDistance -= (maxDistance * 0.1f); // max 10 percentage difference

                                for (var nextLayerIndex = facingDownParentSliceIndex + 1; nextLayerIndex < facingDownParentSliceIndex + amountOfSibblingLayers; nextLayerIndex++)
                                {
                                    if (stlModel.SliceIndexes.Count - 1 > nextLayerIndex)
                                    {
                                        var nextLayerHeight = stlModel.SliceIndexes.ElementAt(nextLayerIndex).Key;

                                        if (facingDownParentSibblingPolyNodes.ContainsKey(previousLayerSliceHeight))
                                        {
                                            var previousLayerWithHeightByPolygon = facingDownParentSibblingPolyNodes[previousLayerSliceHeight];
                                            var previousLayerWithOffset = previousLayerWithHeightByPolygon;// ClipperOffset(previousLayer, 0.1f);

                                            if (facingDownParentSibblingPolyNodes.ContainsKey(nextLayerHeight))
                                            {
                                                var currentLayerWithHeightByPolygon = facingDownParentSibblingPolyNodes[nextLayerHeight];
                                                if (currentLayerWithHeightByPolygon != null)
                                                {
                                                    var b = new PolyTree();
                                                    foreach (var child in currentLayerWithHeightByPolygon)
                                                    {
                                                        b._allPolys.Add(child);
                                                    }
                                                    var totalLowestContourLines = CalcModelIntersectionLines(b, lowestPointsCirclesPolyTree, nextLayerHeight);
                                                    if (totalLowestContourLines.Count > 0)
                                                    {
                                                        var modelIntersectionFilteredPoints = new Dictionary<IntPoint, Vector3>();
                                                        ConvertSupportContourLinesToSupportCircles(totalLowestContourLines, stlModel, (listOfLowestPointOffset * selectedMaterialSupportProfile.SupportLowestPointsDistance), facingDownParentSibblingPolyNodes, supportLayerSupportPoints.Keys.ToList(), modelIntersectionFilteredPoints, nextLayerHeight, nextLayerIndex, previousLayerSliceHeight, selectedMaterialSupportProfile.SupportInfillDistance * selectedMaterialSupportProfile.SupportLowestPointsDistanceOffset[listOfLowestPointsDistanceOffsetIndex], selectedMaterialSupportProfile.SupportOverhangDistance);

                                                        foreach (var modelIntersectionFilteredPoint in modelIntersectionFilteredPoints)
                                                        {
                                                            var pointAllreadyExists = false;
                                                            if (facingDownParent.LowestPointsWithOffset.ContainsKey(listOfLowestPointOffset))
                                                            {
                                                                foreach (var lowestPoint in facingDownParent.LowestPointsWithOffset[listOfLowestPointOffset])
                                                                {
                                                                    if ((modelIntersectionFilteredPoint.Key.XY - lowestPoint.Point.XY).Length < maxDistance)
                                                                    {
                                                                        pointAllreadyExists = true;
                                                                        break;
                                                                    }
                                                                }
                                                            }

                                                            if (!pointAllreadyExists)
                                                            {
                                                                var modelIntersectionPoint = modelIntersectionFilteredPoint.Key;
                                                                var lowestPointNearest = facingDownContour.Key.FindNearestLowestPoint(modelIntersectionPoint);

                                                                //check if point is within largest contour without hole child
                                                                if (!IsPointConnectionInSamePolygon(b, lowestPointNearest, modelIntersectionPoint))
                                                                {
                                                                    var pointExistsInFirstOverhangContour = false;
                                                                    if (!facingDownFirstOverhangContourHasHoleChild)
                                                                    {
                                                                        foreach (var facingDownFirstOverhangContourChild in facingDownFirstOverhangContour.Childs)
                                                                        {
                                                                            if (!facingDownFirstOverhangContourChild.IsHole)
                                                                            {
                                                                                //is within contour
                                                                                if (Clipper.PointInPolygon(modelIntersectionPoint, facingDownFirstOverhangContourChild.Contour) != 0)
                                                                                {
                                                                                    //skip modelintersection
                                                                                    pointExistsInFirstOverhangContour = true;
                                                                                    break;
                                                                                }
                                                                            }
                                                                        }

                                                                    }

                                                                    if (!pointExistsInFirstOverhangContour)
                                                                    {
                                                                        if (!facingDownParent.LowestPointsWithOffset.ContainsKey(listOfLowestPointOffset))
                                                                        {
                                                                            facingDownParent.LowestPointsWithOffset.Add(listOfLowestPointOffset, new List<IntersectionPointWithFilter>());
                                                                        }

                                                                        facingDownParent.LowestPointsWithOffset[listOfLowestPointOffset].Add(new IntersectionPointWithFilter() { Point = modelIntersectionPoint });
                                                                    }
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                break;
                                            }
                                        }
                                        previousLayerSliceHeight = nextLayerHeight;
                                    }
                                }
                            }
                            listOfLowestPointsDistanceOffsetIndex++;

                            previousListOfLowestPointOffset = listOfLowestPointOffset;
                        }
                    }
                }
            });

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 80 });

            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "ModelIntersections", stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();
            MagsAIEngineFilters.FilterOnLowestPointsDuplicates(facingDownContours);

            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter FilterOnLowestPointsDuplicates", stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();
            foreach (var facingDownContourItem in facingDownContours)
            {
                //add support
                foreach (var lowestPointSupportPoint in facingDownContourItem.Key.LowestPoints)
                {
                    if (lowestPointSupportPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        //add lowest point support cones
                        var triangleIntersectionBelow = new Vector3();
                        var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, lowestPointSupportPoint.Point, facingDownContourItem.Key.SliceHeight - 0.05f, triangleIntersectionBelow);
                        facingDownContourItem.Key.LowestIntersectionPoints.Add(new IntersectionData() { BottomPoint = triangleIntersectionBelow, TopPoint = triangleIntersection.IntersectionPoint });
                    }
                }
            }


            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Added LowestIntersectionPoints", stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();

            foreach (var facingDownContourItem in facingDownContours)
            {

                foreach (var contourOffsetItem in facingDownContourItem.Key.LowestPointsWithOffset)
                {
                    //add lowest point support cones
                    foreach (var supportPoint in contourOffsetItem.Value)
                    {
                        var triangleIntersectionBelow = new Vector3();
                        if (supportPoint.Filter == typeOfAutoSupportFilter.None)
                        {
                            var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, supportPoint.Point, facingDownContourItem.Key.SliceHeight, triangleIntersectionBelow);
                            var intersectedData = new IntersectionData() { BottomPoint = triangleIntersectionBelow, TopPoint = triangleIntersection.IntersectionPoint };
                            if (!facingDownContourItem.Key.LowestPointsWithOffsetIntersectionPoint.ContainsKey(contourOffsetItem.Key))
                            {
                                facingDownContourItem.Key.LowestPointsWithOffsetIntersectionPoint.Add(contourOffsetItem.Key, new List<IntersectionData>());
                            }
                            facingDownContourItem.Key.LowestPointsWithOffsetIntersectionPoint[contourOffsetItem.Key].Add(intersectedData);
                            if (triangleIntersection.IntersectionPoint.Z < facingDownContourItem.Key.LowestIntersectionPointsTop)
                            {
                                intersectedData.Filter = typeOfAutoSupportFilter.FilteredByLowestPointOffsetLowerThenLowerPointTop;
                            }
                        }
                    }
                }

                facingDownIndex++;
            }

            MagsAIEngineFilters.FilterOnLowestPointsOffsetWithLowestPointIntersections(facingDownContours, selectedMaterialSupportProfile.SupportTopRadius);
            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter FilterOnLowestPointsOffsetWithLowestPointIntersections", stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();

            MagsAIEngineFilters.FilterOnLowestOffsetPointsXYDistance(facingDownContours, selectedMaterialSupportProfile.SupportTopRadius, selectedMaterialSupportProfile.SupportOverhangDistance);
            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Filter FilterOnLowestOffsetPointsXYDistance", stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();

            facingDownIndex = 0;

            FacingDownContoursDebug = facingDownContours;

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 85 });

            foreach (var facingDownContourItem in facingDownContours)
            {
                foreach (var lowestPointSupportPoint in facingDownContourItem.Key.LowestIntersectionPoints)
                {
                    SupportEngine.AddManualSupport(stlModel, lowestPointSupportPoint.TopPoint, Properties.Settings.Default.DefaultSupportColor, selectedMaterial);
                }

                var supportColors = new System.Drawing.Color[7];
                supportColors[0] = System.Drawing.Color.Aqua;
                supportColors[1] = System.Drawing.Color.Blue;
                supportColors[2] = System.Drawing.Color.Green;
                supportColors[3] = System.Drawing.Color.GreenYellow;
                supportColors[4] = System.Drawing.Color.Yellow;
                supportColors[5] = System.Drawing.Color.Orange;
                supportColors[6] = System.Drawing.Color.Red;

                foreach (var contourOffsetItem in facingDownContourItem.Key.LowestPointsWithOffsetIntersectionPoint)
                {
                    //add lowest point support cones
                    foreach (var supportPoint in contourOffsetItem.Value)
                    {
                        if (supportPoint.Filter == typeOfAutoSupportFilter.None)
                        {
                            var supportConeColor = supportColors[0];
                            if (contourOffsetItem.Key == selectedMaterialSupportProfile.SupportLowestPointsOffset[1])
                            {
                                supportConeColor = supportColors[1];
                            }
                            else if (contourOffsetItem.Key == selectedMaterialSupportProfile.SupportLowestPointsOffset[2])
                            {
                                supportConeColor = supportColors[2];
                            }
                            else if (contourOffsetItem.Key == selectedMaterialSupportProfile.SupportLowestPointsOffset[3])
                            {
                                supportConeColor = supportColors[3];
                            }

                            if (facingDownContourItem.Key.HasComplexConnectedUpperChildContour)
                            {
                                supportConeColor = supportColors[5];
                            }

                            if (supportPoint.TopPoint.Z - facingDownContourItem.Key.SliceHeight > 40 * 0.05)
                            {
                                supportConeColor = supportColors[6];
                            }
                            else
                            {
                                if (!facingDownContourItem.Key.HasComplexConnectedUpperChildContour)
                                {
                                    SupportEngine.AddManualSupport(stlModel, supportPoint.TopPoint, supportConeColor, selectedMaterial);
                                }
                            }
                        }
                    }
                }

                facingDownIndex++;
            }

            GC.Collect();
            GC.Collect();

            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Creating Z-support", stopwatch.ElapsedMilliseconds + "ms");

            #endregion


            //normal support points
            ModelContoursDifference = new Dictionary<float, PolyTree>();

            foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
            {
                ModelContoursDifference.Add(sliceHeight, null);
            }


            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 88 });

            PreCacheModelLayerDifference(stlModel, modelContours, 0, false, stopSlice);


            var supportPointContours = new List<PolygonSupportPoint>();
            ProcessOverhangLayers(stlModel, modelContours, facingDownContours, supportPointContours, selectedMaterialSupportProfile);

            GC.Collect();
            GC.Collect();

            DAL.Managers.LoggingManager.WriteToLog("AutoSupportEngine Manager", "Overhang support", stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 90 });

            //remove support points that exists within lowest offset polygons
            MagsAIEngineFilters.FilterOnSupportPointsWithinLowestPointOffsetXYDistance(facingDownContours, supportPointContours, selectedMaterialSupportProfile.SupportTopRadius);
            MagsAIEngineFilters.FilterOnSupportPointWithinLowestPointsRange(facingDownContours, supportPointContours, selectedMaterialSupportProfile.SupportTopRadius);
            MagsAIEngineFilters.FilterOnSupportPointDistanceWithinPolygon(supportPointContours, selectedMaterialSupportProfile.SupportTopRadius);

            foreach (var supportPointContour in supportPointContours)
            {
                foreach (var supportIntersectionPoint in supportPointContour.SupportIntersections)
                {
                    if (supportIntersectionPoint.Filter == typeOfAutoSupportFilter.None)
                    {
                        SupportEngine.AddManualSupport(stlModel, supportIntersectionPoint.TopPoint, System.Drawing.Color.White, selectedMaterial);
                    }
                }
            }

            ModelProgressChanged?.Invoke(null, new MagsAIProgressEventArgs() { Percentage = 100 });

            return results;
        }

        private static void ProcessOverhangLayers(STLModel3D stlModel, SortedDictionary<float, PolyTree> modelContours, Dictionary<PolygonLowestPoint, Dictionary<float, List<PolyNode>>> facingDownContours, List<PolygonSupportPoint> supportPointContours, SupportProfile selectedMaterialSupportProfile)
        {
            var sliceIndex = 0;
            var previousSliceHeight = 0f;
            var modelContourWithOffset = new ConcurrentDictionary<float, PolyTree>();
            var modelHeightSupportPoints = new ConcurrentDictionary<float, List<Vector3>>();


            //precache
            Parallel.ForEach(stlModel.SliceIndexes.Keys, sliceHeightAsync =>
            {
                var sliceHeight = sliceHeightAsync;
                var contourWithOffset = ClipperOffset(modelContours[sliceHeight], selectedMaterialSupportProfile.SupportOverhangDistance / 2);

                modelContourWithOffset.TryAdd(sliceHeight, contourWithOffset);
            });

            //precache supportcone tops
            Parallel.ForEach(stlModel.SliceIndexes.Keys, sliceHeightAsync =>
            {
                var sliceHeight = sliceHeightAsync;
                if (ModelContoursDifference.ContainsKey(sliceHeight) && ModelContoursDifference[sliceHeight] != null && modelContourWithOffset.ContainsKey(sliceHeight))
                {
                    foreach (var modelContourDifference in ModelContoursDifference[sliceHeight]._allPolys)
                    {
                        if (!modelContourDifference.IsHole)
                        {
                            foreach (var facingDownContourItem in facingDownContours)
                            {
                                foreach (var facingDownLowestPointIntersection in facingDownContourItem.Key.LowestIntersectionPoints)
                                {
                                    if (!facingDownLowestPointIntersection.RemovedByLayerHeight)
                                    {
                                        if (facingDownLowestPointIntersection.TopPoint.Z <= sliceHeight)
                                        {
                                            if (Clipper.PointInPolygon(new IntPoint(facingDownLowestPointIntersection.TopPoint.X * CONTOURPOINT_TO_VECTORPOINT_FACTOR, facingDownLowestPointIntersection.TopPoint.Y * CONTOURPOINT_TO_VECTORPOINT_FACTOR, 0), modelContourDifference.Contour) != 0)
                                            {
                                                facingDownLowestPointIntersection.RemovedByLayerHeight = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    //support points
                    foreach (var modelContourDifference in ModelContoursDifference[sliceHeight]._allPolys)
                    {
                        if (!modelContourDifference.IsHole)
                        {
                            foreach (var supportPointItem in supportPointContours)
                            {
                                foreach (var supportPointIntersection in supportPointItem.SupportIntersections)
                                {
                                    if (!supportPointIntersection.RemovedByLayerHeight)
                                    {
                                        if (supportPointIntersection.TopPoint.Z <= sliceHeight)
                                        {
                                            if (Clipper.PointInPolygon(new IntPoint(supportPointIntersection.TopPoint.X * CONTOURPOINT_TO_VECTORPOINT_FACTOR, supportPointIntersection.TopPoint.Y * CONTOURPOINT_TO_VECTORPOINT_FACTOR, 0), modelContourDifference.Contour) != 0)
                                            {
                                                supportPointIntersection.RemovedByLayerHeight = true;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    var currentSupportPoints = new List<Vector3>();
                    foreach (var facingDownContourItem in facingDownContours)
                    {
                        foreach (var facingDownLowestPointIntersection in facingDownContourItem.Key.LowestIntersectionPoints)
                        {
                            if (facingDownLowestPointIntersection.Filter == typeOfAutoSupportFilter.None)
                            {
                                if (!facingDownLowestPointIntersection.RemovedByLayerHeight && facingDownLowestPointIntersection.TopPoint.Z < sliceHeight)
                                {
                                    currentSupportPoints.Add(facingDownLowestPointIntersection.TopPoint);
                                }
                            }
                        }
                    }

                    //append support points that intersect with layer
                    foreach (var supportPointItem in supportPointContours)
                    {
                        foreach (var supportPointIntersection in supportPointItem.SupportIntersections)
                        {
                            if (supportPointIntersection.Filter == typeOfAutoSupportFilter.None)
                            {
                                if (!supportPointIntersection.RemovedByLayerHeight && supportPointIntersection.TopPoint.Z < sliceHeight)
                                {
                                    currentSupportPoints.Add(supportPointIntersection.TopPoint);
                                }
                            }
                        }
                    }

                    modelHeightSupportPoints.TryAdd(sliceHeight, currentSupportPoints);
                }
            });

            foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
            {
                if (sliceIndex > 0)
                {
                    if (ModelContoursDifference.ContainsKey(sliceHeight) && ModelContoursDifference[sliceHeight] != null && modelContourWithOffset.ContainsKey(sliceHeight))
                    {
                        var currentSupportPoints = modelHeightSupportPoints[sliceHeight];

                        if (currentSupportPoints.Count > 0)
                        {
                            var circlesToRemoveFromLayer = new List<List<IntPoint>>();
                            foreach (var currentLayerSupportPoint in currentSupportPoints)
                            {
                                var supportPointAsCircleIntPoint = new List<IntPoint>();
                                var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, selectedMaterialSupportProfile.SupportInfillDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR / 2, 16, new Vector3(currentLayerSupportPoint.X * CONTOURPOINT_TO_VECTORPOINT_FACTOR, currentLayerSupportPoint.Y * CONTOURPOINT_TO_VECTORPOINT_FACTOR, 0));
                                foreach (var supportPointOnCircle in supportPointAsCircle)
                                {
                                    supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle.X, supportPointOnCircle.Y));
                                }

                                circlesToRemoveFromLayer.Add(supportPointAsCircleIntPoint);
                            }
                            var unionRemoveCircles = MergeSupportCircles(circlesToRemoveFromLayer);
                            var previousLayerWithOverhangOffset = UnionModelSliceLayer(modelContourWithOffset[previousSliceHeight], unionRemoveCircles);
                            var diffLayer = DifferenceModelSliceLayer(modelContours[sliceHeight], previousLayerWithOverhangOffset);

                            var support = CalcContourSupportPoints(diffLayer, selectedMaterialSupportProfile, sliceIndex, sliceHeight, stlModel);

                            if (support != null && support.Count > 0)
                            {
                                var supportIntersection = new PolygonSupportPoint
                                {
                                    SliceHeight = sliceHeight,
                                    SliceIndex = sliceIndex
                                };

                                foreach (var s in support)
                                {
                                    foreach (var sPoint in s.LowestPoints)
                                    {
                                        supportIntersection.SupportPoints.Add(sPoint.Point.AsVector3);

                                        var triangleIntersectionBelow = new Vector3();
                                        var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, new IntPoint(sPoint.Point), sPoint.Point.AsVector3.Z - 0.05f, triangleIntersectionBelow);
                                        supportIntersection.SupportIntersections.Add(new IntersectionData() { TopPoint = triangleIntersection.IntersectionPoint, BottomPoint = triangleIntersectionBelow });
                                    }
                                }

                                supportPointContours.Add(supportIntersection);

                            }
                        }
                    }

                }

                sliceIndex++;
                previousSliceHeight = sliceHeight;

            }
        }

        internal static Dictionary<PolygonLowestPoint, Dictionary<float, List<PolyNode>>> FacingDownContoursDebug = new Dictionary<PolygonLowestPoint, Dictionary<float, List<PolyNode>>>();

        internal static event EventHandler<MagsAIProgressEventArgs> ModelProgressChanged;

        private static void AddModelContourByLayer(float sliceHeight, STLModel3D model, SortedDictionary<float, PolyTree> modelContours)
        {
            var contours = model.GetSliceContours(sliceHeight);
            lock (ModelContoursFacingDown)
            {
                modelContours[sliceHeight] = contours;
            }
        }

        private static bool PointExistsInNotHolePolygon(PolyNode polyNode, IntPoint point, float sliceHeight)
        {

            if (sliceHeight == 17.1f)
            {

            }
            var result = false;
            if (!polyNode.IsHole)
            {
                if (polyNode.ChildCount == 0)
                {
                    //point inside currentPolygon
                    if (Clipper.PointInPolygon(point, polyNode.Contour) != 0)
                    {
                        result = true;
                    }
                }
                else
                {
                    if (Clipper.PointInPolygon(point, polyNode.Contour) != 0)
                    {
                        //check child (holes) if point exists{
                        var pointInChildPolyNode = false;
                        foreach (var child in polyNode.Childs)
                        {
                            //point exists in child?
                            if (Clipper.PointInPolygon(point, child.Contour) == 0)
                            {
                                pointInChildPolyNode = false;
                                break;
                            }
                        }

                        if (!pointInChildPolyNode)
                        {
                            result = true;
                        }
                    }
                }
            }

            return result;
        }

        private static void AddContourFacingDown(int sliceIndex, float sliceHeight, PolyTree currentLayer, PolyTree previousLayer, Dictionary<float, PolyTree> contoursFacingDown)
        {
            var currentPolyNodesWithoutPreviousLayerPoint = new List<PolyNode>();

            if (currentLayer != null)
            {
                //check if there are polynodes below currentlayer that is a point inside this polynode
                foreach (var currentLayerPolyNode in currentLayer._allPolys)
                {
                    var previousLayerPointWithinCurrentPolyNode = false;
                    if (!currentLayerPolyNode.IsHole)
                    {
                        foreach (var previousPolyNode in previousLayer._allPolys)
                        {
                            if (!previousPolyNode.IsHole)
                            {
                                foreach (var previousLayerPoint in previousPolyNode.Contour)
                                {
                                    if (PointExistsInNotHolePolygon(currentLayerPolyNode, previousLayerPoint, sliceHeight))
                                    {
                                        previousLayerPointWithinCurrentPolyNode = true;
                                        break;
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
                                    foreach (var currentLayerPoint in currentLayerPolyNode.Contour)
                                    {
                                        //point inside currentPolygon
                                        if (PointExistsInNotHolePolygon(previousPolyNode, currentLayerPoint, sliceHeight))
                                        {
                                            currentLayerPointWithinPreviousLayer = true;
                                            break;
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
                lock (contoursFacingDown)
                {
                    if (!contoursFacingDown.ContainsKey(sliceHeight))
                    {
                        contoursFacingDown.Add(sliceHeight, new PolyTree());
                    }
                    if (contoursFacingDown[sliceHeight] == null)
                    {
                        contoursFacingDown[sliceHeight] = new PolyTree();
                    }

                    foreach (var currentPolyNodeWithoutPreviousLayerPoint in currentPolyNodesWithoutPreviousLayerPoint)
                    {
                        contoursFacingDown[sliceHeight]._allPolys.Add(currentPolyNodeWithoutPreviousLayerPoint);
                    }
                }
            }

        }

        internal class IntersectionData
        {
            internal Vector3 TopPoint { get; set; }
            internal Vector3 BottomPoint { get; set; }
            internal bool RemovedByLayerHeight { get; set; }
            internal typeOfAutoSupportFilter Filter { get; set; }
        }

        internal enum typeOfAutoSupportFilter
        {
            None = 0,
            FilteredByLowestPointXYOffset = 1,
            FilteredByLowestPointXYRangeOffset = 2,
            FilteredByLowestPointXYWithinRangeOffset = 4,
            FilteredByLowestPointXYWithinZRangeOffset = 8,
            FilteredByDuplicatePoint = 16,
            FilteredBySupportPointWithinLowestPointRangeOffset = 32,
            FilteredByLowestPointOffsetLowerThenLowerPointTop = 64,
            FilteredByLowestPointOffsetXYWithinZRangeOffset = 128,
            FilterOnSupportPointDistanceWithinPolygon = 256,
        }

        internal class IntersectionPointWithFilter
        {
            internal IntPoint Point { get; set; }
            internal typeOfAutoSupportFilter Filter { get; set; }
        }


        internal class PolygonLowestPoint
        {
            internal PolyNode Polygon = new PolyNode();
            internal List<IntersectionPointWithFilter> LowestPoints = new List<IntersectionPointWithFilter>();
            internal List<IntersectionData> LowestIntersectionPoints = new List<IntersectionData>();
            internal Dictionary<float, List<IntersectionPointWithFilter>> LowestPointsWithOffset = new Dictionary<float, List<IntersectionPointWithFilter>>();
            internal Dictionary<float, List<IntersectionData>> LowestPointsWithOffsetIntersectionPoint = new Dictionary<float, List<IntersectionData>>();
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
        }

        internal class PolygonSupportPoint
        {
            internal PolyNode Polygon = new PolyNode();
            internal List<Vector3> SupportPoints = new List<Vector3>();
            internal List<IntersectionData> SupportIntersections = new List<IntersectionData>();
            internal int SliceIndex;
            internal float SliceHeight;
        }

        private static void PreCacheModelLayerDifference(STLModel3D stlModel, SortedDictionary<float, PolyTree> modelContours, int startingSliceIndex, bool debugMode, int stopIndex = -1)
        {
            var precacheAmount = stopIndex == -1 ? stlModel.SliceIndexes.Keys.Count : stopIndex;
            var endSliceIndex = startingSliceIndex + precacheAmount + 1;
            if (endSliceIndex >= modelContours.Keys.Count)
            {
                endSliceIndex = modelContours.Keys.Count - 1;
            }

            //clean previous cache items
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (startingSliceIndex > 0)
            {
                var maxContourHeight = stlModel.SliceIndexes.Keys.ElementAt(startingSliceIndex - 1);
                foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
                {
                    if (sliceHeight < maxContourHeight && modelContours[sliceHeight] != null)
                    {
                        ModelContoursDifference[sliceHeight] = null;
                    }
                }
            }

            //get all slice differences (determine if support cone is valid within layer height
            Parallel.For(startingSliceIndex, endSliceIndex, sliceIndexAsync =>
            //for (var sliceIndexAsync = startingSliceIndex; sliceIndexAsync < endSliceIndex; sliceIndexAsync++)
            {
                if (sliceIndexAsync > 0)
                {
                    var previousSliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndexAsync - 1);
                    var sliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndexAsync);
                    ModelContoursDifference[previousSliceHeightAsync] = DifferenceModelSliceLayer(modelContours[previousSliceHeightAsync], modelContours[sliceHeightAsync]);
                    if (ModelContoursDifference[previousSliceHeightAsync].ChildCount == 0 && ModelContoursDifference[previousSliceHeightAsync]._allPolys.Count == 0)
                    {
                        ModelContoursDifference[previousSliceHeightAsync] = null;
                    }
                }
            });

            Debug.WriteLine("Model contour difference: " + stopwatch.ElapsedMilliseconds + "ms");
        }

        private static void PreCacheModelContours(STLModel3D stlModel, SortedDictionary<float, PolyTree> modelContours, int startingSliceIndex, bool debugMode, int stopIndex = -1)
        {
            var precacheAmount = stopIndex == -1 ? stlModel.SliceIndexes.Keys.Count : stopIndex;
            var endSliceIndex = startingSliceIndex + precacheAmount + 1;
            if (endSliceIndex >= modelContours.Keys.Count)
            {
                endSliceIndex = modelContours.Keys.Count - 1;
            }

            //clean previous cache items
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            if (startingSliceIndex > 0)
            {
                var maxContourHeight = stlModel.SliceIndexes.Keys.ElementAt(startingSliceIndex - 1);
                foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
                {
                    if (sliceHeight < maxContourHeight && modelContours[sliceHeight] != null)
                    {
                        modelContours[sliceHeight] = null;
                        ModelContoursFacingDown[sliceHeight] = null;
                    }
                }
            }

            Debug.WriteLine("Cleaning: " + stopwatch.ElapsedMilliseconds + "ms");


            //get all model slice contours
            var amountOfParallelItems = 250;
            var parallelStepItems = Enumerable.Range(0, endSliceIndex).Where(i => i % amountOfParallelItems == 0);
            Parallel.ForEach(parallelStepItems, parallelStepIndex =>
            //for (var sliceIndexAsync = startingSliceIndex; sliceIndexAsync < endSliceIndex; sliceIndexAsync++)
            {
                var startSliceIndex = parallelStepIndex;
                var endSliceIndex2 = startSliceIndex + amountOfParallelItems;
                if (endSliceIndex < endSliceIndex2)
                {
                    endSliceIndex2 = endSliceIndex;
                }
                for (var sliceIndex = startSliceIndex; sliceIndex < endSliceIndex2; sliceIndex++)
                {
                    if (sliceIndex < stlModel.SliceIndexes.Count)
                    {
                        var sliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt((int)sliceIndex);

                        AddModelContourByLayer(sliceHeightAsync, stlModel, modelContours);
                    }

                }
            });

            Debug.WriteLine("Model contours: " + stopwatch.ElapsedMilliseconds + "ms");

            stopwatch.Reset();
            stopwatch.Start();

            //calc lowest points
            //stlModel.Triangles.UpdateZPoints();

            //foreach (var zSupportPoint in stlModel.Triangles.SupportZPoints)
            //{
            //    //find facing down contour by z height and polygon intersection
            //    var sliceHeight = 0f;
            //    var sliceIndex = 0;
            //    foreach (var sliceIndexHeight in stlModel.SliceIndexes.Keys)
            //    {
            //        if (sliceIndexHeight >= zSupportPoint.Z)// + 0.05f)
            //        {
            //            sliceHeight = sliceIndexHeight;
            //            break;
            //        }
            //        sliceIndex++;
            //    }

            //    if (sliceHeight > 0)
            //    {
            //        var zSupportPointAsIntPoint = new IntPoint(zSupportPoint);
            //        var parentPolygonVolume = double.MaxValue;
            //        var smallestPolygon = new PolyNode();
            //        foreach (var polygon in modelContours[sliceHeight]._allPolys)
            //        {
            //            if (!polygon.IsHole)
            //            {
            //                if (Clipper.PointInPolygon(zSupportPointAsIntPoint, polygon.Contour) != 0)
            //                {
            //                    var polygonVolume = Clipper.Area(polygon.Contour);
            //                    polygonVolume /= CONTOURPOINT_TO_VECTORPOINT_FACTOR;

            //                    if (parentPolygonVolume > polygonVolume)
            //                    {
            //                        parentPolygonVolume = polygonVolume;
            //                        smallestPolygon = polygon;
            //                    }
            //                }
            //            }
            //        }

            //        //if not empty (new PolyNode)
            //        if (!ModelContoursFacingDown.ContainsKey(sliceHeight))
            //        {
            //            ModelContoursFacingDown.Add(sliceHeight, new PolyTree());
            //        }

            //        if (smallestPolygon.Contour.Count == 0)
            //        {
            //            //find by next sliceheight
            //            var nextSliceHeight = stlModel.SliceIndexes.Keys.ElementAt(sliceIndex + 1);
            //            var polyNodes = new List<PolyNode>();
            //            if (PointInModelContour(zSupportPointAsIntPoint, modelContours[nextSliceHeight]._allPolys, polyNodes))
            //            {
            //                foreach (var polygon in polyNodes)
            //                {
            //                    var polygonVolume = Clipper.Area(polygon.Contour);
            //                    polygonVolume /= CONTOURPOINT_TO_VECTORPOINT_FACTOR;

            //                    if (parentPolygonVolume > polygonVolume)
            //                    {
            //                        parentPolygonVolume = polygonVolume;
            //                        smallestPolygon = polygon;
            //                    }
            //                }
            //            }
            //        }

            //        if (smallestPolygon.Contour.Count > 0)
            //        {
            //            var childFound = false;

            //            foreach (var child in ModelContoursFacingDown[sliceHeight].Childs)
            //            {
            //                if (child.Contour == smallestPolygon.Contour)
            //                {
            //                    childFound = true;
            //                    break;
            //                }
            //            }


            //            if (!childFound)
            //            {
            //                ModelContoursFacingDown[sliceHeight].AddChild(smallestPolygon);
            //                ModelContoursFacingDown[sliceHeight]._allPolys.Add(smallestPolygon);
            //            }
            //        }
            //    }
            //}

            Debug.WriteLine("Z-Index Facing down contours: " + stopwatch.ElapsedMilliseconds + "ms");

            //stopwatch.Reset();
            //stopwatch.Start();
            //get all slice that are facing down
            Parallel.ForEach(parallelStepItems, parallelStepIndex =>
            {
                // for (var sliceIndexAsync = startingSliceIndex; sliceIndexAsync < endSliceIndex; sliceIndexAsync++)
                // {

                var sliceStartIndex = parallelStepIndex;
                var endSliceIndex2 = sliceStartIndex + amountOfParallelItems;
                if (endSliceIndex < endSliceIndex2)
                {
                    endSliceIndex2 = endSliceIndex;
                }

                for (var sliceIndex = sliceStartIndex; sliceIndex < endSliceIndex2; sliceIndex++)
                {
                    if (sliceIndex < stlModel.SliceIndexes.Count)
                    {
                        if (sliceIndex > 0)
                        {
                            var previousSliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndex - 1);
                            var sliceHeightAsync = stlModel.SliceIndexes.Keys.ElementAt(sliceIndex);
                            AddContourFacingDown(sliceIndex, sliceHeightAsync, modelContours[sliceHeightAsync], modelContours[previousSliceHeightAsync], ModelContoursFacingDown);
                        }
                    }
                }
                //}
            });

            Debug.WriteLine("Facing down contours: " + stopwatch.ElapsedMilliseconds + "ms");
        }


        private static List<PolygonLowestPoint> CalcContourSupportPoints(PolyTree modelContoursFacingDownOnHeight, SupportProfile supportProfile, int sliceIndex, float sliceHeight, STLModel3D stlModel)
        {
            var result = new List<PolygonLowestPoint>();

            if (modelContoursFacingDownOnHeight != null)
            {
                var maxSupportConeDistance = supportProfile.SupportTopRadius * 3 * CONTOURPOINT_TO_VECTORPOINT_FACTOR;
                //check if there are poly nodes that are facing down
                if (modelContoursFacingDownOnHeight._allPolys.Count > 0)
                {
                    //when exists create support structure based on clipper offset to center and stop when no contour exists
                    var contourFacingDownOffset = new PolyTree();
                    foreach (var modelFacingDownPolyNode in modelContoursFacingDownOnHeight._allPolys)
                    {
                        var contourSupportPoints = new List<IntersectionPointWithFilter>();
                        contourFacingDownOffset = ClipperOffset(modelFacingDownPolyNode.Contour, -supportProfile.SupportOverhangDistance);

                        if (contourFacingDownOffset.Childs.Count == 0)
                        {
                            //find inner contour based on 2x support top 
                            contourFacingDownOffset = ClipperOffset(modelFacingDownPolyNode.Contour, -(2 * supportProfile.SupportTopRadius));

                            if (contourFacingDownOffset.Childs.Count == 0)
                            {
                                contourFacingDownOffset = ClipperOffset(modelFacingDownPolyNode.Contour, -supportProfile.SupportTopRadius);

                                if (contourFacingDownOffset.Childs.Count == 0)
                                {
                                    contourFacingDownOffset.Childs.Add(modelFacingDownPolyNode);
                                }
                            }

                            //process single innerpath
                            var lowestPolyNodeSupportPoints = new List<IntPoint>();
                            foreach (var singlePath in contourFacingDownOffset.Childs)
                            {
                                var pointsOnSinglePath = VectorHelper.GetPointsOnPath(singlePath.Contour, supportProfile.SupportInfillDistance, true);
                                foreach (var pointOnSinglePath in pointsOnSinglePath[0])
                                {
                                    var triangleIntersectionBelow = new Vector3();
                                    var triangleIntersection = Convert2DSupportPointToIntersectionTriangle(stlModel, pointOnSinglePath, sliceHeight, triangleIntersectionBelow);
                                    lowestPolyNodeSupportPoints.Add(new IntPoint(triangleIntersection.IntersectionPoint));
                                }
                            }

                            foreach (var lowestPolyNodeSupportPoint in lowestPolyNodeSupportPoints)
                            {
                                contourSupportPoints.Add(new IntersectionPointWithFilter() { Point = lowestPolyNodeSupportPoint });

                            }

                            //FILTER POINTS: when 2 points are within same contour with distance < 3 * topradius merge them
                            if (contourSupportPoints.Count == 2)
                            {
                                var distanceBetweenPoints = (contourSupportPoints[0].Point.XY - contourSupportPoints[1].Point.XY).Length;
                                if (distanceBetweenPoints < maxSupportConeDistance) //merge the points
                                {
                                    var mergedContourSupportPoint = new IntPoint((contourSupportPoints[0].Point.X + contourSupportPoints[1].Point.X) / 2, (contourSupportPoints[1].Point.Y + contourSupportPoints[1].Point.Y) / 2);
                                    contourSupportPoints.Clear();
                                    contourSupportPoints.Add(new IntersectionPointWithFilter() { Point = mergedContourSupportPoint });
                                }
                            }

                            result.Add(new PolygonLowestPoint() { Polygon = modelFacingDownPolyNode, LowestPoints = contourSupportPoints });
                        }
                        else
                        {
                            var materialSupportConeOverhangDistanceOffset = 3 * supportProfile.SupportTopRadius;
                            while (contourFacingDownOffset.Childs.Count > 0)
                            {
                                contourFacingDownOffset = ClipperOffset(modelFacingDownPolyNode.Contour, -materialSupportConeOverhangDistanceOffset);

                                //process multiple innerpath
                                var lowestPolyNodeSupportPoints = new List<IntPoint>();
                                foreach (var contourFacingDownChild in contourFacingDownOffset.Childs)
                                {
                                    var pointsOnContour = VectorHelper.GetPointsOnPath(contourFacingDownChild.Contour, supportProfile.SupportInfillDistance, true);

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
                                    contourSupportPoints.Add(new IntersectionPointWithFilter() { Point = lowestPolyNodeSupportPoint });
                                }

                                materialSupportConeOverhangDistanceOffset += supportProfile.SupportOverhangDistance;
                            }

                            //when no support point exists in surface use contour as path
                            if (contourSupportPoints.Count == 0)
                            {
                                var pointsOnPath = VectorHelper.GetPointsOnPath(modelFacingDownPolyNode.Contour, supportProfile.SupportInfillDistance, true);
                                if (pointsOnPath.Count == 1)
                                {
                                    contourSupportPoints = new List<IntersectionPointWithFilter>();
                                    foreach (var point in pointsOnPath[0])
                                    {
                                        contourSupportPoints.Add(new IntersectionPointWithFilter() { Point = point });
                                    }

                                }
                                else
                                {

                                }

                            }
                            result.Add(new PolygonLowestPoint() { Polygon = modelFacingDownPolyNode, LowestPoints = contourSupportPoints });
                        }
                    }
                }
            }

            return result;
        }

        private static TriangleIntersection Convert2DSupportPointToIntersectionTriangle(STLModel3D stlModel, IntPoint sliceIntersectionPoint, float previousSliceHeight, Vector3 pointBelowIntersection)
        {
            var result = new TriangleIntersection();

            var sliceIntersectionPointAsVector = sliceIntersectionPoint.AsVector3;
            sliceIntersectionPointAsVector.Z = previousSliceHeight;
            var intersectedTriangles = new List<TriangleIntersection>();
            IntersectionProvider.IntersectTriangle(new Vector3(sliceIntersectionPointAsVector.X, sliceIntersectionPointAsVector.Y, 0), new Vector3(0, 0, 1), stlModel, IntersectionProvider.typeDirection.OneWay, false, out intersectedTriangles);

            if (intersectedTriangles != null)
            {
                var nearestDistance = float.MaxValue;
                var nearestIntersection = new TriangleIntersection();
                foreach (var intersectedTriangle in intersectedTriangles)
                {
                    if (intersectedTriangle.Normal.Z < 0)
                    {
                        var distance = (sliceIntersectionPointAsVector - intersectedTriangle.IntersectionPoint).Length;

                        if (distance < nearestDistance)
                        {
                            nearestDistance = distance;
                            nearestIntersection = intersectedTriangle;
                        }
                    }
                }

                result = nearestIntersection;
            }

            if (result.IntersectionPoint != new Vector3())
            {
                //find intersectionpoint below
                pointBelowIntersection = new Vector3();
                for (var intersectedTriangleIndex = 0; intersectedTriangleIndex < intersectedTriangles.Count; intersectedTriangleIndex++)
                {
                    if (intersectedTriangles[intersectedTriangleIndex].IntersectionPoint.Z == result.IntersectionPoint.Z)
                    {
                        if (intersectedTriangleIndex > 0)
                        {
                            pointBelowIntersection = intersectedTriangles[intersectedTriangleIndex - 1].IntersectionPoint;
                        }

                    }
                }
            }
            else
            {
                result.IntersectionPoint = sliceIntersectionPoint.AsVector3 + new Vector3(0, 0, previousSliceHeight + 0.05f);
            }

            return result;
        }


        private static PolyTree ClipperOffset(PolyTree polyNode, float overhangSupportConeDistance)
        {
            var offsetPolyTree = new PolyTree();
            if (polyNode != null)
            {
                var clipperOffset = new ClipperOffset();
                foreach (var child in polyNode.Childs)
                {
                    if (!child.IsHole)
                    {
                        clipperOffset.AddPath(child.Contour, JoinType.jtMiter, EndType.etClosedPolygon);
                    }
                }

                clipperOffset.Execute(ref offsetPolyTree, overhangSupportConeDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR);
            }

            return offsetPolyTree;
        }

        private static PolyTree ClipperOffset(List<IntPoint> supportLayerContour, float overhangSupportConeDistance)
        {
            var clipperOffset = new ClipperOffset();
            var offsetPolyTree = new PolyTree();
            clipperOffset.AddPath(supportLayerContour, JoinType.jtMiter, EndType.etClosedPolygon);
            clipperOffset.Execute(ref offsetPolyTree, overhangSupportConeDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR);

            return offsetPolyTree;
        }

        private static PolyTree MergeSupportCircles(List<List<IntPoint>> supportPointAsCirclesIntPoint)
        {
            var supportLayerWithSupportCircles = new PolyTree();
            var clipper = new Clipper();
            clipper.AddPath(new List<IntPoint>(), PolyType.ptSubject, true);
            clipper.AddPaths(supportPointAsCirclesIntPoint, PolyType.ptClip, true);

            clipper.Execute(ClipType.ctUnion, supportLayerWithSupportCircles, PolyFillType.pftNonZero);
            return supportLayerWithSupportCircles;
        }

        private static List<List<IntPoint>> CalcModelIntersectionLines(PolyTree currentLayer, PolyTree supportLayer, float nextLayerHeight)
        {
            var totalSupportContourLines = new List<List<IntPoint>>();
            var totalSupportContourSublines = new List<List<IntPoint>>();

            var intersectPoly = IntersectModelSliceLayer(currentLayer, supportLayer);

            var polyNodePoints = new List<IntPoint>();
            foreach (var polyNode in intersectPoly._allPolys)
            {
                polyNodePoints = new List<IntPoint>();
                foreach (var polyNodePoint in polyNode.Contour)
                {
                    var polyNodePointFound = false;
                    foreach (var supportChild in currentLayer._allPolys)
                    {
                        if (supportChild.Contour.Contains(polyNodePoint))
                        {
                            polyNodePointFound = true;
                            polyNodePoints.Add(new IntPoint());
                            break;
                        }
                    }

                    if (!polyNodePointFound)
                    {
                        polyNodePoints.Add(polyNodePoint);
                    }
                }

                //check if list contains new intpoint()
                var intPointExists = false;
                foreach (var polyNodePoint in polyNodePoints)
                {
                    if (polyNodePoint == new IntPoint())
                    {
                        intPointExists = true;
                        break;
                    }
                }

                if (intPointExists)
                {
                    if (polyNodePoints.Count > 0 && polyNodePoints[0] != new IntPoint())
                    {
                        //find first new intpoint
                        var endPointIndex = -1;
                        for (var i = 0; i < polyNodePoints.Count; i++)
                        {
                            if (polyNodePoints[i] == new IntPoint())
                            {
                                break;
                            }
                            else
                            {
                                endPointIndex = i;
                                polyNodePoints.Add(polyNodePoints[i]);
                            }
                        }

                        if (endPointIndex >= 0)
                        {

                            polyNodePoints.RemoveRange(0, endPointIndex + 1);
                        }
                    }
                }

                totalSupportContourSublines.Add(polyNodePoints);



            }

            if (polyNodePoints.Count > 0)
            {
                totalSupportContourSublines.Add(polyNodePoints);
                polyNodePoints = new List<IntPoint>();
            }

            foreach (var kChild in totalSupportContourSublines)
            {
                var lines = new List<IntPoint>();
                var lineStartPointFound = false;
                for (var childContourPointIndex = 0; childContourPointIndex < kChild.Count; childContourPointIndex++)
                {

                    if (kChild[childContourPointIndex] != new IntPoint() && lineStartPointFound == false)
                    {
                        lineStartPointFound = true;
                        lines.Add(kChild[childContourPointIndex]);

                    }
                    else if (childContourPointIndex == kChild.Count - 1 && lineStartPointFound && (kChild[childContourPointIndex] != new IntPoint()))
                    {
                        lines.Add(kChild[childContourPointIndex]);

                        totalSupportContourLines.Add(lines);
                        lines = new List<IntPoint>();

                    }
                    else if ((kChild[childContourPointIndex] == new IntPoint() && lineStartPointFound))
                    {
                        totalSupportContourLines.Add(lines);
                        lines = new List<IntPoint>();

                        lineStartPointFound = false;
                    }

                    else if (lineStartPointFound)
                    {
                        lines.Add(kChild[childContourPointIndex]);
                    }
                }

                if (lines.Count > 0)
                {

                }
            }

            return totalSupportContourLines;
        }


        private static List<List<IntPoint>> ConvertSupportPointsToCircles(List<IntersectionPointWithFilter> supportLayerSupportPoints, float materialSupportInfillDistance)
        {
            var supportPointCircles = new List<List<IntPoint>>();
            var circleDiameter = materialSupportInfillDistance * CONTOURPOINT_TO_VECTORPOINT_FACTOR;
            foreach (var supportPoint in supportLayerSupportPoints)
            {
                var supportPointAsCircleIntPoint = new List<IntPoint>();
                var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, circleDiameter, 16, new Vector3(supportPoint.Point.X, supportPoint.Point.Y, 0));
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

        private static List<List<IntPoint>> ConvertSupportContourLinesToSupportCircles(List<List<IntPoint>> totalSupportContourLines, STLModel3D overhangModel, float materialSupportConeInfillDistance, Dictionary<float, List<PolyNode>> facingDownSibblingNodes, List<IntPoint> currentLayerSupportPoints, Dictionary<IntPoint, Vector3> checkedLayerSupportPoints, float sliceHeight, int sliceIndex, float previousSliceHeight, float materialSupportInfillDistance, float materialSupportConeOverhangDistance)
        {
            var supportPointAsCirclesIntPoint = new List<List<IntPoint>>();
            foreach (var totalSupportContourLine in totalSupportContourLines)
            {
                if (totalSupportContourLine.Count == 1)
                {
                    checkedLayerSupportPoints.Add(totalSupportContourLine[0], new Vector3());

                    var supportPointAsCircleIntPoint = new List<IntPoint>();
                    var supportPoint = totalSupportContourLine[0].AsVector3.Xy;
                    var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, materialSupportInfillDistance, 16, new Vector3(supportPoint.X, supportPoint.Y, 0));

                    foreach (var supportPointOnCircle in supportPointAsCircle)
                    {
                        supportPointAsCircleIntPoint.Add(new IntPoint(supportPointOnCircle));
                    }

                    supportPointAsCirclesIntPoint.Add(supportPointAsCircleIntPoint);
                }
                else
                {
                    var uncheckSupportPoints = VectorHelper.GetPointsOnPath(totalSupportContourLine, materialSupportInfillDistance, false);

                    //FILTER on NEXT MODEL CONTOUR INTERSECTION
                    var nextSliceHeight = overhangModel.SliceIndexes.Keys.ElementAt(sliceIndex + 1);
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
                    var modelIntersectionCheckedSupportPoints = new Dictionary<Vector3, Vector3>();

                    foreach (var supportContourPoint in supportConeDistanceCheckedSupportPoints)
                    {
                        var modelIntersection = new TriangleIntersection();
                        modelIntersection.IntersectionPoint = supportContourPoint.AsVector3;
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
                        var supportPointAsCircle = VectorHelper.GetCircleOutlinePoints(0, materialSupportInfillDistance, 16, new Vector3(checkedSupportPoint.X, checkedSupportPoint.Y, 0));

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
