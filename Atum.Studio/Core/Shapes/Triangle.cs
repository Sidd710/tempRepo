using System;
using System.Collections.Generic;
using Atum.Studio.Core.Structs;
using OpenTK;
using System.Threading;
using Atum.Studio.Core.Models;
using System.ComponentModel;
using Atum.Studio.Core.ModelView;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Utils;
using Atum.Studio.Core.Events;
using System.Diagnostics;
using Atum.Studio.Core.Engines.MagsAI;
using System.Linq;
using Atum.Studio.Core.Helpers;
using System.Collections.Concurrent;
using Atum.DAL.Managers;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Shapes
{
    [Serializable]
    public class Triangle : IDisposable, ICloneable
    {
        public float Volume;
        public VertexClass[] Vectors;
        public Vector3Class[] PreservedVectors;
        public Vector3Class Center;
        public TriangleConnectionInfo Index;
        public Vector3Class Normal { get; set; }
        public float AngleZ;
        public float Top;
        public float Bottom;
        public float Front;
        public float Back;
        public float Left;
        public float Right;

        public TriangleConnectionInfo[] ChildConnections;

        public typeTriangleProperties Properties { get; set; }

        public enum typeTriangleProperties : short
        {
            None = 0,
            HorizontalEdgeUp = 1,
            HorizontalEdgeDown = 2,

            AutoRotationSelected = 4,
            AutoRotationDeselected = 8
        }

        internal SliceLine3D[] SliceLines3D
        {
            get
            {
                var sliceLines = new SliceLine3D[3];
                sliceLines[0] = new SliceLine3D(this.Vectors[0].Position, this.Vectors[1].Position);
                sliceLines[1] = new SliceLine3D(this.Vectors[1].Position, this.Vectors[2].Position);
                sliceLines[2] = new SliceLine3D(this.Vectors[2].Position, this.Vectors[0].Position);

                return sliceLines;
            }
        }

        public Vector3Class[] Points
        {
            get
            {
                var result = new Vector3Class[3];
                result[0] = this.Vectors[0].Position;
                result[1] = this.Vectors[1].Position;
                result[2] = this.Vectors[2].Position;
                return result;
            }
        }



        public Triangle()
        {
            this.Vectors = new VertexClass[3];
            this.Vectors[0] = new VertexClass();
            this.Vectors[1] = new VertexClass();
            this.Vectors[2] = new VertexClass();
            this.Index = new TriangleConnectionInfo();
            this.Center = new Vector3Class();
            this.Normal = new Vector3Class();
        }

        public void CalcAngleZ()
        {
            this.AngleZ = OpenTK.MathHelper.RadiansToDegrees(Vector3Class.CalculateAngle(this.Normal, Vector3Class.UnitZ));
        }



        //internal Triangle TriangleWithMoveTranslation(Vector3 moveTranslation)
        //{
        //    var triangle = new Triangle();
        //    triangle.Vectors[0].Position = this.Vectors[0].Position + new Vector3(moveTranslation.X, moveTranslation.Y, 0);
        //    triangle.Vectors[1].Position = this.Vectors[1].Position + new Vector3(moveTranslation.X, moveTranslation.Y, 0);
        //    triangle.Vectors[2].Position = this.Vectors[2].Position + new Vector3(moveTranslation.X, moveTranslation.Y, 0);
        //    return triangle;
        //}

        public void CalcNormal()
        {
            var u = Vectors[1].Position - Vectors[0].Position;
            var normal = new Vector3Class();
            var v = Vectors[2].Position - Vectors[0].Position;
            normal.X = (u.Y * v.Z) - (u.Z * v.Y);
            normal.Y = (u.Z * v.X) - (u.X * v.Z);
            normal.Z = (u.X * v.Y) - (u.Y * v.X);
            normal = Vector3Class.Normalize(normal);
            this.Normal = normal;

            this.CalcAngleZ();
        }

        public void CalcVolume()
        {
            if (this.Volume == 0)
            {
                this.Volume = (this.Vectors[1].Position - this.Vectors[0].Position).Length + (this.Vectors[2].Position - this.Vectors[1].Position).Length + (this.Vectors[0].Position - this.Vectors[2].Position).Length;
                this.Volume = this.Volume / 2;
                this.Volume = (float)Math.Sqrt((double)this.Volume);
            }
        }

        public float CalcSignedVolume()
        {
            var cross = Vector3Class.Cross(this.Vectors[1].Position, this.Vectors[2].Position);
            return (Vector3Class.Dot(this.Vectors[0].Position, cross)) / 6.0f;
        }

        public void Flip(bool updateModel = true)
        {
            var temp = this.Vectors[1].Position;
            this.Vectors[1].Position = this.Vectors[2].Position;
            this.Vectors[2].Position = temp;

            if (updateModel)
            {
                this.CalcNormal();;
            }
        }

        public bool HPointInside(float x, float y)
        {
            var result = false;

            var planeAB = (this.Vectors[0].Position.X - x) * (this.Vectors[1].Position.Y - y) - (this.Vectors[1].Position.X - x) * (this.Vectors[0].Position.Y - y);
            var planeBC = (this.Vectors[1].Position.X - x) * (this.Vectors[2].Position.Y - y) - (this.Vectors[2].Position.X - x) * (this.Vectors[1].Position.Y - y);
            var planeCA = (this.Vectors[2].Position.X - x) * (this.Vectors[0].Position.Y - y) - (this.Vectors[0].Position.X - x) * (this.Vectors[2].Position.Y - y);

            if (Sign(planeAB) == Sign(planeBC) && Sign(planeBC) == Sign(planeCA))
            {
                return true;
            }

            return result;
        }

        private float Sign(float n)
        {
            return Math.Abs(n) / n;
        }

        public bool IsMAGSAIMarked
        {
            get
            {
                return (this.Properties & Triangle.typeTriangleProperties.AutoRotationSelected) == Triangle.typeTriangleProperties.AutoRotationSelected;
            }
        }

        public void CalcCenter()
        {
            try
            {
                var w_previous = (this.Vectors[2].Position - this.Vectors[0].Position).Length;
                var w_current = 0f;
                var w = -float.MaxValue;
                var wTotal = 0.0f;
                var r_center = new Vector3Class();
                //Vector3.

                for (var i = 0; i < 3; i++)
                {
                    var vertex = this.Vectors[i];
                    if (i < 2)
                    {
                        w_current = (this.Vectors[i].Position - this.Vectors[i + 1].Position).Length;
                    }
                    else
                    {
                        w_current = (this.Vectors[2].Position - this.Vectors[0].Position).Length;
                    }

                    w = (w_current + w_previous);
                    r_center += Vector3Class.Multiply(vertex.Position, w);

                    wTotal += w;
                    w_previous = w_current;
                }

                this.Center = Vector3Class.Multiply(r_center, 1.0f / wTotal);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                //DebugLogger.Instance().LogError(ex.Message);
            }
        }

        internal void CalcMinMaxZ()
        {
            this.Top = this.Vectors[0].Position.Z;
            this.Bottom = this.Vectors[0].Position.Z;
            for (var vectorIndex = 1; vectorIndex < 3; vectorIndex++)
            {
                this.Bottom = Math.Min(this.Vectors[vectorIndex].Position.Z, this.Bottom);
                this.Top = Math.Max(this.Vectors[vectorIndex].Position.Z, this.Top);
            }
        }

        internal void CalcMinMaxY()
        {
            this.Front = this.Back = this.Vectors[0].Position.Y;
            for (var vectorIndex = 1; vectorIndex < 3; vectorIndex++)
            {
                this.Back = this.Vectors[vectorIndex].Position.Y < this.Back ? this.Vectors[vectorIndex].Position.Y : this.Back;
                this.Front = this.Vectors[vectorIndex].Position.Y > this.Front ? this.Vectors[vectorIndex].Position.Y : this.Front;
            }
        }

        internal void CalcMinMaxX()
        {
            this.Left = this.Right = this.Vectors[0].Position.X;
            for (var vectorIndex = 1; vectorIndex < 3; vectorIndex++)
            {
                this.Left = this.Vectors[vectorIndex].Position.X < this.Left ? this.Vectors[vectorIndex].Position.X : this.Left;
                this.Right = this.Vectors[vectorIndex].Position.X > this.Right ? this.Vectors[vectorIndex].Position.X : this.Right;
            }
        }

        internal void UpdateColor(Byte4Class object3dColor, bool forceColor = false)
        {
            var color = new Byte4Class(object3dColor.A, object3dColor.R, object3dColor.G, object3dColor.B);

            if (!forceColor)
            {
                if (RegistryManager.RegistryProfile.DebugMode)
                {
                    if ((this.AngleZ >= 175 && this.AngleZ <= 185))
                    {
                        color.R = (byte)Atum.Studio.Properties.Settings.Default.FaceHorizontal.R;
                        color.G = (byte)Atum.Studio.Properties.Settings.Default.FaceHorizontal.G;
                        color.B = (byte)Atum.Studio.Properties.Settings.Default.FaceHorizontal.B;
                    }
                    else if (this.AngleZ > 160 && this.AngleZ < 200)
                    {
                        color.R = (byte)Atum.Studio.Properties.Settings.Default.FaceFacingDown.R;
                        color.G = (byte)Atum.Studio.Properties.Settings.Default.FaceFacingDown.G;
                        color.B = (byte)Atum.Studio.Properties.Settings.Default.FaceFacingDown.B;
                    }
                    else
                    {
                        color.R = (byte)object3dColor.R;
                        color.G = (byte)object3dColor.G;
                        color.B = (byte)object3dColor.B;
                    }
                }
                else
                {

                    color.R = (byte)object3dColor.R;
                    color.G = (byte)object3dColor.G;
                    color.B = (byte)object3dColor.B;
                }
            }
            else
            {
                color.R = (byte)object3dColor.R;
                color.G = (byte)object3dColor.G;
                color.B = (byte)object3dColor.B;
            }

            for (var vertexIndex = 0; vertexIndex < 3; vertexIndex++)
            {
                this.Vectors[vertexIndex].Color = color;
            }
        }


        public float SquaredLen
        {
            get
            {
                var result = this.Vectors[1].Position - this.Vectors[0].Position;
                return Vector3Class.Dot(result, result);
            }
        }

        public SlicePolyLine3D IntersectZPlane(float zcur)
        {
            try
            {
                var segment = new SlicePolyLine3D();

                Vector3Class zIntersectionPoint;
                var sliceLines = this.SliceLines3D;
                zIntersectionPoint = sliceLines[0].IntersectZ(zcur);
                if (zIntersectionPoint != null)
                {
                    segment.Points.Add(zIntersectionPoint);

                }

                zIntersectionPoint = sliceLines[1].IntersectZ(zcur);
                if (zIntersectionPoint != null)
                {
                    segment.Points.Add(zIntersectionPoint);
                }

                if (segment.Points.Count == 0)
                    return null;

                // there is no sense in doing the 3rd intersection if we don't have 
                // at least 1 point at this stage
                if (segment.Points.Count == 1)
                {
                    zIntersectionPoint = sliceLines[2].IntersectZ(zcur);
                    if (zIntersectionPoint != null)
                    {
                        segment.Points.Add(zIntersectionPoint);
                    }
                }

                if (segment.Points.Count != 2) // might be 0,1 or 3
                    return null;

                return segment;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public void Dispose()
        {
            GC.SuppressFinalize(this);
        }

        ~Triangle()
        {
            this.Dispose();
        }

        public object Clone()
        {
            var cloneTriangle = new Triangle();
            cloneTriangle.Center = new Vector3Class(this.Center.X, this.Center.Y, this.Center.Z);
            cloneTriangle.AngleZ = this.AngleZ;
            cloneTriangle.Properties = this.Properties;
            cloneTriangle.Volume = this.Volume;
            cloneTriangle.Normal = new Vector3Class(this.Normal);
            cloneTriangle.Vectors[0].Color = new Byte4Class(this.Vectors[0].Color.A, this.Vectors[0].Color.R, this.Vectors[0].Color.G, this.Vectors[0].Color.B);
            cloneTriangle.Vectors[1].Color = new Byte4Class(this.Vectors[1].Color.A, this.Vectors[1].Color.R, this.Vectors[1].Color.G, this.Vectors[1].Color.B);
            cloneTriangle.Vectors[2].Color = new Byte4Class(this.Vectors[2].Color.A, this.Vectors[2].Color.R, this.Vectors[2].Color.G, this.Vectors[2].Color.B);
            cloneTriangle.Vectors[0].Position = new Vector3Class(this.Vectors[0].Position.X, this.Vectors[0].Position.Y, this.Vectors[0].Position.Z);
            cloneTriangle.Vectors[1].Position = new Vector3Class(this.Vectors[1].Position.X, this.Vectors[1].Position.Y, this.Vectors[1].Position.Z);
            cloneTriangle.Vectors[2].Position = new Vector3Class(this.Vectors[2].Position.X, this.Vectors[2].Position.Y, this.Vectors[2].Position.Z);

            return cloneTriangle;
        }

        public bool ContainsVector3(Vector3Class point)
        {
            //calc if point is inside triangle
            var pointInsideTriangle = false;
            var p1 = this.Vectors[0].Position;
            var p2 = this.Vectors[1].Position;
            var p3 = this.Vectors[2].Position;
            var direction = new Vector3Class(0, 0, 1);

            for (var i = 0; i < 2; i++)
            {
                //pass 1
                if (i == 0)
                {
                    if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, point - p3), direction) > 0) continue;
                    if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, point - p1), direction) > 0) continue;
                    if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, point - p2), direction) > 0) continue;
                }
                else
                {
                    if (Vector3Class.Dot(Vector3Class.Cross(p1 - p3, point - p3), direction) < 0) continue;
                    if (Vector3Class.Dot(Vector3Class.Cross(p2 - p1, point - p1), direction) < 0) continue;
                    if (Vector3Class.Dot(Vector3Class.Cross(p3 - p2, point - p2), direction) < 0) continue;
                }

                pointInsideTriangle = true;

            }

            return pointInsideTriangle;
        }

        internal void HorizontalMirror(bool updateModel = true)
        {
            this.Vectors[0].Position = new Vector3Class(-this.Vectors[0].Position.X, this.Vectors[0].Position.Y, this.Vectors[0].Position.Z);
            this.Vectors[1].Position = new Vector3Class(-this.Vectors[1].Position.X, this.Vectors[1].Position.Y, this.Vectors[1].Position.Z);
            this.Vectors[2].Position = new Vector3Class(-this.Vectors[2].Position.X, this.Vectors[2].Position.Y, this.Vectors[2].Position.Z);
            this.Flip(updateModel);

            if (updateModel)
            {
                this.CalcNormal();
                this.CalcCenter();
                this.CalcMinMaxX();
                this.CalcAngleZ();
            }
        }

        internal void VerticalMirror(bool updateModel = true)
        {
            this.Vectors[0].Position = new Vector3Class(this.Vectors[0].Position.X, -this.Vectors[0].Position.Y, this.Vectors[0].Position.Z);
            this.Vectors[1].Position = new Vector3Class(this.Vectors[1].Position.X, -this.Vectors[1].Position.Y, this.Vectors[1].Position.Z);
            this.Vectors[2].Position = new Vector3Class(this.Vectors[2].Position.X, -this.Vectors[2].Position.Y, this.Vectors[2].Position.Z);
            this.Flip(updateModel);

            if (updateModel)
            {
                this.CalcNormal();
                this.CalcCenter();
                this.CalcMinMaxX();
                this.CalcAngleZ();
            }
        }
    }


    public class TriangleArrayConnectionInfo
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }

        internal TriangleArrayConnectionInfo(ushort arrayIndex, ushort triangleIndex)
        {
            this.ArrayIndex = arrayIndex;
            this.TriangleIndex = triangleIndex;
        }
    }

    [Serializable]
    public class TriangleConnectionInfo
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }

        public override bool Equals(object obj)
        {
            var otherTriangle = (TriangleConnectionInfo)obj;
            return this.ArrayIndex == otherTriangle.ArrayIndex && this.TriangleIndex == otherTriangle.TriangleIndex;
        }

        private int _hashcode;

        public override int GetHashCode()
        {
            if (this._hashcode == 0)
            {
                this._hashcode = ((this.ArrayIndex * 33333) + this.TriangleIndex);
            }
            return this._hashcode;
        }

    }

    [Serializable]
    public class TriangleConnectionVectorInfo
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }
        public ushort VectorIndex { get; set; }
    }

    public class TriangleConnectionInfoWithEdge
    {
        public ushort ArrayIndex { get; set; }
        public int TriangleIndex { get; set; }
        public Vector3Class P1 { get; set; }
        public Vector3Class P2 { get; set; }

        public bool HasHardEdgeDown { get; set; }
    }

    [Serializable]
    public class TriangleSurfaceContourConnectionInfo
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }
        public ushort VectorIndex { get; set; }
        public Vector3Class Normal { get; set; }
        public Vector3Class StartPoint { get; set; }
        public Vector3Class EndPoint { get; set; }
        public bool HorizontalEdge { get; set; }

        public void FlipPoints()
        {
            var tmpStartPoint = new Vector3Class(this.StartPoint.X, this.StartPoint.Y, this.StartPoint.Z);
            this.StartPoint = this.EndPoint;
            this.EndPoint = tmpStartPoint;
        }
    }

    public class TriangleConnectionInfoList : List<List<List<TriangleConnectionInfo>>> { }

    internal class TriangleConnectedPoints : Dictionary<Vector3Class, List<TriangleConnectionInfo>>
    {
        internal TriangleConnectedPoints()
        {

        }

        internal void UpdateKeys(TriangleInfoList triangles)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            this.Clear();
            for (var arrayIndex = 0; arrayIndex < triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var vectorPoint = triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;
                        if (!this.ContainsKey(vectorPoint))
                        {
                            this.Add(vectorPoint, new List<TriangleConnectionInfo>());
                        }

                        if (arrayIndex == triangles[arrayIndex][triangleIndex].Index.ArrayIndex && triangleIndex == triangles[arrayIndex][triangleIndex].Index.TriangleIndex)
                        {
                            this[vectorPoint].Add(triangles[arrayIndex][triangleIndex].Index);//add by reference
                        }
                        else
                        {
                            this[vectorPoint].Add(new TriangleConnectionInfo() { ArrayIndex = (ushort)arrayIndex, TriangleIndex = (ushort)triangleIndex });
                        }
                    }
                }
            }

            Debug.WriteLine("Connected point update time: " + stopwatch.ElapsedMilliseconds + "ms");
        }
    }


    [Serializable]
    public class TriangleInfoList : List<List<Triangle>>, ICloneable
    {
        public Vector3Class InitialTranslation { get; set; }
        public int TriangleArrayCount
        {
            get
            {
                return this.Count;
            }
        }

        public TriangleInfoList()
        {
            this.Add(new List<Triangle>());
            //this.Connections = new List<List<TriangeConnectionsIndexed>>();
        }

        //internal List<List<TriangeConnectionsIndexed>> Connections { get; set; }

        public TriangleConnectionInfo[] GetConnectedTriangles(TriangleConnectionInfo triangleParentConnection)
        {
            if (this[triangleParentConnection.ArrayIndex][triangleParentConnection.TriangleIndex].ChildConnections == null)
            {
                if (this.EdgePoints == null || this.EdgePoints.Count == 0)
                {
                    this.UpdateConnections();
                }
                var parentTriangle = this[triangleParentConnection.ArrayIndex][triangleParentConnection.TriangleIndex];
                var parentEdgeConnections = new List<TriangleConnectionVectorInfo>();
                if (!this.EdgePoints.ContainsKey(parentTriangle.Vectors[0].Position))
                {
                    this.UpdateConnections();
                }
                parentEdgeConnections.AddRange(this.EdgePoints[parentTriangle.Vectors[0].Position]);
                parentEdgeConnections.AddRange(this.EdgePoints[parentTriangle.Vectors[1].Position]);
                parentEdgeConnections.AddRange(this.EdgePoints[parentTriangle.Vectors[2].Position]);
                //convert edgeconnections to array/triangle index

                var temp = new Dictionary<TriangleConnectionInfo, bool>();

                foreach (var parentEdgeConnection in parentEdgeConnections)
                {
                    if (!(triangleParentConnection.ArrayIndex == parentEdgeConnection.ArrayIndex && triangleParentConnection.TriangleIndex == parentEdgeConnection.TriangleIndex))
                    {
                        var connectionInfo = this[parentEdgeConnection.ArrayIndex][parentEdgeConnection.TriangleIndex].Index;
                        if (!temp.ContainsKey(connectionInfo))
                        {
                            temp.Add(connectionInfo, false);
                        }
                    }
                }

                this[triangleParentConnection.ArrayIndex][triangleParentConnection.TriangleIndex].ChildConnections = temp.Keys.ToArray();
            }

            return this[triangleParentConnection.ArrayIndex][triangleParentConnection.TriangleIndex].ChildConnections;
        }

        public List<TriangleConnectionInfo> GetConnectedTrianglesExtrudeModel(TriangleConnectionInfo triangleParentConnection)
        {
            if (this.EdgePoints == null || this.EdgePoints.Count == 0)
            {
                this.UpdateConnections();
            }
            var result = new List<TriangleConnectionInfo>();
            var parentTriangle = this[triangleParentConnection.ArrayIndex][triangleParentConnection.TriangleIndex];
            var parentEdgeConnections = new List<TriangleConnectionVectorInfo>();
            parentEdgeConnections.AddRange(this.EdgePoints[parentTriangle.Vectors[0].Position]);
            parentEdgeConnections.AddRange(this.EdgePoints[parentTriangle.Vectors[1].Position]);
            parentEdgeConnections.AddRange(this.EdgePoints[parentTriangle.Vectors[2].Position]);
            //convert edgeconnections to array/triangle index

            foreach (var parentEdgeConnection in parentEdgeConnections)
            {
                if (!(triangleParentConnection.ArrayIndex == parentEdgeConnection.ArrayIndex && triangleParentConnection.TriangleIndex == parentEdgeConnection.TriangleIndex))
                {
                    var connectionInfo = new TriangleConnectionInfo() { ArrayIndex = parentEdgeConnection.ArrayIndex, TriangleIndex = parentEdgeConnection.TriangleIndex };
                    if (!result.Any(s => s.ArrayIndex == parentEdgeConnection.ArrayIndex && s.TriangleIndex == parentEdgeConnection.TriangleIndex))
                    {
                        result.Add(connectionInfo);
                    }
                }
            }

            return result;
        }

        public TriangleInfoList(TriangleInfoList triangleList)
        {
            this.Add(new List<Triangle>());
            this[0] = triangleList[0];
        }

        internal void GetVertexArray(int arrayIndex, bool hidden, ref Vertex[] vertexArray)
        {
            vertexArray = new Vertex[this[arrayIndex].Count * 3];
            for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
            {
                vertexArray[triangleIndex * 3] = this[arrayIndex][triangleIndex].Vectors[0].ToStruct();
                vertexArray[triangleIndex * 3 + 1] = this[arrayIndex][triangleIndex].Vectors[1].ToStruct();
                vertexArray[triangleIndex * 3 + 2] = this[arrayIndex][triangleIndex].Vectors[2].ToStruct();
                vertexArray[triangleIndex * 3].Normal = vertexArray[triangleIndex * 3 + 1].Normal = vertexArray[triangleIndex * 3 + 2].Normal = this[arrayIndex][triangleIndex].Normal.ToStruct();
            }

            if (hidden)
            {
                for (var vertexIndex = 0; vertexIndex < vertexArray.Length; vertexIndex++)
                {
                    vertexArray[vertexIndex].Position = new Vector3();
                }
            }
        }


        internal void UpdateWithMoveTranslation(Vector3Class moveTranslation)
        {
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position += moveTranslation;
                    }

                    this[arrayIndex][triangleIndex].CalcCenter();
                    this[arrayIndex][triangleIndex].CalcMinMaxX();
                    this[arrayIndex][triangleIndex].CalcMinMaxY();
                    this[arrayIndex][triangleIndex].CalcMinMaxZ();
                }
            }
        }

        public void CreatePreservedVectors()
        {
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangeIndex = 0; triangeIndex < this[arrayIndex].Count; triangeIndex++)
                {
                    this[arrayIndex][triangeIndex].PreservedVectors = new Vector3Class[3];

                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        this[arrayIndex][triangeIndex].PreservedVectors[vectorIndex] = new Vector3Class(this[arrayIndex][triangeIndex].Vectors[vectorIndex].Position);
                    }
                }
            }
        }

        public void RevertPreservedVectors()
        {
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        if (this[arrayIndex][triangleIndex].PreservedVectors != null)
                        {
                            this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = new Vector3Class(this[arrayIndex][triangleIndex].PreservedVectors[vectorIndex]);
                        }
                    }

                    this[arrayIndex][triangleIndex].CalcMinMaxZ();

                    this[arrayIndex][triangleIndex].CalcCenter();
                    this[arrayIndex][triangleIndex].CalcMinMaxX();
                    this[arrayIndex][triangleIndex].CalcMinMaxY();
                    this[arrayIndex][triangleIndex].CalcNormal();
                }
            }
        }



        private void GetTriangleEdgeIntersection(List<TriangleSurfaceContourConnectionInfo> t, ushort arrayIndex, ushort triangleIndex, Vector3Class source, Vector3Class destination, ushort vectorIndex, Triangle triangleToIntersect, float zPoint)
        {
            var triangleConnection = new TriangleSurfaceContourConnectionInfo()
            {
                ArrayIndex = arrayIndex,
                TriangleIndex = triangleIndex,
                VectorIndex = vectorIndex,
                Normal = Vector3Class.Normalize(destination - source)
            };


            var t2 = IntersectionProvider.IntersectTriangle(source, Vector3Class.Normalize(destination - source), triangleToIntersect, false);
            t2.Z = zPoint;
            //if (DebugLines == null) { DebugLines = new List<Vector3>(); }

            var triangleFound = false;
            foreach (var triangleContour in t)
            {
                if (triangleContour.ArrayIndex == arrayIndex && triangleContour.TriangleIndex == triangleIndex)
                {
                    triangleContour.EndPoint = t2;
                    triangleFound = true;

                    if (triangleContour.StartPoint == triangleContour.EndPoint)
                    {
                        t.Remove(triangleContour);
                        break;
                    }
                    break;
                }
            }

            if (!triangleFound)
            {
                triangleConnection.StartPoint = t2;
                t.Add(triangleConnection);
            }
        }




        public object Clone()
        {
            return new TriangleInfoList(this);
        }


        private int? _totalTrianglesCount;

        public int TotalTrianglesCount
        {
            get
            {
                if (!_totalTrianglesCount.HasValue)
                    _totalTrianglesCount = this.Sum(t => t.Count);

                return _totalTrianglesCount.Value;
            }
        }

        private int? _totalVerticesCount = 0;

        public int TotalVerticesCount
        {
            get
            {
                if ((!_totalVerticesCount.HasValue || _totalVerticesCount == 0))
                    _totalVerticesCount = TotalTrianglesCount * 3;
                return _totalVerticesCount.Value;
            }
        }



        internal Dictionary<Vector3Class, List<TriangleConnectionVectorInfo>> EdgePoints { get; set; }

        internal bool HasTriangleOverhangSide(float sliceHeight, STLModel3D stlModel)
        {
            var vector1 = new Vector3Class();
            var vector2 = new Vector3Class();
            for (var triangleIndex = 0; triangleIndex < stlModel.SliceIndexes[sliceHeight].Count; triangleIndex++)
            {
                var triangleConnection = stlModel.SliceIndexes[sliceHeight][triangleIndex];
                var hasOverhangAngle = VectorHelper.HasTriangleOverhangSide(this[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex], out vector1, out vector2);

                var thirdOverhangVector = this[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex].Vectors[0].Position;
                if (thirdOverhangVector == vector1 || thirdOverhangVector == vector2)
                {
                    thirdOverhangVector = this[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex].Vectors[1].Position;
                }
                if (thirdOverhangVector == vector1 || thirdOverhangVector == vector2)
                {
                    thirdOverhangVector = this[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex].Vectors[2].Position;
                }

                //find connected triangleindexes
                if (hasOverhangAngle)
                {
                    //check side connected triangles if they have a lower position
                    var lowestPointZ = Math.Max(vector1.Z, vector2.Z);
                    //var vector1EdgeConnections = this.EdgePoints[vector1];
                    var connectedEdgeTrianglesIndexes = new List<TriangleConnectionInfo>();
                    if (thirdOverhangVector.Z >= lowestPointZ)
                    {
                        foreach (var childConnection in this[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex].ChildConnections)
                        {
                            if (childConnection != null)
                            {
                                connectedEdgeTrianglesIndexes.Add(childConnection);
                            }
                        }

                        foreach (var connectedEdgeTriangleIndex in connectedEdgeTrianglesIndexes)
                        {
                            var connectedEdgeTriangle = this[connectedEdgeTriangleIndex.ArrayIndex][connectedEdgeTriangleIndex.TriangleIndex];
                            var thirdVector = connectedEdgeTriangle.Vectors[0].Position;
                            if (thirdVector == vector1 || thirdVector == vector2)
                            {
                                thirdVector = connectedEdgeTriangle.Vectors[1].Position;
                            }
                            if (thirdVector == vector1 || thirdVector == vector2)
                            {
                                thirdVector = connectedEdgeTriangle.Vectors[2].Position;
                            }

                            if (thirdVector.Z < Math.Max(vector1.Z, vector2.Z))
                            {
                                return true;
                            }
                        }
                    }
                }
            }

            return false;
        }

        internal ConcurrentDictionary<int, float> CalcOverhangSliceIndex(STLModel3D stlModel)
        {
            LoggingManager.WriteToLog("CalcOverhangSliceIndex", "Started", "");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            var result = new ConcurrentDictionary<int, float>();

            var vector1 = new Vector3Class();
            var vector2 = new Vector3Class();
            var overhangTriangleZPoints = new ConcurrentDictionary<float, List<TriangleConnectionVectorInfo>>();
            Parallel.For(0, this.Count, arrayIndexAsync =>
             {
                 var arrayIndex = arrayIndexAsync;
                 for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                 {
                     var hasOverhangAngle = VectorHelper.HasTriangleOverhangSide(this[arrayIndex][triangleIndex], out vector1, out vector2);

                     var thirdOverhangVector = this[arrayIndex][triangleIndex].Vectors[0].Position;
                     if (thirdOverhangVector == vector1 || thirdOverhangVector == vector2)
                     {
                         thirdOverhangVector = this[arrayIndex][triangleIndex].Vectors[1].Position;
                     }
                     if (thirdOverhangVector == vector1 || thirdOverhangVector == vector2)
                     {
                         thirdOverhangVector = this[arrayIndex][triangleIndex].Vectors[2].Position;
                     }

                     //find connected triangleindexes
                     if (hasOverhangAngle)
                     {
                         //check side connected triangles if they have a lower position
                         var lowestPointZ = Math.Max(vector1.Z, vector2.Z);
                         if (!overhangTriangleZPoints.ContainsKey(lowestPointZ))
                         {
                             var hasLowerConnectedSideTriangle = false;
                             //var vector1EdgeConnections = this.EdgePoints[vector1];
                             var connectedEdgeTrianglesIndexes = new List<TriangleConnectionInfo>();
                             if (thirdOverhangVector.Z >= lowestPointZ)
                             {
                                 foreach (var childConnection in this[arrayIndex][triangleIndex].ChildConnections)
                                 {
                                     if (childConnection != null)
                                     {
                                         connectedEdgeTrianglesIndexes.Add(childConnection);
                                     }
                                 }

                                 foreach (var connectedEdgeTriangleIndex in connectedEdgeTrianglesIndexes)
                                 {
                                     var connectedEdgeTriangle = this[connectedEdgeTriangleIndex.ArrayIndex][connectedEdgeTriangleIndex.TriangleIndex];
                                     var thirdVector = connectedEdgeTriangle.Vectors[0].Position;
                                     if (thirdVector == vector1 || thirdVector == vector2)
                                     {
                                         thirdVector = connectedEdgeTriangle.Vectors[1].Position;
                                     }
                                     if (thirdVector == vector1 || thirdVector == vector2)
                                     {
                                         thirdVector = connectedEdgeTriangle.Vectors[2].Position;
                                     }

                                     if (thirdVector.Z < Math.Max(vector1.Z, vector2.Z))
                                     {
                                         hasLowerConnectedSideTriangle = true;
                                         break;
                                     }
                                 }

                                 if (!hasLowerConnectedSideTriangle)
                                 {
                                     if (RegistryManager.RegistryProfile.DebugMode)
                                     {
                                         this[arrayIndex][triangleIndex].UpdateColor(new Byte4Class(0, 255, 255, 255), true);


                                         foreach (var connectedEdgeTriangleIndex in connectedEdgeTrianglesIndexes)
                                         {
                                             this[connectedEdgeTriangleIndex.ArrayIndex][connectedEdgeTriangleIndex.TriangleIndex].UpdateColor(new Byte4Class(0, 255, 0, 255), true);
                                         }
                                     }

                                     if (!overhangTriangleZPoints.ContainsKey(lowestPointZ))
                                     {
                                         overhangTriangleZPoints.TryAdd(lowestPointZ, new List<TriangleConnectionVectorInfo>());
                                     }

                                     overhangTriangleZPoints[lowestPointZ].Add(new TriangleConnectionVectorInfo() { ArrayIndex = (ushort)arrayIndex, TriangleIndex = (ushort)triangleIndex });
                                 }
                             }
                         }
                     }
                 }
             }
                 );

            //convert triangleHeights to sliceIndexes
            foreach (var overhangHeight in overhangTriangleZPoints.Keys.OrderBy(s => s))
            {
                var sliceIndex = 0;
                foreach (var sliceHeight in stlModel.SliceIndexes.Keys)
                {
                    if (sliceHeight >= overhangHeight)
                    {
                        if (!result.ContainsKey(sliceIndex))
                        {
                            result.TryAdd(sliceIndex, sliceHeight);
                        }

                        break;
                    }
                    sliceIndex++;
                }
            }

            LoggingManager.WriteToLog("CalcOverhangSliceIndex", "Total: ", stopwatch.ElapsedMilliseconds + "ms");

            return result;
        }

        internal void UpdateConnections()
        {
            //var stopWatch = new System.Diagnostics.Stopwatch();
            //stopWatch.Start();
            if (!(this.EdgePoints is null)) this.EdgePoints.Clear();
            if (this.EdgePoints == null || this.EdgePoints.Count == 0)
            {
                this.EdgePoints = new Dictionary<Vector3Class, List<TriangleConnectionVectorInfo>>();

                var stopWatch = new Stopwatch();
                stopWatch.Start();
                try
                {
                    //build connected face info
                    for (ushort arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
                    {
                        var triangleCount = this[arrayIndex].Count;
                        for (ushort triangleIndex = 0; triangleIndex < triangleCount; triangleIndex++)
                        {
                            var triangle = this[arrayIndex][triangleIndex];
                            for (ushort vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                            {
                                var vector = this[arrayIndex][triangleIndex].Vectors[vectorIndex].Position;
                                if (!this.EdgePoints.ContainsKey(vector))
                                {
                                    this.EdgePoints.Add(vector, new List<TriangleConnectionVectorInfo>());
                                }

                                this.EdgePoints[vector].Add(new TriangleConnectionVectorInfo() { ArrayIndex = arrayIndex, TriangleIndex = triangleIndex, VectorIndex = vectorIndex });
                            }

                        }
                        //ThreadPool.QueueUserWorkItem(new WaitCallback(UpdateConnectionsEdgesAsync), arrayIndexClone);
                    }

                    Console.WriteLine("UpdateConnections: " + stopWatch.ElapsedMilliseconds + "ms");

                    // if (this.Connections == null || this.Connections.Count == 0) { 
                    //// UpdateConnectionValues();
                    //     Console.WriteLine("UpdateConnection values: " + stopWatch.ElapsedMilliseconds + "ms");
                    // }



                    //this.EdgePoints.Clear();

                    //Helpers.MemoryHelpers.ForceGCCleanup();
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }

                //Debug.WriteLine("Triangle connections: " + stopWatch.ElapsedMilliseconds);
            }
        }

        public byte[] ExportTrianglesAsBinarySTL()
        {
            //convert vectors to binary stl
            //HEADER

            var objectAsBytes = new List<byte>();
            byte[] data = new byte[80];
            var headerTextBytes = System.Text.Encoding.ASCII.GetBytes("Atum3D");
            for (var i = 0; i < headerTextBytes.Length; i++)
            {
                data[i] = headerTextBytes[i];
            }

            objectAsBytes.AddRange(data);

            //number of triangles
            UInt32 numberOfTriangles = 0;
            foreach (var triangleArray in this)
            {
                numberOfTriangles += (UInt32)triangleArray.Count;
            }
            objectAsBytes.AddRange(BitConverter.GetBytes(numberOfTriangles));

            //vectors
            UInt16 emptyAttribute = 0;
            for (var triangleArrayIndex = 0; triangleArrayIndex < this.Count; triangleArrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[triangleArrayIndex].Count; triangleIndex++)
                {
                    //write vector normal
                    objectAsBytes.AddRange(BitConverter.GetBytes(this[triangleArrayIndex][triangleIndex].Normal.X));
                    objectAsBytes.AddRange(BitConverter.GetBytes(this[triangleArrayIndex][triangleIndex].Normal.Y));
                    objectAsBytes.AddRange(BitConverter.GetBytes(this[triangleArrayIndex][triangleIndex].Normal.Z));

                    foreach (var triangleVector in this[triangleArrayIndex][triangleIndex].Vectors)
                    {
                        objectAsBytes.AddRange(BitConverter.GetBytes(triangleVector.Position.X));
                        objectAsBytes.AddRange(BitConverter.GetBytes(triangleVector.Position.Y));
                        objectAsBytes.AddRange(BitConverter.GetBytes(triangleVector.Position.Z));
                    }

                    objectAsBytes.AddRange(BitConverter.GetBytes(emptyAttribute));
                }
            }

            return objectAsBytes.ToArray();
        }

        internal TriangleSurfaceInfoList FlatSurfaces = new TriangleSurfaceInfoList();

        internal void CalcFlatSurfaces()
        {
            this.FlatSurfaces.Clear();

            try
            {
                var partIndexesChecked = new ConnectedIndexItemList();
                var relatedTriangleIndexes = new ConnectedIndexItemList();

                //get filter angled triangle
                //angle is key
                var angledArrayTriangles = new Dictionary<Vector3, List<TriangleConnectionInfo>>();
                for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
                {
                    var trianglesCount = this[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        if ((this[arrayIndex][triangleIndex].AngleZ > 90 && this[arrayIndex][triangleIndex].AngleZ < 270) && !(this[arrayIndex][triangleIndex].AngleZ >= 175 && this[arrayIndex][triangleIndex].AngleZ <= 185))
                        {
                            var currentTriangleNormal = new Vector3((float)Math.Round(this[arrayIndex][triangleIndex].Normal.X, 2),
                                (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Y, 2),
                                (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Z, 2));
                            this[arrayIndex][triangleIndex].CalcMinMaxY();
                            this[arrayIndex][triangleIndex].CalcMinMaxX();

                            if (!angledArrayTriangles.ContainsKey(currentTriangleNormal))
                            {
                                angledArrayTriangles.Add(currentTriangleNormal, new List<TriangleConnectionInfo>() { this[arrayIndex][triangleIndex].Index });
                            }
                            else
                            {
                                angledArrayTriangles[currentTriangleNormal].Add(this[arrayIndex][triangleIndex].Index);
                            }
                        }
                    }
                }

                //get connected indexes with same angle
                var connectedIndexesWithSameAngle = new TriangeConnectionsIndexed();

                var trianglesWithSameAngle = new ConnectedIndexItemList();
                foreach (var angledTriangleItem in angledArrayTriangles)
                {
                    foreach (var triangleWithSameAngle in angledTriangleItem.Value)
                    {
                        if (!trianglesWithSameAngle.ContainsKey(triangleWithSameAngle.ArrayIndex)) trianglesWithSameAngle.Add(triangleWithSameAngle.ArrayIndex, new ConnectedIndexValueItem());
                        if (!trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].ContainsKey(triangleWithSameAngle.TriangleIndex)) trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].Add(triangleWithSameAngle.TriangleIndex, 0);

                    }
                }

                //combine arrays
                foreach (var angledTriangleItem in angledArrayTriangles)
                {
                    foreach (var triangle in angledTriangleItem.Value)
                    {
                        var surfaceConnections = new TriangleSurfaceInfo();

                        if (!(partIndexesChecked.ContainsKey(triangle.ArrayIndex) && partIndexesChecked[triangle.ArrayIndex].ContainsKey(triangle.TriangleIndex)))
                        {
                            var childItems = new TriangleSurfaceInfo();
                            childItems.Append(triangle);

                            surfaceConnections.Append(triangle);

                            while (childItems.Count > 0)
                            {
                                var level2ChildItems = new TriangleSurfaceInfo();
                                foreach (var childItem in childItems.Keys)
                                {

                                    foreach (var level2ChildItem in GetRecursiveConnectedTrianglesBetweenAngles(this.GetConnectedTriangles(this[childItem.ArrayIndex][childItem.TriangleIndex].Index), ref trianglesWithSameAngle).Keys)
                                    {
                                        if (!(partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex) && partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex)))
                                        {
                                            level2ChildItems.Append(level2ChildItem);
                                            surfaceConnections.Append(level2ChildItem);
                                            if (!partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex))
                                            {
                                                partIndexesChecked.Add(level2ChildItem.ArrayIndex, new ConnectedIndexValueItem());
                                            }

                                            if (!partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex))
                                            {
                                                partIndexesChecked[level2ChildItem.ArrayIndex].Add(level2ChildItem.TriangleIndex, 0);
                                            }

                                        }
                                    }

                                    if (!partIndexesChecked.ContainsKey(childItem.ArrayIndex)) partIndexesChecked.Add(childItem.ArrayIndex, new ConnectedIndexValueItem());
                                    if (!partIndexesChecked[childItem.ArrayIndex].ContainsKey(childItem.TriangleIndex)) partIndexesChecked[childItem.ArrayIndex].Add(childItem.TriangleIndex, 0);
                                }

                                childItems = level2ChildItems;
                            }
                        }

                        if (surfaceConnections.Count > 0)
                        {

                            //surface volume
                            var totalSurfaceVolume = 0f;
                            foreach (var triangleConnection in surfaceConnections.Keys)
                            {
                                totalSurfaceVolume += this[triangleConnection.ArrayIndex][triangleConnection.TriangleIndex].Volume;
                            }

                            if (totalSurfaceVolume > 2f)
                            {
                                //determine contour
                                //surfaceConnections.CalcFlatContourPath(this, SupportManager.DefaultSupportSettings.TopRadius);

                                //are we connected
                                this.FlatSurfaces.Add(surfaceConnections);
                            }
                        }
                    }
                }
                partIndexesChecked.Dispose();
                relatedTriangleIndexes.Dispose();

                this.FlatSurfaces.UpdateBoundries(this);

            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        internal void SelectSurfaceByTriangle(TriangleIntersection triangle)
        {
            TriangleSurfaceInfo selectedSurface = null;
            foreach (var flatSurface in this.FlatSurfaces)
            {
                if (flatSurface.ContainsKey(triangle.Index))
                {
                    selectedSurface = flatSurface;
                    flatSurface.Selected = true;
                }
                else
                {
                    flatSurface.Selected = false;
                }
            }

            if (selectedSurface == null)
            {
                foreach (var horizontalSurface in this.HorizontalSurfaces)
                {
                    if (horizontalSurface.ContainsKey(triangle.Index))
                    {
                        horizontalSurface.Selected = true;
                    }
                    else
                    {
                        horizontalSurface.Selected = false;
                    }
                }
            }
        }


        internal List<Vector3> SurfacePoints = new List<Vector3>();
        internal TriangleSurfaceInfoList HorizontalSurfaces = new TriangleSurfaceInfoList();

        internal void CalcHorizontalSurfaces(float modelMoveTranslationZ, bool cleanSupportStructure = true)
        {
            if (cleanSupportStructure)
            {
                this.HorizontalSurfaces.Clear();
            }
            //split into groups and calculate the centers

            try
            {


                this.HorizontalSurfaces.Clear();

                //split into groups and calculate the centers

                var partIndexesChecked = new ConnectedIndexItemList();
                var relatedTriangleIndexes = new ConnectedIndexItemList();
                var surfaceAngles = new TriangeConnectionsIndexed();

                var angledArrayTriangles = new Dictionary<Vector3, List<TriangleConnectionInfo>>();
                for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
                {
                    var trianglesCount = this[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                    {
                        if (this[arrayIndex][triangleIndex].AngleZ >= 160 && this[arrayIndex][triangleIndex].AngleZ <= 200)
                        {
                            var currentTriangleNormal = new Vector3((float)Math.Round(this[arrayIndex][triangleIndex].Normal.X, 2),
                                (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Y, 2),
                                (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Z, 2));
                            this[arrayIndex][triangleIndex].CalcMinMaxY();
                            this[arrayIndex][triangleIndex].CalcMinMaxX();

                            var value = new ConnectedIndexValueItem();
                            value.Add((ushort)triangleIndex, 0);
                            if (!angledArrayTriangles.ContainsKey(currentTriangleNormal))
                            {
                                angledArrayTriangles.Add(currentTriangleNormal, new List<TriangleConnectionInfo>() { this[arrayIndex][triangleIndex].Index });
                            }
                            else
                            {
                                angledArrayTriangles[currentTriangleNormal].Add(this[arrayIndex][triangleIndex].Index);
                            }
                        }
                    }
                }

                //get connected indexes with same angle
                var trianglesWithSameAngle = new ConnectedIndexItemList();
                foreach (var angledTriangleItem in angledArrayTriangles)
                {
                    foreach (var triangleWithSameAngle in angledTriangleItem.Value)
                    {
                        if (!trianglesWithSameAngle.ContainsKey(triangleWithSameAngle.ArrayIndex)) trianglesWithSameAngle.Add(triangleWithSameAngle.ArrayIndex, new ConnectedIndexValueItem());
                        if (!trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].ContainsKey(triangleWithSameAngle.TriangleIndex)) trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].Add(triangleWithSameAngle.TriangleIndex, 0);

                    }
                }

                //combine arrays
                foreach (var angledTriangleItem in angledArrayTriangles)
                {
                    foreach (var triangle in angledTriangleItem.Value)
                    {
                        var surfaceConnections = new TriangleSurfaceInfo();

                        if (!(partIndexesChecked.ContainsKey(triangle.ArrayIndex) && partIndexesChecked[triangle.ArrayIndex].ContainsKey(triangle.TriangleIndex)))
                        {
                            var childItems = new TriangleSurfaceInfo();
                            childItems.Append(triangle);

                            surfaceConnections.Append(triangle);

                            while (childItems.Count > 0)
                            {

                                var level2ChildItems = new TriangleSurfaceInfo();
                                foreach (var childItem in childItems.Keys)
                                {

                                    foreach (var level2ChildItem in GetRecursiveConnectedTrianglesBetweenAngles(this.GetConnectedTriangles(this[childItem.ArrayIndex][childItem.TriangleIndex].Index), ref trianglesWithSameAngle).Keys)
                                    {
                                        if (!(partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex) && partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex)))
                                        {
                                            level2ChildItems.Append(level2ChildItem);
                                            surfaceConnections.Append(level2ChildItem);
                                            if (!partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex))
                                            {
                                                partIndexesChecked.Add(level2ChildItem.ArrayIndex, new ConnectedIndexValueItem());
                                            }

                                            if (!partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex))
                                            {
                                                partIndexesChecked[level2ChildItem.ArrayIndex].Add(level2ChildItem.TriangleIndex, 0);
                                            }

                                        }
                                    }

                                    if (!partIndexesChecked.ContainsKey(childItem.ArrayIndex)) partIndexesChecked.Add(childItem.ArrayIndex, new ConnectedIndexValueItem());
                                    if (!partIndexesChecked[childItem.ArrayIndex].ContainsKey(childItem.TriangleIndex)) partIndexesChecked[childItem.ArrayIndex].Add(childItem.TriangleIndex, 0);
                                }

                                childItems = level2ChildItems;
                            }
                        }

                        if (surfaceConnections.Count > 0)
                        {
                            surfaceConnections.UpdateBoundries(this);
                            surfaceConnections.CalcVolume(this);

                            this.HorizontalSurfaces.Add(surfaceConnections);
                        }
                    }

                    // }
                }

                //partIndexesChecked.Dispose();
                relatedTriangleIndexes.Dispose();
                //surfaceAngles.Dispose();
                //}
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }
        }

        internal List<MagsAISurfaceWithOverhangSupportCones> GetSurfaceBetweenAngles(float minAngle, float maxAngle)
        {
            var surfacesBetweenAngles = new List<MagsAISurfaceWithOverhangSupportCones>();
            //if (this.Connections.Count == 0)
            //{
            //    this.UpdateConnections();
            //}

            //split into groups and calculate the centers

            try
            {

                //split into groups and calculate the centers

                var partIndexesChecked = new ConnectedIndexItemList();
                var relatedTriangleIndexes = new ConnectedIndexItemList();
                var surfaceAngles = new TriangeConnectionsIndexed();

                var angledArrayTriangles = new Dictionary<Vector3, List<TriangleConnectionInfo>>();
                for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
                {
                    var trianglesCount = this[arrayIndex].Count;
                    for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                    {
                        if (((this[arrayIndex][triangleIndex].AngleZ <= 180 - minAngle && this[arrayIndex][triangleIndex].AngleZ >= 180 - maxAngle) ||
                            (this[arrayIndex][triangleIndex].AngleZ >= 180 + minAngle && this[arrayIndex][triangleIndex].AngleZ <= 180 + maxAngle)) &&
                            (!(this[arrayIndex][triangleIndex].AngleZ == 180 || this[arrayIndex][triangleIndex].AngleZ == 0)))
                        {
                            var currentTriangleNormal = new Vector3((float)Math.Round(this[arrayIndex][triangleIndex].Normal.X, 2),
                                (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Y, 2),
                                (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Z, 2));
                            this[arrayIndex][triangleIndex].CalcMinMaxY();
                            this[arrayIndex][triangleIndex].CalcMinMaxX();

                            var value = new ConnectedIndexValueItem();
                            value.Add((ushort)triangleIndex, 0);
                            if (!angledArrayTriangles.ContainsKey(currentTriangleNormal))
                            {
                                angledArrayTriangles.Add(currentTriangleNormal, new List<TriangleConnectionInfo>() { this[arrayIndex][triangleIndex].Index });
                            }
                            else
                            {
                                angledArrayTriangles[currentTriangleNormal].Add(this[arrayIndex][triangleIndex].Index);
                            }
                        }
                    }
                }

                //get connected indexes with same angle
                var trianglesWithSameAngle = new ConnectedIndexItemList();
                foreach (var angledTriangleItem in angledArrayTriangles)
                {
                    foreach (var triangleWithSameAngle in angledTriangleItem.Value)
                    {
                        if (!trianglesWithSameAngle.ContainsKey(triangleWithSameAngle.ArrayIndex)) trianglesWithSameAngle.Add(triangleWithSameAngle.ArrayIndex, new ConnectedIndexValueItem());
                        if (!trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].ContainsKey(triangleWithSameAngle.TriangleIndex)) trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].Add(triangleWithSameAngle.TriangleIndex, 0);

                    }
                }

                //update indexes
                //UpdateIndexes();


                //combine arrays
                foreach (var angledTriangleItem in angledArrayTriangles)
                {
                    foreach (var triangle in angledTriangleItem.Value)
                    {
                        var surfaceConnections = new TriangleSurfaceInfo();

                        if (!(partIndexesChecked.ContainsKey(triangle.ArrayIndex) && partIndexesChecked[triangle.ArrayIndex].ContainsKey(triangle.TriangleIndex)))
                        {
                            var childItems = new TriangleSurfaceInfo();
                            childItems.Append(triangle);

                            surfaceConnections.Append(triangle);

                            while (childItems.Count > 0)
                            {

                                var level2ChildItems = new TriangleSurfaceInfo();
                                foreach (var childItem in childItems.Keys)
                                {

                                    foreach (var level2ChildItem in GetRecursiveConnectedTrianglesBetweenAngles(this.GetConnectedTriangles(this[childItem.ArrayIndex][childItem.TriangleIndex].Index), ref trianglesWithSameAngle).Keys)
                                    {
                                        if (!(partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex) && partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex)))
                                        {
                                            if (!(partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex) && partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex)))
                                            {
                                                level2ChildItems.Append(level2ChildItem);
                                                surfaceConnections.Append(level2ChildItem);
                                                if (!partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex))
                                                {
                                                    partIndexesChecked.Add(level2ChildItem.ArrayIndex, new ConnectedIndexValueItem());
                                                }

                                                if (!partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex))
                                                {
                                                    partIndexesChecked[level2ChildItem.ArrayIndex].Add(level2ChildItem.TriangleIndex, 0);
                                                }

                                            }

                                        }
                                    }

                                    if (!partIndexesChecked.ContainsKey(childItem.ArrayIndex)) partIndexesChecked.Add(childItem.ArrayIndex, new ConnectedIndexValueItem());
                                    if (!partIndexesChecked[childItem.ArrayIndex].ContainsKey(childItem.TriangleIndex)) partIndexesChecked[childItem.ArrayIndex].Add(childItem.TriangleIndex, 0);
                                }

                                childItems = level2ChildItems;
                            }
                        }

                        if (surfaceConnections.Count > 0)
                        {
                            var magsAISurface = new MagsAISurfaceWithOverhangSupportCones(surfaceConnections);
                            magsAISurface.UpdateBoundries(this);
                            magsAISurface.CalcVolume(this);

                            surfacesBetweenAngles.Add(magsAISurface);
                        }
                    }
                }

                //partIndexesChecked.Dispose();
                relatedTriangleIndexes.Dispose();
                //surfaceAngles.Dispose();
                //}
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }

            return surfacesBetweenAngles;
        }

        internal void UpdateIndexes()
        {
            //update indexes!
            for (ushort arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                var trianglesCount = this[arrayIndex].Count;
                for (ushort triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
                {
                    this[arrayIndex][triangleIndex].Index.ArrayIndex = arrayIndex;
                    this[arrayIndex][triangleIndex].Index.TriangleIndex = triangleIndex;
                }
            }
        }

        internal void CalcFacingDownTriangles(float modelMoveTranslationZ, float topRadius, bool cleanSupportStructure = true)
        {
            //if (this.Connections.Count == 0)
            //{
            //    this.UpdateConnections();
            //}

            if (cleanSupportStructure)
            {
                this.HorizontalSurfaces.Clear();
            }
            //split into groups and calculate the centers

            //try
            //{


            //    this.HorizontalSurfaces.Clear();

            //    //split into groups and calculate the centers

            //    var partIndexesChecked = new ConnectedIndexItemList();
            //    var relatedTriangleIndexes = new ConnectedIndexItemList();
            //    var surfaceAngles = new TriangeConnectionsIndexed();

            //    var angledArrayTriangles = new Dictionary<Vector3, List<TriangleConnectionInfo>>();
            //    for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            //    {
            //        var trianglesCount = this[arrayIndex].Count;
            //        for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
            //        {
            //            if (this[arrayIndex][triangleIndex].AngleZ >= 120 && this[arrayIndex][triangleIndex].AngleZ <= 240)
            //            {
            //                var currentTriangleNormal = new Vector3((float)Math.Round(this[arrayIndex][triangleIndex].Normal.X, 2),
            //                    (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Y, 2),
            //                    (float)Math.Round(this[arrayIndex][triangleIndex].Normal.Z, 2));
            //                this[arrayIndex][triangleIndex].CalcMinMaxY();
            //                this[arrayIndex][triangleIndex].CalcMinMaxX();

            //                var value = new ConnectedIndexValueItem();
            //                value.Add((ushort)triangleIndex, 0);
            //                if (!angledArrayTriangles.ContainsKey(currentTriangleNormal))
            //                {
            //                    angledArrayTriangles.Add(currentTriangleNormal, new List<TriangleConnectionInfo>() { new TriangleConnectionInfo() { ArrayIndex = (ushort)arrayIndex, TriangleIndex = (ushort)triangleIndex } });
            //                }
            //                else
            //                {
            //                    angledArrayTriangles[currentTriangleNormal].Add(new TriangleConnectionInfo() { ArrayIndex = (ushort)arrayIndex, TriangleIndex = (ushort)triangleIndex });
            //                }
            //            }
            //        }
            //    }

            //    //get connected indexes with same angle
            //    var trianglesWithSameAngle = new ConnectedIndexItemList();
            //    foreach (var angledTriangleItem in angledArrayTriangles)
            //    {
            //        foreach (var triangleWithSameAngle in angledTriangleItem.Value)
            //        {
            //            if (!trianglesWithSameAngle.ContainsKey(triangleWithSameAngle.ArrayIndex)) trianglesWithSameAngle.Add(triangleWithSameAngle.ArrayIndex, new ConnectedIndexValueItem());
            //            if (!trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].ContainsKey(triangleWithSameAngle.TriangleIndex)) trianglesWithSameAngle[triangleWithSameAngle.ArrayIndex].Add(triangleWithSameAngle.TriangleIndex, 0);

            //        }
            //    }

            //    //combine arrays
            //    foreach (var angledTriangleItem in angledArrayTriangles)
            //    {

            //        foreach (var triangle in angledTriangleItem.Value)
            //        {
            //            var surfaceConnections = new TriangleSurfaceInfo();

            //            if (!(partIndexesChecked.ContainsKey(triangle.ArrayIndex) && partIndexesChecked[triangle.ArrayIndex].ContainsKey(triangle.TriangleIndex)))
            //            {
            //                var childItems = new TriangleSurfaceInfo();
            //                childItems.Append(triangle.ArrayIndex, triangle.TriangleIndex);

            //                surfaceConnections.Append(triangle.ArrayIndex, triangle.TriangleIndex);

            //                while (childItems.Count > 0)
            //                {

            //                    var level2ChildItems = new TriangleSurfaceInfo();
            //                    foreach (var childItem in childItems)
            //                    {

            //                        foreach (var level2ChildItem in GetRecursiveConnectedTrianglesBetweenAngles(this.Connections[childItem.ArrayIndex][childItem.TriangleIndex], ref trianglesWithSameAngle))
            //                        {
            //                            if (!(partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex) && partIndexesChecked[level2ChildItem.ArrayIndex].ContainsKey(level2ChildItem.TriangleIndex)))
            //                            {
            //                                level2ChildItems.Add(level2ChildItem);
            //                                surfaceConnections.Add(level2ChildItem);
            //                                if (!partIndexesChecked.ContainsKey(level2ChildItem.ArrayIndex)) partIndexesChecked.Add(level2ChildItem.ArrayIndex, new ConnectedIndexValueItem());

            //                                partIndexesChecked[level2ChildItem.ArrayIndex].Add(level2ChildItem.TriangleIndex, 0);

            //                            }
            //                        }

            //                        if (!partIndexesChecked.ContainsKey(childItem.ArrayIndex)) partIndexesChecked.Add(childItem.ArrayIndex, new ConnectedIndexValueItem());
            //                        if (!partIndexesChecked[childItem.ArrayIndex].ContainsKey(childItem.TriangleIndex)) partIndexesChecked[childItem.ArrayIndex].Add(childItem.TriangleIndex, 0);
            //                    }

            //                    childItems = level2ChildItems;
            //                }
            //            }

            //            if (surfaceConnections.Count > 0)
            //            {

            //                surfaceConnections.UpdateBoundries(this);
            //                surfaceConnections.CalcVolume(this);

            //                this.HorizontalSurfaces.Add(surfaceConnections);
            //            }
            //        }

            //        // }
            //    }

            //    //partIndexesChecked.Dispose();
            //    relatedTriangleIndexes.Dispose();
            //    //surfaceAngles.Dispose();
            //    //}
            //}
            //catch (Exception exc)
            //{
            //    Debug.WriteLine(exc.Message);
            //}
        }

        internal void ClearSupportStructure()
        {
            foreach (var surface in this.HorizontalSurfaces)
            {
                surface.SupportStructure.Clear();
            }
        }

        internal void CalcHorizontalSurfaceSupportPoints(float hSupportDistance, bool crossSupport, STLModel3D stlModel, DAL.Materials.Material selectedMaterial, bool skipEdgeCheck = false)
        {

            var minSupportHeight = selectedMaterial.LT2;
            if (selectedMaterial.LT2 > selectedMaterial.LT1 && selectedMaterial.InitialLayers > 0)
            {
                minSupportHeight = selectedMaterial.LT1;
            }

            foreach (var hSupportSurface in this.HorizontalSurfaces)
            {
                hSupportSurface.SupportPoints.Clear();
                hSupportSurface.SupportDistance = hSupportDistance;
                hSupportSurface.CrossSupport = crossSupport;
                hSupportSurface.SupportPoints.AddRange(CalcHorizontalSurfaceSupportPoints(hSupportSurface, hSupportDistance, crossSupport, skipEdgeCheck).Keys);
                if (hSupportSurface.SupportPoints.Count == 0)
                {
                    hSupportSurface.SupportDistance = 0;
                }

            }
        }

        internal ConcurrentDictionary<TriangleIntersection, bool> CalcHorizontalSurfaceSupportPoints(TriangleSurfaceInfo hSupportSurface, float hSupportDistance, bool crossSupport, bool skipEdgeCheck = false)
        {
            hSupportSurface.SupportPoints.Clear();
            var results = new ConcurrentDictionary<TriangleIntersection, bool>();

            //convert to stl
            var surfaceTrianglesAsStl = new STLModel3D();
            surfaceTrianglesAsStl.Triangles = new TriangleInfoList();
            foreach (var surfaceArrayTriangleIndex in hSupportSurface.Keys)
            {
                surfaceTrianglesAsStl.Triangles[0].Add(this[surfaceArrayTriangleIndex.ArrayIndex][surfaceArrayTriangleIndex.TriangleIndex]);
            }

            //calc min/max values
            var surfaceMinX = 1000000f;
            var surfaceMaxX = -1000000f;
            var surfaceMinY = 1000000f;
            var surfaceMaxY = -1000000f;
            foreach (var surfaceConnection in hSupportSurface.Keys)
            {
                foreach (var vector in this[surfaceConnection.ArrayIndex][surfaceConnection.TriangleIndex].Vectors)
                {
                    surfaceMinX = Math.Min(vector.Position.X, surfaceMinX);
                    surfaceMaxX = Math.Max(vector.Position.X, surfaceMaxX);
                    surfaceMinY = Math.Min(vector.Position.Y, surfaceMinY);
                    surfaceMaxY = Math.Max(vector.Position.Y, surfaceMaxY);

                    if (this[surfaceConnection.ArrayIndex][surfaceConnection.TriangleIndex].Center.Z + 0.001d < this[surfaceConnection.ArrayIndex][surfaceConnection.TriangleIndex].Center.Z)
                    {
                        hSupportSurface.HasEdgeDown = true;
                    }
                }
            }


            //get left point with offset
            var crossSupportCount = crossSupport ? 2 : 1; //enable crosssupport
            var multiplyFactorY = (int)((surfaceMaxY - surfaceMinY) / hSupportDistance) + 1;
            var multiplyFactorX = (int)((surfaceMaxX - surfaceMinX) / hSupportDistance) + 1;
            var topRadius = PrintJobManager.SelectedMaterialSummary.Material.SupportProfiles.First().SupportTopRadius;

            if (!hSupportSurface.HasEdgeDown || skipEdgeCheck)
            {
                var gridPoints = new ConcurrentDictionary<TriangleIntersection, bool>();

                var gridPointXMinKeys = new Dictionary<float, float>();
                var gridPointXMaxKeys = new Dictionary<float, float>();

                for (var crossSupportIndex = 0; crossSupportIndex < crossSupportCount; crossSupportIndex++)
                {
                    for (var multiplyFactorIndexY = 0; multiplyFactorIndexY < multiplyFactorY; multiplyFactorIndexY++)
                    {
                        var gridPointY = surfaceMaxY - (hSupportDistance * multiplyFactorIndexY);
                        if (crossSupportIndex == 1)
                        {
                            gridPointY += hSupportDistance / 2;
                        }

                        var minXByY = hSupportSurface.MinXByY(gridPointY, this);
                        var maxXByY = hSupportSurface.MaxXByY(gridPointY, this);

                        for (var multiplyFactorIndexX = 0; multiplyFactorIndexX < multiplyFactorX; multiplyFactorIndexX++)
                        {

                            //get point in horizontal direction within offset
                            var gridPoint = new Vector3Class(surfaceMaxX - (hSupportDistance * multiplyFactorIndexX), gridPointY, 0);

                            if (crossSupportIndex == 1)
                            {
                                gridPoint.X += hSupportDistance / 2;
                            }

                            if ((maxXByY - minXByY) < 0.2f)
                            {
                                gridPoint.X = minXByY + ((maxXByY - minXByY) / 2);
                            }
                            else if (gridPoint.X - 0.2f > minXByY)
                            {
                                gridPoint.X -= 0.2f;
                            }
                            else if (gridPoint.X + 0.2f < maxXByY)
                            {
                                gridPoint.X += 0.2f;
                            }

                            //Y
                            if (!gridPointXMinKeys.ContainsKey(gridPoint.X))
                            {
                                gridPointXMinKeys.Add(gridPoint.X, hSupportSurface.MinYByX(gridPoint.X, this));
                            }

                            if (!gridPointXMaxKeys.ContainsKey(gridPoint.X))
                            {
                                gridPointXMaxKeys.Add(gridPoint.X, hSupportSurface.MaxYByX(gridPoint.X, this));
                            }
                            var minYByX = gridPointXMinKeys[gridPoint.X];
                            var maxYByX = gridPointXMaxKeys[gridPoint.X];
                            if ((maxYByX - minYByX) < 0.2f)
                            {
                                gridPoint.Y = minYByX + ((maxYByX - minYByX) / 2);
                            }
                            else if (gridPoint.Y - 0.2f > minYByX)
                            {
                                gridPoint.Y -= 0.2f;
                            }
                            else if (gridPoint.Y + 0.2f < maxYByX)
                            {
                                gridPoint.Y += 0.2f;
                            }
                            gridPoints.TryAdd(new TriangleIntersection() { IntersectionPoint = gridPoint }, false);
                        }
                    }
                }

                gridPoints = IntersectionProvider.GetIntersectionPointsAsync(gridPoints, surfaceTrianglesAsStl);

                return gridPoints;

            }

            return results;
        }

        internal TriangleInfoList GetTrianglesWithinSphereBoundry(Vector3Class centerPointOfSphere, float radiusOfSphere)
        {
            var cubeModel = new Cube(radiusOfSphere, radiusOfSphere, radiusOfSphere);
            //move cube to it s center
            for (var vertexIndex = 0; vertexIndex < cubeModel.VertexArray.Length; vertexIndex++)
            {
                cubeModel.VertexArray[vertexIndex].Position -= cubeModel.Center;
                //and add centerPoint of shere
                //cubeModel.VertexArray[vertexIndex].Position += centerPointOfSphere;
            }

            //           
            var cubeAsTriangles = new List<Triangle>();
            //for (var triangleIndex = 0; triangleIndex < cubeModel.VertexArray.Length - 1; triangleIndex += 3)
            //{
            //    var cubeTriangle = new Triangle();
            //    cubeTriangle.Vectors[0] = cubeModel.VertexArray[triangleIndex];
            //    cubeTriangle.Vectors[1] = cubeModel.VertexArray[triangleIndex + 1];
            //    cubeTriangle.Vectors[2] = cubeModel.VertexArray[triangleIndex + 2];

            //    cubeAsTriangles.Add(cubeTriangle);
            //}


            var result = new TriangleInfoList();
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    var triangle = this[arrayIndex][triangleIndex];
                    if ((triangle.Bottom <= centerPointOfSphere.Z - radiusOfSphere && triangle.Top >= centerPointOfSphere.Z - radiusOfSphere) ||
                        (triangle.Bottom >= centerPointOfSphere.Z - radiusOfSphere && triangle.Bottom <= centerPointOfSphere.Z + radiusOfSphere) ||
                        (triangle.Top >= centerPointOfSphere.Z - radiusOfSphere && triangle.Top <= centerPointOfSphere.Z + radiusOfSphere))
                    {
                        //triangle.UpdateColor(new Byte4Class(0, 0, 0, 255), true);
                        result[0].Add(triangle);
                    }
                }
            }

            return result;
        }

        internal TriangleInfoList GetTrianglesWithinXYBoundry(float leftPoint, float backPoint, float rightPoint, float frontPoint)
        {
            var result = new TriangleInfoList();

            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    var triangle = this[arrayIndex][triangleIndex];
                    if (triangle.Left == 0 && triangle.Right == 0 && triangle.Front == 0 && triangle.Back == 0)
                    {
                        triangle.CalcMinMaxX();
                        triangle.CalcMinMaxY();
                        triangle.CalcMinMaxZ();
                        triangle.CalcCenter();
                    }
                    if ((triangle.Left <= leftPoint && triangle.Right >= rightPoint) ||
                        (triangle.Left >= leftPoint && triangle.Right <= rightPoint) ||
                        (triangle.Left <= leftPoint && triangle.Right <= rightPoint && triangle.Right >= leftPoint) ||
                        (triangle.Left >= leftPoint && triangle.Left <= rightPoint && triangle.Right >= rightPoint))
                    {
                        if ((triangle.Back <= backPoint && triangle.Front >= frontPoint) ||
                            (triangle.Back >= backPoint && triangle.Front <= frontPoint) ||
                        (triangle.Back <= backPoint && triangle.Front <= frontPoint && triangle.Front >= backPoint) ||
                        (triangle.Back >= backPoint && triangle.Back <= frontPoint && triangle.Front >= frontPoint))
                        {
                          //  triangle.UpdateColor(new Byte4Class(255, 0, 255, 0), true);
                                result[0].Add(triangle);
                        }
                    }
                }

            }
            return result;
        }

       

        internal void CalcModelParts()
        {
            //var stopWatch = new System.Diagnostics.Stopwatch();
            //stopWatch.Start();


            //split into groups and calculate the centers
            try
            {
                var partIndexesChecked = new ConnectedIndexItemList();

                //var stlModel = this;

                //for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
                //{
                //    for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                //    {
                //        if (partIndexesChecked.ContainsKey(arrayIndex) && partIndexesChecked[arrayIndex].ContainsKey(triangleIndex))
                //        {
                //        }
                //        else
                //        {
                //            var relatedTriangleIndexes = new ConnectedIndexItemList();
                //            relatedTriangleIndexes.Add(arrayIndex, new ConnectedIndexValueItem());
                //            relatedTriangleIndexes[arrayIndex].Add(triangleIndex, 0);

                //            //get recursive triangles
                //            var parentTriangle = this.Connections[arrayIndex][triangleIndex];
                //            var childTriangles = GetRecursiveConnectedTriangles(parentTriangle, ref relatedTriangleIndexes);
                //            while (childTriangles.Count > 0)
                //            {
                //                var childTrianglesConnected = new List<KeyValuePair<int, int>>();
                //                foreach (var childTriangleConnectionIndex in childTriangles)
                //                {
                //                    childTrianglesConnected.AddRange(GetRecursiveConnectedTriangles(this.Connections[childTriangleConnectionIndex.Key][childTriangleConnectionIndex.Value], ref relatedTriangleIndexes));
                //                }

                //                childTriangles = childTrianglesConnected;
                //            }


                //            this.Parts.Add(relatedTriangleIndexes);

                //            foreach (var relatedTriangleIndexKey in relatedTriangleIndexes.Keys)
                //            {
                //                if (!partIndexesChecked.ContainsKey(relatedTriangleIndexKey))
                //                {
                //                    partIndexesChecked.Add(relatedTriangleIndexKey, new ConnectedIndexValueItem());
                //                }

                //                foreach (var relatedTriangleIndexValue in relatedTriangleIndexes[relatedTriangleIndexKey])
                //                {
                //                    if (!partIndexesChecked[relatedTriangleIndexKey].ContainsKey(relatedTriangleIndexValue.Key))
                //                    {
                //                        partIndexesChecked[relatedTriangleIndexKey].Add(relatedTriangleIndexValue.Key, 0);
                //                    }
                //                }
                //            }

                //            //calc group item center
                //            var groupItemCenter = new Vector3();
                //            float cent_fac = 1.0f / (float)relatedTriangleIndexes.Count;

                //            foreach (var groupItem in Parts)
                //            {
                //                foreach (var groupItemKey in groupItem.Keys)
                //                {
                //                    foreach (var groupItemValue in groupItem[groupItemKey])
                //                    {
                //                        var triangle = this[groupItemKey][groupItemValue.Key];
                //                        groupItemCenter += Vector3.Multiply(triangle.Center, cent_fac);
                //                    }
                //                }

                //                this.PartsCenter.Add(groupItemCenter);
                //            }
                //        }
                //    }
                // }
            }
            catch (Exception exc)
            {
                Debug.WriteLine(exc.Message);
            }


            //Debug.WriteLine("Model Part: " + stopWatch.ElapsedMilliseconds);
            //stopWatch.Stop();

            //Fix face directions
            //DetectAndRepairFaceDirections(false);
        }

        private TriangleSurfaceInfo GetRecursiveConnectedTrianglesBetweenAngles(TriangleConnectionInfo[] parentTriangles, ref ConnectedIndexItemList angledTriangles)
        {
            var result = new TriangleSurfaceInfo();

            foreach (var connectedItem in parentTriangles)
            {
                if (angledTriangles.ContainsKey(connectedItem.ArrayIndex) && angledTriangles[connectedItem.ArrayIndex].ContainsKey(connectedItem.TriangleIndex))
                {
                    result.Append(connectedItem);
                }
            }


            return result;
        }

        private List<KeyValuePair<int, int>> GetRecursiveConnectedTriangles(List<TriangleConnectionInfo> parentTriangle, ref ConnectedIndexItemList groupConnectionIndexes)
        {
            var result = new List<KeyValuePair<int, int>>();

            //foreach (var connectedItem in parentTriangle)
            //{
            //    if (!groupConnectionIndexes.ContainsKey(connectedItem.ArrayIndex))
            //    {
            //        groupConnectionIndexes.Add(connectedItem.ArrayIndex, new ConnectedIndexValueItem());
            //        groupConnectionIndexes[connectedItem.ArrayIndex].Add(connectedItem.TriangleIndex, 0);
            //        result.Add(new KeyValuePair<int, int>(connectedItem.ArrayIndex, connectedItem.TriangleIndex));
            //        //       GetRecursiveConnectedTriangles(this[connectedIndexKey][connectedInde.xValue], ref groupConnectionIndexes);
            //    }
            //    else if (!groupConnectionIndexes[connectedItem.ArrayIndex].ContainsKey(connectedItem.TriangleIndex))
            //    {
            //        groupConnectionIndexes[connectedItem.ArrayIndex].Add(connectedItem.TriangleIndex, 0);
            //        result.Add(new KeyValuePair<int, int>(connectedItem.ArrayIndex, connectedItem.TriangleIndex));
            //        //GetRecursiveConnectedTriangles(this[connectedIndexKey][connectedIndexValue], ref groupConnectionIndexes);
            //    }
            //}
            return result;
        }



        internal void UpdateFaceColors(Byte4Class object3dColor, bool forceColor)
        {
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    this[arrayIndex][triangleIndex].UpdateColor(object3dColor, forceColor);
                }
            }
        }

        //internal void DetectAndRepairFaceDirections(Byte4 object3dColor)
        //{
        //    // ModelNormalRepairProcessing(null, null);

        //    var stopWatch = new System.Diagnostics.Stopwatch();
        //    Debug.WriteLine("Repair normals start");
        //    stopWatch.Start();
        //    var startIndexKey = 0; ;
        //    var startIndexValue = 0;
        //    var stlModel = this;

        //    var groupItemIndex = 0;
        //    try
        //    {
        //        foreach (var groupItem in Parts)
        //        {
        //            var len_best = -float.MaxValue;
        //            float len_test = 0f;

        //            var groupItems = stlModel.Parts[groupItemIndex];
        //            foreach (var groupItemKey in groupItem.Keys)
        //            {
        //                foreach (var groupItemValue in groupItem[groupItemKey])
        //                {
        //                    //calc start index
        //                    var triangle = this[groupItemKey][groupItemValue.Key];

        //                    len_test = CalcSquaredLen(triangle.Center, stlModel.PartsCenter[groupItemIndex]);
        //                    if (len_test > len_best)
        //                    {
        //                        len_best = len_test;
        //                        startIndexKey = groupItemKey;
        //                        startIndexValue = groupItemValue.Key;
        //                    }
        //                }
        //            }

        //            var tvec = this[startIndexKey][startIndexValue].Center - stlModel.PartsCenter[groupItemIndex];
        //            var checkWindingIndexes = new ConnectedIndexItemList();

        //            if (Vector3.Dot(tvec, this[startIndexKey][startIndexValue].Normal) < 0)
        //            {
        //                this[startIndexKey][startIndexValue].Flip();
        //                this[startIndexKey][startIndexValue].UpdateColor(object3dColor);
        //                //_fixedFaceDirections.Add(this[startIndexKey][startIndexValue]);
        //            }
        //            if (!checkWindingIndexes.ContainsKey(startIndexKey))
        //            {
        //                checkWindingIndexes.Add(startIndexKey, new ConnectedIndexValueItem());
        //            }
        //            checkWindingIndexes[startIndexKey].Add(startIndexValue, 0);

        //            //check winding
        //            var childWindingTrianglesConnections = GetDirectionWindingConnectedTriangles(new TriangleConnectionInfo() { ArrayIndex = startIndexKey, TriangleIndex = startIndexValue }, ref checkWindingIndexes);
        //            while (childWindingTrianglesConnections.Count > 0)
        //            {
        //                var newChildWindingTrianglesConnections = new List<TriangleConnectionInfo>();
        //                foreach (var childWindingTriangleConnection in childWindingTrianglesConnections)
        //                {
        //                    newChildWindingTrianglesConnections.AddRange(GetDirectionWindingConnectedTriangles(childWindingTriangleConnection, ref checkWindingIndexes));
        //                }

        //                childWindingTrianglesConnections = newChildWindingTrianglesConnections;

        //            }

        //            groupItemIndex++;
        //        }
        //        Debug.WriteLine("Repair normals stopped: " + stopWatch.ElapsedMilliseconds);
        //        stopWatch.Stop();

        //        //   ModelNormalRepairProcessed(null, null);

        //    }
        //    catch (Exception exc)
        //    {
        //        Debug.WriteLine(exc.Message);
        //    }
        //}

        internal List<TriangleIntersection> SupportZPoints = new List<TriangleIntersection>();
        internal void UpdateZPoints()
        {
            //this.SupportZPoints.Clear();
            //var edges = new Dictionary<TriangleIntersection, ConnectedIndexItemList>();
            ////build connected face info
            //try
            //{
            //    for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            //    {
            //        var trianglesCount = this[arrayIndex].Count;
            //        for (var triangleIndex = 0; triangleIndex < trianglesCount; triangleIndex++)
            //        {
            //            var triangle = this[arrayIndex][triangleIndex];
            //            foreach (var v in triangle.Vectors)
            //            {
            //                var t = new TriangleIntersection(triangle, v.Position, 0, 0);
            //                if (!edges.ContainsKey(t))
            //                {
            //                    var items = new ConnectedIndexItemList();
            //                    items.Add((ushort)arrayIndex, new ConnectedIndexValueItem());
            //                    items[(ushort)arrayIndex].Add((ushort)triangleIndex, 0);
            //                    edges.Add(t, items);
            //                }
            //                else
            //                {
            //                    var items = (ConnectedIndexItemList)edges[t];
            //                    if (items.ContainsKey((ushort)arrayIndex) && !items[(ushort)arrayIndex].ContainsKey((ushort)triangleIndex))
            //                    {
            //                        items[(ushort)arrayIndex].Add((ushort)triangleIndex, 0);
            //                    }
            //                    else if (!items.ContainsKey((ushort)arrayIndex))
            //                    {
            //                        items.Add((ushort)arrayIndex, new ConnectedIndexValueItem());
            //                        items[(ushort)arrayIndex].Add((ushort)triangleIndex, 0);
            //                    }
            //                }

            //            }
            //        }
            //    }

            //    foreach (TriangleIntersection edgeKey in edges.Keys)
            //    {
            //        bool hasLowerConnectedZPosition = false;
            //        foreach (var connectedItems in (ConnectedIndexItemList)edges[edgeKey])
            //        {
            //            foreach (var connectedTriangleIndex in connectedItems.Value)
            //            {
            //                var connectedTriangle = this[connectedItems.Key][connectedTriangleIndex.Key];
            //                if (connectedTriangle.AngleZ == 180)
            //                {
            //                    hasLowerConnectedZPosition = true;
            //                    break;
            //                }
            //                else if (connectedTriangle.Bottom < edgeKey.IntersectionPoint.Z || connectedTriangle.AngleZ < 90 || connectedTriangle.AngleZ > 270)
            //                {
            //                    hasLowerConnectedZPosition = true;
            //                    break;
            //                }
            //            }

            //            if (hasLowerConnectedZPosition)
            //            {
            //                break;
            //            }
            //        }

            //        if (!hasLowerConnectedZPosition)
            //        {
            //            this.SupportZPoints.Add(edgeKey);
            //        }
            //    }
            //}
            //catch (Exception exc)
            //{
            //    Debug.WriteLine(exc.Message);
            //}
        }

        private float CalcSquaredLen(Vector3 a, Vector3 b)
        {
            var sub = b - a;
            return Vector3.Dot(sub, sub);
        }

        //List<Triangle> _fixedFaceDirections = new List<Triangle>();

        //private List<TriangleConnectionInfo> GetDirectionWindingConnectedTriangles(TriangleConnectionInfo parentConnection, ref ConnectedIndexItemList checkedIndexes)
        //{
        //    var result = new List<TriangleConnectionInfo>();
        //    var parentTriangleConnections = this.Connections[parentConnection.ArrayIndex][parentConnection.TriangleIndex];
        //    var parentTriangle = this[parentConnection.ArrayIndex][parentConnection.TriangleIndex];
        //    foreach (var connectedItem in parentTriangleConnections.Keys)
        //    {
        //        if (checkedIndexes.ContainsKey(connectedItem.ArrayIndex) && checkedIndexes[connectedItem.ArrayIndex].ContainsKey(connectedItem.TriangleIndex))
        //        {
        //        }
        //        else
        //        {
        //            var connectedTriangle = this[connectedItem.ArrayIndex][connectedItem.TriangleIndex];
        //            var connectedEdgeFound = false;

        //            for (var connectedVectorIndex = 0; connectedVectorIndex < 3; connectedVectorIndex++)
        //            {
        //                var connectedVector = connectedTriangle.Vectors[connectedVectorIndex];
        //                var connectedNextVectorIndex = connectedVectorIndex == 2 ? 0 : connectedVectorIndex + 1;
        //                var connectedPreviousVectorIndex = connectedVectorIndex == 0 ? 2 : connectedVectorIndex - 1;


        //                for (var parentVectorIndex = 0; parentVectorIndex < 3; parentVectorIndex++)
        //                {
        //                    var parentVector = parentTriangle.Vectors[parentVectorIndex];
        //                    var parentNextVectorIndex = parentVectorIndex == 2 ? 0 : parentVectorIndex + 1;
        //                    var parentPreviousVectorIndex = parentVectorIndex == 0 ? 2 : parentVectorIndex - 1;


        //                    if (parentVector.Position == connectedVector.Position)
        //                    {
        //                        //check if next point or previous points are the same if so then flip
        //                        if (parentTriangle.Vectors[parentNextVectorIndex].Position == connectedTriangle.Vectors[connectedNextVectorIndex].Position || parentTriangle.Vectors[parentPreviousVectorIndex].Position == connectedTriangle.Vectors[connectedPreviousVectorIndex].Position)
        //                        {

        //                            if (!checkedIndexes.ContainsKey(connectedItem.ArrayIndex))
        //                            {
        //                                checkedIndexes.Add(connectedItem.ArrayIndex, new ConnectedIndexValueItem());
        //                            }
        //                            checkedIndexes[connectedItem.ArrayIndex].Add(connectedItem.TriangleIndex, 0);

        //                            //_fixedFaceDirections.Add(connectedTriangle);
        //                            connectedTriangle.Flip();
        //                            connectedEdgeFound = true;
        //                        }
        //                        //if previouspoint = nextpoint then winding is ok
        //                        else if (parentTriangle.Vectors[parentPreviousVectorIndex].Position == connectedTriangle.Vectors[connectedNextVectorIndex].Position || parentTriangle.Vectors[parentNextVectorIndex].Position == connectedTriangle.Vectors[connectedPreviousVectorIndex].Position)
        //                        {
        //                            if (!checkedIndexes.ContainsKey(connectedItem.ArrayIndex))
        //                            {
        //                                checkedIndexes.Add(connectedItem.ArrayIndex, new ConnectedIndexValueItem());
        //                            }
        //                            checkedIndexes[connectedItem.ArrayIndex].Add(connectedItem.TriangleIndex, 0);

        //                            connectedEdgeFound = true;
        //                        }

        //                        break;
        //                    }

        //                }
        //                if (connectedEdgeFound)
        //                {
        //                    result.Add(new TriangleConnectionInfo() { ArrayIndex = connectedItem.ArrayIndex, TriangleIndex = connectedItem.TriangleIndex });
        //                    break;
        //                };
        //            }

        //        }
        //    }
        //    return result;
        //}

        #region AutoRotationSelection

        //internal List<TriangleConnectionSelections> MarkedTriangleIndexes { get; set; }

        internal void ClearSelectedOrientationTriangles()
        {
            //this.MarkedTriangleIndexes.Clear();
            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    if (this[arrayIndex][triangleIndex].Properties != Triangle.typeTriangleProperties.None)
                    {
                        this[arrayIndex][triangleIndex].Properties = Triangle.typeTriangleProperties.None;
                    }
                }
            }
        }

        internal Vector3Class CalcSelectedOrientationTrianglesNormal()
        {
            var totalVector = new Vector3Class();
            var selectionNormal = new Vector3Class();
            var selectionVolume = 0f;

            for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                {
                    if ((this[arrayIndex][triangleIndex].Properties & Triangle.typeTriangleProperties.AutoRotationSelected) == Triangle.typeTriangleProperties.AutoRotationSelected)
                    {
                        var triangle = this[arrayIndex][triangleIndex];
                        triangle.CalcNormal();
                        selectionNormal += triangle.Normal;
                        selectionVolume += triangle.Volume;
                    }
                }
            }

            if (selectionNormal != Vector3Class.Zero)
            {
                totalVector += (selectionNormal / selectionVolume);
            }

            //if (this.SelectedTriangles != null)
            //{
            //    foreach (var selectedTriangleList in this.SelectedTriangles)
            //    {
            //        if (selectedTriangleList != null)
            //        {
            //            var selectionNormal = new Vector3();
            //            var selectionVolume = 0f;
            //            foreach (var selectedTriangle in selectedTriangleList.Keys)
            //            {
            //                var triangle = this[selectedTriangle.ArrayIndex][selectedTriangle.TriangleIndex];
            //                triangle.CalcNormal();
            //                selectionNormal += triangle.Normal;
            //                selectionVolume += triangle.Volume;
            //            }

            //            if (selectionNormal != Vector3.Zero)
            //            {
            //                totalVector += (selectionNormal / selectionVolume);
            //            }
            //        }
            //    }
            //}

            totalVector = Vector3Class.Normalize(totalVector);

            return totalVector;
        }

        private List<TriangleConnectionInfo> MarkedTriangles
        {
            get
            {
                var markedTriangles = new List<TriangleConnectionInfo>();
                for (var arrayIndex = 0; arrayIndex < this.Count; arrayIndex++)
                {
                    for (var triangleIndex = 0; triangleIndex < this[arrayIndex].Count; triangleIndex++)
                    {
                        if (this[arrayIndex][triangleIndex].IsMAGSAIMarked)
                        {
                            markedTriangles.Add(this[arrayIndex][triangleIndex].Index);
                        }
                    }
                }

                return markedTriangles;
            }
        }

        public void UpdateSelectedOrientationTriangles(STLModel3D selectedModel)
        {
            if (selectedModel.MAGSAISelectionOverlay == null)
            {
                selectedModel.MAGSAISelectionOverlay = new STLModel3D();
                selectedModel.MAGSAISelectionOverlay.Triangles = new TriangleInfoList();
            }
            selectedModel.MAGSAISelectionOverlay.Triangles[0].Clear();
            foreach (var markedTriangle in this.MarkedTriangles)
            {
                selectedModel.MAGSAISelectionOverlay.Triangles[0].Add(this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex]);
            }
            selectedModel.MAGSAISelectionOverlay.UpdateBinding();

        }

        internal void ProcessSelectedOrientationTriangles(Dictionary<TriangleConnectionInfo, bool> selectedTriangles, Byte4Class modelColor, STLModel3D selectedModel)
        {
            //if (this.MarkedTriangleIndexes == null)
            //{
            //    this.MarkedTriangleIndexes = new List<TriangleConnectionSelections>();
            //}

            var deltaSelectedTriangles = new TriangleConnectionSelections();
            foreach (var markedTriangle in selectedTriangles.Keys)
            {
                //check if triangle has IsMarked property
                if (!selectedModel.Triangles[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex].IsMAGSAIMarked)
                {
                    this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex].Properties = this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex].Properties | Triangle.typeTriangleProperties.AutoRotationSelected;
                    selectedModel.MAGSAISelectionOverlay.Triangles[0].Add(this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex]);
                }
            }
            //var selectedTriangleFound = false;
            //foreach (var l in this.MarkedTriangleIndexes)
            //{
            //    if (l.ContainsKey(markedTriangle))
            //    {
            //        selectedTriangleFound = true;
            //        break;
            //    }
            //}

            //if (!selectedTriangleFound)
            //{
            //    deltaSelectedTriangles.Add(markedTriangle, false);

            //    if (this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex].Properties != Triangle.typeTriangleProperties.AutoRotationSelected)
            //    {
            //        this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex].Properties = Triangle.typeTriangleProperties.AutoRotationSelected;

            //        //update scene view
            //        selectedModel.MAGSAISelectionOverlay.Triangles[0].Add(this[markedTriangle.ArrayIndex][markedTriangle.TriangleIndex]);
            //    }

            //}
            //}

            //   if (deltaSelectedTriangles.Count > 0)
            //   {
            //   this.MarkedTriangleIndexes.Add(deltaSelectedTriangles);

            selectedModel.MAGSAISelectionOverlay.UpdateBinding();
            //   }
        }

        internal void ProcessDeSelectedOrientationTriangles(Dictionary<TriangleConnectionInfo, bool> deSelectedTriangles, Byte4Class modelColor, STLModel3D selectedModel)
        {
            foreach (var selectedTriangle in deSelectedTriangles.Keys)
            {
                if (this[selectedTriangle.ArrayIndex][selectedTriangle.TriangleIndex].IsMAGSAIMarked)
                {
                    this[selectedTriangle.ArrayIndex][selectedTriangle.TriangleIndex].Properties -= Triangle.typeTriangleProperties.AutoRotationSelected;
                }

                //update scene view
                selectedModel.MAGSAISelectionOverlay.Triangles[0].Remove(this[selectedTriangle.ArrayIndex][selectedTriangle.TriangleIndex]);
            }

            if (deSelectedTriangles.Count > 0)
            {
                selectedModel.MAGSAISelectionOverlay.UpdateBinding();
            }

        }

        #endregion

        internal void MirrorX()
        {
            for (var triangleArrayIndex = 0; triangleArrayIndex < this.Count; triangleArrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[triangleArrayIndex].Count; triangleIndex++)
                {
                    this[triangleArrayIndex][triangleIndex].Vectors[0].Position.Y *= -1;
                    this[triangleArrayIndex][triangleIndex].Vectors[1].Position.Y *= -1;
                    this[triangleArrayIndex][triangleIndex].Vectors[2].Position.Y *= -1;
                    this[triangleArrayIndex][triangleIndex].Flip();
                    this[triangleArrayIndex][triangleIndex].CalcNormal();
                }
            }
        }

        internal void MirrorY()
        {
            for (var triangleArrayIndex = 0; triangleArrayIndex < this.Count; triangleArrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[triangleArrayIndex].Count; triangleIndex++)
                {
                    this[triangleArrayIndex][triangleIndex].Vectors[0].Position.X *= -1;
                    this[triangleArrayIndex][triangleIndex].Vectors[1].Position.X *= -1;
                    this[triangleArrayIndex][triangleIndex].Vectors[2].Position.X *= -1;
                    this[triangleArrayIndex][triangleIndex].Flip();
                    this[triangleArrayIndex][triangleIndex].CalcNormal();
                }
            }
        }

        internal Vector3Class[] ToVector3Array()
        {
            var vector3Array = new List<Vector3Class>();
            for (var triangleArrayIndex = 0; triangleArrayIndex < this.Count; triangleArrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < this[triangleArrayIndex].Count; triangleIndex++)
                {
                    vector3Array.Add(this[triangleArrayIndex][triangleIndex].Vectors[0].Position);
                    vector3Array.Add(this[triangleArrayIndex][triangleIndex].Vectors[1].Position);
                    vector3Array.Add(this[triangleArrayIndex][triangleIndex].Vectors[2].Position);
                }
            }

            return vector3Array.ToArray(); ;
        }
    }


    [Serializable]
    public class TriangleSurfaceIndexInfo
    {
        public ushort ArrayIndex { get; set; }
        public ushort TriangleIndex { get; set; }
    }


    [Serializable]
    public class TriangleSurfaceInfo : Dictionary<TriangleConnectionInfo, bool>
    {
        private bool _selected;

        //public float MinX { get; set; }
        //public float MaxX { get; set; }

        //public float MinY { get; set; }
        //public float MaxY { get; set; }

        internal int Id { get; set; }

        public float BottomPoint { get; set; }
        public float TopPoint { get; set; }
        public float RightPoint { get; set; }
        public float LeftPoint { get; set; }
        public float FrontPoint { get; set; }
        public float BackPoint { get; set; }

        public bool HasEdgeDown { get; set; }
        internal List<TriangleIntersection> SupportPoints { get; set; }
        internal List<MagsAISurfaceIntersectionData> MagsAISupportPoints { get; set; }
        public float Volume { get; set; }

        public bool ProcessedSupportPoints { get; set; }

        //[XmlIgnore]
        //public List<TriangleContourInfo> Contours { get; set; }

        internal STLModel3D Plane
        {
            get
            {
                var plane = new STLModel3D();
                var stlModel = this.Model as STLModel3D;
                plane.Triangles.Add(new List<Triangle>());
                foreach (var surfacePoint in this.Keys)
                {
                    plane.Triangles[0].Add((Triangle)stlModel.Triangles[surfacePoint.ArrayIndex][surfacePoint.TriangleIndex].Clone());
                }
                return plane;
            }
        }

        public TriangleSurfaceInfo Clone(STLModel3D stlModel)
        {
            var surfaceClone = new TriangleSurfaceInfo();
            surfaceClone.BackPoint = this.BackPoint;
            surfaceClone.BottomPoint = this.BottomPoint;
            surfaceClone.CrossSupport = this.CrossSupport;
            surfaceClone.FrontPoint = this.FrontPoint;
            surfaceClone.HasEdgeDown = this.HasEdgeDown;
            surfaceClone.LeftPoint = this.LeftPoint;
            surfaceClone.RightPoint = this.RightPoint;
            surfaceClone.Selected = false;
            surfaceClone.SupportDistance = this.SupportDistance;

            surfaceClone.SupportPoints = new List<TriangleIntersection>();

            foreach (var triangleIndex in this.Keys)
            {
                surfaceClone.Add(new TriangleConnectionInfo() { ArrayIndex = triangleIndex.ArrayIndex, TriangleIndex = triangleIndex.TriangleIndex }, false);
            }

            foreach (var supportPoint in this.SupportPoints)
            {
                surfaceClone.SupportPoints.Add(new TriangleIntersection() { IntersectionPoint = supportPoint.IntersectionPoint });
            }

            surfaceClone.SupportStructure = new List<SupportCone>();
            foreach (var surfaceSupportCone in this.SupportStructure)
            {
                surfaceClone.SupportStructure.Add(surfaceSupportCone.Clone(stlModel));
            }

            return surfaceClone;
        }

        public void CalcVolume(TriangleInfoList triangles)
        {
            this.Volume = 0f;
            foreach (var triangleIndex in this.Keys)
            {
                this.Volume += triangles[triangleIndex.ArrayIndex][triangleIndex.TriangleIndex].Volume;
            }
        }

        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;

            }
        }

        internal object Model
        {
            get
            {
                foreach (var object3d in ObjectView.Objects3D)
                {
                    if (object3d is STLModel3D && !(object3d is SupportCone) && !(object3d is GroundPane))
                    {
                        var stlModel = object3d as STLModel3D;
                        foreach (var surface in stlModel.Triangles.HorizontalSurfaces)
                        {
                            if (surface == this)
                            {
                                return stlModel;
                            }
                        }

                        foreach (var surface in stlModel.Triangles.FlatSurfaces)
                        {
                            if (surface == this)
                            {
                                return stlModel;
                            }
                        }
                    }
                }
                return null;
            }
        }


        private bool _crossSupport;
        private float _supportDistance;

        [Browsable(true)]
        [Category("Support")]
        [DisplayName("Inbetween Distance")]
        [Description("Support Distance\nUnit: factor")]
        public float SupportDistance
        {
            get
            {
                return this._supportDistance;
            }
            set
            {
                if (this._supportDistance != value)
                {

                    this._supportDistance = value;
                }
            }
        }

        [Browsable(true)]
        [Category("Support")]
        [DisplayName("Cross Support")]
        [Description("Enable/Disable Cross support")]
        public bool CrossSupport
        {
            get
            {
                return this._crossSupport;
            }
            set
            {
                if (this._crossSupport != value)
                {
                    //if (this.CrossSupportProcessing != null) this.CrossSupportProcessing(null, null);
                    this._crossSupport = value;
                }
            }
        }

        public bool IsConeSelected
        {
            get
            {
                var result = false;
                foreach (var supportCone in this.SupportStructure)
                {
                    if (supportCone.Selected)
                    {
                        return true;
                    }
                }
                return result;
            }
        }

        public List<SupportCone> SupportStructure;

        internal Vector3[] SelectionBox
        {
            get
            {
                var selectionbox = new Vector3[24];

                //front
                selectionbox[0] = new Vector3(this.LeftPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[1] = new Vector3(this.RightPoint, this.FrontPoint, this.BottomPoint);

                selectionbox[2] = new Vector3(this.RightPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[3] = new Vector3(this.RightPoint, this.FrontPoint, this.TopPoint);

                selectionbox[4] = new Vector3(this.RightPoint, this.FrontPoint, this.TopPoint);
                selectionbox[5] = new Vector3(this.LeftPoint, this.FrontPoint, this.TopPoint);

                selectionbox[6] = new Vector3(this.LeftPoint, this.FrontPoint, this.TopPoint);
                selectionbox[7] = new Vector3(this.LeftPoint, this.FrontPoint, this.BottomPoint);

                selectionbox[8] = new Vector3(this.LeftPoint, this.BackPoint, this.BottomPoint);
                selectionbox[9] = new Vector3(this.RightPoint, this.BackPoint, this.BottomPoint);

                selectionbox[10] = new Vector3(this.RightPoint, this.BackPoint, this.BottomPoint);
                selectionbox[11] = new Vector3(this.RightPoint, this.BackPoint, this.TopPoint);

                selectionbox[12] = new Vector3(this.RightPoint, this.BackPoint, this.TopPoint);
                selectionbox[13] = new Vector3(this.LeftPoint, this.BackPoint, this.TopPoint);

                selectionbox[14] = new Vector3(this.LeftPoint, this.BackPoint, this.TopPoint);
                selectionbox[15] = new Vector3(this.LeftPoint, this.BackPoint, this.BottomPoint);

                //front to back
                selectionbox[16] = new Vector3(this.LeftPoint, this.FrontPoint, this.TopPoint);
                selectionbox[17] = new Vector3(this.LeftPoint, this.BackPoint, this.TopPoint);

                selectionbox[18] = new Vector3(this.RightPoint, this.FrontPoint, this.TopPoint);
                selectionbox[19] = new Vector3(this.RightPoint, this.BackPoint, this.TopPoint);

                selectionbox[20] = new Vector3(this.LeftPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[21] = new Vector3(this.LeftPoint, this.BackPoint, this.BottomPoint);

                selectionbox[22] = new Vector3(this.RightPoint, this.FrontPoint, this.BottomPoint);
                selectionbox[23] = new Vector3(this.RightPoint, this.BackPoint, this.BottomPoint);
                return selectionbox;
            }
        }

        internal void UpdateGridSupport(SupportEventArgs changingEvents)
        {
            var surfaceTriangles = new TriangleInfoList();
            var supportSurfaceModel = (STLModel3D)this.Model;

            if (this.Count < 5000)
            {
                foreach (var surfaceArrayTriangleIndex in this.Keys)
                {
                    surfaceTriangles[0].Add(supportSurfaceModel.Triangles[surfaceArrayTriangleIndex.ArrayIndex][surfaceArrayTriangleIndex.TriangleIndex]);
                }
            }


            var supportConeThreadEvents = new List<ManualResetEvent>();
            changingEvents.SurfaceTriangles.Triangles = surfaceTriangles;
            foreach (var supportCone in this.SupportStructure)
            {
                var supportConeThreadEvent = new ManualResetEvent(false);
                supportConeThreadEvents.Add(supportConeThreadEvent);
                ThreadPool.QueueUserWorkItem(new WaitCallback((_) =>
                {
                    supportCone.Hidden = false;

                    supportCone.Update(changingEvents, supportSurfaceModel);

                    supportConeThreadEvent.Set();

                }));

            }

            Helpers.ThreadHelper.WaitForAll(supportConeThreadEvents.ToArray());

            supportSurfaceModel.SupportBasement = false;
            supportSurfaceModel.SupportBasement = true;
        }


        internal void CalcLowestPoints(TriangleInfoList triangles)
        {
            if (this.Volume > 25)
            {
                var lowestPoint = new Vector3Class(0, 0, float.MaxValue);
                var lowestTriangleConnection = new TriangleConnectionInfo();
                //find start point
                foreach (var connection in this.Keys)
                {
                    if (triangles[connection.ArrayIndex][connection.TriangleIndex].Bottom < lowestPoint.Z)
                    {
                        if (triangles[connection.ArrayIndex][connection.TriangleIndex].Vectors[0].Position.Z == triangles[connection.ArrayIndex][connection.TriangleIndex].Bottom)
                        {
                            lowestPoint = triangles[connection.ArrayIndex][connection.TriangleIndex].Vectors[0].Position;
                        }
                        else if (triangles[connection.ArrayIndex][connection.TriangleIndex].Vectors[1].Position.Z == triangles[connection.ArrayIndex][connection.TriangleIndex].Bottom)
                        {
                            lowestPoint = triangles[connection.ArrayIndex][connection.TriangleIndex].Vectors[1].Position;
                        }
                        else if (triangles[connection.ArrayIndex][connection.TriangleIndex].Vectors[2].Position.Z == triangles[connection.ArrayIndex][connection.TriangleIndex].Bottom)
                        {
                            lowestPoint = triangles[connection.ArrayIndex][connection.TriangleIndex].Vectors[2].Position;
                        }
                        lowestTriangleConnection = connection;
                    }
                }

                //Engines.MagsAI.MagsAIEngine.DebugPoints.Add(new Engines.MagsAI.MagsAIEdgePoint(new Vector2(), lowestPoint.Xy, new Vector2(), 0, lowestPoint.Z));

                ////create xy pixel raster
                //var xyRaster = new Vector3[1920, 1080];
                //for (var x = 0; x < 1920; x++)
                //{
                //    for (var y = 0; y < 1080; y++)
                //    {
                //        Engines.MagsAI.MagsAIEngine.DebugPoints.Add(new Engines.MagsAI.MagsAIEdgePoint(new Vector2(), new Vector2((x / 20f) - 48, (y / 20f) - 27), new Vector2(), 0, lowestPoint.Z));
                //    }
                //}


            }
        }

        internal void UpdateBoundries(TriangleInfoList triangles)
        {

            var initialBaseline = true;
            if (triangles != null)
            {
                var moveTranslation = new Vector3Class();
                lock (this.SupportStructure)
                {
                    if (this.SupportStructure.Count > 0 && this.SupportStructure[0] != null)
                    {
                        moveTranslation = this.SupportStructure[0].MoveTranslation;
                    }

                    //check surface point
                    foreach (var surfacePoint in this.Keys)
                    {

                        for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                        {
                            var z = triangles[surfacePoint.ArrayIndex][surfacePoint.TriangleIndex].Vectors[vectorIndex].Position.Z;
                            var x = triangles[surfacePoint.ArrayIndex][surfacePoint.TriangleIndex].Vectors[vectorIndex].Position.X + moveTranslation.X;
                            var y = triangles[surfacePoint.ArrayIndex][surfacePoint.TriangleIndex].Vectors[vectorIndex].Position.Y + moveTranslation.Y;
                            if (initialBaseline)
                            {
                                this.BottomPoint = z;
                                this.TopPoint = z;
                                this.LeftPoint = x;
                                this.RightPoint = x;
                                this.FrontPoint = y;
                                this.BackPoint = y;
                                initialBaseline = false;
                            }

                            else
                            {
                                //lowestpoint
                                if (z < this.BottomPoint)
                                {
                                    this.BottomPoint = z;
                                }

                                //left point
                                if (x < this.LeftPoint)
                                {
                                    this.LeftPoint = x;
                                }

                                //right point
                                if (x > this.RightPoint)
                                {
                                    this.RightPoint = x;
                                }

                                //highest point
                                if (z > this.TopPoint)
                                {
                                    this.TopPoint = z;
                                }

                                //frontest point
                                if (y > this.FrontPoint)
                                {
                                    this.FrontPoint = y;
                                }

                                //Depthest point
                                if (this.BackPoint > y)
                                {
                                    this.BackPoint = y;
                                }
                            }
                        }
                    }


                    foreach (var supportCone in this.SupportStructure)
                    {
                        if (supportCone != null)
                        {
                            if (supportCone.BottomPoint < this.BottomPoint)
                            {
                                this.BottomPoint = supportCone.BottomPoint;
                            }
                        }
                    }
                }

            }


        }

        public TriangleSurfaceInfo()
        {
            this.SupportStructure = new List<SupportCone>();
            this.SupportPoints = new List<TriangleIntersection>();
            this.MagsAISupportPoints = new List<MagsAISurfaceIntersectionData>();
            //            this.Contours = new List<TriangleContourInfo>();
        }

        //public bool Contains(int arrayIndex, int triangleIndex)
        //{
        //    var result = false;
        //    foreach (var item in this)
        //    {
        //        if ((item.ArrayIndex == arrayIndex) && (item.TriangleIndex == triangleIndex))
        //        {
        //            result = true;
        //        }
        //    }

        //    return result;
        //}

        public void Append(TriangleConnectionInfo triangleConnection)
        {
            if (!ContainsKey(triangleConnection))
            {
                this.Add(triangleConnection, false);
            }
        }

        public float MinXByY(float y, TriangleInfoList triangles)
        {
            var minX = 100000f;
            foreach (var surfaceItem in this.Keys)
            {
                var triangle = triangles[surfaceItem.ArrayIndex][surfaceItem.TriangleIndex];
                if (triangle.Back <= y && triangle.Front >= y && triangle.Left < minX)
                {
                    minX = triangle.Left;
                }
            }
            return minX;
        }

        public float MaxXByY(float y, TriangleInfoList triangles)
        {
            var maxX = -100000f;
            foreach (var surfaceItem in this.Keys)
            {
                var triangle = triangles[surfaceItem.ArrayIndex][surfaceItem.TriangleIndex];
                if (triangle.Back <= y && triangle.Front >= y && triangle.Right > maxX)
                {
                    maxX = triangle.Right;
                }
            }
            return maxX;
        }

        public float MinYByX(float x, TriangleInfoList triangles)
        {
            var minY = 100000f;
            foreach (var surfaceItem in this.Keys)
            {
                var triangle = triangles[surfaceItem.ArrayIndex][surfaceItem.TriangleIndex];
                if (triangle.Left <= x && triangle.Right >= x && triangle.Back < minY)
                {
                    minY = triangle.Back;
                }
            }

            if (minY == 100000f)
            {
                Debug.WriteLine("test");
            }
            return minY;
        }

        public float MaxYByX(float x, TriangleInfoList triangles)
        {
            var maxY = -100000f;
            foreach (var surfaceItem in this.Keys)
            {
                var triangle = triangles[surfaceItem.ArrayIndex][surfaceItem.TriangleIndex];
                if (triangle.Left <= x && triangle.Right >= x && triangle.Front > maxY)
                {
                    maxY = triangle.Front;
                }
            }
            return maxY;
        }

        public bool HPointInside(float x, float y, TriangleInfoList triangles)
        {
            var result = false;
            foreach (var surfaceItem in this.Keys)
            {
                var triangle = triangles[surfaceItem.ArrayIndex][surfaceItem.TriangleIndex];
                var planeAB = (triangle.Vectors[0].Position.X - x) * (triangle.Vectors[1].Position.Y - y) - (triangle.Vectors[1].Position.X - x) * (triangle.Vectors[0].Position.Y - y);
                var planeBC = (triangle.Vectors[1].Position.X - x) * (triangle.Vectors[2].Position.Y - y) - (triangle.Vectors[2].Position.X - x) * (triangle.Vectors[1].Position.Y - y);
                var planeCA = (triangle.Vectors[2].Position.X - x) * (triangle.Vectors[0].Position.Y - y) - (triangle.Vectors[0].Position.X - x) * (triangle.Vectors[2].Position.Y - y);
                triangle.CalcCenter();

                if (Sign(planeAB) == Sign(planeBC) && Sign(planeBC) == Sign(planeCA))
                {
                    return true;
                }


            }
            return result;
        }
        private float Sign(float n)
        {
            return Math.Abs(n) / n;

        }

        internal STLModel3D AsStlModel(STLModel3D model)
        {
            var surfaceTrianglesAsStl = new STLModel3D();
            surfaceTrianglesAsStl.Triangles = new TriangleInfoList();

            foreach (var surfaceArrayTriangleIndex in this.Keys)
            {
                surfaceTrianglesAsStl.Triangles[0].Add(model.Triangles[surfaceArrayTriangleIndex.ArrayIndex][surfaceArrayTriangleIndex.TriangleIndex]);
            }

            return surfaceTrianglesAsStl;
        }
    }


    [Serializable]
    public class TriangleSurfaceInfoList : List<TriangleSurfaceInfo>
    {
        internal List<SupportCone> SupportStructure
        {
            get
            {
                var result = new List<SupportCone>();
                foreach (var surface in this)
                {
                    result.AddRange(surface.SupportStructure);
                }
                return result;
            }
        }

        internal List<byte[]> ToByteArray()
        {
            var result = new List<byte[]>();

            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, this);
                result.Add(ms.ToArray());
            }

            return result;
        }

        internal void UpdateBoundries(TriangleInfoList triangles)
        {
            try
            {
                lock (this)
                {
                    foreach (var surface in this)
                    {
                        surface.UpdateBoundries(triangles);
                    }
                }
            }
            catch
            {

            }
        }

        internal void UpdateMoveTranslation(Vector3Class moveTranslation)
        {
            foreach (var surface in this)
            {
                for (var supportPointIndex = 0; supportPointIndex < surface.SupportPoints.Count; supportPointIndex++)
                {
                    surface.SupportPoints[supportPointIndex].IntersectionPoint += moveTranslation;
                }
            }
        }

        internal sealed class AllowAllAssemblyVersionsDeserializationBinder : System.Runtime.Serialization.SerializationBinder
        {
            public override Type BindToType(string assemblyName, string typeName)
            {
                Type typeToDeserialize = null;

                String currentAssembly = System.Reflection.Assembly.GetExecutingAssembly().FullName;

                // In this case we are always using the current assembly
                assemblyName = currentAssembly;

                // Get the type using the typeName and assemblyName
                typeToDeserialize = Type.GetType(String.Format("{0}, {1}", typeName, assemblyName));

                return typeToDeserialize;
            }
        }

        //public static TriangleSurfaceInfoList Deserialize(byte[] b)
        //{
        //    TriangleSurfaceInfoList mro = null;
        //    BinaryFormatter formatter = new BinaryFormatter();
        //    MemoryStream ms = new MemoryStream(b);

        //    // To prevent errors serializing between version number differences (e.g. Version 1 serializes, and Version 2 deserializes)
        //    formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();

        //    mro = (TriangleSurfaceInfoList)formatter.Deserialize(ms);
        //    ms.Close();
        //    return mro;
        //}
    }
}