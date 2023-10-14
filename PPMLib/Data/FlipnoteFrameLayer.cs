using System;

namespace PPMLib.Data
{
    public class FlipnoteFrameLayer
    {
        public FlipnotePen Pen { get; set; } = FlipnotePen.PaperInverse;
        private byte[] Buffer { get; } = new byte[32 * 192];
        
        public int this[int x, int y]
        {
            get
            {
                ValidateCoordinates(x, y);
                return (Buffer[32 * y + x / 8] & (1 << (x % 8))) != 0 ? 1 : 0;
            }
            set
            {
                ValidateCoordinates(x, y);
                int pos = 32 * y + x / 8;
                int bit = x % 8;
                byte mask = (byte)(1 << bit);
                Buffer[pos] &= (byte)~mask;
                if (value != 0)
                    Buffer[pos] |= mask;
            }
        }

        private void ValidateCoordinates(int x, int y)
        {
            if (x < 0 || y < 0 || x >= 256 || y >= 192)
                throw new IndexOutOfRangeException($"Invalid layer pixel coordinates: x={x}, y={y}");
        }
    }
}
