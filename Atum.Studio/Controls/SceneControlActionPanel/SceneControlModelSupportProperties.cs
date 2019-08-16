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
using System.Drawing.Drawing2D;
using Atum.Studio.Core.Engines;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlModelSupportProperties : SceneControlActionSubPanelBase
    {
        private SupportCone _supportCone;
        private int _inputHeightTopPoint;
        private int _inputRadiusTopPoint;

        internal SupportCone DataSource
        {
            get
            {
                return this._supportCone;
            }
            set
            {
                this._supportCone = value;
                UpdateControl(new MouseEventArgs(MouseButtons.Left, 0, 1, 0, 0));
            }
        }

        public SceneControlModelSupportProperties()
        {
            InitializeComponent();

            this.HeaderText = "Support properties";
            if (FontManager.Loaded)
            {
                this.lblInformation.Font = FontManager.Montserrat14Regular;
                this.btnApplyToAll.Font = this.btnSetAsDefault.Font = FontManager.Montserrat12Regular;
            }


            this.txtHeightInput.ValueChanged += txtHeight_ValueChanged;
            this.txtRadiusInput.ValueChanged += txtRadius_ValueChanged;

            this._inputHeightTopPoint = this.txtHeightInput.Top;
            this._inputRadiusTopPoint = this.txtRadiusInput.Top;

            //change textbox width to label text + padding)
            var bottomMeasurement = TextRenderer.MeasureText(this.btnBottom.Text, this.btnBottom.Font);
            var middleMeasurement = TextRenderer.MeasureText(this.btnMiddle.Text, this.btnMiddle.Font);
            var topMeasurement = TextRenderer.MeasureText(this.btnTop.Text, this.btnTop.Font);

            var paddingLeftRight = 4;
            var btnCurrentBottomWidth = this.btnBottom.Width;
            this.btnBottom.Width = paddingLeftRight + bottomMeasurement.Width;
            this.btnBottom.Left += btnCurrentBottomWidth - this.btnBottom.Width;
            this.btnMiddle.Width = paddingLeftRight + middleMeasurement.Width;
            this.btnMiddle.Left = this.btnBottom.Left - this.btnMiddle.Width;
            this.btnTop.Width = paddingLeftRight + topMeasurement.Width;
            this.btnTop.Left = this.btnMiddle.Left - this.btnTop.Width;

            this.onClosed += SceneControlModelSupportProperties_onClosed;

        }

        private void SceneControlModelSupportProperties_onClosed(object sender, EventArgs e)
        {
            SceneActionControlManager.CloseSupportPropertiesHandle();
        }

        private void txtRadius_ValueChanged(object sender, float e)
        {
            var gridSupportEvent = new SupportEventArgs()
            {
                BottomHeight = this._supportCone.BottomHeight,
                BottomRadius = this._supportCone.BottomRadius,
                MiddleRadius = this._supportCone.MiddleRadius,
                TopHeight = this._supportCone.TopHeight,
                TopRadius = this._supportCone.TopRadius
            };

            if (this._topAreaSelected)
            {
                if (this._supportCone.IsSurfaceSupport)
                {
                    gridSupportEvent.TopRadius = e / 2;
                    this._supportCone.SupportSurface.UpdateGridSupport(gridSupportEvent);
                }
                else
                {
                    this._supportCone.TopRadius = e / 2;
                }

            }
            else if (this._middleAreaSelected)
            {
                if (this._supportCone.IsSurfaceSupport)
                {
                    gridSupportEvent.MiddleRadius = e / 2;
                    this._supportCone.SupportSurface.UpdateGridSupport(gridSupportEvent);
                }
                else
                {
                    this._supportCone.MiddleRadius = e / 2;
                }

            }
            else if (this._bottomAreaSelected)
            {
                if (this._supportCone.IsSurfaceSupport)
                {
                    gridSupportEvent.BottomRadius = e / 2;
                    this._supportCone.SupportSurface.UpdateGridSupport(gridSupportEvent);
                }
                else
                {
                    this._supportCone.BottomRadius = e / 2;
                }


            }

            DrawSupportCone();
            frmStudioMain.SceneControl.Render();
        }

        private void txtHeight_ValueChanged(object sender, float e)
        {
            var gridSupportEvent = new SupportEventArgs()
            {
                BottomHeight = this._supportCone.BottomHeight,
                BottomRadius = this._supportCone.BottomRadius,
                MiddleRadius = this._supportCone.MiddleRadius,
                TopHeight = this._supportCone.TopHeight,
                TopRadius = this._supportCone.TopRadius
            };

            if (this._topAreaSelected)
            {
                if (this._supportCone.IsSurfaceSupport)
                {
                    gridSupportEvent.TopHeight = e;
                    this._supportCone.SupportSurface.UpdateGridSupport(gridSupportEvent);
                }
                else
                {
                    this._supportCone.TopHeight = e;
                }
            }
            else if (this._bottomAreaSelected)
            {
                if (this._supportCone.IsSurfaceSupport)
                {
                    gridSupportEvent.BottomHeight = e;
                    this._supportCone.SupportSurface.UpdateGridSupport(gridSupportEvent);
                }
                else
                {
                    this._supportCone.BottomHeight = e;
                }
            }

            DrawSupportCone();
            frmStudioMain.SceneControl.Render();
        }


        private int _topHeightArea;

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
            var middleHeight = 40;
            var supportConeMultiplyFactor = 4;

            var topWidthInPixels = (int)Math.Ceiling(_supportCone.TopRadius * supportConeMultiplyFactor);
            var middleWidthInPixels = (int)Math.Ceiling(_supportCone.MiddleRadius * supportConeMultiplyFactor);
            var bottomWidthInPixels = (int)Math.Ceiling(_supportCone.BottomRadius * supportConeMultiplyFactor);
            var bottomHeightInPixels = (int)Math.Ceiling(_supportCone.BottomHeight * supportConeMultiplyFactor);
            var topHeightInPixels = (int)Math.Ceiling(_supportCone.TopHeight * supportConeMultiplyFactor);

            var outlinePoints = new List<Point>()
            {
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint - bottomWidthInPixels),middleHeightPoint + middleHeight + bottomHeightInPixels),
               new Point((int)(middleWidthPoint + bottomWidthInPixels), middleHeightPoint + middleHeight + bottomHeightInPixels),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint + topWidthInPixels), (int)middleHeightPoint - middleHeight - topHeightInPixels),
               new Point((int)(middleWidthPoint - topWidthInPixels), (int)middleHeightPoint - middleHeight - topHeightInPixels),
            };

            g.DrawPolygon(outlinePen, outlinePoints.ToArray());

            if (_topAreaSelected)
            {
                var selectedOutlinePoints = new List<Point>()
            {
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint + topWidthInPixels), (int)middleHeightPoint - middleHeight - topHeightInPixels),
               new Point((int)(middleWidthPoint - topWidthInPixels), (int)middleHeightPoint - middleHeight - topHeightInPixels),
                              new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint - middleHeight),
            };

                g.FillPolygon(outlinePenSelected, selectedOutlinePoints.ToArray());
            }
            else if (_middleAreaSelected)
            {
                var selectedOutlinePoints = new List<Point>()
            {
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint - middleHeight),
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint - middleHeight),
            };

                g.FillPolygon(outlinePenSelected, selectedOutlinePoints.ToArray());

                //    selectedOutlinePoints = new List<Point>()
                //{

                //};

                //    g.FillPolygon(outlinePenSelected, selectedOutlinePoints.ToArray());
            }

            else if (_bottomAreaSelected)
            {
                var selectedOutlinePoints = new List<Point>()
            {
               new Point((int)(middleWidthPoint - middleWidthInPixels), middleHeightPoint + middleHeight),
               new Point((int)(middleWidthPoint - bottomWidthInPixels),middleHeightPoint + middleHeight + bottomHeightInPixels),
               new Point((int)(middleWidthPoint + bottomWidthInPixels), middleHeightPoint + middleHeight + bottomHeightInPixels),
               new Point((int)(middleWidthPoint + middleWidthInPixels), middleHeightPoint + middleHeight),
            };

                g.FillPolygon(outlinePenSelected, selectedOutlinePoints.ToArray());
            }

            _topHeightArea = middleHeightPoint - middleHeight;
        }

        //private void plSupportConeOverview_Paint(object sender, PaintEventArgs e)
        //{
        //    //DrawSupportCone();
        //}

        private bool _topAreaSelected = true;
        private bool _middleAreaSelected;
        private bool _bottomAreaSelected;

        private void plSupportConeOverview_MouseDown(object sender, MouseEventArgs e)
        {

            UpdateControl(e);
        }

        private void UpdateControl(MouseEventArgs e)
        {
            this._topAreaSelected = false;
            this._middleAreaSelected = false;
            this._bottomAreaSelected = false;

            this.txtHeightInput.Visible = false;

            this.btnTop.BackColor = this.BackColor;
            this.btnMiddle.BackColor = this.BackColor;
            this.btnBottom.BackColor = this.BackColor;

            this.btnHeight.Visible = false;

            if (e.Location.Y <= _topHeightArea)
            {
                this._topAreaSelected = true;

                this.btnHeight.Top = this.txtHeightInput.Top = this._inputHeightTopPoint;
                this.btnRadius.Top = this.txtRadiusInput.Top = this._inputRadiusTopPoint;

                this.txtRadiusInput.DisableTextChangeTrigger();
                this.txtRadiusInput.Value = (float)Math.Round(this._supportCone.TopRadius * 2, 2);
                this.txtRadiusInput.EnableTextChangeTrigger();

                this.txtHeightInput.Visible = true;
                this.txtHeightInput.DisableTextChangeTrigger();
                this.txtHeightInput.Value = (float)Math.Round(this._supportCone.TopHeight, 2);
                this.txtHeightInput.EnableTextChangeTrigger();
                this.txtHeightInput.Select();
                this.txtHeightInput.Focus();

                this.btnTop.BackColor = Color.Gray;


                this.btnHeight.Visible = true;
            }
            else if (e.Location.Y > this.plSupportConeOverview.Height - _topHeightArea - 10)
            {
                this._bottomAreaSelected = true;

                this.btnHeight.Top = this.txtHeightInput.Top = this._inputHeightTopPoint;
                this.btnRadius.Top = this.txtRadiusInput.Top = this._inputRadiusTopPoint;

                this.txtRadiusInput.DisableTextChangeTrigger();

               // if (this._supportCone.BottomWidthCorrection == 0)
              //  {
                    this.txtRadiusInput.Value = (float)Math.Round(this._supportCone.BottomRadius * 2, 2);
              //  }
              //  else
               // {
               //     this.txtRadiusInput.Value = (float)Math.Round(this._supportCone.BottomRadiusWithBottomCorrection * 2, 2);
               //     
               // }

                this.txtRadiusInput.EnableTextChangeTrigger();

                this.btnBottom.BackColor = Color.Gray;

                this.txtHeightInput.Visible = true;
                this.txtHeightInput.DisableTextChangeTrigger();
                this.txtHeightInput.Value = (float)Math.Round(this._supportCone.BottomHeight, 2);
                this.txtHeightInput.EnableTextChangeTrigger();
                this.txtHeightInput.Select();
                this.txtHeightInput.Focus();
                this.btnHeight.Visible = true;

            }
            else
            {
                this._middleAreaSelected = true;

                this.btnMiddle.BackColor = Color.Gray;
                this.btnRadius.Top = this.txtRadiusInput.Top = this._inputHeightTopPoint + ((this._inputRadiusTopPoint - this._inputHeightTopPoint) / 2);

                this.txtRadiusInput.DisableTextChangeTrigger();
                this.txtRadiusInput.Value = (float)Math.Round(this._supportCone.MiddleRadius * 2, 2);
                this.txtRadiusInput.EnableTextChangeTrigger();
                this.txtRadiusInput.Select();
                this.txtRadiusInput.Focus();
            }

            DrawSupportCone();
        }

        private void btnTop_Click(object sender, EventArgs e)
        {
            this.UpdateControl(new MouseEventArgs(MouseButtons.Left, 0, 0, 0, 0));
        }

        private void btnBottom_Click(object sender, EventArgs e)
        {
            this.UpdateControl(new MouseEventArgs(MouseButtons.Left, 0, 0, this.plSupportConeOverview.Height - 10, 0));
        }

        private void btnMiddle_Click(object sender, EventArgs e)
        {
            this.UpdateControl(new MouseEventArgs(MouseButtons.Left, 0, 0, (this.plSupportConeOverview.Height / 2) + 10, 0));
        }

        private void SceneControlModelSupportProperties_VisibleChanged(object sender, EventArgs e)
        {
            DrawSupportCone();
        }

        private void btnApplyToAll_Click(object sender, EventArgs e)
        {
            if (this._supportCone != null)
            {
                var selectedModel = this._supportCone.Model;

                if (selectedModel != null)
                {
                    var supportUpdateArgs = new Core.Events.SupportEventArgs();
                    supportUpdateArgs.TopRadius = this._supportCone.TopRadius;
                    supportUpdateArgs.TopHeight = this._supportCone.TopHeight;
                    supportUpdateArgs.MiddleRadius = this._supportCone.MiddleRadius;
                    supportUpdateArgs.BottomHeight = this._supportCone.BottomHeight;
                    supportUpdateArgs.BottomRadius = this._supportCone.BottomRadius;

                    foreach (var supportCone in selectedModel.TotalObjectSupportCones)
                    {
                        if (supportCone != null)
                        {
                            supportCone.Update(supportUpdateArgs, selectedModel);
                        }
                    }
                }
            }
        }

        private void btnSetAsDefault_Click(object sender, EventArgs e)
        {
            var selectedMaterial = PrintJobManager.SelectedMaterialSummary.Material;
            if (selectedMaterial != null)
            {
                if (selectedMaterial.SupportProfiles.Count >= 0 && selectedMaterial.SupportProfiles[0] != null)
                {
                    selectedMaterial.SupportProfiles[0].SupportTopHeight = this._supportCone.TopHeight;
                    selectedMaterial.SupportProfiles[0].SupportTopRadius = this._supportCone.TopRadius;
                    selectedMaterial.SupportProfiles[0].SupportMiddleRadius = this._supportCone.MiddleRadius;
                    selectedMaterial.SupportProfiles[0].SupportBottomHeight = this._supportCone.BottomHeight;
                    selectedMaterial.SupportProfiles[0].SupportBottomRadius = this._supportCone.BottomRadius;
                }
            }
        }
    }
}
