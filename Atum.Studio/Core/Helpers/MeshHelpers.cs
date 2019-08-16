using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Atum.Studio.Core.Helpers
{
    class MeshHelpers
    {
        internal static List<MeshOverhangTriangleIntersection> FindTrianglesOnSliceLine(List<Triangle> mesh, MeshOverhangSliceInfo overhangSlice)
        {
            if (overhangSlice.OuterPathSourcePoint == new Vector3(-17.0105343f, -14.94091f, 0.332000017f))
            {
                
            }

            //create intersection plate to determine intersecting triangles
            var intersectionPlanes = new TriangleInfoList();
            intersectionPlanes.Add(new List<Triangle>());

            var firstPlaneTriangle = new Triangle();
            firstPlaneTriangle.Vectors[0].Position = overhangSlice.OuterPathSourcePoint;
            firstPlaneTriangle.Vectors[1].Position = new Vector3(overhangSlice.OuterPathSourcePoint.X, overhangSlice.OuterPathSourcePoint.Y, overhangSlice.OuterPathIntersectionPoint.Z);
            firstPlaneTriangle.Vectors[2].Position = overhangSlice.OuterPathIntersectionPoint + (overhangSlice.OuterPathSourcePointEdgeNormal * 10);
            firstPlaneTriangle.CalcNormal();
            firstPlaneTriangle.CalcMinMaxX();
            firstPlaneTriangle.CalcMinMaxY();
            firstPlaneTriangle.CalcMinMaxZ();

            intersectionPlanes[0].Add(firstPlaneTriangle);

            var secondPlaneTriangle = new Triangle();
            secondPlaneTriangle = new Triangle();
            secondPlaneTriangle.Vectors[0].Position = new Vector3(overhangSlice.OuterPathSourcePoint.X, overhangSlice.OuterPathSourcePoint.Y, overhangSlice.OuterPathIntersectionPoint.Z);
            secondPlaneTriangle.Vectors[1].Position = overhangSlice.OuterPathSourcePoint;
            secondPlaneTriangle.Vectors[2].Position = overhangSlice.OuterPathIntersectionPoint + (overhangSlice.OuterPathSourcePointEdgeNormal * 10);
            secondPlaneTriangle.CalcNormal();
            secondPlaneTriangle.CalcMinMaxX();
            secondPlaneTriangle.CalcMinMaxY();
            secondPlaneTriangle.CalcMinMaxZ();

            intersectionPlanes[0].Add(secondPlaneTriangle);

            //result.AddRange(intersectionTriangles);
            var intersectedTriangles = new List<MeshOverhangTriangleIntersection>();


            //find all triangles that intersect with intersection plate

            foreach (var triangle in mesh)
            {
                var intersectionWithPlane = new MeshOverhangTriangleIntersection();




                var triangleIntersectionsCount = 0;
                for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                {
                    var lineStartPoint = triangle.Vectors[vectorIndex].Position;
                    var lineEndPoint = triangle.Vectors[0].Position;

                    if (vectorIndex < 2)
                    {
                        lineEndPoint = triangle.Vectors[vectorIndex + 1].Position;
                    }

                    if (lineStartPoint == new Vector3(-12.507534f, -9.86491f, 5.96000051f) || lineEndPoint == new Vector3(-12.507534f, -9.86491f, 5.96000051f))
                    {

                    }


                    if (lineStartPoint == overhangSlice.OuterPathSourcePoint)
                    {
                        intersectionWithPlane.Vectors = triangle.Vectors;
                        if (intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint == Vector3.Zero)
                        {
                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPointLineStart = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPointLineEnd = lineEndPoint;
                        }
                        else if (intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint == Vector3.Zero && intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint != lineStartPoint)
                        {
                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineStart = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineEnd = lineEndPoint;
                        }
                        triangleIntersectionsCount++;
                    }
                    else if (lineStartPoint == overhangSlice.OuterPathIntersectionPoint)
                    {
                        intersectionWithPlane.Vectors = triangle.Vectors;
                        if (intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint == Vector3.Zero)
                        {
                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPointLineStart = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPointLineEnd = lineEndPoint;
                        }
                        else if (intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint == Vector3.Zero && intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint != lineStartPoint)
                        {
                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineStart = lineStartPoint;
                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineEnd = lineEndPoint;
                        }

                        triangleIntersectionsCount++;
                    }
                    else
                    {

                        var intersectionPoints = Utils.IntersectionProvider.IntersectTriangles(lineStartPoint, lineEndPoint - lineStartPoint, intersectionPlanes, Utils.IntersectionProvider.typeDirection.OneWay);
                        if (intersectionPoints.Count > 0)
                        {
                            foreach (var intersectionPoint in intersectionPoints)
                            {
                                var validIntersectionPoint = false;
                                if (lineEndPoint.Z >= lineStartPoint.Z && intersectionPoint.Z >= lineStartPoint.Z && intersectionPoint.Z <= lineEndPoint.Z)
                                {
                                    validIntersectionPoint = true;
                                }
                                else if (lineEndPoint.Z <= lineStartPoint.Z && intersectionPoint.Z <= lineStartPoint.Z && intersectionPoint.Z >= lineEndPoint.Z)
                                {
                                    validIntersectionPoint = true;
                                }

                                if (validIntersectionPoint && intersectionPoint != Vector3.Zero)
                                {
                                    var currentLineLength = (lineEndPoint - lineStartPoint).Length;
                                    var intersectionPointStartLength = (intersectionPoint - lineStartPoint).Length;
                                    var intersectionPointEndLength = (intersectionPoint - lineEndPoint).Length;

                                    if (currentLineLength >= intersectionPointStartLength && currentLineLength >= intersectionPointEndLength)
                                    {
                                        intersectionWithPlane.Vectors = triangle.Vectors;
                                        if (intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint == Vector3.Zero)
                                        {
                                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint = intersectionPoint;
                                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPointLineStart = lineStartPoint;
                                            intersectionWithPlane.IntersectionPoint1Properties.IntersectionPointLineEnd = lineEndPoint;
                                        }
                                        else if (intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint == Vector3.Zero && intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint != intersectionPoint)
                                        {
                                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint = intersectionPoint;
                                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineStart = lineStartPoint;
                                            intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineEnd = lineEndPoint;
                                        }
                                        else
                                        {

                                            //override intersectionpoint2 if previous points are sub same
                                            var intersectionPoint1 = new Vector3((int)(intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint.X * 1000), (int)(intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint.Y * 1000), (int)(intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint.Z * 1000));
                                            var intersectionPoint2 = new Vector3((int)(intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint.X * 1000), (int)(intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint.Y * 1000), (int)(intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint.Z * 1000));

                                            if (intersectionPoint1 == intersectionPoint2)
                                            {
                                                intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint = intersectionPoint;
                                                intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineStart = lineStartPoint;
                                                intersectionWithPlane.IntersectionPoint2Properties.IntersectionPointLineEnd = lineEndPoint;
                                            }
                                        }

                                        triangleIntersectionsCount++;
                                        //break;
                                    }
                                }
                            }
                        }
                    }
                }

                //only save the triangles which contains 2 intersectionpoints
                if (triangleIntersectionsCount >= 2 && intersectionWithPlane.IntersectionPoint2Properties.IntersectionPoint != Vector3.Zero && intersectionWithPlane.IntersectionPoint1Properties.IntersectionPoint != Vector3.Zero)
                {
                    intersectedTriangles.Add(intersectionWithPlane);
                    overhangSlice.InnerIntersectedTriangesOrg.Add(intersectionWithPlane);
                }
            }



            

            //clean up intersected triangle which have same intersection points
            for (var intersectedTriangleIndex = intersectedTriangles.Count - 1; intersectedTriangleIndex >= 0; intersectedTriangleIndex--)
            {
                var intersectedTriangle = intersectedTriangles[intersectedTriangleIndex];
                var intersectedTrianglePoint1Simplified = new Vector3((int)(intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.X * 10000), (int)(intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Y * 10000), (int)(intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Z * 10000));
                var intersectedTrianglePoint2Simplified = new Vector3((int)(intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.X * 10000), (int)(intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Y * 10000), (int)(intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Z * 10000));
                if (intersectedTrianglePoint1Simplified == intersectedTrianglePoint2Simplified)
                {
                    if ((intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint == overhangSlice.OuterPathSourcePoint || intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint == overhangSlice.OuterPathSourcePoint))
                    {

                    }
                    else
                    {
                        intersectedTriangles.RemoveAt(intersectedTriangleIndex);
                    }

                }
                else if (intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Z <= overhangSlice.OuterPathSourcePoint.Z && intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Z <= overhangSlice.OuterPathSourcePoint.Z)
                {
                    intersectedTriangles.RemoveAt(intersectedTriangleIndex);
                }
            }

            //sort triangles by connected points (down -> up)
            var currentPoint = overhangSlice.OuterPathSourcePoint;
            var connectedTriangles = new List<MeshOverhangTriangleIntersection>();
            var sortedIntersectedTriangles = new List<MeshOverhangTriangleIntersection>();


            //find edges that are connected to lowest point
            var connectedIntersectionPointFound = true;

            for (var intersectedTriangleIndex = intersectedTriangles.Count - 1; intersectedTriangleIndex >= 0; intersectedTriangleIndex--)
            {
                var intersectedTriangle = intersectedTriangles[intersectedTriangleIndex];
                if (intersectedTriangle.Points[0] == currentPoint ||
                    intersectedTriangle.Points[1] == currentPoint ||
                    intersectedTriangle.Points[2] == currentPoint)
                {
                    if (intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint != overhangSlice.OuterPathSourcePoint &&
                        intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint != overhangSlice.OuterPathIntersectionPoint)
                    {
                        if (intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Z < intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Z)
                        {
                            intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint = overhangSlice.OuterPathSourcePoint;
                        }
                        else if (intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Z > intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Z)
                        {
                            intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint = overhangSlice.OuterPathSourcePoint;
                        }
                    }

                    intersectedTriangles.RemoveAt(intersectedTriangleIndex);
                    sortedIntersectedTriangles.Add(intersectedTriangle);

                    connectedTriangles.Add(intersectedTriangle);
                }
            }


            //when multiple found find the one that is ONLY connected to the outerpath
            if (sortedIntersectedTriangles.Count > 1)
            {
                var currentSourcePointDistance = 0f;
                var longestDistanceIntersectedTriangle = sortedIntersectedTriangles[0];

                foreach (var sortedIntersectedTriangle in sortedIntersectedTriangles)
                {
                    if (sortedIntersectedTriangle.IntersectionPoint1Properties.IntersectionPoint == overhangSlice.OuterPathSourcePoint)
                    {
                        var newSourcePointDistance = (sortedIntersectedTriangle.IntersectionPoint2Properties.IntersectionPoint - overhangSlice.OuterPathSourcePoint).Length;
                        if (newSourcePointDistance > currentSourcePointDistance)
                        {
                            currentSourcePointDistance = newSourcePointDistance;
                            longestDistanceIntersectedTriangle = sortedIntersectedTriangle;
                        }
                    }
                    if (sortedIntersectedTriangle.IntersectionPoint2Properties.IntersectionPoint == overhangSlice.OuterPathSourcePoint)
                    {
                        var newSourcePointDistance = (sortedIntersectedTriangle.IntersectionPoint1Properties.IntersectionPoint - overhangSlice.OuterPathSourcePoint).Length;

                        if (newSourcePointDistance > currentSourcePointDistance)
                        {
                            currentSourcePointDistance = newSourcePointDistance;
                            longestDistanceIntersectedTriangle = sortedIntersectedTriangle;
                        }
                    }
                }

                sortedIntersectedTriangles.Clear();
                sortedIntersectedTriangles.Add(longestDistanceIntersectedTriangle);
            }

            while (connectedIntersectionPointFound && intersectedTriangles.Count > 0)
            {
                //find intersectedTriangles which contains 2 connected points
                connectedIntersectionPointFound = false;
                var newConnectedTriangles = new List<MeshOverhangTriangleIntersection>();
                for (var intersectedTriangleIndex = intersectedTriangles.Count - 1; intersectedTriangleIndex >= 0; intersectedTriangleIndex--)
                {
                    var intersectedTriangle = intersectedTriangles[intersectedTriangleIndex];
                    foreach (var previousIntersectedTriangle in connectedTriangles)
                    {
                        var connectedPointsCount = 0;

                        foreach (var previousVectorPoint in previousIntersectedTriangle.Points)
                        {
                            foreach (var vectorPoint in intersectedTriangle.Points)
                            {
                                if (vectorPoint == previousVectorPoint)
                                {
                                    connectedPointsCount++;
                                    break;
                                }
                            }
                        }

                        if (connectedPointsCount == 2)
                        {
                            intersectedTriangles.RemoveAt(intersectedTriangleIndex);
                            sortedIntersectedTriangles.Add(intersectedTriangle);
                            newConnectedTriangles.Add(intersectedTriangle);
                            break;
                        }
                    }

                }

                if (newConnectedTriangles.Count > 0)
                {
                    connectedIntersectionPointFound = true;
                    connectedTriangles = newConnectedTriangles;
                }
            }

            //remove triangles which contain same intersections
            for (var intersectedTriangleIndex = sortedIntersectedTriangles.Count - 1; intersectedTriangleIndex >= 0; intersectedTriangleIndex--)
            {
                var intersectedTriangle = sortedIntersectedTriangles[intersectedTriangleIndex];
                var intersectedTrianglePoint1Simplified = new Vector3((int)(intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.X * 1000), (int)(intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Y * 1000), (int)(intersectedTriangle.IntersectionPoint1Properties.IntersectionPoint.Z * 1000));
                var intersectedTrianglePoint2Simplified = new Vector3((int)(intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.X * 1000), (int)(intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Y * 1000), (int)(intersectedTriangle.IntersectionPoint2Properties.IntersectionPoint.Z * 1000));
                if (intersectedTrianglePoint1Simplified == intersectedTrianglePoint2Simplified)
                {
                    sortedIntersectedTriangles.RemoveAt(intersectedTriangleIndex);
                }
            }

            overhangSlice.InnerIntersectedTrianges = sortedIntersectedTriangles;

            return sortedIntersectedTriangles;

        }

   
    }
}
