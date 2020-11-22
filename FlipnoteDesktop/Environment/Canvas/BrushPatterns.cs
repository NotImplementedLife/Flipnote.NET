using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDesktop.Environment.Canvas
{
    public static class BrushPatterns
    {
        public static readonly Pattern Mono = new Pattern(new bool[1, 1] { { true } });
        public static readonly Pattern Dots = new Pattern(new bool[2, 2] { { true, false }, { false, false } });
        public static readonly Pattern DotsLarge = new Pattern(new bool[3, 3] { { true, false, false }, { false, false, false }, { false, false, false } });        
        public static readonly Pattern Column = new Pattern(new bool[1, 2] { { true, false } });
        public static readonly Pattern Row = new Pattern(new bool[2, 1] { { true }, { false } });
        public static readonly Pattern Binary = new Pattern(new bool[2, 2] { { true, false }, { false, true } });
        public static readonly Pattern Window = new Pattern(new bool[2, 4] { { true, true, true, true }, { true, false, true, false } });
        public static readonly Pattern Bullets = new Pattern(new bool[6, 6]
        {
            {true,true,false,false,true,true },
            {true,false,false,false,false,true },
            {false,false,false,false,false,false },
            {false,false,false,false,false,false },
            {true,false,false,false,false,true },
            {true,true,false,false,true,true },
        });

        public static List<string> NamesList()
        {
            var fields = typeof(BrushPatterns).GetFields(BindingFlags.Static | BindingFlags.Public);
            var result = new List<string>();
            foreach(var fi in fields)
            {
                if (fi.IsInitOnly)
                    result.Add(fi.Name);
            }
            return result;
        }
    }
}
