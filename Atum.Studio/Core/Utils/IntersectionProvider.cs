using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Structs;
using OpenTK;
using System.Threading;
using System.Collections.Generic;
using System;
using Atum.Studio.Core.Engines;
using System.Threading.Tasks;
using Atum.Studio.Core.Managers;
using System.Diagnostics;
using Atum.Studio.Core.ModelView;
using System.Collections.Concurrent;

namespace Atum.Studio.Core.Utils
{
    public class IntersectionState
    {
        public EventWaitHandle EventWaitHandle = new ManualResetEvent(false);
        public Vector3Class Origin { get; set; }
        public STLModel3D SurfaceTrianglesAsStl { get; set; }
        public TriangleIntersection ModelIntersection;
    }

    internal class IntersectionProvider
    {
        internal enum typeDirection
        {
            OneWay,
            TwoWay
        }

        public static TriangleInfoList InsersectTriangleByCenterPoint(Vector3Class center, Vector3Class centerNormal, object model3d)
        {
            var intersectedTriangles = new TriangleInfoList();

            if (model3d is TriangleInfoList)
            {
                var triangles = (TriangleInfoList)model3d;
                for (var triangleIndex = 0; triangleIndex < triangles.Count; triangleIndex++)
                {
                    foreach (var triangle in triangles[triangleIndex])
                    {
                        var p1 = new Vector3Class(triangle.Vectors[0].Position.X, triangle.Vectors[0].Position.Y, 0);
                        var p2 = new Vector3Class(triangle.Vectors[1].Position.X, triangle.Vectors[1].Position.Y, 0);
                        var p3 = new Vector3Class(triangle.Vectors[2].Position.X, triangle.Vectors[2].Position.Y, 0);

                        var movedTriangle = new TriangleIntersection();
                        movedTriangle.Vectors[0].Position = p1;
                        movedTriangle.Vectors[1].Position = p2;
                        movedTriangle.Vectors[2].Position = p3;

                        if (PointInsideTriangle(new Vector3Class(center.X, center.Y, 0), movedTriangle))
                        {
                            movedTriangle.Vectors[0].Position.Z = triangle.Vectors[0].Position.Z;
                            movedTriangle.Vectors[1].Position.Z = triangle.Vectors[1].Position.Z;
                            movedTriangle.Vectors[2].Position.Z = triangle.Vectors[2].Position.Z;
                            movedTriangle.CalcCenter();
                            movedTriangle.CalcNormal();
                            movedTriangle.Flip();

                            //if (!directionUP) { movedTriangle.Flip(); }

                            var edge1 = movedTriangle.Vectors[1].Position - movedTriangle.Vectors[0].Position;
                            var edge2 = movedTriangle.Vectors[2].Position - movedTriangle.Vectors[0].Position;
                            var originNormal = Vector3Class.Cross(centerNormal, edge2);
                            float det = Vector3Class.Dot(edge1, originNormal); //determined of matrix
                            if (det == 0) break;
                            float invDet = 1 / det; //invert matrix
                            var s = center - movedTriangle.Vectors[0].Position;

                            var u = Vector3Class.Dot(s, originNormal) * invDet; //barycentric coordinates
                            if (u < 0 || u > 1) break;
                            var qvec = Vector3Class.Cross(s, edge1);
                            var v = Vector3Class.Dot(centerNormal, qvec) * invDet; //barycentric coordinates
                            if (v < 0 || u + v > 1) break;
                            var t = Vector3Class.Dot(edge2, qvec) * invDet; //distince of intersection point

                            movedTriangle.IntersectionPoint.Z = t;

                            intersectedTriangles[0].Add(movedTriangle);

                        }
                    }
                }
            }

            return intersectedTriangles;
        }

        internal static bool PointInsideTriangle(Vector3Class point, Triangle triangle)
        {
            int j = 2;
            var trianglePoints = triangle.Points;
            for (int i = 0; i < trianglePoints.Length; j = i, i++)
            {
                var v0 = new Vector3Class(trianglePoints[j].X, trianglePoints[j].Y, 0);
                var v1 = new Vector3Class(trianglePoints[i].X, trianglePoints[i].Y, 0);

                var edgeDir = v1 - v0;
                var edgeNormal = new Vector3Class(-edgeDir.Y, edgeDir.X, 0);
                float sign = Vector3Class.Dot(point - v0, edgeNormal);

                if (sign > 0f)
                    return false;
            }
            return true;
        }

