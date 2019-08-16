using System.Collections.Generic;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public static class ListOperations
    {
        public static void Push(List<IUndoRedoAction> list, IUndoRedoAction action)
        {
            list.Insert(0, action);
        }
        public static IUndoRedoAction Pop(List<IUndoRedoAction> list)
        {
            IUndoRedoAction topAction = list[0];
            list.RemoveAt(0);
            return topAction;
        }
    }    
}
