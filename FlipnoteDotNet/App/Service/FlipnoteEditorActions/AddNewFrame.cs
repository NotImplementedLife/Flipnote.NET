using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    public class AddNewFrame : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly Frame Frame;

        public AddNewFrame(FramesManager framesManager, Frame frame)
        {
            FramesManager = framesManager;
            Frame = frame;
        }

        public void Do()
        {
            FramesManager.AddFrame(Frame);            
        }

        public void Undo()
        {
            FramesManager.RemoveFrame(FramesManager.Frames.Count - 1);
        }
    }
}
