using PPMLib.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace FlipnoteDotNet.Extensions
{
    public static class FlipnoteExtensions
    {
        public static Bitmap ToBitmap(this FlipnoteFrameLayer layer, FlipnotePaperColor paperColor)
        {
            int[] colors =
            {                
                paperColor.ToColor().ToArgb(),
                layer.Pen.ToColor(paperColor).ToArgb()                
            };

            int[] pixels = new int[256 * 192];

            for (int y = 0; y < 192; y++)
            {
                for (int x = 0; x < 256; x++)
                {                    
                    pixels[y * 256 + x] = colors[layer[x, y]];
                }
            }

            var bmp = new Bitmap(256, 192);
            var data = bmp.LockBits(new Rectangle(0, 0, 256, 192), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static Bitmap ToBitmap(this FlipnoteFrame frame)
        {
            int[] colors =
            {
                frame.PaperColor.ToColor().ToArgb(),
                frame.Pen2.ToColor(frame.PaperColor).ToArgb(),
                frame.Pen1.ToColor(frame.PaperColor).ToArgb(),
                frame.Pen1.ToColor(frame.PaperColor).ToArgb(),                
            };

            int[] pixels = new int[256 * 192];

            for(int y=0;y<192;y++)
            {
                for(int x=0;x<256;x++)
                {
                    var c = 2 * frame.Layer1[x, y] + frame.Layer2[x, y];
                    pixels[y * 256 + x] = colors[c];
                }
            }

            var bmp = new Bitmap(256, 192);
            var data = bmp.LockBits(new Rectangle(0, 0, 256, 192), ImageLockMode.WriteOnly, PixelFormat.Format32bppPArgb);
            Marshal.Copy(pixels, 0, data.Scan0, pixels.Length);
            bmp.UnlockBits(data);
            return bmp;
        }

        public static Color ToColor(this FlipnotePaperColor paperColor)
        {
            if (paperColor == FlipnotePaperColor.White)
                return Constants.Colors.FlipnoteWhite;
            return Constants.Colors.FlipnoteBlack;
        }

        public static Brush ToBrush(this FlipnotePaperColor paperColor) => paperColor.ToColor().GetBrush();

        public static FlipnotePaperColor Invert(this FlipnotePaperColor paperColor)
            => paperColor == FlipnotePaperColor.White ? FlipnotePaperColor.Black : FlipnotePaperColor.White; 

        public static Color ToColor(this FlipnotePen pen, FlipnotePaperColor paperColor)
        {
            switch(pen)
            {
                case FlipnotePen.Red: return Constants.Colors.FlipnoteRed;
                case FlipnotePen.Blue: return Constants.Colors.FlipnoteBlue;
                default: return paperColor.Invert().ToColor();
            }
        }
        public static Brush ToBrush(this FlipnotePen pen, FlipnotePaperColor paperColor) => pen.ToColor(paperColor).GetBrush();
    }
}
