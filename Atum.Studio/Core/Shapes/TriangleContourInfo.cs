using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using OpenTK;
using System;
using System.Collections.Generic;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Shapes
{
    [Serializable]
    public class TriangleContourInfo
    {
        internal static event Action<TriangleContourInfo> ContourSupportChanged;
        internal static event Action<STLModel3D> ContourOuterSupportPointsChanged;

        private float _supportConeTopHeight { get; set; }
        private float _supportConeTopRadius { get; set; }
        private float _supportConeMiddleRadius { get; set; }
        private float _supportConeBottomHeight { get; set; }
        private float _supportConeBottomRadius { get; set; }

        //private bool _selected;
        private float _outlineOffsetDistance;
        private float _outlineDistanceFactor;
        private float _infillOffsetDistance;
        private float _infillDistanceFactor;

        internal List<OuterPathNormal> OuterPoints { get; set; }
        public List<Helpers.ContourHelper.IntPoint> OuterPath { get; set; }
        public List<Vector3> SupportPoints { get; set; }
        public List<List<Helpers.ContourHelper.IntPoint>> InnerPaths { get; set; }
        public List<List<Vector3>> InnerPoints { get; set; }

        internal TriangleContourInfo InfillContour { get; set; }

        internal float SupportConeTopHeight
        {
            get
            {
                return this._supportConeTopHeight;
            }
            set
            {
                if (this._supportConeTopHeight != value)
                {
                    this._supportConeTopHeight = value;

                    foreach (var supportCone in this.SupportStructure)
                    {
                        supportCone.TopHeight = value;
                    }

                    if (this.InfillContour != null)
                    {
                        foreach (var supportCone in this.InfillContour.SupportStructure)
                        {
                            supportCone.TopHeight = value;
                        }
                    }
                }
            }
        }

        internal float SupportConeTopRadius
        {
            get
            {
                return this._supportConeTopRadius;
            }
            set
            {
                if (this._supportConeTopRadius != value)
                {
                    this._supportConeTopRadius = value;

                    var stlModel = this.Model;
                    if (stlModel != null)
                    {
                        var surface = Helpers.STLModelHelper.FindSurfaceByContour(stlModel, this);

                        var surfaceType = Helpers.STLModelHelper.GetSurfaceType(stlModel, surface);
                        if (surfaceType == Helpers.STLModelHelper.TypeOfSurface.Horizontal)
                        {
                            surface.CalcHorizontalContourPath(stlModel.Triangles, this.SupportConeTopRadius);
                        }
                        else if (surfaceType == Helpers.STLModelHelper.TypeOfSurface.Flat)
                        {
                            surface.CalcFlatContourPath(stlModel.Triangles, this.SupportConeTopRadius);
                        }

                        if (surface.Contours != null)
                        {
                            foreach (var contour in surface.Contours)
                            {
                                contour.CalcOuterSupportPoints(stlModel);
                                contour.CreateOuterSupportStructure(stlModel, true, false, true); //must be first to determine outside boundries

                                if (contour.InfillDistanceFactor > 0)
                                {
                                    contour.CalcInfillContour(contour);
                                    contour.CalcInfillSupportPoints(stlModel);
                                }

                                contour.CreateOuterSupportStructure(stlModel, false, true, true);
                                ContourSupportChanged?.Invoke(contour);
                            }
                        }
                    }
                }
            }
        }
        internal float SupportConeMiddleRadius
        {
            get
            {
                return this._supportConeMiddleRadius;
            }
            set
            {
                if (this._supportConeMiddleRadius != value)
                {
                    this._supportConeMiddleRadius = value;

                    foreach (var supportCone in this.SupportStructure)
                    {
                        supportCone.MiddleRadius = value;
                    }


                    if (this.InfillContour != null)
                    {
                        foreach (var supportCone in this.InfillContour.SupportStructure)
                        {
                            supportCone.MiddleRadius = value;
                        }
                    }
                }
            }
        }

        internal float SupportConeBottomHeight
        {
            get
            {
                return this._supportConeBottomHeight;
            }
            set
            {
                if (this._supportConeBottomHeight != value)
                {
                    this._supportConeBottomHeight = value;

                    foreach (var supportCone in this.SupportStructure)
                    {
                        supportCone.BottomHeight = value;
                    }

                    if (this.InfillContour != null)
                    {
                        foreach (var supportCone in this.InfillContour.SupportStructure)
                        {
                            supportCone.BottomHeight = value;
                        }
                    }
                }
            }
        }

        internal float SupportConeBottomRadius
        {
            get
            {
                return this._supportConeBottomRadius;
            }
            set
            {
                if (this._supportConeBottomRadius != value)
                {
                    this._supportConeBottomRadius = value;

                    foreach (var supportCone in this.SupportStructure)
                    {
                        supportCone.BottomRadius = value;
                    }
                    if (this.InfillContour != null)
                    {
                        foreach (var supportCone in this.InfillContour.SupportStructure)
                        {
                            supportCone.BottomRadius = value;
                        }
                    }

                }
            }
        }

        public float OutlineDistanceFactor
        {
            get
            {
                return this._outlineDistanceFactor;
            }
            set
            {
                this._outlineDistanceFactor = value;

                var model = this.Model;
                if (model != null)
                {
                    if (value > 0)
                    {
                        var surface = Helpers.STLModelHelper.FindSurfaceByContour(this.Model, this);
                        var surfaceType = Helpers.STLModelHelper.GetSurfaceType(this.Model, surface);

                        if (surfaceType == Helpers.STLModelHelper.TypeOfSurface.Horizontal)
                        {
                            surface.CalcHorizontalContourPath(this.Model.Triangles, this.SupportConeTopRadius);
                        }
                        else if (surfaceType == Helpers.STLModelHelper.TypeOfSurface.Flat)
                        {
                            surface.CalcFlatContourPath(this.Model.Triangles, this.SupportConeTopRadius);
                        }

                        if (surface.Contours != null)
                        {
                            foreach (var contour in surface.Contours)
                            {
                                contour.CalcOuterSupportPoints(this.Model);
                                contour.CreateOuterSupportStructure(this.Model, true, false, false); //must be first to determine outside boundries

                                if (contour.InfillDistanceFactor > 0)
                                {
                                    contour.CalcInfillContour(contour);
                                    contour.CalcInfillSupportPoints(this.Model);
                                }

                                ContourSupportChanged?.Invoke(contour);
                                contour.CreateOuterSupportStructure(this.Model, false, true, true);
                            }
                        }
                    }
                    else
                    {
                        this.SupportStructure.Clear();
                        if (this.InfillContour != null)
                        {
                            this.InfillContour.SupportStructure.Clear();
                        }
                    }
                }
            
            }
        }

        public float OutlineOffsetDistance
        {
            get
            {
                return this._outlineOffsetDistance;
            }
            set
            {
                this._outlineOffsetDistance = value;

                var model = this.Model;
                if (model != null)
                {
                    if (value > 0)
                    {
                        var surface = Helpers.STLModelHelper.FindSurfaceByContour(this.Model, this);
                        var surfaceType = Helpers.STLModelHelper.GetSurfaceType(this.Model, surface);

                        if (surfaceType == Helpers.STLModelHelper.TypeOfSurface.Horizontal)
                        {
                            surface.CalcHorizontalContourPath(this.Model.Triangles, this.SupportConeTopRadius);
                        }
                        else if (surfaceType == Helpers.STLModelHelper.TypeOfSurface.Flat)
                        {
                            surface.CalcFlatContourPath(this.Model.Triangles, this.SupportConeTopRadius);
                        }

                        if (surface.Contours != null)
                        {
                            foreach (var contour in surface.Contours)
                            {
                                contour.CalcOuterSupportPoints(this.Model);
                                contour.CreateOuterSupportStructure(this.Model, true, false, false); //must be first to determine outside boundries

                                if (contour.InfillDistanceFactor > 0)
                                {
                                    contour.CalcInfillContour(contour);
                                    contour.CalcInfillSupportPoints(this.Model);
                                }

                                ContourSupportChanged?.Invoke(contour);
                                contour.CreateOuterSupportStructure(this.Model, false, true, true);
                            }
                        }
                    }
                    else
                    {
                        this.SupportStructure.Clear();
                        if (this.InfillContour != null)
                        {
                            this.InfillContour.SupportStructure.Clear();
                        }
                    }
                }
            }
        }

        public float InfillOffsetDistance
        {
            get
            {
                return this._infillOffsetDistance;
            }
            set
            {
                this._infillOffsetDistance = value;

                if (this._infillDistanceFactor > 0)
                {
                    var model = this.Model;
                    if (model != null)
                    {
                        CalcInfillContour(this);
                        CalcInfillSupportPoints(model);
                        CreateOuterSupportStructure(model, false, true);
                    }
                }

            }
        }

        public float InfillDistanceFactor
        {
            get
            {
                return this._infillDistanceFactor;
            }
            set
            {
                this._infillDistanceFactor = value;

                if (value > 0)
                {
                    var model = this.Model;
                    if (model != null)
                    {
                        CalcInfillContour(this);
                        CalcInfillSupportPoints(model);
                        CreateOuterSupportStructure(model, false, true);
                    }
                }
                else
                {
                    if (this.InfillContour != null)
                    {
                        this.InfillContour.SupportStructure.Clear();
                    }

                }
            }
        }

        internal STLModel3D Model
        {
            get
            {
                var selectedModel = ObjectView.SelectedModel;
                if (selectedModel != null)
                {
                    return selectedModel as STLModel3D;
                }

                return null;
            }
        }

        [field: NonSerialized]
        public List<SupportCone> SupportStructure;

        public List<Vector3> DebugPoints { get; set; }
        public float LeftPoint { get; set; }
        public float RightPoint { get; set; }
        public float BackPoint { get; set; }
        public float FrontPoint { get; set; }
        public float TopPoint { get; set; }
        public float BottomPoint { get; set; }

        //public bool Selected
        //{
        //    get
        //    {
        //        return this._selected;
        //    }
        //    set
        //    {
        //        foreach (var supportCone in this.SupportStructure)
        //        {
        //            supportCone.Selected = value;
        //        }

        //        foreach (var infillContour in this.InfillContours)
        //        {
        //            if (infillContour != null)
        //            {
        //                foreach (var supportCone in infillContour.SupportStructure)
        //                {
        //                    if (supportCone != null)
        //                    {
        //                        supportCone.Selected = value;
        //                    }
        //                }
        //            }
        //        }

        //        this._selected = value;
        //    }
        //}

        public TriangleContourInfo() : this(SupportManager.DefaultSupportSettings.TopRadius,
            SupportManager.DefaultSupportSettings.TopHeight,
            SupportManager.DefaultSupportSettings.MiddleRadius,
           SupportManager.DefaultSupportSettings.BottomRadius,
            SupportManager.DefaultSupportSettings.BottomHeight,
            UserProfileManager.UserProfile.SupportEngine_Contour_SupportCone_Distance,
            0.2f,
            0.5f,
            0)
        {
        }

        public TriangleContourInfo(float supportConeTopRadius, float supportConeTopHeight, float supportConeMiddleRadius, float supportConeBottomRadius, float supportConeBottomHeight,
            float outlineDistanceFactor, float outlineOffsetDistance, float infillOffsetDistance, float infillDistanceFactor)
        {
            this._supportConeTopHeight = supportConeTopHeight;
            this._supportConeTopRadius = supportConeTopRadius;
            this._supportConeMiddleRadius = supportConeMiddleRadius;
            this._supportConeBottomRadius = supportConeBottomRadius;
            this._supportConeBottomHeight = supportConeBottomHeight;

            this._outlineDistanceFactor = outlineDistanceFactor;
            this._outlineOffsetDistance = outlineOffsetDistance;
            this._infillDistanceFactor = infillDistanceFactor;
            this._infillOffsetDistance = infillOffsetDistance;


            this.OuterPoints = new List<OuterPathNormal>();
            this.OuterPath = new List<Helpers.ContourHelper.IntPoint>();
            this.InnerPaths = new List<List<Helpers.ContourHelper.IntPoint>>();
            this.InnerPoints = new List<List<Vector3>>();
            this.SupportStructure = new List<SupportCone>();
            this.SupportPoints = new List<Vector3>();

            //this.InfillContour = new TriangleContourInfo();

        }


        public void CalcOuterSupportPoints(STLModel3D stlModel)
        {
            if (this.SupportPoints == null)
            {
                this.SupportPoints = new List<Vector3>();
            }

            this.SupportPoints.Clear();

            if (this.OutlineDistanceFactor > 0)
            {
                var currentDistance = 0f;
                var previousDistance = 0f;

                if (this.DebugPoints == null)
                {
                    this.DebugPoints = new List<Vector3>();
                }
                this.DebugPoints.Clear();

                for (var correctedContourPartIndex = 0; correctedContourPartIndex < this.OuterPoints.Count; correctedContourPartIndex++)
                {
                    var contourLine = this.OuterPoints[correctedContourPartIndex];
                    var intersectionPointsBelow = new TriangleInfoList();

                    var contourLineVector = (contourLine.SecondPoint - contourLine.FirstPoint);
                    var contourLineLength = contourLineVector.Length;
                    currentDistance += contourLineLength;

                    var startOffsetDistance = this.OutlineDistanceFactor - previousDistance;

                    while (currentDistance >= this.OutlineDistanceFactor)
                    {
                        var startOffsetVectorPercentage = (startOffsetDistance / contourLineLength);
                        var startOffsetVector = startOffsetVectorPercentage * contourLineVector;
                        this.SupportPoints.Add(contourLine.FirstPoint + startOffsetVector);

                        currentDistance -= this.OutlineDistanceFactor;
                        startOffsetDistance += this.OutlineDistanceFactor;
                    }

                    previousDistance = currentDistance;
                }

                //outside inner points
                for (var innerPointIndex = 0; innerPointIndex < this.InnerPoints.Count; innerPointIndex++)
                {
                    currentDistance = 0f;
                    previousDistance = 0f;

                    for (var correctedContourPartIndex = 0; correctedContourPartIndex < this.InnerPoints[innerPointIndex].Count; correctedContourPartIndex++)
                    {
                        var contourLineStart = this.InnerPoints[innerPointIndex][correctedContourPartIndex];
                        var intersectionPointsBelow = new TriangleInfoList();

                        Vector3 contourLineEnd = new Vector3();
                        if (correctedContourPartIndex == this.InnerPoints[innerPointIndex].Count - 1)
                        {
                            contourLineEnd = this.InnerPoints[innerPointIndex][0];
                        }
                        else
                        {
                            contourLineEnd = this.InnerPoints[innerPointIndex][correctedContourPartIndex + 1];
                        }
                        var contourLineVector = (contourLineEnd - contourLineStart);
                        var contourLineLength = contourLineVector.Length;
                        currentDistance += contourLineLength;

                        var startOffsetDistance = this.OutlineDistanceFactor - previousDistance;

                        while (currentDistance >= this.OutlineDistanceFactor)
                        {
                            var startOffsetVectorPercentage = (startOffsetDistance / contourLineLength);
                            var startOffsetVector = startOffsetVectorPercentage * contourLineVector;
                            this.SupportPoints.Add(contourLineStart + startOffsetVector);

                            currentDistance -= this.OutlineDistanceFactor;
                            startOffsetDistance += this.OutlineDistanceFactor;
                        }

                        previousDistance = currentDistance;
                    }
                }
            }
        }


        public void CreateOuterSupportStructure(STLModel3D stlModel, bool createOuterSupport, bool createInfillSupport, bool raiseOutersupportPointsChanged = true)
        {
            SupportEngine.CreateContourSupport(stlModel, this, createOuterSupport, createInfillSupport);
            if (raiseOutersupportPointsChanged)
            {
                ContourOuterSupportPointsChanged?.Invoke(stlModel);
            }
        }

        public void CalcInfillContour(TriangleContourInfo parentContour)
        {
            if (this.InfillContour != null)
            {
                this.InfillContour.SupportStructure.Clear();
            }
            

            //convert to clipper polygon
            var decimalCorrectionFactor = 10000f;

            var clipperOffset = new Helpers.ContourHelper.ClipperOffset();
            var path = new List<Helpers.ContourHelper.IntPoint>();
            var results = new List<List<Helpers.ContourHelper.IntPoint>>();


            //do 
            var zHeight = 0;
            foreach (var contourLine in this.OuterPoints)
            {
                path.Add(new Helpers.ContourHelper.IntPoint(contourLine.FirstPoint.X * decimalCorrectionFactor, contourLine.FirstPoint.Y * decimalCorrectionFactor, zHeight * decimalCorrectionFactor));
                zHeight++;
            }

            clipperOffset.AddPath(path, Helpers.ContourHelper.JoinType.jtMiter, Helpers.ContourHelper.EndType.etClosedPolygon);
            clipperOffset.Execute(ref results, -(this._infillOffsetDistance * decimalCorrectionFactor));

            var mostLeftPointX = 10000f;
            var mostLeftIndex = 0;
            var currentLeftIndex = 0;

            //calc outerline infill
            var contours = new List<TriangleContourInfo>();
            foreach (var result in results)
            {
                if (result.Count > 0)
                {
                    foreach (var resultPoint in result)
                    {
                        if (resultPoint.X < mostLeftPointX)
                        {
                            mostLeftPointX = resultPoint.X;
                            mostLeftIndex = currentLeftIndex;
                        }
                    }

                    var infillContour = new TriangleContourInfo(parentContour.SupportConeTopRadius, parentContour.SupportConeTopHeight, parentContour.SupportConeMiddleRadius, parentContour.SupportConeBottomRadius, parentContour.SupportConeBottomHeight,
                        parentContour.OutlineDistanceFactor, parentContour.OutlineOffsetDistance, parentContour.InfillOffsetDistance, parentContour.InfillDistanceFactor);
                    var contourTopPoint = 0f;
                    if (this.OuterPath.Count > 0)
                    {
                        contourTopPoint = this.OuterPoints[0].FirstPoint.Z;
                    }

                    infillContour.OuterPath = result;

                    contours.Add(infillContour);

                    currentLeftIndex++;
                }
            }

            if (contours.Count > 0)
            {
                var topPoint = 0f;
                foreach (var outsideVector in this.OuterPoints)
                {
                    topPoint = Math.Max(topPoint, outsideVector.FirstPoint.Z);
                }

                var infillContour = new TriangleContourInfo(this.SupportConeTopRadius, this.SupportConeTopHeight, this.SupportConeMiddleRadius, this.SupportConeBottomRadius, this.SupportConeBottomHeight,
                        this.OutlineDistanceFactor, this.OutlineOffsetDistance, this.InfillOffsetDistance, this.InfillDistanceFactor);
                infillContour.OuterPath = contours[mostLeftIndex].OuterPath;
                infillContour.OuterPoints = contours[mostLeftIndex].OuterPoints;

                //innerpath
                for (var contourIndex = 0; contourIndex < contours.Count; contourIndex++)
                {
                    //if (contourIndex != mostLeftIndex)
                    //{
                        infillContour.InnerPaths.Add(contours[contourIndex].OuterPath);

                        //convert innerpath to inner points using opposite normal
                        clipperOffset = new Helpers.ContourHelper.ClipperOffset();
                        results = new List<List<Helpers.ContourHelper.IntPoint>>();


                        clipperOffset.AddPath(contours[contourIndex].OuterPath, Helpers.ContourHelper.JoinType.jtMiter, Helpers.ContourHelper.EndType.etClosedPolygon);
                        clipperOffset.Execute(ref results, (this._infillOffsetDistance * decimalCorrectionFactor));

                        foreach (var result in results)
                        {
                            var innerPoints = new List<Vector3>();
                            foreach (var point in result)
                            {
                                var correctedPointX = point.X / decimalCorrectionFactor;
                                var correctedPointY = point.Y / decimalCorrectionFactor;
                                innerPoints.Add(new Vector3(correctedPointX, correctedPointY, topPoint));
                            }

                            infillContour.InnerPoints.Add(innerPoints);
                        }
                    //}
                }

                //calc holes offset
                for (var innerContourIndex = 0; innerContourIndex < this.InnerPaths.Count; innerContourIndex++)
                {
                    //convert innerpath to inner points using opposite normal
                    clipperOffset = new Helpers.ContourHelper.ClipperOffset();
                    results = new List<List<Helpers.ContourHelper.IntPoint>>();

                    clipperOffset.AddPath(this.InnerPaths[innerContourIndex], Helpers.ContourHelper.JoinType.jtMiter, Helpers.ContourHelper.EndType.etClosedPolygon);
                    clipperOffset.Execute(ref results, (this._infillOffsetDistance * decimalCorrectionFactor));

                    foreach (var result in results)
                    {
                        infillContour.InnerPaths.Add(result);
                    }
                }

                this.InfillContour = infillContour;
            }
        }

        public void CalcInfillSupportPoints(STLModel3D stlModel)
        {
            var decimalCorrectionFactor = 10000f;

            if (this.InfillContour != null)
            {
                this.InfillContour.LeftPoint = 5000f;
                this.InfillContour.RightPoint = -5000f;
                this.InfillContour.BackPoint = 5000f;
                this.InfillContour.FrontPoint = -5000f;


                foreach (var infillSupportPoint in this.InfillContour.OuterPath)
                {
                    this.InfillContour.LeftPoint = Math.Min(this.InfillContour.LeftPoint, infillSupportPoint.X / decimalCorrectionFactor);
                    this.InfillContour.RightPoint = Math.Max(this.InfillContour.RightPoint, infillSupportPoint.X / decimalCorrectionFactor);
                    this.InfillContour.BackPoint = Math.Min(this.InfillContour.BackPoint, infillSupportPoint.Y / decimalCorrectionFactor);
                    this.InfillContour.FrontPoint = Math.Max(this.InfillContour.FrontPoint, infillSupportPoint.Y / decimalCorrectionFactor);
                }

                //calc grid support points
                var gridOffset = this.InfillDistanceFactor;
                if (gridOffset > 0)
                {
                    for (var gridPointX = 0f; gridPointX < (this.InfillContour.RightPoint - this.InfillContour.LeftPoint); gridPointX += gridOffset)
                    {
                        var pathPointX = (this.InfillContour.LeftPoint + gridPointX) * decimalCorrectionFactor;

                        for (var gridPointY = 0f; gridPointY < (this.InfillContour.FrontPoint - this.InfillContour.BackPoint); gridPointY += gridOffset)
                        {
                            var pathPointY = (this.InfillContour.BackPoint + gridPointY) * decimalCorrectionFactor;
                            var pathPoint = new Helpers.ContourHelper.IntPoint(pathPointX, pathPointY);

                            var isPointInOuterPath = Helpers.ContourHelper.Clipper.PointInPolygon(pathPoint, this.InfillContour.OuterPath);
                            if (isPointInOuterPath == -1 || isPointInOuterPath == 1)
                            {
                                var isPointInInnerPath = false;
                                foreach (var innerPath in this.InfillContour.InnerPaths)
                                {
                                    if (Helpers.ContourHelper.Clipper.PointInPolygon(pathPoint, innerPath) == 1)
                                    {
                                        isPointInInnerPath = true;
                                        break;
                                    }
                                    
                                }
                                if (!isPointInInnerPath)
                                {
                                    //this.InfillContour.SupportPoints.Add(new Vector3(pathPointX / decimalCorrectionFactor, pathPointY / decimalCorrectionFactor, this.OuterPoints[0].Z));
                                }
                            }
                        }
                    }
                }

            }
        }

        internal void UpdateContourSettingsBySupportConeProperties(SupportCone supportCone)
        {
            this.SupportConeBottomHeight = supportCone.BottomHeight;
            this.SupportConeBottomRadius = supportCone.BottomRadius;
            this.SupportConeMiddleRadius = supportCone.MiddleRadius;
            this.SupportConeTopHeight = supportCone.TopHeight;
            this.SupportConeTopRadius = supportCone.TopRadius;
        }

        public WorkspaceSTLModelContour ExportAsWorkspaceFile()
        {
            var workspaceSTLModelContour = new WorkspaceSTLModelContour();
            workspaceSTLModelContour.OutlineDistanceFactor = this.OutlineDistanceFactor;
            workspaceSTLModelContour.OutlineOffsetDistance = this.OutlineOffsetDistance;
            workspaceSTLModelContour.InfillDistanceFactor = this.InfillDistanceFactor;
            workspaceSTLModelContour.InfillOffsetDistance = this.InfillOffsetDistance;
            workspaceSTLModelContour.SupportConeTopHeight = this.SupportConeTopHeight;
            workspaceSTLModelContour.SupportConeTopRadius = this.SupportConeTopRadius;
            workspaceSTLModelContour.SupportConeMiddleRadius = this.SupportConeMiddleRadius;
            workspaceSTLModelContour.SupportConeBottomHeight = this.SupportConeBottomHeight;
            workspaceSTLModelContour.SupportConeBottomRadius = this.SupportConeBottomRadius;

            workspaceSTLModelContour.OuterPath.AddRange(this.OuterPath);
            //workspaceSTLModelContour.OuterPoints.AddRange(this.OuterPoints);
            workspaceSTLModelContour.OuterSupportPoints.AddRange(this.SupportPoints);
            foreach(var innerPath in this.InnerPaths)
            {
                workspaceSTLModelContour.InnerPaths.Add(innerPath);
            }

            foreach (var innerPoints in this.InnerPoints)
            {
                workspaceSTLModelContour.InnerPoints.Add(innerPoints);
            }

            if (this.InfillContour != null)
            {
                workspaceSTLModelContour.InfillContour = this.InfillContour.ExportAsWorkspaceFile();
            }
           

            return workspaceSTLModelContour;
        }

        public static TriangleContourInfo ImportFromWorkspaceFile(WorkspaceSTLModelContour workspaceSTLModelContour)
        {
            var contour = new TriangleContourInfo();
            contour._outlineDistanceFactor = workspaceSTLModelContour.OutlineDistanceFactor;
            contour._outlineOffsetDistance = workspaceSTLModelContour.OutlineOffsetDistance;
            contour._infillDistanceFactor = workspaceSTLModelContour.InfillDistanceFactor;
            contour._infillOffsetDistance = workspaceSTLModelContour.InfillOffsetDistance;

            contour._supportConeTopHeight = workspaceSTLModelContour.SupportConeTopHeight;
            contour._supportConeTopRadius = workspaceSTLModelContour.SupportConeTopRadius;
            contour._supportConeMiddleRadius = workspaceSTLModelContour.SupportConeMiddleRadius;
            contour._supportConeBottomRadius = workspaceSTLModelContour.SupportConeBottomRadius;
            contour._supportConeBottomHeight = workspaceSTLModelContour.SupportConeBottomHeight;

            contour.OuterPath.AddRange(workspaceSTLModelContour.OuterPath);
            //contour.OuterPoints.AddRange(workspaceSTLModelContour.OuterPoints);
            contour.SupportPoints.AddRange(workspaceSTLModelContour.OuterSupportPoints);
            foreach(var innerPath in workspaceSTLModelContour.InnerPaths)
            {
                contour.InnerPaths.Add(innerPath);
            }

            foreach (var innerPoints in workspaceSTLModelContour.InnerPoints)
            {
                contour.InnerPoints.Add(innerPoints);
            }
            
            if (workspaceSTLModelContour.InfillContour != null)
            {
                contour.InfillContour = TriangleContourInfo.ImportFromWorkspaceFile(workspaceSTLModelContour.InfillContour);
            }
    
            return contour;
        }

    }

}
