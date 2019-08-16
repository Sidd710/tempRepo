using Atum.Studio.Controls;
using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Controls.SceneControlActionPanel;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    internal class SceneActionControlManager
    {
        internal static event EventHandler<Events.MovementEventArgs> MoveTranslationChanged;
        internal static event EventHandler<Events.RotationEventArgs> RotationChanged;
        internal static event EventHandler<Events.ScaleEventArgs> ScalingChanged;
        internal static event EventHandler<Events.SelectionBoxSizeEventArgs> SelectionBoxSizeChanged;
        internal static event EventHandler<Events.CloneModelsEventArgs> CloneModels;
        internal static SceneActionPanels ActionPanels;

        internal static SceneControlModelMagsAI ActionPanelMagsAI
        {
            get
            {
                if (IsActionPanelMagsAIVisible)
                {
                    var magsAIActionPanel = (SceneControlModelMagsAI)ActionPanels.Where(s => s is SceneControlModelMagsAI).First();
                    return magsAIActionPanel;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static SceneControlModelScale ActionPanelScale
        {
            get
            {
                if (IsActionPanelScaleVisible)
                {
                    var scaleActionPanel = (SceneControlModelScale)ActionPanels.Where(s => s is SceneControlModelScale).First();
                    return scaleActionPanel;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static SceneControlModelMove ActionPanelMove
        {
            get
            {
                if (IsActionPanelMoveVisible)
                {
                    var moveModelActionPanel = (SceneControlModelMove)ActionPanels.Where(s => s is SceneControlModelMove).First();
                    return moveModelActionPanel;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static bool IsActionPanelMagsAIVisible
        {
            get
            {
                return ActionPanels.Where(s => s is SceneControlModelMagsAI).Count() > 0;
            }
        }

        internal static bool IsActionPanelScaleVisible
        {
            get
            {
                return ActionPanels.Where(s => s is SceneControlModelScale).Count() > 0;
            }
        }

        internal static bool IsActionPanelMoveVisible
        {
            get
            {
                return ActionPanels.Where(s => s is SceneControlModelMove).Count() > 0;
            }
        }

        internal static bool IsSupportPropertiesHandleVisible
        {
            get
            {
                return frmStudioMain.SceneControl.Controls.OfType<SceneControlModelSupportPropertiesHandle>().Count() > 0;
            }
        }

        internal static bool IsSupportPropertiesVisible
        {
            get
            {
                return frmStudioMain.SceneControl.Controls.OfType<SceneControlModelSupportProperties>().Count() > 0;
            }
        }

        internal static SceneControlModelRotate ActionPanelRotate
        {
            get
            {
                if (IsActionPanelRotateVisible)
                {
                    var rotateModelActionPanel = (SceneControlModelRotate)ActionPanels.Where(s => s is SceneControlModelRotate).First();
                    return rotateModelActionPanel;
                }
                else
                {
                    return null;
                }
            }
        }
        internal static bool IsActionPanelRotateVisible
        {
            get
            {
                return ActionPanels.Where(s => s is SceneControlModelRotate).Count() > 0;
            }
        }

        internal static void Initialize(Panel parentPanel)
        {
            ActionPanels = new SceneActionPanels();

            //simulation panel

            // ActionPanels.Add(simulationPanel);

            //manual support cone
            //var manualSupportConePanel = new SceneControlManualSupportCone();
            //manualSupportConePanel.Visible = true;
            //ActionPanels.Add(manualSupportConePanel);

            //project outline
            //    var printjobOutlinePanel = new SceneControlProjectOutlinePanel();
            //    printjobOutlinePanel.NavigationNodeChanged += PrintjobOutlinePanel_NavigationNodeChanged;
            //    ActionPanels.Add(printjobOutlinePanel);

        }


        private static void MovementPanel_ValueChanged(object sender, Events.MovementEventArgs e)
        {
            MoveTranslationChanged?.Invoke(sender, e);
        }

        private static void RotationPanel_ValueChanged(object sender, Events.RotationEventArgs e)
        {
            RotationChanged?.Invoke(sender, e);
        }

        private static void ScalingPanel_ValueChanged(object sender, Events.ScaleEventArgs e)
        {
            ScalingChanged?.Invoke(sender, e);
        }

        private static void SelectionBoxPanel_SelectionBoxSizeValueChanged(object sender, Events.SelectionBoxSizeEventArgs e)
        {
            SelectionBoxSizeChanged?.Invoke(sender, e);
        }

        internal static void ResetPanels()
        {
            var controlIndexesToRemove = new List<int>();
            for (var controlIndex = frmStudioMain.SceneControl.Controls.Count - 1; controlIndex >= 0; controlIndex--)
            {
                var control = frmStudioMain.SceneControl.Controls[controlIndex];
                if (control is SceneControlActionPanelBase)
                {
                    controlIndexesToRemove.Add(controlIndex);
                }
            }

            foreach (var controlIndexToRemove in controlIndexesToRemove)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(new MethodInvoker(delegate
                    {
                        frmStudioMain.SceneControl.Controls.RemoveAt(controlIndexToRemove);
                    }));
                }
                else
                {
                    frmStudioMain.SceneControl.Controls.RemoveAt(controlIndexToRemove);
                }

            }

            if (ActionPanels != null)
            {
                ActionPanels.Clear();
            }

            SceneActionControlManager.RemoveSupportPropertiesHandle();
        }

        internal static void ShowPanel(Enums.MainFormToolStripActionType actionType, PictureBox btnToolstrip, SceneGLControl sceneControl)
        {
            var selectedModel = ObjectView.SelectedModel;

            if ((actionType == Enums.MainFormToolStripActionType.btnModelActionMagsAI || actionType == Enums.MainFormToolStripActionType.btnModelActionMagsAIGridSupport || actionType == Enums.MainFormToolStripActionType.btnModelActionMagsAIManualSupport)
                && IsActionPanelMagsAIVisible)
            {

            }
            else
            {
                ResetPanels();
            }

            switch (actionType)
            {
                case Enums.MainFormToolStripActionType.btnMovePressed:
                    if (UserProfileManager.UserProfiles[0].Settings_Use_Numeric_Input_For_Positioning)
                    {
                        var modelMovePanel = new SceneControlModelMove();
                        //modelMovePanel.ValueChanged += CloneModels;
                        modelMovePanel.Show(btnToolstrip);
                        var selectedLinkedClone = selectedModel.LinkedClones.FirstOrDefault(s => s.Selected);
                        if (selectedLinkedClone != null)
                        {
                            modelMovePanel.DataSource = selectedLinkedClone.Translation + new Vector3Class(0,0, selectedModel.BottomPoint);
                        }
                        else
                        {
                            modelMovePanel.DataSource = selectedModel.MoveTranslation;
                        }
                        modelMovePanel.Select();
                        ActionPanels.Add(modelMovePanel);
                    }
                    break;
                case Enums.MainFormToolStripActionType.btnRotatePressed:
                    if (UserProfileManager.UserProfiles[0].Settings_Use_Numeric_Input_For_Positioning)
                    {
                        var modelRotatePanel = new SceneControlModelRotate();
                        //modelMovePanel.ValueChanged += CloneModels;
                        modelRotatePanel.Show(btnToolstrip);
                        // modelRotatePanel.DataSource=selectedModel.RotationAngleX;
                        modelRotatePanel.RotateX = selectedModel.RotationAngleX;
                        modelRotatePanel.RotateY = selectedModel.RotationAngleY;
                        modelRotatePanel.RotateZ = selectedModel.RotationAngleZ;
                        modelRotatePanel.Select();
                        ActionPanels.Add(modelRotatePanel);
                    }
                    break;
                case Enums.MainFormToolStripActionType.btnModelActionDuplicate:
                    var duplicatePanel = new SceneControlModelDuplicate();
                    duplicatePanel.ValueChanged += CloneModels;
                    duplicatePanel.FillBuildPlateCompleted += DuplicatePanel_FillBuildPlateCompleted;
                    duplicatePanel.Init();
                    duplicatePanel.Show(btnToolstrip);
                    duplicatePanel.Select();
                    ActionPanels.Add(duplicatePanel);
                    break;
                case Enums.MainFormToolStripActionType.btnModelActionMagsAI:
                    var magsAIPanel = ActionPanelMagsAI;
                    if (magsAIPanel == null)
                    {
                        magsAIPanel = new SceneControlModelMagsAI();
                        magsAIPanel.MAGSAICompleted += MagsAIPanel_MAGSAICompleted;
                        magsAIPanel.Show(btnToolstrip);
                        magsAIPanel.ChangeDefaultView(Enums.MainFormToolStripActionType.btnModelActionMagsAI);
                        ActionPanels.Add(magsAIPanel);
                    }
                    else
                    {
                        magsAIPanel.ChangeDefaultView(Enums.MainFormToolStripActionType.btnModelActionMagsAI);
                    }
                    break;
                case Enums.MainFormToolStripActionType.btnModelActionMagsAIManualSupport:
                    var magsAIManualSupportPanel = ActionPanelMagsAI;
                    if (magsAIManualSupportPanel == null)
                    {
                        magsAIManualSupportPanel = new SceneControlModelMagsAI();
                        magsAIManualSupportPanel.MAGSAICompleted += MagsAIPanel_MAGSAICompleted;
                        magsAIManualSupportPanel.Show(btnToolstrip);
                        magsAIManualSupportPanel.ChangeDefaultView(Enums.MainFormToolStripActionType.btnModelActionMagsAIManualSupport);
                        ActionPanels.Add(magsAIManualSupportPanel);
                    }
                    else
                    {
                        magsAIManualSupportPanel.ChangeDefaultView(Enums.MainFormToolStripActionType.btnModelActionMagsAIManualSupport);
                    }

                    break;
                case Enums.MainFormToolStripActionType.btnModelActionMagsAIGridSupport:
                    var magsAIGridSupportPanel = ActionPanelMagsAI;
                    if (magsAIGridSupportPanel == null)
                    {
                        magsAIGridSupportPanel = new SceneControlModelMagsAI();
                        magsAIGridSupportPanel.MAGSAICompleted += MagsAIPanel_MAGSAICompleted;
                        magsAIGridSupportPanel.Show(btnToolstrip);
                        magsAIGridSupportPanel.ChangeDefaultView(Enums.MainFormToolStripActionType.btnModelActionMagsAIGridSupport);
                        ActionPanels.Add(magsAIGridSupportPanel);
                    }
                    else
                    {
                        magsAIGridSupportPanel.ChangeDefaultView(Enums.MainFormToolStripActionType.btnModelActionMagsAIGridSupport);
                    }
                    break;
                case Enums.MainFormToolStripActionType.btnScalePressed:
                    var scalePanel = new SceneControlModelScale();
                    scalePanel.DataSource = selectedModel;
                    scalePanel.ValueChanged += ScalingPanel_ValueChanged;
                    scalePanel.Show(btnToolstrip);
                    scalePanel.Select();
                    ActionPanels.Add(scalePanel);
                    break;
            }

            foreach (var panel in ActionPanels)
            {
                if (sceneControl.InvokeRequired)
                {
                    sceneControl.Invoke(new MethodInvoker(delegate
                    {
                        if (panel.InvokeRequired)
                        {
                            sceneControl.Controls.Add(panel);
                        }

                    }));
                }
                else
                {
                    sceneControl.Controls.Add(panel);
                }

            }
        }

        internal static void AddSupportPropertiesHandle(Point mouseLoction, SupportCone selectedSupportCone)
        {
            var supportPropertiesHandle = new SceneControlModelSupportPropertiesHandle();
            supportPropertiesHandle.Location = mouseLoction;
            supportPropertiesHandle.Top -= (supportPropertiesHandle.Height);
            supportPropertiesHandle.Left += 10;
            supportPropertiesHandle.DataSource = selectedSupportCone;

            if (frmStudioMain.SceneControl.InvokeRequired)
            {
                frmStudioMain.SceneControl.Invoke(new MethodInvoker
                (delegate
                {
                    frmStudioMain.SceneControl.Controls.Add(supportPropertiesHandle);
                }
                ));
            }
            else
            {
                frmStudioMain.SceneControl.Controls.Add(supportPropertiesHandle);
            }

            supportPropertiesHandle.Visible = true;
        }

        internal static void RemoveSupportPropertiesHandle()
        {
            if (IsSupportPropertiesHandleVisible)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(
                    new MethodInvoker(delegate
                    {
                        RemoveSupportPropertiesHandle();
                    }
                        ));
                }
                else
                {
                    var handles = frmStudioMain.SceneControl.Controls.OfType<SceneControlModelSupportPropertiesHandle>().ToList();
                    foreach (var handle in handles)
                    {
                        frmStudioMain.SceneControl.Controls.Remove(handle);
                    }

                    var supportProperties = frmStudioMain.SceneControl.Controls.OfType<SceneControlModelSupportProperties>().ToList();
                    foreach (var supportProperty in supportProperties)
                    {
                        frmStudioMain.SceneControl.Controls.Remove(supportProperty);
                    }
                }

            }
        }

        internal static void CloseSupportPropertiesHandle()
        {
            if (IsSupportPropertiesHandleVisible)
            {
                if (frmStudioMain.SceneControl.InvokeRequired)
                {
                    frmStudioMain.SceneControl.Invoke(
                    new MethodInvoker(delegate
                    {
                        CloseSupportPropertiesHandle();
                    }
                        ));
                }
                else
                {
                    var handle = frmStudioMain.SceneControl.Controls.OfType<SceneControlModelSupportPropertiesHandle>().FirstOrDefault();
                    if (handle != null)
                    {
                        handle.CloseState();
                    }
                }

            }
        }

        private static void MagsAIPanel_MAGSAICompleted(object sender, EventArgs e)
        {
            ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);

            SceneControlToolbarManager.ModelActionsToolbar.SelectButton(SceneControlToolbarManager.ModelActionsToolbar.ModelDuplicateButton);
        }

        private static void DuplicatePanel_FillBuildPlateCompleted(object sender, EventArgs e)
        {
            ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
        }
        
    }
}
