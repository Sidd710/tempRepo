using System;
using OpenTK;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class OrientationGizmoFaceText : STLModel3D
    {
        internal void RotateText(float angleX, float angleY, float angleZ, RotationEventArgs.TypeAxis rotationAxis)
        {
            Matrix4 rotationMatrix = new Matrix4();

            switch (rotationAxis)
            {
                case RotationEventArgs.TypeAxis.X:
                    rotationMatrix = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angleX - this.RotationAngleX));
                    this._rotationAngleX = angleX;
                    break;
                case RotationEventArgs.TypeAxis.Y:
                    rotationMatrix = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(angleY - this.RotationAngleY));
                    this._rotationAngleY = angleY;
                    break;
                case RotationEventArgs.TypeAxis.Z:
                    rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angleZ - this.RotationAngleZ));
                    this._rotationAngleZ = angleZ;
                    break;
            }

            for (var arrayIndex = 0; arrayIndex < this.Triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this.Triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);

                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.X = rotatedVector.X;
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Y = rotatedVector.Y;
                        this.Triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position.Z = rotatedVector.Z;

                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxX();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxY();
                        this.Triangles[arrayIndex][triangleIndex].CalcMinMaxZ();
                    }
                }
            }

        }
    }
}