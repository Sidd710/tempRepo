using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Models.Models3MF
{
    public class Object3MF
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public Mesh Mesh { get; set; }
        public List<Component> Components { get; set; }

    }
    public class Mesh
    {
        public List<Vertex> Vertices { get; set; }
        public List<Triangle> Triangles { get; set; }

    }
    public class Vertex
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
    public class Triangle
    {
        public int V1 { get; set; }
        public int V2 { get; set; }
        public int V3 { get; set; }
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int PId { get; set; }
    }
    public class Component
    {
        public int ObjectId { get; set; }
        public float[] Transform { get; set; }
    }
    public class Resources
    {
        public List<Object3MF> Objects3MF { get; set; }
    }
    public class Model3MF
    {
        public Resources Resources { get; set; }
        public List<BuildItem> BuildItems { get; set; }
    }
    public class BuildItem
    {
        public int ObjectId { get; set; }
        public float[] Transform { get; set; }

    }
}
