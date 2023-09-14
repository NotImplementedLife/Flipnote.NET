using System.Drawing;

namespace FlipnoteDotNet.Extensions
{
    internal static class RectangleExtensions
    {
        public static Rectangle GetPaddedContent(this Rectangle r, int padding)
        {
            return new Rectangle(r.Left + padding, r.Top + padding, r.Width - 2 * padding, r.Height - 2 * padding);
        }
    }
}
