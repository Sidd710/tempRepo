using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.DAL.Materials;
using System.Linq;
using Atum.Studio.Core.Managers;
using Atum.Studio.Controls.NewGui.MaterialCatalogManager;
using Atum.DAL.ApplicationSettings;
using System.IO;
using System.Drawing.Drawing2D;

namespace Atum.Studio.Controls.MaterialEditor
{
    public partial class MaterialEditorMaterial : UserControl
    {
        private bool _magsAITabHidden;

        public event EventHandler<MaterialDisplayNameArgs> NameChanged;

        internal Material _selectedMaterial;
        internal MaterialsBySupplier _selectedSupplier;

        public bool TabMagsAIVisible
        {
            get
            {
                return !_magsAITabHidden;
            }
            set
            {
                _magsAITabHidden = !value;
            }
        }

        public MaterialEditorMaterial()
        {
            InitializeComponent();

        }

        internal void LoadSelectedMaterial(Material selectedMaterial, MaterialsBySupplier selectedSupplier)
        {
            this._selectedMaterial = selectedMaterial;
            this._selectedSupplier = selectedSupplier;
            //resin properties
            try
            {
                this.btnResinModelColor.BackColor = selectedMaterial.ModelColor;
                this.txtResinName.Text = selectedMaterial.Name;
                this.txtDisplayName.Text = selectedMaterial.DisplayName;

                this.txtResinInitialLayers.Value = selectedMaterial.InitialLayers;
                this.txtResinPreparationMovements.Value = selectedMaterial.PreparationLayersCount;
                this.txtResinShrinkFactor.Value = (decimal)selectedMaterial.ShrinkFactor - 1;
                this.txtResinZBleedingOffset.Value = (decimal)selectedMaterial.BleedingOffset;

                this.txtResinArticle.Text = selectedMaterial.ArticleNumber;
                this.txtResinArticleURL.Text = selectedMaterial.ArticleHTTP;
                this.txtResinBatch.Text = selectedMaterial.BatchNumber;
                this.txtResinPrice.Text = selectedMaterial.Price.ToString("0.00");
                this.txtResinDescription.Text = selectedMaterial.Description;


                //material properties
                this.txtLayerThickness1.Value = (decimal)selectedMaterial.LT1;
                this.txtLayerThickness2.Value = (decimal)selectedMaterial.LT2;
                this.txtLayerRehabTime1.Value = (decimal)selectedMaterial.RT1;
                this.txtLayerRehabTime2.Value = (decimal)selectedMaterial.RT2;
                this.txtLayerCurringTime1.Value = (decimal)selectedMaterial.CT1;
                this.txtLayerCurringTime2.Value = (decimal)selectedMaterial.CT2;
                this.txtLayerRehabTimeAfterCuring1.Value = (decimal)selectedMaterial.RTAC1;
                this.txtLayerRehabTimeAfterCuring2.Value = (decimal)selectedMaterial.RTAC2;
                this.txtLayerRetractionHeight1.Value = (decimal)selectedMaterial.RH1;
                this.txtLayerRetractionHeight2.Value = (decimal)selectedMaterial.RH2;
                this.txtLayerRetractionSpeedUp1.Value = (decimal)selectedMaterial.RSU1;
                this.txtLayerRetractionSpeedUp2.Value = (decimal)selectedMaterial.RSU2;
                this.txtLayerTimeAtTop1.Value = (decimal)selectedMaterial.TAT1;
                this.txtLayerTimeAtTop2.Value = (decimal)selectedMaterial.TAT2;
                this.txtLayerRetractionSpeedDown1.Value = (decimal)selectedMaterial.RSD1;
                this.txtLayerRetractionSpeedDown2.Value = (decimal)selectedMaterial.RSD2;
                this.txtLayerLightIntensityStrength1.Value = (decimal)selectedMaterial.LightIntensityPercentage1;
                this.txtLayerLightIntensityStrength2.Value = (decimal)selectedMaterial.LightIntensityPercentage2;

                this.txtLayerTransitionLayer1CT.Value = 0;
                this.txtLayerTransitionLayer1LI.Value = 0;
                this.txtLayerTransitionLayer1RTAC.Value = 0;
                this.txtLayerTransitionLayer2CT.Value = 0;
                this.txtLayerTransitionLayer2LI.Value = 0;
                this.txtLayerTransitionLayer2RTAC.Value = 0;
                this.txtLayerTransitionLayer3CT.Value = 0;
                this.txtLayerTransitionLayer3LI.Value = 0;
                this.txtLayerTransitionLayer3RTAC.Value = 0;
                this.txtLayerTransitionLayer4CT.Value = 0;
                this.txtLayerTransitionLayer4LI.Value = 0;
                this.txtLayerTransitionLayer4RTAC.Value = 0;
                this.txtLayerTransitionLayer5CT.Value = 0;
                this.txtLayerTransitionLayer5LI.Value = 0;
                this.txtLayerTransitionLayer5RTAC.Value = 0;

                if (selectedMaterial.TransitionLayers != null && selectedMaterial.TransitionLayers.Count > 0)
                {
                    if (selectedMaterial.TransitionLayers.Count > 4)
                    {
                        this.txtLayerTransitionLayer5CT.Value = (decimal)selectedMaterial.TransitionLayers[4].CT;
                        this.txtLayerTransitionLayer5LI.Value = (decimal)selectedMaterial.TransitionLayers[4].LI;
                        this.txtLayerTransitionLayer5RTAC.Value = (decimal)selectedMaterial.TransitionLayers[4].RTAC;
                    }
                    if (selectedMaterial.TransitionLayers.Count > 3)
                    {
                        this.txtLayerTransitionLayer4CT.Value = (decimal)selectedMaterial.TransitionLayers[3].CT;
                        this.txtLayerTransitionLayer4LI.Value = (decimal)selectedMaterial.TransitionLayers[3].LI;
                        this.txtLayerTransitionLayer4RTAC.Value = (decimal)selectedMaterial.TransitionLayers[3].RTAC;
                    }
                    if (selectedMaterial.TransitionLayers.Count > 2)
                    {
                        this.txtLayerTransitionLayer3CT.Value = (decimal)selectedMaterial.TransitionLayers[2].CT;
                        this.txtLayerTransitionLayer3LI.Value = (decimal)selectedMaterial.TransitionLayers[2].LI;
                        this.txtLayerTransitionLayer3RTAC.Value = (decimal)selectedMaterial.TransitionLayers[2].RTAC;
                    }
                    if (selectedMaterial.TransitionLayers.Count > 1)
                    {
                        this.txtLayerTransitionLayer2CT.Value = (decimal)selectedMaterial.TransitionLayers[1].CT;
                        this.txtLayerTransitionLayer2LI.Value = (decimal)selectedMaterial.TransitionLayers[1].LI;
                        this.txtLayerTransitionLayer2RTAC.Value = (decimal)selectedMaterial.TransitionLayers[1].RTAC;
                    }
                    if (selectedMaterial.TransitionLayers.Count > 0)
                    {
                        this.txtLayerTransitionLayer1CT.Value = (decimal)selectedMaterial.TransitionLayers[0].CT;
                        this.txtLayerTransitionLayer1LI.Value = (decimal)selectedMaterial.TransitionLayers[0].LI;
                        this.txtLayerTransitionLayer1RTAC.Value = (decimal)selectedMaterial.TransitionLayers[0].RTAC;
                    }

                }

                //support cone properties
                if (selectedMaterial.SupportProfiles == null || selectedMaterial.SupportProfiles.Count == 0)
                {
                    selectedMaterial.SupportProfiles = new List<SupportProfile>();
                    selectedMaterial.SupportProfiles.Add(SupportProfile.CreateDefault());
                }
                if (selectedMaterial.SupportProfiles.First().SupportTopRadius == 0 && selectedMaterial.SupportProfiles.First().SupportTopHeight == 0)
                {
                    selectedMaterial.SupportProfiles.Clear();
                    selectedMaterial.SupportProfiles.Add(SupportProfile.CreateDefault());
                }


                var defaultSupportProfile = selectedMaterial.SupportProfiles.First();
                this.txtSupportConeTopRadius.Value = (decimal)defaultSupportProfile.SupportTopRadius;
                this.txtSupportConeTopHeight.Value = (decimal)defaultSupportProfile.SupportTopHeight;
                this.txtSupportConeMiddleRadius.Value = (decimal)defaultSupportProfile.SupportMiddleRadius;
                this.txtSupportConeBottomRadius.Value = (decimal)defaultSupportProfile.SupportBottomRadius;
                this.txtSupportConeBottomHeight.Value = (decimal)defaultSupportProfile.SupportBottomHeight;
                this.txtSupportConeBottomWidthCorrection.Value = (decimal)defaultSupportProfile.SupportBottomWidthCorrection;

                this.txtMAGSAIInfillDistance.Value = (decimal)defaultSupportProfile.SupportInfillDistance <= 0 ? (decimal)2.1 : (decimal)defaultSupportProfile.SupportInfillDistance;
                this.txtMAGSAIOverhangDistance.Value = (decimal)defaultSupportProfile.SupportOverhangDistance <= 0 ? (decimal)2.1 : (decimal)defaultSupportProfile.SupportOverhangDistance;
                this.txtMAGSAIOutlineDistance.Value = (decimal)defaultSupportProfile.SupportLowestPointsDistance <= 0 ? (decimal)2.1 : (decimal)defaultSupportProfile.SupportLowestPointsDistance;

                this.txtMAGSAILowestPointOffset1CenterDistanceFactor1.Value = 0;
                this.txtMAGSAILowestPointOffset1OutlineDistanceFactor1.Value = 0;
                this.txtMAGSAILowestPointOffset1CenterDistanceFactor2.Value = 0;
                this.txtMAGSAILowestPointOffset1OutlineDistanceFactor2.Value = 0;
                this.txtMAGSAILowestPointOffset1CenterDistanceFactor3.Value = 0;
                this.txtMAGSAILowestPointOffset1OutlineDistanceFactor3.Value = 0;
                this.txtMAGSAILowestPointOffset1CenterDistanceFactor4.Value = 0;
                this.txtMAGSAILowestPointOffset1OutlineDistanceFactor4.Value = 0;

                if (defaultSupportProfile.SupportLowestPointsOffset.Count > 0 && defaultSupportProfile.SupportLowestPointsDistanceOffset.Count > 0)
                {
                    this.txtMAGSAILowestPointOffset1CenterDistanceFactor1.Value = (decimal)defaultSupportProfile.SupportLowestPointsOffset[0];
                    this.txtMAGSAILowestPointOffset1OutlineDistanceFactor1.Value = (decimal)defaultSupportProfile.SupportLowestPointsDistanceOffset[0];

                    if (defaultSupportProfile.SupportLowestPointsOffset.Count > 1 && defaultSupportProfile.SupportLowestPointsDistanceOffset.Count > 1)
                    {
                        this.txtMAGSAILowestPointOffset1CenterDistanceFactor2.Value = (decimal)defaultSupportProfile.SupportLowestPointsOffset[1];
                        this.txtMAGSAILowestPointOffset1OutlineDistanceFactor2.Value = (decimal)defaultSupportProfile.SupportLowestPointsDistanceOffset[1];

                        if (defaultSupportProfile.SupportLowestPointsOffset.Count > 2 && defaultSupportProfile.SupportLowestPointsDistanceOffset.Count > 2)
                        {
                            this.txtMAGSAILowestPointOffset1CenterDistanceFactor3.Value = (decimal)defaultSupportProfile.SupportLowestPointsOffset[2];
                            this.txtMAGSAILowestPointOffset1OutlineDistanceFactor3.Value = (decimal)defaultSupportProfile.SupportLowestPointsDistanceOffset[2];

                            if (defaultSupportProfile.SupportLowestPointsOffset.Count > 3 && defaultSupportProfile.SupportLowestPointsDistanceOffset.Count > 3)
                            {
                                this.txtMAGSAILowestPointOffset1CenterDistanceFactor4.Value = (decimal)defaultSupportProfile.SupportLowestPointsOffset[3];
                                this.txtMAGSAILowestPointOffset1OutlineDistanceFactor4.Value = (decimal)defaultSupportProfile.SupportLowestPointsDistanceOffset[3];
                            }
                        }
                    }
                }

                this.txtMAGSAIAngledSurfaceDistance1.Value = this.txtMAGSAIAngledSurfaceDistance1.Minimum;
                this.txtMAGSAIAngledSurfaceDistance2.Value = this.txtMAGSAIAngledSurfaceDistance2.Minimum;

                if (defaultSupportProfile.SurfaceAngles.Count > 0 && defaultSupportProfile.SurfaceAngleDistanceFactors.Count > 0)
                {
                    this.txtMAGSAIAngledSurfaceDistance1.Value = (decimal)defaultSupportProfile.SurfaceAngleDistanceFactors[0];

                    if (defaultSupportProfile.SurfaceAngles.Count > 1 && defaultSupportProfile.SurfaceAngleDistanceFactors.Count > 1)
                    {
                        this.txtMAGSAIAngledSurfaceDistance2.Value = (decimal)defaultSupportProfile.SurfaceAngleDistanceFactors[1];
                    }
                }

                if (_magsAITabHidden)
                {
                    this.tbMaterialSettings.TabPages.Remove(this.tbMAGSAI);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

            this.txtSupportConeTopHeight.ValueChanged += UpdateSupportConeDrawing;
            this.txtSupportConeBottomHeight.ValueChanged += UpdateSupportConeDrawing;
            this.txtSupportConeTopRadius.ValueChanged += UpdateSupportConeDrawing;
            this.txtSupportConeMiddleRadius.ValueChanged += UpdateSupportConeDrawing;
            this.txtSupportConeBottomRadius.ValueChanged += UpdateSupportConeDrawing;
            this.txtSupportConeBottomWidthCorrection.ValueChanged += UpdateSupportConeDrawing;

            if (this.tbMaterialSettings.SelectedTab == this.tbSupportProperties)
            {
                this.tbMaterialSettings.Update();
                DrawSupportCone();
            }
        }

        private void UpdateSupportConeDrawing(object sender, EventArgs e)
        {
            DrawSupportCone();
        }

        private void DrawSupportCone()
        {
            var maxWidth = this.plSupportConeOverview.Width - 16;
            var maxHeight = this.plSupportConeOverview.Height - 16;

            //every sub percentage is 1 pixel
            var g = plSupportConeOverview.CreateGraphics();
            g.Clear(this.BackColor);

            g.SmoothingMode = SmoothingMode.HighQuality;

            //start with middle section first
            var middleWidthPoint = (maxWidth / 2);
            var middleHeightPoint = maxHeight / 2;

            var outlinePen = new Pen(Color.Gray, 2);
            var outlinePenSelected = new SolidBrush(BrandingManager.Menu_Item_HighlightColor);
            var middleHeight = 80;
            var supportConeMultiplyFactor = 8;

            var topWidthInPixels = (int)Math.Ceiling(this.txtSupportConeTopRadius.Value * supportConeMultiplyFactor);
            var middleWidthInPixels = (int)Math.Ceiling(this.txtSupportConeMiddleRadius.Value * supportConeMultiplyFactor);
            var middleWidthCorrectionInPixels = middleWidthInPixels;
            middleWidthCorrectionInPixels += (int)(middleWidthCorrectionInPixels * this.txtSupportConeBottomWidthCorrection.Value);
            var bottomWidthInPixels = (int)Math.Ceiling(this.txtSupportConeBottomRadius.Value * supportConeMultiplyFactor);
            bottomWidthInPixels += (int)(bottomWidthInPixels * this.txtSupportConeBottomWidthCorrection.Value * 2);
            var bottomHeightInPixels = (int)Math.Ceiling(this.txtSupportConeBottomHeight.Value * supportConeMultiplyFactor);
            var topHeightInPixels = (int)Math.Ceiling(this.txtSupportConeTopHeight.Value * supportConeMultiplyFactor);

            var outlinePoints = new List<Point>()
            {
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint - middleWidthCorrectionInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint - bottomWidthInPixels),middleHeightPoint + middleHeight + bottomHeightInPixels),
               new Point((int)(middleWidthPoint + bottomWidthInPixels), middleHeightPoint + middleHeight + bottomHeightInPixels),
               new Point((int)(middleWidthPoint + middleWidthCorrectionInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint + topWidthInPixels), (int)middleHeightPoint - middleHeight - topHeightInPixels),
               new Point((int)(middleWidthPoint - topWidthInPixels), (int)middleHeightPoint - middleHeight - topHeightInPixels),
            };

            g.DrawPolygon(outlinePen, outlinePoints.ToArray());

            var selectedOutlinePoints = new List<Point>()
            {
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint - middleWidthCorrectionInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint + middleWidthCorrectionInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint - middleHeight),
            };

