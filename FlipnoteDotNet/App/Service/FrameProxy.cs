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
            //Frame.FrameConfig.PaperColorIndex
        }

        [PropertyEditor(typeof(FramePaperColorIndexEditor), name:"Paper Color")]        
        public int PaperColorIndex
        {
            get => Frame.PaperColorIndex; set => Frame.PaperColorIndex = value;
        }

        public int Value1 { get; set; }
        public string Value2 { get; set; }
    }
}
