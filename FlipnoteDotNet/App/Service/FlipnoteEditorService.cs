using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Service.FlipnoteEditorActions;
using FlipnoteDotNet.Canvas;
using static FlipnoteDotNet.PropertyEditor.PropertiesCollection;

namespace FlipnoteDotNet.App.Service
{
    public class FlipnoteEditorService
    {
        private readonly FramesManager FramesManager;
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

        public void RemoveCurrentFrame()
        {
            UndoStack.Do(new RemoveCurrentFrame(
                FramesManager,
                FramesManager.GetCurrentFrameIndex(),
                FramesManager.GetCurrentFrame()
                ));
        }

        public void ClearCurrentFrame()
        {
            var frame = FramesManager.GetCurrentFrame();
            UndoStack.Do(new ClearFrame(
                FramesManager,
                frame,
                frame.Components.ToList()
                ));
        }

        public void AddAssetToCurrentFrame(Asset asset)
        {
            UndoStack.Do(new AddComponentToFrame(FramesManager, FramesManager.GetCurrentFrame(), asset.CreateCanvasComponent()));
        }

        public void RemoveComponentFromFrame(FlipnoteCanvasComponent component)
        {
            var frame = FramesManager.GetCurrentFrame();
            var index = frame.Components.IndexOf(component);
            var action = new RemoveComponentFromFrame(FramesManager, frame, component, index);
            UndoStack.Do(action);
        }

        public void ChangeComponentTransformOnCurrentFrame(FlipnoteCanvasComponent component, 
            CanvasTransform oldTransform, CanvasTransform newTransform)
        {
            UndoStack.Do(new ChangeComponentTransform(FramesManager, FramesManager.GetCurrentFrame(),
                component, oldTransform, newTransform));
        }

        public void ChangeProperty(object target, object oldValue, object newValue, PropertyData propertyData)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            IUndoableAction action = null;

            if (target is FlipnoteCanvasComponent component)
            {
                if (propertyData.Name == "Transform")
                {
                    action = new ChangeComponentTransform(FramesManager, FramesManager.GetCurrentFrame(),
                        component, (CanvasTransform)oldValue, (CanvasTransform)newValue);
                    goto perform;
                }
            }
            
            action = new ReflectionPropertySet(target, oldValue, newValue, propertyData.Setter);
        perform:
            UndoStack.Do(action);
        }

        public int FramesCount => FramesManager.Frames.Count;

        public FrameProxy GetCurrentFrameProxy() => new FrameProxy(FramesManager, FramesManager.GetCurrentFrame());
    }
}
