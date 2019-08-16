using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using static Atum.Studio.Core.Helpers.ContourHelper;

namespace Atum.Studio.Core.Models
{
    [Serializable]
    public class SupportConeV2 : SupportCone
    {
        public SortedDictionary<float, PolyTree> SliceContours;

        public List<Vector3> InterlinkDebugPoints = new List<Vector3>();
        public SortedDictionary<float, List<SupportConeV2InterlinkConnection>> InterlinkConnections;

        [Browsable(false)]
        public TriangleIntersection ModelIntersection { get; set; }

        [Category("Dimensions")]
        [DisplayName("A-Segment Height")]
        [Description("Height of Cone\nUnit: mm")]
        public float ASegmentHeight { get; set; }

        [Category("Dimensions")]
        [DisplayName("A-Segment Radius")]
        [Description("Radius of Cone\nUnit: mm")]
        public float ASegmentRadius { get; set; }

        [Category("Dimensions")]
        [DisplayName("B-Segment Height")]
        [Description("Part Height of Cone\nUnit: mm")]
        public float BSegmentHeight { get; set; }

        [Category("Dimensions")]
        [DisplayName("B-Segment Radius")]
        [Description("Radius of Cone\nUnit: mm")]
        public float BSegmentRadius { get; set; }

        [Category("Dimensions")]
        [DisplayName("C-Segment Radius")]
        [Description("Radius of Sphere\nUnit: mm")]
        public float CSegmentRadius { get; set; }

        [Category("Dimensions")]
        [DisplayName("D-Segment Height")]
        [Description("Part Height of Cone\nUnit: mm")]
        public float DSegmentHeight { get; set; }

        [Category("Dimensions")]
        [DisplayName("D-Segment Radius")]
        [Description("Radius of Cone\nUnit: mm")]
        public float DSegmentRadius { get; set; }

        [Category("Dimensions")]
        [DisplayName("E-Segment Height")]
        [Description("Part Height of Cone\nUnit: mm")]
        public override float TopHeight { get; set; }

        [Category("Dimensions")]
        [DisplayName("E-Segment Radius")]
        [Description("Radius of Cone\nUnit: mm")]
        public override float TopRadius { get; set; }

        [Category("Dimensions")]
        [DisplayName("F-Segment Radius")]
        [Description("Radius of Cone\nUnit: mm")]
        public override float MiddleRadius { get; set; }

        [Category("Dimensions")]
        [DisplayName("G-Segment Height")]
        [Description("Part Height of Cone\nUnit: mm")]
        public override float BottomHeight { get; set; }

        [Category("Dimensions")]
        [DisplayName("G-Segment Radius")]
        [Description("Radius of Cone\nUnit: mm")]
        public override float BottomRadius { get; set; }


        internal Cone BottomSupportCone { get; set; }
        internal Cone MiddleSupportCone { get; set; }
        internal Cone TopSupportCone { get; set; }
        internal Cone DSegment { get; set; }
        internal Sphere CSegment { get; set; }
        internal Cone BSegment { get; set; }
        internal Sphere ASegment { get; set; }
        internal ConeCap ASegmentCap { get; set; }
        internal ConeCap BSegmentCap { get; set; }

        internal Vector3Class RefPointCSegment { get; set; }


        private List<Triangle> TotalTriangles
        {
            get
            {
                var result = new List<Triangle>();
                result.AddRange(this.BottomSupportCone[0]);
                result.AddRange(this.MiddleSupportCone[0]);
                result.AddRange(this.TopSupportCone[0]);
                result.AddRange(this.DSegment[0]);
                result.AddRange(this.CSegment.Triangles[0]);
                result.AddRange(this.BSegment[0]);
                result.AddRange(this.ASegment.Triangles[0]);
                //result.AddRange(this.ASegmentCap[0]);
                // result.AddRange(this.BSegmentCap[0]);

                return result;
            }
        }

