using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Atum.DAL.Materials;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    public partial class MaterialSummary : UserControl
    {
        public event EventHandler onSelected;

        internal bool DisableMouseClick { get; set; }
        internal bool DisableHighlight{get; set;}

        private string _supplier;
        private Material _material;

        private bool _selected;

        public bool Selected
        {
            get
            {
                return this._selected;
            }
            set
            {
                this._selected = value;
                this.UpdateControl();
                if (this._selected)
                    this.OnEnterChange();
                   this.OnLeaveChange();
            }
        }

        public string Supplier
        {
            get
            {
                return this._supplier;
            }
            set
            {
                this._supplier = value;
            }
        }

        public Material Material
        {
            get
            {
                return this._material;
            }
            set
            {
                this._material = value;
                this.UpdateControl();
            }
        }
        public Guid Id
        {
            get; set;
        }

        public MaterialSummary()
        {
            InitializeComponent();

            this.lblMaterialName.Font = FontManager.Montserrat16Regular;
            this.picColor.BorderColor = BrandingManager.Button_ForeColor_Dark;
            this.picColor.BorderThickness = 1;
        }

        public void UpdateControl()
        {
            if (this._material != null)
            {
                this.Id = this._material.Id;

                if (this.lblMaterialName.InvokeRequired)
                {
                    this.lblMaterialName.Invoke(new MethodInvoker(delegate
                    {
                        if (this._material != null)
                        {
                            this.lblMaterialName.Text = this._material.DisplayName;
                        }
                    }));
                }

                else
                {
                    if (this._material != null)
                    {
                        this.lblMaterialName.Text = this._material.DisplayName;
                    }
                }

                if (this._material != null)
                {
                    try
                    {
                        if (this.picColor.InvokeRequired)
                        {
                            this.picColor.Invoke(new MethodInvoker(delegate { this.picColor.BackColor = this._material.ModelColor == null ? Color.Gray : this._material.ModelColor; }));
                        }
                        else
                        {
                            this.picColor.BackColor = this._material.ModelColor == null ? Color.Gray : this._material.ModelColor;
                        }
                    }
                    catch
                    {

                    }
                }
            }
            
            //this.Select();

        }

        private void txtMaterialText_Click(object sender, EventArgs e)
        {
            if (!DisableMouseClick)
            {
                this.onSelected?.Invoke(this, null);

                if (!DisableHighlight)
                {
                    OnEnterChange();
                }
            }
        }

        private void plMaterialSummary_Leave(object sender, EventArgs e)
        {

        }

        //private void plMaterialSummary_MouseEnter(object sender, EventArgs e)
        //{
        //    if (!DisableHighlight)
        //    {
        //        OnEnterChange();
        //    }
        //}

        private void OnEnterChange()
        {
            if (!DisableMouseClick)
            {
                plMaterialSummary.BackColor = BrandingManager.Menu_Item_HighlightColor;
                picColor.Invalidate();
                picColor.Refresh();
                lblMaterialName.ForeColor = Color.White;
            }
        }
        private void OnLeaveChange()
        {
            if (!DisableMouseClick)
            {
                if (!this.Selected)
                {
                    plMaterialSummary.BackColor = Color.White;
                    lblMaterialName.ForeColor = Color.Black;

                    picColor.Invalidate();
                    picColor.Refresh();
                }
            }
        }

        //private void plMaterialSummary_MouseLeave(object sender, EventArgs e)
        //{
        //    if (!DisableHighlight)
        //    {
        //        OnLeaveChange();
        //    }
        //}


        //private void lblMaterialName_MouseEnter(object sender, EventArgs e)
        //{
        //    if (!DisableHighlight)
        //    {
        //        OnEnterChange();
        //    }
        //}

        //private void lblMaterialName_MouseLeave(object sender, EventArgs+ e)
        //{
        //    if (!DisableHighlight)
        //    {
        //        OnLeaveChange();
        //    }
        //}

        //private void picColor_MouseEnter(object sender, EventArgs e)
        //{
        //    if (!DisableHighlight)
        //    {
        //        OnEnterChange();
        //    }
        //}

        //private void picColor_MouseLeave(object sender, EventArgs e)
        //{
        //    OnLeaveChange();
        //}
    }
}
