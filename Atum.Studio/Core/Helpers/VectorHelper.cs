using Atum.DAL.Hardware;
using Atum.DAL.Print;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Objects;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Helpers
{
    internal class VectorHelper
    {
        public static bool LineSegmentsIntersect(Vector2 p, Vector2 p2, Vector2 q, Vector2 q2, out Vector2 intersection, bool considerCollinearOverlapAsIntersect = false)
        {
            intersection = new Vector2();

            var r = p2 - p;
            var s = q2 - q;
            var rxs = r.Cross(s);
            var qpxr = (q - p).Cross(r);

            // If r x s = 0 and (q - p) x r = 0, then the two lines are collinear.
            if (rxs.IsZero() && qpxr.IsZero())
            {
                // 1. If either  0 <= (q - p) * r <= r * r or 0 <= (p - q) * s <= * s
                // then the two lines are overlapping,
                if (considerCollinearOverlapAsIntersect)
                    if ((0 <= (q - p).Dot(r) && (q - p).Dot(r) <= r.Dot(r) || (0 <= (p - q).Dot(s) && (p - q).Dot(s) <= s.Dot(s))))
                        return true;

                // 2. If neither 0 <= (q - p) * r = r * r nor 0 <= (p - q) * s <= s * s
                // then the two lines are collinear but disjoint.
                // No need to implement this expression, as it follows from the expression above.
                return false;
            }

            if (rxs.IsZero() && !qpxr.IsZero())
                return false;

            var t = (q - p).Cross(s) / rxs;
            var u = (q - p).Cross(r) / rxs;
            if (!rxs.IsZero() && (0 <= t && t <= 1) && (0 <= u && u <= 1))
            {
                // We can calculate the intersection point using either t or u.
                intersection = p + r.Dot(t);

                // An intersection was found.
                return true;
            }

            // 5. Otherwise, the two line segments are not parallel but do not intersect.
            return false;
        }


        #region Shape Helpers

        public static List<Vector3Class> CreateCircle(float height, float radius, int slicesCount, bool CCW)
        {
            var vectors = new List<Vector3Class>();

            double step = (double)(Math.PI * 2) / slicesCount;
            double t = 0.0;
            for (int cnt = 0; cnt < slicesCount; cnt++)
            {
                var pnt = new Vector3Class(); // bottom points
                pnt.X = radius * (float)Math.Cos(t);
                pnt.Y = radius * (float)Math.Sin(t);
                pnt.Z = height;
                vectors.Add(pnt);
                t += step;
            }

            return vectors;
        }

        internal static List<Vector3Class> GetCircleOutlinePoints(float height, float radius, int slicesCount, Vector3Class position, float angle = 360f)
        {
            var vectors = new List<Vector3Class>();

            double step = (double)((Math.PI * 2) * (angle / 360f)) / slicesCount;
            double t = 0.0;
            for (int cnt = 0; cnt < slicesCount; cnt++)
            {
                var pnt = new Vector3Class(); // bottom points
                pnt.X = radius * (float)Math.Cos(t) + position.X;
                pnt.Y = radius * (float)Math.Sin(t) + position.Y;
                pnt.Z = height + position.Z;
                vectors.Add(pnt);
                t += step;
            }

            return vectors;
        }

        #endregion

        #region PolyTree Helpers

        internal static List<SliceContourInfo> ConvertPolyNodeToContours(ContourHelper.PolyNode polyNode, List<List<ContourHelper.IntPoint>> originalPolygons)
        {
            var decimalCorrectionFactor = 10000f;
            var result = new List<SliceContourInfo>();
            if (polyNode != null)
            {
                if (!polyNode.IsHole)
                {
                    var contour = new SliceContourInfo();
                    foreach (var point in polyNode.m_polygon)
                    {
                        foreach (var originalPolygon in originalPolygons)
                        {
                            if (originalPolygon.Contains(point))
                            {
                                foreach (var originalPoint in originalPolygon)
                                {
                                    contour.OuterPath.Add(originalPoint);
                                }

                                originalPolygons.Remove(originalPolygon);


                                break;
                            }

                        }
                    }

                    foreach (var child in polyNode.Childs)
                    {
                        if (child.IsHole)
                        {
                            var childPoints = new List<Vector3>();
                            var skipPointEnum = false;

                            foreach (var point in child.m_polygon)
                            {

                                foreach (var originalPolygon in originalPolygons)
                                {
                                    if (originalPolygon.Contains(point))
                                    {
                                        var listOfInnerPoints = new List<Vector3>();
                                        foreach (var originalPoint in originalPolygon)
                                        {
                                            listOfInnerPoints.Add(new Vector3(originalPoint.X / decimalCorrectionFactor, originalPoint.Y / decimalCorrectionFactor, originalPoint.Z / decimalCorrectionFactor));
                                        }

                                        originalPolygons.Remove(originalPolygon);

                                        //contour.InnerPoints.Add(listOfInnerPoints);

                                        skipPointEnum = true;
                                        break;
                                    }
                                }

                                if (skipPointEnum)
                                {
                                    skipPointEnum = false;
                                    break;
                                }
                            }
                            // contour.InnerPoints.Add(childPoints);

                            foreach (var sibblingChild in child.Childs)
                            {
                                result.AddRange(ConvertPolyNodeToContours(sibblingChild, originalPolygons));
                            }
                        }
                    }

                    result.Add(contour);
                }
            }

            return result;
        }

        internal static List<SliceLine2D> ConvertPolyTreeWithOffsetToSliceLine2D(float sliceHeight, List<PolyTree> polytrees, float insideOffset, float outsideOffset)
        {
            //calc model points using polygon offset
            var result = new List<SliceLine2D>();
            if (sliceHeight != 10.65f)
            {
                return result;
            }

            var decimalCorrectionFactor = 10000f;
            var selectedPrinterProjectorVector = new Vector2(RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX, RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY);

            var clipperOffset = new ClipperOffset();
            var holes = new List<PolyNode>();
            var outsides = new List<PolyNode>();
            foreach (var polygon in polytrees)
            {
                foreach (var pointInPath in polygon._allPolys)
                {
                    if (pointInPath.Contour.Count > 2)
                    {
                        clipperOffset.Clear();
                        clipperOffset = new ClipperOffset();

                        var results = new PolyTree();
                        clipperOffset.AddPath(pointInPath.Contour, JoinType.jtMiter, EndType.etClosedPolygon);

                        if (!pointInPath.IsHole)
                        {

                            //detax -2
                            //abs -1
                            clipperOffset.Execute(ref results, outsideOffset * decimalCorrectionFactor);

                            foreach (var offsetPolygon in results.Childs)
                            {
                                outsides.Add(offsetPolygon);
                            }
                        }
                        else
                        {
                            //detax 2
                            //abs 1
                            clipperOffset.Execute(ref results, insideOffset * decimalCorrectionFactor);

                            foreach (var offsetPolygon in results.Childs)
                            {
                                holes.Add(offsetPolygon);
                            }
                        }
                    }
                }

                //prevent line crossing first union the holes (removes holes intersections)
                var unionHolesPolyTree = new PolyTree();
                if (holes.Count > 1)
                {
                    var clipper = new Clipper();
                    foreach (var hole in holes)
                    {
                        clipper.AddPath(hole.Contour, PolyType.ptSubject, true);
                    }


                    clipper.Execute(ClipType.ctUnion, unionHolesPolyTree, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
                }
                else if (holes.Count == 1)
                {
                    unionHolesPolyTree.AddChild(holes[0]);
                }


                //prevent line crossing second union the outsides (removes outside intersections)
                var unionOutsidesPolyTree = new PolyTree();
                if (outsides.Count > 1)
                {
                    var clipper = new Clipper();
                    foreach (var outside in outsides)
                    {
                        clipper.AddPath(outside.Contour, PolyType.ptSubject, true);
                    }


                    clipper.Execute(ClipType.ctUnion, unionOutsidesPolyTree, PolyFillType.pftNonZero, PolyFillType.pftNonZero);
                }
                else if (outsides.Count == 1)
                {
                    unionOutsidesPolyTree.AddChild(outsides[0]);
                }

                //subtract holes from outsides
                var diffPolyTree = new PolyTree();
                var diffPolyTreeClipper = new Clipper();
                foreach (var unionPolyNode in unionOutsidesPolyTree.Childs)
                {
                    diffPolyTreeClipper.AddPath(unionPolyNode.Contour, PolyType.ptSubject, true);
                }

                foreach (var unionPolyNode in unionHolesPolyTree.Childs)
                {
                    diffPolyTreeClipper.AddPath(unionPolyNode.Contour, PolyType.ptClip, true);
                }

                diffPolyTreeClipper.Execute(ClipType.ctDifference, diffPolyTree, PolyFillType.pftNonZero);

                //    var triangle3D = new Triangle();


                //            foreach (var offsetPolygon in results.Childs)
                //            {
                //                //change order to CCW
                //                var directionIsCW = Clipper.Orientation(offsetPolygon.Contour);
                //                if (directionIsCW)
                //                {
                //                    Clipper.ReversePaths(new List<List<IntPoint>>() { offsetPolygon.Contour });
                //                }

                //                //convert to 3d triangle and determine normal
                //                for (var contourIndex = 0; contourIndex < offsetPolygon.Contour.Count; contourIndex++)
                //                {
                //                    triangle3D.Vectors[0].Position = new Vector3(offsetPolygon.Contour[contourIndex].X, offsetPolygon.Contour[contourIndex].Y, 0);

                //                    if (contourIndex == offsetPolygon.Contour.Count - 1)
                //                    {
                //                        triangle3D.Vectors[1].Position = new Vector3(offsetPolygon.Contour[0].X, offsetPolygon.Contour[0].Y, 0);
                //                        triangle3D.Vectors[2].Position = new Vector3(offsetPolygon.Contour[0].X, offsetPolygon.Contour[0].Y, 1);
                //                    }
                //                    else
                //                    {
                //                        triangle3D.Vectors[1].Position = new Vector3(offsetPolygon.Contour[contourIndex + 1].X, offsetPolygon.Contour[contourIndex + 1].Y, 0);
                //                        triangle3D.Vectors[2].Position = new Vector3(offsetPolygon.Contour[contourIndex + 1].X, offsetPolygon.Contour[contourIndex + 1].Y, 1);
                //                    }

                //                    triangle3D.Vectors[0].Position /= decimalCorrectionFactor;
                //                    triangle3D.Vectors[1].Position /= decimalCorrectionFactor;
                //                    triangle3D.Vectors[2].Position /= decimalCorrectionFactor;

                //                    triangle3D.CalcNormal();

                //                    var triangleNormal = triangle3D.Normal;
                //                    if (!pointInPath.IsHole)
                //                    {
                //                        triangleNormal = triangleNormal * -1;
                //                    }


                //                    var line = new SliceLine2D();
                //                    line.Normal = triangleNormal;

                //                    if (contourIndex == offsetPolygon.Contour.Count - 1)
                //                    {
                //                        line.p1 = new SlicePoint2D() { X = triangle3D.Vectors[0].Position.X, Y = triangle3D.Vectors[0].Position.Y};
                //                        //line.p2 = new SlicePoint2D() { X = offsetPolygon.Contour[0].X / decimalCorrectionFactor, Y = offsetPolygon.Contour[0].Y / decimalCorrectionFactor };
                //                        //

                //                        line.p2 = new SlicePoint2D() { X = triangle3D.Vectors[1].Position.X, Y = triangle3D.Vectors[1].Position.Y };
                //                    }
                //                    else
                //                    {
                //                        line.p1 = new SlicePoint2D() { X = triangle3D.Vectors[0].Position.X, Y = triangle3D.Vectors[0].Position.Y};
                //                        line.p2 = new SlicePoint2D() { X = triangle3D.Vectors[1].Position.X, Y = triangle3D.Vectors[1].Position.Y};
                //                    }

                //                    result.Add(line);

                //                }
                //            }
                //        }
                //    }
            }

            return result;
        }

        internal static double GetArea(List<IntPoint> vertices)
        {
            if (vertices.Count < 3)
            {
                return 0;
            }
            double area = GetDeterminant(vertices[vertices.Count - 1].X, vertices[vertices.Count - 1].Y, vertices[0].X, vertices[0].Y);
            for (int i = 1; i < vertices.Count; i++)
            {
                area += GetDeterminant(vertices[i - 1].X, vertices[i - 1].Y, vertices[i].X, vertices[i].Y);
            }
            return area / 2;
        }

        private static double GetDeterminant(double x1, double y1, double x2, double y2)
        {
            return x1 * y2 - x2 * y1;
        }

        #endregion

        #region Vector Helpers

        public static bool IsSameDirection(Vector3Class vector1, Vector3Class vector2)
        {
            var result = false;
            if (Vector3Class.Dot(vector1, vector2) > 0)
            {
                result = true;
            }

            return result;
        }

        internal static List<List<IntPoint>> GetPointsOnPath(List<IntPoint> path, float distance, bool closedPath, bool skipOffset, float offsetDistance)
        {
            var pathAsVector = new List<Vector3Class>();
            foreach (var pathPoint in path)
            {
                pathAsVector.Add(pathPoint.XY.AsVector3());
            }

            var pointsOnPathAsVector = GetPointsOnPath(pathAsVector, distance, closedPath, skipOffset, offsetDistance);
            var pointsOnPathAsIntPoint = new List<IntPoint>();
            foreach (var pointOnPathAsVector in pointsOnPathAsVector)
            {
                pointsOnPathAsIntPoint.Add(new IntPoint(new Vector3Class(pointOnPathAsVector.X, pointOnPathAsVector.Y, 0f)));
            }

            return new List<List<IntPoint>> { pointsOnPathAsIntPoint };
        }

        //internal static List<Vector3> GetPointsOnPath(List<Vector3> path, float distance, float offset)
        //{
        //    var pointsOnPath = new List<Vector3>();

        //    var pathLength = 0f;
        //    for (var correctedContourPartIndex = 0; correctedContourPartIndex < path.Count - 1; correctedContourPartIndex++)
        //    {
        //        var contourLineStart = path[correctedContourPartIndex];
        //        Vector3 pathLineEnd = pathLineEnd = path[correctedContourPartIndex + 1];
        //        var pathLineVector = (pathLineEnd - contourLineStart);
        //        var pathLineLength = pathLineVector.Length;

        //        pathLength += pathLineLength;
        //    }

        //    if (offset == 0 && pathLength > 0)
        //    {
        //        offset = (pathLength % distance) / 2f;
        //    }


        //    if (pathLength > 0)
        //    {
        //        var newDistanceLength = pathLength;
        //        var currentDistance = offset;

        //        if (pathLength <= distance)
        //        {
        //            currentDistance = -pathLength / 2f;
        //            distance = pathLength / 0.5f;
        //        }
        //        else
        //        {
        //            //currentDistance -= distance; //start negative
        //        }


        //        pointsOnPath.Add(path[0]);
        //        for (var correctedContourPartIndex = 0; correctedContourPartIndex < path.Count- 1; correctedContourPartIndex++)
        //        {
        //            var contourLineStart = path[correctedContourPartIndex];

        //            Vector3 contourLineEnd = path[correctedContourPartIndex + 1];
        //            var contourLineVector = (contourLineEnd - contourLineStart);
        //            var contourLineLength = contourLineVector.Length;
        //            currentDistance += contourLineLength;

        //            while (currentDistance > 0)
        //            {
        //                var startOffsetVectorPercentage = (contourLineLength / distance);
        //                var startOffsetVector = contourLineVector / startOffsetVectorPercentage;
        //                contourLineStart += startOffsetVector;
        //                pointsOnPath.Add(contourLineStart);

        //                currentDistance -= distance;
        //            }
        //        }

        //    }

        //    return pointsOnPath;
        //}

        public static List<Vector3Class> GetPointsOnPath(List<Vector3Class> path, float distance, bool closedPath, bool skipOffset, float offsetDistance)
        {
            var pathClone = new List<Vector3Class>();
            pathClone.AddRange(path);

            if (closedPath)
            {
                pathClone.Add(path[0]);
            }

            var pointsOnPath = new List<Vector3Class>();

            var previousDistance = 0f;

            var pathLength = 0f;
            for (var correctedContourPartIndex = 0; correctedContourPartIndex < pathClone.Count - 1; correctedContourPartIndex++)
            {
                var contourLineStart = pathClone[correctedContourPartIndex];
                var pathLineEnd = pathClone[correctedContourPartIndex + 1];
                var pathLineVector = (pathLineEnd - contourLineStart);
                var pathLineLength = pathLineVector.Length;

                pathLength += pathLineLength;
            }

            
            if (pathLength > 0)
            {
                var newDistanceLength = pathLength / 2f;
                var currentDistance = 0f;

                if (pathLength > distance)
                {
                    newDistanceLength = pathLength / (int)((pathLength / distance) + 1);
                }

                if (!closedPath)
                {
                    if (!skipOffset)
                    {
                        currentDistance = newDistanceLength / 2;
                    }
                    else
                    {
                        currentDistance = 0;

                        if (pathLength < distance)
                        {
                            newDistanceLength = pathLength / 2f;
                        }
                        else
                        {
                            newDistanceLength =distance;
                        }
                        
                    }
                }

                currentDistance = offsetDistance;
                if (currentDistance > pathLength)
                {
                    currentDistance = newDistanceLength / 2;
                }

                for (var correctedContourPartIndex = 0; correctedContourPartIndex < pathClone.Count - 1; correctedContourPartIndex++)
                {
                    var contourLineStart = pathClone[correctedContourPartIndex];

                    var contourLineEnd = pathClone[correctedContourPartIndex + 1];
                    var contourLineVector = (contourLineEnd - contourLineStart);
                    var contourLineLength = contourLineVector.Length;
                    currentDistance += contourLineLength;

                    var startOffsetDistance = newDistanceLength - previousDistance;

                    while (currentDistance >= newDistanceLength && startOffsetDistance > 0)
                    {
                        var startOffsetVectorPercentage = (startOffsetDistance / contourLineLength);
                        var startOffsetVector = startOffsetVectorPercentage * contourLineVector;
                        pointsOnPath.Add(contourLineStart + startOffsetVector);

                        currentDistance -= newDistanceLength;
                        startOffsetDistance += newDistanceLength;
                    }

                    previousDistance = currentDistance;
                }

                //remove last point
                if (skipOffset)
                {
                    if (pointsOnPath.Count > 1)
                    {
                        pointsOnPath.RemoveAt(pointsOnPath.Count - 1);
                    }
                }
                
            }

            if (closedPath)
            {
                pointsOnPath.Add(path[0]);
            }

            return pointsOnPath;
        }

        internal static float CalcAngleBetweenVectorPoints(Vector2 point1, Vector2 centerPoint, Vector2 point2)
        {
            var vector1 = Vector2.Normalize((point1 - centerPoint));
            var vector2 = Vector2.Normalize((point2 - centerPoint));
            var dotAngle = Vector2.Dot(vector1, vector2);
            var angle = (float)OpenTK.MathHelper.RadiansToDegrees(Math.Acos(dotAngle));
            return angle;
        }

        internal static void CalcRotationAnglesYZFromVector(Vector3Class vector, bool inverseOrigin, out float zAngle, out float yAngle)
        {
            //first get X angle
            zAngle = OpenTK.MathHelper.RadiansToDegrees((float)Math.Atan2(vector.Y, vector.X));

            if (Math.Round(vector.Y, 2) == 0.00d)
            {
                zAngle = 0;
            }

            //before calc y angle rotate vector using xAngle transform
            Matrix4 rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(-zAngle));

            var rotatedVector = Vector3Class.Transform(vector, rotationMatrix);
            var yAngleNormal = inverseOrigin ? -90 : 90;
            yAngle = yAngleNormal - OpenTK.MathHelper.RadiansToDegrees((float)Math.Atan2(rotatedVector.Z, rotatedVector.X));

            if (Math.Round(1 - vector.Z, 2) == 0.00d)
            {
                yAngle = 0;
            }

            if (yAngle == 360)
            {
                yAngle = 0;
            }
        }     

        internal static bool HasTriangleOverhangSide(Triangle triangle, out Vector3Class vector1, out Vector3Class vector2)
        {
            var result = false;
            vector1 = vector2= new Vector3Class();

            if (triangle.Normal.Z < 0)
            {
                for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                {
                    var startIndex = vectorIndex - 1;
                    if (startIndex < 0)
                    {
                        startIndex = 2;
                    }
                    var endIndex = vectorIndex;
                    var vectorAngle = CalcZAngleBetweenPoints(triangle.Vectors[startIndex].Position, triangle.Vectors[endIndex].Position);
                    if ((vectorAngle >= 0 && vectorAngle <= 1) || (vectorAngle >= 179 && vectorAngle <= 181) || (vectorAngle >= 359 && vectorAngle <= 360))
                    {
                        vector1 = triangle.Vectors[startIndex].Position;
                        vector2 = triangle.Vectors[endIndex].Position;
                        return true;
                    }
                }
            }
            
            return result;
        }

        private static float CalcZAngleBetweenPoints(Vector3Class vector1, Vector3Class vector2)
        {
            float zDiff = vector2.Z - vector1.Z;
            float xLength = (vector2 - new Vector3Class(0,0,zDiff) - vector1).Length;
            var zAngle = (float)(Math.Atan2(zDiff, xLength) * 180.0 / Math.PI);
            if (zAngle < 0)
            {
                zAngle = 360 - -zAngle;
            }
            return zAngle;
        }
        

        #endregion

        #region Sliceline Helpers

        internal static Vector3Class GetPixelPointFromModelPoint(Vector3Class modelPoint, AtumPrinter selectedPrinter)
        {
            int hxres = RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX / 2;
            int hyres = RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY / 2;

            return new Vector3Class((float)(modelPoint.X * 10.0) + 1 + hxres, (float)(modelPoint.Y * 10.0) + 1 + hyres, modelPoint.Z);
        }

        internal static Vector3Class GetModelPointFromPixelPoint(Vector3Class pixelPoint, AtumPrinter selectedPrinter)
        {
            int hxres = RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX / 2;
            int hyres = RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY / 2;

            pixelPoint = pixelPoint - new Vector3Class(hxres + 1, hyres + 1, 0);
            pixelPoint.X /= 10f;
            pixelPoint.Y /= 10f;

            return pixelPoint;
        }

        /// <summary>
        /// this function converts all the 3d polyines to 2d lines so we can process everything 
        /// This is the equivanlant of a 3d->2d projection function
        /// </summary>
        /// <param name="sp"></param>
        /// <returns></returns>
        internal static List<Line3D> Get2dLinesAsPixels(List<SlicePolyLine3D> polyLines3D, AtumPrinter selectedPrinter)
        {
            var lst = new List<Line3D>();
            int hxres = selectedPrinter.ProjectorResolutionX / 2;
            int hyres = selectedPrinter.ProjectorResolutionY / 2;

            foreach (var polyLine3D in polyLines3D)
            {
                if (polyLine3D.Points[0].X == polyLine3D.Points[1].X && polyLine3D.Points[0].Y == polyLine3D.Points[1].Y)
                {
                }
                else
                {
                    var ln = new Line3D(new Vector3Class((float)(polyLine3D.Points[0].X * 10.0) + 1 + hxres, (float)(polyLine3D.Points[0].Y * 10.0) + 1 + hyres, 0),  //point1
                        new Vector3Class((float)(polyLine3D.Points[1].X * 10.0) + 1 + hxres, (float)(polyLine3D.Points[1].Y * 10.0) + 1 + hyres, 0), //point2
                        polyLine3D.Normal); //normal
                    ln.TriangleConnection = polyLine3D.TriangleConnection;
                    lst.Add(ln);
                }
            }

            return lst; // return the list
        }

        internal static List<Line3D> Get2dLinesInMetrics(List<SlicePolyLine3D> polyLines3D)
        {
            var lst = new List<Line3D>();
            foreach (var polyLine3D in polyLines3D)
            {
                if (polyLine3D.Points[0].X == polyLine3D.Points[1].X && polyLine3D.Points[0].Y == polyLine3D.Points[1].Y)
                {
                }
                else
                {
                    var ln = new Line3D(new Vector3Class((polyLine3D.Points[0].X), (polyLine3D.Points[0].Y), 0),  //point1
                        new Vector3Class((polyLine3D.Points[1].X), (polyLine3D.Points[1].Y), 0), //point2
                        polyLine3D.Normal); //normal
                    ln.TriangleConnection = polyLine3D.TriangleConnection;
                    lst.Add(ln);
                }
            }

            return lst; // return the list
        }

        internal static Vector2 GetPixel2DLinesVector()
        {
            int hxres = RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionX / 2;
            int hyres = RenderEngine.PrintJob.SelectedPrinter.ProjectorResolutionY / 2;

            return new Vector2(hxres + 1, hyres + 1);
        }

        #endregion
    }
}