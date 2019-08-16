using Atum.Studio.Core.Models;
using Atum.Studio.Core.Structs;
using OpenTK;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class TranslateManualSupportCone : BaseModel
    {
        public Vector3Class MoveTranslation { get; set; }
        public int SupportConeIndex { get; set; }
    }

    public class RemoveTranslateManualSupportCone : BaseModel
    {
        public Vector3Class TotalMoveTranslation { get; set; }
        public int SupportConeIndex { get; set; }
    }
}
