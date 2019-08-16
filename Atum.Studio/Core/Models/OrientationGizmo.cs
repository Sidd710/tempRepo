using Atum.DAL.OS;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Core.Models
{
    internal class OrientationGizmo : STLModel3D
    {
        internal event EventHandler<SideSelectionType> OrientationChanged;

        internal enum SideSelectionType
        {
            None = 0,
            Top = 231,
            Left = 232,
            Right = 233,
            Bottom = 234,
            Front = 235,
            Back = 236,

            TopLeftFront = 240,
            TopRightFront = 241,
            TopLeftBack = 242,
            TopRightBack = 243,

            BottomLeftFront = 250,
            BottomRightFront = 251,
            BottomLeftBack = 252,
            BottomRightBack = 253

        }

        private List<Triangle> FaceText { get; set; }

        internal OrientationGizmo(float sizeCorrection, SideSelectionType highlightedSide, SideSelectionType selectedSide, bool updateFaceText = false)
        {
            this.HighlightedSide = highlightedSide;
            this.SelectedSide = selectedSide;

            this.Triangles = new TriangleInfoList();


            //right-front-top
            var orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Top, SideSelectionType.Front, SideSelectionType.Right, SideSelectionType.TopRightFront);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //right-back-top
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Top, SideSelectionType.Back, SideSelectionType.Right, SideSelectionType.TopRightBack);
            orientationGizmoPart.VerticalMirror(updateSurfacePlanes: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //left-front-top
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Top, SideSelectionType.Front, SideSelectionType.Left, SideSelectionType.TopLeftFront);
            orientationGizmoPart.HorizontalMirror(updateSurfacePlanes: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //left-back-top
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Top, SideSelectionType.Back, SideSelectionType.Left, SideSelectionType.TopLeftBack);
            orientationGizmoPart.HorizontalMirror(updateSurfacePlanes: false);
            orientationGizmoPart.VerticalMirror(updateSurfacePlanes: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //right-front-bottom
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Front, SideSelectionType.Bottom, SideSelectionType.Right, SideSelectionType.BottomRightFront);
            orientationGizmoPart.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, updateFaceColor: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //right-back-bottom
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Back, SideSelectionType.Bottom, SideSelectionType.Right, SideSelectionType.BottomRightBack);
            orientationGizmoPart.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, updateFaceColor: false);
            orientationGizmoPart.VerticalMirror(updateSurfacePlanes: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //left-front-bottom
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Front, SideSelectionType.Bottom, SideSelectionType.Left, SideSelectionType.BottomLeftFront);
            orientationGizmoPart.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, updateFaceColor: false);
            orientationGizmoPart.HorizontalMirror(updateSurfacePlanes: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            //left-back-bottom
            orientationGizmoPart = CalcOrientationGizmoPart(sizeCorrection, SideSelectionType.Back, SideSelectionType.Bottom, SideSelectionType.Left, SideSelectionType.BottomLeftBack);
            orientationGizmoPart.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, updateFaceColor: false);
            orientationGizmoPart.HorizontalMirror(updateSurfacePlanes: false);
            orientationGizmoPart.VerticalMirror(updateSurfacePlanes: false);
            this.Triangles[0].AddRange(orientationGizmoPart.Triangles[0]);

            UpdateFaceText(sizeCorrection);
            
            if (this.FaceText != null)
            {
                this.Triangles[0].AddRange(this.FaceText);
            }
            
            this.Loaded = true;

        }

        #region Face Text

        internal void UpdateFaceText(float sizeCorrection)
        {
            var defaultFontSize = 7.5f;
            var defaultFontColor = new Byte4Class((int)SideSelectionType.Top, 196, 196, 196 );
            this.FaceText = new List<Triangle>();

            if (OSProvider.IsWindows)
            {
                // front
                defaultFontColor.A = (int)SideSelectionType.Front;
                var faceText = new OrientationGizmoFaceText();
                faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("F", FontStyle.Regular, defaultFontSize * sizeCorrection);
                faceText.RotateText(-90, 0, 0, RotationEventArgs.TypeAxis.X);
                faceText.UpdateBoundries();
                faceText.UpdateDefaultCenter();
                faceText.MoveModelWithTranslationZ(new Vector3Class(0, -(sizeCorrection * 5) - (sizeCorrection / 2) - 0.01f, -(faceText.TopPoint / 2)));
                var faceTextClone = (STLModel3D)faceText.Clone(false);
                faceText.Triangles[0].Clear();
                foreach (var triangle in faceTextClone.Triangles[0])
                {
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = defaultFontColor;
                    triangle.Flip();
                    faceText.Triangles[0].Add(triangle);
                }

                this.FaceText.AddRange(faceText.Triangles[0]);

                //left
                defaultFontColor = new Byte4Class((int)SideSelectionType.Left, 196, 196, 196);
                faceText = new OrientationGizmoFaceText();
                faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("L", FontStyle.Regular, defaultFontSize * sizeCorrection);
                faceText.RotateText(-90, 0, 0, RotationEventArgs.TypeAxis.X);
                faceText.RotateText(-90,0, -90, RotationEventArgs.TypeAxis.Z);
                faceText.UpdateBoundries();
                faceText.UpdateDefaultCenter();
                faceText.MoveModelWithTranslationZ(new Vector3Class(-(sizeCorrection * 5) - (sizeCorrection / 2) - 0.01f, 0, -(faceText.TopPoint / 2)));
                faceTextClone = (STLModel3D)faceText.Clone(false);
                faceText.Triangles[0].Clear();
                foreach (var triangle in faceTextClone.Triangles[0])
                {
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = defaultFontColor;
                    triangle.Flip();
                    faceText.Triangles[0].Add(triangle);
                }

                this.FaceText.AddRange(faceText.Triangles[0]);

                //right
                defaultFontColor = new Byte4Class((int)SideSelectionType.Right, 196, 196, 196);
                faceText = new OrientationGizmoFaceText();
                faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("R", FontStyle.Regular, defaultFontSize * sizeCorrection);
                faceText.RotateText(-90, 0, 0, RotationEventArgs.TypeAxis.X);
                faceText.RotateText(-90, 0, 90, RotationEventArgs.TypeAxis.Z);
                faceText.UpdateBoundries();
                faceText.UpdateDefaultCenter();
                faceText.MoveModelWithTranslationZ(new Vector3Class((sizeCorrection * 5) + (sizeCorrection / 2) + 0.01f, 0, -(faceText.TopPoint / 2)));
                faceTextClone = (STLModel3D)faceText.Clone(false);
                faceText.Triangles[0].Clear();
                foreach (var triangle in faceTextClone.Triangles[0])
                {
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = defaultFontColor;
                    triangle.Flip();
                    faceText.Triangles[0].Add(triangle);
                }

                this.FaceText.AddRange(faceText.Triangles[0]);

                //back
                defaultFontColor = new Byte4Class((int)SideSelectionType.Back, 196, 196, 196);
                defaultFontColor.A = (int)SideSelectionType.Back;
                faceText = new OrientationGizmoFaceText();
                faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("B", FontStyle.Regular, defaultFontSize * sizeCorrection);
                faceText.RotateText(-90, 0, 0, RotationEventArgs.TypeAxis.X);
                faceText.RotateText(-90, 0, 180, RotationEventArgs.TypeAxis.Z);
                faceText.UpdateBoundries();
                faceText.UpdateDefaultCenter();
                faceText.MoveModelWithTranslationZ(new Vector3Class(0, (sizeCorrection * 5) + (sizeCorrection / 2) + 0.01f, -(faceText.TopPoint / 2)));
                faceTextClone = (STLModel3D)faceText.Clone(false);
                faceText.Triangles[0].Clear();
                foreach (var triangle in faceTextClone.Triangles[0])
                {
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = defaultFontColor;
                    triangle.Flip();
                    faceText.Triangles[0].Add(triangle);
                }

                this.FaceText.AddRange(faceText.Triangles[0]);

                //top
                defaultFontColor = new Byte4Class((int)SideSelectionType.Top, 196, 196, 196);
                faceText = new OrientationGizmoFaceText();
                faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("T", FontStyle.Regular, defaultFontSize * sizeCorrection);
                faceText.RotateText(180, 0, 0, RotationEventArgs.TypeAxis.X);
                //faceText.RotateText(0, 0, 180, RotationEventArgs.TypeAxis.Z);
                faceText.UpdateBoundries();
                faceText.UpdateDefaultCenter();
                faceText.MoveModelWithTranslationZ(new Vector3Class(0,0, (sizeCorrection * 5) + (sizeCorrection / 2) + 0.01f));
                faceTextClone = (STLModel3D)faceText.Clone(false);
                faceText.Triangles[0].Clear();
                foreach (var triangle in faceTextClone.Triangles[0])
                {
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = defaultFontColor;
                    triangle.Flip();

                    faceText.Triangles[0].Add(triangle);
                }

                this.FaceText.AddRange(faceText.Triangles[0]);

                //bottom
                defaultFontColor = new Byte4Class((int)SideSelectionType.Bottom, 196, 196, 196);
                faceText = new OrientationGizmoFaceText();
                faceText.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("B", FontStyle.Regular, defaultFontSize * sizeCorrection);
                faceText.RotateText(0, 0, 0, RotationEventArgs.TypeAxis.Z);
                faceText.UpdateBoundries();
                faceText.UpdateDefaultCenter();
                faceText.MoveModelWithTranslationZ(new Vector3Class(0, 0, -(sizeCorrection * 5) - (sizeCorrection / 2) - 0.01f));
                faceTextClone = (STLModel3D)faceText.Clone(false);
                faceText.Triangles[0].Clear();
                foreach (var triangle in faceTextClone.Triangles[0])
                {
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = defaultFontColor;
                    triangle.Flip();
                    faceText.Triangles[0].Add(triangle);
                }

                this.FaceText.AddRange(faceText.Triangles[0]);
            }
        }

        #endregion

        internal void Render(Matrix4 viewMatrix)
        {
            var dimensions = DAL.ApplicationSettings.Settings.OpenGLOrientationGizmoDimensions;
            if (this.Triangles != null)
            {
                GL.Disable(EnableCap.Lighting);
                GL.PushMatrix();

                GL.Viewport(frmStudioMain.SceneControl.Width - dimensions.Width - dimensions.X + 16, frmStudioMain.SceneControl.Height - dimensions.Height - dimensions.Y + 65, dimensions.Width, dimensions.Height);

                GL.MatrixMode(MatrixMode.Projection);
                Matrix4 ortho = Matrix4.CreateOrthographic(dimensions.Width, dimensions.Height, 10, 2500);
                GL.LoadMatrix(ref ortho);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                GL.LoadMatrix(ref viewMatrix);
                //GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.One);
                //GL.Enable(EnableCap.Blend);

                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                GL.EnableClientState(ArrayCap.ColorArray);
                GL.EnableClientState(ArrayCap.VertexArray);
                GL.EnableClientState(ArrayCap.NormalArray);

                try
                {
                    if (this.VBOIndexes != null)
                    {
                        var triangleIndex = 0;
                        foreach (var vboIndex in this.VBOIndexes)
                        {
                            //if (modelIndex > 1)
                            {
                                GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                                GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                                GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                                GL.DrawArrays(PrimitiveType.Triangles, 0, this.Triangles[triangleIndex].Count * 3);

                                triangleIndex++;
                            }
                        }

                    }
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }

                GL.End();

                GL.DisableClientState(ArrayCap.ColorArray);
                GL.DisableClientState(ArrayCap.VertexArray);
                GL.DisableClientState(ArrayCap.NormalArray);

                //GL.Disable(EnableCap.Blend);
                GL.Enable(EnableCap.Lighting);

                GL.PopMatrix();
            }
        }

        private STLModel3D CalcOrientationGizmoPart(float sizeCorrection, SideSelectionType firstSideSelectionType, SideSelectionType secondSideSelectionType, SideSelectionType thirdSideSelectionType, SideSelectionType cornerSelectionType)
        {
            var defaultSize = 5f * sizeCorrection;
            var defaultOffsetSize = defaultSize - (1f * sizeCorrection);
            var defaultOffsetSizeHalf = defaultSize + ((1f * sizeCorrection) / 2f);
            var result = new STLModel3D();
            result.Triangles = new TriangleInfoList();

            var cubePoints = new List<Vector3Class>();
            cubePoints.Add(new Vector3Class(0, 0, defaultOffsetSizeHalf)); //top
            cubePoints.Add(new Vector3Class(0, -defaultOffsetSize, defaultOffsetSizeHalf));   //top
            cubePoints.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSize, defaultOffsetSizeHalf));  //top
            cubePoints.Add(new Vector3Class(defaultOffsetSize, 0, defaultOffsetSizeHalf));  //top

            cubePoints.Add(new Vector3Class(0, -defaultOffsetSize, defaultOffsetSizeHalf)); //top
            cubePoints.Add(new Vector3Class(0, -defaultSize, defaultOffsetSizeHalf));   //top
            cubePoints.Add(new Vector3Class(defaultOffsetSize, -defaultSize, defaultOffsetSizeHalf));  //top
            cubePoints.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSize, defaultOffsetSizeHalf));  //top

            cubePoints.Add(new Vector3Class(defaultOffsetSize, 0, defaultOffsetSizeHalf)); //top
            cubePoints.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSize, defaultOffsetSizeHalf));   //top
            cubePoints.Add(new Vector3Class(defaultSize, -defaultOffsetSize, defaultOffsetSizeHalf));  //top
            cubePoints.Add(new Vector3Class(defaultSize, 0, defaultOffsetSizeHalf));  //top

            cubePoints.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSize, defaultOffsetSizeHalf)); //top
            cubePoints.Add(new Vector3Class(defaultOffsetSize, -defaultSize, defaultOffsetSizeHalf));   //top
            cubePoints.Add(new Vector3Class(defaultSize, -defaultOffsetSize, defaultOffsetSizeHalf));  //top

            var cubeColor = new Byte4Class((byte)(int)firstSideSelectionType, 57, 64, 69  );
            if (firstSideSelectionType == SelectedSide)
            {
                cubeColor = new Byte4Class((byte)(int)firstSideSelectionType, 255, 0, 0 );
            }
            else if (firstSideSelectionType == HighlightedSide)
            {
                cubeColor = new Byte4Class((byte)(int)firstSideSelectionType, 255, 255, 255);
            }

            var cubeIndexes = new List<short[]>
            {
                new short[]{0,1,2}, //front
                new short[]{2,3,0}, //front

                new short[]{4,5,6}, //front
                new short[]{6,7,4}, //front

                new short[]{8,9,10}, //front
                new short[]{10,11,8}, //front

                new short[]{12,13,14}, //front
            };


            var xAngles = new List<int>() { 0, -90 };

            foreach (var xAngle in xAngles)
            {

                if (xAngle == -90)
                {
                    if (secondSideSelectionType != SideSelectionType.Bottom)
                    {
                        cubeColor = new Byte4Class((byte)(int)secondSideSelectionType, 57, 64, 69);
                    }
                    else
                    {
                        cubeColor = new Byte4Class((byte)(int)secondSideSelectionType,128, 128, 128);
                    }

                    if (secondSideSelectionType == SelectedSide)
                    {
                        cubeColor = new Byte4Class((byte)(int)secondSideSelectionType, 255, 0, 0);
                    }
                    else if (secondSideSelectionType == HighlightedSide)
                    {
                        cubeColor = new Byte4Class((byte)(int)secondSideSelectionType, 255,255,255);
                    }
                }

                var tList = new List<Triangle>();
                var stlModel = new STLModel3D();
                stlModel.Triangles = new TriangleInfoList();

                foreach (var cubeIndex in cubeIndexes)
                {
                    var triangle = new Triangle();
                    triangle.Vectors[0].Position = cubePoints[cubeIndex[0]];
                    triangle.Vectors[1].Position = cubePoints[cubeIndex[1]];
                    triangle.Vectors[2].Position = cubePoints[cubeIndex[2]];
                    triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = cubeColor;
                    triangle.CalcNormal();
                    tList.Add(triangle);

                    if (xAngle == -90)
                    {
                        triangle.Flip(true);
                    }
                }

                stlModel.Triangles[0].AddRange(tList);
                stlModel.Rotate(xAngle, 0, 0, RotationEventArgs.TypeAxis.X, updateFaceColor: false);
                if (xAngle == -90)
                {
                    stlModel.MoveModelWithTranslationZ(new Vector3Class(0, -(2 * defaultOffsetSizeHalf), 0));
                }

                result.Triangles[0].AddRange(stlModel.Triangles[0]);
            }
            var tList2 = new List<Triangle>();
            var stlModel2 = new STLModel3D();
            stlModel2.Triangles = new TriangleInfoList();

            cubeColor = new Byte4Class((byte)(int)thirdSideSelectionType, 57, 64,69);
            if (thirdSideSelectionType == SelectedSide)
            {
                cubeColor = new Byte4Class((byte)(int)thirdSideSelectionType, 255, 0, 0);
            }
            else if (thirdSideSelectionType == HighlightedSide)
            {
                cubeColor = new Byte4Class((byte)(int)thirdSideSelectionType, 255, 255, 255);
            }

            foreach (var cubeIndex in cubeIndexes)
            {
                var triangle = new Triangle();
                triangle.Vectors[0].Position = cubePoints[cubeIndex[0]];
                triangle.Vectors[1].Position = cubePoints[cubeIndex[1]];
                triangle.Vectors[2].Position = cubePoints[cubeIndex[2]];
                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = cubeColor;
                triangle.Flip(true);
                tList2.Add(triangle);
            }

            stlModel2.Triangles[0].AddRange(tList2);
            stlModel2.Rotate(0, -90, 0, RotationEventArgs.TypeAxis.Y, updateFaceColor: false);
            stlModel2.MoveModelWithTranslationZ(new Vector3Class((2 * defaultOffsetSizeHalf), 0, 0));

            result.Triangles[0].AddRange(stlModel2.Triangles[0]);

            //highlight parts
            var highlightParts = new List<Vector3Class>();
            highlightParts.Add(new Vector3Class(0, -defaultSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(0, -defaultOffsetSizeHalf, defaultSize));
            highlightParts.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSizeHalf, defaultSize));

            highlightParts.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSizeHalf, defaultSize));
            highlightParts.Add(new Vector3Class(defaultOffsetSize, -defaultSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(0, -defaultSize, defaultOffsetSizeHalf));

            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultOffsetSize, defaultSize));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, 0f, defaultSize));

            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, 0f, defaultSize));
            highlightParts.Add(new Vector3Class(defaultSize, 0, defaultOffsetSizeHalf));

            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, defaultOffsetSize));
            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, 0f));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultSize, 0f));

            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultSize, 0f));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultSize, defaultOffsetSize));
            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, defaultOffsetSize));

            highlightParts.Add(new Vector3Class(defaultOffsetSize, -defaultSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(defaultOffsetSize, -defaultOffsetSizeHalf, defaultSize));
            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, defaultOffsetSize));

            highlightParts.Add(new Vector3Class(defaultOffsetSize, -defaultSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, defaultOffsetSize));
            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSize, defaultOffsetSizeHalf));

            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSize, defaultOffsetSizeHalf));
            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, defaultOffsetSize));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultOffsetSize, defaultSize));

            highlightParts.Add(new Vector3Class(defaultSize, -defaultOffsetSizeHalf, defaultOffsetSize));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultSize, defaultOffsetSize));
            highlightParts.Add(new Vector3Class(defaultOffsetSizeHalf, -defaultOffsetSize, defaultSize));

            var highlightColor = new Byte4Class((byte)(int)cornerSelectionType, 0, 0, 0);
            if (cornerSelectionType == SelectedSide)
            {
                highlightColor = new Byte4Class((byte)(int)cornerSelectionType, 255,0,0);
            }
            else if (cornerSelectionType == HighlightedSide)
            {
                highlightColor = new Byte4Class((byte)(int)cornerSelectionType, 255, 255, 255);
            }

            for (var highlightIndex = 0; highlightIndex < highlightParts.Count; highlightIndex += 3)
            {
                var triangle = new Triangle();
                triangle.Vectors[0].Position = highlightParts[highlightIndex];
                triangle.Vectors[1].Position = highlightParts[highlightIndex + 1];
                triangle.Vectors[2].Position = highlightParts[highlightIndex + 2];

                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = highlightColor;
                triangle.CalcNormal();
                result.Triangles[0].Add(triangle);
            }

            return result;
        }

        SideSelectionType HighlightedSide { get; set; }
        internal SideSelectionType SelectedSide { get; set; }

        internal void ProcessHighlight(MouseEventArgs e, Point point)
        {
            var pixel = new Byte4();
            GL.ReadPixels(e.X, frmStudioMain.SceneControl.Height - e.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);
            switch (pixel.A)
            {
                case (int)SideSelectionType.Front:
                    HighlightedSide = SideSelectionType.Front;
                    break;
                case (int)SideSelectionType.Left:
                    HighlightedSide = SideSelectionType.Left;
                    break;
                case (int)SideSelectionType.Right:
                    HighlightedSide = SideSelectionType.Right;
                    break;
                case (int)SideSelectionType.Top:
                    HighlightedSide = SideSelectionType.Top;
                    break;
                case (int)SideSelectionType.Back:
                    HighlightedSide = SideSelectionType.Back;
                    break;
                case (int)SideSelectionType.Bottom:
                    HighlightedSide = SideSelectionType.Bottom;
                    break;
                case (int)SideSelectionType.BottomLeftBack:
                    HighlightedSide = SideSelectionType.BottomLeftBack;
                    break;
                case (int)SideSelectionType.BottomLeftFront:
                    HighlightedSide = SideSelectionType.BottomLeftFront;
                    break;
                case (int)SideSelectionType.BottomRightBack:
                    HighlightedSide = SideSelectionType.BottomRightBack;
                    break;
                case (int)SideSelectionType.BottomRightFront:
                    HighlightedSide = SideSelectionType.BottomRightFront;
                    break;
                case (int)SideSelectionType.TopLeftBack:
                    HighlightedSide = SideSelectionType.TopLeftBack;
                    break;
                case (int)SideSelectionType.TopLeftFront:
                    HighlightedSide = SideSelectionType.TopLeftFront;
                    break;
                case (int)SideSelectionType.TopRightBack:
                    HighlightedSide = SideSelectionType.TopRightBack;
                    break;
                case (int)SideSelectionType.TopRightFront:
                    HighlightedSide = SideSelectionType.TopRightFront;
                    break;
            }

            ColorHighlightedSide();
        }

        internal void ProcessSelectedSide(MouseEventArgs e, Point point)
        {
            var pixel = new Byte4();
            GL.ReadPixels(e.X, frmStudioMain.SceneControl.Height - e.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);
            switch (pixel.A)
            {
                case (int)SideSelectionType.Front:
                    SelectedSide = SideSelectionType.Front;
                    break;
                case (int)SideSelectionType.Left:
                    SelectedSide = SideSelectionType.Left;
                    break;
                case (int)SideSelectionType.Right:
                    SelectedSide = SideSelectionType.Right;
                    break;
                case (int)SideSelectionType.Top:
                    SelectedSide = SideSelectionType.Top;
                    break;
                case (int)SideSelectionType.Back:
                    SelectedSide = SideSelectionType.Back;
                    break;
                case (int)SideSelectionType.Bottom:
                    SelectedSide = SideSelectionType.Bottom;
                    break;
                case (int)SideSelectionType.BottomLeftBack:
                    SelectedSide = SideSelectionType.BottomLeftBack;
                    break;
                case (int)SideSelectionType.BottomLeftFront:
                    SelectedSide = SideSelectionType.BottomLeftFront;
                    break;
                case (int)SideSelectionType.BottomRightBack:
                    SelectedSide = SideSelectionType.BottomRightBack;
                    break;
                case (int)SideSelectionType.BottomRightFront:
                    SelectedSide = SideSelectionType.BottomRightFront;
                    break;
                case (int)SideSelectionType.TopLeftBack:
                    SelectedSide = SideSelectionType.TopLeftBack;
                    break;
                case (int)SideSelectionType.TopLeftFront:
                    SelectedSide = SideSelectionType.TopLeftFront;
                    break;
                case (int)SideSelectionType.TopRightBack:
                    SelectedSide = SideSelectionType.TopRightBack;
                    break;
                case (int)SideSelectionType.TopRightFront:
                    SelectedSide = SideSelectionType.TopRightFront;
                    break;
            }

            ColorSelectedSide();

        }

        private void ColorSelectedSide()
        {
            this.Triangles = new OrientationGizmo(4, this.HighlightedSide, this.SelectedSide, false).Triangles;
            this.UpdateBinding();

            OrientationChanged?.Invoke(null, this.SelectedSide);
        }

        private void ColorHighlightedSide()
        {
            this.Triangles = new OrientationGizmo(4, this.HighlightedSide, this.SelectedSide, false).Triangles;
            this.UpdateBinding();

            frmStudioMain.SceneControl.Render();
        }
    }
}
