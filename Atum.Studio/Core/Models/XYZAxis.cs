using Atum.Studio.Core.Events;
using Atum.Studio.Core.Shapes;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Core.Models
{
    internal static class XYZAxis
    {
        private static TriangleInfoList _xTriangles;
        private static TriangleInfoList _yTriangles;
        private static TriangleInfoList _zTriangles;

        private static OrientationGizmoFaceText _xLabel;
        private static OrientationGizmoFaceText _yLabel;
        private static OrientationGizmoFaceText _zLabel;

        internal static void CreateXYZAxis()
        {
            _xTriangles = new TriangleInfoList();
            _yTriangles = new TriangleInfoList();
            _zTriangles = new TriangleInfoList();

            var xAxis = new XYZAxisCone(60, 15f, 1.5f, 1.5f, 0f, 0f, 26, new Vector3(), Color.Red, 0, 0, null, false);
            xAxis.RotateByAxis(0, 90, 0, RotationEventArgs.TypeAxis.Y);
            _xTriangles[0].AddRange(xAxis.Triangles[0]);
            var yAxis = new XYZAxisCone(60, 15f, 1.5f, 1.5f, 0f, 0f, 26, new Vector3(), Color.Green, 0, 0, null, false);
            yAxis.RotateByAxis(-90, 0, 0, RotationEventArgs.TypeAxis.X);
            _yTriangles[0].AddRange(yAxis.Triangles[0]);
            var zAxis = new XYZAxisCone(60, 15f, 1.5f, 1.5f, 0f, 0f, 26, new Vector3(), Color.Blue, 0, 0, null, false);
            _zTriangles[0].AddRange(zAxis.Triangles[0]);

            if (DAL.OS.OSProvider.IsWindows)
            {

                //xLabel
                _xLabel = new OrientationGizmoFaceText();
                var xLabelOffset = new Vector3(60, -7.5f, 0);
                _xLabel.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("X", FontStyle.Regular, 15f);
                var xLabelClone = (STLModel3D)_xLabel.Clone();
                foreach (var triangle in xLabelClone.Triangles[0])
                {
                    triangle.Flip();
                    _xLabel.Triangles[0].Add(triangle);
                }
                foreach (var triangle in _xLabel.Triangles[0])
                {
                    triangle.Vectors[0].Position += xLabelOffset;
                    triangle.Vectors[1].Position += xLabelOffset;
                    triangle.Vectors[2].Position += xLabelOffset;
                }

                //yLabel
                _yLabel = new OrientationGizmoFaceText();
                _yLabel.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("Y", FontStyle.Regular, 15f);
                _yLabel.RotateText(0, 0, 180, RotationEventArgs.TypeAxis.Z);
                _yLabel.UpdateBoundries();
                _yLabel.UpdateDefaultCenter(true);
                //_yLabel._rotationAngleX = 0;
                var yLabelClone = (STLModel3D)_yLabel.Clone();
                foreach (var triangle in yLabelClone.Triangles[0])
                {
                    triangle.Flip();
                    _yLabel.Triangles[0].Add(triangle);
                }

                //zLabel
                _zLabel = new OrientationGizmoFaceText();
                _zLabel.Triangles = Engines.FontTessellationEngine.ConvertStringToTriangles("Z", FontStyle.Regular, 15f);
                _zLabel.RotateText(180, 0, 0, RotationEventArgs.TypeAxis.X);
                _zLabel.UpdateBoundries();
                _zLabel.UpdateDefaultCenter(true);
                _zLabel._rotationAngleX = 0;
                var zLabelClone = (STLModel3D)_zLabel.Clone();
                foreach (var triangle in zLabelClone.Triangles[0])
                {
                    triangle.Flip();
                    _zLabel.Triangles[0].Add(triangle);
                }
            }

        }

        internal static void Render(Matrix4 viewMatrix)
        {
            if (_zTriangles != null)
            {
                GL.PushMatrix();

                GL.Viewport(0, 0, 150, 150);

                GL.MatrixMode(MatrixMode.Projection);
                Matrix4 ortho = Matrix4.CreateOrthographic(175, 175, 10, 3000);
                GL.LoadMatrix(ref ortho);

                GL.MatrixMode(MatrixMode.Modelview);
                GL.LoadIdentity();

                GL.LoadMatrix(ref viewMatrix);

                GL.Color4(Color.FromArgb(0, Color.Red));
                GL.Begin(PrimitiveType.Triangles);

                foreach (var triangle in _xTriangles[0])
                {
                    GL.Normal3(triangle.Normal);
                    GL.Vertex3(triangle.Vectors[0].Position);
                    GL.Vertex3(triangle.Vectors[1].Position);
                    GL.Vertex3(triangle.Vectors[2].Position);
                }

                if (DAL.OS.OSProvider.IsWindows)
                {
                    var rotationX = (float)MathHelper.RadiansToDegrees(Math.Atan2(viewMatrix.M32, viewMatrix.M33));
                    
                    _xLabel.RotateText(rotationX, 0, 0, RotationEventArgs.TypeAxis.X);
                    _yLabel.RotateText(rotationX, 0, 0, RotationEventArgs.TypeAxis.X);
                    _zLabel.RotateText(rotationX, 0, 0, RotationEventArgs.TypeAxis.X);
                    
                    foreach (var triangle in _xLabel.Triangles[0])
                    {
                        GL.Normal3(triangle.Normal);
                        GL.Vertex3(triangle.Vectors[0].Position);
                        GL.Vertex3(triangle.Vectors[1].Position);
                        GL.Vertex3(triangle.Vectors[2].Position);
                    }

                    GL.End();
                }

                GL.Color4(Color.FromArgb(0, Color.Green));
                GL.Begin(PrimitiveType.Triangles);
                foreach (var triangle in _yTriangles[0])
                {
                    GL.Normal3(triangle.Normal);
                    GL.Vertex3(triangle.Vectors[0].Position);
                    GL.Vertex3(triangle.Vectors[1].Position);
                    GL.Vertex3(triangle.Vectors[2].Position);
                }

                var rotationY = (float)MathHelper.RadiansToDegrees(Math.Atan2(-viewMatrix.M31, Math.Sqrt(viewMatrix.M32 * viewMatrix.M32 + viewMatrix.M33 * viewMatrix.M33)));
                if (DAL.OS.OSProvider.IsWindows)
                {
                    var yLabelOffset = new Vector3(0f, 75f, 0);
                    foreach (var triangle in _yLabel.Triangles[0])
                    {
                        GL.Normal3(triangle.Normal);
                        GL.Vertex3(triangle.Vectors[0].Position + yLabelOffset);
                        GL.Vertex3(triangle.Vectors[1].Position + yLabelOffset);
                        GL.Vertex3(triangle.Vectors[2].Position + yLabelOffset);
                    }
                }

                GL.End();

                GL.Color4(Color.FromArgb(0, Color.Blue));
                GL.Begin(PrimitiveType.Triangles);
                foreach (var triangle in _zTriangles[0])
                {
                    GL.Normal3(triangle.Normal);
                    GL.Vertex3(triangle.Vectors[0].Position);
                    GL.Vertex3(triangle.Vectors[1].Position);
                    GL.Vertex3(triangle.Vectors[2].Position);
                }

                if (DAL.OS.OSProvider.IsWindows)
                {
                    var zLabelOffset = new Vector3(0f, 0f, 75f);
                    foreach (var triangle in _zLabel.Triangles[0])
                    {
                        GL.Normal3(triangle.Normal);
                        GL.Vertex3(triangle.Vectors[0].Position + zLabelOffset);
                        GL.Vertex3(triangle.Vectors[1].Position + zLabelOffset);
                        GL.Vertex3(triangle.Vectors[2].Position + zLabelOffset);
                    }
                }

                GL.End();

                GL.PopMatrix();
            }
        }

    }
}
