using PPMLib.Rendering;
using System.Drawing;
using System.Linq;

namespace FlipnoteDotNet.Extensions
{
    internal static class FlipnoteVisualSourceExtensions
    {
        public static Bitmap ToBitmap(this FlipnoteVisualSource visualSource, Color color1, Color color2)
        {
            var transparent = Color.Transparent.ToArgb();
            var colors = new int[] { transparent, color1.ToArgb(), color2.ToArgb(), transparent };
            var buffer = visualSource.Data.Select(_ => colors[_ & 3]).ToArray();
            return buffer.ToBitmap32bppPArgb(visualSource.Width, visualSource.Height);
        }
    }
}
