using System;
using System.Collections.Generic;
using Atum.Studio.Core.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System.ComponentModel;
using System.Drawing;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Utils;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Helpers;
using Atum.DAL.Materials;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class SupportCone : STLModel3D
    {
        private STLModel3D ModelTrianglesWithinBoundryAsStl;

        internal float LastSupportedLayerHeight = float.MinValue;

        public enum TypeSupportCone
        {
            Normal = 0,
            Contour = 1,
            Horizontal = 2,
            Flat = 3
        }

        public enum TypeSupportConeVersion
        {
            Version1 = 0,
            Version2 = 10
        }

        [Browsable(false)]
        public TypeSupportConeVersion SupportConeVersion { get; set; }

        [Browsable(false)]
        public bool PenetrationEnabled { get; set; }

        internal TypeSupportCone SupportConeType { get; set; }
        internal TriangleConnectionInfo ModelIntersectedTriangle { get; set; }
        internal float ModelIntersectionAngle;

        internal List<Vector3Class> DebugLines = new List<Vector3Class>();

        internal bool HasModelIntersection { get; set; }

        internal Vector3 ModelIntersectionPoint
        {
            get
            {
                return new Vector3(this.Position.X, this.Position.Y, this.TopPoint);
            }
        }

        internal float _topRadius;
        internal float _totalHeight;
        internal float _topHeight;
        internal float _middleRadius;
        internal float _bottomHeight;
        internal float _bottomRadius;
        internal bool _groundSupport;

        #region Undo/Redo
        [Browsable(false)]
        internal bool IsManualSupportAdding { get; set; }
        [Browsable(false)]
        internal bool IsManualGridSupportAdding { get; set; }
        [Browsable(false)]
        internal Point MousePosition { get; set; }
        [Browsable(false)]
        internal TriangleIntersection PreviousSelectedTriangle { get; set; }
        [Browsable(false)]
        internal TriangleIntersection SelectedTriangle { get; set; }
        #endregion

        internal float DEFAULT_HEIGHT = 25f;

        //overrides
        [Browsable(false)]
        [Category("Dimensions")]
        [DisplayName("Total Height")]
        [Description("Total Height of Support Cone\nUnit: mm")]
        public float TotalHeight
        {
            get
            {
                return this._totalHeight;
            }
            set
            {
                this._totalHeight = value;
                this.UpdateSupportCone();
            }
        }
        [Category("Dimensions")]
        [DisplayName("Top Height")]
        [Description("Top Part Height of Cone\nUnit: mm")]
        public virtual float TopHeight
        {
            get
            {
                return this._topHeight;
            }
            set
            {
                if (this._totalHeight - this.BottomHeight - value <= 0)
                {
                    this._topHeight = this._totalHeight - this.BottomHeight;
                }
                else
                {
                    this._topHeight = value;
                }


                this.UpdateSupportCone();
            }
        }
        [Category("Dimensions")]
        [DisplayName("Top Radius")]
        [Description("Top Part Radius of Cone\nUnit: mm")]
        public virtual float TopRadius
        {
            get
            {
                return this._topRadius;
            }
            set
            {
                if (this._topRadius != value)
                {
                    this._topRadius = value;

                    this.CalcModelIntersectionTriangles(this.Position, this.Model, null);
                    this.UpdateSupportCone();
                }
            }
        }

        [Category("Surface Support")]
        [DisplayName("Cone Distance")]
        [Description("Cone Distance\nUnit: factor")]
        public override float SupportDistance
        {
            get
            {
                if (this.Model != null)
                {
                    lock (this.Model.Triangles.HorizontalSurfaces)
                    {
                        foreach (var surface in this.Model.Triangles.HorizontalSurfaces)
                        {
                            foreach (var supportCone in surface.SupportStructure)
                            {
                                if (supportCone == this)
                                {
                                    return surface.SupportDistance;
                                }
                            }
                        }
                    }

                    lock (this.Model.Triangles.FlatSurfaces)
                    {
                        foreach (var surface in this.Model.Triangles.FlatSurfaces)
                        {
                            foreach (var supportCone in surface.SupportStructure)
                            {
                                if (supportCone == this)
                                {
                                    return surface.SupportDistance;
                                }
                            }
                        }
                    }
                }
                return 0;
            }
            set
            {
                lock (this.Model.Triangles.HorizontalSurfaces)
                {
                    foreach (var surface in this.Model.Triangles.HorizontalSurfaces)
                    {
                        foreach (var supportCone in surface.SupportStructure)
                        {
                            if (supportCone == this)
                            {
                                surface.SupportDistance = value;
                                return;
                            }
                        }
                    }
                }

                lock (this.Model.Triangles.FlatSurfaces)
                {
                    foreach (var surface in this.Model.Triangles.FlatSurfaces)
                    {
                        foreach (var supportCone in surface.SupportStructure)
                        {
                            if (supportCone == this)
                            {
                                surface.SupportDistance = value;
                                return;
                            }
                        }
                    }
                }
            }
        }

        [Category("Surface Support")]
        [DisplayName("Cross Support")]
        [Description("Cross Support\nUnit: Enable/Disable")]
        public override bool CrossSupport
        {
            get
            {
                if (this.Model != null)
                {
                    lock (this.Model.Triangles.HorizontalSurfaces)
                    {
                        foreach (var surface in this.Model.Triangles.HorizontalSurfaces)
                        {

                            foreach (var supportCone in surface.SupportStructure)
                            {
                                if (supportCone == this)
                                {
                                    return surface.CrossSupport;
                                }
                            }
                        }
                    }

                    lock (this.Model.Triangles.FlatSurfaces)
                    {
                        foreach (var surface in this.Model.Triangles.FlatSurfaces)
                        {

                            foreach (var supportCone in surface.SupportStructure)
                            {
                                if (supportCone == this)
                                {
                                    return surface.CrossSupport;
                                }
                            }
                        }
                    }
                }
                return false;
            }
            set
            {
                lock (this.Model.Triangles.HorizontalSurfaces)
                {
                    foreach (var surface in this.Model.Triangles.HorizontalSurfaces)
                    {
                        foreach (var supportCone in surface.SupportStructure)
                        {
                            if (supportCone == this)
                            {
                                surface.CrossSupport = value;
                                return;
                            }
                        }
                    }
                }

                lock (this.Model.Triangles.FlatSurfaces)
                {
                    foreach (var surface in this.Model.Triangles.FlatSurfaces)
                    {
                        foreach (var supportCone in surface.SupportStructure)
                        {
                            if (supportCone == this)
                            {
                                surface.CrossSupport = value;
                                return;
                            }
                        }
                    }
                }
            }
        }

        [Browsable(false)]
        public bool InvalidPenetrationPoints { get; set; }

        [Browsable(false)]
        public bool IsSurfaceSupport
        {
            get
            {
                var surfaceStructureCone = false;

                if (this.Model != null)
                {
                    foreach (var supportCone in this.Model.Triangles.HorizontalSurfaces.SupportStructure)
                    {
                        if (supportCone == this)
                        {
                            surfaceStructureCone = true;
                            break;
                        }
                    }

                    foreach (var supportCone in this.Model.Triangles.FlatSurfaces.SupportStructure)
                    {
                        if (supportCone == this)
                        {
                            surfaceStructureCone = true;
                            break;
                        }
                    }
                }

                return surfaceStructureCone;
            }
        }



        [Browsable(false)]
        public TriangleSurfaceInfo SupportSurface
        {
            get
            {
                TriangleSurfaceInfo surfaceStructure = null;
                if (this.Model != null)
                {
                    lock (this.Model.Triangles.HorizontalSurfaces)
                    {
                        foreach (var surface in this.Model.Triangles.HorizontalSurfaces)
                        {
                            foreach (var supportCone in surface.SupportStructure)
                            {
                                if (supportCone == this)
                                {
                                    surfaceStructure = surface;
                                    return surfaceStructure;
                                }
                            }
                        }
                    }

                    lock (this.Model.Triangles.FlatSurfaces)
                    {
                        foreach (var surface in this.Model.Triangles.FlatSurfaces)
                        {
                            foreach (var supportCone in surface.SupportStructure)
                            {
                                if (supportCone == this)
                                {
                                    surfaceStructure = surface;
                                    return surfaceStructure;
                                }
                            }
                        }
                    }
                }

                return surfaceStructure;
            }
        }

        private void UpdateSupportCone()
        {
            var currentRotationAngleX = this._rotationAngleX;
            var currentRotationAngleZ = this._rotationAngleZ;
            this._rotationAngleZ = this._rotationAngleX = 0;
            var currentPosition = this.Position;
            var surfaceTriangles = new STLModel3D();
            surfaceTriangles.Triangles = new TriangleInfoList();
            if (this.IsSurfaceSupport)
            {
                foreach (var surfaceArrayTriangleIndex in this.SupportSurface.Keys)
                {
                    surfaceTriangles.Triangles[0].Add(this.Model.Triangles[surfaceArrayTriangleIndex.ArrayIndex][surfaceArrayTriangleIndex.TriangleIndex]);
                }
            }
            else
            {
                surfaceTriangles = null;
            }


            this.CreateSupportCone(currentRotationAngleX, currentRotationAngleZ, position: this.Position, model: this.Model, groundSupport: this._groundSupport, surfaceTriangles: surfaceTriangles, supportConeType: this.SupportConeType, bottomWidthCorrection:this.BottomWidthCorrection);
            this.UpdateBoundries();


        }

        [Category("Dimensions")]
        [DisplayName("Middle Radius")]
        [Description("Middle Part Radius of Cone\nUnit: mm")]
        public virtual float MiddleRadius
        {
            get
            {
                return this._middleRadius;
            }
            set
            {
                this._middleRadius = value;
                this.UpdateSupportCone();
            }
        }
        [Category("Dimensions")]
        [DisplayName("Bottom Height")]
        [Description("Bottom Part Height of Cone\nUnit: mm")]
        public virtual float BottomHeight
        {
            get
            {
                return this._bottomHeight;
            }
            set
            {
                if (this._totalHeight - this.TopHeight - value <= 0)
                {
                    this._bottomHeight = this._totalHeight - this.TopHeight;
                }
                else
                {
                    this._bottomHeight = value;
                }

                this.UpdateSupportCone();
            }
        }
        [Category("Dimensions")]
        [DisplayName("Bottom Radius")]
        [Description("Bottom Part Radius of Cone\nUnit: mm")]
        public virtual float BottomRadius
        {
            get
            {
                return this._bottomRadius;
            }
            set
            {
                this._bottomRadius = value;
                this.CalcModelIntersectionTriangles(this.Position, this.Model, null);
                this.UpdateSupportCone();
            }
        }

        public virtual float BottomRadiusWithBottomCorrection
        {
            get
            {
                return this._bottomRadius + (_totalHeight * (this.BottomWidthCorrection / 100));
            }
        }

        internal int _slicesCount { get; set; }

        [Browsable(false)]
        public float BottomWidthCorrection { get; set; }
        internal override bool Hidden { get; set; }

        internal Vector3Class Position { get; set; }
        internal Vector3Class Normal { get; set; }
        internal typeCreationBy CreationBy { get; set; }

        private Cone _bottomCone { get; set; }
        private Cone _middleCone { get; set; }

        internal enum typeCreationBy
        {
            Manual = 0,
            ManualCrossSupport,
            Auto = 5
        }

        internal new Vector3Class Center
        {
            get
            {
                return this.Position;
            }
        }

        internal SupportCone()
            : base(TypeObject.Support, false)
        {
            this._scaleFactorX = 1;
            this._scaleFactorY = 1;
            this._scaleFactorZ = 1;
            this.Normal = new Vector3Class(0, 0, 1);
            this.Triangles = new TriangleInfoList();
        }

        internal SupportCone(TypeObject objectType)
            : base(TypeObject.Support, false)
        {
            this.ObjectType = TypeObject.Support;
            this._scaleFactorX = 1;
            this._scaleFactorY = 1;
            this._scaleFactorZ = 1;
            this.Normal = new Vector3Class(0, 0, 1);
        }

        public SupportCone(float totalHeight, float topHeight, float topRadius, float middleRadius, float bottomHeight, float bottomRadius, int slicesCount, Vector3Class position, Color color, float rotationAngleX = 0, float rotationAngleZ = 0, STLModel3D model = null, bool groundSupport = true, bool useSupportPenetration = false, STLModel3D surfaceTriangles = null, STLModel3D trianglesXYInRange = null, TypeSupportCone supportConeType = TypeSupportCone.Normal, float bottomWidthCorrection = 0f)
: base(TypeObject.Support, false)
        {
            this._groundSupport = groundSupport;
            this._scaleFactorX = 1;
            this._scaleFactorY = 1;
            this._scaleFactorZ = 1;
            this._totalHeight = totalHeight;
            this._topHeight = topHeight;
            this._topRadius = topRadius;
            this._middleRadius = middleRadius;
            this._bottomHeight = bottomHeight;
            this._bottomRadius = bottomRadius;
            this._slicesCount = slicesCount;
            this._color = color;
            this.BottomWidthCorrection = bottomWidthCorrection;

            //when bottom and top are larger then total cone split the new height in half
            if ((this.BottomHeight + this.TopHeight) > totalHeight)
            {
                this._bottomHeight = this._topHeight = this.TotalHeight / 2;
            }

            this.CreateSupportCone(rotationAngleX, rotationAngleZ, position, model, groundSupport, useSupportPenetration, surfaceTriangles: surfaceTriangles, trianglesXYInRange: trianglesXYInRange, supportConeType: supportConeType, bottomWidthCorrection: bottomWidthCorrection);
        }

        private void CreateSupportCone(float rotationAngleX = 0, float rotationAngleZ = 0, Vector3Class position = null, STLModel3D model = null, bool groundSupport = true, bool useSupportPenetration = true, STLModel3D surfaceTriangles = null, STLModel3D trianglesXYInRange = null, TypeSupportCone supportConeType = TypeSupportCone.Normal, float bottomWidthCorrection = 0.00f)
        {
            this.SupportConeType = supportConeType;
            this.PenetrationEnabled = false;
            this.Position = position;
            this.Normal = new Vector3Class(0, 0, 1);
            this.RotationAngleX = rotationAngleX;
            this.RotationAngleZ = rotationAngleZ;
            this.InvalidPenetrationPoints = false;
            this.HasModelIntersection = false;

            var topCapCenterPoint = this.Position + new Vector3Class(0, 0, this.TotalHeight);
            var bottomCapCenterPoint = this.Position;

            if (trianglesXYInRange != null)
            {
                this.ModelTrianglesWithinBoundryAsStl = trianglesXYInRange;
            }

            if (this.ModelTrianglesWithinBoundryAsStl == null)
            {
                CalcModelIntersectionTriangles(topCapCenterPoint, model, surfaceTriangles);
            }

            var supportEnginePenetrationDepthVector = new Vector3Class(0, 0, UserProfileManager.UserProfile.SupportEngine_Penetration_Depth);

            if (this.Triangles == null) { this.Triangles = new TriangleInfoList(); }
            this.Triangles[0].Clear();

            var topRadiusPoints = VectorHelper.CreateCircle(this.TotalHeight, this.TopRadius, this._slicesCount, true);
            var bottomRadiusPoints = VectorHelper.CreateCircle(this.Position.Z, this.BottomRadius, this._slicesCount, false);

            if (groundSupport)
            {
                bottomRadiusPoints = VectorHelper.CreateCircle(0, this.BottomRadius + (_totalHeight * (bottomWidthCorrection / 100)), this._slicesCount, false);
            }

            if (model != null && RotationAngleX == 0 && rotationAngleX == 0 && rotationAngleZ == 0)
            {
                if (useSupportPenetration)
                {
                    //get top penetration points
                    var topPenetrationPoints = new List<Vector3Class>();
                    var topRadiusRequireMoveTranslations = new List<Vector3Class>();
                    var radiusPointIndex = 0;
                    foreach (var topRadiusPoint in topRadiusPoints)
                    {
                        var lowerTopOffset = Math.Max(this.TopHeight, this.TopRadius);
                        var lowerTopRadiusPoint = new Vector3Class(topRadiusPoint.X, topRadiusPoint.Y, topRadiusPoint.Z - (lowerTopOffset * 2));
                        if (!groundSupport)
                        {
                            lowerTopRadiusPoint = new Vector3Class(topRadiusPoint.X, topRadiusPoint.Y, topCapCenterPoint.Z - lowerTopOffset * 2);
                        }

                        bottomCapCenterPoint.Z = topCapCenterPoint.Z - this.TotalHeight - (supportEnginePenetrationDepthVector.Z);

                        //find first intersectionpoint 
                        var supportDistance = 50000f;
                        var nearestIntersectionPoint = new Vector3Class();
                        TriangleIntersection[] intersectedTriangles = null;
                        IntersectionProvider.IntersectTriangle(lowerTopRadiusPoint + new Vector3Class(this.Position.X, this.Position.Y, 0), this.Normal, ModelTrianglesWithinBoundryAsStl, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out intersectedTriangles);
                        var intersectionPointFound = false;
                        var topRadiusMoveTranslation = new Vector3Class();
                        if (intersectedTriangles != null && intersectedTriangles.Length > 0)
                        {
                            foreach (var penetrationIntersectionPoint in intersectedTriangles)
                            {
                                if (penetrationIntersectionPoint.IntersectionPoint != new Vector3Class() && penetrationIntersectionPoint.Normal.Z < 0)
                                {
                                    var supportOffset = (new Vector3Class(0, 0, penetrationIntersectionPoint.IntersectionPoint.Z) - new Vector3Class(0, 0, lowerTopRadiusPoint.Z)).Length;
                                    //point within range?
                                    if (supportOffset < supportDistance && penetrationIntersectionPoint.IntersectionPoint.Z > lowerTopRadiusPoint.Z && penetrationIntersectionPoint.IntersectionPoint.Z - (lowerTopOffset * 5) < lowerTopRadiusPoint.Z)
                                    {
                                        intersectionPointFound = true;

                                        supportDistance = supportOffset;
                                        if (groundSupport)
                                        {
                                            nearestIntersectionPoint = new Vector3Class(lowerTopRadiusPoint.X, lowerTopRadiusPoint.Y, penetrationIntersectionPoint.IntersectionPoint.Z);
                                        }
                                        else
                                        {
                                            penetrationIntersectionPoint.CalcCenter();
                                            nearestIntersectionPoint = new Vector3Class(lowerTopRadiusPoint.X, lowerTopRadiusPoint.Y, penetrationIntersectionPoint.IntersectionPoint.Z - this.Position.Z);
                                        }

                                        ModelIntersectedTriangle = penetrationIntersectionPoint.Index;
                                        ModelIntersectionAngle = OpenTK.MathHelper.RadiansToDegrees(Vector3Class.CalculateAngle(penetrationIntersectionPoint.IntersectionPoint, Vector3Class.UnitZ));
                                    }
                                }
                            }

                            topPenetrationPoints.Add(nearestIntersectionPoint);
                        }

                        if (!intersectionPointFound && topRadiusMoveTranslation == new Vector3Class())
                        {
                            topRadiusMoveTranslation = this.Position - topRadiusPoint;
                            topRadiusRequireMoveTranslations.Add(topRadiusPoint);
                        }

                        radiusPointIndex++;
                    }

                    //second check
                    if (topRadiusRequireMoveTranslations.Count > 0)
                    {
                        //subtract first and last to determine baseline
                        var supportConeOverhangDiameter = (topRadiusRequireMoveTranslations[0] + topRadiusRequireMoveTranslations[topRadiusRequireMoveTranslations.Count - 1]);
                        supportConeOverhangDiameter.Z = 0;

                        //align point in circle order
                        var alignedTopRadiusPoints = new List<Vector3Class>();
                        foreach (var topRadiusPoint in topRadiusPoints)
                        {
                            var alignPoint = new Vector3Class();
                            if (topRadiusRequireMoveTranslations.Contains(topRadiusPoint))
                            {
                                alignPoint = topRadiusPoint;
                            }

                            alignedTopRadiusPoints.Add(alignPoint);
                        }

                        var totalAlignedTopRadiusPoints = alignedTopRadiusPoints.Count;
                        for (var alignedPointIndex = 0; alignedPointIndex < totalAlignedTopRadiusPoints; alignedPointIndex++)
                        {
                            var alignedPoint = alignedTopRadiusPoints[alignedPointIndex];
                            if (alignedPoint == new Vector3Class())
                            {
                                break;
                            }

                            alignedTopRadiusPoints.Add(alignedPoint);
                        }

                        //rev order
                        var beginOrderedPointsIndex = 0;
                        var endOrderedPointsIndex = 0;
                        for (var alignedPointIndex = alignedTopRadiusPoints.Count - 1; alignedPointIndex > 0; alignedPointIndex--)
                        {
                            var alignedPoint = alignedTopRadiusPoints[alignedPointIndex];
                            if (alignedPoint != new Vector3Class() && endOrderedPointsIndex == 0)
                            {
                                endOrderedPointsIndex = alignedPointIndex + 1;
                            }

                            if (endOrderedPointsIndex > 0 && alignedPoint == new Vector3Class())
                            {
                                beginOrderedPointsIndex = alignedPointIndex + 1;
                                break;
                            }
                        }

                        var orderedPointsAsArray = new List<Vector3Class>();

                        for (var alignedPointIndex = beginOrderedPointsIndex; alignedPointIndex < endOrderedPointsIndex; alignedPointIndex++)
                        {
                            orderedPointsAsArray.Add(alignedTopRadiusPoints[alignedPointIndex]);
                        }

                        if (orderedPointsAsArray[0] == new Vector3Class())
                        {
                            orderedPointsAsArray.RemoveAt(0);
                        }

                        //calc normal
                        var supportOverhangCenter = new Vector3Class();
                        var middlePointOffOverhang = new Vector3Class();
                        if (orderedPointsAsArray.Count % 2 == 1)
                        {
                            //add subtract 1 and /2
                            middlePointOffOverhang = orderedPointsAsArray[(orderedPointsAsArray.Count - 1) / 2];
                            middlePointOffOverhang.Z = 0;
                        }
                        else if (orderedPointsAsArray.Count > 0)
                        {
                            // get middle point of arc
                            var beginMiddlePointOverhang = orderedPointsAsArray[0];
                            var endMiddlePointOverhang = orderedPointsAsArray[orderedPointsAsArray.Count - 1];
                            var middlePointArc = (endMiddlePointOverhang - beginMiddlePointOverhang).Normalized() * this.TopRadius;
                            middlePointOffOverhang = middlePointArc;
                            middlePointOffOverhang.Z = 0;
                        }

                        var movedTopRadiusPoints = new List<Vector3Class>();
                        var moveTranslation = new Vector3Class();
                        moveTranslation = (middlePointOffOverhang - supportOverhangCenter);
                        for (var topRadiusPointIndex = 0; topRadiusPointIndex < topRadiusPoints.Count; topRadiusPointIndex++)
                        {
                            movedTopRadiusPoints.Add(topRadiusPoints[topRadiusPointIndex] - moveTranslation);
                        }

                        //check if all moved points have intersection point
                        var movedIntersectionPointFound = false;
                        TriangleIntersection[] intersectedTriangles = null;
                        var movedPenetrationPoints = new List<Vector3Class>();

                        foreach (var movedTopRadiusPoint in movedTopRadiusPoints)
                        {
                            var supportDistance = 50000f;
                            var nearestIntersectionPoint = new Vector3Class();
                            var movedLowerTopRadiusPoint = new Vector3Class(movedTopRadiusPoint.X, movedTopRadiusPoint.Y, movedTopRadiusPoint.Z - this.TopHeight);
                            var lowerTopOffset = Math.Max(this.TopHeight, this.TopRadius);
                            if (!groundSupport) movedLowerTopRadiusPoint = new Vector3Class(movedTopRadiusPoint.X, movedTopRadiusPoint.Y, this.Position.Z - lowerTopOffset * 2);
                            IntersectionProvider.IntersectTriangle(movedLowerTopRadiusPoint + new Vector3Class(this.Position.X, this.Position.Y, 0), this.Normal, ModelTrianglesWithinBoundryAsStl, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out intersectedTriangles);
                            if (intersectedTriangles != null && intersectedTriangles.Length > 0)
                            {
                                foreach (var penetrationIntersectionPoint in intersectedTriangles)
                                {

                                    if (penetrationIntersectionPoint.IntersectionPoint != new Vector3Class() && penetrationIntersectionPoint.Normal.Z < 0)
                                    {
                                        var supportOffset = (new Vector3(0, 0, penetrationIntersectionPoint.IntersectionPoint.Z) - new Vector3(0, 0, movedLowerTopRadiusPoint.Z)).Length;
                                        if (supportOffset < supportDistance && penetrationIntersectionPoint.IntersectionPoint.Z > movedLowerTopRadiusPoint.Z && penetrationIntersectionPoint.IntersectionPoint.Z - (lowerTopOffset * 5) < movedLowerTopRadiusPoint.Z)
                                        {
                                            movedIntersectionPointFound = true;
                                            supportDistance = supportOffset;
                                            if (groundSupport)
                                            {
                                                nearestIntersectionPoint = new Vector3Class(movedLowerTopRadiusPoint.X, movedLowerTopRadiusPoint.Y, penetrationIntersectionPoint.IntersectionPoint.Z);
                                            }
                                            else
                                            {
                                                //align to middle cone
                                                nearestIntersectionPoint = new Vector3Class(movedLowerTopRadiusPoint.X, movedLowerTopRadiusPoint.Y, supportDistance - this.TopHeight);
                                            }

                                            ModelIntersectedTriangle = penetrationIntersectionPoint.Index;
                                            ModelIntersectionAngle = OpenTK.MathHelper.RadiansToDegrees(Vector3Class.CalculateAngle(penetrationIntersectionPoint.IntersectionPoint, Vector3Class.UnitZ));
                                        }
                                    }
                                }
                            }

                            if (movedIntersectionPointFound)
                            {
                                movedPenetrationPoints.Add(nearestIntersectionPoint);
                            }
                        }

                        if (movedPenetrationPoints.Count == movedTopRadiusPoints.Count)
                        {
                            //check if all points are unique
                            var duplicatesFound = false;
                            foreach (var movedPenetrationPoint in movedPenetrationPoints)
                            {
                                var itemFoundCount = 0;
                                foreach (var movedPenetrationPointCheck in movedPenetrationPoints)
                                {
                                    if (movedPenetrationPoint == movedPenetrationPointCheck)
                                    {
                                        itemFoundCount++;
                                    }
                                }

                                if (itemFoundCount > 1)
                                {
                                    duplicatesFound = true;
                                    break;
                                }
                            }

                            if (duplicatesFound)
                            {
                                this.InvalidPenetrationPoints = true;
                            }
                            else
                            {
                                //submit penetration points
                                topPenetrationPoints = movedPenetrationPoints;
                            }

                        }
                        else
                        {
                            //hide this cone because 
                            this.InvalidPenetrationPoints = true;
                        }

                    }

                    //bottom points
                    var bottomPenetrationPoints = new List<Vector3Class>();
                    if (!groundSupport)
                    {
                        foreach (var bottomRadiusPoint in bottomRadiusPoints)
                        {
                            var bottomNormal = new Vector3Class(0, 0, -1);
                            var upperBottomOffset = Math.Max(this.BottomHeight, this.BottomRadius);
                            var upperBottomRadiusPoint = new Vector3(bottomRadiusPoint.X, bottomRadiusPoint.Y, this.Position.Z + upperBottomOffset * 2);

                            //find first intersectionpoint 
                            var supportDistance = 50000f;
                            var nearestIntersectionPoint = new Vector3Class();
                            TriangleIntersection[] intersectedTriangles = null;
                            IntersectionProvider.IntersectTriangle(new Vector3Class(upperBottomRadiusPoint.X, upperBottomRadiusPoint.Y, 1000) + new Vector3Class(this.Position.X, this.Position.Y, 0), new Vector3Class(0, 0, -1), ModelTrianglesWithinBoundryAsStl, Utils.IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out intersectedTriangles);
                            //IntersectionProvider.IntersectTriangle(upperBottomRadiusPoint + new Vector3(this.Position.X, this.Position.Y, 0), new Vector3(0,0,1), ModelTrianglesWithinBoundryAsStl, IntersectionProvider.typeDirection.TwoWay, false, out intersectedTriangles);
                            {
                                if (intersectedTriangles != null && intersectedTriangles.Length > 0)
                                {
                                    foreach (var penetrationIntersectionPoint in intersectedTriangles)
                                    {
                                        if (penetrationIntersectionPoint.IntersectionPoint != new Vector3Class())
                                        {

                                            var supportOffset = (new Vector3Class(0, 0, penetrationIntersectionPoint.IntersectionPoint.Z) - new Vector3Class(0, 0, upperBottomRadiusPoint.Z)).Length;
                                            if (supportOffset < supportDistance && penetrationIntersectionPoint.IntersectionPoint.Z < upperBottomRadiusPoint.Z && penetrationIntersectionPoint.IntersectionPoint.Z < upperBottomRadiusPoint.Z)
                                            {
                                                supportDistance = supportOffset;

                                                //DebugLines.Add(penetrationIntersectionPoint.IntersectionPoint);
                                                //DebugLines.Add(penetrationIntersectionPoint.IntersectionPoint + new Vector3(0, 0, 10));
                                                nearestIntersectionPoint = new Vector3Class(upperBottomRadiusPoint.X, upperBottomRadiusPoint.Y, penetrationIntersectionPoint.IntersectionPoint.Z - this.Position.Z);
                                            }
                                        }
                                    }

                                    bottomPenetrationPoints.Add(nearestIntersectionPoint);
                                }
                            }
                        }
                    }



                    //subtract ground support
                    //var basementOffset = new Vector3Class();
                    //if (groundSupport && model.SupportBasement)
                    //{
                    //    basementOffset = supportEngineBasementThicknessVector;

                    //    for (var penetrationPointIndex = 0; penetrationPointIndex < topPenetrationPoints.Count; penetrationPointIndex++)
                    //    {
                    //        topPenetrationPoints[penetrationPointIndex] -= basementOffset;
                    //    }
                    //}

                    if (topPenetrationPoints.Count == 16 && !topPenetrationPoints.Contains(new Vector3Class()))
                    {
                        topRadiusPoints = topPenetrationPoints;

                        topCapCenterPoint.Z = this.TotalHeight + supportEnginePenetrationDepthVector.Z;
                        topCapCenterPoint += new Vector3Class(0, 0, this.Position.Z);

                        var penetrationPointsCorrected = new List<Vector3Class>();

                        //max z distance = 1.5 * top radius when > then point.z = center.z
                        for (var movedTopPenetrationPointIndex = 0; movedTopPenetrationPointIndex < topPenetrationPoints.Count; movedTopPenetrationPointIndex++)
                        {
                            var previousPenetrationPoint = topPenetrationPoints[15];
                            if (movedTopPenetrationPointIndex > 0)
                            {
                                previousPenetrationPoint = topPenetrationPoints[movedTopPenetrationPointIndex - 1];
                            }

                            var penetrationPoint = topPenetrationPoints[movedTopPenetrationPointIndex];
                            var zBetweenPoints = (previousPenetrationPoint.Z - penetrationPoint.Z);
                            if (zBetweenPoints < 0) zBetweenPoints = -zBetweenPoints;
                            if (zBetweenPoints > this.TopRadius)
                            {
                                this.InvalidPenetrationPoints = true;
                            }
                            else
                            {
                                penetrationPointsCorrected.Add(penetrationPoint);
                            }
                            //    if ((previousPenetrationPoint.Z - penetrationPoint.Z))
                            //    if (penetrationPoint.Z > topCapCenterPoint.Z + (4 * this.TopRadius))
                            //{
                            //    penetrationPointsCorrected.Add(new Vector3(penetrationPoint.X, penetrationPoint.Y, topCapCenterPoint.Z));
                            //    this.InvalidPenetrationPoints = true;
                            //}
                            //else
                            //{
                            //    penetrationPointsCorrected.Add(penetrationPoint);
                            //}
                        }

                        topRadiusPoints = penetrationPointsCorrected;
                    }
                    else
                    {
                        //mark support as invalid
                        this.InvalidPenetrationPoints = true;
                    }

                    if (!groundSupport)
                    {
                        if (bottomPenetrationPoints.Count == 16 && !bottomPenetrationPoints.Contains(new Vector3Class()))
                        {
                            var penetrationPointsCorrected = new List<Vector3Class>();

                            //max z distance = 1.5 * top radius when > then point.z = center.z
                            for (var bottomPenetrationPointIndex = 0; bottomPenetrationPointIndex < bottomPenetrationPoints.Count; bottomPenetrationPointIndex++)
                            {
                                var penetrationPoint = bottomPenetrationPoints[bottomPenetrationPointIndex];
                                if (penetrationPoint.Z < -(6 * this.BottomRadius))
                                {
                                    penetrationPointsCorrected.Add(new Vector3Class(penetrationPoint.X, penetrationPoint.Y, 0));
                                    this.InvalidPenetrationPoints = true;
                                }
                                else
                                {
                                    penetrationPointsCorrected.Add(penetrationPoint);
                                }
                            }
                            bottomRadiusPoints = bottomPenetrationPoints;
                        }
                        else
                        {
                            //mark support as invalid
                            this.InvalidPenetrationPoints = true;
                        }
                    }
                }
            }

            #region TopCone
            var leftTriangle = new Triangle();
            var rightTriangle = new Triangle();
            var topCapOffset = new Vector3Class(0, 0, this.TotalHeight - this.TopHeight - this.BottomHeight);

            //create top cone
            if (this.InvalidPenetrationPoints)
            {
                this.Triangles[0].Clear();
                topRadiusPoints = VectorHelper.CreateCircle(this.TotalHeight, this.TopRadius, this._slicesCount, true);
                var topRadiusPointsWithDepth = new List<Vector3Class>();
                foreach (var topRadiusPoint in topRadiusPoints)
                {
                    topRadiusPointsWithDepth.Add(topRadiusPoint);// + supportEnginePenetrationDepthVector);
                }

                topRadiusPoints = topRadiusPointsWithDepth;
            }
            else if (topRadiusPoints.Count > 0)
            {
                //max z distance = 1.5 * top radius when > then point.z = center.z
                var penetrationPointsCorrected = new List<Vector3Class>();
                for (var topRadiusPointIndex = 0; topRadiusPointIndex < topRadiusPoints.Count; topRadiusPointIndex++)
                {
                    var penetrationPoint = topRadiusPoints[topRadiusPointIndex];// + new Vector3(0, 0, this.Position.Z);
                    if (penetrationPoint.Z > topCapCenterPoint.Z + (2 * this.TopRadius))
                    {
                        penetrationPoint = new Vector3Class(penetrationPoint.X, penetrationPoint.Y, topCapCenterPoint.Z);
                    }

                    if (!groundSupport)
                    {
                        //    penetrationPoint = penetrationPoint - new Vector3(0, 0, topCapCenterPoint.Z);
                        //  penetrationPoint.Z = -penetrationPoint.Z;
                    }

                    penetrationPointsCorrected.Add(penetrationPoint);
                }
                topRadiusPoints = penetrationPointsCorrected;
            }

            //create bottom cone
            if (!groundSupport)
            {
                if (this.InvalidPenetrationPoints)
                {
                    this.Triangles[0].Clear();
                    bottomRadiusPoints = VectorHelper.CreateCircle(0, this.BottomRadius, this._slicesCount, true);
                    var bottomRadiusPointsWithDepth = new List<Vector3Class>();
                    foreach (var bottomRadiusPoint in bottomRadiusPoints)
                    {
                        bottomRadiusPointsWithDepth.Add(bottomRadiusPoint - supportEnginePenetrationDepthVector);
                    }

                    bottomRadiusPoints = bottomRadiusPointsWithDepth;
                }
                else if (bottomRadiusPoints.Count > 0)
                {
                    //max z distance = 1.5 * top radius when > then point.z = center.z
                    var penetrationPointsCorrected = new List<Vector3Class>();
                    for (var bottomRadiusPointIndex = 0; bottomRadiusPointIndex < bottomRadiusPoints.Count; bottomRadiusPointIndex++)
                    {
                        var penetrationPoint = bottomRadiusPoints[bottomRadiusPointIndex] - supportEnginePenetrationDepthVector;// + new Vector3(0, 0, this.Position.Z);
                        penetrationPointsCorrected.Add(penetrationPoint);
                    }
                    bottomRadiusPoints = penetrationPointsCorrected;
                }
            }
            else
            {
                var penetrationPointsCorrected = new List<Vector3Class>();
                for (var bottomRadiusPointIndex = 0; bottomRadiusPointIndex < bottomRadiusPoints.Count; bottomRadiusPointIndex++)
                {
                    var penetrationPoint = bottomRadiusPoints[bottomRadiusPointIndex] - supportEnginePenetrationDepthVector;// + new Vector3(0, 0, this.Position.Z);
                    penetrationPointsCorrected.Add(penetrationPoint);
                }
                bottomRadiusPoints = penetrationPointsCorrected;
            }

            //lowest point of bottomradiuspoints
            var upperBottomCapCenterPoint = new Vector3Class();
            bottomCapCenterPoint = new Vector3Class(0, 0, 1000);
            foreach (var bottomRadiusPoint in bottomRadiusPoints)
            {
                bottomCapCenterPoint.Z = Math.Min(bottomCapCenterPoint.Z, bottomRadiusPoint.Z);
                upperBottomCapCenterPoint.Z = Math.Max(upperBottomCapCenterPoint.Z, bottomRadiusPoint.Z);
            }

            bottomCapCenterPoint.Z += (upperBottomCapCenterPoint.Z - bottomCapCenterPoint.Z) / 2;
            bottomCapCenterPoint += this.Position;
            //  bottomCapCenterPoint = new Vector3();

            var middleRadiusPoints = VectorHelper.GetCircleOutlinePoints(this.BottomHeight, this.MiddleRadius, this._slicesCount, this.Position);
            var middleRadiusPointsWithHeightCorrection = VectorHelper.GetCircleOutlinePoints(this.BottomHeight, this.MiddleRadius + ((_totalHeight / 100) * bottomWidthCorrection), this._slicesCount, this.Position);
            if (!_groundSupport)
            {
                middleRadiusPointsWithHeightCorrection = middleRadiusPoints;
            }

            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = topRadiusPoints[middleRadiusPointIndex] + this.Position + supportEnginePenetrationDepthVector;
                leftTriangle.Vectors[1].Position = middleRadiusPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    leftTriangle.Vectors[2].Position = middleRadiusPoints[0] + topCapOffset;
                }
                else
                {
                    leftTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex + 1] + topCapOffset;
                }

                this.Triangles[0].Add(leftTriangle);

                //right triangle
                rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = topRadiusPoints[middleRadiusPointIndex] + this.Position + supportEnginePenetrationDepthVector;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = middleRadiusPoints[0] + topCapOffset;
                    rightTriangle.Vectors[2].Position = topRadiusPoints[0] + this.Position + supportEnginePenetrationDepthVector;

                }
                else
                {
                    rightTriangle.Vectors[1].Position = middleRadiusPoints[middleRadiusPointIndex + 1] + topCapOffset;
                    rightTriangle.Vectors[2].Position = topRadiusPoints[middleRadiusPointIndex + 1] + this.Position + supportEnginePenetrationDepthVector;
                }

                this.Triangles[0].Add(rightTriangle);
            }
            #endregion


            //create bottom cone
            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = middleRadiusPointsWithHeightCorrection[middleRadiusPointIndex];
                leftTriangle.Vectors[1].Position = bottomRadiusPoints[middleRadiusPointIndex] + this.Position;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    leftTriangle.Vectors[2].Position = bottomRadiusPoints[0] + this.Position;
                }
                else
                {
                    leftTriangle.Vectors[2].Position = bottomRadiusPoints[middleRadiusPointIndex + 1] + this.Position;
                }

                this.Triangles[0].Add(leftTriangle);

                //right triangle
                rightTriangle = new Triangle();

                rightTriangle.Vectors[0].Position = middleRadiusPointsWithHeightCorrection[middleRadiusPointIndex];

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = bottomRadiusPoints[0] + this.Position;
                    rightTriangle.Vectors[2].Position = middleRadiusPointsWithHeightCorrection[0];
                }
                else
                {
                    rightTriangle.Vectors[1].Position = bottomRadiusPoints[middleRadiusPointIndex + 1] + this.Position;
                    rightTriangle.Vectors[2].Position = middleRadiusPointsWithHeightCorrection[middleRadiusPointIndex + 1];
                }

                this.Triangles[0].Add(rightTriangle);
            }

            //create middle cylinder
            for (var middleRadiusPointIndex = 0; middleRadiusPointIndex < middleRadiusPoints.Count; middleRadiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = middleRadiusPointsWithHeightCorrection[middleRadiusPointIndex];
                leftTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    leftTriangle.Vectors[1].Position = middleRadiusPointsWithHeightCorrection[0];
                }
                else
                {
                    leftTriangle.Vectors[1].Position = middleRadiusPointsWithHeightCorrection[middleRadiusPointIndex + 1];
                }

                this.Triangles[0].Add(leftTriangle);

                //right triangle
                rightTriangle = new Triangle();
                rightTriangle.Vectors[0].Position = middleRadiusPoints[middleRadiusPointIndex] + topCapOffset;

                if (middleRadiusPointIndex == middleRadiusPoints.Count - 1)
                {
                    rightTriangle.Vectors[1].Position = middleRadiusPointsWithHeightCorrection[0];
                    rightTriangle.Vectors[2].Position = middleRadiusPoints[0] + topCapOffset;

                }
                else
                {
                    rightTriangle.Vectors[1].Position = middleRadiusPointsWithHeightCorrection[middleRadiusPointIndex + 1];
                    rightTriangle.Vectors[2].Position = middleRadiusPoints[middleRadiusPointIndex + 1] + topCapOffset;

                }

                this.Triangles[0].Add(rightTriangle);
            }

            //create caps
            CreateCap(topRadiusPoints, topCapCenterPoint, supportEnginePenetrationDepthVector, true);
            CreateCap(bottomRadiusPoints, bottomCapCenterPoint, supportEnginePenetrationDepthVector, false);

            foreach (var triangle in this.Triangles[0])
            {
                triangle.CalcCenter();
                triangle.CalcNormal();
            }

            
            this.UpdateBoundries();

            //set penetrationenabled
            if (useSupportPenetration && !this.InvalidPenetrationPoints)
            {
                this.PenetrationEnabled = true;
            }

            CalcModelSupportIntersectionWarning(middleRadiusPointsWithHeightCorrection, bottomCapCenterPoint, topCapOffset);
        }

        private void CalcModelSupportIntersectionWarning(List<Vector3Class> middleRadiusPoints, Vector3Class bottomPoint, Vector3Class topCapOffset)
        {
         //   DebugLines.Clear();

            var modelSupportIntersectionPoints = new List<Vector3Class>();

            //determine model intersection logic
            var directionVectors = new List<Vector3Class>() { new Vector3Class(0, 0, 1), new Vector3Class(0, 0, -1) };
            foreach (var middleRadiusPoint in middleRadiusPoints)
            {
                foreach (var directionVector in directionVectors)
                {
                    TriangleIntersection[] intersectedTriangles = null;

                    IntersectionProvider.IntersectTriangle(new Vector3Class(middleRadiusPoint.Xy), directionVector, ModelTrianglesWithinBoundryAsStl, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out intersectedTriangles);

                    if (intersectedTriangles != null)
                    {
                        foreach (var intersectedTriangle in intersectedTriangles)
                        {
                            if (intersectedTriangle.IntersectionPoint.Z >= bottomPoint.Z + this.BottomHeight && intersectedTriangle.IntersectionPoint.Z <= this.ModelIntersectionPoint.Z - this.TopHeight)
                            {
                                this.HasModelIntersection = true;
                                //DebugLines.Add(intersectedTriangle.IntersectionPoint);
                            }
                        }
                    }
                }
            }
        }

        internal TriangleInfoList CreateCap(List<Vector3Class> radiusPoints, Vector3Class middlePoint, Vector3Class supportEnginePenetrationDepthVector, bool upwards)
        {
            var triangles = new TriangleInfoList();
            var leftTriangle = new Triangle();
            var rightTriangle = new Triangle();
            if (!upwards)
            {
                supportEnginePenetrationDepthVector = new Vector3Class();
            }

            for (var radiusPointIndex = 0; radiusPointIndex < radiusPoints.Count; radiusPointIndex++)
            {
                //create left triagle
                leftTriangle = new Triangle();

                leftTriangle.Vectors[0].Position = middlePoint;
                leftTriangle.Vectors[1].Position = radiusPoints[radiusPointIndex] + this.Position + supportEnginePenetrationDepthVector;

                if (radiusPointIndex == radiusPoints.Count - 1)
                {
                    leftTriangle.Vectors[2].Position = radiusPoints[0] + this.Position + supportEnginePenetrationDepthVector;
                }
                else
                {
                    leftTriangle.Vectors[2].Position = radiusPoints[radiusPointIndex + 1] + this.Position + supportEnginePenetrationDepthVector;
                }

                if (!upwards)
                {
                    leftTriangle.Flip(true);
                }

                this.Triangles[0].Add(leftTriangle);
                triangles[0].Add(leftTriangle);
            }

            return triangles;
        }

        public SupportCone Clone(STLModel3D stlModel)
        {
            var supportConeClone = new SupportCone();

            supportConeClone._bottomHeight = this._bottomHeight;
            supportConeClone._bottomRadius = this._bottomRadius;
            supportConeClone._color = Color.FromArgb(stlModel.ColorAsByte4.A + 100, this._color);
            supportConeClone._groundSupport = this._groundSupport;
            supportConeClone._middleRadius = this._middleRadius;
            supportConeClone._topHeight = this._topHeight;
            supportConeClone._topRadius = this._topRadius;
            supportConeClone._totalHeight = this._totalHeight;
            supportConeClone._slicesCount = this._slicesCount;
            supportConeClone.Position = this.Position;

            //triangles
            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                if (supportConeClone.Triangles.Count == arrayIndex)
                {
                    supportConeClone.Triangles.Add(new List<Triangle>());
                }

                for (var triangleIndex = 0; triangleIndex < this.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    supportConeClone.Triangles[arrayIndex].Add((Triangle)this.Triangles[arrayIndex][triangleIndex].Clone());
                    supportConeClone.Triangles[arrayIndex][triangleIndex].Vectors[0].Color = supportConeClone.Triangles[arrayIndex][triangleIndex].Vectors[1].Color = supportConeClone.Triangles[arrayIndex][triangleIndex].Vectors[2].Color = this.ColorAsByte4;
                }
            }
            return supportConeClone;
        }

        internal STLModel3D Model
        {
            get
            {
                foreach (var object3d in ModelView.ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D)
                    {
                        var stlModel = object3d as STLModel3D;
                        foreach (var supportCone in stlModel.TotalObjectSupportCones)
                        {
                            if (supportCone == this)
                            {
                                return stlModel;
                            }
                        }

                        //check by index id
                        if (stlModel.Index == this.Index - 100)
                        {
                            return stlModel;
                        }
                    }
                }

                return null;
            }
        }

        internal new void Remove()
        {
            if (this.Model != null)
            {
                this.Model.SupportStructure.Remove(this);
            }
        }


        internal void RotateAxisZ(Matrix4 rotationMatrix)
        {
            this.Position = Vector3Class.Transform(this.Position, rotationMatrix);

            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this.Triangles[arrayIndex].Count; triangleIndex++)
                {

                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X = rotatedVector.X;
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y = rotatedVector.Y;
                        this.Triangles[arrayIndex][triangleIndex].CalcCenter();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxX();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxY();

                        this.Triangles[arrayIndex][triangleIndex].CalcNormal();
                    }
                }
            }

            this.UpdateBoundries();
        }

        internal void UpdateHeight(Vector3 intersectionPoint, STLModel3D model, bool useSupportPenetration, float groundDistance)
        {
            var supportEngineBasementThicknessVector = new Vector3(0, 0, UserProfileManager.UserProfile.SupportEngine_Basement_Thickness);

            var groundDistanceCorrection = groundDistance;
            this._totalHeight = intersectionPoint.Z;

            if (model.SupportBasement && this._groundSupport)
            {
                this._totalHeight -= supportEngineBasementThicknessVector.Z;
                groundDistanceCorrection = supportEngineBasementThicknessVector.Z;
            }

            this.Position = new Vector3Class(intersectionPoint.X, intersectionPoint.Y, groundDistanceCorrection);
            this.CreateSupportCone(position: this.Position, model: model, groundSupport: this._groundSupport, useSupportPenetration: useSupportPenetration, supportConeType: this.SupportConeType);
            this.Position = new Vector3Class(intersectionPoint.X, intersectionPoint.Y, groundDistanceCorrection);
            this.MoveTranslation = new Vector3Class(this.MoveTranslation.X, this.MoveTranslation.Y, 0);
            this.UpdateBoundries();
        }

        internal void Update(Events.SupportEventArgs e, STLModel3D model)
        {
            var supportEngineBasementThicknessVector = new Vector3(0, 0, UserProfileManager.UserProfile.SupportEngine_Basement_Thickness);

            try
            {
                this.InvalidPenetrationPoints = false;

                if (e.BottomHeight != 0) this._bottomHeight = e.BottomHeight;
                if (e.BottomRadius != 0)
                {
                    this._bottomRadius = e.BottomRadius;
                    this.CalcModelIntersectionTriangles(this.Position, model, null);
                }

                if (this.BottomPoint > 0 && this.BottomPoint != 1 && !this._groundSupport)
                {
                    this._bottomRadius = e.TopRadius;
                }

                if (e.MiddleRadius != 0) this._middleRadius = e.MiddleRadius;
                if (e.TopHeight != 0) this._topHeight = e.TopHeight;
                if (e.TopRadius != 0)
                {
                    this._topRadius = e.TopRadius;
                    this.CalcModelIntersectionTriangles(this.Position, model, null);
                }

                if (this._groundSupport && model.SupportBasement)
                {
                    this.Position = new Vector3Class(this.Position.X, this.Position.Y, supportEngineBasementThicknessVector.Z);
                }
                else if (this._groundSupport && !model.SupportBasement)
                {
                    this.Position = new Vector3Class(this.Position.X, this.Position.Y, 0);
                }

                this.BottomWidthCorrection = e.BottomWidthCorrection;

                this.UpdateSupportCone();
                this.UpdateBoundries();

            }
            catch (Exception exc)
            {
                DAL.Managers.LoggingManager.WriteToLog("Support Manager", "Update()", exc.Message);
            }
        }

        internal void CalcModelIntersectionTriangles(Vector3Class topCapCenterPoint, STLModel3D model, STLModel3D surfaceFaces)
        {
            if (this.ModelTrianglesWithinBoundryAsStl == null)
            {
                this.ModelTrianglesWithinBoundryAsStl = new STLModel3D();
                this.ModelTrianglesWithinBoundryAsStl.Triangles = new TriangleInfoList();
                this.ModelTrianglesWithinBoundryAsStl.Triangles.Add(new List<Triangle>());
            }
            else
            {
                this.ModelTrianglesWithinBoundryAsStl.Triangles.Clear();
                this.ModelTrianglesWithinBoundryAsStl.Triangles.Add(new List<Triangle>());
            }


            var modelTriangleToUse = model;
            if (surfaceFaces != null)
            {
                modelTriangleToUse = surfaceFaces;
            }

            var boundryOffset = Math.Max(this.BottomRadius, this.TopRadius) * 1.2f;
            var modelTrianglesLeftBoundry = this.Position.X - boundryOffset;
            var modelTrianglesRightBoundry = this.Position.X + boundryOffset;
            var modelTrianglesBackBoundry = this.Position.Y - boundryOffset;
            var modelTrianglesFrontBoundry = this.Position.Y + boundryOffset;

            if (modelTriangleToUse != null)
            {
                this.ModelTrianglesWithinBoundryAsStl.Triangles = modelTriangleToUse.Triangles.GetTrianglesWithinXYBoundry(modelTrianglesLeftBoundry, modelTrianglesBackBoundry, modelTrianglesRightBoundry, modelTrianglesFrontBoundry);
            }
        }

        internal virtual void Draw(Color color, Color selectedSupportConeColor, Color modelIntersectionColor, bool selectionColorEnabled = true,  bool gridSupport = false)
        {
            var drawColor = color;
            if (drawColor == null)
            {
                drawColor = this.Color;
            }

            if (!this.Hidden)
            {
                var moveTranslation = this.MoveTranslation;
                var supportConeColor = drawColor;

                GL.Translate(new Vector3(moveTranslation.X, moveTranslation.Y, 0));

                if (selectionColorEnabled)
                {
                    if (this.Selected || gridSupport)
                    {
                        supportConeColor = Color.FromArgb(this.Color.A, selectedSupportConeColor);
                    }
                    else if (this.HasModelIntersection)
                    {
                        supportConeColor = Color.FromArgb(this.Color.A, modelIntersectionColor);
                    }
                    else
                    {
                        supportConeColor = Color.FromArgb(this.Color.A, drawColor);
                    }
                }
                else
                {
                    supportConeColor = Color.FromArgb(this.Color.A, this._color);
                }

                GL.Color4(supportConeColor);
                GL.Begin(PrimitiveType.Triangles);

                foreach (var triange in this.Triangles[0])
                {
                    if (float.IsNaN(triange.Normal.X))
                    {
                        triange.CalcNormal();
                    }
                    GL.Normal3(triange.Normal.ToStruct());
                    foreach (var point in triange.Points)
                    {
                        GL.Vertex3((point + this.MoveTranslation).ToStruct());
                    }
                }
                GL.End();

                GL.Translate(-new Vector3(moveTranslation.X, moveTranslation.Y, 0));
            }
        }

        internal List<Triangle> SupportConeWithoutBottom
        {
            get
            {
                //bottom triangles 
                var result = new List<Triangle>();

                if (this.Triangles != null && this.Triangles[0].Count > 0)
                {
                    //top
                    for (var i = 0; i < 32; i++)
                    {
                        result.Add(this.Triangles[0][i]);
                    }

                    //top cap
                    for (var i = 128; i < this.Triangles[0].Count; i++)
                    {
                        result.Add(this.Triangles[0][i]);
                    }
                }

                foreach (var triangle in result)
                {
                    triangle.CalcNormal();
                }

                return result;
            }


        }

        internal static object UpdateHeightLock;

        internal void UpdateHeight(float newDeltaHeight)
        {
            if (this._groundSupport && newDeltaHeight != 0f)
            {
                if (UpdateHeightLock == null)
                {
                    UpdateHeightLock = new object();
                }

                lock (UpdateHeightLock)
                {
                    var deltaHeight = newDeltaHeight;
                    this._totalHeight += newDeltaHeight;
                    this.Position = new Vector3Class(this.Position.X, this.Position.Y, 0);

                    //update top cone
                    if (this.Triangles[0].Count > 96)
                    {
                        for (var triangleIndex = 0; triangleIndex < 32; triangleIndex++)
                        {
                            this.Triangles[0][triangleIndex].Vectors[0].Position.Z += deltaHeight;
                            this.Triangles[0][triangleIndex].Vectors[1].Position.Z += deltaHeight;
                            this.Triangles[0][triangleIndex].Vectors[2].Position.Z += deltaHeight;
                        }

                        //update middle cone
                        for (var triangleIndex = 64; triangleIndex < 96; triangleIndex++)
                        {
                            if (triangleIndex % 2 == 0) //right triangle
                            {
                                this.Triangles[0][triangleIndex].Vectors[2].Position.Z += deltaHeight;
                            }
                            else //left triangle
                            {
                                this.Triangles[0][triangleIndex].Vectors[0].Position.Z += deltaHeight;
                                this.Triangles[0][triangleIndex].Vectors[2].Position.Z += deltaHeight;
                            }
                        }

                        //top cap 96-112
                        for (var triangleIndex = 96; triangleIndex < 112; triangleIndex++)
                        {
                            this.Triangles[0][triangleIndex].Vectors[0].Position.Z += deltaHeight;
                            this.Triangles[0][triangleIndex].Vectors[1].Position.Z += deltaHeight;
                            this.Triangles[0][triangleIndex].Vectors[2].Position.Z += deltaHeight;
                        }

                        //middle cap 128-144
                        if (this.Triangles[0].Count > 144)
                        {
                            for (var triangleIndex = 128; triangleIndex < 144; triangleIndex++)
                            {
                                this.Triangles[0][triangleIndex].Vectors[0].Position.Z += deltaHeight;
                                this.Triangles[0][triangleIndex].Vectors[1].Position.Z += deltaHeight;
                                this.Triangles[0][triangleIndex].Vectors[2].Position.Z += deltaHeight;
                            }
                        }
                    }

                    this.UpdateBoundries();

                    this.Hidden = this.TopPoint <= UserProfileManager.UserProfiles[0].SupportEngine_Penetration_Depth + 1f && this._groundSupport;
                }
            }
            else if (!_groundSupport && newDeltaHeight != 0f)
            {
                if (UpdateHeightLock == null)
                {
                    UpdateHeightLock = new object();
                }

                lock (UpdateHeightLock)
                {

                    var deltaHeight = newDeltaHeight;
                    this._totalHeight = newDeltaHeight;
                    this.Position = new Vector3Class(this.Position.X, this.Position.Y, deltaHeight);

                    for (var triangleIndex = 0; triangleIndex < this.Triangles[0].Count; triangleIndex++)
                    {
                        this.Triangles[0][triangleIndex].Vectors[0].Position.Z += deltaHeight;
                        this.Triangles[0][triangleIndex].Vectors[1].Position.Z += deltaHeight;
                        this.Triangles[0][triangleIndex].Vectors[2].Position.Z += deltaHeight;
                    }
                }
            }
        }

        internal void UpdateSupportHeight()
        {
            var parentModel = this.Model;
            this.UpdateBoundries();

            TriangleIntersection[] intersectedModelTriangles = null;
            IntersectionProvider.IntersectTriangle(this.Center + this.MoveTranslation, this.Normal, parentModel, IntersectionProvider.typeDirection.OneWay, false, new Vector3Class(), out intersectedModelTriangles);

            if (intersectedModelTriangles != null && intersectedModelTriangles.Length > 0)
            {
                this.UpdateSupportHeight(intersectedModelTriangles, parentModel, true);
            }

            this.UpdateBoundries();
        }

        internal void UpdateSupportHeight(TriangleIntersection[] intersectedTriangles, STLModel3D model, bool useSupportPenetration)
        {
            if (intersectedTriangles != null)
            {
                if (intersectedTriangles.Length > 0)
                {
                    //find nearest point above support center
                    var intersectionPointDistance = float.MaxValue;
                    var intersectionPoint = new Vector3Class();

                    foreach (var intersectedTriangle in intersectedTriangles)
                    {
                        if (intersectedTriangle.IntersectionPoint != new Vector3Class())
                        {
                            if (intersectedTriangle.IntersectionPoint.Z < intersectionPointDistance)
                            {
                                intersectionPointDistance = intersectedTriangle.IntersectionPoint.Z;
                                intersectionPoint = intersectedTriangle.IntersectionPoint;
                            }
                        }
                    }

                    //get intersection point
                    UpdateHeight(new Vector3(this.Position.X, this.Position.Y, intersectionPoint.Z), model, useSupportPenetration, 0f);
                }
            }
        }

        private float _topMarkerLayerHeight = float.MinValue;
        private Cone _topMarker = null;
        private float _topMarkerRadius = float.MinValue;


        internal void DrawDebugTopPointMarker(SupportProfile supportProfile)
        {
            if (LastSupportedLayerHeight != float.MinValue)
            {

                if (this._topMarker == null || this._topMarkerLayerHeight != this.LastSupportedLayerHeight)
                {

                    this._topMarkerRadius = supportProfile.SupportOverhangDistance;
                    if (ModelIntersectionAngle > 0 && ModelIntersectionAngle < 90)
                    {
                        var overhangAngle = 1f + ( ModelIntersectionAngle / 90f);
                        _topMarkerRadius = supportProfile.SupportOverhangDistance * overhangAngle;
                        _topMarkerRadius *= 2;
                    }


                    this._topMarker = Cone.Create(1 , 0f, 1, 26, new Vector3Class(this.Position.X, this.Position.Y, this.LastSupportedLayerHeight));
                    this._topMarkerLayerHeight = this.LastSupportedLayerHeight;
                }
                
                GL.Begin(PrimitiveType.Triangles);
                GL.Color3(Color.Yellow);
              
                foreach(var topPointMarkerTriangle in this._topMarker[0])
                {
                    GL.Vertex3((topPointMarkerTriangle.Vectors[0].Position).ToStruct());
                    GL.Vertex3((topPointMarkerTriangle.Vectors[1].Position).ToStruct());
                    GL.Vertex3((topPointMarkerTriangle.Vectors[2].Position).ToStruct());
                }
                 
                GL.End();
            }
        }

    }
}
