using System;
using System.Collections.Generic;
using System.Text;

using OpenTK;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Models;

namespace Atum.Studio.Core.Shapes
{
    internal sealed class Sphere: STLModel3D
    {
        public enum eSubdivisions
        {
            Zero = 0,
            One = 1,
            Two = 2,
            Three = 3,
            Four = 4,
            Five=5,
            Six=6,
            Seven=7,
            Eight=8,
        }

        public enum eDir
        {
            All,
            FrontTopRight,
            FrontBottomRight,
            FrontBottomLeft,
            FrontTopLeft,
            BackTopRight,
            BackBottomRight,
            BackBottomLeft,
            BackTopLeft,

        }

        public Sphere(float radius, Vector3 offset, eSubdivisions subdivs, eDir[] sides, Vector3Class position, Byte4Class color)
        {
            float Diameter = radius;

            PrimitiveMode = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;

            if (sides[0] == eDir.All)
            {
                sides = new eDir[] {  eDir.FrontTopRight,
            eDir.FrontBottomRight,
            eDir.FrontBottomLeft,
            eDir.FrontTopLeft,
            eDir.BackTopRight,
            eDir.BackBottomRight,
            eDir.BackBottomLeft,
            eDir.BackTopLeft,};
            }

            VertexArray = new Vertex[sides.Length * 3];
            IndexArray = new uint[sides.Length * 3];

            uint counter = 0;
            foreach (eDir s in sides)
            {
                GetDefaultVertices(s, Diameter, out VertexArray[counter + 0], out VertexArray[counter + 1], out VertexArray[counter + 2]);
                IndexArray[counter + 0] = counter + 0;
                IndexArray[counter + 1] = counter + 1;
                IndexArray[counter + 2] = counter + 2;
                counter += 3;
            }

            if (subdivs != eSubdivisions.Zero)
            {
                for (int s = 0; s < (int)subdivs; s++)
                {
                    #region Assemble Chunks and convert to Arrays
                    List<Chunk> AllChunks = new List<Chunk>();
                    for (uint i = 0; i < IndexArray.Length; i += 3)
                    {
                        Chunk chu;
                        Subdivide(Diameter,
                                   ref VertexArray[IndexArray[i + 0]],
                                   ref VertexArray[IndexArray[i + 1]],
                                   ref VertexArray[IndexArray[i + 2]],
                                   out chu);
                        AllChunks.Add(chu);
                    }

                    Chunk.GetArray(ref AllChunks, out VertexArray, out IndexArray);
                    AllChunks.Clear();
                    #endregion Assemble Chunks and convert to Arrays
                }
            }

            this.Triangles = new TriangleInfoList();
            for (int i = 0; i < IndexArray.Length; i += 3)
            {
                //   Vector3.Add(ref VertexArray[i].Position, ref offset, out VertexArray[i].Position);
                // }
                var vertex = VertexArray[IndexArray[i]];
                var triangle = new Triangle();
                triangle.Vectors[0].Position = new Vector3Class(VertexArray[IndexArray[i]].Position);
                triangle.Vectors[1].Position =  new Vector3Class( VertexArray[IndexArray[i + 1]].Position);
                triangle.Vectors[2].Position = new Vector3Class(VertexArray[IndexArray[i + 2]].Position);
                triangle.CalcNormal();
                triangle.Vectors[0].Position += position;
                triangle.Vectors[1].Position += position;
                triangle.Vectors[2].Position += position;
                triangle.Vectors[2].Color = triangle.Vectors[1].Color = triangle.Vectors[0].Color = color;
                this.Triangles[0].Add(triangle);
            }
            
        }

