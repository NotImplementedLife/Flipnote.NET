using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class ChangeComponentTransform : IUndoableAction
    {
        private FramesManager FramesManager;
        private Frame Frame;
        private FlipnoteCanvasComponent Component;
        private CanvasTransform OldTransform;
        private CanvasTransform NewTransform;

        public ChangeComponentTransform(FramesManager framesManager, Frame frame, FlipnoteCanvasComponent component, CanvasTransform oldTransform, CanvasTransform newTransform)
        {
            FramesManager = framesManager;
            Frame = frame;
            Component = component;
            OldTransform = oldTransform;
            NewTransform = newTransform;
        }

        public void Do()
        {
            Component.Transform = NewTransform;
            if (FramesManager.GetCurrentFrame() == Frame)
                Component.Invalidate();
            FramesManager.RequestThumbnailRedraw(Frame);            
        }

        public void Undo()
        {
            Component.Transform = OldTransform;
            if(FramesManager.GetCurrentFrame()==Frame)
                Component.Invalidate();
            FramesManager.RequestThumbnailRedraw(Frame);
        }
    }
}
