using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.App.Storage
{
    internal static class BytesContainer
    {
        public static readonly Dictionary<string, byte[]> Refs = new();
        public static void Clear() => Refs.Clear();
        public static byte[] Get(string key) => Refs[key];
        public static string Put(byte[] value)
        {
            var key = $"buf{Refs.Count + 1}";
            Refs[key] = value;
            return key;
        }

        public static void Put(string key, byte[] value)
        {            
            Refs[key] = value;            
        }

        public static void ForEach(Action<string, byte[]> action)
        {
            foreach (var (k, v) in Refs)
                action(k, v);            
        }
    }
}
