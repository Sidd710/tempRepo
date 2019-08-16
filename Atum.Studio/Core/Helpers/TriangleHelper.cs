using Atum.DAL.Managers;
using Atum.DAL.Materials;
using Atum.Studio.Core.Engines.MagsAI;
using Atum.Studio.Core.Models;

using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using Atum.Studio.Core.Utils;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Helpers
{
    class TriangleHelper
    {

        internal static TriangleIntersection CalcNearestTriangle(Vector3Class origin, Vector3Class direction, STLModel3D selectedModel, Vector3Class linkedCloneTranslation)
        {
            TriangleIntersection nearestTriangle = null;
            TriangleIntersection[] intersectedTriangles = null;
            if (selectedModel != null)
            {
                //model
                var intersectedSupportCones = new List<Triangle>();
                var supportDistance = 50000f;

                var cameraPosition = linkedCloneTranslation!= null ? origin : origin;
                if (IntersectionProvider.IntersectTriangle(cameraPosition, direction, selectedModel, IntersectionProvider.typeDirection.OneWay, false, linkedCloneTranslation!= null ? linkedCloneTranslation: new Vector3Class(), out intersectedTriangles))
                {
                    if (intersectedTriangles != null)
                    {
                        foreach (var intersectedTriangle in intersectedTriangles)
                        {
                            if (intersectedTriangle != null && intersectedTriangle.IntersectionPoint != null)
                            {
                                var cameraDistance = (SceneView.CameraPosition - intersectedTriangle.IntersectionPoint).Length;
                                if (cameraDistance < supportDistance)
                                {
                                    supportDistance = cameraDistance;

                                    nearestTriangle = intersectedTriangle;
                                }
                            }
                        }
                    }
                }
            }

            return nearestTriangle;
        }

        internal static Dictionary<TriangleConnectionInfo, bool> GetConnectedMesh(TriangleConnectionInfo parentTriangle, Dictionary<TriangleConnectionInfo, bool> availableConnections, TriangleInfoList triangles)
        {
            var connectedTriangles = new Dictionary<TriangleConnectionInfo, bool>();
            //if (triangles.Connections.Count == 0)
            //{
            //    triangles.UpdateConnections();
            //}

            if (availableConnections.Count == 1)
            {
                connectedTriangles.Add(availableConnections.ElementAt(0).Key, false);
            }
            else
            {
                var foundChildConnections = GetConnectedTriangles(new List<TriangleConnectionInfo>() { parentTriangle }, availableConnections, triangles, connectedTriangles);
                while (foundChildConnections.Count > 0)
                {
                    foundChildConnections = GetConnectedTriangles(foundChildConnections, availableConnections, triangles, connectedTriangles);
                }
            }

            return connectedTriangles;
        }


        internal static Dictionary<TriangleConnectionInfo, bool> GetConnectedTrianglesAbove(Dictionary<TriangleConnectionInfo, bool> currentPolygonTriangles, float sliceHeight, float maxSliceHeight, STLModel3D stlModel)
        {
            var totalCurrentPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();
            var newPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();

            foreach (var currentPolygonTriangle in currentPolygonTriangles.Keys)
            {
                totalCurrentPolygonTriangles.Add(currentPolygonTriangle, false);
            }

            while (currentPolygonTriangles.Count > 0)
            {
                foreach (var currentPolygonTriangle in currentPolygonTriangles.Keys)
                {
                    var currentTriangleConnections = stlModel.Triangles.GetConnectedTriangles(currentPolygonTriangle);
                    foreach (var currentTriangleConnection in currentTriangleConnections)
                    {
                        var currentTriangleTop = stlModel.Triangles[currentTriangleConnection.ArrayIndex][currentTriangleConnection.TriangleIndex].Top;
                        var currentTriangleBottom = stlModel.Triangles[currentTriangleConnection.ArrayIndex][currentTriangleConnection.TriangleIndex].Bottom;
                        if ((currentTriangleTop >= sliceHeight && currentTriangleTop <= maxSliceHeight) ||
                            (currentTriangleBottom >= sliceHeight && currentTriangleBottom <= maxSliceHeight) ||
                            (currentTriangleBottom <= sliceHeight && currentTriangleTop >= maxSliceHeight))
                        {
                            if (!totalCurrentPolygonTriangles.ContainsKey(currentTriangleConnection) && !newPolygonTriangles.ContainsKey(currentTriangleConnection))
                            {
                                newPolygonTriangles.Add(currentTriangleConnection, false);
                            }
                        }
                    }
                }

                foreach (var newPolygonTriangle in newPolygonTriangles.Keys)
                {
                    totalCurrentPolygonTriangles.Add(newPolygonTriangle, false);
                }
                currentPolygonTriangles = newPolygonTriangles;
                newPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();
            }

            return totalCurrentPolygonTriangles;
        }

        internal static bool GetConnectedTrianglesEdgeDown(TriangleConnectionInfo[] currentPolygonTriangles, Dictionary<TriangleConnectionInfo, bool> validateFaceDownTriangles, float sliceHeight, float maxSliceHeight, STLModel3D stlModel)
        {
            var totalCurrentPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();
            var newPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();

            foreach (var currentPolygonTriangle in currentPolygonTriangles)
            {
                totalCurrentPolygonTriangles.Add(currentPolygonTriangle, false);
            }

            while (currentPolygonTriangles.Length > 0)
            {
                foreach (var currentPolygonTriangle in currentPolygonTriangles)
                {
                    var currentTriangleConnections = stlModel.Triangles.GetConnectedTriangles(currentPolygonTriangle);
                    foreach (var currentTriangleConnection in currentTriangleConnections)
                    {
                        var currentTriangleTop = stlModel.Triangles[currentTriangleConnection.ArrayIndex][currentTriangleConnection.TriangleIndex].Top;
                        var currentTriangleBottom = stlModel.Triangles[currentTriangleConnection.ArrayIndex][currentTriangleConnection.TriangleIndex].Bottom;
                        if ((currentTriangleTop >= sliceHeight && currentTriangleTop <= maxSliceHeight) ||
                            (currentTriangleBottom >= sliceHeight && currentTriangleBottom <= maxSliceHeight) ||
                            (currentTriangleBottom <= sliceHeight && currentTriangleTop >= maxSliceHeight))
                        {
                            if (!totalCurrentPolygonTriangles.ContainsKey(currentTriangleConnection) && !newPolygonTriangles.ContainsKey(currentTriangleConnection))
                            {
                                if (validateFaceDownTriangles.ContainsKey(currentTriangleConnection))
                                {
                                    return true;
                                }

                                newPolygonTriangles.Add(currentTriangleConnection, false);
                            }
                        }
                    }
                }

                foreach (var newPolygonTriangle in newPolygonTriangles.Keys)
                {
                    totalCurrentPolygonTriangles.Add(newPolygonTriangle, false);
                }
                currentPolygonTriangles = newPolygonTriangles.Keys.ToArray();
                newPolygonTriangles = new Dictionary<TriangleConnectionInfo, bool>();
            }

            return false; ;
        }

        internal static List<TriangleConnectionInfo> GetConnectedTriangles(List<TriangleConnectionInfo> parentConnections, Dictionary<TriangleConnectionInfo, bool> availableConnections, TriangleInfoList triangles, Dictionary<TriangleConnectionInfo, bool> connectedTriangles)
        {
            var foundChildConnections = new List<TriangleConnectionInfo>();

            try
            {
                foreach (var parentConnection in parentConnections)
                {
                    var childConnections = triangles.GetConnectedTriangles(parentConnection);
                    foreach (var childConnection in childConnections)
                    {
                        if (availableConnections.ContainsKey(childConnection))
                        {
                            if (!connectedTriangles.ContainsKey(childConnection))
                            {
                                connectedTriangles.Add(childConnection, false);
                                availableConnections.Remove(childConnection);
                                foundChildConnections.Add(childConnection);
                            }
                        }
                    }
                }
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("Exception Manager", "GetConnectedTriangles", exc.StackTrace);
            }

            return foundChildConnections;
        }

      
     
        internal static void SavePolyNodesContourToTxt(List<PolyNode> parents, List<PolyNode> currentLayer, List<PolyNode> resultLayer, string fileName)
        {
            using (var textWriter = new StreamWriter(@"test\" + fileName + ".txt", false))
            {
                foreach (var polyNode in parents)
                {
                    textWriter.WriteLine("Parent:");

                    foreach (var point in polyNode.Contour)
                    {
                        textWriter.WriteLine(point.X + "," + point.Y);
                    }
                }

                textWriter.WriteLine("---");
                foreach (var current in currentLayer)
                {
                    textWriter.WriteLine("Child:");

                    foreach (var point in current.Contour)
                    {
                        textWriter.WriteLine(point.X + "," + point.Y);
                    }
                }

                textWriter.WriteLine("---");
                foreach (var result in resultLayer)
                {
                    textWriter.WriteLine("Result:");

                    foreach (var point in result.Contour)
                    {
                        textWriter.WriteLine(point.X + "," + point.Y);
                    }
                }
            }
        }

        internal static void GetAllContoursInPolyNodeList(List<PolyNode> polyNodes, bool filterOnNotIsHole, ref List<List<IntPoint>> results)
        {
            foreach (var polyNode in polyNodes)
            {
                if (polyNode.Contour.Count > 0)
                {
                    if ((filterOnNotIsHole && !polyNode.IsHole) || (!filterOnNotIsHole))
                    {

                        results.Add(polyNode.Contour);
                        if (polyNode.Childs.Count > 0)
                        {
                            GetAllContoursInPolyNodeList(polyNode.Childs, filterOnNotIsHole, ref results);
                        }
                    }
                }
            }
        }

        internal static void GetAllChildsInPolyNodeList(List<PolyNode> polyNodes, ref List<PolyNode> results)
        {
            foreach (var polyNode in polyNodes)
            {
                if (polyNode.Contour.Count > 0)
                {
                    results.Add(polyNode);
                    if (polyNode.Childs.Count > 0)
                    {
                        GetAllChildsInPolyNodeList(polyNode.Childs, ref results);
                    }

                }
            }
        }

        internal static void CleanPolyNodesContourToPng()
        {
            if (Directory.Exists("Test"))
            {
                foreach (var file in Directory.EnumerateFiles("Test"))
                {
                    File.Delete(file);
                }
            }
        }

        internal static void SaveLowestPointsContourToPng(LowestPointsPolygons lowestPointContours, PolyTree modelLayerFacingDown, Material selectedMaterial, int sliceHeight)
        {
            try
            {
                var bitmap = new Bitmap(1920, 1200);
                using (var gr = Graphics.FromImage(bitmap))
                {
                    var p = new GraphicsPath();
                    foreach (var lowestPointContour in lowestPointContours)
                    {
                        var k = new List<PointF>();
                        foreach (var supportedContour in lowestPointContour.SupportedContours._allPolys)
                        {
                            k.Clear();
                            foreach (var supportedContourPoint in supportedContour.Contour)
                            {
                                var t = new PointF(supportedContourPoint.X / CONTOURPOINT_TO_VECTORPOINT_FACTOR * 10, supportedContourPoint.Y / CONTOURPOINT_TO_VECTORPOINT_FACTOR * 10);
                                t.X += (1920 / 2);
                                t.Y += (1200 / 2);

                                k.Add(t);
                            }

                            if (k.Count > 0)
                            {
                                k.Add(k[0]);
                            }

                            p.AddPolygon(k.ToArray());
                            gr.FillPath(new SolidBrush(supportedContour.IsHole ? Color.White : Color.Lime), p);
                            p = new GraphicsPath();

                            foreach (var edgeSupportConeOutlinePoint in lowestPointContour.EdgeIntersectionPoints)
                            {
                                var edgePointSupportConeOutline = edgeSupportConeOutlinePoint.CalcOutlinePoints(selectedMaterial);

                                p.AddPolygon(edgePointSupportConeOutline.AsDrawingPointF().ToArray());
                                gr.DrawPath(new Pen(Color.Teal), p);
                                p = new GraphicsPath();
                            }

                            foreach (var intersectionSupportConeOutlinePoint in lowestPointContour.LowestPoints)
                            {
                                var intersectionPointSupportConeOutline = intersectionSupportConeOutlinePoint.CalcOutlinePoints(selectedMaterial);

                                p.AddPolygon(intersectionPointSupportConeOutline.AsDrawingPointF().ToArray());
                                gr.DrawPath(new Pen(Color.Gray), p);
                                p = new GraphicsPath();
                            }
                        }
                    }
                }

                bitmap.Save(@"Test\supportedcontour_" + sliceHeight + ".png");
            }
            catch (Exception exc)
            {

            }
        }

        internal static void SavePolyNodesContourToPng(List<PolyNode> polyNodes, string fileName, List<List<IntPoint>> offsetPolygons = null, bool skipLastOffsetPolygonPoint = false, int correctionFactor = 10)
        {
            var saveToFile = false;
            var bitmap = new Bitmap(1920, 1200);
            using (var gr = Graphics.FromImage(bitmap))
            {
                var p = new GraphicsPath();
                foreach (var polyNode in polyNodes)
                {
                    if (polyNode.Contour.Count > 2)
                    {
                        saveToFile = true;

                        var k = new List<PointF>();
                        foreach (var point in polyNode.Contour)
                        {
                            var t = new PointF((point.X / CONTOURPOINT_TO_VECTORPOINT_FACTOR * correctionFactor), (point.Y / CONTOURPOINT_TO_VECTORPOINT_FACTOR) * correctionFactor);
                            if (correctionFactor == 10)
                            {
                                t.X += (1920 / 2);
                                t.Y += (1200 / 2);
                            }
                            k.Add(t);
                        }

                        p.AddPolygon(k.ToArray());
                        gr.FillPath(new SolidBrush(polyNode.IsHole ? Color.Yellow : Color.Red), p);
                        p = new GraphicsPath();
                    }
                }

                if (offsetPolygons != null)
                {
                    foreach (var offsetPolygon in offsetPolygons)
                    {
                        saveToFile = true;

                        var k = new List<PointF>();


                        if (!skipLastOffsetPolygonPoint)
                        {
                            foreach (var point in offsetPolygon)
                            {
                                var t = new PointF((point.X / CONTOURPOINT_TO_VECTORPOINT_FACTOR) * correctionFactor, (point.Y / CONTOURPOINT_TO_VECTORPOINT_FACTOR) * correctionFactor);
                                t.X += (1920 / 2);
                                t.Y += (1200 / 2);
                                k.Add(t);
                            }


                            if (k.Count > 2)
                            {
                                p.AddPolygon(k.ToArray());
                                gr.DrawPath(new Pen(Color.White), p);
                            }
                        }

                    }
                }

                if (saveToFile)
                {
                    if (Directory.Exists("Test"))
                    {
                        bitmap.Save(@"test\" + fileName + ".png");
                    }
                }
            }
        }

        internal static List<SlicePoint2D> GetIntersectingPoints(int ypos, List<SliceLine2D> lines)
        {
            int ymin = 0;
            int ymax = 0;

            var points = new List<SlicePoint2D>();
            //process crossing lines. When end point intersect with horizontal line do not include points
            foreach (var ln in lines)
            {
                ymin = (int)Math.Round(Math.Min(ln.p1.Y, ln.p2.Y), 0);
                ymax = (int)Math.Round(Math.Max(ln.p1.Y, ln.p2.Y), 0);

                if (ln.p1.Y == ypos && ln.p2.Y == ypos) //skip
                {
                }
                else if (ln.p1.Y == ypos) // point 1 endpoint is on the line,
                {
                    //If the intersection is the ymin of the edge’s
                    //endpoint, count it. Otherwise, don’t
                    if (ln.p1.Y == ymin) // if the 
                    {
                        points.Add(new SlicePoint2D() { X = ln.p1.X, Y = ln.p1.Y, ParentLine = ln });
                    }
                }
                else if (ln.p2.Y == ypos) // point 2 endpoint is on the line,
                {
                    //If the intersection is the ymin of the edge’s
                    //endpoint, count it. Otherwise, don’t
                    if (ln.p2.Y == ymin)
                    {
                        points.Add(new SlicePoint2D() { X = ln.p2.X, Y = ln.p2.Y, ParentLine = ln });
                    }
                }
                else // intersect  
                {
                    var isect = ln.IntersectY(ypos); // singled point of intersection
                    points.Add(new SlicePoint2D() { X = isect.X, Y = isect.Y, ParentLine = ln });
                }
            }

            return points;
        }

    }
}
