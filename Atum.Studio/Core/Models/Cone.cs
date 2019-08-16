using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Models
{
    public class Cone : TriangleInfoList
    {
        public Vector3Class Normal { get; set; }

        public List<Vector3Class> TopPoints
        {
            get
            {
                var result = new List<Vector3Class>();
                for (var i = 0; i < 32; i += 2)
                {
                    result.Add(this[0][i].Vectors[2].Position);
                }
                return result;

            }
        }


        public List<Vector3Class> BottomPoints
        {
            get
            {
                var result = new List<Vector3Class>();
                for (var i = 1; i < 32; i += 2)
                {
                    result.Add(this[0][i].Vectors[2].Position);
                }

                return result;
            }
        }

        public Cone()
        {
        }

        public static Cone Create(float topRadius, float bottomRadius, float height, int slicesCount, Vector3Class positionOffset)
        {
            var coneTriangles = new Cone();
            coneTriangles.Normal = new Vector3Class(0, 0, 1);

            var topPoints = VectorHelper.CreateCircle(height, topRadius, slicesCount, false);
            var bottomPoints = VectorHelper.CreateCircle(0, bottomRadius, slicesCount, false);

            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < bottomPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                var leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = topPoints[middleRadiusPointIndex] + positionOffset;
                leftTriangle.Vectors[1].Position = bottomPoints[middleRadiusPointIndex] + positionOffset;

                if (middleRadiusPointIndex == bottomPoints.Count - 1)
                {
                    leftTriangle.Vectors[2].Position = bottomPoints[0] + positionOffset;
                }
                else
                {
                    leftTriangle.Vectors[2].Position = bottomPoints[middleRadiusPointIndex + 1] + positionOffset;
                }

                leftTriangle.CalcNormal();
                coneTriangles[0].Add(leftTriangle);

                //right triangle
                var rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = topPoints[middleRadiusPointIndex] + positionOffset;

                if (middleRadiusPointIndex == bottomPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = bottomPoints[0] + positionOffset;
                    rightTriangle.Vectors[2].Position = topPoints[0] + positionOffset;

                }
                else
                {
                    rightTriangle.Vectors[1].Position = bottomPoints[middleRadiusPointIndex + 1] + positionOffset;
                    rightTriangle.Vectors[2].Position = topPoints[middleRadiusPointIndex + 1] + positionOffset;
                }

                rightTriangle.CalcNormal();
                coneTriangles[0].Add(rightTriangle);
            }

            return coneTriangles;
        }

        public void Rotate(float angleY, float angleZ)
        {

            Matrix4 rotationMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(angleY));

            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = rotatedVector;
                    }
                }
            }

            this.Normal = Vector3Class.Transform(this.Normal, rotationMatrix);

            rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(angleZ));

            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = rotatedVector;
                        this[arrayIndex][triangleIndex].CalcNormal();
                    }
                }
            }

            this.Normal = Vector3Class.Transform(this.Normal, rotationMatrix);
        }

        public void Translate(Vector3Class translationVector)
        {
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += translationVector;
                    }
                }
            }

            //rotate bottom points
            for (var bottomPointIndex = 0; bottomPointIndex < this.BottomPoints.Count; bottomPointIndex++)
            {
                this.BottomPoints[bottomPointIndex] += translationVector;
            }

            //rotate top points
            for (var topPointIndex = 0; topPointIndex < this.TopPoints.Count; topPointIndex++)
            {
                this.TopPoints[topPointIndex] += translationVector;
            }
        }

        public Vector3Class AttachToModel(STLModel3D stlModel, Vector3Class centerPoint, float sphereRadius, float topHeight)
        {
            var totalModelIntersectionPointCorrectionVector = new Vector3Class();
            var trianglesWithinRange = stlModel.Triangles.GetTrianglesWithinSphereBoundry(centerPoint, sphereRadius * 1.5f);

            var bottomPoints = this.TopPoints;
            var attachedBottomPoints = new Dictionary<Vector3Class, Vector3Class>();

            for (var bottomPointIndex = 0; bottomPointIndex < bottomPoints.Count; bottomPointIndex++)
            {
                var intersectionPoints = IntersectionProvider.IntersectTriangles(bottomPoints[bottomPointIndex] + new Vector3Class(0,0,0.1f), this.Normal, trianglesWithinRange, IntersectionProvider.typeDirection.OneWay);
                //if (intersectionPoints.Count > 0)
                //{
                    var nearestPoint = new Vector3Class();
                    var nearestDistance = float.MaxValue;
                    foreach (var intersectionPoint in intersectionPoints)
                    {
                        var currentDistance = (intersectionPoint - bottomPoints[bottomPointIndex]).Length;
                        if (currentDistance < nearestDistance && currentDistance < topHeight / 2f)
                        {
                            nearestDistance = currentDistance;
                            nearestPoint = intersectionPoint;
                        }
                    }

                    if (nearestPoint != new Vector3Class())
                    {
                        attachedBottomPoints.Add(bottomPoints[bottomPointIndex], nearestPoint);
                    }
                    else
                    {
                        //no intersection point. Try to find point using x amount of points on line between bottompoint and model intersection point
                        var intersectionPointToBottomPointVector = -(bottomPoints[bottomPointIndex] - centerPoint);
                        var intersectionPointFound = false;
                        for (var vectorIndex = 0.9f; vectorIndex >= 0f; vectorIndex -= 0.1f)
                        {
                            var alternativePoint = centerPoint - (intersectionPointToBottomPointVector * vectorIndex);
                            intersectionPoints = IntersectionProvider.IntersectTriangles(alternativePoint, this.Normal, trianglesWithinRange, IntersectionProvider.typeDirection.OneWay);

                            if (intersectionPoints.Count > 0)
                            {
                                foreach (var intersectionPoint in intersectionPoints.Where(s => s != new Vector3Class()))
                                {
                                    if ((alternativePoint - intersectionPoint).Length < topHeight / 2f)
                                    {
                                        attachedBottomPoints.Add(bottomPoints[bottomPointIndex], intersectionPoint);
                                        intersectionPointFound = true;
                                        break;
                                    }
                                }
                            }

                            if (intersectionPointFound)
                            {
                                break;
                            }
                        }

                        //if (!intersectionPointFound)
                        //{
                        //var alternativePoint = centerPoint;// - (intersectionPointToBottomPointVector * 0.0001f);
                        //    intersectionPoints = IntersectionProvider.IntersectTriangles(alternativePoint, this.Normal, trianglesWithinRange, IntersectionProvider.typeDirection.OneWay);

                        //    if (intersectionPoints.Count > 0)
                        //    {
                        //        foreach (var intersectionPoint in intersectionPoints.Where(s => s != new Vector3Class()))
                        //        {
                        //            attachedBottomPoints.Add(bottomPoints[bottomPointIndex], intersectionPoint);
                        //            intersectionPointFound = true;
                        //            break;
                        //        }
                        //    }
                        //}

                        if (!intersectionPointFound)
                        {
                            attachedBottomPoints.Add(bottomPoints[bottomPointIndex], centerPoint);
                        }
                    }
                //}
                //else
                //{
                //    //no intersectionpoint found
                //}
            }

            foreach (var triangle in this[0])
            {
                for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                {
                    if (attachedBottomPoints.ContainsKey(triangle.Vectors[vectorIndex].Position))
                    {
                        totalModelIntersectionPointCorrectionVector += (triangle.Vectors[vectorIndex].Position - attachedBottomPoints[triangle.Vectors[vectorIndex].Position]) / 16f;
                        triangle.Vectors[vectorIndex].Position = attachedBottomPoints[triangle.Vectors[vectorIndex].Position];
                    }
                }
            }

            return totalModelIntersectionPointCorrectionVector;
        }
    }

    public class ConeCap : TriangleInfoList
    {
        public ConeCap(Vector3Class center, List<Vector3Class> capPoints, bool upsideDown)
        {
            for (var capPointIndex = 0; capPointIndex < capPoints.Count; capPointIndex++)
            {
                //create left triagle
                var leftTriangle = new Triangle();
                leftTriangle.Vectors[0].Position = center;
                leftTriangle.Vectors[1].Position = capPoints[capPointIndex];

                if (capPointIndex == capPoints.Count - 1)
                {
                    leftTriangle.Vectors[2].Position = capPoints[0];
                }
                else
                {
                    leftTriangle.Vectors[2].Position = capPoints[capPointIndex + 1];
                }

                if (upsideDown)
                {
                    leftTriangle.Flip();
                }
                leftTriangle.CalcNormal();
                this[0].Add(leftTriangle);
            }
        }
    }
}
