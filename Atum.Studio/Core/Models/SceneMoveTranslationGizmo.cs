using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;
using static Atum.Studio.Core.Enums;

namespace Atum.Studio.Core.Models
{
    class SceneMoveTranslationGizmo : STLModel3D
    {
        private float AxisConeHeight = 6;
        private float AxisConeTopHeight = 3f;
        private float AxisConeTopRadius = 1.2f;
        private float AxisConeMiddleRadius = 0.75f;

        internal STLModel3D SelectedObject { get; private set; }
        internal STLModel3D PreviousSelectedObject { get; private set; }

        public SceneViewSelectedMoveTranslationAxisType SelectedMoveTranslationAxis { get; set; }

        public SceneMoveTranslationGizmo() : base(TypeObject.XYZ, true)
        {
            if (Managers.UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode)
            {
                AxisConeMiddleRadius *= 2.5f;
                AxisConeTopRadius *= 2.5f;
                AxisConeTopHeight *= 2.5f;
            }

            this.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
        }

        public void UpdateControl(SceneViewSelectedMoveTranslationAxisType selectedMoveTranslationAxis, bool selectedModelChanged)
        {
            if (selectedMoveTranslationAxis != SceneViewSelectedMoveTranslationAxisType.Hidden)
            {
                if (selectedMoveTranslationAxis != SelectedMoveTranslationAxis || this.Triangles == null || this.Triangles.Count == 0 || selectedModelChanged || this.SelectedObject.LinkedClones.Count > 1)
                {
                    this.SelectedMoveTranslationAxis = selectedMoveTranslationAxis;
                    this.SelectedObject = (STLModel3D)ObjectView.SelectedModel;

                    if (SelectedObject != null)
                    {
                        if (SceneView.CurrentViewMode == SceneView.ViewMode.ModelRotation && this.SelectedObject.LinkedClones.Count > 1)
                        {
                            this.Hidden = true;
                            SceneControlToolbarManager.ShowRotationSelectedModelContainsLinkedClonesToolTip();
                        }
                        else
                        {
                            LinkedClone selectedLinkedClone = null;
                            for (var i = 0; i < SelectedObject.LinkedClones.Count; i++)
                            {
                                if (SelectedObject.LinkedClones[i].Selected)
                                {
                                    selectedLinkedClone = SelectedObject.LinkedClones[i];
                                    break;
                                }
                            }

                            var stlModel = SelectedObject;
                            var moveTranslationAxisXColor = new Byte4Class(240, 255, 0, 0);
                            if (this.SelectedMoveTranslationAxis == SceneViewSelectedMoveTranslationAxisType.X)
                            {
                                moveTranslationAxisXColor = new Byte4Class(240, 255, 255, 255);
                            }
                            var centerLeftSide = new Vector3((stlModel.LeftPoint - stlModel.RightPoint) / 2, stlModel.Center.Y, 0);
                            var leftSideCone = new SceneMoveTranslationGizmoCone(AxisConeHeight - centerLeftSide.X, AxisConeTopHeight, AxisConeTopRadius, AxisConeMiddleRadius, 10, new Vector3Class(), Color.Red);
                            leftSideCone.Rotate(0, -90, 0, Events.RotationEventArgs.TypeAxis.Y, true);
                            foreach (var triangle in leftSideCone.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = moveTranslationAxisXColor;
                            }

                            var centerRightSide = new Vector3((stlModel.RightPoint - stlModel.LeftPoint) / 2, stlModel.Center.Y, 0);
                            var rightSideCone = new SceneMoveTranslationGizmoCone(AxisConeHeight + centerRightSide.X, AxisConeTopHeight, AxisConeTopRadius, AxisConeMiddleRadius, 10, new Vector3Class(), Color.Red);
                            rightSideCone.Rotate(0, 90, 0, Events.RotationEventArgs.TypeAxis.Y, true);
                            foreach (var triangle in rightSideCone.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = moveTranslationAxisXColor;
                            }

                            //front back
                            var moveTranslationAxisYColor = new Byte4Class(241, 0, 255, 0);
                            if (this.SelectedMoveTranslationAxis == SceneViewSelectedMoveTranslationAxisType.Y)
                            {
                                moveTranslationAxisYColor = new Byte4Class(241, 255, 255, 255);
                            }
                            var centerFrontSide = new Vector3(stlModel.Center.X, (stlModel.FrontPoint - stlModel.BackPoint) / 2, 0);
                            var frontSideCone = new SceneMoveTranslationGizmoCone(AxisConeHeight + centerFrontSide.Y, AxisConeTopHeight, AxisConeTopRadius, AxisConeMiddleRadius, 10, new Vector3Class(), Color.Green);
                            frontSideCone.Rotate(90, 0, 0, Events.RotationEventArgs.TypeAxis.X, true);
                            foreach (var triangle in frontSideCone.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = moveTranslationAxisYColor;
                            }

                            var centerBackSide = new Vector3(stlModel.Center.X, (stlModel.FrontPoint - stlModel.BackPoint) / 2, 0);
                            var backSideCone = new SceneMoveTranslationGizmoCone(AxisConeHeight + centerBackSide.Y, AxisConeTopHeight, AxisConeTopRadius, AxisConeMiddleRadius, 10, new Vector3Class(), Color.Green);
                            backSideCone.Rotate(-90, 0, 0, Events.RotationEventArgs.TypeAxis.X, true);
                            foreach (var triangle in backSideCone.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = moveTranslationAxisYColor;
                            }

                            //top bottom
                            var moveTranslationAxisZColor = new Byte4Class(242, 0, 0, 255);
                            if (this.SelectedMoveTranslationAxis == SceneViewSelectedMoveTranslationAxisType.Z)
                            {
                                moveTranslationAxisZColor = new Byte4Class(242, 255, 255, 255);
                            }
                            var topSideCone = new SceneMoveTranslationGizmoCone(AxisConeHeight + ((stlModel.TopPoint - stlModel.BottomPoint) / 2), AxisConeTopHeight, AxisConeTopRadius, AxisConeMiddleRadius, 10, new Vector3Class(), Color.Blue);
                            foreach (var triangle in topSideCone.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = moveTranslationAxisZColor;
                            }

                            var bottomSideCone = new SceneMoveTranslationGizmoCone(AxisConeHeight + ((stlModel.TopPoint - stlModel.BottomPoint) / 2), AxisConeTopHeight, AxisConeTopRadius, AxisConeMiddleRadius, 10, new Vector3Class(), Color.Blue);
                            bottomSideCone.Rotate(180, 0, 0, Events.RotationEventArgs.TypeAxis.X, true);
                            foreach (var triangle in bottomSideCone.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = moveTranslationAxisZColor;
                            }

                            //initialize triangle array
                            this.Triangles = new TriangleInfoList();
                            this.Triangles[0].AddRange(leftSideCone.Triangles[0]);
                            this.Triangles[0].AddRange(rightSideCone.Triangles[0]);
                            this.Triangles[0].AddRange(frontSideCone.Triangles[0]);
                            this.Triangles[0].AddRange(backSideCone.Triangles[0]);

                            if (SelectedObject != null && SelectedObject is SupportCone)
                            {
                                var selectedSupportCone = SelectedObject as SupportCone;
                                var moveTranslation = new Vector3Class(selectedSupportCone.Center.X, selectedSupportCone.Center.Y, 0);
                                foreach (var triangle in this.Triangles[0])
                                {
                                    triangle.Vectors[0].Position += moveTranslation;
                                    triangle.Vectors[1].Position += moveTranslation;
                                    triangle.Vectors[2].Position += moveTranslation;
                                }

                                this.MoveTranslation = selectedSupportCone.MoveTranslation + new Vector3Class(selectedSupportCone.Model.MoveTranslation.X, selectedSupportCone.Model.MoveTranslation.Y, ((selectedSupportCone.TopPoint - selectedSupportCone.BottomPoint) / 2) + selectedSupportCone.BottomPoint);
                            }
                            else if (selectedLinkedClone != null)
                            {
                                this.Triangles[0].AddRange(topSideCone.Triangles[0]);
                                this.Triangles[0].AddRange(bottomSideCone.Triangles[0]);

                                this.MoveTranslation = selectedLinkedClone.Translation + new Vector3Class(0, 0, ((stlModel.TopPoint - stlModel.BottomPoint) / 2) + stlModel.BottomPoint);
                            }
                            else
                            {
                                this.Triangles[0].AddRange(topSideCone.Triangles[0]);
                                this.Triangles[0].AddRange(bottomSideCone.Triangles[0]);

                                //change all triangles center to center of model
                                if (stlModel.PreviewMoveTranslation.X != 0 || stlModel.PreviewMoveTranslation.Y != 0)
                                {
                                    this.MoveTranslation = stlModel.PreviewMoveTranslation + new Vector3Class(0, 0, (stlModel.TopPoint - stlModel.BottomPoint) / 2);
                                }
                                else
                                {
                                    this.MoveTranslation = stlModel.MoveTranslation + new Vector3Class(0, 0, (stlModel.TopPoint - stlModel.BottomPoint) / 2);
                                }
                            }

                            this.PreviousSelectedObject = SelectedObject;
                        }
                    }
                }
            }
            else
            {
                this.Triangles = new TriangleInfoList();
            }


            if (this.VBOIndexes == null && ObjectView.SelectedModel != null)
            {
                this.BindModel();
            }
            else
            {
                this.UpdateBinding();
            }

        }