            g.FillPolygon(outlinePenSelected, selectedOutlinePoints.ToArray());
        }

        internal void SaveSelectedMaterial()
        {
            //Incrementing ChangeId on material update
            this._selectedMaterial.ChangeId += 1;
            this._selectedMaterial.Name = this.txtResinName.Text;
            this._selectedMaterial.DisplayName = this.txtDisplayName.Text;
            this._selectedMaterial.ModelColor = this.btnResinModelColor.BackColor;

            this._selectedMaterial.InitialLayers = (int)this.txtResinInitialLayers.Value;
            this._selectedMaterial.PreparationLayersCount = (int)this.txtResinPreparationMovements.Value;
            this._selectedMaterial.ShrinkFactor = (double)this.txtResinShrinkFactor.Value + 1;
            this._selectedMaterial.BleedingOffset = (double)this.txtResinZBleedingOffset.Value;

            this._selectedMaterial.ArticleNumber = this.txtResinArticle.Text;
            this._selectedMaterial.ArticleHTTP = this.txtResinArticleURL.Text;
            this._selectedMaterial.BatchNumber = this.txtResinBatch.Text;
            this._selectedMaterial.Price = decimal.Parse(this.txtResinPrice.Text);
            this._selectedMaterial.Description = this.txtResinDescription.Text;


            //material properties
            this._selectedMaterial.LT1 = Math.Round((double)this.txtLayerThickness1.Value, 3);
            this._selectedMaterial.LT2 = Math.Round((double)this.txtLayerThickness2.Value, 3);
            this._selectedMaterial.RT1 = (double)this.txtLayerRehabTime1.Value;
            this._selectedMaterial.RT2 = (double)this.txtLayerRehabTime2.Value;
            this._selectedMaterial.CT1 = Math.Round((double)this.txtLayerCurringTime1.Value, 3);
            this._selectedMaterial.CT2 = Math.Round((double)this.txtLayerCurringTime2.Value, 3);
            this._selectedMaterial.RTAC1 = Math.Round((double)this.txtLayerRehabTimeAfterCuring1.Value, 3);
            this._selectedMaterial.RTAC2 = Math.Round((double)this.txtLayerRehabTimeAfterCuring2.Value, 3);
            this._selectedMaterial.RH1 = (double)this.txtLayerRetractionHeight1.Value;
            this._selectedMaterial.RH2 = (double)this.txtLayerRetractionHeight2.Value;
            this._selectedMaterial.RSU1 = (double)this.txtLayerRetractionSpeedUp1.Value;
            this._selectedMaterial.RSU2 = (double)this.txtLayerRetractionSpeedUp2.Value;
            this._selectedMaterial.TAT1 = (double)this.txtLayerTimeAtTop1.Value;
            this._selectedMaterial.TAT2 = (double)this.txtLayerTimeAtTop2.Value;
            this._selectedMaterial.RSD1 = (double)this.txtLayerRetractionSpeedDown1.Value;
            this._selectedMaterial.RSD2 = (double)this.txtLayerRetractionSpeedDown2.Value;
            this._selectedMaterial.LightIntensityPercentage1 = (float)this.txtLayerLightIntensityStrength1.Value;
            this._selectedMaterial.LightIntensityPercentage2 = (float)this.txtLayerLightIntensityStrength2.Value;

            this._selectedMaterial.TransitionLayers = new DAL.Catalogs.MaterialTransitionLayers();
            if (this.txtLayerTransitionLayer1CT.Value != 0 && this.txtLayerTransitionLayer1LI.Value != 0)
            {
                this._selectedMaterial.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = (float)this.txtLayerTransitionLayer1CT.Value, LI = (float)this.txtLayerTransitionLayer1LI.Value, RTAC = (float)this.txtLayerTransitionLayer1RTAC.Value });

