using Atum.Studio.Controls.OpenGL;
using Atum.Studio.Core.Managers;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class MagsAIOrientationTabPanel : UserControl
    {
        internal enum TypeOfOrientation
        {
            None = 0,
            LayFlat = 1,
            SelectedTriangles = 2,
        }

        private OpenGL.SceneGLControl _glControl;
        internal OpenGL.SceneGLControl GLControl
        {
            get
            {
                return this._glControl;
            }
            set
            {
                this._glControl = value;
                this.plOpenGL.Controls.Add(this._glControl);
                this.toolStrip1.Visible = false;
            }
        }

        internal TypeOfOrientation OrientationType
        {
            get
            {
                if (rdLayFlatByTriangle.Checked)
                {
                    return TypeOfOrientation.LayFlat;
                }
                else if (rdExcludeModelParts.Checked)
                {
                    return TypeOfOrientation.SelectedTriangles;
                }
                else
                {
                    return TypeOfOrientation.None;
                }
            }
        }

        public MagsAIOrientationTabPanel()
        {
            InitializeComponent();
        }


        private void toolStripButtonColor1_Click(object sender, System.EventArgs e)
        {
            using (var colorDialog = new ColorDialog())
            {
                if (colorDialog.ShowDialog() == DialogResult.OK)
                {
                    Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Color_AsColor = colorDialog.Color;
                }
            }
        }

        internal void RemoveGLControl()
        {
            this.Controls.Clear();
        }

        private void trSelectionBoxSize_ValueChanged(object sender, System.EventArgs e)
        {
            Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Size = trSelectionBoxSize.Value;
            Core.ModelView.SceneView.SelectionBoxGimzo.Diameter = trSelectionBoxSize.Value;
            
                Core.ModelView.SceneView.SelectionBoxGimzo.UpdateSelectionBox(new Core.Events.SelectionBoxSizeEventArgs(
                    Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type),
                    this._glControl.Width,
                    this._glControl.Height,
                    new Point(this._glControl.Width / 2, this._glControl.Height / 2));

                this._glControl.Render();
        }

        private void rdExcludeModelParts_CheckedChanged(object sender, System.EventArgs e)
        {
            //Core.Managers.ToolStripActionsManager.Update(Core.Enums.MainFormToolStripActionType.btnAutoRotationSelectionPressed, this._glControl);
            this.toolStrip1.Visible = true;
        }

        private void rdLayFlatByTriangle_CheckedChanged(object sender, System.EventArgs e)
        {
            //Core.Managers.ToolStripActionsManager.Update(Core.Enums.MainFormToolStripActionType.btnLayFlatPressed, this._glControl);

            this.toolStrip1.Visible = false;
        }

        private void MagsAIOrientationTabPanel_Load(object sender, System.EventArgs e)
        {
            if (!DesignMode)
            {
                trSelectionBoxSize.TrackBar.Maximum = 100;
                trSelectionBoxSize.TrackBar.Minimum = 1;
                trSelectionBoxSize.TrackBar.TickFrequency = 10;

                this.toolStrip1.Left = (this._glControl.Width / 2) - (this.toolStrip1.Width / 2);

                trSelectionBoxSize.Value = Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Size;
                this.btnSelectionBoxRound.Checked = Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type == Core.Models.AutoRotationSelectionGizmo.TypeOfSelectionBox.Circle;
                this.btnSelectionBoxSquare.Checked = Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type == Core.Models.AutoRotationSelectionGizmo.TypeOfSelectionBox.Square;

                this.GLControl.Render();
            }
        }

        private void btnSelectionBoxRound_Click(object sender, System.EventArgs e)
        {
            Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type = Core.Models.AutoRotationSelectionGizmo.TypeOfSelectionBox.Circle;

            Core.ModelView.SceneView.SelectionBoxGimzo.UpdateSelectionBox(new Core.Events.SelectionBoxSizeEventArgs(
                Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type),
                this._glControl.Width,
                this._glControl.Height,
                new Point(this._glControl.Width / 2, this._glControl.Height / 2));

            this.btnSelectionBoxSquare.Checked = false;

            this._glControl.Render();
        }

        private void btnSelectionBoxSquare_Click(object sender, System.EventArgs e)
        {
            Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type = Core.Models.AutoRotationSelectionGizmo.TypeOfSelectionBox.Square;

            Core.ModelView.SceneView.SelectionBoxGimzo.UpdateSelectionBox(new Core.Events.SelectionBoxSizeEventArgs(
                Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Type),
                this._glControl.Width,
                this._glControl.Height,
                new Point(this._glControl.Width / 2, this._glControl.Height / 2));

            this.btnSelectionBoxRound.Checked = false;

            this._glControl.Render();
        }

        private void toolStripButtonColor1_Paint(object sender, PaintEventArgs e)
        {
            if (!DesignMode)
            {
                Rectangle bounds = new Rectangle(Point.Empty, toolStripButtonColor1.Size);
                e.Graphics.FillRectangle(new SolidBrush(this.BackColor), bounds);

                bounds = new Rectangle(new Point(4, 4), new Size(14, 14));
                e.Graphics.FillRectangle(new SolidBrush(Core.Managers.UserProfileManager.UserProfiles[0].Settings_Studio_SelectionBox_Color_AsColor), bounds);
            }
        }

        private void MagsAIOrientationTabPanel_Resize(object sender, System.EventArgs e)
        {
            if (this._glControl != null)
            {
                this.toolStrip1.Left = (this._glControl.Width / 2) - (this.toolStrip1.Width / 2);
                this._glControl.Render();
            }
        }

        private void rdOrbit_CheckedChanged(object sender, System.EventArgs e)
        {
            var cameraToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlCameraActionsToolbar>().First();
            ToolbarActionsManager.Update(Core.Enums.MainFormToolStripActionType.btnOrbitPressed, this._glControl, cameraToolbar.CameraActionOrbit);
        }

        private void rdPan_CheckedChanged(object sender, System.EventArgs e)
        {
            var cameraToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlCameraActionsToolbar>().First();
            Core.Managers.ToolbarActionsManager.Update(Core.Enums.MainFormToolStripActionType.btnPanPressed, this._glControl, cameraToolbar.CameraActionPan);
        }

        private void rdZoom_CheckedChanged(object sender, System.EventArgs e)
        {
            var cameraToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlCameraActionsToolbar>().First();
            Core.Managers.ToolbarActionsManager.Update(Core.Enums.MainFormToolStripActionType.btnZoomPressed, this._glControl, cameraToolbar.CameraActionZoom);
        }
    }
}
