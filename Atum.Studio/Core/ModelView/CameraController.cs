using System;
using OpenTK;
using System.Diagnostics;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Core.ModelView
{
    public class OrbitCameraController
    {
        private Matrix4 _view;
        private Matrix4 _viewWithOffset;
        private float _cameraDistance;
        private float _pitchAngle = 0.0f;
        private float _rollAngle = 0.0f;
        private readonly Vector3 _right;
        private readonly Vector3 _up;
        private readonly Vector3 _front;

        private Vector3 _panVector;

        private const float ZoomSpeed = 1.00105f;
        private const float MinimumCameraDistance = 0.1f;

        /// <summary>
        /// Rotation speed, in degrees per pixels
        /// </summary>
        private const float RotationSpeed = 1f;
        private const float PanSpeed = 0.2f;
        private const float InitialCameraDistance = 400.0f;

        private Vector3Class _targetCenterPoint = new Vector3Class();


        public OrbitCameraController(Vector3Class targetCenter)
        {
            _view = Matrix4.CreateFromAxisAngle(new Vector3(0f, 0f, 1f), 0.0001f);
            _viewWithOffset = Matrix4.Identity;
            _cameraDistance = InitialCameraDistance;
            _targetCenterPoint = targetCenter;

            _right = Vector3.UnitX;
            _up = Vector3.UnitZ;
            _front = Vector3.UnitY;
        }


        public void SetTargetCenterPoint(Vector3Class targetCenterPoint)
        {
            _targetCenterPoint = targetCenterPoint;
        }

        public Matrix4 GetCameraView()
        {
            UpdateViewMatrix();


            return _viewWithOffset;
        }

        public Matrix4 GetCameraViewWithoutTranslation()
        {
            var viewWithPitchAndRoll = _view * Matrix4.CreateFromAxisAngle(_right, _pitchAngle) * Matrix4.CreateFromAxisAngle(_front, _rollAngle);
            viewWithPitchAndRoll = Matrix4.LookAt(viewWithPitchAndRoll.Column2.Xyz * _cameraDistance, new Vector3(), viewWithPitchAndRoll.Column1.Xyz);

            return viewWithPitchAndRoll;
        }

        public Vector3Class GetCameraViewWithoutTranslationAsVector3()
        {
            var viewWithPitchAndRoll = _view * Matrix4.CreateFromAxisAngle(_right, _pitchAngle) * Matrix4.CreateFromAxisAngle(_front, _rollAngle);
            viewWithPitchAndRoll = Matrix4.LookAt(viewWithPitchAndRoll.Column2.Xyz * _cameraDistance, new Vector3(), viewWithPitchAndRoll.Column1.Xyz);
            return new Vector3Class(Vector3.Multiply(viewWithPitchAndRoll.Column2.Xyz, _cameraDistance));
        }

        public Vector3Class GetCameraPosition()
        {
            //var cameraPosition = new Vector3(_viewWithOffset.Column2.Xyz.X, _viewWithOffset.Column2.Xyz.Y, _viewWithOffset.Column2.Xyz.Z);
            var viewWithPitchAndRoll = _view * Matrix4.CreateFromAxisAngle(_right, _pitchAngle) * Matrix4.CreateFromAxisAngle(_front, _rollAngle);
            viewWithPitchAndRoll = Matrix4.LookAt(viewWithPitchAndRoll.Column2.Xyz * _cameraDistance + _targetCenterPoint.ToStruct(), _targetCenterPoint.ToStruct(), viewWithPitchAndRoll.Column1.Xyz);
            return new Vector3Class(Vector3.Multiply(viewWithPitchAndRoll.Column2.Xyz, _cameraDistance));
        }

        public Vector3Class GetLightPosition()
        {
            var lightPosition = new Vector3(_view.Column2.Xyz.X, _view.Column2.Xyz.Y, _view.Column2.Xyz.Z);
            return new Vector3Class(Vector3.Multiply(lightPosition, _cameraDistance));
        }


        public void MouseMove(float x, float y)
        {
            if (x == 0 && y == 0)
            {
                return;
            }

            if (x != 0)
            {
                _view *= Matrix4.CreateFromAxisAngle(_up, (float)(x * RotationSpeed * Math.PI / 180.0));
            }

            if (y != 0)
            {
                _view *= Matrix4.CreateFromAxisAngle(_right, (float)(y * RotationSpeed * Math.PI / 180.0));
            }
        }

        internal void Scroll(float cameraDistance)
        {
            _cameraDistance = Math.Max(cameraDistance, MinimumCameraDistance);
        }

        public void Pan(float x, float y)
        {
            _panVector.X += x * PanSpeed;
            _panVector.Y += -y * PanSpeed;
        }

        private void UpdateViewMatrix()
        {
            var viewWithPitchAndRoll = _view * Matrix4.CreateFromAxisAngle(_right, _pitchAngle) * Matrix4.CreateFromAxisAngle(_front, _rollAngle);
            _viewWithOffset = Matrix4.LookAt(viewWithPitchAndRoll.Column2.Xyz * _cameraDistance + _targetCenterPoint.ToStruct(), _targetCenterPoint.ToStruct(), viewWithPitchAndRoll.Column1.Xyz);
            _viewWithOffset *= Matrix4.CreateTranslation(_panVector);
        }

    }

}
