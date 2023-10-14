using PPMLib.Utils;
using System;
using System.Drawing;
using System.IO;
using System.Threading;

namespace PPMLib.Rendering
{
    [Serializable]
    public class FlipnoteVisualSource
    {
        public int Width { get; }
        public int Height { get; }
        public byte[] Data { get; }

        public Size Size => new Size(Width, Height);

        public byte this[int x, int y]
        {
            get => Data[y * Width + x];
            set => Data[y * Width + x] = value;
        }

        public FlipnoteVisualSource(int width, int height)
        {
            if (width == 0 || height == 0)
                throw new ArgumentException("FlipnoteVisualSource must have non-null dimensions");
            Width = width;
            Height = height;
            Data = new byte[Width * Height];
        }

        public FlipnoteVisualSource(FlipnoteVisualSource original, int width, int height, bool dithering = false,
            RescaleMethod rescaleMethod = RescaleMethod.NearestNeighbor)
        {
            Width = width;
            Height = height;
            Data = FlipnoteVisualSourceResizer.GetResizedData(original, width, height, dithering, rescaleMethod);
        }

        public FlipnoteVisualSource Clone()
        {
            var clone = new FlipnoteVisualSource(Width, Height);
            Array.Copy(Data, clone.Data, clone.Data.Length);
            return clone;
        }

        public override string ToString() => $"Visual Source {Width}x{Height}px";

        public byte[] ToBytes()
        {
            using (var ms = new MemoryStream())
            {
                using (var bw = new BinaryWriter(ms))
                {
                    bw.Write(Width);
                    bw.Write(Height);
                    bw.Write(Data);
                }
                return ms.ToArray();
            }
        }

        public static FlipnoteVisualSource FromBinaryReader(BinaryReader br)
        {
            var w = br.ReadInt32();
            var h = br.ReadInt32();
            var data = br.ReadBytes(w * h);
            var vs = new FlipnoteVisualSource(w, h);
            Array.Copy(data, vs.Data, vs.Data.Length);
            return vs;
        }      
    }
}
