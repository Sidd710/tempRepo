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
    public partial class SceneControlModelMove : SceneControlActionPanelBase
    {
       

        public SceneControlModelMove()
        {
            InitializeComponent();

            this.HeaderText = "Move Model";
            this.txtMovementX.ValueChanged += TxtMovementX_ValueChanged;
            this.txtMovementY.ValueChanged += TxtMovementY_ValueChanged;
            this.txtMovementZ.ValueChanged += TxtMovementZ_ValueChanged;
        }

       
        internal Vector3Class DataSource
        {
            set
            {
                this.txtMovementX.DisableTextChangeTrigger();
                this.txtMovementX.Value = (float)Math.Round(value.X, 2);
                this.txtMovementX.EnableTextChangeTrigger();

                this.txtMovementY.DisableTextChangeTrigger();
                this.txtMovementY.Value = (float)Math.Round(value.Y, 2);
                this.txtMovementY.EnableTextChangeTrigger();

                this.txtMovementZ.DisableTextChangeTrigger();
                this.txtMovementZ.Value = (float)Math.Round(value.Z, 2);
                this.txtMovementZ.EnableTextChangeTrigger();
                this.txtMovementZ.EnableDelayedValueChanged();
            }
        }

        private void TxtMovementX_ValueChanged(object sender, float e)
        {
            var selectedModel = ObjectView.SelectedModel;
            var selectedLinkedClone = selectedModel.LinkedClones.FirstOrDefault(s => s.Selected);
            if (selectedLinkedClone != null)
            {
                selectedLinkedClone.Translation = new Vector3Class(e, selectedLinkedClone.Translation.Y, selectedLinkedClone.Translation.Z);
            }
            else
            {
                selectedModel.MoveTranslationX = e;
            }

            SceneView.MoveTranslation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
            frmStudioMain.SceneControl.Render();
        }


        private void TxtMovementY_ValueChanged(object sender, float e)
        {
            var selectedModel = ObjectView.SelectedModel;
            var selectedLinkedClone = selectedModel.LinkedClones.FirstOrDefault(s => s.Selected);
            if (selectedLinkedClone != null)
            {
                selectedLinkedClone.Translation = new Vector3Class(selectedLinkedClone.Translation.X, e, selectedLinkedClone.Translation.Z);
            }
            else
            {
                selectedModel.MoveTranslationY = e;
            }
            SceneView.MoveTranslation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
            frmStudioMain.SceneControl.Render();
        }

        private void TxtMovementZ_ValueChanged(object sender, float e)
        {

            var selectedModel = ObjectView.SelectedModel;
            if ((float)Math.Round(selectedModel.BottomPoint,2) != e)
            {
                var bottomPointDifference = e - selectedModel.BottomPoint;
                selectedModel.MoveTranslationZ = e;
                selectedModel.Triangles.UpdateWithMoveTranslation(new Vector3Class(0, 0, bottomPointDifference));
                selectedModel.UpdateBoundries();
                selectedModel.UpdateBinding();

                foreach(var supportCone in selectedModel.TotalObjectSupportCones)
                {
                    if (supportCone != null)
                    {
                        supportCone.UpdateHeight(bottomPointDifference);
                    }
                }

                //update gizmo
                SceneView.MoveTranslation3DGizmo.UpdateControl(Core.Enums.SceneViewSelectedMoveTranslationAxisType.NoAxisSelected, true);
                frmStudioMain.SceneControl.Render();
            }
        }


        private void SceneControlModelMove_Load(object sender, EventArgs e)
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

        
    }
}
