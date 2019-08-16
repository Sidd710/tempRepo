using Atum.Studio.Core.ModelView;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atum.Studio.Core.Events
{
    class SelectionBoxSizeEventArgs: EventArgs
    {
        internal Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox SelectionBoxType;

        internal SelectionBoxSizeEventArgs(Models.MAGSAIMarkSelectionGizmo.TypeOfSelectionBox selectionBoxType)
        {
            this.SelectionBoxType = selectionBoxType;
        }
    }
}
