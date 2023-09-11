using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Extensions
{
    internal static class QueueExtensions
    {
        public static IEnumerable<T> DequeueAll<T>(this Queue<T> queue)
        {
            while (queue.Count > 0)
                yield return queue.Dequeue();
        }
    }
}
