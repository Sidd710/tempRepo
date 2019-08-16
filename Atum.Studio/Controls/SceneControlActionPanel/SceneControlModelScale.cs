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
using Atum.Studio.Resources;

namespace Atum.Studio.Controls.SceneControlActionPanel
{
    public partial class SceneControlModelScale : SceneControlActionPanelBase
    {
        private enum ScaleLinkType
        {
            Linked = 0,
            Unlinked = 1
        }

        internal event EventHandler<ScaleEventArgs> ValueChanged;

        private STLModel3D _dataSource;
        private ScaleLinkType _scaleLinkType;
        private bool _showMoreOptionsCollapsed;

        internal STLModel3D DataSource
        {
            get
            {
                return this._dataSource;
            }
            set
            {
                this._dataSource = value;

                if (this._dataSource.ScaleFactorX == this._dataSource.ScaleFactorY && this._dataSource.ScaleFactorY == this._dataSource.ScaleFactorZ)
                {
                    this._scaleLinkType = ScaleLinkType.Linked;

                    this.txtUniformScaleFactor.ReadOnly = false;
                    this.plUniformScaleFactor.BackColor = Color.White;
                    this.txtUniformScaleFactor.BackColor = Color.White;

                    this.btnUniformScaleLink.Tag = true;
                    UpdateMoreOptionsCollapseState(true);
                }
                else
                {
                    this.txtUniformScaleFactor.UpdateControl();
                    this.txtUniformScaleFactor.ReadOnly = true;
                    this.plUniformScaleFactor.BackColor = Color.LightGray;
                    this.txtUniformScaleFactor.BackColor = Color.LightGray;

                    this._scaleLinkType = ScaleLinkType.Unlinked;
                    this.btnUniformScaleLink.Tag = false;

                    UpdateMoreOptionsCollapseState(false);
                }

                this.txtXValue.DisableTextChangeTrigger();
                this.txtYValue.DisableTextChangeTrigger();
                this.txtZValue.DisableTextChangeTrigger();

                this.txtXValue.Value = (float)Math.Round(_dataSource.ScaleFactorX * 100, 2);
                this.txtYValue.Value = (float)Math.Round(_dataSource.ScaleFactorY * 100, 2);
                this.txtZValue.Value = (float)Math.Round(_dataSource.ScaleFactorZ * 100, 2);

                this.txtXValue.EnableTextChangeTrigger();
                this.txtYValue.EnableTextChangeTrigger();
                this.txtZValue.EnableTextChangeTrigger();

                UpdateControl();
            }
        }

        private bool IsLinked
        {
            get
            {
                if (this.btnUniformScaleLink.Tag != null)
                {
                    return (bool)this.btnUniformScaleLink.Tag;
                }
                else
                {
                    return false;
                }
            }
        }

        private bool IsFactorValue
        {
            get
            {
                return this.lblXValueType.Text.ToLower().Contains("perce");
            }
        }

