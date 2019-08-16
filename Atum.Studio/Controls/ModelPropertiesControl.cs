using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Text;
using System.Windows.Forms;
using Atum.Studio;
using Atum.Studio.Core.Events;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using Atum.DAL.Materials;

namespace Atum.Studio.Controls
{
    public partial class ModelPropertiesControl : UserControl
    {
        private ModelBrowserInfo _modelBrowser;
        private MaterialsCatalog _materialCatalog;

        internal event EventHandler<SelectionEventArgs> trSelectionChanged;




        //internal int InitialLayers { get { return (int)this.txtInitialLayers.Value; } }
        //internal Material Material
        //{
        //    get
        //    {
        //        var material = new Material();
        //        material.CT1 = (double)this.txtCT1.Value;
        //        material.CT2 = (double)this.txtCT2.Value;
        //        material.LT1 = (double)this.txtLT1.Value;
        //        material.LT2 = (double)this.txtLT2.Value;
        //        material.RH1 = (double)this.txtRH1.Value;
        //        material.RH2 = (double)this.txtRH2.Value;
        //        material.RSD1 = (double)this.txtRSD1.Value;
        //        material.RSD2 = (double)this.txtRSD2.Value;
        //        material.RSU1 = (double)this.txtRSU1.Value;
        //        material.RSU2 = (double)this.txtRSU2.Value;
        //        material.RT1 = (double)this.txtRT1.Value;
        //        material.RT2 = (double)this.txtRT1.Value;
        //        material.TAT1 = (double)this.txtTAT1.Value;
        //        material.TAT2 = (double)this.txtTAT2.Value;
        //        return material;
        //    }
        //}

        public TypeModelProperty ModelPropertyType { get; set; }



        public enum TypeModelProperty
        {
            Model = 15,
            ModelSupport = 16,
        }

        private delegate void SetModelBrowserDataSourceCallback(ModelBrowserInfo modelBrowser);
        internal void SetModelBrowserDataSource(ModelBrowserInfo modelBrowser)
        {
            if (this.trModel.InvokeRequired)
            {
                var d = new SetModelBrowserDataSourceCallback(SetModelBrowserDataSource);
                this.Invoke(d, new object[] { modelBrowser });
            }
            else
            {
                this.trModel.Nodes.Clear();
                this._modelBrowser = modelBrowser;

                if (this._modelBrowser != null)
                {
                    var modelItem = new TreeNode(modelBrowser.FileName);
                    modelItem.Tag = modelBrowser.ModelIndex;
                    var partsNode = new TreeNode("Parts");
                    foreach (var modelPart in modelBrowser.Parts)
                    {
                        partsNode.Nodes.Add(new TreeNode("Part: " + modelPart));
                    }
                    modelItem.Nodes.Add(partsNode);
                    this.trModel.Nodes.Add(modelItem);

                    //support
                    var supportNode = new TreeNode("Support");
                    for (var supportNodeIndex = 0; supportNodeIndex < this._modelBrowser.SupportStructure.Count; supportNodeIndex++)
                    {
                        var supportChildNode = new TreeNode("Cone-" + supportNodeIndex.ToString());
                        supportChildNode.Tag = supportNodeIndex.ToString();
                        supportNode.Nodes.Add(supportChildNode);
                    }

                    modelItem.Nodes.Add(supportNode);

                }
            }
        }

        private void ResetSelectedNode()
        {
            foreach (TreeNode node in this.trModel.Nodes)
            {
                foreach (TreeNode childNode in node.Nodes)
                {
                    foreach (TreeNode valueNode in childNode.Nodes)
                    {
                        valueNode.BackColor = Color.White;
                        valueNode.ForeColor = Color.Black;
                    }

                    childNode.BackColor = Color.White;
                    childNode.ForeColor = Color.Black;
                }
                node.BackColor = Color.White;
                node.ForeColor = Color.Black;
            }
        }

