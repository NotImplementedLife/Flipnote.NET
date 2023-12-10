using System;

namespace FlipnoteDotNet.Commons.Actions
{
    public class UndoableActionManager<A> where A : IUndoableAction
    {
        private readonly PreserveStack<A> Actions = new PreserveStack<A>();

        public void DoAction(A action)
        {
            action.Do();
            Actions.Push(action);
            ActionsListChanged?.Invoke(this, EventArgs.Empty);
        }

        public bool UndoLastAction()
        {
            if (!CanUndo) return false;
            var action = Actions.Pop();
            ActionsListChanged?.Invoke(this, EventArgs.Empty);            
            action.Undo();
            return true;
        }

        public bool RedoLastAction()
        {
            if (!CanRedo) return false;
            var action = Actions.UnPop();
            ActionsListChanged?.Invoke(this, EventArgs.Empty);
            action.Do();
            return true;
        }

        public event EventHandler ActionsListChanged;
        
        public bool CanUndo => Actions.CanPop;
        public bool CanRedo => Actions.CanUnPop;

    }
}
