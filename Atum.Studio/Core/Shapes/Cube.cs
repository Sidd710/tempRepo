using System;
using System.Collections.Generic;
using System.Text;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Models;
using System.Drawing.Imaging;
using System.Diagnostics;
using System.Windows.Forms;

namespace Atum.Studio.Core.Shapes
{
    class Cube : STLModel3D
    {
        internal float xLength { get; set; }
        internal float yHeight { get; set; }
        internal float zDepth { get; set; }

        internal Cube(float xLength, float yHeight, float zDepth)
        {
            this._bindingSupported = true;

            this.xLength = xLength;
            this.zDepth = zDepth;
            this.yHeight = yHeight;

            this.VertexArray = new Structs.Vertex[36];

            //front face
            this.VertexArray[0] = new Structs.Vertex() { Position = new Vector3(0, 0, 0), Normal = new Vector3(0, -1, 0) };
            this.VertexArray[1] = new Structs.Vertex() { Position = new Vector3(xLength, 0, 0) };
            this.VertexArray[2] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, 0) };
            this.VertexArray[3] = new Structs.Vertex() { Position = new Vector3(0, 0, 0), Normal = new Vector3(0, -1, 0) };
            this.VertexArray[4] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, 0) };
            this.VertexArray[5] = new Structs.Vertex() { Position = new Vector3(0, yHeight, 0) };

            //back face
            this.VertexArray[7] = new Structs.Vertex() { Position = new Vector3(0, 0, -zDepth), Normal = new Vector3(0, 1, 0) };
            this.VertexArray[6] = new Structs.Vertex() { Position = new Vector3(xLength, 0, -zDepth) };
            this.VertexArray[8] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, -zDepth) };
            this.VertexArray[10] = new Structs.Vertex() { Position = new Vector3(0, 0, -zDepth), Normal = new Vector3(0, 1, 0) };
            this.VertexArray[9] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, -zDepth) };
            this.VertexArray[11] = new Structs.Vertex() { Position = new Vector3(0, yHeight, -zDepth) };

            //left face
            this.VertexArray[12] = new Structs.Vertex() { Position = new Vector3(0, 0, -zDepth), Normal = new Vector3(-1, 0, 0) };
            this.VertexArray[13] = new Structs.Vertex() { Position = new Vector3(0, 0, 0) };
            this.VertexArray[14] = new Structs.Vertex() { Position = new Vector3(0, yHeight, -zDepth) };
            this.VertexArray[15] = new Structs.Vertex() { Position = new Vector3(0, yHeight, -zDepth), Normal = new Vector3(-1, 0, 0) };
            this.VertexArray[16] = new Structs.Vertex() { Position = new Vector3(0, 0, 0) };
            this.VertexArray[17] = new Structs.Vertex() { Position = new Vector3(0, yHeight, 0) };

            //right face
            this.VertexArray[18] = new Structs.Vertex() { Position = new Vector3(xLength, 0, 0), Normal = new Vector3(1, 0, 0) };
            this.VertexArray[19] = new Structs.Vertex() { Position = new Vector3(xLength, 0, -zDepth) };
            this.VertexArray[20] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, -zDepth) };
            this.VertexArray[21] = new Structs.Vertex() { Position = new Vector3(xLength, 0, 0), Normal = new Vector3(1, 0, 0) };
            this.VertexArray[22] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, -zDepth) };
            this.VertexArray[23] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, 0) };

            //bottom face
            this.VertexArray[24] = new Structs.Vertex() { Position = new Vector3(xLength, 0, 0), Normal = new Vector3(0, 0, -1) };
            this.VertexArray[25] = new Structs.Vertex() { Position = new Vector3(0, 0, 0) };
            this.VertexArray[26] = new Structs.Vertex() { Position = new Vector3(xLength, 0, -zDepth) };
            this.VertexArray[27] = new Structs.Vertex() { Position = new Vector3(0, 0, 0), Normal = new Vector3(0, 0, -1) };
            this.VertexArray[28] = new Structs.Vertex() { Position = new Vector3(0, 0, -zDepth) };
            this.VertexArray[29] = new Structs.Vertex() { Position = new Vector3(xLength, 0, -zDepth) };

            //top face
            this.VertexArray[30] = new Structs.Vertex() { Position = new Vector3(0, yHeight, 0), Normal = new Vector3(0, 0, 1) };
            this.VertexArray[31] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, 0) };
            this.VertexArray[32] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, -zDepth) };
            this.VertexArray[33] = new Structs.Vertex() { Position = new Vector3(0, yHeight, -zDepth), Normal = new Vector3(0, 0, 1) };
            this.VertexArray[34] = new Structs.Vertex() { Position = new Vector3(0, yHeight, 0) };
            this.VertexArray[35] = new Structs.Vertex() { Position = new Vector3(xLength, yHeight, -zDepth) };
        }



        internal Vector3 Center
        {
            get
            {
                return new Vector3(this.xLength / 2, this.yHeight / 2, zDepth / 2);
            }

        }


        internal STLModel3D AsSTLModel3D(System.Drawing.Color modelColor)
        {
            var triangle = new Triangle();
            var triangles = new TriangleInfoList();
            for (var vertexIndex = 0; vertexIndex < this.VertexArray.Length; vertexIndex += 3)
            {
                var currentVertexIndex = vertexIndex % 3;
                triangle = new Triangle();
                triangle.Vectors[currentVertexIndex].Position = new Vector3Class(this.VertexArray[vertexIndex].Position) + new Vector3Class(0, 0, 1);
                triangle.Vectors[currentVertexIndex + 1].Position = new Vector3Class(this.VertexArray[vertexIndex + 1].Position) + new Vector3Class(0, 0, 1);
                triangle.Vectors[currentVertexIndex + 2].Position = new Vector3Class(this.VertexArray[vertexIndex + 2].Position) + new Vector3Class(0, 0, 1);
                triangle.CalcMinMaxZ();

                triangles[0].Add(triangle);
            }

            var stlModel = new STLModel3D(STLModel3D.TypeObject.Model, ObjectView.BindingSupported);
            stlModel.Open(null, false, modelColor, ObjectView.NextObjectIndex, triangles, false, false);
            return stlModel;
        }
    }
}
