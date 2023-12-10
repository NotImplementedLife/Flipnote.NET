namespace FlipnoteDotNet.Commons.Actions
{
    public interface IUndoableAction
    {
        void Do();
        void Undo();
    }
}
