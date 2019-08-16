using System;
using System.Collections.Generic;

using OpenTK;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Shapes
{
    internal sealed class ChamferCube: DrawableShapeInfo
    {

        public enum SubDivs: byte
        {
            Zero,
            One,
            Two,
            Three,
            Four,
        }

        internal ChamferCube(float Width, float Height, float Length, SubDivs subdivs, float radius, bool useDL)
            : base( useDL )
        {   
            Sphere.eSubdivisions sphereSubDivs = Sphere.eSubdivisions.Zero;
            uint hoseSubDivs = 0;

            switch ( subdivs )
            {
            case SubDivs.Zero:
                sphereSubDivs = Sphere.eSubdivisions.Zero;
                hoseSubDivs = 0;
                break;
            case SubDivs.One:
                sphereSubDivs = Sphere.eSubdivisions.One;
                hoseSubDivs = 1;
                break;
            case SubDivs.Two:
                sphereSubDivs = Sphere.eSubdivisions.Two;
                hoseSubDivs = 3;
                break;
            case SubDivs.Three:
                sphereSubDivs = Sphere.eSubdivisions.Three;
                hoseSubDivs = 7;
                break;
            case SubDivs.Four:
                sphereSubDivs = Sphere.eSubdivisions.Four;
                hoseSubDivs = 15;
                break;
            }

            #region Temporary Storage

            List<Chunk> AllChunks = new List<Chunk>();
            OpenTK.Graphics.OpenGL.PrimitiveType TemporaryMode;
            Vertex[] TemporaryVBO;
            uint[] TemporaryIBO;

            #endregion Temporary Storage

            var FrontTopRightEdge = new Vector3( +Width - radius, +Height - radius, +Length - radius );
            var FrontTopLeftEdge = new Vector3(+Width - radius, +Height - radius, -Length + radius);
            var FrontBottomRightEdge = new Vector3(+Width - radius, -Height + radius, +Length - radius);
            var FrontBottomLeftEdge = new Vector3(+Width - radius, -Height + radius, -Length + radius);
            var BackTopRightEdge = new Vector3(-Width + radius, +Height - radius, +Length - radius);
            var BackTopLeftEdge = new Vector3(-Width + radius, +Height - radius, -Length + radius);
            var BackBottomRightEdge = new Vector3(-Width + radius, -Height + radius, +Length - radius);
            var BackBottomLeftEdge = new Vector3(-Width + radius, -Height + radius, -Length + radius);

            #region 8 sliced Spheres
            Sphere tempSphere;
            var tempVector = Vector3.Zero;
            Sphere.eDir[] tempEdge = new Sphere.eDir[1];

            for ( int i = 0; i < 8; i++ )
            {
                switch ( i )
                {
                case 0:
                    tempVector = FrontTopRightEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.FrontTopRight };
                    break;
                case 1:
                    tempVector = FrontTopLeftEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.FrontTopLeft };
                    break;
                case 2:
                    tempVector = FrontBottomRightEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.FrontBottomRight };
                    break;
                case 3:
                    tempVector = FrontBottomLeftEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.FrontBottomLeft };
                    break;
                case 4:
                    tempVector = BackBottomRightEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.BackBottomRight };
                    break;
                case 5:
                    tempVector = BackBottomLeftEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.BackBottomLeft };
                    break;
                case 6:
                    tempVector = BackTopRightEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.BackTopRight };
                    break;
                case 7:
                    tempVector = BackTopLeftEdge;
                    tempEdge = new Sphere.eDir[] { Sphere.eDir.BackTopLeft };
                    break;
                }
            //    tempSphere = new Sphere( radius,
            //                                     tempVector,
            //                                     sphereSubDivs,
            //                                     tempEdge,
            //                                     false );
            //    tempSphere.GetArraysforVBO( out TemporaryMode, out TemporaryVBO, out TemporaryIBO );
            //    tempSphere.Dispose();
            //    AllChunks.Add( new Chunk( ref TemporaryVBO, ref TemporaryIBO ) );
            }
            #endregion 8 sliced Spheres

            #region 12 sliced Hoses

            SlicedHose tempHose;
            SlicedHose.eSide tempSide = SlicedHose.eSide.BackBottom;
            var tempHoseStart = Vector3.Zero;
            var tempHoseEnd = Vector3.Zero;

            for ( int i = 0; i < 12; i++ )
            {
                switch ( i )
                {
                #region Around X Axis
                case 0:
                    tempSide = SlicedHose.eSide.BottomRight;
                    tempHoseStart = BackBottomRightEdge;
                    tempHoseEnd = FrontBottomRightEdge;
                    break;
                case 1:
                    tempSide = SlicedHose.eSide.TopRight;
                    tempHoseStart = BackTopRightEdge;
                    tempHoseEnd = FrontTopRightEdge;
                    break;
                case 2:
                    tempSide = SlicedHose.eSide.TopLeft;
                    tempHoseStart = BackTopLeftEdge;
                    tempHoseEnd = FrontTopLeftEdge;
                    break;
                case 3:
                    tempSide = SlicedHose.eSide.BottomLeft;
                    tempHoseStart = BackBottomLeftEdge;
                    tempHoseEnd = FrontBottomLeftEdge;
                    break;
                #endregion Around X Axis
                #region Around Y Axis
                case 4:
                    tempSide = SlicedHose.eSide.FrontRight;
                    tempHoseStart = FrontBottomRightEdge;
                    tempHoseEnd = FrontTopRightEdge;
                    break;
                case 5:
                    tempSide = SlicedHose.eSide.BackRight;
                    tempHoseStart = BackBottomRightEdge;
                    tempHoseEnd = BackTopRightEdge;
                    break;
                case 6:
                    tempSide = SlicedHose.eSide.BackLeft;
                    tempHoseStart = BackBottomLeftEdge;
                    tempHoseEnd = BackTopLeftEdge;
                    break;
                case 7:
                    tempSide = SlicedHose.eSide.FrontLeft;
                    tempHoseStart = FrontBottomLeftEdge;
                    tempHoseEnd = FrontTopLeftEdge;
                    break;
                #endregion Around Y Axis
                #region Around Z Axis
                case 8:
                    tempSide = SlicedHose.eSide.FrontTop;
                    tempHoseStart = FrontTopRightEdge;
                    tempHoseEnd = FrontTopLeftEdge;
                    break;
                case 9:
                    tempSide = SlicedHose.eSide.BackTop;
                    tempHoseStart = BackTopRightEdge;
                    tempHoseEnd = BackTopLeftEdge;
                    break;
                case 10:
                    tempSide = SlicedHose.eSide.BackBottom;
                    tempHoseStart = BackBottomRightEdge;
                    tempHoseEnd = BackBottomLeftEdge;
                    break;
                case 11:
                    tempSide = SlicedHose.eSide.FrontBottom;
                    tempHoseStart = FrontBottomRightEdge;
                    tempHoseEnd = FrontBottomLeftEdge;
                    break;
                #endregion Around Z Axis
                }
                tempHose = new SlicedHose( tempSide,
                                             hoseSubDivs,
                                             radius,
                                             tempHoseStart,
                                             tempHoseEnd,
                                             false );
                tempHose.GetArraysforVBO( out TemporaryMode, out TemporaryVBO, out TemporaryIBO );
                tempHose.Dispose();
                AllChunks.Add( new Chunk( ref TemporaryVBO, ref TemporaryIBO ) );
            }
            #endregion 12 sliced Hoses

            #region 6 quads for the sides

            Vertex[] tempVBO = new Vertex[4];
            uint[] tempIBO = new uint[6] { 0, 1, 2, 0, 2, 3 }; // all quads share this IBO

            // front face
            tempVBO[0].Normal = tempVBO[1].Normal = tempVBO[2].Normal = tempVBO[3].Normal = Vector3.UnitX;
            tempVBO[0].Position = FrontTopRightEdge + new Vector3( radius, 0.0f, 0.0f );
            tempVBO[1].Position = FrontBottomRightEdge + new Vector3( radius, 0.0f, 0.0f );
            tempVBO[2].Position = FrontBottomLeftEdge + new Vector3( radius, 0.0f, 0.0f);
            tempVBO[3].Position = FrontTopLeftEdge + new Vector3( radius, 0.0f, 0.0f );
            AllChunks.Add( new Chunk( ref tempVBO, ref tempIBO ) );

            // back face
            tempVBO[0].Normal = tempVBO[1].Normal = tempVBO[2].Normal = tempVBO[3].Normal = -Vector3.UnitX;
            tempVBO[0].Position = BackTopLeftEdge - new Vector3( radius, 0.0f, 0.0f );
            tempVBO[1].Position = BackBottomLeftEdge - new Vector3( radius, 0.0f, 0.0f );
            tempVBO[2].Position = BackBottomRightEdge - new Vector3( radius, 0.0f, 0.0f );
            tempVBO[3].Position = BackTopRightEdge - new Vector3( radius, 0.0f, 0.0f );
            AllChunks.Add( new Chunk( ref tempVBO, ref tempIBO ) );

            // top face
            tempVBO[0].Normal = tempVBO[1].Normal = tempVBO[2].Normal = tempVBO[3].Normal = Vector3.UnitY;
            tempVBO[0].Position = BackTopRightEdge + new Vector3( 0.0f, radius, 0.0f );
            tempVBO[1].Position = FrontTopRightEdge + new Vector3( 0.0f, radius, 0.0f );
            tempVBO[2].Position = FrontTopLeftEdge + new Vector3( 0.0f, radius, 0.0f);
            tempVBO[3].Position = BackTopLeftEdge + new Vector3( 0.0f, radius, 0.0f );
            AllChunks.Add( new Chunk( ref tempVBO, ref tempIBO ) );

            // bottom face
            tempVBO[0].Normal = tempVBO[1].Normal = tempVBO[2].Normal = tempVBO[3].Normal = -Vector3.UnitY;
            tempVBO[0].Position = BackBottomLeftEdge - new Vector3( 0.0f, radius, 0.0f );
            tempVBO[1].Position = FrontBottomLeftEdge - new Vector3( 0.0f, radius, 0.0f );
            tempVBO[2].Position = FrontBottomRightEdge - new Vector3( 0.0f, radius, 0.0f );
            tempVBO[3].Position = BackBottomRightEdge - new Vector3( 0.0f, radius, 0.0f );
            AllChunks.Add( new Chunk( ref tempVBO, ref tempIBO ) );

            // right face
            tempVBO[0].Normal = tempVBO[1].Normal = tempVBO[2].Normal = tempVBO[3].Normal = Vector3.UnitZ;
            tempVBO[0].Position = BackTopRightEdge + new Vector3( 0.0f, 0.0f, radius );
            tempVBO[1].Position = BackBottomRightEdge + new Vector3( 0.0f, 0.0f, radius );
            tempVBO[2].Position = FrontBottomRightEdge + new Vector3( 0.0f, 0.0f, radius );
            tempVBO[3].Position = FrontTopRightEdge + new Vector3( 0.0f, 0.0f, radius );
            AllChunks.Add( new Chunk( ref tempVBO, ref tempIBO ) );

            // left face
            tempVBO[0].Normal = tempVBO[1].Normal = tempVBO[2].Normal = tempVBO[3].Normal = -Vector3.UnitZ;
            tempVBO[0].Position = FrontTopLeftEdge - new Vector3( 0.0f, 0.0f, radius );
            tempVBO[1].Position = FrontBottomLeftEdge - new Vector3( 0.0f, 0.0f, radius );
            tempVBO[2].Position = BackBottomLeftEdge - new Vector3( 0.0f, 0.0f, radius );
            tempVBO[3].Position = BackTopLeftEdge - new Vector3( 0.0f, 0.0f, radius );
            AllChunks.Add( new Chunk( ref tempVBO, ref tempIBO ) );


            #endregion 6 quads for the sides

            #region Final Assembly of Chunks
            PrimitiveMode = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
            Chunk.GetArray( ref AllChunks, out VertexArray, out IndexArray );
            AllChunks.Clear();
            #endregion Final Assembly of Chunks
        }
    }
}
