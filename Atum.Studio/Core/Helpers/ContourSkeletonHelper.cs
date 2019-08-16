using Atum.Studio.Core.Objects;
using OpenTK;
using System.Collections.Generic;

namespace Atum.Studio.Core.Helpers
{
    class ContourSkeletonHelper
    {
        static SkeletonPriorityQueue<IEvent> _eventQueue = new SkeletonPriorityQueue<IEvent>();
        static SkeletonVerticesCollection _verticesCollection = new SkeletonVerticesCollection();

        internal static List<Vector2[]> Skeletonize(List<Vector3> polygon)
        {
            var result = new List<Vector2[]>();
            var k2 = new List<Vector3>();
            foreach(var t in polygon)
            {
                k2.Add(new Vector3((int)t.X, (int)t.Y, 0));
            }
            var vertices = SkeletonVerticesCollection.FromPolygon(k2);
            _verticesCollection.Add(vertices);
            foreach (var vertex in vertices)
            {
                vertices._originalEdges.Add(new SkeletonEdge(new SkeletonLineSegment(vertex.Previous.Point, vertex.Point), vertex.Previous.BiSector, vertex.BiSector));
            }

            var k = 0;
            foreach (var _lav in _verticesCollection)
            {
                foreach (var vertex in _lav)
                {
                    k++;
                    var t = vertex.NextEvent(vertices._originalEdges, k);

                    if (k == 270)
                    {

                    }
                    if (t != null)
                    {
                        _eventQueue.Push(t);
                        _eventQueue.Siftdown(0, _eventQueue.Count - 1);
                    }
                }
            }

            var e = new List<SkeletonSubtree>();
            while (_eventQueue.Count > 0)
            {
                var i = _eventQueue.Pop();

                if (i is _SplitEvent)
                {
                    var splitEvent = (_SplitEvent)i;
                    if (!splitEvent.Vertex.IsValid)
                    {
                        //Debug.WriteLine("{0} Discarded outdated split event %s", i.Distance, i);

                        continue;
                    }

                    var t = _verticesCollection.Handle_Split_Event(splitEvent);
                    if (t != null)
                    {
                        e.Add(t.Subtree);

                        if (t.Events != null)
                        {
                            foreach (var b in t.Events)
                            {
                                _eventQueue.Push(b);
                                _eventQueue.Siftdown(0, _eventQueue.Count - 1);
                            }
                        }
                    }
                }
                else if (i is _EdgeEvent)
                {
                    var edgeEvent = (_EdgeEvent)i;

                    if ((!edgeEvent.vertex_a.IsValid) || (!edgeEvent.vertex_b.IsValid))
                    {
                        continue;
                    }

                    var t = _verticesCollection.Handle_Edge_Event(edgeEvent);
                    if (t != null)
                    {
                        e.Add(t.Subtree);

                        if (t.Events != null)
                        {
                            foreach (var b in t.Events)
                            {
                                _eventQueue.Push(b);
                                _eventQueue.Siftdown(0, _eventQueue.Count - 1);
                            }
                        }
                    }
                }
            }

            //return line
            foreach (var t in e)
            {
                if (t != null && t.Sinks != null)
                {
                    foreach (var s in t.Sinks)
                    {
                        result.Add(new Vector2[] { t.Source, s });
                    }
                }
            }

            return result;

        }
    
}
}
