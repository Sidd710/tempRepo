using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using OpenTK;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Helpers;
using System.Threading;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class WorkspaceSTLModel
    {
        public string FileName { get; set; }
        public string Color { get; set; }
        public byte[] Object;
        public byte[] BasementObject;
        public List<byte[]> FlatSurfaces { get; set; }
        public Vector3 MoveTranslation { get; set; }
        public float PreviousScaleFactorX { get; set; }
        public float PreviousScaleFactorY { get; set; }
        public float PreviousScaleFactorZ { get; set; }
        public float ScaleFactorX { get; set; }
        public float ScaleFactorY { get; set; }
        public float ScaleFactorZ { get; set; }
        public float RotationAngleX { get; set; }
        public float RotationAngleY { get; set; }
        public float RotationAngleZ { get; set; }
    //    public bool InternalSupport { get; set; }
        public bool CrossSupport { get; set; }
      //  public bool ZSupport { get; set; }
        public bool SupportBasement { get; set; }
        public bool AxisLocked { get; set; }
        public float SupportDistance { get; set; }
        public float GroundDistance { get; set; }
        public List<LinkedClone> LinkedClones { get; set; }

        public List<TriangleConnectionInfo> MAGSAIMarkedTriangles { get; set; }

        public List<WorkspaceSTLModelSupport> SupportStructure;
        public List<byte[]> HorizontalSurfaces { get; set; }
        public List<WorkspaceSTLModelHorizontalSurface> HorizontalSurfacesAsObjects { get; set; }
        public List<WorkspaceSTLModelFlatSurface> FlatSurfacesAsObjects { get; set; }

        public WorkspaceSTLModel()
        {
            this.MAGSAIMarkedTriangles = new List<TriangleConnectionInfo>();
            this.SupportStructure = new List<WorkspaceSTLModelSupport>();
            this.HorizontalSurfacesAsObjects = new List<WorkspaceSTLModelHorizontalSurface>();
            this.HorizontalSurfaces = new List<byte[]>();
            this.FlatSurfacesAsObjects = new List<WorkspaceSTLModelFlatSurface>();
        }
    }

    [Serializable]
    public class WorkspaceSTLModelSupport
    {
        public string Color { get; set; }
        public float TotalHeight { get; set; }
        public float TopHeight { get; set; }
        public float TopRadius { get; set; }
        public float MiddleRadius { get; set; }
        public float BottomHeight { get; set; }
        public float BottomRadius { get; set; }
        public float BottomWidthCorrection { get; set; }
        public int SlicesCount { get; set; }
        public Vector3 MoveTranslation { get; set; }
        public Vector3 Position { get; set; }
        public float RotationAngleX { get; set; }
        public float RotationAngleZ { get; set; }
        public bool SupportPenetrationEnabled { get; set; }
    }

    [Serializable]
    public class WorkspaceSTLModelContour
    {
        public float SupportConeTopHeight { get; set; }
        public float SupportConeTopRadius { get; set; }
        public float SupportConeMiddleRadius { get; set; }
        public float SupportConeBottomHeight { get; set; }
        public float SupportConeBottomRadius { get; set; }

        public float OutlineDistanceFactor { get; set; }
        public float OutlineOffsetDistance { get; set; }
        public float InfillOffsetDistance { get; set; }
        public float InfillDistanceFactor { get; set; }

        public List<Vector3> OuterPoints { get; set; }
        public List<Helpers.ContourHelper.IntPoint> OuterPath { get; set; }
        public List<List<Helpers.ContourHelper.IntPoint>> InnerPaths { get; set; }
        public List<Vector3> OuterSupportPoints { get; set; }
        public List<List<Vector3>> InnerPoints { get; set; }

        public WorkspaceSTLModelContour InfillContour { get; set; }

        public WorkspaceSTLModelContour()
        {
            this.OuterSupportPoints = new List<Vector3>();
            this.OuterPoints = new List<Vector3>();
            this.OuterPath = new List<Helpers.ContourHelper.IntPoint>();

            this.InnerPaths = new List<List<Helpers.ContourHelper.IntPoint>>();
            this.InnerPoints = new List<List<Vector3>>();
        }
    }

    [Serializable]
    public class WorkspaceSTLModelHorizontalSurfaceIndex
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }
    }

    [Serializable]
    public class WorkspaceSTLModelFlatSurfaceIndex
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }
    }

    [Serializable]
    public class WorkspaceSTLModelHorizontalSurface
    {
        public List<WorkspaceSTLModelHorizontalSurfaceIndex> Indexes { get; set; }
        public List<WorkspaceSTLModelSupport> SupportStructure { get; set; }

        public float SupportDistance { get; set; }
        public bool CrossSupport { get; set; }

        //public float MinX { get; set; }
        //public float MaxX { get; set; }

        //public float MinY { get; set; }
        //public float MaxY { get; set; }

        public float BottomPoint { get; set; }
        public float TopPoint { get; set; }
        public float RightPoint { get; set; }
        public float LeftPoint { get; set; }
        public float FrontPoint { get; set; }
        public float BackPoint { get; set; }

        public bool HasEdgeDown { get; set; }
        public List<Vector3> SupportPoints { get; set; }

        public WorkspaceSTLModelHorizontalSurface()
        {
            this.SupportStructure = new List<WorkspaceSTLModelSupport>();
            this.Indexes = new List<WorkspaceSTLModelHorizontalSurfaceIndex>();
        }
    }

    [Serializable]
    public class WorkspaceSTLModelFlatSurface
    {
        public List<WorkspaceSTLModelFlatSurfaceIndex> Indexes { get; set; }
        public List<WorkspaceSTLModelSupport> SupportStructure { get; set; }

        public float SupportDistance { get; set; }
        public bool CrossSupport { get; set; }

        //public float MinX { get; set; }
        //public float MaxX { get; set; }

        //public float MinY { get; set; }
        //public float MaxY { get; set; }

        public float BottomPoint { get; set; }
        public float TopPoint { get; set; }
        public float RightPoint { get; set; }
        public float LeftPoint { get; set; }
        public float FrontPoint { get; set; }
        public float BackPoint { get; set; }

        public bool HasEdgeDown { get; set; }
        public List<Vector3> SupportPoints { get; set; }

        public WorkspaceSTLModelFlatSurface()
        {
            this.SupportStructure = new List<WorkspaceSTLModelSupport>();
            this.Indexes = new List<WorkspaceSTLModelFlatSurfaceIndex>();
        }
    }
}
