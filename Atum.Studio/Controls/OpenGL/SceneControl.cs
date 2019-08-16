using System;
using System.Collections.Generic;
using System.Windows.Forms;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System.Drawing;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Utils;
using Atum.Studio.Core.Structs;
using static Atum.Studio.Core.Enums;
using Atum.Studio.Core.Engines;
using System.Diagnostics;
using Atum.Studio.Core.Managers.UndoRedo;
using static Atum.Studio.Core.Helpers.ContourHelper;
using Atum.Studio.Core.Helpers;
using System.Linq;
using Atum.Studio.Controls.NewGui.ContextMenu;
using System.Threading.Tasks;

namespace Atum.Studio.Controls.OpenGL
{
    public partial class SceneGLControl : GLControl
    {
        internal event Action<STLModel3D> SelectedObjectChanged;

        internal event EventHandler RenderCompleted;
        internal event EventHandler InvalidOpenGLVersion;

        internal event EventHandler ContextMenu_Undo;
        internal event EventHandler ContextMenu_DeleteModel;
        internal event EventHandler ContextMenu_DeleteSupportCone;


        internal event Action<float> RotationAxisXChanged;
        internal event Action<float> RotationAxisYChanged;
        internal event Action<float> RotationAxisZChanged;

        //internal event Action<Vector3> MoveTranslationChanged;
        internal event Action<Vector3Class> MoveTranslationCompleted;

        private DateTime _lastZoomInActionTimeStamp;
        private Vector3Class _lastZoomInCameraTarget;
        private Vector3Class _lastZoomInCameraTargetDeltaStep;
        private int _lastZoomInCameraTargetDeltaStepIndex;

        private DateTime _lastOrbitInActionTimeStamp;
        private Vector3Class _lastOrbitInCameraTarget;
        private Vector3Class _lastOrbitInCameraTargetDeltaStep;
        private int _lastOrbitInCameraTargetDeltaStepIndex;


        internal event EventHandler ToolTipSelectedModelMoveClicked;
        internal event EventHandler ToolTipSelectedModelRotateClicked;

        public ModelContextMenuStripControl ModelContextFormMenu;
        public SupportConeContextMenuStripControl SupportConeContextFormMenu;


        private TriangleIntersection _selectedTriangle = null;
        private TriangleIntersection _previousSelectedTriangle;

        private Vector3Class _objectMoveTranslation = new Vector3Class();
        private object _previousSelectedObject;

        public bool RightMouseDown { get; set; }
        public bool LeftMouseDown { get; set; }
        public bool _disableRendering { get; set; }

        public bool DrawSelectedTriangleRayTrace = false;

        private Vector3Class _groundIntersection = new Vector3Class();
        private Vector3Class _groundIntersectionPrevious = new Vector3Class();

        private bool _quickViewPan = false;
        internal bool QuickViewSelectTriangleIntersectionCross = false;
        private bool _createdCopyOfModel { get; set; }

        internal TriangleIntersection SelectedTriangle
        {
            get
            {
                return this._selectedTriangle;
            }
        }

        public int MousePositionXBeginSelectionBox { get; set; }
        public int MousePositionYBeginSelectionBox { get; set; }

        public int CurrentMousePositionX { get; set; }
        public int CurrentMousePositionY { get; set; }

        public int CurrentMouseMoveTranslationPositionY { get; set; }
        public int CurrentMouseRotationPositionX { get; set; }
        public int CurrentMouseRotationPositionY { get; set; }

        int _currentMouseZoomPositionY = 0;
        int _currentMousePanPositionX = 0;
        int _currentMousePanPositionY = 0;

        int _currentMouseRotationPositionX = 0;
        int _currentMouseRotationPositionY = 0;
        int _previousMouseRotationPositionX = 0;
        int _previousMouseRotationPositionY = 0;

        private bool _quickViewOrbit = false;
        public float CameraPanX { get; set; }
        public float CameraPanY { get; set; }
        private float _cameraRotationX = 0;
        private float _cameraRotationZ = 0;
        private float _cameraZoom = 1;

        public RenderModeType RenderMode { get; set; }

        public enum RenderModeType
        {
            Screen = 0,
            Thumbnail = 1
        }

        public SceneGLControl(Control parentControl) : base(GetBestGraphicsMode())
        {
            InitializeComponent();

            //add to parentControl
            parentControl.Controls.Add(this);

            this.MakeCurrent();
            this.Dock = DockStyle.Fill;
            this.InitializeOpenGLSettings();

            ModelContextFormMenu = new ModelContextMenuStripControl();
            ModelContextFormMenu.OnSelectMenuItem += ModelContextFormDropdownMenu_OnSelectMenuItem;

            SupportConeContextFormMenu = new SupportConeContextMenuStripControl();
            SupportConeContextFormMenu.OnSelectMenuItem += SupportConeContextFormDropdownMenu_OnSelectMenuItem;

            SelectedObjectChanged += SceneGLControl_SelectedObjectChanged;

        }

        private void SceneGLControl_SelectedObjectChanged(STLModel3D obj)
        {
            switch (SceneView.CurrentViewMode)
            {
                case SceneView.ViewMode.MoveTranslation:
                    if (SceneActionControlManager.ActionPanelMove != null)
                    {
                        var selectedLinkedClone = obj.LinkedClones.FirstOrDefault(s => s.Selected);
                        if (selectedLinkedClone != null)
                        {
                            SceneActionControlManager.ActionPanelMove.DataSource = selectedLinkedClone.Translation + new Vector3Class(0, 0, obj.BottomPoint);
                            SceneView.MoveTranslation3DGizmo.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
                        }
                        else
                        {
                            SceneActionControlManager.ActionPanelMove.DataSource = obj.MoveTranslation;
                        }
                    }
                    break;
                case SceneView.ViewMode.ModelScale:
                    SceneActionControlManager.ActionPanelScale.DataSource = obj;
                    break;
                case SceneView.ViewMode.ModelRotation:
                    SceneActionControlManager.ActionPanelRotate.RotateX = obj.RotationAngleX;
                    SceneActionControlManager.ActionPanelRotate.RotateY = obj.RotationAngleY;
                    SceneActionControlManager.ActionPanelRotate.RotateZ = obj.RotationAngleZ;
                    break;
            }
        }

        #region ToolTips

        private void SelectedModelToolTip_btnRotateClicked(object sender, EventArgs e)
        {
            this.ToolTipSelectedModelRotateClicked?.Invoke(null, null);
        }

        private void SelectedModelToolTip_btnMoveClicked(object sender, EventArgs e)
        {
            this.ToolTipSelectedModelMoveClicked?.Invoke(null, null);
        }

        #endregion


        private void InitializeOpenGLSettings()
        {
            if (!OpenTKHelper.IsValidVersion())
            {
                InvalidOpenGLVersion?.Invoke(null, null);
            }

            GL.ClearColor(.08f, .12f, .16f, 1f);
            GL.Enable(EnableCap.ColorMaterial);
            GL.Enable(EnableCap.DepthTest);
            GL.DepthFunc(DepthFunction.Less);
            GL.ClearDepth(1.0);

            GL.Enable(EnableCap.StencilTest);
            GL.ClearStencil(0);
            GL.StencilMask(0xFFFFFFFF); // read&write

            GL.Enable(EnableCap.CullFace);
            //  GL.FrontFace(FrontFaceDirection.Ccw);
            //GL.CullFace(CullFaceMode.Back);

            //GL.PolygonMode(MaterialFace.FrontAndBack, PolygonMode.Fill);

            GL.Enable(EnableCap.Lighting);
            GL.Enable(EnableCap.LineSmooth);
            GL.ShadeModel(ShadingModel.Smooth);

            GL.Enable(EnableCap.PolygonSmooth);
            GL.Hint(HintTarget.LineSmoothHint, HintMode.Nicest);
            GL.Hint(HintTarget.PolygonSmoothHint, HintMode.Nicest);

            GL.Enable(EnableCap.Light1);
            GL.Light(LightName.Light1, LightParameter.Position, new Vector4(0, 0, 500, 1f));
            GL.Light(LightName.Light1, LightParameter.Ambient, new Color4() { R = 0.35f, G = 0.35f, B = 0.35f, A = 1f });
            GL.Light(LightName.Light1, LightParameter.Diffuse, new Color4() { R = 0.2f, G = 0.2f, B = 0.2f, A = 1f });
            GL.Light(LightName.Light1, LightParameter.Specular, new Color4() { R = 0.25f, G = 0.25f, B = 0.25f, A = 1f });

            float[] mat_diffuse = { 1f, 1f, 1f, 1f };
            float[] mat_specular = { 1f, 1f, 1f, 0.1f };
            float[] mat_shininess = { 5f, 5f, 5f, 1f };
            GL.Material(MaterialFace.Front, MaterialParameter.Diffuse, mat_diffuse);
            GL.Material(MaterialFace.Front, MaterialParameter.Specular, mat_specular);
            GL.Material(MaterialFace.Front, MaterialParameter.Shininess, mat_shininess);

            //events
            this.MouseWheel += new MouseEventHandler(SceneControl_MouseWheel);
            this.MouseMove += new MouseEventHandler(SceneControl_MouseMove);
            this.MouseDown += new MouseEventHandler(SceneControl_MouseDown);
            this.MouseUp += new MouseEventHandler(SceneControl_MouseUp);
            this.MouseClick += new MouseEventHandler(SceneControl_MouseClick);

            MainFormManager.UpdateSceneControlEvents(this);

            //Application.Idle += Application_Idle;
        }

        #region Events


        void SelectObject(object sender, EventArgs e)
        {
            if (this != null && !this._disableRendering)
            {
                try
                {
                    this.MakeCurrent();

                    if (!this.RightMouseDown && !this.LeftMouseDown)
                    {

                        if ((SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport || SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIGridSupport || SceneView.CurrentViewMode == SceneView.ViewMode.LayFlat))
                        {
                            MarkManualSupportIntersection(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, null);
                        }

                        else if (SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation)
                        {
                            if (this != null && !this._disableRendering)
                            {
                                this._selectedTriangle = null;
                                Point pt = this.PointToClient(Control.MousePosition);
                                SceneView.Rotation3DGizmo.MarkSelectedAxis(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, this.Width, this.Height, pt);
                            }
                        }
                        else
                        {
                            SelectModel(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, false);
                        }

                        if (SceneView.CurrentViewMode == SceneView.ViewMode.MoveTranslation)
                        {
                            if (this != null && !this._disableRendering)
                            {
                                this._selectedTriangle = null;
                                Point pt = this.PointToClient(Control.MousePosition);
                                SceneView.MoveTranslation3DGizmo.MarkSelectedAxis(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, this.Width, this.Height, pt);
                            }
                        }
                    }

                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }
            }
            if ((SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport || SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIGridSupport || SceneView.CurrentViewMode == SceneView.ViewMode.LayFlat))
            {
                this.Render();
            }

        }

        private void ModelContextFormDropdownMenu_OnSelectMenuItem(object sender, EventArgs e)
        {
            var selectedContextMenuItem = (ContextMenuItem)sender;
            if (selectedContextMenuItem != null)
            {
                if (selectedContextMenuItem.ItemName == "Undo")
                {
                    ContextMenu_Undo?.Invoke(null, null);
                }
                else if (selectedContextMenuItem.ItemName == "Delete Model")
                {
                    ContextMenu_DeleteModel?.Invoke(null, null);
                }
            }
        }

        private void SupportConeContextFormDropdownMenu_OnSelectMenuItem(object sender, EventArgs e)
        {
            var selectedContextMenuItem = (ContextMenuItem)sender;
            if (selectedContextMenuItem != null)
            {
                if (selectedContextMenuItem.ItemName == "Undo")
                {
                    ContextMenu_Undo?.Invoke(null, null);
                }
                else if (selectedContextMenuItem.ItemName == "Delete Support Cone")
                {
                    ContextMenu_DeleteSupportCone?.Invoke(null, null);
                }
            }
        }

