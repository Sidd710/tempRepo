using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Objects
{
    class SkeletonVerticesCollection : List<SkeletonVertices>
    {

        internal static SkeletonVertices FromPolygon(List<Vector3> polygon)
        {
            return new SkeletonVertices(polygon);
        }

        internal void Invalidate(SkeletonVertex vertex)
        {
            vertex._valid = false;
        }

        internal static SkeletonVertices FromChain(SkeletonVertex head, SkeletonVertices slav)
        {
            SkeletonVertices lav = new SkeletonVertices();
            lav.Head = head;
            lav.Add(head);

            var t = lav.Head.Next;
            lav.Add(t);
            while (t.Point != lav.Head.Point)
            {
                t = t.Next;
                lav.Add(t);
            }

            lav.RemoveAt(lav.Count - 1);
            foreach (var vertex in lav)
            {
                vertex.Vertices = lav;
            }
            return lav;
        }

        internal SkeletonEventResult Handle_Edge_Event(_EdgeEvent egdeEvent)
        {
            var sinks = new List<Vector2>();
            var events = new List<IEvent>();


            var lav = egdeEvent.vertex_a.Vertices;

            if (egdeEvent.vertex_a.Previous == egdeEvent.vertex_b.Next)
            {
                //Debug.WriteLine("{0} Peak event at intersection {1} from <{2},{3},{4}> in {5}", egdeEvent.Distance, egdeEvent.IntersectionPoint, egdeEvent.vertex_a, egdeEvent.vertex_b, egdeEvent.vertex_a.Previous, lav);

                this.Remove(lav);

                foreach (var vertex in lav)
                {
                    sinks.Add(vertex.Point);
                    vertex.Invalidate();
                }
            }
            else
            {
                // Debug.WriteLine("{0} Edge event at intersection {1} from <{2},{3}> in {4}", egdeEvent.Distance, egdeEvent.IntersectionPoint, egdeEvent.vertex_a, egdeEvent.vertex_b, lav);
                var new_vertex = lav.Unify(egdeEvent.vertex_a, egdeEvent.vertex_b, egdeEvent.IntersectionPoint);
                if ((new List<SkeletonVertex>() { egdeEvent.vertex_a, egdeEvent.vertex_b }).Contains(lav.Head))
                {
                    lav.Head = new_vertex;
                }
                sinks.AddRange(new[] { egdeEvent.vertex_a.Point, egdeEvent.vertex_b.Point });
                var next_event = new_vertex.NextEvent(lav._originalEdges);
                if (next_event != null)
                {
                    events.Add(next_event);
                }
            }

            return new SkeletonEventResult(new SkeletonSubtree(egdeEvent.IntersectionPoint, egdeEvent.Distance, sinks), events);
        }

        internal SkeletonEventResult Handle_Split_Event(_SplitEvent splitEvent)
        {
            var lav = splitEvent.Vertex.Vertices;
            //Debug.WriteLine("{0} Split event at intersection {1} from vertex {2}, for edge {3} in {4}", splitEvent.Distance, splitEvent.IntersectionPoint, splitEvent.Vertex, splitEvent.OppositeEdge, lav);

            var sinks = new List<Vector2>() { splitEvent.Vertex.Point };
            var vertices = new List<SkeletonVertex>();

            SkeletonVertex x = null;   // right vertex
            SkeletonVertex y = null;   // left vertex

            var norm = splitEvent.OppositeEdge.V.Normalized();

            foreach (var childLav in this)
            {
                foreach (var v in childLav)
                {
                    //Debug.WriteLine("{0} in {1}", v, v.LAV);

                    if (norm == v.EdgeLeft.V.Normalized() && splitEvent.OppositeEdge.P == v.EdgeLeft.P)
                    {
                        x = v;
                        y = x.Previous;
                    }
                    else if (norm == v.EdgeRight.V.Normalized() && splitEvent.OppositeEdge.P == v.EdgeRight.P)
                    {
                        y = v;
                        x = y.Next;
                    }

                    if (x != null)
                    {
                        var yIntersectionAsPoint = (new Vector2(splitEvent.IntersectionPoint.X - y.Point.X, splitEvent.IntersectionPoint.Y - y.Point.Y)).Normalized();
                        var yLeftBiSectorAsPoint = new PointF(y.BiSector.V.Normalized().X, y.BiSector.V.Normalized().Y);
                        var xleft = Helpers.MathHelper.Cross(yLeftBiSectorAsPoint, new PointF(yIntersectionAsPoint.X, yIntersectionAsPoint.Y)) >= 0;

                        var xIntersectionAsPoint = (new Vector2(splitEvent.IntersectionPoint.X - x.Point.X, splitEvent.IntersectionPoint.Y - x.Point.Y)).Normalized();
                        var xLeftBiSectorAsPoint = new PointF(x.BiSector.V.Normalized().X, x.BiSector.V.Normalized().Y);
                        var xright = Helpers.MathHelper.Cross(xLeftBiSectorAsPoint, new PointF(xIntersectionAsPoint.X, xIntersectionAsPoint.Y)) <= 0;
                        //Debug.WriteLine("Vertex {0} holds edge as {1} edge ({2}, {3})", v, (x == v ? "left" : "right"), xleft, xright);

                        if (xleft && xright)
                        {
                            break;
                        }
                        else
                        {
                            x = null;
                            y = null;
                        }
                    }
                }
            }

            if (x == null)
            {
                //Debug.WriteLine("Failed split event {0} (equivalent edge event is expected to follow)", splitEvent);
                return new SkeletonEventResult(null, null);
            }

            var v1 = new SkeletonVertex(splitEvent.IntersectionPoint, splitEvent.Vertex.EdgeLeft, splitEvent.OppositeEdge);
            var v2 = new SkeletonVertex(splitEvent.IntersectionPoint, splitEvent.OppositeEdge, splitEvent.Vertex.EdgeRight);

            v1.Previous = splitEvent.Vertex.Previous;
            v1.Next = x;

            splitEvent.Vertex.Previous.Next = v1;
            x.Previous = v1;

            v2.Previous = y;
            v2.Next = splitEvent.Vertex.Next;

            splitEvent.Vertex.Next.Previous = v2;
            y.Next = v2;

            List<SkeletonVertices> new_lavs = new List<SkeletonVertices>();

            this.Remove(lav);

            if (lav.Count != x.Vertices.Count)
            {
                this.Remove(x.Vertices);

                new_lavs.Add(SkeletonVerticesCollection.FromChain(v1, lav));
            }
            else
            {
                new_lavs.Add(SkeletonVerticesCollection.FromChain(v1, lav));
                new_lavs.Add(SkeletonVerticesCollection.FromChain(v2, lav));
            }

            foreach (var l in new_lavs)
            {
                if (l.Count > 2)
                {
                    this.Add(l);

                    vertices.Add(l.Head);
                }
                else
                {
                    sinks.Add(l.Head.Next.Point);

                    foreach (var v in l)
                    {
                        v.Invalidate();
                    }
                }
            }

            var events = new List<IEvent>();

            foreach (var vertex in vertices)
            {
                var nextEvent = vertex.NextEvent(lav._originalEdges);
                if (nextEvent != null)
                {
                    events.Add(nextEvent);
                }
            }

            splitEvent.Vertex.Invalidate();
            return new SkeletonEventResult(new SkeletonSubtree(splitEvent.IntersectionPoint, splitEvent.Distance, sinks), events);
        }
    }
}
