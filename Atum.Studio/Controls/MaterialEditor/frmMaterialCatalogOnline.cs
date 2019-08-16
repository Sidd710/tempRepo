using Atum.DAL.Compression.Zip;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Materials;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Atum.Studio.Controls.MaterialEditor
{
    public partial class frmMaterialCatalogOnline : BasePopup
    {
        internal event Action<bool> DownloadMaterialsCompleted;
        private MaterialCatalogOnline _allOnlineMaterials;
        private WaitWindowManager _waitWindowManager;

        public DAL.Materials.MaterialsCatalog SelectedMaterials
        {
            get
            {
                var results = new DAL.Materials.MaterialsCatalog();
                foreach (TreeNode resolutionTreeNode in this.treeView1.Nodes)
                {
                    foreach (TreeNode supplierTreeNode in resolutionTreeNode.Nodes)
                    {
                        foreach (TreeNode materialNode in supplierTreeNode.Nodes)
                        {
                            if (materialNode.Checked)
                            {
                                var supplierFound = false;
                                foreach (var supplier in results)
                                {
                                    if (supplier.Supplier == supplierTreeNode.Text)
                                    {
                                        supplier.Materials.Add(materialNode.Tag as DAL.Materials.Material);
                                        supplierFound = true;
                                    }
                                }

                                if (!supplierFound)
                                {
                                    var supplierMaterial = new DAL.Materials.MaterialsBySupplier();
                                    supplierMaterial.Supplier = supplierTreeNode.Text;
                                    supplierMaterial.Materials.Add(materialNode.Tag as DAL.Materials.Material);
                                    results.Add(supplierMaterial);
                                }
                            }
                        }
                    }
                }

                return results;
            }
        }

        public frmMaterialCatalogOnline()
        {
            InitializeComponent();

            this.Icon = BrandingManager.MainForm_Icon;

            this.btnOK.Text = "Import";
        }

        internal void DownloadMaterialsAsycn(Form parentForm, DAL.Hardware.AtumPrinter selectedPrinter)
        {
            this._allOnlineMaterials = new MaterialCatalogOnline();
            this.treeView1.Nodes.Clear();

            var t = new BackgroundWorker();
            this._waitWindowManager = new WaitWindowManager();
            this._waitWindowManager.Start(parentForm, t);

            t.DoWork += new DoWorkEventHandler(DownloadingMaterialsAsync);
            t.RunWorkerCompleted += new RunWorkerCompletedEventHandler(DownloadingMaterialsAsync_Completed);
            t.RunWorkerAsync(selectedPrinter);

        }

        private void frmMaterialCatalogOnline_Load(object sender, EventArgs e)
        {
            if (DAL.OS.OSProvider.IsOSX)
            {
                this.Size = new Size(this.Width, this.Height + 100);
            }

        }

        private void DownloadingMaterialsAsync_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            materials_downloaded();
            DownloadMaterialsCompleted?.Invoke(true);
        }

        void DownloadingMaterialsAsync(object sender, DoWorkEventArgs e)
        {
            var t = sender as BackgroundWorker;
            var selectedPrinter = (DAL.Hardware.AtumPrinter)e.Argument;
            var currentProgress = 5;
            t.ReportProgress(currentProgress, new WaitWindowUserState(currentProgress, "Downloading Metadata file..."));

            var materialRootFileStream = string.Empty;
            if (selectedPrinter is DAL.Hardware.AtumDLPStation5)
            {
                switch (selectedPrinter.PrinterXYResolution)
                {
                    case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                        materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.atum3D_MaterialCatalog_DLPStation5_XY50);
                        break;
                    case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75: 
                        materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.atum3D_MaterialCatalog_DLPStation5_XY75);
                        break;
                    case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                        materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.atum3D_MaterialCatalog_DLPStation5_XY100);
                        break;
                }
            }
            else if (selectedPrinter is DAL.Hardware.LoctiteV10)
                {
                    switch (selectedPrinter.PrinterXYResolution)
                    {
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron50:
                            materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.Henkel_MaterialCatalog_LoctiteV10_XY50);
                            break;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron75:
                            materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.Henkel_MaterialCatalog_LoctiteV10_XY75);
                            break;
                        case DAL.Hardware.AtumPrinter.PrinterXYResolutionType.Micron100:
                            materialRootFileStream = DownloadManager.DownloadString(Properties.Settings.Default.Henkel_MaterialCatalog_LoctiteV10_XY100);
                            break;
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

                foreach(var materialDownloadFile in materialRootDownloadFile.AvailableMaterialURLs)
                {
                    currentProgress = (int)(currentProgress + materialDownloadPercentage);
                    if (currentProgress > 100)
                    {
                        currentProgress = 100;
                    }
                    t.ReportProgress(currentProgress, new WaitWindowUserState(currentProgress, "Downloading material file..."));

                    try
                    {
                        //download material
                        materialRootFileStream = DownloadManager.DownloadString(materialDownloadFile.URL);
                        materialRootFileStream = materialRootFileStream.Substring(materialRootFileStream.IndexOf('<'));
                        materialRootFileStream = materialRootFileStream.Replace("&", "&amp;");

                        var materialsBySupplierSerializer = new System.Xml.Serialization.XmlSerializer(typeof(DAL.Materials.MaterialsBySupplier));
                        var materialBySupplier = (DAL.Materials.MaterialsBySupplier)materialsBySupplierSerializer.Deserialize(new StringReader(materialRootFileStream));
                        if (materialBySupplier != null && materialBySupplier.Materials.Count > 0)
                        {
                            if (!this._allOnlineMaterials.ContainsKey((int)materialDownloadFile.XYMicron))
                            {
                                this._allOnlineMaterials.Add((int)materialDownloadFile.XYMicron, new DAL.Materials.MaterialsCatalog());
                            }
                            this._allOnlineMaterials[(int)materialDownloadFile.XYMicron].Add(materialBySupplier);
                        }
                    }
                    catch (Exception exc)
                    {

                    }
                }
            }

            t.ReportProgress(100, new WaitWindowUserState(100, "Downloaded supplier materials"));

        }
        
        void materials_downloaded()
        {
            foreach (var catalog in this._allOnlineMaterials)
            {
                var micronTreeNode = new TreeNode();
                micronTreeNode.Name = catalog.Key.ToString();
                micronTreeNode.Text = "XY Resolution: " + catalog.Key.ToString();

                foreach (var supplier in catalog.Value)
                {
                    var supplierTreeNode = new TreeNode();
                    supplierTreeNode.Name = supplier.Supplier;
                    supplierTreeNode.Text = supplier.Supplier;

                    foreach (var material in supplier.Materials)
                    {
                        var materialTreeNode = new TreeNode();
                        materialTreeNode.Name = material.DisplayName;
                        materialTreeNode.Text = material.DisplayName;
                        materialTreeNode.Tag = material;
                        supplierTreeNode.Nodes.Add(materialTreeNode);
                    }

                    micronTreeNode.Nodes.Add(supplierTreeNode);
                    micronTreeNode.Expand();
                }
                this.treeView1.Nodes.Add(micronTreeNode);
            }

            UpdateControls();
        }

        private void UpdateControls()
        {
            this.ChangeControlReadonlyStatus(this.materialEditorMaterial1);

            if (this.treeView1.SelectedNode != null && this.treeView1.SelectedNode.Level == 2)
            {
                this.materialEditorMaterial1.Dock = DockStyle.Fill;
                this.materialEditorMaterial1.Visible = true;
                this.label1.Visible = false;
            }
            else
            {
                this.materialEditorMaterial1.Visible = false;
                this.label1.Visible = true;
            }
            this.ChangeControlReadonlyStatus(this.materialEditorMaterial1);

            this.btnOK.Enabled = this.SelectedMaterials.Count > 0;
        }

        private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (this.treeView1.SelectedNode.Level == 2)
            {
                this.materialEditorMaterial1.CurrentMaterial = this.treeView1.SelectedNode.Tag as DAL.Materials.Material;
            }

            UpdateControls();
        }

        private void ChangeControlReadonlyStatus(Control parentControl)
        {
            foreach (Control control in parentControl.Controls)
            {
                if (!(control is TabControl))
                {
                    if (control is TextBox)
                    {
                        ((TextBox)control).ReadOnly = true;
                    }
                    else if (control is NumericUpDown)
                    {
                        ((NumericUpDown)control).Enabled = false;
                    }
                }

                if (control.HasChildren)
                {
                    ChangeControlReadonlyStatus(control);
                }
            }
        }

        private void treeView1_AfterCheck(object sender, TreeViewEventArgs e)
        {
            foreach (TreeNode childNode in e.Node.Nodes)
            {
                childNode.Checked = e.Node.Checked;

                foreach (TreeNode sibblingNode in childNode.Nodes)
                {
                    sibblingNode.Checked = e.Node.Checked;
                }
            }

            UpdateControls();
        }

    }
}
