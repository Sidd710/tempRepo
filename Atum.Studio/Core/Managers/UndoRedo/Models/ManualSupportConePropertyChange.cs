using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class ManualSupportConePropertyChange : BaseModel
    {
        internal TriangleIntersection SelectedTriangle { get; set; }
        internal TriangleIntersection PreviousSelectedTriangle { get; set; }
        public Point MousePosition { get; set; }
        public SupportCone SupportCone { get; set; }
        public string PropertyName { get; set; }
        public float PropertyValueNew { get; set; }
        public float PropertyValueOld { get; set; }
    }

    public class RemoveManualSupportConePropertyChange
    {
        public ManualSupportCone SupportConeModel { get; set; }
        public string PropertyName { get; set; }
        public SupportCone SupportCone { get; set; }
        public float PropertyValueNew { get; set; }
        public float PropertyValueOld { get; set; }
    }
}
