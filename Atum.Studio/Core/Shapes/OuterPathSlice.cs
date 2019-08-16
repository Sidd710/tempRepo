using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Shapes
{
    internal class MeshOverhangTriangleIntersectionProperties
    {
        internal Vector3 IntersectionPoint { get; set; }
        internal Vector3 IntersectionPointLineStart { get; set; }
        internal Vector3 IntersectionPointLineEnd { get; set; }

    }


    internal class MeshOverhangTriangleIntersection : Triangle
    {
        internal MeshOverhangTriangleIntersectionProperties IntersectionPoint1Properties { get; set; }
        internal MeshOverhangTriangleIntersectionProperties IntersectionPoint2Properties { get; set; }

        internal Triangle TriangleObject
        {
            get
            {
                var triangle = new Triangle()
                {
                    Vectors = this.Vectors,
                };

                triangle.CalcMinMaxX();
                triangle.CalcMinMaxY();
                triangle.CalcMinMaxZ();
                triangle.CalcNormal();
                triangle.CalcCenter();
                return triangle;
            }
        }

        internal MeshOverhangTriangleIntersection()
        {
            this.IntersectionPoint1Properties = new MeshOverhangTriangleIntersectionProperties();
            this.IntersectionPoint2Properties = new MeshOverhangTriangleIntersectionProperties();
        }
    }

    internal class MeshOverhangSliceInfo
    {
        internal Vector3Class OuterPathIntersectionPoint { get; set; }
        internal Vector3Class OuterPathSourcePoint { get; set; }
        internal OuterPathNormal OuterPathIntersectionPointProperties { get; set; }

        internal List<List<IntPoint>> OuterPathContourSegmentVectors { get; set; }
        internal TriangleConnectionInfo OuterPathContourSmallestContourTriangleConnectionIndex { get; set; }

        internal Vector3Class OuterPathSourcePointEdgeNormal { get; set; }
        internal List<MeshOverhangTriangleIntersection> InnerIntersectedTrianges { get; set; }
        internal List<MeshOverhangTriangleIntersection> InnerIntersectedTriangesOrg { get; set; }
        internal List<MeshOverhangTriangleIntersection> InnerIntersectedSplitTrianges { get; set; }
        
        internal bool HasValidOuterPathIntersectionPoint
        {
            get
            {
                if (this.OuterPathIntersectionPointProperties == null)
                {
                    return false;
                }
                else if (this.OuterPathIntersectionPointProperties.EdgeNormal.Z > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }

        internal TriangleInfoList OuterPathIntersectionPlaneTriangles { get; set; }

        internal MeshOverhangSliceInfo()
        {
            this.OuterPathContourSegmentVectors = new List<List<IntPoint>>();
            this.OuterPathIntersectionPlaneTriangles = new TriangleInfoList();
            this.InnerIntersectedSplitTrianges = new List<MeshOverhangTriangleIntersection>();
            this.InnerIntersectedTriangesOrg = new List<MeshOverhangTriangleIntersection>();
        }

        internal void CalcOuterPathSegments(PolyNode outerPathAsPolyNode, Dictionary<Vector3, List<TriangleConnectionInfo>> outerPathUnsorted)
        {
            //find sourcepoint on outer path
            var sourcePointAsPolyNodeContourPoint = new IntPoint((int)(this.OuterPathSourcePoint.X * 10000), (int)(this.OuterPathSourcePoint.Y * 10000), (int)(this.OuterPathSourcePoint.Z * 10000));

            var sourcePointStartIndex = 0;
            for(var contourPointIndex = 0; contourPointIndex < outerPathAsPolyNode.Contour.Count; contourPointIndex++)
            {
                if (outerPathAsPolyNode.Contour[contourPointIndex] == sourcePointAsPolyNodeContourPoint)
                {
                    sourcePointStartIndex = contourPointIndex;
                    break;
                }
            }

            //create order list starting from startindex
            var polyContourWithStartIndex = new List<IntPoint>();
            for (var contourPointIndex = sourcePointStartIndex; contourPointIndex < outerPathAsPolyNode.Contour.Count; contourPointIndex++)
            {
                polyContourWithStartIndex.Add(outerPathAsPolyNode.Contour[contourPointIndex]);
            }
            for (var contourPointIndex = 0; contourPointIndex < sourcePointStartIndex; contourPointIndex++)
            {
                polyContourWithStartIndex.Add(outerPathAsPolyNode.Contour[contourPointIndex]);
            }

            //find intersectionpoint index
            var intersectionPointAsPolyNodeContourPoint = new IntPoint((int)(this.OuterPathIntersectionPointProperties.EdgeStartPoint.X * 10000), (int)(this.OuterPathIntersectionPointProperties.EdgeStartPoint.Y * 10000), (int)(this.OuterPathIntersectionPointProperties.EdgeStartPoint.Z * 10000));

            var segmentContourPoints = new List<IntPoint>();
            for (var contourPointIndex = 0; contourPointIndex < outerPathAsPolyNode.Contour.Count; contourPointIndex++)
            {
                segmentContourPoints.Add(outerPathAsPolyNode.Contour[contourPointIndex]);

                if (outerPathAsPolyNode.Contour[contourPointIndex] == intersectionPointAsPolyNodeContourPoint)
                {
                    this.OuterPathContourSegmentVectors.Add(segmentContourPoints);
                    segmentContourPoints = new List<IntPoint>();
                }
            }

            this.OuterPathContourSegmentVectors.Add(segmentContourPoints);

            var segmentStartBasedOnLowestOuterPathSegments = this.OuterPathContourSegmentVectors[0][0];
            if (this.OuterPathContourSegmentVectors[1].Count < this.OuterPathContourSegmentVectors[0].Count)
            {
                segmentStartBasedOnLowestOuterPathSegments = this.OuterPathContourSegmentVectors[1][0];
            }

            //find triangle based on intpoint
            var outerPathContourTriangleConnectionInfo = new TriangleConnectionInfo();
            var outerPathUnsortedKeys = new List<Vector3>();
            outerPathUnsortedKeys.AddRange(outerPathUnsorted.Keys);

            for (var outerPathUnsortedIndex = 0; outerPathUnsortedIndex < outerPathUnsorted.Count; outerPathUnsortedIndex++)
            {
                var outerPathKey = outerPathUnsortedKeys[outerPathUnsortedIndex];

                var outerPathContourPoint = new IntPoint((int)(outerPathKey.X * 10000), (int)(outerPathKey.Y * 10000), (int)(outerPathKey.Z * 10000));
                if (outerPathContourPoint == segmentStartBasedOnLowestOuterPathSegments)
                {
                    //get next
                    outerPathKey = outerPathUnsortedKeys[outerPathUnsortedIndex + 2];
                    var outerPathValues = outerPathUnsorted[outerPathKey];
                    outerPathContourTriangleConnectionInfo = outerPathValues[0];
                    break;
                }
            }

            this.OuterPathContourSmallestContourTriangleConnectionIndex = outerPathContourTriangleConnectionInfo;
        }

        internal void CreateIntersectionPlanes()
        {
            var firstTriangle = new Triangle();
            firstTriangle.Vectors[0].Position = this.OuterPathSourcePoint;
            firstTriangle.Vectors[1].Position = this.OuterPathSourcePoint + new Vector3Class(0, 0, 10);
            firstTriangle.Vectors[2].Position = this.OuterPathSourcePoint + new Vector3Class(0, 0, 10) + (OuterPathSourcePointEdgeNormal * 10);
            firstTriangle.CalcNormal();
            firstTriangle.CalcMinMaxX();
            firstTriangle.CalcMinMaxY();
            firstTriangle.CalcMinMaxZ();

            this.OuterPathIntersectionPlaneTriangles[0].Add(firstTriangle);

            firstTriangle = new Triangle();
            firstTriangle.Vectors[0].Position = this.OuterPathSourcePoint + new Vector3Class(0, 0, 10);
            firstTriangle.Vectors[1].Position = this.OuterPathSourcePoint;
            firstTriangle.Vectors[2].Position = this.OuterPathSourcePoint + new Vector3Class(0, 0, 10) + (OuterPathSourcePointEdgeNormal * 10);
            firstTriangle.CalcNormal();
            firstTriangle.CalcMinMaxX();
            firstTriangle.CalcMinMaxY();
            firstTriangle.CalcMinMaxZ();

            this.OuterPathIntersectionPlaneTriangles[0].Add(firstTriangle);
        }

       
        internal void CalcNearestIntersectionPoints(List<OuterPathNormal> outerPath)
        {
            var currentIntersectionPointDistance = float.MaxValue;

            foreach (var outerPathEdge in outerPath)
            {
                if (outerPathEdge.FirstPoint != this.OuterPathSourcePoint && outerPathEdge.SecondPoint != this.OuterPathSourcePoint) //exclude current lines
                {
                    var outerPathEdgeVector = (outerPathEdge.SecondPoint - outerPathEdge.FirstPoint);
                    outerPathEdgeVector.Normalize();
                    var outerPathEdgeVectorSimplified = new Vector3((int)(outerPathEdgeVector.X * 1000), ((int)(outerPathEdgeVector.Y * 1000)), ((int)(outerPathEdgeVector.Z * 1000)));
                    var intersectionPoints = Utils.IntersectionProvider.IntersectTriangles(outerPathEdge.FirstPoint, outerPathEdgeVector, this.OuterPathIntersectionPlaneTriangles, Utils.IntersectionProvider.typeDirection.OneWay);
                    if (intersectionPoints.Count > 0)
                    {
                        foreach (var intersectionPoint in intersectionPoints)
                        {
                            if (intersectionPoint != Vector3Class.Zero)
                            {
                                //check if intersectionpoint is on current triangle
                                var currentTriangle = outerPathEdge.EdgeTriangle;
                                var intersectionPointVector = intersectionPoint - outerPathEdge.FirstPoint;
                                intersectionPointVector.Normalize();
                                var intersectionPointVectorSimplified = new Vector3((int)(intersectionPointVector.X * 1000), ((int)(intersectionPointVector.Y * 1000)), ((int)(intersectionPointVector.Z * 1000)));

                                var intersectionPointVectorLength = (intersectionPoint - outerPathEdge.FirstPoint).Length;

                                //a.Add(outerPathEdge.FirstPoint);
                                // b.Add(outerPathEdge.SecondPoint);

                                if (intersectionPointVectorSimplified == outerPathEdgeVectorSimplified)
                                {
                                    //determine scale factor
                                    var outerPathEdgeVectorLength = (outerPathEdge.SecondPoint - outerPathEdge.FirstPoint).Length;

                                    //check if vector is on outerpath edge

                                    if (intersectionPointVectorLength <= outerPathEdgeVectorLength)
                                    {

                                        var outerPathEdgeVectorOffsetPercentage = intersectionPointVectorLength / outerPathEdgeVectorLength;
                                        if (outerPathEdgeVectorOffsetPercentage > 1f)
                                        {
                                            outerPathEdgeVectorOffsetPercentage = 1f;
                                        }
                                        var newIntersectionPointVector = ((outerPathEdge.SecondPoint - outerPathEdge.FirstPoint) * outerPathEdgeVectorOffsetPercentage);

                                        //determine distance of sourcepoint
                                        var sourcePointVector = intersectionPoint - this.OuterPathSourcePoint;
                                        if (sourcePointVector.Length < currentIntersectionPointDistance)
                                        {
                                            currentIntersectionPointDistance = sourcePointVector.Length;
                                            this.OuterPathIntersectionPoint = outerPathEdge.FirstPoint + newIntersectionPointVector;
                                            this.OuterPathIntersectionPointProperties = outerPathEdge;
                                            this.OuterPathIntersectionPointProperties.EdgeStartPoint = outerPathEdge.FirstPoint;
                                            this.OuterPathIntersectionPointProperties.EdgeEndPoint = outerPathEdge.SecondPoint;
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

    internal class OuterPathSlice
    {
        internal List<MeshOverhangSliceInfo> MeshIntersections { get; set; }

        internal OuterPathSlice()
        {
            this.MeshIntersections = new List<MeshOverhangSliceInfo>();
        }

        internal void CalcNearestIntersectionPoints(List<OuterPathNormal> outerPath)
        {
            var currentIntersectionPointDistance = float.MaxValue;

            foreach (var outerPathEdge in outerPath)
            {
                foreach (var sliceIntersectionSourcePoint in MeshIntersections)
                {
                    if (outerPathEdge.FirstPoint != sliceIntersectionSourcePoint.OuterPathSourcePoint && outerPathEdge.SecondPoint != sliceIntersectionSourcePoint.OuterPathSourcePoint) //exclude current lines
                    {
                        var outerPathEdgeVector = (outerPathEdge.SecondPoint - outerPathEdge.FirstPoint);
                        outerPathEdgeVector.Normalize();
                        var outerPathEdgeVectorSimplified = new Vector3Class((int)(outerPathEdgeVector.X * 1000), ((int)(outerPathEdgeVector.Y * 1000)), ((int)(outerPathEdgeVector.Z * 1000)));
                        var intersectionPoints = Utils.IntersectionProvider.IntersectTriangles(outerPathEdge.FirstPoint, outerPathEdgeVector, sliceIntersectionSourcePoint.OuterPathIntersectionPlaneTriangles, Utils.IntersectionProvider.typeDirection.OneWay);
                        if (intersectionPoints.Count > 0)
                        {
                            foreach (var intersectionPoint in intersectionPoints)
                            {
                                if (intersectionPoint != Vector3Class.Zero)
                                {
                                    //check if intersectionpoint is on current triangle
                                    var currentTriangle = outerPathEdge.EdgeTriangle;
                                    var intersectionPointVector = intersectionPoint - outerPathEdge.FirstPoint;
                                    intersectionPointVector.Normalize();
                                    var intersectionPointVectorSimplified = new Vector3Class((int)(intersectionPointVector.X * 1000), ((int)(intersectionPointVector.Y * 1000)), ((int)(intersectionPointVector.Z * 1000)));

                                    var intersectionPointVectorLength = (intersectionPoint - outerPathEdge.FirstPoint).Length;

                                    if (intersectionPointVectorSimplified == outerPathEdgeVectorSimplified)
                                    {
                                        //determine scale factor
                                        var outerPathEdgeVectorLength = (outerPathEdge.SecondPoint - outerPathEdge.FirstPoint).Length;

                                        //check if vector is on outerpath edge

                                        if (intersectionPointVectorLength <= outerPathEdgeVectorLength)
                                        {

                                            var outerPathEdgeVectorOffsetPercentage = intersectionPointVectorLength / outerPathEdgeVectorLength;
                                            if (outerPathEdgeVectorOffsetPercentage > 1f)
                                            {
                                                outerPathEdgeVectorOffsetPercentage = 1f;
                                            }
                                            var newIntersectionPointVector = ((outerPathEdge.SecondPoint - outerPathEdge.FirstPoint) * outerPathEdgeVectorOffsetPercentage);

                                            //determine distance of sourcepoint
                                            var sourcePointVector = intersectionPoint - sliceIntersectionSourcePoint.OuterPathSourcePoint;
                                            if (sourcePointVector.Length < currentIntersectionPointDistance)
                                            {
                                                currentIntersectionPointDistance = sourcePointVector.Length;
                                                sliceIntersectionSourcePoint.OuterPathIntersectionPoint = outerPathEdge.FirstPoint + newIntersectionPointVector;
                                                sliceIntersectionSourcePoint.OuterPathIntersectionPointProperties = outerPathEdge;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (sliceIntersectionSourcePoint.OuterPathIntersectionPoint == Vector3Class.Zero)
                    {

                    }
                }
            }
        }
    }
}
