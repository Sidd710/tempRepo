using Atum.Studio.Core.Events;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    internal static class UndoRedoManager
    {
        internal enum typeOfAction
        {
            STLModelMoveTranslation,
            STLModelScaling,
            STLModelRotatingX,
            STLModelRotatingY,
            STLModelRotatingZ,

            SupportConeMoveTranslation,
        }

        internal static Stack<IUndoRedo> _undoActions = new Stack<IUndoRedo>();
        internal static Stack<IUndoRedo> _redoActions = new Stack<IUndoRedo>();

        internal static void Undo()
        {
            if (_undoActions.Count > 0)
            {
                var undoAction = _undoActions.Pop();

                switch (undoAction.ActionType)
                {
                    case typeOfAction.STLModelMoveTranslation:
                        var stlModel = undoAction.Item as STLModel3D;
                        stlModel.MoveTranslation -= (Vector3)undoAction.Values;

                        foreach (var supportCone in stlModel.SupportStructure)
                        {
                            supportCone.MoveTranslation -= (Vector3)undoAction.Values;
                        }

                        stlModel.LiftModelOnSupport();
                        break;
                    case typeOfAction.STLModelRotatingX: 
                        var stlModelRotationX = undoAction.Item as STLModel3D;
                        stlModelRotationX.Rotate((float)undoAction.Values, stlModelRotationX.RotationAngleY, stlModelRotationX.RotationAngleZ, Events.RotationEventArgs.TypeAxis.X);

                        if (ObjectView.BindingSupported)
                        {
                            if (frmStudioMain.GLCONTROL != null)
                            {
                                if (frmStudioMain.GLCONTROL.InvokeRequired)
                                {
                                    frmStudioMain.GLCONTROL.Invoke(new MethodInvoker(delegate { stlModelRotationX.UpdateBinding(); }));
                                }
                                else
                                {
                                    stlModelRotationX.UpdateBinding();
                                }
                            }
                        }
                        break;
                    case typeOfAction.STLModelRotatingY:
                        var stlModelRotationY = undoAction.Item as STLModel3D;
                        stlModelRotationY.Rotate(stlModelRotationY.RotationAngleX, (float)undoAction.Values, stlModelRotationY.RotationAngleZ, Events.RotationEventArgs.TypeAxis.Y);

                        if (ObjectView.BindingSupported)
                        {
                            if (frmStudioMain.GLCONTROL != null)
                            {
                                if (frmStudioMain.GLCONTROL.InvokeRequired)
                                {
                                    frmStudioMain.GLCONTROL.Invoke(new MethodInvoker(delegate { stlModelRotationY.UpdateBinding(); }));
                                }
                                else
                                {
                                    stlModelRotationY.UpdateBinding();
                                }
                            }
                        }

                        break;
                    case typeOfAction.STLModelRotatingZ:
                        var stlModelRotationZ = undoAction.Item as STLModel3D;
                        stlModelRotationZ.Rotate(stlModelRotationZ.RotationAngleX, stlModelRotationZ.RotationAngleY, (float)undoAction.Values, Events.RotationEventArgs.TypeAxis.Z);

                        if (ObjectView.BindingSupported)
                        {
                            if (frmStudioMain.GLCONTROL != null)
                            {
                                if (frmStudioMain.GLCONTROL.InvokeRequired)
                                {
                                    frmStudioMain.GLCONTROL.Invoke(new MethodInvoker(delegate { stlModelRotationZ.UpdateBinding(); }));
                                }
                                else
                                {
                                    stlModelRotationZ.UpdateBinding();
                                }
                            }
                        }

                        break;
                    case typeOfAction.STLModelScaling:
                        var stlModelScaling = undoAction.Item as STLModel3D;
                        var stlModelScalingArgs = (ScaleEventArgs)undoAction.Values;
                        if (stlModelScalingArgs.Axis == Events.ScaleEventArgs.TypeAxis.ALL)
                        {
                            stlModelScaling.Scale(stlModelScalingArgs.ScaleFactor, stlModelScalingArgs.ScaleFactor, stlModelScalingArgs.ScaleFactor, Events.ScaleEventArgs.TypeAxis.ALL);
                        }
                        else if (stlModelScalingArgs.Axis == Events.ScaleEventArgs.TypeAxis.X) {
                            stlModelScaling.Scale(stlModelScalingArgs.ScaleFactor, stlModelScaling.ScaleFactorY, stlModelScaling.ScaleFactorZ, Events.ScaleEventArgs.TypeAxis.X);
                        }
                        else if (stlModelScalingArgs.Axis == Events.ScaleEventArgs.TypeAxis.Y)
                        {
                            stlModelScaling.Scale(stlModelScalingArgs.ScaleFactor, stlModelScalingArgs.ScaleFactor, stlModelScaling.ScaleFactorZ, Events.ScaleEventArgs.TypeAxis.Y);
                        }
                        else if (stlModelScalingArgs.Axis == Events.ScaleEventArgs.TypeAxis.Z)
                        {
                            stlModelScaling.Scale(stlModelScaling.ScaleFactorX, stlModelScaling.ScaleFactorY, stlModelScalingArgs.ScaleFactor, Events.ScaleEventArgs.TypeAxis.Z);
                        }

                        if (ObjectView.BindingSupported)
                        {
                            if (frmStudioMain.GLCONTROL != null)
                            {
                                if (frmStudioMain.GLCONTROL.InvokeRequired)
                                {
                                    frmStudioMain.GLCONTROL.Invoke(new MethodInvoker(delegate { stlModelScaling.UpdateBinding(); }));
                                }
                                else
                                {
                                    stlModelScaling.UpdateBinding();
                                }
                            }
                        }
                        break;
                    case typeOfAction.SupportConeMoveTranslation:
                        var supportConeMoveUndoAction = undoAction as UndoRedoSupportMoveTranslation;
                        var supportConeMoveTranslation = supportConeMoveUndoAction.Item as SupportCone;
                        supportConeMoveTranslation.MoveTranslation -= (Vector3)supportConeMoveUndoAction.Values;
                        //supportConeMoveTranslation.UpdateHeight(supportConeMoveUndoAction.Height);

                        break;
                }
                //
                _redoActions.Push(undoAction);
            }
        }
    }

    internal interface IUndoRedo
    {
        Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction ActionType { get; set; }
        object Item { get; set; }
        object Values { get; set; }
    }

    internal class UndoRedoObjectMoveTranslation : IUndoRedo
    {

        public Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction ActionType { get; set; }
        public object Item { get; set; }
        public object Values { get; set; }

        public UndoRedoObjectMoveTranslation(Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction actionType, object item, object values)
        {
            this.ActionType = actionType;
            this.Item = item;
            this.Values = values;
        }
    }

    internal class UndoRedoSupportMoveTranslation : IUndoRedo
    {

        public Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction ActionType { get; set; }
        public object Item { get; set; }
        public object Values { get; set; }
        public float Height;

        public UndoRedoSupportMoveTranslation(Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction actionType, object item, object values, float height)
        {
            this.ActionType = actionType;
            this.Item = item;
            this.Values = values;
            this.Height = height;
        }
    }

    internal class UndoRedoObjectRotation : IUndoRedo
    {
        public Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction ActionType { get; set; }
        public object Item { get; set; }
        public object Values { get; set; }

        public UndoRedoObjectRotation(Atum.Studio.Core.Managers.UndoRedoManager.typeOfAction actionType, object item, object values)
        {
            this.ActionType = actionType;
            this.Item = item;
            this.Values = values;
        }
    }
}
