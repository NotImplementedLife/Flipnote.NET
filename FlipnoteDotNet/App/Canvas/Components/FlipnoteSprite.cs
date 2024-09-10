using FlipnoteDotNet.App.Data;
using FlipnoteDotNet.Canvas.Components;
using FlipnoteDotNet.Drawing;

namespace FlipnoteDotNet.App.Canvas.Components
{
    public class FlipnoteSprite : FlipnoteCanvasComponent
    {
        private readonly Bitmap fSource;
        public Bitmap Source => fSource;
        private Bitmap CachedTransformedImage = null;
        private Rectangle CachedTransformedBounds;
        private Bitmap CachedDitheredImage = null;
        private Palette CachedPalette;

        private bool NeedsCacheUpdate = true;
        private bool NeedsDitheringUpdate;

        public FlipnoteSprite(Asset asset, Bitmap bitmap, string name = "")
            : base(name, bitmap.Width, bitmap.Height, ignoreTransform: false, ignoreGraphicsTransform: true, asset)
        {
            fSource = bitmap;
        }

        protected override void OnAttachedToFrame(Frame frame)
        {
            FrameConfig = frame.FrameConfig;
            CachedPalette = FrameConfig.ActualPalette;
            FrameConfig.ColorIndicesChanged += ColorIndicesChanged;
        }

        protected override void OnDetachedFromFrame()
        {
            FrameConfig.ColorIndicesChanged -= ColorIndicesChanged;
            FrameConfig = null;
        }

        private void ColorIndicesChanged(object sender, EventArgs e)
        {
            CachedPalette = FrameConfig.ActualPalette;
            NeedsDitheringUpdate = true;
            Invalidate();
        }

        public override void Update()
        {
            if (NeedsCacheUpdate)
            {
                NeedsCacheUpdate = false;
                NeedsDitheringUpdate = true;
                Config.VisualRenderer.RenderTransform(fSource, DirectTransformValues, out var bmp, out var bounds);
                var old = CachedTransformedImage;
                CachedTransformedImage = bmp;
                CachedTransformedBounds = bounds;
                old?.Dispose();
            }
            if (NeedsDitheringUpdate)
            {
                NeedsDitheringUpdate = false;
                Config.VisualRenderer.OrderedDithering(CachedTransformedImage, CachedPalette, 3, out var bmp, 128);
                var old = CachedDitheredImage;
                CachedDitheredImage = bmp;
                old?.Dispose();
            }
        }

        public override void Draw(Graphics g)
        {
            if (CachedTransformedImage == null)
            {                
                Config.VisualRenderer.RenderTransform(fSource, DirectTransformValues,
                    out CachedTransformedImage, out CachedTransformedBounds);
                Config.VisualRenderer.OrderedDithering(CachedTransformedImage, CachedPalette, 3, out CachedDitheredImage, 128);
            }
            g.DrawImageUnscaled(CachedDitheredImage, CachedTransformedBounds.Location);
        }

        protected override void OnTransformChanged(EventArgs e)
        {
            NeedsCacheUpdate = true;
            base.OnTransformChanged(e);
        }

        public override CanvasComponent Clone()
        {
            var clone = base.Clone() as FlipnoteSprite;
            clone.CachedPalette = FrameConfig.ActualPalette;
            return clone;    
        }
    }
}
