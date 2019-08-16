using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class ManualSupportCone : BaseModel
    {
        internal TriangleIntersection SelectedTriangle { get; set; }
        internal TriangleIntersection PreviousSelectedTriangle { get; set; }
        public Point MousePosition { get; set; }
    }

    public class AddManualSupportCone: BaseModel
    {
        public SupportCone SupportCone { get; set; }
        public int SupportConeIndex { get; set; }
    }

    public class RemoveManualSupportCone
    {
        public ManualSupportCone SupportConeModel { get; set; }
        public SupportCone SupportCone { get; set; }
    }


}
