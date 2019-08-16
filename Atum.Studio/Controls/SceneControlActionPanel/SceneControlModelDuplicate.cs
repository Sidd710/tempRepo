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

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlModelDuplicate : SceneControlActionPanelBase
    {
        internal event EventHandler FillBuildPlateCompleted;

        private readonly int defaultItemHeight = 60;
        internal event EventHandler<CloneModelsEventArgs> ValueChanged;
        //internal int _maxClones;
        PackModelsRequest _packModelsRequest;

        public SceneControlModelDuplicate()
        {
            InitializeComponent();

            this.HeaderText = "Duplicate Model";
            this.btnDuplicatesFillPlate.BackColor = BrandingManager.Button_BackColor_LightGray;
            this.btnDuplicatesFillPlate.ForeColor = BrandingManager.Button_ForeColor_Dark;
            this.btnDuplicatesFillPlate.BorderColor = BrandingManager.Button_ForeColor_Dark;
        }

        private void SceneControlModelDuplicate_Load(object sender, EventArgs e)
        {
            var font = FontManager.Montserrat14Regular;
            this.lblErrorMessage.Font = font;

            this.btnDuplicatesFillPlate.Font = FontManager.Montserrat14Regular;

            if (ObjectView.Objects3D.Count > 1)
            {
                this._packModelsRequest = PackingHelper.CreatePackModelsRequest(null);
            }
        }

        private void DuplicateModelControl_DuplicatesPlus_Click(object sender, EventArgs e)
        {
            var duplicateModelControl = (DuplicateModelControl)sender;
            var amount = duplicateModelControl.TotalAmount + 1;
            var maxClonesInBestSolution = CalcMaxClonesOfModel(duplicateModelControl.ModelFootprint, false);
            duplicateModelControl.MaxClones = maxClonesInBestSolution;

            if (maxClonesInBestSolution >= amount)
            {
                duplicateModelControl.TotalAmount = amount;
                UpdateDuplicates(duplicateModelControl.TotalAmount - 1, amount, duplicateModelControl.ModelFootprint);
                lblErrorMessage.Visible = false;
            }
            else
            {
                lblErrorMessage.Visible = true;
            }
        }

        private void DuplicateModelControl_DuplicatesMinus_Click(object sender, EventArgs e)
        {
            var duplicateModelControl = (DuplicateModelControl)sender;
            var amount = duplicateModelControl.TotalAmount - 1;
            var maxClonesInBestSolution = CalcMaxClonesOfModel(duplicateModelControl.ModelFootprint, true);
            duplicateModelControl.MaxClones = maxClonesInBestSolution;

            if (maxClonesInBestSolution >= amount)
            {
                duplicateModelControl.TotalAmount = amount;
                UpdateDuplicates(duplicateModelControl.TotalAmount + 1, amount, duplicateModelControl.ModelFootprint);
                lblErrorMessage.Visible = false;
            }
            else
            {
                lblErrorMessage.Visible = true;
            }
        }

        private int CalcMaxClonesOfModel(ModelFootprint modelFootprint, bool minModelCount)
        {
            var tmpPackModelsRequest = PackingHelper.CreatePackModelsRequest(null);
            foreach (var duplicateControl in this.spcModelControls.Panel1.Controls.OfType<DuplicateModelControl>())
            {
                if (duplicateControl.ModelFootprint.Model != modelFootprint.Model)
                {
                    tmpPackModelsRequest.ModelFootprints.First(s => s.Model == duplicateControl.ModelFootprint.Model).RequestedCloneCount = duplicateControl.TotalAmount;
                }
                else if (minModelCount)
                {
                    tmpPackModelsRequest.ModelFootprints.First(s => s.Model == duplicateControl.ModelFootprint.Model).RequestedCloneCount = duplicateControl.TotalAmount;
                }
            }

            var solutions = PackingHelper.CalculatePackingSolutions(tmpPackModelsRequest);

            if (solutions.BestSolution != null)
            {
                return solutions.BestSolution.PackedItems.Count(s => s.ModelFootprint.Model == modelFootprint.Model);
            }
            else
            {
                return -1; //invalid or unable to calc more models
            }

        }


        private void DuplicateModelControl_DuplicatesCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            var duplicateModelControl = (DuplicateModelControl)sender;
            var newValue = duplicateModelControl.TotalAmount;

            duplicateModelControl.TotalAmount = newValue;
            var maxClonesInBestSolution = CalcMaxClonesOfModel(duplicateModelControl.ModelFootprint, true);
            duplicateModelControl.MaxClones = maxClonesInBestSolution;

            if (maxClonesInBestSolution >= newValue)
            {
                duplicateModelControl.TotalAmount = newValue;
                UpdateDuplicates(duplicateModelControl.TotalAmount, newValue, duplicateModelControl.ModelFootprint);
                lblErrorMessage.Visible = false;
            }
            else
            {
                maxClonesInBestSolution = CalcMaxClonesOfModel(duplicateModelControl.ModelFootprint, false);
                duplicateModelControl.TotalAmount = maxClonesInBestSolution;
                UpdateDuplicates(maxClonesInBestSolution, maxClonesInBestSolution, duplicateModelControl.ModelFootprint);
                lblErrorMessage.Visible = true;
            }

        }
        private void UpdateDuplicates(int previousAmount, int amount, ModelFootprint modelFootprint)
        {
            //ModelFootprint modelFootprint = this.Tag as ModelFootprint;
            if (modelFootprint != null)
            {
                var tmpPackModelsRequest = PackingHelper.CreatePackModelsRequest(null);
                foreach (var duplicateControl in this.spcModelControls.Panel1.Controls.OfType<DuplicateModelControl>())
                {
                    if (duplicateControl.ModelFootprint.Model != modelFootprint.Model)
                    {
                        tmpPackModelsRequest.ModelFootprints.First(s => s.Model == duplicateControl.ModelFootprint.Model).RequestedCloneCount = duplicateControl.TotalAmount;
                    }
                    else
                    {
                        tmpPackModelsRequest.ModelFootprints.First(s => s.Model == duplicateControl.ModelFootprint.Model).RequestedCloneCount = amount;
                    }
                }

                var solutions = PackingHelper.CalculatePackingSolutions(tmpPackModelsRequest);
                var amountOfPackagedItems = solutions.BestSolution.PackedItems.Count(s => s.ModelFootprint.Model == modelFootprint.Model);
                if (amountOfPackagedItems < amount)
                {
                    //workaround to fix bug somewhere in packaging logic
                    tmpPackModelsRequest = PackingHelper.CreatePackModelsRequest(null);
                    foreach (var duplicateControl in this.spcModelControls.Panel1.Controls.OfType<DuplicateModelControl>())
                    {
                        if (duplicateControl.ModelFootprint.Model != modelFootprint.Model)
                        {
                            tmpPackModelsRequest.ModelFootprints.First(s => s.Model == duplicateControl.ModelFootprint.Model).RequestedCloneCount = duplicateControl.TotalAmount;
                        }
                    }
                    tmpPackModelsRequest = PackingHelper.CreatePackModelsRequest(null);
                    foreach (var duplicateControl in this.spcModelControls.Panel1.Controls.OfType<DuplicateModelControl>())
                    {
                        if (duplicateControl.ModelFootprint.Model != modelFootprint.Model)
                        {
                            tmpPackModelsRequest.ModelFootprints.First(s => s.Model == duplicateControl.ModelFootprint.Model).RequestedCloneCount = duplicateControl.TotalAmount;
                        }
                    }

                    solutions = PackingHelper.CalculatePackingSolutions(tmpPackModelsRequest);
                }

                Dictionary<ModelFootprint, int> modelFootprints = new Dictionary<ModelFootprint, int>();
                modelFootprints.Add(modelFootprint, previousAmount);

                var cloneModelsEventArgs = new CloneModelsEventArgs()
                {
                    PackModelsRequest = tmpPackModelsRequest,
                    PackagingSolutions = solutions,
                    ModelFootprints = modelFootprints
                };

                this.ValueChanged?.Invoke(null, cloneModelsEventArgs);
            }


        }

        private void txtDuplicatesCount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }


        private void btnDuplicatesFillPlate_Click(object sender, EventArgs e)
        {
            SetMaximumClones();
        }

        internal void Init()
        {
            RefreshCurrentDuplicates();
        }

        private void TriggerValueChanged()
        {
            if (ObjectView.Objects3D.Count > 1)
            {

            }
        }

        internal void RefreshCurrentDuplicates()
        {
            if (ObjectView.Objects3D.Count > 1)
            {
                _packModelsRequest = PackingHelper.CreatePackModelsRequest(null);
                var solutions = PackingHelper.CalculatePackingSolutions(_packModelsRequest);
                this.spcModelControls.Panel1.Controls.Clear();
                int i = 0;
                var summaryHeight = 0;
                this.Height = 215;
                foreach (var model in ObjectView.Objects3D)
                {
                    if (model != null && (!(model is GroundPane)))
                    {
                        var stlModel = model as STLModel3D;

                        ModelFootprint modelFootprint = null;
                        if (solutions.BestSolution == null)
                        {
                            modelFootprint = new ModelFootprint(stlModel.RightPoint - stlModel.LeftPoint, stlModel.FrontPoint - stlModel.BackPoint);
                            modelFootprint.Model = stlModel;
                        }
                        else
                        {
                            modelFootprint = solutions.BestSolution.Footprints.First(f => f.Model == stlModel);
                        }
                        
                        if (i > 0)
                        {
                            this.Height += defaultItemHeight;
                        }

                        DuplicateModelControl duplicateModelControl = new DuplicateModelControl(modelFootprint);
                        if (i % 2 != 0)
                        {
                            duplicateModelControl.BackgroundColor = Color.FromArgb(237, 237, 237);
                        }
                        else
                        {
                            duplicateModelControl.BackgroundColor = Color.FromArgb(224, 224, 224);
                        }

                        duplicateModelControl.Left = 0;
                        duplicateModelControl.Top = summaryHeight;
                        duplicateModelControl.DuplicatesCount_KeyUp += DuplicateModelControl_DuplicatesCount_KeyPress;
                        duplicateModelControl.DuplicatesMinus_Click += DuplicateModelControl_DuplicatesMinus_Click;
                        duplicateModelControl.DuplicatesPlus_Click += DuplicateModelControl_DuplicatesPlus_Click;
                        this.spcModelControls.Panel1.Controls.Add(duplicateModelControl);
                        summaryHeight += defaultItemHeight;
                        i++;
                    }
                }
            }
            else
            {
                this.Visible = false;

                //show message
                var tt = new SceneControlManualToolTip();
                tt.SetText("No models available");

                var modelActionsToolbar = SceneControlToolbarManager.ModelActionsToolbar;
                var btnDuplicationButton = modelActionsToolbar.ModelDuplicateButton;
                var duplicationButtonPoint = new Point(btnDuplicationButton.Left + btnDuplicationButton.Width + modelActionsToolbar.Left + 16, btnDuplicationButton.Top + modelActionsToolbar.Top);
                duplicationButtonPoint.Y += (btnDuplicationButton.Height / 2) - (tt.Height / 2);
                tt.ShowToolTip(duplicationButtonPoint);

                ToolbarActionsManager.Update(Core.Enums.MainFormToolStripActionType.btnSelectPressed, frmStudioMain.SceneControl, null);
            }
        }

        private void DuplicateModelControl_DuplicateMaxReached_Click(object sender, EventArgs e)
        {
            lblErrorMessage.Visible = true;
        }

        private void SetMaximumClones()
        {
            if (ObjectView.Objects3D.Count > 1)
            {
                _packModelsRequest = PackingHelper.CreatePackModelsRequest(null);
                _packModelsRequest.PackingType = TypeOfPacking.MaxAmountOfClones;
                var solutions = PackingHelper.CalculatePackingSolutions(_packModelsRequest);

                Dictionary<ModelFootprint, int> modelFootprints = new Dictionary<ModelFootprint, int>();

                foreach (var footprint in solutions.BestSolution.Footprints)
                {
                    var duplicateControl = this.spcModelControls.Panel1.Controls.OfType<DuplicateModelControl>().First(s => s.ModelFootprint.Model == footprint.Model);
                    modelFootprints.Add(footprint, duplicateControl.TotalAmount);
                    duplicateControl.TotalAmount = footprint.CloneCount;
                }


                var cloneModelsEventArtgs = new CloneModelsEventArgs()
                {
                    PackModelsRequest = _packModelsRequest,
                    PackagingSolutions = solutions,
                    ModelFootprints = modelFootprints
                };
                this.ValueChanged?.Invoke(null, cloneModelsEventArtgs);
            }
            else
            {
                this.Visible = false;
            }

            //
            FillBuildPlateCompleted?.Invoke(null, null);
        }
    }
}
