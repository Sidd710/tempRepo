using System;
using System.Drawing;
using System.Windows.Forms;
using Atum.DAL.Materials;
using Atum.Studio.Core.Managers;
using Atum.DAL.Print;
using Atum.Studio.Core.ModelView;
using Atum.DAL.Hardware;
using System.Diagnostics;
using Atum.DAL.Managers;
using System.Linq;

namespace Atum.Studio.Controls.Docking
{
    public partial class PrintJobPropertiesPanel : DockPanelBase
    {
        internal event EventHandler MaterialIndexChanged;

        public void CheckInitialPrinter()
        {
            if (cbDefaultPrinter.Items.Count == 0)
            {
                //using (var printConnectionWizard = new PrinterConnectionWizard.PrinterConnectionWizard())
                //{
                //    do
                //    {

                //    } while (printConnectionWizard.ShowDialog() != DialogResult.OK || printConnectionWizard.SelectedPrinter == null);

                //    printConnectionWizard.SelectedPrinter.Selected = true;
                //    printConnectionWizard.SelectedPrinter.Default = true;

                //    //if (printConnectionWizard.SelectedPrinter.FirstIPAddress != null)
                //    //{
                //    //    DAL.Network.ConnectionManager.Send(PrinterManager.DefaultPrinter, IPAddress.Parse(printConnectionWizard.SelectedPrinter.FirstIPAddress), 11000);
                //    //};
                //    PrinterManager.AvailablePrinters.Add(printConnectionWizard.SelectedPrinter);
                //    PrinterManager.Save();
                //    cbDefaultPrinter.Items.Add(printConnectionWizard.SelectedPrinter);
                //    cbDefaultPrinter.SelectedIndex = 0;

                //    //  UpdateAvailablePrinters();


                //}

            }

        }

        public string PrintJobName
        {
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
            get
            {
                return this.txtPrintJobName.Text;
            }
        }

        public int PrintJobAntiAliasFactor
        {
            set
            {
                this.trAntiAliasFactor.Value = value;
                //if (this.lblAntiAliasFactor.InvokeRequired)
                //{
                //    this.lblAntiAliasFactor.Invoke(new MethodInvoker(() => this.lblAntiAliasFactor.Text = this.trAntiAliasFactor.Value + "%"));
                //}
                //else
                //{
                //    this.lblAntiAliasFactor.Text = this.trAntiAliasFactor.Value + "%";
                //}
            }
            get
            {
                if (this.trAntiAliasFactor.InvokeRequired)
                {
                    var result = 0;
                    this.trAntiAliasFactor.Invoke(new MethodInvoker(() => result = this.trAntiAliasFactor.Value));
                    return result;
                }
                else
                {
                    return this.trAntiAliasFactor.Value;
                }
            }
        }

        //public PrintJob.AntiAliasType PrintJobAntiAliasSide
        //{
        //    set
        //    {
        //        if (this.cbAntiAliasSide.InvokeRequired)
        //        {
        //            this.cbAntiAliasSide.Invoke(new MethodInvoker(() =>
        //            {
        //                switch (value)
        //                {
        //                    case PrintJob.AntiAliasType.None:
        //                        this.cbAntiAliasSide.SelectedIndex = 0;
        //                        break;
        //                    case PrintJob.AntiAliasType.Inside:
        //                        this.cbAntiAliasSide.SelectedIndex = 1;
        //                        break;
        //                    case PrintJob.AntiAliasType.Outside:
        //                        this.cbAntiAliasSide.SelectedIndex = 2;
        //                        break;
        //                }
        //            }
        //            ));
        //        }

        //        else
        //        {
        //            switch (value)
        //            {
        //                case PrintJob.AntiAliasType.None:
        //                    this.cbAntiAliasSide.SelectedIndex = 0;
        //                    break;
        //                case PrintJob.AntiAliasType.Inside:
        //                    this.cbAntiAliasSide.SelectedIndex = 1;
        //                    break;
        //                case PrintJob.AntiAliasType.Outside:
        //                    this.cbAntiAliasSide.SelectedIndex = 2;
        //                    break;
        //            };

