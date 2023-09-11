using FlipnoteDotNet.Extensions;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Constants
{
    internal static class Brushes
    {
        private static Brush GetTransparentBackgroundBrush()
        {
            using (Bitmap bmp = new Bitmap(16, 16))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                for (int y = 0; y < 2; y++)
                {
                    for (int x = 0; x < 2; x++)
                        g.FillRectangle((x + y) % 2 == 0 ? System.Drawing.Brushes.White : System.Drawing.Brushes.LightGray, 8 * x, 8 * y, 8, 8);
                }

                TextureBrush tb = new TextureBrush(bmp);
                return tb;
            }
        }

        public static Brush GetWindowBackgroundBrush(int dx = 0, int dy = 0)
        {
            var bg0 = Color.FromArgb(0xFB, 0xFB, 0xFB).GetBrush();
            var p1 = Color.FromArgb(0xEB, 0xF3, 0xEB).GetPen();

            using (Bitmap bmp = new Bitmap(16, 16))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                var sx = dx == 0 ? 0 : 16 - dx;
                var sy = dy == 0 ? 0 : 16 - dy;
                g.FillRectangle(bg0, 0, 0, 16, 16);
                g.DrawLine(p1, 0, sy, 16, sy);
                g.DrawLine(p1, sx, 0, sx, 16);
                TextureBrush tb = new TextureBrush(bmp);
                return tb;
            }
        }

        public static Brush TransparentBackgroundBrush = GetTransparentBackgroundBrush();
        public static Brush WindowBackgroundBrush = GetWindowBackgroundBrush();
    }
}
