using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using Atum.Studio.Controls.NewGui.PrinterEditorSettings;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using Atum.Studio.Core.Shapes;

namespace Atum.Studio.Controls.OpenGL
{
    public partial class SceneControlPrintJobPropertiesToolbar : SceneControlToolbars.SceneControlToolbarBase
    {
        private Control _parentForm;
        internal event EventHandler<AtumPrinter> SelectedPrinterChanged;
        internal event EventHandler<MaterialSummary> SelectedMaterialChanged;
        public event EventHandler OnEscKeyPressed;

        internal string PrintjobName
        {
            get
            {
                var printjobName = string.Empty;
                if (this.txtPrintJobName.InvokeRequired)
                {
                    this.txtPrintJobName.Invoke(new MethodInvoker(delegate
                    {
                        printjobName = this.txtPrintJobName.Text;
                    }));
                }
                else
                {
                    printjobName = this.txtPrintJobName.Text;
                }

                return printjobName;
            }
            set
            {
                if (this.txtPrintJobName.InvokeRequired)
                {
                    this.txtPrintJobName.Invoke(new MethodInvoker(delegate
                    {
                        this.txtPrintJobName.Text = value;
                    }));
                }
                else
                {
                    this.txtPrintJobName.Text = value;
                }
            }
        }

        internal AtumPrinter SelectedPrinter
        {
            get
            {
                return this.cbPrinters.SelectedItem as AtumPrinter;
            }
            set
            {
                this.cbPrinters.SelectedItem = value;
                UserProfileManager.UserProfiles[0].SelectedPrinter = value;
                UserProfileManager.Save();
            }
        }

        internal MaterialSummary SelectedMaterial
        {
            get
            {
                if (this.cbMaterials.SelectedItem != null)
                {
                    return (this.cbMaterials.SelectedItem as MaterialSummary);
                }
                return null;

            }
            set
            {
                if (this.cbMaterials.SelectedItem != null)
                {
                    SelectedMaterialChanged?.Invoke(null, value);
                }
            }
        }

        internal void StartPrinterWizard()
        {
            CbPrinters_DropdownButtonClicked(null, null);
        }

        internal bool IsPrintJobNameFocused
        {
            get
            {
                return this.txtPrintJobName.Focused;
            }
        }

        internal string ModelWidth
        {
            set
            {
                this.lblWidthValue.Text = value;
            }
        }

        internal string ModelDepth
        {
            set
            {
                this.lblDepthValue.Text = value;
            }
        }

        internal string ModelHeight
        {
            set
            {
                this.lblHeightValue.Text = value;
            }
        }

        internal void ResetPrintJobModelDimensions()
        {
            this.ModelDepth = "-";
            this.ModelHeight = "-";
            this.ModelWidth = "-";
        }


        public SceneControlPrintJobPropertiesToolbar(Control parent)
        {
            InitializeComponent();

            if (parent != null)
            {
                this.Visible = true;

                this._parentForm = parent;

                this.UpdateControl();
            }
        }

        internal void UpdateControl()
        {
            this.UpdatePosition();

            this.txtPrintJobName.Font = FontManager.Montserrat16Regular;
            if (RenderEngine.PrintJob != null && !string.IsNullOrEmpty(RenderEngine.PrintJob.Name))
            {
                this.txtPrintJobName.Text = RenderEngine.PrintJob.Name;
            }
            else
            {
                this.txtPrintJobName.Text = "Untitled";
            }


            if (UserProfileManager.UserProfile != null && UserProfileManager.UserProfile.SelectedPrinter != null)
            {
                var defaultPrinter = UserProfileManager.UserProfile.SelectedPrinter;
                if (defaultPrinter != null)
                {
                    this.cbPrinters.SelectedItem = defaultPrinter;
                }
            }

            if (UserProfileManager.UserProfile != null && UserProfileManager.UserProfile.SelectedMaterial != null)
            {
                var defaultMaterial = UserProfileManager.UserProfile.SelectedMaterial;
                var materialSummary = new MaterialSummary();
                if (defaultMaterial != null)
                {
                    materialSummary.Material = defaultMaterial;

                    var supplierName = string.Empty;
                    foreach (var supplier in MaterialManager.Catalog)
                    {
                        if (supplier.FindMaterialById(defaultMaterial.Id) != null)
                        {
                            supplierName = supplier.Supplier;
                            materialSummary.Supplier = supplierName;
                            break;
                        }
                    }

                    materialSummary.UpdateControl();
                    this.cbMaterials.SelectedItem = materialSummary;

                    //update model colors
                    ObjectView.UpdateModelColors(defaultMaterial.ModelColor);
                }
            }


        }

        private void SceneControlPrintJobPropertiesToolbar_Load(object sender, EventArgs e)
        {
            this.cbPrinters.DropdownButtonClicked += CbPrinters_DropdownButtonClicked;
            this.cbMaterials.DropdownButtonClicked += CbMaterials_DropdownButtonClicked;
        }