                if (this.txtLayerTransitionLayer2CT.Value != 0 && this.txtLayerTransitionLayer2LI.Value != 0)
                {
                    this._selectedMaterial.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = (float)this.txtLayerTransitionLayer2CT.Value, LI = (float)this.txtLayerTransitionLayer2LI.Value, RTAC = (float)this.txtLayerTransitionLayer2RTAC.Value });

                    if (this.txtLayerTransitionLayer3CT.Value != 0 && this.txtLayerTransitionLayer3LI.Value != 0)
                    {
                        this._selectedMaterial.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = (float)this.txtLayerTransitionLayer3CT.Value, LI = (float)this.txtLayerTransitionLayer3LI.Value, RTAC = (float)this.txtLayerTransitionLayer3RTAC.Value });

                        if (this.txtLayerTransitionLayer4CT.Value != 0 && this.txtLayerTransitionLayer4LI.Value != 0)
                        {
                            this._selectedMaterial.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = (float)this.txtLayerTransitionLayer4CT.Value, LI = (float)this.txtLayerTransitionLayer4LI.Value, RTAC = (float)this.txtLayerTransitionLayer4RTAC.Value });

                            if (this.txtLayerTransitionLayer5CT.Value != 0 && this.txtLayerTransitionLayer5LI.Value != 0)
                            {
                                this._selectedMaterial.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = (float)this.txtLayerTransitionLayer5CT.Value, LI = (float)this.txtLayerTransitionLayer5LI.Value, RTAC = (float)this.txtLayerTransitionLayer5RTAC.Value });
                            }
                        }
                    }
                }
            }

            //support cone properties
            var defaultSupportProfile = this._selectedMaterial.SupportProfiles.First();
            defaultSupportProfile.SupportTopRadius = (float)this.txtSupportConeTopRadius.Value;
            defaultSupportProfile.SupportTopHeight = (float)this.txtSupportConeTopHeight.Value;
            defaultSupportProfile.SupportMiddleRadius = (float)this.txtSupportConeMiddleRadius.Value;
            defaultSupportProfile.SupportBottomRadius = (float)this.txtSupportConeBottomRadius.Value;
            defaultSupportProfile.SupportBottomHeight = (float)this.txtSupportConeBottomHeight.Value;

            defaultSupportProfile.SupportBottomWidthCorrection = (float)this.txtSupportConeBottomWidthCorrection.Value;

            //mags ai properties
            defaultSupportProfile.SupportInfillDistance = (float)this.txtMAGSAIInfillDistance.Value;
            defaultSupportProfile.SupportOverhangDistance = (float)this.txtMAGSAIOverhangDistance.Value;
            defaultSupportProfile.SupportLowestPointsDistance = (float)this.txtMAGSAIOutlineDistance.Value;

            defaultSupportProfile.SupportLowestPointsOffset.Clear();
            defaultSupportProfile.SupportLowestPointsDistanceOffset.Clear();

            if (this.txtMAGSAILowestPointOffset1CenterDistanceFactor1.Value > 0 && this.txtMAGSAILowestPointOffset1OutlineDistanceFactor1.Value > 0)
            {
                defaultSupportProfile.SupportLowestPointsOffset.Add((float)this.txtMAGSAILowestPointOffset1CenterDistanceFactor1.Value);
                defaultSupportProfile.SupportLowestPointsDistanceOffset.Add((float)this.txtMAGSAILowestPointOffset1OutlineDistanceFactor1.Value);

                if (this.txtMAGSAILowestPointOffset1CenterDistanceFactor2.Value > 0 && this.txtMAGSAILowestPointOffset1OutlineDistanceFactor2.Value > 0)
                {
                    defaultSupportProfile.SupportLowestPointsOffset.Add((float)this.txtMAGSAILowestPointOffset1CenterDistanceFactor2.Value);
                    defaultSupportProfile.SupportLowestPointsDistanceOffset.Add((float)this.txtMAGSAILowestPointOffset1OutlineDistanceFactor2.Value);

                    if (this.txtMAGSAILowestPointOffset1CenterDistanceFactor3.Value > 0 && this.txtMAGSAILowestPointOffset1OutlineDistanceFactor3.Value > 0)
                    {
                        defaultSupportProfile.SupportLowestPointsOffset.Add((float)this.txtMAGSAILowestPointOffset1CenterDistanceFactor3.Value);
                        defaultSupportProfile.SupportLowestPointsDistanceOffset.Add((float)this.txtMAGSAILowestPointOffset1OutlineDistanceFactor3.Value);

                        if (this.txtMAGSAILowestPointOffset1CenterDistanceFactor4.Value > 0 && this.txtMAGSAILowestPointOffset1OutlineDistanceFactor4.Value > 0)
                        {
                            defaultSupportProfile.SupportLowestPointsOffset.Add((float)this.txtMAGSAILowestPointOffset1CenterDistanceFactor4.Value);
                            defaultSupportProfile.SupportLowestPointsDistanceOffset.Add((float)this.txtMAGSAILowestPointOffset1OutlineDistanceFactor4.Value);
                        }
                    }
                }
            }

            defaultSupportProfile.SurfaceAngles.Clear();
            defaultSupportProfile.SurfaceAngleDistanceFactors.Clear();
            if (this.txtMAGSAIAngledSurfaceDistance1.Value > 0)
            {
                defaultSupportProfile.SurfaceAngles.Add(30);
                defaultSupportProfile.SurfaceAngleDistanceFactors.Add((float)this.txtMAGSAIAngledSurfaceDistance1.Value);

                if (this.txtMAGSAIAngledSurfaceDistance2.Value > 0)
                {
                    defaultSupportProfile.SurfaceAngles.Add(45);
                    defaultSupportProfile.SurfaceAngleDistanceFactors.Add((float)this.txtMAGSAIAngledSurfaceDistance2.Value);
                }
            }


            //find supplier by using material id
            if (_selectedSupplier.Materials.Any(s => s.Id == this._selectedMaterial.Id))
            {
                MaterialManager.SaveMaterial(_selectedSupplier);
            }

            else
            {
                _selectedSupplier.Materials.Add(this._selectedMaterial);
                MaterialManager.SaveMaterial(_selectedSupplier);
            }

        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            this.NameChanged?.Invoke(null, new MaterialDisplayNameArgs() { DisplayName = this.txtResinName.Text });
        }

        private void btnResinModelColor_Click(object sender, EventArgs e)
        {
            using (var colorPicker = new ColorDialog())
            {
                if (colorPicker.ShowDialog() == DialogResult.OK)
                {
                    this.btnResinModelColor.BackColor = colorPicker.Color;
                }
            }
        }

        private void tbMaterialSettings_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tbMaterialSettings.SelectedIndex == 1)
            {
                this.txtLayerThickness1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerRehabTime1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerCurringTime1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerRehabTimeAfterCuring1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerRetractionHeight1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerRetractionSpeedUp1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerTimeAtTop1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerRetractionSpeedDown1.Enabled = this.txtResinInitialLayers.Value > 0;
                this.txtLayerLightIntensityStrength1.Enabled = this.txtResinInitialLayers.Value > 0;
            }

            this.DrawSupportCone();
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveSelectedMaterial();
        }
    }
}
