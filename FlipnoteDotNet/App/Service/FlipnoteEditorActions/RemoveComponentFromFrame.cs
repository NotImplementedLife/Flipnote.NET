using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Canvas.Components;
using FlipnoteDotNet.App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    internal class RemoveComponentFromFrame : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly Frame Frame;        
        private readonly FlipnoteCanvasComponent Component;
        private readonly int Index;

        public RemoveComponentFromFrame(FramesManager framesManager, Frame frame, FlipnoteCanvasComponent component, int index)
        {
            FramesManager = framesManager;
            Frame = frame;
            Component = component;
            Index = index;
        }

        public void Do()
        {
            Frame.RemoveComponent(Component);
            FramesManager.RequestThumbnailRedraw(Frame);
        }

        public void Undo()
        {
            Frame.InsertComponent(Index, Component);
            FramesManager.RequestThumbnailRedraw(Frame);
        }
    }
}
