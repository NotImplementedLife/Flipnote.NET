using System;
using System.Linq;
using System.Runtime.CompilerServices;

namespace FlipnoteDotNet.GUI.VisualSource
{
    public class PaintSurface
    {
        public static readonly int SurfaceWidth = 1024;
        public static readonly int SurfaceHeight = 1024;
        
        public byte[] Buffer 
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get; 
        } = new byte[SurfaceHeight * SurfaceWidth];


        public void Clear() => Array.Clear(Buffer, 0, Buffer.Length);

        public byte[] ToArray() => Buffer.ToArray();        


    }
}
