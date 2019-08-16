using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Models.Defaults
{
    internal class XYCorrection_10x10: STLModel3D
    {
        internal XYCorrection_10x10()
        {
            this._bindingSupported = ObjectView.BindingSupported;

            var cube10x10 = new Cube(10, 10, 10);
            var cubeTriangles = cube10x10.AsSTLModel3D();
            this.Triangles = new TriangleInfoList();
            var stlModelTriangles = this.Triangles;

            //append middle
            foreach (var triangle in cubeTriangles.Triangles[0])
            {
                //move triangle to leftback
                var triangleClone = (Triangle)triangle.Clone();

                stlModelTriangles[0].Add(triangleClone);
            }

            //append other cubes
            foreach (var triangle in cubeTriangles.Triangles[0])
            {
                //move triangle to leftback
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X -= 45f;
                triangleClone.Vectors[0].Position.Y += 45f;
                triangleClone.Vectors[1].Position.X -= 45f;
                triangleClone.Vectors[1].Position.Y += 45f;
                triangleClone.Vectors[2].Position.X -= 45f;
                triangleClone.Vectors[2].Position.Y += 45f;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in cubeTriangles.Triangles[0])
            {
                //move triangle to rightback
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += 45f;
                triangleClone.Vectors[0].Position.Y += 45f;
                triangleClone.Vectors[1].Position.X += 45f;
                triangleClone.Vectors[1].Position.Y += 45f;
                triangleClone.Vectors[2].Position.X += 45f;
                triangleClone.Vectors[2].Position.Y += 45f;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in cubeTriangles.Triangles[0])
            {
                //move triangle to leftfront
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X -= 45f;
                triangleClone.Vectors[0].Position.Y -= 45f;
                triangleClone.Vectors[1].Position.X -= 45f;
                triangleClone.Vectors[1].Position.Y -= 45f;
                triangleClone.Vectors[2].Position.X -= 45f;
                triangleClone.Vectors[2].Position.Y -= 45f;

                stlModelTriangles[0].Add(triangleClone);
            }

            foreach (var triangle in cubeTriangles.Triangles[0])
            {
                //move triangle to rightfront
                var triangleClone = (Triangle)triangle.Clone();
                triangleClone.Vectors[0].Position.X += 45f;
                triangleClone.Vectors[0].Position.Y -= 45f;
                triangleClone.Vectors[1].Position.X += 45f;
                triangleClone.Vectors[1].Position.Y -= 45f;
                triangleClone.Vectors[2].Position.X += 45f;
                triangleClone.Vectors[2].Position.Y -= 45f;

                stlModelTriangles[0].Add(triangleClone);
            }
        }
        
    }
}
