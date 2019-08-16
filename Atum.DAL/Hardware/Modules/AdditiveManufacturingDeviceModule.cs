using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Atum.DAL.Hardware.Modules
{
    [XmlInclude(typeof(WiperModule))]

    [Serializable]
    public abstract class AdditiveManufacturingDeviceModule
    {
        public Guid ModuleId { get; set; }
        public List<AdditiveManufacturingDeviceModuleSetting> AdditiveManufacturingDeviceModuleSettings { get; set; }

        public AdditiveManufacturingDeviceModule()
        {

        }

        public AdditiveManufacturingDeviceModule(Guid moduleId, List<AdditiveManufacturingDeviceModuleSetting> additiveManufacturingDeviceModuleSettings)
        {
            this.ModuleId = moduleId;
            this.AdditiveManufacturingDeviceModuleSettings = additiveManufacturingDeviceModuleSettings;
        }

        public string ToSerialString()
        {
            var stringBuilder = new StringBuilder();
            if (this.AdditiveManufacturingDeviceModuleSettings != null)
            {
                foreach (var setting in this.AdditiveManufacturingDeviceModuleSettings)
                {
                        stringBuilder.Append(setting + ";");
                }

                if (!stringBuilder.ToString().EndsWith(";"))
                {
                    stringBuilder.Append(";");
                }
            }

            return stringBuilder.ToString();
        }
    }
}
