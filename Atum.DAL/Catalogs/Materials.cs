using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Serialization;
using System.Drawing;
using Atum.DAL.Catalogs;
using System.Linq;
using System.IO;
using Atum.DAL.Hardware.Modules;
using System.Windows.Forms;
using Atum.DAL.Hardware;

namespace Atum.DAL.Materials
{

    [Serializable]
    public class Material : INotifyPropertyChanged
    {
        public List<SupportProfile> SupportProfiles;

        public bool IsDefault { get; set; }
        public int XYResolution { get; set; }
        public string PrinterHardwareType { get; set; }

        private int _initialLayers;

        public int InitialLayers
        {
            get { return this._initialLayers; }
            set
            {
                SetField(ref this._initialLayers, value, "InitialLayers");
                this._initialLayers = value;
            }
        }
        private int _preparationLayersCount;
        public int PreparationLayersCount
        {
            get
            {
                return this._preparationLayersCount;
            }
            set
            {
                SetField(ref this._preparationLayersCount, value, "PreparationLayersCount");
                this._preparationLayersCount = value;
            }
        }

        private string _uniqueNumber;
        public string UniqueNumber
        {
            get
            {
                return this._uniqueNumber;
            }
            set
            {
                SetField(ref this._uniqueNumber, value, "UniqueNumber");
                this._uniqueNumber = value;
            }
        }

        private string _color;
        public string Color
        {
            get
            {
                return this._color;
            }
            set
            {
                SetField(ref this._color, value, "Color");
                this._color = value;
            }
        }

        private string _articleNumber;
        public string ArticleNumber
        {
            get
            {
                return this._articleNumber;
            }
            set
            {
                SetField(ref this._articleNumber, value, "ArticleNumber");
                this._articleNumber = value;
            }
        }
        private string _articleHTTP;
        public string ArticleHTTP
        {
            get
            {
                return this._articleHTTP;
            }
            set
            {
                SetField(ref this._articleHTTP, value, "ArticleHTTP");
                this._articleHTTP = value;
            }
        }

        private string _batchNumber;
        public string BatchNumber
        {
            get
            {
                return this._batchNumber;
            }
            set
            {
                SetField(ref this._batchNumber, value, "BatchNumber");
                this._batchNumber = value;
            }
        }

        private decimal _price;
        public decimal Price
        {
            get
            {
                return this._price;
            }
            set
            {
                SetField(ref this._price, value, "Price");
                this._price = value;
            }
        }

        private string _name;
        public string Name
        {
            get
            {
                return this._name;
            }
            set
            {
                SetField(ref this._name, value, "Name");
                this._name = value;
            }
        }

        private string _displayName;
        public string DisplayName
        {
            get
            {
                return this._displayName;
            }
            set
            {
                SetField(ref this._displayName, value, "DisplayName");
                this._displayName = value;
            }
        }

        private string _status;
        public string Status
        {
            get
            {
                return this._status;
            }
            set
            {
                SetField(ref this._status, value, "Status");
                this._status = value;
            }
        }

        private string _description;
        public string Description
        {
            get
            {
                return this._description;
            }
            set
            {
                SetField(ref this._description, value, "Description");
                this._description = value;
            }
        }

        //curing time (sec)
        private double _CT1;
        public double CT1
        {
            get
            {
                return this._CT1;
            }
            set
            {
                SetField(ref this._CT1, value, "CT1");
                this._CT1 = value;
            }
        }

        private double _CT2;
        public double CT2
        {
            get
            {
                return this._CT2;
            }
            set
            {
                SetField(ref this._CT2, value, "CT2");
                this._CT2 = value;
            }
        }

        //retraction height (mm)
        private double _RH1;
        public double RH1
        {
            get
            {
                return this._RH1;
            }
            set
            {
                SetField(ref this._RH1, value, "RH1");
                this._RH1 = value;
            }
        }

