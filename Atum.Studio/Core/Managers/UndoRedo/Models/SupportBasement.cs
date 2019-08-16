using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class SupportBasement : BaseModel
    {
        public bool PreviousSupportBasementUseValue { get; set; }
        public bool UseSupportBasement { get; set; }
    }

    public class RemoveSupportBasement: BaseModel
    {
        public bool UseSupportBasement { get; set; }
    }
}
