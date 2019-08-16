using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Hardware.Modules
{
    [Serializable]
    public class WiperModule : AdditiveManufacturingDeviceModule
    {
        public WiperModule()
        {
            this.ModuleId = Guid.Parse("{DC435959-BBBA-4559-86DE-A79A9CA31E86}");
            this.AdditiveManufacturingDeviceModuleSettings = new List<AdditiveManufacturingDeviceModuleSetting>();
        }
    }
}
