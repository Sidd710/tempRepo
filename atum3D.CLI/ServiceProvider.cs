using Atum.DAL.Compression.Zip;
using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.DAL.Print;
using Atum.Studio.Controls.NewGui;
using Atum.Studio.Controls.NewGui.ExportControl;
using Atum.Studio.Controls.NewGui.MaterialCatalogEditor;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using Atum.Studio.Controls.NewGui.PrinterEditorSettings;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;
using Atum.Studio.Core.Models;
using Atum.Studio.Core.ModelView;
using atum3D.Notification;
using Atum3D.CLI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atum3D.CLI
{
    public static class ServiceProvider
    {
        private static bool _closeMainApplication;
        public static void ShowMaterials()
        {
            using (var materialEditor = new frmMaterialEditor())
            {
                materialEditor.onSelectionChanged += MaterialEditor_onSelectionChanged;
                SceneControlToolbarManager.PrintJobProperties = new Atum.Studio.Controls.OpenGL.SceneControlPrintJobPropertiesToolbar(null);
                SceneControlToolbarManager.SelectedPrinter = new LoctiteV10();

                Material selectedMaterial = null;
                if (UserProfileManager.UserProfile != null && UserProfileManager.UserProfile.SelectedMaterial != null)
                {
                    selectedMaterial = UserProfileManager.UserProfile.SelectedMaterial;
                }
                else if (materialEditor.SelectedMaterial != null)
                {
                    selectedMaterial = materialEditor.SelectedMaterial;
                }
                materialEditor.LoadMaterials(selectedMaterial);
                materialEditor.ShowDialog();
            }
        }

        private static void MaterialEditor_onSelectionChanged(object sender, EventArgs e)
        {
            MaterialSummary selectedMaterialSummary = sender as MaterialSummary;
            if (selectedMaterialSummary != null)
            {
                if (selectedMaterialSummary.Selected)
                {
                    UserProfileManager.UserProfile.SelectedMaterial = selectedMaterialSummary.Material;
                    UserProfileManager.Save();
                    MaterialManager.SaveAllMaterials();
                }
            }
        }
        public static void ShowPrinters()
        {
            AtumPrinter currentPrinter = null;
            if (UserProfileManager.UserProfile != null && UserProfileManager.UserProfile.SelectedPrinter != null)
            {
                currentPrinter = UserProfileManager.UserProfile.SelectedPrinter;
            }
            using (var printerEditor = new frmPrinterEditor())
            {

                printerEditor.LoadPrinters(currentPrinter);
                printerEditor.ShowDialog();
            }
        }

        public static void ShowMaterialManager()
        {
            using (var materialManager = new frmMaterialCatalogManager())
            {
                materialManager.ToolbarXYResolutionVisible = false;
                materialManager.TabMagsAIVisible = false;
                materialManager.ShowDialog();
            }
        }

        public static void OpenProject(string filePath)
        {
            if (!string.IsNullOrEmpty(filePath))
            {
                    ImportWorkspaceFile(filePath);
            }
            else
            {
                Console.WriteLine("Project file path is required.");
            }
        }

        internal static AtumPrinter CalibratePrinter(string printerId, string printerName)
        {
            if (!string.IsNullOrEmpty(printerId) && !string.IsNullOrEmpty(printerName))
            {
                var availablePrinters = PrinterManager.AvailablePrinters;
                var existingPrinter = availablePrinters.Where(x => x.SerialNumber == printerId).FirstOrDefault();
                if (existingPrinter != null)
                {
                    //printer already available in the list
                    Console.WriteLine(printerName + " with the serial key " + printerId + " is already exist.");
                }
                else
                {
                    AtumPrinter atumPrinter = new LoctiteV10();
                    atumPrinter.SerialNumber = printerId;
                    atumPrinter.DisplayName = printerName;
                    atumPrinter.SetDefaultPrinterResolution(AtumPrinter.PrinterXYResolutionType.Micron100);
                    (atumPrinter as LoctiteV10).CalcDefaultTrapezoidValues();
                    atumPrinter.ID = Guid.NewGuid().ToString().ToUpper();
                    PrinterManager.AvailablePrinters.Add(atumPrinter);
                    PrinterManager.Save();
                    Console.WriteLine(printerName + " added successfully.");
                }
            }
            else
            {
                Console.WriteLine("PrinterId and PrinterName must be provided.");
                return null;
            }

            var selectedPrinter = PrinterManager.AvailablePrinters.Where(x => x.SerialNumber == printerId).FirstOrDefault();
            var printerCalibrationDialog = new frmCalibratePrinter(selectedPrinter);
            printerCalibrationDialog.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            var result = printerCalibrationDialog.ShowDialog();
            PrinterManager.Save();
            return selectedPrinter;
        }

        private static void ImportWorkspaceFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                ObjectView.FileProcessed += ObjectView_FileProcessed;
                ObjectView.Create(new[] { filePath }, Color.Blue, ObjectView.BindingSupported, null);

                string jobName = (new FileInfo(filePath)).Name;
                jobName = jobName.Substring(0, jobName.LastIndexOf("."));
                RenderEngine.PrintJob.Name = !string.IsNullOrEmpty(jobName) ? jobName : "Untitled";
            }
            else
            {
                MessageBox.Show("Cached temp file not found", "OS3 integration warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

        }

        private static void ObjectView_FileProcessed(object sender, EventArgs e)
        {
            var progressWindow = new frmCLIProgress();
            var generatePrintjobTask = GenerateExportAsync(progressWindow);
            var progressResult = progressWindow.ShowDialog();
            generatePrintjobTask.Wait();

            GenerateJob_RunWorkerCompleted();
        }

        private static Task GenerateExportAsync(frmCLIProgress progressWindow)
        {
            return Task.Run(() =>
            {
                if (RenderEngine.PrintJob != null)
                {
                    RenderEngine.PreRender();
                    RenderEngine.Render();

                    while (RenderEngine.TotalAmountSlices != RenderEngine.TotalProcessedSlices || RenderEngine._cancelRendering)
                    {
                        float progress = (float)Math.Round((RenderEngine.TotalProcessedSlices / (float)RenderEngine.TotalAmountSlices * 100), 2);
                        progressWindow.SetPercentage(progress);
                        Thread.Sleep(250);
                    }

                    progressWindow.Invoke(new MethodInvoker(delegate
                    {
                        progressWindow.Close();
                    }));

                }
            });
        }

        public static void InitializeRenderEngine()
        {
            AtumPrinter atumPrinter = null;
            if (UserProfileManager.UserProfile != null && UserProfileManager.UserProfile.SelectedPrinter != null)
            {
                var defaultPrinter = UserProfileManager.UserProfile.SelectedPrinter;
                if (defaultPrinter != null)
                {
                    atumPrinter = defaultPrinter;
                }
            }

            Material material = null;
            if (UserProfileManager.UserProfile != null && UserProfileManager.UserProfile.SelectedMaterial != null)
            {
                var defaultMaterial = UserProfileManager.UserProfile.SelectedMaterial;
                if (defaultMaterial != null)
                {
                    material = defaultMaterial;
                }
            }

            if (material == null)
            {
                material = MaterialManager.Catalog.First().Materials.First();
            }

            var printJob = new PrintJob();
            printJob.Name = "Untitled";
            printJob.SelectedPrinter = atumPrinter;
            printJob.Material = material;

            RenderEngine.PrintJob = printJob;
        }

        internal static void GeneratePrintJob(string printerId, string printerName, string jobName, string netFabbFilePath)
        {
            if (!string.IsNullOrEmpty(printerId) && !string.IsNullOrEmpty(printerName))
            {
                AtumPrinter selectPrinter = null;
                var availablePrinters = PrinterManager.AvailablePrinters;
                var existingPrinter = availablePrinters.Where(x => x.SerialNumber == printerId).FirstOrDefault();
                if (existingPrinter != null)
                {
                    selectPrinter = existingPrinter;
                }
                else
                {
                    selectPrinter = CalibratePrinter(printerId, printerName);
                }
                PrintJobManager.SelectedPrinter = selectPrinter;
            }
            else
            {
                //set printer if no printer has been selected in PrintJobManager
                var defaultPrinter = UserProfileManager.UserProfile.SelectedPrinter;
                if (defaultPrinter != null)
                {
                    if (PrintJobManager.SelectedPrinter == null)
                    {
                        PrintJobManager.SelectedPrinter = defaultPrinter;
                    }
                }
            }

            Material selectedMaterial = null;
            if (UserProfileManager.UserProfile.SelectedMaterial != null && UserProfileManager.UserProfile.SelectedMaterial.Id != Guid.Empty)
            {
                selectedMaterial = MaterialManager.Catalog.FindMaterialById(UserProfileManager.UserProfile.SelectedMaterial.Id);
            }
            if (selectedMaterial == null)
            {
                selectedMaterial = MaterialManager.Catalog.First().Materials.First();
            }

            RenderEngine.PrintJob.Name = jobName;

            RenderEngine.PrintJob.Material = selectedMaterial;

            OpenProject(netFabbFilePath);

            while (!_closeMainApplication)
            {
                Thread.Sleep(500);
            }


        }

        private static void GenerateJob_RunWorkerCompleted()
        {
            var frmExportControl = new frmExportControl();
            var exportUserControl = new ExportUserControl();

            frmExportControl.FormClosing += FrmExportControl_FormClosing;

            frmExportControl.Height = exportUserControl.Height;
            frmExportControl.Width = exportUserControl.Width;
            frmExportControl.Controls.Add(exportUserControl);
            exportUserControl.Dock = DockStyle.Fill;

            exportUserControl.InitPrintJob();

            var result = frmExportControl.ShowDialog();
        }

        private static void FrmExportControl_FormClosing(object sender, FormClosingEventArgs e)
        {
            _closeMainApplication = true;
        }
    }
}