        private void GetDefaultVertices(eDir s, float scale, out Vertex first, out Vertex second, out Vertex third)
        {
            Vertex t1 = new Vertex(),
                            t2 = new Vertex(),
                            t3 = new Vertex();
            switch ( s )
            {
            case eDir.FrontTopRight:
                    t1 = new Vertex() { Normal = Vector3.UnitY, Position = Vector3.UnitY * scale };
                    t2 = new Vertex(){Normal = Vector3.UnitZ, Position = Vector3.UnitZ * scale};
                    t3 = new Vertex(){Normal = Vector3.UnitX, Position = Vector3.UnitX * scale};
                break;
            case eDir.FrontBottomRight:
                t1 = new Vertex(){Normal = Vector3.UnitX, Position = Vector3.UnitX * scale};
                t2 = new Vertex(){Normal = Vector3.UnitZ, Position = Vector3.UnitZ * scale};
                t3 = new Vertex(){Normal = -Vector3.UnitY, Position = -Vector3.UnitY * scale};
                break;
            case eDir.FrontBottomLeft:
                t1 = new Vertex(){Normal = Vector3.UnitX, Position = Vector3.UnitX * scale};
                t2 = new Vertex(){Normal = -Vector3.UnitY, Position = -Vector3.UnitY * scale};
                t3 = new Vertex(){Normal = -Vector3.UnitZ, Position = -Vector3.UnitZ * scale};
                break;
            case eDir.FrontTopLeft:
                t1 = new Vertex(){Normal = -Vector3.UnitZ, Position = -Vector3.UnitZ * scale};
                t2 = new Vertex(){Normal = Vector3.UnitY, Position = Vector3.UnitY * scale};
                t3 = new Vertex(){Normal = Vector3.UnitX, Position = Vector3.UnitX * scale};
                break;
            case eDir.BackTopRight:
                t1 = new Vertex(){Normal = Vector3.UnitY, Position = Vector3.UnitY * scale};
                t2 = new Vertex(){Normal = -Vector3.UnitX, Position = -Vector3.UnitX * scale};
                t3 = new Vertex(){Normal = Vector3.UnitZ, Position = Vector3.UnitZ * scale};
                break;
            case eDir.BackBottomRight:
                t1 = new Vertex(){Normal = -Vector3.UnitY, Position = -Vector3.UnitY * scale};
                t2 = new Vertex(){Normal = Vector3.UnitZ, Position = Vector3.UnitZ * scale};
                t3 = new Vertex(){Normal = -Vector3.UnitX, Position = -Vector3.UnitX * scale};
                break;
            case eDir.BackBottomLeft:
                t1 = new Vertex(){Normal = -Vector3.UnitY, Position = -Vector3.UnitY * scale};
                t2 = new Vertex(){Normal = -Vector3.UnitX, Position = -Vector3.UnitX * scale};
                t3 = new Vertex(){Normal = -Vector3.UnitZ, Position = -Vector3.UnitZ * scale};
                break;
            case eDir.BackTopLeft:
                t1 = new Vertex(){Normal = Vector3.UnitY, Position = Vector3.UnitY * scale};
                t2 = new Vertex(){Normal = -Vector3.UnitZ, Position = -Vector3.UnitZ * scale};
                t3 = new Vertex() { Normal = -Vector3.UnitX, Position = -Vector3.UnitX * scale };
                break;
            }
            first = t1;
            second = t2;
            third = t3;
        }


        private void Subdivide(float Scale, ref Vertex first, ref Vertex second, ref Vertex third, out Chunk c)
        {
            c = new Chunk(6, 12);

            c.Vertices[0] = first;
            
            Vector3.Lerp(ref first.Position, ref second.Position, 0.5f,out c.Vertices[1].Normal );
            c.Vertices[1].Normal.Normalize();
            c.Vertices[1].Position = c.Vertices[1].Normal * Scale;
          
            Vector3.Lerp( ref third.Position, ref first.Position, 0.5f, out c.Vertices[2].Normal );
            c.Vertices[2].Normal.Normalize();
            c.Vertices[2].Position = c.Vertices[2].Normal * Scale;
          
            c.Vertices[3] = second;
    
            Vector3.Lerp( ref second.Position, ref third.Position, 0.5f, out c.Vertices[4].Normal );
            c.Vertices[4].Normal.Normalize();
            c.Vertices[4].Position = c.Vertices[4].Normal * Scale;
          
            c.Vertices[5] = third;

            #region Indices
            c.Indices[0]=0;
            c.Indices[1]=1;
            c.Indices[2]=2;
            c.Indices[3]=2;
            c.Indices[4]=1;
            c.Indices[5]=4;
            c.Indices[6]=1;
            c.Indices[7]=3;
            c.Indices[8]=4;
            c.Indices[9]=2;
            c.Indices[10]=4;
            c.Indices[11]=5;
            #endregion Indices
        }

    }
}
