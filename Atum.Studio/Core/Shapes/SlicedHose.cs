using System;
using System.Collections.Generic;
using OpenTK;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.Shapes
{
	internal sealed class SlicedHose : DrawableShapeInfo
	{

        public enum eSide:byte
        {
            // Around X Axis
            BottomRight,
            TopRight,
            TopLeft,
            BottomLeft,

            // Around Y Axis

            FrontRight,
            BackRight,
            BackLeft,
            FrontLeft,

            // Around Z Axis
            FrontBottom,
            BackBottom,
            BackTop,
            FrontTop,
        }

        public SlicedHose( eSide side, uint subdivs, float scale, Vector3 offset1, Vector3 offset2, bool useDL )
            : base( useDL )
        {
            PrimitiveMode = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;

            Vector3 start = Vector3.Zero,
                     end = Vector3.Zero;

            switch ( side )
            {
            #region Around X Axis
            case eSide.BottomRight:
                start = -Vector3.UnitY;
                end = Vector3.UnitZ;
                break;
            case eSide.TopRight:
                start = Vector3.UnitZ;
                end = Vector3.UnitY;
                break;
            case eSide.TopLeft:
                start = Vector3.UnitY;
                end = -Vector3.UnitZ;
                break;
            case eSide.BottomLeft:
                start = -Vector3.UnitZ;
                end = -Vector3.UnitY;
                break;
            #endregion Around X Axis
            #region Around Y Axis
            case eSide.FrontRight:
                start = Vector3.UnitX;
                end = Vector3.UnitZ;
                break;
            case eSide.BackRight:
                start = Vector3.UnitZ;
                end = -Vector3.UnitX;
                break;
            case eSide.BackLeft:
                start = -Vector3.UnitX;
                end = -Vector3.UnitZ;
                break;
            case eSide.FrontLeft:
                start = -Vector3.UnitZ;
                end = Vector3.UnitX;
                break;
#endregion Around Y Axis
            #region Around Z Axis
            case eSide.FrontBottom:
                start = -Vector3.UnitY;
                end = Vector3.UnitX;
                break;
            case eSide.BackBottom:
                start = -Vector3.UnitX;
                end = -Vector3.UnitY;
                break;
            case eSide.BackTop:
                start = Vector3.UnitY;
                end = -Vector3.UnitX;
                break;
            case eSide.FrontTop:
                start = Vector3.UnitX;
                end = Vector3.UnitY;
                break;
#endregion Around Z Axis

            }

            Vertex[] temp = new Vertex[2 + subdivs];

            double divisor = 1.0/ ((double)temp.Length-1.0);
            for ( int i = 0; i < temp.Length; i++ )
            {
                float Multiplier = (float)( i * divisor );

                Slerp( ref start, ref end, Multiplier, out temp[i].Normal );
                temp[i].Normal.Normalize();
                temp[i].Position = temp[i].Normal;
                temp[i].Position *= scale;
            }

            VertexArray = new Vertex[temp.Length * 2];
            IndexArray = new uint[( temp.Length - 1 ) * 2 * 3];

            uint VertexCounter = 0,
                 IndexCounter = 0,
                 QuadCounter = 0;

            for ( int i = 0; i < temp.Length; i++ )
            {
                VertexArray[VertexCounter + 0].Position = temp[i].Position + offset1;
                VertexArray[VertexCounter + 1].Position = temp[i].Position + offset2;
                VertexCounter += 2;

                if ( i < temp.Length - 1 )
                {
                    IndexArray[IndexCounter + 0] = QuadCounter + 0;
                    IndexArray[IndexCounter + 1] = QuadCounter + 1;
                    IndexArray[IndexCounter + 2] = QuadCounter + 2;

                    IndexArray[IndexCounter + 3] = QuadCounter + 2;
                    IndexArray[IndexCounter + 4] = QuadCounter + 1;
                    IndexArray[IndexCounter + 5] = QuadCounter + 3;

                    IndexCounter += 6;
                    QuadCounter += 2;
                }
            }

        }
 
    private void Slerp( ref Vector3 a, ref Vector3 b, float factor, out Vector3 result)
    {
        float t1;
        Vector3.Dot( ref a, ref b, out t1 );
        double theta = System.Math.Acos( t1 );

        float temp = (float)(1.0 / System.Math.Sin(theta));
        float t2 = (float)(System.Math.Sin( ( 1.0 - factor ) * theta ) * temp);
        float t3 = (float)(System.Math.Sin(factor * theta) * temp);

        Vector3 v1 = Vector3.Multiply( a, t2);
        Vector3 v2 = Vector3.Multiply( b, t3 );
        result = Vector3.Add( v1, v2 );
    }


    }
}
