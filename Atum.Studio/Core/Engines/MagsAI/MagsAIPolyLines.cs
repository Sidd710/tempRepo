using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System.Collections.Generic;
using System.Linq;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Engines.MagsAI
{
    internal class MagsAIPolyLines : List<MagsAIPolyLine>
    {
        internal bool ClosedLine { get; set; }
        internal PolyTree PolyTree { get; set; }
        internal List<Vector3Class> DebugPoints = new List<Vector3Class>();

        internal float Length
        {
            get
            {
                var result = 0f;
                foreach(var linePart in this)
                {
                    result += (linePart.P2 - linePart.P1).Length;
                }

                return result;
            }
        }

        public static List<MagsAIPolyLines> FromListOfModelIntersectionsUsingPolyLines(float sliceHeight, List<SlicePolyLine3D> intersectionPoints, SupportProfile selectedMaterialProfile, AtumPrinter selectedPrinter, bool usePixelsAsValues = true)
        {
            var result = new List<MagsAIPolyLines>();

            var intersectionLines = VectorHelper.Get2dLinesAsPixels(intersectionPoints, selectedPrinter);

            if (intersectionLines.Count > 0)
            {
                //convert intersectionLines to fast indexes
                var intersectionLookUp = new Dictionary<Vector3Class, List<Line3D>>();
                foreach (var intersectionPoint in intersectionLines)
                {
                    if (!intersectionLookUp.ContainsKey(intersectionPoint.StartPoint))
                    {
                        intersectionLookUp.Add(intersectionPoint.StartPoint, new List<Line3D>());
                    }
                    intersectionLookUp[intersectionPoint.StartPoint].Add(intersectionPoint);

                    if (!intersectionLookUp.ContainsKey(intersectionPoint.EndPoint))
                    {
                        intersectionLookUp.Add(intersectionPoint.EndPoint, new List<Line3D>());
                    }
                    intersectionLookUp[intersectionPoint.EndPoint].Add(intersectionPoint);
                }

                var polygonTriangleIndexes = new TriangeConnectionsIndexed();

                var p1 = intersectionLookUp.Keys.First();
                var currentLine = intersectionLookUp[p1].First();
                var p2 = currentLine.StartPoint == p1 ? currentLine.EndPoint : currentLine.StartPoint;

                RemoveFromIntersectionLookup(p1, p2, currentLine, intersectionLookUp);

                //if (intersectionLookUp.ContainsKey(p1))
                //{
                //    intersectionLookUp[p1].Remove(currentLine);
                //    if (intersectionLookUp[p1].Count == 0)
                //    {
                //        intersectionLookUp.Remove(p1);
                //    }
                //}

                //if (intersectionLookUp.ContainsKey(p2))
                //{
                //    intersectionLookUp[p2].Remove(currentLine);
                //    if (intersectionLookUp[p2].Count == 0)
                //    {
                //        intersectionLookUp.Remove(p2);
                //    }
                //}

                var polyLine = new MagsAIPolyLines();
                polyLine.Add(new MagsAIPolyLine() { P1 = currentLine.StartPoint, P2 = currentLine.EndPoint, Normal = currentLine.Normal });

                while (intersectionLookUp.Count > 0)
                {
                    //find neighbors using P1 and P2
                    Line3D neightborP1 = null;
                    if (intersectionLookUp.ContainsKey(p1))
                    {
                        neightborP1 = intersectionLookUp[p1].FirstOrDefault();
                        if (neightborP1 != null)
                        {
                            polyLine.Insert(0, new MagsAIPolyLine() { P1 = neightborP1.StartPoint == p1 ? neightborP1.EndPoint : neightborP1.StartPoint, P2 = p1, Normal = neightborP1.Normal });
                            p1 = neightborP1.StartPoint == p1 ? neightborP1.EndPoint : neightborP1.StartPoint;

                            RemoveFromIntersectionLookup(neightborP1.StartPoint, neightborP1.EndPoint, neightborP1, intersectionLookUp);
                        }
                    }

                    Line3D neightborP2 = null;
                    if (intersectionLookUp.ContainsKey(p2))
                    {
                        neightborP2 = intersectionLookUp[p2].FirstOrDefault();

                        if (neightborP2 != null)
                        {
                            polyLine.Add(new MagsAIPolyLine() { P1 = p2, P2 = neightborP2.StartPoint == p2 ? neightborP2.EndPoint : neightborP2.StartPoint, Normal = neightborP2.Normal });
                            p2 = neightborP2.StartPoint == p2 ? neightborP2.EndPoint : neightborP2.StartPoint;

                            RemoveFromIntersectionLookup(neightborP2.StartPoint, neightborP2.EndPoint, neightborP2, intersectionLookUp);
                        }
                    }

                    //no lines found start new polyline
                    if (neightborP1 == null && neightborP2 == null && polyLine.Count > 0)
                    {
                        if (polyLine.Count > 1)
                        {
                            if (polyLine[0].P1 == polyLine.Last().P1 && polyLine[0].P2 == polyLine.Last().P2)
                            {
                                polyLine.RemoveAt(0);
                                polyLine.ClosedLine = true;
                            }

                            if (polyLine[0].P1 == polyLine.Last().P2)
                            {
                                polyLine.ClosedLine = true;
                            }
                        }

                        if (polyLine.Length > 5f)
                        {
                            result.Add(polyLine);
                        }

                        polyLine = new MagsAIPolyLines();

                        if (intersectionLookUp.Count > 0)
                        {
                            p1 = intersectionLookUp.Keys.First();
                            currentLine = intersectionLookUp[p1].First();
                            p2 = currentLine.StartPoint == p1 ? currentLine.EndPoint : currentLine.StartPoint;

                            RemoveFromIntersectionLookup(p1, p2, currentLine, intersectionLookUp);

                            polyLine.Add(new MagsAIPolyLine() { P1 = currentLine.StartPoint, P2 = currentLine.EndPoint, Normal = currentLine.Normal });

                        }
                    }

                }

                if (polyLine.Count > 0)
                {
                    if (polyLine.Count > 1)
                    {
                        if (polyLine[0].P1 == polyLine.Last().P1 && polyLine[0].P2 == polyLine.Last().P2)
                        {
                            polyLine.RemoveAt(0);
                            polyLine.ClosedLine = true;
                        }

                        if (polyLine[0].P1 == polyLine.Last().P2)
                        {
                            polyLine.ClosedLine = true;
                        }
                    }

                    //check length of polyLine
                    if (polyLine.Length > 5f)
                    {
                        result.Add(polyLine);
                    }
                }
            }

            var objectIndex = 0;
            foreach (var r in result)
            {
                r.CalcPolyNode(selectedMaterialProfile, selectedPrinter, sliceHeight, objectIndex);
                objectIndex++;
            }

            return result;
        }


        private static void RemoveFromIntersectionLookup(Vector3Class p1, Vector3Class p2, Line3D line, Dictionary<Vector3Class, List<Line3D>> intersectionLookUp)
        {
            if (intersectionLookUp.ContainsKey(p1))
            {
                intersectionLookUp[p1].Remove(line);
                if (intersectionLookUp[p1].Count == 0)
                {
                    intersectionLookUp.Remove(p1);
                }
            }

            if (intersectionLookUp.ContainsKey(p2))
            {
                intersectionLookUp[p2].Remove(line);
                if (intersectionLookUp[p2].Count == 0)
                {
                    intersectionLookUp.Remove(p2);
                }
            }
        }

        internal void CalcPolyNode(SupportProfile selectedMaterialProfile, AtumPrinter selectedPrinter, float sliceHeight, int objectIndex)
        {
            this.PolyTree = new PolyTree();
            var polygonPoints = new List<IntPoint>();

            foreach (var polyLinePart in this)
            {
                var tStart = (polyLinePart.P1 - new Vector3Class(selectedPrinter.ProjectorResolutionX / 2, selectedPrinter.ProjectorResolutionY / 2, 0)) / 10f;
                tStart -= new Vector3Class(0.1f, 0.1f, 0);
                DebugPoints.Add(tStart);
                polygonPoints.Add(new IntPoint(tStart + new Vector3Class(0,0,sliceHeight)));
            }

            //add the end of the line
            var tEnd = (this.Last().P2 - new Vector3Class(selectedPrinter.ProjectorResolutionX / 2, selectedPrinter.ProjectorResolutionY / 2, 0)) / 10f;
            tEnd -= new Vector3Class(0.1f, 0.1f, 0);
            polygonPoints.Add(new IntPoint( tEnd)); 

            //when not closed use normals to create offset points
            if (!ClosedLine)
            {
                this.Reverse();
              //  this.Reverse();
                var normalVector = new Vector3Class();
                foreach (var polyLinePart in this)
                {
                    normalVector += polyLinePart.Normal;
                    normalVector.Z = 0;
                    normalVector.Normalize();
                    normalVector *= selectedMaterialProfile.SupportOverhangDistance / 2;

                    //do point offset
                    var tPoint = normalVector + ((polyLinePart.P1 - new Vector3Class(selectedPrinter.ProjectorResolutionX / 2, selectedPrinter.ProjectorResolutionY / 2, 0)) / 10f);
                    tPoint -= new Vector3Class(0.1f, 0.1f, 0);
                    DebugPoints.Add(tPoint + new Vector3Class(0,0,sliceHeight));
                    polygonPoints.Add(new IntPoint(tPoint));

                    normalVector = polyLinePart.Normal;
                }

                var polyTreeOffset = new PolyTree();
                var polyNode = new PolyNode();
                polyNode.m_polygon = polygonPoints;
                polyTreeOffset._allPolys.Add(polyNode);

                var c = new Clipper();
                c.AddPath(polyTreeOffset._allPolys[0].Contour, PolyType.ptClip, true);
                c.AddPath(new List<IntPoint>(), PolyType.ptSubject, true);

                c.Execute(ClipType.ctXor, this.PolyTree, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);

                //if (sliceHeight >=13 && sliceHeight <=15f)
               // TriangleHelper.SavePolyNodesContourToPng(this.PolyTree._allPolys, sliceHeight.ToString("0.00") + "-"  + objectIndex.ToString() + "-t2");
            }
            else
            {
                var polyTreeOffset = new PolyTree();
                var polyNode = new PolyNode();
                polyNode.m_polygon = polygonPoints;
                polyTreeOffset._allPolys.Add(polyNode);
                polyTreeOffset = MagsAIEngine.ClipperOffset(polyTreeOffset, selectedMaterialProfile.SupportOverhangDistance / 2);

                if (polyTreeOffset._allPolys.Count > 0)
                {
                    var c = new Clipper();
                    c.AddPath(polygonPoints, PolyType.ptClip, true);
                    c.AddPath(polyTreeOffset._allPolys[0].Contour, PolyType.ptClip, true);
                    c.AddPath(new List<IntPoint>(), PolyType.ptSubject, true);

                    c.Execute(ClipType.ctXor, this.PolyTree, PolyFillType.pftEvenOdd, PolyFillType.pftEvenOdd);
                }
            }
        }

    }
}
