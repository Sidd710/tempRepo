using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Engines.MagsAI;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Slices
{
    public class MinMax_XY
    {
        public int xmin, xmax, ymin, ymax;
    }

    internal class Slice : IDisposable
    {


        private List<SlicePolyLine3D> _modelIntersectionsZ; // list of polyline segments
        private List<List<SlicePolyLine3D>> _supportIntersectionsZ; // list of polyline segments

        internal List<SlicePoint2D> ModelPoints { get; set; }

        internal List<PolyTree>[] ModelPolyTrees { get; set; }
        internal PolyTree ModelAnglesPolyTree { get; set; }
        internal PolyTree ModelWallPolyTree { get; set; }

        internal List<PolyTree>[] ModelBleedingPolyTrees { get; set; }
        internal List<PolyTree>[] SupportPolyTrees { get; set; }

        internal List<PolyNode> LowestPointsPolyNodes { get; set; }
        internal List<PolyNode> OverhangPointsPolyNodes { get; set; }
        internal List<PolyNode> WallSupportPolyNodes { get; set; }

        private List<SlicePolyLine3D> _modelBleedingZIntersections;

        internal float SliceHeight { get; set; }
        internal int SliceIndex { get; set; }

        internal Slice(int sliceIndex, List<SlicePolyLine3D> intersections, List<List<SlicePolyLine3D>> supportIntersections, List<SlicePolyLine3D> modelBleedingIntersections, float sliceHeight)
        {
            this.SliceIndex = sliceIndex;
            this._modelBleedingZIntersections = modelBleedingIntersections;
            this.SliceHeight = sliceHeight;

            this._modelIntersectionsZ = intersections;
            this._supportIntersectionsZ = supportIntersections;

            this.ModelPoints = new List<SlicePoint2D>();

            this.ModelPolyTrees = new List<PolyTree>[1];
            this.ModelBleedingPolyTrees = new List<PolyTree>[1];
            this.SupportPolyTrees = new List<PolyTree>[1];
            this.LowestPointsPolyNodes = new List<PolyNode>();
            this.OverhangPointsPolyNodes = new List<PolyNode>();
            this.WallSupportPolyNodes = new List<PolyNode>();
        }


        /*
         This function calculates the min and max x/y coordinates of this slice
         */
        internal static MinMax_XY CalcMinMax_XY(List<SliceLine2D> lines2d)
        {
            var l1 = (SliceLine2D)lines2d[0];
            MinMax_XY mm = new MinMax_XY();
            //start the min / max off with some valid values
            mm.xmin = mm.xmax = l1.p1.XasPx;
            mm.ymin = mm.ymax = l1.p1.YasPx;

            foreach (var ln in lines2d)
            {
                var p1X = ln.p1.XasPx;
                var p1Y = ln.p1.YasPx;
                var p2X = ln.p2.XasPx;
                var p2Y = ln.p2.YasPx;

                mm.xmin = Math.Min(mm.xmin, p1X);
                mm.xmin = Math.Min(mm.xmin, p2X);

                mm.xmax = Math.Max(mm.xmax, p1X);
                mm.xmax = Math.Max(mm.xmax, p2X);

                mm.ymin = Math.Min(mm.ymin, p1Y);
                mm.ymin = Math.Min(mm.ymin, p2Y);

                mm.ymax = Math.Max(mm.ymax, p1Y);
                mm.ymax = Math.Max(mm.ymax, p2Y);
            }
            return mm;
        }

        internal enum TypeOfNextLineDetection
        {
            FindLastRightNormal = 0,
            FoundRightNormal = 1,
            FindFirstLeftNormal = 2,
        }





        internal List<SlicePoint2D> GetModelPoints(List<SliceLine2D> polylines, int pixelY, bool bleeding)
        {

            var intersecting = GetIntersecting2dYLines(pixelY, polylines);
            //   var points = GetIntersectingPoints(pixelY, intersecting);
            var points = new List<SlicePoint2D>();
            points.Sort();

            //check normal OPTION NORMAL_AUTOMERGE
            try
            {
                if (points.Count > 1)
                {
                    while (points[0].Normal.X > 0)
                    {
                        points.RemoveAt(0);
                    }

                    //check if list has same pixel or 1 left and normal in opposite direction
                    for (var pointIndex = 0; pointIndex < points.Count - 1; pointIndex++)
                    {
                        if (points[pointIndex + 1].XasPx - points[pointIndex].XasPx <= 1
                            && points[pointIndex].Normal.X > 0 && points[pointIndex + 1].Normal.X < 0)
                        {
                            points.RemoveAt(pointIndex + 1);
                            points.RemoveAt(pointIndex);
                        }
                    }


                    //process normals

                    var normalSearchState = TypeOfNextLineDetection.FindLastRightNormal;
                    var normalProcessPoints = new List<SlicePoint2D>();
                    normalProcessPoints.Add(points[0]);
                    for (var pointIndex = 1; pointIndex < points.Count; pointIndex++)
                    {
                        if (normalSearchState == TypeOfNextLineDetection.FindLastRightNormal && points[pointIndex].Normal.X > 0)
                        {
                            normalSearchState = TypeOfNextLineDetection.FoundRightNormal;

                            if (pointIndex == points.Count - 1)
                            {
                                normalProcessPoints.Add(points[pointIndex]);
                            }
                        }
                        else if (normalSearchState == TypeOfNextLineDetection.FoundRightNormal && points[pointIndex].Normal.X < 0)
                        {
                            normalSearchState = TypeOfNextLineDetection.FindLastRightNormal;
                            normalProcessPoints.Add(points[pointIndex - 1]);
                            normalProcessPoints.Add(points[pointIndex]);
                        }
                    }
                    if (normalProcessPoints.Count % 2 == 1)
                    {

                    }

                    if (normalProcessPoints[normalProcessPoints.Count - 1].Normal.X < 0)
                    {
                        normalProcessPoints.RemoveAt(normalProcessPoints.Count - 1);
                    }

                    points = normalProcessPoints;

                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }



            return points;
        }

        internal List<SlicePoint2D> GetModelPointsWithParentLine(List<SliceLine2D> polylines, int pixelY, bool bleeding)
        {

            var intersecting = GetIntersecting2dYLines(pixelY, polylines);
            var points = TriangleHelper.GetIntersectingPoints(pixelY, intersecting);
            points.Sort();

            return points;

        }

        internal List<SlicePoint2D> GetSupportPointsByNormal(List<SlicePoint2D> polyPoints, int pixelY)
        {
            polyPoints.Sort();
            {
                try
                {
                    if (polyPoints.Count > 1)
                    {
                        while (polyPoints[0].Normal.X > 0)
                        {
                            polyPoints.RemoveAt(0);
                        }

                        //check if list has same pixel or 1 left and normal in opposite direction
                        for (var pointIndex = 0; pointIndex < polyPoints.Count - 1; pointIndex++)
                        {
                            if (polyPoints[pointIndex + 1].XasPx - polyPoints[pointIndex].XasPx <= 1
                                && polyPoints[pointIndex].Normal.X > 0 && polyPoints[pointIndex + 1].Normal.X < 0)
                            {
                                polyPoints.RemoveAt(pointIndex + 1);
                                polyPoints.RemoveAt(pointIndex);
                            }
                        }


                        //process normals

                        var normalSearchState = TypeOfNextLineDetection.FindLastRightNormal;
                        var normalProcessPoints = new List<SlicePoint2D>();
                        normalProcessPoints.Add(polyPoints[0]);
                        for (var pointIndex = 1; pointIndex < polyPoints.Count; pointIndex++)
                        {
                            if (normalSearchState == TypeOfNextLineDetection.FindLastRightNormal && polyPoints[pointIndex].Normal.X > 0)
                            {
                                normalSearchState = TypeOfNextLineDetection.FoundRightNormal;

                                if (pointIndex == polyPoints.Count - 1)
                                {
                                    normalProcessPoints.Add(polyPoints[pointIndex]);
                                }
                            }
                            else if (normalSearchState == TypeOfNextLineDetection.FoundRightNormal && polyPoints[pointIndex].Normal.X < 0)
                            {
                                normalSearchState = TypeOfNextLineDetection.FindLastRightNormal;
                                normalProcessPoints.Add(polyPoints[pointIndex - 1]);
                                normalProcessPoints.Add(polyPoints[pointIndex]);
                            }
                        }
                        if (normalProcessPoints.Count % 2 == 1)
                        {

                        }

                        if (normalProcessPoints[normalProcessPoints.Count - 1].Normal.X < 0)
                        {
                            normalProcessPoints.RemoveAt(normalProcessPoints.Count - 1);
                        }

                        polyPoints = normalProcessPoints;

                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }

                return polyPoints;
            }
        }

        internal List<MagsAIPolyLines> ConvertModelIntersectionsToPolyLines(float sliceHeight, SupportProfile selectedMaterialProfile, AtumPrinter selectedPrinter, bool usePixelsAsValues = true)
        {
            return MagsAIPolyLines.FromListOfModelIntersectionsUsingPolyLines(sliceHeight, this._modelIntersectionsZ, selectedMaterialProfile, selectedPrinter, usePixelsAsValues);
        }

        internal void ConvertModelIntersectionsToPolyTrees(float sliceHeight, AtumPrinter selectedPrinter, Material selectedMaterial, SupportProfile selectedMaterialProfile,
            List<float[]> angleSideAngles, List<float[]> wallSideAngles,
            bool usePixelsAsValues = true, bool includeAngledSide = false, bool includeWallSides = false)
        {
            var angledSides = new List<List<PolylineWithNormal>>();
            var wallSides = new List<List<PolylineWithNormal>>();

            this.ModelPolyTrees[0] = PolyTree.FromListOfModelIntersectionsUsingPolygons(sliceHeight, this._modelIntersectionsZ, selectedPrinter,
                angleSideAngles, wallSideAngles,
                out angledSides, out wallSides,
                usePixelsAsValues, processUnion: true,
                includeAngledSides: includeAngledSide, includeWallSides: includeWallSides);


            if (includeAngledSide && angledSides.Count > 0)
            {
                this.ModelAnglesPolyTree = new PolyTree();
                var clipperOffset = new ClipperOffset();
                var clipperResult = new PolyTree();
                foreach (var angledSide in angledSides)
                {
                    var polyList = new List<IntPoint>();
                    foreach (var angledPolygon in angledSide)
                    {
                        polyList.Add(angledPolygon.Point);
                    }


                    ReducePointsInPath(polyList, MagsAIEngine.PixelOffsetOptimisation);

                    clipperOffset.AddPath(polyList, JoinType.jtMiter, EndType.etOpenButt);

                }

                clipperOffset.Execute(ref clipperResult, (selectedMaterialProfile.SupportOverhangDistance) * (CONTOURPOINT_TO_VECTORPOINT_FACTOR));
                clipperResult = IntersectModelSliceLayer(this.ModelPolyTrees[0][0], clipperResult);
                this.ModelAnglesPolyTree = clipperResult;
                ReducePointsInPolyTree(this.ModelAnglesPolyTree, MagsAIEngine.PixelOffsetOptimisation);
            }

            //polytree using polygon offset
            if (includeWallSides && wallSides.Count > 0)
            {
                var clipperOffset = new ClipperOffset();
                foreach (var wallSide in wallSides)
                {
                    var polyList = new List<IntPoint>();
                    foreach (var wallPolygon in wallSide)
                    {
                        polyList.Add(wallPolygon.Point);
                    }

                    ReducePointsInPath(polyList, MagsAIEngine.PixelOffsetOptimisation);
                    clipperOffset.AddPath(polyList, JoinType.jtMiter, EndType.etOpenButt);
                }

                var clipperResult = new PolyTree();
                clipperOffset.Execute(ref clipperResult, (selectedMaterialProfile.SupportOverhangDistance / 2) * (CONTOURPOINT_TO_VECTORPOINT_FACTOR));
                clipperResult = IntersectModelSliceLayer(this.ModelWallPolyTree, clipperResult);

                this.ModelWallPolyTree = clipperResult;
                ReducePointsInPolyTree(this.ModelWallPolyTree, MagsAIEngine.PixelOffsetOptimisation);

                //TriangleHelper.SavePolyNodesContourToPng(this.ModelWallPolyTree._allPolys, "wall_" + sliceHeight.ToString("00.00"));
            }
        }


        internal void ConvertModelBleedingIntersectionsToPolyTrees(float sliceHeight, AtumPrinter selectedPrinter, bool usePixelsAsValues = true)
        {
            var angledSides = new List<List<PolylineWithNormal>>();
            var wallSides = new List<List<PolylineWithNormal>>();
            var facingDownSides = new List<PolyNode>();
            this.ModelBleedingPolyTrees[0] = PolyTree.FromListOfModelIntersectionsUsingPolygons(sliceHeight, this._modelBleedingZIntersections, selectedPrinter,
                null, null,
                out angledSides, out wallSides, usePixelsAsValues);
        }

        internal void ConvertSupportIntersectionsToPolyTrees(float sliceHeight, AtumPrinter selectedPrinter)
        {
            this.SupportPolyTrees[0] = new List<PolyTree>() { PolyTree.FromListOfSupportIntersectionsUsingPolygons(sliceHeight, this._supportIntersectionsZ, selectedPrinter) };
        }

        //internal void RenderSliceAsByteArray(float sliceHeight, AtumPrinter selectedPrinter)
        //{
        //    try
        //    {
        //        var points = new List<SlicePoint2D>();
        //        List<SliceLine2D> lines2d = new List<SliceLine2D>();

        //        var pointsInPixels = new byte[RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX];

        //        var totalPoints = new List<SlicePoint2D>();
        //        List<SliceLine2D> modelBleedingZLines = null;

        //        if (this._modelIntersectionsZ.Count > 0)
        //        {
        //            //model points
        //            if (RenderEngine.PrintJob.Material.BleedingXYOffset_Inside == 0 && RenderEngine.PrintJob.Material.BleedingXYOffset_Outside == 0)
        //            {
        //                //get using line algorithme

        //                var sliceLines = PolyTree.FromListOfIntersections(sliceHeight, this._modelIntersectionsZ, selectedPrinter);
        //                if (sliceLines.Count > 0)
        //                {
        //                    var modelSliceLines = sliceLines;
        //                    foreach (var modelSliceLine in modelSliceLines)
        //                    {
        //                        lines2d.Add(new SliceLine2D()
        //                        {
        //                            Normal = modelSliceLine.Normal,
        //                            p1 = new SlicePoint2D()
        //                            {
        //                                X = modelSliceLine.StartPoint.X,
        //                                Y = modelSliceLine.StartPoint.Y
        //                            },
        //                            p2 = new SlicePoint2D()
        //                            {
        //                                X = modelSliceLine.EndPoint.X,
        //                                Y = modelSliceLine.EndPoint.Y
        //                            }
        //                        }
        //                            );
        //                    }
        //                }
        //            }
        //            //else
        //            //{
        //            //    //use polygon algorithme
        //            //    var modelPolygons = PolyTree.FromListOfIntersectionsUsingPolygons(sliceHeight, this._modelIntersectionsZ);
        //            //   lines2d = VectorHelper.ConvertPolyTreeWithOffsetToSliceLine2D(sliceHeight, modelPolygons, (float)RenderEngine.PrintJob.Material.BleedingXYOffset_Inside, (float)RenderEngine.PrintJob.Material.BleedingXYOffset_Outside);
        //            //}

        //            //bleeding XY points
        //            if (RenderEngine.PrintJob.Material.BleedingOffset > 0 && lines2d != null && lines2d.Count > 0)
        //            {

        //                if (RenderEngine.PrintJob.Material.BleedingXYOffset_Inside == 0 && RenderEngine.PrintJob.Material.BleedingXYOffset_Outside == 0)
        //                {
        //                    //get using line algorithme
        //                    var bleedingZLines = PolyTree.FromListOfIntersections(sliceHeight, this._modelBleedingZIntersections, selectedPrinter);
        //                    foreach (var bleedingZLine in bleedingZLines)
        //                    {
        //                        modelBleedingZLines.Add(new SliceLine2D()
        //                        {
        //                            Normal = bleedingZLine.Normal,
        //                            p1 = new SlicePoint2D()
        //                            {
        //                                X = bleedingZLine.StartPoint.X,
        //                                Y = bleedingZLine.StartPoint.Y
        //                            },
        //                            p2 = new SlicePoint2D()
        //                            {
        //                                X = bleedingZLine.EndPoint.X,
        //                                Y = bleedingZLine.EndPoint.Y
        //                            }
        //                        });

        //                    }
        //                }
        //                else
        //                {
        //                    var modelBleedingZPolygons = PolyTree.FromListOfModelIntersectionsUsingPolygons(sliceHeight + (float)RenderEngine.PrintJob.Material.BleedingOffset, this._modelBleedingZIntersections, RenderEngine.PrintJob.SelectedPrinter);
        //                    modelBleedingZLines = VectorHelper.ConvertPolyTreeWithOffsetToSliceLine2D(sliceHeight + (float)RenderEngine.PrintJob.Material.BleedingOffset, modelBleedingZPolygons, (float)RenderEngine.PrintJob.Material.BleedingXYOffset_Inside, (float)RenderEngine.PrintJob.Material.BleedingXYOffset_Outside);
        //                }
        //            }

        //            if (lines2d.Count > 0)
        //            {
        //                MinMax_XY mm = CalcMinMax_XY(lines2d);
        //                int pointBegin = 0;
        //                int pointEnd = 0;

        //                for (int y = mm.ymin; y < mm.ymax; y++) // this needs to be in scaled value 
        //                {
        //                    points.Clear();

        //                    points.AddRange(GetModelPoints(lines2d, y, false));

        //                    totalPoints.Clear();
        //                    Array.Clear(pointsInPixels, 0, pointsInPixels.Length);

        //                    if (points.Count > 0 && points.Count % 2 == 0)
        //                    {
        //                        for (var pointIndex = 0; pointIndex < points.Count; pointIndex += 2)
        //                        {
        //                            pointBegin = points[pointIndex].XasPx;
        //                            if (pointBegin < 0) pointBegin = 0;
        //                            try
        //                            {
        //                                pointEnd = points[pointIndex + 1].XasPx;
        //                            }
        //                            catch (Exception exc)
        //                            {
        //                                Debug.WriteLine(exc.Message);
        //                            }


        //                            if (pointEnd >= pointsInPixels.Length) pointEnd = pointsInPixels.Length - 1;

        //                            for (var pointInsert = pointBegin; pointInsert <= pointEnd; pointInsert++)
        //                            {
        //                                pointsInPixels[pointInsert] = 1;
        //                            }
        //                        }

        //                        //    var bleedingPoints = new List<SlicePoint2D>();
        //                        if (this.SliceHeight > 0.25f && this._modelBleedingZIntersections != null && this._modelBleedingZIntersections.Count > 0)
        //                        {
        //                            var bleedingPoints = GetModelPoints(modelBleedingZLines, y, true);
        //                            if (bleedingPoints.Count < 2)
        //                            {
        //                                for (var pointInPixelIndex = 0; pointInPixelIndex < pointsInPixels.Length; pointInPixelIndex++)
        //                                {
        //                                    pointsInPixels[pointInPixelIndex] = 0;
        //                                }
        //                            }
        //                            else
        //                            {
        //                                int bleedingBegin = 0;
        //                                int bleedingEnd = 0;
        //                                for (var bleedingPointIndex = 1; bleedingPointIndex < bleedingPoints.Count; bleedingPointIndex += 2)
        //                                {
        //                                    if (bleedingPointIndex == 1)
        //                                    {
        //                                        //reset all leading pixels
        //                                        bleedingBegin = bleedingPoints[0].XasPx;
        //                                        if (bleedingBegin < 0) bleedingBegin = 0;
        //                                        if (bleedingBegin >= pointsInPixels.Length) bleedingBegin = pointsInPixels.Length - 1;
        //                                        for (var bleedingPointInsert = 0; bleedingPointInsert <= bleedingBegin - 1; bleedingPointInsert++)
        //                                        {
        //                                            pointsInPixels[bleedingPointInsert] = 0;
        //                                        }
        //                                    }

        //                                    if (bleedingPointIndex + 1 < bleedingPoints.Count)
        //                                    {
        //                                        bleedingBegin = bleedingPoints[bleedingPointIndex].XasPx;
        //                                        if (bleedingBegin < 0) bleedingBegin = 0;
        //                                        bleedingEnd = bleedingPoints[bleedingPointIndex + 1].XasPx;
        //                                        if (bleedingEnd >= pointsInPixels.Length) bleedingEnd = pointsInPixels.Length - 1;

        //                                        for (var bleedingPointInsert = bleedingBegin; bleedingPointInsert < bleedingEnd; bleedingPointInsert++)
        //                                        {
        //                                            pointsInPixels[bleedingPointInsert] = 0;
        //                                        }
        //                                    }

        //                                }
        //                            }


        //                            // reset all points right from last bleedingpoint
        //                            if (bleedingPoints.Count > 0)
        //                            {
        //                                var bleedingPointIndex = bleedingPoints[bleedingPoints.Count - 1].XasPx + 1;
        //                                if (bleedingPointIndex < 0) bleedingPointIndex = 0;
        //                                for (var bleedingPointInsert = bleedingPointIndex; bleedingPointInsert < pointsInPixels.Length; bleedingPointInsert++)
        //                                {
        //                                    pointsInPixels[bleedingPointInsert] = 0;
        //                                }
        //                            }
        //                        }
        //                        else if (this.SliceHeight > 0.25f && modelBleedingZLines != null && modelBleedingZLines.Count == 0)
        //                        {
        //                            Array.Clear(pointsInPixels, 0, pointsInPixels.Length);
        //                        }

        //                        for (var pointInPixel = 0; pointInPixel < pointsInPixels.Length; pointInPixel++)
        //                        {
        //                            if (pointsInPixels[pointInPixel] == 1)
        //                            {
        //                                for (var pointEndInPixel = pointInPixel; pointEndInPixel < pointsInPixels.Length; pointEndInPixel++)
        //                                {
        //                                    if (pointsInPixels[pointEndInPixel] == 0 || pointEndInPixel == pointsInPixels.Length - 1)
        //                                    {
        //                                        totalPoints.Add(new SlicePoint2D() { X = pointInPixel, Y = y });

        //                                        if (pointEndInPixel == pointsInPixels.Length - 1)
        //                                        {
        //                                            totalPoints.Add(new SlicePoint2D() { X = pointEndInPixel, Y = y });
        //                                        }
        //                                        else
        //                                        {
        //                                            totalPoints.Add(new SlicePoint2D() { X = pointEndInPixel - 1, Y = y });
        //                                        }

        //                                        pointInPixel = pointEndInPixel;

        //                                        break;
        //                                    }
        //                                }

        //                            }
        //                        }
        //                        points.Clear();
        //                        points.AddRange(totalPoints);

        //                        this.ModelPoints.AddRange(totalPoints);
        //                    }
        //                    else if (points.Count % 2 == 1)
        //                    {
        //                        Debug.WriteLine("Invalid model points count in slice: " + sliceHeight + ":" + y);
        //                    }
        //                }
        //            }
        //        }
        //    }


        //    catch (Exception exc)
        //    {
        //        Debug.WriteLine(exc.Message);
        //    }
        //}





        /*
         This function will return a list of lines that intersect with the specified Y scanline
         */
        internal static List<SliceLine2D> GetIntersecting2dYLines(int ypos, List<SliceLine2D> all2dlines)
        {
            var intersecting = new Dictionary<SliceLine2D, SliceLine2D>();
            foreach (var ln in all2dlines)
            {
                if ((ln.p1.Y >= ypos && ln.p2.Y <= ypos) ||
                    (ln.p2.Y >= ypos && ln.p1.Y <= ypos) ||
                    (ln.p2.Y == ypos && ln.p1.Y == ypos))
                {
                    if (!intersecting.ContainsKey(ln))
                    {
                        intersecting.Add(ln, ln);
                    }
                }
            }

            var intersectingList = new List<SliceLine2D>();
            intersectingList.AddRange(intersecting.Values);
            return intersectingList;
        }

        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }
    }
}
