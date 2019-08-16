using Atum.DAL.Materials;
using Atum.Studio.Core.Managers;
using System;
using System.Diagnostics;
using System.Linq;
using Atum.DAL.Hardware;
using System.Windows.Forms;

namespace Atum.Studio.Controls.MagsAI
{
    public partial class MagsAIMaterialTabPanel : UserControl
    {
        internal AtumPrinter SelectedPrinter
        {
            get
            {
                AtumPrinter result = null;
                if (!this.cbDefaultPrinter.InvokeRequired)
                {
                    if (this.cbDefaultPrinter.SelectedItem != null)
                    {
                        result = (AtumPrinter)this.cbDefaultPrinter.SelectedItem;
                    }
                }
                else
                {
                    this.cbDefaultPrinter.Invoke(new MethodInvoker(delegate
                   {
                       if (this.cbDefaultPrinter.SelectedItem != null)
                       {
                           result = (AtumPrinter)this.cbDefaultPrinter.SelectedItem;
                       }
                   }));
                 };
                return result;
            }
        }

        internal bool LiftModelOnSupport
        {
            get
            {
                return this.chkLiftModelOnSupport.Checked;
            }
        }

        internal bool UseSupportBasement
        {
            get
            {
                return this.chkSupportBasement.Checked;
            }
        }

        internal Material SelectedMaterial
        {
            get
            {
                Material selectedMaterial = null;
                try
                {
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
                    else if (this.cbMaterialProduct.SelectedItem != null)
                    {
                        selectedMaterial = this.cbMaterialProduct.SelectedItem as Material;
                    }
                }
                catch
                {

                }
                return selectedMaterial;
            }
        }

        public MagsAIMaterialTabPanel()
        {
            InitializeComponent();
        }
        
        //private void cbMaterialManufacturer_SelectedIndexChanged(object sender, System.EventArgs e)
        //{
        //    try
        //    {
        //        var selectedSupplier = this.cbMaterialManufacturer.SelectedItem as MaterialsBySupplier;

        //        if (selectedSupplier != null && selectedSupplier.Materials != null)
        //        {
        //            this.cbMaterialProduct.DataSource = selectedSupplier.Materials;

        //            var defaultMaterial = MaterialManager.DefaultMaterial;
        //            if (cbMaterialProduct != null)
        //            {
        //                this.cbMaterialProduct.SelectedItem = defaultMaterial;
        //            }
        //        }
        //        else {
        //            this.cbMaterialProduct.DataSource = new object[0];
        //        }
        //    }
                
        //        catch (Exception exc)
        //        {
        //            Debug.WriteLine(exc);
        //        }
        //    }

        private void cbDefaultPrinter_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.RefreshMaterials();
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
                                if (filteredMaterial.DisplayName.Contains("(*)"))
                                {
                                    filteredMaterial.DisplayName = filteredMaterial.DisplayName.Replace("(*)", string.Empty).Trim();
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

                //this.cbMaterialManufacturer_SelectedIndexChanged(null, null);
            }
        }

        private void MagsAIMaterialTabPanel_Load(object sender, EventArgs e)
        {
            if (!DesignMode)
            {
                foreach (var availablePrinter in PrinterManager.AvailablePrinters)
                {
                    this.cbDefaultPrinter.Items.Add(availablePrinter);
                }

                var selectedIndex = 0;
                foreach (var availablePrinter in PrinterManager.AvailablePrinters)
                {
                    if (availablePrinter == PrintJobManager.SelectedPrinter)
                    {
                        break;
                    }
                    selectedIndex++;
                }

                if (selectedIndex > 0 && selectedIndex == this.cbDefaultPrinter.Items.Count)
                {
                    selectedIndex = 0;
                }
                if (this.cbDefaultPrinter.Items.Count > 0)
                {
                    this.cbDefaultPrinter.SelectedIndex = selectedIndex;
                }
            }
        }
    }
}
