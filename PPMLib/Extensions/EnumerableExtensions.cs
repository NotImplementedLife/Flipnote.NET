using System.Collections.Generic;
using System.Linq;

namespace PPMLib.Extensions
{
    internal static class EnumerableExtensions
    {
        public static string JoinToString<T>(this IEnumerable<T> items, string separator = "\n")
            => string.Join(separator, items.Select(_ => _?.ToString() ?? "null"));
    }
}