        public static bool IntersectTriangle(Vector3Class origin, Vector3Class direction, object model3d, typeDirection directionType, bool useSupportMoveTranslation, Vector3Class moveTranslation, out TriangleIntersection[] intersectedPolygons)
        {
            var intersections = 0;
            intersectedPolygons = null;
            var intersections2 = new ConcurrentBag<TriangleIntersection>();
            if (model3d is STLModel3D)
            {
                var stlModel = (STLModel3D)model3d;

                if (stlModel.Triangles != null)
                {
                     for (var arrayIndex = 0; arrayIndex < stlModel.Triangles.Count; arrayIndex++)
                    //Parallel.For(0, stlModel.Triangles.Count, arrayIndexAsync =>
                        {
                    //        var arrayIndex = arrayIndexAsync;
                            for (var triangleIndex = 0; triangleIndex < stlModel.Triangles[arrayIndex].Count; triangleIndex++)
                            {
                                var triangle = stlModel.Triangles[arrayIndex][triangleIndex];

                                if (triangle.Center != origin)
                                {
                                    if (direction.Z == 1 || direction.Z == -1)
                                    {
                                        if (!((triangle.Left <= origin.X && triangle.Right >= origin.X) && (triangle.Back <= origin.Y && triangle.Front >= origin.Y)))
                                        {
                                            continue;
                                        }
                                    }

                                    //do translation off vector
                                    if (triangleIndex == 8)
                                {

                                }

                                    float nDotD = Vector3Class.Dot(triangle.Normal, direction);
                                    if (nDotD != 0)
                                    {
                                        if (nDotD < 0 || directionType == typeDirection.TwoWay)
                                        {
                                            var p1 = triangle.Vectors[0].Position + moveTranslation;

                                            float d = Vector3Class.Dot(triangle.Normal, p1);
                                            float t = -(Vector3Class.Dot(triangle.Normal, origin) + d) / nDotD;
                                            Vector3Class planeIntersection = new Vector3Class();

                                            if (nDotD < 0)
                                            {
                                                t = -(Vector3Class.Dot(-triangle.Normal, origin) + d) / -nDotD;
                                                planeIntersection = origin + (direction * t);
                                            }
                                            else if (directionType == typeDirection.TwoWay)
                                            {
                                                planeIntersection = origin + (direction * t);
                                            }

                                            if (useSupportMoveTranslation) planeIntersection = planeIntersection - stlModel.MoveTranslation - moveTranslation;


                                            var p2 = triangle.Vectors[1].Position + moveTranslation;
                                            var p3 = triangle.Vectors[2].Position + moveTranslation;

                                            for (var i = 0; i < 2; i++)
                                            {
                                                //pass 1
                                                if (i == 0)
                                                {
                                                    if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, planeIntersection - p3), direction) > 0) continue;
                                                    if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, planeIntersection - p1), direction) > 0) continue;
                                                    if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, planeIntersection - p2), direction) > 0) continue;
                                                }
                                                else
                                                {
                                                    if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, planeIntersection - p3), direction) < 0) continue;
                                                    if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, planeIntersection - p1), direction) < 0) continue;
                                                    if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, planeIntersection - p2), direction) < 0) continue;
                                                }

                                                intersections2.Add(new TriangleIntersection(triangle, planeIntersection));

                                                intersections++;
                                            }
                                        }
                                    }
                                }
                            }

                        }//);
                }
            }

            //Console.WriteLine("IntersectTriangle: " + stopWatch.ElapsedMilliseconds + "ms");

            intersectedPolygons = intersections2.ToArray();
            return intersections > 0;

        }

        public static List<Vector3Class> IntersectTriangles(Vector3Class origin, Vector3Class direction, TriangleInfoList triangles, typeDirection directionType)
        {
            var intersectionPoints = new List<Vector3Class>();
            for (var arrayIndex = 0; arrayIndex < triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < triangles[arrayIndex].Count; triangleIndex++)
                {
                    var triangle = triangles[arrayIndex][triangleIndex];

                    if (triangle.Center != origin)
                    {
                        if (direction.Z == 1 || direction.Z == -1)
                        {
                            if (!((triangle.Left <= origin.X && triangle.Right >= origin.X) && (triangle.Back <= origin.Y && triangle.Front >= origin.Y)))
                            {
                                continue;
                            }
                        }

                        //do translation off vector
                        var p1 = triangle.Vectors[0].Position;
                        var p2 = triangle.Vectors[1].Position;
                        var p3 = triangle.Vectors[2].Position;

                        float nDotD = Vector3Class.Dot(triangle.Normal, direction);
                        if (nDotD != 0)
                        {
                            //    if (nDotD < 0)nDotD = 0 - nDotD;

                            float d = Vector3Class.Dot(triangle.Normal, p1);
                            float t = -(Vector3Class.Dot(triangle.Normal, origin) + d) / nDotD;
                            Vector3Class planeIntersection = new Vector3Class();
                            if (nDotD < 0)
                            {
                                t = -(Vector3Class.Dot(-triangle.Normal, origin) + d) / -nDotD;
                                planeIntersection = origin + (direction * t);
                            }
                            else if (directionType == typeDirection.TwoWay)
                            {
                                planeIntersection = origin + (direction * t);
                            }

                            for (var i = 0; i < 2; i++)
                            {
                                //pass 1
                                if (i == 0)
                                {
                                    if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, planeIntersection - p3), direction) > 0) continue;
                                    if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, planeIntersection - p1), direction) > 0) continue;
                                    if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, planeIntersection - p2), direction) > 0) continue;
                                }
                                else
                                {
                                    if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, planeIntersection - p3), direction) < 0) continue;
                                    if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, planeIntersection - p1), direction) < 0) continue;
                                    if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, planeIntersection - p2), direction) < 0) continue;
                                }

                                if (intersectionPoints == null) { intersectionPoints = new List<Vector3Class>(); }
                                intersectionPoints.Add(planeIntersection);

                            }
                        }
                    }
                }
            }


            return intersectionPoints;

        }

        public static Vector3Class IntersectTriangle(Vector3Class origin, Vector3Class direction, Triangle triangle, bool validateIntersectionPoint = true)
        {
            if (triangle.Center != origin)
            {
                //do translation off vector
                var p1 = triangle.Vectors[0].Position;
                var p2 = triangle.Vectors[1].Position;
                var p3 = triangle.Vectors[2].Position;

                float nDotD = Vector3Class.Dot(triangle.Normal, direction);
                if (nDotD != 0)
                {

                    float d = Vector3Class.Dot(triangle.Normal, p1);
                    float t = -(Vector3Class.Dot(triangle.Normal, origin) + d) / nDotD;
                    Vector3Class planeIntersection = new Vector3Class();
                    planeIntersection = origin + (direction * t);
                    if (nDotD < 0)
                    {
                        t = -(Vector3Class.Dot(-triangle.Normal, origin) + d) / -nDotD;
                        planeIntersection = origin + (direction * t);
                    }

                    if (!validateIntersectionPoint)
                    {
                        var trianglePoints = triangle.Points;
                        if (planeIntersection == trianglePoints[0] || planeIntersection == trianglePoints[1] || planeIntersection == trianglePoints[2])
                        {

                        }

                        return planeIntersection;
                    }
                    else
                    {
                        for (var i = 0; i < 2; i++)
                        {
                            //pass 1
                            if (i == 0)
                            {
                                if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, planeIntersection - p3), direction) > 0) continue;
                                if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, planeIntersection - p1), direction) > 0) continue;
                                if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, planeIntersection - p2), direction) > 0) continue;
                            }
                            else
                            {
                                if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, planeIntersection - p3), direction) < 0) continue;
                                if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, planeIntersection - p1), direction) < 0) continue;
                                if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, planeIntersection - p2), direction) < 0) continue;
                            }

                            return planeIntersection;
                        }
                    }
                }
            }
            return new Vector3Class();

        }

        internal static ISectData ISectGroundPlane(Vector3Class direction, Vector3Class origin)
        {
            ISectData isect = null;
            TriangleIntersection[] intersect = null;

            // var groundPanel = CreateGroundPlane();
            if (IntersectTriangle(origin, direction, ObjectView.GroundPaneInflated, typeDirection.OneWay, false, new Vector3Class(), out intersect))
            {
                if (intersect != null && intersect.Length > 0)
                {
                    isect = new ISectData(null, null, intersect[0].IntersectionPoint, origin, direction);
                }
            }

            return isect;
        }

        internal static ConcurrentDictionary<TriangleIntersection, bool> GetIntersectionPointsAsync(ConcurrentDictionary<TriangleIntersection, bool> originPoints, STLModel3D surfaceAsStl)
        {
            var result = new ConcurrentDictionary<TriangleIntersection, bool>();

            try
            {
                Parallel.ForEach(originPoints.Keys, originPointAsync =>
                {
                    var originPoint = originPointAsync;
                    var objectState = new IntersectionState();
                    objectState.EventWaitHandle = new ManualResetEvent(false);
                    objectState.Origin = originPoint.IntersectionPoint;
                    objectState.SurfaceTrianglesAsStl = surfaceAsStl;
                    GetIntersectionPointAsync((object)objectState);

                    if (objectState.ModelIntersection != null && objectState.ModelIntersection.IntersectionPoint != new Vector3Class())
                    {
                        result.TryAdd(objectState.ModelIntersection, false);
                    }
                });
            }
            catch (Exception exc)
            {

            }

            return result;
        }

        private static void GetIntersectionPointAsync(object state)
        {
            try
            {
                var objectState = state as IntersectionState;
                TriangleIntersection[] trianglesIntersected = null;
                IntersectTriangle(new Vector3Class(objectState.Origin.X, objectState.Origin.Y, 0), new Vector3Class(0, 0, 1), objectState.SurfaceTrianglesAsStl, typeDirection.OneWay, false, new Vector3Class(), out trianglesIntersected);
                if (trianglesIntersected != null && trianglesIntersected.Length > 0)
                {
                    objectState.ModelIntersection = trianglesIntersected[0];
                }

                objectState.EventWaitHandle.Set();
            }
            catch (Exception exc)
            {

            }
        }

    }
}
