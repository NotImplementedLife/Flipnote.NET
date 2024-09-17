using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas;

namespace FlipnoteDotNet.App.Storage
{
    public class FrameDTO
    {
        public int[] ColorIndices { get; set; }
        public int PaperColorIndex { get; set; }
        public FlipComponentDTO[] Components { get; set; }

        public Frame ToFrame(CanvasModel canvasModel, PaletteConfig paletteConfig, Func<int, Asset> findAssetById)
        {
            var frame = new Frame(canvasModel, paletteConfig);
            frame.FrameConfig.ColorIndices = ColorIndices.ToArray();
            frame.FrameConfig.PaperColorIndex = PaperColorIndex;

            foreach(var cdto in Components)
            {
                frame.AddComponent(cdto.ToCanvasComponent(findAssetById));
            }            

            return frame;
        }

    }
}
