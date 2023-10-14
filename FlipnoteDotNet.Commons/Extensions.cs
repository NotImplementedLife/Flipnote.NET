﻿namespace FlipnoteDotNet.Commons
{
    public static class Extensions
    {
        public static int Clamp(this int x, int a, int b) => x <= a ? a : x >= b ? b : x;
    }
}