using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Events
{
    internal class RotationEventArgs: EventArgs
    {
        internal enum TypeAxis
        {
            X = 0,
            Y = 1,
            Z= 2,
            None = 10,
        }
        internal TypeAxis Axis { get; set; }
        internal float RotationAngle { get; set; }
    }
}
