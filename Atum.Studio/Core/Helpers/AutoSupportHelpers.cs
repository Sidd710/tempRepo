using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System.Collections.Generic;
using System.Linq;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Helpers
{
    class AutoSupportHelpers
    {
        internal static void FindNextChildConnection(STLModel3D stlModel, float currentLayerHeight, float nextLayerHeight, Dictionary<float, List<PolyNode>> connectedLayerPolyNodes, SortedDictionary<float, PolyTree> modelContours)
        {
            //find by using an upper triangle connection
            if (connectedLayerPolyNodes.ContainsKey(currentLayerHeight))
            {
                var totalTriangleConnections = new List<TriangleConnectionInfo>();
                foreach (var contourTriangleIndex in connectedLayerPolyNodes[currentLayerHeight].First().TriangleConnections.Keys)
                {
                    //var firstContourTriangleIndex = connectedLayerPolyNodes[currentLayerHeight].First().TriangleConnections.First();
                    var firstContourTriangle = stlModel.Triangles[contourTriangleIndex.ArrayIndex][contourTriangleIndex.TriangleIndex];

                    //get connected triangles 
                    var connectedTrianglesIndexes = stlModel.Triangles.GetConnectedTriangles(contourTriangleIndex);
                    totalTriangleConnections.AddRange(connectedTrianglesIndexes);

                    foreach (var connectedTriangleIndex in connectedTrianglesIndexes)
                    {
                        var connectedTriangle = stlModel.Triangles[connectedTriangleIndex.ArrayIndex][connectedTriangleIndex.TriangleIndex];

                        if (connectedTriangle.Top > firstContourTriangle.Top)
                        {
                            //find next polgon by using triangle index
                            foreach (var nextModelContour in modelContours[nextLayerHeight]._allPolys)
                            {
                                if (nextModelContour.TriangleConnections.ContainsKey(connectedTriangleIndex))
                                {
                                    if (!connectedLayerPolyNodes.ContainsKey(nextLayerHeight))
                                    {
                                        connectedLayerPolyNodes.Add(nextLayerHeight, new List<PolyNode>());
                                    }

                                    if (!connectedLayerPolyNodes[nextLayerHeight].Contains(nextModelContour))
                                    {
                                        connectedLayerPolyNodes[nextLayerHeight].Add(nextModelContour);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!connectedLayerPolyNodes.ContainsKey(nextLayerHeight))
                {
                    //find recursive
                    FindNextChildRecursive(stlModel, totalTriangleConnections, connectedLayerPolyNodes, currentLayerHeight, nextLayerHeight, modelContours, 0);
                }
            }
        }

        private static void FindNextChildRecursive(STLModel3D stlModel, List<TriangleConnectionInfo> startRecursiveTriangles, Dictionary<float, List<PolyNode>> connectedLayerPolyNodes, float currentHeight, float nextLayerHeight, SortedDictionary<float, PolyTree> modelContours, int lookUpRecursiveCount)
        {
            if (lookUpRecursiveCount < 25)
            {
                var totalTriangleConnections = new List<TriangleConnectionInfo>();
                var allTrianglesBottomHigherThenNextLayerHeight = true;
                foreach (var contourTriangleIndex in startRecursiveTriangles)
                {
                    var firstContourTriangle = stlModel.Triangles[contourTriangleIndex.ArrayIndex][contourTriangleIndex.TriangleIndex];

                    //get connected triangles 
                    var connectedTrianglesIndexes = stlModel.Triangles.GetConnectedTriangles(contourTriangleIndex);
                    totalTriangleConnections.AddRange(connectedTrianglesIndexes);

                    foreach (var connectedTriangleIndex in connectedTrianglesIndexes)
                    {
                        var connectedTriangle = stlModel.Triangles[connectedTriangleIndex.ArrayIndex][connectedTriangleIndex.TriangleIndex];

                        if (connectedTriangle.Bottom <= nextLayerHeight)
                        {
                            allTrianglesBottomHigherThenNextLayerHeight = false;
                        }

                        if (connectedTriangle.Top > firstContourTriangle.Top)
                        {
                            //find next polgon by using triangle index
                            foreach (var nextModelContour in modelContours[nextLayerHeight]._allPolys)
                            {
                                if (nextModelContour.TriangleConnections.ContainsKey(connectedTriangleIndex))
                                {
                                    if (!connectedLayerPolyNodes.ContainsKey(nextLayerHeight))
                                    {
                                        connectedLayerPolyNodes.Add(nextLayerHeight, new List<PolyNode>());
                                    }

                                    if (!connectedLayerPolyNodes[nextLayerHeight].Contains(nextModelContour))
                                    {
                                        connectedLayerPolyNodes[nextLayerHeight].Add(nextModelContour);
                                    }
                                }
                            }
                        }
                    }
                }

                if (!connectedLayerPolyNodes.ContainsKey(nextLayerHeight) && !allTrianglesBottomHigherThenNextLayerHeight)
                {
                    lookUpRecursiveCount++;
                    FindNextChildRecursive(stlModel, startRecursiveTriangles, connectedLayerPolyNodes, currentHeight, nextLayerHeight, modelContours, lookUpRecursiveCount);
                }
            }
        }
    }
}
