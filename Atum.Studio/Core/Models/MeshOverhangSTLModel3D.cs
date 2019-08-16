using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Models
{
    internal class MeshOverhangSTLModel3D : STLModel3D
    {

        internal Dictionary<Vector3Class, bool> InnerPoints { get; set; }
        internal TriangleInfoList SolidTriangles { get; set; }
        internal List<PolyNode> OuterPaths { get; set; }
        internal Dictionary<float, List<PolyTree>> SliceContours { get; set; }
        internal Dictionary<float, List<SlicePolyLine3D>> SliceIntersections { get; set; }
        internal List<Triangle> OuterPathTriangles { get; set; }

        internal MeshOverhangSTLModel3D()
        {
            this.Triangles = new TriangleInfoList();
            this.InnerPoints = new Dictionary<Vector3Class, bool>();
            this.SolidTriangles = new TriangleInfoList();
            this.SliceContours = new Dictionary<float, List<PolyTree>>();
            this.SliceIntersections = new Dictionary<float, List<SlicePolyLine3D>>();
            this.OuterPathTriangles = new List<Triangle>();
            this.OuterPaths = new List<PolyNode>();
        }

        internal void UpdateInnerPoints()
        {
            this.InnerPoints.Clear();
            for(var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                for(var triangleIndex = 0;triangleIndex< this.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    foreach(var point in this.Triangles[arrayIndex][triangleIndex].Points)
                    {
                        if (!InnerPoints.ContainsKey(point))
                        {
                            InnerPoints.Add(point, false);
                        }
                    }
                }
            }
        }
    }
}
