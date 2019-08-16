using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Engines.PackingEngine;
using Atum.Studio.Core.Events;
using System.IO;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.Studio.Controls.SceneControlToolTips;
using Atum.Studio.Core.Structs;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlModelRotate : SceneControlActionPanelBase
    {
       
        public SceneControlModelRotate()
        {
            InitializeComponent();

            this.HeaderText = "Rotate Model";
            this.txtMovementX.EnterPressed += TxtMovementX_ValueChanged;
            this.txtMovementY.EnterPressed += TxtMovementY_ValueChanged;
            this.txtMovementZ.EnterPressed += TxtMovementZ_ValueChanged;

            this.txtMovementX.EnableDelayedValueChanged();
        }


        internal float RotateX
        {
            set
            {
                this.txtMovementX.DisableTextChangeTrigger();
                var rotationAngle = value;
                if (value > 360)
                {
                    rotationAngle = (value - 360);
                }
                else if (value < 0)
                {
                    rotationAngle = (value + 360);
                }
                this.txtMovementX.Value = rotationAngle;
                this.txtMovementY.EnableTextChangeTrigger();
            }
        }


        internal float RotateY
        {
            set
            {
                this.txtMovementY.DisableTextChangeTrigger();
                var rotationAngle = value;
                if (value > 360)
                {
                    rotationAngle = (value - 360);
                }
                else if (value < 0)
                {
                    rotationAngle = (value + 360);
                }
                this.txtMovementY.Value = rotationAngle;
                this.txtMovementY.EnableTextChangeTrigger();
            }
        }


        internal float RotateZ
        {
            set
            {
                this.txtMovementZ.DisableTextChangeTrigger();
                var rotationAngle = value;
                if (value > 360)
                {
                    rotationAngle = (value - 360);
                }
                else if (value < 0)
                {
                    rotationAngle = (value + 360);
                }
                this.txtMovementZ.Value = rotationAngle;
                this.txtMovementZ.EnableTextChangeTrigger();
            }
        }

        private void SceneControlModelRotate_Load(object sender, EventArgs e)
        {
            var font = FontManager.Montserrat14Regular;

            if (FontManager.Loaded)
            {
                this.lblInformation.Font = font;
            }

            this.txtMovementX.HideSuffixLabel();
            this.txtMovementY.HideSuffixLabel();
            this.txtMovementZ.HideSuffixLabel();
        }

        private void TxtMovementX_ValueChanged(object sender, float e)
        {
            var selectedModel = ObjectView.SelectedModel;
            selectedModel.RotationAngleX = e;
            SceneView.Rotation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedRotationAxisType.X);

            frmStudioMain.SceneControl.Render();
        }

        private void TxtMovementY_ValueChanged(object sender, float e)
        {
            var selectedModel = ObjectView.SelectedModel;

            selectedModel.RotationAngleY = e;

            SceneView.Rotation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedRotationAxisType.Y);
            SceneView.MoveTranslation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);

            frmStudioMain.SceneControl.Render();
        }

        private void TxtMovementZ_ValueChanged(object sender, float e)
        {
            var selectedModel = ObjectView.SelectedModel;
            selectedModel.RotationAngleZ = e;
            SceneView.Rotation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedRotationAxisType.Z);

            frmStudioMain.SceneControl.Render();
        }


    }
}
