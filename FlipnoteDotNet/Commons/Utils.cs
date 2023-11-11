using System;

namespace FlipnoteDotNet.Commons
{
    public static class Utils
    {
        public static void Repeat(int iterationsCount, Action action)
        {
            for (; iterationsCount > 0; iterationsCount--)
                action();
        }
    }
}
