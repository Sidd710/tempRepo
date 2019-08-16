using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System.Drawing;

namespace Atum.Studio.Core.Shapes
{
    [Serializable]
    internal class Cone
    {
        internal Cone(Vector3 position, float bottomRadius, float topRadius, float height, int slicesCount, int supportConeIndex)
        {
            // bottom circle
            GL.Begin(PrimitiveType.TriangleFan);
            GL.Color4(Color.FromArgb(supportConeIndex, Color.LimeGreen));
            GL.Vertex3(position);
            for (int i = slicesCount + 1; i > 0; i--)
            {
                
                GL.Vertex3(position.X + Math.Cos((float)i / slicesCount * 2 * Math.PI) * bottomRadius, position.Y + Math.Sin((float)i / slicesCount * 2 * Math.PI) * bottomRadius, position.Z);
            }
            GL.End();

            GL.Begin(PrimitiveType.TriangleFan);
            //top circle
            GL.Vertex3(position.X, position.Y, position.Z + height);
            for (int i = 0; i < slicesCount + 1; i++)
            {
                GL.Vertex3(position.X + Math.Cos((float)i / slicesCount * 2 * Math.PI) * (topRadius), position.Y + Math.Sin((float)i / slicesCount * 2 * Math.PI) * topRadius, position.Z + height);
            }
            GL.End();

            // the rest
            GL.Begin(PrimitiveType.TriangleStrip);
            for (int i = slicesCount + 1; i > 0; i--)
            {
                GL.Vertex3(position.X + Math.Cos((float)i / slicesCount * 2 * Math.PI) * bottomRadius, position.Y + Math.Sin((float)i / slicesCount * 2 * Math.PI) * bottomRadius, position.Z);
                GL.Vertex3(position.X + Math.Cos((float)i / slicesCount * 2 * Math.PI) * topRadius, position.Y + Math.Sin((float)i / slicesCount * 2 * Math.PI) * topRadius, position.Z + height);
            }
            GL.End();
        }
    }
}
