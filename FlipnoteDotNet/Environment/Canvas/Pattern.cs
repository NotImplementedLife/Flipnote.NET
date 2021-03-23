using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlipnoteDotNet.Environment.Canvas
{
    internal class Pattern
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

        /// <summary>
        /// This option adds extra pixels to drawn shapes (lines) to make it a continuous drawing.
        /// </summary>
        /// <remarks>
        /// Only used by PenPatterns</remarks>
        public bool ContinuousDraw = false;
    }
}
