using System.Drawing;

namespace FlipnoteDotNet.Extensions
{
    internal static class RectangleExtensions
    {
        public static Rectangle GetPaddedContent(this Rectangle r, int padding)
        {
            return new Rectangle(r.Left + padding, r.Top + padding, r.Width - 2 * padding, r.Height - 2 * padding);
        }

        public static Rectangle ScaleToFit(this Rectangle r, Rectangle bounds)
        {
            if (r.Width == 0 || r.Height == 0) return Rectangle.Empty;

            var s = new Size(bounds.Width, r.Height * bounds.Width / r.Width);
            if (s.Height > bounds.Height)
                s = new Size(r.Width * bounds.Height / r.Height, bounds.Height);
            var p = new Point((bounds.Width - s.Width) / 2, (bounds.Height - s.Height) / 2);
            return new Rectangle(p, s);
        }

        public static Rectangle ScaleToFit(this Size r, Rectangle bounds)
        {
            if (r.Width == 0 || r.Height == 0) return Rectangle.Empty;

            var s = new Size(bounds.Width, r.Height * bounds.Width / r.Width);
            if (s.Height > bounds.Height)
                s = new Size(r.Width * bounds.Height / r.Height, bounds.Height);
            var p = new Point((bounds.Width - s.Width) / 2, (bounds.Height - s.Height) / 2);
            return new Rectangle(p, s);
        }


    }
}
