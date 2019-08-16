using Atum.DAL.Materials;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atum.Studio.Controls.NewGui.MaterialDisplay
{
    public partial class frmMaterialOnlineCatalog : NewGUIFormBase
    {
        public event EventHandler onMaterialAdded;
        internal event Action<bool> DownloadMaterialsCompleted;
        private MaterialCatalogOnline _allOnlineMaterials;
        private List<MaterialsBySupplier> SelectedMaterials { get; set; }
        private DAL.Hardware.AtumPrinter _selectedPrinter;

        public DAL.Hardware.AtumPrinter SelectedPrinter
        {
            get
            {
                return this._selectedPrinter;
            }
            set
            {
                this._selectedPrinter = value;

            }

        }
        public frmMaterialOnlineCatalog()
        {
            InitializeComponent();

            this.btnCancel.BorderColor = this.btnCancel.ForeColor = this.btnSave.BackColor = BrandingManager.Button_BackgroundColor_Dark;
            ProgressBarManager.CreateOnlineMaterialProgressbar(this.pnlMaterialDisplay);
        }
        internal void DownloadMaterialsAsync(DAL.Hardware.AtumPrinter selectedPrinter)
        {
            try
            {
                // MessageBox.Show("1");
                this._allOnlineMaterials = new MaterialCatalogOnline();
                Task.Factory.StartNew(() =>
                {
                    //   MessageBox.Show("2");
                    DownloadingMaterialsAsync(selectedPrinter);
                }).ContinueWith(s =>
                DownloadingMaterialsAsync_Completed(null, null));
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }

        private void DownloadingMaterialsAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            FillAvailableMaterials();
            DownloadMaterialsCompleted?.Invoke(true);
        }

        internal void DownloadingMaterialsAsync(DAL.Hardware.AtumPrinter selectedPrinter)
        {
            //   MessageBox.Show("3");
            try
            {
                //     MessageBox.Show("4");
                var currentProgress = 5f;
                ProgressBarManager.UpdateOnlineMaterialPercentage(currentProgress);
                //   MessageBox.Show("5");

                var materialRootFileStream = string.Empty;
                if (selectedPrinter is DAL.Hardware.AtumDLPStation5 || selectedPrinter is DAL.Hardware.LoctiteV10)
                {
                    if (!DAL.ApplicationSettings.Settings.UseOfflineMaterialCatalog)
                    {
                        //use online catalog
                        switch (selectedPrinter.PrinterXYResolution)
                        {
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                                materialRootFileStream = DownloadManager.DownloadString(BrandingManager.AdditiveManufacturingDeviceDLPStation5_XY50);
                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                                materialRootFileStream = DownloadManager.DownloadString(BrandingManager.AdditiveManufacturingDeviceDLPStation5_XY75);
                                break;
                            case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                                materialRootFileStream = DownloadManager.DownloadString(BrandingManager.AdditiveManufacturingDeviceDLPStation5_XY100);
                                break;
                        }
                    }
                }
                else
                {
                    materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.atum3D_MaterialCatalog_AtumVxx);
                }


                var materialRootFile = new System.Xml.Serialization.XmlSerializer(typeof(MaterialCatalogDownloadAllFiles));
                materialRootFileStream = materialRootFileStream.Replace("&", "&amp;");
                var materialRootDownloadFile = (MaterialCatalogDownloadAllFiles)materialRootFile.Deserialize(new StringReader(materialRootFileStream));

                if (materialRootDownloadFile != null)
                {
                    var materialDownloadPercentage = (float)(80f / materialRootDownloadFile.AvailableMaterialURLs.Count);

                    foreach (var materialDownloadFile in materialRootDownloadFile.AvailableMaterialURLs)
                    {
                        currentProgress = (currentProgress + materialDownloadPercentage);
                        if (currentProgress > 100)
                        {
                            currentProgress = 100;
                        }

                        ProgressBarManager.UpdateOnlineMaterialPercentage(currentProgress);

                        try
                        {
                            //download material
                            if ((int)materialDownloadFile.XYMicron == selectedPrinter.PrinterXYResolutionAsInt)
                            {
                                materialRootFileStream = DownloadManager.DownloadString(materialDownloadFile.URL);
                                materialRootFileStream = materialRootFileStream.Substring(materialRootFileStream.IndexOf('<'));
                                materialRootFileStream = materialRootFileStream.Replace("&", "&amp;");

                                var materialsBySupplierSerializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
                                var materialBySupplier = (MaterialsBySupplier)materialsBySupplierSerializer.Deserialize(new StringReader(materialRootFileStream));
                                if (materialBySupplier != null && materialBySupplier.Materials.Count > 0)
                                {
                                    if (!this._allOnlineMaterials.ContainsKey((int)materialDownloadFile.XYMicron))
                                    {
                                        this._allOnlineMaterials.Add((int)materialDownloadFile.XYMicron, new MaterialsCatalog());
                                    }
                                    this._allOnlineMaterials[(int)materialDownloadFile.XYMicron].Add(materialBySupplier);
                                }
                            }
                        }
                        catch (Exception exc)
                        {

                        }
                    }
                }

                currentProgress = 100;
                ProgressBarManager.UpdateOnlineMaterialPercentage(currentProgress);
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }

        }

        private void FrmMaterialDisplay_Load(object sender, EventArgs e)
        {
            this.SelectedPrinter = PrintJobManager.SelectedPrinter;
            if (this.SelectedPrinter != null)
            {
                DownloadMaterialsAsync(this.SelectedPrinter);
            }
        }

        private void FillAvailableMaterials()
        {
            SelectedMaterials = new List<MaterialsBySupplier>();
            UpdateControls();

            var summaryTop = 24;
            var summaryleft = 24;

            var i = 0;
            foreach (var catalog in this._allOnlineMaterials)
            {
                List<MaterialsBySupplier> materialListBySupplier = new List<MaterialsBySupplier>();
                materialListBySupplier = catalog.Value.OrderBy(x => x.Supplier).ToList();

                foreach (var materialBySupplier in materialListBySupplier)
                {
                    foreach (var material in materialBySupplier.Materials)
                    {
                        i += 1;
                        var materialWithSupplier = new MaterialsBySupplier();
                        materialWithSupplier.Supplier = materialBySupplier.Supplier;
                        materialWithSupplier.Materials.Add(material);

                        displayMaterial displayMaterialControl = PushMaterialControl(summaryTop, summaryleft, materialWithSupplier);
                        if (i % 3 == 0)
                        {
                            summaryTop += (displayMaterialControl.Height + 24);
                            summaryleft = 24;
                        }
                        else
                        {
                            summaryleft += (displayMaterialControl.Width + 24);
                        }
                    }
                }
                if (this.pnlMaterialDisplay.InvokeRequired)
                {
                    this.pnlMaterialDisplay.Invoke(new MethodInvoker(delegate
                    {
                        this.pnlMaterialDisplay.AutoScroll = true;
                    }));
                }
                else
                {
                    this.pnlMaterialDisplay.AutoScroll = true;
                }

            }

            #region Testing Data
            //foreach (var materialBySupplier in MaterialManager.Catalog)
            //{
            //    foreach (var material in materialBySupplier.Materials)
            //    {
            //        i += 1;
            //        if (i % 3 == 0)
            //        {
            //            var displayMaterialControl = new displayMaterial(material);
            //            displayMaterialControl.SelectionChanged += DisplayMaterialControl_SelectionChanged;
            //            displayMaterialControl.Top = summaryTop;
            //            displayMaterialControl.Width = 208;
            //            displayMaterialControl.Height = 126;
            //            displayMaterialControl.Left = summaryleft;
            //            this.pnlMaterialDisplay.Controls.Add(displayMaterialControl);
            //            summaryTop += (displayMaterialControl.Height + 24);
            //            summaryleft = 24;
            //        }
            //        else
            //        {
            //            var displayMaterialControl = new displayMaterial(material);
            //            displayMaterialControl.SelectionChanged += DisplayMaterialControl_SelectionChanged;
            //            displayMaterialControl.Top = summaryTop;
            //            displayMaterialControl.Width = 208;
            //            displayMaterialControl.Height = 126;
            //            displayMaterialControl.Left = summaryleft;
            //            this.pnlMaterialDisplay.Controls.Add(displayMaterialControl);
            //            summaryleft += (displayMaterialControl.Width + 24);
            //        }
            //    }
            //}
            //this.pnlMaterialDisplay.AutoScroll = true; 
            #endregion
        }

        private displayMaterial PushMaterialControl(int summaryTop, int summaryleft, MaterialsBySupplier material)
        {
            var displayMaterialControl = new displayMaterial(material);
            displayMaterialControl.SelectionChanged += DisplayMaterialControl_SelectionChanged;
            displayMaterialControl.Top = summaryTop;
            displayMaterialControl.Width = 208;
            //    displayMaterialControl.Height = 126;
            displayMaterialControl.Left = summaryleft;

            if (this.pnlMaterialDisplay.InvokeRequired)
            {
                this.pnlMaterialDisplay.Invoke(new MethodInvoker(delegate
                {
                    this.pnlMaterialDisplay.Controls.Add(displayMaterialControl);
                }));
            }
            else
            {
                this.pnlMaterialDisplay.Controls.Add(displayMaterialControl);
            }


            return displayMaterialControl;
        }

        private void DisplayMaterialControl_SelectionChanged(object sender, EventArgs e)
        {
            displayMaterial displayMaterial = sender as displayMaterial;
            if (displayMaterial != null)
            {
                if (displayMaterial.IsSelected)
                {
                    if (!SelectedMaterials.Contains(displayMaterial.Material))
                    {
                        SelectedMaterials.Add(displayMaterial.Material);
                    }
                }
                else
                {
                    if (SelectedMaterials.Contains(displayMaterial.Material))
                    {
                        SelectedMaterials.Remove(displayMaterial.Material);
                    }
                }
                UpdateControls();
            }
        }

        private void UpdateControls()
        {
            lblSelected.Text = SelectedMaterials.Count + " selected";

            if (btnSave.InvokeRequired)
            {
                btnSave.Invoke(new MethodInvoker(delegate
                {
                    btnSave.Enabled = SelectedMaterials.Count > 0;
                }));
            }
            else
            {
                btnSave.Enabled = SelectedMaterials.Count > 0;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAdd_EnabledChanged(object sender, EventArgs e)
        {
            btnSave.ForeColor = BrandingManager.Button_ForeColor;
            btnSave.BackColor = btnSave.Enabled == false ? BrandingManager.Button_BackgroundColor_Disabled : BrandingManager.Button_BackgroundColor_Dark;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (SelectedMaterials != null)
            {
                foreach (var material in SelectedMaterials)
                {
                    //find supplier catalog
                    var catalog = MaterialManager.Catalog.FirstOrDefault(x => x.Supplier.ToLower() == material.Supplier.ToLower());
                    if (catalog != null)
                    {
                        //check if material exists
                        if (material.Materials[0].Id != Guid.Empty)
                        {
                            var currentMaterial = catalog.FindMaterialById(material.Materials[0].Id);
                            if (currentMaterial == null)
                            {
                                catalog.Materials.Add(material.Materials[0]);
                            }
                            else
                            {
                                catalog.SetMaterialById(material.Materials[0].Id, material.Materials[0]);
                            }
                        }
                        else
                        {
                            //material id == empty
                            material.Materials[0].Id = new Guid();
                            catalog.Materials.Add(material.Materials[0]);
                        }
                    }
                    else
                    {
                        MaterialManager.Catalog.Add(material);
                    }
                }


                this.onMaterialAdded?.Invoke(this, null);

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
        }
    }
}
