﻿namespace FlipnoteDotNet.Extensions
{
    internal static class IntException
    {
        public static int Clamp(this int x, int a, int b) => x <= a ? a : x >= b ? b : x;

    }
}
