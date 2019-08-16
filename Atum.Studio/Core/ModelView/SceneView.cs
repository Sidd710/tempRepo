using System;
using OpenTK.Graphics.OpenGL;
using Atum.Studio.Core.Shapes;
using System.Drawing;
using OpenTK;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Managers;
using Atum.Studio.Properties;
using Atum.Studio.Core.Structs;
using Atum.Studio.Controls.OpenGL;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Diagnostics;
using System.Drawing.Imaging;
using Atum.DAL.Hardware;

namespace Atum.Studio.Core.ModelView
{
    public class SceneView
    {
        internal enum SceneViewRenderModeType
        {
            Solid = 0,
            Wireframe = 1,
            Hidden = 10
        }


        internal static SceneViewRenderModeType ModelRenderMode { get; set; }
        internal static SceneViewRenderModeType SupportRenderMode { get; set; }
        internal static SceneViewRenderModeType GroundPaneRenderMode { get; set; }

        internal static STLModel3D SceneBackground { get; set; }

        internal static OrbitCameraController OrbitCamera;
        internal static OrientationGizmo OrientationGizmo { get; set; }

        internal static Vector3Class CameraPosition = new Vector3Class();
        internal static Vector3Class CameraPositionTargetCenter = new Vector3Class();

        internal static SceneMoveTranslationGizmo MoveTranslation3DGizmo = new SceneMoveTranslationGizmo();
        internal static SceneRotationGizmo Rotation3DGizmo = new SceneRotationGizmo();
        internal static SceneScaleGizmo Scale3DGizmo = new SceneScaleGizmo();

        internal static MAGSAIMarkSelectionGizmo MAGSAISelectionMarker = new MAGSAIMarkSelectionGizmo();

        public static ViewMode CurrentViewMode = ViewMode.SelectObject;
        internal static ViewMode PreviousViewMode = ViewMode.SelectObject;

        private static float _defaultCameraDistance = 300;

        internal static Matrix4 ProjectionMatrix;

        public enum ViewMode
        {
            SelectObject = 0,
            Pan = 1,
            MoveTranslation = 2,
            Orbit = 3,
            Zoom = 4,
            ModelRotation = 5,
            ModelScale = 6,
            SimulationMode = 10,
            LayFlat = 29,
            Duplicate = 40,
            MagsAI = 41,
            MagsAIManualSupport = 42,
            MagsAIGridSupport = 43,
        }

        internal static void Init()
        {
        }

        internal static STLModel3D.TypeObject SelectedModelType
        {
            get
            {
                var selectedModelType = STLModel3D.TypeObject.None;
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && !(object3d is GroundPane))
                    {
                        var stlModel = object3d as STLModel3D;
                        if (stlModel.Selected)
                        {
                            selectedModelType = STLModel3D.TypeObject.Model;
                            break;
                        }

                        foreach (var supportCone in stlModel.TotalObjectSupportCones)
                        {
                            if (supportCone.Selected)
                            {
                                selectedModelType = STLModel3D.TypeObject.Support;
                            }
                        }
                    }
                }

