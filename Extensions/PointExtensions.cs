using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Extensions
{
    internal static class PointExtensions
    {
        public static int DistanceSquared(this Point p, Point q)
        {
            int dx = p.X - q.X;
            int dy = p.Y - q.Y;
            return dx * dx + dy * dy;
        }

        public static bool IsInRange(this Point p, Point o, int radius) => p.DistanceSquared(o) <= radius * radius;
    }
}
