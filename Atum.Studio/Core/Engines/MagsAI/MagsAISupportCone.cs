using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAISupportCone
    {
        public Vector3 TopPoint { get; set; }

        public MagsAISupportCone(Vector3 topPoint)
        {
            this.TopPoint = topPoint;
        }
    }
}