        internal void SetSelectedNode(STLModel3D.TypeObject modelType, int modelIndex)
        {
            ResetSelectedNode();


            if (modelType == STLModel3D.TypeObject.Model)
            {
                var rootNode = this.trModel.Nodes[0];
                this.trModel.AfterSelect -= trModelBrowser_AfterSelect;
                this.trModel.SelectedNode = rootNode;
                rootNode.BackColor = SystemColors.Highlight;
                rootNode.ForeColor = Color.White;
                this.trModel.AfterSelect += trModelBrowser_AfterSelect;
            }
            else if (modelType == STLModel3D.TypeObject.Support)
            {
                foreach (TreeNode rootNode in this.trModel.Nodes)
                {
                    foreach (TreeNode childNode in rootNode.Nodes)
                    {
                        if (childNode.Text == "Support")
                        {
                            foreach (TreeNode supportNode in childNode.Nodes)
                            {
                                if ((string)supportNode.Tag == modelIndex.ToString())
                                {
                                    this.trModel.AfterSelect -= trModelBrowser_AfterSelect;
                                    this.trModel.SelectedNode = supportNode;
                                    supportNode.BackColor = SystemColors.Highlight;
                                    supportNode.ForeColor = Color.White;
                                    this.trModel.AfterSelect += trModelBrowser_AfterSelect;
                                    return;
                                }
                            }
                        }
                    }
                }
            }


            //if (this.trModelOverview.SelectedNode != null)
            //{
            //    this.trModelOverview.SelectedNode.BackColor = SystemColors.Highlight;
            //    this.trModelOverview.SelectedNode.ForeColor = Color.White;
            //}
        }

        public ModelPropertiesControl()
        {
            InitializeComponent();

            this.tabControl1.Height += 30;
            this.tabControl1.Top -= 25;
            this.tabControl1.Left -= 5;
            //this.tabControl1.Width += 10;
        }

        private void ModelPropertiesControl_Load(object sender, EventArgs e)
        {
            this.Width = this.Parent.Width;
            switch (this.ModelPropertyType)
            {
                case TypeModelProperty.Model:
                    {
                        this.lblHeaderText.Text = "Model";
                        break;
                    }
                case TypeModelProperty.ModelSupport:
                    {
                        this.lblHeaderText.Text = "Support Options";
                        this.tabControl1.TabPages.Remove(this.tbModel);

                        this.UpdateControls();
                        break;
                    }
            }

            var currentTop = 0;
            foreach (Control control in this.tabControl1.SelectedTab.Controls)
            {
                if (control.Top > currentTop)
                {
                    currentTop = control.Top;
                }
            }

            this.Height = this.plHeader.Height + currentTop + 30;

        }

        private void plHeader_Paint(object sender, PaintEventArgs e)
        {
            var graphics = e.Graphics;

            //dashed line
            float[] dashValues = { 2, 2 };
            Pen dashedPen = new Pen(Color.Gainsboro, 1);
            dashedPen.DashPattern = dashValues;
            e.Graphics.DrawLine(dashedPen, new Point(1, this.plHeader.Height - 2), new Point(this.Width, this.plHeader.Height - 2));
        }

        internal void DisableEvents()
        {

        }

        internal void EnableEvents()
        {

        }


        private void UpdateControls()
        {
            if (Core.ModelView.ObjectView.Objects3D != null && Core.ModelView.ObjectView.SelectedModel != null)
            {
                //if (this.trModelOverview.SelectedNode != null && this.trModelOverview.SelectedNode.Tag != null)
                //{
                //    if (((STLModel3D.TypeObject)this.trModelOverview.SelectedNode.Tag) == STLModel3D.TypeObject.Model)
                //    {
                //        this.btnModelRepair.Visible = this.btnModelDetectParts.Visible = true;
                //    }
                //    else
                //    {
                //        this.btnModelRepair.Visible = this.btnModelDetectParts.Visible = false;
                //    }
                //}
            }


        }

