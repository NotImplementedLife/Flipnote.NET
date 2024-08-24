using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class SetCurrentFrame : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly Frame OldFrame;
        private readonly Frame NewFrame;

        public SetCurrentFrame(FramesManager framesManager, Frame oldFrame, Frame newFrame)
        {
            FramesManager = framesManager;
            OldFrame = oldFrame;
            NewFrame = newFrame;
        }

        public void Do()
        {
            FramesManager.SetCurrentFrame(NewFrame);
        }

        public void Undo()
        {
            FramesManager.SetCurrentFrame(OldFrame);
        }
    }
}