        internal void MarkSelectedAxis(int X, int Y, int controlWidth, int controlHeight, Point pointToClient)
        {
            int w = controlWidth;
            int h = controlHeight;
            float aspect = ((float)controlWidth) / ((float)controlHeight);

            int window_y = (h - Y) - h / 2;
            double norm_y = (double)(window_y) / (double)(h / 2);
            int window_x = X - w / 2;
            double norm_x = (double)(window_x) / (double)(w / 2);
            float near_height = .2825f; // no detectable error

            float y = (float)(near_height * norm_y);
            float x = (float)(near_height * aspect * norm_x);
            Vector4 ray_pnt = new Vector4(0.0f, 0.0f, 0.0f, 1.0f);
            Vector4 ray_vec = new Vector4((float)x, (float)y, -1f, 0);
            ray_vec.Normalize();

            try
            {
                Matrix4 modelViewMatrix;
                GL.GetFloat(GetPName.ModelviewMatrix, out modelViewMatrix);
                Matrix4 viewInv = Matrix4.Invert(modelViewMatrix);

                //color selection
                var pixel = new Byte4();
                Point pt = pointToClient;
                GL.ReadPixels(pt.X, controlHeight - pt.Y, 1, 1, PixelFormat.Rgba, PixelType.UnsignedByte, ref pixel);
                switch (pixel.A)
                {
                    case 240:
                        this.UpdateControl(SceneViewSelectedMoveTranslationAxisType.X, false);
                        break;
                    case 241:
                        this.UpdateControl(SceneViewSelectedMoveTranslationAxisType.Y, false);
                        break;
                    case 242:
                        this.UpdateControl(SceneViewSelectedMoveTranslationAxisType.Z, false);
                        break;
                    default:
                        this.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, false);
                        break;
                }
            }
            catch (Exception exc)
            {

            }
        }
    }
}
