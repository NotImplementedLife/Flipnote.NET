using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class InsertNewFrameAfterCurrent : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly int InsertIndex;
        private readonly Frame Frame;

        public InsertNewFrameAfterCurrent(FramesManager framesManager, int insertIndex, Frame frame)
        {
            FramesManager = framesManager;
            InsertIndex = insertIndex;
            Frame = frame;
        }

        public void Do()
        {
            FramesManager.InsertFrame(InsertIndex, Frame);
            FramesManager.RequestThumbnailRedraw(Frame);
        }

        public void Undo()
        {
            FramesManager.RemoveFrame(InsertIndex);            
        }
    }
}