        private double _RH2;
        public double RH2
        {
            get
            {
                return this._RH2;
            }
            set
            {
                SetField(ref this._RH2, value, "RH2");
                this._RH2 = value;
            }

        }

        //retraction speed up (mm/sec)
        private double _RSU1;
        public double RSU1
        {
            get
            {
                return this._RSU1;
            }
            set
            {
                SetField(ref this._RSU1, value, "RSU1");
                this._RSU1 = value;
            }
        }

        private double _RSU2;
        public double RSU2
        {
            get
            {
                return this._RSU2;
            }
            set
            {
                SetField(ref this._RSU2, value, "RSU2");
                this._RSU2 = value;
            }
        }

        //time at top (after layer) (sec)
        private double _TAT1;
        public double TAT1
        {
            get
            {
                return this._TAT1;
            }
            set
            {
                SetField(ref this._TAT1, value, "TAT1");
                this._TAT1 = value;
            }
        }

        private double _TAT2;
        public double TAT2
        {
            get
            {
                return this._TAT2;
            }
            set
            {
                SetField(ref this._TAT2, value, "TAT2");
                this._TAT2 = value;
            }
        }

        //rehab time (before layer) (sec)
        private double _RT1;
        public double RT1
        {
            get
            {
                return this._RT1;
            }
            set
            {
                SetField(ref this._RT1, value, "RT1");
                this._RT1 = value;
            }

        }

        private double _RT2;
        public double RT2
        {
            get
            {
                return this._RT2;
            }
            set
            {
                SetField(ref this._RT2, value, "RT2");
                this._RT2 = value;
            }
        }

        //retraction speed down (before layer) (mm/sec)
        private double _RSD1;
        public double RSD1
        {
            get
            {
                return this._RSD1;
            }
            set
            {
                SetField(ref this._RSD1, value, "RSD1");
                this._RSD1 = value;
            }
        }

        private double _RSD2;
        public double RSD2
        {
            get
            {
                return this._RSD2;
            }
            set
            {
                SetField(ref this._RSD2, value, "RSD2");
                this._RSD2 = value;
            }

        }

        private double _RTAC1;
        public double RTAC1
        {
            get
            {
                return this._RTAC1;
            }
            set
            {
                SetField(ref this._RTAC1, value, "RTAC1");
                this._RTAC1 = value;
            }

        }

        private double _RTAC2;
        public double RTAC2
        {
            get
            {
                return this._RTAC2;
            }
            set
            {
                SetField(ref this._RTAC2, value, "RTAC2");
                this._RTAC2 = value;
            }
        }

        //first layer thickness (mm)
        private double _LT1;
        public double LT1
        {
            get
            {
                return this._LT1;
            }
            set
            {
                SetField(ref this._LT1, value, "LT1");
                this._LT1 = value;
            }
        }


        private double _LT2;
        public double LT2
        {
            get
            {
                return this._LT2;
            }
            set
            {
                SetField(ref this._LT2, value, "LT2");
                this._LT2 = value;
            }
        }

        private Guid _id;
        public Guid Id
        {
            get
            {
                return this._id;
            }
            set
            {
                SetField(ref this._id, value, "Id");
                this._id = value;
            }
        }


        private double _shrinkFactor = 1;
        public double ShrinkFactor
        {
            get
            {
                return this._shrinkFactor;
            }
            set
            {
                this._shrinkFactor = value;
            }
        }

        public int ChangeId { get; set; }

        public MaterialTransitionLayers TransitionLayers { get; set; }

        public bool XYSmoothingEnabled { get; set; }
        public List<XYSmoothingSetting> XYSmoothingSettings { get; set; }


        private Material _preserved;

        public void CreatePreserved()
        {
            this._preserved = Utils.Toolbox.DeepClone(this);
        }

        private double _bleedingOffset;
        public double BleedingOffset
        {
            get
            {
                return this._bleedingOffset;
            }
            set
            {
                SetField(ref this._bleedingOffset, value, "BleedingOffset");
                this._bleedingOffset = value;
            }
        }