        //    }
        //    }
        //    get
        //    {
        //        PrintJob.AntiAliasType result = PrintJob.AntiAliasType.Inside;
        //        if (this.cbAntiAliasSide.InvokeRequired)
        //        {
        //            this.cbAntiAliasSide.Invoke(new MethodInvoker(() =>
        //            {
        //                switch (this.cbAntiAliasSide.SelectedIndex)
        //                {
        //                    case 0:
        //                        result = PrintJob.AntiAliasType.None;
        //                        break;
        //                    case 1:
        //                        result = PrintJob.AntiAliasType.Inside;
        //                        break;
        //                    case 2:
        //                        result = PrintJob.AntiAliasType.Outside;
        //                        break;
        //                    default:
        //                        result = PrintJob.AntiAliasType.None;
        //                        break;
        //                }
        //            }));
        //            return result;
        //        }
        //        else
        //        {
        //            switch (this.cbAntiAliasSide.SelectedIndex)
        //            {
        //                case 0:
        //                    result = PrintJob.AntiAliasType.None;
        //                    break;
        //                case 1:
        //                    result = PrintJob.AntiAliasType.Inside;
        //                    break;
        //                case 2:
        //                    result = PrintJob.AntiAliasType.Outside;
        //                    break;
        //            }
        //            return result;
        //        }
        //    }
        //}

        public PrintJobPropertiesPanel()
        {
            InitializeComponent();

            this.ToolstripIconMouseOver = BrandingManager.DockPanelPrintJobPropertiesMouseOver;

            foreach (ToolStripItem button in this.toolStrip1.Items)
            {
                button.MouseMove += button_MouseMove;
            }

            this.tbLayers.TabPages.Remove(this.tbFirstLayers);

            this.RefreshMaterials();
        }

        void button_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        void groupBox1_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        internal MaterialsCatalog DataSource { get; set; }

        internal Material SelectedMaterial
        {
            get
            {
                var selectedMaterial = new Material();
                if (this.cbMaterialProduct.InvokeRequired)
                {
                    this.cbMaterialProduct.Invoke(new MethodInvoker(delegate
                    {
                        if (this.cbMaterialProduct.SelectedItem != null)
                        {
                            selectedMaterial = this.cbMaterialProduct.SelectedItem as Material;
                        }

                    }));
                }
                else
                {
                    if (this.cbMaterialProduct.SelectedItem != null)
                    {
                        selectedMaterial = this.cbMaterialProduct.SelectedItem as Material;
                    }
                    else
                    {
                        selectedMaterial = new Material();
                    }
                }

#if LOCTITE
                if (selectedMaterial.SupportProfiles== null || selectedMaterial.SupportProfiles.Count == 0)
                {
                    selectedMaterial.SupportProfiles = new System.Collections.Generic.List<SupportProfile>() { new SupportProfile() };
                }
                selectedMaterial.SupportProfiles[0].SupportTopHeight = SupportManager.DefaultSupportSettings.TopHeight;
                selectedMaterial.SupportProfiles[0].SupportTopRadius = SupportManager.DefaultSupportSettings.TopRadius;
                selectedMaterial.SupportProfiles[0].SupportMiddleRadius = SupportManager.DefaultSupportSettings.MiddleRadius;
                selectedMaterial.SupportProfiles[0].SupportBottomRadius = SupportManager.DefaultSupportSettings.BottomRadius;
                selectedMaterial.SupportProfiles[0].SupportBottomHeight = SupportManager.DefaultSupportSettings.BottomHeight;
#endif

                if (!FeatureManager.EnableMAGSAI)
                {
                    if (selectedMaterial.SupportProfiles == null || selectedMaterial.SupportProfiles.Count == 0)
                    {
                        selectedMaterial.SupportProfiles = new System.Collections.Generic.List<SupportProfile>() { new SupportProfile() };
                    }
                    selectedMaterial.SupportProfiles[0].SupportTopHeight = SupportManager.DefaultSupportSettings.TopHeight;
                    selectedMaterial.SupportProfiles[0].SupportTopRadius = SupportManager.DefaultSupportSettings.TopRadius;
                    selectedMaterial.SupportProfiles[0].SupportMiddleRadius = SupportManager.DefaultSupportSettings.MiddleRadius;
                    selectedMaterial.SupportProfiles[0].SupportBottomRadius = SupportManager.DefaultSupportSettings.BottomRadius;
                    selectedMaterial.SupportProfiles[0].SupportBottomHeight = SupportManager.DefaultSupportSettings.BottomHeight;
                }

                return selectedMaterial;
            }
        }

