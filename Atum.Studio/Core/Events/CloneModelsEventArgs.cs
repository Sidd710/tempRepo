using Atum.Studio.Core.Engines.PackingEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Events
{
    internal class CloneModelsEventArgs : EventArgs
    {
        internal PackModelsRequest PackModelsRequest { get; set; }
        internal PackingSolutions PackagingSolutions { get; set; }

        internal Dictionary<ModelFootprint, int> ModelFootprints { get; set; }
        //internal int Amount { get; set; }
        //internal ModelFootprint ModelFootprint { get; set; }

    }
    internal class FootPrintActionModel
    {
        public int Amount { get; set; }
        public ModelFootprint ModelFootprint { get; set; }
    }
}
