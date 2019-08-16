using System;
using System.Collections.Generic;
using System.Text;
using Atum.Studio.Core.Shapes;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.ComponentModel;
using System.Drawing;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Utils;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class XYZAxisCone : STLModel3D
    {
        public bool HasModelIntersection { get; set; }

        internal List<Vector3> DebugLines { get; set; }

        private float _topRadius;
        private float _totalHeight;
        private float _topHeight;
        private float _middleRadius;
        private float _bottomHeight;
        private float _bottomRadius;
        internal bool _groundSupport;
        

        internal float DEFAULT_HEIGHT = 25f;
        
        internal int _slicesCount { get; set; }
        internal override bool Hidden { get; set; }

        internal Vector3 Position { get; set; }
        internal Vector3 Normal { get; set; }
        internal typeCreationBy CreationBy { get; set; }

        private Cone _bottomCone { get; set; }
        private Cone _middleCone { get; set; }

        internal enum typeCreationBy
        {
            Manual = 0,
            ManualCrossSupport,
            Auto = 5
        }

        internal new Vector3 Center
        {
            get
            {
                return this.Position;
            }
        }
        
        public XYZAxisCone(float totalHeight, float topHeight, float topRadius, float middleRadius, float bottomHeight, float bottomRadius, int slicesCount, Vector3 position, Color color, float rotationAngleX = 0, float rotationAngleZ = 0, STLModel3D model = null, bool groundSupport = true, bool useSupportPenetration = false, STLModel3D surfaceTriangles = null)
            : base(TypeObject.Support, false)
        {
            this._groundSupport = groundSupport;
            this._scaleFactorX = 1;
            this._scaleFactorY = 1;
            this._scaleFactorZ = 1;
            this._totalHeight = totalHeight;
            this._topHeight = topHeight;
            this._topRadius = topRadius;
            this._middleRadius = middleRadius;
            this._bottomHeight = bottomHeight;
            this._bottomRadius = bottomRadius;
            this._slicesCount = slicesCount;
            this._color = color;
            //   this._rotationAngleX = rotationAngleX;
            //   this._rotationAngleZ = rotationAngleZ;

            //when bottom and top are larger then total cone split the new height in half
            if ((this._bottomHeight + this._topHeight) > totalHeight)
            {
                this._bottomHeight = this._topHeight = this._totalHeight / 2;
            }

            this.CreateSupportCone(rotationAngleX, rotationAngleZ, position, model, groundSupport, useSupportPenetration, surfaceTriangles: surfaceTriangles);
            this.Triangles.UpdateFaceColors(this.ColorAsByte4);
        }



        private void CreateSupportCone(float rotationAngleX = 0, float rotationAngleZ = 0, Vector3 position = new Vector3(), STLModel3D model = null, bool groundSupport = true, bool useSupportPenetration = true, STLModel3D surfaceTriangles = null)
        {
            var supportEnginePenetrationDepthVector = new Vector3(0, 0, UserProfileManager.UserProfile.SupportEngine_Penetration_Depth);
            var supportEngineBasementThicknessVector = new Vector3(0, 0, UserProfileManager.UserProfile.SupportEngine_Basement_Thickness);

            this.Position = position;
            this.Normal = new Vector3(0, 0, 1);
            this.RotationAngleX = rotationAngleX;
            this.RotationAngleZ = rotationAngleZ;

            var topCapCenterPoint = this.Position;
            
            if (this.Triangles == null) { this.Triangles = new TriangleInfoList(); }
            this.Triangles[0].Clear();

            var bottomVectors = new List<Vector3>();

            var topRadiusPoints = VectorHelper.CreateCircle(this._totalHeight, this._topRadius, this._slicesCount, true);
            var bottomPoints = VectorHelper.CreateCircle(0, this._bottomRadius, this._slicesCount, false);
            var middleRadiusPoints = VectorHelper.GetCircleOutlinePoints(this._bottomHeight, this._middleRadius, this._slicesCount, this.Position);

            
            //create bottom cone
            var leftTriangle = new Triangle();
            var rightTriangle = new Triangle();
            var topCapOffset = new Vector3(0, 0, this._totalHeight - this._topHeight - this._bottomHeight);

            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = middleRadiusPoints[middleRadiusPointIndex];
                leftTriangle.Vectors[1].Position = bottomPoints[middleRadiusPointIndex] + this.Position;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    leftTriangle.Vectors[2].Position = bottomPoints[0] + this.Position;
                }
                else
                {
                    leftTriangle.Vectors[2].Position = bottomPoints[middleRadiusPointIndex + 1] + this.Position;
                }

                this.Triangles[0].Add(leftTriangle);

                //right triangle
                rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = middleRadiusPoints[middleRadiusPointIndex];

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = bottomPoints[0] + this.Position;
                    rightTriangle.Vectors[2].Position = middleRadiusPoints[0];

                }
                else
                {
                    rightTriangle.Vectors[1].Position = bottomPoints[middleRadiusPointIndex + 1] + this.Position;
                    rightTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex + 1];
                }

                this.Triangles[0].Add(rightTriangle);
            }

            //create middle cylinder
            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = middleRadiusPoints[middleRadiusPointIndex];
                leftTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    leftTriangle.Vectors[1].Position = middleRadiusPoints[0];
                }
                else
                {
                    leftTriangle.Vectors[1].Position = middleRadiusPoints[middleRadiusPointIndex + 1];
                }

                this.Triangles[0].Add(leftTriangle);

                //right triangle
                rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = middleRadiusPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = middleRadiusPoints[0];
                    rightTriangle.Vectors[2].Position = middleRadiusPoints[0] + topCapOffset;

                }
                else
                {
                    rightTriangle.Vectors[1].Position = middleRadiusPoints[middleRadiusPointIndex + 1];
                    rightTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex + 1] + topCapOffset;

                }

                this.Triangles[0].Add(rightTriangle);
            }


            //create top cone
            if (topRadiusPoints.Count > 0)
            {
                for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
                {
                    //create left triagle
                    leftTriangle = new Triangle();
                    
                    leftTriangle.Vectors[0].Position = topRadiusPoints[middleRadiusPointIndex] + this.Position + supportEnginePenetrationDepthVector;
                    leftTriangle.Vectors[1].Position = middleRadiusPoints[middleRadiusPointIndex] + topCapOffset;
                    
                    if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                    {
                        leftTriangle.Vectors[2].Position = middleRadiusPoints[0] + topCapOffset;
                    }
                    else
                    {
                        leftTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex + 1] + topCapOffset;
                    }

                    this.Triangles[0].Add(leftTriangle);

                    //right triangle
                    rightTriangle = new Triangle();
                    rightTriangle.Vectors[0].Position = topRadiusPoints[middleRadiusPointIndex] + this.Position + supportEnginePenetrationDepthVector;

                    if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                    {
                        rightTriangle.Vectors[1].Position = middleRadiusPoints[0] + topCapOffset;
                        rightTriangle.Vectors[2].Position = topRadiusPoints[0] + this.Position + supportEnginePenetrationDepthVector;

                    }
                    else
                    {
                        rightTriangle.Vectors[1].Position = middleRadiusPoints[middleRadiusPointIndex + 1] + topCapOffset;
                        rightTriangle.Vectors[2].Position = topRadiusPoints[middleRadiusPointIndex + 1] + this.Position + supportEnginePenetrationDepthVector;
                    }

                    this.Triangles[0].Add(rightTriangle);
                }
            }

            foreach (var triangle in this.Triangles[0])
            {
                triangle.CalcCenter();
                triangle.CalcNormal();
            }

            this.UpdateBoundries();
        }

        internal void RotateByAxis(float angleX, float angleY, float angleZ, RotationEventArgs.TypeAxis rotationAxis)
        {
            Matrix4 rotationMatrix = new Matrix4();

            switch (rotationAxis)
            {
                case RotationEventArgs.TypeAxis.X:
                    rotationMatrix = Matrix4.CreateRotationX(OpenTK.MathHelper.DegreesToRadians(angleX - this.RotationAngleX));
                    this._rotationAngleX = angleX;
                    break;
                case RotationEventArgs.TypeAxis.Y:
                    rotationMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(angleY - this.RotationAngleY));
                    this._rotationAngleY = angleY;
                    break;
                case RotationEventArgs.TypeAxis.Z:
                    rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(angleZ - this.RotationAngleZ));
                    this._rotationAngleZ = angleZ;
                    break;
            }

            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3.Transform(this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X = rotatedVector.X;
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y = rotatedVector.Y;
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z = rotatedVector.Z;

                        this.Triangles[arrayIndex][triangleIndex].CalcCenter();
                        this.Triangles[arrayIndex][triangleIndex].CalcNormal();
                    }
                }
            }
        }
    }
}
