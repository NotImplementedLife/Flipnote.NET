using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace FlipnoteDotNet.Extensions
{
    internal static class ColorExtensions
    {
        public static Brush GetBrush(this Color color) => new SolidBrush(color);
        public static Pen GetPen(this Color color, float width = 1, DashStyle dashStyle = DashStyle.Solid)
            => new Pen(color.GetBrush(), width) { DashStyle = dashStyle };

        public static Color Alpha(this Color color, byte alpha) => Color.FromArgb(alpha, color);
    }
}
