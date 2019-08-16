using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.DisplaySettings
{
    public class DpiScalingFactor
    {
        public int ScaleFactorMin { get; set; }
        public int ScaleFactorMax { get; set; }
        public float DpiFactor { get; set; }
        public float CorrectorFactor { get; set; }
    }
}
