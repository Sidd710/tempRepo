using Atum.DAL.Helpers;
using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.MaterialDisplay;
using Atum.Studio.Core.Managers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.MaterialEditor
{
    public partial class frmMaterialEditor : NewGUIFormBase
    {
        
        private readonly int defaultSummaryHeight = 40;

        private bool _materialDisplayNameChanged = false;

        private MaterialMenuStrip _materialMenuStrip;

        public event EventHandler onSelectionChanged;

        public Material SelectedMaterial
        {
            get
            {
                var selectedMaterial = (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<MaterialSummary>()).Where(o => o.Selected).FirstOrDefault();
                if (selectedMaterial!= null && selectedMaterial.Material != null)
                {
                    return selectedMaterial.Material;
                }
                else
                {
                    return null;
                }
            }
        }

        public string SelectedSupplier
        {
            get
            {
                var selectedMaterial = (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<MaterialSummary>()).Where(o => o.Selected).FirstOrDefault();
                if (selectedMaterial != null && selectedMaterial.Material != null)
                {
                    return selectedMaterial.Supplier;
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public frmMaterialEditor()
        {
            InitializeComponent();

            this._materialMenuStrip = new MaterialMenuStrip();
            this._materialMenuStrip.onMaterialAdded += frmMaterialPopup_onMaterialAdded;
            this.newGUIContentSplitContainerBase1.LeftPanel.MouseMove += LeftPanel_MouseMove;
            this.newGUIContentSplitContainerBase1.splitContainer1.SplitterDistance = 300;
            this.spcFooterContainer.SplitterDistance = this.newGUIContentSplitContainerBase1.splitContainer1.SplitterDistance;

            //remove scrollbars
            this.newGUIContentSplitContainerBase1.LeftPanel.AutoScroll = false;
            this.newGUIContentSplitContainerBase1.LeftPanel.HorizontalScroll.Enabled = false;
            this.newGUIContentSplitContainerBase1.LeftPanel.HorizontalScroll.Visible = false;
            this.newGUIContentSplitContainerBase1.LeftPanel.HorizontalScroll.Maximum = 0;
            this.newGUIContentSplitContainerBase1.LeftPanel.AutoScroll = true;

            this.Load += FrmMaterialEditor_Load;

            this.btnApply.ForeColor = BrandingManager.Button_ForeColor;
            this.btnApply.BackColor = this.btnApply.BorderColor = BrandingManager.Button_BackgroundColor_LightDark;
        }

        private void FrmMaterialEditor_Load(object sender, EventArgs e)
        {
            var netFabbHandle = WindowHelper.GetNetFabbWindowHandle();
          //  WindowHelper.SetWindowAsTopMost(this.Handle);
        }

        private void LeftPanel_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMenu();
        }

        public void ShowMenu()
        {
            if (!this.Controls.Contains(_materialMenuStrip))
            {
                _materialMenuStrip.Top = this.newGUIContentSplitContainerBase1.LeftPanel.Height - this._materialMenuStrip.Height;
                _materialMenuStrip.Left = 16;
                _materialMenuStrip.Visible = true;
                this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(_materialMenuStrip);
            }
            _materialMenuStrip.BringToFront();
        }

        public void CloseMenu()
        {
            if (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Contains(_materialMenuStrip))
            {
                this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Remove(_materialMenuStrip);
            }
        }


        private void newGUIContentSplitContainerBase1_Load(object sender, EventArgs e)
        {
            if (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Count == 0)
            {
                LoadMaterials();
            }
        }

        public void LoadMaterials(Material selectedMaterial = null)
        {
            this.newGUIContentSplitContainerBase1.RightPanel.Controls.Clear();
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Clear();

            var materialValueEditor = new MaterialValueEditor();
            materialValueEditor.Dock = DockStyle.Fill;
            materialValueEditor.onTextChanged += MaterialValueEditor_onTextChanged;
            this.newGUIContentSplitContainerBase1.RightPanel.Controls.Add(materialValueEditor);

            var summaryHeight = 0;
            var firstMaterial = true;
            MaterialSummary selectedMaterialSummary = null;

            var selectedPrinter = PrintJobManager.SelectedPrinter;


            var orderedBindingList = new List<MaterialsBySupplier>();
            foreach (var materialSupplier in MaterialManager.Catalog)
            {
                foreach (var material in materialSupplier.GetMaterialsByResolution(selectedPrinter))
                {
                    orderedBindingList.Add(material);
                }
            }

            var orderedList = orderedBindingList.OrderBy(s => s.Materials[0].DisplayName).ToList();


            foreach (var material in orderedList)
            {
                MaterialSummary materialSummary = PushMaterialInControl(summaryHeight, material.Supplier, material.Materials[0]);
                materialSummary.Width = this.newGUIContentSplitContainerBase1.splitContainer1.SplitterDistance;

                if (selectedMaterial != null && selectedMaterial.Id == material.Materials[0].Id)
                {
                    selectedMaterialSummary = materialSummary;
                }

                if (firstMaterial)
                {
                    materialSummary.Selected = true;
                    firstMaterial = false;

                    this.UpdateRightPanel(materialSummary);
                }

                summaryHeight += defaultSummaryHeight;

            }

            if (selectedMaterial != null)
            {
                this.MaterialSummary_onSelected(selectedMaterialSummary, null);
            }
        }

        private MaterialSummary PushMaterialInControl(int summaryHeight, string supplier, Material material)
        {
            if (material.Id == null || material.Id == Guid.Empty)
            {
                material.Id = Guid.NewGuid();
            }
            var materialSummary = new MaterialSummary();
            materialSummary.Id = material.Id;
            materialSummary.onSelected += MaterialSummary_onSelected;
            materialSummary.Material = material;
            materialSummary.Supplier = supplier;
            materialSummary.Left = 0;
            materialSummary.Top = summaryHeight;
            materialSummary.UpdateControl();
            this.newGUIContentSplitContainerBase1.LeftPanel.Controls.Add(materialSummary);
            return materialSummary;
        }

        private void MaterialValueEditor_onTextChanged(object sender, EventArgs e)
        {
            var materialValueEditor = sender as MaterialValueEditor;
            foreach (MaterialSummary materialSummary in this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<MaterialSummary>())
            {
                if (materialSummary.Selected)
                {
                    materialSummary.Material = materialValueEditor.Material;
                    materialSummary.Supplier = materialValueEditor.Supplier;
                    _materialDisplayNameChanged = true;
                }
            }

            //materialValueEditor.Focus();
        }

        private void MaterialSummary_onSelected(object sender, EventArgs e)
        {
            foreach (MaterialSummary materialSummary in this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<MaterialSummary>())
            {
                if (materialSummary.Selected)
                {
                    materialSummary.Selected = false;
                }
            }

            if (sender != null)
            {
                var selectedMaterialSummary = sender as MaterialSummary;
                selectedMaterialSummary.Selected = true;
                this.UpdateRightPanel(selectedMaterialSummary);
                onSelectionChanged?.Invoke(sender, null);
            }
        }

        private void UpdateRightPanel(MaterialSummary selectedMaterial)
        {
            var materialEditor = this.newGUIContentSplitContainerBase1.RightPanel.Controls[0] as MaterialValueEditor;
            materialEditor.Supplier = selectedMaterial.Supplier;
            materialEditor.Material = selectedMaterial.Material;
        }

        private void pbPlusSign_Click(object sender, EventArgs e)
        {
            ShowMenu();
        }
        private void frmMaterialPopup_onMaterialAdded(object sender, EventArgs e)
        {
            if (sender is frmMaterialOnlineCatalog)
            {
                MaterialManager.SaveAllMaterials();
                LoadMaterials();
            }
            else if (sender is MaterialWithSupplierModel)
            {
                MaterialManager.SaveAllMaterials();
                LoadMaterials();
            }

            
        }


        private void pbMinusSign_Click(object sender, EventArgs e)
        {
            var selected = (this.newGUIContentSplitContainerBase1.LeftPanel.Controls.OfType<MaterialSummary>()).Where(o => o.Selected).FirstOrDefault();
            if (new frmMessageBox("Remove selected material", string.Format("Are you sure you want to remove: {0}?", selected.Material.DisplayName), MessageBoxButtons.YesNo, MessageBoxDefaultButton.Button3).ShowDialog() == DialogResult.Yes)
            {
                foreach (var materialSupplier in MaterialManager.Catalog)
                {
                    foreach (var material in materialSupplier.Materials)
                    {
                        if (material.Id == selected.Material.Id)
                        {
                            materialSupplier.Materials.Remove(material);
                            MaterialManager.SaveAllMaterials();
                            LoadMaterials();
                            break;
                        }
                    }
                }
            }
        }

        private void pbMinusSign_MouseMove(object sender, MouseEventArgs e)
        {
            CloseMenu();
        }

        private void spcFooterContainer_Panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Location.X > (pbMinusSign.Left / 2))
            {
                CloseMenu();
            }
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            if (_materialDisplayNameChanged)
            {
                MaterialManager.SaveAllMaterials();
            }

            this.Close();
        }
    }
}
