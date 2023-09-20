using FlipnoteDotNet.Data;

namespace FlipnoteDotNet.Rendering.Frames
{
    public interface ILayerRenderer
    {
        void Render(FrameRenderSurface surface, ILayer layer, int timestamp);
    }
}
