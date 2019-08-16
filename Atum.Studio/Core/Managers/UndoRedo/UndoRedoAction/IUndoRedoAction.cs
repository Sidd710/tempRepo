using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
   public interface IUndoRedoAction
    {
        void Execute();
    }
}
