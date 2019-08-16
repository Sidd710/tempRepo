using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Platform
{
    [Serializable]
    public class PerformanceSettings
    {
        public bool PrintJobGenerationMaxMemoryLimitEnabled { get; set; }
        public int PrintJobGenerationMaxMemory { get; set; }
        public bool PrintJobGenerationMultiThreading { get; set; }
    }
}
