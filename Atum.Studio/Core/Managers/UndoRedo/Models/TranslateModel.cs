using Atum.Studio.Core.Structs;
using OpenTK;
using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class TranslateModel : BaseModel
    {
        public Vector3Class TotalMoveTranslation { get; set; }
    }
}
