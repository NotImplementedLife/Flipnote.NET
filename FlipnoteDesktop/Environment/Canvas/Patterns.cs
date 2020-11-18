using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDesktop.Environment.Canvas
{
    public static class Patterns
    {
        public static Pattern Default = new Pattern(new bool[1, 1] { { true } });
        public static Pattern Dotted1 = new Pattern(new bool[2, 2] { { true, false }, { false, false } });
        public static Pattern Dotted2 = new Pattern(new bool[3, 3] { { true, false, false }, { false, false, false }, { false, false, false } });


    }
}