        public SceneControlModelScale()
        {
            InitializeComponent();

            this.HeaderText = "Scale Model";
            this.btnApply.BackColor = BrandingManager.Button_BackgroundColor_Dark;

            this.txtUniformScaleFactor.AllowNegativeValue = false;
            this.txtUniformScaleFactor.DefaultValue = 1f;
            this.txtUniformScaleFactor.HorizontalAlignment = HorizontalAlignment.Center;

            this.txtXValue.AllowNegativeValue = false;
            this.txtXValue.DefaultValue = 1f;
            this.txtXValue.HorizontalAlignment = HorizontalAlignment.Center;
            this.txtXValue.ValueChanged += TxtXValue_ValueChanged;
            this.txtXValue.EnableTextChangeTrigger();

            this.txtYValue.AllowNegativeValue = false;
            this.txtYValue.DefaultValue = 1f;
            this.txtYValue.HorizontalAlignment = HorizontalAlignment.Center;
            this.txtYValue.ValueChanged += TxtYValue_ValueChanged;
            this.txtYValue.EnableTextChangeTrigger();

            this.txtZValue.AllowNegativeValue = false;
            this.txtZValue.DefaultValue = 1f;
            this.txtZValue.HorizontalAlignment = HorizontalAlignment.Center;
            this.txtZValue.ValueChanged += TxtZValue_ValueChanged;
            this.txtZValue.EnableTextChangeTrigger();

            this.btnXTypeDropdown.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.arrow_black,new Size(18,18));
            this.btnYTypeDropdown.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.arrow_black, new Size(18, 18));
            this.btnZTypeDropdown.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.arrow_black, new Size(18, 18));

            if (FontManager.Loaded)
            {
                //this.dropdownListItems1.Font = FontManager.Montserrat14Regular;
            }
        }

        private void TxtXValue_ValueChanged(object sender, float e)
        {
            UpdateLinkedValues(this.txtXValue.Value, true, false, false);
        }

        private void TxtYValue_ValueChanged(object sender, float e)
        {
            UpdateLinkedValues(this.txtYValue.Value, false, true, false);
        }

        private void TxtZValue_ValueChanged(object sender, float e)
        {
            UpdateLinkedValues(this.txtZValue.Value, false, false, true);
        }

        private void UpdateControl()
        {
            //
            if (this._dataSource.ScaleFactorX == this._dataSource.ScaleFactorY && this._dataSource.ScaleFactorY == this._dataSource.ScaleFactorZ)
            {
                this.btnUniformScaleLink.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.link_black, new Size(18, 18));
                this.txtUniformScaleFactor.Value = this._dataSource.ScaleFactorX;
            }
            else
            {
                this.btnUniformScaleLink.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.link_broken_black, new Size(18, 18));

            }

            this.UpdateLinkedLines();
        }

        private void txtUniformScaleFactor_TextChanged(object sender, EventArgs e)
        {
            if (this.IsFactorValue && this.IsLinked)
            {
                this.txtXValue.DisableTextChangeTrigger();
                this.txtYValue.DisableTextChangeTrigger();
                this.txtZValue.DisableTextChangeTrigger();

                this.txtXValue.Value = (float)Math.Round(this.txtUniformScaleFactor.Value * 100, 2);
                this.txtYValue.Value = (float)Math.Round(this.txtUniformScaleFactor.Value * 100, 2); 
                this.txtZValue.Value = (float)Math.Round(this.txtUniformScaleFactor.Value * 100, 2); 

                this.txtXValue.EnableTextChangeTrigger();
                this.txtYValue.EnableTextChangeTrigger();
                this.txtZValue.EnableTextChangeTrigger();


            }

            this.txtUniformScaleFactor.UpdateControl();
        }

        private void btnUniformScaleLink_Click(object sender, EventArgs e)
        {
            if (_scaleLinkType == ScaleLinkType.Linked)
            {
                _scaleLinkType = ScaleLinkType.Unlinked;
                this.btnUniformScaleLink.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.link_broken_black, new Size(18, 18));
                this.txtUniformScaleFactor.ReadOnly = true;
                this.plUniformScaleFactor.BackColor = Color.LightGray;
                this.txtUniformScaleFactor.BackColor = Color.LightGray;

                this.btnUniformScaleLink.Tag = false;
            }
            else
            {
                _scaleLinkType = ScaleLinkType.Linked;
                this.btnUniformScaleLink.BackgroundImage = SVGParser.GetBitmapFromSVG(SVGImages.link_black, new Size(18, 18));
                this.txtUniformScaleFactor.ReadOnly = false;
                this.plUniformScaleFactor.BackColor = Color.White;
                this.txtUniformScaleFactor.BackColor = Color.White;

                this.btnUniformScaleLink.Tag = true;
            }

            UpdateLinkedLines();
            menuItemToolStripMenuItem_Click(null, null);
        }

        private void UpdateLinkedLines()
        {
            var g = this.plLinkedLines.CreateGraphics();
            g.Clear(this.BackColor);

            if (_scaleLinkType == ScaleLinkType.Linked)
            {
                Pen dashPen = new Pen(Color.Gray, 1);
                float[] dashValues = { 2, 2 };
                dashPen.DashPattern = dashValues;

                var leftUpperPoint = new PointF(this.btnUniformScaleLink.Left + (this.btnUniformScaleLink.Width / 2), 20);
                var leftMiddlePoint = new PointF(leftUpperPoint.X, 10 + ((this.plLinkedLines.Height - 20) / 2));
                var leftLowerPoint = new PointF(leftUpperPoint.X, this.plLinkedLines.Height - 20);
                g.DrawLine(dashPen, leftUpperPoint, new PointF(50, leftUpperPoint.Y));
                g.DrawLine(dashPen, leftUpperPoint, leftLowerPoint);
                g.DrawLine(dashPen, leftMiddlePoint, new PointF(50, leftMiddlePoint.Y));
                g.DrawLine(dashPen, leftLowerPoint, new PointF(50, leftLowerPoint.Y));
            }
        }

        private void UpdateMoreOptionsCollapseState(bool showMoreOptionsCollapsed)
        {
            if (showMoreOptionsCollapsed)
            {
                this.Height = 240;
                this.plMoreOptions.BackgroundImage = Properties.Resources.toolbar_actions_model_arrowdown;
            }
            else
            {
                this.Height = 414;
                this.plMoreOptions.BackgroundImage = Properties.Resources.toolbar_actions_model_arrowup;
            }

            this.plFooter.Top = this.Height - this.plFooter.Height - 56;
            this._showMoreOptionsCollapsed = !showMoreOptionsCollapsed;
            frmStudioMain.SceneControl.Render();
        }

        private void plMoreOptions_Click(object sender, EventArgs e)
        {
            UpdateMoreOptionsCollapseState(_showMoreOptionsCollapsed);
            UpdateLinkedLines();
        }

        private void lblMoreOptions_Click(object sender, EventArgs e)
        {
            this.plMoreOptions_Click(null, null);
        }

        private void lblXValue_Click(object sender, EventArgs e)
        {
            //ChangeScaleFactorType();
            //dropdownListItems1.Width = this.lblXValue.Width;
           // var clientRect = this.lblXValue.PointToScreen(new Point(0, this.lblXValue.Height));
            //dropdownListItems1.Show(clientRect);

        }

        private void lblXValueType_Click(object sender, EventArgs e)
        {
            lblXValue_Click(null, null);
        }

        private void btnXTypeDropdown_Click(object sender, EventArgs e)
        {
            lblXValue_Click(null, null);
        }

        private void ChangeScaleFactorType()
        {
            //if (this.lblXValueType.Text.ToLower().Contains("milli"))
            //{
            //    this.dropdownListItems1.Items[0].Text = "Percentage";
            //}
            //else
            //{
            //    this.dropdownListItems1.Items[0].Text = "Millimeter";
            //}
        }
        
        private void lblYValue_Click(object sender, EventArgs e)
        {
            //ChangeScaleFactorType();
            //dropdownListItems1.Width = this.lblYValue.Width;
            //var clientRect = this.lblYValue.PointToScreen(new Point(0, this.lblYValue.Height));
            //dropdownListItems1.Show(clientRect);
        }

        private void lblYValueType_Click(object sender, EventArgs e)
        {
            lblYValue_Click(null, null);
        }

        private void btnYTypeDropdown_Click(object sender, EventArgs e)
        {
            lblYValue_Click(null, null);
        }

        private void lblZValue_Click(object sender, EventArgs e)
        {
            //ChangeScaleFactorType();
            //dropdownListItems1.Width = this.lblZValue.Width;
            //var clientRect = this.lblZValue.PointToScreen(new Point(0, this.lblZValue.Height));
            //dropdownListItems1.Show(clientRect);
        }

        private void lblZValueType_Click(object sender, EventArgs e)
        {
            lblZValue_Click(null, null);
        }

        private void btnZTypeDropdown_Click(object sender, EventArgs e)
        {
            lblZValue_Click(null, null);
        }

        private void menuItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.lblXValueType.Text = this.dropdownListItems1.Items[0].Text;
            //this.lblYValueType.Text = this.dropdownListItems1.Items[0].Text;
            //this.lblZValueType.Text = this.dropdownListItems1.Items[0].Text;

            this.txtXValue.DisableTextChangeTrigger();
            this.txtYValue.DisableTextChangeTrigger();
            this.txtZValue.DisableTextChangeTrigger();

            if (!this.IsFactorValue)
            {
                //get values in mm
                this.txtXValue.Value = (float)Math.Round(this._dataSource.Width, 2);
                this.txtYValue.Value = (float)Math.Round(this._dataSource.Depth, 2);
                this.txtZValue.Value = (float)Math.Round(this._dataSource.Height, 2);
            }
            else
            {
                //get values in percentage
                this.txtXValue.Value = (float)Math.Round(_dataSource.ScaleFactorX, 2) * 100;
                this.txtYValue.Value = (float)Math.Round(_dataSource.ScaleFactorY, 2) * 100;
                this.txtZValue.Value = (float)Math.Round(_dataSource.ScaleFactorZ, 2) * 100;
            }

            this.txtXValue.EnableTextChangeTrigger();
            this.txtYValue.EnableTextChangeTrigger();
            this.txtZValue.EnableTextChangeTrigger();

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (this.IsLinked)
            {
                if (this.IsFactorValue)
                {
                    this._dataSource.Scale(this.txtXValue.Value / 100, this.txtYValue.Value / 100, this.txtZValue.Value / 100, ScaleEventArgs.TypeAxis.ALL, true, true);

                    this.txtUniformScaleFactor.Value = this.txtXValue.Value / 100;
                }
                else
                {
                    if ((float)this.txtXValue.Tag != this._dataSource.ScaleFactorX)
                    {
                        this._dataSource.Scale((float)this.txtXValue.Tag, (float)this.txtYValue.Tag, (float)this.txtZValue.Tag, ScaleEventArgs.TypeAxis.ALL, true, true);

                        this.txtUniformScaleFactor.Value = (float)this.txtXValue.Tag;
                    }
                }
            }
            else
            {
                if (this.IsFactorValue)
                {
                    this._dataSource.Scale(this.txtXValue.Value / 100, this.txtYValue.Value / 100, this.txtZValue.Value / 100, ScaleEventArgs.TypeAxis.ALL, true, true);
                }
                else
                {
                    this._dataSource.Scale(this.txtXValue.Value / this._dataSource.Width, this.txtYValue.Value / this._dataSource.Depth, this.txtZValue.Value / this._dataSource.Height, ScaleEventArgs.TypeAxis.ALL, true, true);

                    this.txtUniformScaleFactor.Value = this._dataSource._scaleFactorX;
                }
            }

            this._dataSource.UpdateBinding();
            this._dataSource.SupportStructure.Clear();
            this._dataSource.SupportBasement = false;
            SceneControlToolbarManager.UpdateModelDimensions(this._dataSource.Width, this._dataSource.Depth, this._dataSource.Height);
            frmStudioMain.SceneControl.Render();
        }

        private void UpdateLinkedValues(float value, bool xValue, bool yValue = false, bool zValue = false)
        {
            this.txtXValue.DisableTextChangeTrigger();
            this.txtYValue.DisableTextChangeTrigger();
            this.txtZValue.DisableTextChangeTrigger();

            if (this.IsFactorValue)
            {
                if (this.IsLinked)
                {
                    if (this.txtXValue.Value != value) { this.txtXValue.Value = value; }
                    if (this.txtYValue.Value != value) { this.txtYValue.Value = value; }
                    if (this.txtZValue.Value != value) { this.txtZValue.Value = value; }
                }
            }
            else
            {
                if (this.IsLinked)
                {
                    if (xValue)
                    {
                        var scaleFactor = value / this._dataSource.Width;
                        this.txtYValue.Value = (this._dataSource.Depth / this._dataSource.ScaleFactorY) ;
                        this.txtYValue.Value *= scaleFactor;
                        this.txtYValue.Tag = (this._dataSource.Depth / this._dataSource.ScaleFactorY);
                        this.txtZValue.Value = (this._dataSource.Height / this._dataSource.ScaleFactorZ);
                        this.txtZValue.Value *= scaleFactor;
                        this.txtZValue.Tag = (this._dataSource.Height / this._dataSource.ScaleFactorX);

                        this.txtXValue.Tag = scaleFactor * this._dataSource.ScaleFactorX;

                        
                    }
                    else if (yValue)
                    {

                        var scaleFactor = value / (this._dataSource.Depth / this._dataSource.ScaleFactorX);
                        this.txtXValue.Value = (this._dataSource.Width / this._dataSource.ScaleFactorX) * scaleFactor;
                        this.txtXValue.Tag = this.txtXValue.Value;
                        this.txtZValue.Value = (this._dataSource.Height / this._dataSource.ScaleFactorZ) * scaleFactor;
                        this.txtZValue.Tag = this.txtZValue.Value;

                        this.txtYValue.Tag = (this._dataSource.Depth / this._dataSource.ScaleFactorY) * scaleFactor;
                    }
                    else if (zValue)
                    {
                        var scaleFactor = value / this._dataSource.Height;
                        this.txtXValue.Value = (this._dataSource.Width / this._dataSource.ScaleFactorX) * scaleFactor;
                        this.txtXValue.Tag = this.txtXValue.Value;
                        this.txtYValue.Value = (this._dataSource.Depth / this._dataSource.ScaleFactorY) * scaleFactor;
                        this.txtYValue.Tag = this.txtYValue.Value;

                        this.txtZValue.Tag = (this._dataSource.Height / this._dataSource.ScaleFactorZ) * scaleFactor;
                    }
                }
                else
                {
                    
                }
            }

            this.txtXValue.EnableTextChangeTrigger();
            this.txtYValue.EnableTextChangeTrigger();
            this.txtZValue.EnableTextChangeTrigger();
        }
    }
}
