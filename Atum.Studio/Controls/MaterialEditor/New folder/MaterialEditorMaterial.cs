using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.DAL.Materials;
using Atum.Studio.Core.Managers;

namespace Atum.Studio.Controls.MaterialEditor
{
    public partial class MaterialEditorMaterial : UserControl
    {
        public event EventHandler<MaterialDisplayNameArgs> DisplayNameChanged;
        
        public Material CurrentMaterial
        {
            get
            {
                return (Material)this.materialBindingSource.DataSource;
            }
            set
            {
                if (value != null)
                {
                    if (string.IsNullOrEmpty(value.DisplayName) && string.IsNullOrEmpty(value.Name))
                    {
                        //new material save defaults
                        value.InitialLayers = Convert.ToInt16(this.txtInitialLayers.Value);
                        value.LT1 = Convert.ToDouble(this.txtLT1.Value);
                        value.LT2 = Convert.ToDouble(this.txtLT2.Value);
                        value.RT1 = Convert.ToDouble(this.txtRT1.Value);
                        value.RT2 = Convert.ToDouble(this.txtRT2.Value);
                        value.CT1 = Convert.ToDouble(this.txtCT1.Value);
                        value.CT2 = Convert.ToDouble(this.txtCT2.Value);
                        value.RH1 = Convert.ToDouble(this.txtRH1.Value);
                        value.RH2 = Convert.ToDouble(this.txtRH2.Value);
                        value.RSU1 = Convert.ToDouble(this.txtRSU1.Value);
                        value.RSU2 = Convert.ToDouble(this.txtRSU2.Value);
                        value.TAT1 = Convert.ToDouble(this.txtTAT1.Value);
                        value.TAT2 = Convert.ToDouble(this.txtTAT2.Value);
                        value.RSD1 = Convert.ToDouble(this.txtRSD1.Value);
                        value.RSD2 = Convert.ToDouble(this.txtRSD2.Value);
                    }
                }
                
                this.materialBindingSource.DataSource = value;

             
                //       this.txtInitialLayers_ValueChanged(null, null);
            }
        }

        public MaterialEditorMaterial()
        {
            InitializeComponent();

            this.txtDisplayName.Focus();

            this.tbLayers.Height = (int)(this.tableLayoutPanel3.Height * DisplayManager.GetResolutionScaleFactor()) + this.tbLayers.GetTabRect(0).Height;

        }

        private void txtInitialLayers_ValueChanged(object sender, EventArgs e)
        {
            if (this.txtInitialLayers.Value >= 1)
            {
                if (!this.tbLayers.TabPages.Contains(this.tbFirstLayers))
                {
                    this.tbLayers.TabPages.Insert(1,this.tbFirstLayers);
                }
            }
            else if (this.txtInitialLayers.Value == 0)
            {
                if (this.tbLayers.TabPages.Contains(this.tbFirstLayers))
                {
                    this.tbLayers.TabPages.Remove(this.tbFirstLayers);
                }
            }
        }

        private void MaterialEditorMaterial_Load(object sender, EventArgs e)
        {
            if (this.txtInitialLayers.Value == 0)
            {
                this.tbLayers.TabPages.Remove(this.tbFirstLayers);
            }

            this.tbLayers.Top = this.txtDisplayName.Top + this.txtDisplayName.Height + 10;
        }

        private void txtDisplayName_TextChanged(object sender, EventArgs e)
        {
            if (this.txtDisplayName.Text.Length > 0)
            {
                if (DisplayNameChanged != null)
                {
                    this.DisplayNameChanged(null, new MaterialDisplayNameArgs() { DisplayName = this.txtDisplayName.Text, Material = this.CurrentMaterial });
                }
            }
        }

        private void plModelColor_MouseClick(object sender, MouseEventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                this.plModelColor.BackColor = colorDialog1.Color;
            }
        }
    }
}
