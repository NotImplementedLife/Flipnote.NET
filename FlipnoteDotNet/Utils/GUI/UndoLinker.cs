using FlipnoteDotNet.App.Actions;

namespace FlipnoteDotNet.Utils.GUI
{
    public static class UndoLinker
    {
        public static void Connect(UndoStack undoStack, ToolStripItem undoControl, ToolStripItem redoControl)
        {
            undoStack.StateChanged += (o, e) =>
            {
                if (undoControl != null) undoControl.Enabled = undoStack.CanUndo;
                if (redoControl != null) redoControl.Enabled = undoStack.CanRedo;
            };
            if (undoControl != null)
            {
                undoControl.Enabled = undoStack.CanUndo;
                undoControl.Click += (o, e) => undoStack.Undo();
            }
            if (redoControl != null)
            {
                redoControl.Enabled = undoStack.CanRedo;
                redoControl.Click += (o, e) => undoStack.Redo();
            }
        }
    }
}
