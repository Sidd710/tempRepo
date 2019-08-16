using Atum.Studio.Core.Events;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Models.Defaults
{
    internal class TrapezoidCorrection : STLModel3D
    {
        internal TrapezoidCorrection()
        {
            this._bindingSupported = ObjectView.BindingSupported;

            var groundPanelZOffset = 0.2f;
            var horizontalX = Managers.PrinterManager.DefaultPrinter.ProjectorResolutionX / 20;
            var verticalY = Managers.PrinterManager.DefaultPrinter.ProjectorResolutionY / 20;
            var horizontalOffsetX = (horizontalX / 2) - 4.5f;
            var horizontalOffsetVertX = (horizontalX / 2) - 1.5f;
            var groundPanelOffsetX = (horizontalX / 2) - 7.5f;
            var groundPanelOffsetVertX = (horizontalX / 2) - 1.5f;
            var verticalOffsetX = (verticalY / 2) - 1.5f;
            var verticallOffsetVertX = (verticalY / 2) - 6f;
            var groundPanelVerticalOffsetX = (verticalY / 2) - 7.5f;
            var groundPanelVerticalOffsetVertX = (verticalY / 2) - 1f;
            var groundPanel30x30 = new Cube(15, 15,groundPanelZOffset);
            var cube10x10 = new Cube(10, 10, 10);
            var horizontal20x10 = new Cube(9, 3, 10);
            var vertical20x10 = new Cube(3, 6, 10);
            var cubeTriangles = cube10x10.AsSTLModel3D();
            var groundPanelTriangles = groundPanel30x30.AsSTLModel3D();
            var horizontalTriangles = horizontal20x10.AsSTLModel3D();
            var verticalTriangles = vertical20x10.AsSTLModel3D();
            this.Triangles = new TriangleInfoList();
            var stlModelTriangles = this.Triangles;

            //append middle
            foreach (var triangle in cubeTriangles.Triangles[0])
            {
                //move triangle to leftback
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;
                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in groundPanelTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                stlModelTriangles[0].Add(triangleClone);
            }

            //upper left
            foreach (var triangle in horizontalTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X -= horizontalOffsetX;
                triangleClone.Vectors[0].Position.Y += verticalOffsetX;
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.X -= horizontalOffsetX;
                triangleClone.Vectors[1].Position.Y += verticalOffsetX;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.X -= horizontalOffsetX;
                triangleClone.Vectors[2].Position.Y += verticalOffsetX;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in verticalTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X -= horizontalOffsetVertX;
                triangleClone.Vectors[0].Position.Y += verticallOffsetVertX;
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.X -= horizontalOffsetVertX;
                triangleClone.Vectors[1].Position.Y += verticallOffsetVertX;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.X -= horizontalOffsetVertX;
                triangleClone.Vectors[2].Position.Y += verticallOffsetVertX;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in groundPanelTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X -= groundPanelOffsetX;
                triangleClone.Vectors[0].Position.Y += groundPanelVerticalOffsetX;
                triangleClone.Vectors[1].Position.X -= groundPanelOffsetX;
                triangleClone.Vectors[1].Position.Y += groundPanelVerticalOffsetX;
                triangleClone.Vectors[2].Position.X -= groundPanelOffsetX;
                triangleClone.Vectors[2].Position.Y += groundPanelVerticalOffsetX;

                stlModelTriangles[0].Add(triangleClone);
            }


            //upper right
            foreach (var triangle in horizontalTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += horizontalOffsetX;
                triangleClone.Vectors[0].Position.Y += verticalOffsetX;
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.X += horizontalOffsetX;
                triangleClone.Vectors[1].Position.Y += verticalOffsetX;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.X += horizontalOffsetX;
                triangleClone.Vectors[2].Position.Y += verticalOffsetX;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in verticalTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += horizontalOffsetVertX;
                triangleClone.Vectors[0].Position.Y += verticallOffsetVertX;
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.X += horizontalOffsetVertX;
                triangleClone.Vectors[1].Position.Y += verticallOffsetVertX;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.X += horizontalOffsetVertX;
                triangleClone.Vectors[2].Position.Y += verticallOffsetVertX;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in groundPanelTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += groundPanelOffsetX;
                triangleClone.Vectors[0].Position.Y += groundPanelVerticalOffsetX;
                triangleClone.Vectors[1].Position.X += groundPanelOffsetX;
                triangleClone.Vectors[1].Position.Y += groundPanelVerticalOffsetX;
                triangleClone.Vectors[2].Position.X += groundPanelOffsetX;
                triangleClone.Vectors[2].Position.Y += groundPanelVerticalOffsetX;

                stlModelTriangles[0].Add(triangleClone);
            }

            //lower right
            foreach (var triangle in horizontalTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += horizontalOffsetX;
                triangleClone.Vectors[0].Position.Y -= verticalOffsetX;
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.X += horizontalOffsetX;
                triangleClone.Vectors[1].Position.Y -= verticalOffsetX;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.X += horizontalOffsetX;
                triangleClone.Vectors[2].Position.Y -= verticalOffsetX;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in verticalTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += horizontalOffsetVertX;
                triangleClone.Vectors[0].Position.Y -= verticallOffsetVertX;
                triangleClone.Vectors[0].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[1].Position.X += horizontalOffsetVertX;
                triangleClone.Vectors[1].Position.Y -= verticallOffsetVertX;
                triangleClone.Vectors[1].Position.Z += groundPanelZOffset;
                triangleClone.Vectors[2].Position.X += horizontalOffsetVertX;
                triangleClone.Vectors[2].Position.Y -= verticallOffsetVertX;
                triangleClone.Vectors[2].Position.Z += groundPanelZOffset;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in groundPanelTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += groundPanelOffsetX;
                triangleClone.Vectors[0].Position.Y -= groundPanelVerticalOffsetX;
                triangleClone.Vectors[1].Position.X += groundPanelOffsetX;
                triangleClone.Vectors[1].Position.Y -= groundPanelVerticalOffsetX;
                triangleClone.Vectors[2].Position.X += groundPanelOffsetX;
                triangleClone.Vectors[2].Position.Y -= groundPanelVerticalOffsetX;

                stlModelTriangles[0].Add(triangleClone);
            }

            //lower left (triangle
            var lowerLeftCube = new Cube(9, 9, 10);
            var lowerLeftCubeTriangles = lowerLeftCube.AsSTLModel3D();
            lowerLeftCubeTriangles.Rotate(0, 0, 90, Events.RotationEventArgs.TypeAxis.Z);
            foreach (var triangle in lowerLeftCubeTriangles.Triangles[0])
            {
                triangle.Vectors[0].Position.X -= horizontalOffsetX;
                triangle.Vectors[0].Position.Y -= verticallOffsetVertX + 1.5f;
                triangle.Vectors[0].Position.Z += groundPanelZOffset;
                triangle.Vectors[1].Position.X -= horizontalOffsetX;
                triangle.Vectors[1].Position.Y -= verticallOffsetVertX + 1.5f;
                triangle.Vectors[1].Position.Z += groundPanelZOffset;
                triangle.Vectors[2].Position.X -= horizontalOffsetX;
                triangle.Vectors[2].Position.Y -= verticallOffsetVertX + 1.5f;
                triangle.Vectors[2].Position.Z += groundPanelZOffset;
            }

            stlModelTriangles[0].Add(lowerLeftCubeTriangles.Triangles[0][1]);
            stlModelTriangles[0].Add(lowerLeftCubeTriangles.Triangles[0][3]);
            stlModelTriangles[0].Add(lowerLeftCubeTriangles.Triangles[0][4]);
            stlModelTriangles[0].Add(lowerLeftCubeTriangles.Triangles[0][5]);
            stlModelTriangles[0].Add(lowerLeftCubeTriangles.Triangles[0][10]); //
            stlModelTriangles[0].Add(lowerLeftCubeTriangles.Triangles[0][11]); //
            var tempTriangle = new Triangle();
            tempTriangle.Vectors[0] = lowerLeftCubeTriangles.Triangles[0][10].Vectors[2];
            tempTriangle.Vectors[1] = lowerLeftCubeTriangles.Triangles[0][10].Vectors[1];
            tempTriangle.Vectors[2] = lowerLeftCubeTriangles.Triangles[0][4].Vectors[1];
            stlModelTriangles[0].Add(tempTriangle); //

            tempTriangle = new Triangle();
            tempTriangle.Vectors[0] = lowerLeftCubeTriangles.Triangles[0][4].Vectors[1];
            tempTriangle.Vectors[1] = lowerLeftCubeTriangles.Triangles[0][4].Vectors[0];
            tempTriangle.Vectors[2] = lowerLeftCubeTriangles.Triangles[0][10].Vectors[2];
            stlModelTriangles[0].Add(tempTriangle);

            foreach (var triangle in groundPanelTriangles.Triangles[0])
            {
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X -= groundPanelOffsetX;
                triangleClone.Vectors[0].Position.Y -= groundPanelVerticalOffsetX;
                triangleClone.Vectors[1].Position.X -= groundPanelOffsetX;
                triangleClone.Vectors[1].Position.Y -= groundPanelVerticalOffsetX;
                triangleClone.Vectors[2].Position.X -= groundPanelOffsetX;
                triangleClone.Vectors[2].Position.Y -= groundPanelVerticalOffsetX;

                stlModelTriangles[0].Add(triangleClone);
            }
            
        }
    }
}
