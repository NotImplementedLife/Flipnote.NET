namespace FlipnoteDotNet.Drawing.Renderers
{
    public readonly struct VisualRenderOptions
    {
        public readonly bool UseDithering = false;
        public readonly Rectangle? SurfaceBounds = null;
        public VisualRenderOptions(bool useDithering = false, Rectangle? surfaceBounds = default)
        {
            UseDithering = useDithering;
            SurfaceBounds = surfaceBounds;
        }
    }
}
