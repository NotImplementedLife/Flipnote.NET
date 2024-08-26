using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.App.Editors;
using FlipnoteDotNet.PropertyEditor;

namespace FlipnoteDotNet.App.Service
{
    public class FrameProxy
    {
        public readonly FramesManager FramesManager;
        public readonly Frame Frame;

        public FrameProxy(FramesManager framesManager, Frame frame)
        {
            FramesManager = framesManager;
            Frame = frame;                  
        }

        [PropertyEditor(typeof(FramePaperColorIndexEditor), name:"Paper Color")]        
        public int PaperColorIndex
        {
            get => Frame.PaperColorIndex;
            set
            {
                Frame.PaperColorIndex = value;
                FramesManager.RequestThumbnailRedraw(Frame);
                //Frame.FrameConfig.In
            }
        }

        [PropertyEditor(typeof(FrameSubpaletteEditor), name: "Subpalette")]
        public int[] ColorIndices
        {
            get => Frame.FrameConfig.ColorIndices;
            set
            {
                Frame.FrameConfig.ColorIndices = value;
                FramesManager.RequestThumbnailRedraw(Frame);
            }
        }        
    }
}
