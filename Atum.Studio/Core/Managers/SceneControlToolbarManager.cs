using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.ExportControl;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Controls.SceneControlToolbars;
using Atum.Studio.Controls.SceneControlToolTips;
using Atum.Studio.Core.ModelView;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    public class SceneControlToolbarManager
    {
        internal static List<SceneControlToolbarBase> Toolbars = null;

        public static SceneControlPrintJobPropertiesToolbar PrintJobProperties;
        internal static event EventHandler<AtumPrinter> SelectedPrinterChanged;
        internal static event EventHandler OnEscKeyPressed;

        internal static string PrintjobName
        {
            get
            {
                return PrintJobProperties.PrintjobName;
            }
            set
            {
                PrintJobProperties.PrintjobName = value;
            }
        }

        public static AtumPrinter SelectedPrinter
        {
            get
            {
                if (PrintJobProperties != null)
                {
                    return PrintJobProperties.SelectedPrinter;
                }
                else
                {
                    return null;
                }
            }
            set
            {
                if (PrintJobProperties == null)
                {
                    PrintJobProperties = new SceneControlPrintJobPropertiesToolbar(null);
                }
                PrintJobProperties.SelectedPrinter = value;
                if (PrintJobProperties != null && PrintJobProperties.ParentForm != null)
                {
                    SceneView.UpdateGroundPane(value);
                }
            }
        }


        internal static MaterialSummary SelectedMaterial
        {
            get
            {
                if (PrintJobProperties != null)
                {
                    return PrintJobProperties.SelectedMaterial;
                }
                else{
                    return null;
                }
            }
        }

        internal static SceneControlModelActionsToolbar ModelActionsToolbar
        {
            get
            {
                var modelToolbar = Toolbars.OfType<SceneControlModelActionsToolbar>().FirstOrDefault();
                if (modelToolbar != null)
                {
                    return modelToolbar;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static SceneControlPrintJobPropertiesToolbar PrintJobPropertiesToolbar
        {
            get
            {
                var printjobProperties = Toolbars.OfType<SceneControlPrintJobPropertiesToolbar>().FirstOrDefault();
                if (printjobProperties != null)
                {
                    return printjobProperties;
                }
                else
                {
                    return null;
                }
            }
        }

        internal static void Initialize(Control parentPanel)
        {
            MaterialManager.SelectedMaterialUpdated -= MaterialManager_SelectedMaterialUpdated;
            if (PrintJobProperties == null)
            {
                PrintJobProperties = new SceneControlPrintJobPropertiesToolbar(parentPanel);
                PrintJobProperties.OnEscKeyPressed += PrintJobProperties_OnEscKeyPressed;
            }
            else
            {
                PrintJobProperties.UpdateControl();
            }
            MaterialManager.SelectedMaterialUpdated += MaterialManager_SelectedMaterialUpdated;
            PrintJobProperties.SelectedPrinterChanged += PrintJobProperties_SelectedPrinterChanged;

            var modelActionToolbar = new SceneControlModelActionsToolbar();
            modelActionToolbar.RotationSelectedModelContainsLinkedClones += ModelActionToolbar_RotationSelectedModelContainsLinkedClones;
            modelActionToolbar.ScaleSelectedModelContainsLinkedClones += ModelActionToolbar_ScaleSelectedModelContainsLinkedClones;
            modelActionToolbar.MAGSAINoModelSelected += ModelActionToolbar_MAGSAISelectedModelContainsLinkedClones;
            //modelActionToolbar.MoveSelectedModelContainsLinkedClones += ModelActionToolbar_MoveSelectedModelContainsLinkedClones;

            Toolbars = new List<SceneControlToolbarBase>();
            Toolbars.Add(modelActionToolbar);
            Toolbars.Add(new SceneControlCameraActionsToolbar());
            Toolbars.Add(new SceneControlRenderModeToolbar());
            Toolbars.Add(PrintJobProperties);
        }
        private static void PrintJobProperties_OnEscKeyPressed(object sender, EventArgs e)
        {
            OnEscKeyPressed.Invoke(sender, e);
        }

        private static void ModelActionToolbar_MAGSAISelectedModelContainsLinkedClones(object sender, EventArgs e)
        {
            var tt = new SceneControlManualToolTip();
            tt.SetText("No model selected");

            var btnSupport = ModelActionsToolbar.ModelSupportButton;
            var btnToolTipStartPoint = new Point(btnSupport.Left + btnSupport.Width + ModelActionsToolbar.Left + 16, btnSupport.Top + ModelActionsToolbar.Top);
            btnToolTipStartPoint.Y += (btnSupport.Height / 2) - (tt.Height / 2);
            tt.ShowToolTip(btnToolTipStartPoint);
        }

        //internal static void ShowMoveSelectedModelContainsLinkedClonesToolTip()
        //{
        //    ModelActionToolbar_MoveSelectedModelContainsLinkedClones(null, null);
        //}

        internal static void ShowMAGSAISelectedModelContainsLinkedClonesToolTip()
        {
            ModelActionToolbar_MAGSAISelectedModelContainsLinkedClones(null, null);
        }

        internal static void ShowRotationSelectedModelContainsLinkedClonesToolTip()
        {
            ModelActionToolbar_RotationSelectedModelContainsLinkedClones(null, null);
        }

        private static void ModelActionToolbar_RotationSelectedModelContainsLinkedClones(object sender, EventArgs e)
        {
            var tt = new SceneControlManualToolTip();
            tt.SetText("Unavailable for duplicated model");

            var btnRotate = ModelActionsToolbar.ModelRotateButton;
            var btnToolTipStartPoint = new Point(btnRotate.Left + btnRotate.Width + ModelActionsToolbar.Left + 16, btnRotate.Top + ModelActionsToolbar.Top);
            btnToolTipStartPoint.Y += (btnRotate.Height / 2) - (tt.Height / 2);
            tt.ShowToolTip(btnToolTipStartPoint);
        }

        private static void ModelActionToolbar_ScaleSelectedModelContainsLinkedClones(object sender, EventArgs e)
        {
            var tt = new SceneControlManualToolTip();
            tt.SetText("Unavailable for duplicated model");

            var btnScale = ModelActionsToolbar.ModelScaleButton;
            var btnToolTipStartPoint = new Point(btnScale.Left + btnScale.Width + ModelActionsToolbar.Left + 16, btnScale.Top + ModelActionsToolbar.Top);
            btnToolTipStartPoint.Y += (btnScale.Height / 2) - (tt.Height / 2);
            tt.ShowToolTip(btnToolTipStartPoint);
        }

        private static void MaterialManager_SelectedMaterialUpdated(object sender, MaterialSummary e)
        {
            if (PrintJobProperties != null)
            {
                //user profile is updated using property below
                PrintJobProperties.SelectedMaterial = e;

                //update all supportcones

            }
            
        }

        internal static void Reinitialize(SceneGLControl sceneControl)
        {
            if (sceneControl != null)
            {
                sceneControl.ModelContextFormMenu.Close(false);
                sceneControl.SupportConeContextFormMenu.Close(false);

                for (var controlIndex = sceneControl.Controls.Count - 1; controlIndex > 1; controlIndex--)
                {
                    if (sceneControl.Controls[controlIndex] is SceneControlToolbarBase)
                    {
                        sceneControl.Controls.RemoveAt(controlIndex);
                    }
                }

                if (frmStudioMain.SceneControl.Controls.OfType<ExportUserControl>().Count() == 0)
                {

                    Initialize(sceneControl.Parent);

                    ShowToolbars(sceneControl);

                    var selectedModel = ObjectView.SelectedModel;
                    if (selectedModel != null)
                    {
                        UpdateModelDimensions(selectedModel.Width, selectedModel.Depth, selectedModel.Height);
                    }

                    //sceneControl.Render();
                }
            }
        }

        private static void PrintJobProperties_SelectedPrinterChanged(object sender, AtumPrinter e)
        {
            SelectedPrinterChanged?.Invoke(sender, e);
        }

        internal static void UpdateModelDimensions(float? width, float? depth, float? height)
        {
            PrintJobProperties.ModelWidth = width.HasValue ? width.Value.ToString("0.00") + " mm" : "-";
            PrintJobProperties.ModelDepth = depth.HasValue ? depth.Value.ToString("0.00") + " mm" : "-";
            PrintJobProperties.ModelHeight = height.HasValue ? height.Value.ToString("0.00") + " mm" : "-";
        }

        internal static void ShowToolbars(SceneGLControl sceneControl)
        {
            var controlsToRemove = new List<Control>();
            foreach (Control control in sceneControl.Controls)
            {
                if (control is SceneControlToolbarBase || control is ExportUserControl)
                {
                    controlsToRemove.Add(control);
                }
            }

            foreach (var control in controlsToRemove)
            {
                sceneControl.Controls.Remove(control);
            }

            //add new controls
            foreach (var panel in Toolbars)
            {
                sceneControl.Controls.Add(panel);
            }

            sceneControl.Render();
        }

        internal static void HideToolbars(SceneGLControl sceneControl)
        {
            var controlsToRemove = new List<Control>();
            foreach (Control control in sceneControl.Controls)
            {
                controlsToRemove.Add(control);
            }
            foreach (var control in controlsToRemove)
            {
                sceneControl.Controls.Remove(control);
            }

            sceneControl.Render();

        }

        internal static void Redraw(SceneGLControl sceneControl)
        {
            sceneControl.ModelContextFormMenu.Close(false);
            sceneControl.SupportConeContextFormMenu.Close(false);
            foreach (Control control in sceneControl.Controls)
            {
                if (control is SceneControlToolbarBase)
                {
                    control.Invalidate();
                    control.Refresh();
                }
            }
        }
    }
}
