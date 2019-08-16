using System;
using System.Drawing;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core;
using System.Windows.Forms;
using System.Linq;

namespace Atum.Studio.Controls.OpenGL
{
    internal partial class SceneControlModelActionsToolbar : SceneControlToolbars.SceneControlToolbarBase
    {
        //internal event EventHandler MoveSelectedModelContainsLinkedClones;
        internal event EventHandler RotationSelectedModelContainsLinkedClones;
        internal event EventHandler ScaleSelectedModelContainsLinkedClones;
        internal event EventHandler MAGSAINoModelSelected;

        private Color defaultButtonBackgroundColor = Color.WhiteSmoke;
        private Color selectedButtonBackgroundColor = Color.FromArgb(255, 230, 230, 230);

        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _tooltipModelActionMove;
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _tooltipModelActionRotate;
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _tooltipModelActionScale;
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _tooltipModelActionSupport;
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _tooltipModelActionDuplicate;

        internal PictureBox ModelMoveButton
        {
            get
            {
                return this.btnModelActionMove;
            }
        }

        internal PictureBox ModelRotateButton
        {
            get
            {
                return this.btnModelActionRotate;
            }
        }

        internal PictureBox ModelScaleButton
        {
            get
            {
                return this.btnModelActionScale;
            }
        }

        internal PictureBox ModelSupportButton
        {
            get
            {
                return this.btnModelActionSupport;
            }
        }

        internal PictureBox ModelDuplicateButton
        {
            get
            {
                return this.btnModelActionDuplicate;
            }
        }

