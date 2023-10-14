using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        public static Color GetContrastColor(this Color c)
        {
            var l = 0.2126 * c.R + 0.7152 * c.G + 0.0722 * c.B;            
            return l < 128 ? Color.White : Color.Black;
        }

        public static string ToHexString(this Color color)
        {
            return $"#{color.R:X2}{color.G:X2}{color.B:X2}";
        }
    }
}
