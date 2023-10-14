using FlipnoteDotNet.Data;
using FlipnoteDotNet.GUI.Canvas;

namespace FlipnoteDotNet.Rendering
{
    public interface ILayerCanvasComponent : ICanvasComponent
    {
        ILayer Layer { get; }
        int Timestamp { get; set; }
        void Initialize(ILayer layer, LayerRenderingOptions options, int timestamp);
        void Refresh();
    }
}