        private void trModelBrowser_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.trModel.SelectedNode != null)
            {
                if ((this.trModel.SelectedNode.Index == 0) && this.trModel.SelectedNode.Parent == null)
                {
                    var selectionEvent = new SelectionEventArgs();
                    selectionEvent.ModelIndex = int.Parse(this.trModel.SelectedNode.Tag.ToString());
                    selectionEvent.ObjectType = STLModel3D.TypeObject.Model;
                    this.trSelectionChanged(null, selectionEvent);
                }
                else if (this.trModel.SelectedNode.Tag != null)
                {
                    var selectionEvent = new SelectionEventArgs();
                    selectionEvent.ModelIndex = int.Parse(this.trModel.Nodes[0].Tag.ToString());
                    selectionEvent.SupportIndex = int.Parse(this.trModel.SelectedNode.Tag.ToString());
                    selectionEvent.ObjectType = STLModel3D.TypeObject.Support;
                    this.trSelectionChanged(null, selectionEvent);
                }

            }
        }

        private void trModel_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            ResetSelectedNode();
        }



        //private void txtInitialLayers_ValueChanged(object sender, EventArgs e)
        //{
        //    if (this.txtInitialLayers.Value >= 1)
        //    {
        //        if (!this.tbLayers.TabPages.Contains(this.tbFirstLayers))
        //        {
        //            this.tbLayers.TabPages.Add(this.tbFirstLayers);
        //        }
        //    }
        //    else if (this.txtInitialLayers.Value == 0)
        //    {
        //        if (this.tbLayers.TabPages.Contains(this.tbFirstLayers))
        //        {
        //            this.tbLayers.TabPages.Remove(this.tbFirstLayers);
        //        }
        //    }
        //}

        //private void cbMaterialManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    this.cbMaterialProduct.DataSource = this._materialCatalog[this.cbMaterialManufacturer.SelectedIndex].Materials;
        //}


        //void RefreshMaterials()
        //{
        //    //bind manufacturer comboxbox
        //    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
        //    this._materialCatalog = new MaterialsCatalog();
        //    foreach (var supplierXML in System.IO.Directory.GetFiles(Atum.DAL.ApplicationSettings.Settings.MaterialsPath))
        //    {
        //        var supplierMaterials = (MaterialsBySupplier)serializer.Deserialize(new System.IO.StreamReader(supplierXML));
        //        this._materialCatalog.Add(supplierMaterials);
        //    }
        //    this.cbMaterialManufacturer.DataSource = this._materialCatalog;
        //}

        //private void cbMaterialProduct_SelectedIndexChanged(object sender, EventArgs e)
        //{

        //    //initial layers
        //    if (this.cbMaterialProduct.SelectedItem != null)
        //    {
        //        var selectedMaterial = (Material)this.cbMaterialProduct.SelectedItem;
        //        this.txtCT1.Value = (decimal)selectedMaterial.CT1;
        //        this.txtCT2.Value = (decimal)selectedMaterial.CT2;
        //        this.txtLT1.Value = (decimal)selectedMaterial.LT1;
        //        this.txtLT2.Value = (decimal)selectedMaterial.LT2;
        //        this.txtRH1.Value = (decimal)selectedMaterial.RH1;
        //        this.txtRH2.Value = (decimal)selectedMaterial.RH2;
        //        this.txtRT1.Value = (decimal)selectedMaterial.RT1;
        //        this.txtRT2.Value = (decimal)selectedMaterial.RT2;
        //        this.txtRSD1.Value = (decimal)selectedMaterial.RSD1;
        //        this.txtRSD2.Value = (decimal)selectedMaterial.RSD2;
        //        this.txtRSU1.Value = (decimal)selectedMaterial.RSU1;
        //        this.txtRSU2.Value = (decimal)selectedMaterial.RSU2;
        //        this.txtTAT1.Value = (decimal)selectedMaterial.TAT1;
        //        this.txtTAT2.Value = (decimal)selectedMaterial.TAT2;
        //    }
        //}

    }
}
