using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.DAL.Catalogs
{
    public class SupportCatalogItem
    {
        public bool IsDefault { get; set; }
        public float TopHeight { get; set; }
        public float TopRadius { get; set; }
        public float MiddleRadius { get; set; }
        public float BottomHeight { get; set; }
        public float BottomRadius { get; set; }
    }

    public class SupportCatalog : List<SupportCatalogItem>
    {
        public static SupportCatalog _supportCatalog;

        public static SupportCatalog Catalog
        {
            get
            {
                return _supportCatalog;
            }
            set
            {
                _supportCatalog = value;
            }
        }

        public static void SetAsDefault(SupportCatalogItem supportConeAsDefault)
        {
            //disable all defaults
            foreach (var supportCone in Catalog)
            {
                supportCone.IsDefault = false;
            }

            //set as default
            foreach (var supportCone in Catalog)
            {
                if (supportCone == supportConeAsDefault)
                {
                    supportCone.IsDefault = true;
                    break;
                }
            }
        }

    }
}
