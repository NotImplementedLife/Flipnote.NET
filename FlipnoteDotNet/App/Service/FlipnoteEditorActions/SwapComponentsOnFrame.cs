using FlipnoteDotNet.App.Actions;
using FlipnoteDotNet.App.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Service.FlipnoteEditorActions
{
    public class SwapComponentsOnFrame : IUndoableAction
    {
        private readonly FramesManager FramesManager;
        private readonly Frame Frame;
        private readonly int Index1;
        private readonly int Index2;

        public SwapComponentsOnFrame(FramesManager framesManager, Frame frame, int index1, int index2)
        {
            FramesManager = framesManager;
            Frame = frame;
            Index1 = index1;
            Index2 = index2;
        }

        private void Swap()
        {
            Frame.SwapComponents(Index1, Index2);
            FramesManager.RequestThumbnailRedraw(Frame);
        }

        public void Do() => Swap();
        public void Undo() => Swap();        
    }
}
