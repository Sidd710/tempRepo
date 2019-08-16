using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Engines.MagsAI
{
    [Serializable]
    public class MAGSAIDebugComment
    {
        public byte[] ScreenshotAsByteArray { get; set; }
        public string Comment { get; set; }

    }
}
