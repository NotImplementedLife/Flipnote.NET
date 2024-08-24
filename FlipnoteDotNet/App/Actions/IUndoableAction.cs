namespace FlipnoteDotNet.App.Actions
{
    public interface IUndoableAction
    {
        void Do();
        void Undo();
    }
}
