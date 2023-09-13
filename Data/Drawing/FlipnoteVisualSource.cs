using FlipnoteDotNet.Extensions;
using System;
using System.Drawing;
using System.Linq;

namespace FlipnoteDotNet.Data.Drawing
{
    internal class FlipnoteVisualSource
    {
        public int Width { get; }
        public int Height { get; }
        public byte[] Data { get; }

        public FlipnoteVisualSource(int width, int height)
        {
            Width = width;
            Height = height;
            Data = new byte[Width * Height];
        }

        public Bitmap ToBitmap(Color color1, Color color2)
        {
            var transparent = Color.Transparent.ToArgb();
            var colors = new int[] { transparent, color1.ToArgb(), color2.ToArgb(), transparent };
            var buffer = Data.Select(_ => colors[_ & 3]).ToArray();
            return buffer.ToBitmap32bppPArgb(Width, Height);
        }

        /*public FlipnoteVisualSource Clone()
        {
            var clone = new FlipnoteVisualSource(Width, Height);
            Array.Copy(Data, clone.Data, clone.Data.Length);
            return clone;
        }*/
             
    }
}
