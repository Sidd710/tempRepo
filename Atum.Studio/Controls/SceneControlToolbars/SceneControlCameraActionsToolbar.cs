using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using Atum.Studio.Controls.NewGui;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Managers;
using static Atum.Studio.Core.Enums;

namespace Atum.Studio.Controls.OpenGL
{
    internal partial class SceneControlCameraActionsToolbar : SceneControlToolbars.SceneControlToolbarBase
    {
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _toolTipCameraActionPan;
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _toolTipCameraActionZoom;
        private SceneControlToolTips.SceneControlToolbarToolTipRightSide _toolTipCameraActionOrbit;

        internal PictureBox CameraActionPan
        {
            get
            {
                return this.btnCameraActionPan;
            }
        }

        internal PictureBox CameraActionZoom
        {
            get
            {
                return this.btnCameraActionZoom;
            }
        }

        internal PictureBox CameraActionOrbit
        {
            get
            {
                return this.btnCameraActionOrbit;
            }
        }


        public SceneControlCameraActionsToolbar()
        {
            InitializeComponent();

            this.Visible = true;

            this.btnCameraActionPan.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.pan_white_unselected, this.btnCameraActionPan.Size);
            this.btnCameraActionOrbit.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.orbit_white_unselected, this.btnCameraActionOrbit.Size);
            this.btnCameraActionZoom.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.zoom_white_unselected, this.btnCameraActionZoom.Size);

            this.Top = 364;
            this.Left = 16;

        }

        private void SceneControlRenderModeToolbar_Load(object sender, EventArgs e)
        {



            this._toolTipCameraActionPan = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnCameraActionPan);
            this._toolTipCameraActionPan.SetToolTip(this.btnCameraActionPan, "empty");
            this._toolTipCameraActionPan.Text = "Pan (Shortcut key: P)";

            this._toolTipCameraActionZoom = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnCameraActionZoom);
            this._toolTipCameraActionZoom.SetToolTip(this.btnCameraActionZoom, "empty");
            this._toolTipCameraActionZoom.Text = "Zoom (Shortcut key: Z)";

            this._toolTipCameraActionOrbit = new SceneControlToolTips.SceneControlToolbarToolTipRightSide(this, this.btnCameraActionOrbit);
            this._toolTipCameraActionOrbit.SetToolTip(this.btnCameraActionOrbit, "empty");
            this._toolTipCameraActionOrbit.Text = "Orbit (Shortcut key: O)";
        }

        private void btnCameraActionPan_Click(object sender, EventArgs e)
        {
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnPanPressed, frmStudioMain.SceneControl, this.CameraActionPan);
        }

        private void btnCameraActionZoom_Click(object sender, EventArgs e)
        {
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnZoomPressed, frmStudioMain.SceneControl, this.CameraActionZoom);
        }

        private void btnCameraActionOrbit_Click(object sender, EventArgs e)
        {
            ToolbarActionsManager.Update(MainFormToolStripActionType.btnOrbitPressed, frmStudioMain.SceneControl, this.CameraActionOrbit);
        }
    }      
}
