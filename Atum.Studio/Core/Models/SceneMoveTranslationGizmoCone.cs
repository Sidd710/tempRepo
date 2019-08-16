using System;
using System.Collections.Generic;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System.ComponentModel;
using System.Drawing;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class SceneMoveTranslationGizmoCone : STLModel3D
    {
        private float _topRadius;
        private float _totalHeight;
        private float _topHeight;
        private float _middleRadius;

        [Category("Dimensions")]
        [DisplayName("Total Height")]
        [Description("Total Height of Support Cone\nUnit: mm")]
        public float TotalHeight
        {
            get
            {
                return this._totalHeight;
            }
            set
            {
                this._totalHeight = value;
            }
        }
        [Category("Dimensions")]
        [DisplayName("Top Height")]
        [Description("Top Part Height of Cone\nUnit: mm")]
        public float TopHeight
        {
            get
            {
                return this._topHeight;
            }
            set
            {
                this._topHeight = value;
            }
        }
        [Category("Dimensions")]
        [DisplayName("Top Radius")]
        [Description("Top Part Radius of Cone\nUnit: mm")]
        public float TopRadius
        {
            get
            {
                return this._topRadius;
            }
            set
            {
                this._topRadius = value;
            }
        }

        [Browsable(false)]
        public bool InvalidPenetrationPoints { get; set; }

        [Category("Dimensions")]
        [DisplayName("Middle Radius")]
        [Description("Middle Part Radius of Cone\nUnit: mm")]
        public float MiddleRadius
        {
            get
            {
                return this._middleRadius;
            }
            set
            {
                this._middleRadius = value;
            }
        }
        private float _middleHeight;
        public float MiddleHeight
        {
            get
            {
                return this._middleHeight;
            }
            set
            {
                this._middleHeight = value;
            }
        }

        internal Vector3Class Position { get; set; }
        internal Vector3Class Normal { get; set; }

        public SceneMoveTranslationGizmoCone(float totalHeight, float topHeight, float topRadius, float middleRadius, float middleHeight, Vector3Class position, Color color)
            : base(TypeObject.Support, false)
        {
            this._scaleFactorX = 1;
            this._scaleFactorY = 1;
            this._scaleFactorZ = 1;
            this._totalHeight = totalHeight;
            this._topHeight = topHeight;
            this._topRadius = topRadius;
            this._middleRadius = middleRadius;
            this._middleHeight = middleHeight;

            this._color = color;

            this.CreateSupportCone(0, 0, position);
            this.Triangles.UpdateFaceColors(this.ColorAsByte4, false);
        }

        private void CreateSupportCone(float rotationAngleX = 0, float rotationAngleZ = 0, Vector3Class position = null)
        {
            this.Position = position;
            this.Normal = new Vector3Class(0, 0, 1);
            this.RotationAngleX = rotationAngleX;
            this.RotationAngleZ = rotationAngleZ;

            if (this.Triangles == null) { this.Triangles = new TriangleInfoList(); }
            this.Triangles[0].Clear();

            var bottomVectors = new List<Vector3Class>();
            var topCapOffset = new Vector3Class(0, 0, this.TotalHeight - this.TopHeight);
            //create top circle
            var topRadiusPoints = VectorHelper.CreateCircle(topCapOffset.Z, this.TopRadius, 26, true);
            var middleRadiusPoints = VectorHelper.GetCircleOutlinePoints(topCapOffset.Z, this.MiddleRadius, 26, this.Position);
            var bottomPoints = VectorHelper.GetCircleOutlinePoints(0, this.MiddleRadius, 26, this.Position);

            var leftTriangle = new Triangle();
            var rightTriangle = new Triangle();

            //create middle cylinder
            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = bottomPoints[middleRadiusPointIndex];
                leftTriangle.Vectors[2].Position = bottomPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == bottomPoints.Count - 1)
                {
                    leftTriangle.Vectors[1].Position = bottomPoints[0];
                }
                else
                {
                    leftTriangle.Vectors[1].Position = bottomPoints[middleRadiusPointIndex + 1];
                }

                this.Triangles[0].Add(leftTriangle);

                //right triangle
                rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = bottomPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == bottomPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = bottomPoints[0] + this.Position;
                    rightTriangle.Vectors[2].Position = bottomPoints[0] + this.Position + topCapOffset;

                }
                else
                {
                    rightTriangle.Vectors[1].Position = bottomPoints[middleRadiusPointIndex + 1] +this.Position;
                    rightTriangle.Vectors[2].Position = bottomPoints[middleRadiusPointIndex + 1] + this.Position + topCapOffset;

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

                    leftTriangle.Vectors[0].Position = new Vector3Class(0,0,TotalHeight) + this.Position;
                    leftTriangle.Vectors[1].Position = topRadiusPoints[middleRadiusPointIndex] + this.Position;

                    if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                    {
                        leftTriangle.Vectors[2].Position = topRadiusPoints[0] + this.Position;
                    }
                    else
                    {
                        leftTriangle.Vectors[2].Position = topRadiusPoints[middleRadiusPointIndex + 1] + this.Position;
                    }

                    this.Triangles[0].Add(leftTriangle);

                    //right triangle
                    rightTriangle = new Triangle();
                    rightTriangle.Vectors[0].Position = new Vector3Class(0, 0, TotalHeight);

                    if (middleRadiusPointIndex == topRadiusPoints.Count - 1)
                    {
                        rightTriangle.Vectors[1].Position = topRadiusPoints[0] + this.Position;
                        rightTriangle.Vectors[2].Position = new Vector3Class(0, 0, TotalHeight) + this.Position;

                    }
                    else
                    {
                        rightTriangle.Vectors[1].Position = topRadiusPoints[middleRadiusPointIndex + 1] + this.Position;
                        rightTriangle.Vectors[2].Position = new Vector3Class(0, 0, TotalHeight) + this.Position;
                    }

                    this.Triangles[0].Add(rightTriangle);
                }
            }

            //create top cap
            if (topRadiusPoints.Count > 0)
            {
                for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
                {
                    //create left triagle
                    leftTriangle = new Triangle();

                    leftTriangle.Vectors[0].Position = new Vector3Class(0, 0, TotalHeight - TopHeight);
                    leftTriangle.Vectors[1].Position = topRadiusPoints[middleRadiusPointIndex];

                    if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                    {
                        leftTriangle.Vectors[2].Position = topRadiusPoints[0];
                    }
                    else
                    {
                        leftTriangle.Vectors[2].Position = topRadiusPoints[middleRadiusPointIndex + 1];
                    }
                    leftTriangle.Flip();

                    this.Triangles[0].Add(leftTriangle);

                    //right triangle
                    rightTriangle = new Triangle();
                    rightTriangle.Vectors[0].Position = new Vector3Class(0, 0, TotalHeight - TopHeight);

                    if (middleRadiusPointIndex == topRadiusPoints.Count - 1)
                    {
                        rightTriangle.Vectors[1].Position = topRadiusPoints[0];
                        rightTriangle.Vectors[2].Position = new Vector3Class(0, 0, TotalHeight - TopHeight);

                    }
                    else
                    {
                        rightTriangle.Vectors[1].Position = topRadiusPoints[middleRadiusPointIndex + 1];
                        rightTriangle.Vectors[2].Position = new Vector3Class(0, 0, TotalHeight - TopHeight);
                    }
                    rightTriangle.Flip();
                    this.Triangles[0].Add(rightTriangle);
                }
            }

            foreach (var triangle in this.Triangles[0])
            {
                triangle.CalcCenter();
                triangle.CalcNormal();
            }
        }
        
    }
}
