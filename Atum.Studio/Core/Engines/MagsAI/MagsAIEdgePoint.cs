using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAIEdgePoint
    {
        internal Vector2 PreviousPoint { get; set; }
        internal Vector2 EdgePoint { get; set; }
        internal Vector2 NextPoint { get; set; }
        internal float Angle { get; set; }
        internal float SliceHeight { get; set; }

        internal Vector2 ContourIntersectionPoint { get; set; }

        internal MagsAIEdgePoint()
        {
            
        }

        internal MagsAIEdgePoint(Vector2 previousPoint, Vector2 edgePoint, Vector2 nextPoint, float angle, float sliceHeight)
        {
            this.PreviousPoint = previousPoint;
            this.EdgePoint = edgePoint;
            this.NextPoint = nextPoint;

            this.Angle = angle;
            this.SliceHeight = sliceHeight;
        }

        internal List<Vector3Class> CalcOutlinePointsAsVector3(Material selectedMaterial)
        {
            return VectorHelper.GetCircleOutlinePoints(0, selectedMaterial.SupportProfiles.First().SupportInfillDistance, 26, new Vector3Class(this.EdgePoint));
        }
    }
}
