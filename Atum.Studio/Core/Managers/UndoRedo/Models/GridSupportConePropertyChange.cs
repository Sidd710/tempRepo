using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class GridSupportConePropertyChange : BaseModel
    {
        public bool IsHorizontalSurface { get; set; }
        public bool IsFlatSurface { get; set; }

        internal int SelectedSurfaceIndex { get; set; }

        public string PropertyName { get; set; }
        public float PropertyValueNew { get; set; }
        public float PropertyValueOld { get; set; }
        
    }
    public class RemoveGridSupportConePropertyChange
    {
        public GridSupportCone SupportConeModel { get; set; }
        public string PropertyName { get; set; }
        public SupportCone SupportCone { get; set; }
        public float PropertyValueNew { get; set; }
        public float PropertyValueOld { get; set; }
    }


}
