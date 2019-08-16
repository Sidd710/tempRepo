using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Shapes
{
    internal class ModelBrowserInfo
    {
        internal string FileName { get; set; }
        internal int ModelIndex { get; set; }
        internal List<string> SupportStructure {get;set; }
        internal List<string> Parts { get; set; }

        internal ModelBrowserInfo()
        {
            this.SupportStructure = new List<string>();
            this.Parts = new List<string>();
        }
    }
}
