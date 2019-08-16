using Atum.DAL.ApplicationSettings;
using Atum.DAL.Managers;
using Atum.DAL.Materials;
using Atum.Studio.Controls;
using Atum.Studio.Controls.NewGui.MaterialEditor;
using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Atum.Studio.Core.Managers
{
    public class MaterialManager
    {
        private static MaterialsCatalog _materialCatalog;

        private static event EventHandler<string> MaterialFileAdded;
        private static event EventHandler<string> MaterialFileChanged;
        private static event EventHandler<string> MaterialFileRemoved;

        internal static event EventHandler<MaterialSummary> SelectedMaterialUpdated;

        private static DateTime LastAddedFileEvent;
        private static string LastAddedFilePath;


        private static DateTime LastChangedFileEvent;
        private static string LastChangedFilePath;

        private static FileSystemWatcher _fileSystemWatcher;

        public static void Start(bool addHandlers)
        {

            //bind manufacturer comboxbox
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
            _materialCatalog = new MaterialsCatalog();

            //if no material catalog is found then save default items
            try
            {
                //                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.BasePath);
                //                if (hasLocalWriteAccess)
                //                {
                //                    LoggingManager.WriteToLog("Material Manager", "Base Path", Settings.MaterialsPath);

                //                    if (!Directory.Exists(Settings.MaterialsPath))
                //                    {
                //                        LoggingManager.WriteToLog("Material Manager", "Creating Base Path", Settings.MaterialsPath);

                //                        Directory.CreateDirectory(Settings.MaterialsPath);

                //                        LoggingManager.WriteToLog("Material Manager", "Created Base Path", Settings.MaterialsPath);

                //                        //add default materials
                //                        LoggingManager.WriteToLog("Material Manager", "Saving default materials", Settings.MaterialsPath);

                //#if LOCTITE
                //                        var defaultMaterialFilePath = Path.Combine(Settings.MaterialsPath, "Henkel.xml");
                //                        File.WriteAllBytes(defaultMaterialFilePath, Properties.Resources.Material_Henkel_xml);
                //#else
                //                        var defaultMaterialFilePath = Path.Combine(Settings.MaterialsPath, "3DMaterials.xml");
                //                        File.WriteAllBytes(defaultMaterialFilePath, Properties.Resources.Material_3DMaterials_xml);
                //#endif

                //                        LoggingManager.WriteToLog("Material Manager", "Saved default materials", Settings.MaterialsPath);
                //                    }
                //                }
                //                else
                {
                    LoggingManager.WriteToLog("Material Manager", "Base Path", Settings.RoamingMaterialsPath);

                    if (!Directory.Exists(Settings.RoamingMaterialsPath))
                    {
                        LoggingManager.WriteToLog("Material Manager", "Creating Base Path", Settings.RoamingMaterialsPath);

                        Directory.CreateDirectory(Settings.RoamingMaterialsPath);

                        LoggingManager.WriteToLog("Material Manager", "Created Base Path", Settings.RoamingMaterialsPath);
                    }
                    //add default materials
                    if ((!Directory.Exists(Settings.MaterialsPath) && Directory.GetFiles(Settings.RoamingMaterialsPath).Length == 0))
                    {
                        LoggingManager.WriteToLog("Material Manager", "Saving default materials", Settings.RoamingMaterialsPath);

#if LOCTITE
                        File.WriteAllBytes(Path.Combine(Settings.RoamingMaterialsPath, "Henkel.xml"), Properties.Resources.Material_Henkel_xml);
#else
                        File.WriteAllBytes(Path.Combine(Settings.RoamingMaterialsPath, "3D-Materials.xml"), Properties.Resources.Material_3DMaterials_xml);
#endif


                        LoggingManager.WriteToLog("Material Manager", "Saved default materials", Settings.RoamingMaterialsPath);
                    }
                    else if ((Directory.Exists(Settings.MaterialsPath) && Directory.GetFiles(Settings.MaterialsPath).Length == 0))
                    {
                        try
                        {
                            LoggingManager.WriteToLog("Material Manager", "Saving default materials", Settings.MaterialsPath);

#if LOCTITE
                        File.WriteAllBytes(Path.Combine(Settings.MaterialsPath, "Henkel.xml"), Properties.Resources.Material_Henkel_xml);
#else
                            File.WriteAllBytes(Path.Combine(Settings.MaterialsPath, "3D-Materials.xml"), Properties.Resources.Material_3DMaterials_xml);
#endif

                            LoggingManager.WriteToLog("Material Manager", "Saved default materials", Settings.RoamingMaterialsPath);
                        }
                        catch
                        {

                        }

                    }
                }

                if (Directory.Exists(Settings.MaterialsPath))
                {
                    foreach (var supplierXML in Directory.GetFiles(Settings.MaterialsPath))
                    {
                        LoggingManager.WriteToLog("Material Manager", "Loading material", supplierXML);

                        var updateMaterialFile = false;
                        MaterialsBySupplier updateSupplierMaterials = null;
                        using (var materialReader = new StreamReader(supplierXML))
                        {
                            var supplierMaterials = (MaterialsBySupplier)serializer.Deserialize(materialReader);
                            supplierMaterials.FilePath = supplierXML.Replace("/", "\\");

                            LoggingManager.WriteToLog("Material Manager", "Loaded material", supplierXML);
                            _materialCatalog.Add(supplierMaterials);

                            foreach (var material in supplierMaterials.Materials)
                            {
                                if (material.Id == Guid.Empty)
                                {
                                    material.Id = Guid.NewGuid();
                                    updateMaterialFile = true;
                                }

                                //material.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = 100, LI = 15 });

                                if (material.SupportProfiles == null || material.SupportProfiles.Count == 0)
                                {
                                    material.SupportProfiles.Add(new SupportProfile());
                                    material.SupportProfiles[0].Selected = true;
                                    material.SupportProfiles[0].Default = true;
                                }
                            }

                            updateSupplierMaterials = supplierMaterials;

                            materialReader.Close();
                        }

                        if (updateMaterialFile)
                        {
                            updateSupplierMaterials.SaveToFile();
                        }
                    }

                    if (addHandlers)
                    {
                        AddMaterialWatcher(Settings.MaterialsPath.Replace("/", "\\"));
                    }
                }

                else if (Directory.Exists(Settings.RoamingMaterialsPath))
                {
                    foreach (var supplierXML in Directory.GetFiles(Settings.RoamingMaterialsPath))
                    {
                        LoggingManager.WriteToLog("Material Manager", "Loading material", supplierXML);

                        var updateMaterialFile = false;
                        MaterialsBySupplier updateSupplierMaterials = null;
                        using (var materialReader = new StreamReader(supplierXML))
                        {
                            var supplierMaterials = (MaterialsBySupplier)serializer.Deserialize(materialReader);
                            supplierMaterials.FilePath = supplierXML.Replace("/", "\\"); ;

                            LoggingManager.WriteToLog("Material Manager", "Loaded material", supplierXML);

                            var supplierNameFound = false;
                            foreach (var supplier in _materialCatalog)
                            {
                                if (!string.IsNullOrEmpty(supplier.Supplier))
                                {
                                    if (supplier.Supplier.ToLower().Trim() == supplierMaterials.Supplier.ToLower().Trim())
                                    {
                                        supplierNameFound = true;
                                        break;
                                    }
                                }
                            }

                            if (!supplierNameFound)
                            {
                                foreach (var material in supplierMaterials.Materials)
                                {
                                    if (material.Id == Guid.Empty)
                                    {
                                        material.Id = Guid.NewGuid();
                                        updateMaterialFile = true;
                                    }
                                    // material.TransitionLayers.Add(new DAL.Catalogs.MaterialTransitionLayer() { CT = 100, LI = 15 });

                                    if (material.SupportProfiles == null || material.SupportProfiles.Count == 0)
                                    {
                                        material.SupportProfiles.Add(SupportProfile.CreateDefault());
                                    }
                                }
                                _materialCatalog.Add(supplierMaterials);
                            }

                            updateSupplierMaterials = supplierMaterials;

                            materialReader.Close();

                            if (updateMaterialFile)
                            {
                                updateSupplierMaterials.SaveToFile();
                            }
                        }
                    }

                    if (addHandlers)
                    {
                        AddMaterialWatcher(Settings.RoamingMaterialsPath.Replace("/", "\\"));
                    }
                }
            }
            catch (Exception exc)
            {
                new frmMessageBox("Material Manager", exc.ToString(), MessageBoxButtons.OK, MessageBoxDefaultButton.Button2).ShowDialog();
                LoggingManager.WriteToLog("Material Manager", "Exception", exc);
            }

        }

        internal static bool _stopped;
        internal static void Stop()
        {
            _stopped = true;
        }

        private static void MaterialManager_MaterialFileAdded(object sender, string e)
        {
            System.Threading.Thread.Sleep(500);
            AddSupplier(e);
        }

        private static void AddSupplier(string filePath)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
            if (File.Exists(filePath))
            {
                using (var materialReader = new StreamReader(filePath))
                {
                    var newSupplier = (MaterialsBySupplier)serializer.Deserialize(materialReader);
                    newSupplier.FilePath = filePath.Replace("/", "\\");
                    materialReader.Close();

                    var supplierExists = false;
                    foreach(var supplier in _materialCatalog)
                    {
                        if (supplier.FilePath == newSupplier.FilePath)
                        {
                            supplierExists = true;
                            break;
                        }
                        }

                    if (!supplierExists)
                    {
                        _materialCatalog.Add(newSupplier);
                    }
                    
                }
            }
        }

        private static void MaterialManager_MaterialFileChanged(object sender, string e)
        {
            System.Threading.Thread.Sleep(500);
            UpdateSupplierMaterials(e);
        }

        private static void UpdateSupplierMaterials(string filePath)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
            if (File.Exists(filePath))
            {
                using (var materialReader = new StreamReader(filePath))
                {
                    var updatedSupplierMaterial = (MaterialsBySupplier)serializer.Deserialize(materialReader);
                    updatedSupplierMaterial.FilePath = filePath.Replace("/", "\\");
                    materialReader.Close();

                    //find current supplier
                    MaterialsBySupplier currentSupplier = _materialCatalog.FirstOrDefault(s => s.FilePath == filePath);
                    if (currentSupplier != null)
                    {
                        //check if current selectedMaterial is part of supplier materials
                        var selectedMaterialSummary = PrintJobManager.SelectedMaterialSummary;
                        if (selectedMaterialSummary != null && selectedMaterialSummary.Material != null && selectedMaterialSummary.Material.Id != Guid.Empty)
                        {
                            var selectedMaterial = updatedSupplierMaterial.FindMaterialById(selectedMaterialSummary.Material.Id);
                            if (selectedMaterial != null && selectedMaterial.ChangeId != selectedMaterialSummary.Material.ChangeId)
                            {
                                selectedMaterialSummary.Material = selectedMaterial;
                                selectedMaterialSummary.Supplier = updatedSupplierMaterial.Supplier;
                                SelectedMaterialUpdated?.Invoke(null, selectedMaterialSummary);
                            }
                        }

                        //update materials
                        foreach (var updatedMaterial in updatedSupplierMaterial.Materials)
                        {
                            var materialIdExists = false;
                            for (var currentMaterialIndex = 0; currentMaterialIndex < currentSupplier.Materials.Count; currentMaterialIndex++)
                            {
                                if (currentSupplier.Materials[currentMaterialIndex].Id == updatedMaterial.Id)
                                {
                                    currentSupplier.Materials[currentMaterialIndex] = updatedMaterial;
                                    materialIdExists = true;
                                    break;
                                }
                            }

                            if (!materialIdExists)
                            {
                                currentSupplier.Materials.Add(updatedMaterial);
                            }
                        }
                    }
                }
            }
        }

        public static void RemoveSupplier(MaterialsBySupplier selectedSupplier)
        {
            foreach (var supplier in Catalog)
            {
                if (selectedSupplier.Supplier == supplier.Supplier)
                {
                    if (selectedSupplier.FilePath != null && File.Exists(selectedSupplier.FilePath))
                    {
                        File.Delete(selectedSupplier.FilePath);
                    }

                    Catalog.Remove(supplier);

                    break;
                }
            }
        }

        public static void SaveMaterial(MaterialsBySupplier selectedSupplier)
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
            //if no material catalog is found then save default items
            try
            {
                if (!Directory.Exists(Settings.RoamingMaterialsPath))
                {
                    LoggingManager.WriteToLog("Material Manager", "Creating materials path", Settings.RoamingMaterialsPath);

                    Directory.CreateDirectory(Settings.RoamingMaterialsPath);

                    LoggingManager.WriteToLog("Material Manager", "Created materials path", Settings.RoamingMaterialsPath);
                }


                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.BasePath);
                if (hasLocalWriteAccess)
                {
                    if (!Directory.Exists(Settings.MaterialsPath))
                    {
                        LoggingManager.WriteToLog("Material Manager", "Creating materials path", Settings.MaterialsPath);

                        Directory.CreateDirectory(Settings.MaterialsPath);

                        LoggingManager.WriteToLog("Material Manager", "Created materials path", Settings.MaterialsPath);
                    }

                    var supplierMaterialFilePath = Path.Combine(Settings.MaterialsPath, selectedSupplier.Supplier + ".xml");
                    using (var streamWriter = new StreamWriter(supplierMaterialFilePath))
                    {
                        try
                        {
                            foreach (var material in selectedSupplier.Materials)
                            {
                                if (string.IsNullOrEmpty(material.DisplayName))
                                {
                                    material.DisplayName = material.Name;
                                }
                            }

                            LoggingManager.WriteToLog("Material Manager", "Saving material settings file", supplierMaterialFilePath);

                            serializer.Serialize(streamWriter, selectedSupplier);

                            LoggingManager.WriteToLog("Material Manager", "Saved material settings file", supplierMaterialFilePath);
                        }
                        catch (Exception exc)
                        {
                            LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file (exception)", exc);
                        }

                        streamWriter.Close();

                    }
                }
                else
                {

                    var supplierMaterialFilePath = Path.Combine(Settings.RoamingMaterialsPath, selectedSupplier.Supplier + ".xml");
                    using (var streamWriter = new StreamWriter(supplierMaterialFilePath, false))
                    {
                        try
                        {
                            foreach (var material in selectedSupplier.Materials)
                            {
                                if (string.IsNullOrEmpty(material.DisplayName))
                                {
                                    material.DisplayName = material.Name;
                                }
                            }

                            LoggingManager.WriteToLog("Material Manager", "Saving material settings file", supplierMaterialFilePath);

                            serializer.Serialize(streamWriter, selectedSupplier);

                            LoggingManager.WriteToLog("Material Manager", "Saved material settings file", supplierMaterialFilePath);
                        }
                        catch (Exception exc)
                        {
                            LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file (exception)", exc);
                        }

                        streamWriter.Close();
                    }

                }

            }
            catch
            {

            }
        }


        public static void SaveAllMaterials()
        {
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
            //if no material catalog is found then save default items
            try
            {
                if (!Directory.Exists(Settings.RoamingMaterialsPath))
                {
                    LoggingManager.WriteToLog("Material Manager", "Creating materials path", Settings.RoamingMaterialsPath);

                    Directory.CreateDirectory(Settings.RoamingMaterialsPath);

                    LoggingManager.WriteToLog("Material Manager", "Created materials path", Settings.RoamingMaterialsPath);
                }


                var hasLocalWriteAccess = FileSystemManager.HasWriteAccess(Settings.BasePath);
                if (hasLocalWriteAccess)
                {
                    if (!Directory.Exists(Settings.MaterialsPath))
                    {
                        LoggingManager.WriteToLog("Material Manager", "Creating materials path", Settings.MaterialsPath);

                        Directory.CreateDirectory(Settings.MaterialsPath);

                        LoggingManager.WriteToLog("Material Manager", "Created materials path", Settings.MaterialsPath);
                    }
                    foreach (var materialSupplierFile in Directory.GetFiles(Settings.MaterialsPath))
                    {
                        try
                        {
                            LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file", materialSupplierFile);

                            File.Delete(materialSupplierFile);

                            LoggingManager.WriteToLog("Material Manager", "Removed previous material settings file", materialSupplierFile);
                        }
                        catch (Exception exc)
                        {
                            LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file (exception)", exc);
                        }
                    }


                    foreach (var materialSupplier in _materialCatalog)
                    {
                        var supplierMaterialFilePath = Path.Combine(Settings.MaterialsPath, materialSupplier.Supplier + ".xml");
                        using (var streamWriter = new StreamWriter(supplierMaterialFilePath))
                        {
                            try
                            {
                                foreach (var material in materialSupplier.Materials)
                                {
                                    if (string.IsNullOrEmpty(material.DisplayName))
                                    {
                                        material.DisplayName = material.Name;
                                    }
                                }

                                LoggingManager.WriteToLog("Material Manager", "Saving material settings file", supplierMaterialFilePath);

                                serializer.Serialize(streamWriter, materialSupplier);

                                LoggingManager.WriteToLog("Material Manager", "Saved material settings file", supplierMaterialFilePath);
                            }
                            catch (Exception exc)
                            {
                                LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file (exception)", exc);
                            }

                            streamWriter.Close();
                        }
                    }
                }
                else
                {

                    foreach (var materialSupplierFile in Directory.GetFiles(Settings.RoamingMaterialsPath))
                    {
                        try
                        {
                            LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file", materialSupplierFile);

                            File.Delete(materialSupplierFile);

                            LoggingManager.WriteToLog("Material Manager", "Removed previous material settings file", materialSupplierFile);
                        }
                        catch (Exception exc)
                        {
                            LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file (exception)", exc);
                        }
                    }

                    foreach (var materialSupplier in _materialCatalog)
                    {
                        var supplierMaterialFilePath = Path.Combine(Settings.RoamingMaterialsPath, materialSupplier.Supplier + ".xml");
                        using (var streamWriter = new StreamWriter(supplierMaterialFilePath, false))
                        {
                            try
                            {
                                foreach (var material in materialSupplier.Materials)
                                {
                                    if (string.IsNullOrEmpty(material.DisplayName))
                                    {
                                        material.DisplayName = material.Name;
                                    }
                                }

                                LoggingManager.WriteToLog("Material Manager", "Saving material settings file", supplierMaterialFilePath);

                                serializer.Serialize(streamWriter, materialSupplier);

                                LoggingManager.WriteToLog("Material Manager", "Saved material settings file", supplierMaterialFilePath);
                            }
                            catch (Exception exc)
                            {
                                LoggingManager.WriteToLog("Material Manager", "Removing previous material settings file (exception)", exc);
                            }

                            streamWriter.Close();
                        }

                    }
                }


            }
            catch
            {

            }
        }

        public static MaterialsCatalog Catalog
        {
            get
            {
                return _materialCatalog;
            }
            set
            {
                _materialCatalog = value;
            }
        }

        private static void AddMaterialWatcher(string materialPath)
        {
            LastAddedFilePath = string.Empty;
            LastAddedFileEvent = DateTime.Now;

            LastChangedFilePath = string.Empty;
            LastChangedFileEvent = DateTime.Now;

            MaterialFileAdded += MaterialManager_MaterialFileAdded;
            MaterialFileChanged += MaterialManager_MaterialFileChanged;

            _fileSystemWatcher = new FileSystemWatcher();
            _fileSystemWatcher.Path = materialPath.Substring(0, materialPath.Length - 1);
            _fileSystemWatcher.NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.CreationTime;
            _fileSystemWatcher.Changed += _fileSystemWatcher_Changed;
            _fileSystemWatcher.Created += _fileSystemWatcher_Created;
            _fileSystemWatcher.Deleted += _fileSystemWatcher_Deleted;
            _fileSystemWatcher.Filter = "*.xml";
            _fileSystemWatcher.IncludeSubdirectories = false;
            _fileSystemWatcher.EnableRaisingEvents = true;
        }

        private static void _fileSystemWatcher_Deleted(object sender, FileSystemEventArgs e)
        {
            if (!_stopped)
            {
                MaterialFileRemoved?.Invoke(null, e.FullPath);
            }
        }

        private static void _fileSystemWatcher_Created(object sender, FileSystemEventArgs e)
        {

            if (LastAddedFilePath != e.FullPath || (DateTime.Now - LastAddedFileEvent).TotalSeconds > 1)
            {
                if (!_stopped)
                {
                    LastAddedFileEvent = DateTime.Now;
                    LastAddedFilePath = e.FullPath;

                    MaterialFileAdded?.Invoke(null, e.FullPath);
                }
            }
        }

        private static void _fileSystemWatcher_Changed(object sender, FileSystemEventArgs e)
        {
            if (LastChangedFilePath != e.FullPath || (DateTime.Now - LastChangedFileEvent).TotalSeconds > 1)
            {
                //prevent duplicate events
                if (!_stopped)
                {
                    LastChangedFileEvent = DateTime.Now;
                    LastChangedFilePath = e.FullPath;
                    MaterialFileChanged?.Invoke(null, e.FullPath);
                }
            }
        }
    }
}