        private double _bleedingXYOffset_Inside;
        public double BleedingXYOffset_Inside
        {
            get
            {
                return this._bleedingXYOffset_Inside;
            }
            set
            {
                this._bleedingXYOffset_Inside = value;
            }
        }


        private double _bleedingXYOffset_Outside;
        public double BleedingXYOffset_Outside
        {
            get
            {
                return this._bleedingXYOffset_Outside;
            }
            set
            {
                this._bleedingXYOffset_Outside = value;
            }
        }

        public MaterialTechnicalSpecificationsAndResinVisibleProperties TechnicalSpecificationsAndResinVisibleProperties { get; set; }
        public MaterialTechnicalSpecificationsAndResinProperties TechnicalSpecificationsAndResinProperties { get; set; }

        private double _smoothingOffset;
        public double SmoothingOffset
        {
            get
            {
                return this._smoothingOffset;
            }
            set
            {
                SetField(ref this._smoothingOffset, value, "SmoothingOffset");
                this._smoothingOffset = value;
            }
        }

        private Color _modelColor;
        [XmlIgnore]
        public Color ModelColor
        {
            get
            {
                return this._modelColor;
            }
            set
            {
                SetField(ref this._modelColor, value, "ModelColor");
                this._modelColor = value;
            }
        }

        [XmlElement("ModelColor")]
        public int BackColorAsArgb
        {
            get { return this._modelColor.ToArgb(); }
            set { this._modelColor = System.Drawing.Color.FromArgb(value); }
        }

        private float _lightIntensityPercentage1 = 100; //beware 100% is limited to 80%
        [XmlElement("LightIntensityPercentage1")]
        public float LightIntensityPercentage1
        {
            get { return this._lightIntensityPercentage1; }
            set
            {
                SetField(ref this._lightIntensityPercentage1, value, "LightIntensityPercentage1");
                this._lightIntensityPercentage1 = value > 100 ? 100 : value;
            }
        }

        //default

        private float _lightIntensityPercentage2 = 100; //beware 100% is limited to 80%
        [XmlElement("LightIntensityPercentage2")]
        public float LightIntensityPercentage2
        {
            get { return this._lightIntensityPercentage2; }
            set
            {
                SetField(ref this._lightIntensityPercentage2, value, "LightIntensityPercentage2");
                this._lightIntensityPercentage2 = value > 100 ? 100 : value;
            }
        }

        public void RevertChanges()
        {
            this._articleHTTP = this._preserved.ArticleHTTP;
            this._articleNumber = this._preserved.ArticleNumber;
            this._batchNumber = this._preserved.BatchNumber;
            this._color = this._preserved.Color;
            this._CT1 = this._preserved.CT1;
            this._CT2 = this._preserved.CT2;
            this._initialLayers = this._preserved.InitialLayers;
            this._LT1 = this._preserved.LT1;
            this._LT2 = this._preserved.LT2;
            this._bleedingOffset = this._preserved.BleedingOffset;
            this._preparationLayersCount = this._preserved.PreparationLayersCount;
            this._price = this._preserved.Price;
            this._RH1 = this._preserved.RH1;
            this._RH2 = this._preserved.RH2;
            this._RSD1 = this._preserved.RSD1;
            this._RSD2 = this._preserved.RSD2;
            this._RSU1 = this._preserved.RSU1;
            this._RSU2 = this._preserved.RSU2;
            this._RT1 = this._preserved.RT1;
            this._RT2 = this._preserved.RT2;
            this._RTAC1 = this._preserved.RTAC1;
            this._RTAC2 = this._preserved.RTAC2;
            this._status = this._preserved.Status;
            this._TAT1 = this._preserved.TAT1;
            this._TAT2 = this._preserved.TAT2;
            this._uniqueNumber = this._preserved.UniqueNumber;
            this._smoothingOffset = this._preserved.SmoothingOffset;
            this._shrinkFactor = this._preserved.ShrinkFactor;
            this._modelColor = this._preserved.ModelColor;
        }

