using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class RemoveCurrentFrame : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly int Index;
        private readonly Frame Frame;

        public RemoveCurrentFrame(FramesManager framesManager, int index, Frame frame)
        {
            FramesManager = framesManager;
            Index = index;
            Frame = frame;
        }

        public void Do()
        {
            FramesManager.RemoveFrame(Index);
        }

        public void Undo()
        {
            FramesManager.InsertFrame(Index, Frame);
            FramesManager.RequestThumbnailRedraw(Frame);
        }
    }
}