        internal SupportConeV2(float topHeight, float topRadius, float middleRadius, float bottomHeight, float bottomRadius, int slicesCount, TriangleIntersection modelIntersection, Color color, STLModel3D stlModel = null, bool groundSupport = true, bool useSupportPenetration = false, STLModel3D surfaceTriangles = null, TypeSupportCone supportConeType = TypeSupportCone.Normal, float bottomWidthCorrection = 0f,
            float aSegmentRadius = float.MinValue, float aSegmentHeight = float.MinValue, float bSegmentRadius = float.MinValue, float bSegmentHeight = float.MinValue, float cSegmentRadius = float.MinValue, float dSegmentRadius = float.MinValue, float dSegmentHeight = float.MinValue, bool calcSliceContours = true)
        {
            this._color = color;

            this.SupportConeVersion = TypeSupportConeVersion.Version2;

            this.ModelIntersection = modelIntersection;

            this.ASegmentHeight = aSegmentHeight;
            this.ASegmentRadius = aSegmentRadius;
            this.BSegmentHeight = bSegmentHeight;
            this.BSegmentRadius = bSegmentRadius;
            this.CSegmentRadius = cSegmentRadius;
            this.DSegmentHeight = dSegmentHeight;
            this.DSegmentRadius = dSegmentRadius;

            //supportcone properties
            this.TopRadius = topRadius;
            this.TopHeight = topHeight;
            this.MiddleRadius = middleRadius;
            this.BottomHeight = bottomHeight;
            this.BottomRadius = bottomRadius;

            this.MiddleRadius = bSegmentRadius;
            middleRadius = bSegmentRadius;

            //determine rotation angle
            float angleY;
            float angleZ;
            VectorHelper.CalcRotationAnglesYZFromVector(modelIntersection.Normal, false, out angleZ, out angleY);

            var aSegmentRotationY = 0f;

            this._rotationAngleY = angleY;
            this._rotationAngleZ = angleZ;

            if (angleZ < 0)
            {
                this._rotationAngleZ = 360 + angleZ;
            }

            if (this.RotationAngleY > 180 && this.RotationAngleY < 270f)
            {
                this._rotationAngleY = 225 - (((270f - this.RotationAngleY) / 90f) * 45f);
                aSegmentRotationY = 270 - (270f - angleY);
            }
            else if (this.RotationAngleY > 90 && this.RotationAngleY < 180)
            {
                this._rotationAngleY = 180 - (((180 - this.RotationAngleY) / 90f) * 45f);
                aSegmentRotationY = 180 - (180 - angleY);
            }

            //create segments           
            this.ASegment = new Sphere(aSegmentRadius, new Vector3(),
                    Sphere.eSubdivisions.Two,
                    new Sphere.eDir[] { Sphere.eDir.All },
                    modelIntersection.IntersectionPoint,
                    new Structs.Byte4Class(this.Color.A, color.R, color.G, color.B)
                    );
            
            this.RefPointCSegment = new Vector3Class(0, 0, aSegmentHeight + bSegmentHeight + cSegmentRadius);
            var refPointRotationVector = this.Rotate(this.RotationAngleY, this.RotationAngleZ, this.RefPointCSegment);
            this.RefPointCSegment = refPointRotationVector + modelIntersection.IntersectionPoint;
            var refPointCSegmentXY = new Vector3Class(this.RefPointCSegment.X, this.RefPointCSegment.Y, 0);
            refPointRotationVector.Normalize();



            var middleHeight = this.RefPointCSegment.Z - cSegmentRadius - dSegmentHeight - topHeight - bottomHeight;

            //add bottom caps to b-seqment
            this.BSegment = Cone.Create(bSegmentRadius, aSegmentRadius - 0.1f, (aSegmentRadius / 2) + bSegmentHeight + cSegmentRadius, slicesCount, new Vector3Class());

            //rotate B segment using normal
            this.BSegment.Rotate(this.RotationAngleY, this.RotationAngleZ);
            this.BSegment.Translate(modelIntersection.IntersectionPoint);
            this.BSegmentCap = new ConeCap(this.RefPointCSegment, this.BSegment.BottomPoints, false);

            //merge a and b-segments
            //var aSegmentBottomPoints = this.ASegment.BottomPoints;
            var bSegmentTopPoints = this.BSegment.TopPoints;


            //middle circle
            this.CSegment = (new Sphere(cSegmentRadius * 1.1f,
                new Vector3(),
                Sphere.eSubdivisions.Two,
                new Sphere.eDir[] { Sphere.eDir.All },
                this.RefPointCSegment,
                new Structs.Byte4Class(this.Color.A, color.R, color.G, color.B)));

            if (middleHeight >= 0f)
            {
                this.DSegment = Cone.Create(dSegmentRadius, topRadius, dSegmentHeight + cSegmentRadius, slicesCount, this.RefPointCSegment - new Vector3Class(0, 0, (dSegmentHeight + cSegmentRadius)));

                this.TopSupportCone = Cone.Create(topRadius, middleRadius, topHeight, slicesCount, this.RefPointCSegment - new Vector3Class(0, 0, dSegmentHeight + cSegmentRadius + topHeight));
                this.MiddleSupportCone = Cone.Create(middleRadius, middleRadius + (middleHeight * bottomWidthCorrection / 100f), middleHeight, slicesCount, refPointCSegmentXY + new Vector3Class(0, 0, bottomHeight));
            }
            else
            {
                this.DSegment = new Cone();
                this.TopSupportCone = new Cone();
                this.MiddleSupportCone = Cone.Create(middleRadius, middleRadius, (this.RefPointCSegment.Z - this.BottomHeight), slicesCount, refPointCSegmentXY + new Vector3Class(0, 0, this.BottomHeight));
            }

            this.BottomSupportCone = Cone.Create(middleRadius + (middleHeight * (bottomWidthCorrection / 100f)), bottomRadius + (middleHeight * (bottomWidthCorrection / 100f)), bottomHeight, slicesCount, refPointCSegmentXY);
            var capPoints = VectorHelper.CreateCircle(0, bottomRadius + (middleHeight * bottomWidthCorrection / 100f), slicesCount, false);
            var supportConeCap = base.CreateCap(capPoints, new Vector3Class(), new Vector3Class(), false);

            foreach (var supportConeCapPoint in supportConeCap[0])
            {
                supportConeCapPoint.Vectors[0].Position += refPointCSegmentXY;
                supportConeCapPoint.Vectors[1].Position += refPointCSegmentXY;
                supportConeCapPoint.Vectors[2].Position += refPointCSegmentXY;
            }
            this.BottomSupportCone[0].AddRange(supportConeCap[0]);

            this.Triangles = new TriangleInfoList();
            this.Triangles[0].AddRange(this.TotalTriangles);

            foreach (var triangle in this.Triangles[0])
            {
                triangle.CalcMinMaxX();
                triangle.CalcMinMaxY();
                triangle.CalcMinMaxZ();
                triangle.CalcCenter();
                triangle.UpdateColor(this.ColorAsByte4, true);
            }

            this.UpdateBoundries();

            if (this.VBOIndexes == null || this.VBOIndexes.Length == 0)
            {
                this.BindModel();
                this.UpdateBinding();
            }

        }

