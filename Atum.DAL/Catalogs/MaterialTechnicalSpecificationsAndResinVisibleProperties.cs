using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Catalogs
{
  
    [Serializable]
    public class MaterialTechnicalSpecificationsAndResinVisibleProperty
    {
        public TypeOfTechnicalSpecificationOrResinProperty TechnicalSpecificationOrResinProperty { get; set; }

        public MaterialTechnicalSpecificationsAndResinVisibleProperty()
        {

        }

        public MaterialTechnicalSpecificationsAndResinVisibleProperty(TypeOfTechnicalSpecificationOrResinProperty techSpecType)
        {
            this.TechnicalSpecificationOrResinProperty = techSpecType;
        }
    }

    [Serializable]
    public class MaterialTechnicalSpecificationsAndResinVisibleProperties: List<MaterialTechnicalSpecificationsAndResinVisibleProperty>
    {

    }
}
