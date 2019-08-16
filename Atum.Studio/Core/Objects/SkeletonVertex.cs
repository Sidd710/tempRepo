using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Atum.Studio.Core.Objects
{
    class SkeletonVertex
    {
        public SkeletonRay2 BiSector { get; set; }
        public bool Is_Reflex { get; set; }
        internal bool _valid;
        public SkeletonLineSegment EdgeLeft { get; set; }
        public SkeletonLineSegment EdgeRight { get; set; }

        public SkeletonVertices Vertices { get; set; }
        public SkeletonVertex Next { get; set; }
        public SkeletonVertex Previous { get; set; }
        public Vector2 Point { get; set; }

        List<IEvent> _events = new List<IEvent>();

        internal SkeletonVertex(Vector2 p, SkeletonLineSegment l1, SkeletonLineSegment l2, Vector2[] direction_vectors = null)
        {
            this.EdgeLeft = l1;
            this.EdgeRight = l2;
            this.Point = p;
            this._valid = true;

            var creator_vectors = new Vector2[] { l1.V.Normalized() * -1, l2.V.Normalized() };
            var reflex_vectors = direction_vectors;

            if (direction_vectors == null)
            {
                reflex_vectors = creator_vectors;
            }

            var cross = Vector3.Cross(new Vector3(reflex_vectors[0]), new Vector3(reflex_vectors[1])).Z;
            if (cross < 0)
            {
                this.Is_Reflex = true;
            }

            this.BiSector = new SkeletonRay2(this.Point, Vector2.Add(creator_vectors[0], creator_vectors[1]) * (this.Is_Reflex ? -1 : 1));
            this.Vertices = new SkeletonVertices();
        }

        public void Invalidate()
        {

            this._valid = false;
        }

        public bool IsValid
        {
            get
            {
                return this._valid;
            }
        }

        public IEvent NextEvent(List<SkeletonEdge> edges, int l = 0)
        {
            if (this.Is_Reflex)
            {
                foreach (var edge in edges)
                {
                    if (edge.Edge == this.EdgeLeft || edge.Edge == this.EdgeRight)
                    {
                        continue;
                    }

                    if (l == 270)
                    {

                    }

                    var leftDot = Math.Abs(Vector2.Dot(this.EdgeLeft.V.Normalized(), edge.Edge.V.Normalized()));
                    var rightDot = Math.Abs(Vector2.Dot(this.EdgeRight.V.Normalized(), edge.Edge.V.Normalized()));

                    var selfEdge = leftDot < rightDot ? this.EdgeLeft : this.EdgeRight;
                    var otherEdge = leftDot > rightDot ? this.EdgeLeft : this.EdgeRight;
                    var i = new SkeletonLine2(selfEdge).Intersect(edge.Edge);

                    if (i != Vector2.Zero && !ApproximatelyEquals(i, this.Point))
                    {
                        //# locate candidate b
                        var linvec = (this.Point - i).Normalized();
                        var edvec = edge.Edge.V.Normalized();

                        if (Vector2.Dot(linvec, edvec) < 0)
                        {
                            edvec = -edvec;
                        }

                        var bisecvec = edvec + linvec;

                        if (Helpers.MathHelper.Abs(bisecvec) == 0)
                        {
                            continue;
                        }

                        var bisector = new SkeletonLine2(i, bisecvec);

                        var b = bisector.Intersect(this.BiSector);

                        // Console.WriteLine("Intersection B:" + b);
                        if (b == Vector2.Zero)
                        {
                            continue;
                        }

                        if (l == 270)
                        {

                        }

                        //# check eligibility of b
                        //# a valid b should lie within the area limited by the edge and the bisectors of its two vertices:
                        var xleft = Helpers.MathHelper.Cross(edge.BiSector_Left.V.Normalized(), (b - edge.BiSector_Left.P).Normalized()) > 0;
                        var xright = Helpers.MathHelper.Cross(edge.BiSector_Right.V.Normalized(), (b - edge.BiSector_Right.P).Normalized()) < 0;
                        var xedge = Helpers.MathHelper.Cross(edge.Edge.V.Normalized(), (b - edge.Edge.P).Normalized()) < 0;

                        if (!(xleft && xright && xedge))
                        {
                            continue;
                        }

                        _events.Add(new _SplitEvent(new SkeletonLine2(edge.Edge).Distance(b), b, this, edge.Edge));
                    }
                }
            }

            var i_prev = this.BiSector.Intersect(this.Previous.BiSector);
            var i_next = this.BiSector.Intersect(this.Next.BiSector);


            if (i_prev != Vector2.Zero)
            {
                _events.Add(new _EdgeEvent(new SkeletonLine2(this.EdgeLeft).Distance(i_prev), i_prev, this.Previous, this));
            }

            if (i_next != Vector2.Zero)
            {
                _events.Add(new _EdgeEvent(new SkeletonLine2(this.EdgeRight).Distance(i_next), i_next, this, this.Next));
            }

            if (_events.Count > 0)
            {
                var minEv = float.MaxValue;
                IEvent minLineSegment = null;
                foreach (var item in _events)
                {
                    var distance = Helpers.MathHelper.Distance(this.Point, item.IntersectionPoint);
                    if (distance < minEv)
                    {
                        minEv = distance;
                        minLineSegment = item;
                    }
                }

                _events.Clear();

                //Debug.WriteLine("Generated new event for {0}: {1}", this, minLineSegment);
                return minLineSegment;
            }

            return null;


            bool ApproximatelyEquals(Vector2 a, Vector2 b)
            {
                return a == b || (Helpers.MathHelper.Abs(a - b) <= Math.Max(Helpers.MathHelper.Abs(a), Helpers.MathHelper.Abs(b)) * 0.001);
            }

        }

        public override string ToString()
        {
            return string.Format("{0},{1}", this.Point.X, this.Point.Y);
        }
    }

    class SkeletonVertices : List<SkeletonVertex>
    {
        public SkeletonVertex Head { get; set; }
        public List<List<PointF>> Contours = new List<List<PointF>>();
        public List<SkeletonEdge> _originalEdges = new List<SkeletonEdge>();

        internal SkeletonVertices()
        {

        }

        internal SkeletonVertices(List<Vector3> polygon)
        {

            for (var pointIndex = 0; pointIndex < polygon.Count; pointIndex++)
            {
                var previousPoint = new Vector2();
                var currentPoint = new Vector2(polygon[pointIndex].X, polygon[pointIndex].Y);
                var nextPoint = new Vector2();

                if (pointIndex == 0)
                {
                    nextPoint = polygon[pointIndex + 1].Xy;
                    previousPoint = polygon.Last().Xy;
                }
                else if (pointIndex == polygon.Count - 1)
                {
                    nextPoint = polygon[0].Xy;
                    previousPoint = polygon[pointIndex - 1].Xy;
                }
                else
                {
                    nextPoint = polygon[pointIndex + 1].Xy;
                    previousPoint = polygon[pointIndex - 1].Xy;
                }

                var vertex = new SkeletonVertex(currentPoint, new SkeletonLineSegment(previousPoint, currentPoint), new SkeletonLineSegment(currentPoint, nextPoint));

                if (this.Head == null)
                {
                    this.Head = vertex;
                    vertex.Previous = vertex.Next = vertex;
                }
                else
                {
                    vertex.Next = this.Head;
                    vertex.Previous = this.Head.Previous;
                    vertex.Previous.Next = vertex;
                    this.Head.Previous = vertex;
                }

                this.Add(vertex);

                foreach (var item in this)
                {
                    item.Vertices = this;
                }
            }
        }

        internal SkeletonVertex Unify(SkeletonVertex vertex_a, SkeletonVertex vertex_b, Vector2 point)
        {

            var replacement = new SkeletonVertex(point, vertex_a.EdgeLeft, vertex_b.EdgeRight, new Vector2[] { vertex_b.BiSector.V.Normalized(), vertex_a.BiSector.V.Normalized() });
            replacement.Vertices = this;
            if ((new List<SkeletonVertex>() { vertex_a, vertex_b }).Contains(this.Head))
            {
                this.Head = replacement;
            }
            
            vertex_a.Previous.Next = replacement;
            vertex_b.Next.Previous = replacement;

            replacement.Previous = vertex_a.Previous;
            replacement.Next = vertex_b.Next;

            this.Remove(vertex_a);
            this.Remove(vertex_b);
            this.Add(replacement);

            foreach (var v in this)
            {
                v.Vertices = this;
            }

            vertex_a.Invalidate();
            vertex_b.Invalidate();

            return replacement;
        }
    }
}
