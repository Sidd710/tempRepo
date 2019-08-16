using System;
using System.Collections.Generic;
using Atum.Studio.Core.Shapes;
using OpenTK;
using System.ComponentModel;
using System.Drawing;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Managers;
using static Atum.Studio.Core.Helpers.ContourHelper;
using Atum.DAL.Materials;
using Atum.DAL.Hardware;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class SupportConeV2InterlinkConnection : STLModel3D
    {
        private float _radius;

        public SortedDictionary<float, PolyTree> SliceContours;

        internal Vector3Class StartPoint { get; set; }
        internal Vector3Class EndPoint { get; set; }

        [Category("Dimensions")]
        [DisplayName("Radius")]
        [Description("Radius\nUnit: mm")]
        public float Radius
        {
            get
            {
                return this._radius;
            }
            set
            {
                this._radius = value;
            }
        }

        internal Vector3Class Position { get; set; }
        internal Vector3Class Normal { get; set; }

        public SupportConeV2InterlinkConnection(float radius, Vector3Class startPoint, Vector3Class endPoint, Color color)
            : base(TypeObject.Support, false)
        {
            this._radius = radius;
            this._color = color;

            this.Update(startPoint, endPoint);
            this.Triangles.UpdateFaceColors(this.ColorAsByte4, false);

            this.UpdateBoundries();


            this.Loaded = true;
        }

        private void Update(Vector3Class startPoint, Vector3Class endPoint)
        {
            this.StartPoint = startPoint;
            this.EndPoint = endPoint;

            float yAngle = 0f;
            float zAngle = 0f;
            VectorHelper.CalcRotationAnglesYZFromVector(endPoint - startPoint, false, out zAngle, out yAngle);

            var diffVector = endPoint - startPoint;
            var diffVector2 = diffVector;
            diffVector2.Z = 0;
            yAngle = VectorHelper.CalcAngleBetweenVectorPoints(new Vector2(diffVector2.Length, 0), new Vector2(0, 0), new Vector2(diffVector2.Length, diffVector.Z));
            var totalHeight = (endPoint - startPoint).Length;

            if (this.Triangles == null) { this.Triangles = new TriangleInfoList(); }
            this.Triangles[0].Clear();

            this.Triangles = new TriangleInfoList();

            var capPointsCount = 26;
            var capPoints = VectorHelper.CreateCircle(0, this.Radius, capPointsCount, true);

            var rotationMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(yAngle));
            for (var capPointIndex = 0; capPointIndex < capPointsCount; capPointIndex++)
            {
                var rotatedVector = Vector3Class.Transform(capPoints[capPointIndex], rotationMatrix);
                var rotationXYScaling = capPoints[capPointIndex].X / rotatedVector.X;
                capPoints[capPointIndex] = new Vector3Class(capPoints[capPointIndex].X, capPoints[capPointIndex].Y, rotatedVector.Z * rotationXYScaling);
            }

            for (var capPointIndex = 0; capPointIndex < capPointsCount; capPointIndex++)
            {
                var capTriangle = new Triangle();
                capTriangle.Vectors[0].Position = capPoints[capPointIndex];
                capTriangle.Vectors[1].Position = capPoints[capPointIndex == capPointsCount - 1 ? 0 : capPointIndex + 1];
                //capTriangle.Vectors[2].Position = startPoint;
                capTriangle.CalcNormal();
                this.Triangles[0].Add(capTriangle);
            }

            //bottom capPoints 
            for (var capPointIndex = 0; capPointIndex < capPointsCount; capPointIndex++)
            {
                var bottomCapTriangle = (Triangle)this.Triangles[0][capPointIndex].Clone();
                bottomCapTriangle.Flip();
                this.Triangles[0].Add(bottomCapTriangle);
            }

            //move top cap to total height
            for (var capPointIndex = 0; capPointIndex < capPointsCount; capPointIndex++)
            {
                this.Triangles[0][capPointIndex].Vectors[0].Position.Z += totalHeight;
                this.Triangles[0][capPointIndex].Vectors[1].Position.Z += totalHeight;
                this.Triangles[0][capPointIndex].Vectors[2].Position.Z += totalHeight;
            }

            //create cylinder triangles
            for (var capPointIndex = 0; capPointIndex < capPointsCount; capPointIndex++)
            {
                var leftTriangle = new Triangle();
                leftTriangle.Vectors[0].Position = this.Triangles[0][capPointIndex].Vectors[0].Position;
                leftTriangle.Vectors[1].Position = this.Triangles[0][capPointIndex + capPointsCount].Vectors[0].Position;
                leftTriangle.Vectors[2].Position = this.Triangles[0][capPointIndex + capPointsCount].Vectors[2].Position;
                leftTriangle.CalcNormal();
                this.Triangles[0].Add(leftTriangle);

                var rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = this.Triangles[0][capPointIndex].Vectors[0].Position;
                rightTriangle.Vectors[1].Position = this.Triangles[0][capPointIndex + capPointsCount].Vectors[2].Position;
                rightTriangle.Vectors[2].Position = this.Triangles[0][capPointIndex].Vectors[1].Position;
                rightTriangle.CalcNormal();
                this.Triangles[0].Add(rightTriangle);
            }

            this.UpdateBoundries();
            this.Rotate(0, 90 - yAngle, 0, Events.RotationEventArgs.TypeAxis.Y, updateFaceColor: false);
            this.Rotate(0, 0, zAngle, Events.RotationEventArgs.TypeAxis.Z, updateFaceColor: false);
            foreach (var triangle in this.Triangles[0])
            {
                triangle.Vectors[0].Position += startPoint;
                triangle.Vectors[1].Position += startPoint;
                triangle.Vectors[2].Position += startPoint;
            }

            this.UpdateBoundries();
        }

        internal void CalcSlicesContours(Material selectedMaterial, AtumPrinter selectedPrinter)
        {
            var contours = new SortedDictionary<float, PolyTree>();
            
            foreach (var triangle in this.Triangles[0])
            {
                triangle.CalcMinMaxZ();
            }

            this.CalcSliceIndexes(selectedMaterial, false);

            var sliceIndex = 0;
            foreach (var sliceHeight in this.SliceIndexes.Keys)
            {
                contours.Add(sliceHeight, STLModel3D.GetSliceContoursForBaseSTLModel3D(this, sliceIndex, sliceHeight, selectedPrinter));

                sliceIndex++;
            }

            this.SliceContours = contours;
        }
    }
}
