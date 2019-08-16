using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Atum.Studio.Core.Materials
{
    public class MaterialCatalogOnline : SortedDictionary<int, DAL.Materials.MaterialsCatalog>
    {

    }

    [Serializable, XmlRoot("MaterialCatalogDownloadAllFiles")]
    public class MaterialCatalogDownloadAllFiles
    {
        public List<MaterialCatalogDownloadFile> AvailableMaterialURLs { get; set; }
        public MaterialCatalogDownloadAllFiles()
        {
            this.AvailableMaterialURLs = new List<MaterialCatalogDownloadFile>();
        }

    }

    [Serializable]
    public class MaterialCatalogDownloadFile
    {
        public float XYMicron { get; set; }
        public string URL { get; set; }
    }
}