        void SceneControl_MouseClick(object sender, MouseEventArgs e)
        {
            //always remove printjob selection
            SceneControlToolbarManager.PrintJobPropertiesToolbar.DeselectPrintJobName();

            if (SceneView.IsMousePositionWithinOrbitGizmoBoundries(this.PointToClient(Control.MousePosition)))
            {
                if (!_quickViewOrbit)
                {
                    SceneView.OrientationGizmo.ProcessSelectedSide(e, this.PointToClient(Control.MousePosition));
                }
            }

            if ((SceneView.CurrentViewMode != SceneView.ViewMode.MagsAIManualSupport))
            {
                var currentSelectedModel = ObjectView.SelectedModel;

                //do detect new model when rotating model is busy
                if (currentSelectedModel != null && SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation &&
                    ((currentSelectedModel.PreviewRotationX != 0) || currentSelectedModel.PreviewRotationY != 0 || currentSelectedModel.PreviewRotationZ != 0))
                {


                }
                else
                {

                    //find selected model by click
                    SelectModel(e.X, e.Y, false);

                    currentSelectedModel = ObjectView.SelectedModel;

                    if (currentSelectedModel != null)
                    {
                        SceneControlToolbarManager.UpdateModelDimensions(currentSelectedModel.Width, currentSelectedModel.Depth, currentSelectedModel.Height);
                    }
                }


                if (SceneView.CurrentViewMode == SceneView.ViewMode.MoveTranslation)
                {
                    Control control = sender as Control;
                    Point pt = control.PointToClient(Control.MousePosition);
                    SelectModel(pt.X, pt.Y, false);
                    var selectedObject = ObjectView.SelectedObject;

                    if (this != null && !this._disableRendering)
                    {
                        this._selectedTriangle = null;
                        Point pt2 = this.PointToClient(Control.MousePosition);

                        if (SceneView.MoveTranslation3DGizmo.SelectedObject != selectedObject)
                        {
                            SceneView.MoveTranslation3DGizmo.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
                        }

                        SceneView.MoveTranslation3DGizmo.MarkSelectedAxis(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, this.Width, this.Height, pt2);
                    }
                }
                else if (SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation)
                {
                    if (this != null && !this._disableRendering)
                    {
                        this._selectedTriangle = null;
                        Point pt = this.PointToClient(Control.MousePosition);

                        if (SceneView.Rotation3DGizmo.SelectedObject != currentSelectedModel)
                        {
                            SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.None);
                        }
                        else if (currentSelectedModel.PreviewRotationX != 0 || currentSelectedModel.PreviewRotationY != 0 || currentSelectedModel.PreviewRotationZ != 0)
                        {
                            //         SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.None);
                        }
                        else
                        {
                            SceneView.Rotation3DGizmo.MarkSelectedAxis(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, this.Width, this.Height, pt);
                        }
                    }
                }
            }
            else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport)
            {
                if (this._selectedTriangle != null && this._selectedTriangle.IntersectionPoint != new Vector3Class())
                {
                    var selectedModel = ObjectView.SelectedModel;
                    if (selectedModel != null)
                    {
                        Console.WriteLine("Adding manual support cone");
                        Console.WriteLine("Adding manual support cone: Intersection point found");
                        Console.WriteLine("Ground point:" + this._selectedTriangle.IntersectionPoint);
                        var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;

                        var supportCone = SupportEngine.AddManualSupport(selectedModel, this._selectedTriangle, selectedMaterialSummary.Material);
                        selectedModel.UpdateSupportBasement();

                        if (supportCone != null)
                        {
                            var supportConeModel = new ManualSupportCone() { IsUndo = true, ModelIndex = selectedModel.Index };

                            PushSupportConeInStack(supportConeModel, selectedModel);

                            selectedModel.UpdateBinding();
                        }
                    }
                }

            }
            else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
            {
                var pixel = new Byte4();
                Control control = sender as Control;
                if (control != null)
                {
                    Point pt = control.PointToClient(Control.MousePosition);
                    GL.ReadPixels(pt.X, this.Height - pt.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);

                    SelectModel(pt.X, pt.Y, false);
                    MarkManualSupportIntersection(pt.X, pt.Y, ObjectView.SelectedModel);

                    if (e.Button == MouseButtons.Left)
                    {
                        var selectedModel = ObjectView.SelectedModel;
                        var selectedTriangles = SelectModelTrianglesByXYPoint(selectedModel);
                        if (selectedTriangles.Count > 0)
                        {
                            var selectedTriangleIndex = this._selectedTriangle.Index;
                            var connectedTriangles = TriangleHelper.GetConnectedMesh(selectedTriangleIndex, selectedTriangles, selectedModel.Triangles);
                            selectedModel.Triangles.ProcessSelectedOrientationTriangles(connectedTriangles, selectedModel.ColorAsByte4, selectedModel);
                            selectedModel.UpdateBinding();
                        }
                    }

                    else if (e.Button == MouseButtons.Right)
                    {
                        if (this._selectedTriangle != null)
                        {
                            var selectedModel = ObjectView.SelectedModel;
                            var selectedTriangles = SelectModelTrianglesByXYPoint(selectedModel);
                            var selectedTriangleIndex = this._selectedTriangle.Index;
                            var connectedTriangles = TriangleHelper.GetConnectedMesh(selectedTriangleIndex, selectedTriangles, selectedModel.Triangles);
                            selectedModel.Triangles.ProcessDeSelectedOrientationTriangles(connectedTriangles, selectedModel.ColorAsByte4, selectedModel);
                            selectedModel.UpdateBinding();
                        }
                    }

                    Render();
                }
            }

            if (e.Button == MouseButtons.Right && !_quickViewPan && !_quickViewOrbit)
            {
                //only when model is selected show context menu

                var selectedModel = ObjectView.SelectedObject;
                if (selectedModel != null && (!(selectedModel is GroundPane)) && UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode &&
                    (SceneView.CurrentViewMode != SceneView.ViewMode.MagsAI))
                {
                    if ((!(selectedModel is SupportCone)))
                    {
                        SupportConeContextFormMenu.Close(false);
                        ModelContextFormMenu.ShowOnMouseUp = true;
                    }
                    else
                    {
                        ModelContextFormMenu.Close(false);
                        SupportConeContextFormMenu.ShowOnMouseUp = true;
                    }
                }
            }

