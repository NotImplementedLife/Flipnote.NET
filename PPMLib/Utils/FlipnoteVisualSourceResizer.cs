using PPMLib.Rendering;
using System;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace PPMLib.Utils
{
    public static class FlipnoteVisualSourceResizer
    {
        // custom 2d color space
        // (0,0) = paper, (N,0) = pen1, (0,N) = pen2        
        public struct C012 
        {
            public const int N = 64;
            public readonly int X;
            public readonly int Y;

            public C012(int x, int y)
            {
                X = x;
                Y = y;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static C012 FromColorId(byte index)
            {
                if (index == 1) return Color1;
                if (index == 2) return Color2;
                return Color0;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public C012 CollapseToBaseColor()
            {
                return Y > 0 && 3L * Y * Y > 1L * X * X ? Color2 : X <= 0 ? Color0 : Color1;
            }

            private static C012 GetPolar(double angle) => new C012((int)(N * Math.Cos(angle)), (int)(N * Math.Sin(angle)));

            public static readonly C012 Color0 = GetPolar(7 * Math.PI / 6);
            public static readonly C012 Color1 = GetPolar(11 * Math.PI / 6);
            public static readonly C012 Color2 = GetPolar(Math.PI / 2);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public byte ResolveIndex()
            {
                return Y > 0 && 3L * Y * Y > 1L * X * X ? (byte)2 : X <= 0 ? (byte)0 : (byte)1;
            }

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static C012 operator +(C012 a, C012 b) => new C012(a.X + b.X, a.Y + b.Y);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static C012 operator -(C012 a, C012 b) => new C012(a.X - b.X, a.Y - b.Y);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static C012 operator *(C012 a, int x) => new C012(a.X * x, a.Y * x);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public static C012 operator /(C012 a, int x) => new C012(a.X / x, a.Y / x);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public C012 Normalize()
            {
                var d = (int)(Math.Sqrt(1L * X * X + 1L * Y * Y));
                if (d == 0) return Color0;
                return new C012(X * N / d, Y * N / d);
            }

        }

        public static C012[] ToColorVectors(byte[] data)
        {
            var result = new C012[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = C012.FromColorId(data[i]);             
            return result;
        }        

        public static C012[] Resize(C012[] original, int oldWidth, int oldHeight, int width, int height,
            RescaleMethod rescaleMethod = RescaleMethod.NearestNeighbor)
        {
            switch (rescaleMethod)
            {
                case RescaleMethod.Bilinear: return RescaleAlgorithms.Bilinear(original, oldWidth, oldHeight, width, height);
                default: return RescaleAlgorithms.NearestNeighbor(original, oldWidth, oldHeight, width, height);
            }                        
        }

        private static readonly int[] DitherDx = { 1, -1, 0, 1 };
        private static readonly int[] DitherDy = { 0, 1, 1, 1 };
        private static readonly int[] DitherDd = { 7, 3, 5, 1 };

        public static void ApplyDithering(C012[] source, int width)
        {
            int h = source.Length / width;
          
            for (int y = 0; y < h; y++) 
            {
                for (int x = 0; x < width; x++) 
                {
                    int index = y * width + x;
                    if (index >= source.Length) break;
                    var oldC = source[index];
                    source[index] = source[index].CollapseToBaseColor();
                    var err = oldC - source[index];

                    for (int i = 0; i < 4; i++) 
                    {
                        int ix = x + DitherDx[i];
                        int iy = y + DitherDy[i];
                        int iindex = iy * width + ix;
                        if (ix < 0 || ix >= width || iy >= h || iindex >= source.Length) continue;
                        source[iindex] += err * DitherDd[i] / 16;
                    }
                }
            }                                 
        }

        public static byte[] GetResizedData(FlipnoteVisualSource source, int width, int height, bool dithering
            , RescaleMethod rescaleMethod = RescaleMethod.NearestNeighbor)
        {            
            var originalColors = ToColorVectors(source.Data);

            var newColors = Resize(originalColors, source.Width, source.Height, width, height, rescaleMethod);
            if (dithering) ApplyDithering(newColors, width);


            var result = new byte[newColors.Length];

            for (int i = 0; i < newColors.Length; i++)
                result[i] = newColors[i].ResolveIndex();

            return result;            
        }

    }
}
