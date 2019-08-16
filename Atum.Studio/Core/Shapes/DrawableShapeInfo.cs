﻿using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using Atum.Studio.Core.Structs;
using System.ComponentModel;

namespace Atum.Studio.Core.Shapes
{
        [Serializable]
        public abstract class DrawableShapeInfo : IDisposable
        {
            protected PrimitiveType PrimitiveMode;
            internal Vertex[] VertexArray;
            protected uint[] IndexArray;

         
            #region Display List

            private bool UseDisplayList;
            private int DisplayListHandle = 0;

            #endregion Display List

            public DrawableShapeInfo(bool useDisplayList)
            {
                UseDisplayList = useDisplayList;
                PrimitiveMode = PrimitiveType.Triangles;
                VertexArray = null;
                IndexArray = null;
            }

                    #region Convert to VBO


            public void GetArraysforVBO(out PrimitiveType primitives, out VertexClass[] vertices, out uint[] indices)
        {
            primitives = PrimitiveMode;

            vertices = new VertexClass[VertexArray.Length];
            for (uint i = 0; i < VertexArray.Length; i++)
            {
                //vertices[i].Normal = VertexArray[i].Normal;
                vertices[i].Position = new Vector3Class( VertexArray[i].Position);
            }

            indices = IndexArray;
        }

        #endregion Convert to VBO


        
            private void DrawImmediateMode()
            {
                GL.Begin(PrimitiveMode);
                {
                    if (IndexArray == null)
                        foreach (Vertex v in VertexArray)
                        {
                           // GL.TexCoord2(v.TexCoord.X, v.TexCoord.Y);
                            //GL.Normal3(v..X, v.Normal.Y, v.Normal.Z);
                            GL.Vertex3(v.Position.X, v.Position.Y, v.Position.Z);
                        }
                    else
                    {
                        for (uint i = 0; i < IndexArray.Length; i++)
                        {
                            uint index = IndexArray[i];
                           // GL.TexCoord2(VertexArray[index].TexCoord.X, VertexArray[index].TexCoord.Y);
                           // GL.Normal3(VertexArray[index].Normal.X, VertexArray[index].Normal.Y, VertexArray[index].Normal.Z);
                            GL.Vertex3(VertexArray[index].Position.X, VertexArray[index].Position.Y, VertexArray[index].Position.Z);
                        }
                    }
                }
                GL.End();
            }

            /// <summary>
            /// Does not touch any state/matrices. Does call Begin/End and Vertex&Co.
            /// Creates and compiles a display list if not present yet. Requires an OpenGL context.
            /// </summary>
            public void Draw()
            {
                if (!UseDisplayList)
                    DrawImmediateMode();
                else
                    if (DisplayListHandle == 0)
                    {
                        if (VertexArray == null)
                            throw new Exception("Cannot draw null Vertex Array.");
                        DisplayListHandle = GL.GenLists(1);
                        GL.NewList(DisplayListHandle, ListMode.CompileAndExecute);
                        DrawImmediateMode();
                        GL.EndList();
                    }
                    else
                        GL.CallList(DisplayListHandle);
            }

            #region IDisposable Members

            /// <summary>
            /// Removes reference to VertexArray and IndexArray.
            /// Deletes the Display List, so it requires an OpenGL context.
            /// The instance is effectively destroyed.
            /// </summary>
            public void Dispose()
            {
                if (VertexArray != null)
                    VertexArray = null;
                if (IndexArray != null)
                    IndexArray = null;
                if (DisplayListHandle != 0)
                {
                    GL.DeleteLists(DisplayListHandle, 1);
                    DisplayListHandle = 0;
                }
            }

            #endregion
        }
    }