using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Core.Utils
{
    public static class Numbers
    {

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsBetween(this int x, int a, int b)
        {
            return x >= a && x <= b;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IntervalsIntersect(int x1, int x2, int y1, int y2)
        {
            return x1 <= y2 && y1 <= x2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Clamp(this float x, float a, float b) => x <= a ? a : x >= b ? b : x;
    }
}