            MainFormManager.CloseMenu();
        }

        void SceneControl_MouseUp(object sender, MouseEventArgs e)
        {
            var selectedObject = ObjectView.SelectedObject;
            STLModel3D selectedModel = null;
            SupportCone selectedSupportCone = null;
            if (selectedObject != null && (!(selectedObject is SupportCone)))
            {
                selectedModel = selectedObject as STLModel3D;
            }
            else if (selectedObject != null && selectedObject is SupportCone)
            {
                selectedSupportCone = selectedObject as SupportCone;
            }

            if (e.Button == MouseButtons.Right)
            {
                if (selectedSupportCone != null)
                {
                    SupportConeContextFormMenu.Show();
                }
                else if (selectedModel != null)
                {
                    ModelContextFormMenu.Show();
                }
            }
            else
            {
                ModelContextFormMenu.Close();
                SupportConeContextFormMenu.Close();

                SceneActionControlManager.RemoveSupportPropertiesHandle();

                if (selectedSupportCone != null)
                {
                    SceneActionControlManager.AddSupportPropertiesHandle(e.Location, selectedSupportCone);
                }

            }

            if (SceneView.CurrentViewMode == SceneView.ViewMode.MoveTranslation)
            {

                if (selectedModel != null && selectedModel.PreviewMoveTranslation != new Vector3Class())
                {
                    //update support cone
                    MoveTranslationCompleted?.Invoke(new Vector3Class(selectedModel.PreviewMoveTranslation - selectedModel.MoveTranslation));

                    if (_objectMoveTranslation != new Vector3Class())
                    {
                        var totalMoveTranslation = -(this._objectMoveTranslation - selectedModel.MoveTranslation);
                        if (totalMoveTranslation != new Vector3Class())
                        {
                            selectedModel.UpdateSupportBasement();
                        }

                        this._objectMoveTranslation = new Vector3Class();
                    }
                }
            }
            else if ((this.LeftMouseDown || this._quickViewPan) && SceneView.CurrentViewMode == SceneView.ViewMode.Pan)
            {
            }
            else if ( SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation)
            {
                if (selectedSupportCone != null)
                {
                    selectedModel = selectedSupportCone.Model;
                }
                if (selectedModel != null && (selectedModel.PreviewRotationX != 0f || selectedModel.PreviewRotationY != 0f || selectedModel.PreviewRotationZ != 0f))
                {
                    Console.WriteLine("Drag mouse up / down to change the rotation angle");
                    //QuickTipManager.ShowTip("Quick Tip:", "Drag mouse up / down to \r\nchange the rotation angle");
                    switch (SceneView.Rotation3DGizmo.SelectedRotationAxis)
                    {
                        case SceneViewSelectedRotationAxisType.X:
                            if (SceneActionControlManager.ActionPanelRotate != null) SceneActionControlManager.ActionPanelRotate.RotateX = selectedModel.PreviewRotationX + selectedModel.RotationAngleX;
                            selectedModel.RotationAngleX += selectedModel.PreviewRotationX;
                            break;
                        case SceneViewSelectedRotationAxisType.Y:
                            if (SceneActionControlManager.ActionPanelRotate != null) SceneActionControlManager.ActionPanelRotate.RotateY = selectedModel.PreviewRotationY + selectedModel.RotationAngleY;
                            selectedModel.RotationAngleY += selectedModel.PreviewRotationY;
                            break;
                        case SceneViewSelectedRotationAxisType.Z:
                            if (SceneActionControlManager.ActionPanelRotate != null) SceneActionControlManager.ActionPanelRotate.RotateZ = selectedModel.PreviewRotationZ + selectedModel.RotationAngleZ;
                            selectedModel.RotationAngleZ += selectedModel.PreviewRotationZ;
                            break;
                        default:
                            Console.WriteLine("Selected axis undefined");
                            break;
                    }

                }
                else
                {
                    Console.WriteLine("No model selected");
                }
            }
            else
            {
                Console.WriteLine("Unknown action type");
            }

            //double check model preview properties
            this.RightMouseDown = false;
            this.LeftMouseDown = false;
            this.Cursor = Cursors.Default;

            this._groundIntersectionPrevious = new Vector3Class();

           

            if (this._quickViewOrbit || this._quickViewOrbit)
            {
                ModelContextFormMenu.Close();
                SupportConeContextFormMenu.Close();
            }

            this._quickViewPan = false;
            this._quickViewOrbit = false;
        }

        void SceneControl_MouseDown(object sender, MouseEventArgs e)
        {

            var previousSelectedObject = ObjectView.SelectedObject;
            if (SceneView.CurrentViewMode != SceneView.ViewMode.ModelRotation)
            {
                SelectModel(e.X, e.Y, false);
            }
            var currentSelectedObject = ObjectView.SelectedObject;

            SupportCone previousSelectedSupportCone = null;
            STLModel3D previousSelectedModel = null;
            SupportCone currentSelectedSupportCone = null;
            STLModel3D currentSelectedModel = null;
            TriangleSurfaceInfo currentSelectedSurface = null;

            if (previousSelectedObject is SupportCone)
            {
                previousSelectedSupportCone = previousSelectedObject as SupportCone;
                previousSelectedModel = previousSelectedSupportCone.Model;

            }
            else if (previousSelectedObject is STLModel3D)
            {
                previousSelectedModel = previousSelectedObject as STLModel3D;

            }
            else if (previousSelectedObject is TriangleSurfaceInfo)
            {
                previousSelectedModel = (STLModel3D)(previousSelectedObject as TriangleSurfaceInfo).Model;
            }

            if (currentSelectedObject is SupportCone)
            {
                currentSelectedSupportCone = currentSelectedObject as SupportCone;
                currentSelectedModel = currentSelectedSupportCone.Model;

            }
            else if (currentSelectedObject is STLModel3D)
            {
                currentSelectedModel = currentSelectedObject as STLModel3D;
            }
            else if (currentSelectedObject is TriangleSurfaceInfo)
            {
                currentSelectedSurface = (currentSelectedObject as TriangleSurfaceInfo);
                currentSelectedModel = currentSelectedSurface.Model as STLModel3D;
            }


            if (previousSelectedObject != null)
            {
                if (previousSelectedSupportCone != null && currentSelectedSupportCone != null && previousSelectedSupportCone != currentSelectedSupportCone)
                {
                    previousSelectedModel.DeselectObjects();

                    SelectedObjectChanged?.Invoke(currentSelectedSupportCone);
                }
                else if (previousSelectedModel != null && currentSelectedModel != null && previousSelectedModel != currentSelectedModel)
                {
                    previousSelectedModel.DeselectObjects();

                    //selected model changed
                    SelectedObjectChanged?.Invoke(currentSelectedModel);
                }
            }

            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
            var orientationGizmoDimensions = DAL.ApplicationSettings.Settings.OpenGLOrientationGizmoDimensions;

            if (currentSelectedModel != null && e.Button == MouseButtons.Right && SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
            {
                this.RightMouseDown = true;
                var selectedTriangles = SelectModelTrianglesByXYPoint(currentSelectedModel);
                if (selectedTriangles.Count == 0)
                {
                    //orbit initialize

                    if (this._cameraRotationZ >= 360) this._cameraRotationZ = this._cameraRotationZ - 360;
                    if (this._cameraRotationZ < 0) this._cameraRotationZ = 360 + this._cameraRotationZ;
                    if (this._cameraRotationX >= 360) this._cameraRotationX = this._cameraRotationX - 360;
                    if (this._cameraRotationX < 0) this._cameraRotationX = 360 + this._cameraRotationX;
                    this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                    this.CurrentMouseRotationPositionY = Control.MousePosition.Y;

                    this._quickViewOrbit = true;
                    this.RightMouseDown = true;
                }
            }
            else if (e.Button == MouseButtons.Right || (e.X > this.Width - orientationGizmoDimensions.Width - orientationGizmoDimensions.X && e.X <= this.Width - orientationGizmoDimensions.X && e.Y > orientationGizmoDimensions.Y && e.Y < orientationGizmoDimensions.Y + orientationGizmoDimensions.Height))
            {
                frmStudioMain.SceneControl.ModelContextFormMenu.Close();
                frmStudioMain.SceneControl.SupportConeContextFormMenu.Close();
                var cameraPositionTarget = UpdateCameraTargetPositionForOrbit(e.X, e.Y);
                OrbitMoveCameraTargetPosition(cameraPositionTarget);
                //OrbitModelSelectedCameraMove(cameraPositionTarget);
                this.RightMouseDown = true;

                this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                this.CurrentMouseRotationPositionY = Control.MousePosition.Y;

                //remove MAGS marker circle when 
                var mouseInOrbitGizmo = (e.X > this.Width - orientationGizmoDimensions.Width - orientationGizmoDimensions.X && e.X <= this.Width - orientationGizmoDimensions.X && e.Y > orientationGizmoDimensions.Y && e.Y < orientationGizmoDimensions.Y + orientationGizmoDimensions.Height);
                if (mouseInOrbitGizmo && SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                {
                    SceneView.MAGSAISelectionMarker.Hidden = true;
                }
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.SelectObject)
            {
                if (!this.LeftMouseDown)
                {
                    this.LeftMouseDown = true;
                    var relativeMousePosition = this.PointToClient(Control.MousePosition);
                    this.MousePositionXBeginSelectionBox = this.CurrentMousePositionX = relativeMousePosition.X;
                    this.MousePositionYBeginSelectionBox = this.CurrentMousePositionY = relativeMousePosition.Y;
                }
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.MoveTranslation)
            {
                //check if model is selected/clicked before moving
                var modelTypeClicked = STLModel3D.TypeObject.Model;

                var clickedModel = GetModelByXYMousePosition(e.X, e.Y, out modelTypeClicked);
                var selectedModel = ObjectView.SelectedModel;

                if (clickedModel == ObjectView.SelectedModel
                    || SceneView.MoveTranslation3DGizmo.SelectedMoveTranslationAxis == SceneViewSelectedMoveTranslationAxisType.X
                     || SceneView.MoveTranslation3DGizmo.SelectedMoveTranslationAxis == SceneViewSelectedMoveTranslationAxisType.Y
                 || SceneView.MoveTranslation3DGizmo.SelectedMoveTranslationAxis == SceneViewSelectedMoveTranslationAxisType.Z)
                {
                    this.LeftMouseDown = true;
                    this.CurrentMouseMoveTranslationPositionY = Control.MousePosition.Y;

                    if (currentSelectedModel != null)
                    {
                        var selectedLinkedClone = currentSelectedModel.LinkedClones.FirstOrDefault(s => s.Selected);
                        if (selectedLinkedClone != null)
                        {
                            if (SceneActionControlManager.IsActionPanelMoveVisible)
                            {
                                SceneActionControlManager.ActionPanelMove.DataSource = selectedLinkedClone.Translation + new Vector3Class(0, 0, currentSelectedModel.BottomPoint);
                            }
                        }

                        currentSelectedModel.PreviewMoveTranslation = currentSelectedModel.MoveTranslation;
                    }
                }
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.Pan)
            {
                this.LeftMouseDown = true;
                this._currentMousePanPositionX = Control.MousePosition.X;
                this._currentMousePanPositionY = Control.MousePosition.Y;

                SceneActionControlManager.RemoveSupportPropertiesHandle();
            }
            else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIGridSupport)
            {

                SceneActionControlManager.RemoveSupportPropertiesHandle();

                //Get selected triangle
                var origin = new Vector3Class();
                var direction = new Vector3Class();
                GetSceneControlOriginAndDirection(this.CurrentMousePositionX, this.CurrentMousePositionY, out origin, out direction);
                var selectedModel = ObjectView.SelectedModel;
                if (selectedModel != null)
                {
                    _selectedTriangle = TriangleHelper.CalcNearestTriangle(origin - new Vector3Class(selectedModel.MoveTranslation.X, selectedModel.MoveTranslation.Y, 0), direction, selectedModel, null);

                    if (_selectedTriangle != null)
                    {
                        //determine if triangle is part of surface
                        selectedModel.Triangles.SelectSurfaceByTriangle(_selectedTriangle);
                    }

                    var horizontalSurfaceIndex = 0;
                    foreach (var horizontalSurface in currentSelectedModel.Triangles.HorizontalSurfaces)
                    {
                        if (horizontalSurface.Selected)
                        {
                            horizontalSurface.SupportStructure.Clear();
                            horizontalSurface.SupportDistance = 3;
                            SupportEngine.CreateSurfaceSupport(horizontalSurface, currentSelectedModel, selectedMaterialSummary.Material, true);

                            var gridSupportConeModel = new GridSupportCone() { IsHorizontalSurface = true, IsUndo = true, ModelIndex = currentSelectedModel.Index };
                            gridSupportConeModel.SelectedSurfaceIndex = horizontalSurfaceIndex;
                            PushGridSupportConeInStack(gridSupportConeModel, currentSelectedModel);

                            break;

                        }

                        horizontalSurfaceIndex++;
                    }

                    var flatSurfaceIndex = 0;
                    foreach (var flatSurface in currentSelectedModel.Triangles.FlatSurfaces)
                    {
                        if (flatSurface.Selected)
                        {
                            flatSurface.SupportStructure.Clear();

                            flatSurface.SupportDistance = 3;
                            SupportEngine.CreateSurfaceSupport(flatSurface, currentSelectedModel, selectedMaterialSummary.Material, true);

                            var gridSupportConeModel = new GridSupportCone() { IsFlatSurface = true, IsUndo = true, ModelIndex = currentSelectedModel.Index };
                            gridSupportConeModel.SelectedSurfaceIndex = flatSurfaceIndex;
                            PushGridSupportConeInStack(gridSupportConeModel, currentSelectedModel);

                            break;
                        }
                    }

                    flatSurfaceIndex++;
                }
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.Orbit)
            {
                LeftMouseDown = true;
                _lastOrbitInCameraTarget = UpdateCameraTargetPositionForOrbit(e.X, e.Y);

                if (!UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode) this.RightMouseDown = true;

                this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                this.CurrentMouseRotationPositionY = Control.MousePosition.Y;
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.Zoom)
            {
                this.LeftMouseDown = true;
                this._currentMouseZoomPositionY = Control.MousePosition.Y;
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation)
            {
                this.LeftMouseDown = true;
                this._currentMouseRotationPositionX = e.X;
                this._currentMouseRotationPositionY = e.Y;
                this._previousMouseRotationPositionX = e.X;
                this._previousMouseRotationPositionY = e.Y;


                if (SceneView.Rotation3DGizmo.SelectedRotationAxis == SceneViewSelectedRotationAxisType.X)
                {
                    //QuickTipManager.ShowTip(new ModelRotationAxisXQuickTip());
                }
                else if (SceneView.Rotation3DGizmo.SelectedRotationAxis == SceneViewSelectedRotationAxisType.Y)
                {
                    //QuickTipManager.ShowTip(new ModelRotationAxisYQuickTip());
                }
                else if (SceneView.Rotation3DGizmo.SelectedRotationAxis == SceneViewSelectedRotationAxisType.Z)
                {
                    //QuickTipManager.ShowTip(new ModelRotationAxisZQuickTip());
                }
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
            {
                if (currentSelectedModel != null)
                {
                    this.LeftMouseDown = true;
                    var selectedTriangles = SelectModelTrianglesByXYPoint(currentSelectedModel);
                    if (selectedTriangles.Count > 0)
                    {
                        currentSelectedModel.Triangles.ProcessSelectedOrientationTriangles(selectedTriangles, currentSelectedModel.ColorAsByte4, currentSelectedModel);
                    }
                    else
                    {
                        //pan initialize
                        this._currentMousePanPositionX = Control.MousePosition.X;
                        this._currentMousePanPositionY = Control.MousePosition.Y;
                        this._quickViewPan = true;
                    }
                }
            }
            else if (e.Button == MouseButtons.Left && SceneView.CurrentViewMode == SceneView.ViewMode.LayFlat)
            {
                if (this._selectedTriangle != null)
                {
                    float zAngle; float yAngle;
                    VectorHelper.CalcRotationAnglesYZFromVector(this._selectedTriangle.Normal, true, out zAngle, out yAngle);
                    currentSelectedModel.Rotate(0, 0, currentSelectedModel.RotationAngleZ - zAngle, Core.Events.RotationEventArgs.TypeAxis.Z);
                    currentSelectedModel.Rotate(0, currentSelectedModel.RotationAngleY - yAngle, 0, Core.Events.RotationEventArgs.TypeAxis.Y);
                    currentSelectedModel.UpdateDefaultCenter();
                    currentSelectedModel.LiftModelOnSupport();
                    currentSelectedModel.UpdateBinding();
                }
            }
        }

        void SceneControl_MouseWheel(object sender, MouseEventArgs e)
        {
            if (!MainFormManager.IsInExportMode)
            {
                SceneActionControlManager.RemoveSupportPropertiesHandle();

                var cameraTargetPosition = UpdateCameraTargetPosition(e.X, e.Y);

                //when new target position found execute zoom behavior
                if (cameraTargetPosition != new Vector3Class())
                {
                    ZoomOnMouseWheel(e.Delta, cameraTargetPosition);
                }
                else
                {
                    ZoomOnMouseWheel(e.Delta, _lastZoomInCameraTarget);
                }
            }

        }

        private Vector3Class UpdateCameraTargetPosition(int mousePositionX, int mousePositionY)
        {
            var modelType = STLModel3D.TypeObject.Model;
            if ((DateTime.Now - _lastZoomInActionTimeStamp).TotalMilliseconds > 500)
            {
                var highlightedModel = GetModelByXYMousePosition(mousePositionX, mousePositionY, out modelType);

                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (!(object3d is GroundPane) && object3d is STLModel3D)
                    {
                        var stlModel = object3d as STLModel3D;
                        foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                        {
                            horizontalSurface.Selected = false;
                        }

                        foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                        {
                            flatSurface.Selected = false;
                        }
                    }
                }


                var newCameraTargetPosition = new Vector3Class();
                if (highlightedModel != null)
                {
                    //get raytrace
                    var origin = new Vector3Class();
                    var direction = new Vector3Class();

                    GetSceneControlOriginAndDirection(mousePositionX, mousePositionY, out origin, out direction);

                    var cameraTargetPositionTriangleIntersection = TriangleHelper.CalcNearestTriangle(origin - new Vector3Class(highlightedModel.MoveTranslation.X, highlightedModel.MoveTranslation.Y, 0), direction, highlightedModel, null);
                    if (cameraTargetPositionTriangleIntersection != null && cameraTargetPositionTriangleIntersection.IntersectionPoint != new Vector3Class())
                    {
                        newCameraTargetPosition = cameraTargetPositionTriangleIntersection.IntersectionPoint + highlightedModel.MoveTranslation;
                    }
                }

                //nothing found by model. Checking groundPane
                if (newCameraTargetPosition == new Vector3Class())
                {
                    var groundModel = ObjectView.GroundPane;
                    var groundIntersectionPoint = GetGroundIntersectionPoint(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, true);
                    if (groundIntersectionPoint != null)
                    {
                        newCameraTargetPosition = groundIntersectionPoint;
                    }
                    else
                    {
                        //no ground intersection
                        //determine to use current target center when background is selected
                        if ((DateTime.Now - _lastZoomInActionTimeStamp).TotalMilliseconds <= 500)
                        {
                            newCameraTargetPosition = _lastZoomInCameraTarget;
                        }
                        else
                        {
                            newCameraTargetPosition = SceneView.CameraPositionTargetCenter;
                        }
                    }
                }

                _lastZoomInCameraTarget = newCameraTargetPosition;
                return newCameraTargetPosition;
            }
            else
            {
                return _lastZoomInCameraTarget;
            }
        }

        internal void DrawSelectionBoxAndSelectItems()
        {
            if (frmStudioMain.SceneControl.Context.IsCurrent && !SceneView.MAGSAISelectionMarker.Hidden)
            {
                GL.Disable(EnableCap.DepthTest);
                GL.Disable(EnableCap.Lighting);
                GL.MatrixMode(MatrixMode.Projection);
                GL.LoadIdentity();
                GL.Ortho(0, this.Width, 0, this.Height, -1, 1);
                GL.Viewport(0, 0, this.Width, this.Height);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                GL.Color3(UserProfileManager.UserProfile.Settings_Studio_SelectionBox_Color_AsColor);
                GL.Begin(PrimitiveType.Lines);

                var circleCenterPoint = new Vector3(this.CurrentMousePositionX, this.Height - this.CurrentMousePositionY, 0f);

                //draw outline    
                for (var circleLineIndex = 0; circleLineIndex < SceneView.MAGSAISelectionMarker.OutlinePoints.Count; circleLineIndex++)
                {
                    GL.Vertex2(SceneView.MAGSAISelectionMarker.OutlinePoints[circleLineIndex]);

                    if (circleLineIndex == SceneView.MAGSAISelectionMarker.OutlinePoints.Count - 1)
                    {
                        GL.Vertex2(SceneView.MAGSAISelectionMarker.OutlinePoints[0]);
                    }
                    else
                    {
                        GL.Vertex2(SceneView.MAGSAISelectionMarker.OutlinePoints[circleLineIndex + 1]);
                    }
                }

                GL.End();

                //invert color
                var selectionCenterPointColor = Color.FromArgb(0, 255 - UserProfileManager.UserProfile.Settings_Studio_SelectionBox_Color_AsColor.R, 255 - UserProfileManager.UserProfile.Settings_Studio_SelectionBox_Color_AsColor.G, 255 - UserProfileManager.UserProfile.Settings_Studio_SelectionBox_Color_AsColor.B);
                GL.Color3(selectionCenterPointColor);
                GL.Begin(PrimitiveType.Lines);

                for (var circleLineIndex = 0; circleLineIndex < SceneView.MAGSAISelectionMarker.CrossPoints.Count; circleLineIndex += 2)
                {
                    GL.Vertex2(SceneView.MAGSAISelectionMarker.CrossPoints[circleLineIndex]);
                    GL.Vertex2(SceneView.MAGSAISelectionMarker.CrossPoints[circleLineIndex + 1]);
                }

                GL.End();

                GL.Enable(EnableCap.DepthTest);
                GL.Enable(EnableCap.Lighting);
            }
        }

        private void ZoomOnMouseWheel(int delta, Vector3Class selectedZoomTriangle)
        {
            float movePercent = 0.05f;
            if (UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode)
            {
                movePercent *= 3f;
            }
            this._cameraZoom -= (float)(delta * movePercent);

            //check to see it scroll is applied continues. If so don't change the target point
            if (_lastZoomInActionTimeStamp == null)
            {
                _lastZoomInCameraTarget = selectedZoomTriangle;

                _lastZoomInCameraTargetDeltaStep = (_lastZoomInCameraTarget - SceneView.CameraPositionTargetCenter) / 20;

                _lastZoomInCameraTargetDeltaStepIndex = 20;
                Debug.WriteLine("New Zoom target: " + _lastZoomInCameraTarget);
            }
            else if ((DateTime.Now - _lastZoomInActionTimeStamp).TotalMilliseconds <= 500)
            {
                if (_lastZoomInCameraTargetDeltaStepIndex > 0)
                {
                    _lastZoomInCameraTargetDeltaStepIndex--;
                }
                else
                {
                    //do not move camera target anymore after step amount
                    _lastZoomInCameraTargetDeltaStep = new Vector3Class();
                }

                Debug.WriteLine("Maintaining Zoom target: " + _lastZoomInCameraTarget);
            }
            else
            {

                _lastZoomInCameraTarget = selectedZoomTriangle;
                _lastZoomInCameraTargetDeltaStep = (_lastZoomInCameraTarget - SceneView.CameraPositionTargetCenter) / 20;
                _lastZoomInCameraTargetDeltaStepIndex = 20;
                Debug.WriteLine("Changing Zoom target: " + _lastZoomInCameraTarget);
            }


            MoveCameraTargetCenterToNewPosition(SceneView.CameraPositionTargetCenter + _lastZoomInCameraTargetDeltaStep, false);

            _lastZoomInActionTimeStamp = DateTime.Now;

            this.Render();

        }

        private Vector3Class UpdateCameraTargetPositionForOrbit(int mousePositionX, int mousePositionY)
        {
            var modelType = STLModel3D.TypeObject.Model;
            var selectedModel = GetModelByXYMousePosition(mousePositionX, mousePositionY, out modelType);

            foreach (var object3d in ObjectView.Objects3D)
            {
                if (!(object3d is GroundPane) && object3d is STLModel3D)
                {
                    var stlModel = object3d as STLModel3D;
                    foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                    {
                        horizontalSurface.Selected = false;
                    }

                    foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                    {
                        flatSurface.Selected = false;
                    }
                }
            }

            //get raytrace
            var origin = new Vector3Class();
            var direction = new Vector3Class();

            GetSceneControlOriginAndDirection(mousePositionX, mousePositionY, out origin, out direction);

            var cameraTargetPosition = new Vector3Class();

            //support
            if (selectedModel != null)
            {
                //  Debug.WriteLine("Find intersection point");
                origin -= new Vector3Class(selectedModel.MoveTranslationX, selectedModel.MoveTranslationY, 0);

                var nearestIntersectedTriangle = TriangleHelper.CalcNearestTriangle(origin, direction, selectedModel, null);
                if (nearestIntersectedTriangle != null)
                {
                    cameraTargetPosition = nearestIntersectedTriangle.IntersectionPoint;
                }

                if (cameraTargetPosition != new Vector3Class())
                {
                    if (selectedModel != null)
                    {
                        cameraTargetPosition += selectedModel.MoveTranslation;
                    }
                }
            }

            //nothing found by model intersection but model selected then use model center
            if (cameraTargetPosition == new Vector3Class() && selectedModel != null && !(selectedModel is SupportCone))
            {
                cameraTargetPosition = selectedModel.Center + selectedModel.MoveTranslation;
            }


            //nothing found by model. Checking groundPane
            if (cameraTargetPosition == new Vector3Class())
            {
                var groundIntersectionPoint = GetGroundIntersectionPoint(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, true);
                if (groundIntersectionPoint != null)
                {
                    cameraTargetPosition = groundIntersectionPoint;
                }
                else
                {
                    //no ground intersection
                    //determine to use current target center when background is selected
                    if ((DateTime.Now - _lastOrbitInActionTimeStamp).TotalMilliseconds <= 500)
                    {
                        cameraTargetPosition = _lastOrbitInCameraTarget;
                    }
                    else
                    {
                        cameraTargetPosition = SceneView.CameraPositionTargetCenter;
                    }
                }
            }

            return cameraTargetPosition;
        }
        private void OrbitMoveCameraTargetPosition(Vector3Class selectedOrbitTriangle)
        {
            SceneActionControlManager.RemoveSupportPropertiesHandle();

            //float movePercent = 0.05F;
            //this._cameraZoom -= (float)(delta * movePercent);


            //check to see it scroll is applied continues. If so don't change the target point
            if (_lastOrbitInActionTimeStamp == null)
            {
                _lastOrbitInCameraTarget = selectedOrbitTriangle;

                _lastOrbitInCameraTargetDeltaStep = (_lastOrbitInCameraTarget - SceneView.CameraPositionTargetCenter) / 20;

                _lastOrbitInCameraTargetDeltaStepIndex = 20;
                //Debug.WriteLine("New Orbit target: " + _lastOrbitInCameraTarget);
            }
            else if ((DateTime.Now - _lastOrbitInActionTimeStamp).TotalMilliseconds <= 500)
            {
                if (_lastOrbitInCameraTargetDeltaStepIndex > 0)
                {
                    _lastOrbitInCameraTargetDeltaStepIndex--;
                }
                else
                {
                    //do not move camera target anymore after step amount
                    _lastOrbitInCameraTargetDeltaStep = new Vector3Class();
                }

                //Debug.WriteLine("Maintaining Orbit target: " + _lastOrbitInCameraTarget);
            }
            else
            {

                _lastOrbitInCameraTarget = selectedOrbitTriangle;
                _lastOrbitInCameraTargetDeltaStep = (_lastOrbitInCameraTarget - SceneView.CameraPositionTargetCenter) / 20;
                ;
                _lastOrbitInCameraTargetDeltaStepIndex = 20;
                //Debug.WriteLine("Changing Orbit target: " + _lastOrbitInCameraTarget);
            }

            //Debug.WriteLine((DateTime.Now - _lastOrbitInActionTimeStamp).TotalMilliseconds);
            MoveCameraTargetCenterToNewPosition(SceneView.CameraPositionTargetCenter + _lastOrbitInCameraTargetDeltaStep, false);
            _lastOrbitInActionTimeStamp = DateTime.Now;
        }

        private Vector3Class GetGroundIntersectionPoint(int X, int Y, bool useColorCodeBackgroundAsExit)
        {
            int w = this.Width;
            int h = this.Height;
            float aspect = ((float)this.Width) / ((float)this.Height);

            int window_y = (h - Y) - h / 2;
            double norm_y = (double)(window_y) / (double)(h / 2);
            int window_x = X - w / 2;
            double norm_x = (double)(window_x) / (double)(w / 2);
            float near_height = .2825f; // no detectable error

            float y = (float)(near_height * norm_y);
            float x = (float)(near_height * aspect * norm_x);
            Vector4 ray_pnt = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            Vector4 ray_vec = new Vector4((float)x, (float)y, -1f, 0);
            ray_vec.Normalize();

            Matrix4 modelViewMatrix = SceneView.OrbitCamera.GetCameraView();
            Matrix4 viewInv = Matrix4.Invert(modelViewMatrix);

            Vector4 t_ray_pnt = new Vector4();
            Vector4 t_ray_vec = new Vector4();

            Vector4.Transform(ref ray_vec, ref viewInv, out t_ray_vec);
            Vector4.Transform(ref ray_pnt, ref viewInv, out t_ray_pnt);

            //color selection
            var pixel = new Byte4();
            Point pt = this.PointToClient(Control.MousePosition);

            GL.ReadPixels(pt.X, this.Height - pt.Y - 1, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);

            //background selected and validation

            var backgroundSelectionValidated = false;
            if (useColorCodeBackgroundAsExit && pixel.A == 200)
            {
                backgroundSelectionValidated = true;
            }
            else if (!useColorCodeBackgroundAsExit)
            {
                backgroundSelectionValidated = true;
            }

            var groundIntersectionPoint = new Vector3Class();
            if (backgroundSelectionValidated)
            {
                var origin = new Vector3Class(t_ray_pnt.X, t_ray_pnt.Y, t_ray_pnt.Z);
                var direction = new Vector3Class(t_ray_vec.X, t_ray_vec.Y, t_ray_vec.Z);

                var groundIntersection = IntersectionProvider.ISectGroundPlane(direction, origin);
                if (groundIntersection != null)
                {
                    groundIntersectionPoint = groundIntersection.intersect;
                }
            }

            return groundIntersectionPoint;
        }


        private int _previousMouseMoveXPosition;
        private int _previousMouseMoveYPosition;
        private void SceneControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.X != this.CurrentMousePositionX || e.Y != this.CurrentMousePositionY)
            {
                this.CurrentMousePositionX = e.X;
                this.CurrentMousePositionY = e.Y;
            }

            if (!this.Focused) { this.Focus(); }
            var currentMouseMoveXPosition = e.X;
            var currentMouseMoveYPosition = e.Y;
            this.CurrentMousePositionX = e.X;

            //mouse within orientation gizmo?

            if (SceneView.IsMousePositionWithinOrbitGizmoBoundries(e.Location))
            {
                if (RightMouseDown)
                {
                    if ((this.CurrentMouseRotationPositionX - Control.MousePosition.X < -5 || this.CurrentMouseRotationPositionX - Control.MousePosition.X > 5)
                        || (this.CurrentMouseRotationPositionY - Control.MousePosition.Y < -5 || this.CurrentMouseRotationPositionY - Control.MousePosition.Y > 5) ||
                                        _quickViewOrbit
                    )
                    {
                        _quickViewOrbit = true;
                        this._cameraRotationX -= (this.CurrentMouseRotationPositionX - Control.MousePosition.X);
                        this._cameraRotationZ -= (this.CurrentMouseRotationPositionY - Control.MousePosition.Y);

                        if (this._cameraRotationZ >= 360) this._cameraRotationZ = this._cameraRotationZ - 360;
                        if (this._cameraRotationZ < 0) this._cameraRotationZ = 360 + this._cameraRotationZ;
                        if (this._cameraRotationX >= 360) this._cameraRotationX = this._cameraRotationX - 360;
                        if (this._cameraRotationX < 0) this._cameraRotationX = 360 + this._cameraRotationX;
                        this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                        this.CurrentMouseRotationPositionY = Control.MousePosition.Y;

                        OrbitMoveCameraTargetPosition(new Vector3Class());
                    }
                    else
                    {
                        _quickViewOrbit = false;
                    }
                }
                else
                {
                    SceneView.OrientationGizmo.ProcessHighlight(e, this.PointToClient(Control.MousePosition));
                }

                SceneView.MAGSAISelectionMarker.Hidden = true;
            }
            else
            {
                SceneView.MAGSAISelectionMarker.Hidden = false;
                if (this._previousMouseMoveXPosition != currentMouseMoveXPosition || this._previousMouseMoveYPosition != currentMouseMoveYPosition)
                {
                    this._selectedTriangle = null;

                    var selectedModel = ObjectView.SelectedModel;
                    this._previousMouseMoveXPosition = currentMouseMoveXPosition;
                    this._previousMouseMoveYPosition = currentMouseMoveYPosition;


                    if (this.LeftMouseDown || this._quickViewPan)
                    {
                        switch (SceneView.CurrentViewMode)
                        {
                            case SceneView.ViewMode.Orbit:
                                this._cameraRotationX -= (this.CurrentMouseRotationPositionX - Control.MousePosition.X);
                                this._cameraRotationZ -= (this.CurrentMouseRotationPositionY - Control.MousePosition.Y);

                                if (this._cameraRotationZ >= 360) this._cameraRotationZ = this._cameraRotationZ - 360;
                                if (this._cameraRotationZ < 0) this._cameraRotationZ = 360 + this._cameraRotationZ;
                                if (this._cameraRotationX >= 360) this._cameraRotationX = this._cameraRotationX - 360;
                                if (this._cameraRotationX < 0) this._cameraRotationX = 360 + this._cameraRotationX;
                                this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                                this.CurrentMouseRotationPositionY = Control.MousePosition.Y;

                                Console.WriteLine("Current camera position: " + this._cameraRotationX + ":" + this._cameraRotationZ);

                                OrbitMoveCameraTargetPosition(_lastOrbitInCameraTarget);

                                this._quickViewOrbit = true;
                                break;
                            case SceneView.ViewMode.Pan:
                                this.Cursor = Cursors.Hand;
                                this.CameraPanX -= this._currentMousePanPositionX - Control.MousePosition.X;
                                this.CameraPanY -= this._currentMousePanPositionY - Control.MousePosition.Y;

                                this._currentMousePanPositionX = Control.MousePosition.X;
                                this._currentMousePanPositionY = Control.MousePosition.Y;
                                break;
                            case SceneView.ViewMode.Zoom:
                                this.Cursor = Cursors.Hand;
                                this._cameraZoom -= this._currentMouseZoomPositionY - Control.MousePosition.Y;

                                this._currentMouseZoomPositionY = Control.MousePosition.Y;
                                break;

                            case SceneView.ViewMode.MoveTranslation:

                                if (selectedModel != null && (!(selectedModel is SupportCone)))
                                {
                                    selectedModel = selectedModel as STLModel3D;
                                    var zDistance = (float)(this.CurrentMouseMoveTranslationPositionY - e.Y) / 10f;
                                    MoveTranslation(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, zDistance);
                                    if (SceneActionControlManager.ActionPanelMove != null)
                                    {
                                        //check if linkedclone is selected
                                        var selectedLinkedClone = selectedModel.LinkedClones.FirstOrDefault(s => s.Selected);
                                        if (selectedLinkedClone != null)
                                        {
                                            SceneActionControlManager.ActionPanelMove.DataSource = selectedLinkedClone.Translation + new Vector3Class(0, 0, selectedModel.PreviewMoveTranslation.Z);
                                        }
                                        else
                                        {
                                            SceneActionControlManager.ActionPanelMove.DataSource = selectedModel.PreviewMoveTranslation;
                                        }
                                    }

                                    this.CurrentMouseMoveTranslationPositionY = e.Y;
                                }

                                break;
                            case SceneView.ViewMode.ModelRotation:

                                if (ObjectView.SelectedModel != null && !(ObjectView.SelectedModel is SupportCone))
                                {
                                    var yDistance = this._currentMouseRotationPositionY - e.Y;
                                    var rotationDistance = -yDistance;
                                    var rotationAngle = (float)((rotationDistance) % 360);

                                    var stlModel = (STLModel3D)ObjectView.SelectedModel;
                                    switch (SceneView.Rotation3DGizmo.SelectedRotationAxis)
                                    {
                                        case SceneViewSelectedRotationAxisType.X:
                                            stlModel.PreviewRotationX += rotationAngle;
                                            if (stlModel.PreviewRotationX >= 360)
                                            {
                                                stlModel.PreviewRotationX -= 360;
                                            }
                                            else if (stlModel.PreviewRotationX < 0)
                                            {
                                                stlModel.PreviewRotationX += 360;
                                            }
                                            if (SceneActionControlManager.ActionPanelRotate != null) { SceneActionControlManager.ActionPanelRotate.RotateX = stlModel.PreviewRotationX + stlModel.RotationAngleX; }
                                            RotationAxisXChanged?.Invoke(stlModel.PreviewRotationX);
                                            break;
                                        case SceneViewSelectedRotationAxisType.Y:
                                            stlModel.PreviewRotationY += rotationAngle;
                                            if (stlModel.PreviewRotationY >= 360)
                                            {
                                                stlModel.PreviewRotationY -= 360;
                                            }
                                            else if (stlModel.PreviewRotationY < 0)
                                            {
                                                stlModel.PreviewRotationY += 360;
                                            }
                                            if (SceneActionControlManager.ActionPanelRotate != null) { SceneActionControlManager.ActionPanelRotate.RotateY = stlModel.PreviewRotationY + stlModel.RotationAngleY; }
                                            RotationAxisYChanged?.Invoke(stlModel.PreviewRotationY);

                                            break;
                                        case SceneViewSelectedRotationAxisType.Z:
                                            stlModel.PreviewRotationZ += rotationAngle;
                                            if (stlModel.PreviewRotationZ >= 360)
                                            {
                                                stlModel.PreviewRotationZ -= 360;
                                            }
                                            else if (stlModel.PreviewRotationZ < 0)
                                            {
                                                stlModel.PreviewRotationZ += 360;
                                            }
                                            if (SceneActionControlManager.ActionPanelRotate != null) { SceneActionControlManager.ActionPanelRotate.RotateZ = stlModel.PreviewRotationZ + stlModel.RotationAngleZ; }
                                            RotationAxisZChanged?.Invoke(stlModel.PreviewRotationZ);
                                            break;
                                    }

                                    this._currentMouseRotationPositionY = e.X;
                                    this._currentMouseRotationPositionY = e.Y;

                                }

                                break;
                            case SceneView.ViewMode.MagsAI: //select triangles
                                if (!this._quickViewPan)
                                {
                                    var selectedTriangles = SelectModelTrianglesByXYPoint(selectedModel);
                                    if (selectedTriangles.Count > 0)
                                    {
                                        var selectedTriangleIndex = this._selectedTriangle.Index;
                                        selectedModel = ObjectView.SelectedModel;
                                        var connectedTriangles = TriangleHelper.GetConnectedMesh(selectedTriangleIndex, selectedTriangles, selectedModel.Triangles);
                                        selectedModel.Triangles.ProcessSelectedOrientationTriangles(connectedTriangles, selectedModel.ColorAsByte4, selectedModel);
                                        selectedModel.UpdateBinding();
                                    }
                                }
                                else
                                {
                                    this.Cursor = Cursors.Hand;
                                    this.CameraPanX -= this._currentMousePanPositionX - Control.MousePosition.X;
                                    this.CameraPanY -= this._currentMousePanPositionY - Control.MousePosition.Y;

                                    this._currentMousePanPositionX = Control.MousePosition.X;
                                    this._currentMousePanPositionY = Control.MousePosition.Y;
                                }
                                break;
                        }
                    }
                    else if (this.RightMouseDown)
                    {
                        switch (SceneView.CurrentViewMode)
                        {
                            case SceneView.ViewMode.MagsAI: //select triangles

                                if (!this._quickViewOrbit)
                                {
                                    var selectedTriangles = SelectModelTrianglesByXYPoint(selectedModel);
                                    if (selectedTriangles.Count > 0)
                                    {
                                        Debug.WriteLine("Selected triangles count: " + selectedTriangles.Count);

                                        var selectedTriangleIndex = this._selectedTriangle.Index;
                                        selectedModel = ObjectView.SelectedModel;
                                        var connectedTriangles = Core.Helpers.TriangleHelper.GetConnectedMesh(selectedTriangleIndex, selectedTriangles, selectedModel.Triangles);
                                        selectedModel.Triangles.ProcessDeSelectedOrientationTriangles(connectedTriangles, selectedModel.ColorAsByte4, selectedModel);
                                        selectedModel.UpdateBinding();
                                    }
                                }
                                else
                                {
                                    this._cameraRotationX -= (this.CurrentMouseRotationPositionX - Control.MousePosition.X);
                                    this._cameraRotationZ -= (this.CurrentMouseRotationPositionY - Control.MousePosition.Y);

                                    if (this._cameraRotationZ >= 360) this._cameraRotationZ = this._cameraRotationZ - 360;
                                    if (this._cameraRotationZ < 0) this._cameraRotationZ = 360 + this._cameraRotationZ;
                                    if (this._cameraRotationX >= 360) this._cameraRotationX = this._cameraRotationX - 360;
                                    if (this._cameraRotationX < 0) this._cameraRotationX = 360 + this._cameraRotationX;
                                    this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                                    this.CurrentMouseRotationPositionY = Control.MousePosition.Y;
                                }

                                break;

                            default:

                                this._cameraRotationX -= (this.CurrentMouseRotationPositionX - Control.MousePosition.X);
                                this._cameraRotationZ -= (this.CurrentMouseRotationPositionY - Control.MousePosition.Y);

                                if (this._cameraRotationZ >= 360) this._cameraRotationZ = this._cameraRotationZ - 360;
                                if (this._cameraRotationZ < 0) this._cameraRotationZ = 360 + this._cameraRotationZ;
                                if (this._cameraRotationX >= 360) this._cameraRotationX = this._cameraRotationX - 360;
                                if (this._cameraRotationX < 0) this._cameraRotationX = 360 + this._cameraRotationX;
                                this.CurrentMouseRotationPositionX = Control.MousePosition.X;
                                this.CurrentMouseRotationPositionY = Control.MousePosition.Y;

                                Console.WriteLine("Current camera position: " + this._cameraRotationX + ":" + this._cameraRotationZ);

                                OrbitMoveCameraTargetPosition(_lastOrbitInCameraTarget);

                                this._quickViewOrbit = true;
                                break;

                        }
                    }
                    else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                    {
                        SceneView.MAGSAISelectionMarker.UpdateMousePosition(new Point(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition), this.Width, this.Height);
                    }
                    else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport)
                    {
                        var origin = new Vector3Class();
                        var direction = new Vector3Class();
                        GetSceneControlOriginAndDirection(currentMouseMoveXPosition, currentMouseMoveYPosition, out origin, out direction);
                        if (selectedModel != null)
                        {
                            _selectedTriangle = TriangleHelper.CalcNearestTriangle(origin - new Vector3Class(selectedModel.MoveTranslation.X, selectedModel.MoveTranslation.Y, 0), direction, selectedModel, null);
                        }
                    }
                }

                this.Render();

            }
        }

        private Dictionary<TriangleConnectionInfo, bool> SelectModelTrianglesByXYPoint(STLModel3D selectedModel)
        {
            var selectedTriangles = new Dictionary<TriangleConnectionInfo, bool>();
            if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
            {
                SceneView.MAGSAISelectionMarker.UpdateMousePosition(new Point(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition), this.Width, this.Height);

                if ((this.LeftMouseDown || this.RightMouseDown) && !this._quickViewPan && !this._quickViewOrbit)
                {

                    MarkManualSupportIntersection(this._previousMouseMoveXPosition, this._previousMouseMoveYPosition, selectedModel);
                    if (_selectedTriangle != null)
                    {
                        var point2d = GetScreenPointFromVector3(this._selectedTriangle.IntersectionPoint, this.Width, this.Height);
                        var totalPointsWithinSelectionBox = 0;
                        var selectionBoxGizmo = SceneView.MAGSAISelectionMarker;
                        var selectionBoxGizmoAsPath = new List<IntPoint>();
                        for (var pointIndex = 0; pointIndex < selectionBoxGizmo.OutlinePoints.Count; pointIndex++)
                        {
                            selectionBoxGizmoAsPath.Add(new IntPoint(selectionBoxGizmo.OutlinePoints[pointIndex].X, selectionBoxGizmo.OutlinePoints[pointIndex].Y));
                        }

                        var selectionBoxPolyTree = new PolyTree();
                        selectionBoxPolyTree.m_polygon = selectionBoxGizmoAsPath;


                        var cameraVector = SceneView.CameraPosition;
                        if (selectedModel != null)
                        {

                            var processedSibblingTriangles = new Dictionary<TriangleConnectionInfo, bool>();
                            processedSibblingTriangles.Add(this._selectedTriangle.Index, false);

                            var sibblingTriangles = new Dictionary<TriangleConnectionInfo, bool>();
                            sibblingTriangles.Add(this._selectedTriangle.Index, false);

                            var newSibblingTriangles = new Dictionary<TriangleConnectionInfo, bool>();
                            while (sibblingTriangles.Count > 0)
                            {
                                //process childs of sibblings
                                foreach (var sibblingTriangleIndex in sibblingTriangles.Keys)
                                {
                                    foreach (var newSibblingChildIndex in selectedModel.Triangles.GetConnectedTriangles(sibblingTriangleIndex))
                                    {
                                        if (!newSibblingTriangles.ContainsKey(newSibblingChildIndex) && !processedSibblingTriangles.ContainsKey(newSibblingChildIndex))
                                        {
                                            var sibblingTriangle = selectedModel.Triangles[newSibblingChildIndex.ArrayIndex][newSibblingChildIndex.TriangleIndex];

                                            //first check normal same direction as camera
                                            if (VectorHelper.IsSameDirection(cameraVector, sibblingTriangle.Normal))
                                            {
                                                var triangleVectorScreenPoints = new Point[]
                                                {
                                                    GetScreenPointFromVector3(sibblingTriangle.Vectors[0].Position + new Vector3Class(selectedModel.MoveTranslation.Xy), this.Width, this.Height),
                                                    GetScreenPointFromVector3(sibblingTriangle.Vectors[1].Position + new Vector3Class(selectedModel.MoveTranslation.Xy), this.Width, this.Height),
                                                    GetScreenPointFromVector3(sibblingTriangle.Vectors[2].Position + new Vector3Class(selectedModel.MoveTranslation.Xy), this.Width, this.Height)
                                                };

                                                var triangleVectorScreenPointsAsIntPoints = new List<IntPoint>();
                                                for (var screenPointIndex = 0; screenPointIndex < 3; screenPointIndex++)
                                                {
                                                    var triangleScreenPointAsIntPoint = new IntPoint(triangleVectorScreenPoints[screenPointIndex].X, this.Height - triangleVectorScreenPoints[screenPointIndex].Y);
                                                    triangleVectorScreenPointsAsIntPoints.Add(triangleScreenPointAsIntPoint);

                                                    if (Clipper.PointInPolygon(triangleScreenPointAsIntPoint, selectionBoxGizmoAsPath) != 0)
                                                    {
                                                        //on or inside selectionbox
                                                        totalPointsWithinSelectionBox++;

                                                        newSibblingTriangles.Add(newSibblingChildIndex, false);
                                                        break;
                                                    }
                                                }

                                                var pointExistsinPolygon = false;
                                                foreach (var triangleVectorScreenPointsAsIntPoint in triangleVectorScreenPointsAsIntPoints)
                                                {
                                                    if (Clipper.PointInPolygon(triangleVectorScreenPointsAsIntPoint, selectionBoxGizmoAsPath) != 0)
                                                    {
                                                        pointExistsinPolygon = true;
                                                        break;
                                                    }
                                                }

                                                if (!pointExistsinPolygon)
                                                {
                                                    foreach (var selectionBoxPoint in selectionBoxGizmoAsPath)
                                                    {
                                                        if (Clipper.PointInPolygon(selectionBoxPoint, triangleVectorScreenPointsAsIntPoints) != 0)
                                                        {
                                                            pointExistsinPolygon = true;
                                                            break;
                                                        }
                                                    }
                                                }

                                                if (pointExistsinPolygon)
                                                {
                                                    totalPointsWithinSelectionBox++;

                                                    if (!newSibblingTriangles.ContainsKey(newSibblingChildIndex))
                                                    {
                                                        newSibblingTriangles.Add(newSibblingChildIndex, false);
                                                    }
                                                }

                                            }
                                        }
                                    }
                                }

                                sibblingTriangles.Clear();
                                foreach (var newSibblingTriangle in newSibblingTriangles.Keys)
                                {
                                    sibblingTriangles.Add(newSibblingTriangle, false);
                                    processedSibblingTriangles.Add(newSibblingTriangle, false);
                                }

                                newSibblingTriangles = new Dictionary<TriangleConnectionInfo, bool>();
                            }

                            selectedTriangles = processedSibblingTriangles;

                        }
                    }

                }
            }

            return selectedTriangles;
        }

        private Vector3Class GetGroundIntersectionPoint(int X, int Y)
        {
            Vector3Class groundIntersectionPoint = null;
            int w = this.Width;
            int h = this.Height;
            float aspect = ((float)this.Width) / ((float)this.Height);

            int window_y = (h - Y) - h / 2;
            double norm_y = (double)(window_y) / (double)(h / 2);
            int window_x = X - w / 2;
            double norm_x = (double)(window_x) / (double)(w / 2);
            float near_height = .2825f; // no detectable error

            float y = (float)(near_height * norm_y);
            float x = (float)(near_height * aspect * norm_x);
            Vector4 ray_pnt = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            Vector4 ray_vec = new Vector4((float)x, (float)y, -1f, 0);
            ray_vec.Normalize();

            Matrix4 modelViewMatrix = SceneView.OrbitCamera.GetCameraView();
            Matrix4 viewInv = Matrix4.Invert(modelViewMatrix);

            Vector4 t_ray_pnt = new Vector4();
            Vector4 t_ray_vec = new Vector4();

            Vector4.Transform(ref ray_vec, ref viewInv, out t_ray_vec);
            Vector4.Transform(ref ray_pnt, ref viewInv, out t_ray_pnt);

            var origin = new Vector3Class(t_ray_pnt.X, t_ray_pnt.Y, t_ray_pnt.Z);
            var direction = new Vector3Class(t_ray_vec.X, t_ray_vec.Y, t_ray_vec.Z);

            var groundIntersection = IntersectionProvider.ISectGroundPlane(direction, origin);
            if (groundIntersection != null)
            {
                groundIntersectionPoint = groundIntersection.intersect;
            }

            return groundIntersectionPoint;
        }

        private void MoveTranslation(int previousX, int previousY, float zDistanceInPixels)
        {
            try
            {
                var groundIntersectionPoint = GetGroundIntersectionPoint(previousX, previousY);
                if (groundIntersectionPoint != null)
                {
                    if (this._groundIntersectionPrevious == new Vector3Class())
                    {
                        this._groundIntersectionPrevious = groundIntersectionPoint;
                    }
                    this._groundIntersection = groundIntersectionPoint;

                    Console.WriteLine(groundIntersectionPoint);
                    var selectedObject = ObjectView.SelectedObject as STLModel3D;
                    if (selectedObject != null && selectedObject is STLModel3D)
                    {
                        var distance = this._groundIntersection - this._groundIntersectionPrevious;
                        if (selectedObject.ObjectType == STLModel3D.TypeObject.Support)
                        {
                            var selectedSupportCone = selectedObject as SupportCone;

                            switch (SceneView.MoveTranslation3DGizmo.SelectedMoveTranslationAxis)
                            {
                                case Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected:
                                    selectedSupportCone.MoveTranslation += distance;
                                    break;
                                case Core.Enums.SceneViewSelectedMoveTranslationAxisType.X:
                                    selectedSupportCone.MoveTranslation += new Vector3Class(distance.X, 0, 0);
                                    break;
                                case Core.Enums.SceneViewSelectedMoveTranslationAxisType.Y:
                                    selectedSupportCone.MoveTranslation += new Vector3Class(0, distance.Y, 0);
                                    break;
                            }

                            SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedSupportCone.MoveTranslation + new Vector3Class(0, 0, (selectedObject.TopPoint - selectedObject.BottomPoint) / 2) + new Vector3Class(selectedSupportCone.Model.MoveTranslation.Xy);
                        }
                        else
                        {

                            var selectedLinkedClone = selectedObject.LinkedClones.FirstOrDefault(s => s.Selected);

                            if (selectedLinkedClone != null)
                            {
                                switch (SceneView.MoveTranslation3DGizmo.SelectedMoveTranslationAxis)
                                {
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected:
                                        selectedLinkedClone.Translation += distance;
                                        SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedLinkedClone.Translation + new Vector3Class(0, 0, ((selectedObject.TopPoint - selectedObject.BottomPoint) / 2) + selectedObject.BottomPoint);
                                        break;
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.X:

                                        selectedLinkedClone.Translation += new Vector3Class(distance.X, 0, 0);
                                        SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedLinkedClone.Translation + new Vector3Class(0, 0, ((selectedObject.TopPoint - selectedObject.BottomPoint) / 2) + selectedObject.BottomPoint);
                                        break;
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.Y:
                                        selectedLinkedClone.Translation += new Vector3Class(0, distance.Y, 0);
                                        SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedLinkedClone.Translation + new Vector3Class(0, 0, ((selectedObject.TopPoint - selectedObject.BottomPoint) / 2) + selectedObject.BottomPoint);
                                        break;
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.Z:
                                        selectedObject.PreviewMoveTranslation += new Vector3Class(0, 0, zDistanceInPixels);
                                        selectedObject.DisableSupportDrawing = true;
                                        SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedLinkedClone.Translation + new Vector3Class(0, 0, (selectedObject.TopPoint - selectedObject.BottomPoint) / 2) + new Vector3Class(0, 0, selectedObject.PreviewMoveTranslation.Z);
                                        break;
                                }


                            }
                            else
                            {
                                switch (SceneView.MoveTranslation3DGizmo.SelectedMoveTranslationAxis)
                                {
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected:
                                        selectedObject.PreviewMoveTranslation += new Vector3Class(distance.X, distance.Y, 0);
                                        break;
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.X:
                                        selectedObject.PreviewMoveTranslation += new Vector3Class(distance.X, 0, 0);
                                        break;
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.Y:
                                        selectedObject.PreviewMoveTranslation += new Vector3Class(0, distance.Y, 0);
                                        break;
                                    case Core.Enums.SceneViewSelectedMoveTranslationAxisType.Z:
                                        selectedObject.PreviewMoveTranslation += new Vector3Class(0, 0, zDistanceInPixels);
                                        selectedObject.DisableSupportDrawing = true;
                                        break;
                                }

                                //update gizmo
                                SceneView.MoveTranslation3DGizmo.MoveTranslation = selectedObject.PreviewMoveTranslation + new Vector3Class(0, 0, (selectedObject.TopPoint - selectedObject.BottomPoint) / 2);
                                if (selectedObject.SupportBasementStructure != null)
                                {
                                    selectedObject.SupportBasementStructure.MoveTranslation += this._groundIntersection - this._groundIntersectionPrevious;
                                }
                            }

                            //SceneActionControlManager.ActionPanelMove.DataSource = selectedObject.PreviewMoveTranslation;
                        }

                        this._groundIntersectionPrevious = this._groundIntersection;
                    }
                }
            }
            catch { }

        }

        internal void MoveCameraTargetCenterToNewPosition(Vector3Class newCameraTargetPosition, bool useAnimationEffect, float newCameraZoom = -10000f)
        {
            if (useAnimationEffect)
            {
                var changeCameraSteps = 2; //50 frames in a second
                var accelerationSteps = 0;

                var currentCameraTargetPositon = SceneView.CameraPositionTargetCenter;

                var cameraDistance = (newCameraTargetPosition - currentCameraTargetPositon).Length;
                if (cameraDistance < 10)
                {
                    changeCameraSteps = 20;
                    accelerationSteps = 0;
                }
                var changeVector = (newCameraTargetPosition - currentCameraTargetPositon) / changeCameraSteps;

                var cameraZoomStep = 0f;
                var cameraZoomOffset = 0f;

                if (newCameraZoom != -10000f)
                {
                    cameraZoomOffset = _cameraZoom - newCameraZoom;
                    cameraZoomStep = cameraZoomOffset / changeCameraSteps;
                }

                var accelerationDistance = changeVector * accelerationSteps;

                //start accelaration
                var totalAccelerationDistance = new Vector3Class();
                for (var stepIndex = accelerationSteps; stepIndex > 0; stepIndex--)
                {
                    var t = Vector3Class.Multiply(accelerationDistance, (float)Math.Cos((double)(stepIndex * 5d) * (Math.PI / 180d))) / 20;
                    totalAccelerationDistance += t;
                    SceneView.CameraPositionTargetCenter += t;
                    this.Render();

                    if (newCameraZoom != -10000f)
                    {
                        _cameraZoom -= cameraZoomStep;
                    }
                }

                //middle part of movement including offset of accelaration
                var offsetAcceleration = ((accelerationDistance - totalAccelerationDistance) * 2) / (changeCameraSteps - (2 * accelerationSteps));
                for (var changeCameraStepIndex = 0; changeCameraStepIndex < changeCameraSteps - (2 * accelerationSteps); changeCameraStepIndex++)
                {
                    if (newCameraZoom != -10000f)
                    {
                        _cameraZoom -= cameraZoomStep;
                    }

                    SceneView.CameraPositionTargetCenter += (changeVector + offsetAcceleration);
                    this.Render();
                }

                //decceleration
                for (var stepIndex = accelerationSteps; stepIndex > 0; stepIndex--)
                {
                    if (newCameraZoom != float.NaN)
                    {
                        _cameraZoom -= cameraZoomStep;
                    }

                    var t = Vector3Class.Multiply(accelerationDistance, (float)Math.Sin((double)(stepIndex * 5d) * (Math.PI / 180d))) / 20;
                    SceneView.CameraPositionTargetCenter += t;
                    this.Render();
                }
            }

            SceneView.CameraPositionTargetCenter = newCameraTargetPosition;
            this.Render();
        }

        internal void MoveCameraToPosition(string positionTag)
        {
            var changeCameraSteps = 50; //50 frames in a second
            var newCameraPosition = positionTag;
            var newCameraPositionX = float.Parse(newCameraPosition.Split(',')[0]);
            var newCameraPositionZ = float.Parse(newCameraPosition.Split(',')[1]);
            if ((newCameraPositionX - _cameraRotationX) > 180) { newCameraPositionX = -(360 - newCameraPositionX); }
            if ((newCameraPositionZ - _cameraRotationZ) > 180) { newCameraPositionZ = -(360 - newCameraPositionZ); }
            var changeStepsX = (newCameraPositionX - _cameraRotationX);
            var changeStepsZ = (newCameraPositionZ - _cameraRotationZ);
            if (changeStepsX < -180) { changeStepsX = 360 + changeStepsX; };
            if (changeStepsZ < -180) { changeStepsZ = 360 + changeStepsZ; };
            var changeStepX = changeStepsX / changeCameraSteps;
            var changeStepZ = changeStepsZ / changeCameraSteps;

            if (changeStepX != 0 || changeStepZ != 0)
            {
                for (var changeCameraStepIndex = 0; changeCameraStepIndex < changeCameraSteps; changeCameraStepIndex++)
                {
                    this._cameraRotationX += changeStepX;
                    this._cameraRotationZ += changeStepZ;
                    this.Render();
                }
            }

            if (this._cameraRotationZ < 0)
            {
                this._cameraRotationZ += 360;
            }
        }

        private STLModel3D GetModelByXYMousePosition(int X, int Y, out STLModel3D.TypeObject modelType)
        {
            if (this != null && !this._disableRendering)
            {
                if (!this.Disposing)
                {

                    int w = this.Width;
                    int h = this.Height;
                    float aspect = ((float)this.Width) / ((float)this.Height);


                    var pixel = new Byte4();
                    Point pt = this.PointToClient(MousePosition);
                    GL.ReadPixels(pt.X, this.Height - pt.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);
                    int window_y = (h - Y) - h / 2;
                    double norm_y = (double)(window_y) / (double)(h / 2);
                    int window_x = X - w / 2;
                    double norm_x = (double)(window_x) / (double)(w / 2);
                    float near_height = .2825f; // no detectable error

                    float y = (float)(near_height * norm_y);
                    float x = (float)(near_height * aspect * norm_x);
                    Vector4 ray_pnt = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                    Vector4 ray_vec = new Vector4((float)x, (float)y, -1f, 0);
                    ray_vec.Normalize();

                    float[] projectionMatrix = new float[16];
                    GL.GetFloat(GetPName.ModelviewMatrix, projectionMatrix);
                    GL.MatrixMode(MatrixMode.Projection);
                    GL.LoadMatrix(projectionMatrix);

                    Matrix4 modelViewMatrix;
                    GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
                    try
                    {
                        //color selection

                        if (pixel.A == 200 || pixel.A == 255) //ground/space area
                        {
                            modelType = STLModel3D.TypeObject.Ground;
                            return null;
                        }
                        else
                        {
                            foreach (var model in ObjectView.Objects3D)
                            {
                                if (model != null)
                                {
                                    if (model is STLModel3D && !(model is GroundPane))
                                    {
                                        var availableModel = model as STLModel3D;
                                        if (pixel.A > 100)
                                        {
                                            if (availableModel.Index == pixel.A - 100) //find support cones by model
                                            {
                                                modelType = STLModel3D.TypeObject.Support;
                                                availableModel.Selected = true;

                                                //deselect previous selection

                                                foreach (var model2 in ObjectView.Objects3D)
                                                {
                                                    if (model2 != null)
                                                    {
                                                        if (model2 != availableModel)
                                                        {
                                                            (model2 as STLModel3D).DeselectObjects();
                                                        }
                                                    }
                                                }

                                                return availableModel;
                                            }
                                        }
                                        else if (availableModel.Index == pixel.A)
                                        {
                                            //check if model has linked clones
                                            if (availableModel.LinkedClones.Count > 0)
                                            {
                                                var origin = new Vector3Class();
                                                var direction = new Vector3Class();

                                                GetSceneControlOriginAndDirection(X, Y, out origin, out direction);

                                                var distances = new float[availableModel.LinkedClones.Count];

                                                //find all linked clone intersection distances using MT framework
                                                Parallel.For(0, availableModel.LinkedClones.Count, linkedCloneIndexAsync =>
                                                {
                                                    {
                                                        var linkedCloneIndex = linkedCloneIndexAsync;
                                                        var linkedClone = availableModel.LinkedClones[linkedCloneIndex];

                                                        var cameraTargetPositionTriangleIntersection = TriangleHelper.CalcNearestTriangle(origin - new Vector3Class(availableModel.MoveTranslation.X, availableModel.MoveTranslation.Y, 0), direction, availableModel, linkedClone.Translation);
                                                        if (cameraTargetPositionTriangleIntersection != null && cameraTargetPositionTriangleIntersection.IntersectionPoint != new Vector3Class())
                                                        {
                                                            distances[linkedCloneIndex] = (origin - cameraTargetPositionTriangleIntersection.IntersectionPoint).Length;
                                                        }
                                                    }
                                                });

                                                //Determine the nearest linked clone
                                                var lowestDistance = 50000f;
                                                var lowestDistanceLCIndex = 0;
                                                for (var distanceIndex = 0; distanceIndex < distances.Length; distanceIndex++)
                                                {
                                                    if (distances[distanceIndex] < lowestDistance && distances[distanceIndex] > 0)
                                                    {
                                                        lowestDistance = distances[distanceIndex];
                                                        lowestDistanceLCIndex = distanceIndex;
                                                    }

                                                    availableModel.LinkedClones[distanceIndex].Selected = false;
                                                }

                                                //Select the nearest linked clone
                                                availableModel.LinkedClones[lowestDistanceLCIndex].Selected = true;
                                            }

                                            else
                                            {
                                                foreach (var model2 in ObjectView.Objects3D)
                                                {
                                                    if (model2 != null)
                                                    {
                                                        if (model2 != availableModel)
                                                        {
                                                            (model2 as STLModel3D).Selected = false;
                                                            (model2 as STLModel3D).DeselectObjects();
                                                        }
                                                    }
                                                }
                                            }

                                            modelType = STLModel3D.TypeObject.Model;
                                            availableModel.Selected = true;
                                            return availableModel;
                                        }
                                    }
                                }
                            }
                        }

                    }

                    catch (Exception exc)
                    {

                    }
                }
            }

            modelType = STLModel3D.TypeObject.None;
            return null;
        }


        private void SelectModel(int X, int Y, bool modelOnly)
        {
            Console.WriteLine("Model selection");
            if (this != null && !this._disableRendering)
            {
                if (!this.Disposing)
                {

                    try
                    {
                        var modelType = STLModel3D.TypeObject.Model;
                        var selectedModel = GetModelByXYMousePosition(X, Y, out modelType);
                        if (selectedModel != null)
                        {
                            if (modelType == STLModel3D.TypeObject.Model)
                            {
                                selectedModel.Selected = true;
                                selectedModel.DeselectObjects();
                            }
                            else if (modelType == STLModel3D.TypeObject.Support && !modelOnly)
                            {
                                var intersectedSupportCones = new List<SupportCone>();
                                var supportDistance = 50000f;
                                SupportCone selectedSupportCone = null;


                                var origin = new Vector3Class();
                                var direction = new Vector3Class();
                                GetSceneControlOriginAndDirection(X, Y, out origin, out direction);

                                TriangleIntersection[] intersectedTriangles = null;
                                foreach (var supportCone in selectedModel.TotalObjectSupportCones)
                                {
                                    supportCone.Selected = false;

                                    if (selectedModel.LinkedClones.Count > 0)
                                    {
                                        foreach (var linkedClone in selectedModel.LinkedClones)
                                        {
                                            var cameraPosition = origin - new Vector3Class(selectedModel.MoveTranslationX, selectedModel.MoveTranslationY, 0);
                                            cameraPosition -= -new Vector3Class(linkedClone.Translation.X, linkedClone.Translation.Y, 0);

                                            if (IntersectionProvider.IntersectTriangle(cameraPosition, direction, supportCone, IntersectionProvider.typeDirection.OneWay, true, new Vector3Class(), out intersectedTriangles))
                                            {
                                                foreach (var intersectedTriangle in intersectedTriangles)
                                                {
                                                    if (intersectedTriangle.IntersectionPoint != new Vector3Class())
                                                    {
                                                        var supportConeDistance = (cameraPosition - intersectedTriangle.IntersectionPoint).Length;
                                                        if (supportConeDistance < supportDistance)
                                                        {
                                                            supportDistance = supportConeDistance;
                                                            selectedSupportCone = supportCone;
                                                        }
                                                    }
                                                }

                                            }
                                        }
                                    }
                                    else
                                    {
                                        var cameraPosition = origin - new Vector3Class(selectedModel.MoveTranslationX, selectedModel.MoveTranslationY, 0);

                                        if (IntersectionProvider.IntersectTriangle(cameraPosition, direction, supportCone, IntersectionProvider.typeDirection.OneWay, true, new Vector3Class(), out intersectedTriangles))
                                        {
                                            foreach (var intersectedTriangle in intersectedTriangles)
                                            {
                                                if (intersectedTriangle.IntersectionPoint != new Vector3Class())
                                                {
                                                    var supportConeDistance = (cameraPosition - intersectedTriangle.IntersectionPoint).Length;
                                                    if (supportConeDistance < supportDistance)
                                                    {
                                                        supportDistance = supportConeDistance;
                                                        selectedSupportCone = supportCone;
                                                    }
                                                }
                                            }

                                        }
                                    }




                                }

                                if (selectedSupportCone != null)
                                {
                                    if (SceneView.CurrentViewMode != SceneView.ViewMode.MagsAIManualSupport && SceneView.CurrentViewMode != SceneView.ViewMode.ModelRotation && SceneView.CurrentViewMode != SceneView.ViewMode.MoveTranslation)
                                    {
                                        ObjectView.DeselectModels();

                                        selectedSupportCone.Model.Selected = true;
                                        selectedSupportCone.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception exc)
                    {
                        Debug.WriteLine(exc.Message);
                    }


                }
            }
        }

        private void GetSceneControlOriginAndDirection(int X, int Y, out Vector3Class origin, out Vector3Class direction)
        {
            this.MakeCurrent();

            int w = this.Width;
            int h = this.Height;
            float aspect = ((float)this.Width) / ((float)this.Height);

            int window_y = (h - Y) - h / 2;
            double norm_y = (double)(window_y) / (double)(h / 2);
            int window_x = X - w / 2;
            double norm_x = (double)(window_x) / (double)(w / 2);
            float near_height = .2825f; // no detectable error

            float y = (float)(near_height * norm_y);
            float x = (float)(near_height * aspect * norm_x);
            Vector4 ray_pnt = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            Vector4 ray_vec = new Vector4((float)x, (float)y, -1f, 0);
            ray_vec.Normalize();

            Matrix4 modelViewMatrix = SceneView.OrbitCamera.GetCameraView();
            //GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
            Matrix4 viewInv = Matrix4.Invert(modelViewMatrix);

            Vector4 t_ray_pnt = new Vector4();
            Vector4 t_ray_vec = new Vector4();

            Vector4.Transform(ref ray_vec, ref viewInv, out t_ray_vec);
            Vector4.Transform(ref ray_pnt, ref viewInv, out t_ray_pnt);

            origin = new Vector3Class(t_ray_pnt.X, t_ray_pnt.Y, t_ray_pnt.Z);
            direction = new Vector3Class(t_ray_vec.X, t_ray_vec.Y, t_ray_vec.Z);


        }

        private TriangleSurfaceInfo GetSelectedPlane(int X, int Y)
        {
            TriangleSurfaceInfo selectedPlane = null;

            if (this != null && !this._disableRendering)
            {
                lock (this)
                {
                    this.MakeCurrent();

                    int w = this.Width;
                    int h = this.Height;
                    float aspect = ((float)this.Width) / ((float)this.Height);

                    int window_y = (h - Y) - h / 2;
                    double norm_y = (double)(window_y) / (double)(h / 2);
                    int window_x = X - w / 2;
                    double norm_x = (double)(window_x) / (double)(w / 2);
                    float near_height = .2825f; // no detectable error

                    float y = (float)(near_height * norm_y);
                    float x = (float)(near_height * aspect * norm_x);
                    Vector4 ray_pnt = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
                    Vector4 ray_vec = new Vector4((float)x, (float)y, -1f, 0);
                    ray_vec.Normalize();

                    Matrix4 modelViewMatrix;
                    GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
                    Matrix4 viewInv = Matrix4.Invert(modelViewMatrix);

                    //color selection
                    var pixel = new Byte4();
                    Point pt = this.PointToClient(Control.MousePosition);
                    GL.ReadPixels(pt.X, this.Height - pt.Y, 1, 1, OpenTK.Graphics.OpenGL.PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);

                    Vector4 t_ray_pnt = new Vector4();
                    Vector4 t_ray_vec = new Vector4();

                    Vector4.Transform(ref ray_vec, ref viewInv, out t_ray_vec);
                    Vector4.Transform(ref ray_pnt, ref viewInv, out t_ray_pnt);

                    var origin = new Vector3Class(t_ray_pnt.X, t_ray_pnt.Y, t_ray_pnt.Z);
                    var direction = new Vector3Class(t_ray_vec.X, t_ray_vec.Y, t_ray_vec.Z);

                    //support
                    Triangle selectedTriangle = null;
                    foreach (var object3d in ObjectView.Objects3D)
                    {

                        if (object3d is STLModel3D && !(object3d is GroundPane))
                        {
                            var stlModel = object3d as STLModel3D;

                            //model
                            if (stlModel.Selected && pixel.A < 100)
                            {
                                selectedTriangle = TriangleHelper.CalcNearestTriangle(origin, direction, stlModel, null);

                                //get selected plane
                                if (selectedTriangle != null)
                                {
                                    foreach (var surfacePlane in stlModel.Triangles.HorizontalSurfaces)
                                    {
                                        foreach (var surfacePlanePoint in surfacePlane.Keys)
                                        {
                                            if (stlModel.Triangles[surfacePlanePoint.ArrayIndex][surfacePlanePoint.TriangleIndex].Vectors == selectedTriangle.Vectors)
                                            {
                                                return surfacePlane;
                                            }
                                        }
                                    }

                                    //flat surfaces
                                    foreach (var surfacePlane in stlModel.Triangles.FlatSurfaces)
                                    {
                                        foreach (var surfacePlanePoint in surfacePlane.Keys)
                                        {
                                            if (stlModel.Triangles[surfacePlanePoint.ArrayIndex][surfacePlanePoint.TriangleIndex].Vectors == selectedTriangle.Vectors)
                                            {
                                                return surfacePlane;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

            }
            return selectedPlane;
        }

        private void MarkManualSupportIntersection(int X, int Y, STLModel3D selectedModel)
        {
            if (this != null && !this._disableRendering)
            {
                lock (this)
                {
                    var modelType = STLModel3D.TypeObject.Model;
                    var highlightedModel = selectedModel;
                    if (selectedModel == null)
                    {
                        highlightedModel = GetModelByXYMousePosition(X, Y, out modelType);
                    }
                    if (highlightedModel != null)
                    {
                        ObjectView.DeselectModels();

                        highlightedModel.Selected = true;

                    }
                    else if (ObjectView.SelectedModel != null)
                    {
                        highlightedModel = ObjectView.SelectedModel;
                    }

                    foreach (var object3d in ObjectView.Objects3D)
                    {
                        if (!(object3d is GroundPane) && object3d is STLModel3D)
                        {
                            var stlModel = object3d as STLModel3D;
                            foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                            {
                                horizontalSurface.Selected = false;
                            }

                            foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                            {
                                flatSurface.Selected = false;
                            }
                        }
                    }

                    //get raytrace
                    var origin = new Vector3Class();
                    var direction = new Vector3Class();

                    GetSceneControlOriginAndDirection(X, Y, out origin, out direction);
                    if (highlightedModel != null)
                    {
                        origin -= new Vector3Class(highlightedModel.MoveTranslationX, highlightedModel.MoveTranslationY, 0);

                        //support
                        if (highlightedModel != null)
                        {
                            Debug.WriteLine("Find intersection point");
                            this._selectedTriangle = TriangleHelper.CalcNearestTriangle(origin, direction, highlightedModel, null);

                            if (this._selectedTriangle != null && this._selectedTriangle.IntersectionPoint != null)
                            {
                                Debug.WriteLine("Intersected triangle found: " + this._selectedTriangle.IntersectionPoint);
                            }

                            //convert selected triangle to surface
                            var selectedTriangleArrayIndex = -1;
                            var selectedTriangleIndex = -1;

                            if (this._selectedTriangle != null)
                            {
                                for (var triangleArrayIndex = 0; triangleArrayIndex < highlightedModel.Triangles.Count; triangleArrayIndex++)
                                {
                                    for (var triangleIndex = 0; triangleIndex < highlightedModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
                                    {
                                        if (highlightedModel.Triangles[triangleArrayIndex][triangleIndex].Vectors == this._selectedTriangle.Vectors)
                                        {
                                            selectedTriangleArrayIndex = triangleArrayIndex;
                                            selectedTriangleIndex = triangleIndex;
                                            break;
                                        }
                                    }
                                }
                            }

                            if (selectedTriangleIndex > -1)
                            {
                                foreach (var horizontalSurface in highlightedModel.Triangles.HorizontalSurfaces)
                                {
                                    if (horizontalSurface.ContainsKey(highlightedModel.Triangles[selectedTriangleArrayIndex][selectedTriangleIndex].Index))
                                    {
                                        horizontalSurface.Selected = true;
                                    }
                                }

                                //grid support
                                foreach (var flatSurface in highlightedModel.Triangles.FlatSurfaces)
                                {
                                    if (flatSurface.ContainsKey(highlightedModel.Triangles[selectedTriangleArrayIndex][selectedTriangleIndex].Index))
                                    {
                                        flatSurface.Selected = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        static Object renderLock = new object();
        static bool renderLocked = false;
        internal void Render(bool raiseRenderCompletedEvent = false)
        {
            if (!renderLocked)
            {
                lock (renderLock)
                {
                    renderLocked = true;
                }

                if (!this._disableRendering)
                {
                    // _tmrGameModeRunning.Stop();
                    if (!this.Context.IsCurrent)
                    {
                        if (this.InvokeRequired) { this.Invoke(new MethodInvoker(delegate { this.MakeCurrent(); })); }
                        else { this.MakeCurrent(); }
                    }

                    SceneView.RenderAsModelView(this, this._cameraRotationX, this._cameraRotationZ, this._cameraZoom, this.CameraPanX, this.CameraPanY, false, false);

                    if (PrintJobManager.CurrentPrintJobSettings.Material != null)
                    {
                        if (PrintJobManager.CurrentPrintJobSettings.Material.SupportProfiles.Count == 0)
                        {
                            PrintJobManager.CurrentPrintJobSettings.Material.SupportProfiles.Add(DAL.Materials.SupportProfile.CreateDefault());
                        }

                        ObjectView.DrawObjects(this, PrintJobManager.CurrentPrintJobSettings.Material.SupportProfiles[0]);
                    }
                    if (PrintJobManager.SelectedPrinter != null)
                    {
                        SceneView.DrawGroundPane();
                    }



                    if (!this.Context.IsCurrent)
                    {
                        if (this.InvokeRequired) { this.Invoke(new MethodInvoker(delegate { this.MakeCurrent(); })); }
                        else { this.MakeCurrent(); }
                    }


                    if (ObjectView.Objects3D.Count > 1)
                    {
                        if (SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation)
                        {
                            if (this.Context.IsCurrent)
                            {
                                SceneView.DrawRotation3DGizmo();
                            }
                        }
                        else if (SceneView.CurrentViewMode == SceneView.ViewMode.MoveTranslation)
                        {
                            if (this.Context.IsCurrent)
                            {
                                SceneView.DrawMoveTranslation3DGizmo();
                            }
                        }
                    }

                    if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                    {
                        DrawSelectionBoxAndSelectItems();
                    }
                    else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport)
                    {
                        ObjectView.DrawNewSupportConeGhost();
                    }

                    if (!this.Context.IsCurrent)
                    {
                        if (this.InvokeRequired) { this.Invoke(new MethodInvoker(delegate { this.MakeCurrent(); })); }
                        else { this.MakeCurrent(); }
                    }

                    this.SwapBuffers();

                    if (raiseRenderCompletedEvent) this.RenderCompleted?.Invoke(null, null);
                }


                this.QuickViewSelectTriangleIntersectionCross = false;

                lock (renderLock)
                {
                    renderLocked = false;
                }

            }

        }
        void DrawSelectedTriangle()
        {
            if (this._selectedTriangle != null)
            {
                var stlModelMoveTransation = new Vector3Class();
                if (ObjectView.SelectedModel is STLModel3D)
                {
                    stlModelMoveTransation = (ObjectView.SelectedModel).MoveTranslation;
                }

                GL.Begin(PrimitiveType.Lines);
                GL.Color3(Color.Red);

                GL.LineWidth(50);
                //  foreach(var vertex in selectionArrow.
                GL.Vertex3(this._selectedTriangle.IntersectionPoint.ToStruct() + new Vector3(stlModelMoveTransation.X, stlModelMoveTransation.Y, 0));
                //            System.Diagnostics.Debug.WriteLine(this._selectedTriangle.IntersectionPoint.Z);
                GL.Vertex3(this._selectedTriangle.Normal.ToStruct() * 100 + this._selectedTriangle.IntersectionPoint.ToStruct());

                GL.LineWidth(1);
                GL.End();


            }


        }


        private static GraphicsMode GetBestGraphicsMode()
        {

            List<GraphicsMode> modes = new List<GraphicsMode>();
            foreach (ColorFormat color in new ColorFormat[] { 32, 24, 16, 8 })
                foreach (int depth in new int[] { 24, 16 })
                    foreach (int stencil in new int[] { 8, 0 })
                        foreach (int samples in new int[] { 0, 2, 4, 6, 8, 16 })
                            foreach (bool stereo in new bool[] { false, true })
                            {
                                try
                                {
                                    GraphicsMode mode = new GraphicsMode(color, depth, stencil, samples, 0, 2, stereo);
                                    if (!modes.Contains(mode))
                                        modes.Add(mode);
                                }
                                catch
                                { }
                            }


            foreach (ColorFormat color in new ColorFormat[] { 32, 24, 16, 8 })
                foreach (int depth in new int[] { 24, 16 })
                    foreach (int stencil in new int[] { 8, 0 })
                        foreach (int samples in new int[] { 8, 6, 4, 2, 0 })
                        {
                            foreach (var mode in modes)
                                if (mode.ColorFormat == color && mode.Depth == depth && mode.Stencil == stencil && mode.Samples == samples)
                                {
                                    return mode;
                                }

                        }


            return null;
        }


        internal Bitmap CreateScreenshot()
        {
            //Render();

            Bitmap bmp = new Bitmap(this.ClientSize.Width, this.ClientSize.Height);
            System.Drawing.Imaging.BitmapData data = bmp.LockBits(this.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            GL.ReadPixels(0, 0, this.ClientSize.Width, this.ClientSize.Height, PixelFormat.Bgr, PixelType.UnsignedByte, data.Scan0);

            bmp.UnlockBits(data);
            bmp.RotateFlip(RotateFlipType.RotateNoneFlipY);
            return bmp;
        }

        internal Point GetScreenPointFromVector3(Vector3Class objPos, int width, int height)
        {
            Matrix4 modelMatrix = Matrix4.Identity;

            float aspectRatio = (float)width / (float)height;
            //Matrix4 projectionMatrix;
            Matrix4 viewMatrix = SceneView.OrbitCamera.GetCameraView();

            var result = OpenTKHelper.Project(objPos, SceneView.ProjectionMatrix, viewMatrix, modelMatrix);
            //return y as invert of viewport height
            result.Y = height - result.Y;
            return new Point((int)result.X, (int)result.Y);
        }


        #region Support Engine
        private void AddingAnySupport(ManualSupportCone supportConeModel, DAL.Materials.Material selectedMaterial)
        {
            if (supportConeModel.IsUndo)
            {
                ObjectView.SelectObjectByIndex(supportConeModel.ModelIndex);

                this._selectedTriangle = supportConeModel.SelectedTriangle;
                this._previousSelectedTriangle = supportConeModel.PreviousSelectedTriangle;
            }
            if (ObjectView.SelectedModel is STLModel3D)
            {
                var stlModel = ObjectView.SelectedModel as STLModel3D;
                this._previousSelectedObject = stlModel;

                if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIManualSupport && this._selectedTriangle != null)
                {
                    SupportEngine.AddManualSupport(stlModel, this._selectedTriangle, selectedMaterial);
                    stlModel.UpdateSupportBasement();


                    PushSupportConeInStack(supportConeModel, stlModel);
                }
                else if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAIGridSupport)
                {
                    foreach (var horizontalSurface in stlModel.Triangles.HorizontalSurfaces)
                    {
                        if (horizontalSurface.Selected)
                        {
                            horizontalSurface.SupportStructure.Clear();
                            horizontalSurface.SupportDistance = 3;
                            SupportEngine.CreateSurfaceSupport(horizontalSurface, stlModel, selectedMaterial, true);
                            break;

                        }
                    }

                    foreach (var flatSurface in stlModel.Triangles.FlatSurfaces)
                    {
                        if (flatSurface.Selected)
                        {
                            flatSurface.SupportStructure.Clear();

                            flatSurface.SupportDistance = 3;
                            SupportEngine.CreateSurfaceSupport(flatSurface, stlModel, selectedMaterial, true);

                            break;
                        }
                    }
                }
            }
            Render();
        }

        private void PushGridSupportConeInStack(GridSupportCone gridSupportConeModel, STLModel3D stlModel)
        {
            gridSupportConeModel.ModelIndex = stlModel.Index;
            gridSupportConeModel.IsUndo = true;

            UndoRedoManager.GetInstance.PushReverseAction(x => RemoveGridSupportCone(x), new RemoveGridSupportCone() { GridSupportCone = gridSupportConeModel, SelectedSurfaceIndex = gridSupportConeModel.SelectedSurfaceIndex });
            Render();
        }

        private void PushSupportConeInStack(ManualSupportCone supportConeModel, STLModel3D stlModel)
        {
            supportConeModel.ModelIndex = stlModel.Index;
            supportConeModel.IsUndo = true;

            var lastAddedSupportCone = stlModel.SupportStructure[stlModel.SupportStructure.Count - 1];
            lastAddedSupportCone.MousePosition = supportConeModel.MousePosition;
            lastAddedSupportCone.PreviousSelectedTriangle = supportConeModel.PreviousSelectedTriangle;
            lastAddedSupportCone.SelectedTriangle = supportConeModel.SelectedTriangle;

            UndoRedoManager.GetInstance.PushReverseAction(x => RemoveManualSupportCone(x), new RemoveManualSupportCone() { SupportConeModel = supportConeModel, SupportCone = lastAddedSupportCone });
            Render();
        }

        private void RemoveManualSupportCone(RemoveManualSupportCone removeSupportConeModel)
        {
            var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
            var supportCone = removeSupportConeModel.SupportCone;

            if (supportCone != null)
            {
                ManualSupportCone supportConeModel = new ManualSupportCone();
                if (removeSupportConeModel.SupportConeModel != null)
                {
                    supportConeModel.MousePosition = removeSupportConeModel.SupportConeModel.MousePosition;
                    supportConeModel.SelectedTriangle = removeSupportConeModel.SupportConeModel.SelectedTriangle;
                    supportConeModel.PreviousSelectedTriangle = removeSupportConeModel.SupportConeModel.PreviousSelectedTriangle;
                    supportConeModel.IsUndo = true;
                }
                var selectedModel = supportCone.Model;
                supportConeModel.ModelIndex = selectedModel.Index;
                UndoRedoManager.GetInstance.PushReverseAction(x => AddingAnySupport(x, selectedMaterialSummary.Material), supportConeModel);
                selectedModel.SupportStructure.RemoveAll(s => s.Position == removeSupportConeModel.SupportCone.Position);

                SceneActionControlManager.RemoveSupportPropertiesHandle();

                foreach (var surface in selectedModel.Triangles.HorizontalSurfaces)
                {
                    foreach (var surfaceSupportCone in surface.SupportStructure)
                    {
                        if (supportCone == surfaceSupportCone)
                        {
                            surface.SupportStructure.Remove(surfaceSupportCone);

                            break;
                        }
                    }
                }

                foreach (var surface in selectedModel.Triangles.FlatSurfaces)
                {
                    foreach (var surfaceSupportCone in surface.SupportStructure)
                    {
                        if (supportCone == surfaceSupportCone)
                        {
                            surface.SupportStructure.Remove(surfaceSupportCone);

                            break;
                        }
                    }
                }

                if (selectedModel != null)
                {
                    selectedModel.UpdateSupportBasement();
                }
            }

            Render();
        }

        private void RemoveGridSupportCone(RemoveGridSupportCone removeGridSupportConeModel)
        {
            var selectedMaterial = PrintJobManager.SelectedMaterialSummary;

            ObjectView.SelectObjectByIndex(removeGridSupportConeModel.GridSupportCone.ModelIndex);
            var selectedModel = ObjectView.SelectedModel;
            if (removeGridSupportConeModel.GridSupportCone.IsHorizontalSurface)
            {
                selectedModel.Triangles.HorizontalSurfaces[removeGridSupportConeModel.SelectedSurfaceIndex].SupportStructure.Clear();
            }
            else if (removeGridSupportConeModel.GridSupportCone.IsFlatSurface)
            {
                selectedModel.Triangles.FlatSurfaces[removeGridSupportConeModel.SelectedSurfaceIndex].SupportStructure.Clear();
            }

            if (selectedModel != null)
            {
                selectedModel.UpdateSupportBasement();
            }

            Render();
        }

        private void SceneGLControl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var cameraTargetPosition = UpdateCameraTargetPosition(e.X, e.Y);
            MoveCameraTargetCenterToNewPosition(cameraTargetPosition, false);

            this.CameraPanX = 0;
            this.CameraPanY = 0;

            this.LeftMouseDown = false;
            this.RightMouseDown = false;
        }

        internal void ProcessKeyDown(object sender, KeyEventArgs e)
        {
            this.SceneGLControl_KeyDown(sender, e);
        }

        private void SceneGLControl_KeyDown(object sender, KeyEventArgs e)
        {
            var keyData = e.KeyData;
            var selectedObject = ObjectView.SelectedObject;

            if (keyData == Keys.Up && selectedObject != null && (!(selectedObject is SupportCone)))
            {
                if (ObjectView.SelectedModel is STLModel3D)
                {
                    var selectedModel = ObjectView.SelectedModel;
                    selectedModel.MoveTranslationY += 1f;

                    //update controls
                    //MoveTranslationYChanged?.Invoke(selectedModel.MoveTranslationY);
                }
            }
            else if (keyData == Keys.Down && selectedObject != null && (!(selectedObject is SupportCone)))
            {
                if (ObjectView.SelectedModel is STLModel3D)
                {
                    var selectedModel = ObjectView.SelectedModel;
                    selectedModel.MoveTranslationY -= 1f;

                    //update controls
                    //MoveTranslationYChanged?.Invoke(selectedModel.MoveTranslationY);
                }
            }
            else if (keyData == Keys.Right && selectedObject != null && (!(selectedObject is SupportCone)))
            {
                if (ObjectView.SelectedModel is STLModel3D)
                {
                    var selectedModel = ObjectView.SelectedModel;
                    selectedModel.MoveTranslationX += 1f;

                    //update controls
                    // MoveTranslationXChanged?.Invoke(selectedModel.MoveTranslationX);
                }
            }
            else if (keyData == Keys.Left && selectedObject != null && (!(selectedObject is SupportCone)))
            {
                if (ObjectView.SelectedModel is STLModel3D)
                {
                    var selectedModel = ObjectView.SelectedModel;
                    selectedModel.MoveTranslationX -= 1f;

                    //update controls
                    //MoveTranslationXChanged?.Invoke(selectedModel.MoveTranslationX);

                }
            }
            else if (keyData == Keys.Control && Control.MouseButtons == MouseButtons.Left)
            {
                if (!this._quickViewPan)
                {
                    this._quickViewPan = true;
                    SceneView.PreviousViewMode = SceneView.CurrentViewMode;
                    SceneView.CurrentViewMode = SceneView.ViewMode.Pan;

                    this._currentMousePanPositionX = Control.MousePosition.X;
                    this._currentMousePanPositionY = Control.MousePosition.Y;

                }
            }

            //else if (keyData == (Keys.C | Keys.Control))
            //{
            //    this.HotKeyCtlCPressed.Invoke(null, null);
            //}
            //else if (keyData == (Keys.P | Keys.Control))
            //{
            //    this.HotKeyCtlPPressed.Invoke(null, null);
            //}
            //else if (keyData == Keys.Oem4 && SceneView.CurrentViewMode == SceneView.ViewMode.AutoRotateSelection) //[
            //{
            //    this.HotKeyDecreaseSelectedBoxSizePressed?.Invoke(null, null);
            //}
            //else if (keyData == Keys.Oem6 && SceneView.CurrentViewMode == SceneView.ViewMode.AutoRotateSelection) //]
            //{
            //    this.HotKeyIncreaseSelectedBoxSizePressed?.Invoke(null, null);
            //}

        }

        private void SceneGLControl_Scroll(object sender, ScrollEventArgs e)
        {

        }
    }


    #endregion

}
#endregion
