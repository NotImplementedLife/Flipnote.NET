using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service.FlipnoteEditorActions;
using FlipnoteDotNet.Canvas;

namespace FlipnoteDotNet.App.Service
{
    public class FlipnoteEditorService
    {        
        private FramesManager FramesManager;
        private readonly UndoStack UndoStack;

        public FlipnoteEditorService(FramesManager framesManager, UndoStack undoStack)
        {            
            FramesManager = framesManager;
            UndoStack = undoStack;
        }

        public void SetCurrentFrame(int index)
        {
            UndoStack.Do(new SetCurrentFrame(FramesManager, FramesManager.GetCurrentFrame(), FramesManager.GetFrame(index)));
        }

        public void AddNewFrame()
        {
            UndoStack.Do(new AddNewFrame(FramesManager, FramesManager.CreateNewFrame()));
        }

        public void InsertNewFrameAfterCurrent()
        {
            UndoStack.Do(new InsertNewFrameAfterCurrent(
                FramesManager,
                FramesManager.GetCurrentFrameIndex() + 1,
                FramesManager.CreateNewFrame()));
        }

        public void DuplicateCurrentFrame()
        {
            UndoStack.Do(new InsertNewFrameAfterCurrent(
                FramesManager,
                FramesManager.GetCurrentFrameIndex() + 1,
                FramesManager.DuplicateCurrentFrame()));
        }

        public void AddAssetToCurrentFrame(Asset asset)
        {
            UndoStack.Do(new AddComponentToFrame(FramesManager, FramesManager.GetCurrentFrame(), asset.CreateCanvasComponent()));
        }

        public void ChangeComponentTransformOnCurrentFrame(FlipnoteCanvasComponent component, 
            CanvasTransform oldTransform, CanvasTransform newTransform)
        {
            UndoStack.Do(new ChangeComponentTransform(FramesManager, FramesManager.GetCurrentFrame(),
                component, oldTransform, newTransform));
        }

    }
}