        internal void CalcSlicesContours(Material selectedMaterial, AtumPrinter selectedPrinter)
        {
            var resultAB = new SortedDictionary<float, PolyTree>();

            var abSegmentAsSTLModel = new SupportCone();
            abSegmentAsSTLModel.Triangles = this.ASegment.Triangles;
            //abSegmentAsSTLModel.Triangles[0].AddRange(this.ASegmentCap[0]);
            abSegmentAsSTLModel.Triangles[0].AddRange(this.BSegment[0]);
            abSegmentAsSTLModel.Triangles[0].AddRange(this.BSegmentCap[0]);

            foreach (var triangle in abSegmentAsSTLModel.Triangles[0])
            {
                triangle.CalcMinMaxZ();
            }

            abSegmentAsSTLModel.UpdateBoundries();
            abSegmentAsSTLModel.CalcSliceIndexes(selectedMaterial, false);

            var sliceIndex = 0;
            foreach (var sliceHeight in abSegmentAsSTLModel.SliceIndexes.Keys)
            {
                resultAB.Add(sliceHeight, STLModel3D.GetSliceContoursForSupportConeV2(abSegmentAsSTLModel, sliceIndex, sliceHeight, selectedPrinter));

                sliceIndex++;
            }
            //c segment
            var cSegmentAsSTLModel = new SupportCone();
            cSegmentAsSTLModel.Triangles = this.CSegment.Triangles;

            foreach (var triangle in cSegmentAsSTLModel.Triangles[0])
            {
                triangle.CalcMinMaxZ();
                triangle.CalcMinMaxX();
                triangle.CalcMinMaxY();
                triangle.CalcCenter();
            }

            cSegmentAsSTLModel.UpdateBoundries();
            cSegmentAsSTLModel.CalcSliceIndexes(selectedMaterial, false);

            var resultC = new SortedDictionary<float, PolyTree>();
            sliceIndex = 0;
            foreach (var sliceHeight in cSegmentAsSTLModel.SliceIndexes.Keys)
            {
                resultC.Add(sliceHeight, STLModel3D.GetSliceContoursForSupportConeV2(cSegmentAsSTLModel, sliceIndex, sliceHeight, selectedPrinter));

                sliceIndex++;
            }

            //defg segment
            var defgSegmentAsSTLModel = new SupportCone();
            defgSegmentAsSTLModel.Triangles = this.DSegment;
            defgSegmentAsSTLModel.Triangles[0].AddRange(this.TopSupportCone[0]);
            defgSegmentAsSTLModel.Triangles[0].AddRange(this.MiddleSupportCone[0]);
            defgSegmentAsSTLModel.Triangles[0].AddRange(this.BottomSupportCone[0]);

            foreach (var triangle in defgSegmentAsSTLModel.Triangles[0])
            {
                triangle.CalcMinMaxZ();
            }

            defgSegmentAsSTLModel.UpdateBoundries();
            defgSegmentAsSTLModel.CalcSliceIndexes(selectedMaterial, false);

            var resultDEFG = new SortedDictionary<float, PolyTree>();
            sliceIndex = 0;
            foreach (var sliceHeight in defgSegmentAsSTLModel.SliceIndexes.Keys)
            {
                resultDEFG.Add(sliceHeight, STLModel3D.GetSliceContoursForSupportConeV2(defgSegmentAsSTLModel, sliceIndex, sliceHeight, selectedPrinter));

                sliceIndex++;
            }

            //combine all 
            foreach (var sliceHeight in resultC.Keys)
            {
                if (!resultAB.ContainsKey(sliceHeight))
                {
                    resultAB.Add(sliceHeight, resultC[sliceHeight]);
                }
                else
                {
                    resultAB[sliceHeight] = ContourHelper.UnionModelSliceLayer(resultAB[sliceHeight], resultC[sliceHeight]);
                }
            }

            foreach (var sliceHeight in resultDEFG.Keys)
            {
                if (!resultAB.ContainsKey(sliceHeight))
                {
                    resultAB.Add(sliceHeight, resultDEFG[sliceHeight]);
                }
                else
                {
                    resultAB[sliceHeight] = ContourHelper.UnionModelSliceLayer(resultAB[sliceHeight], resultDEFG[sliceHeight]);
                }
            }


            this.SliceContours = resultAB;

            foreach(var interlinkConnectionIndex in this.InterlinkConnections)
            {
                if (interlinkConnectionIndex.Value != null)
                {
                    foreach(var interlinkConnection in interlinkConnectionIndex.Value)
                    {
                        if (interlinkConnection != null)
                        {
                            interlinkConnection.CalcSlicesContours(selectedMaterial, selectedPrinter);
                        }
                    }
                }
            }
        }

