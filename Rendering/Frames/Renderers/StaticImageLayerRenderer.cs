using FlipnoteDotNet.Data;
using FlipnoteDotNet.Data.Layers;
using FlipnoteDotNet.Utils.Manipulator;

namespace FlipnoteDotNet.Rendering.Frames.Renderers
{
    [Manipulates(typeof(StaticImageLayer))]
    internal class StaticImageLayerRenderer : ILayerRenderer
    {
        public void Render(FrameRenderSurface surface, ILayer _layer, int timestamp)
        {
            var layer = _layer as StaticImageLayer;
            var scaleX = layer.ScaleX.GetValueAt(timestamp);
            var scaleY = layer.ScaleY.GetValueAt(timestamp);
            var x = layer.X.GetValueAt(timestamp);
            var y = layer.Y.GetValueAt(timestamp);
            var visual = layer.VisualSource.Clone();

            // visual.Data


        }
    }
}
