using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class BaseModel
    {
        public int ModelIndex { get; set; }
        public bool IsUndo { get; set; }
    }
}
