using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Core.Structs;
using OpenTK.Graphics.OpenGL;
using OpenTK;
using System;
using System.Drawing;
using static Atum.Studio.Core.Enums;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Core.Models
{
    class SceneRotationGizmo : STLModel3D
    {

        public SceneViewSelectedRotationAxisType SelectedRotationAxis { get; set; }
        public SceneRotationGizmoAngle RotationOverlay { get; set; }

        private const int TORUSSEGMENTCOUNT = 300;
        private const int TORUSSEGMENTBANDCOUNT = 50;
        private float TORUSOUTSIDERINGDIAMETER = 0.625f;

        private float _modelDiameter = 0f;
        private int _selectedModelIndex = 0;

        internal STLModel3D SelectedObject { get; private set; }
        internal STLModel3D PreviousSelectedObject { get; private set; }

        public SceneRotationGizmo() : base(TypeObject.XYZ, true)
        {
            if (Managers.UserProfileManager.UserProfile.Settings_Enable_Touch_Interface_Mode)
            {
                TORUSOUTSIDERINGDIAMETER *= 2.5f;
            }

            this.UpdateControl(SceneViewSelectedRotationAxisType.None);

            this.RotationOverlay = new SceneRotationGizmoAngle();
        }

        public void UpdateSelectedAngleOverlay(STLModel3D selectedModel, RotationEventArgs.TypeAxis rotationAxis)
        {
            if (rotationAxis == RotationEventArgs.TypeAxis.None)
            {
                this.RotationOverlay = new SceneRotationGizmoAngle();
            }
            else if (selectedModel.PreviewRotationX != 0)
            {
                RotationOverlay.UpdatAngle(_modelDiameter, TORUSOUTSIDERINGDIAMETER * 2, TORUSSEGMENTCOUNT, TORUSSEGMENTBANDCOUNT, selectedModel.PreviewRotationX, rotationAxis);
            }
            else if (selectedModel.PreviewRotationY != 0)
            {
                RotationOverlay.UpdatAngle(_modelDiameter, TORUSOUTSIDERINGDIAMETER * 2, TORUSSEGMENTCOUNT, TORUSSEGMENTBANDCOUNT, -selectedModel.PreviewRotationY, rotationAxis);
            }
            else if (selectedModel.PreviewRotationZ != 0)
            {
                RotationOverlay.UpdatAngle(_modelDiameter, TORUSOUTSIDERINGDIAMETER * 2, TORUSSEGMENTCOUNT, TORUSSEGMENTBANDCOUNT, selectedModel.PreviewRotationZ, rotationAxis);
            }
        }

        public void UpdateControl(SceneViewSelectedRotationAxisType selectedRotationAxis)
        {
            this.SelectedObject = (STLModel3D)ObjectView.SelectedModel;
            if (SelectedObject != null)
            {
                if (this.SelectedObject.LinkedClones.Count > 1 && SceneView.CurrentViewMode== SceneView.ViewMode.ModelRotation)
                {
                    this.Hidden = true;
                    SceneControlToolbarManager.ShowRotationSelectedModelContainsLinkedClonesToolTip();
                }
                else
                {
                    this.Hidden = false;
                    if (selectedRotationAxis != SceneViewSelectedRotationAxisType.Hidden)
                    {
                        this.SelectedRotationAxis = selectedRotationAxis;

                        if (this.SelectedObject != null && !(this.SelectedObject is SupportCone))
                        {
                            //diameter equals front bottom --> back top distance
                            _modelDiameter = (new Vector3(this.SelectedObject.LeftPoint, this.SelectedObject.FrontPoint, 0) - (new Vector3(this.RightPoint, this.BackPoint, this.TopPoint))).Length;
                            _selectedModelIndex = this.SelectedObject.Index;

                            //initialize triangle array
                            this.Triangles = new TriangleInfoList();

                            //create axis X
                            var rotationAxisXColor = new Byte4Class(240, 255, 0, 0);
                            if (this.SelectedRotationAxis == SceneViewSelectedRotationAxisType.X)
                            {
                                rotationAxisXColor = new Byte4Class(240, 255, 255, 255);
                            }
                            var rotationAxisX = new Torus(_modelDiameter, TORUSOUTSIDERINGDIAMETER, TORUSSEGMENTCOUNT, TORUSSEGMENTBANDCOUNT);
                            foreach (var triangle in rotationAxisX.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = rotationAxisXColor;
                            }


                            //add triangle to stl model
                            rotationAxisX.Rotate(0, 45, 0, Events.RotationEventArgs.TypeAxis.Y, true, false);
                            this.Triangles[0].AddRange(rotationAxisX.Triangles[0]);

                            //create axis Y
                            var rotationAxisY = new Torus(_modelDiameter, TORUSOUTSIDERINGDIAMETER, TORUSSEGMENTCOUNT, TORUSSEGMENTBANDCOUNT);
                            var rotationAxisYColor = new Byte4Class(241, 0, 255, 0);
                            if (this.SelectedRotationAxis == SceneViewSelectedRotationAxisType.Y)
                            {
                                rotationAxisYColor = new Byte4Class(241, 255, 255, 255);
                            }

                            foreach (var triangle in rotationAxisY.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = rotationAxisYColor;
                            }

                            //add triangle to stl model
                            rotationAxisY.Rotate(45, 0, 0, Events.RotationEventArgs.TypeAxis.X, true, false);
                            this.Triangles[0].AddRange(rotationAxisY.Triangles[0]);

                            //create z rotation axis
                            var rotationAxisZ = new Torus(_modelDiameter, TORUSOUTSIDERINGDIAMETER, TORUSSEGMENTCOUNT, TORUSSEGMENTBANDCOUNT);
                            var rotationAxisZColor = new Byte4Class(242, 0, 0, 255);
                            if (this.SelectedRotationAxis == SceneViewSelectedRotationAxisType.Z)
                            {
                                rotationAxisZColor = new Byte4Class(242, 255, 255, 255);
                            }
                            foreach (var triangle in rotationAxisZ.Triangles[0])
                            {
                                triangle.Vectors[0].Color = triangle.Vectors[1].Color = triangle.Vectors[2].Color = rotationAxisZColor;
                            }

                            this.Triangles[0].AddRange(rotationAxisZ.Triangles[0]);

                            foreach (var triangle in this.Triangles[0])
                            {
                                triangle.CalcNormal();
                            }

                            //change all triangles center to center of model
                            this.MoveTranslation = this.SelectedObject.MoveTranslation;
                            this.MoveTranslation += new Vector3Class(0, 0, (this.SelectedObject.TopPoint - this.SelectedObject.BottomPoint) / 2);

                        }
                    }
                    else
                    {
                        //reset bindings
                        this.Triangles = new TriangleInfoList();
                    }

                    if (this.SelectedRotationAxis == SceneViewSelectedRotationAxisType.None)
                    {
                        UpdateSelectedAngleOverlay(this.SelectedObject, RotationEventArgs.TypeAxis.None);
                    }

                    if (this.VBOIndexes == null)
                    {
                        this.BindModel();
                    }
                    else
                    {
                        this.UpdateBinding();
                    }
                }

                this.PreviousSelectedObject = this.SelectedObject;
            }

        }

        internal void MarkSelectedAxis(int X, int Y, int controlWidth, int controlHeight, Point pointToClient)
        {
            try
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
                        SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.X);
                        break;
                    case 241:
                        SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.Y);
                        break;
                    case 242:
                        SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.Z);
                        break;
                }
            }
            catch (Exception exc)
            {

            }
        }
    }
}
