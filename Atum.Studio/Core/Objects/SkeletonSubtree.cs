using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonSubtree
    {
        internal Vector2 Source { get; set; }
        internal float Height { get; set; }
        internal List<Vector2> Sinks { get; set; }

        internal SkeletonSubtree(Vector2 source, float height, List<Vector2> sinks)
        {
            this.Source = source;
            this.Height = height;
            this.Sinks = sinks;
        }
    }
}
