using System;
using System.Collections.Generic;
using System.Linq;

namespace FlipnoteDotNet.Utils
{
    public static class Collections
    {
        public static IEnumerable<T> Of<T>(params T[] items) => items;

        public static IEnumerable<T> Generate<T>(int count, Func<T> f) => Enumerable.Range(0, count).Select(_ => f());
    }
}
