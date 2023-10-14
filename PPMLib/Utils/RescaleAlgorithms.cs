using PPMLib.Extensions;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using static PPMLib.Utils.FlipnoteVisualSourceResizer;

namespace PPMLib.Utils
{
    internal static class RescaleAlgorithms
    {
        public static C012[] NearestNeighbor(C012[] original, int oldWidth, int oldHeight, int width, int height)
        {
            var newColors = new C012[width * height];            
            for (int y = 0; y < height; y++)
            {
                int y0 = Math.DivRem(y * oldHeight, height, out _);                

                for (int x = 0; x < width; x++)
                {
                    int x0 = Math.DivRem(x * oldWidth, width, out _);
                    newColors[y * width + x] = original[y0 * oldWidth + x0];
                }
            }

            return newColors;
        }

        public static C012[] Bilinear(C012[] original,int oldWidth, int oldHeight, int width, int height)
        {
            var newColors = new C012[width * height];            

            float xRatio = width > 1 ? (oldWidth - 1.0f) / (width - 1) : 0;
            float yRatio = height > 1 ? (oldHeight - 1.0f) / (height - 1) : 0;
            //Debug.WriteLine($"{xRatio}:{yRatio}");

            for (int y = 0; y < height; y++)
            {
                var ny = yRatio * y;
                int yl = (int)Math.Floor(ny);
                int yh = (int)Math.Ceiling(ny);
                if (yh >= oldHeight) yh = oldHeight - 1;
                float yWeight = ny - yl;

                for (int x = 0; x < width; x++)
                {
                    var nx = xRatio * x;
                    int xl = (int)Math.Floor(nx);
                    int xh = (int)Math.Ceiling(nx);
                    if (xh >= oldWidth) xh = oldWidth - 1;

                    float xWeight = nx - xl; 

                    var a = original[yl * oldWidth + xl];
                    var b = original[yl * oldWidth + xh];
                    var c = original[yh * oldWidth + xl];
                    var d = original[yh * oldWidth + xh];

                    int ixw = (int)(xWeight * 256);  
                    int iyw = (int)(yWeight * 256);

                    var s = (a * (256 - ixw) * (256 - iyw) + b * ixw * (256 - iyw) + c * iyw * (256 - ixw) + d * ixw * iyw) / 256;
                    newColors[y * width + x] = s.Normalize();
                }
            }
            return newColors;
        }
    }
}
