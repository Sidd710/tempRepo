using Atum.DAL.Hardware;
using Atum.DAL.Materials;
using Atum.DAL.Print;
using Atum.Studio;
using Atum.Studio.Core.Engines;
using Atum.Studio.Core.Managers;
using Atum3D.CLI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace atum3D.CLI
{
    class Program
    {

        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            FontManager.LoadDefaultFonts();
            PrinterManager.Start();
            MaterialManager.Start(true);
            UserProfileManager.Start();
            ProgressBarManager.InitialiseMain();
            RegistryManager.GetRegistryProfileSettings();
            PerformanceSettingsManager.Start();
            ServiceProvider.InitializeRenderEngine();

            string[] cmd = args;
           // cmd = Console.ReadLine().Trim().Split('/');

            Dictionary<string, string> keyValues = new Dictionary<string, string>();
            RetrieveKeyValues(cmd, keyValues);

            if (keyValues.ContainsKey("actiontype"))
            {
                var actionType = keyValues["actiontype"];
                //cmd.FirstOrDefault()
                switch (actionType.ToLower())
                {
                    case "project":
                        if (cmd.Length > 1)
                        {
                            string filePath = cmd[1];
                            ServiceProvider.OpenProject(filePath);
                        }
                        break;
                    case "materialselection":
                        ServiceProvider.ShowMaterials();
                        break;
                    case "showprinters":
                        ServiceProvider.ShowPrinters();
                        break;
                    case "materialmanager":
                        ServiceProvider.ShowMaterialManager();
                        break;
                    case "calibration":
                        var printerId = keyValues["printerid"];
                        var printerName = keyValues["printername"];
                        var buildRoomSizePath = keyValues["netfabbbuildroomxml"];
                        var calibratedPrinter = ServiceProvider.CalibratePrinter(printerId, printerName);
                        File.WriteAllText(buildRoomSizePath, string.Format(Atum3D.CLI.Properties.Resources.PrinterCalibration_BuildRoomSize_XML,
                            (calibratedPrinter.MaxBuildSizeX * calibratedPrinter.TrapeziumCorrectionFactorX).ToString(), 
                            (calibratedPrinter.MaxBuildSizeY * calibratedPrinter.TrapeziumCorrectionFactorY).ToString(), 
                            200.ToString()));


                        break;
                    case "createprintjob":
                        var printJobPrinterId = keyValues["printerid"];
                        var printJobPrinterName = keyValues["printername"];
                        var jobName = keyValues["jobname"];
                        var jobPath = keyValues["jobpath"];

                        ServiceProvider.GeneratePrintJob(printJobPrinterId, printJobPrinterName, jobName, jobPath);
                        break;
                }
            }
            Console.WriteLine("Please provide ActionType.");
        }

        private static void RetrieveKeyValues(string[] cmd, Dictionary<string, string> keyValues)
        {
            if (cmd.Length > 0)
            {
                for(var itemIndex = 0; itemIndex<cmd.Length; itemIndex++)
                {
                    var item = cmd[itemIndex];

                    if (item.StartsWith("/"))
                    {
                        item = item.Substring(1);
                    }

                    if (item.IndexOf(":") != -1)
                    {
                        var columnIndex = item.IndexOf(":");
                        var key = item.Substring(0, columnIndex).ToLower();
                        var value = item.Substring(columnIndex + 1).Replace("^", string.Empty).Trim(' ');
                        if (!keyValues.ContainsKey(key))
                        {
                            keyValues.Add(key, value);
                        }
                    }
                }
            }
        }
    }
}
