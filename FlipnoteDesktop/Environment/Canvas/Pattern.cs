using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDesktop.Environment.Canvas
{
    public class Pattern
    {
        public Pattern(bool[,] pixels)
        {
            Rows = pixels.GetLength(0);
            Cols = pixels.GetLength(1);
            Pixels = pixels.Clone() as bool[,];
        }

        public int Rows { get; }
        public int Cols { get; }
        public bool[,] Pixels;

        public bool GetPixelAt(int x,int y)
        {
            return Pixels[y % Rows, x % Cols];
        }
    }
}