        private void CbPrinters_DropdownButtonClicked(object sender, EventArgs e)
        {
            var currentPrinter = cbPrinters.SelectedItem as AtumPrinter;
            using (var printerEditor = new frmPrinterEditor())
            {
                if (cbPrinters.SelectedItem != null)
                {
                    printerEditor.LoadPrinters(cbPrinters.SelectedItem as AtumPrinter);
                }

                printerEditor.ShowDialog(this);

                while (printerEditor.SelectedPrinter == null)
                {
                    printerEditor.ShowDialog(this);
                }

                if (cbPrinters.SelectedItem != printerEditor.SelectedPrinter)
                {
                    this.SelectedPrinterChanged?.Invoke(printerEditor.SelectedPrinter, null);
                    cbPrinters.SelectedItem = printerEditor.SelectedPrinter;

                    if (printerEditor.SelectedPrinter != null)
                    {
                        UserProfileManager.UserProfiles[0].SelectedPrinter = printerEditor.SelectedPrinter;
                        UserProfileManager.Save();
                    }
                }
            }

            var selectedPrinter = cbPrinters.SelectedItem as AtumPrinter;

            if (currentPrinter == null || selectedPrinter.PrinterXYResolution != currentPrinter.PrinterXYResolution || selectedPrinter.GetType() != currentPrinter.GetType())
            {
                var printerMaterials = new BindingList<MaterialsBySupplier>();
                foreach (var materialSupplier in MaterialManager.Catalog)
                {
                    foreach (var material in materialSupplier.GetMaterialsByResolution(selectedPrinter))
                    {
                        printerMaterials.Add(material);
                    }
                }
                if (printerMaterials.Count > 0)
                {
                    var materialSummary = new MaterialSummary();
                    materialSummary.Material = printerMaterials.OrderBy(m => m.Materials[0].Name).First().Materials[0];
                    this.cbMaterials.SelectedItem = materialSummary;

                    ObjectView.UpdateModelColors(materialSummary.Material.ModelColor);

                    if (SceneView.CurrentViewMode == SceneView.ViewMode.MagsAI)
                    {
                        var selectedModel = ObjectView.SelectedModel;
                        if (selectedModel != null)
                        {
                            selectedModel.ChangeTrianglesToBlendViewMode(true);
                        }
                    }
                    UserProfileManager.UserProfile.SelectedMaterial = materialSummary.Material;
                    UserProfileManager.Save();

                }
            }
        }

        private void cbPrinters_Click(object sender, EventArgs e)
        {
            SceneControlToolbarManager.PrintJobPropertiesToolbar.DeselectPrintJobName();
            this.CbPrinters_DropdownButtonClicked(null, null);
        }

        private void CbMaterials_DropdownButtonClicked(object sender, EventArgs e)
        {
            SceneControlToolbarManager.PrintJobPropertiesToolbar.DeselectPrintJobName();
            using (var materialEditor = new frmMaterialEditor())
            {
                materialEditor.KeyDown += MaterialEditor_KeyDown;
                if (cbMaterials.SelectedItem != null)
                {
                    materialEditor.LoadMaterials((cbMaterials.SelectedItem as MaterialSummary).Material);
                }
                materialEditor.ShowDialog();


                if (materialEditor.SelectedMaterial != null)
                {
                    var materialSummary = new MaterialSummary();
                    materialSummary.Material = materialEditor.SelectedMaterial;
                    materialSummary.Supplier = materialEditor.SelectedSupplier;
                    materialSummary.UpdateControl();
                    this.cbMaterials.SelectedItem = materialSummary;

                    SelectedMaterialChanged?.Invoke(null, materialSummary);
                }
            }
        }

        internal new void UpdatePosition()
        {
            if (this._parentForm != null)
            {
                this.Top = this._parentForm.Height - this.Height - 16;
                this.Left = (this._parentForm.Width / 2) - (this.Width / 2);
            }
        }

        private void cbMaterials_Click(object sender, EventArgs e)
        {
            this.CbMaterials_DropdownButtonClicked(null, null);
        }

        private void txtPrintJobName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyData == Keys.Enter)
            {
                DeselectPrintJobName();


            }
        }

        private void MaterialEditor_KeyDown(object sender, KeyEventArgs e)
        {
            var materialEditor = (frmMaterialEditor)sender;
            if (e.KeyCode == Keys.Escape)
            {
                materialEditor.Close();
                //here parent 
                OnEscKeyPressed.Invoke(materialEditor, null);
            }
        }

        private void txtPrintJobName_MouseClick(object sender, MouseEventArgs e)
        {
            TouchScreenManager.ShowOSK(this.txtPrintJobName);
        }

        internal void DeselectPrintJobName()
        {
            if (this.txtPrintJobName.InvokeRequired)
            {
                this.txtPrintJobName.Invoke(new MethodInvoker(delegate
                {
                    this.txtPrintJobName.Enabled = false;
                    this.txtPrintJobName.Enabled = true;
                }));
            }
            else
            {
                this.txtPrintJobName.Enabled = false;
                this.txtPrintJobName.Enabled = true;
            }
            
        }
    }
}