        internal MaterialsCatalog SelectedMaterialCatalog
        {
            get
            {
                var result = new MaterialsCatalog();
                MaterialsBySupplier selectedManufacturer = null;
                if (this.cbMaterialManufacturer.InvokeRequired)
                {
                    this.cbMaterialManufacturer.Invoke(new MethodInvoker(() => selectedManufacturer = (MaterialsBySupplier)this.cbMaterialManufacturer.SelectedItem));
                }
                else
                {
                    selectedManufacturer = (MaterialsBySupplier)this.cbMaterialManufacturer.SelectedItem;
                }
                if (selectedManufacturer != null)
                {
                    result.Add(new MaterialsBySupplier() { Supplier = selectedManufacturer.Supplier });
                    Material selectedMaterial = null;
                    if (this.cbMaterialProduct.InvokeRequired)
                    {
                        this.cbMaterialProduct.Invoke(new MethodInvoker(() => selectedMaterial = (Material)this.cbMaterialProduct.SelectedItem));
                    }
                    else
                    {
                        selectedMaterial = (Material)this.cbMaterialProduct.SelectedItem;
                    }
                    if (selectedMaterial != null)
                    {
                        result[0].Materials.Add(selectedMaterial);
                    }
                }
                return result;
            }
            set
            {
                if (value != null && value.Count == 1)
                {
                    var supplierFound = false;
                    for (var supplierIndex = 0; supplierIndex < this.cbMaterialManufacturer.Items.Count; supplierIndex++)
                    {
                        var supplier = ((MaterialsBySupplier)this.cbMaterialManufacturer.Items[supplierIndex]);
                        if (supplier.Supplier.ToLower() == value[0].Supplier.ToLower())
                        {
                            supplierFound = true;
                            if (this.cbMaterialManufacturer.InvokeRequired)
                            {
                                this.cbMaterialManufacturer.Invoke(new MethodInvoker(delegate
                                {
                                    this.cbMaterialManufacturer.SelectedIndex = supplierIndex;
                                }));
                            }
                            else
                            {
                                this.cbMaterialManufacturer.SelectedIndex = supplierIndex;
                            }


                            var materialFound = false;
                            var valueMaterial = value[0].Materials[0];
                            for (var materialIndex = 0; materialIndex < this.cbMaterialProduct.Items.Count; materialIndex++)
                            {
                                var material = ((Material)this.cbMaterialProduct.Items[materialIndex]);
                                if (!string.IsNullOrEmpty(material.DisplayName))
                                {
                                    if (material.DisplayName.ToLower() == valueMaterial.DisplayName.ToLower())
                                    {
                                        this.cbMaterialProduct.SelectedIndexChanged -= cbMaterialProduct_SelectedIndexChanged;

                                        if (this.cbMaterialProduct.InvokeRequired)
                                        {
                                            this.cbMaterialProduct.Invoke(new MethodInvoker(delegate
                                            {
                                                this.cbMaterialProduct.SelectedIndex = materialIndex;
                                            }));
                                        }
                                        else
                                        {
                                            this.cbMaterialProduct.SelectedIndex = materialIndex;
                                        }

                                        materialFound = true;
                                        this.materialLayers.DataSource = valueMaterial;
                                        this.cbMaterialProduct.SelectedIndexChanged += cbMaterialProduct_SelectedIndexChanged;
                                        break;
                                    }
                                }
                            }

                            if (!materialFound)
                            {
                                this.cbMaterialProduct.SelectedIndexChanged -= cbMaterialProduct_SelectedIndexChanged;
                                supplier.Materials.Add(valueMaterial);

                                if (this.cbMaterialProduct.InvokeRequired)
                                {
                                    this.cbMaterialProduct.Invoke(new MethodInvoker(delegate
                                    {
                                        this.cbMaterialProduct.SelectedIndex = this.cbMaterialProduct.Items.Count - 1;
                                    }));
                                }
                                else
                                {
                                    this.cbMaterialProduct.SelectedIndex = this.cbMaterialProduct.Items.Count - 1;
                                }

                                this.materialLayers.DataSource = valueMaterial;
                                this.cbMaterialProduct.SelectedIndexChanged += cbMaterialProduct_SelectedIndexChanged;
                            }

                            break;
                        }
                    }

                    if (!supplierFound)
                    {
                        MaterialsBySupplier supplier = null;
                        var valueMaterial = value[0].Materials[0];
                        if (this.cbMaterialManufacturer.InvokeRequired)
                        {
                            this.cbMaterialManufacturer.Invoke(new MethodInvoker(delegate
                            {
                                this.cbMaterialManufacturer.Items.Add(value[0]);
                                this.cbMaterialManufacturer.SelectedIndex = this.cbMaterialManufacturer.Items.Count - 1;
                                supplier = (MaterialsBySupplier)this.cbMaterialManufacturer.Items[this.cbMaterialManufacturer.SelectedIndex];
                            }));
                        }
                        else
                        {
                            this.cbMaterialManufacturer.Items.Add(value[0]);
                            this.cbMaterialManufacturer.SelectedIndex = this.cbMaterialManufacturer.Items.Count - 1;
                            supplier = (MaterialsBySupplier)this.cbMaterialManufacturer.Items[this.cbMaterialManufacturer.SelectedIndex];
                        }


                        if (this.cbMaterialProduct.InvokeRequired)
                        {
                            this.cbMaterialProduct.Invoke(new MethodInvoker(delegate
                            {
                                this.cbMaterialProduct.SelectedIndexChanged -= cbMaterialProduct_SelectedIndexChanged;
                                this.cbMaterialProduct.SelectedIndex = this.cbMaterialProduct.Items.Count - 1;
                                this.materialLayers.DataSource = valueMaterial;
                                this.cbMaterialProduct.SelectedIndexChanged += cbMaterialProduct_SelectedIndexChanged;
                                //MaterialManager.CurrentMaterial = valueMaterial;
                            }));
                        }
                        else
                        {
                            this.cbMaterialProduct.SelectedIndexChanged -= cbMaterialProduct_SelectedIndexChanged;
                            this.cbMaterialProduct.SelectedIndex = this.cbMaterialProduct.Items.Count - 1;
                            this.materialLayers.DataSource = valueMaterial;
                            this.cbMaterialProduct.SelectedIndexChanged += cbMaterialProduct_SelectedIndexChanged;
                        }

                    }
                }
                //update current material
                cbMaterialProduct_SelectedIndexChanged(null, null);
            }
        }


