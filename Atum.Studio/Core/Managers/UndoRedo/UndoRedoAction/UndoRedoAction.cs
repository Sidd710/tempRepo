using System;
using System.Collections.Generic;
using System.Text;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public delegate void UndoRedoOperation<T>(T actionData);
    public class UndoRedoAction<T> : IUndoRedoAction
    {
        private UndoRedoOperation<T> _operation;
        private T _actionData;
        
        public UndoRedoAction(UndoRedoOperation<T> operation, T actionData)
        {
            _operation = operation;
            _actionData = actionData;
        }
        public void Execute()
        {
            _operation(_actionData);
        }
    }
}
