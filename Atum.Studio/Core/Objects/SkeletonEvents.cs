using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class IEvent
    {
        public float Distance { get; set; }
        public Vector2 IntersectionPoint { get; set; }

        public override string ToString()
        {
            return string.Format("{0}", this.Distance);
        }
    }

    class _SplitEvent : IEvent
    {
        internal SkeletonVertex Vertex { get; set; }
        internal SkeletonLineSegment OppositeEdge { get; set; }

        internal _SplitEvent(float distance, Vector2 intersectionPoint, SkeletonVertex vertex, SkeletonLineSegment oppositeEdge)
        {
            this.Distance = distance;
            this.IntersectionPoint = intersectionPoint;
            this.Vertex = vertex;
            this.OppositeEdge = oppositeEdge;
        }
    }

    class _EdgeEvent : IEvent
    {
        internal SkeletonVertex vertex_a { get; set; }
        internal SkeletonVertex vertex_b { get; set; }
        internal SkeletonVertex opposite_edge_b { get; set; }

        internal _EdgeEvent(float distance, Vector2 intersectionPoint, SkeletonVertex vertex_a, SkeletonVertex vertex_b)
        {
            this.Distance = distance;
            this.IntersectionPoint = intersectionPoint;
            this.vertex_a = vertex_a;
            this.vertex_b = vertex_b;
        }
    }

    class SkeletonEventResult
    {
        internal SkeletonSubtree Subtree { get; set; }
        internal List<IEvent> Events { get; set; }

        internal SkeletonEventResult(SkeletonSubtree subtree, List<IEvent> events)
        {
            this.Subtree = subtree;
            this.Events = events;
        }
    }
}