        private void cbMaterialProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //initial layers
                Material selectedMaterial = null;
                if (this.cbMaterialProduct.InvokeRequired)
                {
                    this.cbMaterialProduct.Invoke(new MethodInvoker(delegate
                    {
                        selectedMaterial = (Material)this.cbMaterialProduct.SelectedItem;
                    }));
                }
                else
                {
                    selectedMaterial = (Material)this.cbMaterialProduct.SelectedItem;
                }


                if (selectedMaterial != null)
                {
                    if (this.txtInitialLayers.InvokeRequired)
                    {
                        this.txtInitialLayers.Invoke(new MethodInvoker(delegate
                        {
                            this.txtInitialLayers.Enabled = false;
                        }));
                    }
                    else
                    {
                        this.txtInitialLayers.Enabled = false;
                    }



                    if (selectedMaterial.LT1 == 0) { selectedMaterial.LT1 = 0.05; }
                    if (selectedMaterial.LT2 == 0) { selectedMaterial.LT2 = 0.05; }
                    this.materialLayers.DataSource = selectedMaterial;

                    //update material manager
                    //MaterialManager.CurrentMaterial = selectedMaterial;
                    // MaterialManager.SetAsDefault(selectedMaterial);

                    //update models with colors
                    foreach (var object3d in ObjectView.Objects3D)
                    {
                        if (object3d is Core.Models.STLModel3D && !(object3d is Core.Shapes.GroundPane))
                        {
                            var stlModel = (Core.Models.STLModel3D)object3d;
                            var currentModelIndex = stlModel.Index;
                            stlModel._color = Color.FromArgb(currentModelIndex, selectedMaterial.ModelColor);
                            stlModel.Triangles.UpdateFaceColors(stlModel.ColorAsByte4);
                            stlModel.UpdateBinding();
                        }
                    }

                    this.MaterialIndexChanged?.Invoke(null, null);

                    if (this.txtInitialLayers.InvokeRequired)
                    {
                        this.txtInitialLayers.Invoke(new MethodInvoker(delegate
                        {
                            this.txtInitialLayers.Enabled = true;
                        }));
                    }
                    else
                    {
                        this.txtInitialLayers.Enabled = true;
                    }
                }
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("Exception Manager", "cbMaterialProduct_SelectedIndexChanged", exc.ToString());
            }
        }

        private void cbMaterialManufacturer_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (this.cbMaterialManufacturer.SelectedItem != null)
                {
                    var selectedSupplier = this.cbMaterialManufacturer.SelectedItem as MaterialsBySupplier;
                    if (selectedSupplier == null && this.cbMaterialManufacturer.Items.Count > 0)
                    {
                        this.cbMaterialManufacturer.SelectedIndex = 0;
                        selectedSupplier = this.cbMaterialManufacturer.SelectedItem as MaterialsBySupplier;
                    }


                    this.cbMaterialProduct.DataSource = selectedSupplier.Materials;

                    var defaultMaterial = MaterialManager.DefaultMaterial;
                    if (cbMaterialProduct != null)
                    {
                        this.cbMaterialProduct.SelectedItem = defaultMaterial;

                        if (this.cbMaterialProduct.SelectedItem == null && this.cbMaterialProduct.Items.Count > 0)
                        {
                            this.cbMaterialProduct.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        this.cbMaterialProduct_SelectedIndexChanged(null, null);
                    }
                }

            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("Exception Manager", "cbMaterialManufacturer_SelectedIndexChanged", exc);
            }

        }


        private void txtInitialLayers_ValueChanged(object sender, EventArgs e)
        {
            if (this.txtInitialLayers.Value >= 1)
            {
                if (!this.tbLayers.TabPages.Contains(this.tbFirstLayers))
                {
                    this.tbLayers.TabPages.Add(this.tbFirstLayers);
                }
            }
            else if (this.txtInitialLayers.Value == 0)
            {
                if (this.tbLayers.TabPages.Contains(this.tbFirstLayers))
                {
                    this.tbLayers.TabPages.Remove(this.tbFirstLayers);
                }
            }

            //	if (MaterialManager.CurrentMaterial != null){MaterialManager.CurrentMaterial.InitialLayers = (int)this.txtInitialLayers.Value;}
        }

        internal void RefreshPrinters()
        {
            this.cbDefaultPrinter.Items.Clear();

            var selectedIndex = 0;
            for (var printerIndex = 0; printerIndex < PrinterManager.AvailablePrinters.Count; printerIndex++)
            {
                if (PrinterManager.SelectedPrinters[printerIndex].Default)
                {
                    selectedIndex = printerIndex;
                }

                this.cbDefaultPrinter.Items.Add(PrinterManager.SelectedPrinters[printerIndex]);
            }

            if (this.cbDefaultPrinter.Items.Count > 0) this.cbDefaultPrinter.SelectedIndex = selectedIndex;
        }
        internal void RefreshMaterials()
        {
            this.cbMaterialManufacturer.Items.Clear();

            var selectedPrinter = this.cbDefaultPrinter.SelectedItem as AtumPrinter;
            if (MaterialManager.Catalog != null)
            {
                if (!(selectedPrinter is AtumDLPStation5) && !(selectedPrinter is LoctiteV10))
                {
                    //filter on hardwaretype and resolution
                    foreach (var supplier in MaterialManager.Catalog)
                    {
                        var filteredSupplier = new MaterialsBySupplier();
                        filteredSupplier.Supplier = supplier.Supplier;
                        var filteredMaterials = supplier.Materials.Where(m => string.IsNullOrEmpty(m.PrinterHardwareType));
                        foreach (var filteredMaterial in filteredMaterials)
                        {
                            filteredSupplier.Materials.Add(filteredMaterial);
                            if (filteredMaterial.IsDefault)
                            {
                                if (!filteredMaterial.DisplayName.Contains("(*)"))
                                {
                                    filteredMaterial.DisplayName += " (*)";
                                }
                            }
                        }

                        if (filteredSupplier.Materials.Count > 0)
                        {
                            this.cbMaterialManufacturer.Items.Add(filteredSupplier);
                        }
                    }
                }
                else
                {
                    //filter on hardwaretype and resolution
                    foreach (var supplier in MaterialManager.Catalog)
                    {
                        var filteredSupplier = new MaterialsBySupplier();
                        filteredSupplier.Supplier = supplier.Supplier;
                        var filteredMaterials = supplier.Materials.Where(m => m.XYResolution == selectedPrinter.PrinterXYResolutionAsInt && m.PrinterHardwareType == selectedPrinter.PrinterHardwareType);
                        foreach (var filteredMaterial in filteredMaterials)
                        {
                            filteredSupplier.Materials.Add(filteredMaterial);
                            if (filteredMaterial.IsDefault)
                            {
                                if (!filteredMaterial.DisplayName.Contains("(*)"))
                                {
                                    filteredMaterial.DisplayName += " (*)";
                                }
                            }
                        }

                        if (filteredSupplier.Materials.Count > 0)
                        {
                            this.cbMaterialManufacturer.Items.Add(filteredSupplier);
                        }
                    }
                }

                var supplierIndex = 0;
                foreach (var supplierItem in this.cbMaterialManufacturer.Items)
                {
                    var supplier = supplierItem as MaterialsBySupplier;
                    if (supplier.Materials.Any(m => m.IsDefault))
                    {
                        break;
                    }

                    supplierIndex++;
                }

                if (supplierIndex >= this.cbMaterialManufacturer.Items.Count)
                {
                    supplierIndex = 0;
                }

                if (this.cbMaterialManufacturer.Items.Count > 0)
                {
                    this.cbMaterialManufacturer.SelectedIndex = supplierIndex;
                }

                this.cbMaterialManufacturer_SelectedIndexChanged(null, null);
            }
        }

        private void btnAddPrinter_Click(object sender, EventArgs e)
        {
            using (var printerManager = new PrinterConnectionManagerPopup())
            {
                printerManager.StartPosition = FormStartPosition.Manual;
                printerManager.Location =
                    new Point(
                        UserProfileManager.UserProfile.MainWindowLocation.X + (UserProfileManager.UserProfile.MainWindowSize.Width / 2) - (printerManager.Size.Width / 2),
                        UserProfileManager.UserProfile.MainWindowLocation.Y + (UserProfileManager.UserProfile.MainWindowSize.Height / 2) - (printerManager.Size.Height / 2)
                        );
                if (printerManager.ShowDialog() == DialogResult.OK)
                {
                    PrinterManager.Save();

                    this.RefreshPrinters();
                }
            }
        }

        private void btnMaterialEditor_Click(object sender, EventArgs e)
        {
            using (var popup = new Atum.Studio.Controls.MaterialEditor.frmMaterialEditorPopup())
            {
                popup.SelectedMaterial = this.cbMaterialProduct.SelectedItem as Material;
                popup.StartPosition = FormStartPosition.Manual;
                popup.Location =
                    new Point(
                        UserProfileManager.UserProfile.MainWindowLocation.X + (UserProfileManager.UserProfile.MainWindowSize.Width / 2) - (popup.Size.Width / 2),
                        UserProfileManager.UserProfile.MainWindowLocation.Y + (UserProfileManager.UserProfile.MainWindowSize.Height / 2) - (popup.Size.Height / 2)
                        );
                if (popup.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    this.RefreshMaterials();

                    ////update models with colors
                    //foreach (var object3d in ObjectView.Objects3D)
                    //{
                    //    if (object3d is Core.Models.STLModel3D && !(object3d is Core.Shapes.GroundPane))
                    //    {
                    //        var stlModel = (Core.Models.STLModel3D)object3d;
                    //        var currentModelIndex = stlModel.Index;
                    //        stlModel._color = Color.FromArgb(currentModelIndex, selectedMaterial.ModelColor);
                    //        stlModel.Triangles.UpdateFaceColors(stlModel.ColorAsByte4);
                    //        stlModel.UpdateBinding();
                    //    }
                    //}
                }
            }
        }

        private void toolStrip1_MouseMove(object sender, MouseEventArgs e)
        {
            base.SetFocusToDockPanel();
        }

        private void trAntiAliasFactor_Scroll(object sender, EventArgs e)
        {
            //this.lblAntiAliasFactor.Text = this.trAntiAliasFactor.Value + "%";
            UserProfileManager.UserProfiles[0].Settings_PrintJob_AntiAlias_Factor = this.trAntiAliasFactor.Value;
        }

        private void cbAntiAliasSide_SelectedIndexChanged(object sender, EventArgs e)
        {
            //UserProfileManager.UserProfiles[0].Settings_PrintJob_AntiAlias_Side = this.cbAntiAliasSide.SelectedIndex;
        }

        private void cbDefaultPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            PrinterManager.SetDefaultPrinter((AtumPrinter)this.cbDefaultPrinter.SelectedItem);
            RefreshMaterials();
        }

        private void PrintJobPropertiesPanel_Shown(object sender, EventArgs e)
        {
            this.tbLayers.Height = (int)(this.tableLayoutPanel3.Height * DisplayManager.GetResolutionScaleFactor()) + this.tbLayers.GetTabRect(0).Height;
        }
    }
}