        [XmlIgnore]
        public bool IsDirty { get; private set; }
        public event PropertyChangedEventHandler PropertyChanged;
        protected void SetField<T>(ref T field, T value, string propertyName)
        {
            if (!EqualityComparer<T>.Default.Equals(field, value))
            {
                field = value;
                IsDirty = true;
                OnPropertyChanged(propertyName);
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public Material()
        {
            this.ModelColor = Properties.Settings.Default.DefaultModelColor;
            this.TransitionLayers = new MaterialTransitionLayers();
            this.XYSmoothingSettings = new List<XYSmoothingSetting>();
            this.AdditiveManufacturingDeviceModules = new List<AdditiveManufacturingDeviceModule>();
        }

        #region MODULES
        #region WIPER
        public List<AdditiveManufacturingDeviceModule> AdditiveManufacturingDeviceModules { get; set; }

        #endregion
        #endregion


    }

    [Serializable]
    public class MaterialsBySupplier
    {
        public BindingList<Material> Materials { get; set; }
        public BindingList<MaterialsBySupplier> GetMaterialsByResolution(AtumPrinter selectedPrinter)
        {
            var materialsByResolution = new BindingList<MaterialsBySupplier>();
            foreach (var material in this.Materials)
            {
                if (material.XYResolution == selectedPrinter.PrinterXYResolutionAsInt && selectedPrinter is AtumDLPStation5 && !string.IsNullOrEmpty(material.PrinterHardwareType))
                {
                    var materialBySupplier = new MaterialsBySupplier();
                    materialBySupplier.Supplier = this.Supplier;
                    materialBySupplier.Materials.Add(material);
                    materialsByResolution.Add(materialBySupplier);
                }
                else if (material.XYResolution == selectedPrinter.PrinterXYResolutionAsInt && ((selectedPrinter is AtumV15Printer) || (selectedPrinter is AtumV20Printer)) && string.IsNullOrEmpty(material.PrinterHardwareType))
                {
                    var materialBySupplier = new MaterialsBySupplier();
                    materialBySupplier.Supplier = this.Supplier;

                    if (material.SupportProfiles == null)
                    {
                        material.SupportProfiles = new List<SupportProfile>();
                    }

                    if (material.SupportProfiles.Count == 0)
                    {
                        material.SupportProfiles.Add(SupportProfile.CreateDefault());
                    }
                    materialBySupplier.Materials.Add(material);
                    materialsByResolution.Add(materialBySupplier);
                }
                else if (material.XYResolution == selectedPrinter.PrinterXYResolutionAsInt && selectedPrinter is LoctiteV10 && !string.IsNullOrEmpty(material.PrinterHardwareType))
                {
                    var materialBySupplier = new MaterialsBySupplier();
                    materialBySupplier.Supplier = this.Supplier;

                    if (material.SupportProfiles == null)
                    {
                        material.SupportProfiles = new List<SupportProfile>();
                    }

                    if (material.SupportProfiles.Count == 0)
                    {
                        material.SupportProfiles.Add(SupportProfile.CreateDefault());
                    }
                    materialBySupplier.Materials.Add(material);
                    materialsByResolution.Add(materialBySupplier);
                }
            }

            var orderedBindingList = new BindingList<MaterialsBySupplier>();
            if (materialsByResolution.Count > 0)
            {
                var orderedList = materialsByResolution.OrderBy(s => s.Materials[0].Name).ToList();
                foreach (var orderedMaterial in orderedList)
                {
                    orderedBindingList.Add(orderedMaterial);
                }
            }



            return orderedBindingList;
        }

        public string Supplier { get; set; }

        [XmlIgnore]
        public string FilePath { get; set; }

        public MaterialsBySupplier()
        {
            this.Materials = new BindingList<Material>();
            this.Supplier = string.Empty;
        }

        public override string ToString()
        {
            return this.Supplier;
        }

        public Material FindMaterialById(Guid materialId)
        {
            Material result = null;

            foreach (var material in this.Materials)
            {
                if (material.Id == materialId)
                {
                    return material;
                }
            }
            return result;
        }

        public void SetMaterialById(Guid materialId, Material updatedMaterial)
        {
            for (var materialIndex = 0; materialIndex < this.Materials.Count; materialIndex++)
            {
                if (this.Materials[materialIndex].Id == materialId)
                {
                    this.Materials[materialIndex] = updatedMaterial;
                    break;
                }
            }
        }

        public void SaveToFile()
        {
            if (!string.IsNullOrEmpty(this.FilePath))
            {
                this.FilePath = this.FilePath.Replace("/", "\\");
                var supplierNameFromFilePath = new FileInfo(this.FilePath).Name;
                if (supplierNameFromFilePath.ToLower() != this.Supplier.ToLower())
                {
                    if (File.Exists(this.FilePath))
                    {
                        File.Delete(this.FilePath);
                    }
                }
            }

            this.FilePath = Path.Combine(ApplicationSettings.Settings.RoamingMaterialsPath, this.Supplier + ".xml");
            this.FilePath = this.FilePath.Replace("/", "\\");
            var serializer = new System.Xml.Serialization.XmlSerializer(typeof(MaterialsBySupplier));
            using (var streamWriter = new StreamWriter(this.FilePath, false))
            {
                serializer.Serialize(streamWriter, this);

                streamWriter.Close();
            }

        }
    }

    public class MaterialsCatalog : BindingList<MaterialsBySupplier>
    {
        public Material FindMaterialById(Guid materialId)
        {
            Material result = null;

            foreach (var supplier in this)
            {
                foreach (var material in supplier.Materials)
                {
                    if (material.Id == materialId)
                    {
                        return material;
                    }
                }
            }
            return result;
        }
    }
}

//testcode
//var materials = new Core.Materials.MaterialsBySupplier();
//            materials.Supplier = "Sup1" ;
//            materials.Materials.Add(new Core.Materials.Material(){
//                UniqueNumber = "12345",
//                Color = "Red",
//                ArticleNumber = "A12345",
//                ArticleHTTP = "http://article",
//                BatchNumber = "B12345",
//                Status = "S12345",

//                CT1 = 10,
//                CT2 = 5,
//                CT3 = 3.5,
//                CT4 = 3.5,
//                CT5 = 3.5,

//                RH1 = 10,
//                RH2 = 5,
//                RH3 = 5,
//                RH4 = 5,
//                RH5 = 2,

//                RSU1 = 0.5,
//                RSU2 = 3,
//                RSU3 = 3,
//                RSU4 = 3,
//                RSU5 = 3,

//                TAT1 = 5,
//                TAT2 = 2,
//                TAT3 = 2,
//                TAT4 = 10,
//                TAT5 = 2,

//                RT1 =  0,
//                RT2 =  0.5,
//                RT3 = 1,
//                RT4 = 1,
//                RT5 = 1,

//                RSD1 = 5,
//                RSD2 = 5,
//                RSD3 = 5,
//                RSD4 = 5,
//                RSD5 = 5,

//                FLT = 1
//            }
//            );

//
//    var serializer = new System.Xml.Serialization.XmlSerializer(typeof(Core.Materials.MaterialsBySupplier));
//    using(var exportMaterial = new System.IO.StreamWriter(Application.StartupPath + "\\Materials\\" + materials.Supplier + ".xml", false)){
//    serializer.Serialize(exportMaterial, materials);
//}

//    var materialCatalog = new Core.Materials.MaterialsCatalog();
//    foreach(var supplierXML in System.IO.Directory.GetFiles(Core.ApplicationSettings.Settings.MaterialsPath)){
//        var supplierMaterials = (Core.Materials.MaterialsBySupplier)serializer.Deserialize(new System.IO.StreamReader(supplierXML));
//        materialCatalog.Add(supplierMaterials.Supplier, supplierMaterials);
//    }
