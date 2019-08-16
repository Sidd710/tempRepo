using OpenTK;
using System.Collections.Generic;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class PolygonSupportPointCollection
    {
        internal PolyNode Polygon = new PolyNode();
        internal List<MagsAIIntersectionData> OverhangPoints = new List<MagsAIIntersectionData>();
   //     internal List<MagsAIIntersectionData> LayerBasedOverhangPoints = new List<MagsAIIntersectionData>();
        internal List<MagsAISurfaceIntersectionData> HorizontalSurfacePoints = new List<MagsAISurfaceIntersectionData>();
        internal List<MagsAIIntersectionData> EdgePoints = new List<MagsAIIntersectionData>();
        internal int SliceIndex;
        internal float SliceHeight;
    }
}
