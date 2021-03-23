using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Environment.Canvas
{
    internal static class PenPatterns
    {
        public static readonly Pattern Mono = new Pattern(new bool[1, 1] { { true } });
        public static readonly Pattern CaligraphyLeft = new Pattern(new bool[3, 3]
            {
                {  true, false, false },
                { false,  true, false },
                { false, false,  true }
            }) { ContinuousDraw = true };
        public static readonly Pattern CaligraphyRight = new Pattern(new bool[3, 3]
            {
                { false, false,  true },
                { false,  true, false },
                {  true, false, false }
            }) { ContinuousDraw = true };       

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
