using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Events
{
    internal class SelectionEventArgs: EventArgs
    {
        internal int ModelIndex { get; set; }
        internal int SupportIndex { get; set; }
        internal Models.STLModel3D.TypeObject ObjectType { get; set; }
        
    }
}
