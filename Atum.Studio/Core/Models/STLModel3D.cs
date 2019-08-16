using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Events;
using OpenTK;
using Atum.Studio.Core.ModelView;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;
using Atum.Studio.Core.Engines;
using System.Diagnostics;
using Atum.Studio.Core.Managers;
using static Atum.Studio.Core.Helpers.ContourHelper;
using System.Linq;
using System.Threading;
using Atum.Studio.Core.Helpers;
using System.Runtime.Serialization.Formatters.Binary;
using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Engines.MagsAI;
using static Atum.Studio.Core.Shapes.TriangleSurfaceInfoList;
using Atum.Studio.Core.Managers.UndoRedo;
using Atum.DAL.ApplicationSettings;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class STLModel3D : DrawableShapeInfo
    {
        internal event EventHandler ModelRemoved;

        internal event EventHandler SupportXYDistanceProcessing;
        internal event EventHandler SupportSurfaceProcessing;
        // internal event EventHandler InternalSupportProcessing;
        internal event EventHandler CrossSupportProcessing;
        //internal event EventHandler ZSupportProcessing;
        internal event EventHandler<RotationEventArgs> RotationProcessing;
        internal event EventHandler<ScaleEventArgs> ScaleProcessing;

        internal static event EventHandler<OpenFileEventArgs> OpenFileProcessing;
        internal static event EventHandler<OpenFileEventArgs> OpenFileProcesssed;

        internal static event EventHandler WorkspaceSTLModelSupportAdded;

        internal event EventHandler ExportingToAPFProgress;

        internal float LeftPoint { get; set; }
        internal float RightPoint { get; set; }
        internal float BottomPoint { get; set; }
        internal float TopPoint { get; set; }
        internal float FrontPoint { get; set; }
        internal float BackPoint { get; set; }

        internal float Width
        {
            get
            {
                return (this.RightPoint - this.LeftPoint);
            }
        }

        internal float Depth
        {
            get
            {
                return (this.FrontPoint - this.BackPoint);
            }
        }

        internal float Height
        {
            get
            {
                return (this.TopPoint - this.BottomPoint);
            }
        }

        internal float FootprintRightPoint
        {
            get
            {
                float supportBasementRightPoint = _supportBasement ? SupportBasementStructure.RightPoint : RightPoint;
                float supportConesRightPoint = TotalObjectSupportCones.Count > 0 ? TotalObjectSupportCones.Max(s => s.RightPoint) : 0;
                return Helpers.MathHelper.Max(supportBasementRightPoint, supportConesRightPoint, RightPoint);
            }
        }

        internal float FootprintLeftPoint
        {
            get
            {
                float supportBasementLeftPoint = _supportBasement ? SupportBasementStructure.LeftPoint : LeftPoint;
                float supportConesLeftPoint = TotalObjectSupportCones.Count > 0 ? TotalObjectSupportCones.Min(s => s.LeftPoint) : 0;
                return Helpers.MathHelper.Min(supportBasementLeftPoint, supportConesLeftPoint, LeftPoint);
            }
        }


        internal float FootprintFrontPoint
        {
            get
            {
                float supportBasementFrontPoint = _supportBasement ? SupportBasementStructure.FrontPoint : FrontPoint;
                float supportConesFrontPoint = TotalObjectSupportCones.Count > 0 ? TotalObjectSupportCones.Max(s => s.FrontPoint) : 0;
                return Helpers.MathHelper.Max(supportBasementFrontPoint, supportConesFrontPoint, FrontPoint);
            }
        }

        internal float FootprintBackPoint
        {
            get
            {
                float supportBasementBackPoint = _supportBasement ? SupportBasementStructure.BackPoint : BackPoint;
                float supportConesBackPoint = TotalObjectSupportCones.Count > 0 ? TotalObjectSupportCones.Min(s => s.BackPoint) : 0;
                return Helpers.MathHelper.Min(supportBasementBackPoint, supportConesBackPoint, BackPoint);
            }
        }

        internal bool DisableSupportDrawing { get; set; }

        [Browsable(false)]
        public SortedDictionary<float, List<TriangleConnectionInfo>> SliceIndexes { get; set; }

        //internal Vector3[] _selectionboxTextFrontFacingUpText;
        //internal Vector3[] _selectionboxTextFrontFacingDownText;
        //internal Vector3[] _selectionboxTextHeightFacingFrontText;
        //internal Vector3[] _selectionboxTextHeightFacingBackText;
        //internal Vector3[] _selectionboxTextDepthFacingUpText;
        //internal Vector3[] _selectionboxTextDepthFacingDownText;

        internal int Index { get { return this.Color.A; } }
        internal TypeObject ObjectType { get; set; }

        internal bool IsSmallModel
        {
            get
            {
                return this.Triangles.Count < 30;
            }
        }

        internal Vector3Class UndoMoveTranslation { get; set; }

        public enum TypeObject
        {
            Model = 0,
            Support = 1,
            XYZ = 2,
            Ground = 99,
            None = 100
        }


        [Browsable(false)]
        public string FileName { get; set; }
        public TriangleInfoList Triangles;
        [Browsable(false)]
        public Vector3Class MoveTranslation = new Vector3Class();

        [Browsable(false)]
        [Category("Movement")]
        [DisplayName("Translation-X")]
        [Description("Move Translation X\nUnit: mm")]
        public float MoveTranslationX
        {
            get
            {
                return this.MoveTranslation.X;
            }
            set
            {
                this.MoveTranslation = new Vector3Class(value, this.MoveTranslation.Y, this.MoveTranslation.Z);

                this.Triangles.HorizontalSurfaces.UpdateBoundries(this.Triangles);
                this.Triangles.FlatSurfaces.UpdateBoundries(this.Triangles);

                //update base plate 

                //  this.Triangles.HorizontalSurfaces.UpdateMoveTranslation(new Vector3(-movementDelta,0 , 0));
                this.UpdateSupportBasement();

                SceneView.MoveTranslation3DGizmo.UpdateControl(Enums.SceneViewSelectedMoveTranslationAxisType.X, false);
            }
        }

        [Browsable(false)]
        [Category("Movement")]
        [DisplayName("Translation-Y")]
        [Description("Move Translation Y\nUnit: mm")]
        public float MoveTranslationY
        {
            get
            {
                return this.MoveTranslation.Y;
            }
            set
            {
                this.MoveTranslation = new Vector3Class(this.MoveTranslation.X, value, this.MoveTranslation.Z);

                this.Triangles.HorizontalSurfaces.UpdateBoundries(this.Triangles);
                this.Triangles.FlatSurfaces.UpdateBoundries(this.Triangles);
                SceneView.MoveTranslation3DGizmo.UpdateControl(Enums.SceneViewSelectedMoveTranslationAxisType.Y, false);
            }
        }

        [Browsable(false)]
        [Category("Movement")]
        [DisplayName("Translation-Z")]
        [Description("Move Translation Z\nUnit: mm")]
        public float MoveTranslationZ
        {
            get
            {
                return this.MoveTranslation.Z;
            }
            set
            {
                if (this.MoveTranslation.Z != value)
                {
                    this.MoveTranslation = new Vector3Class(this.MoveTranslation.X, this.MoveTranslation.Y, value);
                };
            }
        }

        private List<LinkedClone> _linkedClones = new List<LinkedClone>();
        /// <summary>
        /// A set of clones which share the same model data but have a different position and rotation on the buildplate.
        /// </summary>
        [Browsable(false)]
        public List<LinkedClone> LinkedClones
        {
            get { return _linkedClones; }
            set { _linkedClones = value; }
        }

        [Browsable(false)]
        public float PreviousScaleFactorX { get; set; } //correctionfactor
        [Browsable(false)]
        public float PreviousScaleFactorY { get; set; }//correctionfactor
        [Browsable(false)]
        public float PreviousScaleFactorZ { get; set; } //correctionfactor

        [Browsable(true)]
        [Category("General")]
        [DisplayName("Color")]
        [Description("Color of object in designer\nUnit: Color")]
        public Color Color
        {
            get
            {
                return this._color;
            }
            set
            {
                this._color = Color.FromArgb(this.Index, value);
                this.Triangles.UpdateFaceColors(this.ColorAsByte4, false);

                if (frmStudioMain.SceneControl != null)
                {
                    if (frmStudioMain.SceneControl.InvokeRequired)
                    {
                        frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate { this.UpdateBinding(); }));
                    }
                    else
                    {
                        this.UpdateBinding();
                    }
                }
            }
        }
        internal Byte4Class ColorAsByte4
        {
            get
            {
                return new Byte4Class(this.Color.A, this.Color.R, this.Color.G, this.Color.B);
            }
        }

        internal float _scaleFactorX;
        internal float _scaleFactorY;
        internal float _scaleFactorZ;

        internal float _rotationAngleX;
        internal float _rotationAngleY;
        internal float _rotationAngleZ;

        internal Color _color;
        internal bool _internalSupport;
        internal bool _crossSupport;
        // internal bool _zSupport;
        internal float _supportDistance;
        internal bool _supportBasement;

        private float _volume;
        internal bool _bindingSupported;
        private bool _hidden;

        public bool Loaded { get; set; }

        internal bool _selected;

        internal event EventHandler<STLModel3D> ModelSelected;

        internal bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                if (value != this._selected)
                {
                    this._selected = value;

                    if (value)
                    {
                        this.ModelSelected?.Invoke(this, null);
                    }
                }
            }
        }
        //internal bool Highlighted { get; set; }

        private float _previewRotationX;
        internal float PreviewRotationX
        {
            get
            {
                return this._previewRotationX;
            }
            set
            {
                this._previewRotationX = value;
            }
        }

        private float _previewRotationY;
        internal float PreviewRotationY
        {
            get
            {
                return this._previewRotationY;
            }
            set
            {
                this._previewRotationY = value;
            }
        }

        private float _previewRotationZ;
        internal float PreviewRotationZ
        {
            get
            {
                return this._previewRotationZ;
            }
            set
            {
                this._previewRotationZ = value;
            }
        }

        private Vector3Class _previewMoveTranslation = new Vector3Class();
        internal Vector3Class PreviewMoveTranslation
        {
            get
            {
                return _previewMoveTranslation;
            }
            set
            {
                this._previewMoveTranslation = value;


            }
        }

        [Browsable(true)]
        [Category("Support")]
        [DisplayName("Cone Distance")]
        [Description("Cone Distance\nUnit: factor")]
        public virtual float SupportDistance
        {
            get
            {
                return this._supportDistance;
            }
            set
            {
                if (this._supportDistance != value)
                {
                    if (this.SupportXYDistanceProcessing != null) { this.SupportXYDistanceProcessing(null, null); }
                    this._supportDistance = value;

                }
            }
        }

        public void ClearMarkings()
        {
            if (this.Triangles != null)
            {
                this.Triangles.ClearSelectedOrientationTriangles();
            }

            if (this.MAGSAISelectionOverlay != null)
            {
                if (this.MAGSAISelectionOverlay.Triangles != null)
                {
                    this.MAGSAISelectionOverlay.Triangles = new TriangleInfoList();

                    this.MAGSAISelectionOverlay.UpdateBinding();
                }

            }
        }

        public void ClearSupport()
        {
            foreach (var flatSurface in this.Triangles.FlatSurfaces)
            {
                flatSurface.SupportStructure.Clear();
            }

            foreach (var horizontalSurface in this.Triangles.HorizontalSurfaces)
            {
                horizontalSurface.SupportStructure.Clear();
            }

            this.SupportStructure.Clear();
        }

        [Browsable(true)]
        [Category("Support")]
        [DisplayName("Support Basement")]
        [Description("Support Basement: Connects the lower support cone parts as a rectangular base")]
        public virtual bool SupportBasement
        {
            get
            {
                return this._supportBasement;
            }
            set
            {
                this._supportBasement = value;

                if (this._supportBasement)
                {
                    //get outer boundries
                    var leftPoint = float.MaxValue;
                    var rightPoint = -float.MaxValue;
                    var frontPoint = -float.MaxValue;
                    var backPoint = float.MaxValue;

                    var x = float.MaxValue;
                    var y = float.MaxValue;

                    //model bottom points
                    for (var triangleArrayIndex = 0; triangleArrayIndex < this.Triangles.Count; triangleArrayIndex++)
                    {
                        for (var triangleIndex = 0; triangleIndex < this.Triangles[triangleArrayIndex].Count; triangleIndex++)
                        {
                            for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                            {
                                if (this.Triangles[triangleArrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z <= 0.1f || (this.Triangles[triangleArrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z <= 1.1f && SupportBasement))
                                {
                                    x = this.Triangles[triangleArrayIndex][triangleIndex].Vectors[vectorIndex].Position.X;
                                    y = this.Triangles[triangleArrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y;

                                    //left point
                                    leftPoint = Math.Min(leftPoint, x);

                                    //right point
                                    rightPoint = Math.Max(rightPoint, x);

                                    //frontest point
                                    frontPoint = Math.Max(frontPoint, y);

                                    //Depthest point
                                    backPoint = Math.Min(backPoint, y);
                                }
                            }
                        }
                    }

                    foreach (var supportCone in this.TotalObjectSupportCones)
                    {
                        if (!supportCone.Hidden)
                        {
                            if (supportCone.Triangles != null)
                            {
                                for (var triangleArrayIndex = 0; triangleArrayIndex < supportCone.Triangles.Count; triangleArrayIndex++)
                                {
                                    for (var triangleIndex = 0; triangleIndex < supportCone.Triangles[triangleArrayIndex].Count; triangleIndex++)
                                    {
                                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                                        {
                                            x = supportCone.Triangles[triangleArrayIndex][triangleIndex].Vectors[vectorIndex].Position.X + supportCone.MoveTranslationX;
                                            y = supportCone.Triangles[triangleArrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y + supportCone.MoveTranslationY;

                                            //left point
                                            leftPoint = Math.Min(leftPoint, x);

                                            //right point
                                            rightPoint = Math.Max(rightPoint, x);

                                            //frontest point
                                            frontPoint = Math.Max(frontPoint, y);

                                            //Depthest point
                                            backPoint = Math.Min(backPoint, y);

                                        }
                                    }
                                }
                            }
                        }
                    }

                    if (x != float.MaxValue && y != float.MaxValue)
                    {

                        var cubeWidth = (new Vector3(rightPoint + 4f, 0, 0) - new Vector3(leftPoint, 0, 0)).Length;
                        var cubeDepth = (new Vector3(frontPoint + 4f, 0, 0) - new Vector3(backPoint, 0, 0)).Length;
                        this.SupportBasementStructure = (new Cube(cubeWidth, cubeDepth, Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness)).AsSTLModel3D(this.Color);
                        for (var baseTriangleIndex = 0; baseTriangleIndex < this.SupportBasementStructure.Triangles[0].Count; baseTriangleIndex++)
                        {

                            //center object to 0,0
                            this.SupportBasementStructure.Triangles[0][baseTriangleIndex].Vectors[0].Position += new Vector3Class((-cubeWidth / 2), -(cubeDepth / 2), 0);
                            this.SupportBasementStructure.Triangles[0][baseTriangleIndex].Vectors[1].Position += new Vector3Class((-cubeWidth / 2), -(cubeDepth / 2), 0);
                            this.SupportBasementStructure.Triangles[0][baseTriangleIndex].Vectors[2].Position += new Vector3Class((-cubeWidth / 2), -(cubeDepth / 2), 0);
                        }

                        this.SupportBasementStructure.UpdateBoundries();
                        var centerSupportsX = ((rightPoint - leftPoint) / 2) + leftPoint;
                        var basementCenterX = ((this.SupportBasementStructure.RightPoint - this.SupportBasementStructure.LeftPoint) / 2) + this.SupportBasementStructure.LeftPoint;
                        var basementSupportCenterXOffset = centerSupportsX - basementCenterX;

                        var centerSupportsY = ((frontPoint - backPoint) / 2) + backPoint;
                        var basementCenterY = ((this.SupportBasementStructure.FrontPoint - this.SupportBasementStructure.BackPoint) / 2) + this.SupportBasementStructure.BackPoint;
                        var basementSupportCenterYOffset = centerSupportsY - basementCenterY;

                        var supportBaseCenter = new Vector3Class(basementSupportCenterXOffset, basementSupportCenterYOffset, 0);
                        ////move to center of support cone
                        for (var baseTriangleIndex = 0; baseTriangleIndex < this.SupportBasementStructure.Triangles[0].Count; baseTriangleIndex++)
                        {

                            //center object to 0,0
                            this.SupportBasementStructure.Triangles[0][baseTriangleIndex].Vectors[0].Position += supportBaseCenter;
                            this.SupportBasementStructure.Triangles[0][baseTriangleIndex].Vectors[1].Position += supportBaseCenter;
                            this.SupportBasementStructure.Triangles[0][baseTriangleIndex].Vectors[2].Position += supportBaseCenter;
                        }
                    }
                    else
                    {
                        this._supportBasement = false;
                        this.SupportBasementStructure = null;
                    }

                }
                else
                {
                    this._supportBasement = false;
                    this.SupportBasementStructure = null;
                }
            }
        }
        internal STLModel3D SupportBasementStructure { get; set; }

        internal void UpdateSupportBasement()
        {
            if (this.SupportBasement)
            {
                this.SupportBasement = false;
                this.SupportBasement = true;
            }
        }


        //[Browsable(true)]
        //[Category("Support")]
        //[DisplayName("Internal Support")]
        //[Description("Enable/Disable internal support")]
        //public bool InternalSupport
        //{
        //    get
        //    {
        //        return this._internalSupport;
        //    }
        //    set
        //    {
        //        if (this._internalSupport != value)
        //        {
        //            //this.InternalSupportProcessing?.Invoke(null, null);
        //            this._internalSupport = value;
        //        }
        //    }
        //}
        //[Browsable(true)]
        //[Category("Support")]
        //[DisplayName("Z-Support")]
        //[Description("Enable/Disable z-support")]
        //public bool ZSupport
        //{
        //    get
        //    {
        //        return this._zSupport;
        //    }
        //    set
        //    {
        //        if (this._zSupport != value)
        //        {
        //            //this.ZSupportProcessing?.Invoke(null, null);
        //            this._zSupport = value;
        //        }
        //    }
        //}
        [Browsable(true)]
        [Category("Support")]
        [DisplayName("Cross Support")]
        [Description("Enable/Disable Cross support")]
        public virtual bool CrossSupport
        {
            get
            {
                return this._crossSupport;
            }
            set
            {
                if (this._crossSupport != value)
                {
                    this.CrossSupportProcessing?.Invoke(null, null);
                    this._crossSupport = value;
                }
            }
        }


        internal long TotalTriangles
        {
            get
            {
                long numberOfTriangles = 0;
                for (var triangleArrayIndex = 0; triangleArrayIndex < this.Triangles.Count; triangleArrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < this.Triangles[triangleArrayIndex].Count; triangleIndex++)
                    {
                        numberOfTriangles++;
                    }
                }

                //support cones
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    for (var triangleArrayIndex = 0; triangleArrayIndex < supportCone.Triangles.Count; triangleArrayIndex++)
                    {
                        for (var triangleIndex = 0; triangleIndex < supportCone.Triangles[triangleArrayIndex].Count; triangleIndex++)
                        {
                            numberOfTriangles++;
                        }
                    }
                }


                //support bottom plane support
                if (this.SupportBasementStructure != null && this.SupportBasementStructure.Triangles != null)
                {
                    numberOfTriangles += this.SupportBasementStructure.Triangles[0].Count;
                }
                return numberOfTriangles;
            }
        }

        internal float Volume
        {
            get
            {
                this._volume = 0;
                foreach (var triangleArray in this.Triangles)
                {
                    foreach (var triangle in triangleArray)
                    {
                        this._volume += triangle.CalcSignedVolume();
                    }
                }

                if (this.SupportBasementStructure != null)
                {
                    foreach (var triangle in this.SupportBasementStructure.Triangles[0])
                    {
                        foreach (var triangleArray in this.Triangles)
                        {
                            triangle.CalcSignedVolume();
                            this._volume += triangle.Volume;
                        }
                    }
                }

                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    foreach (var triangle in supportCone.Triangles[0])
                    {
                        foreach (var triangleArray in this.Triangles)
                        {
                            triangle.CalcSignedVolume();
                            this._volume += triangle.Volume;
                        }
                    }
                }



                return this._volume;
            }
        }


        [Category("Scalability")]
        [DisplayName("All Axis Locked")]
        [Description("When all axis are locked the model is scaled by all axis")]
        public bool AxisLocked { get; set; }

        [Category("Scalability")]
        [DisplayName("Factor-X")]
        [Description("Scaling Factor X\nUnit: Factor")]
        public float ScaleFactorX
        {
            get
            {
                return this._scaleFactorX;
            }
            set
            {
                this.ScaleProcessing?.Invoke(null, new ScaleEventArgs() { Axis = this.AxisLocked ? ScaleEventArgs.TypeAxis.ALL : ScaleEventArgs.TypeAxis.X, ScaleFactor = value });
            }
        }
        [Category("Scalability")]
        [DisplayName("Factor-Y")]
        [Description("Scaling Factor Y\nUnit: Factor")]
        public float ScaleFactorY
        {
            get
            {
                return this._scaleFactorY;
            }
            set
            {
                this.ScaleProcessing?.Invoke(null, new ScaleEventArgs() { Axis = this.AxisLocked ? ScaleEventArgs.TypeAxis.ALL : ScaleEventArgs.TypeAxis.Y, ScaleFactor = value });
            }
        }
        [Category("Scalability")]
        [DisplayName("Factor-Z")]
        [Description("Scaling Factor Z\nUnit: Factor")]
        public float ScaleFactorZ
        {
            get
            {
                return this._scaleFactorZ;
            }
            set
            {
                this.ScaleProcessing?.Invoke(null, new ScaleEventArgs() { Axis = this.AxisLocked ? ScaleEventArgs.TypeAxis.ALL : ScaleEventArgs.TypeAxis.Z, ScaleFactor = value });
            }
        }
        [Browsable(false)]
        [Category("Rotation")]
        [DisplayName("Angle-X")]
        [Description("Rotation Angle X\nUnit: Degrees")]
        public float RotationAngleX
        {
            get
            {
                return this._rotationAngleX;
            }
            set
            {
                if (this._rotationAngleX != value)
                {
                    this.RotationProcessing?.Invoke(null, new RotationEventArgs() { Axis = RotationEventArgs.TypeAxis.X, RotationAngle = value });
                }
            }
        }
        [Browsable(false)]
        [Category("Rotation")]
        [DisplayName("Angle-Y")]
        [Description("Rotation Angle Y\nUnit: Degrees")]
        public float RotationAngleY
        {
            get
            {
                return this._rotationAngleY;
            }
            set
            {
                if (this._rotationAngleY != value)
                {
                    this.RotationProcessing?.Invoke(null, new RotationEventArgs() { Axis = RotationEventArgs.TypeAxis.Y, RotationAngle = value });
                }
            }
        }

        [Browsable(false)]
        [Category("Rotation")]
        [DisplayName("Angle-Z")]
        [Description("Rotation Angle Z\nUnit: Degrees")]
        public float RotationAngleZ
        {
            get
            {
                return this._rotationAngleZ;
            }
            set
            {
                if (this._rotationAngleZ != value)
                {
                    this.RotationProcessing?.Invoke(null, new RotationEventArgs() { Axis = RotationEventArgs.TypeAxis.Z, RotationAngle = value });
                }
            }
        }

        internal virtual bool Hidden
        {
            get { return this._hidden; }
            set
            {
                this._hidden = value;
            }
        }


        public List<SupportCone> SupportStructure;
        public List<SupportCone> SupportHelperStructure;
        public object SupportStructureLock = new object();

        internal Vector3Class Center
        {
            get
            {
                var distanceX = (new Vector3Class(this.LeftPoint, 0, 0) - new Vector3Class(this.RightPoint, 0, 0)).Length;
                var distanceY = (new Vector3Class(0, this.FrontPoint, 0) - new Vector3Class(0, this.BackPoint, 0)).Length;
                var distanceZ = (new Vector3Class(0, this.BottomPoint, 0) - new Vector3Class(0, this.TopPoint, 0)).Length;
                var x = this.LeftPoint + (distanceX / 2);
                var y = this.BackPoint + (distanceY / 2);
                var z = this.BottomPoint + (distanceZ / 2);

                var center = new Vector3Class(x, y, z);
                return center;
            }
        }

        #region SelectionBox

        internal Vector3Class[] SelectionBox
        {
            get
            {
                var selectionbox = new Vector3Class[24];

                //axis-x


                //front
                selectionbox[0] = new Vector3Class(this.LeftPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[1] = new Vector3Class(this.RightPoint, this.FrontPoint, this.BottomPoint);

                selectionbox[2] = new Vector3Class(this.RightPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[3] = new Vector3Class(this.RightPoint, this.FrontPoint, this.TopPoint);

                selectionbox[4] = new Vector3Class(this.RightPoint, this.FrontPoint, this.TopPoint);
                selectionbox[5] = new Vector3Class(this.LeftPoint, this.FrontPoint, this.TopPoint);

                selectionbox[6] = new Vector3Class(this.LeftPoint, this.FrontPoint, this.TopPoint);
                selectionbox[7] = new Vector3Class(this.LeftPoint, this.FrontPoint, this.BottomPoint);

                selectionbox[8] = new Vector3Class(this.LeftPoint, this.BackPoint, this.BottomPoint);
                selectionbox[9] = new Vector3Class(this.RightPoint, this.BackPoint, this.BottomPoint);

                selectionbox[10] = new Vector3Class(this.RightPoint, this.BackPoint, this.BottomPoint);
                selectionbox[11] = new Vector3Class(this.RightPoint, this.BackPoint, this.TopPoint);

                selectionbox[12] = new Vector3Class(this.RightPoint, this.BackPoint, this.TopPoint);
                selectionbox[13] = new Vector3Class(this.LeftPoint, this.BackPoint, this.TopPoint);

                selectionbox[14] = new Vector3Class(this.LeftPoint, this.BackPoint, this.TopPoint);
                selectionbox[15] = new Vector3Class(this.LeftPoint, this.BackPoint, this.BottomPoint);

                //front to back
                selectionbox[16] = new Vector3Class(this.LeftPoint, this.FrontPoint, this.TopPoint);
                selectionbox[17] = new Vector3Class(this.LeftPoint, this.BackPoint, this.TopPoint);

                selectionbox[18] = new Vector3Class(this.RightPoint, this.FrontPoint, this.TopPoint);
                selectionbox[19] = new Vector3Class(this.RightPoint, this.BackPoint, this.TopPoint);

                selectionbox[20] = new Vector3Class(this.LeftPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[21] = new Vector3Class(this.LeftPoint, this.BackPoint, this.BottomPoint);

                selectionbox[22] = new Vector3Class(this.RightPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[23] = new Vector3Class(this.RightPoint, this.BackPoint, this.BottomPoint);
                return selectionbox;
            }
        }

        //#region SelectionboxText

        //public void UpdateSelectionboxText()
        //{
        //    if (DAL.OS.OSProvider.IsWindows)
        //    {
        //        var fontSize = 5.7f;

        //        //height
        //        var selectionHeight = (new Vector3(0, 0, this.TopPoint) - new Vector3(0, 0, this.BottomPoint)).Length;
        //        var textAsStlModel = new STLModel3D() { Triangles = FontTessellationEngine.ConvertStringToTriangles("> " + selectionHeight.ToString("0.0#") + " <", FontStyle.Regular, fontSize) };
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Rotate(180, 0, 0, RotationEventArgs.TypeAxis.X);
        //        textAsStlModel.Rotate(180, 90, 0, RotationEventArgs.TypeAxis.Y);
        //        textAsStlModel.Rotate(180, 90, -90, RotationEventArgs.TypeAxis.Z);
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Triangles.UpdateWithMoveTranslation(new Vector3(this.RightPoint + 1 + textAsStlModel.RightPoint, -this.BackPoint, (this.BottomPoint + ((this.TopPoint - this.BottomPoint) / 2)) - (textAsStlModel.TopPoint / 2)));

        //        //faces needs to be flipped
        //        for (var triangleArrayIndex = 0; triangleArrayIndex < textAsStlModel.Triangles.Count; triangleArrayIndex++)
        //        {
        //            for (var triangleIndex = 0; triangleIndex < textAsStlModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
        //            {
        //                textAsStlModel.Triangles[triangleArrayIndex][triangleIndex].Flip();
        //            }
        //        }

        //        this._selectionboxTextHeightFacingFrontText = textAsStlModel.Triangles.ToVector3Array();

        //        textAsStlModel.Rotate(180, 90, 90, RotationEventArgs.TypeAxis.Z);
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Triangles.UpdateWithMoveTranslation(new Vector3(this.RightPoint + 1 + textAsStlModel.RightPoint, -this.BackPoint, (this.BottomPoint + ((this.TopPoint - this.BottomPoint) / 2)) - (textAsStlModel.TopPoint / 2)));

        //        this._selectionboxTextHeightFacingBackText = textAsStlModel.Triangles.ToVector3Array();

        //        //front
        //        var selectionWidth = (this.RightPoint - this.LeftPoint);
        //        textAsStlModel = new STLModel3D() { Triangles = FontTessellationEngine.ConvertStringToTriangles("> " + selectionWidth.ToString("0.0#") + " <", FontStyle.Regular, fontSize) };
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Rotate(180, 0, 0, RotationEventArgs.TypeAxis.X);
        //        textAsStlModel.Triangles.UpdateWithMoveTranslation(new Vector3(0, -this.FrontPoint - textAsStlModel.FrontPoint - 1, this.BottomPoint + 0.01f));

        //        //faces needs to be flipped
        //        for (var triangleArrayIndex = 0; triangleArrayIndex < textAsStlModel.Triangles.Count; triangleArrayIndex++)
        //        {
        //            for (var triangleIndex = 0; triangleIndex < textAsStlModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
        //            {
        //                textAsStlModel.Triangles[triangleArrayIndex][triangleIndex].Flip();
        //            }
        //        }

        //        this._selectionboxTextFrontFacingUpText = textAsStlModel.Triangles.ToVector3Array();

        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.Triangles.MirrorX();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Triangles.UpdateWithMoveTranslation(new Vector3(0, -this.FrontPoint - textAsStlModel.FrontPoint - 1, this.BottomPoint + 0.01f));

        //        //faces needs to be flipped
        //        for (var triangleArrayIndex = 0; triangleArrayIndex < textAsStlModel.Triangles.Count; triangleArrayIndex++)
        //        {
        //            for (var triangleIndex = 0; triangleIndex < textAsStlModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
        //            {
        //                textAsStlModel.Triangles[triangleArrayIndex][triangleIndex].Flip();
        //            }
        //        }


        //        this._selectionboxTextFrontFacingDownText = textAsStlModel.Triangles.ToVector3Array();

        //        //depth
        //        var selectionDepth = (new Vector3(0, 0, this.BackPoint) - new Vector3(0, 0, this.FrontPoint)).Length;

        //        textAsStlModel = new STLModel3D() { Triangles = FontTessellationEngine.ConvertStringToTriangles("> " + selectionDepth.ToString("0.0#") + " <", FontStyle.Regular, fontSize) };
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Rotate(180, 0, 0, RotationEventArgs.TypeAxis.X);
        //        textAsStlModel.Rotate(180, 0, -90, RotationEventArgs.TypeAxis.Z);
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Triangles.UpdateWithMoveTranslation(new Vector3(this.LeftPoint + textAsStlModel.LeftPoint - 1, 0, this.TopPoint));

        //        for (var triangleArrayIndex = 0; triangleArrayIndex < textAsStlModel.Triangles.Count; triangleArrayIndex++)
        //        {
        //            for (var triangleIndex = 0; triangleIndex < textAsStlModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
        //            {
        //                textAsStlModel.Triangles[triangleArrayIndex][triangleIndex].Flip();
        //            }
        //        }

        //        this._selectionboxTextDepthFacingUpText = textAsStlModel.Triangles.ToVector3Array();

        //        textAsStlModel.Triangles.MirrorX();
        //        textAsStlModel.Rotate(180, 0, 0, RotationEventArgs.TypeAxis.X);
        //        textAsStlModel.Rotate(180, 0, -90, RotationEventArgs.TypeAxis.Z);
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.UpdateDefaultCenter();
        //        textAsStlModel.UpdateBoundries();
        //        textAsStlModel.Triangles.UpdateWithMoveTranslation(new Vector3(this.LeftPoint + textAsStlModel.LeftPoint - 1, 0, this.TopPoint));

        //        for (var triangleArrayIndex = 0; triangleArrayIndex < textAsStlModel.Triangles.Count; triangleArrayIndex++)
        //        {
        //            for (var triangleIndex = 0; triangleIndex < textAsStlModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
        //            {
        //                textAsStlModel.Triangles[triangleArrayIndex][triangleIndex].Flip();
        //            }
        //        }

        //        this._selectionboxTextDepthFacingDownText = textAsStlModel.Triangles.ToVector3Array();
        //    }

        //}

        //#endregion

        #endregion

        public STLModel3D() : base(true) { }

        public STLModel3D(STLModel3D stlModel, bool enableIndexColor, bool updateConnections, bool deselectModels)
            : base(true)
        {
            stlModel.MoveTranslation = new Vector3Class();
            if (deselectModels)
            {
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D)
                    {
                        ((STLModel3D)object3d).Selected = false;
                    }
                }
            }

            //triangles
            this.Triangles = new TriangleInfoList();
            if (stlModel.SliceIndexes != null)
            {
                this.SliceIndexes = new SortedDictionary<float, List<TriangleConnectionInfo>>();
                lock (stlModel.SliceIndexes)
                {
                    foreach (var sliceIndex in stlModel.SliceIndexes)
                    {
                        this.SliceIndexes.Add(sliceIndex.Key, sliceIndex.Value);
                    }
                }
            }

            if (stlModel.ModelBleedingSliceIndexes != null)
            {
                this.ModelBleedingSliceIndexes = new SortedDictionary<float, List<TriangleConnectionInfo>>();
                lock (stlModel.ModelBleedingSliceIndexes)
                {
                    foreach (var sliceIndex in stlModel.ModelBleedingSliceIndexes)
                    {
                        this.ModelBleedingSliceIndexes.Add(sliceIndex.Key, sliceIndex.Value);
                    }
                }
            }

            this.AxisLocked = stlModel.AxisLocked;

            //general settings
            this.LeftPoint = stlModel.LeftPoint;
            this.RightPoint = stlModel.RightPoint;
            this.TopPoint = stlModel.TopPoint;
            this.BottomPoint = stlModel.BottomPoint;
            this.BackPoint = stlModel.BackPoint;
            this.FrontPoint = stlModel.FrontPoint;
            this.VertexArray = stlModel.VertexArray;
            this._scaleFactorX = stlModel._scaleFactorX;
            this._scaleFactorY = stlModel._scaleFactorY;
            this._scaleFactorZ = stlModel._scaleFactorZ;
            this._rotationAngleX = stlModel._rotationAngleX;
            this._rotationAngleY = stlModel._rotationAngleY;
            this._rotationAngleZ = stlModel._rotationAngleZ;
            this._supportDistance = stlModel._supportDistance;
            //this._zSupport = stlModel._zSupport;
            this._crossSupport = stlModel._crossSupport;
            this._internalSupport = stlModel._internalSupport;
            this.MoveTranslation = new Vector3Class(stlModel.MoveTranslation.X + (stlModel.RightPoint * 2) + 2, stlModel.MoveTranslation.Y, stlModel.MoveTranslation.Z);
            this.ObjectType = stlModel.ObjectType;
            this.Selected = true;
            this.FileName = stlModel.FileName;
            if (enableIndexColor)
            {
                this._color = Color.FromArgb(ObjectView.NextObjectIndex, stlModel.Color);
            }
            else
            {
                this._color = Color.FromArgb(255, stlModel.Color);
            }

            //triangles
            if (updateConnections) this.Triangles.UpdateConnections();
            for (var arrayIndex = 0; arrayIndex < stlModel.Triangles.Count; arrayIndex++)
            {
                if (this.Triangles.Count == arrayIndex)
                {
                    this.Triangles.Add(new List<Triangle>());
                }

                for (var triangleIndex = 0; triangleIndex < stlModel.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    this.Triangles[arrayIndex].Add((Triangle)stlModel.Triangles[arrayIndex][triangleIndex].Clone());
                    this.Triangles[arrayIndex][triangleIndex].Vectors[0].Color.A = this.ColorAsByte4.A;
                    this.Triangles[arrayIndex][triangleIndex].Vectors[1].Color.A = this.ColorAsByte4.A;
                    this.Triangles[arrayIndex][triangleIndex].Vectors[2].Color.A = this.ColorAsByte4.A;
                }
            }


            //support structure
            if (stlModel.SupportStructure != null)
            {
                this.SupportStructure = new List<SupportCone>();
                foreach (var supportCone in stlModel.SupportStructure)
                {
                    var supportConeClone = supportCone.Clone(this);
                    supportConeClone.MoveTranslation = new Vector3Class(supportCone.MoveTranslation.X, supportCone.MoveTranslation.Y, supportCone.MoveTranslation.Z);
                    this.SupportStructure.Add(supportConeClone);
                }
            }

            if (stlModel.SupportHelperStructure == null)
            {
                stlModel.SupportHelperStructure = new List<SupportCone>();
            }

            //support baseplate
            this._supportBasement = stlModel.SupportBasement;
            if (stlModel.SupportBasementStructure != null)
            {
                this.SupportBasementStructure = new STLModel3D(stlModel.SupportBasementStructure, false, false, false);
            }

            //horizonal surface
            this.Triangles.HorizontalSurfaces = new TriangleSurfaceInfoList();
            foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
            {
                this.Triangles.HorizontalSurfaces.Add(surface.Clone(this));
            }

            //flat surface
            this.Triangles.FlatSurfaces = new TriangleSurfaceInfoList();
            foreach (var surface in stlModel.Triangles.FlatSurfaces)
            {
                this.Triangles.FlatSurfaces.Add(surface.Clone(this));
            }

            //update selection text
            //this.UpdateSelectionboxText();

            //update binding
            this._bindingSupported = stlModel._bindingSupported;

            if (frmStudioMain.SceneControl != null && this.VBOIndexes != null)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate { this.UpdateBinding(); }));
                }
                else
                {
                    this.UpdateBinding();
                }
            }

            this.Loaded = true;
        }

        internal PolyTree GetSliceContourStructure(int sliceIndex, float sliceHeight, AtumPrinter selectedPrinter, Material selectedMaterial,
            out PolyTree modelAngledContours, out PolyTree modelWallContours, List<float[]> angleSideAngles, List<float[]> wallSideAngles,
            bool includeAngledContours = false, bool includeWallContours = false)
        {
            var modelZPolys = RenderEngine.GetZIntersections(RenderEngine.GetZPolys(this, this.SliceIndexes, sliceHeight), sliceHeight);
            using (var modelWithSupportSlice = new Slices.Slice(sliceIndex, modelZPolys, new List<List<SlicePolyLine3D>>(), new List<SlicePolyLine3D>(), sliceHeight))
            {
                modelWithSupportSlice.ConvertModelIntersectionsToPolyTrees(sliceHeight, selectedPrinter,
                    selectedMaterial, selectedMaterial.SupportProfiles.First(), angleSideAngles, wallSideAngles, false,
                includeAngledContours, includeWallContours);

                modelAngledContours = modelWithSupportSlice.ModelAnglesPolyTree;
                modelWallContours = modelWithSupportSlice.ModelWallPolyTree;

                if (modelWithSupportSlice.ModelPolyTrees.Length > 0 && modelWithSupportSlice.ModelPolyTrees[0].Count > 0)
                {
                    // TriangleHelper.SavePolyNodesContourToPng(modelWithSupportSlice.ModelPolyTrees[0][0]._allPolys, sliceHeight.ToString("00.00") + "-m");

                    return modelWithSupportSlice.ModelPolyTrees[0][0];
                }
            }

            return new PolyTree();
        }

        internal static PolyTree GetSliceContoursForBaseSTLModel3D(STLModel3D baseObject, int sliceIndex, float sliceHeight, AtumPrinter selectedPrinter)
        {
            var zPolys = RenderEngine.GetZIntersectionsBaseSTLModel3D(baseObject, sliceHeight);
            using (var baseSlice = new Slices.Slice(sliceIndex, null, new List<List<SlicePolyLine3D>>() { zPolys }, new List<SlicePolyLine3D>(), sliceHeight))
            {
                baseSlice.ConvertSupportIntersectionsToPolyTrees(sliceHeight, selectedPrinter);

                if (baseSlice.SupportPolyTrees.Length > 0 && baseSlice.SupportPolyTrees[0].Count > 0)
                {
                    return baseSlice.SupportPolyTrees[0][0];
                }
            }

            return new PolyTree();
        }


        internal static PolyTree GetSliceContoursForSupportConeV2(SupportCone supportCone, int sliceIndex, float sliceHeight, AtumPrinter selectedPrinter)
        {
            var supportZPolys = RenderEngine.GetZIntersectionsSupport(supportCone, sliceHeight);
            using (var supportSlice = new Slices.Slice(sliceIndex, null, new List<List<SlicePolyLine3D>>() { supportZPolys }, new List<SlicePolyLine3D>(), sliceHeight))
            {
                supportSlice.ConvertSupportIntersectionsToPolyTrees(sliceHeight, selectedPrinter);

                if (supportSlice.SupportPolyTrees.Length > 0 && supportSlice.SupportPolyTrees[0].Count > 0)
                {
                    //TriangleHelper.SavePolyNodesContourToPng(new List<PolyNode>() { supportSlice.SupportPolyTrees[0][0]._allPolys[0] }, "test");
                    return supportSlice.SupportPolyTrees[0][0];
                }
            }

            return new PolyTree();
        }

        internal List<MagsAIPolyLines> GetSlicePolyLines(int sliceIndex, float sliceHeight, SupportProfile selectedMaterialProfile, AtumPrinter selectedPrinter)
        {
            var result = new List<MagsAIPolyLines>();
            var modelZPolys = RenderEngine.GetZIntersections(RenderEngine.GetZPolys(this, this.SliceIndexes, sliceHeight), sliceHeight);
            using (var modelWithSupportSlice = new Slices.Slice(sliceIndex, modelZPolys, new List<List<SlicePolyLine3D>>(), new List<SlicePolyLine3D>(), sliceHeight))
            {
                var polyLinesAsPolyNodes = modelWithSupportSlice.ConvertModelIntersectionsToPolyLines(sliceHeight, selectedMaterialProfile, selectedPrinter, false);
                result.AddRange(polyLinesAsPolyNodes);
            }

            return result;
        }

        internal void MoveModelWithTranslationZ(Vector3Class moveTranslation)
        {
            this.Triangles.UpdateWithMoveTranslation(moveTranslation);

            if (this.TotalObjectSupportCones != null)
            {
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    supportCone.Triangles.UpdateWithMoveTranslation(moveTranslation);
                }
            }
        }

        internal List<SupportCone> TotalObjectSupportCones
        {
            get
            {
                var result = new List<SupportCone>();
                try
                {

                    if (this.SupportStructure != null)
                    {
                        foreach (var supportCone in this.SupportStructure)
                        {
                            if (supportCone != null)
                            {
                                result.Add(supportCone);
                            }
                        }
                    }

                    if (this.Triangles != null)
                    {
                        if (this.Triangles.HorizontalSurfaces != null)
                        {
                            foreach (var surface in this.Triangles.HorizontalSurfaces)
                            {
                                if (surface != null)
                                {
                                    if (surface.SupportStructure != null)
                                    {
                                        foreach (var supportCone in surface.SupportStructure)
                                        {
                                            if (supportCone != null)
                                            {
                                                result.Add(supportCone);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (this.Triangles.FlatSurfaces != null)
                        {
                            foreach (var surface in this.Triangles.FlatSurfaces)
                            {
                                if (surface != null)
                                {
                                    if (surface.SupportStructure != null)
                                    {
                                        foreach (var supportCone in surface.SupportStructure)
                                        {
                                            if (supportCone != null)
                                            {
                                                result.Add(supportCone);
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch
                {

                }

                return result;
            }
        }

        internal List<Triangle> ExportToSTL()
        {
            var stlTriangles = new List<Triangle>();
            var stlModel = this;
            //triangles
            //write model
            for (var triangleArrayIndex = 0; triangleArrayIndex < stlModel.Triangles.Count; triangleArrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < stlModel.Triangles[triangleArrayIndex].Count; triangleIndex++)
                {
                    var triangle = (Triangle)stlModel.Triangles[triangleArrayIndex][triangleIndex].Clone();
                    triangle.Vectors[0].Position += new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                    triangle.Vectors[1].Position += new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                    triangle.Vectors[2].Position += new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                    stlTriangles.Add(triangle);
                }
            }

            //write support
            foreach (var supportCone in stlModel.TotalObjectSupportCones)
            {
                for (var triangleArrayIndex = 0; triangleArrayIndex < supportCone.Triangles.Count; triangleArrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < supportCone.Triangles[triangleArrayIndex].Count; triangleIndex++)
                    {
                        var triangle = (Triangle)supportCone.Triangles[triangleArrayIndex][triangleIndex].Clone();
                        triangle.Vectors[0].Position += supportCone.MoveTranslation + new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                        triangle.Vectors[1].Position += supportCone.MoveTranslation + new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                        triangle.Vectors[2].Position += supportCone.MoveTranslation + new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                        stlTriangles.Add(triangle);
                    }
                }
            }


            //support plane
            if (stlModel.SupportBasementStructure != null && stlModel.SupportBasementStructure.Triangles != null)
            {
                for (var triangleIndex = 0; triangleIndex < stlModel.SupportBasementStructure.Triangles[0].Count; triangleIndex++)
                {
                    var triangle = (Triangle)stlModel.SupportBasementStructure.Triangles[0][triangleIndex].Clone();
                    triangle.Vectors[0].Position += new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                    triangle.Vectors[1].Position += new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                    triangle.Vectors[2].Position += new Vector3Class(stlModel.MoveTranslationX, stlModel.MoveTranslationY, 0);
                    stlTriangles.Add(triangle);
                }
            }

            return stlTriangles;
        }

        internal WorkspaceSTLModel ExportToXMLFile()
        {
            var exportSTLModel = new WorkspaceSTLModel();
            exportSTLModel.FileName = this.FileName;
            exportSTLModel.Color = ColorTranslator.ToHtml(this.Color);
            exportSTLModel.MoveTranslation = this.MoveTranslation.ToStruct();
            exportSTLModel.CrossSupport = this.CrossSupport;
            //exportSTLModel.InternalSupport = this.InternalSupport;
            exportSTLModel.RotationAngleX = this.RotationAngleX;
            exportSTLModel.RotationAngleY = this.RotationAngleY;
            exportSTLModel.RotationAngleZ = this.RotationAngleZ;
            exportSTLModel.PreviousScaleFactorX = this.PreviousScaleFactorX;
            exportSTLModel.PreviousScaleFactorY = this.PreviousScaleFactorY;
            exportSTLModel.PreviousScaleFactorZ = this.PreviousScaleFactorZ;
            exportSTLModel.ScaleFactorX = this.ScaleFactorX;
            exportSTLModel.ScaleFactorY = this.ScaleFactorY;
            exportSTLModel.ScaleFactorZ = this.ScaleFactorZ;
            exportSTLModel.SupportDistance = this.SupportDistance;
            //exportSTLModel.ZSupport = this.ZSupport;
            exportSTLModel.AxisLocked = this.AxisLocked;

            //support cones
            if (this.SupportStructure != null)
            {
                foreach (var supportCone in this.SupportStructure)
                {
                    exportSTLModel.SupportStructure.Add(new WorkspaceSTLModelSupport()
                    {
                        BottomHeight = supportCone.BottomHeight,
                        BottomRadius = supportCone.BottomRadius,
                        Color = ColorTranslator.ToHtml(supportCone.Color),
                        MiddleRadius = supportCone.MiddleRadius,
                        Position = supportCone.Position.ToStruct(),
                        SlicesCount = supportCone._slicesCount,
                        TopHeight = supportCone.TopHeight,
                        TopRadius = supportCone.TopRadius,
                        TotalHeight = supportCone.TotalHeight,
                        MoveTranslation = supportCone.MoveTranslation.ToStruct(),
                        RotationAngleX = supportCone.RotationAngleX,
                        RotationAngleZ = supportCone.RotationAngleZ,
                        SupportPenetrationEnabled = supportCone.PenetrationEnabled,
                        BottomWidthCorrection = supportCone.BottomWidthCorrection
                    });

                    ExportingToAPFProgress?.Invoke(null, null);
                }
            }

            // Linked clones
            exportSTLModel.LinkedClones = this.LinkedClones;

            // basement support
            exportSTLModel.SupportBasement = this.SupportBasement;
            if (this.SupportBasementStructure != null)
            {
                exportSTLModel.BasementObject = this.SupportBasementStructure.Triangles.ExportTrianglesAsBinarySTL();
            }

            //horizontal surfaces
            try
            {
                if (this.Triangles != null)
                {
                    foreach (var surface in this.Triangles.HorizontalSurfaces)
                    {
                        var workspaceSurface = new WorkspaceSTLModelHorizontalSurface()
                        {
                            BackPoint = surface.BackPoint,
                            BottomPoint = surface.BottomPoint,
                            CrossSupport = surface.CrossSupport,
                            FrontPoint = surface.FrontPoint,
                            HasEdgeDown = surface.HasEdgeDown,
                            LeftPoint = surface.LeftPoint,
                            RightPoint = surface.RightPoint,
                            SupportDistance = surface.SupportDistance,
                            // SupportPoints = (List<Vector3>)surface.SupportPoints.SelectMany(s => s.IntersectionPoint),
                            TopPoint = surface.TopPoint
                        };

                        foreach (var surfaceIndex in surface.Keys)
                        {
                            workspaceSurface.Indexes.Add(new WorkspaceSTLModelHorizontalSurfaceIndex()
                            {
                                ArrayIndex = surfaceIndex.ArrayIndex,
                                TriangleIndex = surfaceIndex.TriangleIndex,
                            });
                        }

                        foreach (var surfaceSupportCone in surface.SupportStructure)
                        {

                            workspaceSurface.SupportStructure.Add(new WorkspaceSTLModelSupport()
                            {
                                BottomHeight = surfaceSupportCone.BottomHeight,
                                BottomRadius = surfaceSupportCone.BottomRadius,
                                Color = ColorTranslator.ToHtml(surfaceSupportCone.Color),
                                MiddleRadius = surfaceSupportCone.MiddleRadius,
                                Position = surfaceSupportCone.Position.ToStruct(),
                                SlicesCount = surfaceSupportCone._slicesCount,
                                TopHeight = surfaceSupportCone.TopHeight,
                                TopRadius = surfaceSupportCone.TopRadius,
                                TotalHeight = surfaceSupportCone.TotalHeight,
                                MoveTranslation = surfaceSupportCone.MoveTranslation.ToStruct(),
                                RotationAngleX = surfaceSupportCone.RotationAngleX,
                                RotationAngleZ = surfaceSupportCone.RotationAngleZ,
                                SupportPenetrationEnabled = surfaceSupportCone.PenetrationEnabled,
                                BottomWidthCorrection = surfaceSupportCone.BottomWidthCorrection
                            });

                            ExportingToAPFProgress?.Invoke(null, null);
                        }

                        exportSTLModel.HorizontalSurfacesAsObjects.Add(workspaceSurface);
                    }

                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
            //exportSTLModel.HorizontalSurfaces = this.Triangles.HorizontalSurfaces.ToByteArray();

            //flat surfaces
            foreach (var surface in this.Triangles.FlatSurfaces)
            {
                var workspaceSurface = new WorkspaceSTLModelFlatSurface()
                {
                    BackPoint = surface.BackPoint,
                    BottomPoint = surface.BottomPoint,
                    CrossSupport = surface.CrossSupport,
                    FrontPoint = surface.FrontPoint,
                    HasEdgeDown = surface.HasEdgeDown,
                    LeftPoint = surface.LeftPoint,
                    RightPoint = surface.RightPoint,
                    SupportDistance = surface.SupportDistance,
                    //  SupportPoints = surface.SupportPoints,
                    TopPoint = surface.TopPoint
                };

                foreach (var surfaceIndex in surface.Keys)
                {
                    workspaceSurface.Indexes.Add(new WorkspaceSTLModelFlatSurfaceIndex()
                    {
                        ArrayIndex = surfaceIndex.ArrayIndex,
                        TriangleIndex = surfaceIndex.TriangleIndex,
                    });
                }

                foreach (var surfaceSupportCone in surface.SupportStructure)
                {

                    workspaceSurface.SupportStructure.Add(new WorkspaceSTLModelSupport()
                    {
                        BottomHeight = surfaceSupportCone.BottomHeight,
                        BottomRadius = surfaceSupportCone.BottomRadius,
                        Color = ColorTranslator.ToHtml(surfaceSupportCone.Color),
                        MiddleRadius = surfaceSupportCone.MiddleRadius,
                        Position = surfaceSupportCone.Position.ToStruct(),
                        SlicesCount = surfaceSupportCone._slicesCount,
                        TopHeight = surfaceSupportCone.TopHeight,
                        TopRadius = surfaceSupportCone.TopRadius,
                        TotalHeight = surfaceSupportCone.TotalHeight,
                        MoveTranslation = surfaceSupportCone.MoveTranslation.ToStruct(),
                        RotationAngleX = surfaceSupportCone.RotationAngleX,
                        RotationAngleZ = surfaceSupportCone.RotationAngleZ,
                        SupportPenetrationEnabled = surfaceSupportCone.PenetrationEnabled,
                        BottomWidthCorrection = surfaceSupportCone.BottomWidthCorrection
                    });
                }

                exportSTLModel.FlatSurfacesAsObjects.Add(workspaceSurface);
            }

            exportSTLModel.Object = this.Triangles.ExportTrianglesAsBinarySTL();

            //export MAGS AI marked triangles
            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    if (this.Triangles[arrayIndex][triangleIndex].Properties == Triangle.typeTriangleProperties.AutoRotationSelected)
                    {
                        exportSTLModel.MAGSAIMarkedTriangles.Add(new TriangleConnectionInfo() { ArrayIndex = (ushort)arrayIndex, TriangleIndex = (ushort)triangleIndex });
                    }
                }
            }

            ExportingToAPFProgress?.Invoke(null, null);

            return exportSTLModel;
        }

        public void VerticalMirror(bool updateSurfacePlanes = true, bool updateModel = true)
        {
            if (this.Triangles != null)
            {
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var trianglesCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        this.Triangles[arrayIndex][triangleIndex].VerticalMirror(updateModel);
                    }
                }
            }

            //support cone
            if (this.SupportStructure != null)
            {
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    supportCone.VerticalMirror(updateModel, false);
                }
            }

            //support basement
            if (this.SupportBasementStructure != null)
            {
                this.SupportBasementStructure.VerticalMirror(updateModel, false);
            }

            if (updateSurfacePlanes)
            {
                if (this.Triangles != null)
                {
                    this.Triangles.CalcFlatSurfaces();
                    this.Triangles.CalcHorizontalSurfaces(this.MoveTranslationZ);
                }

                this.UpdateBoundries();
                this.UpdateBinding();
            }
        }

        public void HorizontalMirror(bool updateSurfacePlanes = true, bool updateModel = true)
        {
            if (this.Triangles != null)
            {
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var trianglesCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        this.Triangles[arrayIndex][triangleIndex].HorizontalMirror(updateModel);
                    }
                }
            }

            //support cone
            if (this.SupportStructure != null)
            {
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    supportCone.HorizontalMirror(false, false);
                }
            }

            //support basement
            if (this.SupportBasementStructure != null)
            {
                this.SupportBasementStructure.HorizontalMirror(updateModel, false);
                this.SupportBasementStructure.UpdateBinding();
            }

            if (updateSurfacePlanes)
            {
                if (this.Triangles != null)
                {
                    this.Triangles.CalcFlatSurfaces();
                    this.Triangles.CalcHorizontalSurfaces(this.MoveTranslationZ);
                }

                this.UpdateBoundries();
                this.UpdateBinding();
            }
        }

        public STLModel3D(TypeObject objectType, bool bindingSupported)
            : base(true)
        {

            this.ObjectType = objectType;
            this._bindingSupported = bindingSupported;
            this.SupportStructure = new List<SupportCone>();
        }

        public STLModel3D(TypeObject objectType, bool bindingSupported, WorkspaceSTLModel workspaceModel, int modelIndex, bool isUndoAction)
            : base(true)
        {
            
            ProgressBarManager.UpdateMainPercentage(5);
            this.ObjectType = objectType;
            this._bindingSupported = bindingSupported;

            this.SupportStructure = new List<SupportCone>();

            using (var ms = new MemoryStream())
            {
                ms.Write(workspaceModel.Object, 0, workspaceModel.Object.Length);
                ms.Position = 0;
                this.FileName = workspaceModel.FileName;
                this.LoadSTL_Binary(ms, false);
            }

            ProgressBarManager.UpdateMainPercentage(10);
            if (workspaceModel.LinkedClones != null)
            {
                this.LinkedClones = workspaceModel.LinkedClones;
            }


            //load MAGS AI marked triangles
            ProgressBarManager.UpdateMainPercentage(15);
            if (workspaceModel.MAGSAIMarkedTriangles != null)
            {
                foreach (var magsAIMarkedTriangle in workspaceModel.MAGSAIMarkedTriangles)
                {
                    this.Triangles[magsAIMarkedTriangle.ArrayIndex][magsAIMarkedTriangle.TriangleIndex].Properties = Triangle.typeTriangleProperties.AutoRotationSelected;
                }
            }

            ProgressBarManager.UpdateMainPercentage(20);

            //update minmaxz/default center
            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    this.Triangles[arrayIndex][triangleIndex].CalcCenter();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxX();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxY();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                    this.Triangles[arrayIndex][triangleIndex].CalcVolume();
                    this.Triangles.GetConnectedTriangles(this.Triangles[arrayIndex][triangleIndex].Index);
                }
            }

            ProgressBarManager.UpdateMainPercentage(25);

            this._color = Color.FromArgb(modelIndex, ColorTranslator.FromHtml(workspaceModel.Color));
            this.MoveTranslation = new Vector3Class(workspaceModel.MoveTranslation.X, workspaceModel.MoveTranslation.Y, workspaceModel.MoveTranslation.Z);
            this._crossSupport = workspaceModel.CrossSupport;
            //            this._internalSupport = workspaceModel.InternalSupport;
            this._rotationAngleX = workspaceModel.RotationAngleX;
            this._rotationAngleY = workspaceModel.RotationAngleY;
            this._rotationAngleZ = workspaceModel.RotationAngleZ;
            this._scaleFactorX = workspaceModel.ScaleFactorX;
            this._scaleFactorY = workspaceModel.ScaleFactorY;
            this._scaleFactorZ = workspaceModel.ScaleFactorZ;
            this.PreviousScaleFactorX = workspaceModel.PreviousScaleFactorX;
            this.PreviousScaleFactorX = workspaceModel.PreviousScaleFactorY;
            this.PreviousScaleFactorZ = workspaceModel.PreviousScaleFactorZ;
            this._supportDistance = workspaceModel.SupportDistance;
            //    this._zSupport = workspaceModel.ZSupport;
            this.AxisLocked = workspaceModel.AxisLocked;
            this._supportBasement = workspaceModel.SupportBasement;

            if (this._supportBasement)
            {
                using (var ms = new MemoryStream())
                {
                    ms.Write(workspaceModel.BasementObject, 0, workspaceModel.BasementObject.Length);
                    ms.Position = 0;
                    this.SupportBasementStructure = new STLModel3D();
                    this.SupportBasementStructure.LoadSTL_Binary(ms, false);
                    this.SupportBasementStructure._scaleFactorX = this.SupportBasementStructure._scaleFactorY = this.SupportBasementStructure._scaleFactorZ = 1f;
                }
            }

            ProgressBarManager.UpdateMainPercentage(30);

            this.Triangles.UpdateConnections();
            this.UpdateBoundries();
            //this.UpdateSelectionboxText();
            this.Triangles.UpdateFaceColors(this.ColorAsByte4, false);
            this.ProcessModelGeometry(this); //parts

            this.BindModel();
            this.UpdateBinding();
            this.Loaded = true;
            ObjectView.AddModel(this, true, false);

            //update main control
            // frmStudioMain.SceneControl.Render();
            //add support structure

            var totalSupportCone = workspaceModel.SupportStructure.Count;
            var supportPointEvents = new List<ManualResetEvent>();
            if (workspaceModel.SupportStructure != null)
            {
                foreach (var object3dSupport in workspaceModel.SupportStructure)
                {
                    var object3dPosition = object3dSupport.Position;
                    var supportPointEvent = new ManualResetEvent(false);
                    var groundSupport = false;
                    if (workspaceModel.SupportBasement && (object3dSupport.Position.Z == UserProfileManager.UserProfile.SupportEngine_Basement_Thickness))
                    {
                        groundSupport = true;
                    }
                    else if (object3dSupport.Position.Z == 0)
                    {
                        groundSupport = true;
                    }

                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        ImportEngine.AddManualSupportCone(object3dSupport.TotalHeight, object3dSupport.TopHeight, object3dSupport.TopRadius, object3dSupport.MiddleRadius, object3dSupport.BottomHeight, object3dSupport.BottomRadius,
                            object3dSupport.SlicesCount, new Vector3Class(object3dSupport.Position), new Vector3Class(), Color.FromArgb(this.Color.A + 100, ColorTranslator.FromHtml(object3dSupport.Color)), this, new Vector3Class(object3dSupport.MoveTranslation),
                            object3dSupport.RotationAngleX, object3dSupport.RotationAngleZ, groundSupport: groundSupport, useSupportPenetration: true, supportBottomWidthCorrection: object3dSupport.BottomWidthCorrection);
                        supportPointEvent.Set();
                    });
                    supportPointEvents.Add(supportPointEvent);
                }
            }

            ProgressBarManager.UpdateMainPercentage(50);

            if (supportPointEvents.Count > 0)
            {
                int supportConesCompleted = 0;
                while (supportConesCompleted != supportPointEvents.Count)
                {
                    supportConesCompleted = 0;
                    Thread.Sleep(50);

                    foreach (var supportPointEvent in supportPointEvents)
                    {
                        if (supportPointEvent.WaitOne(0))
                        {
                            supportConesCompleted++;
                        }
                    }

                    ProgressBarManager.UpdateMainPercentage(50 + (((float)supportConesCompleted / (float)supportPointEvents.Count)) * 15f);
                }
            }

            ProgressBarManager.UpdateMainPercentage(65);

            //var contoursFoundInProjectFile = false;
            var horizontalSupportEvents = new List<ManualResetEvent>();
            foreach (var importHorizontalSurface in workspaceModel.HorizontalSurfacesAsObjects)
            {
                var horizontalSurface = new TriangleSurfaceInfo()
                {
                    BackPoint = importHorizontalSurface.BackPoint,
                    BottomPoint = importHorizontalSurface.BottomPoint,
                    CrossSupport = importHorizontalSurface.CrossSupport,
                    FrontPoint = importHorizontalSurface.FrontPoint,
                    HasEdgeDown = importHorizontalSurface.HasEdgeDown,
                    LeftPoint = importHorizontalSurface.LeftPoint,
                    RightPoint = importHorizontalSurface.RightPoint,
                    //   SupportPoints = importHorizontalSurface.SupportPoints,
                    SupportDistance = importHorizontalSurface.SupportDistance,
                    TopPoint = importHorizontalSurface.TopPoint
                };

                foreach (var importSurfaceIndex in importHorizontalSurface.Indexes)
                {
                    horizontalSurface.Add(new TriangleConnectionInfo()
                    {
                        ArrayIndex = importSurfaceIndex.ArrayIndex,
                        TriangleIndex = importSurfaceIndex.TriangleIndex,
                    }, false);
                }

                foreach (var importSupportCone in importHorizontalSurface.SupportStructure)
                {
                    var groundSupport = (importSupportCone.Position.Z == 0 && !workspaceModel.SupportBasement) || (importSupportCone.Position.Z == Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness && workspaceModel.SupportBasement) ? true : false;
                    var horizontalSupportEvent = new ManualResetEvent(false);
                    var surfaceClone = horizontalSurface;

                    ThreadPool.QueueUserWorkItem(arg =>
                    {
                        ImportEngine.AddGridSupportCone(importSupportCone.TotalHeight, importSupportCone.TopHeight, importSupportCone.TopRadius, importSupportCone.MiddleRadius, importSupportCone.BottomHeight, importSupportCone.BottomRadius,
                    importSupportCone.SlicesCount, importSupportCone.Position, new Vector3(), Color.FromArgb(this.Color.A + 100, ColorTranslator.FromHtml(importSupportCone.Color)), this, importSupportCone.MoveTranslation,
                    importSupportCone.RotationAngleX, importSupportCone.RotationAngleZ, groundSupport, surfaceClone, useSupportPenetration: importSupportCone.SupportPenetrationEnabled, supportBottomWidthCorrection: importSupportCone.BottomWidthCorrection);

                        horizontalSupportEvent.Set();
                    });

                    horizontalSupportEvents.Add(horizontalSupportEvent);
                }


                horizontalSurface.UpdateBoundries(this.Triangles);

                this.Triangles.HorizontalSurfaces.Add(horizontalSurface);
            }

            if (horizontalSupportEvents.Count > 0)
            {
                int supportConesCompleted = 0;
                while (supportConesCompleted != horizontalSupportEvents.Count)
                {
                    supportConesCompleted = 0;
                    Thread.Sleep(50);

                    foreach (var supportPointEvent in horizontalSupportEvents)
                    {
                        if (supportPointEvent.WaitOne(0))
                        {
                            supportConesCompleted++;
                        }
                    }

                    ProgressBarManager.UpdateMainPercentage(65 + (((float)supportConesCompleted / (float)supportPointEvents.Count)) * 15);
                }
            }


            ProgressBarManager.UpdateMainPercentage(80);

            //add flat surfaces
            if (workspaceModel.FlatSurfaces != null)
            {
                var result = new TriangleSurfaceInfoList();

                BinaryFormatter bf = new BinaryFormatter();
                bf.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
                foreach (var surfaceAsByteArray in workspaceModel.FlatSurfaces)
                {
                    using (MemoryStream msSurfaceAsByteArray = new MemoryStream(surfaceAsByteArray))
                    {
                        try
                        {
                            this.Triangles.FlatSurfaces = (TriangleSurfaceInfoList)bf.Deserialize(msSurfaceAsByteArray);
                        }
                        catch (Exception exc)
                        {
                            //format exception
                            //convert binary data to xml string
                        }

                        foreach (var surface in this.Triangles.FlatSurfaces)
                        {
                            surface.Selected = false;
                        }
                    }
                }

                foreach (var surfaceSupportCone in this.Triangles.FlatSurfaces.SupportStructure)
                {
                    surfaceSupportCone._color = Color.FromArgb(this.Color.A + 100, surfaceSupportCone.Color.R, surfaceSupportCone.Color.G, surfaceSupportCone.Color.B);
                }
            }

            ProgressBarManager.UpdateMainPercentage(80);

            var flatSupportEvents = new List<ManualResetEvent>();
            if (workspaceModel.FlatSurfacesAsObjects != null)
            {
                foreach (var importFlatSurface in workspaceModel.FlatSurfacesAsObjects)
                {
                    var flatSurface = new TriangleSurfaceInfo()
                    {
                        BackPoint = importFlatSurface.BackPoint,
                        BottomPoint = importFlatSurface.BottomPoint,
                        CrossSupport = importFlatSurface.CrossSupport,
                        FrontPoint = importFlatSurface.FrontPoint,
                        HasEdgeDown = importFlatSurface.HasEdgeDown,
                        LeftPoint = importFlatSurface.LeftPoint,
                        RightPoint = importFlatSurface.RightPoint,
                        //   SupportPoints = importFlatSurface.SupportPoints,
                        SupportDistance = importFlatSurface.SupportDistance,
                        TopPoint = importFlatSurface.TopPoint
                    };

                    foreach (var importSurfaceIndex in importFlatSurface.Indexes)
                    {
                        flatSurface.Add(new TriangleConnectionInfo()
                        {
                            ArrayIndex = importSurfaceIndex.ArrayIndex,
                            TriangleIndex = importSurfaceIndex.TriangleIndex,
                        }, false);
                    }

                    foreach (var importSupportCone in importFlatSurface.SupportStructure)
                    {
                        var groundSupport = (importSupportCone.Position.Z == 0 && !workspaceModel.SupportBasement) || (importSupportCone.Position.Z == UserProfileManager.UserProfile.SupportEngine_Basement_Thickness && workspaceModel.SupportBasement) ? true : false;
                        var flatSupportEvent = new ManualResetEvent(false);
                        var surfaceClone = flatSurface;

                        ThreadPool.QueueUserWorkItem(arg =>
                        {
                            ImportEngine.AddGridSupportCone(importSupportCone.TotalHeight, importSupportCone.TopHeight, importSupportCone.TopRadius, importSupportCone.MiddleRadius, importSupportCone.BottomHeight, importSupportCone.BottomRadius,
                        importSupportCone.SlicesCount, importSupportCone.Position, new Vector3(), Color.FromArgb(this.Color.A + 100, ColorTranslator.FromHtml(importSupportCone.Color)), this, importSupportCone.MoveTranslation,
                        importSupportCone.RotationAngleX, importSupportCone.RotationAngleZ, groundSupport, surfaceClone, useSupportPenetration: true, supportBottomWidthCorrection: importSupportCone.BottomWidthCorrection);

                            flatSupportEvent.Set();
                        });

                        flatSupportEvents.Add(flatSupportEvent);
                    }


                    flatSurface.UpdateBoundries(this.Triangles);

                    this.Triangles.FlatSurfaces.Add(flatSurface);

                }
            }

            if (flatSupportEvents.Count > 0)
            {
                int supportConesCompleted = 0;
                while (supportConesCompleted != flatSupportEvents.Count)
                {
                    supportConesCompleted = 0;
                    Thread.Sleep(50);

                    foreach (var supportPointEvent in flatSupportEvents)
                    {
                        if (supportPointEvent.WaitOne(0))
                        {
                            supportConesCompleted++;
                        }
                    }

                    ProgressBarManager.UpdateMainPercentage(80 + (((float)supportConesCompleted / (float)supportPointEvents.Count)) * 15f);
                }
            }

            ProgressBarManager.UpdateMainPercentage(95);

            this.Loaded = true;

            ProgressBarManager.UpdateMainPercentage(100);
        }

        internal void Open(string fileName, bool useDL, Color color, int index, TriangleInfoList triangles = null, bool changeCameraTargetPositionToCenterOfModel = true, bool enableProgressStatus = true, string objectName = null, bool disableCentering = false)
        {
            var t = new Stopwatch();
            t.Start();
            if (enableProgressStatus) OpenFileProcessing?.Invoke(null, new OpenFileEventArgs() { Percentage = 1, Message = "Loading stl file..." });

            this._internalSupport = true;
            PrimitiveMode = PrimitiveType.Triangles;

            this.SupportStructure = new List<SupportCone>();
            this._color = Color.FromArgb(index, color);
            this._scaleFactorX = this._scaleFactorY = this._scaleFactorZ = 1;

            IndexArray = null;

            if (fileName != null || triangles != null)
            {
                if (fileName != null)
                {
                    this.FileName = (new FileInfo(fileName)).Name;
                    //first load binary if failes load ascii
                    if (!LoadSTL_ASCII(fileName))
                    {
                        if (!LoadSTL_Binary(fileName, false))
                        {
                            LoadSTL_Binary(fileName, true);
                        }
                    };
                }
                else if (triangles != null)
                {
                    LoadSTL_Triangles(triangles);
                }

                if (enableProgressStatus) OpenFileProcessing?.Invoke(null, new OpenFileEventArgs() { Percentage = 25, Message = "Determine model structure..." });

                this._supportDistance = 3f;

                if (enableProgressStatus) OpenFileProcessing?.Invoke(null, new OpenFileEventArgs() { Percentage = 35, Message = "Determine default center..." });

                if (!disableCentering) this.UpdateDefaultCenter();

                if (enableProgressStatus) OpenFileProcessing?.Invoke(null, new OpenFileEventArgs() { Percentage = 60, Message = "Updating model structure..." });

                ProcessModelGeometry(this);

                if (enableProgressStatus) OpenFileProcessing?.Invoke(null, new OpenFileEventArgs() { Percentage = 90, Message = "Updating model colors..." });

                this.Triangles.UpdateFaceColors(this.ColorAsByte4, false);

                this.Triangles.UpdateConnections();
                this.Triangles.CalcFlatSurfaces();
                this.Triangles.CalcHorizontalSurfaces(this.MoveTranslationZ);
                this.Triangles.HorizontalSurfaces.UpdateBoundries(this.Triangles);
                this.Triangles.FlatSurfaces.UpdateBoundries(this.Triangles);

                Parallel.For(0, this.Triangles.Count, arrayIndexAsync =>
                {
                    var triangleArrayIndex = arrayIndexAsync;
                    for (var triangleIndex = 0; triangleIndex < this.Triangles[triangleArrayIndex].Count; triangleIndex++)
                    {
                        this.Triangles.GetConnectedTriangles(this.Triangles[triangleArrayIndex][triangleIndex].Index);
                    }
                });

                this.AxisLocked = true;

                if (frmStudioMain.SceneControl != null && this.VBOIndexes != null)
                {
                    if (frmStudioMain.SceneControl.InvokeRequired)
                    {
                        frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate { this.UpdateBinding(); }));
                    }
                    else
                    {
                        this.UpdateBinding();
                    }
                }

            }

            if (objectName != null)
            {
                this.FileName = objectName;
            }

            //selectiobox text
            //this.UpdateSelectionboxText();

            if (!string.IsNullOrEmpty(fileName) || triangles != null)
            {
                if (enableProgressStatus) OpenFileProcesssed?.Invoke(null, new OpenFileEventArgs() { Percentage = 100, Message = string.Empty });
            }

            if (changeCameraTargetPositionToCenterOfModel)
            {
                SceneView.CameraPositionTargetCenter = this.Center;
            }

            this.Loaded = true;

            MemoryHelpers.ForceGCCleanup();

            Console.WriteLine("Total model processing: " + t.ElapsedMilliseconds + "ms");

        }


        internal void CreateTriangleSnapshotPoints()
        {
            this.Triangles.CreatePreservedVectors();

            this.UndoMoveTranslation = this.MoveTranslation;

            if (this.TotalObjectSupportCones != null)
            {
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    supportCone.CreateTriangleSnapshotPoints();
                }
            }

            if (this.SupportBasementStructure != null)
            {
                this.SupportBasementStructure.CreateTriangleSnapshotPoints();
            }
        }

        internal void RevertFromUndo(STLModel3D undoModel)
        {
            this.Triangles = undoModel.Triangles;
            this.SupportBasementStructure = undoModel.SupportBasementStructure;
            this.SupportStructure = undoModel.SupportStructure;
            this._scaleFactorX = undoModel.ScaleFactorX;
            this._scaleFactorY = undoModel.ScaleFactorY;
            this._scaleFactorZ = undoModel.ScaleFactorZ;

            this.UpdateBoundries();
            this.UpdateBinding();
        }

        internal void RevertTriangleSnapshotPoints()
        {
            if (this.SupportStructure != null)
            {
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    supportCone.RevertTriangleSnapshotPoints();
                }
            }

            if (this.SupportBasementStructure != null)
            {
                this.SupportBasementStructure.RevertTriangleSnapshotPoints();
            }


            //update triangle points
            this.Triangles.RevertPreservedVectors();
        }

        internal void RemoveMoveTranslationWithVectors(Vector3 modelTranslation)
        {
            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    this.Triangles[arrayIndex][triangleIndex].Vectors[0].Position -= new Vector3Class(modelTranslation.X, modelTranslation.Y, 0);
                    this.Triangles[arrayIndex][triangleIndex].Vectors[1].Position -= new Vector3Class(modelTranslation.X, modelTranslation.Y, 0);
                    this.Triangles[arrayIndex][triangleIndex].Vectors[2].Position -= new Vector3Class(modelTranslation.X, modelTranslation.Y, 0);

                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                }
            }
        }

        internal void RepairNormals()
        {
            //this.Triangles.DetectAndRepairFaceDirections(this.ColorAsByte4);
            if (frmStudioMain.SceneControl != null)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate { this.UpdateBinding(); }));
                }
                else
                {
                    if (frmStudioMain.SceneControl.InvokeRequired)
                    {
                        frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate { this.UpdateBinding(); }));
                    }
                    else
                    {
                        this.UpdateBinding();
                    }
                }
            }
        }

        internal void Remove()
        {
            ObjectView.Objects3D.Remove(this);
            ModelRemoved?.Invoke(null, null);
        }

        internal void ProcessModelGeometry(STLModel3D stlModel)
        {
            try
            {
                Debug.WriteLine("Model parts start");
                var stopwatch = new System.Diagnostics.Stopwatch();
                stopwatch.Start();

                stlModel.Triangles.CalcModelParts();
                Debug.WriteLine("Model parts stopped: " + stopwatch.ElapsedMilliseconds + "ms");
                stopwatch.Stop();
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        internal void UpdateSurfaceSupportASync(TriangleSurfaceInfo surface)
        {
            this.SupportSurfaceProcessing?.Invoke(surface, null);
        }

        internal int GetBottomSupportConesCount()
        {
            var result = 0;
            if (this.SupportStructure.Count > 0)
            {
                foreach (var supportCone in this.SupportStructure)
                {
                    if (supportCone.BottomPoint == 0)
                    {
                        result++;
                    }
                }
            }

            if (this.Triangles.HorizontalSurfaces.Count > 0)
            {
                foreach (var supportCone in this.Triangles.HorizontalSurfaces.SupportStructure)
                {
                    if (supportCone.BottomPoint == 0)
                    {
                        result++;
                    }
                }
            }
            return result;
        }

        internal void DeselectObjects()
        {
            if (this.SupportStructure != null)
            {
                lock (this.SupportStructure)
                {
                    foreach (var supportCone in this.TotalObjectSupportCones)
                    {
                        if (supportCone.Selected)
                        {
                            supportCone.Selected = false;
                        }
                    }
                }
            }

            lock (this.Triangles.HorizontalSurfaces.SupportStructure)
            {
                foreach (var surface in this.Triangles.HorizontalSurfaces)
                {
                    if (surface.Selected)
                    {
                        surface.Selected = false;
                    }
                }
            }

            lock (this.Triangles.FlatSurfaces.SupportStructure)
            {
                foreach (var surface in this.Triangles.FlatSurfaces)
                {
                    if (surface.Selected)
                    {
                        surface.Selected = false;
                    }
                }
            }

        }

        private void UpdateModelIntersectionTriangles()
        {
            if (SupportStructure != null)
            {
                foreach (var supportCone in this.TotalObjectSupportCones)
                {
                    if (supportCone != null)
                    {
                        supportCone.CalcModelIntersectionTriangles(supportCone.Position, this, null);
                    }
                }
            }

        }

        public bool LoadSTL_Binary(string filename, bool forceBinaryReading)
        {
            var ms = new MemoryStream();
            using (FileStream file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                byte[] bytes = new byte[file.Length];
                file.Read(bytes, 0, (int)file.Length);
                ms.Write(bytes, 0, (int)file.Length);
            }

            ms.Position = 0;
            return this.LoadSTL_Binary(ms, forceBinaryReading);
        }

        public void LoadSTL_Triangles(TriangleInfoList triangles)
        {
            try
            {
                this.Triangles = new TriangleInfoList();
                this.Triangles.InitialTranslation = triangles.InitialTranslation;
                bool initialBaseline = true;
                for (var triangleArrayIndex = 0; triangleArrayIndex < triangles.Count; triangleArrayIndex++)
                {
                    if (this.Triangles.Count < triangleArrayIndex + 1)
                    {
                        this.Triangles.Add(new List<Triangle>());
                    }

                    for (var triangleIndex = 0; triangleIndex < triangles[triangleArrayIndex].Count; triangleIndex++)
                    {
                        var triangle = triangles[triangleArrayIndex][triangleIndex];
                        this.Triangles[triangleArrayIndex].Add(triangle);

                        foreach (var vector in triangle.Vectors)
                        {
                            if (initialBaseline)
                            {
                                this.BottomPoint = vector.Position.Z;
                                this.TopPoint = vector.Position.Z;
                                this.LeftPoint = vector.Position.X;
                                this.RightPoint = vector.Position.X;
                                this.FrontPoint = vector.Position.Y;
                                this.BackPoint = vector.Position.Y;
                                initialBaseline = false;
                            }

                            //lowestpoint
                            if (vector.Position.Z < this.BottomPoint)
                            {
                                this.BottomPoint = vector.Position.Z;
                            }

                            //left point
                            if (vector.Position.X < this.LeftPoint)
                            {
                                this.LeftPoint = vector.Position.X;
                            }

                            //right point
                            if (vector.Position.X > this.RightPoint)
                            {
                                this.RightPoint = vector.Position.X;
                            }

                            //highest point
                            if (vector.Position.Z > this.TopPoint)
                            {
                                this.TopPoint = vector.Position.Z;
                            }

                            //frontest point
                            if (vector.Position.Y > this.FrontPoint)
                            {
                                this.FrontPoint = vector.Position.Y;
                            }

                            //Depthest point
                            if (this.BackPoint > vector.Position.Y)
                                this.BackPoint = vector.Position.Y;
                        }

                        triangle.CalcCenter();
                        triangle.CalcNormal();
                        triangle.CalcAngleZ();
                        triangle.CalcVolume();
                    }
                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc);
            }
        }
        public bool LoadSTL_Binary(MemoryStream inputStream, bool forceBinaryReading)
        {

            ushort triangleIndex = 0;
            ushort triangleArrayIndex = 0;

            try
            {
                using (BinaryReader br = new BinaryReader(inputStream))
                {
                    byte[] data = new byte[80];
                    data = br.ReadBytes(80); // read the header
                    var headerText = ASCIIEncoding.Default.GetString(data);

                    if (!forceBinaryReading)
                    {
                        if (headerText.ToLower().StartsWith("solid\r\n"))
                        {
                            return false;
                        }
                    }

                    long numtri = br.ReadUInt32();

                    this.Triangles = new TriangleInfoList();

                    bool initialBaseline = true;

                    float x = 0;
                    float y = 0;
                    float z = 0;

                    var normal = new Vector3Class();
                    while (br.BaseStream.Position < br.BaseStream.Length - 12 - 24)
                    {

                        //skip normal
                        var normalx = br.ReadSingle();
                        var normaly = br.ReadSingle();
                        var normalz = br.ReadSingle();
                        normal = new Vector3Class() { X = normalx, Y = normaly, Z = normalz };
                        //Debug.WriteLine(normal.X + ";" + normal.Y + ";" + normal.Z);

                        if (triangleIndex > 33333)
                        {
                            triangleIndex = 0;
                            triangleArrayIndex++;
                            this.Triangles.Add(new List<Triangle>());

                        }
                        var triangle = new Triangle();
                        triangle.Index = new TriangleConnectionInfo() { ArrayIndex = triangleArrayIndex, TriangleIndex = triangleIndex };

                        for (int vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        { //iterate through the points
                            x = br.ReadSingle();
                            y = br.ReadSingle();
                            z = br.ReadSingle();

                            if (initialBaseline)
                            {
                                this.BottomPoint = z;
                                this.TopPoint = z;
                                this.LeftPoint = x;
                                this.RightPoint = x;
                                this.FrontPoint = y;
                                this.BackPoint = y;
                                initialBaseline = false;
                            }

                            //lowestpoint
                            if (z < this.BottomPoint)
                            {
                                this.BottomPoint = z;
                            }

                            //left point
                            if (x < this.LeftPoint)
                            {
                                this.LeftPoint = x;
                            }

                            //right point
                            if (x > this.RightPoint)
                            {
                                this.RightPoint = x;
                            }

                            //highest point
                            if (z > this.TopPoint)
                            {
                                this.TopPoint = z;
                            }

                            //frontest point
                            if (y > this.FrontPoint)
                            {
                                this.FrontPoint = y;
                            }

                            //Depthest point
                            if (this.BackPoint > y)
                            {
                                this.BackPoint = y;
                            }

                            triangle.Vectors[vectorIndex] = new VertexClass() { Position = new Vector3Class(x, y, z) };
                            triangle.Normal = normal;
                        }

                        triangle.CalcCenter();
                        triangle.CalcNormal();
                        triangle.CalcAngleZ();
                        triangle.CalcVolume();

                        this.Triangles[triangleArrayIndex].Add(triangle);

                        uint attr = br.ReadUInt16(); // not used attribute
                        triangleIndex++;
                    }
                }

                return true;
            }
            catch (EndOfStreamException exc)
            {
                Debug.WriteLine(exc.Message);
                return true;
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);

                return false;
            }
        }
        #region ASCII
        /// <summary>
        /// This function loads an ascii STL file
        /// </summary>
        /// <param name="filename"></param>
        /// <returns></returns>
        public bool LoadSTL_ASCII(string filename)
        {
            ushort triangleIndex = 0;
            ushort triangleArrayIndex = 0;

            bool initialBaseline = true;

            float x = 0;
            float y = 0;
            float z = 0;


            try
            {
                using (var sr = new StreamReader(filename))
                {
                    this.Triangles = new TriangleInfoList();

                    bool initialCheck = true;
                    //first line should be "solid <name> " 
                    string line = sr.ReadLine();
                    string[] toks = line.Split(' ');
                    if (!toks[0].ToLower().StartsWith("solid"))
                        return false; // does not start with "solid"
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine().Trim();//facet

                        if (initialCheck && !line.ToLower().Contains("facet"))
                        {
                            return false;
                        }
                        else
                        {
                            initialCheck = false;
                        }
                        if (line.ToLower().StartsWith("facet"))
                        {
                            line = sr.ReadLine().Trim();//outerloop
                            if (!line.ToLower().StartsWith("outer loop"))
                            {
                                return false;
                            }

                            if (triangleIndex > 33333)
                            {
                                triangleIndex = 0;
                                triangleArrayIndex++;
                                this.Triangles.Add(new List<Triangle>());
                            }
                            var triangle = new Triangle();
                            triangle.Index = new TriangleConnectionInfo() { ArrayIndex = triangleArrayIndex, TriangleIndex = triangleIndex };


                            for (int vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                            { //iterate through the points
                                char[] delimiters = new char[] { ' ' };
                                line = sr.ReadLine().Trim(); // vertex
                                toks = line.Split(delimiters, StringSplitOptions.RemoveEmptyEntries);
                                if (!toks[0].ToLower().Equals("vertex"))
                                {
                                    return false;
                                }

                                x = float.Parse(toks[1].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                y = float.Parse(toks[2].Trim(), System.Globalization.CultureInfo.InvariantCulture);
                                z = (float)Double.Parse(toks[3].Trim(), System.Globalization.CultureInfo.InvariantCulture);

                                if (initialBaseline)
                                {
                                    this.BottomPoint = z;
                                    this.TopPoint = z;
                                    this.LeftPoint = x;
                                    this.RightPoint = x;
                                    this.FrontPoint = y;
                                    this.BackPoint = y;
                                    initialBaseline = false;
                                }

                                //lowestpoint
                                if (z < this.BottomPoint)
                                {
                                    this.BottomPoint = z;
                                }

                                //left point
                                if (x < this.LeftPoint)
                                {
                                    this.LeftPoint = x;
                                }

                                //right point
                                if (x > this.RightPoint)
                                {
                                    this.RightPoint = x;
                                }

                                //highest point
                                if (z > this.TopPoint)
                                {
                                    this.TopPoint = z;
                                }

                                //frontest point
                                if (y > this.FrontPoint)
                                {
                                    this.FrontPoint = y;
                                }

                                //Depthest point
                                if (this.BackPoint > y)
                                {
                                    this.BackPoint = y;
                                }

                                triangle.Vectors[vectorIndex] = new VertexClass() { Position = new Vector3Class(x, y, z) };

                            }

                            triangle.CalcNormal();
                            triangle.CalcCenter();
                            triangle.CalcAngleZ();
                            triangle.CalcVolume();

                            this.Triangles[triangleArrayIndex].Add(triangle);
                            triangleIndex++;

                            line = sr.ReadLine().Trim();//endloop
                            if (!line.Equals("endloop"))
                            {
                                return false;
                            }
                            line = sr.ReadLine().Trim().ToLower(); // endfacet
                            if (!line.Equals("endfacet"))
                            {
                                return false;
                            }

                        } // endfacet
                        else if (line.ToLower().StartsWith("endsolid"))
                        {

                            //      Update(); // initial positions please...
                        }
                        else
                        {
                            //        DebugLogger.Instance().LogError("Error in LoadSTL ASCII, facet expected");
                        }
                    } // end of input stream

                    sr.Close();

                }

                if (this.Triangles.Count > 0 && this.Triangles[0].Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
        }

        internal void Scale(float scaleFactorX, float scaleFactorY, float scaleFactorZ, ScaleEventArgs.TypeAxis axis, bool allowUndo, bool updateBoundries = true, bool updateModel = true, bool isUndoAction = true, ActionModel actionModel = null)
        {
            if (allowUndo)
            {

                if (isUndoAction)
                {
                    ActionModel actionModelUndo = new ActionModel();
                    actionModelUndo.IsUndo = true;
                    actionModelUndo.Arguments = new List<object>();
                    actionModelUndo.ModelIndex = this.Index;

                    UndoRedoManager.GetInstance.PushReverseAction(x => Scale(scaleFactorX, scaleFactorY, scaleFactorZ, axis, true, false, true, false, x), actionModelUndo);


                    //save current state
                    var tmpPath = Path.Combine(Settings.RoamingAutoSavePath, Guid.NewGuid().ToString() + ".tmp");
                    actionModelUndo.Arguments.Add(tmpPath);
                    ExportEngine.ExportSelectedModelProperties(tmpPath, actionModelUndo.ModelIndex);
                }
                else if (!isUndoAction)
                {
                    //deserial undo model
                    var undoModel = ImportEngine.ImportModelUndoFileAsync((string)actionModel.Arguments[0], actionModel.ModelIndex);
                    ObjectView.SelectObjectByIndex(actionModel.ModelIndex);
                    var selectedModel = ObjectView.SelectedModel;
                    selectedModel.RevertFromUndo(undoModel);

                    SceneControlToolbarManager.UpdateModelDimensions(selectedModel.Width, selectedModel.Depth, selectedModel.Height);

                    if (SceneActionControlManager.IsActionPanelScaleVisible)
                    {
                        SceneActionControlManager.ActionPanelScale.DataSource = selectedModel;
                    }
                }
            }

            if (this.Triangles != null)
            {
                var previousScaleFactorX = this._scaleFactorX;
                var previousScaleFactorY = this._scaleFactorY;
                var previousScaleFactorZ = this._scaleFactorZ;

                switch (axis)
                {
                    case ScaleEventArgs.TypeAxis.X:
                        {
                            this._scaleFactorX = scaleFactorX;
                            break;
                        }
                    case ScaleEventArgs.TypeAxis.Y:
                        {
                            this._scaleFactorY = scaleFactorY;
                            break;
                        }
                    case ScaleEventArgs.TypeAxis.Z:
                        {
                            this._scaleFactorZ = scaleFactorZ;
                            break;
                        }
                    case ScaleEventArgs.TypeAxis.ALL:
                        {
                            this._scaleFactorX = scaleFactorX;
                            this._scaleFactorY = scaleFactorY;
                            this._scaleFactorZ = scaleFactorZ;
                            break;
                        }
                }

                //update vectors
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var trianglesCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            var vector = this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;
                            switch (axis)
                            {
                                case ScaleEventArgs.TypeAxis.X:
                                    this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X = (this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X / previousScaleFactorX) * scaleFactorX;
                                    break;
                                case ScaleEventArgs.TypeAxis.Y:
                                    this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y = (this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y / previousScaleFactorY) * scaleFactorY;
                                    break;
                                case ScaleEventArgs.TypeAxis.Z:
                                    this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z = (this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z / previousScaleFactorZ) * scaleFactorZ;
                                    break;
                                case ScaleEventArgs.TypeAxis.ALL:
                                    this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X = (this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X / previousScaleFactorX) * scaleFactorX;
                                    this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y = (this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y / previousScaleFactorY) * scaleFactorY;
                                    this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z = (this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z / previousScaleFactorZ) * scaleFactorZ;
                                    break;
                            }
                        }
                    }
                }

                if (updateBoundries)
                {
                    this.UpdateBoundries();
                    if (this.Triangles.HorizontalSurfaces != null) { this.Triangles.HorizontalSurfaces.UpdateBoundries(this.Triangles); }
                    if (this.Triangles.FlatSurfaces != null) { this.Triangles.FlatSurfaces.UpdateBoundries(this.Triangles); }
                }
            }
        }

        internal void Rotate(float angleX, float angleY, float angleZ, RotationEventArgs.TypeAxis rotationAxis, bool xyzAxis = false, bool updateFaceColor = true)
        {
            Matrix4 rotationMatrix = new Matrix4();

            switch (rotationAxis)
            {
                case RotationEventArgs.TypeAxis.X:
                    rotationMatrix = Matrix4.CreateRotationX(OpenTK.MathHelper.DegreesToRadians(angleX - this.RotationAngleX));
                    this._rotationAngleX = angleX;
                    break;
                case RotationEventArgs.TypeAxis.Y:
                    rotationMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(angleY - this.RotationAngleY));
                    this._rotationAngleY = angleY;
                    break;
                case RotationEventArgs.TypeAxis.Z:
                    rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(angleZ - this.RotationAngleZ));
                    this._rotationAngleZ = angleZ;
                    break;
            }

            Parallel.For(0, this.Triangles.Count, arrayIndexAsync =>
            {
                var arrayIndex = arrayIndexAsync;
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = rotatedVector;
                    }

                    this.Triangles[arrayIndex][triangleIndex].CalcCenter();
                    this.Triangles[arrayIndex][triangleIndex].CalcNormal();
                    this.Triangles[arrayIndex][triangleIndex].CalcAngleZ();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxX();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxY();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                }
            });

            if (rotationAxis == RotationEventArgs.TypeAxis.Z)
            {
                if (!xyzAxis)
                {
                    if (this.SupportStructure != null)
                    {
                        //rotate support cones
                        foreach (var supportCone in this.SupportStructure)
                        {
                            supportCone.RotateAxisZ(rotationMatrix);
                        }

                        foreach (var horizontalSurface in this.Triangles.HorizontalSurfaces)
                        {
                            if (horizontalSurface.SupportStructure != null)
                            {
                                foreach (var supportCone in horizontalSurface.SupportStructure)
                                {
                                    supportCone.RotateAxisZ(rotationMatrix);
                                }
                            }
                        }

                        foreach (var flatSurface in this.Triangles.FlatSurfaces)
                        {
                            if (flatSurface.SupportStructure != null)
                            {
                                foreach (var supportCone in flatSurface.SupportStructure)
                                {
                                    supportCone.RotateAxisZ(rotationMatrix);
                                }
                            }
                        }
                    }
                }
            }

            if (updateFaceColor)
            {
                var modelColor = PrintJobManager.CurrentPrintJobSettings.Material.ModelColor;
                if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                {
                    this.Triangles.UpdateFaceColors(new Byte4Class(128, modelColor.R, modelColor.G, modelColor.B), false);
                }
                else
                {
                    this.Triangles.UpdateFaceColors(new Byte4Class((byte)this.Index, modelColor.R, modelColor.G, modelColor.B), false);
                }

                UpdateModelIntersectionTriangles();
            }
            this.UpdateBoundries();

            this.UpdateSupportBasement();
        }

        //internal void CalcSliceIndexesAsync(DAL.Materials.Material material, List<ManualResetEvent> sliceIndexesThreadEvents, bool includeValues)
        //{
        //    var sliceIndexesThreadEvent = new ManualResetEvent(false);
        //    sliceIndexesThreadEvents.Add(sliceIndexesThreadEvent);

        //    //multithreading
        //    ThreadPool.QueueUserWorkItem(new WaitCallback((_) =>
        //    {
        //        CalcSliceIndexes(material, includeValues);
        //        sliceIndexesThreadEvent.Set();
        //    }));
        //}

        internal STLModel3D MAGSAISelectionOverlay;

        internal SortedDictionary<float, List<TriangleConnectionInfo>> ModelBleedingSliceIndexes { get; set; }

        internal void CalcSliceIndexes(Material material, bool includeValues, IList<float> modelKeys = null)
        {

            //  var stopwatch = new Stopwatch();
            //  stopwatch.Start();

            var initialLayersHeight = Math.Round(material.InitialLayers * material.LT1, 3);
            var initialLayersHeightIndex = (float)Math.Round(initialLayersHeight - material.LT1, 3);


            this.SliceIndexes = new SortedDictionary<float, List<TriangleConnectionInfo>>();
            this.ModelBleedingSliceIndexes = new SortedDictionary<float, List<TriangleConnectionInfo>>();
            if (modelKeys == null)
            {
                var totalDefaultSlices = ((this.TopPoint - (material.LT1 * material.InitialLayers)) / material.LT2) + (Managers.UserProfileManager.UserProfile.SupportEngine_Basement_Thickness / material.LT2 * material.ShrinkFactor);

                this.SliceIndexes.Add(0.005f, new List<TriangleConnectionInfo>()); //begin little higher the 0 because of small offsets when rotating

                var currentZHeight = 0f;
                if (material.InitialLayers > 0)
                {
                    for (var materialInitialIndex = 1; materialInitialIndex < material.InitialLayers; materialInitialIndex++)
                    {
                        currentZHeight += (float)decimal.Parse(material.LT1.ToString());
                        var currentZHeightKey = (float)Math.Round(currentZHeight, 3);

                        if (!this.SliceIndexes.ContainsKey(currentZHeight))
                        {
                            this.SliceIndexes.Add(currentZHeightKey, new List<TriangleConnectionInfo>());
                        }


                        if (material.BleedingOffset != 0)
                        {
                            var bleedingHeightKey = (float)Math.Round(currentZHeightKey + material.BleedingOffset, 3);
                            if (bleedingHeightKey >= 0)
                            {
                                if (!this.ModelBleedingSliceIndexes.ContainsKey(bleedingHeightKey))
                                {
                                    this.ModelBleedingSliceIndexes.Add(bleedingHeightKey, new List<TriangleConnectionInfo>());
                                }
                            }
                        }
                    }
                }

                for (var materialDefaultIndex = material.InitialLayers; materialDefaultIndex < totalDefaultSlices; materialDefaultIndex++)
                {
                    currentZHeight += (float)material.LT2;
                    var currentZHeightKey = (float)Math.Round(currentZHeight, 3);
                    if (!this.SliceIndexes.ContainsKey(currentZHeightKey))
                    {
                        this.SliceIndexes.Add(currentZHeightKey, new List<TriangleConnectionInfo>());
                    }

                    if (material.BleedingOffset != 0)
                    {
                        var bleedingHeightKey = (float)Math.Round(currentZHeightKey + material.BleedingOffset, 3);
                        if (bleedingHeightKey >= 0)
                        {
                            if (!this.ModelBleedingSliceIndexes.ContainsKey(bleedingHeightKey))
                            {
                                this.ModelBleedingSliceIndexes.Add(bleedingHeightKey, new List<TriangleConnectionInfo>());
                            }
                        }
                    }
                }
            }
            else
            {
                //support cone use calculated models keys
                foreach (var modelKey in modelKeys)
                {
                    this.SliceIndexes.Add(modelKey, new List<TriangleConnectionInfo>());
                }
            }

            //create lookup index to increase performance

            if (includeValues)
            {
                CalcSliceIndexValues(this.SliceIndexes, material, initialLayersHeight, false);

                if (material.BleedingOffset > 0)
                {
                    CalcSliceIndexValues(this.ModelBleedingSliceIndexes, material, initialLayersHeight, true);
                }
            }

            if (this is STLModel3D && (!(this is SupportCone)))
            {
                var sliceKeys = this.SliceIndexes.Keys.ToList();
                if (this.SupportBasementStructure != null)
                {
                    this.SupportBasementStructure.CalcSliceIndexes(material, includeValues, sliceKeys);
                }
            }


        }

        internal void CalcSliceIndexValues(SortedDictionary<float, List<TriangleConnectionInfo>> indexKeys, Material material, double initialLayersHeight, bool useBleedingOffset)
        {
            var sliceIndexKeyLookUp = new Dictionary<int, float>();
            var sliceIndexKeyLookUpIndex = 0;


            foreach (var sliceIndexKey in indexKeys.Keys)
            {
                sliceIndexKeyLookUp.Add(sliceIndexKeyLookUpIndex, sliceIndexKey);
                sliceIndexKeyLookUpIndex++;
            }

            Triangle triangle = null;
            for (var triangleArrayIndex = 0; triangleArrayIndex < this.Triangles.Count; triangleArrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this.Triangles[triangleArrayIndex].Count; triangleIndex++)
                {
                    triangle = this.Triangles[triangleArrayIndex][triangleIndex];
                    if (triangle.Bottom == 0 && triangle.Top == 0)
                    {
                        triangle.CalcMinMaxZ();
                    }

                    var triangleBottom = (double)Math.Round(triangle.Bottom, 3);
                    var triangleTop = (double)Math.Round(triangle.Top, 3);


                    //when initial used subtract the initiallayerheight from top and bottom and divide that value to find the lower and upper index
                    //triangleBottom -= material.BleedingOffset;
                    var triangleBottomIndex = (int)Math.Floor(triangleBottom / material.LT2) - 2 - (material.InitialLayers);
                    if (useBleedingOffset)
                    {
                        triangleBottomIndex -= (int)(material.BleedingOffset / material.LT2) + 1;
                    }

                    if (triangleBottomIndex < 0)
                    {
                        triangleBottomIndex = 0;
                    }

                    //add triangle between bottom and top index
                    for (var triangleLookupIndex = triangleBottomIndex; triangleLookupIndex < int.MaxValue; triangleLookupIndex++)
                    {
                        if (sliceIndexKeyLookUp.ContainsKey(triangleLookupIndex))
                        {
                            var sliceHeight = sliceIndexKeyLookUp[triangleLookupIndex];
                            if (sliceHeight < triangle.Bottom)
                            {
                                continue;
                            }
                            //stop when last sliceheigt is larger the index
                            if (Math.Round(sliceHeight, 3) > triangleTop)
                            {
                                break;
                            }
                            indexKeys[sliceHeight].Add(new TriangleConnectionInfo() { ArrayIndex = (ushort)triangleArrayIndex, TriangleIndex = (ushort)triangleIndex });
                        }
                        else
                        {
                            break;
                        }
                    }

                    //}
                };

            }

            //cleanup last sliceindexes when they are empty
            var totalSliceIndexes = indexKeys.Count();
            for (var indexKey = totalSliceIndexes - 1; indexKey > 0; indexKey--)
            {
                var index = indexKeys.ElementAt(indexKey);
                if (index.Value.Count() == 0)
                {
                    indexKeys.Remove(index.Key);
                }
                else
                {
                    break;
                }
            }

        }

        internal void ClearSliceIndexes()
        {
            if (this.SliceIndexes != null)
            {
                this.SliceIndexes.Clear();
            }

            if (this.ModelBleedingSliceIndexes != null)
            {
                this.ModelBleedingSliceIndexes.Clear();
            }

            foreach (var supportCone in this.TotalObjectSupportCones)
            {
                if (supportCone != null)
                {
                    supportCone.ClearSliceIndexes();
                }
            }

        }


        internal void LiftModelOnSupport()
        {
            var bottomDelta = this.MoveTranslationZ - this.BottomPoint;
            var moveTranslation = new Vector3Class(0, 0, bottomDelta);
            //this.Triangles.UpdateWithMoveTranslation(new Vector3Class(0, 0, bottomDelta));

            Parallel.For(0, this.Triangles.Count, arrayIndexAsync =>
            {
                //for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                //{
                var arrayIndex = arrayIndexAsync;
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    this.Triangles[arrayIndex][triangleIndex].Vectors[0].Position += moveTranslation;
                    this.Triangles[arrayIndex][triangleIndex].Vectors[1].Position += moveTranslation;
                    this.Triangles[arrayIndex][triangleIndex].Vectors[2].Position += moveTranslation;
                    this.Triangles[arrayIndex][triangleIndex].CalcCenter();
                    this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                }
            });
            //}

            //move object on top of ground
            this.UpdateBoundries();

            //update binding
            if (this._bindingSupported)
            {
                if (frmStudioMain.SceneControl != null)
                {
                    if (frmStudioMain.SceneControl.InvokeRequired)
                    {
                        frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate { this.UpdateBinding(); }));
                    }
                    else
                    {
                        this.UpdateBinding();
                    }
                }
            }
        }

        #region Binding
        internal int[] VBOIndexes { get; private set; }
        internal void BindModel()
        {
            if (this.Triangles == null)
            {
                this.Triangles = new TriangleInfoList();
            }
            this.VBOIndexes = new int[this.Triangles.TriangleArrayCount];
            if (frmStudioMain.SceneControl != null)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate
                    {
                        lock (this.VBOIndexes)
                        {
                            lock (frmStudioMain.SceneControl)
                            {
                                frmStudioMain.SceneControl.MakeCurrent();

                                GL.GenBuffers(this.Triangles.TriangleArrayCount, VBOIndexes);

                                for (var arrayIndex = 0; arrayIndex < this.Triangles.TriangleArrayCount; arrayIndex++)
                                {
                                    var vertexArray = new Vertex[0];
                                    this.Triangles.GetVertexArray(arrayIndex, this._hidden, ref vertexArray);
                                    GL.BindBuffer(BufferTarget.ArrayBuffer, VBOIndexes[arrayIndex]);
                                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vertex.Stride * vertexArray.Length), vertexArray, BufferUsageHint.DynamicDraw);
                                    vertexArray = null;
                                }
                                frmStudioMain.SceneControl.Context.MakeCurrent(null);
                            }
                        }
                    }));
                }
                else
                {
                    lock (this.VBOIndexes)
                    {
                        lock (frmStudioMain.SceneControl)
                        {
                            frmStudioMain.SceneControl.MakeCurrent();

                            GL.GenBuffers(this.Triangles.TriangleArrayCount, VBOIndexes);

                            for (var arrayIndex = 0; arrayIndex < this.Triangles.TriangleArrayCount; arrayIndex++)
                            {
                                var vertexArray = new Vertex[0];
                                this.Triangles.GetVertexArray(arrayIndex, this._hidden, ref vertexArray);
                                GL.BindBuffer(BufferTarget.ArrayBuffer, VBOIndexes[arrayIndex]);
                                GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(VertexClass.Stride * vertexArray.Length), vertexArray, BufferUsageHint.DynamicDraw);
                                vertexArray = null;
                            }
                            frmStudioMain.SceneControl.Context.MakeCurrent(null);
                        }
                    }

                    //   this.Triangles.VertexArray = null;
                }
            }
            //  GC.Collect();
            //  GC.Collect();
        }

        internal void UpdateBinding()
        {
            try
            {
                if (this.VBOIndexes != null)
                {
                    if (frmStudioMain.SceneControl != null)
                    {
                        if (frmStudioMain.SceneControl.InvokeRequired)
                        {
                            frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate
                            {
                                lock (frmStudioMain.SceneControl)
                                {
                                    frmStudioMain.SceneControl.MakeCurrent();
                                    for (var arrayIndex = 0; arrayIndex < this.Triangles.TriangleArrayCount; arrayIndex++)
                                    {
                                        var vertexArray = new Vertex[0];
                                        this.Triangles.GetVertexArray(arrayIndex, this._hidden, ref vertexArray);

                                        GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBOIndexes[arrayIndex]);
                                        GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(Vertex.Stride * vertexArray.Length), vertexArray, BufferUsageHint.DynamicDraw);
                                        vertexArray = null;
                                    }
                                    frmStudioMain.SceneControl.Context.MakeCurrent(null);

                                }
                            }));
                        }
                        else
                        {
                            lock (frmStudioMain.SceneControl)
                            {
                                frmStudioMain.SceneControl.MakeCurrent();
                                for (var arrayIndex = 0; arrayIndex < this.Triangles.TriangleArrayCount; arrayIndex++)
                                {
                                    var vertexArray = new Vertex[0];
                                    this.Triangles.GetVertexArray(arrayIndex, this._hidden, ref vertexArray);

                                    GL.BindBuffer(BufferTarget.ArrayBuffer, this.VBOIndexes[arrayIndex]);
                                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(VertexClass.Stride * vertexArray.Length), vertexArray, BufferUsageHint.DynamicDraw);

                                    vertexArray = null;
                                }
                                frmStudioMain.SceneControl.Context.MakeCurrent(null);
                            }
                        }
                    }

                }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            //     this.Triangles.VertexArray = null;
            //GC.Collect();
            //GC.Collect();

        }

        internal void UnbindModel()
        {
            if (this._bindingSupported)
            {

                if (this.VBOIndexes != null && this.VBOIndexes.Length > 0)
                {
                    lock (frmStudioMain.SceneControl)
                    {
                        frmStudioMain.SceneControl.MakeCurrent();

                        foreach (var vboIndex in this.VBOIndexes)
                        {
                            GL.DeleteBuffer(vboIndex);
                        }
                    }
                }

            }
        }
        #endregion



        public void UpdateBoundries()
        {
            var initialBaseline = true;
            if (this.Triangles != null)
            {
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var trianglesCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            if (this.Triangles[arrayIndex][triangleIndex] != null)
                            {
                                var z = this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z;
                                var x = this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X;
                                var y = this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y;
                                if (initialBaseline)
                                {
                                    this.BottomPoint = z;
                                    this.TopPoint = z;
                                    this.LeftPoint = x;
                                    this.RightPoint = x;
                                    this.FrontPoint = y;
                                    this.BackPoint = y;
                                    initialBaseline = false;
                                }

                                else
                                {
                                    //lowestpoint
                                    if (z < this.BottomPoint)
                                    {
                                        this.BottomPoint = z;
                                    }

                                    //left point
                                    if (x < this.LeftPoint)
                                    {
                                        this.LeftPoint = x;
                                    }

                                    //right point
                                    if (x > this.RightPoint)
                                    {
                                        this.RightPoint = x;
                                    }

                                    //highest point
                                    if (z > this.TopPoint)
                                    {
                                        this.TopPoint = z;
                                    }

                                    //frontest point
                                    if (y > this.FrontPoint)
                                    {
                                        this.FrontPoint = y;
                                    }

                                    //Depthest point
                                    if (this.BackPoint > y)
                                    {
                                        this.BackPoint = y;
                                    }
                                }
                            }
                        }
                    }
                }
            }

            if (this.SupportStructure != null)
            {
                lock (this.SupportStructureLock)
                {
                    foreach (var supportCone in this.SupportStructure)
                    {
                        supportCone.UpdateBoundries();
                    }
                }
            }

            if (this.Triangles != null)
            {
                if (this.Triangles.HorizontalSurfaces == null) this.Triangles.HorizontalSurfaces = new TriangleSurfaceInfoList();
                this.Triangles.HorizontalSurfaces.UpdateBoundries(this.Triangles);

                if (this.Triangles.FlatSurfaces == null) this.Triangles.FlatSurfaces = new TriangleSurfaceInfoList();
                this.Triangles.FlatSurfaces.UpdateBoundries(this.Triangles);
            }
        }

        internal void UpdateDefaultCenter(bool isXYZAxis = false, bool useEdgeAsOrigin = false)
        {
            var translation = new Vector3Class(-this.Center.X, -this.Center.Y, -this.BottomPoint);

            this.MoveTranslation = new Vector3Class(translation.Xy);

            if (useEdgeAsOrigin)
            {
                this.MoveTranslation = new Vector3Class();
                var bottomPointTranslation = new Vector3Class(0, 0, this.BottomPoint + this.Triangles.InitialTranslation.Z);

                //center model
                this.MoveModelWithTranslationZ(translation);

                this.UpdateBoundries();

                //move model using 3mf translation
                this.MoveTranslation = bottomPointTranslation;
                this.MoveModelWithTranslationZ(bottomPointTranslation);
                this.UpdateBoundries();

                var currentPrinter = PrintJobManager.SelectedPrinter;
                var buildPlatformEdgeX = (currentPrinter.ProjectorResolutionX / 10) * currentPrinter.TrapeziumCorrectionFactorX;
                buildPlatformEdgeX /= 2;
                var buildPlatformEdgeY = (currentPrinter.ProjectorResolutionY / 10) * currentPrinter.TrapeziumCorrectionFactorY;
                buildPlatformEdgeY /= 2;

                //change center point to left front
                //this.MoveTranslation -= new Vector3Class(this.LeftPoint, -this.FrontPoint, 0);
                this.MoveTranslation -= new Vector3Class(translation.X, translation.Y, 0);

                this.MoveTranslation += new Vector3Class(this.Triangles.InitialTranslation.Xy);
                this.MoveTranslation -= new Vector3Class(buildPlatformEdgeX, buildPlatformEdgeY, 0);


                this.UpdateBoundries();
            }
            else
            {
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var trianglesCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += translation;
                        }

                        this.Triangles[arrayIndex][triangleIndex].CalcCenter();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxX();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxY();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                    }
                }

                this.UpdateBoundries();
            }






        }

        internal void UpdateTrianglesMinMaxZ()
        {
            // var translation = new Vector3(-this.Center.X, -this.Center.Y, -this.BottomPoint);
            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                    }
                }
            }

            //support
            foreach (var support in this.TotalObjectSupportCones)
            {
                for (var arrayIndex = 0; arrayIndex < support.Triangles.Count; arrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < support.Triangles[arrayIndex].Count; triangleIndex++)
                    {
                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            support.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();

                        }
                    }
                }
            }
        }

        #endregion

        public object Clone(bool updateConnections)
        {
            return new STLModel3D(this, this._color.A != 255 && this._color.A != 0, updateConnections, false);
        }

        public void SetBrowsableProperty(string strPropertyName, bool bIsBrowsable)
        {
            // Get the Descriptor's Properties
            PropertyDescriptor theDescriptor = TypeDescriptor.GetProperties(this.GetType())[strPropertyName];

            // Get the Descriptor's "Browsable" Attribute
            BrowsableAttribute theDescriptorBrowsableAttribute = (BrowsableAttribute)theDescriptor.Attributes[typeof(BrowsableAttribute)];
            FieldInfo isBrowsable = theDescriptorBrowsableAttribute.GetType().GetField("Browsable", BindingFlags.IgnoreCase | BindingFlags.NonPublic | BindingFlags.Instance);

            // Set the Descriptor's "Browsable" Attribute
            isBrowsable.SetValue(theDescriptorBrowsableAttribute, bIsBrowsable);
        }

        bool _blendMode;
        internal void ChangeTrianglesToBlendViewMode(bool forceBlendMode = false)
        {
            if (!_blendMode || forceBlendMode)
            {
                var blendValue = Properties.Settings.Default.SceneViewMAGSBlendFactor;
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var triangleCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
                    {
                        if (this.Triangles[arrayIndex][triangleIndex].Properties != Triangle.typeTriangleProperties.AutoRotationSelected)
                        {
                            this.Triangles[arrayIndex][triangleIndex].Vectors[0].Color.A = blendValue;
                            this.Triangles[arrayIndex][triangleIndex].Vectors[1].Color.A = blendValue;
                            this.Triangles[arrayIndex][triangleIndex].Vectors[2].Color.A = blendValue;
                        }
                    }
                }

                _blendMode = true;

                this.UpdateBinding();
            }
        }

        internal TriangleInfoList GetMAGSAIMarkedTriangles()
        {
            var markedTriangles = new TriangleInfoList();
            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                var trianglesCount = this.Triangles[arrayIndex].Count;
                for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    if (this.Triangles[arrayIndex][triangleIndex].Properties == Triangle.typeTriangleProperties.AutoRotationSelected)
                    {
                        markedTriangles[0].Add(this.Triangles[arrayIndex][triangleIndex]);
                    }
                }
            }

            return markedTriangles;
        }


        internal void ChangeTrianglesToModelViewMode()
        {
            if (_blendMode)
            {
                var blendValue = (byte)this.Index;
                for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
                {
                    var triangleCount = this.Triangles[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
                    {
                        if (this.Triangles[arrayIndex][triangleIndex].Properties != Triangle.typeTriangleProperties.AutoRotationSelected)
                        {
                            this.Triangles[arrayIndex][triangleIndex].Vectors[0].Color.A = blendValue;
                            this.Triangles[arrayIndex][triangleIndex].Vectors[1].Color.A = blendValue;
                            this.Triangles[arrayIndex][triangleIndex].Vectors[2].Color.A = blendValue;
                        }
                    }
                }

                _blendMode = false;

                this.UpdateBinding();
            }
        }
    }

}
