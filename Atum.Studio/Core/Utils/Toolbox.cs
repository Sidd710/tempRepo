using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using OpenTK;

namespace Atum.Studio.Core.Utils
{
    static class Toolbox
    {
        internal static T DeepClone<T>(T a)
        {
            using (var stream = new MemoryStream())
            {
                var formatter = new BinaryFormatter();
                formatter.Serialize(stream, a);
                stream.Position = 0;
                return (T)formatter.Deserialize(stream);
            }
        }

        internal static Vector3 RotateVectorX(Vector3 vector, float angle)
        {
            Matrix4 rotationMatrix = new Matrix4();

            rotationMatrix = Matrix4.CreateRotationX(MathHelper.DegreesToRadians(angle));
            var rotatedVector = Vector3.Transform(vector, rotationMatrix);
            return rotatedVector;
        }

        internal static Vector3 RotateVectorZ(Vector3 vector, float angle)
        {
            Matrix4 rotationMatrix = new Matrix4();

            rotationMatrix = Matrix4.CreateRotationZ(MathHelper.DegreesToRadians(angle));
            var rotatedVector = Vector3.Transform(vector, rotationMatrix);
            return rotatedVector;
        }
        //internal static Vector3 RotateYVector(Vector3 pointToRotate, Vector3 centerPoint, double angleInDegrees)
        //{
        //    double angleInRadians = angleInDegrees * (Math.PI / 180);
        //    double cosTheta = Math.Cos(angleInRadians);
        //    double sinTheta = Math.Sin(angleInRadians);
        //    return new Vector3
        //    {
        //        X =
        //            (float)
        //            (cosTheta * (pointToRotate.X - centerPoint.X) -
        //            sinTheta * (pointToRotate.Z - centerPoint.Z) + centerPoint.X),
        //        Z =
        //            (float)
        //            (sinTheta * (pointToRotate.X - centerPoint.X) +
        //            cosTheta * (pointToRotate.Z - centerPoint.Z) + centerPoint.Z),
        //        Y = pointToRotate.Y
        //    };
        //}

        //internal static Vector3 RotateXVector(Vector3 pointToRotate, Vector3 centerPoint, double angleInDegrees)
        //{
        //    double angleInRadians = angleInDegrees * (Math.PI / 180);
        //    double cosTheta = Math.Cos(angleInRadians);
        //    double sinTheta = Math.Sin(angleInRadians);
        //    return new Vector3
        //    {
        //        Y =
        //             (float)
        //             (cosTheta * (pointToRotate.Y - centerPoint.Y) -
        //             sinTheta * (pointToRotate.Z - centerPoint.Z) + centerPoint.Y),
        //        Z =
        //            (float)
        //            (sinTheta * (pointToRotate.Y - centerPoint.Y) +
        //            cosTheta * (pointToRotate.Z - centerPoint.Z) + centerPoint.Z),
        //        X = pointToRotate.X
        //    };
        //}

        //internal static Vector3 RotateZVector(Vector3 pointToRotate, Vector3 centerPoint, double angleInDegrees)
        //{
        //    double angleInRadians = angleInDegrees * (Math.PI / 180);
        //    double cosTheta = Math.Cos(angleInRadians);
        //    double sinTheta = Math.Sin(angleInRadians);
        //    return new Vector3
        //    {
        //        Y =
        //             (float)
        //             (cosTheta * (pointToRotate.X - centerPoint.X) -
        //             sinTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.X),
        //        X =
        //            (float)
        //            (sinTheta * (pointToRotate.X - centerPoint.X) +
        //            cosTheta * (pointToRotate.Y - centerPoint.Y) + centerPoint.Y),
        //        Z = pointToRotate.Z
        //    };
        //}
    }
}
