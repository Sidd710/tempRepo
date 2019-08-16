using System;
using System.Collections.Generic;
using System.Text;
using OpenTK.Graphics;

namespace Atum.Studio.Core.Platform
{
    [Serializable]
    public class DisplaySettings
    {
        public int BPP { get; set; }
        public int Depth { get; set; }
        public int Stencil { get; set; }
        public int Sample { get; set; }

        public DisplaySettings()
        {
        }
    }
}
