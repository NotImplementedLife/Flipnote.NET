namespace FlipnoteDotNet.App.Actions
{
    public class UndoStack
    {
        private readonly Stack<IUndoableAction> Done = new Stack<IUndoableAction>();
        private readonly Stack<IUndoableAction> Undone = new Stack<IUndoableAction>();

        public void Do(IUndoableAction action)
        {            
            action.Do();
            Done.Push(action);
            Undone.Clear();
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Undo()
        {
            if(Done.Count == 0) { return; }
            var action = Done.Pop();
            action.Undo();
            Undone.Push(action);
            StateChanged?.Invoke(this, EventArgs.Empty);
        }

        public void Redo()
        {
            if(Undone.Count==0) { return; }
            var action = Undone.Pop();
            action.Do();
            Done.Push(action);
            StateChanged?.Invoke(this, EventArgs.Empty);
        }
        public bool CanUndo => Done.Count != 0;
        public bool CanRedo => Undone.Count != 0;
        public event EventHandler StateChanged;
    }
}
