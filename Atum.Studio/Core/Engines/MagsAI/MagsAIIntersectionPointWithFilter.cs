using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAIIntersectionPointWithFilter
    {
        internal IntPoint Point { get; set; }
        internal typeOfAutoSupportFilter Filter { get; set; }

        internal MagsAISupportConeOutlinePoints CalcOutlinePoints(Material selectedMaterial)
        {
            return new MagsAISupportConeOutlinePoints(VectorHelper.GetCircleOutlinePoints(0, selectedMaterial.SupportProfiles.First().SupportOverhangDistance, 26, this.Point.AsVector3()));
        }

        internal MagsAISupportConeOutlinePoints CalcOutlinePointsUsingRadius(float radius)
        {
            return new MagsAISupportConeOutlinePoints(VectorHelper.GetCircleOutlinePoints(0, radius, 26, this.Point.AsVector3()));
        }
    }
}
