using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Helpers;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Models
{
    class SceneRotationGizmoAngle : STLModel3D
    {
        public SceneRotationGizmoAngle() : base(TypeObject.None, true)
        {

        }

        internal void UpdatAngle(float innerRadius, float outerRadius, int torusSegmentCount, int torusSegmentBandCount, float angle, RotationEventArgs.TypeAxis rotationAxis)
        {
                //var selectionTorusOverlay = new Torus(innerRadius, outerRadius, torusSegmentCount, torusSegmentBandCount, angle);
                var selectionTorusOverlayColor = new Byte4Class(128, Properties.Settings.Default.RotationGizmoAngleSelectionOverlay.R, Properties.Settings.Default.RotationGizmoAngleSelectionOverlay.G, Properties.Settings.Default.RotationGizmoAngleSelectionOverlay.B);
                //foreach (var triangle in selectionTorusOverlay.Triangles[0])
                //{
                //    triangle.Vectors[0].Color = selectionTorusOverlayColor;
                //    triangle.Vectors[1].Color = selectionTorusOverlayColor;
                //    triangle.Vectors[2].Color = selectionTorusOverlayColor;
                //}

                //if (angle < 0)
                //{
                //    foreach (var triangle in selectionTorusOverlay.Triangles[0])
                //    {
                //        triangle.Flip();
                //    }
                //}

                //prepare angle infill

                var rotationAngleOutlinePoints = VectorHelper.GetCircleOutlinePoints(0, innerRadius, torusSegmentCount, new Vector3Class(), angle);
                //convert outlinepoints to triangles
                var rotationAngleModel = new STLModel3D();
                var rotationAngleModelMirror = new STLModel3D();
                rotationAngleModel.Triangles = new TriangleInfoList();
                rotationAngleModelMirror.Triangles = new TriangleInfoList();
                for (var rotationAngleOutlinePointIndex = 0; rotationAngleOutlinePointIndex < rotationAngleOutlinePoints.Count - 1; rotationAngleOutlinePointIndex++)
                {
                    var triangle = new Triangle();
                    triangle.Vectors[0].Position = new Vector3Class();
                    triangle.Vectors[0].Color = selectionTorusOverlayColor;
                    triangle.Vectors[1].Color = selectionTorusOverlayColor;
                    triangle.Vectors[1].Position = rotationAngleOutlinePoints[rotationAngleOutlinePointIndex];

                    triangle.Vectors[2].Color = selectionTorusOverlayColor;
                    if (rotationAngleOutlinePointIndex == rotationAngleOutlinePoints.Count - 1)
                    {
                        triangle.Vectors[2].Position = rotationAngleOutlinePoints[0];
                    }
                    else
                    {
                        triangle.Vectors[2].Position = rotationAngleOutlinePoints[rotationAngleOutlinePointIndex + 1];
                    }

                    triangle.CalcNormal();
                    rotationAngleModel.Triangles[0].Add(triangle);
                    var flipClone = (Triangle)triangle.Clone();
                    flipClone.Flip();
                    rotationAngleModelMirror.Triangles[0].Add(flipClone);
                }

                //switch (rotationAxis)
                //{
                //    case RotationEventArgs.TypeAxis.X:
                //        selectionTorusOverlay.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                //        selectionTorusOverlay.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);
                //        break;
                //    case RotationEventArgs.TypeAxis.Y:
                //        selectionTorusOverlay.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                //        break;
                //}


                switch (rotationAxis)
                {
                    case RotationEventArgs.TypeAxis.X:
              //          selectionTorusOverlay.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
              //          selectionTorusOverlay.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);

                        rotationAngleModel.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                        rotationAngleModel.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);

                        rotationAngleModelMirror.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                        rotationAngleModelMirror.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);
                        break;
                    case RotationEventArgs.TypeAxis.Y:
               //         selectionTorusOverlay.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                        rotationAngleModel.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                        rotationAngleModelMirror.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                        break;
                }

            //    this.Triangles = selectionTorusOverlay.Triangles;
            //remove last torus triangles
            this.Triangles = new TriangleInfoList();
                //this.Triangles[0].RemoveRange(0, torusSegmentBandCount * 2);
                this.Triangles[0].AddRange(rotationAngleModel.Triangles[0]);
                this.Triangles[0].AddRange(rotationAngleModelMirror.Triangles[0]);




                ////add end tag
                //var startTag = new Box(outerRadius * 4, 0.05f, outerRadius * 2, true);
                //var endTag = new Box(outerRadius * 4, 0.05f, outerRadius * 2, true);

                //var selectionTagColor = new Byte4Class(255,255,255,255);
                //var startPosition = this.Triangles[0][0].Points[2];
                //var endPosition = this.Triangles[0][3560].Points[0];

                //switch (rotationAxis)
                //{
                //    case RotationEventArgs.TypeAxis.X:
                //        endTag.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                //        endTag.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);
                //        endTag.UpdateDefaultCenter();
                //        endTag.Rotate(angle + 90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);

                //        startTag.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                //        startTag.UpdateDefaultCenter();
                //        startTag.Rotate(0, 0, 90, RotationEventArgs.TypeAxis.Z, false, false);

                //        endPosition.X -= (outerRadius);
                //        startPosition.X = endPosition.X;
                //        break;
                //    case RotationEventArgs.TypeAxis.Y:
                //        endTag.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                //        endTag.Rotate(0, -angle, 0, RotationEventArgs.TypeAxis.Y, false, false);

                //        startPosition.X -= outerRadius;
                //        endPosition.Y += (2 * outerRadius);
                //        startPosition.Y = endPosition.Y;

                //        startTag.Rotate(90, 0, 0, RotationEventArgs.TypeAxis.X, false, false);
                //        break;
                //    case RotationEventArgs.TypeAxis.Z:
                //        startPosition.Z = -endTag.Height / 2;
                //        foreach (var triangle in endTag.Triangles[0])
                //        {
                //            triangle.Vectors[0].Position -= new Vector3Class(0, 0, endTag.Height);
                //            triangle.Vectors[1].Position -= new Vector3Class(0, 0, endTag.Height);
                //            triangle.Vectors[2].Position -= new Vector3Class(0, 0, endTag.Height);
                //        }

                //        endTag.Rotate(0, 0, angle, RotationEventArgs.TypeAxis.Z, false, false);

                //        break;
                //}

                //foreach (var triangle in endTag.Triangles[0])
                //{
                //    triangle.Vectors[0].Position += endPosition;
                //    triangle.Vectors[0].Color = selectionTagColor;
                //    triangle.Vectors[1].Position += endPosition;
                //    triangle.Vectors[1].Color = selectionTagColor;
                //    triangle.Vectors[2].Position += endPosition;
                //    triangle.Vectors[2].Color = selectionTagColor;
                //}

                //this.Triangles[0].AddRange(endTag.Triangles[0]);

                ////add start tag
                //foreach (var triangle in startTag.Triangles[0])
                //{
                //    triangle.Vectors[0].Position += startPosition;
                //    triangle.Vectors[0].Color = selectionTagColor;
                //    triangle.Vectors[1].Position += startPosition;
                //    triangle.Vectors[1].Color = selectionTagColor;
                //    triangle.Vectors[2].Position += startPosition;
                //    triangle.Vectors[2].Color = selectionTagColor;
                //}

                //this.Triangles[0].AddRange(startTag.Triangles[0]);



                //////textline
                ////var textLineVector = new Box(10f, 0.05f, 0.05f, false);
                //////textLineVector.Rotate(0, 0, angle - 30, Events.RotationEventArgs.TypeAxis.Z, true, false);
                ////var textLineOffsetVector = new Vector3(endTag.Triangles[0][1].Points[2].X, endTag.Triangles[0][1].Points[2].Y, 0);
                ////textLineOffsetVector.Scale(new Vector3(1.1f));
                ////foreach (var triangle in textLineVector.Triangles[0])
                ////{
                ////    triangle.Vectors[0].Position += new Vector3(textLineOffsetVector.X, textLineOffsetVector.Y, -outerRadius);
                ////    triangle.Vectors[0].Color = selectionTagColor;
                ////    triangle.Vectors[1].Position += new Vector3(textLineOffsetVector.X, textLineOffsetVector.Y, -outerRadius);
                ////    triangle.Vectors[1].Color = selectionTagColor;
                ////    triangle.Vectors[2].Position += new Vector3(textLineOffsetVector.X, textLineOffsetVector.Y, -outerRadius);
                ////    triangle.Vectors[2].Color = selectionTagColor;
                ////}

                ////this.Triangles[0].AddRange(textLineVector.Triangles[0]);

                //////angle text
                ////var fontSize = 5.7f;
                ////var textAsTriangles = FontTessellationEngine.ConvertStringToTriangles(angle.ToString(), FontStyle.Regular, fontSize);
                ////var textColor = new Byte4(new[] { (byte)0, (byte)Properties.Settings.Default.SceneControlFontColor.R, (byte)Properties.Settings.Default.SceneControlFontColor.G, (byte)Properties.Settings.Default.SceneControlFontColor.B });
                ////foreach (var textTriangle in textAsTriangles[0])
                ////{
                ////    textTriangle.Vectors[0].Position += new Vector3(textLineOffsetVector.X, textLineOffsetVector.Y, -outerRadius);
                ////    textTriangle.Vectors[0].Color = selectionTagColor;
                ////    textTriangle.Vectors[1].Position += new Vector3(textLineOffsetVector.X, textLineOffsetVector.Y, -outerRadius);
                ////    textTriangle.Vectors[1].Color = selectionTagColor;
                ////    textTriangle.Vectors[2].Position += new Vector3(textLineOffsetVector.X, textLineOffsetVector.Y, -outerRadius);
                ////    textTriangle.Vectors[2].Color = selectionTagColor;

                ////}

                ////this.Triangles[0].AddRange(textAsTriangles[0]);

                if (this.VBOIndexes == null)
                {
                    this.BindModel();
                }
                else
                {
                    this.UpdateBinding();
                }
            }
        }

}
