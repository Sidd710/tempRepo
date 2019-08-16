using Atum.Studio.Core.Models;
using Atum.Studio.Core.Structs;
using OpenTK;

namespace Atum.Studio.Core.Shapes.Custom
{
    internal class AtumBox : STLModel3D
    {
        internal float Width { get; set; }

        internal float Depth { get; set; }

        internal float Height { get; set; }

        internal new Vector3 Center
        {
            get
            {
                return new Vector3(this.Width / 2f, this.Depth / 2f, this.Height / 2f);
            }
        }

        internal AtumBox(float width, float depth, float height, bool centerXYPosition, bool includeBottom)
          : base(STLModel3D.TypeObject.Ground, true)
        {
            Vector3Class[] vector3Array = new Vector3Class[3];
            this.Width = width;
            this.Depth = depth;
            this.Height = height;
            this.Triangles = new TriangleInfoList();

            //bottom
            Triangle triangle = new Triangle();
            triangle.Normal = -Vector3Class.UnitZ;
            triangle.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, 0.0f) };
            triangle.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width, 0.0f, 0.0f) };
            this.Triangles[0].Add(triangle);

            Triangle triangle2 = new Triangle();
            triangle2.Normal = -Vector3Class.UnitZ;
            triangle2.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle2.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, 0.0f) };
            triangle2.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, 0.0f) };
            this.Triangles[0].Add(triangle2);

            if (!includeBottom)
            {
                triangle.Flip();
                triangle2.Flip();
            }

            //
            Triangle triangle3 = new Triangle();
            triangle3.Normal = Vector3Class.UnitZ;
            triangle3.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            triangle3.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width / 2f, 0.0f, this.Height) };
            triangle3.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width / 2f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle3);

            Triangle triangle4 = new Triangle();
            triangle4.Normal = Vector3Class.UnitZ;
            triangle4.Vectors[0] = new VertexClass() { Position = new Vector3Class(this.Width / 2f, this.Depth / 2f, this.Height) };
            triangle4.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth / 2f, this.Height) };
            triangle4.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle4);

            Triangle triangle5 = new Triangle();
            triangle5.Normal = Vector3Class.UnitZ;
            triangle5.Vectors[0] = new VertexClass() { Position = new Vector3Class(this.Width / 2f, 0.0f, this.Height), };
            triangle5.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth / 2f, this.Height) };
            triangle5.Vectors[2] = new VertexClass() { Position = new Vector3Class(this.Width / 2f, this.Depth / 2f, this.Height) };
            this.Triangles[0].Add(triangle5);

            Triangle triangle6 = new Triangle();
            triangle6.Normal = Vector3Class.UnitZ;
            triangle6.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            triangle6.Vectors[1] = new VertexClass() { Position = new Vector3Class(this.Width, this.Depth, this.Height) };
            triangle6.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle6);

            Triangle triangle7 = new Triangle();
            triangle7.Normal = -Vector3Class.UnitX;
            triangle7.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle7.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            triangle7.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle7);

            Triangle triangle8 = new Triangle();
            triangle8.Normal = -Vector3Class.UnitX;
            triangle8.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, 0.0f) };
            triangle8.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle8.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle8);

            Triangle triangle9 = new Triangle();
            triangle9.Normal = Vector3Class.UnitX;
            triangle9.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            triangle9.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth, 0.0f) };
            triangle9.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth, this.Height) };
            this.Triangles[0].Add(triangle9);

            Triangle triangle10 = new Triangle();
            triangle10.Normal = Vector3Class.UnitX;
            triangle10.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, this.Depth, this.Height) };
            triangle10.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth / 2f, this.Height) };
            triangle10.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth / 2f, this.Height / 2f) };
            this.Triangles[0].Add(triangle10);

            vector3Array[2] = triangle10.Vectors[1].Position;

            Triangle triangle11 = new Triangle();
            triangle11.Normal = Vector3Class.UnitX;
            triangle11.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, this.Depth / 2f, this.Height / 2f) };
            triangle11.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, 0.0f, this.Height / 2f) };
            triangle11.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            this.Triangles[0].Add(triangle11);

            vector3Array[1] = triangle11.Vectors[1].Position;

            Triangle triangle12 = new Triangle();
            triangle12.Normal = Vector3Class.UnitX;
            triangle12.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, 0.0f, this.Height / 2f) };
            triangle12.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth / 2f, this.Height / 2f) };
            triangle12.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth / 2f, this.Height) };
            this.Triangles[0].Add(triangle12);

            Triangle triangle13 = new Triangle();
            triangle13.Normal = Vector3Class.UnitY;
            triangle13.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, 0.0f) };
            triangle13.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            triangle13.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            this.Triangles[0].Add(triangle13);

            Triangle triangle14 = new Triangle();
            triangle14.Normal = Vector3Class.UnitY;
            triangle14.Vectors[0] = new VertexClass() { Position = new Vector3Class(width / 2f, 0.0f, height / 2f) };
            triangle14.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, 0.0f, height / 2f) };
            triangle14.Vectors[2] = new VertexClass() { Position = new Vector3Class(width / 2f, 0.0f, this.Height) };
            this.Triangles[0].Add(triangle14);

            vector3Array[0] = triangle14.Vectors[2].Position;

            Triangle triangle15 = new Triangle();
            triangle15.Normal = Vector3Class.UnitY;
            triangle15.Vectors[0] = new VertexClass() { Position = new Vector3Class(width, 0.0f, 0.0f) };
            triangle15.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, 0.0f, height / 2f) };
            triangle15.Vectors[2] = new VertexClass() { Position = new Vector3Class(width / 2f, 0.0f, this.Height / 2f) };
            this.Triangles[0].Add(triangle15);

            Triangle triangle16 = new Triangle();
            triangle16.Normal = Vector3Class.UnitY;
            triangle16.Vectors[0] = new VertexClass() { Position = new Vector3Class(width / 2f, 0.0f, height / 2f) };
            triangle16.Vectors[1] = new VertexClass() { Position = new Vector3Class(width / 2f, 0.0f, height) };
            triangle16.Vectors[2] = new VertexClass() { Position = new Vector3Class(0.0f, 0.0f, this.Height) };
            this.Triangles[0].Add(triangle16);

            Triangle triangle17 = new Triangle();
            triangle17.Normal = -Vector3Class.UnitY;
            triangle17.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, 0.0f) };
            triangle17.Vectors[1] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, this.Height) };
            triangle17.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth, 0.0f) };
            this.Triangles[0].Add(triangle17);

            Triangle triangle18 = new Triangle();
            triangle18.Normal = -Vector3Class.UnitY;
            triangle18.Vectors[0] = new VertexClass() { Position = new Vector3Class(0.0f, this.Depth, height) };
            triangle18.Vectors[1] = new VertexClass() { Position = new Vector3Class(width, this.Depth, this.Height) };
            triangle18.Vectors[2] = new VertexClass() { Position = new Vector3Class(width, this.Depth, 0.0f) };
            this.Triangles[0].Add(triangle18);

            Triangle triangle19 = new Triangle();
            triangle19.Normal = Vector3Class.UnitX;
            triangle19.Vectors[0] = new VertexClass() { Position = vector3Array[0] };
            triangle19.Vectors[1] = new VertexClass() { Position = vector3Array[1] };
            triangle19.Vectors[2] = new VertexClass() { Position = vector3Array[2] };
            triangle19.CalcNormal();

            this.Triangles[0].Add(triangle19);

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
