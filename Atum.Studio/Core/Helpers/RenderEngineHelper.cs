using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Helpers
{
    class RenderEngineHelper
    {
    //    internal static int  ConvertPolyTreesToPixels(byte[] pixelValues, List<PolyNode> polyNodes, int projectorResolutionX, int projectorResolutionY, bool isModel, byte colorIndex = 255)
    //    {
    //        var polyNodeToTrees = new List<PolyTree>[1];
    //        polyNodeToTrees[0] = new List<PolyTree>() { new PolyTree { _childs = polyNodes } };
    //        return ConvertPolyTreesToPixels(pixelValues, polyNodeToTrees, projectorResolutionX, projectorResolutionY, isModel, colorIndex);
    //    }

            internal static int ConvertPolyTreesToPixels(byte[] pixelValues, List<PolyTree>[] polyTrees, int projectorResolutionX, int projectorResolutionY, bool isModel, byte colorIndex = 255)
        {
            var amountOfWhitePixels = 0;
            int beginPixel = 0;
            int beginPixelOfRow = 0;
            int endPixel = 0;
            int endPixelOfRow = 0;

            //points by object. Do not combine them to one list
            var currentObjectIndex = 0;
            List<Dictionary<int, List<SlicePoint2D>>> objectIntersectionsPoints = new List<Dictionary<int, List<SlicePoint2D>>>(); //[objectindex][y][points]
            for (var objectIndex = 0; objectIndex < polyTrees.Length; objectIndex++)
            {
                if (isModel)
                {
                    objectIntersectionsPoints.Add(GetModelIntersectionPointsFromPolyTrees(polyTrees[objectIndex], projectorResolutionX, projectorResolutionY));
                }
                else
                {
                    foreach (var supportPolyTree in polyTrees[objectIndex])
                    {
                        foreach (var supportPolyNode in supportPolyTree._allPolys)
                        {
                            objectIntersectionsPoints.Add(GetSupportIntersectionPointsFromPolyNode(supportPolyNode, projectorResolutionX, projectorResolutionY));
                        }
                    }
                }

                currentObjectIndex++;
            }

            foreach(var objectIntersectionPoint in objectIntersectionsPoints) {
                foreach(var objectPixelYIndex in objectIntersectionPoint.Keys)
                {
                    if (objectIntersectionPoint[objectPixelYIndex] != null)
                    {
                        objectIntersectionPoint[objectPixelYIndex].Sort();

                        //convert points to pixels
                        for (var objectPointXIndex = 0; objectPointXIndex < objectIntersectionPoint[objectPixelYIndex].Count - 1; objectPointXIndex += 2)
                        {
                            var p1 = objectIntersectionPoint[objectPixelYIndex][objectPointXIndex];
                            var p2 = objectIntersectionPoint[objectPixelYIndex][objectPointXIndex + 1];
                            if (p1.X <= p2.X)
                            {
                                if (p2.Y >= 0 && p1.Y >= 0)
                                {
                                    //line (parts out of scope
                                    if ((int)Math.Floor(p2.X) >= projectorResolutionX && (int)Math.Floor(p1.X) >= projectorResolutionX)
                                    {
                                        continue;
                                    }
                                    else if ((int)Math.Floor(p2.X) < 0 && (int)Math.Floor(p1.X) < 0)
                                    {
                                        continue;
                                    }

                                    if ((int)Math.Floor(p2.X) < 0)
                                    {
                                        p2.X = 0;
                                    }
                                    if ((int)Math.Floor(p1.X) < 0)
                                    {
                                        p1.X = 0;
                                    }
                                    if ((int)Math.Floor(p2.X) >= projectorResolutionX - 1)
                                    {
                                        p2.X = projectorResolutionX - 1;
                                    }
                                    if ((int)Math.Floor(p1.X) >= projectorResolutionX)
                                    {
                                        p1.X = projectorResolutionX - 1;
                                    }

                                    beginPixel = (((int)p1.Y * projectorResolutionX + (int)Math.Ceiling(p1.X)) * 3);
                                    beginPixelOfRow = (((int)p1.Y * projectorResolutionX) * 3);
                                    
                                    endPixel = (int)(((p2.Y * projectorResolutionX + (int)Math.Floor(p2.X)) * 3) + 2);
                                    endPixelOfRow = (((((int)p1.Y + 1) * projectorResolutionX) * 3) - 1);

                                    if (endPixel > endPixelOfRow)
                                    {
                                        endPixel = endPixelOfRow;
                                    }

                                    //if (pixelValues.Length - 1 >= beginPixel && beginPixel >= beginPixelOfRow)
                                    //{
                                    //    pixelValues[beginPixel] = colorIndex;
                                    //    pixelValues[beginPixel + 1] = colorIndex;
                                    //    pixelValues[beginPixel + 2] = colorIndex;
                                    //    amountOfWhitePixels++;
                                    //}

                                    //fill color between begin and endpoints
                                    if (pixelValues.Length - 1 >= endPixel && endPixel >= 0)
                                    {
                                        amountOfWhitePixels += (endPixel - beginPixel) / 3;

                                        pixelValues[endPixel] = colorIndex;
                                        pixelValues[endPixel - 1] = colorIndex;
                                        pixelValues[endPixel - 2] = colorIndex;

                                        if (beginPixel < endPixel)
                                        {
                                            for (var betweenPixel = beginPixel; betweenPixel < endPixel; betweenPixel++)
                                            {
                                                if (betweenPixel >= 0)
                                                {
                                                    pixelValues[betweenPixel] = colorIndex;
                                                }
                                            }
                                        }
                                        else
                                        {

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return amountOfWhitePixels;
        }

        #region Model

        private static Dictionary<int, List<SlicePoint2D>> GetModelIntersectionPointsFromPolyTrees(List<PolyTree> modelPolyTrees, int projectorResolutionX, int projectorResolutionY)
        {
            var modelIntersectionPoints = new Dictionary<int, List<SlicePoint2D>>();
            var modelPointsByY = new SortedList<int, List<SliceLine2D>>();
            if (modelPolyTrees != null)
            {
                foreach (var polyTree in modelPolyTrees)
                {
                    foreach (var child in polyTree.Childs)
                    {
                        GetIntersectionPointByPolyNode(child, projectorResolutionY, modelPointsByY);
                    }
                }

                //get intersectionPoints

                foreach (var modelPointIndex in modelPointsByY.Keys)
                {
                    if (modelPointIndex > 0)
                    {
                        if (modelPointsByY[modelPointIndex] != null)
                        {
                            //beware zero based index
                            GetModelIntersectingPoints(modelPointIndex, modelPointsByY[modelPointIndex], ref modelIntersectionPoints);
                            //do not sort! will be done in later stage
                        }
                    }
                }
            }

            return modelIntersectionPoints;

        }

        private static void GetIntersectionPointByPolyNode(PolyNode polyNode, int projectorResolutionY, SortedList<int, List<SliceLine2D>> modelPointsByY)
        { 

            for (var contourLineIndex = 0; contourLineIndex < polyNode.Contour.Count; contourLineIndex++)
            {
                var contourLine = new SliceLine2D();
                contourLine.p1 = new SlicePoint2D(polyNode.Contour[contourLineIndex]);
                if (contourLineIndex < polyNode.Contour.Count - 1)
                {
                    contourLine.p2 = new SlicePoint2D(polyNode.Contour[contourLineIndex + 1]);
                }
                else
                {
                    contourLine.p2 = new SlicePoint2D(polyNode.Contour[0]);
                }

                if (contourLine.p2.Y < contourLine.p1.Y)
                {
                    //flip order
                    var tempP1 = contourLine.p1;
                    contourLine.p1 = contourLine.p2;
                    contourLine.p2 = tempP1;
                }

                //check if line has a pixel intersecting a whole pixel
                var startLinePixelY = (int)Math.Ceiling(contourLine.p1.Y);
                var endLinePixelY = (int)Math.Floor(contourLine.p2.Y);
                if (endLinePixelY > projectorResolutionY - 1)
                {
                    endLinePixelY = projectorResolutionY - 1;
                }

                if (startLinePixelY < projectorResolutionY && startLinePixelY <= endLinePixelY)
                {
                    if (contourLine.p1.Y != contourLine.p2.Y)
                    {
                        for (var lineContainsIndexY = startLinePixelY; lineContainsIndexY <= endLinePixelY; lineContainsIndexY++)
                        {
                            if (!modelPointsByY.ContainsKey(lineContainsIndexY))
                            {
                                modelPointsByY.Add(lineContainsIndexY, new List<SliceLine2D>());
                            }

                            modelPointsByY[lineContainsIndexY].Add(contourLine);
                        }
                    }
                }
            }

            foreach(var child in polyNode.Childs)
            {
                GetIntersectionPointByPolyNode(child, projectorResolutionY, modelPointsByY);
            }
        }

        private static void GetModelRasterPoints(PolyNode polyNode, int projectorResolutionY, SortedList<int, List<SliceLine2D>> modelPointsByY)
        {
            var contourRasterPoints = new List<SlicePoint2D>();

            for (var contourLineIndex = 0; contourLineIndex < polyNode.Contour.Count; contourLineIndex++)
            {
                var contourLine = new SliceLine2D();
                contourLine.p1 = new SlicePoint2D(polyNode.Contour[contourLineIndex]);
                if (contourLineIndex < polyNode.Contour.Count - 1)
                {
                    contourLine.p2 = new SlicePoint2D(polyNode.Contour[contourLineIndex + 1]);
                }
                else
                {
                    contourLine.p2 = new SlicePoint2D(polyNode.Contour[0]);
                }

                //check if line has a pixel intersecting a whole pixel
                var startLinePixelY = (int)contourLine.p1.Y;
                var startLineY = contourLine.p1.Y;
                var endLinePixelY = (int)contourLine.p2.Y;
                var endLineY = contourLine.p2.Y;

                if (contourLine.p1.Y > contourLine.p2.Y)
                {
                    startLinePixelY = (int)contourLine.p2.Y;
                    startLineY = contourLine.p2.Y;
                    endLinePixelY = (int)contourLine.p1.Y;
                    endLineY = contourLine.p1.Y;
                }

                var rasterPoints = new List<SlicePoint2D>();
                if (startLinePixelY != endLinePixelY)
                {
                    //get intersectionpoints
                    for(var i = startLinePixelY - 1;i <= endLinePixelY;i++)
                    {
                        if (i >= startLineY && i <= endLineY)
                        {
                            rasterPoints.Add(contourLine.IntersectY(i));
                        }
                    }
                }

                //x raster points
                var startLinePixelX = (int)contourLine.p1.X;
                var startLineX = contourLine.p1.X;
                var endLinePixelX = (int)contourLine.p2.X;
                var endLineX = contourLine.p2.X;

                if (contourLine.p1.X > contourLine.p2.X)
                {
                    startLinePixelX = (int)contourLine.p2.X;
                    startLineX = contourLine.p2.X;
                    endLinePixelX = (int)contourLine.p1.X;
                    endLineX = contourLine.p1.X;
                }

                if (startLinePixelX != endLinePixelX)
                {
                    //get intersectionpoints
                    for (var i = startLinePixelX - 1; i <= endLinePixelX; i++)
                    {
                        if (i >= startLineX && i <= endLineX)
                        {
                            rasterPoints.Add(contourLine.IntersectX(i));
                        }
                    }
                }


                if (rasterPoints.Count > 0)
                {
                    //order by distance
                    var rasterPointsOrdered = new SortedDictionary<float, SlicePoint2D>();
                    var lineStartPointAsVector2 = new Vector2(contourLine.p1.X, contourLine.p1.Y);
                    foreach (var rasterPoint in rasterPoints)
                    {
                        var pointDistance = (new Vector2(rasterPoint.X, rasterPoint.Y) - (lineStartPointAsVector2)).Length;
                        rasterPointsOrdered.Add(pointDistance, rasterPoint);
                    }

                    contourRasterPoints.AddRange(rasterPointsOrdered.Values);
                }
            }

            var bitmap = new Bitmap(1920, 1080);
            var g = Graphics.FromImage(bitmap);
            var path = new GraphicsPath();

            var points = new List<PointF>();
            foreach(var contourRasterPoint in contourRasterPoints)
            {
                points.Add(new PointF(contourRasterPoint.X, contourRasterPoint.Y));
            }

            path.AddLines(points.ToArray());

            g.DrawLines(new Pen(Color.Red, 1), points.ToArray());

            points = new List<PointF>();
            for (var contourRasterPointIndex = 0; contourRasterPointIndex < contourRasterPoints.Count - 1; contourRasterPointIndex++)
            {
                //calc rasterline volume
                var rasterPointStart = contourRasterPoints[contourRasterPointIndex];
                if (contourRasterPointIndex == 0)
                {
                    rasterPointStart = contourRasterPoints[contourRasterPoints.Count - 1];
                }

                var rasterPointEnd = contourRasterPoints[contourRasterPointIndex + 1];

                var rasterPointVolume = rasterPointStart.X - (int)rasterPointStart.X;
                rasterPointVolume += rasterPointStart.Y - (int)rasterPointStart.Y;
                rasterPointVolume += rasterPointEnd.X - (int)rasterPointEnd.X;
                rasterPointVolume += rasterPointEnd.Y - (int)rasterPointEnd.Y;
                rasterPointVolume /= 4;
                rasterPointVolume *= 100;
                if (rasterPointVolume < 1f)
                {
                    rasterPointVolume = 1f;
                }
                rasterPointVolume = 255 / rasterPointVolume;

                g.DrawLine(new Pen(Color.FromArgb(255, (int)rasterPointVolume, 255, 255)), rasterPointStart.X, rasterPointStart.Y, rasterPointEnd.X, rasterPointEnd.Y);
            }

            bitmap.Save("t.png");
        }

        internal static void GetModelIntersectingPoints(int y, List<SliceLine2D> lines, ref Dictionary<int, List<SlicePoint2D>> intersectionPoints)
        {
            float ymin = 0;
            float ymax = 0;

            if (!intersectionPoints.ContainsKey(y))
            {
                intersectionPoints.Add(y, new List<SlicePoint2D>());
            }

            //process crossing lines. When end point intersect with horizontal line do not include points
            foreach (var ln in lines)
            {
                ymin = Math.Min(ln.p1.Y, ln.p2.Y);
                ymax = Math.Max(ln.p1.Y, ln.p2.Y);

                if (ln.p1.Y == y && ln.p2.Y == y) //skip
                {
                }
                else if (ln.p1.Y == y) // point 1 endpoint is on the line,
                {
                    //If the intersection is the ymin of the edge’s
                    //endpoint, count it. Otherwise, don’t
                    if (ln.p1.Y == ymin) // if the 
                    {
                        var point = ln.p1;
                        point.Normal = ln.Normal;
                        intersectionPoints[y].Add(point);
                    }
                }
                else if (ln.p2.Y == y) // point 2 endpoint is on the line,
                {
                    //If the intersection is the ymin of the edge’s
                    //endpoint, count it. Otherwise, don’t
                    if (ln.p2.Y == ymin)
                    {
                        var point = ln.p2;
                        point.Normal = ln.Normal;
                        intersectionPoints[y].Add(point);
                    }
                }
                else // intersect  
                {
                    var isect = ln.IntersectY(y); // singled point of intersection
                    isect.Normal = ln.Normal;
                    intersectionPoints[y].Add(isect);
                }
            }
        }


        #endregion

        #region Support
        private static Dictionary<int, List<SlicePoint2D>> GetSupportIntersectionPointsFromPolyNode(PolyNode polyNode, int projectorResolutionX, int projectorResolutionY)
        {
            var totalSupportStructureIntersectionPoints = new List<Dictionary<int, List<SlicePoint2D>>>();
            var supportPointsByY = new SortedList<int, List<SliceLine2D>>();

            for (var contourLineIndex = 0; contourLineIndex < polyNode.Contour.Count; contourLineIndex++)
            {
                var contourLine = new SliceLine2D();
                contourLine.p1 = new SlicePoint2D(polyNode.Contour[contourLineIndex]);
                if (contourLineIndex < polyNode.Contour.Count - 1)
                {
                    contourLine.p2 = new SlicePoint2D(polyNode.Contour[contourLineIndex + 1]);
                }
                else
                {
                    contourLine.p2 = new SlicePoint2D(polyNode.Contour[0]);
                }

                if (contourLine.p2.Y < contourLine.p1.Y)
                {
                    //flip order
                    var tempP1 = contourLine.p1;
                    contourLine.p1 = contourLine.p2;
                    contourLine.p2 = tempP1;
                }

                //check if line has a pixel intersecting a whole pixel
                var startLinePixelY = (int)Math.Ceiling(contourLine.p1.Y);
                var endLinePixelY = (int)Math.Floor(contourLine.p2.Y);
                if (endLinePixelY > projectorResolutionY - 1)
                {
                    endLinePixelY = projectorResolutionY - 1;
                }

                if (startLinePixelY < projectorResolutionY && startLinePixelY <= endLinePixelY)
                {
                    if (contourLine.p1.Y != contourLine.p2.Y)
                    {
                        for (var lineContainsIndexY = startLinePixelY; lineContainsIndexY <= endLinePixelY; lineContainsIndexY++)
                        {
                            if (!supportPointsByY.ContainsKey(lineContainsIndexY))
                            {
                                supportPointsByY.Add(lineContainsIndexY, new List<SliceLine2D>());
                            }

                            supportPointsByY[lineContainsIndexY].Add(contourLine);
                        }
                    }
                }
            }

            //get intersectionPoints for this support cone
            var supportIntersectionPoints = new Dictionary<int, List<SlicePoint2D>>();
            foreach (var supportPointIndex in supportPointsByY.Keys)
            {
                if (supportPointsByY[supportPointIndex] != null)
                {
                    //beware zero based index
                    GetIntersectingSupportPoints(supportPointIndex, supportPointsByY[supportPointIndex], ref supportIntersectionPoints);

                    //get normal based intersection points
                    //supportIntersectionPoints[supportPointIndex] = GetSupportPointsByNormal(supportIntersectionPoints[supportPointIndex], supportPointIndex);
                    //do not sort! will be done in later stage
                }
            }

            return supportIntersectionPoints;

        }

        internal static void GetIntersectingSupportPoints(int y, List<SliceLine2D> lines, ref Dictionary<int, List<SlicePoint2D>> intersectionPoints)
        {
            int ymin = 0;
            int ymax = 0;

            //process crossing lines. When end point intersect with horizontal line do not include points
            foreach (var ln in lines)
            {
                ymin = (int)Math.Round(Math.Min(ln.p1.Y, ln.p2.Y), 0);
                ymax = (int)Math.Round(Math.Max(ln.p1.Y, ln.p2.Y), 0);

                if (!intersectionPoints.ContainsKey(y))
                {
                    intersectionPoints.Add(y, new List<SlicePoint2D>());
                }

                if (ln.p1.Y == y && ln.p2.Y == y) //skip
                {

                }
                else if (ln.p1.Y == y) // point 1 endpoint is on the line,
                {
                    //If the intersection is the ymin of the edge’s
                    //endpoint, count it. Otherwise, don’t
                    if (ln.p1.Y == ymin) // if the 
                    {
                        intersectionPoints[y].Add(ln.p1);
                    }


                }
                else if (ln.p2.Y == y) // point 2 endpoint is on the line,
                {
                    //If the intersection is the ymin of the edge’s
                    //endpoint, count it. Otherwise, don’t
                    if (ln.p2.Y == ymin)
                    {
                        intersectionPoints[y].Add(ln.p2);
                    }
                }
                else // intersect  
                {

                    var isect = ln.IntersectY(y); // singled point of intersection
                    intersectionPoints[y].Add(isect);
                }
            }

            //append double points as endpoint or begin point
            if (intersectionPoints[y].Count == 2)
            {
                foreach (var ln in lines)
                {
                    if (ln.p1 == ln.p2 && ln.p1.Y == y)
                    {
                        if (ln.p1.X < intersectionPoints[y][0].X)
                        {
                            intersectionPoints[y][0].X = ln.p1.X;
                        }

                        if (ln.p2.X < intersectionPoints[y][1].X)
                        {
                            intersectionPoints[y][1].X = ln.p2.X;
                        }
                    }
                }
            }

            if (intersectionPoints[y].Count > 2)
            {
                //find left point
                float minPoint = float.MaxValue;
                float maxPoint = float.MinValue;
                SlicePoint2D leftLine = null;
                SlicePoint2D rightLine = null;
                for (var intersectionPointIndex = 0; intersectionPointIndex < intersectionPoints[y].Count; intersectionPointIndex++)
                {
                    if (intersectionPoints[y][intersectionPointIndex].X < minPoint)
                    {
                        minPoint = intersectionPoints[y][intersectionPointIndex].X;
                        leftLine = intersectionPoints[y][intersectionPointIndex];
                    }

                    if (intersectionPoints[y][intersectionPointIndex].X > maxPoint)
                    {
                        maxPoint = intersectionPoints[y][intersectionPointIndex].X;
                        rightLine = intersectionPoints[y][intersectionPointIndex];
                    }
                }
                intersectionPoints[y].Clear();
                intersectionPoints[y].Add(leftLine);
                intersectionPoints[y].Add(rightLine);
            }


            //always add/substract 1 px left and right
            if (intersectionPoints[y].Count == 2)
            {
                if (intersectionPoints[y][0].X < intersectionPoints[y][1].X)
                {

                    if (intersectionPoints[y][0].X > 0)
                    {
                        intersectionPoints[y][0].X--;
                    }

                    intersectionPoints[y][1].X++;
                }
                else
                {
                    if (intersectionPoints[y][1].X > 0)
                    {
                        intersectionPoints[y][1].X--;
                    }

                    intersectionPoints[y][0].X++;
                }
            }
        }


        #endregion

    }
}
