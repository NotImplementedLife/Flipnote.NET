using System.Collections.Generic;
using System.Drawing;

namespace FlipnoteDotNet.Extensions
{
    internal static class RectangleExtensions
    {
        public static Rectangle GetIntersection(this Rectangle r1, Rectangle r2)
        {
            var i = r1;
            i.Intersect(r2);
            return i;
        }

        public static IEnumerable<(int X, int Y)> EnumeratePoints(this Rectangle r)
        {
            for(int y=r.Top;y<r.Bottom;y++)
            {
                for (int x = r.Left; x < r.Right; x++)
                    yield return (x, y);
            }
        }

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
