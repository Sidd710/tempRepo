using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Shapes
{
    internal class GroundPane: STLModel3D
    {
        public GroundPane(float xLength,  float yHeight, float zDepth){
            this.UpdateGroundPane(xLength, yHeight, zDepth);
        }

        internal void UpdateGroundPane(float xLength, float yHeight, float zDepth)
        {
      
            this.Triangles = new TriangleInfoList();

            //front face
            var triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, 0, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, 0, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, 0)};
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, 0, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0, yHeight, 0)};
            this.Triangles[0].Add(triangle);

            //back face
            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, 0, -zDepth)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, 0, -zDepth)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, -zDepth)};
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, 0, -zDepth)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, -zDepth)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0, yHeight, -zDepth)};
            this.Triangles[0].Add(triangle);

            //left face
            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, 0, -zDepth)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0, 0, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0, yHeight, -zDepth)};
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, yHeight, -zDepth)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0, 0, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0, yHeight, 0)};
            this.Triangles[0].Add(triangle);

            //right face
            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(xLength, 0, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, 0, -zDepth)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, -zDepth)};
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(xLength, 0, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, -zDepth)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, 0)};
            this.Triangles[0].Add(triangle);

            //bottom face
            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(xLength, 0, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0, 0, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, 0, -zDepth)};
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, 0, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0, 0, -zDepth)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, 0, -zDepth)};
            this.Triangles[0].Add(triangle);

            //top face
            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, yHeight, 0)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, -zDepth)};
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0, yHeight, -zDepth)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0, yHeight, 0)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xLength, yHeight, -zDepth)};
            this.Triangles[0].Add(triangle);

            for (var vertexIndex = 0; vertexIndex < this.Triangles[0].Count; vertexIndex++)
            {
                this.Triangles[0][vertexIndex].Vectors[0].Position.X -= xLength / 2;
                this.Triangles[0][vertexIndex].Vectors[1].Position.X -= xLength / 2;
                this.Triangles[0][vertexIndex].Vectors[2].Position.X -= xLength / 2;
                this.Triangles[0][vertexIndex].Vectors[0].Position.Y -= yHeight / 2;
                this.Triangles[0][vertexIndex].Vectors[1].Position.Y -= yHeight / 2;
                this.Triangles[0][vertexIndex].Vectors[2].Position.Y -= yHeight / 2;

                this.Triangles[0][vertexIndex].Vectors[0].Color = this.Triangles[0][vertexIndex].Vectors[1].Color = this.Triangles[0][vertexIndex].Vectors[2].Color = new Byte4Class( 200, Properties.Settings.Default.SceneGroundColor.R, Properties.Settings.Default.SceneGroundColor.G, Properties.Settings.Default.SceneGroundColor.B);
                this.Triangles[0][vertexIndex].CalcMinMaxX();
                this.Triangles[0][vertexIndex].CalcMinMaxY();
                this.Triangles[0][vertexIndex].CalcMinMaxZ();
            }

            //grid column coloring
            var xPosition = 0f;
            var yPosition = (yHeight / 2f);
            var zPosition = 0.02f;
            var lineThinkness = 0.2f;
            var gridLineColor = new Byte4Class(200, 0, 0, 0);
            for (var gridLineColumnIndex = 1; gridLineColumnIndex < 16; gridLineColumnIndex++)
            {
                xPosition = ((xLength / 16f / 10) * gridLineColumnIndex * 10) - (xLength / 2f) - (lineThinkness / 2f);
                var gridColumnIndexZero = 36 + ((gridLineColumnIndex - 1) * 6);
                triangle = new Triangle();
                triangle.Normal = Vector3Class.UnitY;
                triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(xPosition, yPosition, zPosition)};
                triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xPosition, -yPosition, zPosition)};
                triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xPosition + lineThinkness, yPosition, zPosition)};
                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = gridLineColor;
                this.Triangles[0].Add(triangle);

                triangle = new Triangle();
                triangle.Normal = Vector3Class.UnitY;
                triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(xPosition, -yPosition, zPosition)};
                triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xPosition + lineThinkness, -yPosition, zPosition)};
                triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xPosition + lineThinkness, yPosition, zPosition)};
                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = gridLineColor;
                this.Triangles[0].Add(triangle);
            }

            //grid row coloring
            xPosition = (xLength / 2f);

            yPosition = -(yHeight / 2);
            for (var gridLineRowIndex = 1; gridLineRowIndex < 10; gridLineRowIndex++)
            {
                yPosition += (yHeight / 10);

                triangle = new Triangle();
                triangle.Normal = Vector3Class.UnitY;
                triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(-xPosition, yPosition, zPosition)};
                triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xPosition, yPosition, zPosition)};
                triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xPosition, yPosition + lineThinkness, zPosition)};
                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = gridLineColor;
                this.Triangles[0].Add(triangle);

                triangle = new Triangle();
                triangle.Normal = Vector3Class.UnitY;
                triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(-xPosition, yPosition, zPosition)};
                triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xPosition, yPosition + lineThinkness, zPosition)};
                triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(-xPosition, yPosition + lineThinkness, zPosition)};
                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = gridLineColor;
                this.Triangles[0].Add(triangle);
            }

            //front text
            var defaultFontSize = 15f;
            var sizeCorrection = 4f;
            var faceText = new OrientationGizmoFaceText();
            faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("front", FontStyle.Regular, defaultFontSize * sizeCorrection);
            faceText.RotateText(180, 0, 0, RotationEventArgs.TypeAxis.X);
            faceText._scaleFactorX = faceText._scaleFactorY = faceText._scaleFactorZ = 1;
            faceText.Scale(0.15f, 0.15f, 0.15f, ScaleEventArgs.TypeAxis.ALL, false);
            faceText.UpdateBoundries();
            faceText.UpdateDefaultCenter();
            faceText.MoveModelWithTranslationZ(new Vector3Class(0, -(yHeight / 2) + sizeCorrection + sizeCorrection, 0.02f));

            var faceTextColor = new Byte4Class( 200, Properties.Settings.Default.SceneGroundFrontTextColor.R, Properties.Settings.Default.SceneGroundFrontTextColor.G, Properties.Settings.Default.SceneGroundFrontTextColor.B );
            foreach (var triangleText in faceText.Triangles[0])
            {
                triangleText.Vectors[0].Color = triangleText.Vectors[1].Color = triangleText.Vectors[2].Color = faceTextColor;
                triangleText.Flip();
            }

            this.Triangles[0].AddRange(faceText.Triangles[0]);

            //line below front text
            xPosition = xLength / 2f;
            yPosition = yHeight / 2f;
            zPosition = 0.02f;

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(-xPosition, -yPosition, zPosition)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xPosition, -yPosition, zPosition)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(xPosition, -yPosition + lineThinkness, zPosition)};
            triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = faceTextColor;
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(-xPosition, -yPosition, zPosition)};
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(xPosition, -yPosition + lineThinkness, zPosition)};
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(-xPosition, -yPosition + lineThinkness, zPosition)};
            triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = faceTextColor;
            this.Triangles[0].Add(triangle);

        }
    }
}
