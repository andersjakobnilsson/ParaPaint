using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;

namespace ParaPaint
{
    public class ActionVector : ObservableCollection<IAction> {}

    public class ActionHandler
    {
        #region Members
        
        private ActionVector undoStack = new ActionVector();

        #endregion Members

        #region Accessors

        public ActionVector UndoStack 
        {
            get { return undoStack; }
        }

        #endregion

        public void Perform(IAction action)
        {
            undoStack.Add(action);
            action.Perform();
        }

        public void Undo()
        {
            IAction latest = undoStack.ElementAt(undoStack.Count-1);
            undoStack.RemoveAt(undoStack.Count - 1);
            latest.Undo();
        }

        public void Clear()
        {
            undoStack.Clear();
        }

        public bool Empty()
        {
            return undoStack.Count == 0;
        }
    }
}