        public SceneControlModelActionsToolbar()
        {
            InitializeComponent();

            this.btnModelActionMove.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.move_black, new Size(40,40));
            this.btnModelActionRotate.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.rotate_black, new Size(40, 40));
            this.btnModelActionScale.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.scalepanel_black, new Size(40, 40));
            this.btnModelActionSupport.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.support_black, new Size(40, 40));

            this.Left = 16;
            this.Top = 32;
            this.Visible = true;
        }

        private void btnModelActionMove_Click(object sender, EventArgs e)
        {
            if (this.btnModelActionMove.BackColor == this.defaultButtonBackgroundColor) //not selected state
            {

                var selectedModel = ObjectView.SelectedModel;
                if (selectedModel != null)
                {
                    //if (selectedModel.LinkedClones.Count > 1)
                    //{
                    //    MoveSelectedModelContainsLinkedClones?.Invoke(null, null);
                    //    ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
                    //    SceneView.CurrentViewMode = SceneView.ViewMode.MoveTranslation;
                    //}
                    //else
                    //{
                        this.ResetBackgroundColors();
                        ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnMovePressed, frmStudioMain.SceneControl, this.btnModelActionMove);
                        UpdateSelectedButtonBackground(this.btnModelActionMove);
                   // }
                }
            }
            else
            {
                DeselectButtons();

                ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
            }
        }

        internal void SelectMAGSAIButton()
        {
            this.btnModelActionSupport_Click(true, null);
        }

        internal void DeselectButtons()
        {
            this.ResetBackgroundColors();
        }

        private void ResetActionPanels()
        {
            ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, this.btnModelActionRotate);
        }

        private void btnModelActionRotate_Click(object sender, EventArgs e)
        {
            if (this.btnModelActionRotate.BackColor == this.defaultButtonBackgroundColor) //not selected state
            {
                this.ResetBackgroundColors();
                var selectedModel = ObjectView.SelectedModel;
                if (selectedModel != null)
                {
                    if (selectedModel.LinkedClones.Count > 1)
                    {
                        RotationSelectedModelContainsLinkedClones?.Invoke(null, null);
                        ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
                        SceneView.CurrentViewMode = SceneView.ViewMode.ModelRotation;
                    }
                    else
                    {
                        ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnRotatePressed, frmStudioMain.SceneControl, this.btnModelActionRotate);
                    }
                    UpdateSelectedButtonBackground(this.btnModelActionRotate);
                }
            }
            else
            {
                DeselectButtons();

                ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
            }
        }

        private void btnModelActionScale_Click(object sender, EventArgs e)
        {
                if (this.btnModelActionScale.BackColor == this.defaultButtonBackgroundColor) //not selected state
                {
                    this.ResetBackgroundColors();
                    var selectedModel = ObjectView.SelectedModel;
                    if (selectedModel != null)
                    {
                        if (selectedModel.LinkedClones.Count > 1)
                        {
                            ScaleSelectedModelContainsLinkedClones?.Invoke(null, null);
                            ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
                            SceneView.CurrentViewMode = SceneView.ViewMode.ModelScale;
                        }
                        else
                        {
                            ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnScalePressed, frmStudioMain.SceneControl, this.btnModelActionScale);
                        }
                        UpdateSelectedButtonBackground(this.btnModelActionScale);
                    }
                }
                else
                {
                    DeselectButtons();

                    ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
                }
        }

        private void btnModelActionSupport_Click(object sender, EventArgs e)
        {
            var forceMAGSAIInit = this.btnModelActionSupport.BackColor == this.defaultButtonBackgroundColor;
            if (sender != null && sender is bool)
            {
                forceMAGSAIInit = (bool)sender;
            }
            if (forceMAGSAIInit) //not selected state
            {
                var selectedModel = ObjectView.SelectedModel;
                if (selectedModel != null)
                {
                        this.ResetBackgroundColors();
                        ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnModelActionMagsAI, frmStudioMain.SceneControl, this.btnModelActionSupport);
                        UpdateSelectedButtonBackground(this.btnModelActionSupport);
                }
                else
                {
                ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
                MAGSAINoModelSelected?.Invoke(null, null);

                }
            }
            else
            {
                DeselectButtons();

                ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
            }
        }

        private void btnModelActionManualSupport_Click(object sender, EventArgs e)
        {
            if (this.btnModelActionSupport.BackColor == this.defaultButtonBackgroundColor) //not selected state
            {
                if (ObjectView.SelectedModel != null)
                {
                    this.ResetBackgroundColors();
                    ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnModelActionMagsAI, frmStudioMain.SceneControl, this.btnModelActionSupport);
                    UpdateSelectedButtonBackground(this.btnModelActionSupport);
                }
            }
        }

        private void btnModelActionGridSupport_Click(object sender, EventArgs e)
        {
            if (this.btnModelActionSupport.BackColor == this.defaultButtonBackgroundColor) //not selected state
            {
                if (ObjectView.SelectedModel != null)
                {
                    this.ResetBackgroundColors();
                    ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnModelActionMagsAI, frmStudioMain.SceneControl, this.btnModelActionSupport);
                    UpdateSelectedButtonBackground(this.btnModelActionSupport);
                }
            }
        }

        private void btnModelActionDuplicate_Click(object sender, EventArgs e)
        {
            if (this.btnModelActionDuplicate.BackColor == this.defaultButtonBackgroundColor) //not selected state
            {
                if (ObjectView.SelectedModel != null)
                {
                    this.ResetBackgroundColors();
                    ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnModelActionDuplicate, frmStudioMain.SceneControl, this.btnModelActionDuplicate);
                    UpdateSelectedButtonBackground(this.btnModelActionDuplicate);
                }
            }
            else
            {
                DeselectButtons();

                ToolbarActionsManager.Update(Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
            }
        }

        private void UpdateSelectedButtonBackground(PictureBox selectedButton)
        {
            selectedButton.BackColor = this.selectedButtonBackgroundColor;
        }

        internal void ResetBackgroundColors()
        {
            foreach (var pictureBox in this.Controls.OfType<PictureBox>())
            {
                pictureBox.BackColor = this.defaultButtonBackgroundColor;
            }
        }

        internal void SelectButton(PictureBox button)
        {
            var buttonName = button.Name.ToLower();
            if (buttonName.Contains("move"))
            {
                this.btnModelActionMove_Click(null, null);
            }
            else if (buttonName.Contains("rotate"))
            {
                this.btnModelActionRotate_Click(null, null);
            }
            else if (buttonName.Contains("scale"))
            {
                this.btnModelActionScale_Click(null, null);
            }
            else if (buttonName.Contains("manualsupport"))
            {
                this.btnModelActionGridSupport_Click(null, null);
            }
            else if (buttonName.Contains("gridsupport"))
            {
                this.btnModelActionGridSupport_Click(null, null);
            }
            else if (buttonName.Contains("support"))
            {
                this.btnModelActionSupport_Click(null, null);
            }
            else if (buttonName.Contains("duplicate"))
            {
                this.btnModelActionDuplicate_Click(null, null);
            }
        }

        private void SceneControlModelActionsToolbar_Load(object sender, EventArgs e)
        {
            this._tooltipModelActionMove = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnModelActionMove, 10);
            this._tooltipModelActionMove.SetToolTip(this.btnModelActionMove, "empty");
            this._tooltipModelActionMove.Text = "Move model (Shortcut key: M)  ";

            this._tooltipModelActionRotate = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnModelActionRotate, 10);
            this._tooltipModelActionRotate.SetToolTip(this.btnModelActionRotate, "empty");
            this._tooltipModelActionRotate.Text = "Rotate model (Shortcut key: R)  ";

            this._tooltipModelActionScale = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnModelActionScale, 10);
            this._tooltipModelActionScale.SetToolTip(this.btnModelActionScale, "empty");
            this._tooltipModelActionScale.Text = "Scale model (Shortcut key: X)  ";

            this._tooltipModelActionSupport = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnModelActionSupport, 10);
            this._tooltipModelActionSupport.SetToolTip(this.btnModelActionSupport, "empty");
            this._tooltipModelActionSupport.Text = "Support model (Shortcut key: F)  ";

            this._tooltipModelActionDuplicate = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnModelActionDuplicate, 10);
            this._tooltipModelActionDuplicate.SetToolTip(this.btnModelActionDuplicate, "empty");
            this._tooltipModelActionDuplicate.Text = "Duplicate model (Shortcut key: D)   ";
        }


    }
}
