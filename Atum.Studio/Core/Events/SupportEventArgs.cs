using Atum.Studio.Core.Models;
using System;

namespace Atum.Studio.Core.Events
{
    internal class SupportEventArgs: EventArgs
    {
        internal float TopHeight { get; set; }
        internal float TopRadius { get; set; }
        internal float MiddleRadius { get; set; }
        internal float BottomHeight { get; set; }
        internal float BottomRadius { get; set; }
        internal float BottomWidthCorrection { get; set; }
        internal bool IntermediarySupport { get; set; }
        internal STLModel3D SurfaceTriangles { get; private set; }

        internal SupportEventArgs()
        {
            this.SurfaceTriangles = new STLModel3D();
            this.SurfaceTriangles.Triangles = new Shapes.TriangleInfoList();
        }
    }
}
