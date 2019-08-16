using Atum.Studio.Core.Helpers;
using OpenTK;
using System.Collections.Generic;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Objects
{
    internal class SliceContourInfo
    {

        public List<ContourHelper.IntPoint> OuterPath { get; set; }
        public SliceContourInfo ParentContour { get; private set; }
        public PolyTree PolyTree { get; set; }

        public List<SliceContourInfo> ChildContourTree { get; set; }

        public List<Vector3> OuterPoints
        {
            get
            {
                var result = new List<Vector3>();
                foreach (var t in this.OuterPath)
                {
                    result.Add(new Vector3(t.X / 10000f, t.Y / 10000f, t.Z / 10000f));
                }
                return result;
            }
        }


        //public List<Vector2[]> Skeleton { get; set; }

        public SliceContourInfo()
        {
            this.OuterPath = new List<ContourHelper.IntPoint>();
            this.ChildContourTree = new List<SliceContourInfo>();
            //this.Skeleton = new List<Vector2[]>();
        }

        public void CalcChildContourTree(float maxSupportConeDistance)
        {
            this.ChildContourTree.Clear();

            var decimalCorrectionFactor = 10000;
            var clipper = new ContourHelper.ClipperOffset();
            var results = new List<List<ContourHelper.IntPoint>>();
            var destinationPolyTree = new ContourHelper.PolyTree();
            clipper.AddPath(this.OuterPath, ContourHelper.JoinType.jtMiter, ContourHelper.EndType.etClosedPolygon);
            clipper.Execute(ref destinationPolyTree, -maxSupportConeDistance * decimalCorrectionFactor);
            foreach (var polyNode in destinationPolyTree.Childs)
            {
                var contour = new SliceContourInfo();
                contour.OuterPath.AddRange(polyNode.Contour);
                contour.ParentContour = this;
                contour.CalcChildContourTree(maxSupportConeDistance);
                this.ChildContourTree.Add(contour);
            }
        }

        public void DrawContour(SliceContourInfo contour)
        {
            var childOuterPoints = contour.OuterPoints;
            for (var i = 0; i < childOuterPoints.Count; i++)
            {
                if (i == childOuterPoints.Count - 1)
                {
                    OpenTK.Graphics.OpenGL.GL.Vertex3(childOuterPoints[i]);
                    OpenTK.Graphics.OpenGL.GL.Vertex3(childOuterPoints[0]);
                }
                else
                {
                    OpenTK.Graphics.OpenGL.GL.Vertex3(childOuterPoints[i]);
                    OpenTK.Graphics.OpenGL.GL.Vertex3(childOuterPoints[i + 1]);
                }
            }
            foreach (var childContour in contour.ChildContourTree)
            {
                DrawContour(childContour);
            }

        }
    }
}
