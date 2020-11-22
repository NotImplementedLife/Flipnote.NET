using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDesktop.Environment.Canvas
{
    public static class PenPatterns
    {
        public static readonly Pattern Mono = new Pattern(new bool[1, 1] { { true } });
        

        public static List<string> NamesList()
        {
            var fields = typeof(PenPatterns).GetFields(BindingFlags.Static | BindingFlags.Public);
            var result = new List<string>();
            foreach (var fi in fields)
            {
                if (fi.IsInitOnly)
                    result.Add(fi.Name);
            }
            return result;
        }
    }
}
