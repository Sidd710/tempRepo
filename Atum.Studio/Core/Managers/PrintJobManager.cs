using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.DAL.Print;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Managers
{
    public class PrintJobManager
    {
        internal static PrintJob CurrentPrintJobSettings
        {
            get
            {
                var printJob = new PrintJob();
                printJob.Name = PrintjobName;
                printJob.SelectedPrinter = SelectedPrinter;

                if (SelectedMaterialSummary.Material != null)
                {
                    printJob.Material = SelectedMaterialSummary.Material;
                }
                else
                {
                    if (printJob.SelectedPrinter != null) {
                        foreach (var supplier in MaterialManager.Catalog)
                        {
                            if (supplier.GetMaterialsByResolution(SelectedPrinter).Count > 0)
                            {
                                printJob.Material = supplier.GetMaterialsByResolution(SelectedPrinter)[0].Materials[0];
                                break;
                            }
                        }
                    }
                }



                return printJob;
            }
        }

        public static AtumPrinter SelectedPrinter
        {
            get
            {
                return SceneControlToolbarManager.SelectedPrinter;
            }
            set
            {
                SceneControlToolbarManager.SelectedPrinter = value;
            }
        }

        public static MaterialSummary SelectedMaterialSummary
        {
            get
            {
                var selectedMaterial = SceneControlToolbarManager.SelectedMaterial;
                if (selectedMaterial == null)
                {
                    return new MaterialSummary();
                }
                else
                {
                    return selectedMaterial;
                }
            }
        }

        internal static string ProjectSaveFilePath { get; set; }

        internal static string PrintjobName
        {
            get
            {
                return SceneControlToolbarManager.PrintjobName;
            }
            set
            {
                SceneControlToolbarManager.PrintjobName = value;
            }
        }
    }
}