        internal override void Draw(Color color, Color selectedSupportConeColor, Color modelIntersectionColor, bool selectionColorEnabled = true, bool gridSupport = false)
        {
            var moveTranslation = this.MoveTranslation;
            var supportConeColor = color;

            GL.Translate(new Vector3(moveTranslation.X, moveTranslation.Y, 0));

            if (selectionColorEnabled &&
                (SceneView.CurrentViewMode == SceneView.ViewMode.SelectObject ||
                SceneView.CurrentViewMode == SceneView.ViewMode.Pan ||
                SceneView.CurrentViewMode == SceneView.ViewMode.Orbit ||
                SceneView.CurrentViewMode == SceneView.ViewMode.Zoom ||
                SceneView.CurrentViewMode == SceneView.ViewMode.Duplicate)
                )
            {
                if (this.Selected)
                {
                    supportConeColor = Color.FromArgb(this.Color.A, selectedSupportConeColor);
                }

                else
                {
                    supportConeColor = Color.FromArgb(this.Color.A, color);
                }

            }
            else
            {
                supportConeColor = Color.FromArgb(this.Color.A, this._color);
            }

            GL.PushMatrix();
            GL.EnableClientState(ArrayCap.ColorArray);
            GL.EnableClientState(ArrayCap.VertexArray);
            GL.EnableClientState(ArrayCap.NormalArray);

            var triangleIndex = 0;
            foreach (var vboIndex in this.VBOIndexes)
            {
                GL.BindBuffer(BufferTarget.ArrayBuffer, vboIndex);
                GL.ColorPointer(4, ColorPointerType.UnsignedByte, Vertex.Stride, new IntPtr(0));
                GL.VertexPointer(3, VertexPointerType.Float, Vertex.Stride, 4);
                GL.NormalPointer(NormalPointerType.Float, Vertex.Stride, new IntPtr(16));
                GL.DrawArrays(PrimitiveType.Triangles, 0, this.Triangles[triangleIndex].Count * 3);

                triangleIndex++;
            }
            GL.DisableClientState(ArrayCap.ColorArray);
            GL.DisableClientState(ArrayCap.VertexArray);
            GL.DisableClientState(ArrayCap.NormalArray);

            GL.PopMatrix();

            //var totalTriangles = this.TotalTriangles;
            //foreach (var triange in totalTriangles)
            //{
            //    GL.Normal3(triange.Normal.ToStruct());
            //    foreach (var point in triange.Points)
            //    {
            //        GL.Vertex3((point + this.MoveTranslation).ToStruct());
            //    }
            //}


            //if (this.InterlinkConnections != null)
            //{
            //    foreach(var interlinkConnections in this.InterlinkConnections.Values)
            //    {
            //        foreach (var interlinkConnection in interlinkConnections)
            //        {
            //            foreach (var triange in interlinkConnection.Triangles[0])
            //            {
            //                GL.Normal3(triange.Normal.ToStruct());
            //                foreach (var point in triange.Points)
            //                {
            //                    GL.Vertex3((point + this.MoveTranslation).ToStruct());
            //                }
            //            }
            //        }
            //    }
            //}

            GL.Translate(-new Vector3(moveTranslation.X, moveTranslation.Y, 0));

        }


