using System;
using System.Collections.Generic;
using System.Text;
using Atum.DAL.Hardware;
using System.ComponentModel;
using Atum.Studio.Core.ModelView;
using System.Diagnostics;
using Atum.DAL.Managers;
using Atum.DAL.ApplicationSettings;
using System.IO;
using Atum.Studio.Controls.NewGui.PrinterEditorSettings;
using System.Windows.Forms;
using System.Linq;
using Atum.Studio.Controls.OpenGL;
using Atum.DAL.Hardware.Modules;

namespace Atum.Studio.Core.Managers
{
    public static class PrinterManager
    {
        internal static event EventHandler AvailablePrinters_Changed;

        public static BindingList<AtumPrinter> AvailablePrinters { get; set; }

        internal static System.Xml.Serialization.XmlSerializer _serializer;

        public static void Start()
        {
            _serializer = new System.Xml.Serialization.XmlSerializer(typeof(BindingList<AtumPrinter>));
            AvailablePrinters = new BindingList<AtumPrinter>();

            var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.SettingsPath);
            if (hasLocalWriteAccess&& File.Exists(Settings.PrinterProfilesPath))
            {
                LoggingManager.WriteToLog("Printer Manager", "Loaded settings", Settings.PrinterProfilesPath);

                using (var streamReader = new System.IO.StreamReader(Settings.PrinterProfilesPath, false))
                {
                    var xmlContent = streamReader.ReadToEnd();
                    xmlContent = xmlContent.Replace("<AdditiveManufacturingDevice>", "<AtumPrinter>");
                    xmlContent = xmlContent.Replace("<AdditiveManufacturingDevice ", "<AtumPrinter ");
                    xmlContent = xmlContent.Replace("</AdditiveManufacturingDevice>", "</AtumPrinter>");
                    xmlContent = xmlContent.Replace("<ArrayOfAdditiveManufacturingDevice", "<ArrayOfAtumPrinter");
                    xmlContent = xmlContent.Replace("</ArrayOfAdditiveManufacturingDevice>", "</ArrayOfAtumPrinter>");
                    
                    var xmlContentStreamReader = new StringReader(xmlContent);
                    AvailablePrinters = (BindingList<AtumPrinter>)_serializer.Deserialize(xmlContentStreamReader);
                    foreach (var printer in AvailablePrinters)
                    {
                        printer.AtumPrinter_Loaded();
                    //    printer.HardwareModules.Add(new WiperModule());
                    }
                }

                LoggingManager.WriteToLog("Printer Manager", "Loaded settings", Settings.PrinterProfilesPath);
            }
            else if (System.IO.File.Exists(Settings.RoamingPrinterProfilesPath))
            {
                LoggingManager.WriteToLog("Printer Manager", "Loading settings", Settings.RoamingPrinterProfilesPath);

                using (var streamReader = new System.IO.StreamReader(Settings.RoamingPrinterProfilesPath, false))
                {
                    var xmlContent = streamReader.ReadToEnd();
                    xmlContent = xmlContent.Replace("<AdditiveManufacturingDevice>", "<AtumPrinter>");
                    xmlContent = xmlContent.Replace("<AdditiveManufacturingDevice ", "<AtumPrinter ");
                    xmlContent = xmlContent.Replace("</AdditiveManufacturingDevice>", "</AtumPrinter>");
                    xmlContent = xmlContent.Replace("<ArrayOfAdditiveManufacturingDevice", "<ArrayOfAtumPrinter");
                    xmlContent = xmlContent.Replace("</ArrayOfAdditiveManufacturingDevice>", "</ArrayOfAtumPrinter>");
                    var xmlContentStreamReader = new StringReader(xmlContent);
                    AvailablePrinters = (BindingList<AtumPrinter>)_serializer.Deserialize(xmlContentStreamReader);
                    foreach (var printer in AvailablePrinters)
                    {
                        printer.AtumPrinter_Loaded();
                    //    printer.HardwareModules.Add(new WiperModule());
                    }
                }

                LoggingManager.WriteToLog("Printer Manager", "Loaded settings", Settings.RoamingPrinterProfilesPath);
            }

