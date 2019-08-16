using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.DAL.Hardware
{
    [Serializable]
    public class TrapeziumCorrection
    {
        public float TrapeziumCorrectionInputA { get; set; }
        public float TrapeziumCorrectionInputB { get; set; }
        public float TrapeziumCorrectionInputC { get; set; }
        public float TrapeziumCorrectionInputD { get; set; }
        public float TrapeziumCorrectionInputE { get; set; }
        public float TrapeziumCorrectionInputF { get; set; }

        public float TrapeziumCorrectionRawInputA { get; set; }
        public float TrapeziumCorrectionRawInputB { get; set; }
        public float TrapeziumCorrectionRawInputC { get; set; }
        public float TrapeziumCorrectionRawInputD { get; set; }
        public float TrapeziumCorrectionRawInputE { get; set; }
        public float TrapeziumCorrectionRawInputF { get; set; }

        public DateTime TrapeziumCorrectionSetDateTime { get; set; }
    }
}
