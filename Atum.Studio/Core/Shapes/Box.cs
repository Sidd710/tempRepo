using Atum.Studio.Core.Models;
using Atum.Studio.Core.Structs;
using OpenTK;

namespace Atum.Studio.Core.Shapes
{
    internal class Box : STLModel3D
    {
        internal new float Width { get; set; }

        internal new float Depth { get; set; }

        internal new float Height { get; set; }

        internal new Vector3 Center
        {
            get
            {
                return new Vector3(this.Width / 2f, this.Depth / 2f, this.Height / 2f);
            }
        }

        internal Box(float width, float depth, float height, bool centerXYPosition)
          : base(STLModel3D.TypeObject.Ground, true)
        {
            this.Width = width;
            this.Depth = depth;
            this.Height = height;
            this.Triangles = new TriangleInfoList();

            Triangle triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, 0.0f) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width, 0.0f, 0.0f) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, 0.0f) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, 0.0f) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, 0.0f, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, 0.0f, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitX;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, this.Depth, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, 0.0f, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, 0.0f, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth, 0.0f) };
            this.Triangles[0].Add(triangle);

            triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitY;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, height) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth, this.Height) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth, 0.0f) };
            this.Triangles[0].Add(triangle);

            if (!centerXYPosition)
                return;
            var vector3 = -new Vector3Class(width / 2f, depth / 2f, 0.0f);
            for (int index = 0; index < this.Triangles[0].Count; ++index)
            {
                this.Triangles[0][index].Vectors[0].Position += vector3;
                this.Triangles[0][index].Vectors[1].Position += vector3;
                this.Triangles[0][index].Vectors[2].Position += vector3;
            }
            this.MoveTranslation = new Vector3Class(width / 2f, depth / 2f, height);
        }
    }
}
