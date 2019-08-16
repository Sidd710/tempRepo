using Atum.Studio.Core.Models;
using Atum.Studio.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class GridSupportCone : BaseModel
    {
        public bool IsHorizontalSurface { get; set; }
        public bool IsFlatSurface { get; set; }

        internal int SelectedSurfaceIndex { get; set; }
    }

    public class RemoveGridSupportCone
    {
        public GridSupportCone GridSupportCone { get; set; }
        internal int SelectedSurfaceIndex { get; set; }
    }

    public class AddGridSupport: BaseModel
    {
        public bool IsHorizontalSurface { get; set; }
        public bool IsFlatSurface { get; set; }

        public Object Surface { get; set; }
        internal int SelectedSurfaceIndex { get; set; }
    }
}
