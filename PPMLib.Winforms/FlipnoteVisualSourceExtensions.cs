using PPMLib.Rendering;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace PPMLib.Winforms
{
    public static class FlipnoteVisualSourceExtensions
    {
        public static Bitmap ToBitmap(this FlipnoteVisualSource visualSource, Color color1, Color color2)
        {            
            var transparent = Color.Transparent.ToArgb();
            var colors = new int[] { transparent, color1.ToArgb(), color2.ToArgb(), transparent };
            var buffer = visualSource.Data.Select(_ => colors[_ & 3]).ToArray();
            return buffer.ToBitmap32bppPArgb(visualSource.Width, visualSource.Height);
        }

        private static Bitmap ToBitmap32bppPArgb(this int[] data, int width, int height)
        {           
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            var bmpData = bmp.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.WriteOnly, bmp.PixelFormat);
            Marshal.Copy(data, 0, bmpData.Scan0, width * height);
            bmp.UnlockBits(bmpData);
            return bmp;
        }
    }
}