                return selectedModelType;
            }
        }

        public static void RenderAsModelView(Controls.OpenGL.GLControl glControl, float cameraRotationX, float cameraRotationZ, float cameraZoom, float panX, float panY, bool thumbnailRendering = false, bool thumbnailLarge = false)
        {

            try
            {
                if (glControl.Context.IsCurrent)
                {

                    GL.Clear(ClearBufferMask.DepthBufferBit | ClearBufferMask.StencilBufferBit | ClearBufferMask.ColorBufferBit);

                    if (!thumbnailRendering)
                    {
                        DrawGradient(glControl);

                        //GL.ClearColor(Settings.Default.SceneBackgroundColor);
                        if (UserProfileManager.UserProfiles[0].SelectionOptions_Enable_XYZ_Axis)
                        {
                            DrawOrientationGizmo();
                        }
                    }
                    else
                    {
                        if (!thumbnailLarge)
                        {
                            GL.ClearColor(Color.White);
                        }
                        else
                        {

                            GL.ClearColor(Color.FromArgb(230, 230, 230, 230));
                        }
                    }

                    int w = glControl.Width;
                    int h = glControl.Height;

                    float aspectRatio = (float)w / (float)h;

                    GL.Viewport(0, 0, w, h);

                    ProjectionMatrix = Matrix4.CreatePerspectiveFieldOfView(0.55f, aspectRatio, 1, 2000);

                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadIdentity();
                    GL.LoadMatrix(ref ProjectionMatrix);

                    GL.MatrixMode(MatrixMode.Modelview);
                    GL.LoadIdentity();

                    OrbitCamera = new OrbitCameraController(CameraPositionTargetCenter);
                    OrbitCamera.MouseMove(cameraRotationX, cameraRotationZ);
                    OrbitCamera.Pan(panX, panY);
                    OrbitCamera.Scroll(_defaultCameraDistance + cameraZoom);

                    var lookat = OrbitCamera.GetCameraView();
                    CameraPosition = OrbitCamera.GetCameraPosition();

                    //GL.Matrix
                    GL.LoadMatrix(ref lookat);


                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        internal static void DrawOrientationGizmo()
        {
            try
            {
                if (OrbitCamera == null)
                {
                    OrbitCamera = new OrbitCameraController(new Vector3Class());
                }

                if (OrientationGizmo == null)
                {
                    OrientationGizmo = new OrientationGizmo(4, OrientationGizmo.SideSelectionType.None, OrientationGizmo.SideSelectionType.None, true);
                    OrientationGizmo.OrientationChanged += OrientationGizmo_OrientationChanged;
                    OrientationGizmo.BindModel();
                    OrientationGizmo.UpdateBinding();
                }

                OrientationGizmo.Render(OrbitCamera.GetCameraViewWithoutTranslation());
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        private static void OrientationGizmo_OrientationChanged(object sender, OrientationGizmo.SideSelectionType e)
        {
            Console.WriteLine(e);

            switch (e)
            {
                case OrientationGizmo.SideSelectionType.Front:
                    frmStudioMain.SceneControl.MoveCameraToPosition("0,270,0");
                    break;
                case OrientationGizmo.SideSelectionType.Right:
                    frmStudioMain.SceneControl.MoveCameraToPosition("270,270,0");
                    break;
                case OrientationGizmo.SideSelectionType.Left:
                    frmStudioMain.SceneControl.MoveCameraToPosition("90,270,0");
                    break;
                case OrientationGizmo.SideSelectionType.Back:
                    frmStudioMain.SceneControl.MoveCameraToPosition("180,270,0");
                    break;
                case OrientationGizmo.SideSelectionType.TopRightFront:
                    frmStudioMain.SceneControl.MoveCameraToPosition("315,315,0");
                    break;
                case OrientationGizmo.SideSelectionType.TopLeftFront:
                    frmStudioMain.SceneControl.MoveCameraToPosition("45,315,0");
                    break;
                case OrientationGizmo.SideSelectionType.TopRightBack:
                    frmStudioMain.SceneControl.MoveCameraToPosition("225,315,0");
                    break;
                case OrientationGizmo.SideSelectionType.TopLeftBack:
                    frmStudioMain.SceneControl.MoveCameraToPosition("135,315,0");
                    break;
                case OrientationGizmo.SideSelectionType.BottomRightFront:
                    frmStudioMain.SceneControl.MoveCameraToPosition("315,225,0");
                    break;
                case OrientationGizmo.SideSelectionType.BottomLeftFront:
                    frmStudioMain.SceneControl.MoveCameraToPosition("45,225,0");
                    break;
                case OrientationGizmo.SideSelectionType.BottomRightBack:
                    frmStudioMain.SceneControl.MoveCameraToPosition("225,225,0");
                    break;
                case OrientationGizmo.SideSelectionType.BottomLeftBack:
                    frmStudioMain.SceneControl.MoveCameraToPosition("135,225,0");
                    break;
                case OrientationGizmo.SideSelectionType.Top:
                    frmStudioMain.SceneControl.MoveCameraToPosition("0,0,0");
                    frmStudioMain.SceneControl.MoveCameraTargetCenterToNewPosition(new Vector3Class(0, 0, 0), true, 1f);
                    break;
                case OrientationGizmo.SideSelectionType.Bottom:
                    frmStudioMain.SceneControl.MoveCameraToPosition("0,180,0");
                    break;
            }
        }

        internal static void UpdateGroundPane(AtumPrinter selectedPrinter)
        {
            if (selectedPrinter != null)
            {
                var groundPaneObject = new GroundPane((selectedPrinter.ProjectorResolutionX / 10) * selectedPrinter.TrapeziumCorrectionFactorX, (selectedPrinter.ProjectorResolutionY / 10) * selectedPrinter.TrapeziumCorrectionFactorY, 0.02f);

                if (ObjectView.Objects3D.Count > 0 && ObjectView.Objects3D[0] is GroundPane)
                {
                    ((GroundPane)ObjectView.Objects3D[0]).Triangles = groundPaneObject.Triangles;
                    groundPaneObject = ((GroundPane)ObjectView.Objects3D[0]);
                }
                else
                {
                    ObjectView.Objects3D.Insert(0, groundPaneObject);
                }

                if (groundPaneObject.VBOIndexes == null)
                {
                    groundPaneObject.BindModel();
                }
                groundPaneObject.UpdateBinding();
            }
        }

        internal static void DrawGroundPane()
        {
            try
            {
                if (frmStudioMain.SceneControl.Context.IsCurrent)
                {
                    if (CameraPosition.Z > 1)
                    {
                        if (SceneView.GroundPaneRenderMode == SceneView.SceneViewRenderModeType.Solid)
                        {
                            //ground pane
                            GL.PushMatrix();
                            if (ObjectView.GroundPane != null)
                            {
                                GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);
                                //GL.Enable(EnableCap.Texture2D);
                                GL.EnableClientState(ArrayCap.ColorArray);
                                GL.EnableClientState(ArrayCap.VertexArray);
                                GL.EnableClientState(ArrayCap.NormalArray);

                                GL.BindBuffer(BufferTarget.ArrayBuffer, ObjectView.GroundPane.VBOIndexes[0]);
                                GL.ColorPointer(4, ColorPointerType.UnsignedByte, VertexClass.Stride, new IntPtr(0));

                                GL.VertexPointer(3, VertexPointerType.Float, VertexClass.Stride, 4);
                                GL.NormalPointer(NormalPointerType.Float, VertexClass.Stride, new IntPtr(16));
                                GL.DrawArrays(PrimitiveType.Triangles, 0, ObjectView.GroundPane.Triangles.TotalVerticesCount);

                                GL.DisableClientState(ArrayCap.ColorArray);
                                GL.DisableClientState(ArrayCap.VertexArray);
                                GL.DisableClientState(ArrayCap.NormalArray);
                                // GL.Disable(EnableCap.Texture2D);
                                //  GL.Disable(EnableCap.Light0);
                            }
                            GL.PopMatrix();
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
                //  DAL.Managers.LoggingManager.WriteToLog("XX", "exc", exc);
            }
        }

        internal static void DrawRotation3DGizmo()
        {
            try
            {
                GL.PushMatrix();

                if (Rotation3DGizmo == null)
                {
                    Rotation3DGizmo = new SceneRotationGizmo();
                }

                if (!Rotation3DGizmo.Hidden)
                {
                    //do translation
                    GL.Translate(Rotation3DGizmo.MoveTranslation.ToStruct());

                    GL.EnableClientState(ArrayCap.ColorArray);
                    GL.EnableClientState(ArrayCap.VertexArray);
                    GL.EnableClientState(ArrayCap.NormalArray);

                    if (Rotation3DGizmo.VBOIndexes != null)
                    {
                        var triangleArrayIndex = 0;
                        foreach (var vboIndex in Rotation3DGizmo.VBOIndexes)
                        {
                            GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                            GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                            GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                            GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                            GL.DrawArrays(PrimitiveType.Triangles, 0, Rotation3DGizmo.Triangles[triangleArrayIndex].Count * 3);

                            triangleArrayIndex++;
                        }

                        if (Rotation3DGizmo.RotationOverlay.VBOIndexes != null)
                        {
                            GL.Color4((int)Properties.Settings.Default.RotationGizmoAngleSelectionOverlay.R, Properties.Settings.Default.RotationGizmoAngleSelectionOverlay.G, Properties.Settings.Default.RotationGizmoAngleSelectionOverlay.B, 50);
                            GL.BlendFunc(BlendingFactorSrc.SrcAlpha, BlendingFactorDest.OneMinusSrcAlpha);
                            GL.Enable(EnableCap.Blend);

                            triangleArrayIndex = 0;
                            foreach (var vboIndex in Rotation3DGizmo.RotationOverlay.VBOIndexes)
                            {

                                GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                                GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                                GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                                GL.DrawArrays(PrimitiveType.Triangles, 0, Rotation3DGizmo.RotationOverlay.Triangles[triangleArrayIndex].Count * 3);

                                triangleArrayIndex++;
                            }

                            GL.Disable(EnableCap.Blend);
                        }

                    }

                    GL.DisableClientState(ArrayCap.NormalArray);
                    GL.DisableClientState(ArrayCap.VertexArray);
                    GL.DisableClientState(ArrayCap.ColorArray);

                    GL.Translate(-Rotation3DGizmo.MoveTranslation.ToStruct());
                }
                GL.PopMatrix();
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        internal static void DrawScale3DGizmo()
        {
            GL.PushMatrix();

            if (Scale3DGizmo == null)
            {
                Scale3DGizmo = new SceneScaleGizmo();
            }

            //do translation
            GL.Translate(MoveTranslation3DGizmo.MoveTranslation.ToStruct());

            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            if (MoveTranslation3DGizmo.VBOIndexes != null)
            {
                var triangleArrayIndex = 0;
                foreach (var vboIndex in MoveTranslation3DGizmo.VBOIndexes)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                    GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                    GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                    GL.DrawArrays(PrimitiveType.Triangles, 0, MoveTranslation3DGizmo.Triangles[triangleArrayIndex].Count * 3);

                    triangleArrayIndex++;
                }
            }

            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);

            GL.Translate(-MoveTranslation3DGizmo.MoveTranslation.ToStruct());

            GL.PopMatrix();
        }


        internal static void Reset3DGizmos()
        {
            if (MoveTranslation3DGizmo != null) MoveTranslation3DGizmo.UpdateControl(Enums.SceneViewSelectedMoveTranslationAxisType.Hidden, false);
            if (Rotation3DGizmo != null) Rotation3DGizmo.UpdateControl(Enums.SceneViewSelectedRotationAxisType.Hidden);
        }

        internal static void DrawMoveTranslation3DGizmo()
        {
            GL.PushMatrix();

            if (MoveTranslation3DGizmo == null)
            {
                MoveTranslation3DGizmo = new SceneMoveTranslationGizmo();
            }

            //do translation
            GL.Translate(MoveTranslation3DGizmo.MoveTranslation.ToStruct());

            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            if (MoveTranslation3DGizmo.VBOIndexes != null)
            {
                var triangleArrayIndex = 0;
                foreach (var vboIndex in MoveTranslation3DGizmo.VBOIndexes)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                    GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                    GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                    GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                    GL.DrawArrays(PrimitiveType.Triangles, 0, MoveTranslation3DGizmo.Triangles[triangleArrayIndex].Count * 3);

                    triangleArrayIndex++;
                }
            }

            GL.DisableClientState(ArrayCap.NormalArray);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.ColorArray);

            GL.Translate(-MoveTranslation3DGizmo.MoveTranslation.ToStruct());

            GL.PopMatrix();
        }

        private static void DrawSolidColorBackground(SceneGLControl sceneControl, System.Drawing.Color color)
        {
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.Begin(PrimitiveType.Triangles);
            GL.Color3(color);
            GL.Vertex3(-1.0, -1.0, 0.0);
            GL.Color3(color);
            GL.Vertex3(1.0, 1.0, 0.0);
            GL.Vertex3(-1.0, 1.0, 0.0);
            GL.Color3(color);
            GL.Vertex3(-1.0, -1.0, 0.0);
            GL.Vertex3(1.0, -1.0, 0.0);
            GL.Color3(color);
            GL.Vertex3(1.0, 1.0, 0.0);
            GL.End();
            GL.Ortho(0.0, (double)sceneControl.Width, 0.0, (double)sceneControl.Height, -1.0, 1.0);
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);
        }

        public static void ResetObjectsCache()
        {
        }

        public static void ChangeViewMode(ViewMode viewMode, SceneGLControl sceneControl, PictureBox buttonPressed)
        {
            ResetObjectsCache();

            CurrentViewMode = viewMode;


            switch (viewMode)
            {
                case ViewMode.ModelRotation:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnRotatePressed, buttonPressed, sceneControl);
                    break;
                case ViewMode.SelectObject:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnSelectPressed, buttonPressed, sceneControl);
                    SceneView.Reset3DGizmos();
                    sceneControl.RightMouseDown = false;
                    break;
                case ViewMode.MoveTranslation:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnMovePressed, buttonPressed, sceneControl);
                    break;
                case ViewMode.Pan:
                    sceneControl.RightMouseDown = false;
                    break;
                case ViewMode.Duplicate:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnModelActionDuplicate, buttonPressed, sceneControl);
                    break;
                case ViewMode.ModelScale:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnScalePressed, buttonPressed, sceneControl);
                    break;
                case ViewMode.MagsAI:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnModelActionMagsAI, buttonPressed, sceneControl);
                    sceneControl.RightMouseDown = false;
                    break;
                case ViewMode.MagsAIManualSupport:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnModelActionMagsAIManualSupport, buttonPressed, sceneControl);
                    sceneControl.RightMouseDown = false;
                    break;
                case ViewMode.MagsAIGridSupport:
                    SceneActionControlManager.ShowPanel(Enums.MainFormToolStripActionType.btnModelActionMagsAIGridSupport, buttonPressed, sceneControl);
                    sceneControl.RightMouseDown = false;
                    break;
                case ViewMode.LayFlat:
                    sceneControl.RightMouseDown = false;
                    break;
            }

            sceneControl.DrawSelectedTriangleRayTrace = true;
            sceneControl.Render(true);
        }


        static void DrawGradient(Controls.OpenGL.GLControl glControl)
        {
            if (SceneBackground == null)
            {
                SceneBackground = new STLModel3D();
                SceneBackground.Triangles = new TriangleInfoList();

                var gradientColor1 = new Byte4Class(255, 19, 20, 21);
                var gradientColor2 = new Byte4Class(255, 39, 42, 61);

                var triangle = new Triangle();
                triangle.Vectors[0].Position = new Vector3Class(-1f, -1f, 0f);
                triangle.Vectors[0].Color = gradientColor2;
                triangle.Vectors[1].Position = new Vector3Class(1f, 1f, 0);
                triangle.Vectors[1].Color = gradientColor1;
                triangle.Vectors[2].Position = new Vector3Class(-1f, 1f, 0);
                triangle.Vectors[2].Color = gradientColor1;

                SceneBackground.Triangles[0].Add(triangle);

                triangle = new Triangle();
                triangle.Vectors[0].Position = new Vector3Class(-1f, -1f, 0f);
                triangle.Vectors[0].Color = gradientColor2;
                triangle.Vectors[1].Position = new Vector3Class(1f, -1f, 0);
                triangle.Vectors[1].Color = gradientColor2;
                triangle.Vectors[2].Position = new Vector3Class(1f, 1f, 0);
                triangle.Vectors[2].Color = gradientColor1;

                SceneBackground.Triangles[0].Add(triangle);

                SceneBackground.Loaded = true;
                SceneBackground.BindModel();
                SceneBackground.UpdateBinding();
            }

            GL.Viewport(0, 0, glControl.Width, glControl.Height);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();
            GL.Disable(EnableCap.DepthTest);
            GL.Disable(EnableCap.Lighting);
            GL.PolygonMode(MaterialFace.Front, PolygonMode.Fill);

            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);

            foreach (var vboIndex in SceneBackground.VBOIndexes)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                GL.DrawArrays(PrimitiveType.Triangles, 0, SceneBackground.Triangles[0].Count * 3);
            }

            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.VertexArray);

            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.Lighting);

        }


        internal static void DrawMAGSAISelectionOverlay(STLModel3D selectedModel)
        {
            if (CurrentViewMode == ViewMode.MagsAI)
            {
                if (selectedModel.MAGSAISelectionOverlay == null)
                {
                    selectedModel.MAGSAISelectionOverlay = new STLModel3D();
                    selectedModel.MAGSAISelectionOverlay.Triangles = new TriangleInfoList();
                }

                if (selectedModel.MAGSAISelectionOverlay.VBOIndexes == null)
                {
                    selectedModel.MAGSAISelectionOverlay.BindModel();
                }

                if (selectedModel.MAGSAISelectionOverlay.Triangles[0].Count > 0)
                {
                   // GL.PushMatrix();
                    //GL.Translate(new Vector3(selectedModel.MoveTranslation.X, selectedModel.MoveTranslation.Y, 0));

                    //if (selectedModel.PreviewMoveTranslation != new Vector3Class())
                    //{
                    //    GL.Translate((selectedModel.PreviewMoveTranslation - new Vector3Class(0, 0, selectedModel.MoveTranslationZ)).ToStruct());
                    //}
                    //else
                    //{
                    //    GL.Translate(new Vector3(selectedModel.MoveTranslation.X, selectedModel.MoveTranslation.Y, 0));
                    //}

                    GL.PushAttrib(AttribMask.AllAttribBits);
                    GL.Enable(EnableCap.PolygonOffsetFill);
                    GL.PolygonOffset(-2.5f, -2.5f);
                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Line);
                    GL.LineWidth(3.0f);
                    GL.Color3(1f, 1f, 1f);

                    GL.EnableClientState(ArrayCap.VertexArray);
                    GL.EnableClientState(ArrayCap.NormalArray);

                    foreach (var vboIndex in selectedModel.MAGSAISelectionOverlay.VBOIndexes)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                        GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                        GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                        GL.DrawArrays(PrimitiveType.Triangles, 0, selectedModel.MAGSAISelectionOverlay.Triangles[0].Count * 3);
                    }

                    GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);
                    GL.Color3(0.8f, 0.8f, 0.8f);
                    foreach (var vboIndex in selectedModel.MAGSAISelectionOverlay.VBOIndexes)
                    {
                        GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                        GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                        GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                        GL.DrawArrays(PrimitiveType.Triangles, 0, selectedModel.MAGSAISelectionOverlay.Triangles[0].Count * 3);
                    }

                    GL.DisableClientState(ArrayCap.VertexArray);
                    GL.DisableClientState(ArrayCap.NormalArray);


                  //  GL.Translate(-new Vector3(selectedModel.MoveTranslation.X, selectedModel.MoveTranslation.Y, 0));
                    GL.PopAttrib();
                 //   GL.PopMatrix();
                }
            }
        }

        private static Rectangle GetOrbitGizmoBoundries()
        {
            var orbitOffsetXY = 50;
            var orbitBoundries = new Rectangle();
            var orientationGizmoDimensions = DAL.ApplicationSettings.Settings.OpenGLOrientationGizmoDimensions;

            orbitBoundries.X = frmStudioMain.SceneControl.Width - orientationGizmoDimensions.Width - orientationGizmoDimensions.X - orbitOffsetXY;
            orbitBoundries.Y = 0;
            orbitBoundries.Width = frmStudioMain.SceneControl.Width - orbitBoundries.X;
            orbitBoundries.Height = orientationGizmoDimensions.Height + orbitOffsetXY;

            return orbitBoundries;
        }

        internal static bool IsMousePositionWithinOrbitGizmoBoundries(Point mousePosition)
        {
            var orbitBoundries = GetOrbitGizmoBoundries();
            return orbitBoundries.Contains(mousePosition);
        }
    }


}
