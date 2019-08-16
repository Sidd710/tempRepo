using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;
using System.Windows.Forms;
using static Atum.Studio.Core.Enums;

namespace Atum.Studio.Core.Managers
{
    internal class ToolbarActionsManager
    {
        internal static void Start()
        {
        }

        internal static void Update(MainFormToolStripActionType actionType, Controls.OpenGL.SceneGLControl sceneControl, PictureBox buttonPressed)
        {
            SceneControlToolbarManager.PrintJobPropertiesToolbar.DeselectPrintJobName();

            if (sceneControl != null)
            {
                sceneControl.DrawSelectedTriangleRayTrace = false;
                sceneControl.ModelContextFormMenu.Close(true);
                sceneControl.SupportConeContextFormMenu.Close(true);

                var selectedObjectType = SceneView.SelectedModelType;

                //only process magsai blending is current view (before changing) is MAGS AI mode
                if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                {
                    foreach (STLModel3D stlModel in ObjectView.Objects3D)
                    {
                        if (!(stlModel is GroundPane)){
                            stlModel.ChangeTrianglesToModelViewMode();
                        }
                    }
                }

                switch (actionType)
                {
                    case MainFormToolStripActionType.btnPanPressed:
                        SceneControlToolbarManager.ModelActionsToolbar.DeselectButtons();
                        SceneActionControlManager.ResetPanels();
                        SceneView.ChangeViewMode(SceneView.ViewMode.Pan, sceneControl, buttonPressed);
                        SceneActionControlManager.RemoveSupportPropertiesHandle();

                        break;
                    case MainFormToolStripActionType.btnZoomPressed:
                        SceneControlToolbarManager.ModelActionsToolbar.DeselectButtons();
                        SceneActionControlManager.ResetPanels();
                        SceneView.ChangeViewMode(SceneView.ViewMode.Zoom, sceneControl, buttonPressed);
                        SceneActionControlManager.RemoveSupportPropertiesHandle();

                        break;

                    case MainFormToolStripActionType.btnOrbitPressed:
                        SceneControlToolbarManager.ModelActionsToolbar.DeselectButtons();
                        SceneActionControlManager.ResetPanels();
                        SceneView.ChangeViewMode(SceneView.ViewMode.Orbit, sceneControl, buttonPressed);
                        SceneActionControlManager.RemoveSupportPropertiesHandle();

                        break;
                    case MainFormToolStripActionType.btnModelActionDuplicate:
                        SceneView.ChangeViewMode(SceneView.ViewMode.Duplicate, sceneControl, buttonPressed);

                        break;
                    case MainFormToolStripActionType.btnModelActionMagsAI:
                        SceneView.ChangeViewMode(SceneView.ViewMode.MagsAI, sceneControl, buttonPressed);
                        var selectedModel = ObjectView.SelectedModel;
                        selectedModel.ChangeTrianglesToBlendViewMode(false);
                        break;
                    case MainFormToolStripActionType.btnModelActionMagsAIManualSupport:
                        SceneView.ChangeViewMode(SceneView.ViewMode.MagsAIManualSupport, sceneControl, buttonPressed);
                        break;
                    case MainFormToolStripActionType.btnModelActionMagsAIGridSupport:
                        SceneView.ChangeViewMode(SceneView.ViewMode.MagsAIGridSupport, sceneControl, buttonPressed);
                        break;
                    case MainFormToolStripActionType.btnSelectPressed:
                        SceneControlToolbarManager.ModelActionsToolbar.DeselectButtons();

                        SceneActionControlManager.ResetPanels();
                        SceneView.ChangeViewMode(SceneView.ViewMode.SelectObject, sceneControl, buttonPressed);

                        break;
                    case MainFormToolStripActionType.btnMovePressed:
                        SceneView.ChangeViewMode(SceneView.ViewMode.MoveTranslation, sceneControl, buttonPressed);
                        if (ObjectView.SelectedObject is TriangleSurfaceInfo)
                        {
                            SceneView.MoveTranslation3DGizmo.UpdateControl(SceneViewSelectedMoveTranslationAxisType.Hidden, true);
                        }
                        else
                        {
                            SceneView.MoveTranslation3DGizmo.UpdateControl(SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
                        }

                        break;
                    case MainFormToolStripActionType.btnRotatePressed:
                        SceneView.ChangeViewMode(SceneView.ViewMode.ModelRotation, sceneControl, buttonPressed);
                        SceneView.Rotation3DGizmo.UpdateControl(SceneViewSelectedRotationAxisType.None);

                        break;
                    case MainFormToolStripActionType.btnScalePressed:
                        SceneView.ChangeViewMode(SceneView.ViewMode.ModelScale, sceneControl, buttonPressed);

                        break;
                    case MainFormToolStripActionType.btnManualGridSupportCone:
                        SceneView.ChangeViewMode(SceneView.ViewMode.MagsAIGridSupport, sceneControl, buttonPressed);

                        break;
                    case MainFormToolStripActionType.btnLayFlatPressed:
                        SceneView.ChangeViewMode(SceneView.ViewMode.LayFlat, sceneControl, buttonPressed);

                        break;

                }
                sceneControl.Render();
            }
        }

    }
}
