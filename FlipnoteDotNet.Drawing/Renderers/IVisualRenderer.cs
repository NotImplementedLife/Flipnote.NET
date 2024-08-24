using System.Numerics;

namespace FlipnoteDotNet.Drawing.Renderers
{
    public interface IVisualRenderer
    {
        void RenderTransform(Bitmap source, Matrix3x2 transformMatrix,
            out Bitmap destBitmap, out Rectangle destBounds,
            VisualRenderOptions options = default);
        void OrderedDithering(Bitmap source, Palette palette, int order, out Bitmap dest, int alphaThreshold = 128);        
    }
}
