using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Atum.Studio.Core.Managers.UndoRedo
{
    public sealed class UndoRedoManager  //Using singleton pattern as of now
    {
        private static UndoRedoManager instance;
        private static object syncRoot = new Object();
        private UndoRedoManager() { }
        public static UndoRedoManager GetInstance
        {
            get
            {
                lock (syncRoot)
                {
                    if (instance == null)
                    {
                        instance = new UndoRedoManager();
                    }
                }
                return instance;
            }
        }

        private List<IUndoRedoAction> _undoActions = new List<IUndoRedoAction>();
        private List<IUndoRedoAction> _redoActions = new List<IUndoRedoAction>();
        private bool _undoInProgress = false;
        private bool _redoInProgress = false;
        private int _maximumActions = 25;
        public void PushReverseAction<T>(UndoRedoOperation<T> undoOperation, T undoData)
        {
            List<IUndoRedoAction> stack = null;
            
            if (_undoInProgress)
            {
                stack = _redoActions;
            }
            else
            {
                stack = _undoActions;
            }

            if ((!_undoInProgress) && (!_redoInProgress))
            {
                //_redoActions.Clear();
            }
            if (_curTran == null)
            {
                ListOperations.Push(stack, new UndoRedoAction<T>(undoOperation, undoData));
            }
            else
            {
                (stack[0] as UndoTransaction).AddUndoRedoOperation(new UndoRedoAction<T>(undoOperation, undoData));
            }

            if (stack.Count > _maximumActions)
            {
                stack.RemoveRange(_maximumActions - 1, stack.Count - _maximumActions);
            }
        }
        public void Undo()
        {
            try
            {
                _undoInProgress = true;

                Debug.WriteLine("UndoRedo Manager Available undo actions count: " + _undoActions.Count);

                if (_undoActions.Count == 0)
                {
                    return;
                }
                object oUndoAction = ListOperations.Pop(_undoActions);

                Type undoDataType = oUndoAction.GetType();

                if (typeof(UndoTransaction).Equals(undoDataType))
                {
                    StartTransaction(oUndoAction as UndoTransaction);
                }

                undoDataType.InvokeMember("Execute", BindingFlags.InvokeMethod, null, oUndoAction, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _undoInProgress = false;
                EndTransaction(_curTran);
            }
        }
        public void Redo()
        {
            try
            {
                _redoInProgress = true;
                if (_redoActions.Count == 0)
                {
                    return;
                }
                object oUndoData = ListOperations.Pop(_redoActions);

                Type undoDataType = oUndoData.GetType();

                if (typeof(UndoTransaction).Equals(undoDataType))
                {
                    StartTransaction(oUndoData as UndoTransaction);
                }

                undoDataType.InvokeMember("Execute", BindingFlags.InvokeMethod, null, oUndoData, null);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
            }
            finally
            {
                _redoInProgress = false;
                EndTransaction(_curTran);
            }
        }
        private UndoTransaction _curTran;
        public void StartTransaction(UndoTransaction tran)
        {
            if (_curTran == null)
            {
                _curTran = tran;
                ListOperations.Push(_undoActions, new UndoTransaction(tran.Name));
                ListOperations.Push(_redoActions, new UndoTransaction(tran.Name));
            }
        }
        public void EndTransaction(UndoTransaction tran)
        {
            if (_curTran == tran)
            {
                _curTran = null;
                if (_undoActions.Count > 0)
                {
                    UndoTransaction t = _undoActions[0] as UndoTransaction;
                    if (t != null && t.OperationsCount == 0)
                    {
                        ListOperations.Pop(_undoActions);
                    }
                }

                if (_redoActions.Count > 0)
                {
                    UndoTransaction t = _redoActions[0] as UndoTransaction;
                    if (t != null && t.OperationsCount == 0)
                    {
                        ListOperations.Pop(_redoActions);
                    }
                }
            }
        }
        public bool HasUndoActions
        {
            get { return _undoActions.Count != 0; }
        }
        public bool HasRedoActions
        {
            get { return _redoActions.Count != 0; }
        }
    }    
}