        private Vector3Class Rotate(float angleY, float angleZ, Vector3Class vector)
        {
            Matrix4 rotationMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(angleY));
            vector = Vector3Class.Transform(vector, rotationMatrix);

            rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(angleZ));
            vector = Vector3Class.Transform(vector, rotationMatrix);

            return vector;
        }

        private void Rotate(float angleY, float angleZ, TriangleInfoList triangles)
        {
            Matrix4 rotationMatrix = Matrix4.CreateRotationY(OpenTK.MathHelper.DegreesToRadians(angleY));

            for (var arrayIndex = 0; arrayIndex < triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = rotatedVector;
                    }
                }
            }

            rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(angleZ));

            for (var arrayIndex = 0; arrayIndex < triangles.Count; arrayIndex++)
            {
                for (var triangleIndex = 0; triangleIndex < triangles[arrayIndex].Count; triangleIndex++)
                {
                    for (var vectorIndex = 0; vectorIndex < 3; vectorIndex++)
                    {
                        var rotatedVector = Vector3Class.Transform(triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position, rotationMatrix);
                        triangles[arrayIndex][triangleIndex].Vectors[vectorIndex].Position = rotatedVector;
                    }
                }
            }
        }

        public void Update()
        {
            var supportCone = new SupportConeV2(this.TopHeight, this.TopRadius, this.MiddleRadius, this.BottomHeight, this.BottomRadius, 16, this.ModelIntersection, this.Color, this.Model, this._groundSupport, true, null, this.SupportConeType, this.BottomWidthCorrection,
                this.ASegmentRadius, this.ASegmentHeight, this.BSegmentRadius, this.BSegmentHeight, this.CSegmentRadius, this.DSegmentRadius, this.DSegmentHeight, true);

            this.ASegment = supportCone.ASegment;
            this.BSegment = supportCone.BSegment;
            this.CSegment = supportCone.CSegment;
            this.DSegment = supportCone.DSegment;

            this.TopSupportCone = supportCone.TopSupportCone;
            this.MiddleSupportCone = supportCone.MiddleSupportCone;
            this.BottomSupportCone = supportCone.BottomSupportCone;

            this.Triangles = supportCone.Triangles;
        }

        public void UpdateInterlinkConnections(SupportConeV2 targetSupportCone, SupportProfile supportProfile)
        {
            for (var intermittedHeight = this.BottomHeight + supportProfile.SupportIntermittedConnectionHeight; intermittedHeight < targetSupportCone.RefPointCSegment.Z; intermittedHeight += supportProfile.SupportIntermittedConnectionHeight)
            {
                var startPoint = new Vector3Class(this.RefPointCSegment.X, this.RefPointCSegment.Y, intermittedHeight);
                var endPoint = targetSupportCone.RefPointCSegment;
                var xyDistance = endPoint - startPoint;
                xyDistance.Z = 0;
                endPoint.Z = intermittedHeight + (supportProfile.SupportIntermittedConnectionHeight * xyDistance.Length);

                if (InterlinkConnections == null)
                {
                    this.InterlinkConnections = new SortedDictionary<float, List<SupportConeV2InterlinkConnection>>();
                }

                if (!this.InterlinkConnections.ContainsKey(intermittedHeight))
                {
                    this.InterlinkConnections.Add(intermittedHeight, new List<SupportConeV2InterlinkConnection>());
                }

                this.InterlinkConnections[intermittedHeight].Add(new SupportConeV2InterlinkConnection(0.1f, startPoint, endPoint, Color.Yellow));
            }
        }

        public void UpdateInterlinkConnections(SupportProfile supportProfile)
        {
            this.InterlinkDebugPoints = new List<Vector3>();
            this.InterlinkConnections = new SortedDictionary<float, List<SupportConeV2InterlinkConnection>>();

            //find nearest XY supportcones
            var supportConeDistances = new SortedDictionary<float, List<SupportConeV2>>();
            foreach (var supportCone in this.Model.SupportStructure.OfType<SupportConeV2>())
            {
                if (supportCone != null && supportCone.RefPointCSegment != this.RefPointCSegment)
                {
                    var supportConeDistanceXY = (this.RefPointCSegment.Xy - supportCone.RefPointCSegment.Xy).Length;
                    if (!supportConeDistances.ContainsKey(supportConeDistanceXY))
                    {
                        supportConeDistances.Add(supportConeDistanceXY, new List<SupportConeV2>());
                    }

                    supportConeDistances[supportConeDistanceXY].Add(supportCone);
                }
            }

            //check if there are support cones within the range of > 3x middle radius && distance < 5
            var supportConesWithinXYRange = new SortedDictionary<float, List<SupportConeV2>>();
            foreach (var xyDistance in supportConeDistances.Keys)
            {
                if (xyDistance > 2 * this.MiddleRadius && xyDistance < 5)
                {
                    if (!supportConesWithinXYRange.ContainsKey(xyDistance))
                    {
                        supportConesWithinXYRange.Add(xyDistance, new List<SupportConeV2>());
                    }

                    supportConesWithinXYRange[xyDistance].AddRange(supportConeDistances[xyDistance]);
                }
            }

            //create interlink at each intermitted height
            var supportConeHeight = this.RefPointCSegment.Z;
            var interlinkedSupportConesWithinRanges = new List<SupportConeV2>();
            for (var intermittedHeight = this.BottomHeight + supportProfile.SupportIntermittedConnectionHeight; intermittedHeight < supportConeHeight; intermittedHeight += supportProfile.SupportIntermittedConnectionHeight)
            {
                var interlinkSupportCones = new List<SupportConeV2>();

                //find first support cone that is high enough
                foreach (var supportConesWithinXYValues in supportConesWithinXYRange)
                {
                    foreach (var supportCone in supportConesWithinXYValues.Value)
                    {
                        if (interlinkSupportCones.Count == 2)
                        {
                            break;
                        }

                        if (supportCone != null)
                        {
                            if (supportCone.RefPointCSegment.Z - (supportProfile.SupportIntermittedConnectionHeight * supportConesWithinXYValues.Key) > intermittedHeight)
                            {
                                //check if support cone is within range
                                if (interlinkSupportCones.Count == 1)
                                {
                                    if (IsSupportConeWithinXYAngleRange(interlinkSupportCones.First(), supportCone, 120, 240))
                                    {
                                        interlinkSupportCones.Add(supportCone);

                                        if (!interlinkedSupportConesWithinRanges.Contains(supportCone))
                                        {
                                            interlinkedSupportConesWithinRanges.Add(supportCone);
                                        }

                                        //if ((supportCone.RefPointCSegment.Xy - interlinkSupportCones[0].RefPointCSegment.Xy).Length < 1.1)
                                        //{
                                        //    continue;
                                        //}
                                    }

                                    if (interlinkSupportCones.Count == 2)
                                    {
                                        break;
                                    }
                                }
                                else if (interlinkSupportCones.Count == 0)
                                {
                                    // no support cones then add first found
                                    if ((supportCone.RefPointCSegment.Xy - this.RefPointCSegment.Xy).Length < (supportProfile.SupportOverhangDistance * 1.1))
                                    {
                                        interlinkSupportCones.Add(supportCone);

                                        if (!interlinkedSupportConesWithinRanges.Contains(supportCone))
                                        {
                                            interlinkedSupportConesWithinRanges.Add(supportCone);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                //create connections to interlinked support cones
                var startPoint = new Vector3Class(this.RefPointCSegment.X, this.RefPointCSegment.Y, intermittedHeight);

                foreach (var interLinkSupportCone in interlinkSupportCones)
                {
                    var endPoint = interLinkSupportCone.RefPointCSegment;
                    var xyDistance = endPoint - startPoint;
                    xyDistance.Z = 0;
                    endPoint.Z = intermittedHeight + (supportProfile.SupportIntermittedConnectionHeight * xyDistance.Length);

                    if (!this.InterlinkConnections.ContainsKey(intermittedHeight))
                    {
                        this.InterlinkConnections.Add(intermittedHeight, new List<SupportConeV2InterlinkConnection>());
                    }

                    this.InterlinkConnections[intermittedHeight].Add(new SupportConeV2InterlinkConnection(0.1f, startPoint, endPoint, Color.Yellow));
                }
            }

            var t = new List<SupportConeV2InterlinkConnection>();
            foreach (var v in this.InterlinkConnections.Values)
            {
                t.AddRange(v);
            }
            if (t.Count < 2)
            {
                var g = 0f;
                //this.InterlinkConnections.Add(0, new List<SupportConeV2InterlinkConnection>()); 
                //this.InterlinkConnections[0].Add(new SupportConeV2InterlinkConnection(0.1f, ( this.RefPointCSegment), (this.RefPointCSegment) + new Vector3Class(5,0,0), Color.Yellow));

                if (t.Count == 1)
                {
                    //create new support cone within the boundries of first and second support cone
                    CreateSubSupportConeWithinRange(interlinkedSupportConesWithinRanges.First(), supportProfile, 45);
                }
                else if (t.Count == 0)
                {
                    CreateSubSupportConeWithinRange(supportProfile, 45);
                    CreateSubSupportConeWithinRange(supportProfile, -45);
                    CreateSubSupportConeWithinRange(supportProfile, 135);
                    CreateSubSupportConeWithinRange(supportProfile, -135);
                }
            }

            //update opengl bindings using triangle array 1 index
            t.Clear();
            foreach (var v in this.InterlinkConnections.Values)
            {
                t.AddRange(v);
            }

            foreach (var interlinkConnection in t)
            {
                if (interlinkConnection != null)
                {
                    this.Triangles[0].AddRange(interlinkConnection.Triangles[0]);
                }
            }


            this.UpdateBinding();
        }

        private SupportConeV2 CreateSubSupportConeWithinRange(SupportProfile supportProfile, int rotationAngle)
        {
            var rotationMatrix = Matrix4.CreateRotationZ(OpenTK.MathHelper.DegreesToRadians(rotationAngle));
            var rotatedVector = Vector3Class.Transform(new Vector3Class(supportProfile.SupportInfillDistance, 0, 0), rotationMatrix);
            var newSupportPointVector = rotatedVector - this.RefPointCSegment;
            newSupportPointVector.Z = 0;
            newSupportPointVector.Normalize();
            rotatedVector = Vector3Class.Transform(newSupportPointVector, rotationMatrix);
            rotatedVector.Z = -1;
            rotatedVector.Normalize();

            if (this.Model.SupportHelperStructure == null)
            {
                this.Model.SupportHelperStructure = new List<SupportCone>();
            }

            var subSupportCone = new SupportConeV2(this.TopHeight, this.TopRadius, this.MiddleRadius, this.BottomHeight, this.BottomRadius, 26, new TriangleIntersection(new Triangle() { Normal = rotatedVector }, this.RefPointCSegment), Color.AntiqueWhite,
                aSegmentRadius: this.ASegmentRadius,
                aSegmentHeight: this.ASegmentHeight,
                bSegmentRadius: this.BSegmentRadius,
                bSegmentHeight: this.BSegmentHeight + (4 * this.MiddleRadius),
                cSegmentRadius: this.CSegmentRadius,
                dSegmentRadius: this.DSegmentRadius,
                dSegmentHeight: this.DSegmentHeight
                );

            //add support interlinks
            this.UpdateInterlinkConnections(subSupportCone, supportProfile);

            if (this.Model.SupportHelperStructure == null)
            {
                this.Model.SupportHelperStructure = new List<SupportCone>();
            }
            this.Model.SupportHelperStructure.Add(subSupportCone);
            
            this.UpdateBinding();

            return subSupportCone;
        }

        private SupportConeV2 CreateSubSupportConeWithinRange(SupportConeV2 interlinkedSupportCone, SupportProfile supportProfile, int rotationAngle)
        {
            var rotationMatrix = Matrix4.CreateRotationZ(rotationAngle);
            var newSupportPointVector = interlinkedSupportCone.RefPointCSegment - this.RefPointCSegment;
            newSupportPointVector.Z = 0;
            newSupportPointVector.Normalize();
            var rotatedVector = Vector3Class.Transform(newSupportPointVector, rotationMatrix);
            rotatedVector.Z = -1;
            rotatedVector.Normalize();

            if (this.Model.SupportHelperStructure == null)
            {
                this.Model.SupportHelperStructure = new List<SupportCone>();
            }

            var subSupportCone = new SupportConeV2(this.TopHeight, this.TopRadius, this.MiddleRadius, this.BottomHeight, this.BottomRadius, 26, new TriangleIntersection(new Triangle() { Normal = rotatedVector }, this.RefPointCSegment), Color.AntiqueWhite,
                aSegmentRadius: this.ASegmentRadius,
                aSegmentHeight: this.ASegmentHeight,
                bSegmentRadius: this.BSegmentRadius,
                bSegmentHeight: this.BSegmentHeight + (4 * this.MiddleRadius),
                cSegmentRadius: this.CSegmentRadius,
                dSegmentRadius: this.DSegmentRadius,
                dSegmentHeight: this.DSegmentHeight
                );

            //add support interlinks
            this.UpdateInterlinkConnections(subSupportCone, supportProfile);

            if (this.Model.SupportHelperStructure == null)
            {
                this.Model.SupportHelperStructure = new List<SupportCone>();
            }
            this.Model.SupportHelperStructure.Add(subSupportCone);
            
            this.UpdateBinding();

            return subSupportCone;
            //
        }

        private bool IsSupportConeWithinXYAngleRange(SupportConeV2 supportCone1, SupportConeV2 supportCone2, float angleMin, float angleMax)
        {
            var xyAngleBetweenSupportCones = VectorHelper.CalcAngleBetweenVectorPoints(supportCone1.RefPointCSegment.Xy, this.RefPointCSegment.Xy, supportCone2.RefPointCSegment.Xy);
            Console.WriteLine(xyAngleBetweenSupportCones);

            if (xyAngleBetweenSupportCones < angleMin || xyAngleBetweenSupportCones > angleMax)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
