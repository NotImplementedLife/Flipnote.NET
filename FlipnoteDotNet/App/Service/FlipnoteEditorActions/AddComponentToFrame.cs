using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class AddComponentToFrame : IUndoableAction
    {
        private FramesManager FramesManager;
        private Frame Frame;
        private FlipnoteCanvasComponent Component;

        public AddComponentToFrame(FramesManager framesManager, Frame frame, FlipnoteCanvasComponent component)
        {
            FramesManager = framesManager;
            Frame = frame;
            Component = component;
        }

        public void Do()
        {
            Frame.AddComponent(Component);
            FramesManager.RequestThumbnailRedraw(Frame);
        }

        public void Undo()
        {
            Frame.RemoveComponent(Component);
            FramesManager.RequestThumbnailRedraw(Frame);
        }
    }
}
