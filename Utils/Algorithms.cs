using System;
using System.Drawing;

namespace FlipnoteDotNet.Utils
{
    internal static class Algorithms
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            (b, a) = (a, b);
        }

        public static void Bresenham(Point p1, Point p2, Action<Point> callback)
            => Bresenham(p1.X, p1.Y, p2.X, p2.Y, (x, y) => callback(new Point(x, y)));

        public static void Bresenham(int x, int y, int x2, int y2, Action<int, int> callback)
        {
            int w = x2 - x;
            int h = y2 - y;
            int dx1 = 0, dy1 = 0, dx2 = 0, dy2 = 0;
            if (w < 0) dx1 = -1; else if (w > 0) dx1 = 1;
            if (h < 0) dy1 = -1; else if (h > 0) dy1 = 1;
            if (w < 0) dx2 = -1; else if (w > 0) dx2 = 1;
            int longest = Math.Abs(w);
            int shortest = Math.Abs(h);
            if (!(longest > shortest))
            {
                longest = Math.Abs(h);
                shortest = Math.Abs(w);
                if (h < 0) dy2 = -1; else if (h > 0) dy2 = 1;
                dx2 = 0;
            }
            int numerator = longest >> 1;
            for (int i = 0; i <= longest; i++)
            {
                callback(x, y);                
                numerator += shortest;
                if (!(numerator < longest))
                {
                    numerator -= longest;
                    x += dx1;
                    y += dy1;
                }
                else
                {
                    x += dx2;
                    y += dy2;
                }
            }
        }
    }
}
