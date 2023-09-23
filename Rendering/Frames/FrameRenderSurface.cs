using FlipnoteDotNet.Extensions;
using PPMLib.Data;
using PPMLib.Rendering;
using PPMLib.Utils;
using System.Drawing;

namespace FlipnoteDotNet.Rendering.Frames
{
    public class FrameRenderSurface
    {
        public byte[] Pixels { get; } = new byte[256 * 192];

        public static readonly Rectangle FrameBounds = new Rectangle(0, 0, 256, 192);

        public void DrawVisualSource(FlipnoteVisualSource vs, int x, int y)
        {
            foreach (var (ix, iy) in new Rectangle(x, y, vs.Width, vs.Height).GetIntersection(FrameBounds).EnumeratePoints())
            {
                byte c = vs[ix - x, iy - y];
                if (c == 0) continue;
                Pixels[256 * iy + ix] = c;                
            }
        }

        public void DrawVisualSource(FlipnoteVisualSource vs, Rectangle bounds, bool dithering, RescaleMethod rescaleMethod)
        {
            var rescaledVs = new FlipnoteVisualSource(vs, bounds.Width, bounds.Height, dithering, rescaleMethod);
            DrawVisualSource(rescaledVs, bounds.X, bounds.Y);
        }

        public FlipnoteFrame ToFlipnoteFrame(FlipnotePaperColor paperColor, FlipnotePen pen1, FlipnotePen pen2)
        {
            var frame = new FlipnoteFrame(paperColor, pen1, pen2);

            for (int y = 0; y < 192; y++)
            {
                for (int x = 0; x < 256; x++)
                {
                    int index = y * 256 + x;
                    if ((Pixels[index] & 1) != 0)
                        frame.Layer1[x, y] |= 1;
                    if ((Pixels[index] & 2) != 0) 
                        frame.Layer2[x, y] |= 1;
                }
            }

            return frame;
        }
    }
}
