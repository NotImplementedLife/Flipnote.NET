using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    public class ClearFrame : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly Frame Frame;
        private readonly List<CanvasComponent> Components;

        public ClearFrame(FramesManager framesManager, Frame frame, List<CanvasComponent> components)
        {
            FramesManager = framesManager;
            Frame = frame;
            Components = components;
        }

        public void Do()
        {
            Frame.ClearComponents();
            FramesManager.RequestThumbnailRedraw(Frame);
        }

        public void Undo()
        {
            foreach (var comp in Components)
                Frame.AddComponent(comp as FlipnoteCanvasComponent);
            FramesManager.RequestThumbnailRedraw(Frame);
        }
    }
}
