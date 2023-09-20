using PPMLib.Data;

namespace FlipnoteDotNet.Rendering.Frames
{
    public class FrameRenderSurface
    {
        public byte[] Pixels { get; } = new byte[256 * 192];

        public FlipnoteFrame ToFlipnoteFrame(FlipnotePaperColor paperColor, FlipnotePen pen1, FlipnotePen pen2)
        {
            var frame = new FlipnoteFrame(paperColor, pen1, pen2);            

            for (int y = 0; y < 192; y++) 
            {
                for (int x = 0; x < 256; x++)
                {
                    int index = y * 256 + x;
                    if ((Pixels[index] & 1) != 0)
                        frame.Layer1[x, y] = 1;
                    if ((Pixels[index] & 2) != 0)
                        frame.Layer2[x, y] = 1;
                }
            }
            return frame;
        }
    }
}