            if (PrinterManager.AvailablePrinters == null || PrinterManager.AvailablePrinters.Count == 0)
            {
                if (!Process.GetCurrentProcess().ProcessName.ToLower().Contains("cli")){
                    var printjobPropertiesToolbar = SceneControlToolbarManager.Toolbars.OfType<SceneControlPrintJobPropertiesToolbar>().First();
                    printjobPropertiesToolbar.StartPrinterWizard();
                }

            }

            Save();
        }

        internal static void Append(AtumPrinter printer)
        {
            var printerFound = false;
            //
            foreach (var printerConnection in printer.Connections)
            {
                foreach (var availablePrinter in AvailablePrinters)
                {
                    foreach (var networkPrinterConnection in availablePrinter.Connections)
                    {
                        if (networkPrinterConnection.MacAddress == printerConnection.MacAddress)
                        {
                            printerFound = true;
                        }
                    }
                }
            }

            if (!printerFound)
            {
                try
                {
                    AvailablePrinters_Changed?.Invoke(null, null);
                    AvailablePrinters.Add(printer);
                }
                catch (Exception exc)
                {
                    Debug.WriteLine(exc.Message);
                }

            }
        }

        public static void Save()
        {
            try
            {
                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.BasePath);
                if (hasLocalWriteAccess)
                {
                    LoggingManager.WriteToLog("Printer Manager", "Saving settings", Settings.PrinterProfilesPath);
                    if (!System.IO.Directory.Exists(Settings.SettingsPath)) System.IO.Directory.CreateDirectory(Settings.SettingsPath);

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var streamWriter = new System.IO.StreamWriter(memoryStream))
                        {
                            _serializer.Serialize(streamWriter, AvailablePrinters);

                            memoryStream.Position = 0;

                            var memoryStreamContentAsUTF8 = Encoding.UTF8.GetString(memoryStream.GetBuffer());
                            memoryStreamContentAsUTF8 = memoryStreamContentAsUTF8.Replace("ArrayOfAtumPrinter", "ArrayOfAdditiveManufacturingDevice");
                            memoryStreamContentAsUTF8 = memoryStreamContentAsUTF8.Replace("<AtumPrinter", "<AdditiveManufacturingDevice");
                            memoryStreamContentAsUTF8 = memoryStreamContentAsUTF8.Replace("</AtumPrinter", "</AdditiveManufacturingDevice");

                            using (var streamContentWriter = new StreamWriter(Settings.PrinterProfilesPath, false))
                            {
                                streamContentWriter.Write(memoryStreamContentAsUTF8);
                            }
                        }
                    }

                    LoggingManager.WriteToLog("Printer Manager", "Saved settings", Settings.PrinterProfilesPath);
                }
                else
                {

                    if (!System.IO.Directory.Exists(Settings.RoamingSettingsPath)) System.IO.Directory.CreateDirectory(Settings.RoamingSettingsPath);

                    LoggingManager.WriteToLog("Printer Manager", "Saving settings", Settings.RoamingPrinterProfilesPath);

                    using (var memoryStream = new MemoryStream())
                    {
                        using (var streamWriter = new System.IO.StreamWriter(memoryStream))
                        {
                            _serializer.Serialize(streamWriter, AvailablePrinters);

                            memoryStream.Position = 0;

                            var memoryStreamContentAsUTF8 = Encoding.UTF8.GetString(memoryStream.GetBuffer());
                            memoryStreamContentAsUTF8 = memoryStreamContentAsUTF8.Replace("ArrayOfAtumPrinter", "ArrayOfAdditiveManufacturingDevice");
                            memoryStreamContentAsUTF8 = memoryStreamContentAsUTF8.Replace("<AtumPrinter", "<AdditiveManufacturingDevice");
                            memoryStreamContentAsUTF8 = memoryStreamContentAsUTF8.Replace("</AtumPrinter", "</AdditiveManufacturingDevice");

                            using (var streamContentWriter = new StreamWriter(Settings.RoamingPrinterProfilesPath, false))
                            {
                                streamContentWriter.Write(memoryStreamContentAsUTF8);
                            }
                        }
                    }
                    LoggingManager.WriteToLog("Printer Manager", "Saving settings", Settings.RoamingPrinterProfilesPath);
                }
            }
            catch (Exception exc)
            {
                LoggingManager.WriteToLog("Printer Manager", "Saved settings (exception)", exc);
            }
        }
    }
}
