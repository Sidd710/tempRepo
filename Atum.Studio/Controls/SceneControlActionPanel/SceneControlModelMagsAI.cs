using System;
using System.Drawing;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Events;
using System.Drawing.Drawing2D;
using static Atum.Studio.Core.Enums;
using Atum.Studio.Core.Helpers;
using System.Threading.Tasks;
using System.Linq;
using Atum.Studio.Controls.OpenGL;
using System.Threading;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlModelMagsAI : SceneControlActionPanelBase
    {
        private Font _fontRegular = null;
        private bool _showMoreOptionsCollapsed = true;

        internal event EventHandler MAGSAICompleted;

        internal void ChangeDefaultView(MainFormToolStripActionType viewType)
        {
            if (viewType == MainFormToolStripActionType.btnModelActionMagsAI)
            {
                this.btnGenerateSupport.Enabled = true;
                if (this._showMoreOptionsCollapsed)
                {
                    UpdateMoreOptionsCollapseState(true);
                }
            }
            else if (viewType == MainFormToolStripActionType.btnModelActionMagsAIManualSupport)
            {
                if (!this._showMoreOptionsCollapsed)
                {
                    UpdateMoreOptionsCollapseState(false);
                }

                btnModelAddSingleSupport_Click(null, null);
            }
            else if (viewType == MainFormToolStripActionType.btnModelActionMagsAIGridSupport)
            {
                if (!this._showMoreOptionsCollapsed)
                {
                    UpdateMoreOptionsCollapseState(false);
                }

                btnModelAddGridSupport_Click(null, null);
            }
        }

        public SceneControlModelMagsAI()
        {
            InitializeComponent();

            this.onClosed += SceneControlModelMagsAI_onClosed;
            this.btnSelectionCircle.BackColor = this.btnSelectionCircle.SelectedBackgroundColor = BrandingManager.Button_HighlightColor;
            this.btnGenerateSupport.BackColor = this.btnModelMarkings.BorderColor = this.btnModelMarkings.ForeColor = BrandingManager.Button_BackgroundColor_Dark;

            this.HeaderText = "MAGS AI";

            if (FontManager.Loaded)
            {
                _fontRegular = FontManager.Montserrat14Regular;
            }
            else
            {
                this._fontRegular = new Font(FontFamily.GenericSerif, 14, FontStyle.Regular, GraphicsUnit.Pixel);
            }

            this.lblInformation.Font = this._fontRegular;
            this.btnGenerateSupport.Font = this._fontRegular;
            this.lblMoreOptions.Font = this._fontRegular;
            this.plMoreOptions.Left = this.lblMoreOptions.Left + this.lblMoreOptions.Width;

            //left icon
            var leftIconGraphicsPath = new GraphicsPath();
            leftIconGraphicsPath.AddEllipse(new RectangleF((this.btnSelectionCircle.Width / 2f) - 6, (btnSelectionCircle.Height / 2f) - 7.5f, 16, 16));
            this.btnSelectionCircle.IconGraphicsPath = leftIconGraphicsPath;
            this.btnSelectionCircle.IsSelected = true;

            //right icon
            var rightIconGraphicsPath = new GraphicsPath();
            rightIconGraphicsPath.AddRectangle(new RectangleF((this.btnSelectionCircle.Width / 2f) - 8, (btnSelectionCircle.Height / 2f) - 6, 14, 14));
            this.btnSelectionSquare.IconGraphicsPath = rightIconGraphicsPath;
            this.btnSelectionSquare.IsSelected = false;

            this.magsAISelectionSize.ValueChanged -= magsAISelectionSize_ValueChanged;
            this.magsAISelectionSize.Value = UserProfileManager.UserProfile.Settings_Studio_SelectionBox_Size;
            this.magsAISelectionSize.ValueChanged += magsAISelectionSize_ValueChanged;

            if (UserProfileManager.UserProfile.Settings_Studio_SelectionBox_Type == Core.Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox.Circle)
            {
                this.btnSelectionCircle_Click(null, null);
            }
            else
            {
                this.btnSelectionSquare_Click(null, null);
            }

            this.UpdateMoreOptionsCollapseState(true);
            MAGSAICompleted?.Invoke(null, null);
        }

        private void SceneControlModelMagsAI_onClosed(object sender, EventArgs e)
        {
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
    }

        private void txtDuplicatesCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnSelectionSquare_Click(object sender, EventArgs e)
        {
            this.btnSelectionCircle.BackColor = Color.White;
            this.btnSelectionCircle.IsSelected = false;
            this.btnSelectionCircle.Invalidate();

            this.btnSelectionSquare.BackColor = this.btnSelectionSquare.SelectedBackgroundColor;
            this.btnSelectionSquare.IsSelected = true;
            this.btnSelectionSquare.Invalidate();

            SceneView.MAGSAISelectionMarker.Diameter = UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Size * 10;
            UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type = Core.Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox.Square;
            UserProfileManager.Save();


            SceneView.MAGSAISelectionMarker.UpdateSelectionBox(new SelectionBoxSizeEventArgs(
                UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type),
                frmStudioMain.SceneControl.Width,
                frmStudioMain.SceneControl.Height,
                new Point(frmStudioMain.SceneControl.Width / 2, frmStudioMain.SceneControl.Height / 2));

            frmStudioMain.SceneControl.Render();
        }

        private void btnSelectionCircle_Click(object sender, EventArgs e)
        {
            this.btnSelectionCircle.BackColor = this.btnSelectionCircle.SelectedBackgroundColor;
            this.btnSelectionCircle.IsSelected = true;
            this.btnSelectionCircle.Invalidate();

            this.btnSelectionSquare.BackColor = Color.White;
            this.btnSelectionSquare.IsSelected = false;
            this.btnSelectionSquare.Invalidate();

            SceneView.MAGSAISelectionMarker.Diameter = UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Size * 10;
            UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type = Core.Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox.Circle;
            UserProfileManager.Save();

            SceneView.MAGSAISelectionMarker.UpdateSelectionBox(new SelectionBoxSizeEventArgs(
                UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type),
                frmStudioMain.SceneControl.Width,
                frmStudioMain.SceneControl.Height,
                new Point(frmStudioMain.SceneControl.Width / 2, frmStudioMain.SceneControl.Height / 2));

            frmStudioMain.SceneControl.Render();
        }

        private void UpdateMoreOptionsCollapseState(bool showMoreOptionsCollapsed)
        {
            if (showMoreOptionsCollapsed)
            {
                this.Height = 298;
                this.plMoreOptions.BackgroundImage = Properties.Resources.toolbar_actions_model_arrowdown;
            }
            else
            {
                this.Height = 382;
                this.plMoreOptions.BackgroundImage = Properties.Resources.toolbar_actions_model_arrowup;
            }

            this._showMoreOptionsCollapsed = !showMoreOptionsCollapsed;
            frmStudioMain.SceneControl.Render();
        }

        private void btnModelAddSingleSupport_Click(object sender, EventArgs e)
        {
            this.btnGenerateSupport.Enabled = false;

            if (this.plModelAddManualSupportRound.BackColor != BrandingManager.Button_HighlightColor)
            {
                this.plModelAddManualSupportRound.BackColor = BrandingManager.Button_HighlightColor;
                this.plModelAddManualSupport.BackColor = BrandingManager.Button_HighlightColor;
                this.plModelAddManualSupport.BackgroundImage = Properties.Resources.toolbar_actions_model_singlesuppor_white;
            }

            this.plModelAddGridSupportRound.BackColor = Color.White;
            this.plAddGridSupport.BackColor = Color.White;
            this.plAddGridSupport.BackgroundImage = Properties.Resources.toolbar_actions_model_gridsupport;

            SceneView.CurrentViewMode = SceneView.ViewMode.MagsAIManualSupport;
        }

        private void roundedButton2_Click(object sender, EventArgs e)
        {
            this.btnModelAddSingleSupport_Click(null, null);
        }

        private void plModelAddManualSupport_Click(object sender, EventArgs e)
        {
            this.btnModelAddSingleSupport_Click(null, null);
        }

        private void btnModelAddGridSupport_Click(object sender, EventArgs e)
        {
            this.btnGenerateSupport.Enabled = false;
            if (this.plModelAddGridSupportRound.BackColor != BrandingManager.Button_HighlightColor)
            {
                this.plModelAddGridSupportRound.BackColor = BrandingManager.Button_HighlightColor;
                this.plAddGridSupport.BackColor = BrandingManager.Button_HighlightColor;
                this.plAddGridSupport.BackgroundImage = Properties.Resources.toolbar_actions_model_gridsupport_white;
            }

            this.plModelAddManualSupportRound.BackColor = Color.White;
            this.plModelAddManualSupport.BackColor = Color.White;
            this.plModelAddManualSupport.BackgroundImage = Properties.Resources.toolbar_actions_model_singlesupport;

            SceneView.CurrentViewMode = SceneView.ViewMode.MagsAIGridSupport;
        }

        private void plAddGridSupport_Click(object sender, EventArgs e)
        {
            this.btnModelAddGridSupport_Click(null, null);
        }

        private void plModelAddGridSupportRound_Click(object sender, EventArgs e)
        {
            this.btnModelAddGridSupport_Click(null, null);
        }

        private void lblMoreOptions_Click(object sender, EventArgs e)
        {
            this.UpdateMoreOptionsCollapseState(this._showMoreOptionsCollapsed);
            if (this._showMoreOptionsCollapsed)
            {
                ToolbarActionsManager.Update(MainFormToolStripActionType.btnModelActionMagsAIManualSupport, frmStudioMain.SceneControl, null);
            }
            else
            {
                ToolbarActionsManager.Update(MainFormToolStripActionType.btnModelActionMagsAI, frmStudioMain.SceneControl, null);
            }
        }

        private void plMoreOptions_Click(object sender, EventArgs e)
        {
            this.lblMoreOptions_Click(null, null);
        }

        private void magsAISelectionSize_ValueChanged(object sender, decimal value)
        {
            SceneView.MAGSAISelectionMarker.Diameter = magsAISelectionSize.Value * 10;
            UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Size = magsAISelectionSize.Value;

            SceneView.MAGSAISelectionMarker.UpdateSelectionBox(new SelectionBoxSizeEventArgs(
                UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type),
                frmStudioMain.SceneControl.Width,
                frmStudioMain.SceneControl.Height,
                new Point(frmStudioMain.SceneControl.Width / 2, frmStudioMain.SceneControl.Height / 2));

            UserProfileManager.Save();

            frmStudioMain.SceneControl.Render();
        }

        private void btnModelClearSupport_Click(object sender, EventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null)
            {
                selectedModel.ClearMarkings();
                frmStudioMain.SceneControl.Render();
            }
        }

        private void btnGenerateSupport_Click(object sender, EventArgs e)
        {
            this.btnGenerateSupport.Enabled = false;
            var selectedModel = ObjectView.SelectedModel;
            selectedModel.SupportBasement = false;

            //calc new model normal
            var autorotationNormal = selectedModel.Triangles.CalcSelectedOrientationTrianglesNormal();

            float zAngle; float yAngle;
            VectorHelper.CalcRotationAnglesYZFromVector(autorotationNormal, false, out zAngle, out yAngle);

            //rotate model 
            if (!float.IsNaN(zAngle) && zAngle != 0)
            {
                selectedModel.Rotate(0, 0, selectedModel.RotationAngleZ - zAngle, RotationEventArgs.TypeAxis.Z);
                selectedModel.UpdateDefaultCenter();
            }

            if (!float.IsNaN(yAngle) && yAngle != 0)
            {
                selectedModel.Rotate(0, selectedModel.RotationAngleY - yAngle, 0, RotationEventArgs.TypeAxis.Y);
                selectedModel.UpdateDefaultCenter();
            }

            selectedModel.UpdateBoundries();
            selectedModel.MoveTranslationZ = 5f;
            selectedModel.LiftModelOnSupport();
            selectedModel.UpdateBinding();
            selectedModel.Triangles.UpdateSelectedOrientationTriangles(selectedModel);

            //   frmStudioMain.SceneControl.Enabled = false;
            Task.Run(() => { 
                try { Core.Engines.MagsAI.MagsAIEngine.Calculate(selectedModel, PrintJobManager.CurrentPrintJobSettings.Material, PrintJobManager.SelectedPrinter); }
            catch {
            } }).ContinueWith(s =>
            {
                frmStudioMain.SceneControl.EnableRendering();
                if (UserProfileManager.UserProfile.Settings_Use_Support_Basement)
                {
                    selectedModel.SupportBasement = true;
                }

                if (this.btnGenerateSupport.InvokeRequired)
                {
                    this.btnGenerateSupport.Invoke(new MethodInvoker(

                        delegate
                        {
                            this.btnGenerateSupport.Enabled = true;
                        }
                        ));
                }
                else
                {
                    this.btnGenerateSupport.Enabled = true;
                }
            }).ContinueWith(t => {
                MAGSAICompleted?.Invoke(null, null);
                }, CancellationToken.None, TaskContinuationOptions.None,
    TaskScheduler.FromCurrentSynchronizationContext());
                

            


        }

        private void plSupportClearAll_Click(object sender, EventArgs e)
        {
            btnSupportClearAll_Click(null, null);
        }

        private void btnSupportClearAll_Click(object sender, EventArgs e)
        {
            var selectedModel = ObjectView.SelectedModel;
            if (selectedModel != null)
            {
                selectedModel.ClearSupport();
                selectedModel.SupportBasement = false;

                frmStudioMain.SceneControl.Render();
            }
        }

        private void picSupportClearAll_Click(object sender, EventArgs e)
        {
            btnSupportClearAll_Click(null, null);
        }
    }
}
