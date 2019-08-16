using System;
using System.Collections.Generic;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public class UndoTransaction : IDisposable, IUndoRedoAction
    {
        private string _name;
        private List<IUndoRedoAction> _undoRedoActions = new List<IUndoRedoAction>();
        public UndoTransaction(string name = "")
        {
            _name = name;
            UndoRedoManager.GetInstance.StartTransaction(this);
        }
        public string Name
        {
            get { return _name; }
        }
        public void Dispose()
        {
            UndoRedoManager.GetInstance.EndTransaction(this);
        }
        public void AddUndoRedoOperation(IUndoRedoAction operation)
        {
            _undoRedoActions.Insert(0, operation);
        }
        public int OperationsCount
        {
            get { return _undoRedoActions.Count; }
        }

        #region Implementation of IUndoRedoAction
        public void Execute()
        {
            _undoRedoActions.ForEach((a) => a.Execute());
        }
        #endregion
    }
}
