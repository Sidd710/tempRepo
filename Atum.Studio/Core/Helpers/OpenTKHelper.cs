using Atum.Studio.Controls;
using Atum.Studio.Core.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Core.Helpers
{
    internal static class OpenTKHelper
    {
        public static bool IsValidVersion()
        {
            var version = new Version(GL.GetString(StringName.Version).Substring(0, 3));
            var target = new Version(2, 0);

            if (version < target)
            {
                new frmMessageBox("Computer does not meet requirements",
                    String.Format("This computer does not meet the requirement below: " + Environment.NewLine + " - Current OpenGL version {0}: Required version >{1}."
                    + Environment.NewLine + Environment.NewLine
                    + Managers.BrandingManager.MainForm_StudioName + " will be closed after pressing [OK]", version, target)
                    ,MessageBoxButtons.OK, MessageBoxDefaultButton.Button2);
                return false;
            }

            return true;
        }

        public static Vector3Class Project(Vector3Class vector, Matrix4 projection, Matrix4 view, Matrix4 world)
        {
            Matrix4 worldViewProjection = Matrix4.Mult(Matrix4.Mult(world, view), projection);

            Vector4 result;

            result.X =
                vector.X * worldViewProjection.M11 +
                vector.Y * worldViewProjection.M21 +
                vector.Z * worldViewProjection.M31 +
                worldViewProjection.M41;
            result.Y =
                vector.X * worldViewProjection.M12 +
                vector.Y * worldViewProjection.M22 +
                vector.Z * worldViewProjection.M32 +
                worldViewProjection.M42;
            result.Z =
                vector.X * worldViewProjection.M13 +
                vector.Y * worldViewProjection.M23 +
                vector.Z * worldViewProjection.M33 +
                worldViewProjection.M43;
            result.W =
                vector.X * worldViewProjection.M14 +
                vector.Y * worldViewProjection.M24 +
                vector.Z * worldViewProjection.M34 +
                worldViewProjection.M44;

            result /= result.W;


            result.X = (frmStudioMain.SceneControl.Width * ((result.X + 1.0f) / 2.0f));
            result.Y = (frmStudioMain.SceneControl.Height * ((result.Y + 1.0f) / 2.0f));

            //result.Z = minZ + ((maxZ - minZ) * ((result.Z + 1.0f) / 2.0f)); 

            return new Vector3Class(result.X, result.Y, result.Z);
        }

        private static bool WithinEpsilon(float a, float b)
        {
            float num = a - b;

            return ((-1.401298E-45f <= num) && (num <= float.Epsilon));
        }


        public static Vector2 Subtract(this Vector2 v, Vector2 w)
        {
            return new Vector2(v.X - w.X, v.Y - w.Y);
        }

        public static Vector2 Add(this Vector2 v, Vector2 w)
        {
            return new Vector2(v.X + w.X, v.Y + w.Y);
        }

        public static double Dot(this Vector2 v, Vector2 w)
        {
            return v.X * w.X + v.Y * w.Y;
        }

        public static Vector2 Dot(this Vector2 v, double mult)
        {
            return new Vector2((float)(v.X * mult), (float)(v.Y * mult));
        }

        public static double Cross(this Vector2 xy, Vector2 v)
        {
            return xy.X * v.Y - xy.Y * v.X;
        }

        private const double Epsilon = 1e-10;

        public static bool IsZero(this double d)
        {
            return Math.Abs(d) < Epsilon;
        }
    }
}
