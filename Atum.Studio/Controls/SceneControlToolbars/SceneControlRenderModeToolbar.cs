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

namespace Atum.Studio.Controls.OpenGL
{
    internal partial class SceneControlRenderModeToolbar : SceneControlToolbars.SceneControlToolbarBase
    {

        private SceneControlToolTips.SceneControlToolbarToolTipLeftSide _toolTipModelRenderMode;
        private SceneControlToolTips.SceneControlToolbarToolTipLeftSide _toolTipSupportRenderMode;
        private SceneControlToolTips.SceneControlToolbarToolTipLeftSide _toolTipGroundPaneRenderMode;

        internal SceneControlRenderModeToolbar()
        {
            InitializeComponent();

            this.btnModelRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.model_solid, btnModelRenderMode.Size);
            this.btnSupportRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.support_solid, btnModelRenderMode.Size);
            this.btnGroundPaneRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.grid_solid, btnModelRenderMode.Size);

            this.Visible = true;
        }

        private void SceneControlRenderModeToolbar_Load(object sender, EventArgs e)
        {
            this.Top = 120;
            this.Left = frmStudioMain.SceneControl.Width - this.Width - 28 - 16;

            this._toolTipModelRenderMode = new SceneControlToolTips.SceneControlToolbarToolTipLeftSide(this, this.btnModelRenderMode);
            this._toolTipModelRenderMode.SetToolTip(this.btnModelRenderMode, "empty");
            this._toolTipModelRenderMode.Text = "Model Rendering";

            this._toolTipSupportRenderMode = new SceneControlToolTips.SceneControlToolbarToolTipLeftSide(this, this.btnSupportRenderMode);
            this._toolTipSupportRenderMode.SetToolTip(this.btnSupportRenderMode, "empty");
            this._toolTipSupportRenderMode.Text = "Support Rendering";

            this._toolTipGroundPaneRenderMode = new SceneControlToolTips.SceneControlToolbarToolTipLeftSide(this, this.btnGroundPaneRenderMode);
            this._toolTipGroundPaneRenderMode.SetToolTip(this.btnGroundPaneRenderMode, "empty");
            this._toolTipGroundPaneRenderMode.Text = "Platform Rendering";

        }

        private void btnModelRenderMode_Click(object sender, EventArgs e)
        {
            switch (SceneView.ModelRenderMode)
            {
                case SceneView.SceneViewRenderModeType.Solid:
                    SceneView.ModelRenderMode = SceneView.SceneViewRenderModeType.Wireframe;
                    this.btnModelRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.model_wireframe, btnModelRenderMode.Size);
                    break;
                case SceneView.SceneViewRenderModeType.Wireframe:
                    SceneView.ModelRenderMode = SceneView.SceneViewRenderModeType.Hidden;
                    this.btnModelRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.model_hide, btnModelRenderMode.Size);
                    break;
                case SceneView.SceneViewRenderModeType.Hidden:
                    SceneView.ModelRenderMode = SceneView.SceneViewRenderModeType.Solid;
                    this.btnModelRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.model_solid, btnModelRenderMode.Size);
                    break;
            }

            frmStudioMain.SceneControl.Render();
        }

        private void btnSupportRenderMode_Click(object sender, EventArgs e)
        {
            switch (SceneView.SupportRenderMode)
            {
                case SceneView.SceneViewRenderModeType.Solid:
                    SceneView.SupportRenderMode = SceneView.SceneViewRenderModeType.Wireframe;
                    this.btnSupportRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.support_wireframes, btnSupportRenderMode.Size);
                    break;
                case SceneView.SceneViewRenderModeType.Wireframe:
                    SceneView.SupportRenderMode = SceneView.SceneViewRenderModeType.Hidden;
                    this.btnSupportRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.support_hide, btnSupportRenderMode.Size);
                    break;
                case SceneView.SceneViewRenderModeType.Hidden:
                    SceneView.SupportRenderMode = SceneView.SceneViewRenderModeType.Solid;
                    this.btnSupportRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.support_solid, btnSupportRenderMode.Size);
                    break;
            }

            frmStudioMain.SceneControl.Render();

        }

        private void btnGroundPaneRenderMode_Click(object sender, EventArgs e)
        {
            switch (SceneView.GroundPaneRenderMode)
            {
                case SceneView.SceneViewRenderModeType.Solid:
                    SceneView.GroundPaneRenderMode = SceneView.SceneViewRenderModeType.Hidden;
                    this.btnGroundPaneRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.grid_hide, btnGroundPaneRenderMode.Size);
                    break;
                case SceneView.SceneViewRenderModeType.Hidden:
                    SceneView.GroundPaneRenderMode = SceneView.SceneViewRenderModeType.Solid;
                    this.btnGroundPaneRenderMode.Image = SVGParser.GetBitmapFromSVG(Resources.SVGImages.grid_solid, btnGroundPaneRenderMode.Size);
                    break;
            }

            frmStudioMain.SceneControl.Render();
        }

    
    }      
}
